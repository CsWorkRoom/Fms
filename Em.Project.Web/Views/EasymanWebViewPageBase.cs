using Abp.Web.Mvc.Views;

namespace Easyman.Web.Views
{
    public abstract class EasymanWebViewPageBase : EasymanWebViewPageBase<dynamic>
    {

    }

    public abstract class EasymanWebViewPageBase<TModel> : AbpWebViewPage<TModel>
    {
        protected EasymanWebViewPageBase()
        {
            LocalizationSourceName = EasymanConsts.LocalizationSourceName;
        }
    }
}