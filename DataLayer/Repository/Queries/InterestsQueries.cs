using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Text;

namespace DataLayer.Repository.Queries
{
    public static class InterestsQueries
    {
        public static SqlCommand GetSuggestedInterests(SqlConnection connection, int userId)
        {
            SqlCommand sqlCommand = connection.CreateCommand();

            string mainQuery = $@"SELECT ";

            var query = $@";WITH PG AS (
                            {mainQuery}
                        )
                        SELECT PG.Id as WindowsUpdateId,
                               PG.KB,
                               PG.Title,
                               PG.Products,
                               PG.Description,
                               PG.SecurityBulletin,
                               PG.Classifications,
                               PG.Severity,
                               PG.Reboot,
                               PG.ReleaseDate,
                               PG.Cve,
                               PG.Cvss,
                               PG.[NumberOfDevices],
                               PG.[NumberOfServers],
                               PG.TotalRows
                        FROM PG";

            sqlCommand.CommandText = query;
            sqlCommand.Parameters.AddWithValue("@isRevoked", 0);

            return sqlCommand;
        }
    }
}