namespace EFCore中发布领域事件的合适时机;

public class User : BaseEntity
{
    public Guid Id { get; init; }
    public string UserName { get; init; }
    public string Email { get; private set; }
    public string? NickName { get; private set; }
    public int? Age { get; private set; }
    public bool IsDeleted { get; private set; }
    private User() { }
    public User(string userName, string email)
    {
        this.Id = Guid.NewGuid();
        this.UserName = userName;
        this.Email = email;
        this.IsDeleted = false;
        AddDomainEvent(new UserAddedEvent(this));
    }

    public void ChangeEmail(string value)
    {
        this.Email = value;
        AddDomainEventIfAbsent(new UserUpdatedEvent(Id));
    }

    public void ChangeNickName(string? value)
    {
        this.NickName = value;
        AddDomainEventIfAbsent(new UserUpdatedEvent(Id));
    }
    public void ChangeAge(int value)
    {
        this.Age = value;
        AddDomainEventIfAbsent(new UserUpdatedEvent(Id));
    }
    public void SoftDelete()
    {
        this.IsDeleted = true;
        AddDomainEvent(new UserSoftDeletedEvent(Id));
    }
}