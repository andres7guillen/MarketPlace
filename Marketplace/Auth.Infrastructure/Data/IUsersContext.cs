using Cassandra.Mapping;

namespace Auth.Infrastructure.Data;

public interface IUsersContext
{
    Mapper GetMapper();
}
