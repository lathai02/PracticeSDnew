using AutoMapper;
using NHibernate.Mapping.ByCode.Impl;
using Shares.Constants;
using Shares.Models;
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
        private readonly StudentManager _studentManager;

        public Controllers(StudentManager studentManager)
        {
            _studentManager = studentManager;
        }

        public async Task ManageStudentAsync()
        {
            List<string> menuFeature = (List<string>)AppConstants.MENU_FEATURE;
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
                    int chooseNum = NumberUtils.InputIntegerNumber("Please choose a feature by number:", 1, 7);

                    switch (chooseNum)
                    {
                        case 1:
                            await _studentManager.DisplayStudentList();
                            break;
                        case 2:
                            await _studentManager.AddOrUpdateStudent();
                            break;
                        case 3:
                            await _studentManager.AddOrUpdateStudent(true);
                            break;
                        case 4:
                            await _studentManager.DeleteStudent();
                            break;
                        case 5:
                            await _studentManager.DisplayStudentList(true);
                            break;
                        case 6:
                            await _studentManager.SearchStudentById();
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
