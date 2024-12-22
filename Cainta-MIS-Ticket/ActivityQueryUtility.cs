using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Cainta_MIS_Ticket
{
    public static class ActivityQueryUtility
    {
        public static string GetActivityQueryUtility()
        {
            return @"
                    SELECT
                        DATE(t.date) AS 'date',
                        t.office,
                        t.problem,
                        t.solution
                    FROM
                        ticket_technician t;
                    ";
        }
    }
}
