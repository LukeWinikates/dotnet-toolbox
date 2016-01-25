using System;
using System.Threading;
using dotnet_toolbox.api.PackageCrawling;
using Moq;
using StackExchange.Redis;
using Xunit;

namespace dotnet_toolbox.api.tests.PackageCrawling
{
    public class PackageCrawlerJobListenerTest {
        [Fact]
        public void Listen_PollsRedisForNewJobsAndCrawlsThem() {
            var mockDatabase = new Mock<IDatabase>();
            var fakeTimer = new FakeTimer();
            var mockCrawler = new Mock<ICrawler>();
            var listener = new PackageCrawlerJobListener(fakeTimer, mockDatabase.Object, mockCrawler.Object);
            mockDatabase.SetupSequence(m => m.ListLeftPop("PackageCrawlerJob", CommandFlags.None))
                .Returns("Appetizer").Returns("MainCourse").Returns("Dessert");
                
            listener.Listen();
            fakeTimer.Fire();
            
            mockCrawler.Verify(m => m.CrawlProject("Appetizer"));
            mockCrawler.Verify(m => m.CrawlProject("MainCourse"));
            mockCrawler.Verify(m => m.CrawlProject("Dessert"));
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