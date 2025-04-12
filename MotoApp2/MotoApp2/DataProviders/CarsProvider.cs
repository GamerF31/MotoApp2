using MotoApp2.Entities;
using MotoApp2.Entities.Repositories;

namespace MotoApp2.DataProviders
{
    public class CarsProvider : ICarsProvider
    {
        private readonly IRepository<Car> _carsRepository;

        public CarsProvider(IRepository<Car> carsRepository)
        {
            _carsRepository = carsRepository;
        }

        public List<Car> FilterByCountry(string country)
        {
            return _carsRepository.GetAll()
                .Where(car => car.Country.Equals(country, StringComparison.OrdinalIgnoreCase))
                .ToList();
        }

        public List<Car> TakeCars(int count)
        {
            return _carsRepository.GetAll()
                .Take(count)
                .ToList();
        }

        public Car? GetById(int id)
        {
            return _carsRepository.GetAll()
                .FirstOrDefault(car => car.Id == id);
        }

        public decimal GetMinCost()
        {
            return _carsRepository.GetAll()
                .Min(car => car.StandardCost);
        }

        public List<Car> OrderByModel()
        {
            return _carsRepository.GetAll()
                .OrderBy(car => car.Model)
                .ToList();
        }
    }
}
