using PriceBot.API.Configurations;
using PriceBot.Infra.IoC;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddSettingsConfiguration(builder.Configuration);
builder.Services.RegisterServices(builder.Configuration);
builder.Services.AddHangfireConfiguration();

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHangfireConfiguration();
app.UseControllersConfiguration();
SeedConfiguration.Seed(app);

app.Run();
