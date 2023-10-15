using MediatR;
using Users.Domain.Entities;

namespace Users.Domain.Events;

public record class UserAccessResultEvent(PhoneNumber PhoneNumber, UserAccessResult Result) : INotification;