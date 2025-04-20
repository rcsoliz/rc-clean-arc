using System.Net.Http.Headers;
using System.Net.Http.Json;
using BlazorAppRc.Models;
using Microsoft.JSInterop;
namespace BlazorAppRc.Services
{
    public class AuthService
    {
        //private readonly HttpClient _httpClient;
        //private readonly IJSRuntime _jsRuntime;
        private readonly AuthState _authState;

        private readonly HttpClient _http;
        private readonly IJSRuntime _js;
        public AuthService(HttpClient http, IJSRuntime js)
        {
            _http = http;
            _js = js;
        }

        public async Task<bool> RefreshTokenAsync()
        {
            var refreshToken = await _js.InvokeAsync<string>("localStorage.getItem", "refreshToken");

            if (string.IsNullOrEmpty(refreshToken))
                return false;

            var response = await _http.PostAsJsonAsync("api/authentication/refresh-token", new { RefreshToken = refreshToken });

            if (!response.IsSuccessStatusCode)
                return false;

            var result = await response.Content.ReadFromJsonAsync<TokenResponse>();

            if (result is null) return false;

            await _js.InvokeVoidAsync("localStorage.setItem", "jwt", result.JwtToken);
            await _js.InvokeVoidAsync("localStorage.setItem", "refreshToken", result.RefreshToken);

            return true;
        }

        public async Task<string?> GetJwtTokenAsync()
        {
            return await _js.InvokeAsync<string>("localStorage.getItem", "jwt");
        }


        //public AuthService(HttpClient httpClient, IJSRuntime jsRuntime, AuthState authState)
        //{
        //    _httpClient = httpClient;
        //    _jsRuntime = jsRuntime;
        //    _authState = authState;
        //}
        //public async Task<bool> Login(LoginModel loginModel)
        //{
        //    var response = await _httpClient.PostAsJsonAsync("api/auth/login", loginModel);
        //    if (response.IsSuccessStatusCode)
        //    {
        //        var tokenResponse = await response.Content.ReadFromJsonAsync<TokenResponse>();
        //        if (tokenResponse != null)
        //        {
        //            await _jsRuntime.InvokeVoidAsync("localStorage.setItem", "jwtToken", tokenResponse.JwtToken);
        //            await _jsRuntime.InvokeVoidAsync("localStorage.setItem", "refreshToken", tokenResponse.RefreshToken);
        //            _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", tokenResponse.JwtToken);
        //            _authState.IsAuthenticated = true;
        //            return true;
        //        }
        //    }
        //    return false;
        //}
        public async Task Logout()
        {
            await _js.InvokeVoidAsync("localStorage.removeItem", "jwtToken");
            await _js.InvokeVoidAsync("localStorage.removeItem", "refreshToken");
            _http.DefaultRequestHeaders.Authorization = null;
            _authState.IsAuthenticated = false;
        }
        public async Task<bool> IsAuthenticated()
        {
            var token = await _js.InvokeAsync<string>("localStorage.getItem", "jwtToken");
            return !string.IsNullOrEmpty(token);
        }
    }
}
