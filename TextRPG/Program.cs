using System.Net.Security;
using System.Xml.Schema;
using static TextRPG.Program;
using System.Threading;
using System.Diagnostics;

namespace TextRPG
{
    internal class Program
    {
        const int LENGTH = 20;
        const int SHOPLENGTH = 50;

        enum Lobby
        {
            State = 1,
            Inventory,
            Shop
        }

        public enum PowerType
        {
            Attack = 1,
            Defense
        };

        static public int InputNumber()
        {
            Console.WriteLine("원하시는 행동을 입력해주세요.");
            Console.Write(">> ");
            int num = int.Parse(Console.ReadLine());
            return num;
        }

        public class Player
        {
            int level = 1;
            string name = "르탄";
            string chad = "전사";
            int attack = 10;
            int defense = 5;
            int hp = 100;
            int gold = 1500;
            Item[] itemList = new Item[LENGTH];
            int itemIndex = 0;

            public int Level { get { return level; } set { level = value; } }
            public string Name { get { return name; } set { name = value; } }
            public string Chad { get { return chad; } set { chad = value; } }
            public int Attack { get { return attack; } set { attack = value; } }
            public int Defense { get { return defense; } set { defense = value; } }
            public int Hp { get { return hp; } set { hp = value; } }
            public int Gold { get { return gold; } set { gold = value; } }
            public Item[] ItemList { get { return itemList; } set { itemList = value; } }
            public int ItemIndex { get { return itemIndex; } set { itemIndex = value; } }

            public void addItem(Item item)
            {
                itemList[itemIndex++] = item;
            }

            public void showCharacterState()
            {
                while (true)
                {
                    Console.Clear();
                    Console.WriteLine("상태 보기");
                    Console.WriteLine("캐릭터의 정보가 표시됩니다.");
                    Console.WriteLine($"Lv. {level}");
                    Console.WriteLine($"이  름 : {name}");
                    Console.WriteLine($"Chad {chad}");

                    // 장비 공격력, 방어력
                    int eAttack = calcPower()[0], eDefense = calcPower()[1]; ;
                    if (eAttack == 0) Console.WriteLine($"공격력 : {attack}");
                    else Console.WriteLine($"공격력 : {attack} (+{eAttack})");
                    if (eDefense == 0) Console.WriteLine($"방어력 : {defense}");
                    else Console.WriteLine($"방어력 : {defense} (+{eDefense})");
                    Console.WriteLine($"체  력 : {hp}");
                    Console.WriteLine($"Gold : {gold}G \n");
                    Console.WriteLine("0. 나가기");

                    if (InputNumber() == 0)
                    {
                        Console.Clear();
                        break;
                    }
                }
            }

            // 장비 공격력, 방어력 계산
            public int[] calcPower()
            {
                int attack = 0;
                int defense = 0;
                for (int i = 0; i < itemList.Length; i++)
                {
                    if (itemList[i] != null && itemList[i].Equip)
                    {
                        if (itemList[i].PType == PowerType.Attack) attack += itemList[i].Power;
                        else if (itemList[i].PType == PowerType.Defense) defense += itemList[i].Power;
                    }
                }
                int[] arr = new int[] { attack, defense };
                return arr;
            }

            public void showItemList()
            {
                while (true)
                {
                    Console.Clear();
                    Console.WriteLine("인벤토리");
                    Console.WriteLine("보유 중인 아이템을 관리할 수 있습니다.\n");
                    Console.WriteLine("[아이템 목록]\n");
                    searchItem(itemList, false);

                    Console.WriteLine("1. 장착 관리");
                    Console.WriteLine("0. 나가기\n");

                    int num = InputNumber();
                    if (num == 1) { equipmentManager(); }
                    else if (num == 0)
                    {
                        Console.Clear();
                        break;
                    }
                }
            }

            public void searchItem(Item[] itemList, bool index)
            {
                for (int i = 0; i < itemList.Length; i++)
                {
                    if (itemList[i] == null) break;
                    string e = "";
                    string type;
                    if (itemList[i].Equip == true) { e = "[E]"; }
                    if (itemList[i].PType == PowerType.Attack) { type = "공격력 +"; }
                    else { type = "방어력 +"; }

                    if (index == true)
                    {
                        Console.WriteLine($"- {((i + 1) + " " + e + itemList[i].Name).PadRight(15)} | " +
                            $"{(type + itemList[i].Power).PadRight(8)} | {itemList[i].Explanation.PadRight(30)}");
                    }
                    else
                    {
                        Console.WriteLine($"- {(e + itemList[i].Name).PadRight(15)} | " +
                            $"{(type + itemList[i].Power).PadRight(8)} | {itemList[i].Explanation.PadRight(30)}");
                    }
                }
                Console.WriteLine();
            }

