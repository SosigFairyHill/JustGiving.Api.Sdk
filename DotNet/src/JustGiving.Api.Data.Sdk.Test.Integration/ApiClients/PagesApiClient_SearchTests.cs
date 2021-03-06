﻿using System;
using JustGiving.Api.Data.Sdk.ApiClients;
using JustGiving.Api.Data.Sdk.Model.CustomCodes;
using JustGiving.Api.Data.Sdk.Test.Integration.TestExtensions;
using JustGiving.Api.Sdk;
using NUnit.Framework;

namespace JustGiving.Api.Data.Sdk.Test.Integration.ApiClients
{
    [TestFixture, Category("Slowest")]
    public class PagesApiClient_SearchTests : ApiTestFixture
    {
        private DateTime _startDate;
        private DateTime _endDate;

        [TestFixtureSetUp]
        public void FixtureSetUp()
        {
            var clientConfiguration = GetDefaultDataClientConfiguration()
                .With((clientConfig) => clientConfig.WireDataFormat = WireDataFormat.Xml);
            
            var client = new JustGivingDataClient(clientConfiguration);
            var customCodeClient = CreateCustomCodeClient(client);

            customCodeClient.SetPageCustomCodes(TestContext.KnownPageIdWithCustomCodes, new PageCustomCodes
                                                                          {
                                                                              CustomCode1 =
                                                                                  TestContext.KnownPageCustomCode1,
                                                                              CustomCode2 =
                                                                                  TestContext.KnownPageCustomCode2,
                                                                              CustomCode3 =
                                                                                  TestContext.KnownPageCustomCode3,
                                                                              CustomCode4 =
                                                                                  TestContext.KnownPageCustomCode4,
                                                                              CustomCode5 =
                                                                                  TestContext.KnownPageCustomCode5,
                                                                              CustomCode6 =
                                                                                  TestContext.KnownPageCustomCode6
                                                                          });

            customCodeClient.SetEventCustomCodes(TestContext.KnownEventIdForEventCustomCodes, new EventCustomCodes
                                                                                               {
                                                                                                   CustomCode1 = TestContext.KnownEventCustomCode1,
                                                                                                   CustomCode2 = TestContext.KnownEventCustomCode2,
                                                                                                   CustomCode3 = TestContext.KnownEventCustomCode3
                                                                                               });
            _startDate = TestContext.ValidPageSearchStartDate;
            _endDate = TestContext.ValidPageSearchEndDate;
        }

        [TestCase(TestContext.KnownEventCustomCode1, "", "")]
        [TestCase("", TestContext.KnownEventCustomCode2, "")]
        [TestCase("", "", TestContext.KnownEventCustomCode3)]
        [TestCase(TestContext.KnownEventCustomCode1, "", TestContext.KnownEventCustomCode3)]
        [TestCase(TestContext.KnownEventCustomCode1, TestContext.KnownEventCustomCode2, "")]
        [TestCase("", TestContext.KnownEventCustomCode2, TestContext.KnownEventCustomCode3)]
        [TestCase(TestContext.KnownEventCustomCode1, TestContext.KnownEventCustomCode2, TestContext.KnownEventCustomCode3)]
        public void EventCustomCodes_Existing_ReturnsResult(string customCode1, string customCode2, string customCode3)
        {
            var clientConfiguration = XmlDataClientConfiguration();

            var client = new JustGivingDataClient(clientConfiguration);
            var pagesClient = CreatePagesClient(client);
            var report = pagesClient.Search(new PageCreatedSearchQuery { EventCustomCode1 = customCode1, EventCustomCode2 = customCode2, EventCustomCode3 = customCode3, IsActivePage = TestContext.PageStatus }, _startDate, _endDate);

            Assert.That(report.Pages.Count, Is.GreaterThan(0));
        }

