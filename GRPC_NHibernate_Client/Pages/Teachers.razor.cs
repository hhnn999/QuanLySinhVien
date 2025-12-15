using AntDesign;
using AntDesign.TableModels;
using GRPC_NHibernate_Client.Pages.Components;
using GRPC_NHibernate_Client.Services;
using Microsoft.AspNetCore.Components;
using Shared.DTOs.RequestModel;
using System.ComponentModel.DataAnnotations;


namespace GRPC_NHibernate_Client.Pages
{
    public partial class Teachers : ComponentBase
    {
        [Inject] public TeacherApiClient TeacherApi { get; set; }
        [Inject] public ModalService ModalService { get; set; }
        [Inject] public ConfirmService ConfirmService { get; set; }

        List<TeacherRecord> teacherDb = new();
        IEnumerable<TeacherRecord> _selectedRows = new List<TeacherRecord>();
        ITable _table;
        List<TeacherRecord> _dataSource;
        int _total;

        void OnChange(QueryModel<TeacherRecord> query)
        {
            _total = teacherDb.AsQueryable().ExecuteTableQuery(query).Count();
            _dataSource = teacherDb.AsQueryable().ExecuteTableQuery(query).CurrentPagedRecords(query).ToList();
        }

        async Task StartEdit(TeacherRecord? row)
        {
            var data = row == null
                ? new TeacherRecord()
                : new TeacherRecord
                {
                    Id = row.Id,
                    Code = row.Code,
                    FullName = row.FullName,
                    BirthDate = row.BirthDate
                };

            TeacherForm formComponent = null!;
            ModalRef<bool> modalRef = null!;

            modalRef = ModalService.CreateModal<bool>(new ModalOptions
            {
                Title = "New Teacher",
                Width = 800,

                Content = (RenderFragment)(builder =>
                {
                    builder.OpenComponent<TeacherForm>(0);
                    builder.AddAttribute(2, "Model", data);
                    builder.AddComponentReferenceCapture(1, r => formComponent = (TeacherForm)r);
                    builder.CloseComponent();
                }),

                OnOk = async (_) =>
                {
                    if (!formComponent.FormRef.Validate())
                        return;

                    await TeacherApi.Create(new TeacherRequest
                    {
                        Code = data.Code,
                        FullName = data.FullName,
                        BirthDate = data.BirthDate
                    });

                    await modalRef.CloseAsync();
                    _table.ReloadData();
                },

                OnCancel = async (_) =>
                {
                    await modalRef.CloseAsync();
                }
            });

            await InvokeAsync(StateHasChanged);
        }

        async Task Delete(TeacherRecord row)
        {
            if (!await Confirm($"Delete teacher [{row.FullName}]?")) return;

            await TeacherApi.Delete(new IdRequest { Id = row.Id });

            teacherDb.Remove(row);
            _table.ReloadData();
        }

        async Task DeleteAll()
        {
            if (!await Confirm($"Delete {_selectedRows.Count()} teachers?")) return;

            foreach (var item in _selectedRows)
            {
                await TeacherApi.Delete(new IdRequest { Id = item.Id });
            }

            teacherDb = teacherDb.Except(_selectedRows).ToList();
            _selectedRows = new List<TeacherRecord>();
            _table.ReloadData();
        }

        private async Task<bool> Confirm(string msg)
        {
            return await ConfirmService.Show(msg, "Confirm", ConfirmButtons.YesNo, ConfirmIcon.Warning)
                == ConfirmResult.Yes;
        }

        protected override async Task   OnInitializedAsync()
        {
            teacherDb = (await TeacherApi.GetAll()).Select(x => new TeacherRecord
            {
                Id = x.Id,
                Code = x.Code,
                FullName = x.FullName,
                BirthDate = x.BirthDate
            }).ToList();

            _total = teacherDb.Count;
            _dataSource = teacherDb.Take(8).ToList();

            await InvokeAsync(() => _table?.ReloadData());
        }

        public record TeacherRecord
        {
            public int Id { get; set; }

            [Required]
            public string Code { get; set; }

            [Required]
            public string FullName { get; set; }

            public DateTime? BirthDate { get; set; }
        }
    }
}
