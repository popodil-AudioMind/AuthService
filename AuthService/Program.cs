using AuthService.Context;
using AuthService.Services;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Steeltoe.Discovery.Client;
using Steeltoe.Extensions.Configuration;
using AuthService.Data;
using AuthService.SQL;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddDiscoveryClient();

string connectionstring;
if (builder.Environment.IsDevelopment()) connectionstring = builder.Configuration.GetValue<string>("ConnectionStrings:DevConnection");
else connectionstring = builder.Configuration.GetValue<string>("ConnectionStrings:DefaultConnection");
try
{
    builder.Services.AddDbContextPool<AuthDatabaseContext>(
        options => options.UseMySql(connectionstring.ToString(), ServerVersion.AutoDetect(connectionstring))
    );
}
catch (Exception)
{
    throw;
}

builder.Services.AddScoped<ISqlLoginUser, SqlLoginUser>();

var app = builder.Build();

// Configure the HTTP request pipeline.
DatabaseManagementService.MigrationInitialisation(app);
//if (app.Environment.IsDevelopment())
//{
    app.UseSwagger();
    app.UseSwaggerUI(c => c.SwaggerEndpoint("/swagger/v1/swagger.json", "RestAPI v1"));
//}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
