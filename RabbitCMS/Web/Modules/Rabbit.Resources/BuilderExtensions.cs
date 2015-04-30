using Rabbit.Kernel;
using System.Web.Optimization;

namespace Rabbit.Resources
{
    public static class BuilderExtensions
    {
        #region Field

        private const string Prefix = "~/Modules/Rabbit.Resources/";
        private const string ScriptPrefix = Prefix + "Scripts/";
        private const string ContentPrefix = Prefix + "Content/";

        #endregion Field

        #region Public Method

        public static void UseResources(this IKernelBuilder kernelBuilder)
        {
            BundleTable.EnableOptimizations = true;
            RegisterResources(BundleTable.Bundles);
        }

        #endregion Public Method

        #region Private Method

        private static void RegisterResources(BundleCollection bundles)
        {
            bundles.Add(new ScriptBundle("~/bundles/jquery").Include(GetScriptPath("jQuery/jquery.1.11.1.min.js")));
            bundles.Add(new ScriptBundle("~/bundles/jquery2").Include(GetScriptPath("jQuery/jquery.2.1.1.min.js")));
            bundles.Add(new ScriptBundle("~/bundles/bootstrap").Include(GetScriptPath("Bootstrap/bootstrap.min.js")));
            bundles.Add(new StyleBundle("~/content/bootstrap").Include(GetContentPath("Bootstrap/bootstrap.min.css")));
            bundles.Add(new StyleBundle("~/content/font-awesome").Include(GetContentPath("font-awesome/css/font-awesome.min.css")));
        }

        private static string GetScriptPath(string path)
        {
            return ScriptPrefix + path;
        }

        private static string GetContentPath(string path)
        {
            return ContentPrefix + path;
        }

        #endregion Private Method
    }
}