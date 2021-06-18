using ContactManagementServices.Controllers;
using DataAccessLayer.DbContexts;
using DataAccessLayer.Interfaces;
using DataAccessLayer.Models;
using DataAccessLayer.Repositories;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace UnitTests
{
    [TestClass]
    public class ContactManagementServicesTests
    {
        private readonly ContactController _contactController;

        /// <summary>
        /// ContactManagementServicesTests constructor
        /// </summary>
        public ContactManagementServicesTests()
        {
            ILogger<ContactController> _logger = new Mock<ILogger<ContactController>>().Object;
            string connectionString = "Server=***;Database=Organization;User Id=*****;Password=*******;Trusted_Connection=True;";
            var options = new DbContextOptionsBuilder<OrganizationContext>()
            .UseSqlServer(connectionString)
            .Options;

            OrganizationContext _organizationContext = new OrganizationContext(options);
            IRepository _contactAsyncRepository =  new Repository<OrganizationContext>(_organizationContext);

            _contactController = new ContactController(_logger, _contactAsyncRepository);
        }

        /// <summary>
        /// Check service health
        /// </summary>
        [TestMethod]
        public void TC_CheckServiceHealth()
        {
            // Arrange
            var result = _contactController.CheckHealth() as OkResult;

            // Assert
            Assert.IsNotNull(result);
            Assert.AreEqual(200, result.StatusCode);
        }

        /// <summary>
        /// Get all contacts
        /// </summary>
        [TestMethod]
        public async Task TC_GetAllContactsAsync()
        {
            // Arrange
            var result = await _contactController.GetAll() as OkObjectResult;
            var contacts = result.Value as List<Contact>; 
            
            // Assert
            Assert.IsTrue(contacts.Count > 0);
        }

        /// <summary>
        /// Get contact by id
        /// </summary>
        [TestMethod]
        public async Task TC_GetContactByIdAsync()
        {
            // Arrange
            var result = await _contactController.GetById(1) as OkObjectResult;
            var contact = result.Value as Contact;

            // Assert
            Assert.IsNotNull(contact);
            Assert.AreEqual(1, contact.Id);
        }

        /// <summary>
        /// Get contact by invalid id
        /// </summary>
        [TestMethod]
        public async Task TC_GetContactByInvalidIdAsync()
        {
            // Arrange
            var result = await _contactController.GetById(9999999999999) as NotFoundResult;
            
            // Assert
            Assert.IsNotNull(result);
            Assert.AreEqual(404, result.StatusCode);
        }

        /// <summary>
        /// Create a contact
        /// </summary>
        [TestMethod]
        public async Task TC_CreateContactAsync()
        {
            // Arrange
            Random generator = new();
            string name = "Test" + generator.Next(0, 10000000);

            var contact = new Contact()
            {
                FirstName = name,
                LastName = name,
                Email = name + "@test.com",
                Status = true
            };

            // Act
            var result = await _contactController.CreateAsync(contact) as OkObjectResult;
            var contacts = result.Value as List<Contact>;

            var newContact = contacts.Find(x => x.FirstName == contact.FirstName && x.Email == contact.Email);

            // Assert
            Assert.IsNotNull(newContact);
        }

        /// <summary>
        /// Create a contact with invalid data as per maximum length allowed
        /// </summary>
        [TestMethod]
        public async Task TC_CreateContactWithMoreThanMaxLengthDataAsync()
        {
            // Arrange
            string name = "TestNameTestNameTestNameTestNameTestNameTestNameTestNameTestNameTestNameTestNameTestNameTestName";

            var contact = new Contact()
            {
                FirstName = name,   // maxlength - 30
                LastName = name,    // maxlength - 30
                Email = name + "@test.com", // maxlength - 50
                Status = true
            };

            // Act
            var result = await _contactController.CreateAsync(contact) as BadRequestObjectResult;

            // Assert
            Assert.IsNotNull(result);
            Assert.AreEqual(400, result.StatusCode);
        }

        /// <summary>
        /// Edit Contact
        /// </summary>
        [TestMethod]
        public async Task TC_EditContactAsync()
        {
            // Arrange
            var result = await _contactController.GetById(1) as OkObjectResult;
            var contact = result.Value as Contact;

            Random generator = new Random();
            string name = contact.FirstName + generator.Next(0, 10000000);
            contact.FirstName = name;

            // Act
            _ = await _contactController.EditAsync(contact) as OkObjectResult;

            result = await _contactController.GetById(1) as OkObjectResult;
            var editContact = result.Value as Contact;

            // Assert
            Assert.IsNotNull(editContact);
            Assert.AreEqual(contact.FirstName, editContact.FirstName);
        }

        /// <summary>
        /// Inactivate a contact
        /// </summary>
        [TestMethod]
        public async Task TC_DeleteContactAsync()
        {
           // Arrange
            _ = await _contactController.DeleteAsync(1) as OkObjectResult;

            OkObjectResult result = await _contactController.GetById(1) as OkObjectResult;
            var deleteContact = result.Value as Contact;

            // Assert
            Assert.IsNotNull(deleteContact);
            Assert.IsTrue(!deleteContact.Status);
        }

        /// <summary>
        /// Inactivate a contact by invalid Id
        /// </summary>
        [TestMethod]
        public async Task TC_DeleteContactByInvalidIdAsync()
        {
            // Arrange
            var result = await _contactController.DeleteAsync(99999999999) as NotFoundResult;

            // Assert
            Assert.IsNotNull(result);
            Assert.AreEqual(404, result.StatusCode);
        }
    }
}
