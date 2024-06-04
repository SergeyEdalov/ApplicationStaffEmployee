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
    [RegularExpression(@"([А-ЯЁ][а-яё]+)|([A-Z][a-z]+)", ErrorMessage = "Неверный формат данных")]
    public string FullName { get; set; } = null!;

    [RegularExpression("^(0[1-9]|[12][0-9]|3[01])\\.(0[1-9]|1[0-2])\\.(19|20)\\d{2}$\r\n", ErrorMessage = "Неверный формат даты")]
    [Display(Name = "Дата рождения")]
    public DateTime Birthday { get; set; }
    
    [Display(Name = "Отдел")]
    public string Department { get; set; }
    
    [Display(Name = "Должность")]
    public string JobTitle { get; set; }

    [RegularExpression("^(0[1-9]|[12][0-9]|3[01])\\.(0[1-9]|1[0-2])\\.(19|20)\\d{2}$\r\n", ErrorMessage = "Неверный формат даты")]
    [Display(Name = "Дата начала работы")]
    public DateTime WorkStart { get; set; }
    
    [Display(Name = "Заработная плата")]    
    public decimal Salary { get; set; }

    //Подумать над своей валидацией или подключить библиотеку FluentValidation.DependencyInjection
    public IEnumerable<ValidationResult> Validate(ValidationContext validationContext)
    {
        yield return ValidationResult.Success!;
    }
}
