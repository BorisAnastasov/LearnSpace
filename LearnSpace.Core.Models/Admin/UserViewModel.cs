namespace LearnSpace.Core.Models.Admin
{
    public class UserViewModel
    {
        public string Id { get; set; }
        public string UserName { get; set; }
        public string FullName { get; set; }
        public List<string> Roles { get; set; }
    }
}
