// Data transfer objects for requests

public class RegisterModel
{
    public string Username { get; set; }
    public string Email { get; set; }
    public string Password { get; set; }
    public string Role { get; set; } = "customer";
}

public class LoginModel
{
    public string Email { get; set; }
    public string Password { get; set; }
}
public class UserUpdateModel
{
    public string? Email { get; set; } 
    public string? Role { get; set; } 
    public string? Name { get; set; } 
    public string? Address { get; set; } 
    public string? PhoneNumber { get; set; } 
    public string? Status { get; set; } 
    public float? VendorRating { get; set; }
    public List<string> VendorIds { get; internal set; }
}
