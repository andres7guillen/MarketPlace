using CSharpFunctionalExtensions;

namespace Auth.Domain.Entities;

public class User
{
    public User() { }
    public string UserName { get; set; }  
    public Guid Id { get; set; }
    public string Password { get; set; }
    public string Role { get; set; }
    public string Email { get; set; }
    public string Name { get; set; }
    public string SurName { get; set; }
    public DateTime Birthday { get; set; }
    public DateTime CreationDate { get; set; }
    public int Age { get; set; }
    public string ProfilePhoto { get; set; } = string.Empty;
    public bool IsPremium { get; set; }
    private User(string userName, Guid id, string password, string role, string email, string name, string surName, DateTime birthday, DateTime creationDate, int age, string profilePhoto, bool isPremium)
    {
        UserName = userName;
        Id = id;
        Password = password;
        Role = role;
        Email = email;
        Name = name;
        SurName = surName;
        Birthday = birthday;
        CreationDate = creationDate;
        Age = age;
        ProfilePhoto = profilePhoto;
        IsPremium = isPremium;
    }

    public static Result<User> Create(string userName, Guid id, string password, string role, string email, string name, string surName, DateTime birthday, DateTime creationDate, int age, string profilePhoto, bool isPremium) 
    { 
        return Result.Success(new User(userName, id, password, role, email, name, surName, birthday, creationDate, age, profilePhoto, isPremium));
    }

}
