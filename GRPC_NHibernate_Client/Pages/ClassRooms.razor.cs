using AntDesign;
using AntDesign.TableModels;
using GRPC_NHibernate_Client.Pages.Components;
using GRPC_NHibernate_Client.Services;
using Microsoft.AspNetCore.Components;
using Shared.DTOs.RequestModel;
using Shared.DTOs.ResponseModel;
using System.ComponentModel.DataAnnotations;

namespace GRPC_NHibernate_Client.Pages
{
    public partial class ClassRooms : ComponentBase
    {
        [Inject] public ClassRoomApiClient ClassApi { get; set; } = default!;
        [Inject] public TeacherApiClient TeacherApi { get; set; } = default!;
        [Inject] public ModalService ModalService { get; set; } = default!;
        [Inject] public ConfirmService ConfirmService { get; set; } = default!;

        List<ClassRoomRecord> classDb = new();
        IEnumerable<ClassRoomRecord> _selectedRows = new List<ClassRoomRecord>();
        ITable _table = default!;
        List<ClassRoomRecord> ClassRoomRecords = new();
        int _total;
        bool showModal = false;
        ClassRoomRecord modalModel = new();

        void OnChange(QueryModel<ClassRoomRecord> query)
        {
            _total = classDb.AsQueryable().ExecuteTableQuery(query).Count();
            ClassRoomRecords = classDb.AsQueryable()
                .ExecuteTableQuery(query)
                .CurrentPagedRecords(query)
                .ToList();
        }

        async Task StartEdit(ClassRoomRecord? row)
        {
            var data = row == null
                ? new ClassRoomRecord()
                : new ClassRoomRecord
                {
                    Id = row.Id,
                    Code = row.Code,
                    Name = row.Name,
                    Subject = row.Subject,
                    TeacherId = row.TeacherId
                };

            showModal = true;
        }

        void CloseModal()
        {
            showModal = false;
        }
      
        async Task Save(ClassRoomRecord data)
        {
            await SaveClassroomDatas(data);

            showModal = false;
        }

        async Task SaveClassroomDatas(ClassRoomRecord data)
        {
            await ClassApi.Create(new ClassRequest
            {
                Code = data.Code,
                Name = data.Name,
                Subject = data.Subject,
                TeacherId = data.TeacherId
            });

            await Reload();
        }

        async Task Delete(ClassRoomRecord row)
        {
            if (!await Confirm($"Delete class [{row.Name}]?")) return;

            await ClassApi.Delete(new IdRequest { Id = row.Id });
            await Reload();
        }

        async Task DeleteAll()
        {
            if (!await Confirm($"Delete {_selectedRows.Count()} classes?")) return;

            foreach (var item in _selectedRows)
                await ClassApi.Delete(new IdRequest { Id = item.Id });

            await Reload();
        }

        async Task Reload()
        {
            classDb = (await ClassApi.GetAll())
                .Select(x => new ClassRoomRecord
                {
                    Id = x.Id,
                    Code = x.Code,
                    Name = x.Name,
                    Subject = x.Subject,
                    TeacherId = x.Teacher?.Id ?? 0
                }).ToList();

            _table.ReloadData();
        }

        private async Task<bool> Confirm(string msg)
        {
            return await ConfirmService.Show(msg, "Confirm", ConfirmButtons.YesNo, ConfirmIcon.Warning)
                == ConfirmResult.Yes;
        }

        protected override async Task OnInitializedAsync()
        {
            classDb = (await ClassApi.GetAll())
                .Select(x => new ClassRoomRecord
                {
                    Id = x.Id,
                    Code = x.Code,
                    Name = x.Name,
                    Subject = x.Subject,
                    TeacherName = x.Teacher?.FullName
                })
                .ToList();

            _total = classDb.Count;
            ClassRoomRecords = classDb.Take(8).ToList();
        }

        public class ClassRoomRecord
        {
            public int Id { get; set; }

            [Required]
            public string Code { get; set; } = string.Empty;

            [Required]
            public string Name { get; set; } = string.Empty;

            public string Subject { get; set; } = string.Empty;

            [Required]
            public int TeacherId { get; set; }
            public string? TeacherName { get; internal set; }
        }
    }
}
