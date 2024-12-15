using IDEDataLayer.UserIdentityContext;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Identity;
using IDECore.Service.Interface;
using IDECore.Service;
using System.Text;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

#region Mc Identity Options

builder.Services.AddDbContext<UserContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("UserManager_dbConnection")));

builder.Services.AddDefaultIdentity<IdentityUser>().AddEntityFrameworkStores<UserContext>();

#endregion

#region IOC

builder.Services.AddTransient<IUserManagerService, UserManagerService>();
builder.Services.AddTransient<IEmailSender, EmailSender>();
builder.Services.AddTransient<IAuthenticateService, AuthenticateService>();

#endregion

#region Jwt

var key = Encoding.ASCII.GetBytes(builder.Configuration["Authentication:SecretForKey"]);

builder.Services.AddAuthentication(options =>
{
    options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
    options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
}).AddJwtBearer(oprions =>
{
    oprions.RequireHttpsMetadata = false;
    oprions.SaveToken = true;
    oprions.TokenValidationParameters = new TokenValidationParameters
    {
        ValidateIssuerSigningKey = true,
        IssuerSigningKey = new SymmetricSecurityKey(key),
        ValidateIssuer = true,
        ValidateAudience = true,
        ValidIssuer = builder.Configuration["Authentication:Issuer"],
        ValidAudience = builder.Configuration["Authentication:Audience"]
    };
});

#endregion

builder.Services.AddControllers();
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
