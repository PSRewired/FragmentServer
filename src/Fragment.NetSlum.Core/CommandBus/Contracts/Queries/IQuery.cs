using MediatR;

namespace Fragment.NetSlum.Core.CommandBus.Contracts.Queries;

public interface IQuery<out TResult> : IRequest<TResult> { }
