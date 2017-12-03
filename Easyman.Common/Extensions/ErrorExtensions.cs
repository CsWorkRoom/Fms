#region 版本信息
/* ========================================================================
* 【本类功能概述】
* 
* 文件名：ErrorExtensions
* 版本：4.0.30319.42000
* 作者：zl 时间：2016/3/16 17:42:25
* 邮箱：zaixy_8802@126.com
* ========================================================================
*/
#endregion

using System.Data.Entity.Validation;

#region 主体



namespace EasyMan
{
    using System;
    using System.Linq;
    using System.Text;


    public static class ErrorExtensions
    {
        public static string GetTrueMessage(this Exception exception)
        {
            var validationException = exception as DbEntityValidationException;
            if (validationException != null)
            {
                return GetValidationExceptionMessage(validationException);
            }
            else
            {
                while (exception.InnerException!=null)
                {
                    exception = exception.InnerException;
                }

                return exception.Message;
            }
        }

        private static string GetValidationExceptionMessage(DbEntityValidationException exception)
        {
            var messageBuilder = new StringBuilder();


            foreach (var error in exception.EntityValidationErrors)
            {
                messageBuilder.Append("{0},".FormatWith(error.ValidationErrors.First().ErrorMessage));
            }

            return messageBuilder.ToString();
        }
    }
}
#endregion
