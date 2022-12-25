using System.ComponentModel.DataAnnotations;

namespace StudentGrades.Data;

public class StudentGrade
{
    public int StudentGradeId { get; set; }

    [Required(ErrorMessage = "Пожалуйста, введите имя студента")]
    public string StudentName { get; set; } = "";

    [Required(ErrorMessage = "Пожалуйста, введите название предмета")]
    public string CourseName { get; set; } = "";

    [Required(ErrorMessage = "Пожалуйста, введите оценку")]
    [Range(0, 10, ErrorMessage = "Оценка должна быть в пределах от 0 до 10")]
    public int? Grade { get; set; }
}