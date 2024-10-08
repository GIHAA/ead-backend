/*
 * File: RegisterModel.cs, LoginModel.cs, UserUpdateModel.cs
 * Project: Healthy Bites.Dtos
 * Description: These Data Transfer Objects (DTOs) represent the models used for requests related to user registration, login, 
 *              and user updates. 
 *              - `RegisterModel`: Contains Username, Email, Password, and Role (default is "customer") for user registration.
 *              - `LoginModel`: Contains Email and Password for user login.
 *              - `UserUpdateModel`: Contains optional fields like Email, Role, Name, Address, PhoneNumber, and Status for updating user details.
 */

/*
 * File: RegisterModel.cs, LoginModel.cs, and UserUpdateModel.cs
 * Project: HealthyBites
 * Description: These files define the data transfer objects (DTOs) used for user registration, login, and user update requests. 
 *              Each class represents the structure of incoming requests, ensuring data consistency and validation when handling user data.
 * 
 * Authors: Cooray N.T.L. it21177996
 * 
 * Classes:
 * - RegisterModel: Used for user registration requests. Contains properties such as Username, Email, Password, and Role.
 * - LoginModel: Used for user login requests, containing Email and Password fields.
 * - UserUpdateModel: Represents an update request for user details such as email, role, name, address, and phone number.
 * 
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
