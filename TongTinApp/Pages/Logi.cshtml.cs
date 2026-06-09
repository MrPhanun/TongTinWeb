using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using TongTinApp.Data;
using System.Data;

namespace TongTinApp.Pages
{
    public class LoginModel : PageModel
    {
        private readonly Database _db;

        public LoginModel(Database db)
        {
            _db = db;
        }

        [BindProperty]
        public string? UserInput { get; set; }

        public void OnGet() { }

        public IActionResult OnPostContinue()
        {
            if (string.IsNullOrEmpty(UserInput))
            {
                ModelState.AddModelError("", "Please enter username");
                return Page();
            }

            // ✅ Check database for matching email/phone
            DataTable dt = _db.GetAll();
            bool exists = false;

            foreach (DataRow row in dt.Rows)
            {
                if (row["Name"].ToString() == UserInput)
                {
                    exists = true;
                    break;
                }
            }

            if (exists == true)
            {
                // If found, go to Login page for password step
                return RedirectToPage("/LoginPassword");
            }
            else
            {
                // If not found, show error
                ModelState.AddModelError("", "Wrong user login");
                return Page();
            }
            
        }
    }
}
