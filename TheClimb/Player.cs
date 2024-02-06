namespace TheClimb
{
    [Serializable]
    internal class Player 
    {
        
        public string name;
        public int health;
        public int maxHealth;
        public int heighestLevel;
        public int lastLevel;
        public Item[] invent = new Item[10];
        public Item[] equipped = new Item[7];
        public float gold;
        public int potion;


        public Player()
        {

            name = "";
            maxHealth = 100;
            health = maxHealth;
            heighestLevel = 1;
            lastLevel = 0;
            invent = new Item[10];
            equipped = new Item[7];
            gold = 0f;
            potion = 10;

            Item empty = new Item();
            empty.name = "Empty";
            for (int i = 0; i < invent.Length; i++)
            {
                invent[i] = empty;
            }
            for (int i = 0; i < equipped.Length; i++)
            {
                equipped[i] = empty;
            }


        }
    }
}
