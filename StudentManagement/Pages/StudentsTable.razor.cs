using AntDesign;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Web;
using Shares.Constants;
using Shares.Models;
using StudentManagement.Services;
using System.ServiceModel.Channels;
using System.Threading.Tasks;

namespace StudentManagement.Pages
{
    public partial class StudentsTable : ComponentBase
    {
        [Inject]
        private StudentService _studentManager { get; set; } = default!;

        [Inject]
        private NotificationService _notificationService { get; set; } = default!;

        private List<Student> students = new();
        private List<Class> classList = new();

        private string? studentId;
        private string? studentName;
        private DateTime? studentDob;
        private string? studentAddress;
        private Class? selectedClass;

        private bool isSubmitted = false;
        private string? studentIdError;
        private int selectedClassId;
        private bool visible;
        private bool isEditMode = false;
        private string? txtValue { get; set; }

        private int currentPage = 1;
        private int pageSize = AppConstants.PAGE_SIZE;
        private int totalStudents = 0;
        private bool isSortByName { get; set; } = false;

        protected override async Task OnInitializedAsync()
        {
            await GetStudentCurrentPage();
            classList = await _studentManager.GetAllClassesAsync();
            selectedClass = classList[0];
            totalStudents = await _studentManager.GetTotalCountAsync();
        }

        private async Task Handle(string value)
        {
            txtValue = value;

            if (string.IsNullOrEmpty(value))
            {
                await GetStudentCurrentPage(currentPage);
                StateHasChanged();
                return;
            }

            var student = await _studentManager.GetStudentByIdAsync(value);
            var listStudentSearch = new List<Student>();
            if (student != null)
            {
                listStudentSearch.Add(student);
            }
            students = listStudentSearch;

            totalStudents = students.Count;

            StateHasChanged();
        }

        private async Task OnSortByNameChanged()
        {
            await GetStudentCurrentPage(currentPage, isSortByName);
        }

        private async Task SubmitForm()
        {
            isSubmitted = true;

            if (IsFormInvalid())
            {
                return;
            }

            Student s = new Student
            {
                Id = studentId ?? "",
                Name = studentName ?? "",
                Address = studentAddress ?? "",
                DateOfBirth = studentDob ?? default,
                Class = classList.FirstOrDefault(c => c.Id == selectedClassId)
            };

            string message;
            if (isEditMode)
            {
                message = await _studentManager.AddOrUpdateStudentAsync(s, true);

            }
            else
            {
                message = await _studentManager.AddOrUpdateStudentAsync(s);
            }
            await GetStudentCurrentPage(currentPage);

            Close();          
            await ShowNotification(message, isEditMode ? "Update Student" : "Add student");
        }

        protected async Task DeleteStudent(string id)
        {
            var res = await _studentManager.DeleteStudentAsync(id);

            await GetStudentCurrentPage(currentPage);

            if (!string.IsNullOrEmpty(res))
            {
                await ShowNotification("Delete Student", res, NotificationType.Success);
            }
            else
            {
                await ShowNotification("Delete Student", "Failed to delete student", NotificationType.Error);
            }
        }

        private async Task GetStudentCurrentPage(int currentPage = 1, bool isSortByName = false)
        {
            this.currentPage = currentPage;
            students = await _studentManager.GetStudentListAsync(currentPage, pageSize, isSortByName);
            totalStudents = await _studentManager.GetTotalCountAsync();
            StateHasChanged();
        }

        private void SetStudentDetails(Student student)
        {
            studentId = student.Id;
            studentName = student.Name;
            studentAddress = student.Address;
            studentDob = student.DateOfBirth;
            selectedClassId = student.Class?.Id ?? 0;
            selectedClass = classList.FirstOrDefault(c => c.Id == selectedClassId);
        }

        private void Close() => visible = false;

        private async Task HandlePageChange(PaginationEventArgs args)
        {
            currentPage = args.Page;
            await GetStudentCurrentPage(currentPage, isSortByName);
        }

        private void ResetForm()
        {
            studentId = string.Empty;
            studentName = string.Empty;
            studentAddress = string.Empty;
            studentDob = null;
            selectedClassId = 0;
            isSubmitted = false;
            studentIdError = string.Empty;
        }

        private void Open(Student student)
        {
            SetStudentDetails(student);
            isEditMode = true;
            visible = true;
        }

        private void Open()
        {
            ResetForm();
            isEditMode = false;
            visible = true;
        }

        private bool IsFormInvalid()
        {
            return string.IsNullOrEmpty(studentId) ||
                   string.IsNullOrEmpty(studentName) ||
                   studentDob == default ||
                   string.IsNullOrEmpty(studentAddress) ||
                   selectedClassId == 0;
        }

        private void UpdateStudent(Student student)
        {
            Open(student);
        }

        private void OnSelectedItemChangedHandler(Class value)
        {
            selectedClass = value;
        }

        private EventCallback<string> OnStudentIdChange => EventCallback.Factory.Create<string>(this, ValidateStudentId);

        private void ValidateStudentId(string value)
        {
            studentId = value;
            studentIdError = System.Text.RegularExpressions.Regex.IsMatch(studentId, AppConstants.STUDENT_ID_PARTERN)
                ? null
                : "Student ID không hợp lệ. Định dạng: AA123456.";
        }

        private EventCallback<DateTimeChangedEventArgs<DateTime?>> OnStudentDobChange =>
                EventCallback.Factory.Create<DateTimeChangedEventArgs<DateTime?>>(this, OnDobChange);

        private void OnDobChange(DateTimeChangedEventArgs<DateTime?> args)
        {
            studentDob = args.Date;
        }

        protected async Task ShowNotification(string title, string message, NotificationType type = NotificationType.Success)
        {
            Console.WriteLine($"ShowNotification called: {title} - {message}");
            if (_notificationService == null)
            {
                Console.WriteLine("NotificationService is null!");
                return;
            }

            await _notificationService.Open(new NotificationConfig()
            {
                Message = title,
                Description = message,
                NotificationType = type,
                Duration = 4
            });
        }
    }
}
