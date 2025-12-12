using DiaMate.Data;
using DiaMate.Data.models;
using DiaMate.Extentions;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

// Database & Identity
builder.Services.AddDbContext<AppDbContext>(op =>
    op.UseSqlServer(builder.Configuration.GetConnectionString("MyConnection"))
);

builder.Services.AddIdentity<AppUser, IdentityRole>()
    .AddEntityFrameworkStores<AppDbContext>();

// CORS policy - ????? ????
builder.Services.AddCors(options =>
{
    options.AddPolicy(name: "AllowLocalDev",
        policy =>
        {
            policy
                .WithOrigins("http://localhost:5173") // ??? allowed ????? ???????
                .AllowAnyHeader()
                .AllowAnyMethod()
                // .AllowCredentials() // ???? ?????? ?? ?????? cookies ?? credentials
                ;
        });
});

// Controllers, Swagger
builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

// Authentication (JWT) - ????? AddCustomJwtAuth ???? Authentication/Authorization services
builder.Services.AddCustomJwtAuth(builder.Configuration);

var app = builder.Build();

// Configure the HTTP request pipeline.
app.UseSwagger();
app.UseSwaggerUI();

app.UseHttpsRedirection();

// IMPORTANT: UseCors must come BEFORE authentication/authorization so OPTIONS doesn't get blocked
app.UseCors("AllowLocalDev");

app.UseAuthentication();   // ???? ?? ???? JWT/Identity
app.UseAuthorization();

app.MapControllers();

app.Run();

