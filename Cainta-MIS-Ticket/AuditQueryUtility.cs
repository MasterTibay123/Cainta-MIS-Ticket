using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Cainta_MIS_Ticket
{
    public static class AuditQueryUtility
    {
        public static string GetAuditQuery()
        {
            return @"
                   SELECT
                        a.id,
	                    DATE(a.log_date) AS DATE,
                        DATE_FORMAT(TIME(a.log_time), '%h:%i %p') AS TIME,
	                    a.username AS USERNAME,
                        a.activity AS ACTIVITY,
                        a.code AS CODE
                    FROM
                        audit_trail a
                    ";
        }
    }
}
