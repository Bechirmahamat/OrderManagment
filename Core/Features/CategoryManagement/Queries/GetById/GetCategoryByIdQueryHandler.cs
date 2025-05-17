
using Core.Interface;
using Core.Responses;
using MediatR;
using Microsoft.Extensions.Logging;

namespace Core.Features.CategoryManagement.Queries.GetById
{
    public class GetCategoryByIdQueryHandler : IRequestHandler<GetCategoryByIdQuery, GenericResponse>
    {
        private readonly IUnitOfWork unitOfWork;
        private readonly ILogger<GetCategoryByIdQueryHandler> logger;

        public GetCategoryByIdQueryHandler(IUnitOfWork unitOfWork, ILogger<GetCategoryByIdQueryHandler> logger)
        {
            this.unitOfWork = unitOfWork;
            this.logger = logger;
        }

        public async Task<GenericResponse> Handle(GetCategoryByIdQuery request, CancellationToken cancellationToken)
        {
            try
            {
                var result = await unitOfWork.Categories.GetById(request.id);
                if (result == null)
                {
                    return new GenericResponse
                    {
                        Success = false,
                        Message = $"Category with id {request.id} dont extis.",

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
