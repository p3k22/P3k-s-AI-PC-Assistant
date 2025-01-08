var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllers();
builder.Services.AddHttpClient();
builder.Services.AddLogging();
builder.Services.AddScoped<ChatGPT.Services.ChatGptApi>();
builder.Services.AddScoped<OpenAITextToSpeech.Services.TextToSpeech>();
builder.Services.AddScoped<GoogleTextToSpeech.Services.TextToSpeech>();

builder.WebHost.UseUrls("http://0.0.0.0:5000");

var app = builder.Build();

app.MapControllers();

app.Run();

//dotnet publish -c Release -o ./published