using System;
using PlanningPoker.Client.Exceptions;
using PlanningPoker.Client.Utilities;
using Xunit;

namespace PlanningPoker.Client.Tests.UtilitiesTests
{
    public class GetUserTests
    {
        UserCacheProvider _userCacheProvider;
        public GetUserTests()
        {
            _userCacheProvider = new UserCacheProvider();
        }
        [Fact]
        public async void GivenGetUserIsCalled_WhenSessionIdIsNull_ThenExceptionIsThrown()
        {
            await Assert.ThrowsAsync<ArgumentNullException>(
                async () =>
                {
                    await _userCacheProvider.GetUser(null, "UserId");
                });
        }
        [Fact]
        public async void GivenGetUserIsCalled_WhenSessionIdIsEmpty_ThenExceptionIsThrown()
        {
            await Assert.ThrowsAsync<ArgumentNullException>(
                async () =>
                {
                    await _userCacheProvider.GetUser("", "UserId");
                });
        }
        [Fact]
        public async void GivenGetUserIsCalled_WhenUserIdIsNull_ThenExceptionIsThrown()
        {
            await Assert.ThrowsAsync<ArgumentNullException>(
                async () =>
                {
                    await _userCacheProvider.GetUser("SessionId", null);
                });
        }
        [Fact]
        public async void GivenGetUserIsCalled_WhenUserIdIsEmpty_ThenExceptionIsThrown()
        {
            await Assert.ThrowsAsync<ArgumentNullException>(
                async () =>
                {
                    await _userCacheProvider.GetUser("SessionId", "");
                });
        }
        [Fact]
        public async void GivenGetUserIsCalled_WhenSpecifiedUserIsNotFound_ThenExceptionIsThrown()
        {
            await Assert.ThrowsAsync<NotFoundException>(
                async () =>
                {
                    await _userCacheProvider.GetUser("SessionId", "UserId");
                });
        }
        [Fact]
        public async void GivenGetUserIsCalled_WhenSpecifiedHasBeenAdded_ThenUserIsReturned()
        {
            var expectedSessionId = "12345";
            var expectedUserId = "6789";

            await _userCacheProvider.AddUser(expectedSessionId, expectedUserId, "AToken");

            var result = await _userCacheProvider.GetUser(expectedSessionId, expectedUserId);

            Assert.NotNull(result);
        }
        [Fact]
        public async void GivenGetUserIsCalled_WhenSpecifiedUserHasBeenAdded_ThenUserReturnedMatchesExpectedValues()
        {
            var expectedSessionId = "12345";
            var expectedUserId = "6789";
            var expectedUserName = "User123";
            var expectedToken = "Token123";
            var expectedIsHost = true;
            var expectedIsObserver = true;


            await _userCacheProvider.AddUser(expectedSessionId, expectedUserId, expectedToken);
            await _userCacheProvider.UpdateUser(expectedSessionId, expectedUserId, expectedToken, expectedUserName, expectedIsHost, expectedIsObserver);

            var result = await _userCacheProvider.GetUser(expectedSessionId, expectedUserId);

            Assert.Equal(expectedSessionId, result.SessionId);
            Assert.Equal(expectedUserId, result.UserId);
            Assert.Equal(expectedUserName, result.UserName);
            Assert.Equal(expectedToken, result.Token);
            Assert.Equal(expectedIsHost, result.IsHost);
            Assert.Equal(expectedIsObserver, result.IsObserver);
        }
    }
}