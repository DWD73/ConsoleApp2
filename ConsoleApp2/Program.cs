using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace ConsoleApp11
{
    class Program
    {
        static void Main(string[] args)
        {
            TestCar testCar = new TestCar();

            Console.WriteLine("Заказать авто");
            Console.ReadLine();         

            var (jsonText, elapsedTime) = testCar.CarSerialize();
            Console.Out.WriteLine($"{jsonText}\n\nВремя процесса сериализации\t{elapsedTime}");

            Console.ReadLine();

            Console.WriteLine("Получить счет");

            Console.ReadLine();

            var (jsonText2, elapsedTime2, total) = testCar.CarDeSerialize();
            Console.Out.WriteLine($"{jsonText2}\n\nВремя процесса десериализации\t{elapsedTime2}\n\n{total}");        

            Console.ReadLine();



        }
    }  

    public class TestCar
    {
        public Car car;
        Stopwatch stopwatch;
        string jsonCar;

        public TestCar()
        {          
            car = new Car();
            car.GetDataCar();
        }
        
        private static JsonSerializerOptions GetOptions()
        {
            var options = new JsonSerializerOptions()
            {
                PropertyNamingPolicy = JsonNamingPolicy.CamelCase,
                WriteIndented = true,
                IgnoreNullValues = true,
                DictionaryKeyPolicy = JsonNamingPolicy.CamelCase,
                Converters =
                {
                    new JsonStringEnumConverter(JsonNamingPolicy.CamelCase)
                }
            };

            return options;

        }

        

        public (string jsonCar, string elapsedTime) CarSerialize()
        {           
            stopwatch = stopwatch ??= new Stopwatch();

            stopwatch.Start();
            
            jsonCar = JsonSerializer.Serialize<Car>(car, GetOptions());          
            File.WriteAllText("Car.csv", jsonCar);

            stopwatch.Stop();
            
            TimeSpan ts = stopwatch.Elapsed;

            string elapsedTime = String.Format("{0:00}:{1:00}:{2:00}.{3:00}",
            ts.Hours, ts.Minutes, ts.Seconds,
            ts.Milliseconds / 10);

            return (jsonCar, elapsedTime);         
        }

        public (string jsonCar, string elapsedTime, string total) CarDeSerialize()
        {
            stopwatch = stopwatch ??= new Stopwatch();
            stopwatch.Start();

            jsonCar = File.ReadAllText("Car.csv");
            Car car2 = JsonSerializer.Deserialize<Car>(jsonCar, GetOptions());

            stopwatch.Stop();

            TimeSpan ts = stopwatch.Elapsed;

            string elapsedTime = String.Format("{0:00}:{1:00}:{2:00}.{3:00}",
            ts.Hours, ts.Minutes, ts.Seconds,
            ts.Milliseconds / 10);
           
            string total = car.ToString();

            return (jsonCar, elapsedTime, total);
        }


    }

    public class Car
    {
        Random rnd = new Random();

        public Model Model { get; set; }
        public double Price { get; set; }
        [JsonPropertyName("Qty")]
        public int Quantity { get; set; }
        public Dictionary<int, string> dictionary { get; set; } = default;            
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
            dictionary = dictionary ?? new Dictionary<int, string>();
            dictionary?.Add(1, $"{HashcodeCar}");
        }     

        public override string ToString()
        {
            return $"Общая сумма заказа {Price * Quantity}";
        }

    }   

    public enum Model : int
    {
        Toyote, Volvo, Hyundai, Kia
    }
}
