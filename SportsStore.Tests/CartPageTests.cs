using System.Linq;
using System.Text;
using System.Text.Json;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Moq;
// using Newtonsoft.Json;
using SportsStore.Models;
using SportsStore.Pages;
using Xunit;

namespace SportsStore.Tests
{
    public class CartPageTests{
        [Fact]
        public void Can_Update_Cart()
        {
            // Given create mock repository
            Mock<IStoreRepository> mockRepo=new Mock<IStoreRepository>();
            mockRepo.Setup(m=> m.Products).Returns((new Product[]{
                new Product{ProductID=1,Name="P1"}
            }).AsQueryable<Product>()
            );

            Cart testcart=new Cart();
            Mock<ISession> mockSession=new Mock<ISession>();
            mockSession.Setup(s=>s.Set(It.IsAny<string>(),It.IsAny<byte[]>()))
            .Callback<string,byte[]>((key,val)=>{
                testcart=JsonSerializer.Deserialize<Cart>(Encoding.UTF8.GetString(val));
            });
       

        Mock<HttpContext> mockContext=new Mock<HttpContext>();
        mockContext.SetupGet(c=>c.Session).Returns(mockSession.Object);
        

            // When
        CartModel cartModel=new CartModel(mockRepo.Object,testcart){
            PageContext=new PageContext(new ActionContext{
                HttpContext=mockContext.Object,
                RouteData=new Microsoft.AspNetCore.Routing.RouteData(),
                ActionDescriptor=new PageActionDescriptor()
            })
        };
        cartModel.OnPost(1,"myUrl");

            // Then

        Assert.Single(testcart.Lines);
        Assert.Equal("P1",testcart.Lines.First().Product.Name);
        Assert.Equal(1,testcart.Lines.First().Quantity);
        }
        [Fact]
        public void Can_Load_Cart()
        {
            Product p1=new Product{ProductID=1,Name="P1"};
             Product p2=new Product{ProductID=2,Name="P2"};
            // Given
         Mock<IStoreRepository> mock=new Mock<IStoreRepository>();
            mock.Setup(m=> m.Products).Returns((new Product[]{
               p1,
                 p2
            }).AsQueryable<Product>());
            // When
        Cart testcart=new Cart();
        testcart.AddItem(p1,2);
        testcart.AddItem(p2,1);

        Mock<ISession> mockSession=new Mock<ISession>();
        byte[] data=Encoding.UTF8.GetBytes(JsonSerializer.Serialize(testcart));
        mockSession.Setup(c=>c.TryGetValue(It.IsAny<string>(),out data));

        Mock<HttpContext> mockContext=new Mock<HttpContext>();
        mockContext.SetupGet(c=>c.Session).Returns(mockSession.Object);
        CartModel cartModel=new CartModel(mock.Object,testcart){
            PageContext=new PageContext(new ActionContext{
                HttpContext=mockContext.Object,
                RouteData=new Microsoft.AspNetCore.Routing.RouteData(),
                ActionDescriptor=new PageActionDescriptor()
            })
        };
        cartModel.OnGet("myUrl");
        //Assert
        Assert.Equal(2,cartModel.Cart.Lines.Count());
        Assert.Equal("myUrl",cartModel.ReturnUrl);
            // Then
        }
    }
}