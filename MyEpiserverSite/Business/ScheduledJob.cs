using System;
using System.IO;
using System.Linq;
using EPiServer;
using EPiServer.Core;
using EPiServer.DataAbstraction;
using EPiServer.PlugIn;
using EPiServer.Scheduler;
using EPiServer.ServiceLocation;
using MyEpiserverSite.Models.Pages;
using MyEpiserverSite.Utilities;

namespace MyEpiserverSite.Business
{
    [ScheduledPlugIn(
        DisplayName = "Scheduled job example", 
        Description = "Scheduled job that writes down amount of standard pages to file",
        InitialTime = "1.12:0:0",
        IntervalLength = 5,
        IntervalType = ScheduledIntervalType.Minutes
        )]
    public class ScheduledJob : ScheduledJobBase
    {
        private static Injected<IContentLoader> ContentLoader { get; set; }       
        private static string _text;
        private bool _stopSignaled;

        public ScheduledJob()
        {
            IsStoppable = true;
        }

        public override void Stop()
        {
            _stopSignaled = true;
        }

        public override string Execute()
        {
            #region
            //OnStatusChanged(String.Format("Starting execution of {0}", this.GetType()));
            //var startPages = ContentLoader.Service.GetChildren<StartPage>(ContentReference.RootPage);
            //var page = startPages.Select(s => s.ContentLink).FirstOrDefault();
            //var standardPages = ContentLoader.Service.GetChildren<StandardPage>(page).Where(p=> p.PageTypeName == "StandardPage");
            #endregion

            var standardPages = ContentLoader.Service.GetChildren<StandardPage>(ContentReference.StartPage).ToList();
            int pageCount = standardPages.Count();

            foreach (var spage in standardPages)
            {
                var children = ContentLoader.Service.GetChildren<StandardPage>(spage.ContentLink).ToList();
                if (children.Any())
                {
                    pageCount += children.Count();
                }
            }

            _text = $"The site has {pageCount} standard pages.";
            var path = @"C:\Users\Shkomi\Documents\Temp\epischeduledjobs.log";
            var log = PersistenceUtility.TextToFile(_text);
            File.AppendAllLines(path,log);

            //Thread.Sleep(5000); 
            if (_stopSignaled)
            {
                return "Stop of job was called";
            }

            return $"The job was executed at {DateTime.Now}";
        }
    }
}