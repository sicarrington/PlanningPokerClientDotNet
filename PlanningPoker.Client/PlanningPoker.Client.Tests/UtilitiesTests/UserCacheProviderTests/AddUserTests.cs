using System;
using PlanningPoker.Client.Utilities;
using Xunit;

namespace PlanningPoker.Client.Tests.UtilitiesTests
{
    public class AddUserTests
    {
        UserCacheProvider _userCacheProvider;
        public AddUserTests()
        {
            _userCacheProvider = new UserCacheProvider();
        }
        [Fact]
        public async void GivenAddUserIsCalled_WhenSessionIdIsNull_ThenExceptionIsThrown()
        {
            await Assert.ThrowsAsync<ArgumentNullException>(
                 async () =>
                 {
                     await _userCacheProvider.AddUser(null, "userId", "userToken");
                 });
        }
        [Fact]
        public async void GivenAddUserIsCalled_WhenSessionIdIsEmpty_ThenExceptionIsThrown()
        {
            await Assert.ThrowsAsync<ArgumentNullException>(
                 async () =>
                 {
                     await _userCacheProvider.AddUser("", "userId", "userToken");
                 });
        }
        [Fact]
        public async void GivenAddUserIsCalled_WhenUserIdIsNull_ThenExceptionIsThrown()
        {
            await Assert.ThrowsAsync<ArgumentNullException>(
                             async () =>
                             {
                                 await _userCacheProvider.AddUser("SessionId", null, "userToken");
                             });
        }
        [Fact]
        public async void GivenAddUserIsCalled_WhenUserIdIsEmpty_ThenExceptionIsThrown()
        {
            await Assert.ThrowsAsync<ArgumentNullException>(
                             async () =>
                             {
                                 await _userCacheProvider.AddUser("SessionId", "", "userToken");
                             });
        }
        [Fact]
        public async void GivenAddUserIsCalled_WhenUserTokenIsNull_ThenExceptionIsThrown()
        {
            await Assert.ThrowsAsync<ArgumentNullException>(
                             async () =>
                             {
                                 await _userCacheProvider.AddUser("SessionId", "userId", null);
                             });
        }
        [Fact]
        public async void GivenAddUserIsCalled_WhenUserTokenIsEmpty_ThenExceptionIsThrown()
        {
            await Assert.ThrowsAsync<ArgumentNullException>(
                             async () =>
                             {
                                 await _userCacheProvider.AddUser("SessionId", "userId", "");
                             });
        }
        [Fact]
        public async void GivenAddUserIsCalled_WhenUserAlreadyExists_ThenExceptionIsThrown()
        {
            await _userCacheProvider.AddUser("SessionId", "userId", "12345");

            await Assert.ThrowsAsync<InvalidOperationException>(
                             async () =>
                             {
                                 await _userCacheProvider.AddUser("SessionId", "userId", "12345");
                             });
        }
        [Fact]
        public async void GivenAddUserIsCalled_WhenUserDoesntExist_ThenAddSucceeds()
        {
            await _userCacheProvider.AddUser("SessionId", "userId", "12345");
        }
    }
}