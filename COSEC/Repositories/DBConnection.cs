using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace COSEC.Repositories
{
    public class DBConnection
    {

        public static string Connection = "Server=DESKTOP-ILIIGRJ\\SQLEXPRESS;Database=COSEC;Trusted_Connection=True;user id=sa;password=SQL@15@dc;";

        public static string Conn { get => Connection; }
    }
}
