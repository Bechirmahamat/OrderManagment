
using Core.Interface;
using Core.Responses;
using MediatR;
using Microsoft.Extensions.Logging;

namespace Core.Features.CompanyManagement.Queries.GetById
{
    public class GetCompanyIdQueryHandler : IRequestHandler<GetCompnayByIdQuery, GenericResponse>
    {
        private readonly IUnitOfWork unitOfWork;
        private readonly ILogger<GetCompanyIdQueryHandler> logger;

        public GetCompanyIdQueryHandler(IUnitOfWork unitOfWork, ILogger<GetCompanyIdQueryHandler> logger)
        {
            this.unitOfWork = unitOfWork;
            this.logger = logger;
        }

        public async Task<GenericResponse> Handle(GetCompnayByIdQuery request, CancellationToken cancellationToken)
        {
            try
            {
                var result = await unitOfWork.Companies.GetById(request.id);
                if (result == null)
                {
                    return new GenericResponse
                    {
                        Success = false,
                        Message = $"Company with id {request.id} dont extis.",

                    };
                }
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
