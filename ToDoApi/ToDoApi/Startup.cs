using AutoMapper;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Microsoft.OpenApi.Models;
using ToDoApi.Scope;
using ToDoApi.Services;
using ToDoApi.Utils;
using ToDoInfrastructure;

namespace ToDoApi
{
    public class Startup
    {

        private readonly ILogger<Startup> _logger;
        private readonly string Cors = "_cors";

        public Startup(IConfiguration configuration, ILogger<Startup> logger)
        {
            Configuration = configuration;
            _logger = logger;
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {

            string domain = $"https://{Configuration["Auth0:Domain"]}/";

            services.AddAuthentication(options =>
            {
                options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
            }).AddJwtBearer(options =>
            {
                options.Authority = domain;
                options.Audience = Configuration["Auth0:Audience"];
            });

            services.AddAuthorization(options =>
            {
                options.AddPolicy("read:to-do-lists", policy => policy.Requirements.Add(new HasScopeRequirement("read:to-do-lists", domain)));
                options.AddPolicy("read:to-do-list", policy => policy.Requirements.Add(new HasScopeRequirement("read:to-do-list", domain)));
                options.AddPolicy("create:to-do-list", policy => policy.Requirements.Add(new HasScopeRequirement("create:to-do-list", domain)));
                options.AddPolicy("update:to-do-list", policy => policy.Requirements.Add(new HasScopeRequirement("update:to-do-list", domain)));
                options.AddPolicy("delete:to-do-list", policy => policy.Requirements.Add(new HasScopeRequirement("delete:to-do-list", domain)));
                options.AddPolicy("update-position:to-do-list", policy => policy.Requirements.Add(new HasScopeRequirement("update-position:to-do-list", domain)));
                options.AddPolicy("share:to-do-list", policy => policy.Requirements.Add(new HasScopeRequirement("share:to-do-list", domain)));


                options.AddPolicy("read:to-do-items", policy => policy.Requirements.Add(new HasScopeRequirement("read:to-do-items", domain)));
                options.AddPolicy("read:to-do-item", policy => policy.Requirements.Add(new HasScopeRequirement("read:to-do-item", domain)));
                options.AddPolicy("create:to-do-item", policy => policy.Requirements.Add(new HasScopeRequirement("create:to-do-item", domain)));
                options.AddPolicy("update:to-do-item", policy => policy.Requirements.Add(new HasScopeRequirement("update:to-do-item", domain)));
                options.AddPolicy("delete:to-do-item", policy => policy.Requirements.Add(new HasScopeRequirement("delete:to-do-item", domain)));

            });

            services.AddSingleton<IAuthorizationHandler, HasScopeHandler>();

            services.AddCors(options =>
            {
                options.AddPolicy(name: Cors, builder => {
                    builder.SetIsOriginAllowed(_ => true).AllowAnyHeader()
                                        .AllowAnyMethod().AllowCredentials();
                });
            });

            services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("v1", new OpenApiInfo { Title = "My API", Version = "v1" });
            });

            services.AddDbContext<ToDoDbContext>(
                 options => options.UseSqlServer(Configuration["ConnectionString"]));

            services.AddAutoMapper(typeof(Startup));

            services.AddScoped<IToDoListService, ToDoListService>();
            services.AddScoped<IToDoItemService, ToDoItemService>();

            //services.AddSingleton<IHostedService, ReminderService>();

            services.Configure<ReminderOptions>(Configuration.GetSection(
                                        ReminderOptions.Reminder));

            services.AddMvc(options => options.EnableEndpointRouting = false);

        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env, ToDoDbContext toDoDbContext, IHostApplicationLifetime lifetime)
        {
            app.UseAuthentication();
         
            app.UseCors(Cors);

            app.UseMvc();

            app.UseSwagger();

            app.UseSwaggerUI(c =>
            {
                c.SwaggerEndpoint("/swagger/v1/swagger.json", "My API V1");
            });

            toDoDbContext.Database.Migrate();

            lifetime.ApplicationStarted.Register(() => _logger.LogDebug("ToDoApi started!"));
            lifetime.ApplicationStopped.Register(() => _logger.LogDebug("ToDoApi stopped!"));
        }
    }
}
