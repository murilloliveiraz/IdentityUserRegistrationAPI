using APIThatSendAnEmailToUpdatePassword.Context;
using APIThatSendAnEmailToUpdatePassword.Helpers;
using APIThatSendAnEmailToUpdatePassword.Models;
using APIThatSendAnEmailToUpdatePassword.Services;
using APIThatSendAnEmailToUpdatePassword.Services.Interfaces;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.Configure<EmailSettings>(builder.Configuration
                                         .GetSection("EmailSettings"));

string? connectionString = builder.Configuration.GetConnectionString("Localhost");
builder.Services.AddDbContext<UsersContext>(options =>
        options.UseNpgsql(connectionString), ServiceLifetime.Transient, ServiceLifetime.Transient
    );

builder.Services.AddIdentity<Usuario, IdentityRole>()
        .AddEntityFrameworkStores<UsersContext>()
        .AddDefaultTokenProviders();

builder.Services
    .AddScoped<IEmailService, EmailService>()
    .AddScoped<UsersService>();

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
