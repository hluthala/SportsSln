using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.AspNetCore.Mvc.ViewComponents;
using Moq;
using SportsStore.Components;
using SportsStore.Models;
using Xunit;

namespace SportsStore.Tests{
    public class NavigationMenuViewComponentTests{
        [Fact]
        public void Indicates_Selected_Category()
        {
            // Given
            string categorySelected="Apples";

             Mock<IStoreRepository> mock=new Mock<IStoreRepository>();
            mock.Setup(m=> m.Products).Returns((new Product[]{
                new Product{ProductID=1,Name="P1",Category="Apples"},
                 new Product{ProductID=2,Name="P2",Category="Apples"},
                  new Product{ProductID=3,Name="P3",Category="Plums"},
                   new Product{ProductID=4,Name="P4",Category="Oranges"},
                    new Product{ProductID=5,Name="P5",Category="Plums"},
                     new Product{ProductID=6,Name="P6",Category="Oranges"}
            }).AsQueryable<Product>());

            NavigationMenuViewComponent target=new NavigationMenuViewComponent(mock.Object);
            target.ViewComponentContext=new ViewComponentContext{
                ViewContext=new ViewContext{
                    RouteData=new Microsoft.AspNetCore.Routing.RouteData()
                }
            };
            target.RouteData.Values["category"]=categorySelected;
            // When
            string result=(string)(target.Invoke() as ViewComponentResult).ViewData["SelectedCategory"];

            // Then
            Assert.Equal(categorySelected,result);
        }
        [Fact]
        public void Can_Select_Categories()
        {
            // Given
        Mock<IStoreRepository> mock=new Mock<IStoreRepository>();
            mock.Setup(m=> m.Products).Returns((new Product[]{
                new Product{ProductID=1,Name="P1",Category="Apples"},
                 new Product{ProductID=2,Name="P2",Category="Apples"},
                  new Product{ProductID=3,Name="P3",Category="Plums"},
                   new Product{ProductID=4,Name="P4",Category="Oranges"},
                    new Product{ProductID=5,Name="P5",Category="Plums"},
                     new Product{ProductID=6,Name="P6",Category="Oranges"}
            }).AsQueryable<Product>());
           NavigationMenuViewComponent target=new NavigationMenuViewComponent(mock.Object);
           //Act=get the set of categories
           string[] results=((IEnumerable<string>)(target.Invoke() as 
           ViewComponentResult).ViewData.Model).ToArray();
            // When
        //Assert
        Assert.True(Enumerable.SequenceEqual(new string[]{"Apples","Oranges","Plums"},results));
            // Then
        }
    }
}