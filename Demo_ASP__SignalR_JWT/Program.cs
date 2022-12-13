using Demo_ASP__SignalR_JWT.Hubs;
using Demo_ASP__SignalR_JWT.Tools;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using System.Text;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllers();

builder.Services.AddSignalR();

builder.Services.AddTransient<TokenManager>();

builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
                .AddJwtBearer(options =>
                {
                    byte[] secret = Encoding.UTF8.GetBytes(builder.Configuration["Token:Secret"]);

                    options.SaveToken = true;
                    options.TokenValidationParameters = new TokenValidationParameters()
                    {
                        ValidateIssuer = true,
                        ValidIssuer = builder.Configuration["Token:Issuer"],
                        ValidateAudience = true,
                        ValidAudience = builder.Configuration["Token:Audience"],
                        ValidateIssuerSigningKey = true,
                        IssuerSigningKey = new SymmetricSecurityKey(secret),
                        ValidateLifetime = true
                    };

                    // https://learn.microsoft.com/en-us/aspnet/core/signalr/authn-and-authz?view=aspnetcore-6.0
                    options.Events = new JwtBearerEvents
                    {
                        OnMessageReceived = context =>
                        {
                            var accessToken = context.Request.Query["access_token"];

                            // If the request is for our hub...
                            var path = context.HttpContext.Request.Path;
                            if (!string.IsNullOrEmpty(accessToken) &&
                                path.StartsWithSegments("/MessageHub"))
                            {
                                // Read the token out of the query string
                                context.Token = accessToken;
                            }
                            return Task.CompletedTask;
                        }
                    };
                });

builder.Services.AddCors(options =>
{
    options.AddPolicy("DemoPolicy", builder =>
        builder.WithOrigins("http://localhost:5094")
            .AllowAnyHeader()
            .AllowAnyMethod()
            .AllowCredentials()
    );
});


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

app.UseCors("DemoPolicy");

app.UseAuthentication();

app.UseAuthorization();

app.MapHub<MessageHub>("/MessageHub");

app.MapControllers();

app.Run();
