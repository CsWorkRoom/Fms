using Abp.Domain.Repositories;
using Easyman.Domain;
using EasyMan;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Threading.Tasks;

namespace Easyman.Managers
{
    public class FunctionManager : EasyManDomainService<Function>
    {
        private readonly IRepository<Function> _funRepository;
        private readonly IRepository<FunctionRole, long> _funRoleRepository;
        public FunctionManager(IRepository<Function> funRepository,
            IRepository<FunctionRole, long> funRoleRepository)
            : base(funRepository)
        {
            _funRepository = funRepository;
            _funRoleRepository = funRoleRepository;
        }

        public async Task<Function> GetFunctionAsync(int id)
        {
            var aa = _funRepository.GetAll().Where(a => a.Id == id).ToList().FirstOrDefault();
            var bb = _funRepository.GetAll().Where(a => a.Id == id).ToList();
            var cc = await _funRepository.GetAllListAsync();

            return await _funRepository.FirstOrDefaultAsync(f => f.Id == id);
        }

        public async Task<Function> GetFunctionAsync(string code)
        {
            return await _funRepository.FirstOrDefaultAsync(f => f.Code == code);
        }

        public Function GetFunction(int id)
        {
            return _funRepository.FirstOrDefault(a => a.Id == id);
        }

        public Function GetFunction(string code)
        {
            return _funRepository.FirstOrDefault(a => a.Code == code);
        }

        public IEnumerable<FunctionRole> GetFunctionRolesByRoleId(int roleId)
        {
            return _funRoleRepository.GetAll().Where(w => w.RoleId == roleId);
        }

        public IEnumerable<FunctionRole> GetFunctionRolesByfunId(int funId)
        {
            return _funRoleRepository.GetAll().Where(w => w.FunId == funId);
        }

        public int SaveOrUpdateFunction(Function Function)
        {
            if (_funRepository.GetAll().Any(a => a.Id != Function.Id && a.Code == Function.Code))
            {
                throw new Exception("编码重复");
            }
            else
            {
                //新增或者更新菜单
                var id = _funRepository.InsertOrUpdateAndGetId(Function);
                CurrentUnitOfWork.SaveChanges();
                _funRepository.Update(Function);
                CurrentUnitOfWork.SaveChanges();


                return id;
            }
        }


        public void SetRoles(int funId, string roleIds)
        {

            var fun = GetFunction(funId);
            var roleIdList = roleIds.HasValue() ? roleIds.Split(',') : new string[] { };


            foreach (var funRole in fun.Roles.ToList())
            {
                var role = funRole.Role;

                if (roleIdList.All(roleId => roleId != role.Id.ToString(CultureInfo.InvariantCulture)))
                {
                    _funRoleRepository.Delete(funRole);
                }
            }


            //Add to added roles
            foreach (var roleId in roleIdList)
            {
                var all = fun.Roles.All(ur => ur.RoleId != roleId.ToInt32(0));
                if (all)
                {
                    _funRoleRepository.Insert(new FunctionRole(funId, roleId.ToInt32(0)));
                }
            }
        }

        public void DeleteFungitaion(int funId)
        {
            var fun = GetFunction(funId);

            if (fun == null) return;
            fun.Roles.Each(e => _funRoleRepository.DeleteAsync(e));

            _funRepository.Delete(fun);
        }
    }
}
