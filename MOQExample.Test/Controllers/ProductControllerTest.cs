using FluentValidation;
using FluentValidation.TestHelper;
using Microsoft.Extensions.Logging;
using Moq;
using MOQCoreAPI.Controllers;
using MOQCoreAPI.Domain;
using MOQCoreAPI.Exceptions;
using MOQCoreAPI.Repositories;
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
        private readonly ProductValidator _validator =new ProductValidator();

        private readonly Random _random = new Random();


        public ProductControllerTest()
        {
            controller = new ProductController(_productRepoMock.Object, _loggerMock.Object);
        }

        ////////naming convention UnitOfWork_SateUnderTest_ExpectedResult()
        ////[Fact]
        ////// public void GetById_WhenProductExists_ShouldReturnProduct()
        ////public void GetById_ShouldReturnProduct_WhenProductExists()
        ////{
        ////    ////Arrange

        ////    var rndProduct=CreateRandomProduct();

        ////    _productRepoMock.Setup(p => p.Get(rndProduct.Id)).Returns(rndProduct);
        ////    var controller = new ProductController(_productRepoMock.Object, _loggerMock.Object);

        ////    ////Act
        ////    var actual = controller.GetById(rndProduct.Id);

        ////    ////Assert
        ////   // Assert.Same(product, actual);
        ////    Assert.Equal(rndProduct.Id, actual.Id);
        ////    //Assert.Equal(name, actual.Name);
        ////}



        ////[Fact]
        ////public void GetById_ShouldThrowInvalidIdException_WhenProductIdIsZeroOrLess()
        ////{
        ////    ////Arrange
        ////    var id = 12;
        ////    Expression<Action<ILogger>> expression = x => x.Log(
        ////        It.IsAny<LogLevel>(),
        ////        It.IsAny<EventId>(),
        ////        It.IsAny<It.IsAnyType>(),
        ////        It.IsAny<Exception>(),
        ////        (Func<It.IsAnyType, Exception, string>)It.IsAny<object>()
        ////    );

        ////    _loggerMock.Setup(expression).Verifiable();
        ////    _productRepoMock.Setup(p => p.Get(id)).Throws(new InvalidGuidException(nameof(Product), id));
        ////    var controller = new ProductController(_productRepoMock.Object, _loggerMock.Object);

        ////    ////Act and Assert
        ////    var result = Assert.Throws<InvalidGuidException>(() => controller.GetById(id));
        ////    _loggerMock.Verify(expression, Times.Once);

        ////    ////OR
        ////    //var result = Assert.Throws<InvalidGuidException>(() => controller.GetById(id));
        ////    //var exception = Assert.IsType<InvalidGuidException>(result);
        ////    //Assert.Equal(exception.Message, result.Message);
        ////    //Assert.Equal(exception.Reason, result.Reason);
        ////}


        ////[Fact]
        ////public void GetById_ShouldLogException_WhenProductDosNotExists()
        ////{
        ////    ////Arrange

        ////    var product = new Product { Id = 12, Name = "Book", Price = 122 };


        ////    var id = 12;
        ////    Expression<Action<ILogger>> expression = x => x.Log(
        ////        It.IsAny<LogLevel>(),
        ////        It.IsAny<EventId>(),
        ////        It.IsAny<It.IsAnyType>(),
        ////        It.IsAny<Exception>(),
        ////        (Func<It.IsAnyType, Exception, string>)It.IsAny<object>()
        ////    );

        ////    _loggerMock.Setup(expression).Verifiable();

        ////    _productRepoMock.Setup(p => p.Get(id)).Throws<NotFoundException>();
        ////    //_productRepoMock.Setup(p => p.Get(id)).Returns(product);

        ////    var controller = new ProductController(_productRepoMock.Object, _loggerMock.Object);

        ////    ////Act
        ////    Assert.Throws<NotFoundException>(() => controller.GetById(12));

        ////    ////Assert
        ////    _loggerMock.Verify(expression, Times.Once);
        ////}


        private Product CreateRandomProduct()
        {
            return new()
            {
                Id = _random.Next(10),
                Name = $"Test Produc {_random.Next(20)}",
                Price = _random.Next(1000)
            };

        }


        //[Fact]      
        //public void AddProduct_ShouldThrowException_WhenProductNameIsNullorEmpty()
        //{
        //    ////// Arrange
        //    var product = new Product { Id = 45, Name = "", Price = 122 };
        //    // _productRepoMock.Setup(p => p.AddProduct(product)).Returns(true);
        //    _productRepoMock.Setup(p => p.AddProduct(product)).Throws<ArgumentNullException>();

        //    var controller = new ProductController(_productRepoMock.Object, _loggerMock.Object);

        //    ////// Act

        //    ////// Assert
        //    Assert.Throws<ArgumentNullException>(()=>controller.AddProduct(product));


        //}

        //[Fact]
        //public void AddProduct_ShouldThrowException_WhenProductIdIsLessThenZero()
        //{
        //    ////// Arrange
        //    var product = new Product { Id = 0, Name = "Book", Price = 122 };
        //    //_productRepoMock.Setup(p => p.AddProduct(product)).Returns(true);
        //    _productRepoMock.Setup(p => p.AddProduct(product)).Throws<ArgumentOutOfRangeException>();
        //    var controller = new ProductController(_productRepoMock.Object, _loggerMock.Object);

        //    ////// Act

        //    ////// Assert
        //    Assert.Throws<ArgumentOutOfRangeException>(() => controller.AddProduct(product));


        //}

        //[Fact]
        //public void AddProduct_ShouldReturnTrue_WhenProductAddedSuccessfully()
        //{
        //    ////Arrange
        //    var product = new Product { Id = 4, Name = "iPad 12 Pro", Price = 1500 };

        //    _productRepoMock.Setup(p => p.AddProduct(product)).Returns(true);

        //    var controller = new ProductController(_productRepoMock.Object, _loggerMock.Object);
        //    ////Act
        //    var actual = controller.AddProduct(product);

        //    ////Assert
        //    Assert.True(actual);

        //}




        [Fact]
        public void AddProduct_ReturnBadRequestResult_WhenModelIsInvalid()
        {

            // Arrange
            var product = new Product { Id = 0, Name = "", Price = 122 };

            var result=_validator.TestValidate(product);

            //var error=  result.ShouldHaveValidationErrorFor(model => model.Name);
            var error = result.ShouldHaveValidationErrorFor(model => model.Name)
                .WithErrorMessage("Product name is empty.")
                .WithSeverity(Severity.Error)
                .WithErrorCode("NotEmptyValidator");

            var er = result.ShouldHaveAnyValidationError();

            Assert.Equal(2, er.Count());                    
           
        }



    }
}
