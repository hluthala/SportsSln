using System;
using System.Linq;
using Microsoft.AspNetCore.Mvc;
using Moq;
using SportsStore.Controllers;
using SportsStore.Models;
using SportsStore.Models.ViewModels;
using Xunit;

namespace SportsStore.Tests{
    public class HomeControllerTests{
        [Fact]
        public void Generate_category_specific_Product_Count()
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
            HomeController target=new HomeController(mock.Object);
            target.PageSize=3;
            // When
        Func<ViewResult,ProductListViewModel> GetModel=result=>
        result?.ViewData?.Model as ProductListViewModel;

        int? res1=GetModel(target.Index("Cat1"))?.PagingInfo.TotalItems;
        int? res2=GetModel(target.Index("Cat2"))?.PagingInfo.TotalItems;
        int? res3=GetModel(target.Index("Cat3"))?.PagingInfo.TotalItems;
        int? resAll=GetModel(target.Index(null))?.PagingInfo.TotalItems;
            // Then
            Assert.Equal(3,res1);
             Assert.Equal(2,res2);
              Assert.Equal(1,res3);
               Assert.Equal(6,resAll);
        }
    }
}