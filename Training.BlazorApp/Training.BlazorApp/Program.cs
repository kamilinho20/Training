using Training.BlazorApp.Client.Pages;
using Training.BlazorApp.Components;
using Training.Core.Interface;
using Training.Services;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddRazorComponents()
    .AddInteractiveServerComponents()
    .AddInteractiveWebAssemblyComponents();

builder.Services.AddHttpClient("GymApi", client =>
{
    client.BaseAddress = new Uri("https://localhost:7187/api/"); // Adjust the base address to your API
});

builder.Services.AddScoped<IClientService, ClientService>();
builder.Services.AddScoped<IEmployeeService, EmployeeService>();
builder.Services.AddScoped<IIndividualTrainingService, IndividualTrainingService>();
builder.Services.AddScoped<IGroupTrainingService, GroupTrainingService>();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseWebAssemblyDebugging();
}
else
{
    app.UseExceptionHandler("/Error", createScopeForErrors: true);
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

app.UseHttpsRedirection();

app.UseStaticFiles();
app.UseAntiforgery();

app.MapRazorComponents<App>()
    .AddInteractiveServerRenderMode()
    .AddInteractiveWebAssemblyRenderMode()
    .AddAdditionalAssemblies(typeof(Training.BlazorApp.Client._Imports).Assembly,
    typeof(Training.Razor._Imports).Assembly);

app.Run();
