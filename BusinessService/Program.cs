using BusinessService.Kafka;
using BusinessService.MasstransitRabbitMq;
using GreenPipes;
using MassTransit;
using MassTransit.Definition;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.Extensions.FileProviders;
using Microsoft.IdentityModel.Tokens;
using System.Text;
using System.Text.Json.Serialization;


var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
// For MYSQL
builder.Services.AddDbContext<AppDbContext>(
options =>
{
    options.UseMySql(builder.Configuration.GetConnectionString("DevConnection"),
    Microsoft.EntityFrameworkCore.ServerVersion.Parse("8.0.23-mysql"));
});

builder.Services.AddTransient<IProductRepository, ProductRepository>();
builder.Services.AddTransient<IEmployeeService, EmployeeService>();

// For Masstransit and RabbitMq
builder.Services.AddMassTransit(x =>
{
    x.AddConsumer<CategoryAddUpdateConsumer>();
    x.AddConsumer<CategoryDeleteConsumer>();
    x.AddBus(provider => Bus.Factory.CreateUsingRabbitMq(cfg =>
    {
        cfg.Host(new Uri("rabbitmq://localhost"), h =>
        {
            h.Username("guest");
            h.Password("guest");
        });
        cfg.ReceiveEndpoint("AddUpdateCategoryQueue", ep =>
        {
            ep.PrefetchCount = 16;
            ep.UseMessageRetry(r => r.Interval(2, 100));
            ep.ConfigureConsumer<CategoryAddUpdateConsumer>(provider);
        });
        cfg.ReceiveEndpoint("DeleteCategoryQueue", ep =>
        {
            ep.PrefetchCount = 16;
            ep.UseMessageRetry(r => r.Interval(2, 100));
            ep.ConfigureConsumer<CategoryDeleteConsumer>(provider);
        });
    }));
});

builder.Services.AddMassTransitHostedService();
// For Kafka
builder.Services.AddHostedService<AddUpdateProvinceCityConsumer>();
builder.Services.AddHostedService<DeleteProvinceCityConsumer>();
// For InMemoryCache
builder.Services.AddScoped<IInMemoryCacheService, InMemoryCacheService>();

builder.Services.AddControllers().AddJsonOptions(options =>
{
    options.JsonSerializerOptions.ReferenceHandler = ReferenceHandler.IgnoreCycles;
    options.JsonSerializerOptions.WriteIndented = true;
});

// For IHttpClientFactory in HttpClient 
builder.Services.AddHttpContextAccessor();
builder.Services.AddHttpClient("Employee", u => u.BaseAddress =
new Uri(builder.Configuration["ServiceUrls:EmployeeAPI"]));

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
// Adding Authentication  
builder.Services.AddAuthentication(options =>
{
    options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
    options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
    options.DefaultScheme = JwtBearerDefaults.AuthenticationScheme;
})

// Adding Jwt Bearer  
.AddJwtBearer(options =>
{
    options.SaveToken = true;
    options.RequireHttpsMetadata = false;
    options.TokenValidationParameters = new TokenValidationParameters()
    {
        ValidateIssuer = true,
        ValidateAudience = true,
        ValidAudience = builder.Configuration["JWT:ValidAudience"],
        ValidIssuer = builder.Configuration["JWT:ValidIssuer"],
        IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(builder.Configuration["JWT:Secret"]))
    };
});
var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}
// To connect with Frontend like Angular
app.UseCors(options => options.WithOrigins("http://localhost:4200")
   .AllowAnyMethod()
   .AllowAnyHeader()
);
app.UseHttpsRedirection();
app.UseStaticFiles(new StaticFileOptions()
{
    FileProvider = new PhysicalFileProvider(Path
    .Combine(Directory.GetCurrentDirectory(), @"Resources")),
    RequestPath = new PathString("/Resources")
});
app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();

app.Run();