            public void equipmentManager()
            {
                while (true)
                {
                    Console.Clear();
                    Console.WriteLine("인벤토리 - 장착 관리");
                    Console.WriteLine("보유 중인 아이템을 관리할 수 있습니다.\n");
                    Console.WriteLine("[아이템 목록]\n");
                    searchItem(itemList, true);
                    Console.WriteLine("0. 나가기\n");

                    int num = InputNumber();
                    if (num == 0) break;

                    // 번호가 1일 때 인덱스는 0 이어야 됨
                    int newNum = num - 1;
                    if (newNum >= 0 && newNum < itemList.Length)
                    {
                        if (itemList[newNum] == null)
                        {
                            Console.WriteLine("잘못된 입력입니다.");
                            Thread.Sleep(300);
                            break;
                        }
                        else
                        {
                            if (itemList[newNum].Equip == true) itemList[newNum].Equip = false;
                            else itemList[newNum].Equip = true;
                        }
                        searchItem(itemList, true);
                    }
                }
            }
        }

        public class Item
        {
            string name;
            PowerType pType;
            int power;
            string explanation;
            int price;
            bool equip;
            bool isBuy;

            public string Name { get { return name; } set { name = value; } }
            public PowerType PType { get { return pType; } set { pType = value; } }
            public int Power { get { return power; } set { power = value; } }
            public string Explanation { get { return explanation; } set { explanation = value; } }
            public int Price { get { return price; } set { price = value; } }
            public bool Equip { get { return equip; } set { equip = value; } }
            public bool IsBuy { get { return isBuy; } set { isBuy = value; } }

            public Item(string name, PowerType pType, int power, string explanation, int price, bool equip = false, bool isBuy = false)
            {
                Name = name;
                PType = pType;
                Power = power;
                Explanation = explanation;
                Price = price;
                Equip = equip;
                IsBuy = isBuy;
            }
        }

        public class Shop
        {
            Player player;
            Item[] shopItemList = new Item[SHOPLENGTH];
            int shopItemIndex = 0;

            public Player Player { get { return player; } set { player = value; } }
            public Item[] ShopItemList { get { return shopItemList; } set { shopItemList = value; } }
            public int ShopItemIndex { get { return shopItemIndex; } set { shopItemIndex = value; } }

            public Shop(Player player)
            {
                Player = player;
            }

            public void addShopItem(Item item)
            {
                shopItemList[shopItemIndex++] = item;
            }

            public void showShopList()
            {
                while (true)
                {
                    Console.Clear();
                    Console.WriteLine("상점");
                    Console.WriteLine("필요한 아이템을 얻을 수 있는 상점입니다.\n");
                    Console.WriteLine("[보유 골드]");
                    Console.WriteLine($"{player.Gold} G \n");
                    Console.WriteLine("[아이템 목록]");
                    shopSearchItem(shopItemList, false);
                    Console.WriteLine("1. 아이템 구매");
                    Console.WriteLine("0. 나가기\n");

                    int num = InputNumber();
                    if (num == 1) { buyItem(); break; }
                    else if (num == 0) { Console.Clear(); break; }
                }
            }

            public void shopSearchItem(Item[] shopItemList, bool index)
            {
                for (int i = 0; i < shopItemList.Length; i++)
                {
                    if (shopItemList[i] == null) break;
                    string type;
                    string price = shopItemList[i].Price.ToString() + " G";
                    if (shopItemList[i].PType == PowerType.Attack) { type = "공격력 +"; }
                    else { type = "방어력 +"; }
                    for (int k = 0; k < player.ItemList.Length; k++)
                    {
                        if (player.ItemList[k] == null) break;
                        if (shopItemList[i].Name.Equals(player.ItemList[k].Name))
                        {
                            shopItemList[i].IsBuy = true;
                            price = "구매완료";
                            break;
                        }
                    }

                    if (index == true)
                    {
                        Console.WriteLine($"- {((i + 1) + " " + shopItemList[i].Name).PadRight(10)} | {(type + shopItemList[i].Power).PadRight(8)}" +
                           $" | {shopItemList[i].Explanation.PadRight(25)} | {price.PadRight(10)}");
                    }
                    else
                    {
                        Console.WriteLine($"- {(shopItemList[i].Name).PadRight(10)} | {(type + shopItemList[i].Power).PadRight(8)}" +
                           $" | {shopItemList[i].Explanation.PadRight(25)} | {price.PadRight(10)}");
                    }
                }
                Console.WriteLine();
            }

