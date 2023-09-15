using connectMySQL.Model;
using Microsoft.EntityFrameworkCore;
using MySql.Data.MySqlClient;
using connectMySQL.Model;
using connectMySQL.Repositories;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();



//
//var configuration = new ConfigurationBuilder()
//    .SetBasePath(builder.Environment.ContentRootPath)
//    .AddJsonFile("appsettings.json")
//    .Build();

//var connectionString = configuration.GetConnectionString("DefaultConnection");
var connectionString = "Server=localhost;Port=3306;Database=newDB;Uid=root;Pwd=password;";
Console.WriteLine("connection string");
Console.WriteLine(connectionString);
builder.Services.AddTransient<MySqlConnection>(_ => new MySqlConnection(connectionString));

builder.Services.AddDbContext<UserContext>(options =>
    options.UseMySQL(connectionString));



builder.Services.AddScoped<IUserRepository, UserRepository>();



// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();









var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
