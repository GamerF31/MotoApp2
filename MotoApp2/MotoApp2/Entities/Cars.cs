namespace MotoApp2.Entities
{
    public delegate void OperationHandler(string data);

    public class Cars : EntityBase
    {
        public string Model { get; set; }
        public int Year { get; set; }
        public string Country { get; set; }

        public event OperationHandler? CarAdded;

        public void AddCar()
        {
            CarAdded?.Invoke($"Car Added: {Model}, Year: {Year}, Country: {Country}");
        }
        public override string ToString() => $"Id: {Id}, Model: {Model}, Year: {Year}, Country: {Country}";
    }
}