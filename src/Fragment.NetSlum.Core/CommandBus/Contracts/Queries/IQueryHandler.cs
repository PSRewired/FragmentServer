namespace Fragment.NetSlum.Core.CommandBus.Contracts.Queries;

public interface IQueryHandler<in TQuery, TResult> where TQuery : IQuery<TResult>
{
    public Task<TResult> Handle(TQuery command, CancellationToken cancellationToken);
}
