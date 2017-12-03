using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Easyman.Dto
{
    public class FuncsModel
    {
        public FuncsModel()
        {
            Functions = new List<FuncModel>();
        }
        public List<FuncModel> Functions { get; set; }
    }
    public class FuncModel
    {
        public int Id { get; set; }
        public string Code { get; set; }

        public string Name { get; set; }
    }
}
