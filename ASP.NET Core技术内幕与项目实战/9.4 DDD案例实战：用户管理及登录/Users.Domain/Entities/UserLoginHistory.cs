namespace Users.Domain.Entities;

public record UserLoginHistory : IAggregateRoot
 {
     public long Id { get; init; }
     public Guid? UserId { get; init; }            //用户ID
     public PhoneNumber PhoneNumber { get; init; } //手机号
     public DateTime CreatedDateTime { get; init; }//时间
     public string Messsage { get; init; }         //消息
     private UserLoginHistory() { }
     public UserLoginHistory(Guid? userId, PhoneNumber phoneNumber, string message)
     {
         this.UserId = userId;
         this.PhoneNumber = phoneNumber;
         this.CreatedDateTime = DateTime.Now;
         this.Messsage = message;
     }
 }