using System.Security.Claims;

namespace Identity.Api.Requests;

public record TokenGenerationRequest(Guid UserId, string Email, Dictionary<string, object> CustomClaims);