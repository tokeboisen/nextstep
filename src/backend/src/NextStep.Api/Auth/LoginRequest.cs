namespace NextStep.Api.Auth;

public record LoginRequest(string Email, string Password);
public record LoginResponse(string Token);
