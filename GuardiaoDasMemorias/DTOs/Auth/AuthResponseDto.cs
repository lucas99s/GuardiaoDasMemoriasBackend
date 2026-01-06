namespace GuardiaoDasMemorias.DTOs.Auth;

public class AuthResponseDto
{
    public required string Token { get; set; }
    public required string Email { get; set; }
    public required string UserId { get; set; }
    public string? UserName { get; set; }
    public DateTime ExpiresAt { get; set; }
}
