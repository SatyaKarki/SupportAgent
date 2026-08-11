var builder = WebApplication.CreateBuilder(args);
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddSingleton<SupportAgent.Api.DemoDataStore>();
builder.Services.AddSingleton<SupportAgent.Api.SupportTools>();
builder.Services.AddSingleton<SupportAgent.Api.SupportAgent>();
builder.Services.AddControllers();

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseRouting();
app.UseAuthorization();

app.MapControllers();

app.Run();
