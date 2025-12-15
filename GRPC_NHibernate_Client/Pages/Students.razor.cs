using GRPC_NHibernate_Client.Services;
using Microsoft.AspNetCore.Components;
using Microsoft.JSInterop;
using Shared.DTOs.RequestModel;
using Shared.DTOs.ResponseModel;

public class StudentsBase : ComponentBase
{
    [Inject] public StudentApiClient StudentService { get; set; }
    [Inject] public ClassRoomApiClient ClassService { get; set; }
    [Inject] public TeacherApiClient TeacherService { get; set; }
    [Inject] public IJSRuntime JS { get; set; }

    protected List<StudentResponse> students = new();
    protected List<ClassResponse> classRooms = new();
    protected List<TeacherResponse> teachers = new();
    protected List<StudentResponse> filteredStudents =>
        students.Where(s =>
            string.IsNullOrWhiteSpace(searchTerm) ||
            s.Code.Contains(searchTerm, StringComparison.OrdinalIgnoreCase) ||
            s.FullName.Contains(searchTerm, StringComparison.OrdinalIgnoreCase)
        ).ToList();

    protected string searchTerm = "";
    protected string code = "";
    protected string fullName = "";
    protected string address = "";
    protected DateTime? birthDate = DateTime.Now;
    protected int selectedClassRoomId = 0;

    protected string teacherCode = "";
    protected string teacherFullName = "";
    protected DateTime? teacherBirthDate = DateTime.Now;

    protected string codeClass = "";
    protected string newClassName = "";
    protected string subjectClass = "";
    protected int selectedTeacherId = 0;

    // UI state
    protected bool sortAsc = true;
    protected bool showCreateClassModal = false;
    protected bool showTeacherListModal = false;
    protected bool showClassListModal = false;
    protected bool showCreateTeacherModal = false;

    protected StudentResponse editingStudent = null;

    protected override async Task OnInitializedAsync()
    {
        await LoadStudents();
        await LoadClassRooms();
        await LoadTeachers();
    }

    protected async Task LoadStudents()
    {
        students = await StudentService.GetStudents();
        students = students.OrderBy(s => GetLastName(s.FullName)).ToList();
    }

    protected async Task LoadClassRooms()
    {
        classRooms = await ClassService.GetAll();
    }

    protected async Task LoadTeachers()
    {
        teachers = await TeacherService.GetAll();
    }

    protected async Task SaveStudent()
    {
        if (string.IsNullOrWhiteSpace(fullName)) return;

        var req = new StudentRequest
        {
            Id = editingStudent?.Id ?? 0,
            Code = code,
            FullName = fullName,
            Address = address,
            BirthDate = birthDate ?? DateTime.Now,
            ClassRoomId = selectedClassRoomId
        };

        if (editingStudent == null)
            await StudentService.AddStudent(req);
        else
            await StudentService.UpdateStudent(req);

        editingStudent = null;

        code = "";
        fullName = "";
        address = "";
        birthDate = DateTime.Now;
        selectedClassRoomId = 0;

        await LoadStudents();
    }

    protected async Task DeleteStudent(int id)
    {
        if (await JS.InvokeAsync<bool>("confirm", $"Xóa sinh viên ID {id}?"))
        {
            await StudentService.DeleteStudent(new IdRequest { Id = id });
            await LoadStudents();
        }
    }

    protected void EditStudent(StudentResponse s)
    {
        editingStudent = s;
        code = s.Code;
        fullName = s.FullName;
        address = s.Address;
        birthDate = s.BirthDate ?? DateTime.Now;
        selectedClassRoomId = s.ClassRoom?.Id ?? 0;
    }

    protected void CancelEdit()
    {
        editingStudent = null;
        fullName = "";
        address = "";
        birthDate = DateTime.Now;
        selectedClassRoomId = 0;
    }

    protected async Task CreateClassRoom()
    {
        var req = new ClassRequest
        {
            Code = codeClass,
            Name = newClassName,
            Subject = subjectClass,
            TeacherId = selectedTeacherId
        };

        await ClassService.Create(req);
        await LoadClassRooms();

        codeClass = "";
        newClassName = "";
        subjectClass = "";
        selectedTeacherId = 0;

        showCreateClassModal = false;
    }

    protected async Task DeleteClassRoom(int id)
    {
        if (await JS.InvokeAsync<bool>("confirm", $"Xóa lớp ID {id}?"))
        {
            await ClassService.Delete(new IdRequest { Id = id });
            classRooms.RemoveAll(c => c.Id == id);
        }
    }

    protected async Task CreateTeacher()
    {
        var req = new TeacherRequest
        {
            Code = teacherCode,
            FullName = teacherFullName,
            BirthDate = teacherBirthDate ?? DateTime.Now
        };

        await TeacherService.Create(req);
        await LoadTeachers();

        teacherCode = "";
        teacherFullName = "";
        teacherBirthDate = DateTime.Now;

        showCreateTeacherModal = false;
    }

    protected async Task DeleteTeacher(int id)
    {
        if (await JS.InvokeAsync<bool>("confirm", $"Xóa giáo viên ID {id}?"))
        {
            await TeacherService.Delete(new IdRequest { Id = id });
            teachers.RemoveAll(t => t.Id == id);
        }
    }

    protected void ToggleSort()
    {
        sortAsc = !sortAsc;

        students = sortAsc
            ? students.OrderBy(s => GetLastName(s.FullName)).ToList()
            : students.OrderByDescending(s => GetLastName(s.FullName)).ToList();
    }

    protected string GetLastName(string fullName)
    {
        var parts = fullName.Trim().Split(' ');
        return parts.Last();
    }

    protected void ClearSearch()
    {
        searchTerm = "";
    }
}
