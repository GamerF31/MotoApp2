using MotoApp2.Data;
using MotoApp2.DataProviders;
using MotoApp2.Entities;
using MotoApp2.Entities.Repositories;
using MotoApp2.Entities.Repositories.Extensions;
using System;
using System.IO;
using System.Xml.Linq;

class Program
{
    static void Main(string[] args)
    {
        var carsRepository = new SqlRepository<Car>(new MotoAppDbContext(), CarsAdded);
        carsRepository.ItemAdded += sqlRepositoryOnItemAdded;

        var customersRepository = new SqlRepository<Customer>(new MotoAppDbContext(), CustomersAdded);
        customersRepository.ItemAdded += sqlRepositoryOnItemAdded2;

        var carsProvider = new CarsProvider(carsRepository);

        DefaultCars(carsRepository);

        CreateToXml(carsRepository.GetAll(), customersRepository.GetAll());

        QueryToXml();

        string input;
        do
        {

            Console.WriteLine("\nCustomer View");
            Console.WriteLine("1. Show Customers");
            Console.WriteLine("2. Add Customers");
            Console.WriteLine("3. Show Cars");
            Console.WriteLine("4. Add Cars");
            Console.WriteLine("5. Filter Cars by Country");
            Console.WriteLine("6. Show First N Cars");
            Console.WriteLine("7. Find Car by ID");
            Console.WriteLine("8. Show Minimum Car Cost");
            Console.WriteLine("9. Order Cars by Model");
            Console.WriteLine("10. Import Cars from XML");
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
                case "5":
                    Console.Write("Enter country: ");
                    string country = Console.ReadLine();
                    var filtered = carsProvider.FilterByCountry(country);
                    PrintCars(filtered);
                    break;
                case "6":
                    Console.Write("How many cars? ");
                    if (int.TryParse(Console.ReadLine(), out int count))
                    {
                        var taken = carsProvider.TakeCars(count);
                        PrintCars(taken);
                    }
                    else Console.WriteLine("Invalid number.");
                    break;
                case "7":
                    Console.Write("Enter car ID: ");
                    if (int.TryParse(Console.ReadLine(), out int id))
                    {
                        var car = carsProvider.GetById(id);
                        Console.WriteLine(car != null ? car.ToString() : "Car not found.");
                    }
                    else Console.WriteLine("Invalid ID.");
                    break;
                case "8":
                    Console.WriteLine($"Minimum car cost: {carsProvider.GetMinCost()}");
                    break;
                case "9":
                    var ordered = carsProvider.OrderByModel();
                    PrintCars(ordered);
                    break;
                case "10":
                    var carsXml = QueryToXml();
                    foreach(var car in carsXml)
                    {
                        Console.WriteLine(car);

                    }
                    break;
                default:
                    if (input != "q")
                        Console.WriteLine("Wrong input, Try again");
                    break;
            }
        } while (input != "q");
        CreateToXml(carsRepository.GetAll(), customersRepository.GetAll());
    }

    static void CarsAdded(Car item)
    {
        Console.WriteLine($"{item.Model} added");
        WriteToAuditFile($"{item.Model} added to repository");
    }

    static void CustomersAdded(Customer item)
    {
        Console.WriteLine($"{item.Name} {item.Surname} added");
        WriteToAuditFile($"{item.Name} {item.Surname} added to repository");
    }

    static void sqlRepositoryOnItemAdded(object? sender, Car e)
    {
        Console.WriteLine($"Cars added => {e.Model} from {sender?.GetType().Name}");
    }

    static void sqlRepositoryOnItemAdded2(object? sender, Customer e)
    {
        Console.WriteLine($"Cars added => {e.Name} {e.Surname} from {sender?.GetType().Name}");
    }

    static void DefaultCars(IRepository<Car> carsRepository)
    {
        var defaultCars = new[]
        {
            new Car { Model = "Audi", Year = 2018, Country = "Germany", StandardCost = 21000M },
            new Car { Model = "Ford Focus", Year = 2009, Country = "Sweden", StandardCost = 8700M },
            new Car { Model = "Golf", Year = 2011, Country = "Italy", StandardCost = 11000M },
            new Car { Model = "Fiat", Year = 2012, Country = "Poland", StandardCost = 7500M },
            new Car { Model = "Opel Astra", Year = 2017, Country = "Spain", StandardCost = 15000M },
            new Car { Model = "Toyota Corolla", Year = 2019, Country = "Japan", StandardCost = 18500M },
            new Car { Model = "Peugeot 208", Year = 2015, Country = "France", StandardCost = 9700M },
            new Car { Model = "BMW 3 Series", Year = 2020, Country = "Germany", StandardCost = 28000M },
            new Car { Model = "Tesla Model 3", Year = 2021, Country = "USA", StandardCost = 35000M },
            new Car { Model = "Lada Vesta", Year = 2018, Country = "Russia", StandardCost = 6400M },
            new Car { Model = "Volvo XC60", Year = 2016, Country = "Sweden", StandardCost = 22500M },
            new Car { Model = "Renault Clio", Year = 2014, Country = "France", StandardCost = 8400M },
            new Car { Model = "Hyundai i30", Year = 2019, Country = "South Korea", StandardCost = 14000M },
            new Car { Model = "Mazda 3", Year = 2017, Country = "Japan", StandardCost = 16000M },
            new Car { Model = "Seat Leon", Year = 2015, Country = "Spain", StandardCost = 10200M },
            new Car { Model = "Skoda Octavia", Year = 2018, Country = "Czech Republic", StandardCost = 13200M },
            new Car { Model = "Chevrolet Spark", Year = 2013, Country = "USA", StandardCost = 5800M },
            new Car { Model = "Kia Ceed", Year = 2020, Country = "South Korea", StandardCost = 15300M },
            new Car { Model = "Alfa Romeo Giulietta", Year = 2016, Country = "Italy", StandardCost = 17200M },
            new Car { Model = "Dacia Duster", Year = 2021, Country = "Romania", StandardCost = 12500M }
};
        carsRepository.AddBatch(defaultCars);
    }


    static void AddCars(IRepository<Car> carsRepository)
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
        Console.Write("Enter Car Cost: ");
        if (!decimal.TryParse(Console.ReadLine(), out decimal cost) || cost < 0)
        {
            Console.WriteLine("Invalid Cost, Please enter a valid value");
            return;
        }

        var newCar = new Car
        {
            Model = model,
            Year = year,
            Country = country,
            StandardCost = cost
        };
        carsRepository.AddBatch(new[] { newCar });
        Console.WriteLine("Car Added");
        CreateToXml(carsRepository.GetAll(), new List<Customer>());
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

        var newCustomer = new Customer { Name = name, Surname = surname, Age = age };
        customerRepository.AddBatch(new[] { newCustomer });
        Console.WriteLine("Customer Added");
        CreateToXml(new List<Car>(), customerRepository.GetAll());
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
    static void PrintCars(List<Car> cars)
    {
        if (!cars.Any())
        {
            Console.WriteLine("No cars found.");
            return;
        }

        foreach (var car in cars)
        {
            Console.WriteLine(car);
        }
    }
    
    static void CreateToXml(IEnumerable<Car> cars, IEnumerable<Customer> customers)
    {
        var document = new XDocument(
            new XElement("Data",
                new XElement("Cars",
                    cars.Select(car => new XElement("Car",
                        new XAttribute("Model", car.Model),
                        new XAttribute("Year", car.Year),
                        new XAttribute("Country", car.Country),
                        new XAttribute("Cost", car.StandardCost)))),
                new XElement("Customers",
                customers.Select(customer => new XElement("Customer",
                    new XAttribute("Name", customer.Name),
                    new XAttribute("Surname", customer.Surname),
                    new XAttribute("Age", customer.Age))))
                ));
        document.Save("CarsAndCustomers.xml");
    }

    static List<Car> QueryToXml()
    {
        if (!File.Exists("CarsAndCustomers.xml"))
        {
            Console.WriteLine("XML file not found.");
            return new List<Car>(); 
        }

        var document = XDocument.Load("CarsAndCustomers.xml");
        var cars = document.Element("Data")?.Element("Cars")?.Elements("Car")
            .Select(x => new Car
            {
                Model = x.Attribute("Model")?.Value,
                Year = int.Parse(x.Attribute("Year")?.Value),
                Country = x.Attribute("Country")?.Value,
                StandardCost = decimal.Parse(x.Attribute("Cost")?.Value)
            }).ToList();

        return cars;
    }

}
