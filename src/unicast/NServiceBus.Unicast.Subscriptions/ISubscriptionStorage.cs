using System.Collections.Generic;

namespace NServiceBus.Unicast.Subscriptions
{
	/// <summary>
	/// Defines storage for subscriptions
	/// </summary>
    public interface ISubscriptionStorage
    {
        /// <summary>
        /// Subscribes the given client address to messages of the given types.
        /// </summary>
        /// <param name="client"></param>
        /// <param name="messageTypes"></param>
	    void Subscribe(string client, IEnumerable<string> messageTypes);

        /// <summary>
        /// Unsubscribes the given client address from messages of the given types.
        /// </summary>
        /// <param name="client"></param>
        /// <param name="messageTypes"></param>
        void Unsubscribe(string client, IEnumerable<string> messageTypes);

        /// <summary>
        /// Returns a list of addresses of subscribers that previously requested to be notified
        /// of messages of the given message types.
        /// </summary>
        /// <param name="messageTypes"></param>
        /// <returns></returns>
        IEnumerable<string> GetSubscribersForMessage(IEnumerable<string> messageTypes);

        /// <summary>
        /// Notifies the subscription storage that now is the time to perform
        /// any initialization work
        /// </summary>
        void Init();
    }
}
