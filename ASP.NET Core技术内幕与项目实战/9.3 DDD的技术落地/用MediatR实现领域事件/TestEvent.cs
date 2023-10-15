using MediatR;

namespace 用MediatR实现领域事件;

//在事件的发布者和处理者之间进行数据传递的类
public record TestEvent(string UserName) : INotification;
