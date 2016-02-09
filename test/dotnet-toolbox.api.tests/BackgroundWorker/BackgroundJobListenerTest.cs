using System;
using System.Threading;
using dotnet_toolbox.api.NuspecCrawler;
using dotnet_toolbox.api.BackgroundWorker;
using Moq;
using StackExchange.Redis;
using Xunit;

namespace dotnet_toolbox.api.tests.BackgroundWorker
{
    public class BackgroundJobListenerTest
    {
        Mock<IDatabase> mockDatabase = new Mock<IDatabase>();
        FakeTimer fakeTimer = new FakeTimer();
        Mock<ICrawler> mockCrawler = new Mock<ICrawler>();
        BackgroundJobListener listener;

        public BackgroundJobListenerTest()
        {
            listener = new BackgroundJobListener(fakeTimer, mockDatabase.Object, "QueueName");
            mockDatabase.SetupSequence(m => m.ListRightPopLeftPush("QueueName", "QueueName.Started", CommandFlags.None))
                .Returns("Appetizer").Returns("MainCourse").Returns("Dessert");

        }

        [Fact]
        public void ListenWith_PollsRedisForNewJobsAndRunsThem()
        {
            mockDatabase.SetupSequence(m => m.ListRightPopLeftPush("QueueName", "QueueName.Started", CommandFlags.None))
                .Returns("Appetizer").Returns("MainCourse").Returns("Dessert");
            
            listener.ListenWith(mockCrawler.Object.CrawlProject);

            fakeTimer.Fire();

            mockCrawler.Verify(m => m.CrawlProject("Appetizer"));
            mockCrawler.Verify(m => m.CrawlProject("MainCourse"));
            mockCrawler.Verify(m => m.CrawlProject("Dessert"));
            mockDatabase.Verify(m => m.ListRemove("QueueName.Started", "Appetizer", 0, CommandFlags.None));
            mockDatabase.Verify(m => m.ListRemove("QueueName.Started", "MainCourse", 0, CommandFlags.None));
            mockDatabase.Verify(m => m.ListRemove("QueueName.Started", "Dessert", 0, CommandFlags.None));
        }

        [Fact]
        public void Listen_WhenListenCallbackThrows_FailedJobIsLeftInPendingQueue()
        {
            mockCrawler.Setup(m => m.CrawlProject("MainCourse")).Throws(new Exception("Kabloey!"));
            listener.ListenWith(mockCrawler.Object.CrawlProject);
            fakeTimer.Fire();

            mockCrawler.Verify(m => m.CrawlProject("Appetizer"));
            mockCrawler.Verify(m => m.CrawlProject("MainCourse"));
            mockCrawler.Verify(m => m.CrawlProject("Dessert"));
            mockDatabase.Verify(m => m.ListRemove("QueueName.Started", "Appetizer", 0, CommandFlags.None));
            mockDatabase.Verify(m => m.ListRemove("QueueName.Started", "MainCourse", 0, CommandFlags.None), Times.Never);
            mockDatabase.Verify(m => m.ListRemove("QueueName.Started", "Dessert", 0, CommandFlags.None));
        }
    }


    internal class FakeTimer : ITimerProvider
    {
        private Action action;

        public Timer StartWithCallback(Action action)
        {
            this.action = action;
            return null;
        }

        internal void Fire()
        {
            action();
        }
    }
}