using Auth.Application.Features.User.GetAllUsers;
using Auth.Application.Features.Users.CreateUser;
using Auth.Application.Features.Users.GetUserToken;
using Auth.Application.Mappers;
using Auth.Domain.Interfaces;
using Auth.Infrastructure.Data;
using Auth.Infrastructure.Mapping;
using Auth.Infrastructure.Repositories;
using Cassandra;
using Cassandra.Mapping;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using System.Text;


var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddSingleton<ICluster>(sp =>
{
    var configuration = sp.GetRequiredService<IConfiguration>();

    return Cluster.Builder()
        .AddContactPoint(configuration["DataBaseSettings:HostName"])
        .Build();
});

builder.Services.AddSingleton<Cassandra.ISession>(sp =>
{
    var cluster = sp.GetRequiredService<ICluster>();
    var configuration = sp.GetRequiredService<IConfiguration>();

    var keyspace = configuration["DataBaseSettings:KeySpace"];
    return cluster.Connect(keyspace);
});

builder.Services.AddSingleton<MappingConfiguration>(sp =>
{
    var config = new MappingConfiguration();
    config.Define(new CassandraUserMapping());
    return config;
});

builder.Services.AddControllers();
builder.Services.AddSingleton(typeof(CassandraUserMapping));
builder.Services.AddScoped<IUsersContext,UsersContext>();
builder.Services.AddScoped(typeof(CassandraCluster));
builder.Services.AddScoped<IUserRepository, UserRepository>();

var jwtSettings = builder.Configuration["Authentication:JWT:Key"];

builder.Services.AddAuthentication(options =>
{
    options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
    options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
})
.AddJwtBearer(options =>
{
    options.TokenValidationParameters = new TokenValidationParameters
    {
        ValidateIssuer = false,
        ValidateAudience = false,
        ValidateLifetime = true,
        ValidateIssuerSigningKey = true,
        IssuerSigningKey = new SymmetricSecurityKey(
            Encoding.UTF8.GetBytes(jwtSettings))
    };
});

builder.Services.AddMediatR(cfg => {
    cfg.RegisterServicesFromAssembly(typeof(CreateUserAccountCommandHandler).Assembly);
    cfg.RegisterServicesFromAssembly(typeof(GetAllUsersQueryHandler).Assembly);
    cfg.RegisterServicesFromAssembly(typeof(GetUserTokenQueryHandler).Assembly);
});

// Register AutoMapper profiles from the assembly containing UserMapperProfile
builder.Services.AddAutoMapper(cfg =>
{
    cfg.AddMaps(typeof(UserMapperProfile).Assembly);
});

// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(c =>
{
    c.SwaggerDoc("v1", new OpenApiInfo { Title = "Auth Api", Version = "v1" });
    c.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
    {
        Description = "JWT Authorization header using the Bearer scheme. Example: \"Authorization: Bearer {token}\"",
        Name = "Authorization",
        In = ParameterLocation.Header,
        Type = SecuritySchemeType.ApiKey,
        Scheme = "Bearer"
    });
    c.AddSecurityRequirement(new OpenApiSecurityRequirement
    {
        {
            new OpenApiSecurityScheme
            {
                Reference = new OpenApiReference
                {
                    Type = ReferenceType.SecurityScheme,
                    Id = "Bearer"
                }
            },
            new string[] {}
        }
    });

});



var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();
app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();

app.Run();
