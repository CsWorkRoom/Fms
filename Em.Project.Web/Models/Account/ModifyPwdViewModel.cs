using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace Easyman.Web.Models.Account
{
    public class ModifyPwdViewModel
    {
        [Required]
        public string OldPassword { get; set; }

        [Required]
        public string NewPassword { get; set; }

        [Required]
        public string ConfirmPassword { get; set; }

        public int UserId { get; set; }

        public string TenancyName { get; set; }
    }
}