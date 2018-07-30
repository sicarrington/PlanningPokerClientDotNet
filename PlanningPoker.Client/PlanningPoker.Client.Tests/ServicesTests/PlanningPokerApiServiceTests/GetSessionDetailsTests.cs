using System;
using System.Net;
using System.Net.Http;
using Microsoft.Extensions.Options;
using Moq;
using PlanningPoker.Client.Exceptions;
using PlanningPoker.Client.Model;
using PlanningPoker.Client.Services;
using Xunit;

namespace PlanningPoker.Client.Tests.ServicesTests.PlanningPokerApiServiceTests
{
    public class GetSessionDetailsTests
    {
        private Mock<IOptions<ConnectionSettings>> _options;
        private Mock<ConnectionSettings> _connectionSettings;
        private Mock<FakeHttpMessageHandler> _fakeHttpMessageHandler;
        private HttpClient _httpClient;
        private PlanningPokerApiService _planningPokerApiService;
        private string _serviceUri = "http://asuperservice.sicplanningpokerservice.net/api/";
        private string _apiKey = "9897654";

        public GetSessionDetailsTests()
        {
            _connectionSettings = new Mock<ConnectionSettings>();
            _connectionSettings.Setup(x => x.PlanningApiUri).Returns(new Uri(_serviceUri));
            _connectionSettings.Setup(x => x.ApiKey).Returns(_apiKey);
            _options = new Mock<IOptions<ConnectionSettings>>();
            _options.Setup(x => x.Value).Returns(_connectionSettings.Object);
            _fakeHttpMessageHandler = new Mock<FakeHttpMessageHandler> { CallBase = true };
            _httpClient = new HttpClient(_fakeHttpMessageHandler.Object);

            _planningPokerApiService = new PlanningPokerApiService(_httpClient, _options.Object);
        }

        [Fact]
        public async void GivenGetSessionDetailsIsCalled_WhenSessionIdPassedIsNull_ThenExceptionIsThrown()
        {
            await Assert.ThrowsAsync<ArgumentNullException>(async () =>
            {
                await _planningPokerApiService.GetSessionDetails(null);
            });
        }
        [Fact]
        public async void GivenGetSessionDetailsIsCalled_WhenSessionIdPassedIsEmpty_ThenExceptionIsThrown()
        {
            await Assert.ThrowsAsync<ArgumentNullException>(async () =>
            {
                await _planningPokerApiService.GetSessionDetails("");
            });
        }
        [Fact]
        public async void GivenGetSessionDetailsIsCalled_WhenSessionIdIsPassed_ThenCorrectServiceUrlIsInvoked()
        {
            var sessionId = "12345";

            _fakeHttpMessageHandler.Setup(f => f.Send(It.IsAny<HttpRequestMessage>()))
                .Returns(new HttpResponseMessage
                {
                    StatusCode = HttpStatusCode.OK,
                    Content = new StringContent($"{{\"sessionId\": \"{sessionId}\",\"storyPointType\": 2,\"participants\": " +
                        "[{\"id\": \"628d73c8-3f1c-44eb-9775-ac0c3cb347e4\"," +
                    "\"name\": \"Simon\"," +
                    "\"currentVote\": 0," +
                    "\"sessionId\": \"430577\"," +
                    "\"isHost\": true," +
                    "\"isObserver\": false," +
                    "\"currentVoteDescription\": \"Not Voted\"}]}")
                });

            await _planningPokerApiService.GetSessionDetails(sessionId);

            _fakeHttpMessageHandler.Verify(x => x.Send(
                It.Is<HttpRequestMessage>(m =>
                    m.Method == HttpMethod.Get &&
                    m.RequestUri == new Uri($"{_serviceUri}Sessions/{sessionId}"))), Times.Once);
        }
        [Fact]
        public async void GivenGetSessionDetailsIsCalled_WhenSpecifiedSessionIsNotFound_ThenErrorIsThrown()
        {
            var sessionId = "12345";

            _fakeHttpMessageHandler.Setup(f => f.Send(It.IsAny<HttpRequestMessage>()))
                .Returns(new HttpResponseMessage
                {
                    StatusCode = HttpStatusCode.NotFound
                });

            await Assert.ThrowsAsync<NotFoundException>(async () =>
            {
                await _planningPokerApiService.GetSessionDetails(sessionId);
            });
        }
        [Fact]
        public async void GivenGetSessionDetailsIsCalled_WhenSpecifiedSessionDetailsAreReturnedByService_ThenDetailsAreCorrectlyMappedAndReturned()
        {
            var sessionId = "12345";

            _fakeHttpMessageHandler.Setup(f => f.Send(It.IsAny<HttpRequestMessage>()))
                .Returns(new HttpResponseMessage
                {
                    StatusCode = HttpStatusCode.OK,
                    Content = new StringContent($"{{\"sessionId\": \"{sessionId}\",\"storyPointType\": 2,\"participants\": " +
                        "[{\"id\": \"628d73c8-3f1c-44eb-9775-ac0c3cb347e4\"," +
                    "\"name\": \"Simon\"," +
                    "\"currentVote\": 0," +
                    "\"sessionId\": \"430577\"," +
                    "\"isHost\": true," +
                    "\"isObserver\": false," +
                    "\"currentVoteDescription\": \"Not Voted\"}]}")
                });

            var result = await _planningPokerApiService.GetSessionDetails(sessionId);

            Assert.NotNull(result);

            Assert.Equal(sessionId, result.SessionId);
            Assert.Equal(2, result.StoryPointType);
            Assert.Equal(1, result.Participants.Count);
            Assert.Equal("628d73c8-3f1c-44eb-9775-ac0c3cb347e4", result.Participants[0].Id);
            Assert.Equal("Simon", result.Participants[0].Name);
            Assert.Equal(StoryPoint.NotVoted, result.Participants[0].CurrentVote);
            Assert.Equal(true, result.Participants[0].IsHost);
            Assert.Equal(false, result.Participants[0].IsObserver);
        }
    }
}