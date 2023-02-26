using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using DealerShip.Models;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using Newtonsoft.Json;
using System.Text.Json.Serialization;
using System.Text.Json;

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
        [Route("GetAllCars")]
        public async Task<ActionResult> GetAllCars()
        {
            var cars = await _context.Cars.ToArrayAsync();
            
            return Ok(cars);
        }


        [HttpGet]
        [Route("AllLinks")]
        public async Task<ActionResult> PutLinksOnAllCars()
        {
            var cars = await _context.Cars.ToArrayAsync();
            var links = new List<Link>();
            foreach (var car in cars)
            {
                var link = new Link {Href = $"https://localhost:44394/api/DealerShip/GetCarById/{car.Id}" + car.Id, Rel = "self" };
                car.Links.Add(link);

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
        public async Task<ActionResult> UpdateCar(int id,Car car)
        {
            if(id != car.Id)
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
                if(!_context.Cars.Any(car=> car.Id==id))
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
                return BadRequest("Car is already sold, cannot modify");
            }
            _context.Cars.Remove(car);
            await _context.SaveChangesAsync();
            return Ok(car);
        }

        [HttpDelete]
        [Route("DeleteAllCars")]
        public async Task<ActionResult> DeleteAllCars()
        {
            var cars = _context.Cars.Where(c=>c.IsSold==false);
            _context.Cars.RemoveRange(cars);
            await _context.SaveChangesAsync();
            return Ok("Deleted all cars,except sold ones");
        }
        [HttpPost]
        [Route("DeleteAllCarsByMake")]
        public async Task<ActionResult> DeleteAllCarsByMake(string brand)
        {
            var cars = await _context.Cars.Where(c => c.Brand == brand).ToArrayAsync();
            
            _context.Cars.RemoveRange(cars);
            await _context.SaveChangesAsync();
            return Ok(cars);
        }
        [HttpPost]
        [Route("DeleteAllCarsByModel")]
        public async Task<ActionResult> DeleteAllCarsByModel(string model)
        {
            var cars = await _context.Cars.Where(c => c.Model == model).ToArrayAsync();
            _context.Cars.RemoveRange(cars);
            await _context.SaveChangesAsync();
            return Ok(cars);
        }

        [HttpPost]
        [Route("DeleteAllCarsByYear")]
        public async Task<ActionResult> DeleteAllCarsByYear(int year)
        {
            var cars = await _context.Cars.Where(c => c.Year == year).ToArrayAsync();
            _context.Cars.RemoveRange(cars);
            await _context.SaveChangesAsync();
            return Ok(cars);
        }
        [HttpPost]
        [Route("DeleteAllCarsByPrice")]
        public async Task<ActionResult> DeleteAllCarsByPrice(double price)
        {
            var cars = await _context.Cars.Where(c => c.Price == price).ToArrayAsync();
            _context.Cars.RemoveRange(cars);
            await _context.SaveChangesAsync();
            return Ok(cars);
        }

        [HttpPost]
        [Route("DeleteAllCarsByColor")]
        public async Task<ActionResult> DeleteAllCarsByColor(string color)
        {
            var cars = await _context.Cars.Where(c => c.Color == color).ToArrayAsync();
            _context.Cars.RemoveRange(cars);
            await _context.SaveChangesAsync();
            return Ok(cars);
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
            if(!_context.Customers.Any(c=>c.Id==customerId) || !_context.Cars.Any(c => c.Id == carId))
            {
                return BadRequest();
            }
            var customer = await _context.Customers.FindAsync(customerId);
            var car= await _context.Cars.FindAsync(carId);
            if (car.IsSold)
            {
                return BadRequest("car is already sold");
            }

            customer.BoughtCars.Add(car);
            car.IsSold= true;
            car.CustomerId= customer.Id;
            ;
            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {

                return BadRequest();
            }
            return Ok(customer);
        }

        [HttpGet]
        [Route("ListAllCustomerWithBoughtCars")]
        public async Task<ActionResult> ListAllCustomersWithCars()
        {

            
            var customers = await _context.Customers.Where(c => c.BoughtCars != null).ToArrayAsync();
            var cars = await _context.Cars.Where(c => c.IsSold == true && c.CustomerId != null).ToArrayAsync();
            return Ok(customers);

            
            
            
        }
    }
}