        [TestCase(TestContext.KnownPageCustomCode1, "","","","","")]
        [TestCase("", TestContext.KnownPageCustomCode2, "", "", "", "")]
        [TestCase("", "", TestContext.KnownPageCustomCode3, "", "", "")]
        [TestCase("", "", "", TestContext.KnownPageCustomCode4, "", "")]
        [TestCase("", "", "", "", TestContext.KnownPageCustomCode5, "")]
        [TestCase("", "", "", "", "", TestContext.KnownPageCustomCode6)]
        [TestCase(TestContext.KnownPageCustomCode1, TestContext.KnownPageCustomCode2, TestContext.KnownPageCustomCode3, TestContext.KnownPageCustomCode4, TestContext.KnownPageCustomCode5, TestContext.KnownPageCustomCode6)]
        public void PageCustomCodes_Existing_ReturnsResult(string pageCustomCode1, string pageCustomCode2, string pageCustomCode3, string pageCustomCode4, string pageCustomCode5, string pageCustomCode6)
        {
            var clientConfiguration = XmlDataClientConfiguration();

            var client = new JustGivingDataClient(clientConfiguration); 
            var pagesClient = CreatePagesClient(client);
            var report = pagesClient.Search(new PageCreatedSearchQuery { PageCustomCode1 = pageCustomCode1, PageCustomCode2 = pageCustomCode2, PageCustomCode3 = pageCustomCode3, PageCustomCode4 = pageCustomCode4, PageCustomCode5 = pageCustomCode5, PageCustomCode6 = pageCustomCode6 }, _startDate, _endDate);

            Assert.That(report.Pages.Count, Is.GreaterThan(0));
        }

        [Test]
        public void FilterByStatus_IncludesExpectedResult()
        {
            var clientConfiguration = XmlDataClientConfiguration();

            var client = new JustGivingDataClient(clientConfiguration);
            var pagesClient = CreatePagesClient(client);
            var report = pagesClient.Search(new PageCreatedSearchQuery { PageCustomCode1 = TestContext.KnownPageCustomCode1, IsActivePage = TestContext.PageStatus }, _startDate, _endDate);

            Assert.That(report.Pages.Count, Is.GreaterThan(0));
        }

        [Test]
        public void FilterByStatus_ExcludesExpectedResult()
        {
            var clientConfiguration = XmlDataClientConfiguration();
            
            var client = new JustGivingDataClient(clientConfiguration);
            var report = client.Pages.Search(new PageCreatedSearchQuery { PageCustomCode1 = TestContext.KnownPageCustomCode1, IsActivePage = !TestContext.PageStatus }, _startDate, _endDate);

            Assert.That(report.Pages.Count, Is.EqualTo(0));
        }

        [Test]
        public void FilterByAppeal_IncludesExpectedResult()
        {
            var clientConfiguration = XmlDataClientConfiguration();

            var client = new JustGivingDataClient(clientConfiguration);
            var pagesClient = CreatePagesClient(client);
            var report = pagesClient.Search(new PageCreatedSearchQuery { PageCustomCode1 = TestContext.KnownPageCustomCode1, AppealName = TestContext.KnownAppealName }, _startDate, _endDate);

            Assert.That(report.Pages.Count, Is.GreaterThan(0));
        }

        [Test]
        public void FilterByAppeal_ExcludesExpectedResult()
        {
            var clientConfiguration = XmlDataClientConfiguration();

            var client = new JustGivingDataClient(clientConfiguration);
            var pagesClient = CreatePagesClient(client);
            var report = pagesClient.Search(new PageCreatedSearchQuery { PageCustomCode1 = TestContext.KnownPageCustomCode1, AppealName = "flurble" }, _startDate, _endDate);

            Assert.That(report.Pages.Count, Is.EqualTo(0));
        }

        [Test]
        public void FilterByExpiresAfterDate_ExcludesExpectedResult()
        {
            var clientConfiguration = XmlDataClientConfiguration();

            var client = new JustGivingDataClient(clientConfiguration); 
            var pagesClient = CreatePagesClient(client);
            var report = pagesClient.Search(new PageCreatedSearchQuery { PageCustomCode1 = TestContext.KnownPageCustomCode1, PageExpiresAfter = TestContext.KnownExpiryDate.AddDays(1) }, new DateTime(2004, 1, 1), new DateTime(2004, 2, 1));
            Assert.That(report.Pages.Count, Is.EqualTo(0));
        }

        private static DataClientConfiguration XmlDataClientConfiguration()
        {
            var clientConfiguration = GetDefaultDataClientConfiguration()
                .With((clientConfig) => clientConfig.WireDataFormat = WireDataFormat.Xml);
            return clientConfiguration;
        }

