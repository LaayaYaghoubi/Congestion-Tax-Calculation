using CongestionTaxCalculator.API.Configs.Services;
using CongestionTaxCalculator.Persistence.EF.SeedData.Contracts;

var config = GetEnvironment();
var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddControllers();

builder.Host.AddAutofacConfig(config);

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();
app.MapControllers();
await SeedData(app);
app.Run();

static IConfigurationRoot GetEnvironment(
    string settingFileName = "appsettings.json")
{
    var baseDirectory = Directory.GetCurrentDirectory();

    return new ConfigurationBuilder()
        .SetBasePath(baseDirectory)
        .AddJsonFile(settingFileName, optional: true, reloadOnChange: true)
        .AddEnvironmentVariables()
        .Build();
}

async Task SeedData(WebApplication webApplication)
{
    using var scope = webApplication.Services.CreateScope();

    var service = scope.ServiceProvider
        .GetRequiredService<SeedDataService>();
    await service.Execute();
}