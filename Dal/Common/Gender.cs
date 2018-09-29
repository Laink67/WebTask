using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Reflection;

namespace Dal.Common
{
    /// <summary>
    /// Перечисление пола сотрудника
    /// </summary>
    /// 
    public enum Gender
    {
        [Display(Name = "М")]
        Male = 1,

        [Display(Name = "Ж")]
        Female = 0
    }
}
