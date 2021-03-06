using System;
using System.Net;
using GG.Api.Sdk.Test.Unit;
using JustGiving.Api.Sdk.ApiClients;
using JustGiving.Api.Sdk.Model.Search;
using NUnit.Framework;

namespace JustGiving.Api.Sdk.Test.Unit.ApiClients
{
    [TestFixture]
    [Category("Fast")]
    public class SearchApiTests
    {
        private SearchApi _api = null;
        private MockHttpClient<CharitySearchResults> _client = null;

        [SetUp]
        public void Setup()
        {
            _client = new MockHttpClient<CharitySearchResults>(HttpStatusCode.OK);
            _api = ApiClient.Create<SearchApi, CharitySearchResults>(_client);
        }

        [Test]
        public void CharitySearch_AnySearch_PerformsHttpGet()
        {
            _api.CharitySearch("test");
            _client.LastRequest.Method = "GET";
        }

        [Test]
        public void CharitySearch_SearchTermsSpecified_UrlEncodesSearchTerm()
        {
            const string SEARCH = "test/value and &something";
            var expected = Uri.EscapeDataString(SEARCH);
            _api.CharitySearch(SEARCH);
            var actual = _client.LastRequestedUrl;

            Assert.That(actual, Is.StringContaining(expected));
        }

        [Test]
        public void CharitySearch_NoPageSpecified_DefaultsPageTo1()
        {
            _api.CharitySearch("test");
            var url = _client.LastRequestedUrl;

            Assert.That(url, Is.StringContaining("page=1"));
        }

        [Test]
        public void CharitySearch_NoPageSizeSpecified_DefaultsPageSizeTo20()
        {
            _api.CharitySearch("test");
            var url = _client.LastRequestedUrl;

            Assert.That(url, Is.StringContaining("pageSize=20"));
        }

        [Test]
        public void CharitySearch_AllParamsSpecified_CallsUrlWithGetParams()
        {
            _api.CharitySearch("test", 100, 42);

            var url = _client.LastRequestedUrl;
            Assert.That(url, Is.StringContaining("q=test"));
            Assert.That(url, Is.StringContaining("page=100"));
            Assert.That(url, Is.StringContaining("pageSize=42"));
        }
    }
}