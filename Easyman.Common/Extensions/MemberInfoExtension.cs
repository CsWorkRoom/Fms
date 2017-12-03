using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using EasyMan;

namespace Easyman
{
    public static class MemberInfoExtension
    {
        public static string GetSelectField(this PropertyInfo property)
        {

            if (property.PropertyType == typeof(int) || property.PropertyType == typeof(long))
            {
                return @"{0}".FormatWith(property.Name);
            }
            else
            {
                return @"'{0}'".FormatWith(property.Name);
            }
        }
    }
}
