using System;
using System.IO;
using System.Linq;
using Newtonsoft.Json.Linq;
using System.Collections.Generic;
using NuGet.Versioning;

namespace DotNetOutdated
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var parser = new ProjectParser();
            var checker = new OutdateChecker();
            var dependencies = parser.GetAllDependencies("./project.json");
            var result = checker.Run(dependencies);

            if (result.Outdated.Count() > 0)
            {
                Console.ForegroundColor = ConsoleColor.Red;
                Console.WriteLine("Oh no! You have outdate dependencies.");
                foreach (var dependency in result.Outdated)
                {
                    string message = $"- {dependency.Name} is currently {dependency.CurrentVersion}, but stable version is {dependency.TargetVersion}";
                    Console.WriteLine(message);
                }
            }
            else 
            {
                Console.ForegroundColor = ConsoleColor.Green;
                Console.WriteLine("Good Job! Your project's dependencies are all up to date!");
            }

            Console.ResetColor();
        }
    }
}