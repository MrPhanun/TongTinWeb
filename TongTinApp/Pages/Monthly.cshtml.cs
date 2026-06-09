using Microsoft.AspNetCore.Mvc.RazorPages;
using TongTinApp.Data;
using System.Collections.Generic;

namespace TongTinApp.Pages
{
    public class MonthlyModel : PageModel
    {
        private readonly Database _db;
        public MonthlyModel(Database db)
        {
            _db = db;
        }

        public List<MonthlyPaymentRecord> Results { get; set; } = new();

        public void OnGet()
        {
            Results = _db.GetMonthlyPayments();
        }

        public void OnPostSearch(string SearchText)
        {
            Results = _db.SearchMonthlyPayments(SearchText);
        }

    }
}
