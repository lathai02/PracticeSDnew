﻿using Services.Interfaces;
using Shares.Constants;
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
                            await _studentService.PrintStudentListAsync();
                            break;
                        case 2:
                            await _studentService.AddStudentAsync();
                            break;
                        case 3:
                            await _studentService.UpdateStudentAsync();
                            break;
                        case 4:
                            await _studentService.DeleteStudentAsync();
                            break;
                        case 5:
                            await _studentService.SortStudentListByNameAsync();
                            break;
                        case 6:
                            await _studentService.SearchByStudentIdAsync();
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
