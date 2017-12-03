using Abp.UI;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Easyman.Common
{
    public static class ExceptionHelper
    {
        public static void ErrorMsg(this string value)
        {
            throw new UserFriendlyException(value);
        }

        public static void NoneException(this DataRow dr, string columnName, object value)
        {
            try
            {
                dr[columnName] = value;
            }
            catch { }
        }
    }
}
