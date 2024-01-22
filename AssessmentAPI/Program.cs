using AssessmentAPI.DataContext;
using AssessmentAPI.Repositories;
using AssessmentAPI.Service;
using MySqlConnector;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

//Injecting the connection string
builder.Services.AddMySqlDataSource(builder.Configuration.GetConnectionString("DefaultConnection"));

//Injecting DapperContext
builder.Services.AddSingleton<DapperContext>();

//Injecting Repository
builder.Services.AddSingleton<JobRepository>();
builder.Services.AddSingleton<WeatherRepository>();
builder.Services.AddSingleton<WeatherDatafetcher>();
builder.Services.AddHttpClient<WeatherDatafetcher>();

//Injecting health check
builder.Services.AddHealthChecks();

//Register services
builder.Services.AddHostedService<RegularIntervalService>();


var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
