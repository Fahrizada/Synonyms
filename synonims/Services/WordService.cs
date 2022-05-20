using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using synonims.Interface;
using synonims.Models;
using System.Text.RegularExpressions;

namespace synonims.Services
{
    public class WordService:BaseInterface
    {
        public SynonimsContext _db;
        public WordService(SynonimsContext db)
        {
            _db = db;
        }

        public bool Create(string word, string synonym)
        {
            //This check is here to make sure that input is valid
            if (string.IsNullOrEmpty(synonym) || string.IsNullOrEmpty(word))
                throw new Exception("Inputs cant be empty.");

            //I used regex to check if there is some irregularity in the input
            Regex regex = new Regex("^[a-zA-Z]+$");

            if (!regex.IsMatch(word) || !regex.IsMatch(synonym))
                throw new Exception("Only letters are allowed as inputs.");
           
            //First I check if this given word or synonym is already created. If it is than I dont need to
            //create it, I just need to send word and its synonym to MatchService 

            var checkWord=_db.Words.Where(x=>x.Name == word).FirstOrDefault();
            var checkSynonym=_db.Words.Where(x=>x.Name == synonym).FirstOrDefault();

            if (checkWord == null)
            {
                Word NewWord = new Word
                {
                    Name = word,
                };

                _db.Words.Add(NewWord);
                _db.SaveChanges();
            }
            if (checkSynonym == null)
            {
                Word NewSynonym = new Word
                {
                    Name = synonym,
                };

                _db.Words.Add(NewSynonym);
                _db.SaveChanges();
            }

            MatchService matchService = new MatchService(_db);
            matchService.Create(word,synonym);
            
            return true;
        }

        public ActionResult Index()
        {
            throw new NotImplementedException();
        }
    }
}
