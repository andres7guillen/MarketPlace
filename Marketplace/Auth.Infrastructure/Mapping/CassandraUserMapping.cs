using Auth.Domain.Entities;
using Cassandra.Mapping;

namespace Auth.Infrastructure.Mapping;

public class CassandraUserMapping : Mappings
{
    public CassandraUserMapping()
    {
        For<User>()
            .TableName("user")
            .PartitionKey(u => u.UserName)

            .Column(u => u.UserName, cm => cm.WithName("user_username"))
            .Column(u => u.Id, cm => cm.WithName("id"))
            .Column(u => u.Password, cm => cm.WithName("user_password"))
            .Column(u => u.Role, cm => cm.WithName("user_role"))
            .Column(u => u.Email, cm => cm.WithName("email"))
            .Column(u => u.Name, cm => cm.WithName("name"))
            .Column(u => u.SurName, cm => cm.WithName("surname"))
            .Column(u => u.Birthday, cm => cm.WithName("birthday"))
            .Column(u => u.CreationDate, cm => cm.WithName("creation_date"))
            .Column(u => u.Age, cm => cm.WithName("age"))
            .Column(u => u.ProfilePhoto, cm => cm.WithName("profile_photo"))
            .Column(u => u.IsPremium, cm => cm.WithName("is_premium"));
    }
}
