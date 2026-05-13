using Cassandra.Mapping;

namespace Auth.Infrastructure.Data;

public class UsersContext : IUsersContext
{
    private readonly Cassandra.ISession _session;
    private readonly MappingConfiguration _config;

    public UsersContext(Cassandra.ISession session, MappingConfiguration config)
    {
        _session = session;
        _config = config;
    }

    public Mapper GetMapper()
    {
        return new Mapper(_session, _config);
    }
}