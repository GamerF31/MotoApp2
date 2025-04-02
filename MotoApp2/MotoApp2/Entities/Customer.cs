namespace MotoApp2.Entities
{
    public class Customer : EntityBase
    {
        public string Name { get; set; }
        public string Surname { get; set; }
        public int Age { get; set; }
        public override string ToString() => $"Id: {Id},Customer Name: {Name}, Customer Surname {Surname}, Customer Age {Age}";
    }
}