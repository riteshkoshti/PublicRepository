using DataAccessLayer.Interfaces;
using DataAccessLayer.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace ContactManagementServices.Controllers
{
    [ApiController] 
    [Route("[controller]")]
    public class ContactController : Controller
    {
        private readonly ILogger<ContactController> _logger;
        private readonly IRepository _contactAsyncRepository;

        /// <summary>
        /// Contact controller constructor
        /// </summary>
        /// <param name="logger"></param>
        public ContactController(ILogger<ContactController> logger, IRepository contactRepository)
        {
            _logger = logger;
            _contactAsyncRepository = contactRepository;
        }

        [HttpHead]
        public IActionResult CheckHealth()
        {
            return Ok();
        }

        /// <summary>
        /// Returns all the contacts in the organization
        /// </summary>
        /// <returns>All contacts</returns>
        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            try
            {
                return Ok(await _contactAsyncRepository.SelectAll<Contact>());
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.ToString());
                return BadRequest($"Failed to get response : { ex.Message}");
            }          
        }

        /// <summary>
        /// Returns contact by id in the organization
        /// </summary>
        /// <returns>contact by id</returns>
        [HttpGet("id")]
        public async Task<IActionResult> GetById(long id)
        {
            try
            {
                var item = await _contactAsyncRepository.SelectById<Contact>(id);

                if (item == null)
                {
                    return NotFound();
                }

                return Ok(item);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.ToString());
                return BadRequest($"Failed to get response : { ex.Message}");
            }          
        }

        /// <summary>
        /// Adds new contact in the organization
        /// </summary>
        /// <param name="contact"></param>
        /// <returns>all the contacts post creating a new contact</returns>
        [HttpPost]
        public async Task<IActionResult> CreateAsync([FromBody] Contact contact)
        {
            if (contact is null)
            {
                return BadRequest();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    var contacts = await _contactAsyncRepository.SelectAll<Contact>();

                    if (contacts.Any(c => c.Email == contact.Email))
                    {
                        return BadRequest($"Failed to create contact because email '{contact.Email}' already exists.");
                    }

                    await _contactAsyncRepository.CreateAsync(contact);
                }
                catch (Exception ex)
                {
                    _logger.LogError(ex.ToString());
                    return BadRequest($"Failed to get response : { ex.Message}");
                }
            }

            return await GetAll();
        }

        /// <summary>
        ///  Edits a contact in the organization
        /// </summary>
        /// <param name="contact"></param>
        /// <returns></returns>
        [HttpPut]
        public async Task<IActionResult> EditAsync([FromBody] Contact contact)
        {

            if (contact is null)
            {
                return BadRequest();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    await _contactAsyncRepository.UpdateAsync(contact);
                }
                catch (DbUpdateException ex)
                {
                    _logger.LogError(ex.ToString());
                    return BadRequest($"Failed to update contact due to unique email issue or invalid Id");
                }
                catch (Exception ex)
                {
                    _logger.LogError(ex.ToString());
                    return BadRequest($"Failed to get response : { ex.Message}");
                }
            }

            return Ok();
        }

        /// <summary>
        ///  Inactivates a contact in the organization
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpDelete("id")]
        public async Task<IActionResult> DeleteAsync(long id)
        {          
            try
            {
                var item = await _contactAsyncRepository.SelectById<Contact>(id);

                if (item == null)
                {
                    return NotFound();
                }

                // Inactivating contact
                item.Status = false;
                await _contactAsyncRepository.DeleteAsync(item);

                return NoContent();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.ToString());
                return BadRequest($"Failed to get response : { ex.Message}");
            }
        }
    }
}