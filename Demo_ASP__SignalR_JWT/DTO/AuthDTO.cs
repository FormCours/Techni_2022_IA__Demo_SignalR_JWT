using System.ComponentModel.DataAnnotations;

namespace Demo_ASP__SignalR_JWT.DTO
{
    public class AuthLoginDTO
    {
        [Required]
        public string Username { get; set; }

        [Required]
        public string Password { get; set; }
    }

    public class AuthTokenDTO
    {
        public string Token { get; set; }
    }
}
