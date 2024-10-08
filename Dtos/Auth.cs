/*
 * File: RegisterModel.cs, LoginModel.cs, UserUpdateModel.cs
 * Project: Healthy Bites.Dtos
 * Description: These Data Transfer Objects (DTOs) represent the models used for requests related to user registration, login, 
 *              and user updates. 
 *              - `RegisterModel`: Contains Username, Email, Password, and Role (default is "customer") for user registration.
 *              - `LoginModel`: Contains Email and Password for user login.
 *              - `UserUpdateModel`: Contains optional fields like Email, Role, Name, Address, PhoneNumber, and Status for updating user details.
 */

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
}
