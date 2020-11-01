using Lab2.Interfaces;
using Lab2.Models;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace Lab2
{
    static class Program
    {
        private static IEnumerable<Client> _newClients = new List<Client>();
        private static IEnumerable<City> _newCities = new List<City>();
        private static List<Call> _newCalls = new List<Call>();


        private static List<IReadableFromString> ReadClientsAndCitiesFromFile(Type type, string fileName)
        {
            List<IReadableFromString> readables = new List<IReadableFromString>();

            using (var context = new ApplicationContext())
            {
                string[] rows = File.ReadAllLines(fileName);
                
                foreach (var row in rows)
                {
                    var obj = Activator.CreateInstance(type) as IReadableFromString;

                    obj.ReadFromStringArray(row.Split(';'));
                    readables.Add(obj);
                }

                context.AddRange(readables);
                context.SaveChanges();
            }

            return readables;
        }

        private static void ReadCallsFromFile(string fileName)
        {
            using (var context = new ApplicationContext())
            {
                string[] rows = File.ReadAllLines(fileName);

                foreach (var row in rows)
                {
                    string[] words = row.Split(';');

                    Call call = new Call();
                    call.ClientId = _newClients.ElementAt(int.Parse(words[1]) - 1).Id;
                    call.CityId = _newCities.ElementAt(int.Parse(words[2]) - 1).Id;
                    call.ConversationDuration = int.Parse(words[3]);
                    call.DateStart = DateTime.Parse(words[4]);

                    _newCalls.Add(call);
                }

                context.AddRange(_newCalls);
                context.SaveChanges();
            }
        }

        private static void DeleteData()
        {
            using (var context = new ApplicationContext())
            {
                context.Calls.RemoveRange(context.Calls);
                context.Clients.RemoveRange(context.Clients);
                context.Cities.RemoveRange(context.Cities);
                context.SaveChanges();
            }
            
        }

        public static void PrintUnionTable()
        {
            Console.WriteLine("\n\n===== Union Tables =====");
            using (var context = new ApplicationContext())
            {
                var data = context.Calls
                    .Join(context.Clients, calls => calls.ClientId, clients => clients.Id, (o, c) => new { o.CityId, ClientId = c.Id, c.Name, c.Surname })
                    .Join(context.Cities, c => c.CityId, o => o.Id, (c, o) => new { City = o.CityName, c.ClientId, c.Name, c.Surname })
                    .GroupBy(t => new { t.Name, t.Surname, t.ClientId })
                    .Where(g => g.Count() >= 2)
                    .Select(e => new { e.Key, Count = e.Count(), Cities = e.Select(e => e.City).ToList() })
                    .ToDictionary(e => e.Key, e => e.Cities);

                foreach (var el in data)
                {
                    Console.WriteLine($"'{el.Key.Name} {el.Key.Surname}' " +
                        $"City: [{string.Join(", ", el.Value)}], " +
                        $"Cities Count: {el.Value}");
                }
            }
        }

        public static void InsertDataFromFileToDB()
        {
            _newClients = Enumerable.Cast<Client>(ReadClientsAndCitiesFromFile(typeof(Client), "clients.txt"));
            _newCities = Enumerable.Cast<City>(ReadClientsAndCitiesFromFile(typeof(City), "cities.txt"));
            ReadCallsFromFile("calls.txt");
        }

        private static void ShowData()
        {
            using (var context = new ApplicationContext())
            {
                PrintTable("Clients", context.Clients.ToList());
                PrintTable("Cities", context.Cities.ToList());
                PrintTable("Calls", context.Calls.ToList());
            }
            
        }

        private static void PrintTable(string table, IEnumerable<IModel> query)
        {
            Console.WriteLine($"\n\n===== { table } =====");
            using (var context = new ApplicationContext())
            {
                foreach (var element in query)
                {
                    Console.WriteLine(element);
                }
            }
        }

        static void Main(string[] args)
        {
            Console.OutputEncoding = System.Text.Encoding.UTF8;

            DeleteData();
            InsertDataFromFileToDB();
            ShowData();
            PrintUnionTable();
            
        }
    }
}
