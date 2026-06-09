using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using TongTinApp.Data;
using System.Data;

namespace TongTinApp.Pages
{
    public class PasswordModel : PageModel
    {
        private readonly Database _db;

        public PasswordModel(Database db)
        {
            _db = db;
        }

        [BindProperty]
        public string? PasswordInput { get; set; }

        public void OnGet() { }

        

        public IActionResult OnPostLogin()
        {
            if (string.IsNullOrEmpty(PasswordInput))
            {
                ModelState.AddModelError("", "Please enter Role");
                return Page();
            }

            // ✅ Check database for matching email/phone
            DataTable dt = _db.GetAll();
            bool exists = false;

            foreach (DataRow row in dt.Rows)
            {
                if (row["Role"].ToString() == PasswordInput)
                {
                    exists = true;
                    break;
                }
            }

            if (exists == true)
            {
                // If found, go to Login page for password step
                // In Index.cshtml.cs (after successful login)

                return RedirectToPage("/Dashboard");
            }
            else
            {
                // If not found, show error
                ModelState.AddModelError("", "Wrong role login");
                return Page();
            }
            
        }
        
    }
}
