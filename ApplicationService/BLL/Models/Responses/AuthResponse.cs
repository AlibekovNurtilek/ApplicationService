namespace ApplicationService.BLL.Models.Responces
{
    public class AuthResponse
    {
        public bool Success { get; set; }
        public string? AccessToken { get; set; }

        public string? Message { get; set; }
        public string? Role { get; set; }
    }
}