        [Test]
        public void FilterByExpiresBeforeDate_ExcludesExpectedResult()
        {
            var clientConfiguration = XmlDataClientConfiguration();

            var client = new JustGivingDataClient(clientConfiguration);
            var pagesClient = CreatePagesClient(client);
            var report = pagesClient.Search(new PageCreatedSearchQuery { PageCustomCode1 = TestContext.KnownPageCustomCode1, PageExpiresBefore = TestContext.KnownExpiryDate.AddDays(-1) }, new DateTime(2004, 1, 1), new DateTime(2004, 2, 1));

            Assert.That(report.Pages.Count, Is.EqualTo(0));
        }

        [Test]
        public void FilterByExpiryDate_IncludesExpectedResult()
        {
            var clientConfiguration = XmlDataClientConfiguration();
            var client = new JustGivingDataClient(clientConfiguration);
            var pagesClient = CreatePagesClient(client);
            var report = pagesClient.Search(new PageCreatedSearchQuery
                                         {
                                             PageCustomCode1 = TestContext.KnownPageCustomCode1,
                                             PageExpiresAfter = new DateTime(2000, 01, 01), //2017-03-08 00:00:00.000
                                             PageExpiresBefore = new DateTime(2020, 01, 01)
                                         }, _startDate, _endDate);

            Assert.That(report.Pages.Count, Is.GreaterThan(0));
        }

        [TestCase(TestContext.KnownEventCustomCode1, "", "")]
        [TestCase("", TestContext.KnownEventCustomCode2, "")]
        [TestCase("", "", TestContext.KnownEventCustomCode3)]
        [TestCase(TestContext.KnownEventCustomCode1, "", TestContext.KnownEventCustomCode3)]
        [TestCase(TestContext.KnownEventCustomCode1, TestContext.KnownEventCustomCode2, "")]
        [TestCase("", TestContext.KnownEventCustomCode2, TestContext.KnownEventCustomCode3)]
        [TestCase(TestContext.KnownEventCustomCode1, TestContext.KnownEventCustomCode2, TestContext.KnownEventCustomCode3)]
        public void EventCustomCodes_Existing_WithDates_ReturnsResult(string customCode1, string customCode2, string customCode3)
        {
            var clientConfiguration = XmlDataClientConfiguration();
            var client = new JustGivingDataClient(clientConfiguration);

            var pagesClient = CreatePagesClient(client);
            var report = pagesClient.Search(new PageCreatedSearchQuery { EventCustomCode1 = customCode1, EventCustomCode2 = customCode2, EventCustomCode3 = customCode3 }, _startDate, _endDate);

            Assert.That(report.Pages.Count, Is.GreaterThan(0));
        }

        [TestCase("QWERTYUIOP", "", "")]
        [TestCase("", "QWERTYUIOP", "")]
        [TestCase("", "", "QWERTYUIOP")]
        public void EventCustomCodes_NotExisting_NoResult(string customCode1, string customCode2, string customCode3)
        {
            var clientConfiguration = XmlDataClientConfiguration();

            var client = new JustGivingDataClient(clientConfiguration);
            var pagesClient = CreatePagesClient(client);
            var report = pagesClient.Search(new PageCreatedSearchQuery { EventCustomCode1 = customCode1, EventCustomCode2 = customCode2, EventCustomCode3 = customCode3 }, new DateTime(2004, 1, 1), new DateTime(2004, 2, 1));

            Assert.That(report.Pages.Count, Is.EqualTo(0));
        }

        [TestCase("QWERTYUIOP", TestContext.KnownEventCustomCode2, "")]
        [TestCase("QWERTYUIOP", "", TestContext.KnownEventCustomCode3)]
        [TestCase(TestContext.KnownEventCustomCode1, "QWERTYUIOP", TestContext.KnownEventCustomCode3)]
        public void EventCustomCodes_SomeExistingSomeNot_NoResult(string customCode1, string customCode2, string customCode3)
        {
            var clientConfiguration = XmlDataClientConfiguration();

            var client = new JustGivingDataClient(clientConfiguration);
            var pagesClient = CreatePagesClient(client);
            var report = pagesClient.Search(new PageCreatedSearchQuery { EventCustomCode1 = customCode1, EventCustomCode2 = customCode2, EventCustomCode3 = customCode3 }, new DateTime(2004, 1, 1), new DateTime(2004, 2, 1));

            Assert.That(report.Pages.Count, Is.EqualTo(0));
        }        
    }
}
