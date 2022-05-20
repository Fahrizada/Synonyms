using Microsoft.AspNetCore.Mvc;

namespace synonims.Interface
{
    public interface BaseInterface
    {
        public ActionResult Index();

        public bool Create(string word, string synonym);

    }
}
