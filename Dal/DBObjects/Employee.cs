using Dal.Common;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Reflection;

namespace Dal.DBObjects
{
    /// <summary>
    /// Экземпляр сотрудника из БД
    /// </summary>
    public class Employee : BasedObject
    {
        public string FIO { get; set; }
        public DateTime Date { get; set; }
        public string Phone { get; set; }
        public Gender Gender { get; set; }
        public string GenderStr  => EnumHelper<Gender>.GetDisplayValue(Gender);
        public Department Department { get; set; }
        public int DepartmentId { get; set; }
        public string Comment { get; set; }

        public bool Retired => (DateTime.Now - Date).Days > (Gender == Gender.Female ? 60 : 65) * 365;

        public Employee() { }

        public Employee(int id, string fio, DateTime date, string phone, Gender gender, int departmentid, string comment)
        {
            Id = id;
            FIO = fio;
            Date = date.Date;
            Phone = phone;
            Gender = gender;
            DepartmentId = departmentid;
            Comment = comment;
        }
    }
}
public static class EnumHelper<T>
{
    public static IList<T> GetValues(Enum value)
    {
        var enumValues = new List<T>();

        foreach (FieldInfo fi in value.GetType().GetFields(BindingFlags.Static | BindingFlags.Public))
        {
            enumValues.Add((T)Enum.Parse(value.GetType(), fi.Name, false));
        }
        return enumValues;
    }

    public static T Parse(string value)
    {
        return (T)Enum.Parse(typeof(T), value, true);
    }

    public static IList<string> GetNames(Enum value)
    {
        return value.GetType().GetFields(BindingFlags.Static | BindingFlags.Public).Select(fi => fi.Name).ToList();
    }

    public static IList<string> GetDisplayValues(Enum value)
    {
        return GetNames(value).Select(obj => GetDisplayValue(Parse(obj))).ToList();
    }

    private static string lookupResource(Type resourceManagerProvider, string resourceKey)
    {
        foreach (PropertyInfo staticProperty in resourceManagerProvider.GetProperties(BindingFlags.Static | BindingFlags.NonPublic | BindingFlags.Public))
        {
            if (staticProperty.PropertyType == typeof(System.Resources.ResourceManager))
            {
                System.Resources.ResourceManager resourceManager = (System.Resources.ResourceManager)staticProperty.GetValue(null, null);
                return resourceManager.GetString(resourceKey);
            }
        }

        return resourceKey; // Fallback with the key name
    }

    public static string GetDisplayValue(T value)
    {
        var fieldInfo = value.GetType().GetField(value.ToString());

        var descriptionAttributes = fieldInfo.GetCustomAttributes(
            typeof(DisplayAttribute), false) as DisplayAttribute[];

        if (descriptionAttributes[0].ResourceType != null)
            return lookupResource(descriptionAttributes[0].ResourceType, descriptionAttributes[0].Name);

        if (descriptionAttributes == null) return string.Empty;
        return (descriptionAttributes.Length > 0) ? descriptionAttributes[0].Name : value.ToString();
    }
}

