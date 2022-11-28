using Microsoft.AspNetCore.Mvc;
using Moq;
using SportsStore.Controllers;
using SportsStore.Models;
using Xunit;

namespace SportsStore.Tests
{
    public class OrderControllerTests{
        [Fact]
        public void Cannot_Checkout_Empty_Cart()
        {
            //Arrange - create a mock repository
             Mock<IOrderRepository> mock=new Mock<IOrderRepository>();

             //Create an empty cart

             Cart cart=new Cart();
             //create an order 
             Order order=new Order();

             OrderController target= new OrderController(mock.Object,cart);

             //Act
             ViewResult result=target.Checkout(order) as ViewResult;

             // Assert - check that the order hasn't been stored
             mock.Verify(m=>m.SaveOrder(It.IsAny<Order>()),Times.Never);
             //Asser -check that the method is returning the detail view
             Assert.True(string.IsNullOrEmpty(result.ViewName));
             // Assert - check that i am passing an invalid model to the view
             Assert.False(result.ViewData.ModelState.IsValid);
            // Given
        
            // When
        
            // Then
        }
        [Fact]
        public void Cannot_Checkout_Invalid_ShippingDetails()
        {
            // Given

         //Arrange - create a mock repository
             Mock<IOrderRepository> mock=new Mock<IOrderRepository>();

             //Create  cart with one item

             Cart cart=new Cart();
             cart.AddItem(new Product(),1);
             //create an instance of controller        
             OrderController target= new OrderController(mock.Object,cart);
            // Arrange add an error to the model
            target.ModelState.AddModelError("error","error");

            //Act try checkout
            ViewResult result=target.Checkout(new Order()) as ViewResult;

            //Assert check that the order hasn't been passed stored
            mock.Verify(m=>m.SaveOrder(It.IsAny<Order>()),Times.Never);

            //Assert check that the method is returning the default view

            Assert.True(string.IsNullOrEmpty(result.ViewName));
            //Assert check that i am passing an invalid model to the view
            Assert.False(result.ViewData.ModelState.IsValid);
        
            // Then
        }
        [Fact]
        public void Can_Checkout_And_Submit_Order()
        {
            // Given
             //Arrange - create a mock repository
             Mock<IOrderRepository> mock=new Mock<IOrderRepository>();

             //Create  cart with one item

             Cart cart=new Cart();
             cart.AddItem(new Product(),1);
             //create an instance of controller        
             OrderController target= new OrderController(mock.Object,cart);

             //Act try to checkout
             RedirectToPageResult result=target.Checkout(new Order()) as RedirectToPageResult;

             //Assert check that the order has been stored
             mock.Verify(m=>m.SaveOrder(It.IsAny<Order>()),Times.Once);

             //Assert check that the method is redirecting to the completed action

             Assert.Equal("/Completed",result.PageName);
            // When
        
            // Then
        }
    }
}