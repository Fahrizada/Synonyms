namespace synonims.Models
{
    /*this model is created for containing one word ex:wash
    second model called Match will containg id from word wash and id from its synonim. ex:clean
    This concept gives this logic both-way search possibility because it will hold both word ids */
    public class Word
    {
        public int Id { get; set; }
        public string? Name { get; set; }
    }
}
