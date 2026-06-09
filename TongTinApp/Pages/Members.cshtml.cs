using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.Collections.Generic;
using System.Data;

namespace TongTinApp.Pages
{
    public class MembersModel : PageModel
    {
        private readonly TongTinApp.Data.Database _db;

        public MembersModel(TongTinApp.Data.Database db)
        {
            _db = db;
        }

        public List<Member> MemberList { get; set; } = new();

        public void OnGet()
        {
            LoadMembers();
        }

        // Add new member
       public IActionResult OnPostAdd(string Name, string Role)
        {
            // Get the current user from session (set at login)
            string currentUser = HttpContext.Session.GetString("UserName") ?? "System";
            DateTime now = DateTime.Now;

            // Call Database.Add with all required parameters
            _db.Add(Name, Role, "កំពុងបង់", "មិនទាន់ទទួល",
                    currentUser, now,
                    currentUser, now,
                    "Active");

            // After inserting, reload the page
            return Page();
        }

        // Update existing member (Name, Role, Status, Doing only)
        public IActionResult OnPostUpdate(int Id, string Name, string Role, string Status, string Doing)
        {
            string userInput = HttpContext.Session.GetString("UserName") ?? "System";

            _db.Update(Id, Name, Role,
                       Status, Doing,
                       userInput, DateTime.Now,
                       userInput, DateTime.Now,
                       "Active");

            LoadMembers();
            return Page();
        }

        // Delete member
        public IActionResult OnPostDelete(int Id)
        {
            _db.Delete(Id);
            LoadMembers();
            return Page();
        }

        // Search members
        public IActionResult OnPostSearch(string Keyword)
        {
            var dt = _db.Search(Keyword);
            MemberList.Clear();
            foreach (DataRow row in dt.Rows)
            {
                MemberList.Add(new Member
                {
                    ID = (int)row["ID"], // internal use only
                    Name = row["Name"].ToString(),
                    Role = row["Role"].ToString(),
                    Status = row["Status"].ToString(),
                    Remark = row["Doing"]?.ToString() ?? "",
                    RegisterBy = row["RegBy"].ToString(),
                    RegisterDate = (DateTime)row["RegDate"],
                    UpdateBy = row["UpdateBy"].ToString(),
                    UpdateDate = (DateTime)row["UpdateDate"]
                });
            }
            return Page();
        }

        private void LoadMembers()
        {
            var dt = _db.GetAll();
            MemberList.Clear();
            foreach (DataRow row in dt.Rows)
            {
                MemberList.Add(new Member
                {
                    ID = (int)row["ID"],
                    Name = row["Name"].ToString(),
                    Role = row["Role"].ToString(),
                    Status = row["Status"].ToString(),
                    Remark = row["Doing"]?.ToString() ?? "",
                    RegisterBy = row["RegBy"].ToString(),
                    RegisterDate = (DateTime)row["RegDate"],
                    UpdateBy = row["UpdateBy"].ToString(),
                    UpdateDate = (DateTime)row["UpdateDate"]
                });
            }
        }
    }
    public class Member
    {
        public int ID { get; set; }          // internal use only
        public string Name { get; set; }
        public string Role { get; set; }
        public string Status { get; set; }
        public string Remark { get; set; }
        public string RegisterBy { get; set; }
        public DateTime RegisterDate { get; set; }
        public string UpdateBy { get; set; }
        public DateTime UpdateDate { get; set; }
    }
    
}
