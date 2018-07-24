namespace PlanningPoker.Client.Model
{
    internal class UserCacheItem
    {
        internal virtual string SessionId { get; set; }
        internal virtual string UserId { get; set; }
        internal virtual string UserName { get; set; }
        internal virtual bool? IsHost { get; set; }
        internal virtual bool? IsObserver { get; set; }
        internal virtual string Token { get; set; }
        internal UserCacheItem()
        {

        }
    }
}