using System;
using System.IO;
using System.Linq;
using MySql.Data.MySqlClient;

namespace Lab1
{
    static class Program
    {
        private const string Host = "localhost";
        private const string Database = "phone_conversation";
        private const string User = "root";
        private const string Password = "";

        private readonly static string[] Tables = { "clients", "cities", "calls"};
        private static MySqlConnection _connection;

        private static void MakeConnection()
        {
            _connection = new MySqlConnection($"Database={Database};Datasource={Host};User={User};Password={Password}");
            _connection.Open();
        }

        private static void InsertData(string table)
        {
            string[] rows = File.ReadAllLines($"{table}.txt");
            foreach (var row in rows)
            {
                var command = _connection.CreateCommand();
                command.CommandText = $"INSERT INTO {table} VALUES({row.Replace(';', ',')})";
                command.ExecuteNonQuery();
            }
            
        }

        private static void DeleteData()
        {
            foreach (var table in Tables.Reverse())
            {
                var command = _connection.CreateCommand();
                command.CommandText = $"DELETE FROM {table}";
                command.ExecuteNonQuery();
            }
        }

        private static void PrintTable(string table)
        {
            var command = _connection.CreateCommand();
            command.CommandText = $"SELECT * FROM {table}";
            var reader = command.ExecuteReader();

            Console.WriteLine($"---------- Table {table} ----------");
            while (reader.Read())
            {
                object[] values = new object[reader.FieldCount];
                reader.GetValues(values);

                string str = values.Aggregate((v1, v2) => $"{v1} | {v2}").ToString();
                Console.WriteLine(str);
            }
            Console.WriteLine("\n\n");
            reader.Close();
        }

        private static void PrintDetails()
        {
            var command = _connection.CreateCommand();
            command.CommandText = $"SELECT * FROM calls ca JOIN clients cl ON ca.client_id = cl.id";
            var reader = command.ExecuteReader();

            Console.WriteLine($"---------- Details ----------");
            while (reader.Read())
            {
                object[] values = new object[reader.FieldCount];
                reader.GetValues(values);

                string str = values.Aggregate((v1, v2) => $"{v1} | {v2}").ToString();
                Console.WriteLine(str);
            }
            Console.WriteLine("\n\n");
            reader.Close();
        }

        private static void PrintCount()
        {
            var command = _connection.CreateCommand();
            command.CommandText = $"SELECT * FROM calls ca JOIN clients cl ON ca.client_id = cl.id GROUP BY ca.client_id HAVING COUNT(*) >= 2";
            var reader = command.ExecuteReader();

            int rowCount = 0;
            while (reader.Read())
            {
                ++rowCount;
            }

            Console.WriteLine($"\n\nCount of clients that have two calls in another cities: {rowCount}\n\n");
            Console.WriteLine("\n\n");
            reader.Close();
        }

        static void Main(string[] args)
        {
            try
            {
                MakeConnection();
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
            }

            DeleteData();
            InsertData("clients");
            PrintTable("clients");
            InsertData("cities");
            PrintTable("cities");
            InsertData("calls");
            PrintTable("calls");
            PrintDetails();  
            PrintCount();
        }
    }
}
