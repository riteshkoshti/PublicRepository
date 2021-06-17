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
            string connectionString = "************Put Your Environment Connection String****************";
            var options = new DbContextOptionsBuilder<OrganizationContext>()
            .UseSqlServer(connectionString)
            .Options;

            OrganizationContext _organizationContext = new OrganizationContext(options);
            IRepository _contactAsyncRepository =  new Repository<OrganizationContext>(_organizationContext);

            _contactController = new ContactController(_logger, _contactAsyncRepository);
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
        /// Create a contact
        /// </summary>
        [TestMethod]
        public async Task TC_CreateContactAsync()
        {
            // Arrange
            Random generator = new Random();
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
    }
}
