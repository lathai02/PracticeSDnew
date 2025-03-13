using Shares.Constants;
using Shares.ServiceContracts;
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
        private readonly IStudentProto _studentProto;

        public Controllers(IStudentProto studentProto)
        {
            _studentProto = studentProto;
        }

        public async Task ManageStudentAsync()
        {
            List<string> menuFeature = AppConstants.MENU_FEATURE;

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
                            await _studentProto.PrintStudentListAsync(new Empty());
                            break;
                        case 2:
                            await _studentProto.AddStudentAsync(new Empty());
                            Console.WriteLine("Student added successfully.");
                            break;
                        case 3:
                            await _studentProto.UpdateStudentAsync(new Empty());
                            break;
                        case 4:
                            await _studentProto.DeleteStudentAsync(new Empty());
                            break;
                        case 5:
                            await _studentProto.SortStudentListByNameAsync(new Empty());
                            break;
                        case 6:
                            await _studentProto.SearchByStudentIdAsync(new Empty());
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
