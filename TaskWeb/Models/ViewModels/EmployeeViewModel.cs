using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Web.Mvc;
using Dal.Common;
using Dal.DBObjects;

namespace TaskWeb.Models.ViewModels
{
    /// <summary>
    /// Модель сотрудника для передачи в представление
    /// </summary>
    public class EmployeeViewModel
    {
        public int Id { get; set; }

        [Display(Name = "ФИО")]
        [Required(ErrorMessage = "Поле не заполнено")]
        [StringLength(50, MinimumLength = 2, ErrorMessage = "Длина строки должна быть от 2 до 50 символов")]
        public string FIO { get; set; }

        [Display(Name = "Дата")]
        [DataType(DataType.Date)]
        [DisplayFormat(DataFormatString = "{0:yyyy-MM-dd}", ApplyFormatInEditMode = true)]
        [Required(ErrorMessage = "Поле не заполнено")]
        public DateTime Date { get; set; }

        [Display(Name = "Телефон")]
        [Required(ErrorMessage = "Поле не заполнено")]
        public string Phone { get; set; }

        [Display(Name = "Пол")]
        public Gender Gender { get; set; }

        public string PageTitle { get; set; }

        public int DepartmentId { get; set; }

        [Display(Name = "Отдел")]
        public SelectList Departments { get; set; }

        public string Title { get; set; }

        [Display(Name = "Примечание")]
        public string Comment { get; set; }

        [Display(Name = "Пенсионер")]
        public int Pensioner { get; set; }

        public EmployeeViewModel(){}

        public EmployeeViewModel(Employee employee, List<Department> departments)
        {
            Id = employee.Id;
            FIO = employee.FIO;
            Date = employee.Date;
            Phone = employee.Phone;
            Gender = employee.Gender;
            DepartmentId = employee.DepartmentId;
            Comment = employee.Comment;
            Departments = new SelectList(departments, nameof(Department.Id), nameof(Department.Title));
            PageTitle = Id != 0 ? "Редактирование сотрудника" : "Добавление сотрудника";
        }

        public EmployeeViewModel(Employee employee, string title)
        {
            Id = employee.Id;
            FIO = employee.FIO;
            Date = employee.Date;
            Phone = employee.Phone;
            Gender = employee.Gender;
            DepartmentId = employee.DepartmentId;
            Comment = employee.Comment;
            Title = title;
            PageTitle = Id != 0 ? "Редактирование сотрудника" : "Добавление сотрудника";
        }


        public Employee GetEmployee()
        {
            return new Employee(Id, FIO, Date, Phone, Gender, DepartmentId, Comment);
        }

    }
}
