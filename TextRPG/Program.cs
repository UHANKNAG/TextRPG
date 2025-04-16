using System.ComponentModel.Design;
using System.Net.NetworkInformation;
using System.Reflection.Emit;
using System.Xml.Linq;
using static System.Formats.Asn1.AsnWriter;
using static TextRPG.Inventory;
using static TextRPG.Store;

namespace TextRPG
{
    public class Status
    {
        public int level = 1;
        public string name = "";
        public string job = "전사";
        public int offense = 10;
        public int defense = 5;
        public int stamina = 100;
        public int gold = 30000;

        public void DisplayStatus()
        {
            bool isSuccess = false; // 입력 관리

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
            isSuccess = int.TryParse(Console.ReadLine(), out int input);

            if (isSuccess)
            {
                if (input == 0)
                {
                    Console.Clear();
                    return;
                }
                else
                {
                    Console.Clear();
                    Console.WriteLine("잘못된 입력입니다.");
                }
            }
            else
            {
                Console.Clear();
                Console.WriteLine("잘못된 입력입니다.");
            }
            
        }
    }

    public class Inventory
    {
        public Item[] items = new Item[6];
        bool isInstall = false; // 장착 관리
        bool isSuccess = false; // 입력 관리

        public struct Item {
            public bool isEquip;   // 장비를 장착했는가?
            public string itemName;
            public string ability;
            public int value;
            public string description;

            public void ItemInfo()
            {
                Console.WriteLine($"{itemName}\t\\ {ability} +{value}\t\\ {description}");
            }
        }

        public void PrintItem(bool showIndex)
        {
            for (int i = 0; i < items.Length; i++)
            {
                if (items[i].Equals(default(Item)))
                {
                    break;
                }

                Console.Write("- ");
                if (showIndex)
                {
                    Console.Write($"{i + 1}\t");
                }
                if (items[i].isEquip)
                {
                    Console.Write("[E] ");
                }
                items[i].ItemInfo();
            }
            Console.WriteLine();
        }

        public void DisplayIventory(Status status)
        {

            Console.Clear();
            Console.Write("인벤토리");
            if (isInstall)
                Console.Write(" - 장착 관리");
            Console.WriteLine("\n보유 중인 아이템을 관리할 수 있습니다.\n");

            Console.WriteLine("[아이템 목록]");
            PrintItem(isInstall);

            Console.WriteLine();

            if (isInstall)
            {
                Console.WriteLine("0. 나가기\n");
            }
            else
            {
                Console.WriteLine("1. 장착 관리");
                Console.WriteLine("0. 나가기\n");
            }

            Console.WriteLine("원하시는 행동을 입력해 주세요.");
            isSuccess = int.TryParse(Console.ReadLine(), out int input);

            if (isSuccess) {
                if (isInstall)
                {
                    if (input == 0)
                    {
                        Console.Clear();
                        return;
                    }
                    else
                    {
                        Console.Clear();
                        InstallManagement(input, status);
                        isInstall = false;
                    }

                }
                else
                {
                    switch (input)
                    {
                        case 0:
                            Console.Clear();
                            return;
                        case 1:
                            isInstall = true;
                            DisplayIventory(status);
                            break;
                        default:
                            Console.Clear();
                            Console.WriteLine("잘못된 입력입니다.");
                            break;
                    }
                }
            }
            else
            {
                Console.Clear();
                Console.WriteLine("잘못된 입력입니다.");
            }
             
        }

        public void InstallManagement(int input, Status status)
        {
            if (input > 0 && input < items.Length + 1)
            {
                // 아이템 개수보다 높은 숫자를 입력할 경우
                if (items[input-1].Equals(default(Item)))
                {
                    Console.WriteLine("잘못된 입력입니다.");
                }
                else
                {
                    if (!items[input - 1].isEquip)  // 장착을 안 하고 있는 경우
                    {
                        // 장착 [E] 표시 추가
                        items[input - 1].isEquip = true;
                        Console.WriteLine($"{items[input - 1].itemName}을(를) 장착하였습니다.");
                        // 능력치 반영 (더하기)
                        if (items[input - 1].ability == "공격력")
                        {
                            status.offense += items[input - 1].value;
                        }
                        else
                        {
                            status.defense += items[input - 1].value;
                        }

                    }
                    else
                    {
                        items[input - 1].isEquip = false;
                        Console.WriteLine($"{items[input - 1].itemName}을(를) 장착 해제하였습니다.");
                        // 능력치 반영 (빼기)
                        if (items[input - 1].ability == "공격력")
                        {
                            status.offense -= items[input - 1].value;
                        }
                        else
                        {
                            status.defense -= items[input - 1].value;
                        }
                    }
                }
            }
            else
            {
                Console.Clear();
                Console.WriteLine("잘못된 입력입니다.");
            }
        }
    }

