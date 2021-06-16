using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Globalization;
using System.Threading.Tasks;

namespace ConsoleApp11
{
    class Program
    {
        static void Main(string[] args)
        {
            TestCar testCar = new TestCar();
            int countCar;

            while (true)
            {                
                Console.Write("Заказать авто?\nУкажите количество - ");                             
                
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
            bool flagBoxing = false;
            string flagText = "в строку";

            Console.WriteLine($"Как упаковать заказ в строку {CarBoxing.S} или файл {CarBoxing.F}");

            char flag = char.ToUpper(Console.ReadKey(true).KeyChar, CultureInfo.CreateSpecificCulture("en-US"));           

            if (flag == (char)ConsoleKey.F)
            {
                flagBoxing = true;
                flagText = "в файл";
            }

            for (int i = 0; i < countCar; i++)
            {
                testCar.PayCar();
            
                var (jsonText, elapsedTime) = testCar.CarSerialize(flagBoxing);
                jsonTextIn = $"{jsonText}\n\nВремя процесса json-сериализации {flagText} \t{elapsedTime}\n\n";
                
              
                var (jsonText2, elapsedTime2, total) = testCar.CarDeSerialize(flagBoxing);
                jsonTextOut = $"{jsonText2}\n\nВремя процесса json-десериализации\t{elapsedTime2}\n\n{total}\n\n";               

            }           

            Console.Out.WriteLine($"{jsonTextIn}{jsonTextOut}\n");

            Console.WriteLine($"Нажмите клавишу {ConsoleKey.Escape} для выхода из программы\nили {ConsoleKey.Enter} для продолжения.\n");

            if (Console.ReadKey().Key == ConsoleKey.Escape)
            {
                Environment.Exit(0);
            }
        }
    }  
}
