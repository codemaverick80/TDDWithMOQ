using Microsoft.Extensions.Logging;
using Moq;
using MOQExample.Controllers;
using MOQExample.Domain;
using MOQExample.Exceptions;
using MOQExample.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;
using Xunit;

////https://stackoverflow.com/questions/66307477/how-to-verify-iloggert-log-extension-method-has-been-called-using-moq
////https://chrissainty.com/unit-testing-ilogger-in-aspnet-core/
namespace MOQExample.Test.Controllers
{
    public class ProductControllerTest
    {
        readonly ProductController controller;
        private readonly Mock<IProductRepository> _productRepoMock = new Mock<IProductRepository>();
        private readonly Mock<ILogger> _loggerMock = new Mock<ILogger>();

        private readonly Random _random = new Random();


        public ProductControllerTest()
        {
            controller = new ProductController(_productRepoMock.Object, _loggerMock.Object);
        }

        ////naming convention UnitOfWork_SateUnderTest_ExpectedResult()
        [Fact]
        // public void GetById_WhenProductExists_ShouldReturnProduct()
        public void GetById_ShouldReturnProduct_WhenProductExists()
        {
            ////Arrange
         
            var rndProduct=CreateRandomProduct();

            _productRepoMock.Setup(p => p.Get(rndProduct.Id)).Returns(rndProduct);
            var controller = new ProductController(_productRepoMock.Object, _loggerMock.Object);

            ////Act
            var actual = controller.GetById(rndProduct.Id);

            ////Assert
           // Assert.Same(product, actual);
            Assert.Equal(rndProduct.Id, actual.Id);
            //Assert.Equal(name, actual.Name);
        }

        //[Fact]
        //public void GetById_ShouldThrowInvalidIdException_WhenProductIdIsZeroOrLess()
        //{
        //    ////Arrange
        //    var id = 12;

        //    Expression<Action<ILogger>> expression = x => x.Log(
        //        It.IsAny<LogLevel>(),
        //        It.IsAny<EventId>(),
        //        It.IsAny<It.IsAnyType>(),
        //        It.IsAny<Exception>(),
        //        (Func<It.IsAnyType, Exception, string>)It.IsAny<object>()
        //    );

        //    _loggerMock.Setup(expression).Verifiable();

        //    _productRepoMock.Setup(p => p.Get(id)).Throws(new InvalidGuidException(nameof(Product), id));

        //    var controller = new ProductController(_productRepoMock.Object, _loggerMock.Object);

        //    ////Act
        //    Assert.Throws<InvalidGuidException>(() => controller.GetById(id));

        //    ////Assert
        //    _loggerMock.Verify(expression, Times.Once);
        //}


        [Fact]
        public void GetById_ShouldThrowInvalidIdException_WhenProductIdIsZeroOrLess()
        {
            ////Arrange
            var id = 12;
            Expression<Action<ILogger>> expression = x => x.Log(
                It.IsAny<LogLevel>(),
                It.IsAny<EventId>(),
                It.IsAny<It.IsAnyType>(),
                It.IsAny<Exception>(),
                (Func<It.IsAnyType, Exception, string>)It.IsAny<object>()
            );

            _loggerMock.Setup(expression).Verifiable();
            _productRepoMock.Setup(p => p.Get(id)).Throws(new InvalidGuidException(nameof(Product), id));
            var controller = new ProductController(_productRepoMock.Object, _loggerMock.Object);

            ////Act and Assert
            var result = Assert.Throws<InvalidGuidException>(() => controller.GetById(id));
            _loggerMock.Verify(expression, Times.Once);

            ////OR
            //var result = Assert.Throws<InvalidGuidException>(() => controller.GetById(id));
            //var exception = Assert.IsType<InvalidGuidException>(result);
            //Assert.Equal(exception.Message, result.Message);
            //Assert.Equal(exception.Reason, result.Reason);
        }


        [Fact]
        public void GetById_ShouldLogException_WhenProductDosNotExists()
        {
            ////Arrange

            var product = new Product { Id = 12, Name = "Book", Price = 122 };


            var id = 12;
            Expression<Action<ILogger>> expression = x => x.Log(
                It.IsAny<LogLevel>(),
                It.IsAny<EventId>(),
                It.IsAny<It.IsAnyType>(),
                It.IsAny<Exception>(),
                (Func<It.IsAnyType, Exception, string>)It.IsAny<object>()
            );

            _loggerMock.Setup(expression).Verifiable();

            _productRepoMock.Setup(p => p.Get(id)).Throws<NotFoundException>();
            //_productRepoMock.Setup(p => p.Get(id)).Returns(product);

            var controller = new ProductController(_productRepoMock.Object, _loggerMock.Object);

            ////Act
            Assert.Throws<NotFoundException>(() => controller.GetById(12));

            ////Assert
            _loggerMock.Verify(expression, Times.Once);
        }



        //[Fact]
        //public void GetById_ShouldReturnProduct_WhenProductExists()
        //{
        //    ////Arrange
        //    var id = 12;
        //    var name = "iWatch";
        //    var product = new Product { Id = id, Name = name };

