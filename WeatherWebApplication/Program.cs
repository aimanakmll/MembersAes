using System;
using System.Security.Cryptography;
using Member.Application;
using Member.Infrastructure;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

// Hardcoded connection string
string connectionString = "Data Source=.\\sqlexpress;Initial Catalog=SystemsAPI;Integrated Security=True";

//// Register services with dependency injection
builder.Services.AddScoped<IMemberRepository>(provider =>
{
    // Generate or fetch the encryption key
    string encryptionKey = "ENC"; // You should generate or retrieve this securely

    var encryptService = new EncryptService(encryptionKey);
    return new MemberRepository(connectionString, encryptService);
});

builder.Services.AddScoped<IMemberService, MemberService>();

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





// Generate a random encryption key
//byte[] key = new byte[32]; // 256 bits key size
//using (var rng = new RNGCryptoServiceProvider())
//{
//    rng.GetBytes(key);
//}
//string encryptionKey = Convert.ToBase64String(key);