using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using StudentGrades.Data;

namespace StudentGrades.Pages;

[BindProperties]
public class AddGradesModel : PageModel
{
    private readonly StudentGradesDbContext _context;

    public AddGradesModel(StudentGradesDbContext context)
    {
        _context = context;
    }

    public StudentGrade StudentGrade { get; set; } = new();

    public void OnGet()
    {
    }

    public async Task<IActionResult> OnPostAsync()
    {
        if (!ModelState.IsValid) return Page();

        _context.Grades.Add(StudentGrade);
        await _context.SaveChangesAsync();

        TempData["SuccessMessage"] =
            $"Оценка \"{StudentGrade.Grade}\" по предмету {StudentGrade.CourseName} успешно выставлена студенту {StudentGrade.StudentName}";

        return RedirectToPage("/AddGrades");
    }
}