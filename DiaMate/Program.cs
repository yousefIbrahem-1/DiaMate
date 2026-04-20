using DiaMate.Data;
using DiaMate.Data.models;
using DiaMate.Extentions;
using DiaMate.IServices;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

// Database & Identity
builder.Services.AddDbContext<AppDbContext>(op =>
    op.UseSqlServer(builder.Configuration.GetConnectionString("MyConnection"))
);

builder.Services.AddIdentity<AppUser, IdentityRole>()
    .AddEntityFrameworkStores<AppDbContext>();

// UPDATED CORS: Now allows any local origin (like Vite's 5173 or React's 3000)
builder.Services.AddCors(options =>
{
    options.AddPolicy(name: "AllowLocalDev",
        policy =>
        {
            policy
                .SetIsOriginAllowed(origin => new Uri(origin).Host == "localhost")
                .AllowAnyHeader()
                .AllowAnyMethod();
        });
});

builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddScoped<IEmailService, EmailService>();
builder.Services.AddScoped<IQrCodeService, QrCodeService>();

builder.Services.Configure<IdentityOptions>(options =>
{
    options.SignIn.RequireConfirmedEmail = true;
});

builder.Services.AddCustomJwtAuth(builder.Configuration);

var app = builder.Build();

// Enable Swagger for all environments in Docker so the developer can see it
app.UseSwagger();
app.UseSwaggerUI();

// Commented out to prevent "Connection Refused" issues in local Docker dev
// app.UseHttpsRedirection(); 

app.UseCors("AllowLocalDev");

app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();


// Forces the app to listen on the standard Docker port
app.Urls.Add("http://0.0.0.0:80");

app.Run();