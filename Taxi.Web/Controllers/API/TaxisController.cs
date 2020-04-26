using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Taxi.Web.Data;
using Taxi.Web.Data.Entities;
using Taxi.Web.Helpers;

namespace Taxi.Web.Controllers.API
{
    [Route("api/[controller]")]
    [ApiController]
    public class TaxisController : ControllerBase
    {
        private readonly DataContext _context;
        private readonly IConverterHelper _converterHelper;

        public TaxisController(DataContext context, 
            IConverterHelper converterHelper)
        {
            _context = context;
            _converterHelper = converterHelper;
        }

        // GET: api/Taxis
        [HttpGet]
        public async Task<ActionResult<IEnumerable<TaxiEntity>>> GetTaxis()
        {
            return await _context.Taxis.ToListAsync();
        }

        [HttpGet("{plaque}")]
        public async Task<ActionResult> GetTaxiEntity([FromRoute] string plaque)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            TaxiEntity taxiEntity = await _context.Taxis
                .Include(t => t.User)
                .Include(t => t.Trips)
                      .ThenInclude(t => t.TripDetails)
                .Include(t => t.Trips)
                    .ThenInclude(t => t.User)
                .FirstOrDefaultAsync(t => t.Plaque == plaque);

            if (taxiEntity == null)
            {
                taxiEntity = new TaxiEntity { Plaque = plaque.ToUpper() };
                _context.Taxis.Add(taxiEntity);
                await _context.SaveChangesAsync();
            }

            return Ok(_converterHelper.ToTaxiResponse(taxiEntity));
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> PutTaxiEntity(int id, TaxiEntity taxiEntity)
        {
            if (id != taxiEntity.Id)
            {
                return BadRequest();
            }

            _context.Entry(taxiEntity).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!TaxiEntityExists(id))
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

        // POST: api/Taxis
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for
        // more details see https://aka.ms/RazorPagesCRUD.
        [HttpPost]
        public async Task<ActionResult<TaxiEntity>> PostTaxiEntity(TaxiEntity taxiEntity)
        {
            _context.Taxis.Add(taxiEntity);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetTaxiEntity", new { id = taxiEntity.Id }, taxiEntity);
        }

        // DELETE: api/Taxis/5
        [HttpDelete("{id}")]
        public async Task<ActionResult<TaxiEntity>> DeleteTaxiEntity(int id)
        {
            var taxiEntity = await _context.Taxis.FindAsync(id);
            if (taxiEntity == null)
            {
                return NotFound();
            }

            _context.Taxis.Remove(taxiEntity);
            await _context.SaveChangesAsync();

            return taxiEntity;
        }

        private bool TaxiEntityExists(int id)
        {
            return _context.Taxis.Any(e => e.Id == id);
        }
    }
}
