namespace MotoApp2.Entities
{
    public delegate void OperationHandler(string data);

    public class Car : EntityBase
    {
        public string Model { get; set; }
        public int Year { get; set; }
        public string Country { get; set; }
        public decimal StandardCost { get; set; }

        public event OperationHandler? CarAdded;

        public void AddCar()
        {
            CarAdded?.Invoke($"Car Added: {Model}, Year: {Year}, Country: {Country}, StandardCost: {StandardCost}");
        }
        public override string ToString() => $"Id: {Id}, Model: {Model}, Year: {Year}, Country: {Country}, StandardCost: {StandardCost}";
    }
}