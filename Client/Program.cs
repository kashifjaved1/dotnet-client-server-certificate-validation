using System.Security.Cryptography.X509Certificates;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

// Bind client certificate from config
var config = builder.Configuration;
var clientCertPath = config["ClientCertificate:Path"];
var clientCertPassword = config["ClientCertificate:Password"];
var clientCertificate = new X509Certificate2(clientCertPath, clientCertPassword);

// Register typed HttpClient with certificate
builder.Services.AddHttpClient("ServerClient")
.ConfigurePrimaryHttpMessageHandler(() =>
{
    var handler = new HttpClientHandler();
    handler.ClientCertificates.Add(clientCertificate);
    handler.ServerCertificateCustomValidationCallback = (message, cert, chain, errors) =>
    {
        Console.WriteLine($"[ClientAPI] Server cert: {cert.Subject}");
        return cert.Subject.Contains("CN=localhost");
    };

    return handler;
});

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();
app.UseAuthorization();
app.MapControllers();
app.Run();