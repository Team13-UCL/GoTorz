using Microsoft.Data.SqlClient;
using System.Collections.Generic;
using System.Data;
using System.Threading.Tasks;
using GoTorz.Model;

namespace GoTorz.Services
{
    public class PackageService
    {
        private readonly string _connectionString;

        public PackageService(string connectionString)
        {
            _connectionString = connectionString;
        }

        public async Task<List<Package>> GetPackagesAsync()
        {
            var packages = new List<Package>();

            using (var connection = new SqlConnection(_connectionString))
            {
                await connection.OpenAsync();

                using (var command = new SqlCommand("SELECT PackageId, PlaneId, HotelId FROM Package", connection))
                {
                    using (var reader = await command.ExecuteReaderAsync())
                    {
                        while (await reader.ReadAsync())
                        {
                            packages.Add(new Package
                            {
                                PackageId = reader.GetInt32(0),
                                PlaneId = reader.GetInt32(1),
                                HotelId = reader.GetInt32(2)
                            });
                        }
                    }
                }
            }

            return packages;
        }
    }
}