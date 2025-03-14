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
        private readonly IStudentProto _studentProto;
        private readonly IClassProto _classProto;
        private readonly IMapper _mapper;

        public Controllers(IStudentProto studentProto, IClassProto classProto, IMapper mapper)
        {
            _studentProto = studentProto;
            _classProto = classProto;
            _mapper = mapper;
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
                    int chooseNum = 0;
                    chooseNum = NumberUtils.InputIntegerNumber("Please choose a feature by number:", 1, 7);

                    switch (chooseNum)
                    {
                        case 1:
                            var students = await _studentProto.PrintStudentListAsync(new Empty());
                            StringUtils.PrintList(students.Students, "Student List");
                            break;
                        case 2:
                            var studentAddId = StringUtils.InputString("Enter student id:", AppConstants.STUDENT_ID_PARTERN);
                            var studentName = StringUtils.InputString("Enter student name:");
                            var studentAddress = StringUtils.InputString("Enter student address:");
                            var studentDob = DateTimeUtils.InputDateTime($"Enter student dob ({AppConstants.DATE_FORMAT}): ");

                            var classResponse = await _classProto.GetAllClassWithTeacherAsync(new Empty());
                            StringUtils.PrintList(classResponse.Classes, "Class List");

                            ClassRequest AddClassRequest = new ClassRequest
                            {
                                ClassId = NumberUtils.InputIntegerNumber("Enter class id: ", 1, classResponse.Classes.Count)
                            };

                            var addClass = await _classProto.GetClassByIdAsync(AddClassRequest);

                            Student stu = new Student
                            {
                                Id = studentAddId,
                                Name = studentName,
                                Address = studentAddress,
                                DateOfBirth = studentDob,
                                Class = _mapper.Map<Class>(addClass)
                            };

                            RequestStudentAdd addStudentRequest = _mapper.Map<RequestStudentAdd>(stu);

                            await _studentProto.AddStudentAsync(addStudentRequest);
                            Console.WriteLine("Student added successfully.");
                            break;
                        case 3:
                            var studentUpdateId = StringUtils.InputString("Enter student id to update:", AppConstants.STUDENT_ID_PARTERN);
                            RequestStudent UpdateStudentRequest = new RequestStudent
                            {
                                StudentId = studentUpdateId
                            };
                            var student = await _studentProto.SearchByStudentIdAsync(UpdateStudentRequest);
                            if (student == null)
                            {
                                Console.WriteLine("Student not found!");
                                return;
                            }

                            var studentNameUpdate = StringUtils.InputString("Enter student name:");
                            var studentAddressUpdate = StringUtils.InputString("Enter student address:");
                            var studentDobUpdate = DateTimeUtils.InputDateTime($"Enter student dob ({AppConstants.DATE_FORMAT}): ");

                            var classResponseUpdate = await _classProto.GetAllClassWithTeacherAsync(new Empty());
                            StringUtils.PrintList(classResponseUpdate.Classes, "Class List");

                            ClassRequest UpdateClassRequest = new ClassRequest
                            {
                                ClassId = NumberUtils.InputIntegerNumber("Enter class id: ", 1, classResponseUpdate.Classes.Count)
                            };

                            var updateClass = await _classProto.GetClassByIdAsync(UpdateClassRequest);

                            Student stuUpdate = new Student
                            {
                                Id = studentUpdateId,
                                Name = studentNameUpdate,
                                Address = studentAddressUpdate,
                                DateOfBirth = studentDobUpdate,
                                Class = _mapper.Map<Class>(updateClass)
                            };

                            RequestStudentAdd updateStudentRequest = _mapper.Map<RequestStudentAdd>(stuUpdate);

                            await _studentProto.UpdateStudentAsync(updateStudentRequest);
                            break;
                        case 4:
                            RequestStudent deleteStudentRequest = new RequestStudent
                            {
                                StudentId = StringUtils.InputString("Enter student id to delete:", AppConstants.STUDENT_ID_PARTERN)
                            };

                            await _studentProto.DeleteStudentAsync(deleteStudentRequest);
                            Console.WriteLine("Student deleted successfully.");
                            break;
                        case 5:
                            var studentSortByNameList = await _studentProto.SortStudentListByNameAsync(new Empty());
                            StringUtils.PrintList(studentSortByNameList.Students, "Student List");
                            break;
                        case 6:
                            RequestStudent SearchStudentRequest = new RequestStudent
                            {
                                StudentId = StringUtils.InputString("Enter student id to search:", AppConstants.STUDENT_ID_PARTERN)
                            };

                            var studentSearchResponse = await _studentProto.SearchByStudentIdAsync(SearchStudentRequest);
                            if (studentSearchResponse == null)
                            {
                                Console.WriteLine("Student not found!");
                                return;
                            }
                            Console.WriteLine(studentSearchResponse.ToString());
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
