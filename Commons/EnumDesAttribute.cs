using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
namespace Asp.NetCore
{
    [System.AttributeUsage(AttributeTargets.All, Inherited = false, AllowMultiple =
    false)]
    public class EnumDesAttribute : Attribute
    {
        public string Description { get; init; }
        public EnumDesAttribute(string des)
        {
            Description = des;
        }
        public string GetDescription()
        {
            return Description;
        }
    }
}