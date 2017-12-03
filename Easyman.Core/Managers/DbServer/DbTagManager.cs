using Abp.Domain.Repositories;
using Easyman.Domain;
using EasyMan;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Easyman.Managers
{

    public class DbTagManager : EasyManDomainService<DbTag,long>
    {
        private readonly IRepository<DbTag,long> _dbTagRepository;
        public DbTagManager(IRepository<DbTag,long> dbTagRepository)
            : base(dbTagRepository)
        {
            _dbTagRepository = dbTagRepository;
        }

        public IList<DbTag> GetAllDbTag()
        {
            return _dbTagRepository.GetAllList();
        }

        public DbTag GetDbTag(long id)
        {
            return _dbTagRepository.FirstOrDefault(w => w.Id == id);
        }

        public DbTag GetDbTag(string name)
        {
            return _dbTagRepository.FirstOrDefault(w => w.Name == name);
        }

        public Task<DbTag> GetDbTagAsync(long id)
        {
            return _dbTagRepository.FirstOrDefaultAsync(w => w.Id == id);
        }


        public DbTag SaveOrUpdateDbTag(DbTag dbTag)
        {
            if (_dbTagRepository.GetAll().Any(a => a.Id != dbTag.Id && a.Name == dbTag.Name))
            {
                throw new Exception("数据库标识名重复");
            }
            else
            {
                //新增或者更新
                var tag = _dbTagRepository.InsertOrUpdate(dbTag);
                CurrentUnitOfWork.SaveChanges();

                return tag;
            }
        }

        public void DeleteDbTag(long id)
        {
            var dbTag = GetDbTag(id); 

            if (dbTag == null) return;

            _dbTagRepository.Delete(dbTag);
        }

    }
}
