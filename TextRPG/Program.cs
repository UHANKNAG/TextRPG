using System.Reflection.Emit;
using System.Xml.Linq;
using static TextRPG.Inventory;
using static TextRPG.Store;

namespace TextRPG
{
    public class Status
    {
        public int level = 1;
        public string name = "Chad";
        public string job = "전사";
        public int offense = 10;
        public int defense = 5;
        public int stamina = 100;
        public int gold = 1500;

        public void DisplayStatus()
        {
            Console.Clear();
            Console.WriteLine("상태 보기 \n캐릭터의 정보가 표시됩니다.\n");

            Console.WriteLine($"Lv. {level}");
            Console.WriteLine($"{name} ( {job} )");
            Console.WriteLine($"공격력: {offense}");
            Console.WriteLine($"방어력: {defense}");
            Console.WriteLine($"체 력: {stamina}");
            Console.WriteLine($"Gold: {gold} G");
            Console.WriteLine();

            Console.WriteLine("0. 나가기\n");

            Console.WriteLine("원하시는 행동을 입력해 주세요.");
            int input = int.Parse(Console.ReadLine());
            if (input == 0)
                return;
        }
    }

    public class Inventory
    {
        public struct Item {
            public bool isEquip;   // 장비를 장착했는가?
            public string itemName;
            public string ability;
            public int value;
            public string description;

            public void ItemInfo()
            {
                Console.Write("- ");
                if (isEquip) {
                    Console.Write("[E]");
                }
                Console.WriteLine($"{itemName}\t\t\\ {ability} +{value}\t\\ {description}");
            }
        }

        public Item[] items = new Item[6];

        public void DisplayIventory()
        {
            Console.Clear();
            Console.WriteLine("인벤토리 \n보유 중인 아이템을 관리할 수 있습니다.\n");

            Console.WriteLine("[아이템 목록]");
            foreach (Item t in items) {
                t.ItemInfo();
            }
            Console.WriteLine();

            Console.WriteLine("1. 장착 관리");
            Console.WriteLine("0. 나가기\n");

            Console.WriteLine("원하시는 행동을 입력해 주세요.");
            int input = int.Parse(Console.ReadLine());
            switch (input)
            {
                case 0: return;
                case 1: return;
            }     
        }
    }

    public class Store()
    {
        public struct Product
        {
            public bool isSoldout;   // 팔렸는가?
            public string pName;
            public string pAbility;
            public int pValue;
            public string pDescription;
            public int price;

            public void ProductInfo()
            {
                Console.Write($"{pName}\t\\ {pAbility} +{pValue}\t\\ {pDescription}\t\t\\ ");
                if (isSoldout)
                {
                    Console.WriteLine("구매 완료");
                }
                else {
                    Console.WriteLine($"{price} G");
                }
            }
        }

        Product[] products = new Product[]
        {
            new Product
            {
                isSoldout = false,
                pName = "수련자 갑옷",
                pAbility = "방어력",
                pValue = 5,
                pDescription = "수련에 도움을 주는 갑옷입니다.",
                price = 1000
            },
            new Product
            {
                isSoldout = true,
                pName = "무쇠갑옷",
                pAbility = "방어력",
                pValue = 9,
                pDescription = "무쇠로 만들어져 튼튼한 갑옷입니다.",
                price = 1500
            },
            new Product
            {
                isSoldout = false,
                pName = "스파르타 갑옷",
                pAbility = "방어력",
                pValue = 15,
                pDescription = "스파르타 전사들이 사용한 갑옷입니다.",
                price = 3500
            },
            new Product
            {
                isSoldout = false,
                pName = "낡은 검",
                pAbility = "공격력",
                pValue = 2,
                pDescription = "쉽게 볼 수 있는 낡은 검입니다.",
                price = 600
            },
            new Product
            {
                isSoldout = false,
                pName = "청동 도끼",
                pAbility = "공격력",
                pValue = 5,
                pDescription = "어디선가 사용됐던 것 같은 도끼입니다.",
                price = 1500
            },
            new Product
            {
                isSoldout = true,
                pName = "스파르타 창",
                pAbility = "공격력",
                pValue = 7,
                pDescription = "스파르타 전사들이 사용한 창입니다.",
                price = 2500
            },
        };
        bool  isBuy= false;

