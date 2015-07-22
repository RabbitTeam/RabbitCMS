using Rabbit.Users.Models;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace Rabbit.Users.ViewModels
{
    public class UserEditViewModel
    {
        public string Id { get; set; }

        [DisplayName("用户名称"), Required, StringLength(20)]
        public string UserName { get; set; }

        [DisplayName("账号名称"), Required, StringLength(20)]
        public string AccountName { get; set; }

        [DisplayName("密码"), StringLength(20, MinimumLength = 5)]
        public string Password { get; set; }

        [DisplayName("确认密码"), Compare("Password")]
        public string ConfirmPassword { get; set; }

        public static explicit operator UserEditViewModel(UserRecord record)
        {
            return new UserEditViewModel
            {
                Id = record.Id,
                AccountName = record.Account.Account,
                Password = record.Account.Password,
                UserName = record.Name
            };
        }

        public UserRecord UpdateRecord(UserRecord record)
        {
            record.Name = UserName;
            record.Account.Account = AccountName;
            if (!string.IsNullOrEmpty(Password))
                record.Account.SetPassword(Password);

            return record;
        }
    }
}