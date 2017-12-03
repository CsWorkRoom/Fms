using Abp.Application.Services;
using Abp.MultiTenancy;
using Abp.Zero.EntityFramework;
using Easyman.Migrations.SeedData;
using EntityFramework.DynamicFilters;
using System.Data.Entity.Migrations;

namespace Easyman.Migrations
{
    public sealed class Configuration : DbMigrationsConfiguration<Easyman.EntityFramework.EmProjectDbContext>, IMultiTenantSeed
    {
        public AbpTenantBase Tenant { get; set; }

        public Configuration()
        {
            AutomaticMigrationsEnabled = false;
            ContextKey = "Easyman";
        }

        protected override void Seed(Easyman.EntityFramework.EmProjectDbContext context)
        {
            context.DisableAllFilters();

            if (Tenant == null)
            {
                //Host seed
                new InitialHostDbBuilder(context).Create();

                //Default tenant seed (in host database).
                new DefaultTenantCreator(context).Create();
                new TenantRoleAndUserBuilder(context, 1).Create();

                new FrameDataInitCreator(context).Create();//部分--框架数据初始化
            }
            else
            {
                //You can add seed for tenant databases and use Tenant property...
            }

            context.SaveChanges();
        }
    }
}
