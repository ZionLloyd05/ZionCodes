using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Microsoft.OpenApi.Models;
using Newtonsoft.Json;
using ZionCodes.Core.Brokers.DateTimes;
using ZionCodes.Core.Brokers.Loggings;
using ZionCodes.Core.Brokers.Storages;
using ZionCodes.Core.Models.Users;
using ZionCodes.Core.Services.Articles;
using ZionCodes.Core.Services.Categories;
using ZionCodes.Core.Services.Comments;
using ZionCodes.Core.Services.Tags;
using ZionCodes.Core.Services.Users;
using ZionCodes.Web.Api.Brokers.UserManagement;

namespace ZionCodes.Core
{
    public class Startup
    {
        public Startup(IConfiguration configuration) =>
            Configuration = configuration;

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            AddNewtonSoftJson(services);
            services.AddControllers();
            services.AddAutoMapper(typeof(Startup));
            services.AddScoped<IUserManagementBroker, UserManagementBroker>();
            services.AddDbContext<StorageBroker>();
            services.AddScoped<IStorageBroker, StorageBroker>();
            services.AddTransient<ILogger, Logger<LoggingBroker>>();
            services.AddTransient<ILoggingBroker, LoggingBroker>();
            services.AddTransient<IDateTimeBroker, DateTimeBroker>();
            services.AddTransient<ICategoryService, CategoryService>();
            services.AddTransient<ITagService, TagService>();
            services.AddTransient<IArticleService, ArticleService>();
            services.AddTransient<ICommentService, CommentService>();
            services.AddTransient<IUserService, UserService>();

            services.AddIdentityCore<User>(option => {
                option.SignIn.RequireConfirmedEmail = true;
                option.Password.RequireDigit = false;
                option.Password.RequiredLength = 6;
                option.Password.RequireNonAlphanumeric = false;
                option.Password.RequireUppercase = false;
                option.Password.RequireLowercase = false;
                option.User.RequireUniqueEmail = true;
            })
                    .AddRoles<Role>()
                    .AddEntityFrameworkStores<StorageBroker>()
                    .AddDefaultTokenProviders();

            services.AddSwaggerGen(c =>
            {
                var openApiInfo = new OpenApiInfo
                {
                    Title = "ZionCodes API",
                    Version = "v1"
                };

                c.SwaggerDoc(
                   name: "v1",
                   info: openApiInfo
                );
            });
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();

            }

            app.UseCors(x => x
                .AllowAnyOrigin()
                .AllowAnyMethod()
                .AllowAnyHeader());

            app.UseSwagger();
            app.UseSwaggerUI(c => c.SwaggerEndpoint(
                "/swagger/v1/swagger.json",
                "ZionCodes API v1"
                ));

            app.UseHttpsRedirection();
            app.UseRouting();
            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
                endpoints.MapControllers());
        }

        private static void AddNewtonSoftJson(IServiceCollection services)
        {
            services.AddMvc().AddNewtonsoftJson(options =>
            {
                options.SerializerSettings.ReferenceLoopHandling = ReferenceLoopHandling.Ignore;
                options.SerializerSettings.Converters.Add(new Newtonsoft.Json.Converters.StringEnumConverter());
                options.SerializerSettings.NullValueHandling = NullValueHandling.Ignore;
            });
        }
    }
}