        //    var mock = new Mock<IProductRepository>();
        //    mock.Setup(p => p.Get(id)).Returns(product);

        //    var controller = new ProductController(mock.Object);

        //    ////Act
        //    var actual = controller.GetById(id);

        //    ////Assert
        //    Assert.Same(product, actual);
        //    Assert.Equal(id, actual.Id);
        //    Assert.Equal(name, actual.Name);
        //}

        //[Fact]
        //public void GetById_ShouldReturnNothing_WhenProductDoesNotExists()
        //{
        //    //Arrange           
        //    var mock = new Mock<IProductRepository>();
        //    mock.Setup(p => p.Get(It.IsAny<int>())).Returns(() => null);
        //    var controller = new ProductController(mock.Object);

        //    // Act
        //    var actualProduct = controller.GetById(22);
        //    //Assert
        //    Assert.Null(actualProduct);
        //}

        //[Fact]
        //public void GetAllProduct_ShouldReturnProductList_WhenProductExists()
        //{
        //    ////Arrange
        //    var products = new List<Product>();
        //    products.Add(new Product { Id = 1, Name = "iPhone 13 Max Pro", Price = 1200 });
        //    products.Add(new Product { Id = 2, Name = "MacBook Pro 16 M1 Max", Price = 2300 });
        //    products.Add(new Product { Id = 3, Name = "Dell XPS 17", Price = 2400 });

        //    var mock = new Mock<IProductRepository>();
        //    mock.Setup(p => p.GetAll()).Returns(products);

        //    var controller = new ProductController(mock.Object);

        //    ////Act
        //    var actual = controller.GetProducts();

        //    ////Assert

        //    Assert.Equal(3, actual.Count);

        //}

     



        [Fact]      
        public void AddProduct_ShouldThrowException_WhenProductNameIsNullorEmpty()
        {
            ////// Arrange
            var product = new Product { Id = 45, Name = "", Price = 122 };
            // _productRepoMock.Setup(p => p.AddProduct(product)).Returns(true);
            _productRepoMock.Setup(p => p.AddProduct(product)).Throws<ArgumentNullException>();
           
            var controller = new ProductController(_productRepoMock.Object, _loggerMock.Object);

            ////// Act

            ////// Assert
            Assert.Throws<ArgumentNullException>(()=>controller.AddProduct(product));
     

        }

        [Fact]
        public void AddProduct_ShouldThrowException_WhenProductIdIsLessThenZero()
        {
            ////// Arrange
            var product = new Product { Id = 0, Name = "Book", Price = 122 };
            //_productRepoMock.Setup(p => p.AddProduct(product)).Returns(true);
            _productRepoMock.Setup(p => p.AddProduct(product)).Throws<ArgumentOutOfRangeException>();
            var controller = new ProductController(_productRepoMock.Object, _loggerMock.Object);

            ////// Act

            ////// Assert
            Assert.Throws<ArgumentOutOfRangeException>(() => controller.AddProduct(product));


        }

        [Fact]
        public void AddProduct_ShouldReturnTrue_WhenProductAddedSuccessfully()
        {
            ////Arrange
            var product = new Product { Id = 4, Name = "iPad 12 Pro", Price = 1500 };

            _productRepoMock.Setup(p => p.AddProduct(product)).Returns(true);

            var controller = new ProductController(_productRepoMock.Object, _loggerMock.Object);
            ////Act
            var actual = controller.AddProduct(product);

            ////Assert
            Assert.True(actual);

        }



        private Product CreateRandomProduct()
        {
            return new()
            {
                Id = _random.Next(10),
                Name = $"Test Produc {_random.Next(20)}",
                Price= _random.Next(1000)
            };

        }



        //[Fact]
        //public void AddProduct_ShouldThrowException_WhenProductIdIsLessThenZero()
        //{
        //    ////Arrange

        //    var product = new Product { Id = 0, Name = "Book", Price = 122 };

        //    var _productRepositoryMock = new Mock<IProductRepository>();
        //    _productRepositoryMock.Setup(p => p.AddProduct(product)).Returns(true);

        //    var controller = new ProductController(_productRepositoryMock.Object);
        //    ////Act
        //    // var service = _productService.AddProduct(product);

        //    ////Assert
        //    Assert.Throws<ArgumentOutOfRangeException>(() => controller.AddProduct(product));
        //    // Assert.False(service);

        //}



        //[Theory]
        //[InlineData()]
        //public void GetById_ShouldGetProduct()
        //{
        //    ////Arrange
        //    var id = 12;
        //    var name = "iWatch";
        //    var product = new Product { Id = id, Name = name };

        //    var mock = new Mock<IProductRepository>();
        //    mock.Setup(p => p.Get(id)).Returns(product);

        //    var controller = new ProductController(mock.Object);

        //    ////Act
        //    var actual = controller.GetById(id);

        //    ////Assert
        //    Assert.Same(product, actual);
        //    Assert.Equal(id, actual.Id);
        //    Assert.Equal(name, actual.Name);
        //}



        ///*
        // *  public Product GetById(int id)
        //{
        //    return _productRepository.Get(id);
        //}
        // */






    }
}
