using MediatR;

namespace EFCore中发布领域事件的合适时机.EventHandler
{
    public class ModifyUserLogHandler : INotificationHandler<UserUpdatedEvent>
    {
        private readonly UserDbContext context;
        private readonly ILogger<ModifyUserLogHandler> logger;
        public ModifyUserLogHandler(UserDbContext context, ILogger<ModifyUserLogHandler> logger)
        {
            this.context = context;
            this.logger = logger;
        }
        public async Task Handle(UserUpdatedEvent notification, CancellationToken cancellationToken)
        {
            /*
             * FindAsync会首先从上下文的缓存中获取对象，而修改操作之前被修改的对象已经存在于缓存中了，
             * 所以用FindAsync不仅能够获取还没有提交到数据库的对象，而且由于FindAsync操作不会再到数据库中查询，因此程序的性能更高
             */
            var user = await context.Users.FindAsync(notification.Id);

            logger.LogInformation($"通知用户{user.Email}的信息被修改");
        }
    }
}
