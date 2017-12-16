using Abp.Application.Services.Dto;
using Abp.AutoMapper;
using Abp.UI;
using Easyman.Common;
using Easyman.Domain;
using Easyman.Dto;
using Easyman.Managers;
using EasyMan;
using EasyMan.Dtos;
using System;
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
            //var depart = AutoMapper.Mapper.Map<Department>(input);
            var depart = _departmentManagerManager.GetDepartment(input.Id) ?? new Department();
            depart = Fun.ClassToCopy(input, depart, (new string[] { "Id" }).ToList());
            _departmentManagerManager.SaveOrUpdateDepartment(depart);
        }
        /// <summary>
        /// 删除
        /// </summary>
        /// <param name="input"></param>
        public void DeletePost(EntityDto<long> input)
        {
            
                var type = _departmentManagerManager.GetDepartment(input.Id);
            if (type == null)
            {
                throw new UserFriendlyException("操作出错，该部门或已删除！");
            }
                var depar = _departmentManagerManager.GetDepartmentCount(input.Id);
                if (depar > 0)
                {
                    throw new UserFriendlyException("删除出错，该部门下有子部门，请先删除子部门在进行此删除操作！");

                }
                else
                {
                    _departmentManagerManager.DeleteDepartment(input.Id);

                }
           
        }

        #endregion

    }
}
