using FluentValidation;
using HotelsBooking.API.Adapters;
using HotelsBooking.API.Constants;
using HotelsBooking.API.Middlewares;
using HotelsBooking.API.Options;
using HotelsBooking.BLL.DTO;
using HotelsBooking.BLL.Interfaces;
using HotelsBooking.BLL.Services;
using HotelsBooking.BLL.Validators;
using HotelsBooking.DAL.Data;
using HotelsBooking.DAL.Interfaces;
using HotelsBooking.DAL.Repositories;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using System.Text;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(c =>
{
    c.SwaggerDoc("v1", new OpenApiInfo { Title = "HotelsBooking API", Version = "v1" });

    c.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
    {
        Description = "JWT Authorization header using the Bearer scheme",
        Type = SecuritySchemeType.Http,
        Scheme = "bearer"
    });

    c.AddSecurityRequirement(new OpenApiSecurityRequirement
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

builder.Services.AddDbContext<ApplicationContext>(options =>
{
    options.UseNpgsql(
        builder.Configuration.GetConnectionString(nameof(ApplicationContext)),
        o => o.UseNetTopologySuite()
    );
});

builder.Services.AddAutoMapper(cfg =>
{
    cfg.AddMaps(typeof(Program).Assembly);
    cfg.AddMaps(typeof(AuthService).Assembly);
    cfg.AddMaps(typeof(HotelService).Assembly);
    cfg.AddMaps(typeof(RoomService).Assembly);
    cfg.AddMaps(typeof(AmenityService).Assembly);
    cfg.AddMaps(typeof(ReviewService).Assembly);
    cfg.AddMaps(typeof(BookingService).Assembly);
});

builder.Services.AddScoped<IAuthService, AuthService>();
builder.Services.AddScoped<ITokensService, TokensService>();
builder.Services.AddScoped<IPasswordService, PasswordService>();
builder.Services.AddScoped<IHotelService, HotelService>();
builder.Services.AddScoped<IImageService, ImageService>();
builder.Services.AddScoped<IRoomService, RoomService>();
builder.Services.AddScoped<IAmenityService, AmenityService>();
builder.Services.AddScoped<IReviewService, ReviewService>();
builder.Services.AddScoped<IBookingService, BookingService>();
builder.Services.AddScoped<IPdfGenerator, PdfGenerator>();
builder.Services.AddScoped<ISmtpEmailSender, SmtpEmailSender>();
builder.Services.AddScoped<IStripeService, StripeService>();

builder.Services.AddSingleton<IRootPath, WebHostAdapter>();

builder.Services.AddScoped<IUserRepository, UserRepository>();
builder.Services.AddScoped<IHotelRepository, HotelRepository>();
builder.Services.AddScoped<IHotelPhotoRepository, HotelPhotoRepository>();
builder.Services.AddScoped<IRoomRepository, RoomRepository>();
builder.Services.AddScoped<IRoomPhotoRepository, RoomPhotoRepository>();
builder.Services.AddScoped<IAmenityRepository, AmenityRepository>();
builder.Services.AddScoped<IReviewRepository, ReviewRepository>();
builder.Services.AddScoped<IBookingRepository, BookingRepository>();

//builder.Services.AddFluentValidationAutoValidation();
builder.Services.AddScoped<IValidator<RegisterDTO>, RegisterDTOValidator>();
builder.Services.AddScoped<IValidator<LoginDTO>, LoginDTOValidator>();
builder.Services.AddScoped<IValidator<CreateHotelDTO>, CreateHotelDTOValidator>();
builder.Services.AddScoped<IValidator<UpdateHotelDTO>, UpdateHotelDTOValidator>();
builder.Services.AddScoped<IValidator<CreateRoomDTO>, CreateRoomDTOValidator>();
builder.Services.AddScoped<IValidator<UpdateRoomDTO>, UpdateRoomDTOValidator>();
builder.Services.AddScoped<IValidator<CreateAmenityDTO>, CreateAmenityDTOValidator>();
builder.Services.AddScoped<IValidator<UpdateAmenityDTO>, UpdateAmenityDTOValidator>();
builder.Services.AddScoped<IValidator<CreateReviewDTO>, CreateReviewDTOValidator>();
builder.Services.AddScoped<IValidator<UpdateReviewDTO>, UpdateReviewDTOValidator>();
builder.Services.AddScoped<IValidator<CreateBookingDTO>, CreateBookingDTOValidator>();
builder.Services.AddScoped<IValidator<UpdateBookingStatusDTO>, UpdateBookingStatusDTOValidator>();


builder.Services.Configure<JwtOptions>(builder.Configuration.GetSection("JWT"));
var jwtOptions = builder.Configuration.GetSection("JWT").Get<JwtOptions>();

builder.Services.AddAuthentication(options =>
{
    options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
    options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
})
    .AddJwtBearer(options =>
    {
        options.TokenValidationParameters = new TokenValidationParameters
        {
            ValidateIssuer = true,
            ValidateAudience = true,
            ValidateLifetime = true,
            ValidateIssuerSigningKey = true,
            ValidIssuer = jwtOptions.Issuer,
            ValidAudience = jwtOptions.Audience,
            IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(jwtOptions.Key))
        };
    });

builder.Services.AddAuthorizationBuilder()
    .AddPolicy(Policies.Admin, policy => policy.RequireRole(Roles.Admin))
    .AddPolicy(Policies.HotelOwner, policy => policy.RequireRole(Roles.HotelOwner))
    .AddPolicy(Policies.Client, policy => policy.RequireRole(Roles.Client));

var app = builder.Build();

app.UseMiddleware<ExceptionHandlingMiddleware>();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();
app.UseStaticFiles();

app.Run();
