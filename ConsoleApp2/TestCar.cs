using System;
using System.Diagnostics;
using System.IO;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace ConsoleApp11
{
    public class TestCar
    {
        public Car car;
        Stopwatch stopwatchIn;
        Stopwatch stopwatchOut;
        string jsonCar;

        public TestCar()
        {          
            car = new Car();          
        }

        internal void PayCar()
        {           
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
            stopwatchIn = stopwatchIn ??= new Stopwatch();          
            
            stopwatchIn.Start();            
            
            jsonCar = JsonSerializer.Serialize<Car>(car, GetOptions());

            if(flagSerialize)
            {
                File.WriteAllText("Car.csv", jsonCar);
            }           

            stopwatchIn.Stop();
            
            string elapsedTime = TimeElapsedToString(stopwatchIn.Elapsed);


            return (jsonCar, elapsedTime);         
        }

        public (string jsonCar, string elapsedTime, string total) CarDeSerialize(bool flagSerialize)
        {
            stopwatchOut = stopwatchOut ??= new Stopwatch();
           
            stopwatchOut.Start();

            if (flagSerialize)
            {
                jsonCar = File.ReadAllText("Car.csv");
            }
                     
            Car car2 = JsonSerializer.Deserialize<Car>(jsonCar, GetOptions());

            stopwatchOut.Stop();         

            string elapsedTime = TimeElapsedToString(stopwatchOut.Elapsed);          

            string total = car.ToString();

            return (jsonCar, elapsedTime, total);
        }

    }
}
