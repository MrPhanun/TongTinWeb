using Microsoft.AspNetCore.Mvc.RazorPages;   // ✅ required for PageModel
using Microsoft.AspNetCore.Mvc;              // optional, for IActionResult etc.
using TongTinApp.Data;                       // ✅ so you can use Member and Database


public class WinnerModel : PageModel
{
    private readonly Database _db;
    public WinnerModel(Database db) { _db = db; }

    public List<Member> Members { get; set; } = new();  // ✅ now uses TongTinApp.Data.Member
    public string? WinnerName { get; set; }

    public void OnGet()
    {
        Members = _db.GetMembers();
    }

    public void OnPostSearch(string SearchText)
    {
        Members = _db.SearchMembers(SearchText);
    }

    public void OnPostSpin(int WinnerId)
    {
        // Update DB
        _db.UpdateWinner(WinnerId);

        // Refresh list (only Active remain)
        Members = _db.GetMembers();

        // Show winner name
        var winner = Members.FirstOrDefault(m => m.Id == WinnerId);
        WinnerName = winner?.Name;
    }       

}
