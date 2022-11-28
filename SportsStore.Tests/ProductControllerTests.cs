using Moq;
using Xunit;
using SportsStore.Models;
using System.Linq;
using SportsStore.Controllers;
using System.Collections.Generic;
using Microsoft.AspNetCore.Mvc;
using SportsStore.Models.ViewModels;

namespace SportsStore.Tests
{
    public class ProductControllerTests{

        [Fact]
        public void Can_Paginate()
        {
            //Arrange
             Mock<IStoreRepository> mock=new Mock<IStoreRepository>();
            mock.Setup(m=> m.Products).Returns((new Product[]{
                new Product{ProductID=1,Name="P1"},
                 new Product{ProductID=2,Name="P2"},
                  new Product{ProductID=3,Name="P3"},
                   new Product{ProductID=4,Name="P4"},
                    new Product{ProductID=5,Name="P5"},
                     new Product{ProductID=6,Name="P6"}
            }).AsQueryable<Product>());
            HomeController controller=new HomeController(mock.Object);
            controller.PageSize=3;

            //Act
            // IEnumerable<Product> result=(controller.Index(2) as 
            // ViewResult).ViewData.Model as IEnumerable<Product>;

            ProductListViewModel result=controller.Index(null,2).ViewData.Model as ProductListViewModel;

            //Assert
             Product[] prodArray=result.Products.ToArray();
            Assert.True(prodArray.Length==2);
            Assert.Equal("P4",prodArray[0].Name);
             Assert.Equal("P5",prodArray[1].Name);
        }
        [Fact]
        public void Can_Use_Repository()
        {
            // Arrange
            Mock<IStoreRepository> mock=new Mock<IStoreRepository>();
            mock.Setup(m=> m.Products).Returns((new Product[]{
                new Product{ProductID=1,Name="P1"},
                 new Product{ProductID=1,Name="P2"}
            }).AsQueryable<Product>());

            HomeController controller=new HomeController(mock.Object);

            //Act
            // in case the method index is IACTIONRESULT
            // IEnumerable<Product> result=(controller.Index() as ViewResult).ViewData.Model as IEnumerable<Product>;
           ProductListViewModel result=controller.Index(null).ViewData.Model as ProductListViewModel;

            //Assert
            Product[] prodArray=result.Products.ToArray();
            Assert.True(prodArray.Length==2);
            Assert.Equal("P1",prodArray[0].Name);
             Assert.Equal("P2",prodArray[1].Name);
        }
        [Fact]
        public void Can_Filter_Products()
        {
            // Given
        Mock<IStoreRepository> mock=new Mock<IStoreRepository>();
            mock.Setup(m=> m.Products).Returns((new Product[]{
                new Product{ProductID=1,Name="P1",Category="Cat1"},
                 new Product{ProductID=2,Name="P2",Category="Cat2"},
                  new Product{ProductID=3,Name="P3",Category="Cat1"},
                   new Product{ProductID=4,Name="P4",Category="Cat2"},
                    new Product{ProductID=5,Name="P5",Category="Cat1"},
                     new Product{ProductID=6,Name="P6",Category="Cat3"}
            }).AsQueryable<Product>());
            HomeController controller=new HomeController(mock.Object);
            controller.PageSize=3;
            // When
        Product[] result=(controller.Index("Cat2",1).ViewData.Model 
        as ProductListViewModel).Products.ToArray();
            // Then
            //Assert
            Assert.Equal(2,result.Length);
            Assert.True(result[0].Name=="P2" && result[0].Category=="Cat2");
            Assert.True(result[1].Name=="P4" && result[1].Category=="Cat2");
        }
    }
}