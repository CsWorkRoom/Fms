using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace Easyman.Web.Models.Account
{
    public class ModifyPwdViewModel
    {
        [Required(ErrorMessage = "旧密码不能为空")]
        public string OldPassword { get; set; }

        [Required(ErrorMessage = "新密码不能为空")]
        public string NewPassword { get; set; }

        [Required(ErrorMessage = "新确认密码不能为空")]
        public string ConfirmPassword { get; set; }

        public int UserId { get; set; }

        public string TenancyName { get; set; }
    }
}