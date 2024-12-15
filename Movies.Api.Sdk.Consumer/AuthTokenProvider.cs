/*
 * @author: Cesar Lopez Lerma
 * @copyright 2024 - All rights reserved
 */
using System;
using System.IdentityModel.Tokens.Jwt;
using System.Net.Http.Json;

namespace Movies.Api.Sdk.Consumer;

public class AuthTokenProvider
{
    private readonly HttpClient _httpClient;
    private string _cachedToken = string.Empty;
    private static readonly SemaphoreSlim Lock = new(1,1);

    public AuthTokenProvider(HttpClient httpClient)
    {
        _httpClient = httpClient;
    }

    public async Task<string> GetTokenAsync()
    {
        if (!string.IsNullOrEmpty(_cachedToken))
        {
            var jwt = new JwtSecurityTokenHandler().ReadJwtToken(_cachedToken);
            var expiryTimeText = jwt.Claims.Single(claim => claim.Type == "exp").Value;
            var expiryDateTime = UnixTimeStampToDateTime(int.Parse(expiryTimeText));
            if (expiryDateTime > DateTime.UtcNow)
            {
                return _cachedToken;
            }
        }

        await Lock.WaitAsync();
        var response = await _httpClient.PostAsJsonAsync("http://localhost:8081/token", new
        {
            userId = "3fa85f64-5717-4562-b3fc-2c963f66afa6",
            email  = "string",
            customClaims = new Dictionary<string, object>
            {
                {"admin", true},
                {"trusted_member", true}
            }

        });

        var newToken = await response.Content.ReadAsStringAsync();
        _cachedToken = newToken;

        Lock.Release();

        return _cachedToken;
    }

    private static DateTime UnixTimeStampToDateTime(int unixTimeStamp)
    {
        var dateTime = new DateTime(1970, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc);
        dateTime = dateTime.AddSeconds(unixTimeStamp).ToLocalTime();
        return dateTime;
    }
}
