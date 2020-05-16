using System;
using Xunit;
using SolarCoffee.Data;
using SolarCoffee.Services.Customer;
using Microsoft.EntityFrameworkCore;
using SolarCoffee.Data.Models;
using FluentAssertions;
using System.Linq;
using System.Collections.Generic;
using Moq;

namespace SolarCoffee.Test
{
    public class TestCustomerService
    {
        [Fact]
        public void CustomerService_GetsAllCustomers_GivenTheyExist()
        {
            // using in memory data base Microsoft.EntityFrameworkCore.InMemory
            var options = new DbContextOptionsBuilder<SolarDbContext>().UseInMemoryDatabase("gets_all").Options;

            using var context = new SolarDbContext(options);

            var sut = new CustomerService(context);

            sut.CreateCustomer(new Customer { Id = 1213 });
            sut.CreateCustomer(new Customer { Id = 1214 });
            sut.CreateCustomer(new Customer { Id = 1215 });

            var allCustomers = sut.GetAllCustomers();
            // fluent assertions
            allCustomers.Count.Should().Be(3);
        }

        [Fact]
        public void CustomerService_CreatesCustomer_GivenNewCustomerObject()
        {
            // using in memory data base Microsoft.EntityFrameworkCore.InMemory
            var options = new DbContextOptionsBuilder<SolarDbContext>().UseInMemoryDatabase("Add_writes_to_Database").Options;

            using var context = new SolarDbContext(options);

            var sut = new CustomerService(context);
            sut.CreateCustomer(new Customer { Id = 1213 });
            context.Customers.Single().Id.Should().Be(1213);
        }

        [Fact]
        public void CustomerService_DeletesCustomer_GivenCustomerId()
        {
            // using in memory data base Microsoft.EntityFrameworkCore.InMemory
            var options = new DbContextOptionsBuilder<SolarDbContext>().UseInMemoryDatabase("deletes_one").Options;

            using var context = new SolarDbContext(options);

            var sut = new CustomerService(context);
            sut.CreateCustomer(new Customer { Id = 1213 });
            sut.DeleteCustomer(1213);
            var allCustomers = sut.GetAllCustomers();
            allCustomers.Count.Should().Be(0);
        }

        [Fact]
        public void CustomerService_OrdersByLastName_WhenGetAllCustomersInvoked()
        {
            // arrange boiler plate
            var data = new List<Customer>
            {
                new Customer { Id = 123, LastName = "Xulu"  },
                new Customer { Id = 124, LastName = "Haka"  },
                new Customer { Id = 125, LastName = "Laka"  },
                new Customer { Id = 126, LastName = "Ching" }
            }.AsQueryable();

            // dependency mock moq

            // setting up the mock as a db data
            var mockSet = new Mock<DbSet<Customer>>();
            mockSet.As<IQueryable<Customer>>()
                .Setup(m => m.Provider)
                .Returns(data.Provider);

            mockSet.As<IQueryable<Customer>>()
                .Setup(m => m.Expression)
                .Returns(data.Expression);

            mockSet.As<IQueryable<Customer>>()
                .Setup(m => m.ElementType)
                .Returns(data.ElementType);

            mockSet.As<IQueryable<Customer>>()
                .Setup(m => m.GetEnumerator())
                .Returns(data.GetEnumerator());

            var mockContext = new Mock<SolarDbContext>();

            // set up the mock context
            mockContext.Setup(c => c.Customers).Returns(mockSet.Object);

            // act
            var sut = new CustomerService(mockContext.Object);
            var customers = sut.GetAllCustomers();

            // assert
            customers.Count.Should().Be(4);
            // asserts the orders that it returns data by the last name
            customers[0].Id.Should().Be(126);
            customers[1].Id.Should().Be(124);
            customers[2].Id.Should().Be(125);
            customers[3].Id.Should().Be(123);
        }

    }
}
