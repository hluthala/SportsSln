using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Routing;
using Microsoft.AspNetCore.Razor.TagHelpers;
using Moq;
using SportsStore.Controllers;
using SportsStore.Infrastructure;
using SportsStore.Models;
using SportsStore.Models.ViewModels;
using Xunit;

namespace SportsStore.Tests{
   public class PageLinkTagHelperTests{
       [Fact]
       public void Can_Send_Pagination_View_Model()
       {
           // Given
       Mock<IStoreRepository> mock=new Mock<IStoreRepository>();
       mock.Setup(m=>m.Products).Returns((new Product[]{
           new Product{ProductID=1,Name="P1"},
           new Product{ProductID=1,Name="P2"},
           new Product{ProductID=1,Name="P3"},
           new Product{ProductID=1,Name="P4"},
           new Product{ProductID=1,Name="P5"}
       }).AsQueryable<Product>());
       // Arrange
       HomeController controller=new HomeController(mock.Object){PageSize=3};
       //Act 
       ProductListViewModel result=controller.Index(null,2).ViewData.Model as  ProductListViewModel;
       //Assert
       PagingInfo pagingInfo=result.PagingInfo;
       Assert.Equal(2,pagingInfo.CurrentPage);
       Assert.Equal(3,pagingInfo.ItemsPerpage);
       Assert.Equal(5,pagingInfo.TotalItems);
       Assert.Equal(2,pagingInfo.TotalItems);
           // When
       
           // Then
       }
       [Fact]
       public void Can_Generate_Page_Links()
       {
           // Given
       var urlHelper=new Mock<IUrlHelper>();
       
           // When
       urlHelper.SetupSequence(x=>x.Action(It.IsAny<UrlActionContext>()))
       .Returns("Test/Page1")
       .Returns("Test/Page2")
       .Returns("Test/Page3");

       var urlHelperFactory=new Mock<IUrlHelperFactory>();
       urlHelperFactory.Setup(f=>f.GetUrlHelper(It.IsAny<ActionContext>()))
       .Returns(urlHelper.Object);

       PageLinkTagHelper helper=new PageLinkTagHelper(urlHelperFactory.Object){
           PageModel=new PagingInfo{
               CurrentPage=2,TotalItems=28,ItemsPerpage=10
           },PageAction="Test"
       };
       TagHelperContext ctx=
       new TagHelperContext(new TagHelperAttributeList(),new Dictionary<object,object>(),"");
        var content=new Mock<TagHelperContent>();
        TagHelperOutput output=
        new TagHelperOutput("div",new TagHelperAttributeList(),
        (cache,encoder)=>Task.FromResult(content.Object));
           // Then
           //Act
           helper.Process(ctx,output);
           //Assert
           Assert.Equal(@"<a href=""Test/Page1"">1</a>"
           +@"<a href=""Test/Page2"">2</a>"
           +@"<a href""Test/Page3"">3</a>",output.Content.GetContent());
       }
   }
}