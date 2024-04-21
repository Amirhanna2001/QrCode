using System.Globalization;
using System.Text;
using AutoMapper;
using LimitlessCareDrPortal.Repository;
using LimitlessCareDrPortal.UnitOfWork;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Http.Features;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Localization;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using QrCode.API.AzureBlobServices;
using QrCode.API.Services;
using QrCode.DB;
using QrCode.DB.Models;
using QrCode.Repository;

namespace LimitlessCareDrPortal;

public class Startup
{
    public readonly IConfiguration Configuration;

    public Startup(IConfiguration configuration)
    {
        Configuration = configuration;
    }

    // This method gets called by the runtime. Use this method to add services to the container.
#pragma warning disable CA1506 // 'ConfigureServices' is coupled with '53' different types from '37' different namespaces. Rewrite or refactor the code to decrease its class coupling below '41'. (https://learn.microsoft.com/dotnet/fundamentals/code-analysis/quality-rules/ca1506)
    public void ConfigureServices(IServiceCollection services)
#pragma warning restore CA1506 // 'ConfigureServices' is coupled with '53' different types from '37' different namespaces. Rewrite or refactor the code to decrease its class coupling below '41'. (https://learn.microsoft.com/dotnet/fundamentals/code-analysis/quality-rules/ca1506)
    {
        // To Handel Refrence looping
        _ = services.AddControllers()
                .AddNewtonsoftJson(o =>
                o.SerializerSettings.ReferenceLoopHandling =
                     Newtonsoft.Json.ReferenceLoopHandling.Ignore);
        _ = services.AddControllersWithViews();
        _ = services.AddMvc(option => option.EnableEndpointRouting = false);
        _ = services.AddControllers()
        .AddJsonOptions(opts => opts.JsonSerializerOptions.PropertyNamingPolicy = null);

        _ = services.AddControllers()
       .AddJsonOptions(options =>
       {
           options.JsonSerializerOptions.PropertyNameCaseInsensitive = true;
           options.JsonSerializerOptions.PropertyNamingPolicy = null;
       });

        services.AddIdentity<AppUser, IdentityRole>()
                .AddEntityFrameworkStores<DatabaseContext>()
                .AddDefaultTokenProviders();

        services.Configure<IdentityOptions>(options =>
        {
            options.Password.RequireNonAlphanumeric = false;
        });

        services.AddAuthentication(options =>
        {
            options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
            options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
            options.DefaultScheme = JwtBearerDefaults.AuthenticationScheme;
        })
        .AddJwtBearer(options =>
        {
            options.SaveToken = true;
            options.RequireHttpsMetadata = false;
            options.TokenValidationParameters = new TokenValidationParameters
            {
                ValidateIssuer = true,
                ValidIssuer = Configuration["JWT:Issuer"],
                ValidateAudience = true,
                ValidAudience = Configuration["JWT:Audience"],
                IssuerSigningKey =
                     new SymmetricSecurityKey(Encoding.UTF8.GetBytes(Configuration["JWT:Key"]))

            };
        });
        _ = services.AddLocalization(opts => { opts.ResourcesPath = "Resources"; });
        _ = services.Configure<RequestLocalizationOptions>(
        opts =>
        {
            var supportedCultures = new List<CultureInfo>
            {
                new CultureInfo("en"),
                new CultureInfo("ar"),
            };

            opts.DefaultRequestCulture = new RequestCulture("en");

            // Formatting numbers, dates, etc.
            //opts.SupportedCultures = supportedCultures;
            // UI strings that we have localized.
            opts.SupportedUICultures = supportedCultures;
        });
        _ = services.Configure<FormOptions>(o =>
        {
            o.ValueLengthLimit = int.MaxValue;
            o.MultipartBodyLengthLimit = int.MaxValue;
            o.MemoryBufferThreshold = int.MaxValue;
        });

        _ = services.AddSwaggerGen(swagger =>
        {
            swagger.SwaggerDoc("v2", new OpenApiInfo
            {
                Title = "QrCode App APIs",
                Description = "QrCode App APIs",
                Contact = new OpenApiContact() { Email = string.Empty },
                Version = "1.0.0",
            });
        });

        // Mapper service
        var mappingConfig = new MapperConfiguration(mc =>
        {
        });

        var mapper = mappingConfig.CreateMapper();
        _ = services.AddSingleton(mapper);

        // Database Service

        _ = services.AddDbContextPool<DatabaseContext>(
         option => option.UseSqlServer(Configuration.GetConnectionString("DefaultConnection")));

		_ = services.AddScoped(typeof(IGenericRepository<>), typeof(GenericRepository<>));
		_ = services.AddScoped<IUnitOfWork, UnitOfWork.UnitOfWork>();
		_ = services.AddScoped<IQRCodeRepository, QRCodeRepository>();
        _ = services.AddScoped<IQRScanRepository, QRScanRepository>();
        _ = services.AddScoped<IEmailServices, EmailServices>();
        _ = services.AddScoped<IAzureBlobService, AzureBlobService>();
        _ = services.AddScoped<IQrCodeServices, QrCodeServices>();
    }

    // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
    public void Configure(IApplicationBuilder app, IWebHostEnvironment env, DatabaseContext context)
    {
        if (env.IsDevelopment())
        {
            _ = app.UseDeveloperExceptionPage();
        }
        else
        {
            _ = app.UseExceptionHandler("/Home/Error");

            // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
            _ = app.UseHsts();
        }
        _ = app.UseHttpsRedirection();
        _ = app.UseStaticFiles();
        var localizeOptions = app.ApplicationServices.GetService<IOptions<RequestLocalizationOptions>>();
        _ = app.UseRequestLocalization(localizeOptions.Value);
        _ = app.UseCookiePolicy(new CookiePolicyOptions()
        {
            MinimumSameSitePolicy = SameSiteMode.Strict,
        });
        _ = app.UseRouting();
        _ = app.UseSwagger();
        _ = app.UseSwaggerUI(c =>
        {
            c.SwaggerEndpoint("/swagger/v2/swagger.json", "My API");
        });
        app.UseCors(x => x
                    .AllowAnyOrigin()
                    .AllowAnyMethod()
                    .AllowAnyHeader()
                    .SetIsOriginAllowed(o => true));
        _ = app.UseAuthentication();
        _ = app.UseAuthorization();
        _ = app.UseEndpoints(endpoints =>
        {
            _ = endpoints.MapControllerRoute(
                name: "default",
                pattern: "{controller=AdminUser}/{action=Limitlesscare}/{id?}");
        });
        context.Database.Migrate();
    }
}