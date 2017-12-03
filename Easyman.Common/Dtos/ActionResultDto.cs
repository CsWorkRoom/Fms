#region 版本信息
/* ========================================================================
* 【本类功能概述】
* 
* 文件名：ActionResultDto
* 版本：4.0.30319.42000
* 作者：zl 时间：2016/4/15 13:25:21
* 邮箱：zaixy_8802@126.com
* ========================================================================
*/
#endregion

using Abp.Application.Services.Dto;

#region 主体



namespace EasyMan.Dtos
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using System.Threading.Tasks;


    public class ActionResultDto : IEntityDto
    {
        private ActionResultDto()
        {
        }

        public string Message { get; set; }
        public bool IsSuccess { get; set; }

        public int Id { get; set; }

        public static ActionResultDto Success(string message)
        {
            return  new ActionResultDto()
            {
                IsSuccess = true,
                Message = message
            };
        }

        public static ActionResultDto Error(string message)
        {
            return new ActionResultDto()
            {
                IsSuccess = false,
                Message = message
            };
        }
    }
}
#endregion
