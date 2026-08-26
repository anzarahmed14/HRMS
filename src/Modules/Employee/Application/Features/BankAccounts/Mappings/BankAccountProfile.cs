using AutoMapper;
using HRMS.Application.Features.BankAccounts.Commands.CreateBankAccount;
using HRMS.Application.Features.BankAccounts.Commands.UpdateBankAccount;
using HRMS.Application.Features.BankAccounts.DTOs;
using HRMS.Modules.Employee.Domain.Entities;

namespace HRMS.Application.Features.BankAccounts.Mappings;

public class BankAccountProfile : Profile
{
    public BankAccountProfile()
    {
        CreateMap<CreateBankAccountCommand, BankAccount>();

        CreateMap<UpdateBankAccountCommand, BankAccount>();

        CreateMap<BankAccount, BankAccountDto>();
    }
}