    public class Store
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
                pName = "많이 낡은 검",
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
        bool isBuy= false;      // 입점 관리
        bool isSuccess = false; // 입력 관리

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
            isSuccess = int.TryParse(Console.ReadLine(), out int input);

            if (isSuccess)
            {
                if (isBuy)
                {
                    if (input == 0)
                    {
                        Console.Clear();
                        return;
                    }
                    else
                    {
                        Console.Clear();
                        BuyProduct(input, status, ref inventory, ref products);
                        isBuy = false;
                    }
                }
                else
                {
                    switch (input)
                    {
                        case 0:
                            Console.Clear();
                            return;
                        case 1:
                            isBuy = true;
                            DisplayStore(status, inventory);
                            break;
                        default:
                            Console.Clear();
                            Console.WriteLine("잘못된 입력입니다.");
                            break;
                    }
                }
            }
            else
            {
                Console.Clear();
                Console.WriteLine("잘못된 입력입니다.");
            }
        }

        public void PrintItem(bool showIndex)
        {
            for (int i = 0; i < products.Length; i++)
            {
                if (showIndex)
                    Console.Write($"- {i + 1}\t");
                else
                    Console.Write("- ");

                products[i].ProductInfo();
            }
            Console.WriteLine();
        }

        public void BuyProduct(int input, Status status, ref Inventory inventory, ref Product[] products)
        {
            Inventory inv = inventory;
            if (input > 0 && input < products.Length + 1) {
                if(!products[input-1].isSoldout) // 안 팔린 경우
                {
                    // 보유 금액이 충분하다면
                    if(products[input - 1].price <= status.gold)
                    {
                        // 문구 출력
                        Console.Clear();
                        Console.WriteLine("구매를 완료하였습니다.\n");
                        // 재화 감소
                        status.gold = status.gold - products[input - 1].price;
                        // 인벤토리에 아이템 추가
                        for (int i = 0; i < inv.items.Length; i++) {
                            if (inv.items[i].Equals(default(Item))) {
                                inv.items[i].itemName = products[input - 1].pName;
                                inv.items[i].ability = products[input - 1].pAbility;
                                inv.items[i].value = products[input - 1].pValue;
                                inv.items[i].description = products[input - 1].pDescription;

                                break;
                            }
                        }
                        // 상점에 구매완료 표시
                        products[input - 1].isSoldout = true;
                    }
                    else    // 보유 금액이 부족하다면
                    {
                        Console.Clear();
                        Console.WriteLine("Gold가 부족합니다.");
                    }
                }
                else    // 팔린 경우
                {
                    Console.Clear();
                    Console.WriteLine("이미 구매한 아이템입니다.");
                }
            }
            else
            {
                Console.Clear();
                Console.WriteLine("잘못된 입력입니다.");
            }
        }
    }

    internal class Program
    {
        bool isEnd = false;

        static void Main(string[] args)
        {
            Program p = new Program();
            Status status = new Status();
            Inventory inventory = new Inventory();
            Store store = new Store();

            Console.WriteLine("스파르타 마을에 오신 여러분 환영합니다.");
            Console.WriteLine("이곳에서 던전으로 들어가기 전 활동을 할 수 있습니다.\n");

            Console.WriteLine("이름을 입력해 주세요");
            status.name = Console.ReadLine();
            Console.WriteLine($"입력하신 이름은 {status.name}입니다.");
            Console.WriteLine();

            do
            {
                StartGame(p, status, inventory, store);
            } while (!p.isEnd);
            
        }

        static void StartGame(Program p, Status status, Inventory inventory, Store store)
        {
            bool isSuccess = false;

            Console.WriteLine("1. 상태 보기 \n2. 인벤토리 \n3. 상점\n0. 끝내기\n");
            Console.WriteLine("원하시는 행동을 입력해 주세요.");

            string input = Console.ReadLine();
            
            isSuccess = int.TryParse(input, out int select);

            if (isSuccess)
            {
                if (select >= 0 && select < 4)
                {
                    switch (select)
                    {
                        case 0:
                            p.isEnd = true;
                            Console.WriteLine("게임을 종료합니다.");
                            break;
                        case 1:
                            status.DisplayStatus();
                            break;
                        case 2:
                            inventory.DisplayIventory(status);
                            break;
                        case 3:
                            store.DisplayStore(status, inventory);
                            break;
                    }
                }
                else
                {
                    Console.Clear();
                    Console.WriteLine("잘못된 입력입니다.\n");
                }
            }
            else
            {
                Console.Clear();
                Console.WriteLine("잘못된 입력입니다.\n");
            }
        }
    }
}
