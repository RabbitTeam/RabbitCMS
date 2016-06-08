using System;
using System.CodeDom;
using System.CodeDom.Compiler;
using System.Collections;
using System.Globalization;
using System.IO;
using System.IO.Compression;
using System.Net;
using System.Text;
using System.Web;
using System.Web.Services.Description;
using Microsoft.CSharp;

namespace Jkzl.Activitys.Tool
{
    public class WebRequestHelper
    {
        /// <summary>
        /// 总线请求超时时间
        /// </summary>
        private static int HttpWebRequestTimeout
        {
            get
            {
                return 8000;
            }
        }
        /// <summary>
        /// 获取java提供的webservice
        /// </summary>
        /// <param name="url"></param>
        /// <param name="methodname"></param>
        /// <param name="args"></param>
        /// <param name="gzip"></param>
        /// <param name="nameSpace"></param>
        /// <returns></returns>
        public static string GetJavaWebServicePostWeb(string url, string methodname, object[] args, string nameSpace = "Commom.Core.WebService")
        {
            string @namespace = nameSpace;
            string classname = GetWsClassName(url);
            string responseString = "";
            try
            {
                ////获取Web Service描述
                var wc = new WebClient();
                var stream = wc.OpenRead(url + "?wsdl"); //一定要以?wsdl结尾
                if (stream == null) return "";
                ServiceDescription sd = ServiceDescription.Read(stream);
                var sdi = new ServiceDescriptionImporter
                {
                    ProtocolName = "soap",
                    Style = ServiceDescriptionImportStyle.Client
                };
                sdi.AddServiceDescription(sd, "", "");

                //指定命名空间  
                var cn = new CodeNamespace(@namespace);//这里随便指定一个命名空间，但要与后面的一致  

                //生成客户端代理类代码
                var ccu = new CodeCompileUnit();
                ccu.Namespaces.Add(cn);
                sdi.Import(cn, ccu);
                //建立C#编译器  
                var csc = new CSharpCodeProvider();
                ICodeCompiler icc = csc.CreateCompiler();

                //设定编译参数
                var cplist = new CompilerParameters { GenerateExecutable = false, GenerateInMemory = true };
                //添加编译条件 
                cplist.ReferencedAssemblies.Add("System.dll");
                cplist.ReferencedAssemblies.Add("System.XML.dll");
                cplist.ReferencedAssemblies.Add("System.Web.Services.dll");
                cplist.ReferencedAssemblies.Add("System.Data.dll");

                //编译代理类
                CompilerResults cr = icc.CompileAssemblyFromDom(cplist, ccu);
                if (true == cr.Errors.HasErrors)
                {
                    var sb = new System.Text.StringBuilder();
                    foreach (System.CodeDom.Compiler.CompilerError ce in cr.Errors)
                    {
                        sb.Append(ce.ToString());
                        sb.Append(System.Environment.NewLine);
                    }
                    throw new Exception(sb.ToString());
                }

                //生成代理实例，并调用方法
                //获取程序集  
                System.Reflection.Assembly assembly = cr.CompiledAssembly;

                //获取程序集类型  
                //前面的namespace就是命名空间，必须要与前面指定的一致  
                //后面的classname就是service的类名  
                //如果所有的服务器都是一致的类名，这里就可以写死，否则要动态提供类名 
                Type t = assembly.GetType(@namespace + "." + classname, true, true);
                object obj = Activator.CreateInstance(t);
                //获取方法  
                System.Reflection.MethodInfo mi = t.GetMethod(methodname);
                //调用方法  
                //如果方法没有参数，第二个参数可以传递null，否则就要传递object数组，数组元素的顺序要与参数的顺序一致  
                //如果所有服务器的方法签名都是一致的，object数组的顺序就可以写死了，否则还要动态调整元素的数量及顺序
                responseString = mi.Invoke(obj, args) as string;
            }
            catch (Exception ex)
            {
            }
            return (responseString + "").Length == 0 ? "{\"Code\":-10000,\"Message\":\"超时\",\"Data\":\"{}\"}" : responseString;
        }

        private static string GetWsClassName(string wsUrl)
        {
            string[] parts = wsUrl.Split('/');
            string[] pps = parts[parts.Length - 1].Split('.');

            return pps[0];
        }

