﻿using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading;
using System.Web;
using Castle.Core.Internal;
using EPiServer;
using EPiServer.Core;
using EPiServer.DataAbstraction;
using EPiServer.PlugIn;
using EPiServer.Scheduler;
using EPiServer.ServiceLocation;
using ICSharpCode.SharpZipLib.Zip;
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
            //OnStatusChanged(String.Format("Starting execution of {0}", this.GetType()));
            //var standardPages = ContentLoader.Service.GetChildren<PageData>(ContentReference.StartPage);
            var startPages = ContentLoader.Service.GetChildren<StartPage>(ContentReference.RootPage);
            var page = startPages.Select(s => s.ContentLink).FirstOrDefault();

            var standardPages = ContentLoader.Service.GetChildren<StandardPage>(page).Where(p=> p.PageTypeName == "StandardPage");
            _text = $"The site has {standardPages.Count()} standard pages.";

            var path = @"C:\Users\Shkomi\Documents\Temp\epischeduledjobs.log";
            var log = PersistenceUtility.TextToFile(_text);
            File.AppendAllLines(path,log);

            Thread.Sleep(5000); 
            if (_stopSignaled)
            {
                return "Stop of job was called";
            }

            return $"The job was executed at {DateTime.Now}";
        }

        
    }
}