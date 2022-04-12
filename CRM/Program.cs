
using CRM;
using Microsoft.AspNetCore.Builder;
using System;

var builder = WebApplication.CreateBuilder(args);

App.ConfigureServices(builder.Services, builder.Configuration, builder.Environment);

var app = builder.Build();

AppContext.SetSwitch("Npgsql.EnableLegacyTimestampBehavior", true);

await app
    .Configure(builder.Environment)
    .RunAsync();