using Core.Interface;
using Core.Responses;
using FluentValidation;
using MediatR;
using Microsoft.Extensions.Logging;

namespace Core.Features.ProductManagement.Commands.Create
{
    internal class CreateProductCommandhandler : IRequestHandler<CreateProductCommand, GenericResponse>
    {

        private readonly IUnitOfWork unitOfWork;
        private readonly ILogger<CreateProductCommandhandler> logger;
        private readonly IValidator<CreateProductCommand> validator;

        public CreateProductCommandhandler(IUnitOfWork unitOfWork, ILogger<CreateProductCommandhandler> logger, IValidator<CreateProductCommand> validator)
        {
            this.unitOfWork = unitOfWork;
            this.logger = logger;
            this.validator = validator;
        }
        public async Task<GenericResponse> Handle(CreateProductCommand request, CancellationToken cancellationToken)
        {
            try
            {
                //validate the request
                var validationResult = await validator.ValidateAsync(request, cancellationToken);
                if (!validationResult.IsValid)
                {
                    return new GenericResponse
                    {
                        Success = false,
                        Message = "Validation failed",
                        Errors = validationResult.Errors.Select(e => e.ErrorMessage).ToList()
                    };
                }
                //check if the category exists
                var category = await unitOfWork.Categories.GetById(request.CategoryId);
                if (category == null)
                {
                    return new GenericResponse
                    {
                        Success = false,
                        Message = $"Category with id {request.CategoryId} not found.",
                    };
                }
                //check if the company exists
                var company = await unitOfWork.Companies.GetCompanyByManagerId(request.ManagerId);
                if (company == null)
                {
                    return new GenericResponse
                    {
                        Success = false,
                        Message = $"Company with id {request.ManagerId} not found.",
                    };
                }
                //create the product
                var result = await unitOfWork.Products.Create(new Domain.Entities.Product()
                {
                    Name = request.Name,
                    Description = request.Description,
                    Price = request.Price,
                    StockCount = request.StockCount,
                    CategoryId = request.CategoryId,
                    CompanyId = company.Id
                });
                if (result == false)
                {
                    return new GenericResponse
                    {
                        Success = false,
                        Message = $"Error happend while creating product .Please try again later",
                    };
                }
                //save
                await unitOfWork.SaveChangesAsync();
                return new GenericResponse
                {
                    Success = true,
                    Message = "Created Successfully",
                };

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
