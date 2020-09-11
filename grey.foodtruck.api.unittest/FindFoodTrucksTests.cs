using grey.foodtruck.api.Assets;
using grey.foodtruck.api.unittest.Helper;
using Microsoft.AspNetCore.Mvc;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace grey.foodtruck.api.unittest
{
    [TestClass]
    public class FindFoodTrucksTest
    {
        private TestHelper testHelper;

        [TestInitialize]
        public void Init()
        {
            testHelper = new TestHelper();
        }

        [TestMethod]
        public void No_Input_Test()
        {
            Dictionary<string, string> svcInputs = new Dictionary<string, string>();

            ListLogger logger = testHelper.CreateLogger(LoggerTypes.List) as ListLogger;
            var result = FindFoodTrucks.Run(testHelper.CreateHttpRequest(svcInputs), logger);

            Assert.AreEqual(1, logger.Logs.Count);
            Assert.AreEqual(ResourceStrings.REQUEST_STARTED_MESSAGE, logger.Logs.First());

            Assert.AreEqual(TaskStatus.RanToCompletion, result.Status);
            OkObjectResult resultObject = result.Result as OkObjectResult;

            Assert.AreEqual(200, resultObject.StatusCode);
            Assert.AreEqual(ResourceStrings.MISSING_COORDINATE_INPUT_MESSAGE, resultObject.Value);
        }

        [TestMethod]
        public void Invalid_Longitude_Input_Test()
        {
            Dictionary<string, string> svcInputs = new Dictionary<string, string>()
            {
                { "longitude", "apple" },
                { "latitude", "1" }
            };

            ListLogger logger = testHelper.CreateLogger(LoggerTypes.List) as ListLogger;
            var result = FindFoodTrucks.Run(testHelper.CreateHttpRequest(svcInputs), logger);

            Assert.AreEqual(1, logger.Logs.Count);
            Assert.AreEqual(ResourceStrings.REQUEST_STARTED_MESSAGE, logger.Logs.First());

            Assert.AreEqual(TaskStatus.RanToCompletion, result.Status);
            OkObjectResult resultObject = result.Result as OkObjectResult;

            Assert.AreEqual(200, resultObject.StatusCode);
            Assert.AreEqual(ResourceStrings.INVALID_LONGITUDE_INPUT_MESSAGE, resultObject.Value);
        }

        [TestMethod]
        public void Invalid_Latitude_Input_Test()
        {
            Dictionary<string, string> svcInputs = new Dictionary<string, string>()
            {
                { "longitude", "1" },
                { "latitude", "apple" }
            };

            ListLogger logger = testHelper.CreateLogger(LoggerTypes.List) as ListLogger;
            var result = FindFoodTrucks.Run(testHelper.CreateHttpRequest(svcInputs), logger);

            Assert.AreEqual(1, logger.Logs.Count);
            Assert.AreEqual(ResourceStrings.REQUEST_STARTED_MESSAGE, logger.Logs.First());

            Assert.AreEqual(TaskStatus.RanToCompletion, result.Status);
            OkObjectResult resultObject = result.Result as OkObjectResult;

            Assert.AreEqual(200, resultObject.StatusCode);
            Assert.AreEqual(ResourceStrings.INVALID_LATITUDE_INPUT_MESSAGE, resultObject.Value);
        }

        [TestMethod]
        public void Valid_Inputs_Test()
        {
            Dictionary<string, string> svcInputs = new Dictionary<string, string>()
            {
                { "longitude", "1" },
                { "latitude", "2" }
            };

            ListLogger logger = testHelper.CreateLogger(LoggerTypes.List) as ListLogger;
            var result = FindFoodTrucks.Run(testHelper.CreateHttpRequest(svcInputs), logger);

            Assert.AreEqual(1, logger.Logs.Count);
            Assert.AreEqual(ResourceStrings.REQUEST_STARTED_MESSAGE, logger.Logs.First());

            Assert.AreEqual(TaskStatus.RanToCompletion, result.Status);
            OkObjectResult resultObject = result.Result as OkObjectResult;

            Assert.AreEqual(200, resultObject.StatusCode);
            Assert.AreEqual(ResourceStrings.INVALID_LATITUDE_INPUT_MESSAGE, resultObject.Value);
        }
    }
}
