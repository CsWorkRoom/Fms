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
    public class DistrictManager : EasyManDomainService<District,long>
    {
        private readonly IRepository<District,long> _districtRepository;
        public DistrictManager(IRepository<District,long> districtRepository)
            : base(districtRepository)
        {
            _districtRepository = districtRepository;
        }


        /// <summary>
        /// 获取所有组织
        /// </summary>
        /// <returns></returns>
        public List<District> GetAllDistrcit()
        {

            var data = _districtRepository.GetAll().Where(x => x.IsUse == true);

            return data.ToList();

        }
        public District GetDistrict(long id)
        {
            return _districtRepository.FirstOrDefault(w => w.Id == id);
        }

        public District GetDistrict(string code)
        {
            return _districtRepository.FirstOrDefault(w => w.Code == code);
        }

        public Task<District> GetDistrictAsync(long id)
        {
            return _districtRepository.FirstOrDefaultAsync(w => w.Id == id);
        }

        public IEnumerable<object> GetDistrictTree()
        {
            var districts = Query;

            var districtNodes = districts.Select(s => new
            {
                id = s.Id,
                name = s.Name,
                open = false,
                pId = s.ParentId,
                iconSkin = s.Children.Any() ? "root" : "menu"
            }).ToList();

            return districtNodes;
        }

        public long SaveOrUpdateDistrict(District district)
        {
            if (_districtRepository.GetAll().Any(a => a.Id != district.Id  && a.TenantId == district.TenantId
                                               && (a.Code == district.Code|| a.Name == district.Name)))
            {
                throw new Exception("名称或编码重复");
            }
            else
            {
                if(district.ParentId==null)
                {
                    district.CurLevel = 1;
                }
                else
                {
                    district.CurLevel = _districtRepository.Get(district.ParentId.Value).CurLevel + 1;//生成当前层级
                }
                //新增或者更新
                var id = _districtRepository.InsertOrUpdateAndGetId(district);
                CurrentUnitOfWork.SaveChanges();

                return id;
            }
        }

        public void DeleteDistrict(long districtId)
        {
            var district = GetDistrict(districtId);

            if (district == null) return;

            _districtRepository.Delete(district);
        }

    }
}