        /// <summary>
        /// .net Webservice post 请求
        /// </summary>
        /// <param name="url">webservice地址</param>
        /// <param name="methodName">方法名</param>
        /// <param name="parameters">参数列表</param>
        /// <param name="gzip">是否需要gzip解压</param>
        /// <param name="nameSpace"></param>
        /// <returns></returns>
        public static string GetHttpWebServicePostWeb(string url, string methodName, Hashtable parameters, bool gzip, string nameSpace = "http://tempuri.org/")
        {
            try
            {
                string postData = ParsToStringWebservice(parameters);
                var param = new StringBuilder();
                param.AppendLine("<?xml version='1.0' encoding='utf-8'?>");
                param.AppendLine(
                    @"<soap12:Envelope xmlns:xsi='http://www.w3.org/2001/XMLSchema-instance' xmlns:xsd='http://www.w3.org/2001/XMLSchema' xmlns:soap12='http://www.w3.org/2003/05/soap-envelope'>");
                param.AppendLine("<soap12:Body>");
                param.AppendLine("<" + methodName + " xmlns='" + nameSpace + "'>");
                param.AppendLine(postData);
                param.AppendLine("</" + methodName + ">");
                param.AppendLine("</soap12:Body>");
                param.AppendLine("</soap12:Envelope>");

                byte[] bs = Encoding.UTF8.GetBytes(param.ToString());

                var myRequest = (HttpWebRequest)WebRequest.Create(url);
                myRequest.Method = "POST";
                myRequest.ContentType = "application/soap+xml; charset=utf-8";
                myRequest.ContentLength = bs.Length;
                myRequest.ServicePoint.Expect100Continue = false;
                //                myRequest.Timeout = 15000;
                using (Stream reqStream = myRequest.GetRequestStream())
                {
                    reqStream.Write(bs, 0, bs.Length);
                }
                string responseString = string.Empty;
                //获取数据
                try
                {
                    using (var myResponse = (HttpWebResponse)myRequest.GetResponse())
                    {
                        Stream receiveStream = myResponse.GetResponseStream();
                        if (gzip)
                            receiveStream = new GZipStream(receiveStream, CompressionMode.Decompress);
                        var sr = new StreamReader(receiveStream, Encoding.UTF8);

                        responseString = sr.ReadToEnd();
                    }
                }
                catch (WebException wex)
                {
                    var pageContent = new StreamReader(wex.Response.GetResponseStream())
                                          .ReadToEnd();
                }
                catch (Exception ex)
                {
                    responseString = ResponseStringError(url, parameters, responseString);
                }

                return responseString.Length == 0 ? "{\"Code\":-10000,\"Message\":\"超时\",\"Data\":\"{}\"}" : responseString;
            }
            catch (Exception ex)
            {
                return "";
            }
        }

        private static string ResponseStringError(string url, Hashtable parameters, string responseString)
        {
            responseString = "{\"Code\":-10000,\"Message\":\"超时\",\"Data\":\"{}\"}";
            
            return responseString;
        }

        /// <summary>
        ///     webService参数设定
        /// </summary>
        /// <param name="pars"></param>
        /// <returns></returns>
        private static string ParsToStringWebservice(Hashtable pars)
        {
            var sb = new StringBuilder();
            foreach (string k in pars.Keys)
            {
                sb.Append("<" + HttpUtility.UrlEncode(k) + ">" + ConvertXml(pars[k] as string) + "</" + HttpUtility.UrlEncode(k) + ">");
            }
            return sb.ToString();
        }

        /// <summary>
        /// 普通url请求
        /// </summary>
        /// <param name="url">url地址</param>
        /// <param name="parameters">参数列表</param>
        /// <param name="isGet">请求是否是get</param>
        /// <param name="gzip">是否gzip解压</param>
        /// <returns></returns>
        public static string GetHttpNewWeb(string url, Hashtable parameters, bool isGet, bool gzip)
        {
            string postData = ParsToString(parameters);
            return GetHttpNewWeb(url, postData, isGet, gzip);
        }

