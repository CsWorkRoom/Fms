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
    public class DepartmentManager : EasyManDomainService<Department,long>
    {
        private readonly IRepository<Department,long> _departmentRepository;
        public DepartmentManager(IRepository<Department,long> departmentRepository)
            : base(departmentRepository)
        {
            _departmentRepository = departmentRepository;
        }

        public Department GetDepartment(long id)
        {
            return _departmentRepository.FirstOrDefault(w => w.Id == id);
        }

        public Department GetDepartment(string code)
        {
            return _departmentRepository.FirstOrDefault(w => w.Code == code);
        }

        public Task<Department> GetDepartmentAsync(long id)
        {
            return _departmentRepository.FirstOrDefaultAsync(w => w.Id == id);
        }

        public IEnumerable<object> GetDepartmentTree()
        {
            var departments = Query;

            var departmentNodes = departments.Select(s => new
            {
                id = s.Id,
                name = s.Name,
                open = false,
                pId = s.ParentId,
                iconSkin = s.Children.Any() ? "root" : "menu"
            }).ToList();

            return departmentNodes;
        }

        public long SaveOrUpdateDepartment(Department department)
        {
            if (_departmentRepository.GetAll().Any(a => a.Id != department.Id  && a.TenantId == department.TenantId
                                               && (a.Code == department.Code|| a.Name == department.Name)))
            {
                throw new Exception("名称或编码重复");
            }
            else
            {
                //新增或者更新
                var id = _departmentRepository.InsertOrUpdateAndGetId(department);
                CurrentUnitOfWork.SaveChanges();

                return id;
            }
        }

        public void DeleteDepartment(long departmentId)
        {
            var department = GetDepartment(departmentId);

            if (department == null) return;

            _departmentRepository.Delete(department);
        }

    }
}
