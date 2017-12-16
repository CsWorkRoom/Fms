using Easyman.DefaultData;
using Easyman.Domain;
using Easyman.EntityFramework;
using System.Linq;

namespace Easyman.Migrations.SeedData
{
    public class DefaultNavigationBuilder
    {
        private readonly EmProjectDbContext _context;

        public DefaultNavigationBuilder(EmProjectDbContext context)
        {
            _context = context;
        }

        public void Create()
        {
            CreateDefualtNavgiation();
        }


        private void CreateDefualtNavgiation()
        {
            StaticDatas.Navigations.DefaultNavs.ForEach(e =>
            {
                var nav = _context.Module.FirstOrDefault(r => r.Code == e.Code);
                if (nav == null)
                {
                    var module = new Module() {
                        Name = e.Name,
                        Code = e.Code,
                        ShowOrder = e.ShowOrder,
                        ParentId = e.ParentId,
                        ApplicationType=e.ApplicationType,
                        Url = e.Url,
                        Icon=e.Icon,
                        IsUse=e.IsUse,
                        TenantId = e.TenantId };
                    _context.Module.Add(module);

                    _context.SaveChanges();
                }
            });
        }
    }
}
