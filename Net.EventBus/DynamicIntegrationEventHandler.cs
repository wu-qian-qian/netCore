using Dynamic.Json;
namespace Net.EventBus
{
    public abstract class DynamicIntegrationEventHandler : IIntegrationEventHandler
    {
        public  Task Handle(string eventName, string eventData)
        {
            dynamic dyna = DJson.Parse(eventData);
            return HandleDynamic(eventName, eventData);
        }
        /// <summary>
        /// 业务逻辑
        /// </summary>
        /// <param name="eveName"></param>
        /// <param name="dynamic"></param>
        /// <returns></returns>
        public abstract Task HandleDynamic(string eveName,dynamic dynamic);
    }
}
