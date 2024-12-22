using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Cainta_MIS_Ticket
{
    public static class FilesQueryUtility
    {
        public static string GetFilesInQuery()
        {
            return @"
                SELECT
                     DATE(fi.date) AS DATE,
                     fi.department,
                     fi.items,
                     fi.description,
                     fi.recieved
                 FROM
                     filesin fi
                 ";
        }


        public static string GetFilesOutQuery()
        {
            return @"
                    SELECT
                        DATE(fo.date) AS DATE,
                        fo.department,
                        fo.items,
                        fo.description
                    FROM
                        filesout fo
                    ";
        }
    }
}
