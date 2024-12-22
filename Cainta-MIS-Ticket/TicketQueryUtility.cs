using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Cainta_MIS_Ticket
{
    public static class TicketQueryUtility
    {
        public static string GetTicketTechQuery()
        {
            return @"
            SELECT
                t.id,
                DATE(t.date) AS DATE,
                t.office,
                t.caller,
                t.problem,
                t.solution,
                tech.image,
                t.recommendation,
                t.resolved,
                tech.name
            FROM
                ticket_technician t
            LEFT JOIN
                technician tech ON t.technician_id = tech.technician_id
        ";
        }

        public static string GetTicketTechDetailsQuery()
        {
            return @"
                    SELECT
                        t.id,
                        DATE(t.date) AS DATE,
                        t.office,
                        t.caller,
                        t.problem,
                        t.solution,
                        tech.name,
                        t.recommendation,
                        t.resolved
                    FROM
                        ticket_technician t
                    LEFT JOIN
                        technician tech ON t.technician_id = tech.technician_id
                    WHERE
                        t.id = @idNo;
                    ";
        }

        public static string GetTicketEncoderQuery()
        {
            return @"
        SELECT
            te.id,
            DATE(te.date) AS DATE,
            enc.image,
            te.office,
            te.resolved,
            enc.name
        FROM
            ticket_encoder te
        LEFT JOIN
            encoder enc ON te.encoder_id = enc.encoder_id
        ";
        }

        public static string GetTicketEncoderDetailsQuery()
        {
            return @"
                    SELECT
                        te.id,
                        DATE(te.date) AS DATE,
                        enc.image,
                        te.office,
                        te.resolved,
                        enc.name
                    FROM
                        ticket_encoder te
                    LEFT JOIN
                        encoder enc ON te.encoder_id = enc.encoder_id
                    WHERE
                        te.id = @idNo;
                    ";
        }
    }
}
