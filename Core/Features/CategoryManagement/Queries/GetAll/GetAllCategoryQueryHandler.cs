using Core.Interface;
using Core.Responses;
using MediatR;
using Microsoft.Extensions.Logging;

namespace Core.Features.CategoryManagement.Queries.GetAll
{
    public class GetAllCategoryQueryHandler : IRequestHandler<GetAllCategoryQuery, GenericResponse>
    {

        private readonly IUnitOfWork unitOfWork;
        private readonly ILogger<GetAllCategoryQueryHandler> logger;

        public GetAllCategoryQueryHandler(IUnitOfWork unitOfWork, ILogger<GetAllCategoryQueryHandler> logger)
        {
            this.unitOfWork = unitOfWork;
            this.logger = logger;
        }


        public async Task<GenericResponse> Handle(GetAllCategoryQuery request, CancellationToken cancellationToken)
        {
            try
            {
                var result = await unitOfWork.Categories.GetAll();
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
