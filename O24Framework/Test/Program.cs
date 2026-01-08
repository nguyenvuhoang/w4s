using Microsoft.Extensions.AI;
using O24OpenAPI.Kit.OCR.Abstractions;
using O24OpenAPI.Kit.OCR.Options;
using O24OpenAPI.Kit.OCR.Services;
using OpenAI;
using Tesseract;
using Test.Features;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllers();
builder.Services.AddOpenApi();
builder.Services.AddSwaggerGen();

// OCR service: đăng ký OUTSIDE factory
var opt = new TesseractOcrOptions
{
    TessdataPath = Path.Combine(AppContext.BaseDirectory, "tessdata"),
    DefaultLanguage = "vie",
    EngineMode = EngineMode.LstmOnly,
    PageSegMode = PageSegMode.SparseText,
};
opt.EngineVariables["user_defined_dpi"] = "300";
opt.EngineVariables["preserve_interword_spaces"] = "1";
builder.Services.AddSingleton<IOcrService>(_ => new TesseractOcrService(opt));

// Chat client
builder.Services.AddSingleton<IChatClient>(sp =>
{
    var config = sp.GetRequiredService<IConfiguration>().GetSection("AI:OpenAI");
    return new OpenAIClient(config["ApiKey"]!)
        .GetChatClient(config["Model"] ?? "gpt-4o-mini")
        .AsIChatClient();
});

builder.Services.AddLinKitCqrs();

// (mục 3) Zalo services sẽ add ở đây (trước Build)
builder.Services.AddMemoryCache();
builder.Services.AddSingleton<Test.Features.Otp.IOtpSender, Test.Features.Otp.MockOtpSender>();
builder.Services.AddSingleton<Test.Features.Otp.OtpService>();
builder.Services.Configure<Test.Features.Otp.OtpOptions>(
    builder.Configuration.GetSection("Otp"));
var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
}

app.UseSwagger();
app.UseSwaggerUI();

app.UseHttpsRedirection();
app.UseAuthorization();
app.MapControllers();
app.MapGeneratedEndpoints();

app.Run();
