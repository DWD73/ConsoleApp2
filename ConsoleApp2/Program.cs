using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Text.Json;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace ConsoleApp11
{
    class Program
    {
        static void Main(string[] args)
        {
            TestCar testCar = new TestCar();

            while (true)
            {               
                Console.Write("Заказать авто?\nУкажите количество - ");
                //Console.ReadLine();

                int countCar = 1;
                
                if(int.TryParse(Console.ReadLine(), out countCar))
                {
                    Start(testCar, countCar);
                }
                else 
                {
                    Console.WriteLine("Не корректное число");
                }           

            }

        }

        private static void Start(TestCar testCar, int countCar)
        {
            string jsonTextIn = default, jsonTextOut = default;

            for(int i = 0; i < countCar; i++)
            {
                testCar.PayCar();
            
                var (jsonText, elapsedTime) = testCar.CarSerialize(true);
                jsonTextIn = $"{jsonText}\n\nВремя процесса json-сериализации\t{elapsedTime}\n\n";
                
              
                var (jsonText2, elapsedTime2, total) = testCar.CarDeSerialize(true);
                jsonTextOut = $"{jsonText2}\n\nВремя процесса json-десериализации\t{elapsedTime2}\n\n{total}\n\n";               

            }
            Console.Out.WriteLine($"{jsonTextIn}{jsonTextOut}");
           

            //Console.ReadLine();

            if (Console.ReadKey().Key == ConsoleKey.Escape)
            {
                Environment.Exit(0);
            }
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
            
        }

        internal void PayCar()
        {
            //car = new Car();
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

        private string TimeElapsedToString(TimeSpan ts)
        {
            return String.Format("{0:00}:{1:00}:{2:00}.{3:00}",
            ts.Hours, ts.Minutes, ts.Seconds,
            ts.Milliseconds / 10);
        }

        public (string jsonCar, string elapsedTime) CarSerialize(bool flagSerialize)
        {           
            stopwatch = stopwatch ??= new Stopwatch();

            stopwatch.Start();
            
            jsonCar = JsonSerializer.Serialize<Car>(car, GetOptions());

            if(flagSerialize)
            {
                File.WriteAllText("Car.csv", jsonCar);
            }
            

            stopwatch.Stop();
            
            TimeSpan ts = stopwatch.Elapsed;

            string elapsedTime = TimeElapsedToString(ts);           

            return (jsonCar, elapsedTime);         
        }


        

        //public (string jsonCar, string elapsedTime) CarSerialize(int k)
        //{
        //    stopwatch = stopwatch ??= new Stopwatch();

        //    stopwatch.Start();

        //    //using (FileStream fileStream = new FileStream("Car.csv", FileMode.Create, FileAccess.Write))
        //    //{
        //    //    //jsonCar = JsonSerializer.Serialize<Car>(car, GetOptions());
        //    //    await JsonSerializer.SerializeAsync<Car>(fileStream, car);
        //    //    //await JsonSerializer.SerializeAsync<Person>(fs, tom);
        //    //    //File.WriteAllText("Car.csv", jsonCar);
        //    //}

        //    GGG();

        //    stopwatch.Stop();


        //    TimeSpan ts = stopwatch.Elapsed;

        //    string elapsedTime = TimeElapsedToString(ts);

        //    return (jsonCar, elapsedTime);
        //}

        //private async Task GGG()
        //{
        //    using (FileStream fileStream = new FileStream("Car.csv", FileMode.Create, FileAccess.Write))
        //    {
        //        //jsonCar = JsonSerializer.Serialize<Car>(car, GetOptions());
        //        await JsonSerializer.SerializeAsync<Car>(fileStream, car);
        //        //await JsonSerializer.SerializeAsync<Person>(fs, tom);
        //        //File.WriteAllText("Car.csv", jsonCar);
        //    }
        //}



        public (string jsonCar, string elapsedTime, string total) CarDeSerialize(bool flagSerialize)
        {
            stopwatch = stopwatch ??= new Stopwatch();
            stopwatch.Start();

            if(flagSerialize)
            {
                jsonCar = File.ReadAllText("Car.csv");
            }
                     
            Car car2 = JsonSerializer.Deserialize<Car>(jsonCar, GetOptions());

            stopwatch.Stop();

            TimeSpan ts = stopwatch.Elapsed;

            string elapsedTime = TimeElapsedToString(ts);

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
        //public Dictionary<int, string> dictionary { get; set; } = default;            
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
            //dictionary = dictionary ?? new Dictionary<int, string>();
            //dictionary?.Add(rnd.Next(10000000), $"{HashcodeCar}");
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
