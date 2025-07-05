namespace BlazorHRManagement.Api.Abstraction;

public interface IMediator
{
    Task<IMediator> sendCommandAsync<TCommand,TResult>(TCommand command,CancellationToken cancellationToken );

    Task<IMediator> sendQueryAsync<TQuery, Tresult>(TQuery command, CancellationToken cancellationToken);
}
