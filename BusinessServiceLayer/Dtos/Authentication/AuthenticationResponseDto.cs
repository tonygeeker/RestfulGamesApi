namespace RestfulGamesApi.BusinessServiceLayer.Dtos.Authentication
{
    public class AuthenticationResponseDto<TProfile> where TProfile : class
    {
        public TProfile Profile { get; set; }
        public AuthenticationTokenDto AuthenticationToken { get; set; }
    }
}
