using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;

namespace DealerShip.Models
{
    public static class ModelBuilderExtension
    {
        public static void Seed(this ModelBuilder modelBuilder)
        {
            var cars = new List<Car>
{
    new Car { Id = 1, Brand = "Ford", Model = "Mustang", Year = 1969, Color = "red", Price = 50000, IsSold = true, CustomerId=1},
new Car { Id = 2, Brand = "Chevrolet", Model = "Camaro", Year = 1970, Color = "blue", Price = 55000, IsSold = false },
new Car { Id = 3, Brand = "Dodge", Model = "Challenger", Year = 1971, Color = "green", Price = 60000, IsSold = false },
new Car { Id = 4, Brand = "Plymouth", Model = "Barracuda", Year = 1972, Color = "yellow", Price = 65000, IsSold = false },
new Car { Id = 5, Brand = "Pontiac", Model = "GTO", Year = 1968, Color = "black", Price = 70000, IsSold = false },
new Car { Id = 6, Brand = "BMW", Model = "M3", Year = 2018, Color = "gray", Price = 80000, IsSold = false },
new Car { Id = 7, Brand = "Mercedes-Benz", Model = "AMG GT", Year = 2019, Color = "white", Price = 90000, IsSold = false },
new Car { Id = 8, Brand = "Audi", Model = "R8", Year = 2020, Color = "red", Price = 100000, IsSold = false },
new Car { Id = 9, Brand = "Tesla", Model = "Model S", Year = 2021, Color = "blue", Price = 110000, IsSold = false },
new Car { Id = 10, Brand = "Ferrari", Model = "458 Italia", Year = 2014, Color = "yellow", Price = 120000, IsSold = false },
new Car { Id = 11, Brand = "Lamborghini", Model = "Aventador", Year = 2015, Color = "orange", Price = 130000, IsSold = false },
new Car { Id = 12, Brand = "McLaren", Model = "720S", Year = 2019, Color = "green", Price = 140000, IsSold = false },
new Car { Id = 13, Brand = "Porsche", Model = "911", Year = 2022, Color = "black", Price = 150000, IsSold = false },
new Car { Id = 14, Brand = "Alfa Romeo", Model = "Giulia Quadrifoglio", Year = 2019, Color = "red", Price = 160000, IsSold = false },
new Car { Id = 15, Brand = "Jaguar", Model = "F-Type", Year = 2021, Color = "blue", Price = 170000, IsSold = false },
new Car { Id = 16, Brand = "Lotus", Model = "Evora GT", Year = 2020, Color = "yellow", Price = 180000, IsSold = false },
new Car { Id = 17, Brand = "Maserati", Model = "GranTurismo", Year = 2018, Color = "white", Price = 190000, IsSold = false },
new Car { Id = 18, Brand = "Bentley", Model = "Continental GT", Year = 2017, Color = "gray", Price = 200000, IsSold = false },
new Car { Id = 19, Brand = "Rolls-Royce", Model = "Ghost", Year = 2022, Color = "black", Price = 210000, IsSold = false },
new Car { Id = 20, Brand = "Bugatti", Model = "Chiron", Year = 2021, Color = "orange", Price = 220000, IsSold = false }
};
            
            modelBuilder.Entity<Car>().HasData(cars);

            modelBuilder.Entity<Customer>().HasData(
                 new Customer { Id = 1, Name = "Alex", BirthDate = "2001-04-30" },
    new Customer { Id = 2, Name = "Beth", BirthDate = "1995-11-23" },
    new Customer { Id = 3, Name = "Charlie", BirthDate = "1988-06-15" },
    new Customer { Id = 4, Name = "David", BirthDate = "1973-09-01" },
    new Customer { Id = 5, Name = "Ella", BirthDate = "1998-12-12" },
    new Customer { Id = 6, Name = "Frank", BirthDate = "1984-02-17" },
    new Customer { Id = 7, Name = "Gina", BirthDate = "1990-08-08" },
    new Customer { Id = 8, Name = "Henry", BirthDate = "1978-05-04" },
    new Customer { Id = 9, Name = "Isabella", BirthDate = "2005-01-21" },
    new Customer { Id = 10, Name = "Jack", BirthDate = "1992-07-19" });

            /*
            modelBuilder.Entity<Link>().HasData(
                new Link { CarId = 1, Href="api/GetCarById/1",Rel="Find this car" },
                new Link { CarId=1,Href="api/UpdateCarById/1",Rel="Update this car"},
                new Link { CarId = 1, Href = "api/DeleteCarById/1", Rel = "Delete this car" },
                new Link { CarId=1,Href="api/CustomerIdBoughtCarId",Rel="buy this car"},


                new Link { CarId = 2, Href = "api/GetCarById/2", Rel = "Find this car" },
                new Link { CarId = 2, Href = "api/UpdateCarById/2", Rel = "Update this car" },
                new Link { CarId = 2, Href = "api/DeleteCarById/2", Rel = "Delete this car" },
                new Link { CarId = 2, Href = "api/CustomerIdBoughtCarId", Rel = "buy this car" }
                );
            */
        }
    }
}
