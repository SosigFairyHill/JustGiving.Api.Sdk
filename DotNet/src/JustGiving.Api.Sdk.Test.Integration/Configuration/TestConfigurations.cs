﻿using System;
using System.Configuration;
using System.Linq.Expressions;

namespace JustGiving.Api.Sdk.Test.Integration.Configuration
{
    public class TestConfigurations : ConfigurationSection, ITestConfigurations
    {
        [ConfigurationProperty("rflDomain", IsRequired = false, DefaultValue = "www.local.raceforlifesponsorme.org")]
        public string RflDomain
        {
            get { return (string) this["rflDomain"]; }
            set { this["rflDomain"] = value; }  
        }
        [ConfigurationProperty("charityUserUserName", IsRequired = false)]
        public string CharityUserUserName
        {
            get { return (string)this["charityUserUserName"]; }
            set { this["charityUserUserName"] = value; }
        }

        [ConfigurationProperty("validEventId", IsRequired = false)]
        public int ValidEventId
        {
            get { return (int) this["validEventId"]; }
            set { this["validEventId"] = value; }
        }

        [ConfigurationProperty("rflUsername")]
        public string RflUsernName
        {
            get { return (string) this["rflUsername"]; }
            set { this["rflUsername"] = value; }
        }
    }

    public class TestConfigurationsHelper
    {
        public static TReturnType GetProperty<TReturnType>(Expression<Func<ITestConfigurations, TReturnType>> func)
        {
            var memberExpression = (MemberExpression)func.Body;
            var propertyName = memberExpression.Member.Name;

            return GetProperty <TReturnType>(propertyName);
        }

        private static TReturnType GetProperty<TReturnType>(string propertyToFind)
        {
            var configuration = (ITestConfigurations) ConfigurationManager.GetSection("testConfigurations");
            foreach (var property in configuration.GetType().GetProperties())
            {
                if (property.Name == propertyToFind)
                {
                    var value = property.GetValue(configuration, null);
                    return (TReturnType)value;
                }
            }

            return default(TReturnType);
        }
    }
}