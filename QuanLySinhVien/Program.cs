using Microsoft.Extensions.DependencyInjection;
using QuanLySinhVien.Repositories;
using QuanLySinhVien.Services;

var services = new ServiceCollection();

string connString = "Server=.\\SQLEXPRESS;Database=StudentManagement;Trusted_Connection=True;";

services.AddSingleton<IStudentRepository>(sp => new StudentRepositoryAdo(connString));
services.AddSingleton<StudentService>();

var provider = services.BuildServiceProvider();
var studentService = provider.GetRequiredService<StudentService>();

await RunMenu();

async Task RunMenu()
{
    while (true)
    {
        Console.WriteLine("=== QUAN LY SINH VIEN ===");
        Console.WriteLine("1. Xem danh sach sinh vien");
        Console.WriteLine("2. Them sinh vien");
        Console.WriteLine("3. Sua sinh vien");
        Console.WriteLine("4. Xoa sinh vien");
        Console.WriteLine("5. Sap xep sinh vien theo ten");
        Console.WriteLine("6. Tim theo ma");
        Console.WriteLine("0. Thoat");
        Console.Write("Chon(0-6): ");
        var key = Console.ReadLine();
        Console.Clear();
        switch (key)
        {
            case "1":
                await ShowAll();
                break;
            case "2":
                await AddStudent();
                break;
            case "3":
                await EditStudent();
                break;
            case "4":
                await DeleteStudent();
                break;
            case "5":
                await ShowSorted();
                break;
            case "6":
                await FindByCode();
                break;
            case "0":
                return;
            default:
                Console.WriteLine("Khong hop le");
                break;
        }
        Console.WriteLine("Nhan Enter de tiep tuc...");
        Console.ReadLine();
        Console.Clear();
    }
}

async Task ShowAll()
{
    var list = await studentService.GetAllAsync();
    Console.WriteLine("Danh sach sinh vien:");
    foreach (var s in list)
    {
        Console.WriteLine($"{s.StudentId} | {s.Code} | {s.FullName} | {s.BirthDate:yyyy-MM-dd} | {s.Address} | Lop: {s.ClassRoom?.Name}");
    }
}

async Task AddStudent()
{
    var s = new Student();
    Console.Write("Ma: "); s.Code = Console.ReadLine() ?? "";
    Console.Write("Ho va ten: "); s.FullName = Console.ReadLine() ?? "";
    Console.Write("Ngay sinh (yyyy-MM-dd): ");
    if (DateTime.TryParse(Console.ReadLine(), out var dt)) s.BirthDate = dt;
    Console.Write("Đia chi: "); s.Address = Console.ReadLine() ?? "";
    // For simplicity, not selecting class here
    await studentService.AddAsync(s);
    Console.WriteLine("Them sinh vien thanh cong!");
}

async Task EditStudent()
{
    Console.Write("Nhap id sinh vien can sua: ");
    if (int.TryParse(Console.ReadLine(), out var id))
    {
        var s = await studentService.GetAllAsync();
        var st = s.FirstOrDefault(x => x.StudentId == id);
        if (st == null) { Console.WriteLine("Khong tim thay"); return; }
        Console.Write($"Ma ({st.Code}): ");
        var c = Console.ReadLine(); if (!string.IsNullOrWhiteSpace(c)) st.Code = c;
        Console.Write($"Ho ten ({st.FullName}): ");
        c = Console.ReadLine(); if (!string.IsNullOrWhiteSpace(c)) st.FullName = c;
        Console.Write($"Đia chi ({st.Address}): ");
        c = Console.ReadLine(); if (!string.IsNullOrWhiteSpace(c)) st.Address = c;
        await studentService.UpdateAsync(st);
        Console.WriteLine("Da cap nhat");
    }
}

async Task DeleteStudent()
{
    Console.Write("Nhap id sinh vien can xoa: ");
    if (int.TryParse(Console.ReadLine(), out var id))
    {
        await studentService.DeleteAsync(id);
        Console.WriteLine("Da xoa");
    }
    else
    {
        Console.WriteLine("Khong tim thay id sinh vien phu hop.");
    }
}

async Task ShowSorted()
{
    var list = await studentService.GetSortedByNameAsync();
    foreach (var s in list) Console.WriteLine($"{s.StudentId} | {s.Code} | {s.FullName}");
}

async Task FindByCode()
{
    Console.Write("Nhap ma sinh vien: ");
    var code = Console.ReadLine() ?? "";
    var all = await studentService.GetAllAsync();
    var st = all.FirstOrDefault(x => x.Code.Equals(code, StringComparison.OrdinalIgnoreCase));
    if (st == null) Console.WriteLine("Khong tim thay");
    else Console.WriteLine($"{st.StudentId} | {st.Code} | {st.FullName} | Lop: {st.ClassRoom?.Name}");
}
