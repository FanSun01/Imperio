namespace DomainEvents.Infrastructure
{
    public interface IDomainEventHandler<in TDomainEvent> where TDomainEvent : IDomainEvent
    {
        Task HandleAysnc(TDomainEvent domainEvent, CancellationToken cancellationToken = default);
    }


}
