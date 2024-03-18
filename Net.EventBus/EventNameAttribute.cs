using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Net.EventBus
{
    /// <summary>
    /// 标记到事件消费者身上，可多标记
    /// </summary>
    [AttributeUsage(AttributeTargets.Class,AllowMultiple =true)]
    public class EventNameAttribute:Attribute
    {
        public string Name { get; set; }

        public EventNameAttribute(string name)
        {
            this.Name = name;
        }
    }
}
