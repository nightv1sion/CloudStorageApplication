﻿using Audio.API.Model;
using Audio.API.Services;
using Audio.API.Services.Contracts;
using DatabaseInfrastructure.Helper;
using Microsoft.EntityFrameworkCore;

namespace Audio.API.Extensions;

public static class ServicesExtensions
{
    public static void ConfigureDatabaseContext(this IServiceCollection services, IConfiguration configuration)
    {
        if (configuration["ASPNETCORE_ENVIRONMENT"] == "Development")
        {
            services.AddDbContext<ApplicationDatabaseContext>(
                x =>
                {
                    x.UseInMemoryDatabase("TestDatabase");
                });
        }
        else {
            services.AddDbContext<ApplicationDatabaseContext>(
            x => x.UseSqlServer(
                configuration.GetConnectionString()));
        }
    }

    public static void ConfigureServices(this IServiceCollection services)
    {
        services.AddScoped<IFileService, FileService>();
        services.AddScoped<IAudioFileService, AudioFileService>();
    }
}