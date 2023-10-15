using RabbitMQ.Client;
using RabbitMQ.Client.Events;
using System.Text;

var factory = new ConnectionFactory()
{
    HostName = "127.0.0.1",
    DispatchConsumersAsync = true,
};

string exchangeName = "myExchange";
string eventName = "myEvent";
string queueName = "queue1";

using var conn = factory.CreateConnection();
using var channel = conn.CreateModel();

channel.ExchangeDeclare(exchange: exchangeName, type: "direct");

channel.QueueDeclare(queue: queueName, durable: true, exclusive: false, autoDelete: false, arguments: null);

//表示这个交换机会根据消息routingkey的值进行相等性匹配，消息会发布到和它的routingkey绑定的队列中去
channel.QueueBind(queue: queueName, exchange: exchangeName, routingKey: eventName);

var consumer = new AsyncEventingBasicConsumer(channel);
//阻塞执行
//当一条消息触发的Received的回调方法执行完成后，才会触发下一条消息的Received事件
consumer.Received += Consumer_Received;

//在当前通道中监听 queueName 队列，并进行消费
channel.BasicConsume(queue: queueName, autoAck: false, consumer: consumer);
Console.ReadLine();

//由于同样一条消息可能会被重复投递（出错了），因此我们一定要确保消息处理的代码是幂等的
async Task Consumer_Received(object sender, BasicDeliverEventArgs args)
{
    try
    {
        var bytes = args.Body.ToArray();
        string msg = Encoding.UTF8.GetString(bytes);
        Console.WriteLine(DateTime.Now + "收到了消息" + msg);
        //进行消息的确认
        channel.BasicAck(args.DeliveryTag, multiple: false);
        await Task.Delay(1000);
    }
    catch (Exception ex)
    {
        //安排消息重发
        channel.BasicReject(args.DeliveryTag, true);
        Console.WriteLine("处理收到的消息出错" + ex);
    }
}