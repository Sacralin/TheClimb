using System.IO;
using System.Runtime.Serialization.Formatters.Binary;

namespace TheClimb
{
    internal class Program
    {
        static void Main(string[] args)
        {
            bool debug = false;
            string lastAction = "";
            Player player = new Player();
            bool firstBattle = true;
            Item empty = new Item();
            empty.name = "Empty";
            player.invent[0] = new Equipable(0, 1, 1);
            player.equipped[1] = new Equipable(1, 1, 1);
            

            Console.WriteLine("Welcome to the tower!\n The only way out is up!\n");
            if (debug == true) { Console.WriteLine("Debug On"); }
            Console.WriteLine("\n Press any key to continue.");
            Console.ReadKey();
            Console.Clear();
            Console.WriteLine("Please enter your name.");
            player.name = Console.ReadLine();
            string filePath = Directory.GetParent(Directory.GetCurrentDirectory()).Parent.FullName + "/" + player.name + ".txt";
            Console.Clear();

            while (true)
            {
                Console.WriteLine($"Gold: {player.gold} / Attack: {player.equipped[1].damageMin} - {player.equipped[1].damageMax} / Defense " +
                $"{player.equipped[0].armour + player.equipped[2].armour + player.equipped[3].armour + player.equipped[4].armour + player.equipped[5].armour + player.equipped[6].armour}" +
                $" / Health: {player.health}/{player.maxHealth} / Potions: {player.potion}");


                Console.WriteLine($"\n Where do you want to go {player.name}?");
                Console.WriteLine("\n <Fight> Level " + player.heighestLevel + "\n <Inventory> \n <Shop> \n <Spells> \n <Potion> \n <Save> \n <Load> ");
                lastAction = Console.ReadLine();

                if (lastAction.ToLower().Contains("fight"))
                {
                    Fight();
                }
                else if (lastAction.ToLower().Contains("inv"))
                {
                    Inventory();
                }
                else if (lastAction.ToLower().Contains("shop"))
                {
                    Shop();
                }
                else if (lastAction.ToLower().Contains("spell"))
                {
                    Console.Clear();
                    Console.WriteLine("Not Yet Implimented");

                }
                else if (lastAction.ToLower().Contains("potion"))
                {
                    if (player.health != player.maxHealth)
                    {
                        if (player.potion > 0)
                        {
                            Console.Clear();
                            Console.WriteLine("Potion used.");
                            player.health += 50;
                            player.potion--;
                            if (player.health > 100)
                            {
                                player.health = 100;
                            }
                        }
                    }
                    else
                    {
                        Console.Clear();
                        Console.WriteLine("You are already at max health.");
                    }
                }
                else if (lastAction.ToLower().Contains("save"))
                {
                    try
                    {
                        CheckFile(filePath);
                        Save(filePath, player);
                        Console.Clear();
                        Console.WriteLine("Game Saved.");
                    }
                    catch (Exception ex)
                    {
                        Debug(ex.Message);
                    }
                }
                else if (lastAction.ToLower().Contains("load"))
                {
                    player = (Player)Load(filePath);

                }
                else
                {
                    Console.Clear();
                }
            }

            void Fight()
            {
                Console.Clear();
                Console.WriteLine("Enter floor number." + "\n\n Heighest Floor: " + player.heighestLevel + "\n Last Level: " + player.lastLevel);
                lastAction = Console.ReadLine();

                try //  try/catch for parse-int exception 
                {
                    if (int.Parse(lastAction) <= player.heighestLevel)
                    {
                        Enemy enemy = new Enemy();
                        Random rnd = new Random();
                        enemy.enemyLevel = int.Parse(lastAction);
                        int index = rnd.Next(enemy.enemyNames.Length);
                        Debug(index.ToString());
                        enemy.name = enemy.enemyNames[index];
                        if (firstBattle == true)
                        {   //first battle enemy just has a wooden sword.
                            enemy.equipped[1] = new Equipable(1, 1, 1);
                        }
                        else
                        {   //Give enemies access to new random armour every 5 levels and increase enemy max hp.
                            enemy.equipped[1] = new Equipable(1, rnd.Next(1, 5), enemy.enemyLevel);
                            if (enemy.enemyLevel > 5) { enemy.equipped[0] = new Equipable(0, rnd.Next(1, 5), enemy.enemyLevel); }
                            if (enemy.enemyLevel > 10) { enemy.equipped[5] = new Equipable(5, rnd.Next(1, 5), enemy.enemyLevel); enemy.maxHealth += 10; }
                            if (enemy.enemyLevel > 15) { enemy.equipped[4] = new Equipable(4, rnd.Next(1, 5), enemy.enemyLevel); enemy.maxHealth += 10; }
                            if (enemy.enemyLevel > 20) { enemy.equipped[6] = new Equipable(6, rnd.Next(1, 5), enemy.enemyLevel); enemy.maxHealth += 10; }
                            if (enemy.enemyLevel > 25) { enemy.equipped[3] = new Equipable(3, rnd.Next(1, 5), enemy.enemyLevel); enemy.maxHealth += 10; }
                            if (enemy.enemyLevel > 30) { enemy.equipped[2] = new Equipable(2, rnd.Next(1, 5), enemy.enemyLevel); enemy.maxHealth += 10; }
                        }
                        bool inCombat = true;
                        Console.Clear();
                        Fightinfo(enemy);
                        player.lastLevel = enemy.enemyLevel;
                        while (inCombat)
                        {
                            lastAction = Console.ReadLine();
                            Console.Clear();
                            if (lastAction.ToLower().Contains("attack"))
                            {
                                Fightinfo(enemy);
                                //you attack
                                // future paul make player and enemy superclass and clean this up
                                if (player.equipped[1].name == "Empty")
                                {
                                    Console.WriteLine($"\nYou attack {enemy.name} with your fist!");
                                }
                                else
                                {
                                    Console.WriteLine($"\nYou attack {enemy.name} with your {player.equipped[1].name}!");
                                }
                                int enemyArmour = enemy.equipped[0].armour + enemy.equipped[2].armour + enemy.equipped[3].armour + enemy.equipped[4].armour + enemy.equipped[5].armour + enemy.equipped[6].armour;
                                int playerDamage = rnd.Next(player.equipped[1].damageMin, player.equipped[1].damageMax + 1);
                                bool isCrit = false;
                                if (rnd.Next(1, 101) < player.equipped[1].critChance)
                                {
                                    isCrit = true;
                                    playerDamage *= 2;
                                }
                                int totalPlayerDamage = playerDamage - enemyArmour;
                                Debug(totalPlayerDamage.ToString());
                                if (totalPlayerDamage > 0)
                                {
                                    if (isCrit) { Console.WriteLine("Critical Strike!"); }
                                    Console.WriteLine($"You deal {totalPlayerDamage} damage!\n");
                                    enemy.health -= totalPlayerDamage;
                                    Debug("damage dealt \n EnemyHP: " + enemy.health);
                                }
                                else
                                {
                                    Console.WriteLine($"{enemy.name}'s armour blocks the attack!");
                                    Debug("damage blocked");
                                }

                                Debug("Enemy Attacks");
                                //enemy attacks
                                int playerArmour = player.equipped[0].armour + player.equipped[2].armour + player.equipped[3].armour + player.equipped[4].armour + player.equipped[5].armour + player.equipped[6].armour;
                                Debug(playerArmour.ToString());
                                int enemyDamage = rnd.Next(enemy.equipped[1].damageMin, enemy.equipped[1].damageMax + 1);
                                Debug(enemyDamage.ToString());
                                isCrit = false;
                                if (rnd.Next(1, 101) < enemy.equipped[1].critChance)
                                {
                                    isCrit = true;
                                    enemyDamage *= 2;
                                }
                                int totalEnemyDamage = enemyDamage - playerArmour;
                                Debug(totalEnemyDamage.ToString());
                                if (totalEnemyDamage > 0)
                                {
                                    if (isCrit) { Console.WriteLine("Critical Strike!"); }
                                    Console.WriteLine($"{enemy.name} strikes you with his {enemy.equipped[1].name}!");
                                    Console.WriteLine($"{enemy.name} deals {totalEnemyDamage} to you!");
                                    player.health -= totalEnemyDamage;
                                }
                                else
                                {
                                    Console.WriteLine("Your armour blocks the attack!");
                                }

                            }
                            else if (lastAction.ToLower().Contains("magic"))
                            {
                                Console.WriteLine("Not Yet Implemented");
                                Console.ReadKey();
                            }
                            else if (lastAction.ToLower().Contains("item"))
                            {
                                Console.WriteLine("Not Yet Implemented");
                                Console.ReadKey();
                            }
                            else if (lastAction.ToLower().Contains("run"))
                            {
                                if (rnd.Next(6) > 3)
                                {
                                    Console.WriteLine("You run away.");
                                    Console.ReadKey();
                                    break;
                                }
                                else
                                {
                                    Console.WriteLine("You were unable to run.");
                                    Console.ReadKey();
                                    Console.Clear();
                                    Fightinfo(enemy);
                                }
                            }
                            else if (lastAction.ToLower().Contains("debug"))
                            {
                                if (debug == true) { debug = false; }
                                else { debug = true; }
                            }
                            else
                            {
                                Fightinfo(enemy);
                            }

                            if (player.health <= 0)
                            {
                                Console.WriteLine("\nYou have been defeated!");
                                Console.WriteLine("Press any key to continue.");
                                Console.ReadKey();
                                Console.Clear();
                                player.health = player.maxHealth / 2;
                                break;
                            }
                            if (enemy.health <= 0)
                            {

                                Console.WriteLine($"\nYou have defeated {enemy.name}!");
                                if (enemy.enemyLevel == player.heighestLevel) { player.heighestLevel++; }
                                if (firstBattle) //give player full set of starter gear
                                {
                                    Console.WriteLine("\nYou have recieved:");
                                    int itt = 2;
                                    for (int i = 0; i < player.invent.Length; i++)
                                    {
                                        if (player.invent[i].name == "Empty" && itt != 7)
                                        {
                                            player.invent[i] = new Equipable(itt, 1, 0);
                                            Console.WriteLine(player.invent[i].name);
                                            itt++;
                                        }
                                    }

                                }
                                else
                                {
                                    //give player a random piece of gear @ enemy level
                                    for (int i = 0; i < player.invent.Length; i++)
                                    {
                                        if (player.invent[i].name == "Empty")
                                        {
                                            player.invent[i] = new Equipable(rnd.Next(6), rnd.Next(1, 5), enemy.enemyLevel);
                                            Console.WriteLine("\nYou have recieved:");
                                            Console.WriteLine(player.invent[i].name);
                                            break;
                                        }
                                        else
                                        {
                                            Console.WriteLine("You recieved no rewards\n Make space in your inventory.");
                                        }
                                    }
                                }
                                firstBattle = false;
                                Console.WriteLine("\nPress any key to continue.");
                                Console.ReadKey();
                                Console.Clear();
                                break;
                            }
                        }
                    }
                }
                catch (Exception e)
                {
                    Console.Clear();
                }
            }

            void Fightinfo(Enemy enemy) //combat UI
            {
                Console.WriteLine("You have encountered: " + enemy.name);
                Console.WriteLine("Enemy Health: " + enemy.health + "/" + enemy.maxHealth);

                Console.WriteLine("\nYour Health: " + player.health + "/" + player.maxHealth);
                Console.WriteLine("\n<Attack>  <Magic>" +
                                  "\n<Item>    <Run>");
            }

            void Inventory() // inventory management
            {
                bool inv = true;
                while (inv)
                {
                    Console.Clear();
                    Console.WriteLine("Your inventory contains:\n"); // display invent
                    int list = 0;
                    foreach (var item in player.invent)
                    {
                        if (player.invent[list].name != "Empty")
                        {
                            if (player.invent[list].equipmentSlot == 1) { Console.WriteLine(list + ": " + player.invent[list].name + " " + player.invent[list].damageMin + " - " + player.invent[list].damageMax); }
                            else { Console.WriteLine(list + ": " + player.invent[list].name + " " + player.invent[list].armour); }
                        }
                        else { Console.WriteLine(list + ": " + player.invent[list].name); }
                        list++;
                    }
                    list = 0;
                    Console.WriteLine("\nYour equipped items: \n"); // display equipped
                    foreach (var item in player.equipped)
                    {
                        if (player.equipped[list].name != "Empty")
                        {
                            if (player.equipped[list].equipmentSlot == 1) { Console.WriteLine(list + ": " + player.equipped[list].name + " " + player.equipped[list].damageMin + " - " + player.equipped[list].damageMax); }
                            else { Console.WriteLine(list + ": " + player.equipped[list].name + " " + player.equipped[list].armour); }
                        }
                        else { Console.WriteLine(list + ": " + player.equipped[list].name); }

                        list++;
                    }

                    Console.WriteLine("\nEnter 'Help' for inventory commands");

                    lastAction = Console.ReadLine();
                    if (lastAction.ToLower().Contains("help")) // display commands
                    {
                        Console.Clear();
                        Console.WriteLine("<Equip> Equip item from inventory.");
                        Console.WriteLine("<Remove> Remove an equiped item.");
                        Console.WriteLine("<Examine> Examine inventory item");
                        Console.WriteLine("<Sell> Sell an item");
                        Console.ReadKey();
                    }
                    else if (lastAction.ToLower().Contains("equip"))
                    {
                        try
                        {
                            int selectedSlot = FindNumberInString();
                            Debug(selectedSlot.ToString());
                            int equipSlot = player.invent[selectedSlot].equipmentSlot;
                            Debug(equipSlot.ToString());
                            if (player.equipped[equipSlot].name == "Empty") // inv to empty equip slot
                            {
                                player.equipped[equipSlot] = player.invent[selectedSlot];
                                player.invent[selectedSlot] = empty;
                            }
                            else // inv to occupied equip slot
                            {
                                Item holder = player.equipped[equipSlot];
                                player.equipped[equipSlot] = player.invent[selectedSlot];
                                player.invent[selectedSlot] = holder;
                            }
                        }
                        catch (Exception ex) { Debug("Handle exception properly later lol"); }

                    }
                    else if (lastAction.ToLower().Contains("remove"))
                    {
                        int selectedSlot = FindNumberInString();
                        bool itemMoved = false;
                        for (int i = 0; i < player.invent.Length; i++)
                        {
                            if (player.invent[i].name == "Empty")
                            {
                                player.invent[i] = player.equipped[selectedSlot];
                                player.equipped[selectedSlot] = empty;
                                itemMoved = true;
                            }
                        }
                        if (itemMoved == false)
                        {
                            Console.WriteLine("Inventory is full!");
                            Console.ReadKey();
                        }

                    }
                    else if (lastAction.ToLower().Contains("examine"))
                    {
                        FindNumberInString();
                        Console.Clear();
                        Console.WriteLine("Not yet Implemented");
                        Console.ReadKey();
                    }
                    else if (lastAction.ToLower().Contains("sell"))
                    {
                        int selectedSlot = FindNumberInString();
                        Console.Clear();
                        player.gold += player.invent[selectedSlot].value;
                        Console.WriteLine("Item sold for " + player.invent[selectedSlot].value);
                        player.invent[selectedSlot] = empty;
                        Console.ReadKey();
                    }
                    else
                    {
                        Console.Clear();
                        break;

                    }
                }
            }

            int FindNumberInString()
            {
                string selectedItem = "";
                for (int i = 0; i < lastAction.Length; i++)
                {
                    if (Char.IsDigit(lastAction[i]))
                    {
                        selectedItem += lastAction[i];
                    }
                }
                return int.Parse(selectedItem);
            }

            void Debug(string text)
            {
                if (debug == true)
                {
                    Console.WriteLine(text);
                    Console.ReadKey();
                }
            }

            void Shop()
            {
                bool shop = true;
                Console.Clear();
                ShopInfo();
                while (shop)
                {
                    lastAction = Console.ReadLine();
                    if (lastAction.ToLower().Contains("stone"))
                    {
                        Console.Clear();
                        Buy(20f, 1);
                    }
                    else if (lastAction.ToLower().Contains("spike"))
                    {
                        Console.Clear();
                        Buy(10f, 0);
                    }
                    else if (lastAction.ToLower().Contains("helm"))
                    {
                        Console.Clear();
                        Buy(10f, 2);
                    }
                    else if (lastAction.ToLower().Contains("chest"))
                    {
                        Console.Clear();
                        Buy(10f, 4);
                    }
                    else if (lastAction.ToLower().Contains("leg"))
                    {
                        Console.Clear();
                        Buy(10f, 5);
                    }
                    else if (lastAction.ToLower().Contains("glove"))
                    {
                        Console.Clear();
                        Buy(10f, 3);
                    }
                    else if (lastAction.ToLower().Contains("boot"))
                    {
                        Console.Clear();
                        Buy(10f, 6);
                    }
                    else if (lastAction.ToLower().Contains("potion"))
                    {
                        if (player.gold > 1.50f)
                        {
                            player.gold -= 1.50f;
                            player.potion++;
                            Console.Clear();
                            Console.WriteLine("Potion purchased.");
                        }
                        else
                        {
                            Console.Clear();
                            Console.WriteLine("You do not have enough gold.");
                        }
                    }
                    else
                    {
                        Console.Clear();
                        break;
                    }
                    ShopInfo();
                }
            }

            void ShopInfo()
            {
                Console.WriteLine("Gold: " + player.gold);
                Console.WriteLine("\nSharpening <Stone> +2 Damage to equipped weapon. - 20g");
                Console.WriteLine("Shield <Spike> +1 Damage to equipped shield. - 10g");
                Console.WriteLine("<Helm> Armour Patch +1 Armour to equipped helm - 10g");
                Console.WriteLine("<Chest> Armour Patch +1 Armour to equipped chest - 10g");
                Console.WriteLine("<Leg> Armour Patch +1 Armour to equipped leggings - 10g");
                Console.WriteLine("<Glove> Armour Patch +1 Armour to equipped gloves - 10g");
                Console.WriteLine("<Boot> Armour Patch +1 Armour to equipped boots - 10g");
                Console.WriteLine("x1 <Potion> - 1.50g\n");
            }

            void Buy(float gold, int slot)
            {

                if (player.equipped[slot].isMagic == false)
                {
                    if (player.gold > gold)
                    {
                        player.gold -= gold;
                        player.equipped[slot].isMagic = true;
                        Equipable item = (Equipable)player.equipped[slot];
                        item.Update();
                        Console.WriteLine("Item upgraded.");
                    }
                    else { Console.WriteLine("You do not have enough gold."); }
                }
                else if (player.equipped[slot].name == "Empty")
                {
                    Console.WriteLine("There is no item equipped in that slot.");
                }
                else
                {
                    Console.WriteLine("Item is already modified.");
                }

            }

            void CheckFile(string filePath)
            {
                Debug(filePath);
                if (File.Exists(filePath))
                {
                    Debug("File Exists");
                }
                else
                {
                    using (File.Create(filePath))
                    Debug("File Created");
                }
            }

            static void Save(string filePath, object obj)
            {
                using (Stream stream = File.Open(filePath, FileMode.Create))
                {
                    BinaryFormatter formatter = new BinaryFormatter();
                    formatter.Serialize(stream, obj);
                }
            }

            static object Load(string filePath)
            {
                using (Stream stream = File.Open(filePath, FileMode.Open))
                {
                    BinaryFormatter formatter = new BinaryFormatter();
                    return formatter.Deserialize(stream);
                }
            }
        }
    }
}