        public static string GetHttpNewWeb(string url, string paramStr, bool isGet, bool gzip)
        {
            try
            {
                Encoding encoding = Encoding.UTF8;
                byte[] data = encoding.GetBytes(paramStr);
                //准备请求
                var myRequest = (HttpWebRequest)WebRequest.Create(isGet ? url + "?" + paramStr : url);
                myRequest.Method = isGet ? "GET" : "POST";
                myRequest.ContentType = "application/x-www-form-urlencoded";
                myRequest.Timeout = Convert.ToInt32(HttpWebRequestTimeout);
                if (!isGet)
                {
                    myRequest.ContentLength = data.Length;
                    Stream stream = myRequest.GetRequestStream();
                    //发送数据
                    stream.Write(data, 0, data.Length);
                    stream.Close();
                }
                string responseString = string.Empty;
                //获取数据
                try
                {
                    using (var myResponse = (HttpWebResponse)myRequest.GetResponse())
                    {
                        Stream requestStream = myResponse.GetResponseStream();
                        if (requestStream != null)
                        {
                            if (gzip)
                            {
                                requestStream = new GZipStream(requestStream, CompressionMode.Decompress);
                            }
                            var sr = new StreamReader(requestStream, Encoding.UTF8);
                            responseString = sr.ReadToEnd();
                            sr.Close();
                        }
                    }
                }
                catch (Exception ex)
                {
                    responseString = ResponseStringError(url + paramStr, null, responseString);
                }

                return responseString.Length == 0 ? "{\"Code\":-10000,\"Message\":\"超时\",\"Data\":\"{}\"}" : responseString;
            }
            catch (Exception ex)
            {                
                return "";
            }
        }

        /// <summary>
        ///     参数设定
        /// </summary>
        /// <param name="pars"></param>
        /// <returns></returns>
        private static string ParsToString(Hashtable pars)
        {
            if (pars == null) return "";
            var sb = new StringBuilder();
            foreach (string k in pars.Keys)
            {
                if (sb.Length > 0)
                {
                    sb.Append("&");
                }
                sb.Append(HttpUtility.UrlEncode(k) + "=" + (pars[k].ToString()));
            }
            return sb.ToString();
        }

        /// <summary>
        ///     IT中心新接口
        /// </summary>
        /// <param name="url"></param>
        /// <param name="authInfo"></param>
        /// <param name="sequenceNo"></param>
        /// <param name="api"></param>
        /// <param name="param"></param>
        /// <param name="paramType"></param>
        /// <param name="outType"></param>
        /// <param name="v"></param>
        /// <returns></returns>
        public static string GetHttpBossApiWeb(string url, string authInfo, string sequenceNo, string api, string param,
            int paramType, int outType, string v)
        {
            var parameters = new Hashtable
            {
                {"AuthInfo", authInfo},
                {"SequenceNo", sequenceNo},
                {"Api", api},
                {"Param", param},
                {"ParamType", paramType},
                {"OutType", outType},
                {"V", v}
            };
            return GetHttpNewWeb(url, parameters, false, false);
        }

        #region XML转义字符处理
        /// <summary>
        /// XML转义字符处理
        /// </summary>
        public static string ConvertXml(string xml)
        {

            xml = (char)1 + xml;   //为了避免首字母为要替换的字符，前加前缀

            for (int intNext = 0; true; )
            {
                int intIndexOf = xml.IndexOf("&", intNext, System.StringComparison.Ordinal);
                intNext = intIndexOf + 1;  //避免&被重复替换
                if (intIndexOf <= 0)
                {
                    break;
                }
                else
                {
                    xml = xml.Substring(0, intIndexOf) + "&amp;" + xml.Substring(intIndexOf + 1);
                }
            }

            for (; true; )
            {
                int intIndexOf = xml.IndexOf("<", System.StringComparison.Ordinal);
                if (intIndexOf <= 0)
                {
                    break;
                }
                else
                {
                    xml = xml.Substring(0, intIndexOf) + "&lt;" + xml.Substring(intIndexOf + 1);
                }
            }

            for (; true; )
            {
                int intIndexOf = xml.IndexOf(">", System.StringComparison.Ordinal);
                if (intIndexOf <= 0)
                {
                    break;
                }
                else
                {
                    xml = xml.Substring(0, intIndexOf) + "&gt;" + xml.Substring(intIndexOf + 1);
                }
            }

            for (; true; )
            {
                int intIndexOf = xml.IndexOf("\"", System.StringComparison.Ordinal);
                if (intIndexOf <= 0)
                {
                    break;
                }
                else
                {
                    xml = xml.Substring(0, intIndexOf) + "&quot;" + xml.Substring(intIndexOf + 1);
                }
            }

            return xml.Replace(((char)1).ToString(CultureInfo.InvariantCulture), "");

        }
        #endregion


    }
}