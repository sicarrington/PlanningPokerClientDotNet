using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Threading.Tasks;
using PlanningPoker.Client.Exceptions;
using PlanningPoker.Client.Model;

namespace PlanningPoker.Client.Utilities
{
    internal class UserCacheProvider
    {
        private ConcurrentDictionary<string, UserCacheItem> _userCache;
        internal UserCacheProvider()
        {
            _userCache = new ConcurrentDictionary<string, UserCacheItem>();
        }
        internal virtual Task<UserCacheItem> GetUser(string sessionId, string userId)
        {
            if (string.IsNullOrWhiteSpace(sessionId))
            {
                throw new ArgumentNullException(nameof(sessionId));
            }
            if (string.IsNullOrWhiteSpace(userId))
            {
                throw new ArgumentNullException(userId);
            }
            UserCacheItem user;
            if (_userCache.TryGetValue(getKey(sessionId, userId), out user))
            {
                return Task.FromResult(user);
            }
            else
            {
                throw new NotFoundException("User not found");
            }
        }
        internal virtual Task AddUser(string sessionId, string userId, string token)
        {
            if (string.IsNullOrWhiteSpace(sessionId))
            {
                throw new ArgumentNullException(nameof(sessionId));
            }
            if (string.IsNullOrWhiteSpace(userId))
            {
                throw new ArgumentNullException(nameof(userId));
            }
            if (string.IsNullOrWhiteSpace(token))
            {
                throw new ArgumentNullException(nameof(token));
            }
            string storageKey = getKey(sessionId, userId);
            if (!_userCache.ContainsKey(storageKey))
            {
                AddOrUpdate(sessionId, userId, token);
            }
            else
            {
                throw new InvalidOperationException("User already exists");
            }
            return Task.CompletedTask;
        }
        internal Task UpdateUser(string sessionId, string userId, string token, string userName, bool isHost, bool isObserver)
        {
            if (string.IsNullOrWhiteSpace(sessionId))
            {
                throw new ArgumentNullException(nameof(sessionId));
            }
            if (string.IsNullOrWhiteSpace(userId))
            {
                throw new ArgumentNullException(nameof(userId));
            }
            if (string.IsNullOrWhiteSpace(token))
            {
                throw new ArgumentNullException(nameof(token));
            }
            if (string.IsNullOrWhiteSpace(userName))
            {
                throw new ArgumentNullException(nameof(userName));
            }
            string storageKey = getKey(sessionId, userId);
            if (_userCache.ContainsKey(storageKey))
            {
                AddOrUpdate(sessionId, userId, token, userName, isHost, isObserver);
            }
            else
            {
                throw new InvalidOperationException("User does snot exist");
            }

            return Task.CompletedTask;
        }
        private void AddOrUpdate(string sessionId, string userId, string token, string userName = null, bool? isHost = null, bool? isObserver = null)
        {
            string storageKey = getKey(sessionId, userId);
            var newValue = new UserCacheItem
            {
                SessionId = sessionId,
                UserId = userId,
                Token = token,
                UserName = userName,
                IsHost = isHost,
                IsObserver = isObserver
            };
            _userCache.AddOrUpdate(storageKey, newValue, (key, oldValue) =>
            {
                return newValue;
            });
        }
        private string getKey(string sessionId, string userId)
        {
            return $"{sessionId}:{userId}";
        }
    }
}