using Abp.UI;
using Castle.DynamicProxy;
using System;
using System.Data;
using Abp.Runtime.Validation;

namespace Easyman
{
    /// <summary>
    /// 异常处理AOP
    /// </summary>
    public class ExceptionInterceptor : IInterceptor
    {
        public void Intercept(IInvocation invocation)
        {
            try
            {
                invocation.Proceed();
            }
            //ef 校验异常
            catch (DataException ex)
            {
                throw new UserFriendlyException(ex.InnerException.InnerException.Message);
            }
            //校验异常
            catch(AbpValidationException)
            {
                throw ;
            }
            catch (Exception e)
            {
                throw new UserFriendlyException(e.Message);
            }
        }
    }
}
