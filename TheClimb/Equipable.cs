using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection.PortableExecutable;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace TheClimb
{
    [Serializable]
    internal class Equipable : Item
    {
        public Equipable(int equipmentSlot, int quality, int level)
        {

            this.equipmentSlot = equipmentSlot; //0 shield, 1 weapon, 2 head,3 hands, 4 chest, 5 legs, 6 feet
            baseArmour = quality;
            this.level = level;
            armour = baseArmour + level;
            this.quality = quality;
            value = 0.75f * quality;

            if (quality == 1)
            {
                if (equipmentSlot == 1 || equipmentSlot == 0)
                {
                    name = "Wooden ";
                }
                else
                {
                    name = "Leather ";
                }

            }
            else if (quality == 2)
            {
                name = "Iron ";
            }
            else if (quality == 3)
            {
                name = "Steel ";
            }
            else if (quality == 4)
            {
                name = "Titanium ";
            }


            if (equipmentSlot == 0)
            {
                name += "Shield";
                if (isMagic == true)
                {
                    damageMin += 1;
                    damageMax += 1;
                }
            }
            else if (equipmentSlot == 1)
            {
                name += "Sword";
                armour = 0;
                damageMin = level + 10 + quality;
                damageMax = damageMin + (quality * quality);
                if (isMagic == true)
                {
                    damageMin += 2;
                    damageMax += 2;
                }
            }
            else if (equipmentSlot == 2)
            {
                name += "Helm";
                if (isMagic == true)
                {
                    armour += 1;
                }
            }
            else if (equipmentSlot == 3)
            {
                name += "Gloves";
                if (isMagic == true)
                {
                    armour += 1;
                }

            }
            else if (equipmentSlot == 4)
            {
                name += "Chest";
                if (isMagic == true)
                {
                    armour += 1;
                }
            }
            else if (equipmentSlot == 5)
            {
                name += "Leggings";
                if (isMagic == true)
                {
                    armour += 1;
                }
            }
            else if (equipmentSlot == 6)
            {
                name += "Boots";
                if (isMagic == true)
                {
                    armour += 1;
                }
            }

        }

        public void Update()
        {
            if (equipmentSlot == 0)
            {
                if (isMagic == true)
                {
                    name = "Modified " + name;
                    damageMin += 1;
                    damageMax += 1;
                }
            }
            else if (equipmentSlot == 1)
            {
                if (isMagic == true)
                {
                    name = "Modified " + name;
                    damageMin += 2;
                    damageMax += 2;
                }
            }
            else if (equipmentSlot == 2)
            {
                if (isMagic == true)
                {
                    name = "Modified " + name;
                    armour += 1;
                }
            }
            else if (equipmentSlot == 3)
            {

                if (isMagic == true)
                {
                    name = "Modified " + name;
                    armour += 1;
                }

            }
            else if (equipmentSlot == 4)
            {

                if (isMagic == true)
                {
                    name = "Modified " + name;
                    armour += 1;
                }
            }
            else if (equipmentSlot == 5)
            {

                if (isMagic == true)
                {
                    name = "Modified " + name;
                    armour += 1;
                }
            }
            else if (equipmentSlot == 6)
            {
                if (isMagic == true)
                {
                    name = "Modified " + name;
                    armour += 1;
                }
            }
        }

    }
}
