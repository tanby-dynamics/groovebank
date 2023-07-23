using Core.Data;
using Core.Features.Samples;
using Core.Features.Storage;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddRazorPages();
builder.Services.AddServerSideBlazor();

builder.Services.AddSingleton<IDbFactory>(new DbFactory(builder.Environment.IsProduction()));
builder.Services.AddScoped<ISampleService, SampleService>();
builder.Services.AddSingleton<IStorageService>(new StorageService(builder.Environment.IsProduction()));

var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Error");
}

app.UseStaticFiles();

app.UseRouting();

app.MapBlazorHub();
app.MapFallbackToPage("/_Host");

app.Run();

