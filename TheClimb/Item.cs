using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TheClimb
{

    [Serializable]
    internal class Item
    {

        public string name;
        public int baseArmour;
        public int armour;
        public float value;
        public bool isMagic;
        public int level;
        public int equipmentSlot;
        public string description;
        public int damageMin;
        public int damageMax;
        public int quality;
        public int critChance;

        public Item()
        {

            this.name = "";
            this.baseArmour = 0;
            this.armour = 0;
            this.value = 0;
            this.isMagic = false;
            this.level = 0;
            this.equipmentSlot = -1;
            this.description = "No Description Available.";
            this.damageMin = 0;
            this.damageMax = 0;
            this.quality = 0;
            this.critChance = 33;


        }
    }
}
