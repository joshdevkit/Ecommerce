using Ecommerce.Infrastructure;
using Ecommerce.Infrastructure.Constants;
using Ecommerce.WebApi.Events;
using Ecommerce.WebApi.Middlewares;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using System.Text;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddInfrastructure(builder.Configuration);

builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowFrontend", policy =>
    {
        policy.WithOrigins("http://localhost:3000")
              .AllowAnyHeader()
              .AllowAnyMethod()
              .AllowCredentials();
    });
});

AppConstants.Jwt.Initialize(builder.Configuration);

builder.Services.AddAuthentication(options =>
{
    options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
    options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
})
.AddJwtBearer(options =>
{
    // Create signing key for signature verification (HMAC-SHA256)
    var signingKey = new SymmetricSecurityKey(
        Encoding.UTF8.GetBytes(AppConstants.Jwt.Key)
    );

    // Create encryption key for token decryption (AES-256)
    var encryptionKey = new SymmetricSecurityKey(
        Encoding.UTF8.GetBytes(AppConstants.Jwt.EncryptionKey)
    );

    options.TokenValidationParameters = new TokenValidationParameters
    {
        // Validate the token issuer (who created the token)
        ValidateIssuer = true,
        ValidIssuer = AppConstants.Jwt.Issuer,

        // Validate the token audience (who the token is intended for)
        ValidateAudience = true,
        ValidAudience = AppConstants.Jwt.Audience,

        // Validate token expiration
        ValidateLifetime = true,

        // Validate the signing key
        ValidateIssuerSigningKey = true,
        IssuerSigningKey = signingKey,

        // Token decryption key for JWE tokens
        TokenDecryptionKey = encryptionKey,

        // No clock skew - tokens expire exactly at the specified time
        ClockSkew = TimeSpan.Zero
    };

    // Attach custom events to handle authentication failures and forbidden responses
    options.Events = new JwtAuthenticationEvents();
});

builder.Services.AddAuthorizationBuilder()
    .AddPolicy("AdminOnly", policy =>
        policy.RequireRole("Admin"));

var app = builder.Build();

app.UseMiddleware<GlobalExceptionMiddleware>();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseCors("AllowFrontend");
//app.UseHttpsRedirection();

// CRITICAL: Authentication must come BEFORE Authorization
app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();

app.Run();