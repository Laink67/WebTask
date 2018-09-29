using System.ComponentModel.DataAnnotations;
using Dal.DBObjects;

namespace TaskWeb.Models.ViewModels
{
    /// <summary>
    /// Модель отдела для передачи в представление
    /// </summary>
    public class DepartmentViewModel
    {
        public int Id { get; set; }

        [Display(Name = "Название")]
        [Required(ErrorMessage = "Поле не заполнено")]
        [StringLength(50, MinimumLength = 2, ErrorMessage = "Длина строки должна быть от 3 до 50 символов")]
        public string Title { get; set; }

        [Display(Name = "Примечание")]
        public string Comment { get; set; }

        public int? ParentId { get; set; }

        public string PageTitle { get; set; }


        public DepartmentViewModel()
        {
            PageTitle = "Добавление нового отдела";
        }

            public DepartmentViewModel(int? parentid)
        {
            PageTitle = "Добавление нового отдела";
            ParentId = parentid;
        }

        public DepartmentViewModel(Department department)
        {
            Id = department.Id;
            Title = department.Title;
            Comment = department.Comment;
            ParentId = ParentId;
            PageTitle = "Редактирование отдела";
        }

        public Department GetDepartment()
        {
            return new Department(Id, Title, Comment, ParentId);
        }

    }
}