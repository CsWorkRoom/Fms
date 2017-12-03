#region 版本信息
/* ========================================================================
* 【本类功能概述】
* 
* 文件名：EasyManDomainService
* 版本：4.0.30319.42000
* 作者：zl 时间：2016/3/15 16:32:44
* 邮箱：zaixy_8802@126.com
* ========================================================================
*/
#endregion

using Abp.Domain.Entities;
using Abp.Domain.Repositories;
using Abp.Domain.Services;
using Abp.Runtime.Session;

#region 主体



namespace EasyMan
{
    using System.Linq;


    public abstract class EasyManDomainService<T> : EasyManDomainService<T, int> where T : class, IEntity<int>
    {
        protected EasyManDomainService(IRepository<T, int> manageRepository) : base(manageRepository)
        {
        }
    }

    public abstract class EasyManDomainService<T, TPrimaryKey> : DomainService where T : class,IEntity<TPrimaryKey>
    {
        private readonly IRepository<T, TPrimaryKey> _manageRepository;

        public EasyManDomainService(IRepository<T, TPrimaryKey> manageRepository)
        {
            _manageRepository = manageRepository;
            AbpSession = NullAbpSession.Instance;
        }

        public IQueryable<T> Query
        {
            get { return _manageRepository.GetAll(); }
        }

        public IAbpSession AbpSession { get; set; }

    }
}
#endregion
