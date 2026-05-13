namespace Auth.Application.Responses;

public class UserResponse
{
    public string UserName { get;  set; }
    public Guid Id { get;  set; }
    public string Password { get;  set; }
    public string Role { get;  set; }
    public string Email { get;  set; }
    public string Name { get;  set; }
    public string SurName { get;  set; }
    public DateTime Birthday { get;  set; }
    public DateTime CreationDate { get;  set; }
    public int Age { get;  set; }
    public string ProfilePhoto { get;  set; } = string.Empty;
    public bool IsPremium { get;  set; }
}
