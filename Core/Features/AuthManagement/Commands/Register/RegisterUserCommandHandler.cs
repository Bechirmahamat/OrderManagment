using Core.Interface;
using Core.Responses;
using Core.Services;
using MediatR;
using Microsoft.Extensions.Logging;

namespace Core.Features.AuthManagement.Commands.Register
{
    public class RegisterUserCommandHandler : IRequestHandler<RegisterUserCommand, GenericResponse>
    {
        private readonly IAuthRepository authRepository;
        private readonly IJwtServiceProvider jwtServiceProvider;
        private readonly ILogger<RegisterUserCommandHandler> logger;

        public RegisterUserCommandHandler(IAuthRepository authRepository, ILogger<RegisterUserCommandHandler> logger, IJwtServiceProvider jwtServiceProvider)
        {
            this.authRepository = authRepository;
            this.logger = logger;
            this.jwtServiceProvider = jwtServiceProvider;
        }
        public async Task<GenericResponse> Handle(RegisterUserCommand request, CancellationToken cancellationToken)
        {
            try
            {
                // check if user exit
                var isAvailable = await authRepository.UserExists(request.Email);
                if (isAvailable)
                {

                    return new GenericResponse()
                    {
                        Message = "User with Email address already exist",
                        Success = false
                    };
                }


                var user = new Domain.Entities.User
                {
                    FirstName = request.FirstName,
                    LastName = request.LastName,
                    Email = request.Email,
                };
                var result = await authRepository.RegisterUser(user, request.Password);

                if (!result)
                {
                    return new GenericResponse
                    {
                        Success = false,
                        Message = "User registration failed"
                    };
                }
                var userData = await authRepository.GetUserByEmail(request.Email);
                // generate token
                var token = jwtServiceProvider.GenerateToken(userData);
                var refreshToken = jwtServiceProvider.GenerateRefreshToken();
                // save refresh token to database
                await authRepository.SaveRefreshToken(userData.Id, refreshToken);
                return new GenericResponse
                {
                    Success = true,
                    Message = "User registered successfully",
                    Data = new
                    {
                        user.Id,
                        user.FirstName,
                        user.LastName,
                        user.Email,
                        Token = token,
                        RefreshToken = refreshToken
                    }
                };


            }
            catch (Exception ex)
            {
                logger.LogError(ex.Message);
                return new GenericResponse()
                {
                    Message = ex.Message,
                    Success = false
                };
            }
        }
    }

}
