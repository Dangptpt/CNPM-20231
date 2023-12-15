﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Project.Models;
using Project.Models.Models;

namespace Project.Controllers.VehicleController
{
    [Route("api/vehicle")]
    [ApiController]
    [Authorize(Roles = "user")]
    public class VehiclesController : ControllerBase
    {
        private readonly ProjectContext _context;

        public VehiclesController(ProjectContext context)
        {
            _context = context;
        }

        // GET: api/vehicle/all
        [HttpGet("all")]
        public async Task<ActionResult<IEnumerable<object>>> GetVehicles()
        {
            if (_context.Vehicles == null)
            {
                return NotFound();
            }
            var all = await _context.Vehicles.ToListAsync();
            var list= new List<object>();
            foreach (var vehicle in all)
            {
                var user = await _context.People.FirstOrDefaultAsync(u => u.PersonId == vehicle.PersonId);
                var obj = new
                {
                    vehicle = vehicle,
                    name = user?.Name,
                };
                list.Add(obj);
            }
            return list;
        }


        // GET: api/vehicle?licenseplate=&ownerName=&category=
        [HttpGet]
        public async Task<ActionResult<IEnumerable<object>>> GetVehicle(string? licenseplate, string? ownerName, string? category)
        {
            if (_context.Vehicles == null)
            {
                return NotFound();
            }

            licenseplate = licenseplate ?? string.Empty;
            ownerName = ownerName ?? string.Empty;
            category = category ?? string.Empty;

            var vehicles = await _context.Vehicles
                                .Include(v => v.Person)
                                .Where(p => p.LicensePlate.Contains(licenseplate)
                                            && p.Category.Contains(category)
                                            && p.Person.Name.Contains(ownerName))
                                .ToListAsync();
            var list = new List<object>();
            foreach (var vehicle in vehicles)
            {
                var user = await _context.People.FirstOrDefaultAsync(u => u.PersonId == vehicle.PersonId);
                var obj = new
                {
                    vehicle = vehicle,
                    name = user?.Name,
                };
                list.Add(obj);
            }
            return list;

        }
        [HttpGet("id")]
        public async Task<ActionResult<object>> GetVehicleById(Guid id)
        {
            if (_context.Vehicles == null)
            {
                return NotFound();
            }
            
            var vehicles = await _context.Vehicles.FirstOrDefaultAsync(vehicles => vehicles.VehicleId == id);
         
                var user = await _context.People.FirstOrDefaultAsync(u => u.PersonId == vehicles.PersonId);
                var obj = new
                {
                    vehicle = vehicles,
                    name = user?.Name,
                };
                
            
            return obj;

        }

        // PUT: api/vehicle/[:id]
        [HttpPut("{id}")]
        public async Task<IActionResult> PutVehicle(Guid id, Vehicle vehicle)
        {
            if (id != vehicle.VehicleId)
            {
                return BadRequest();
            }

            _context.Entry(vehicle).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!VehicleExists(id))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            return NoContent();
        }


        // POST: api/vehicle
        [HttpPost]
        public async Task<ActionResult<Vehicle>> PostVehicle(Vehicle vehicle)
        {
            if (_context.Vehicles == null)
            {
                return Problem("Entity set 'ProjectContext.Vehicles'  is null.");
            }
            vehicle.VehicleId = Guid.NewGuid();
            _context.Vehicles.Add(vehicle);
            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateException)
            {
                if (VehicleExists(vehicle.VehicleId))
                {
                    return Conflict();
                }
                else
                {
                    throw;
                }
            }

            return CreatedAtAction("GetVehicle", new { id = vehicle.VehicleId }, vehicle);
        }


        // DELETE: api/vehicle/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteVehicle(Guid id)
        {
            if (_context.Vehicles == null)
            {
                return NotFound();
            }
            var vehicle = await _context.Vehicles.FindAsync(id);
            if (vehicle == null)
            {
                return NotFound();
            }

            _context.Vehicles.Remove(vehicle);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        private bool VehicleExists(Guid id)
        {
            return (_context.Vehicles?.Any(e => e.VehicleId == id)).GetValueOrDefault();
        }
    }
}
