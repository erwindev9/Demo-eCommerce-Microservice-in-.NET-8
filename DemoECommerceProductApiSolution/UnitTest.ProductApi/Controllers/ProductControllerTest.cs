using FakeItEasy;
using FluentAssertions;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using ProductApi.Application.DTOs;
using ProductApi.Application.Interfaces;
using ProductApi.Domain.Entities;
using ProductApi.Presentation.Controllers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace UnitTest.ProductApi.Controllers
{
    public class ProductControllerTest
    {
        private readonly IProduct productInterface;
        private readonly ProductsController productsController;

        public ProductControllerTest()
        {
            //setup dependencies
            productInterface = A.Fake<IProduct>();
            //setup system under test - sut
            productsController = new ProductsController(productInterface);
        }

        //get all products
        [Fact]
        public async Task GetProduct_WhenProductsExist_ReturnsOkResultWithProducts()
        {
            // Arrange
            var products = new List<Product>
            {
                new() { Id = 1, Name = "Product1", Quantity = 10, Price = 100.7m },
                new()  { Id = 2, Name = "Product2", Quantity = 10, Price = 100.7m }
            };

            A.CallTo(() => productInterface.GetAllAsync()).Returns(products);
            // Act
            var result = await productsController.GetProducts();
            // Assert
            var okResult = result.Result as OkObjectResult; 
            okResult.Should().NotBeNull();
            okResult!.StatusCode.Should().Be(StatusCodes.Status200OK);

            var returnedProducts = okResult.Value as IEnumerable<ProductDTO>;
            returnedProducts.Should().NotBeNull();
            returnedProducts!.Should().HaveCount(2);
            returnedProducts!.First().Id.Should().Be(1);
            returnedProducts!.Last().Id.Should().Be(2);
        }

        [Fact]
        public async Task GetProduct_WhenNoProductsExist_ReturnsNotFoundResponse()
        {
            // Arrange
            var products = new List<Product>(); 

            // Setup fake response for all getallasync ;
            A.CallTo(() => productInterface.GetAllAsync()).Returns(products);
            // Act
            var result = await productsController.GetProducts();
            // Assert
            var notFoundResult = result.Result as NotFoundObjectResult;
            notFoundResult.Should().NotBeNull();
            notFoundResult!.StatusCode.Should().Be(StatusCodes.Status404NotFound);
            notFoundResult.Value.Should().Be("No products detected in the database");

            var message = notFoundResult.Value as string;
            message.Should().Be("No products detected in the database");
        }
    }
}
