using AutoMapper;
using HRMS.Application.Features.Documents.Commands.CreateEmployeeDocument;
using HRMS.Application.Features.Documents.Commands.UpdateEmployeeDocument;
using HRMS.Application.Features.Documents.DTOs;
using HRMS.Modules.Employee.Domain.Entities;

namespace HRMS.Application.Features.Documents.Mappings;

public class EmployeeDocumentProfile : Profile
{
    public EmployeeDocumentProfile()
    {
        CreateMap<CreateEmployeeDocumentCommand, EmployeeDocument>();

        CreateMap<UpdateEmployeeDocumentCommand, EmployeeDocument>();

        CreateMap<EmployeeDocument, EmployeeDocumentDto>();
    }
}

