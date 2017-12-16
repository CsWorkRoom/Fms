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
    public class DistrictAppService : EasymanAppServiceBase, IDistrictAppService
    {
        #region 初始化

        private readonly DistrictManager _districtManagerManager;

        public DistrictAppService(DistrictManager districtManagerManager)
        {
            _districtManagerManager = districtManagerManager;
        }

        #endregion

        #region 公有方法

       
        /// <summary>
        /// 获取所有组织
        /// </summary>
        /// <returns></returns>
        public List<District> GetAllDistrcit()
        {

            var data = _districtManagerManager.GetAllDistrcit();

            return data;

        }

        public District GetDistrict(long id)
        {
            return _districtManagerManager.GetDistrict(id);
        }

        public District GetDistrict(string code)
        {

            return _districtManagerManager.GetDistrict(code);
        }

        public DistrictSearchOutput GetDpartmentsSearch(DistrictSearchInput input)
        {
            var parentSearch = input.SearchList.FirstOrDefault(f => f.Name == "ParentName");

            if (parentSearch != null)
            {
                input.SearchList.Remove(parentSearch);
                parentSearch.Name = "Parent.Name";
                input.SearchList.Add(parentSearch);
            }

            var rowCount = 0;
            var navs = _districtManagerManager.Query.SearchByInputDto(input, out rowCount);
            var outPut = new DistrictSearchOutput()
            {
                Datas = navs.ToList().Select(s => s.MapTo<DistrictOutput>()).ToList(),
                Page = new Pager(input.Page) { TotalCount = rowCount }
            };

            return outPut;
        }

        public Task<IEnumerable<object>> GetDistrictTreeJson()
        {
            return Task.FromResult(_districtManagerManager.GetDistrictTree());
        }

        public void SavePost(DistrictInput input)
        {
            //var depart = Fun.ClassToCopy<DistrictInput, District>(input);
            //var depart = AutoMapper.Mapper.Map<District>(input);
            var depart = _districtManagerManager.GetDistrict(input.Id) ?? new District();
            depart = Fun.ClassToCopy(input, depart, (new string[] { "Id" }).ToList());
            _districtManagerManager.SaveOrUpdateDistrict(depart);
        }

        public void DeletePost(EntityDto<long> input)
        {
            _districtManagerManager.DeleteDistrict(input.Id);
        }

        #endregion

    }
}
