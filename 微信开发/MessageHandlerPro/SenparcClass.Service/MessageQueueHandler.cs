using Senparc.CO2NET.MessageQueue;
using Senparc.NeuChar.Entities;
using Senparc.Weixin.MP.AdvancedAPIs;
using Senparc.Weixin.MP.Entities;

namespace SenparcClass.Service;
public class MessageQueueHandler
{
    public IResponseMessageBase SendMessage(string openId, string appId, IResponseMessageBase responseMessage)
    {
        var messageQueue = new SenparcMessageQueue();

        if (responseMessage is ResponseMessageText textMessageTimeout)
        {
            {
                var mqKey = SenparcMessageQueue.GenerateKey("MessageQueueHandlerSendMessage", responseMessage.GetType(), Guid.NewGuid().ToString(), "SendMessage");
                Console.WriteLine($"------------{mqKey}------------");

                messageQueue.Add(mqKey, () =>
                {
                    textMessageTimeout.Content += "\r\n\r\n - 这条消息来自客服接口（MessageQueueHandler 队列）-1";

                    //发送客服消息
                    CustomApi.SendText(appId, openId, textMessageTimeout.Content);

                });
            }

            {
                var mqKey = SenparcMessageQueue.GenerateKey("MessageQueueHandlerSendMessage", responseMessage.GetType(), Guid.NewGuid().ToString(), "SendMessage");
                Console.WriteLine($"------------{mqKey}------------");

                messageQueue.Add(mqKey, () =>
                {
                    textMessageTimeout.Content += "\r\n\r\n - 这条消息来自客服接口（MessageQueueHandler 队列）-2";

                    //发送客服消息
                    CustomApi.SendText(appId, openId, textMessageTimeout.Content);

                });
            }

            {
                var mqKey = SenparcMessageQueue.GenerateKey("MessageQueueHandlerSendMessage", responseMessage.GetType(), Guid.NewGuid().ToString(), "SendMessage");
                Console.WriteLine($"------------{mqKey}------------");

                messageQueue.Add(mqKey, () =>
                {
                    textMessageTimeout.Content += "\r\n\r\n - 这条消息来自客服接口（MessageQueueHandler 队列）-3";

                    //发送客服消息
                    CustomApi.SendText(appId, openId, textMessageTimeout.Content);

                });
            }

            return new ResponseMessageNoResponse();
        }

        return responseMessage;
    }
}
