using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.Internal;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Logging.Abstractions;
using Microsoft.Extensions.Primitives;
using System.Collections.Generic;

namespace grey.foodtruck.api.unittest.Helper
{
    /// <summary>
    /// Source found here: https://docs.microsoft.com/en-us/azure/azure-functions/functions-test-a-function
    /// </summary>
    public class TestHelper
    {
        private Dictionary<string, StringValues> CreateDictionary(Dictionary<string, string> queryValues)
        {
            var qs = new Dictionary<string, StringValues>();
            foreach (var queryItem in queryValues.Keys)
            {
                qs.Add(queryItem, queryValues[queryItem]);
            }

            return qs;
        }

        public HttpRequest CreateHttpRequest(Dictionary<string, string> queryValues)
        {
            var context = new DefaultHttpContext();
            var request = context.Request;
            request.Query = new QueryCollection(CreateDictionary(queryValues));
            return request;
        }

        public ILogger CreateLogger(LoggerTypes type = LoggerTypes.Null)
        {
            ILogger logger;

            if (type == LoggerTypes.List)
            {
                logger = new ListLogger();
            }
            else
            {
                logger = NullLoggerFactory.Instance.CreateLogger("Null Logger");
            }

            return logger;
        }
    }
}
