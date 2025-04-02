using MotoApp2.Data;
using MotoApp2.Entities;
using MotoApp2.Entities.Repositories;
using MotoApp2.Entities.Repositories.Extensions;
using System;
using System.IO;

class Program
{
    static void Main(string[] args)
    {
        var carsRepository = new SqlRepository<Cars>(new MotoAppDbContext(), CarsAdded);
        carsRepository.ItemAdded += sqlRepositoryOnItemAdded;

        var customersRepository = new SqlRepository<Customer>(new MotoAppDbContext(), CustomersAdded);
        customersRepository.ItemAdded += sqlRepositoryOnItemAdded2;

        DefaultCars(carsRepository);

        string input;
        do
        {

            Console.WriteLine("\nCustomer View");
            Console.WriteLine("1. Show Customers");
            Console.WriteLine("2. Add Customers");
            Console.WriteLine("3. Show Cars");
            Console.WriteLine("4. Add Cars");
            Console.WriteLine("\nPress q to quit");

            input = Console.ReadLine();
            switch (input)
            {
                case "1":
                    WriteAllToConsole(customersRepository);
                    break;
                case "2":
                    AddCustomers(customersRepository);
                    break;
                case "3":
                    WriteAllToConsole(carsRepository);
                    break;
                case "4":
                    AddCars(carsRepository);
                    break;
                default:
                    if (input != "q")
                        Console.WriteLine("Wrong input, Try again");
                    break;
            }
        } while (input != "q");
    }

    static void CarsAdded(Cars item)
    {
        Console.WriteLine($"{item.Model} added");
        WriteToAuditFile($"{item.Model} added to repository");
    }

    static void CustomersAdded(Customer item)
    {
        Console.WriteLine($"{item.Name} {item.Surname} added");
        WriteToAuditFile($"{item.Name} {item.Surname} added to repository");
    }

    static void sqlRepositoryOnItemAdded(object? sender, Cars e)
    {
        Console.WriteLine($"Cars added => {e.Model} from {sender?.GetType().Name}");
    }

    static void sqlRepositoryOnItemAdded2(object? sender, Customer e)
    {
        Console.WriteLine($"Cars added => {e.Name} {e.Surname} from {sender?.GetType().Name}");
    }

    static void DefaultCars(IRepository<Cars> carsRepository)
    {
        var defaultCars = new[]
        {
            new Cars { Model = "Audi", Year = 2018, Country = "Germany"},
            new Cars { Model = "Ford Focus", Year = 2009, Country = "Sweden"},
            new Cars { Model = "Golf", Year = 2011, Country = "Italy"},
            new Cars { Model = "Fiat", Year = 2012, Country = "Poland"},
            new Cars { Model = "Opel Astra", Year = 2017, Country = "Spain"}
        };
        carsRepository.AddBatch(defaultCars); 
    }

    static void AddCars(IRepository<Cars> carsRepository)
    {
        Console.Write("Enter Car Model: ");
        string model = Console.ReadLine();
        Console.Write("Enter Car Country: ");
        string country = Console.ReadLine();
        Console.Write("Enter Car Year: ");
        if (!int.TryParse(Console.ReadLine(), out int year) || year < 0)
        {
            Console.WriteLine("Invalid Year, Please enter a valid year");
            return;
        }
        var newCar = new Cars
        {
            Model = model,
            Year = year,
            Country = country
        };
        carsRepository.AddBatch(new[] { newCar});
        Console.WriteLine("Car Added");
    }

    static void AddCustomers(IRepository<Customer> customerRepository)
    {
        Console.Write("Customer Name: ");
        string name = Console.ReadLine();
        Console.Write("Customer Surname: ");
        string surname = Console.ReadLine();
        Console.Write("Customer Age: ");
        if (!int.TryParse(Console.ReadLine(), out int age) || age < 0)
        {
            Console.WriteLine("Invalid Age, Please enter a valid age");
            return;
        }

        var newCustomer = new Customer { Name = name, Surname = surname, Age = age, };
        customerRepository.AddBatch(new[] {newCustomer});
        Console.WriteLine("Customer Added");
    }


    static void WriteToAuditFile(string message)
    {
        using (StreamWriter sw = new StreamWriter("audit_log.txt", true))
        {
            sw.WriteLine($"{DateTime.Now} - {message}");
        }
    }

    static void WriteAllToConsole(IReadRepository<IEntity> repository)
    {
        var items = repository.GetAll();
        if (items == null || !items.Any())
        {
            Console.WriteLine("No items found.");
        }
        else
        {
            foreach (var item in items)
            {
                Console.WriteLine(item);
            }
        }
    }
}
