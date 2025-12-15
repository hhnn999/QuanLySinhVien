using AntDesign.Charts;
using GRPC_NHibernate_Client.Services;
using Microsoft.AspNetCore.Components;
using Shared.DTOs.RequestModel;

namespace GRPC_NHibernate_Client.Pages
{
    public partial class StudentChart : ComponentBase
    {
        [Inject] public StudentApiClient StudentService { get; set; }

        protected bool isLoading = true;

        public class ClassChartData
        {
            public string className { get; set; }
            public int studentCount { get; set; }
        }

        private List<ClassChartData> chartData = new List<ClassChartData>();

        // Config biểu đồ
        private ColumnConfig config = new ColumnConfig
        {
            XField = "className",
            YField = "studentCount",
            Label = new ColumnViewConfigLabel
            {
                Visible = true,
                Position = "middle",
                Formatter = ""
            },
            ColumnStyle = new GraphicStyle
            {
                Fill = "#1f77b4"
            },
            Padding = "auto",
        };

    

        protected override async Task OnInitializedAsync()
        {
            isLoading = true;
            try
            {
                await LoadChartData();
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Lỗi tải dữ liệu biểu đồ: {ex.Message}");
            }
            finally
            {
                isLoading = false;
            }
        }

        private async Task LoadChartData()
        {
            var result = await StudentService.GetStudentCountByClassAsync(new IdRequest());

            chartData = result.Items
               .Select(i => new ClassChartData
               {
                   className = i.ClassName,
                   studentCount = i.StudentCount
               })
               .ToList();
        }
    }
}
