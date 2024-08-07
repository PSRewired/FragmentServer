using System.Threading;
using System.Threading.Tasks;

namespace Fragment.NetSlum.Core.CommandBus.Contracts.Queries;

public interface IQueryHandler<in TQuery, TResult> where TQuery : IQuery<TResult>
{
    public ValueTask<TResult> Handle(TQuery command, CancellationToken cancellationToken);
}
