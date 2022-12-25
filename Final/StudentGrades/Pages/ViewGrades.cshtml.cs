using Microsoft.AspNetCore.Mvc.RazorPages;
using StudentGrades.Data;

namespace StudentGrades.Pages;

public class ViewGradesModel : PageModel
{
    private readonly StudentGradesDbContext _context;

    public ViewGradesModel(StudentGradesDbContext context)
    {
        _context = context;
    }

    public IList<StudentGrade> StudentGrades { get; private set; } = new List<StudentGrade>();

    public void OnGet()
    {
        StudentGrades = _context.Grades.OrderBy(grade => grade.StudentGradeId).ToList();
    }
}