using AutoMapper;
using HRMS.Application.Features.BankAccounts.DTOs;
using HRMS.BuildingBlocks.Application.Abstractions.Persistence;
using HRMS.Modules.Employee.Domain.Entities;
using MediatR;

namespace HRMS.Application.Features.BankAccounts.Queries.GetBankAccountById;

public class GetBankAccountByIdQueryHandler
    : IRequestHandler<GetBankAccountByIdQuery, BankAccountDto?>
{
    private readonly IReadRepository<BankAccount, Guid> _repository;
    private readonly IMapper _mapper;

    public GetBankAccountByIdQueryHandler(
        IReadRepository<BankAccount, Guid> repository,
        IMapper mapper)
    {
        _repository = repository;
        _mapper = mapper;
    }

    public async Task<BankAccountDto?> Handle(
        GetBankAccountByIdQuery request,
        CancellationToken cancellationToken)
    {
        var bankAccount = await _repository.GetByIdAsync(
            request.Id,
            cancellationToken);

        if (bankAccount is null)
            return null;

        return _mapper.Map<BankAccountDto>(bankAccount);
    }
}
