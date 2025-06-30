using System.Net.Security;
using System.Security.Cryptography.X509Certificates;
using Microsoft.AspNetCore.Server.Kestrel;
using Microsoft.AspNetCore.Server.Kestrel.Https;

var builder = WebApplication.CreateBuilder(args);

// Load server certificate from relative path
var config = builder.Configuration;
var serverCertPath = config["Kestrel:Certificates:Server:Path"];
var serverCertPassword = config["Kestrel:Certificates:Server:Password"];
var allowedClientSubject = config["ClientCertificate:AllowedSubject"];
var serverCert = new X509Certificate2(serverCertPath, serverCertPassword);

builder.WebHost.ConfigureKestrel(options =>
{
    options.ListenAnyIP(5001, listen =>
    {
        listen.UseHttps(new HttpsConnectionAdapterOptions
        {
            ServerCertificate = serverCert,
            ClientCertificateMode = ClientCertificateMode.RequireCertificate,
            ClientCertificateValidation = (cert, chain, errors) =>
            {
                //return TestCertificateByForceRejection(cert, chain, errors); // uncomment it to test cert. validation

                Console.WriteLine($"[Server] Received client cert: {cert.Subject}");
                return cert.Subject.Equals(allowedClientSubject, StringComparison.OrdinalIgnoreCase);
            }
        });
    });
});

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

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


bool TestCertificateByForceRejection(X509Certificate2? cert, X509Chain? chain, SslPolicyErrors errors)
{
    Console.WriteLine($"[Server] Cert rejected: {cert.Subject}");
    return false;
}