        public void DisplayStore(Status status, Inventory inventory)
        {
            Console.Clear();
            Console.Write("상점 ");
            if (isBuy)
                Console.Write("- 아이템 구매");
            Console.WriteLine("\n필요한 아이템을 얻을 수 있는 상점입니다.\n");

            Console.WriteLine($"[보유 골드]\n{status.gold} G\n");

            Console.WriteLine("[아이템 목록]");
            PrintItem(isBuy);

            if (isBuy)
            {
                Console.WriteLine("0. 나가기\n");
            }
            else
            {
                Console.WriteLine("1. 아이템 구매");
                Console.WriteLine("0. 나가기\n");
            }
            
            Console.WriteLine("원하시는 행동을 입력해 주세요.");
            int input = int.Parse(Console.ReadLine());

            if (isBuy)
            {
                if (input == 0) {
                    return;
                }
                else
                    BuyProduct(input, status, inventory);
            }
            else
            {
                switch (input)
                {
                    case 0: return;
                    case 1:
                        isBuy = true;
                        DisplayStore(status, inventory);
                        break;
                }
                Console.WriteLine(status.gold);
            }
            
        }

        public void PrintItem(bool showIndex)
        {
            for (int i = 0; i < products.Length; i++)
            {
                if (showIndex)
                    Console.Write($"- {i + 1} ");
                else
                    Console.Write("- ");

                products[i].ProductInfo();
            }
            Console.WriteLine();
        }

        public void BuyProduct(int input, Status status, Inventory inventory)
        {
            int gold = status.gold;
            Product p = products[input-1];
            Inventory inv = inventory;
            if (input > 0 && input < products.Length) {
                if(!products[input-1].isSoldout) // 안 팔린 경우
                {
                    // 보유 금액이 충분하다면
                    if(p.price <= gold)
                    {
                        // 문구 출력
                        Console.WriteLine("구매를 완료하였습니다.");
                        // 재화 감소
                        gold = gold - p.price;
                        // 인벤토리에 아이템 추가
                        for (int i = 0; i < inv.items.Length; i++) {
                            if (inv.items[i].Equals(default(Item))) {
                                inv.items[i].itemName = p.pName;
                                inv.items[i].ability = p.pAbility;
                                inv.items[i].value = p.pValue;
                                inv.items[i].description = p.pDescription;

                                break;
                            }
                        }
                        // 상점에 구매완료 표시
                        p.isSoldout = true;
                    }
                    else    // 보유 금액이 부족하다면
                    {
                        Console.WriteLine("Gold가 부족합니다.");
                    }
                }
                else    // 팔린 경우
                {
                    Console.WriteLine("이미 구매한 아이템입니다.");
                }
            }
            else
            {
                Console.WriteLine("잘못된 입력입니다.");
            }
        }
    }


    internal class Program
    {
        static void Main(string[] args)
        {
            Status status = new Status();
            Inventory inventory = new Inventory();
            Store store = new Store();
            bool isSuccess = false; 

            Console.WriteLine("스파르타 마을에 오신 여러분 환영합니다.");
            Console.WriteLine("이곳에서 던전으로 들어가기 전 활동을 할 수 있습니다.\n");
            Console.WriteLine("1. 상태 보기 \n2. 인벤토리 \n3. 상점\n");
            Console.WriteLine("원하시는 행동을 입력해 주세요.");

            string input = Console.ReadLine();
            isSuccess = int.TryParse(input, out int select);

            if (isSuccess)
            {
                if (select == 1 || select == 2 || select == 3) {
                    switch (select)
                    {
                        case 1:
                            status.DisplayStatus();
                            break;
                        case 2:
                            inventory.DisplayIventory();
                            break;
                        case 3:
                            store.DisplayStore(status, inventory);
                            break;
                    }
                }
                else
                {
                    Console.WriteLine("잘못된 입력입니다.");
                }
            }
            else {
                Console.WriteLine("잘못된 입력입니다.");
            }
        }
    }
}
