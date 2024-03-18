using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Net.EventBus
{
    public  interface IEventBus
    {
        /// <summary>
        /// 调用事件
        /// </summary>
        /// <param name="eventName"></param>
        /// <param name="josnData"></param>
        void Publish(string eventName, string josnData);
        /// <summary>
        /// 把事件消费者存入内存管理器
        /// </summary>
        /// <param name="eventName"></param>
        /// <param name="handlerType"></param>
        void Subscribe(string eventName, Type handlerType);
        /// <summary>
        /// 把事件从内存管理器中移除
        /// </summary>
        /// <param name="eventName"></param>
        /// <param name="handlerType"></param>
        void UnSubscribe(string eventName, Type handlerType);
    }
}
