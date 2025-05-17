
using Core.Interface;
using Core.Responses;
using MediatR;
using Microsoft.Extensions.Logging;

namespace Core.Features.ProductManagement.Queries.GetById
{
    internal class GetProductByIdQueryHandler : IRequestHandler<GetProductByIdQuery, GenericResponse>
    {
        private readonly IUnitOfWork unitOfWork;
        private readonly ILogger<GetProductByIdQueryHandler> logger;

        public GetProductByIdQueryHandler(IUnitOfWork unitOfWork, ILogger<GetProductByIdQueryHandler> logger)
        {
            this.unitOfWork = unitOfWork;
            this.logger = logger;
        }

        public async Task<GenericResponse> Handle(GetProductByIdQuery request, CancellationToken cancellationToken)
        {
            try
            {
                var result = await unitOfWork.Products.GetById(request.id);
                if (result == null)
                {
                    return new GenericResponse
                    {
                        Success = false,
                        Message = $"Product with id {request.id} dont extis.",

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
                    Message = "An error occurred while creating the Product.",
                    Errors = new List<string> { ex.Message }
                };

            }
        }
    }
}
