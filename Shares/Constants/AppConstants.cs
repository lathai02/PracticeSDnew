using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Shares.Constants
{
    public static class AppConstants
    {
        public const string DATE_FORMAT  = "dd/MM/yyyy";

        public const string STUDENT_ID_PARTERN = "^[A-Za-z]{2}\\d{6}$";

        public static readonly IReadOnlyList<string> MENU_FEATURE = new List<string>
        {
            "1.Show student list:",
            "2.Add Student:",
            "3.Update student by Id:",
            "4.Delete student by Id:",
            "5.Sort student list by name:",
            "6.Search by student id:",
            "7.Exit."
        };
    }
}
