using DeerImpact.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web.Http;

namespace deer.bargedata.com.Controllers
{
    public class InteractionsController : ApiController
    {
        [HttpPost]
        public async Task<HttpResponseMessage> GetNearByInteractions( int range, decimal lat, decimal lon)
        {
            List<Point> points = await GetInteractions(range, lat, lon);
            return Request.CreateResponse<List<Point>>(HttpStatusCode.OK, points);
            
        }

        [HttpPost]
        public async Task<HttpResponseMessage> RecordInteraction( decimal lat, decimal lon,
            InteractionType type, decimal phoneNumber, decimal currentLat, decimal currentLon)
        {
            await DeerImpact.Data.DataAccess.InsertInteraction(DateTime.Now, lat, lon, type, phoneNumber);
            List<Point> points = await GetInteractions(5280, currentLat, currentLon);
            return Request.CreateResponse<List<Point>>(HttpStatusCode.OK, points);

        }

        [HttpGet]
        public HttpResponseMessage Test()
        {
            return Request.CreateResponse(HttpStatusCode.OK, "We Are Good");
        }

        private async Task<List<Point>> GetInteractions(int range, decimal lat, decimal lon)
        {
            List<Point> points = await DeerImpact.Data.DataAccess.GetNearByInteractions(range, lat, lon);

            return points;
        }
    }
}
