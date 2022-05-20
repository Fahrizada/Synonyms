using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using synonims.Models;
using synonims.Services;

namespace synonims.Controllers
{
    public class WordController : Controller
    {
        public SynonimsContext _db;
        public IHttpContextAccessor _contextAccessor;
        public WordController(SynonimsContext db, IHttpContextAccessor contextAccessor)
        {
            _db = db;
            _contextAccessor = contextAccessor;
        }

        // GET: WordController
        public ActionResult Index()
        {
            return View("Add");
        }
        public ActionResult Search()
        {
            return View();
        }

        // GET: WordController/GetSynonym
        public ActionResult GetSynonym(string word)
        {
            try
            {
                MatchService matchService = new MatchService(_db);

                //Here I am taking value that was enterd in, and I am sending it to matchService
                //to get what is synonym for that word
                //After that I am calling view to display what matchService returned
                var SentWord = Request.Form["word"];

                var syns = matchService.GetSynonym(SentWord);

                return View("FindSynonyms", syns);
            }
            catch (Exception ex)
            {
                return View("Warning", ex);
            }
        }

        // GET: WordController/Create
        public ActionResult Create(string word, string synonym)
        {
            WordService wordService = new WordService(_db);

            synonym = Request.Form["synonym"];

            try
            {
                wordService.Create(word, synonym);
                return RedirectToAction("Index");
            }
            catch (Exception ex) {
                
                return View("Warning", ex);
            }
        }


        
    }
}
