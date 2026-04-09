using BusinessLayer.Interface;
using BusinessLayer.RabbitMQ;
using BusinessLayer.Service;
using DataBaseLayer.Interfaces;
using DataBaseLayer.Repository;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.Data.SqlClient;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using System.Data;
using System.Text;
using Databaseayer.Repository;
using DataBaseLayer.Repository;

namespace Fundoo
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            var connectionString = builder.Configuration.GetConnectionString("DefaultConnection")
                                   ?? throw new InvalidOperationException("Connection string missing");

            var jwtKey = builder.Configuration["Jwt:Key"]
                         ?? throw new InvalidOperationException("JWT key missing");

            // AddScoped creates one SqlConnection per request so related services share the same connection lifetime.
            builder.Services.AddScoped<IDbConnection>(_ =>
                new SqlConnection(connectionString));
            
            builder.Services.AddTransient<IUserBL, UserBL>();
            builder.Services.AddTransient<IUserDL, UserDL>();
            
            builder.Services.AddScoped<IAuthDL, AuthDL>();
            builder.Services.AddScoped<IAuthBL, AuthBL>();
            
            builder.Services.AddScoped<INoteDL, NoteDL>();
            builder.Services.AddScoped<INoteBL, NoteBL>();
            
            builder.Services.AddScoped<ILabelDL, LabelDL>();
            builder.Services.AddScoped<ILabelBL, LabelBL>();
            
            builder.Services.AddScoped<IReminderDL, ReminderDL>();
            builder.Services.AddScoped<IReminderBL, ReminderBL>();
            
            
            builder.Services.AddScoped<ICollaboratorBL, CollaboratorBL>(); 
            builder.Services.AddScoped<ICollaboratorDL, CollaboratorDL>();

            // AddAuthentication/AddJwtBearer tells ASP.NET Core to validate Bearer tokens on protected endpoints.
            builder.Services.AddAuthentication(options =>
            {
                options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
            })
            .AddJwtBearer(options =>
            {
                options.RequireHttpsMetadata = false;
                options.SaveToken = true;

                options.TokenValidationParameters = new TokenValidationParameters
                {
                    ValidateIssuer = true,
                    ValidateAudience = true,
                    ValidateLifetime = true,
                    ValidateIssuerSigningKey = true,

                    ValidIssuer = builder.Configuration["Jwt:Issuer"],
                    ValidAudience = builder.Configuration["Jwt:Audience"],
                    IssuerSigningKey = new SymmetricSecurityKey(
                        Encoding.UTF8.GetBytes(jwtKey))
                };
            });
            
            builder.Services.AddScoped<IRabbitMQProducer, RabbitMQProducer>();
            builder.Services.AddScoped<EmailService>();
            builder.Services.AddAuthorization();

            builder.Services.AddControllers();

            // This Swagger setup adds the Authorize button so JWTs can be sent from the Swagger UI.
            builder.Services.AddEndpointsApiExplorer();
            builder.Services.AddSwaggerGen(options =>
            {
                options.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
                {
                    Name = "Authorization",
                    Type = SecuritySchemeType.Http,
                    Scheme = "bearer",
                    BearerFormat = "JWT",
                    In = ParameterLocation.Header,
                    Description = "Enter: Bearer {your token}"
                });

                options.AddSecurityRequirement(new OpenApiSecurityRequirement
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
                        Array.Empty<string>()
                    }
                });
            });

            var app = builder.Build();

            if (app.Environment.IsDevelopment())
            {
                app.UseSwagger();
                app.UseSwaggerUI();
            }

            // UseAuthentication must run before UseAuthorization so policy checks can read user claims from the token.
            app.UseHttpsRedirection();
            app.UseMiddleware<ExceptionMiddleware>(); 
            app.UseAuthentication();   //  STEP 1
            app.UseAuthorization();    //  STEP 2

            app.MapControllers();

            app.Run();
        }
    }
}
