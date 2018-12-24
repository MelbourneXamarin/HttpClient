using System.Collections.Generic;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using HttpDemo.Models;
using HttpDemo.Services;
using Moq;
using Xunit;

namespace MessageHandlerApp.UnitTests
{
    public class ApiServiceWithTest_Test
    {
        private Mock<MoqHttpMessageHandler> moqHttpMessageHandler;
        private readonly HttpClient httpClient;
        public ApiServiceWithTest_Test()
        {
            moqHttpMessageHandler = new Mock<MoqHttpMessageHandler> { CallBase = true };
        }

        [Fact]
        public async Task Successful_Response_Single_Item_In_List()
        {
            //Arrange
            moqHttpMessageHandler.Setup(f => f.Send(It.IsAny<HttpRequestMessage>())).Returns(new HttpResponseMessage
            {
                StatusCode = HttpStatusCode.OK,
                Content = new StringContent("{\"Id\":\"1\",\"Time\":1,\"ConnectionType\":0,\"CallsForConnection\":1}",
                                            Encoding.UTF8,
                                            "application/json")
            });

            var api = new ApiServiceWithTests(moqHttpMessageHandler.Object);

            //Act
            var result = await api.Get<List<Connection>>("Connection/1");

            //Assert
            Assert.NotNull(result);
            Assert.IsType<List<Connection>>(result);
            Assert.Single(result);
        }

        [Fact]
        public async Task Successful_Response_List()
        {
            //Arrange
            moqHttpMessageHandler.Setup(f => f.Send(It.IsAny<HttpRequestMessage>())).Returns(new HttpResponseMessage
            {
                StatusCode = HttpStatusCode.OK,
                Content = new StringContent("[{\"Id\":\"1\",\"Time\":1,\"ConnectionType\":0,\"CallsForConnection\":1}," +
                                            "{\"Id\":\"2\",\"Time\":1,\"ConnectionType\":0,\"CallsForConnection\":12}]",
                                            Encoding.UTF8,
                                            "application/json")
            });

            var api = new ApiServiceWithTests(moqHttpMessageHandler.Object);

            //Act
            var result = await api.Get<List<Connection>>("Connection");

            //Assert
            Assert.NotNull(result);
            Assert.IsType<List<Connection>>(result);
            Assert.Equal(result.Count, 2);
        }
    }
}
