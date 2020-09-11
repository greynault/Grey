using System;
using System.IO;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Extensions.Http;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using grey.foodtruck.api.Entities;
using grey.foodtruck.api.Assets;
using System.Xml;
using System.ServiceModel.Syndication;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using grey.foodtruck.utilities;

namespace grey.foodtruck.api
{
    public static class FindFoodTrucks
    {
        public static IEnumerable<FoodTruckItem> FoodTrucks { get; set; }
        [FunctionName("FindFoodTrucks")]
        public static async Task<IActionResult> Run(
            [HttpTrigger(AuthorizationLevel.Anonymous, "get", "post", Route = null)] HttpRequest req,
            ILogger log)
        {
            log.LogInformation(ResourceStrings.REQUEST_STARTED_MESSAGE);

            string longitude = req.Query["longitude"];
            string latitude = req.Query["latitude"];

            string requestBody = await new StreamReader(req.Body).ReadToEndAsync();
            dynamic data = JsonConvert.DeserializeObject(requestBody);
            longitude = longitude ?? data?.longitude;
            latitude = latitude ?? data?.latitude;

            GenericReturn validationResult = ValidateInputs(latitude, longitude);
            if (!validationResult.Success)
            {
                return new OkObjectResult(validationResult.Message);
            }

            ReloadFoodTrucks();

            //move 5 to configuration, and then eventually an optional overriding input
            IEnumerable<FoodTruckItem> closestFoodTrucks = GetClosestItems((PointF)validationResult.ReturnValue, 5);

            string responseMessage = JsonConvert.SerializeObject(closestFoodTrucks);

            return new OkObjectResult(responseMessage);
        }

        private static void ReloadFoodTrucks()
        {
            if (ShouldPopulateFoodTrucks())
            {
                List<FoodTruckItem> newFoodTruckItems = new List<FoodTruckItem>();

                //Move to configuration
                string url = "https://data.sfgov.org/api/views/rqzj-sfat/rows.rss";
                using (XmlReader reader = XmlReader.Create(url))
                {
                    SyndicationFeed feed = SyndicationFeed.Load(reader);
                    foreach (SyndicationItem item in feed.Items)
                    {
                        HtmlParserUtility parser = new HtmlParserUtility((item.Content as TextSyndicationContent).Text);

                        string expirationDate = parser.GetValueForKey("ExpirationDate");
                        if (!string.IsNullOrWhiteSpace(expirationDate) && DateTime.TryParse(expirationDate, out DateTime expirationDateObject))
                        {
                            if (expirationDateObject <= DateTime.Now)
                            {
                                continue;
                            }
                        }

                        PointF location = parser.GetPointForKey("Location");
                        string foodTruckName = parser.GetValueForKey("Applicant");
                        if (!location.IsEmpty && !newFoodTruckItems.Any(f => string.Equals(f.Name, foodTruckName, StringComparison.OrdinalIgnoreCase)))
                        {
                            newFoodTruckItems.Add(new FoodTruckItem
                            {
                                Location = location,
                                Name = foodTruckName
                            });
                        }
                    }
                }

                FoodTrucks = newFoodTruckItems;
            }
        }

        private static bool ShouldPopulateFoodTrucks()
        {
            if (FoodTrucks == null || !FoodTrucks.Any())
            {
                return true;
            }

            return false;
        }

        private static GenericReturn ValidateInputs(string latitude, string longitude)
        {
            GenericReturn returnValue = new GenericReturn();
            if (string.IsNullOrWhiteSpace(latitude) || string.IsNullOrWhiteSpace(longitude))
            {
                returnValue.Message = ResourceStrings.MISSING_COORDINATE_INPUT_MESSAGE;
                return returnValue;
            }

            if (!float.TryParse(latitude, out float Latitude))
            {
                returnValue.Message = ResourceStrings.INVALID_LATITUDE_INPUT_MESSAGE;
                return returnValue;
            }

            if (!float.TryParse(longitude, out float Longitude))
            {
                returnValue.Message = ResourceStrings.INVALID_LONGITUDE_INPUT_MESSAGE;
                return returnValue;
            }

            returnValue.ReturnValue = new PointF(Latitude, Longitude);
            returnValue.Success = true;
            return returnValue;
        }

        public static IEnumerable<FoodTruckItem> GetClosestItems(PointF query, int numberToReturn)
        {
            return FoodTrucks.OrderBy(x => CalculateDistance(query, x.Location)).Take(numberToReturn);
        }

        public static double CalculateDistance(PointF pt1, PointF pt2)
        {
            return Math.Sqrt((pt2.Y - pt1.Y) * (pt2.Y - pt1.Y) + (pt2.X - pt1.X) * (pt2.X - pt1.X));
        }
    }
}
