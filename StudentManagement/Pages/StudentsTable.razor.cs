using AntDesign;
using AutoMapper;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Web;
using Shares.Constants;
using Shares.Dtos;
using Shares.Models;
using StudentManagement.Services;
using System.ServiceModel.Channels;
using System.Threading.Tasks;
using StudentManagement.Pages.Components;

namespace StudentManagement.Pages
{
    public partial class StudentsTable : ComponentBase
    {
        [Inject] private StudentService _studentManager { get; set; } = default!;

        [Inject] private INotificationService _notificationService { get; set; } = default!;

        [Inject] IMapper _mapper { get; set; }

        private StudentFormComponent studentFormComponent;
        private List<Student> students = new();
        private List<StudentDto> studentDtos = new();
        private string? txtValue { get; set; }
        private int currentPage = 1;
        private int pageSize = AppConstants.PAGE_SIZE;
        private int totalStudents = 0;

        private string selectedSortOption = "Id";

        private List<SortOption> sortOptions = new List<SortOption>
        {
            new SortOption { Value = "Name", Label = "Sort by Name" },
            new SortOption { Value = "DateOfBirth", Label = "Sort by Date of Birth" },
            new SortOption { Value = "Id", Label = "Sort by Id" }
        };

        protected override async Task OnInitializedAsync()
        {
            await LoadDataAsync(1, selectedSortOption);
            totalStudents = await _studentManager.GetTotalCountAsync();
        }

        private async void OnSortOptionChanged(SortOption option)
        {
            selectedSortOption = option.Value;
            await LoadDataAsync(currentPage, selectedSortOption);
        }

        private async Task Handle(string value)
        {
            txtValue = value;

            if (string.IsNullOrEmpty(value))
            {
                await LoadDataAsync(currentPage, selectedSortOption); // Thêm currentSortField vào đây
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

        private void OpenStudentFormComponent()
        {
            studentFormComponent.Open();
        }

        protected async Task DeleteStudent(string id)
        {
            var res = await _studentManager.DeleteStudentAsync(id);

            await LoadDataAsync(currentPage, selectedSortOption); // Thêm currentSortField vào đây

            if (!string.IsNullOrEmpty(res))
            {
                await ShowNotification("Delete Student", res, NotificationType.Success);
            }
            else
            {
                await ShowNotification("Delete Student", "Failed to delete student", NotificationType.Error);
            }
        }


        private async Task LoadDataAsync(int currentPage, string sortField)
        {
            this.currentPage = currentPage;
            students = await _studentManager.GetStudentListAsync(this.currentPage, pageSize, sortField);

            // Kiểm tra nếu trang hiện tại không có dữ liệu và không phải là trang đầu tiên
            if (students.Count == 0 && this.currentPage > 1)
            {
                this.currentPage--;
                students = await _studentManager.GetStudentListAsync(this.currentPage, pageSize, sortField);
            }

            studentDtos = MapStudentsToDtos(students, this.currentPage, pageSize);
            totalStudents = await _studentManager.GetTotalCountAsync();
            StateHasChanged();
        }

        private async Task HandlePageChange(PaginationEventArgs args)
        {
            currentPage = args.Page;
            await LoadDataAsync(currentPage, selectedSortOption); // Truyền currentSortField thay vì isSortByName
        }


        private void UpdateStudent(StudentDto studentDto)
        {
            var student = _mapper.Map<Student>(studentDto);
            studentFormComponent.Open(student);
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
        public List<StudentDto> MapStudentsToDtos(List<Student> students, int pageNumber, int pageSize)
        {
            var studentDtos = _mapper.Map<List<StudentDto>>(students);

            int startNumber = (pageNumber - 1) * pageSize + 1;
            for (int i = 0; i < studentDtos.Count; i++)
            {
                studentDtos[i].Number = startNumber + i;
            }

            return studentDtos;
        }
    }
}
