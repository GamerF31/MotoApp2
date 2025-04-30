using Microsoft.Extensions.DependencyInjection;
using Microsoft.EntityFrameworkCore;
using MotoApp2.Data;
using MotoApp2.Entities;
using MotoApp2.Entities.Repositories;
using MotoApp2.DataProviders;
using System;
using MotoApp2;

class Program
{
    static void Main(string[] args)
    {
        var services = new ServiceCollection();
        services.AddSingleton<IApp, App>();
        services.AddSingleton<IRepository<Car>, SqlRepository<Car>>();              
        services.AddSingleton<IRepository<Customer>, SqlRepository<Customer>>();
        services.AddSingleton<ICarsProvider, CarsProvider>();
        services.AddDbContext<MotoAppDbContext>(options =>
            options.UseSqlServer("Data Source=.\\SQLEXPRESS;Initial Catalog=CarsStorage;Integrated Security=True;Encrypt=False"));
        services.AddDbContext<DbContext, MotoAppDbContext>();
        var serviceProvider = services.BuildServiceProvider();

        var dbContext = serviceProvider.GetRequiredService<MotoAppDbContext>();
        dbContext.Database.EnsureCreated();

        var app = serviceProvider.GetService<IApp>()!;
        app.Run();
    }
}
