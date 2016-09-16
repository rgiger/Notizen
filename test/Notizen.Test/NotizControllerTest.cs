using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Net;
using System.Security.Claims;
using System.Security.Principal;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ViewFeatures;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Microsoft.Extensions.Testing.Abstractions;
using Moq;
using Notizen.Controllers;
using Notizen.DbModel.Notizen;
using Xunit;

namespace Notizen.Test
{
    public class NotizControllerTest
    {
        private readonly IServiceProvider _serviceProvider;
        private ApplicationDbContext _dbContext;

        public NotizControllerTest()
        {
            var services = new ServiceCollection();

            services.AddDbContext<ApplicationDbContext>(options => options.UseInMemoryDatabase());

            var tempDataFactory = new Mock<ITempDataDictionaryFactory>();
            tempDataFactory.Setup(x => x.GetTempData(It.IsAny<HttpContext>())).Returns(new Mock<ITempDataDictionary>().Object);
            services.AddSingleton(tempDataFactory.Object);

            _serviceProvider = services.BuildServiceProvider();
            _dbContext = _serviceProvider.GetService<ApplicationDbContext>();

        }

        [Fact]
        public void GetListeLeer()
        {
            
            var controller = new NotizController(_dbContext);
            var x = ((ContentResult)controller.Liste()).Content;
             Assert.Equal("", x);
            // Assert.NotEqual(name, ((ContentResult)controller.Name(settingLower)).Content);
        }

    }
}
