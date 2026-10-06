namespace ClinicApp.ViewModel
{
    public class AccountIndexViewModel
    {
        public int PageIndex { get; set; } = 1;
        public int PageSize { get; set; } = 20;
        public int TotalCount { get; set; }
        public AccountType? TypeFilter { get; set; }
        public string SearchName { get; set; }
        public List<AccountDetialsViewModel> allUsers { get; set; }
        = new List<AccountDetialsViewModel>();


    }
    public class AccountDetialsViewModel { 
        public string email { get; set; }
        public string username { get; set; }
        public AccountType type { get; set; }
        public int age { get; set; }
        public string UserId { get; set; }
        public string name { get; set; }
        public int id { get; set; }
    }
}
