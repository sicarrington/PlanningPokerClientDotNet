using System;
using PlanningPoker.Client.Utilities;
using Xunit;

namespace PlanningPoker.Client.Tests.UtilitiesTests
{
    public class UpdateUserTests
    {
        UserCacheProvider _userCacheProvider;
        public UpdateUserTests()
        {
            _userCacheProvider = new UserCacheProvider();
        }
        [Fact]
        public async void GivenUpdateUserIsCalled_WhenSessionIdIsNull_ThenExceptionIsThrown()
        {
            await Assert.ThrowsAsync<ArgumentNullException>(async () =>
            {
                await _userCacheProvider.UpdateUser(null, "UserId", "Token", "UserName", true, true);
            });
        }
        [Fact]
        public async void GivenUpdateUserIsCalled_WhenSessionIdIsEmpty_ThenExceptionIsThrown()
        {
            await Assert.ThrowsAsync<ArgumentNullException>(async () =>
            {
                await _userCacheProvider.UpdateUser("", "UserId", "Token", "UserName", true, true);
            });
        }
        [Fact]
        public async void GivenUpdateUserIsCalled_WhenUserIdIsNull_ThenExceptionIsThrown()
        {
            await Assert.ThrowsAsync<ArgumentNullException>(async () =>
            {
                await _userCacheProvider.UpdateUser("SessionId", null, "Token", "UserName", true, true);
            });
        }
        [Fact]
        public async void GivenUpdateUserIsCalled_WhenUserIdIsEmpty_ThenExceptionIsThrown()
        {
            await Assert.ThrowsAsync<ArgumentNullException>(async () =>
                        {
                            await _userCacheProvider.UpdateUser("SessionId", "", "Token", "UserName", true, true);
                        });
        }
        [Fact]
        public async void GivenUpdateUserIsCalled_WhenTokenIsNull_ThenExceptionIsThrown()
        {
            await Assert.ThrowsAsync<ArgumentNullException>(async () =>
                                    {
                                        await _userCacheProvider.UpdateUser("SessionId", "UserId", null, "UserName", true, true);
                                    });
        }
        [Fact]
        public async void GivenUpdateUserIsCalled_WhenTokenIsEmpty_ThenExceptionIsThrown()
        {
            await Assert.ThrowsAsync<ArgumentNullException>(async () =>
                                    {
                                        await _userCacheProvider.UpdateUser("SessionId", "UserId", "", "UserName", true, true);
                                    });
        }
        [Fact]
        public async void GivenUpdateUserIsCalled_WhenUserNameIsNull_ThenExceptionIsThrown()
        {
            await Assert.ThrowsAsync<ArgumentNullException>(async () =>
                                    {
                                        await _userCacheProvider.UpdateUser("SessionId", "UserId", "Token", null, true, true);
                                    });
        }
        [Fact]
        public async void GivenUpdateUserIsCalled_WhenUserNameIsEmpty_ThenExceptionIsThrown()
        {
            await Assert.ThrowsAsync<ArgumentNullException>(async () =>
                        {
                            await _userCacheProvider.UpdateUser("SessionId", "UserId", "Token", "", true, true);
                        });
        }
        [Fact]
        public async void GivenUpdateUserIsCalled_WhenPrarmetersPassedAreValidButUserIsNotYetAdded_ThenMethodSucceeds()
        {
            await Assert.ThrowsAsync<InvalidOperationException>(async () =>
                       {
                           await _userCacheProvider.UpdateUser("SessionId", "UserId", "Token", "UserName", true, true);
                       });
        }
        [Fact]
        public async void GivenUpdateUserIsCalled_WhenPrarmetersPassedAreValidAndUserIsAlreadyAdded_ThenMethodSucceeds()
        {
            await _userCacheProvider.AddUser("SessionId", "UserId", "Token");
            await _userCacheProvider.UpdateUser("SessionId", "UserId", "Token", "UserName", true, true);
        }
    }
}
