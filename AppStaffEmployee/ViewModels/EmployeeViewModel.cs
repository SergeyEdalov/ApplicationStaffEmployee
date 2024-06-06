using Microsoft.AspNetCore.Mvc;
using System.ComponentModel.DataAnnotations;

namespace AppStaffEmployee.ViewModels;

public class EmployeeViewModel : IValidatableObject
{
    [HiddenInput(DisplayValue = false)]
    public Guid? Id { get; set; }

    [Required(ErrorMessage = "ФИО обязательно")]
    [Display(Name = "Ф.И.О.")]
    [StringLength(100, MinimumLength = 2)]
    [RegularExpression(@"^[А-ЯЁA-Z][а-яёa-z]+(?:\s[А-ЯЁA-Z][а-яёa-z]+){1,2}$", ErrorMessage = "Неверный формат данных")]
    public string FullName { get; set; } = null!;

    [Display(Name = "Дата рождения")]
    [DataType(DataType.Date)]
    //[RegularExpression(@"^(0[1-9]|[12][0-9]|3[01])\.(0[1-9]|1[0-2])\.(\d{2})$", ErrorMessage = "Неверный формат даты")]
    public DateTime Birthday { get; set; }
    
    [Display(Name = "Отдел")]
    public string Department { get; set; }
    
    [Display(Name = "Должность")]
    public string JobTitle { get; set; }

    [Display(Name = "Дата начала работы")]
    [DataType(DataType.Date)]
    //[RegularExpression(@"^(0[1-9]|[12][0-9]|3[01])\.(0[1-9]|1[0-2])\.(\d{2})$", ErrorMessage = "Неверный формат даты")]
    public DateTime WorkStart { get; set; }
    
    [Display(Name = "Заработная плата")]
    [RegularExpression(@"^\d+(\.\d{1,2})?$", ErrorMessage = "Зарплата должна быть числом с до двумя десятичными знаками.")]
    public decimal Salary { get; set; }

    //Подумать над своей валидацией или подключить библиотеку FluentValidation.DependencyInjection
    public IEnumerable<ValidationResult> Validate(ValidationContext validationContext)
    {
        yield return ValidationResult.Success!;
    }
}
