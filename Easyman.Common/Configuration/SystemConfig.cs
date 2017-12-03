using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Easyman.Common
{
    public class SystemConfig
    {
        public SystemConfig()
        {
            TablePrefix = "WX_";
            IsShowVerifyCode = false;
        }

        public string TablePrefix { get; set; }

        public bool IsShowVerifyCode { get; set; }

        public string WeChatDomain { get; set; }

        public string ServicePublicNumberId { get; set; }

        public bool IsOAuthScopeBase { get; set; }

        /// <summary>
        /// 是否验证短信验证码
        /// </summary>
        public bool IsValidateBindSmsCode { get; set; }
    }
}
