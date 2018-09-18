using PlanningPoker.Client.Exceptions;
using PlanningPoker.Client.Utilities;
using Xunit;

namespace PlanningPoker.Client.Tests.UtilitiesTests.UserCacheItemTests
{
    public class DeleteUserTests
    {
        UserCacheProvider _sut;
        public DeleteUserTests()
        {
            _sut = new UserCacheProvider();
        }
        [Fact]
        public async void GivenDeleteUserIsCalled_WhenUserDoesNotExistsInCache_ThenReturnsFalse()
        {
            var result = await _sut.DeleteUser("12345", "543322");

            Assert.False(result);
        }
        [Fact]
        public async void GivenDeleteUserIsCalled_WhenUserExistsInCache_ThenTrueIsReturned()
        {
            var sessionId = "12345";
            var userId = "6789";

            await _sut.AddUser(sessionId, userId, "123456767");

            var result = await _sut.DeleteUser(sessionId, userId);

            Assert.True(result);
        }
    }
}