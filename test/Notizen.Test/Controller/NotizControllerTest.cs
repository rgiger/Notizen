using System.Net;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Moq;
using Notizen.Controllers;
using Notizen.DbModel.Notizen;
using Xunit;

namespace Notizen.Test.Controller
{
    public sealed class NotizControllerTest
    {
        private readonly ApplicationDbContext _dbContext;
        
        private Mock<HttpContext> MockHttpContext { get; }

        public NotizControllerTest()
        {
            var services = new ServiceCollection();
            
            services.AddEntityFrameworkInMemoryDatabase();
            services.AddDbContext<ApplicationDbContext>(options => options.UseInMemoryDatabase());
            services.AddMvc();

            var serviceProvider = services.BuildServiceProvider();
            _dbContext = serviceProvider.GetService<ApplicationDbContext>();
            
            MockHttpContext = new Mock<HttpContext>();
            var mockSession = new Mock<ISession>();
            MockHttpContext.SetupGet(c=>c.Session).Returns(mockSession.Object);
        }


        [Fact]
        public void EditierenNotizNotFound()
        {
            var controller = new NotizController(_dbContext)
            {
                ControllerContext = new ControllerContext() {HttpContext = MockHttpContext.Object }
            };
            
            var resultbad = (StatusCodeResult)controller.Editieren(1000);
            Assert.Equal((int)HttpStatusCode.NotFound, resultbad.StatusCode);
        }

    }
}
