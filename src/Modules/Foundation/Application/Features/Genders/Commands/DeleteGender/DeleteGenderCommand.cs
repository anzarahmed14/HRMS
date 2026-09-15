using MediatR;

namespace HRMS.Modules.Foundation.Application.Features.Genders.Commands.DeleteGender;

public record DeleteGenderCommand(
    Guid Id) : IRequest;
