namespace Net.EventBus
{
    public interface IIntegrationEventHandler
    {
        /// <summary>
        /// 事件被触发被调用
        /// </summary>
        /// <param name="eventName"></param>
        /// <param name="eventData"></param>
        /// <returns></returns>
        Task Handle(string eventName, string eventData);
    }
}