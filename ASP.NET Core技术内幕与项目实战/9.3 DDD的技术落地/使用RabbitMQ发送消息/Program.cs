using RabbitMQ.Client;
using System.Text;

var factory = new ConnectionFactory()
{
    //RabbitMQ服务器地址
    HostName = "127.0.0.1",
    //异步调度消费者
    DispatchConsumersAsync = true
};

string exchangeName = "myExchange";                  //交换机的名字
string eventName = "myEvent";                       //routingKey的值

//创建了一个客户端到RabbitMQ的TCP连接
//这个TCP连接我们尽量重复使用
using var conn = factory.CreateConnection();
while (true)
{
    string msg = DateTime.Now.TimeOfDay.ToString(); //待发送消息

    //信道关闭之后，消息才会发出
    using (var channel = conn.CreateModel())        //创建虚似信道
    {
        var properties = channel.CreateBasicProperties();
        properties.DeliveryMode = 2;

        /*
         * 使用ExchangeDeclare方法声明了一个指定名字的交换机，如果指定名字的交换机已经存在，则不再重复创建
         * direct表示这个交换机会根据消息routingkey的值进行相等性匹配，消息会发布到和它的routingkey绑定的队列中去
         */
        channel.ExchangeDeclare(exchange: exchangeName, type: "direct");//声明交换机（用于把消息路由到一个或者多个队列中）

        byte[] body = Encoding.UTF8.GetBytes(msg);

        channel.BasicPublish(exchange: exchangeName, routingKey: eventName,
            mandatory: true, basicProperties: properties, body: body);    //发布消息
    }

    Console.WriteLine("发布了消息:" + msg);
    Thread.Sleep(1000);
}