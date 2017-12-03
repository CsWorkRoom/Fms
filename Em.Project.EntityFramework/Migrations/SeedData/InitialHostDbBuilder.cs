using Easyman.EntityFramework;
using EntityFramework.DynamicFilters;

namespace Easyman.Migrations.SeedData
{
    public class InitialHostDbBuilder
    {
        private readonly EmProjectDbContext _context;

        public InitialHostDbBuilder(EmProjectDbContext context)
        {
            _context = context;
        }

        public void Create()
        {
            _context.DisableAllFilters();

            new DefaultEditionsCreator(_context).Create();
            new DefaultLanguagesCreator(_context).Create();
            new HostRoleAndUserCreator(_context).Create();
            new DefaultSettingsCreator(_context).Create();
            new DefaultNavigationBuilder(_context).Create();
        }
    }
}
