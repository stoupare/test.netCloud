namespace Fortune_Teller_UI.Services
{

    public class Root
    {
        public BuzzGraph[] buzzGraphResultList;
    }


    public class BuzzGraph
    {
        public string[] Names { get; set; }
        public int Value { get; set; }
        public bool Excluded { get; set; } 
    }
}