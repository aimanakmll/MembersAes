using System;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Member.Application;
using Member.Infrastructure;
using Member.Application.Services;
using MediatR;
using Member.Application.Handlers;
using System.Reflection;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container
builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

// Hardcoded connection string (in practice, use configuration)
string connectionString = "Data Source=.\\sqlexpress;Initial Catalog=SystemsAPI;Integrated Security=True";

// Register EncryptServiceBase with a consistent encryption key
string encryptionKey = "ENC"; // Ideally, this should be securely generated or fetched

builder.Services.AddScoped<EncryptServiceBase>(provider => {
    return new EncryptService(encryptionKey); // Single instance of EncryptService with a consistent key
});

// Register MemberRepositoryBase and reuse the same EncryptService
builder.Services.AddScoped<MemberRepositoryBase>(provider =>
{
    var encryptService = provider.GetRequiredService<EncryptServiceBase>(); // Reuse the existing EncryptService
    return new MemberRepository(connectionString, encryptService); // Return repository with connection and encryption
});

// Register RSA encryption service
builder.Services.AddScoped<RSAEncryptService>();

// Other service registrations
builder.Services.AddScoped<MemberServiceBase, MemberService>();

builder.Services.AddMediatR(typeof(AddMemberCommandHandler).Assembly);
builder.Services.AddMediatR(typeof(GetAllMembersQueryHandler).Assembly);

var app = builder.Build();

// Configure the HTTP request pipeline
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();
app.UseAuthorization();

app.MapControllers();

app.Run();
