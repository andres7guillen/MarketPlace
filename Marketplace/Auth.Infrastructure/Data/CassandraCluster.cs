using Cassandra;
using Microsoft.Extensions.Configuration;


namespace Auth.Infrastructure.Data;

public class CassandraCluster
{
    public Cluster  ConfiguredCluster{ get; set; }

    public CassandraCluster(IConfiguration configuration)
    {
        var hostName = configuration.GetSection("DataBaseSettings:HostName").Value;
        var port = configuration.GetSection("DataBaseSettings:Port").Value;
        ConfiguredCluster = Cluster.Builder()
            .AddContactPoint(hostName)
            .WithPort(int.Parse(port))
            .Build();
    }
}
