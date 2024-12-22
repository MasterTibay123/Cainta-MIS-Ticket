using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Cainta_MIS_Ticket
{
    public static class AccountQueryUtility
    {
        public static string GetAccountQueryUtility()
        {
            return @"
                    SELECT
                        u.id AS `NO.`,
                        u.username AS USERNAME,
                        u.email AS EMAIL
                    FROM
                        users u;
                    ";
        }
    }
}