            public void buyItem()
            {
                while (true)
                {
                    Thread.Sleep(300);
                    Console.Clear();
                    Console.WriteLine("상점 - 아이템 구매");
                    Console.WriteLine("필요한 아이템을 얻을 수 있는 상점입니다.\n");
                    Console.WriteLine("[보유 골드]");
                    Console.WriteLine($"{player.Gold} G \n");
                    Console.WriteLine("[아이템 목록]");
                    shopSearchItem(shopItemList, true);
                    Console.WriteLine("0. 나가기\n");

                    int num = InputNumber();
                    if (num >= 1 && num <= shopItemIndex)
                    {
                        // 항목이 1이면 인덱스는 0 이어야 됨
                        int newNum = num - 1;
                        if (shopItemList[newNum].IsBuy)
                        {
                            Console.WriteLine("이미 구매한 아이템입니다.");
                            Thread.Sleep(300);
                        }
                        else
                        {
                            if (player.Gold >= shopItemList[newNum].Price)
                            {
                                Console.WriteLine("구매를 완료했습니다.");
                                player.Gold -= shopItemList[newNum].Price;
                                player.addItem(shopItemList[newNum]);
                                shopItemList[newNum].IsBuy = true;
                            }
                            else
                            {
                                Console.WriteLine("Gold가 부족합니다.");
                                Thread.Sleep(300);
                            }
                        }
                    }
                    else if (num < 0 && num > shopItemIndex)
                    {
                        Console.WriteLine("잘못된 입력입니다.");
                        Thread.Sleep(300);
                    }
                    else if (num == 0)
                    {
                        break;
                    }
                }
            }
        }
        static void Main(string[] args)
        {
            Player player = new Player();
            player.addItem(new Item("무쇠갑옷", PowerType.Defense, 5, "무쇠로 만들어져 튼튼한 갑옷입니다.", 1800, true, true));
            player.addItem(new Item("스파르타의 창", PowerType.Attack, 7, "스파르타의 전사들이 사용했다는 전설의 창입니다.", 2700, true, true));
            player.addItem(new Item("낡은 검", PowerType.Attack, 2, "쉽게 볼 수 있는 낡은 검입니다.", 600, false, true));
            Shop shop = new Shop(player);
            shop.addShopItem(new Item("수련자 갑옷", PowerType.Defense, 5, "수련에 도움을 주는 갑옷입니다.", 1000));
            shop.addShopItem(new Item("무쇠갑옷", PowerType.Defense, 9, "무쇠로 만들어져 튼튼한 갑옷입니다.", 1800));
            shop.addShopItem(new Item("스파르타의 갑옷", PowerType.Defense, 15, "스파르타의 전사들이 사용했다는 전설의 갑옷입니다.", 3500));
            shop.addShopItem(new Item("낡은 검", PowerType.Attack, 2, "쉽게 볼 수 있는 낡은 검 입니다.", 600));
            shop.addShopItem(new Item("청동 도끼", PowerType.Attack, 5, "어디선가 사용됐던거 같은 도끼입니다.", 1500));
            shop.addShopItem(new Item("스파르타의 창", PowerType.Attack, 7, "스파르타의 전사들이 사용했다는 전설의 창입니다.", 2700));

            while (true)
            {
                Console.Clear();
                Console.WriteLine("스파르타 마을에 오신 여러분 환영합니다.");
                Console.WriteLine("이곳에서 던전으로 들어가기전 활동을 할 수 있습니다.\n");
                Console.WriteLine("1. 상태 보기 \n2. 인벤토리 \n3. 상점\n");
                int num = InputNumber();

                if (num >= (int)Lobby.State && num <= (int)Lobby.Shop)
                {
                    switch (num)
                    {
                        case (int)Lobby.State: player.showCharacterState(); break;
                        case (int)Lobby.Inventory: player.showItemList(); break;
                        case (int)Lobby.Shop: shop.showShopList(); break;
                    }
                }
                else
                {
                    Console.WriteLine("잘못된 입력입니다.");
                    Thread.Sleep(300);
                }
            }
        }
    }
}
