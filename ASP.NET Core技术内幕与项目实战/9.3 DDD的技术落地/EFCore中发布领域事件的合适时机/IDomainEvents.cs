using MediatR;

namespace EFCore中发布领域事件的合适时机;

//供聚合根进行事件注册的接口
public interface IDomainEvents
{
    IEnumerable<INotification> GetDomainEvents();//获取注册的领域事件
    void AddDomainEvent(INotification eventItem);//注册领域事件
    void AddDomainEventIfAbsent(INotification eventItem);//如果领域事件不存在，则注册事件
    void ClearDomainEvents();                            //清除注册的领域事件
}