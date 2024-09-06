using BuildingBlocks.CQRS.Command;
using BuildingBlocks.CQRS.Query;
using FluentValidation;

namespace BuildingBlocks.CQRS.Behaviors;

public class CommandValidationBehaviors<TRequest, TResponse>(IEnumerable<IValidator<TRequest>> validators)
    : BaseValidator<TRequest, TResponse>(validators)
    where TRequest : ICommand<TResponse> where TResponse : notnull
{
}

public class QueryValidationBehaviors<TRequest, TResponse>(IEnumerable<IValidator<TRequest>> validators)
    : BaseValidator<TRequest, TResponse>(validators)
    where TRequest : IQuery<TResponse> where TResponse : notnull
{
}