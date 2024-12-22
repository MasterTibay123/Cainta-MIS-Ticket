using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Cainta_MIS_Ticket
{
    public static class ReportQueryUtility
    {
        public static string GetReportQuery(string filterCondition = "")
        {
            string query = @"
        -- First Query: Select from ticket_technician and technician
        SELECT
            DATE(t.date) AS DATE,
            DATE_FORMAT(TIME(t.date), '%h:%i %p') AS TIME,
            t.office,
            tech.name,
            t.resolved,
            t.id
        FROM
            ticket_technician t
        LEFT JOIN
            technician tech ON t.technician_id = tech.technician_id ";

            if (!string.IsNullOrEmpty(filterCondition))
            {
                query += $"WHERE {filterCondition} ";
            }

            query += @"
        UNION ALL

        -- Second Query: Select from ticket_encoder and encoder
        SELECT
            DATE(te.date) AS DATE,
            DATE_FORMAT(TIME(te.date), '%h:%i %p') AS TIME,
            te.office,
            enc.name,
            te.resolved,
            te.id
        FROM
            ticket_encoder te
        LEFT JOIN
            encoder enc ON te.encoder_id = enc.encoder_id ";

            if (!string.IsNullOrEmpty(filterCondition))
            {
                query += $"WHERE {filterCondition} ";
            }

            return query;
        }
    }
}



