namespace ShoppingApp.Core.Entity.User.Dto;

public class LoginResponseDto
{
    public UserDto User { get; set; }
    public string Token { get; set; }
    public DateTime ExpiredAt { get; set; }
}