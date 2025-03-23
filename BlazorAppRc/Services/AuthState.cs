namespace BlazorAppRc.Services
{
    public class AuthState
    {
        public string? Id { get; set; }
        public bool IsAuthenticated { get; set; } = false;
        public string? Username { get; set; }   
    }
}
