using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using DealerShip.Models;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using Newtonsoft.Json;
using System.Text.Json.Serialization;
using System.Text.Json;
using System.Security.Cryptography.X509Certificates;

namespace DealerShip.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class DealerShipController : ControllerBase
    {
        private readonly DealerShipContext _context;


        public DealerShipController(DealerShipContext context)
        {
            _context = context;
            _context.Database.EnsureCreated();
        }

        [HttpGet]
        [Route("GetAllCarsWithoutLinks")]
        public async Task<ActionResult> GetAllCars()
        {
            var cars = await _context.Cars.ToArrayAsync();

            return Ok(cars);
        }


        [HttpGet]
        [Route("GetAllCarsWithLinks")]
        public async Task<ActionResult> PutLinksOnAllCars()
        {
            var cars = await _context.Cars.ToArrayAsync();
            
            foreach (var car in cars)
            {
                car.Links = new List<Link>();
                var link = new Link { Href = $"https://localhost:7174/api/DealerShip/GetCarById/{car.Id}", Rel = "self" };
                var link2 = new Link { Href = $"https://localhost:7174/api/DealerShip/UpdateCar/{car.Id}", Rel = "update" };
                var link3 = new Link { Href = $"https://localhost:7174/api/DealerShip/DeleteCar/{car.Id}", Rel = "delete" };
                
                car.Links.Add(link);
                car.Links.Add(link2);
                car.Links.Add(link3);
                _context.Entry(car).State = EntityState.Modified;
            }
            await _context.SaveChangesAsync();
            return Ok(cars);
        }

        [HttpGet]
        [Route("GetCarById/{id}")]
        public async Task<ActionResult> GetCarById(int id)
        {
            var car = await _context.Cars.FindAsync(id);
            if (car == null)
            {
                return NotFound();
            }

            if (car.Links == null)
            {
                car.Links = new List<Link>();
                var link = new Link { Href = $"https://localhost:7174/api/DealerShip/GetCarById/{car.Id}", Rel = "self" };
                var link2 = new Link { Href = $"https://localhost:7174/api/DealerShip/UpdateCar/{car.Id}", Rel = "update" };
                var link3 = new Link { Href = $"https://localhost:7174/api/DealerShip/DeleteCar/{car.Id}", Rel = "delete" };

                car.Links.Add(link);
                car.Links.Add(link2);
                car.Links.Add(link3);
                _context.Entry(car).State = EntityState.Modified;
                await _context.SaveChangesAsync();
            }
            return Ok(car);
        }

        [HttpPost]
        [Route("AddCar")]
        public async Task<ActionResult> AddCar(Car car)
        {
            _context.Cars.Add(car);
            await _context.SaveChangesAsync();
            return CreatedAtAction(nameof(GetCarById), new { id = car.Id }, car);
        }

        [HttpPut]
        [Route("UpdateCar/{id}")]
        public async Task<ActionResult> UpdateCar(int id, Car car)
        {
            if (id != car.Id)
            {
                return BadRequest();
            }
            else if (car.IsSold)
            {
                return BadRequest("Car is already sold, cannot modify");
            }
            _context.Entry(car).State = EntityState.Modified;
            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!_context.Cars.Any(car => car.Id == id))
                {
                    return NotFound();
                }

                else
                {
                    throw;
                }
            }
            return Ok(car);
        }
        [HttpPost]
        [Route("DeleteCar/{id}")]
        public async Task<ActionResult> DeleteCar(int id)
        {
            var car = await _context.Cars.FindAsync(id);
            if (car == null)
            {
                return NotFound();
            }
            else if (car.IsSold)
            {
                return BadRequest("Car is already sold, cannot delete");
            }
            _context.Cars.Remove(car);
            await _context.SaveChangesAsync();
            return Ok(car);
        }

        [HttpDelete]
        [Route("DeleteAllCars")]
        public async Task<ActionResult> DeleteAllCars()
        {
            var cars = _context.Cars.Where(c => c.IsSold == false);
            _context.Cars.RemoveRange(cars);
            await _context.SaveChangesAsync();
            return Ok("Deleted all cars,except sold ones");
        }



        [HttpGet]
        [Route("GetAllCustomers")]
        public async Task<ActionResult> GetAllCustomer()
        {
            var customers = await _context.Customers.ToArrayAsync();
            return Ok(customers);
        }

        [HttpGet]
        [Route("GetCustomerById")]
        public async Task<ActionResult> GetCustomerById(int id)
        {
            var customer = await _context.Customers.FindAsync(id);
            return Ok(customer);
        }

        [HttpPut]
        [Route("CustomerIDBoughtCarId")]
        public async Task<ActionResult> CustomerIDBoughtCarID(int customerId, int carId)
        {
            if (!_context.Customers.Any(c => c.Id == customerId) || !_context.Cars.Any(c => c.Id == carId))
            {
                return BadRequest();
            }

            var customer = await _context.Customers.FindAsync(customerId);
            var car = await _context.Cars.FindAsync(carId);

            if (car.IsSold)
            {
                return BadRequest("Car is already sold.");
            }
            if (customer.BoughtCars == null)
            {
                customer.BoughtCars = new List<Car>();
            }

            car.IsSold = true;
            car.CustomerId = customerId;
            customer.BoughtCars.Add(car);

            await _context.SaveChangesAsync();

            return Ok(customer);
        }

        [HttpGet]
        [Route("ListAllCustomerWithBoughtCars")]
        public async Task<ActionResult> ListAllCustomersWithCars()
        {


            var customers = await _context.Customers.Where(c => c.BoughtCars.Count != 0).ToArrayAsync();
            var cars = await _context.Cars.Where(c => c.IsSold == true && c.CustomerId != null).ToArrayAsync();
            foreach (var car in cars)
            {
                if (car.Links == null)
                {
                    car.Links = new List<Link>();

                    var link = new Link { Href = $"https://localhost:7174/api/DealerShip/GetCarById/{car.Id}", Rel = "self" };

                    car.Links.Add(link);
                    _context.Entry(car).State = EntityState.Modified;
                    await _context.SaveChangesAsync();
                }
            }
            return Ok(customers);
        }

        [HttpGet]
        [Route("GetCustomersWithCarsThatAreWorthMoreThan/{price}")]
        public async Task<ActionResult> ListAllCustomersWithCars(double price)
        {
            var customers = await _context.Customers.Where(c => c.BoughtCars.Any(c => c.Price >= price)).ToArrayAsync();
            var cars = await _context.Cars.Where(c => c.IsSold == true && c.CustomerId != null).ToArrayAsync();

            foreach (var car in cars)
            {
                if (car.Links == null)
                {
                    car.Links = new List<Link>();

                    var link = new Link { Href = $"https://localhost:7174/api/DealerShip/GetCarById/{car.Id}", Rel = "self" };

                    car.Links.Add(link);
                    _context.Entry(car).State = EntityState.Modified;
                    await _context.SaveChangesAsync();
                }
            }
            return Ok(customers);
        }

        ///functii care nu au ajuns in "prod" pentru ca sunt prea multe si relaticv useless
        /*
       [HttpPost]
       [Route("DeleteAllCarsByMake")]
       public async Task<ActionResult> DeleteAllCarsByMake(string brand)
       {
           var cars = await _context.Cars.Where(c => c.Brand == brand&&c.IsSold==false).ToArrayAsync();
           _context.Cars.RemoveRange(cars);
           await _context.SaveChangesAsync();
           return Ok(cars);
       }
       [HttpPost]
       [Route("DeleteAllCarsByModel")]
       public async Task<ActionResult> DeleteAllCarsByModel(string model)
       {
           var cars = await _context.Cars.Where(c => c.Model == model && c.IsSold == false).ToArrayAsync();
           _context.Cars.RemoveRange(cars);
           await _context.SaveChangesAsync();
           return Ok(cars);
       }

       [HttpPost]
       [Route("DeleteAllCarsByYear")]
       public async Task<ActionResult> DeleteAllCarsByYear(int year)
       {
           var cars = await _context.Cars.Where(c => c.Year == year && c.IsSold == false).ToArrayAsync();
           _context.Cars.RemoveRange(cars);
           await _context.SaveChangesAsync();
           return Ok(cars);
       }
       [HttpPost]
       [Route("DeleteAllCarsByPrice")]
       public async Task<ActionResult> DeleteAllCarsByPrice(double price)
       {
           var cars = await _context.Cars.Where(c => c.Price == price && c.IsSold == false).ToArrayAsync();
           _context.Cars.RemoveRange(cars);
           await _context.SaveChangesAsync();
           return Ok(cars);
       }

       [HttpPost]
       [Route("DeleteAllCarsByColor")]
       public async Task<ActionResult> DeleteAllCarsByColor(string color)
       {
           var cars = await _context.Cars.Where(c => c.Color == color && c.IsSold == false).ToArrayAsync();
           _context.Cars.RemoveRange(cars);
           await _context.SaveChangesAsync();
           return Ok(cars);
       }
       */


    }
}
