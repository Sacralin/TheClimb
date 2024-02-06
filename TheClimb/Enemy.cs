using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TheClimb
{

    internal class Enemy
    {
        public string name;
        public int health;
        public int maxHealth;
        public int enemyLevel;
        public string[] enemyNames = { "Cessair", "Abaddon", "Jabez", "Alastor", "Keres", "Andras", "Lilitu", "Azazel", "Gorgon", "Agrona", "Mercurius" };
        public Item[] equipped = new Item[7];

        public Enemy()
        {
            this.name = "";
            this.maxHealth = 50;
            this.health = maxHealth;
            this.enemyLevel = 0;
            this.equipped = new Item[7];

            Item empty = new Item();
            empty.name = "Empty";
            for (int i = 0; i < equipped.Length; i++)
            {
                equipped[i] = empty;
            }
        }



        //https://blog.reedsy.com/character-name-generator/archetype/villain/
    }
}
