using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data;
using System.Data.SqlClient;

namespace DeerImpact.Data
{
    public static class DataAccess
    {

        private const string CONN = "Server=tcp:tolu1.database.windows.net,1433;Initial Catalog = DeerImpactData; Persist Security Info=False;User ID = barkley.hughes; Password=dagger26@; MultipleActiveResultSets=False;Encrypt=True;TrustServerCertificate=False;Connection Timeout = 30";
        public static async Task InsertInteraction(DateTime timeStamp, decimal lat, decimal lon,
                                                    InteractionType type, decimal phoneNumber)
        {
            string cmdText = "csp_InsertInteraction";

            using(var sqlCon = new SqlConnection(CONN))
            {
                await sqlCon.OpenAsync();
                using (var cmd = new SqlCommand(cmdText, sqlCon))
                {
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.AddWithValue("@timeStamp", timeStamp);
                    cmd.Parameters.AddWithValue("@interactionType", type);
                    cmd.Parameters.AddWithValue("@lat", lat);
                    cmd.Parameters.AddWithValue("@lon", lon);
                    cmd.Parameters.AddWithValue("@phoneNumber", phoneNumber);

                    var result = await cmd.ExecuteNonQueryAsync();
                }
            }
        }

        public static async Task<List<Point>> GetNearByInteractions(DateTime currentDate, int range,
            decimal currentLat, decimal currentLon)
        {
            List<Point> points = new List<Point>();
            string cmdText = "csp_GetNearByInteractions";

            using (var sqlCon = new SqlConnection(CONN))
            {
                await sqlCon.OpenAsync();
                
                using (var cmd = new SqlCommand(cmdText, sqlCon))
                {
                    cmd.CommandTimeout = 10000;
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.AddWithValue("@timeStamp", currentDate);
                    cmd.Parameters.AddWithValue("@range", range);
                    cmd.Parameters.AddWithValue("@currentLat", currentLat);
                    cmd.Parameters.AddWithValue("@currentLon", currentLon);


                    var rdr = await cmd.ExecuteReaderAsync();

                    if (rdr.HasRows)
                    {
                        int latOrdinal = rdr.GetOrdinal("Lat");
                        int lonOrdinal = rdr.GetOrdinal("Lon");
                        int feetOrdinal = rdr.GetOrdinal("Feet");
                        while (await rdr.ReadAsync())
                        {
                            Point newOne = new Point
                            {
                                Lat = rdr.IsDBNull(latOrdinal) ? 0.00 : rdr.GetDouble(latOrdinal),
                                Long = rdr.IsDBNull(lonOrdinal) ? 0.00 : rdr.GetDouble(lonOrdinal),
                                Feet = rdr.IsDBNull(feetOrdinal) ? 0 : rdr.GetInt32(feetOrdinal)
                            };
                            points.Add(newOne);
                        }
                    }

                }
            }

            return points;
        }

        public static async Task<List<Point>> GetNearByInteractions( int range,
            decimal currentLat, decimal currentLon)
        {
            List<Point> points = new List<Point>();
            string cmdText = "csp_GetNearByInteractions";

            using (var sqlCon = new SqlConnection(CONN))
            {
                await sqlCon.OpenAsync();

                using (var cmd = new SqlCommand(cmdText, sqlCon))
                {
                    cmd.CommandTimeout = 10000;
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.AddWithValue("@range", range);
                    cmd.Parameters.AddWithValue("@currentLat", currentLat);
                    cmd.Parameters.AddWithValue("@currentLon", currentLon);


                    var rdr = await cmd.ExecuteReaderAsync();

                    if (rdr.HasRows)
                    {
                        int latOrdinal = rdr.GetOrdinal("Lat");
                        int lonOrdinal = rdr.GetOrdinal("Lon");
                        int feetOrdinal = rdr.GetOrdinal("Feet");
                        while (await rdr.ReadAsync())
                        {
                            Point newOne = new Point
                            {
                                Lat = rdr.IsDBNull(latOrdinal) ? 0.00 : rdr.GetDouble(latOrdinal),
                                Long = rdr.IsDBNull(lonOrdinal) ? 0.00 : rdr.GetDouble(lonOrdinal),
                                Feet = rdr.IsDBNull(feetOrdinal) ? 0 : rdr.GetDouble(feetOrdinal)
                            };
                            points.Add(newOne);
                        }

                    }

                }
            }

            return points;
        }


    }
}
