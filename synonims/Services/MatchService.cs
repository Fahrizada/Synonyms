using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using synonims.Interface;
using synonims.Models;
using System.Text.RegularExpressions;

namespace synonims.Services
{
    public class MatchService : BaseInterface
    {
        public SynonimsContext _db;
        public MatchService(SynonimsContext db)
        {
            _db = db;
        }
        public bool Create(string word, string synonym)
        {
            //First I found these words to get their IDs, and I know that they cant be null 
            //because I alredy check that in previos step in WordService(line 35,45)
            //Than I check if these words are existing in Match table, and if they dont I created new Match

            var checkWord = _db.Words.Where(x => x.Name == word).FirstOrDefault();
            var checkSynonym = _db.Words.Where(x => x.Name == synonym).FirstOrDefault();

            var checkMatch=_db.Matches.Where(x=>x.FirstWordId == checkWord.Id && x.SecondWordId==checkSynonym.Id
            || x.SecondWordId == checkWord.Id && x.FirstWordId == checkSynonym.Id).FirstOrDefault();

            if(checkMatch == null)
            {
                Models.Match match = new Models.Match
                {
                    FirstWordId = checkWord.Id,
                     SecondWordId = checkSynonym.Id
                };
                _db.Matches.Add(match);
                _db.SaveChanges();
                throw new Exception("You have successfuly added a synonym");
            }
            else
            {
                throw new Exception("This synonym of this word alredy exist");
            }

            
        }

        
        public List<Word> GetSynonym(string word)
        {
            //This check is here to make sure that input is valid
            if ( string.IsNullOrEmpty(word))
                throw new Exception("Inputs cant be empty.");


            //I used regex to check if there is some irregularity in the input
            Regex regex = new Regex("^[a-zA-Z]+$");

            if (!regex.IsMatch(word))
                throw new Exception("Only letters are allowed as inputs.");

            //I am finding the word that was enterd
            var FirstWord = _db.Words.Where(x => x.Name == word).FirstOrDefault();

            var matches = _db.Matches.ToList();
            var words = _db.Words.ToList();

            //Because I took the word that was enterd and its Id, if it is null that means that 
            //this word isnt created yet,if it is not I am calling function that is recursion
            if (FirstWord != null)
                return GetSynonymsRecursively(FirstWord, matches, words);
            else
                throw new Exception("This word doesnt exist in our database");
        }
        List<Word> GetSynonymsRecursively(Word InitialWord, List<Models.Match> matches, List<Word> words)
        {
            //This recursive function will find all synonyms and go deeper and find their synonyms and so on
            var synonyms = new List<Word>();

            if (InitialWord == null)
                return synonyms;

            synonyms.Add(InitialWord);

            var InitialMatches = matches.Where(x => x.FirstWordId == InitialWord.Id || x.SecondWordId == InitialWord.Id).ToList();


            foreach (var f in InitialMatches) {

                List<int> Ids = new List<int>();

                Ids.Add(f.FirstWordId);
                Ids.Add(f.SecondWordId);

                Ids.Remove(InitialWord.Id);

                var newWord = words.Where(x => x.Id == Ids.First()).FirstOrDefault();


                words.Remove(newWord);
                synonyms.AddRange(GetSynonymsRecursively(newWord, matches, words));
            }
            return synonyms;
        }

        public ActionResult Index()
        {
            throw new NotImplementedException();
        }
    }
}
