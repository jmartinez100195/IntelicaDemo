namespace Intelica.Authentication.API.Domain.PageAggegate.Domain
{
    public class Page
    {
        public Guid PageID { get; private set; }
        public string PageRoot { get; private set; }
        public bool PageActive { get; private  set; }
    }
}