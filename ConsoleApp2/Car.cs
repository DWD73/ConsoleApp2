using System;
using System.Text.Json.Serialization;

namespace ConsoleApp11
{
    public class Car
    {
        Random rnd = new Random();

        public Model Model { get; private set; }
        public double Price { get; set; }
        [JsonPropertyName("Qty")]
        public int Quantity { get; set; }                   
        public int HashcodeCar { get; set; }      

        public Car()
        {

        }

        public void GetDataCar()
        {
            Model = (Model)rnd.Next(4);
            Price = Convert.ToDouble(rnd.Next(1000, 15000) * 1.5);
            Quantity = rnd.Next(1, 10);          
            HashcodeCar = this.GetHashCode();
            
        }     

        public override string ToString()
        {
            return $"Общая сумма заказа {Price * Quantity}";
        }

    }   
}
