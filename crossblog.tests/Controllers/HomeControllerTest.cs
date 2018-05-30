using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using crossblog.Controllers;
using crossblog.Domain;
using crossblog.Dto;
using crossblog.Extensions;
using crossblog.Model;
using crossblog.tests.Extensions;
using Microsoft.AspNetCore.Mvc;
using Moq;
using Xunit;

namespace crossblog.tests.Controllers
{
    public class HomeControllerTest
    {

        [Fact]
        public void Index_Test()
        {
            // arrange 
            var txt = "Blog Api";

            // act
            var controller = new HomeController();
            var fetch = controller.Index();

            // assert
            var result = Assert.IsType<ContentResult>(fetch);
            Assert.Contains(txt, result.Content);
        }

        [Fact]
        public void Error_Test()
        {
            // arrange 
            var mockStatus = 404;

            // act
            var controller = new HomeController();
            var fetch = controller.Error(mockStatus);


            // assert
            var result = Assert.IsType<ObjectResult>(fetch);
            Assert.NotNull(result);
            var response = (ApiResponse)result.Value;
            Assert.NotNull(response);
            Assert.False(response.IsSuccess);
            Assert.NotEqual("", response.ErrorMessage);
        }

    }
}