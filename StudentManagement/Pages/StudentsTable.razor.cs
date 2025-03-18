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
        public StudentService _studentManager { get; set; } = default!;

        private List<Student> students = new();
        private List<Class> classList = new();

        private string? messageDelete = null;
        private string? messageAdd = null;

        private string studentId;
        private string studentName;
        private DateTime? studentDob;
        private string studentAddress;
        private Class? selectedClass;

        private bool isSubmitted = false;
        private string? studentIdError;
        private int selectedClassId;
        private bool visible;
        private bool isEditMode = false;
        private string txtValue { get; set; }

        private int currentPage = 1;
        private int pageSize = 5;
        private int totalStudents = 0;

        protected override async Task OnInitializedAsync()
        {
            await LoadStudents();
            classList = await _studentManager.GetAllClass();
            selectedClass = classList[0];
            totalStudents = await _studentManager.GetTotalCountAsync();
        }

        private async Task HandlePageChange(PaginationEventArgs args)
        {
            currentPage = args.Page;
            students = await _studentManager.GetStudentList(currentPage, pageSize);
        }

        private async Task Handle(string value)
        {
            txtValue = value;
            var student = await _studentManager.GetStudentByIdAsync(value);
            var listStudentSearch = new List<Student>();
            if (student != null)
            {
                listStudentSearch.Add(student);
            }
            students = listStudentSearch;

            totalStudents = await _studentManager.GetTotalCountAsync();
            StateHasChanged();
        }

        private void Close() => visible = false;

        private void Open()
        {
            ResetForm();
            isEditMode = false;
            visible = true;
        }

        private async Task SortStudentByName()
        {
            students = await _studentManager.GetStudentList(1, pageSize, true);
            StateHasChanged();
        }

        private void Open(Student student)
        {
            studentId = student.Id;
            studentName = student.Name;
            studentAddress = student.Address;
            studentDob = student.DateOfBirth;
            selectedClassId = student.Class?.Id ?? 0;
            selectedClass = classList.FirstOrDefault(c => c.Id == selectedClassId);

            isEditMode = true;
            visible = true;
        }

        private void ResetForm()
        {
            studentId = string.Empty;
            studentName = string.Empty;
            studentAddress = string.Empty;
            studentDob = null;
            selectedClassId = 0;
            messageAdd = null;
            isSubmitted = false;
            studentIdError = string.Empty;
        }

        protected void UpdateStudent(Student student, Class c)
        {
            Open(student);
        }

        private void OnSelectedItemChangedHandler(Class value)
        {
            selectedClass = value;
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
                Id = studentId,
                Name = studentName,
                Address = studentAddress,
                DateOfBirth = studentDob ?? default,
                Class = classList.FirstOrDefault(c => c.Id == selectedClassId)
            };

            if (isEditMode)
            {
                messageAdd = await _studentManager.AddOrUpdateStudent(s, true);
            }
            else
            {
                messageAdd = await _studentManager.AddOrUpdateStudent(s);
            }
            totalStudents = await _studentManager.GetTotalCountAsync();

            await LoadStudents();
            StateHasChanged();
            Close();
            currentPage = 1;
        }

        private async Task LoadStudents()
        {
            students = await _studentManager.GetStudentList(1, pageSize);
        }

        protected async Task DeleteStudent(string id)
        {
            var res = await _studentManager.DeleteStudent(id);
            if (!string.IsNullOrEmpty(res))
            {
                messageDelete = res;
                StateHasChanged();
            }
            students = await _studentManager.GetStudentList(1, pageSize);
            totalStudents = await _studentManager.GetTotalCountAsync();
            currentPage = 1;
        }

        private bool IsFormInvalid()
        {
            return string.IsNullOrEmpty(studentId) ||
                   string.IsNullOrEmpty(studentName) ||
                   studentDob == default ||
                   string.IsNullOrEmpty(studentAddress) ||
                   selectedClassId == 0;
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
    }
}
