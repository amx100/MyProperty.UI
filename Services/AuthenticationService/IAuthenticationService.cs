namespace Services
{
    public interface IAuthenticationService
    {
        Task<GeneralResponseDto> Register(RegistrationDto registrationDto, CancellationToken cancellationToken = default);

        Task<AuthenticationDto> Login(LoginDto loginDto, CancellationToken cancellationToken = default);
    }
}