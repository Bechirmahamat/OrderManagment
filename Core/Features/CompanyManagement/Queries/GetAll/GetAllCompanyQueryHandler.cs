using Core.Interface;
using Core.Responses;
using MediatR;
using Microsoft.Extensions.Logging;

namespace Core.Features.CompanyManagement.Queries.GetAll
{
    public class GetAllCompanyQueryHandler : IRequestHandler<GetAllCompanyQuery, GenericResponse>
    {

        private readonly IUnitOfWork unitOfWork;
        private readonly ILogger<GetAllCompanyQueryHandler> logger;

        public GetAllCompanyQueryHandler(IUnitOfWork unitOfWork, ILogger<GetAllCompanyQueryHandler> logger)
        {
            this.unitOfWork = unitOfWork;
            this.logger = logger;
        }


        public async Task<GenericResponse> Handle(GetAllCompanyQuery request, CancellationToken cancellationToken)
        {
            try
            {
                var result = await unitOfWork.Companies.GetAll();
                return new GenericResponse
                {
                    Success = true,
                    Message = "successfully Executed.",
                    Data = result
                }
                  ;

            }
            catch (Exception ex)
            {
                logger.LogError(ex.Message);
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
