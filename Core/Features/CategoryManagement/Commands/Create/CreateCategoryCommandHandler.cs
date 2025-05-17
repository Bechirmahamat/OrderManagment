using Core.Interface;
using Core.Responses;
using MediatR;
using Microsoft.Extensions.Logging;

namespace Core.Features.CategoryManagement.Commands.Create
{
    internal class CreateCategoryCommandHandler : IRequestHandler<CreateCategoryCommand, GenericResponse>
    {
        private readonly IUnitOfWork unitOfWork;
        private readonly ILogger<CreateCategoryCommandHandler> logger;

        public CreateCategoryCommandHandler(IUnitOfWork unitOfWork, ILogger<CreateCategoryCommandHandler> logger)
        {
            this.unitOfWork = unitOfWork;
            this.logger = logger;
        }
        public async Task<GenericResponse> Handle(CreateCategoryCommand request, CancellationToken cancellationToken)
        {

            try
            {
                var category = new Domain.Entities.ProductCategory() { Name = request.Name, Description = request.Desciption };
                var result = await unitOfWork.Categories.Create(category);
                if (result == false)
                {
                    return new GenericResponse
                    {
                        Success = false,
                        Message = $"Error happend while creating category .Please try again later",

                    };
                }
                //save 
                await unitOfWork.SaveChangesAsync();
                return new GenericResponse
                {
                    Success = true,
                    Message = "Created Successfully",

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
