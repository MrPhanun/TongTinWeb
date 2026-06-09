using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using TongTinApp.Data;
using System.Data;

namespace TongTinApp.Pages
{
    public class IndexModel : PageModel
    {
        private readonly Database _db;

        public IndexModel(Database db)
        {
            _db = db;
        }

        [BindProperty]
        public string? UserInput { get; set; }
       
        public void OnGet() { }
         [BindProperty]
        public string Username { get; set; } = string.Empty;

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
                    HttpContext.Session.SetString("UserName", UserInput);
                    exists = true;
                    break;
                }
            }

            if (exists == true)
            {
                // In Index.cshtml.cs (after successful login)

                // If found, go to Login page for password step
                return RedirectToPage("/Password");
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
