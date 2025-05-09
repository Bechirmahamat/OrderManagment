using Core.Features.AuthManagement.Commands.Login;
using Core.Features.AuthManagement.Commands.Register;
using Core.Interface;
using Core.Responses;
using Core.Services;
using MediatR;
using Microsoft.Extensions.Logging;

namespace Core.Features.AuthManagement.Commands.Logins
{
    public class LoginCommandHandler : IRequestHandler<LoginCommand, GenericResponse>
    {
        private readonly IAuthRepository authRepository;
        private readonly IJwtServiceProvider jwtServiceProvider;
        private readonly ILogger<RegisterUserCommandHandler> logger;

        public LoginCommandHandler(IAuthRepository authRepository, ILogger<RegisterUserCommandHandler> logger, IJwtServiceProvider jwtServiceProvider)
        {
            this.authRepository = authRepository;
            this.logger = logger;
            this.jwtServiceProvider = jwtServiceProvider;
        }
        public async Task<GenericResponse> Handle(LoginCommand request, CancellationToken cancellationToken)
        {
            try
            {
                var isAvailable = await authRepository.UserExists(request.email);
                if (!isAvailable)
                {

                    return new GenericResponse()
                    {
                        Message = "User with Account Provided does not exits",
                        Success = false
                    };

                }

                var user = await authRepository.Login(request.email, request.password);
                if (user == null)
                {
                    return new GenericResponse()
                    {
                        Message = "Incorrect Password",
                        Success = false
                    };
                }

                var token = jwtServiceProvider.GenerateToken(user);
                var refreshToken = jwtServiceProvider.GenerateRefreshToken();
                // save refresh token to database
                await authRepository.SaveRefreshToken(user.Id, refreshToken);
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
