using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Easyman.Common.Helper
{
        public class ResultModel
    {
        public ResultModel()
        {
            success = true;
        }
        public bool success { get; set; }

        public string msg { get; set; }

        public dynamic data { get; set; }

        public static ResultModel Success(dynamic data)
        {
            return new ResultModel { data = data };
        }

        public static ResultModel Error(string msg)
        {
            return new ResultModel { success = false, msg = msg };
        }
    }
}
