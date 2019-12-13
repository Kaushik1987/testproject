using AwsWebApi.Models;
using FizzWare.NBuilder;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace AwsWebApi.Core
{
    public class Utils
    {
        public static IEnumerable<Employee> GetEmployees(int lenth)
        {
            var depts = new List<string> { "HR", "Finance", "Management", "IT", "Admin", "Sefaty" };
            var gender = new List<char> { 'M', 'F' };
            var ocupation = new List<string> { "QA", "Developer", "Black Smith", "Gold Smith" };

            var daysGenerator = new RandomGenerator();
            var priceGenerator = new RandomGenerator();

            var emps = Builder<Employee>.CreateListOfSize(lenth)
           .All()
           .With(c => c.FirstName = Faker.Name.First())
           .With(c => c.LastName = Faker.Name.Last())
           .With(c => c.City = Faker.Address.City())
           .With(c => c.Zip = Faker.Address.ZipCode())
           .With(c => c.State = Faker.Address.UsState())
           .With(c => c.Street = Faker.Address.StreetAddress())
           .With(c => c.DOB = DateTime.Now.AddDays(-daysGenerator.Next(1, 100)))
           .With(c => c.Department == Pick<string>.RandomItemFrom(depts))
           .With(c => c.Gender == Pick<Char>.RandomItemFrom(gender))
           .With(c => c.Occupation == Pick<string>.RandomItemFrom(ocupation))
           .With(c => c.Salary == priceGenerator.Next(500.01M, 50000.99M))
           .Build();

            Random rnd = new Random();

            emps.Select(x =>
            {
                x.Department = depts[rnd.Next(depts.Count)];
                x.Gender = gender[rnd.Next(gender.Count)];
                x.Occupation = ocupation[rnd.Next(ocupation.Count)];
                return x;
            }).ToList();
            return emps;
        }
    }
}