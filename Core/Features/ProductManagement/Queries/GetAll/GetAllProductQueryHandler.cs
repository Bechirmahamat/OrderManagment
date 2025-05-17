using Core.Interface;
using Core.Responses;
using MediatR;
using Microsoft.Extensions.Logging;

namespace Core.Features.ProductManagement.Queries.GetAll
{
    public class GetAllProductQueryHandler : IRequestHandler<GetAllProductQuery, GenericResponse>
    {
        private readonly IUnitOfWork unitOfWork;
        private readonly ILogger<GetAllProductQueryHandler> logger;

        public GetAllProductQueryHandler(IUnitOfWork unitOfWork, ILogger<GetAllProductQueryHandler> logger)
        {
            this.unitOfWork = unitOfWork;
            this.logger = logger;
        }
        public async Task<GenericResponse> Handle(GetAllProductQuery request, CancellationToken cancellationToken)
        {
            try
            {
                var result = await unitOfWork.Products.GetAll();
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
                    Message = "An error occurred while Getting all the Product.",
                    Errors = new List<string> { ex.Message }
                };
            }
        }
    }
}
