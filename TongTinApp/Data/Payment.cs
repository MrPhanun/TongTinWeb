namespace TongTinApp.Data
{
   public class MonthlyPaymentRecord
    {
        public int Id { get; set; }
        public string Name { get; set; } = "";
        public string Role { get; set; } = "";
        public string Status { get; set; } = "";
        public string Doing { get; set; } = "";
        public int Book { get; set; }
        public decimal Money { get; set; }
        public string Month { get; set; } = "";
        public int Year { get; set; }
    }
}
