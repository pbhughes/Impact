using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Threading.Tasks;
using System.Collections.Generic;
using DeerImpact.Data;

namespace Test.DataAccess
{
    [TestClass]
    public class TestInteractions
    {
        [TestMethod]
        public async Task TestInsert()
        {
            decimal lat = 37.335333M;
            decimal lon = -88.078681M;

            await DeerImpact.Data.DataAccess.InsertInteraction(DateTime.Now,
                lat, lon, DeerImpact.Data.InteractionType.Siting, 2707049633);
        }

        [TestMethod]
        public async Task TestGetNearBy()
        {
            decimal lat = 37.335727M;
            decimal lon = -88.078985M;
            int range = 500;

            List<Point> points = await DeerImpact.Data.DataAccess.GetNearByInteractions(range,lat, lon);

            if (range == 500)
                Assert.IsTrue(points.Count > 0);
        }
    }
}
