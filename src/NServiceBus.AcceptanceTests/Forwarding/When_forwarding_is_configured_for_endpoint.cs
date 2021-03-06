﻿namespace NServiceBus.AcceptanceTests.Forwarding
{
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading.Tasks;
    using AcceptanceTesting;
    using EndpointTemplates;
    using NUnit.Framework;

    public class When_forwarding_is_configured_for_endpoint : NServiceBusAcceptanceTest
    {
        [Test]
        public async Task Should_forward_message()
        {
            var context = await Scenario.Define<Context>()
                .WithEndpoint<EndpointThatForwards>(b => b.When((session, c) => session.SendLocal(new MessageToForward())))
                .WithEndpoint<ForwardReceiver>()
                .Done(c => c.GotForwardedMessage)
                .Run();

            Assert.IsTrue(context.GotForwardedMessage);
            CollectionAssert.AreEqual(context.ForwardedHeaders, context.ReceivedHeaders, "Headers should be preserved on the forwarded message");
        }

        public class Context : ScenarioContext
        {
            public bool GotForwardedMessage { get; set; }
            public IReadOnlyDictionary<string, string> ForwardedHeaders { get; set; }
            public IReadOnlyDictionary<string, string> ReceivedHeaders { get; set; }
        }

        public class ForwardReceiver : EndpointConfigurationBuilder
        {
            public ForwardReceiver()
            {
                EndpointSetup<DefaultServer>()
                    .CustomEndpointName("endpoint_forward_receiver");
            }

            public class MessageToForwardHandler : IHandleMessages<MessageToForward>
            {
                public MessageToForwardHandler(Context context)
                {
                    testContext = context;
                }

                public Task Handle(MessageToForward message, IMessageHandlerContext context)
                {
                    testContext.ForwardedHeaders = context.MessageHeaders.ToDictionary(x => x.Key, x => x.Value);
                    testContext.GotForwardedMessage = true;
                    return Task.FromResult(0);
                }

                Context testContext;
            }
        }

        public class EndpointThatForwards : EndpointConfigurationBuilder
        {
            public EndpointThatForwards()
            {
                EndpointSetup<DefaultServer>(c => c.ForwardReceivedMessagesTo("endpoint_forward_receiver"));
            }

            public class MessageToForwardHandler : IHandleMessages<MessageToForward>
            {
                public MessageToForwardHandler(Context testContext)
                {
                    this.testContext = testContext;
                }

                public Task Handle(MessageToForward message, IMessageHandlerContext context)
                {
                    testContext.ReceivedHeaders = context.MessageHeaders.ToDictionary(x => x.Key, x => x.Value);
                    return Task.FromResult(0);
                }

                Context testContext;
            }
        }

        public class MessageToForward : IMessage
        {
        }
    }
}