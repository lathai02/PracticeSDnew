using AntDesign.Charts;
using AutoMapper;
using Microsoft.AspNetCore.Components;
using Shares.Dtos;
using Shares.Enums;
using Shares.ServiceContracts;
using StudentManagement.Services;
using Title = AntDesign.Charts.Title;
using AntDesign;
using Blazor.ECharts.Components;
using NHibernate.Stat;
using Shares.Models;
using NHibernate.Mapping.ByCode.Impl;
using System.ComponentModel;
using OfficeOpenXml;
using LicenseContext = OfficeOpenXml.LicenseContext;
using Microsoft.JSInterop;
using OfficeOpenXml.Style;

namespace StudentManagement.Pages
{
    public partial class StatistcTeacherChart : ComponentBase
    {
        [Inject]
        private StudentService _studentManager { get; set; } = default!;
        [Inject]
        IMapper _mapper { get; set; }
        [Inject]
        private IJSRuntime JS { get; set; }

        protected List<TeacherStatistic>? statistics = new List<TeacherStatistic>();
        protected ColumnConfig? statisticConfig;
        List<ClassWithStudentsResponse> listClassWithStudentsResponse = new List<ClassWithStudentsResponse>();
        private string? txtValue { get; set; }

        private async Task GetChartDataAsync(string name)
        {
            TeacherResponseChart teacher = await _studentManager.GetTeacherByNameAsync(name);
            listClassWithStudentsResponse = _mapper.Map<List<ClassWithStudentsResponse>>(teacher);
            statistics = null;
            StateHasChanged();
            statistics = GetTeacherStatistic(listClassWithStudentsResponse);
            await Task.Delay(100);

            statisticConfig = new ColumnConfig
            {
                IsGroup = true,
                XField = "subject",
                YField = "value",
                YAxis = new ValueAxis() { Min = 0 },
                Label = new ColumnViewConfigLabel() { Visible = true },
                SeriesField = "type",
                Color = new string[] { "#1ca9e6", "#f88c24" },
            };

            StateHasChanged();
        }

        private List<TeacherStatistic> GetTeacherStatistic(List<ClassWithStudentsResponse> classWithStudentsResponse)
        {

            List<TeacherStatistic> res = new List<TeacherStatistic>();
            foreach (var item in classWithStudentsResponse)
            {
                var classStatistic = new TeacherStatistic
                {
                    Subject = item.SubjectName,
                    Type = StatisticType.NumberOfClasses.ToString(),
                    Value = item.Classes.Count
                };
                var studentStatistic = new TeacherStatistic
                {
                    Subject = item.SubjectName,
                    Type = StatisticType.NumberOfStudents.ToString(),
                    Value = item.Students.Count
                };
                res.Add(classStatistic);
                res.Add(studentStatistic);
            }

            return res;
        }

        private async Task Handle(string value)
        {
            txtValue = value;
            await GetChartDataAsync(txtValue);
        }

        private async Task ExportFileExcel()
        {
            // Lấy ngày và giờ hiện tại theo định dạng yyyyMMdd_HHmmss
            string currentDateTime = DateTime.Now.ToString("yyyyMMdd_HHmmss");

            // Gọi hàm JS để chọn thư mục
            var folderName = await JS.InvokeAsync<string>("selectFolder");

            if (!string.IsNullOrEmpty(folderName))
            {
                // Tạo tên file với ngày giờ
                var fileName = $"ThongKe_{currentDateTime}.xlsx";
                var filePath = Path.Combine(folderName, fileName);

                ExcelPackage.LicenseContext = LicenseContext.NonCommercial;
                using (var package = new ExcelPackage())
                {
                    var worksheet = package.Workbook.Worksheets.Add("Thống kê lớp, sinh viên");

                    // Title
                    worksheet.Cells[1, 1].Value = "Thống kê lớp, sinh viên đang giảng dạy";
                    worksheet.Cells[1, 1, 1, 6].Merge = true;
                    worksheet.Cells[1, 1].Style.Font.Size = 16;
                    worksheet.Cells[1, 1].Style.Font.Bold = true;
                    worksheet.Cells[1, 1].Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;

                    int currentRow = 3;

                    foreach (var data in listClassWithStudentsResponse)
                    {
                        // Subject Name
                        worksheet.Cells[currentRow, 1].Value = $"Tên môn học: {data.SubjectName}";
                        worksheet.Cells[currentRow, 1, currentRow, 6].Merge = true;
                        worksheet.Cells[currentRow, 1].Style.Font.Bold = true;
                        currentRow++;

                        // Header
                        string[] headers = { "Lớp", "Giáo viên", "Mã sinh viên", "Tên sinh viên", "Ngày sinh", "Địa chỉ" };
                        for (int i = 0; i < headers.Length; i++)
                        {
                            worksheet.Cells[currentRow, i + 1].Value = headers[i];
                            worksheet.Cells[currentRow, i + 1].Style.Font.Bold = true;
                            worksheet.Cells[currentRow, i + 1].Style.Border.BorderAround(ExcelBorderStyle.Thin);
                        }
                        currentRow++;

                        // Data
                        foreach (var classItem in data.Classes)
                        {
                            foreach (var student in classItem.Students)
                            {
                                worksheet.Cells[currentRow, 1].Value = classItem.Name;
                                worksheet.Cells[currentRow, 2].Value = classItem.TeacherName;
                                worksheet.Cells[currentRow, 3].Value = student.StudentId;
                                worksheet.Cells[currentRow, 4].Value = student.StudentName;
                                worksheet.Cells[currentRow, 5].Value = student.StudentDateOfBirth;
                                worksheet.Cells[currentRow, 6].Value = student.StudentAddress;
                                currentRow++;
                            }
                        }
                        currentRow++; // Tạo khoảng cách giữa các môn học
                    }

                    // Auto fit columns
                    worksheet.Cells[worksheet.Dimension.Address].AutoFitColumns();

                    // Save
                    File.WriteAllBytes(filePath, package.GetAsByteArray());
                    Console.WriteLine($"File saved at: {filePath}");
                }
            }
        }

    }
}
