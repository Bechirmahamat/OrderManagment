using Core.Interface;
using Core.Responses;
using Domain.Entities;
using MediatR;

namespace Core.Features.CompanyManagement.Commands.Create
{
    public class CreateCompanyCommandHandler : IRequestHandler<CreateCompanyCommand, GenericResponse>
    {

        private readonly IAuthRepository authRepository;
        private readonly IUnitOfWork unitOfWork;
        public CreateCompanyCommandHandler(IUnitOfWork unitOfWork, IAuthRepository authRepository)
        {
            this.unitOfWork = unitOfWork;
            this.authRepository = authRepository;
        }


        public async Task<GenericResponse> Handle(CreateCompanyCommand request, CancellationToken cancellationToken)
        {
            try
            {
                var manager = await authRepository.GetUserById(request.ManagerId);
                if (manager == null)
                {
                    return new GenericResponse
                    {
                        Success = false,
                        Message = "Manager not found.",
                        Errors = new List<string> { "Manager not found." }
                    };
                }
                var company = new Company
                {
                    Name = request.Name,
                    Description = request.Description,
                    ManagerId = request.ManagerId
                };
                await unitOfWork.Companies.Create(company);
                await unitOfWork.SaveChangesAsync();

                // update the user role to manager
                var isManager = await authRepository.AddUserRole(request.ManagerId, "Manager");
                if (!isManager)
                {
                    // rollback the company creation
                    await unitOfWork.Companies.Delete(company.Id);
                    await unitOfWork.SaveChangesAsync();
                    return new GenericResponse
                    {
                        Success = false,
                        Message = "Failed to assign manager role.",
                        Errors = new List<string> { "Failed to assign manager role." }
                    };
                }

                return new GenericResponse
                {
                    Success = true,
                    Message = "Company created successfully.",
                    Data = company
                };

            }
            catch (Exception ex)
            {
                return new GenericResponse
                {
                    Success = false,
                    Message = "An error occurred while creating the company.",
                    Errors = new List<string> { ex.Message }
                };
            }
        }
    }
}

