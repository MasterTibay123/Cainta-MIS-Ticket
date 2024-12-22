using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Cainta_MIS_Ticket
{
    public static class UserSession
    {
        public static int UserId { get; set; }
        public static string Username { get; set; }
        public static string Usertype { get; set; }
        public static byte[] PictureData { get; set; }

        public static void ClearSession()
        {
            UserId = 0;
            Username = null;
            Usertype = null;
            PictureData = null;
        }
    }
}
