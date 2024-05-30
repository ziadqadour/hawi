using Contracts;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using Repository;
using Hawi.Dtos;
using System.Text;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;
using Hawi.Extensions;
using Microsoft.AspNetCore.HttpOverrides;
using System.Text.Json;
using Microsoft.EntityFrameworkCore;
using Hawi.Models;
using Hawi.Contracts;
using Hawi.Repository;
using CorePush.Apple;
using CorePush.Google;
using Microsoft.Extensions.Configuration;

namespace Hawi
{

    class Program
    {

        static async Task Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            builder.Services.ConfigureCors();

            //mapper
            builder.Services.AddAutoMapper(typeof(Program));

            builder.Services.AddControllers();
            
            //firebase
            builder.Services.AddTransient<INotificationService, NotificationService>();
            builder.Services.AddHttpClient<FcmSender>();
            builder.Services.AddHttpClient<ApnSender>();
            //
            builder.Services.AddAutoMapper(typeof(Program));
            builder.Services.AddAuthentication();

            //twillio
            builder.Services.Configure<twillioseting>(builder.Configuration.GetSection("Twilio"));
            builder.Services.AddTransient<ISMRepository, SMRepository>();

            builder.Services.AddTransient<IDateTimeBetweenRange, DateTimeBetweenRange>();

            builder.Services.AddScoped<ChampionshipFunctions>()
                   .AddScoped<MatchFunctions>()
                   .AddScoped<UserFunctions>()
                   .AddScoped<Notification>()
                   .AddScoped<ImageFunctions>()
                   .AddScoped<Authentiaction>()
                   .AddScoped<AddRecordsInDB>()
                   .AddScoped<ArticleFunctions>()
                   .AddScoped<HawiContext>();
                  

            builder.Services.AddEndpointsApiExplorer();
            builder.Services.AddSwaggerGen();
           
            //firebase
            // Configure strongly typed settings objects
            var appSettingsSection = builder.Configuration.GetSection("FcmNotification");
            builder.Services.Configure<FcmNotificationSetting>(appSettingsSection);

            builder.Services.AddDbContext<HawiContext>(options =>
            {
                options.UseSqlServer(builder.Configuration.GetConnectionString("LinksConnection"));
            });

            var app = builder.Build();

            app.UseSwagger();
            app.UseSwaggerUI();


            app.UseHttpsRedirection();
            app.UseStaticFiles();

            app.UseForwardedHeaders(new ForwardedHeadersOptions
            {
                ForwardedHeaders = ForwardedHeaders.All
            });

            app.UseCors("CorsPolicy");
            app.UseRouting();
            app.UseAuthentication();
            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers(); 
            });

            //app.UseHttpsRedirection();
            app.MapControllers();
            app.Run();
        }
    }
}
