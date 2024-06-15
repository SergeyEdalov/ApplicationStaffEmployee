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
    public DateTime Birthday { get; set; }
    
    [Display(Name = "Отдел")]
    public string Department { get; set; }
    
    [Display(Name = "Должность")]
    public string JobTitle { get; set; }

    [Display(Name = "Дата начала работы")]
    [DataType(DataType.Date)]
    public DateTime WorkStart { get; set; }
    
    [Display(Name = "Заработная плата")]
    public decimal Salary { get; set; }

    //Заготовка под кастомную валидацию (или подключить библиотеку FluentValidation.DependencyInjection)
    public IEnumerable<ValidationResult> Validate(ValidationContext validationContext)
    {
        yield return ValidationResult.Success!;
    }
}
