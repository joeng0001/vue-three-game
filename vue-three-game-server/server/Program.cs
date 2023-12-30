using server.Model;
using Microsoft.EntityFrameworkCore;
using MySql.Data.MySqlClient;
using server.Model;
using server.Repositories;

var builder = WebApplication.CreateBuilder(args);
builder.Services.AddControllers();

var connectionString = "Server=localhost;Port=3306;Database=newDB;Uid=root;Pwd=password;";
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

app.UseCors(builder =>
{
    //builder.WithOrigins("http://localhost:8080")
    builder.AllowAnyOrigin()
           .AllowAnyMethod()
           .AllowAnyHeader();
});


app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
