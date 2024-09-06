using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using SoapApi2.Contracts;
using SoapApi2.Infrastructure;
using SoapApi2.Repositories;
using SoapApi2.Services;
using SoapCore;


var builder = WebApplication.CreateBuilder(args);
builder.Services.AddSoapCore();
builder.Services.AddScoped<IUserRepository, UserRespository>();
builder.Services.AddScoped<IUserContract, UserService>();

builder.Services.AddScoped<IBookRepository, BookRespository>();
builder.Services.AddScoped<IBookContract, BookService>();


builder.Services.AddDbContext<RelationalDbContext>(options => 
    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection"))
);


var app = builder.Build();
app.UseSoapEndpoint<IUserContract>("/UserService.svc", new SoapEncoderOptions());
app.UseSoapEndpoint<IBookContract>("/BookService.svc", new SoapEncoderOptions());

var app = builder.Build();
app.UseSoapEndpoint<IUserContract>("/UserService.svc", new SoapEncoderOptions());

app.Run();