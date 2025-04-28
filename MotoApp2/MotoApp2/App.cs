using MotoApp2.Entities;
using MotoApp2.Entities.Repositories;
using MotoApp2.DataProviders;
using System;
using System.IO;
using System.Linq;
using System.Xml.Linq;
using System.Collections.Generic;
using MotoApp2.Entities.Repositories.Extensions;
using MotoApp2.Data;

namespace MotoApp2
{
    public class App : IApp
    {
        private readonly IRepository<Car> _carsRepository;
        private readonly IRepository<Customer> _customersRepository;
        private readonly ICarsProvider _carsProvider;

        public App(IRepository<Car> carsRepository, IRepository<Customer> customersRepository, ICarsProvider carsProvider)
        {
            _carsRepository = carsRepository;
            _customersRepository = customersRepository;
            _carsProvider = carsProvider;
        }

        public void Run()
        {
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
                Console.WriteLine("11.Edit Car by ID");
                Console.WriteLine("12.Delete Car by ID");
                Console.WriteLine("\nPress q to quit");

                input = Console.ReadLine();
                switch (input)
                {
                    case "1":
                        if (!_customersRepository.GetAll().Any())
                        {
                            Console.WriteLine("Customers repository is empty");
                        }
                        WriteAllToConsole(_customersRepository);
                        break;
                    case "2":
                        AddCustomer();
                        break;
                    case "3":
                        if (!_carsRepository.GetAll().Any())
                        {
                            DefaultCars(_carsRepository);
                        }
                        WriteAllToConsole(_carsRepository);
                        break;
                    case "4":
                        AddCar();
                        break;
                    case "5":
                        FilterCarsByCountry();
                        break;
                    case "6":
                        ShowFirstNCars();
                        break;
                    case "7":
                        FindCarById();
                        break;
                    case "8":
                        ShowMinCarCost();
                        break;
                    case "9":
                        OrderCarsByModel();
                        break;
                    case "10":
                        var carsXml = QueryToXml();
                        foreach (var car in carsXml)
                        {
                            Console.WriteLine(car);
                        }
                        break;
                    case "11":
                        EditCar();
                        break;
                    case "12":
                        DeleteCar();
                        break;
                    default:
                        if (input != "q")
                            Console.WriteLine("Wrong input, Try again");
                        break;
                }
            } while (input != "q");
            CreateToXml(_carsRepository.GetAll(), _customersRepository.GetAll());
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
        private void WriteAllToConsole<T>(IRepository<T> repository) where T : class, IEntity
        {
            var items = repository.GetAll();
            foreach (var item in items)
            {
                Console.WriteLine(item);
            }
        }

        private void AddCustomer()
        {
            Console.Write("Customer Name: ");
            string name = Console.ReadLine();
            Console.Write("Customer Surname: ");
            string surname = Console.ReadLine();
            Console.Write("Customer Age: ");
            int age = int.Parse(Console.ReadLine());

            var newCustomer = new Customer { Name = name, Surname = surname, Age = age };
            _customersRepository.Add(newCustomer);
            _customersRepository.Save();
            Console.WriteLine("Customer Added");

            CreateToXml(_carsRepository.GetAll(), _customersRepository.GetAll());  
        }

        private void AddCar()
        {
            Console.Write("Enter Car Model: ");
            string model = Console.ReadLine();
            Console.Write("Enter Car Country: ");
            string country = Console.ReadLine();
            Console.Write("Enter Car Year: ");
            int year = int.Parse(Console.ReadLine());
            Console.Write("Enter Car Cost: ");
            decimal cost = decimal.Parse(Console.ReadLine());

            var newCar = new Car
            {
                Model = model,
                Year = year,
                Country = country,
                StandardCost = cost
            };

            _carsRepository.Add(newCar);
            _carsRepository.Save();
            Console.WriteLine("Car Added");

            CreateToXml(_carsRepository.GetAll(), _customersRepository.GetAll());  
        }

        private void FilterCarsByCountry()
        {
            Console.Write("Enter country: ");
            string country = Console.ReadLine();
            var filteredCars = _carsProvider.FilterByCountry(country);
            foreach (var car in filteredCars)
            {
                Console.WriteLine(car);
            }
        }

        private void ShowFirstNCars()
        {
            Console.Write("Enter the number of cars to display: ");
            int n = int.Parse(Console.ReadLine());
            var cars = _carsProvider.TakeCars(n);
            foreach (var car in cars)
            {
                Console.WriteLine(car);
            }
        }

        private void FindCarById()
        {
            Console.Write("Enter car ID: ");
            int id = int.Parse(Console.ReadLine());
            var car = _carsProvider.GetById(id);
            if (car != null)
            {
                Console.WriteLine(car);
            }
            else
            {
                Console.WriteLine("Car not found.");
            }
        }

        private void ShowMinCarCost()
        {
            var minCost = _carsProvider.GetMinCost();
            Console.WriteLine($"Minimum car cost: {minCost}");
        }

        private void OrderCarsByModel()
        {
            var cars = _carsProvider.OrderByModel();
            foreach (var car in cars)
            {
                Console.WriteLine(car);
            }
        }

        /*
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
        static void WriteToAuditFile(string message)
        {
            using (StreamWriter sw = new StreamWriter("audit_log.txt", true))
            {
                sw.WriteLine($"{DateTime.Now} - {message}");
            }
        }
        */
        private void EditCar()
        {
            Console.Write("Enter Car ID to edit: ");
            int id = int.Parse(Console.ReadLine());

            var car = _carsProvider.GetById(id);
            if (car == null)
            {
                Console.WriteLine("Car not found.");
                return;
            }

            Console.WriteLine($"Editing Car: {car.Model}, Year: {car.Year}, Country: {car.Country}, Cost: {car.StandardCost}");

            Console.Write("Enter new Model (leave empty to keep current): ");
            string model = Console.ReadLine();
            if (!string.IsNullOrWhiteSpace(model)) car.Model = model;

            Console.Write("Enter new Country (leave empty to keep current): ");
            string country = Console.ReadLine();
            if (!string.IsNullOrWhiteSpace(country)) car.Country = country;

            Console.Write("Enter new Year (leave empty to keep current): ");
            string yearInput = Console.ReadLine();
            if (int.TryParse(yearInput, out int year)) car.Year = year;

            Console.Write("Enter new Cost (leave empty to keep current): ");
            string costInput = Console.ReadLine();
            if (decimal.TryParse(costInput, out decimal cost)) car.StandardCost = cost;

            _carsRepository.Save();
            Console.WriteLine("Car updated successfully.");

            CreateToXml(_carsRepository.GetAll(), _customersRepository.GetAll());
        }

        private void DeleteCar()
        {
            Console.Write("Enter Car ID to delete: ");
            int id = int.Parse(Console.ReadLine());

            var car = _carsProvider.GetById(id);
            if (car == null)
            {
                Console.WriteLine("Car not found.");
                return;
            }

            _carsRepository.Remove(car);
            _carsRepository.Save();
            Console.WriteLine("Car deleted successfully.");

            CreateToXml(_carsRepository.GetAll(), _customersRepository.GetAll());
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
                )
            );
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
}
