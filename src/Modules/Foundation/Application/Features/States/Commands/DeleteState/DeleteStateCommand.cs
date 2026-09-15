using MediatR;

namespace HRMS.Modules.Foundation.Application.Features.States.Commands.DeleteState;

public sealed record DeleteStateCommand(
    Guid Id) : IRequest;
