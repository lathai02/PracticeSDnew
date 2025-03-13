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

        public static readonly List<string> MENU_FEATURE = new List<string>
        {
            "1.Show student list:",
            "2.AddAsync Student:",
            "3.UpdateAsync student by Id:",
            "4.DeleteAsync student by Id:",
            "5.Sort student list by name:",
            "6.Search by student id:",
            "7.Exit."
        };
    }
}
