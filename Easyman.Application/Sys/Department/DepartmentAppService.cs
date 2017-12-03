using Abp.Application.Services.Dto;
using Abp.AutoMapper;
using Easyman.Common;
using Easyman.Domain;
using Easyman.Dto;
using Easyman.Managers;
using EasyMan;
using EasyMan.Dtos;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Easyman.Sys
{
    public class DepartmentAppService : EasymanAppServiceBase, IDepartmentAppService
    {
        

        #region 初始化

        private readonly DepartmentManager _departmentManagerManager;

        public DepartmentAppService(DepartmentManager departmentManagerManager)
        {
            _departmentManagerManager = departmentManagerManager;
        }

        #endregion

        #region 公有方法

        public Department GetDepartment(long id)
        {
            return _departmentManagerManager.GetDepartment(id);
        }

        public Department GetDepartment(string code)
        {

            return _departmentManagerManager.GetDepartment(code);
        }

        public DepartmentSearchOutput GetDpartmentsSearch(DepartmentSearchInput input)
        {
            var parentSearch = input.SearchList.FirstOrDefault(f => f.Name == "ParentName");

            if (parentSearch != null)
            {
                input.SearchList.Remove(parentSearch);
                parentSearch.Name = "Parent.Name";
                input.SearchList.Add(parentSearch);
            }

            var rowCount = 0;
            var navs = _departmentManagerManager.Query.SearchByInputDto(input, out rowCount);
            var outPut = new DepartmentSearchOutput()
            {
                Datas = navs.ToList().Select(s => s.MapTo<DepartmentOutput>()).ToList(),
                Page = new Pager(input.Page) { TotalCount = rowCount }
            };

            return outPut;
        }

        public Task<IEnumerable<object>> GetDepartmentTreeJson()
        {
            return Task.FromResult(_departmentManagerManager.GetDepartmentTree());
        }

        public void SavePost(DepartmentInput input)
        {
            //var depart= Fun.ClassToCopy<DepartmentInput, Department>(input);
            var depart = AutoMapper.Mapper.Map<Department>(input);
            _departmentManagerManager.SaveOrUpdateDepartment(depart);
        }

        public void DeletePost(EntityDto<long> input)
        {
            _departmentManagerManager.DeleteDepartment(input.Id);
        }

        #endregion

    }
}
