using Core.Interface;
using Core.Responses;
using Domain.Entities;
using MediatR;
using Microsoft.Extensions.Logging;

namespace Core.Features.OrderManagement.Commands.Create
{

    internal class CreateOrderCommandHandler : IRequestHandler<CreateOrderCommand, GenericResponse>
    {
        private readonly IUnitOfWork unitOfWork;
        private readonly ILogger<CreateOrderCommandHandler> logger;
        private GenericResponse Failure(string message) => new GenericResponse { Success = false, Message = message };
        public CreateOrderCommandHandler(IUnitOfWork unitOfWork, ILogger<CreateOrderCommandHandler> logger)
        {
            this.unitOfWork = unitOfWork;
            this.logger = logger;
        }
        public async Task<GenericResponse> Handle(CreateOrderCommand request, CancellationToken cancellationToken)
        {


            try
            {
                // Validate the request
                if (string.IsNullOrEmpty(request.UserId))
                {
                    return Failure("User ID is required.");

                }
                if (request.OrderItems == null || !request.OrderItems.Any())
                {
                    return Failure("Order items are required.");
                }

                if (request.OrderItems.Any(oi => oi.Quantity <= 0))
                {
                    return Failure("Order item quantity must be greater than zero.");
                }
                // Check if the company exists
                var company = await unitOfWork.Companies.GetById(request.CompanyId);
                if (company == null)
                {
                    return Failure("Company not found.");
                }
                // Check if the products exist
                var productIds = request.OrderItems.Select(oi => oi.ProductId).ToList();
                var products = await unitOfWork.Products.GetByIds(productIds);
                if (products.Count != productIds.Count)
                    return Failure("One or more products not found.");
                // Check if the products belong to the company
                if (products.Any(p => p.CompanyId != request.CompanyId))
                {
                    return Failure("One or more products do not belong to the company.");
                }

                // Check if the productQuantity is available
                foreach (var orderItem in request.OrderItems)
                {
                    var product = products.FirstOrDefault(p => p.Id == orderItem.ProductId);
                    if (product == null || product.StockCount < orderItem.Quantity)
                    {
                        return Failure(product == null
                            ? $"Product with ID {orderItem.ProductId} not found."
                            : $"Product {product.Name} is not available in the requested quantity.");
                    }
                }
                // Create the order
                var order = new Order(request.UserId, request.CompanyId)
                {
                    OrderStatus = request.OrderStatus,
                    OrderItems = request.OrderItems.Select(oi => new OrderItem() { ProductId = oi.ProductId, Quantity = oi.Quantity }).ToList()
                };
                // Save the order to the database
                var result = await unitOfWork.Orders.Create(order);

                if (result == false)
                {
                    return new GenericResponse
                    {
                        Success = false,
                        Message = "Error occurred while creating the order. Please try again later."
                    };
                }
                // Save changes to the database
                await unitOfWork.SaveChangesAsync();
                // Update the stock count for each product
                foreach (var orderItem in order.OrderItems)
                {
                    var product = products.FirstOrDefault(p => p.Id == orderItem.ProductId);
                    if (product != null)
                    {
                        product.StockCount -= orderItem.Quantity;
                        await unitOfWork.Products.Update(product);
                    }
                }
                // Save changes to the database again
                await unitOfWork.SaveChangesAsync();

                return new GenericResponse
                {
                    Success = true,
                    Message = "Order created successfully.",
                };
            }
            catch (Exception ex)
            {
                logger.LogError(ex.Message);
                return new GenericResponse
                {
                    Success = false,
                    Message = "An error occurred while creating the Order.",
                    Errors = new List<string> { ex.Message }
                };
            }
        }
    }
}
