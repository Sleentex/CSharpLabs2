using Lab2;
using Lab2.Interfaces;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace Lab3
{
    public class HtmlWriter
    {
        public static async Task ShowPage(HttpContext httpContext, string pageName, string[] columns, List<string[]> rows)
        {
            await httpContext.Response.WriteAsync(File.ReadAllText(@".\wwwroot\templates\header.html"));

            string tableData =
                $"<h1>{pageName}</h1>" +
                "<table class='table table-striped'>" +
                "<tr>";

            foreach (var column in columns)
            {
                tableData += $"<th>{column}</th>";
            }

            await httpContext.Response.WriteAsync(tableData + "</tr>");

            foreach (var row in rows)
            {
                string htmlRow = "<tr>";

                foreach (var cell in row)
                {
                    htmlRow += $"<td>{cell}</td>";
                }

                await httpContext.Response.WriteAsync(htmlRow + "</tr>"); 
            }

            await httpContext.Response.WriteAsync("</table>");
            await httpContext.Response.WriteAsync(File.ReadAllText(@".\wwwroot\templates\footer.html"));
        }


        public static async Task WriteClientsPage(HttpContext httpContext)
        {
            string pageName = "Список клієнтів";
            List<string[]> clientsList = new List<string[]>();
            string[] columns = new [] { "Прізвище", "Ім'я", "По батькові", "Адреса", "Номер телефону" };

            using (var context = new ApplicationContext())
            {
                foreach (var el in context.Clients.ToList())
                {
                    string[] client = new string[] { el.Surname, el.Name, el.MiddleName, el.Address, el.PhoneNumber };

                    clientsList.Add(client);
                }
            }

            await ShowPage(httpContext, pageName, columns, clientsList);
        }

        public static async Task WriteCitiesPage(HttpContext httpContext)
        {
            string pageName = "Список міст";
            List<string[]> citiesList = new List<string[]>();
            string[] columns = new[] { "Назва", "Телефонний код", "Тариф" };

            using (var context = new ApplicationContext())
            {
                foreach (var el in context.Cities.ToList())
                {
                    string[] client = new string[] { el.CityName, el.PhoneCode, el.Tariff.ToString() };

                    citiesList.Add(client);
                }
            }

            await ShowPage(httpContext, pageName, columns, citiesList);
        }


        public static async Task WriteMainPage(HttpContext httpContext)
        {
            string pageName = "Головна сторінка";
            List<string[]> unionList = new List<string[]>();
            string[] columns = new[] { "Прізвище", "Ім'я", "Міста" };

            using (var context = new ApplicationContext())
            {
                var query = context.Calls
                    .Join(context.Clients, calls => calls.ClientId, clients => clients.Id, (o, c) => new { CityId = o.CityId, Name = c.Name, Surname = c.Surname })
                    .Join(context.Cities, c => c.CityId, o => o.Id, (c, o) => new { City = o.CityName, Name = c.Name, Surname = c.Surname }).ToList()
                    .GroupBy(table => new { table.Surname, table.Name })
                    .Where(g => g.Count() >= 2);


                foreach (var el in query)
                {
                    string[] cities = el.Select(q => q.City).ToArray();
                    string[] union = new string[] { el.Key.Surname, el.Key.Name, string.Join(", ", cities) };

                    unionList.Add(union);
                }

                await ShowPage(httpContext, pageName, columns, unionList);
            }
        }
    }
}
