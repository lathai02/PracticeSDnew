using Services.Interfaces;
using Shares.Utils;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Controllers
{
    public class Controllers
    {
        private readonly IStudentService _studentService;

        public Controllers(IStudentService studentService)
        {
            _studentService = studentService;
        }

        public void ManageStudent()
        {
            List<string> menuFeature = new List<string> {
                "1.Show student list:",
                "2.Add Student:",
                "3.Update student by Id:",
                "4.Delete student by Id:",
                "5.Sort student list by name:",
                "6.Search by student id:",
                "7.Exit." };

            bool exitFlag = false;

            do
            {
                if (exitFlag)
                {
                    break;
                }

                StringUtils.PrintList(menuFeature, "MENU");

                try
                {
                    int chooseNum = 0;
                    chooseNum = NumberUtils.InputIntegerNumber("Please choose a feature by number:", 1, 7);

                    switch (chooseNum)
                    {
                        case 1:
                            _studentService.PrintStudentList();
                            break;
                        case 2:
                            _studentService.AddStudent();
                            break;
                        case 3:
                            _studentService.UpdateStudent();
                            break;
                        case 4:
                            _studentService.DeleteStudent();
                            break;
                        case 5:
                            _studentService.SortStudentListByName();
                            break;
                        case 6:
                            _studentService.SearchByStudentId();
                            break;
                        case 7:
                            exitFlag = true;
                            break;
                    }

                    Console.WriteLine();
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"Message: {ex.Message}");
                }
            } while (true);
        }
    }
}
