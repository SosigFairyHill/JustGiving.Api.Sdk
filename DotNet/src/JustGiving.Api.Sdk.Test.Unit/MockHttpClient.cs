﻿using System;
using System.Collections.Generic;
using System.IO;
using System.Net;
using System.Runtime.Serialization;
using JustGiving.Api.Sdk.Http;
using Microsoft.Http;

namespace GG.Api.Sdk.Test.Unit
{
    public class MockHttpClient<TResponseType> : IHttpClient where TResponseType : class, new()
    {
        private readonly HttpStatusCode _resultcode;
        public static Dictionary<string, string> Headers;

        public MockHttpClient(HttpStatusCode resultcode)
        {
            _resultcode = resultcode;
            Headers = new Dictionary<string, string>();
        }

        public HttpRequestMessage LastRequest { get; set; }

        public string LastRequestedUrl
        {
            get
            {
                if (LastRequest == null)
                    throw new NullReferenceException(
                        "Unable to Retrieve Last Called URL - Nothing has been Requested!");

                // NOTE: We Use AbsoluteUri here since ToString Removes *some* URL Encoding.
                return LastRequest.Uri.AbsoluteUri;
            }
        }

        public void Dispose()
        {
            throw new NotImplementedException();
        }

        public HttpResponseMessage Get(string uri)
        {
            throw new NotImplementedException();
        }

        public HttpResponseMessage Get(string uri, string contentType)
        {
            throw new NotImplementedException();
        }

        public HttpResponseMessage Delete(string url)
        {
            throw new NotImplementedException();
        }

        public void AddHeader(string key, string value)
        {
            Headers.Add(key, value);
        }

        public void Put(string url, string contentType, HttpContent body)
        {
            throw new NotImplementedException();
        }

        public HttpResponseMessage Send(HttpRequestMessage httpRequestMessage)
        {
            LastRequest = httpRequestMessage;
            var response = new HttpResponseMessage();
            var content = new TResponseType();
            response.Content = BuildPayload(content);
            response.StatusCode = _resultcode;
            return response;
        }

        public void SendAsync(HttpRequestMessage httpRequestMessage)
        {
            throw new NotImplementedException();
        }

        public HttpResponseMessage Post(string url, string contentType, HttpContent body)
        {
            throw new NotImplementedException();
        }


        private static HttpContent BuildPayload<TPayloadType>(TPayloadType objectToSerialise)
        {
            var payloadContent = SerializeContentToXml(objectToSerialise);
            var payload = HttpContent.Create(payloadContent, "application/xml");
            return payload;
        }

        private static string SerializeContentToXml<TPayloadType>(TPayloadType objectToSerialise)
        {
            using (var memoryStream = new MemoryStream())
            {
                var dataContractSerializer = new DataContractSerializer(objectToSerialise.GetType());
                dataContractSerializer.WriteObject(memoryStream, objectToSerialise);
                memoryStream.Flush();
                memoryStream.Position = 0;
                using (var reader = new StreamReader(memoryStream))
                {
                    return reader.ReadToEnd();
                }
            }
        }
    }
}