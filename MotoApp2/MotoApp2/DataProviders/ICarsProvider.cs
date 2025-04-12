using MotoApp2.Entities;
namespace MotoApp2.DataProviders
{
    public interface ICarsProvider
    {
        List<Car> FilterByCountry(string country);
        List<Car> TakeCars(int count);
        Car? GetById(int id);
        decimal GetMinCost();
        List<Car> OrderByModel();

    }
}
