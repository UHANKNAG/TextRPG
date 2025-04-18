﻿using System.ComponentModel.Design;
using System.Net.NetworkInformation;
using System.Reflection.Emit;
using System.Xml.Linq;
using static System.Formats.Asn1.AsnWriter;
using static TextRPG.Inventory;

namespace TextRPG
{
    public class Status
    {
        public int level = 1;
        public string name = "";
        public string job = "전사";
        public float offense = 10f;
        public int defense = 5;
        public int stamina = 100;
        public int gold = 3000;

        public bool setWeapon = false;
        public bool setArmor = false;

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

    public class Rest
    {
        int restPrice = 500;

        public void DisplayRest(Status status)
        {
            Console.Clear();
            Console.WriteLine("휴식하기");
            Console.WriteLine($"{restPrice} G를 내면 체력을 회복할 수 있습니다. (보유 골드: {status.gold} G) \n");

            Console.WriteLine("1. 휴식하기");
            Console.WriteLine("0. 나가기");
            Console.WriteLine();

            Console.WriteLine("원하시는 행동을 입력해 주세요.");
            bool isSuccess = int.TryParse(Console.ReadLine(), out int input);

            if (isSuccess)
            {
                switch (input)
                {
                    case 0: 
                        Console.Clear();
                        return;
                    case 1:
                        Console.Clear();
                        TakeRest(status);
                        break;
                    default: 
                        Console.Clear();
                        Console.WriteLine("잘못된 입력입니다.");
                        break;
                }
            }
            else
            {
                Console.Clear();
                Console.WriteLine("잘못된 입력입니다.");
            }
        }

        public void TakeRest(Status status)
        {
            // 돈이 충분하면
            if (status.gold >= restPrice)
            {
                if (status.stamina < 100)
                {
                    status.gold -= restPrice;
                    status.stamina = 100;
                    Console.WriteLine("체력을 회복하였습니다.");
                }
                else
                {
                    Console.WriteLine("체력이 모두 회복되어 있습니다.");
                }
            }
            else
            {
                Console.WriteLine("Gold가 부족합니다.");
            }
        }
    }

    public class Inventory
    {
        public Item[] items = new Item[10];
        bool isInstall = false; // 장착 관리
        bool isSuccess = false; // 입력 관리
        public int count = 0;

        public struct Item
        {
            public bool isEquip;   // 장비 장착 여부
            public string itemName;
            public bool ability;    // 공격력이면 true, 방어력이면 false
            public int value;
            public string description;
            public int itemPrice;

            public void ItemInfo()
            {
                string abilityName;
                if (ability)
                    abilityName = "공격력";
                else
                    abilityName = "방어력";
                
                Console.WriteLine($"{itemName}\t\\ {abilityName} +{value}\t\\ {description}");
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
                count++;
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

            if (isSuccess)
            {
                if (isInstall)
                {
                    isInstall = false;
                    if (input == 0)
                    {
                        Console.Clear();
                        return;
                    }
                    else
                    {
                        Console.Clear();
                        InstallManagement(input, status);
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
                if (items[input - 1].Equals(default(Item)))
                {
                    Console.WriteLine("잘못된 입력입니다.");
                }
                else
                {
                    if (!items[input - 1].isEquip)  // 장착을 안 하고 있는 경우
                    {
                        // 능력치 반영 (더하기)
                        // 공격력일 경우
                        if (items[input - 1].ability)
                        {
                            if (status.setWeapon)
                            {
                                for (int i = 0; i < items.Length; i++)
                                {
                                    if (items[i].Equals(default(Item)))
                                        break;

                                    if(items[i].isEquip && items[i].ability)
                                    {
                                        items[i].isEquip = false;
                                        Uninstall(i, status);
                                        break;
                                    } 
                                }

                            }
                            status.offense += items[input - 1].value;
                            status.setWeapon = true;
                        }
                        // 방어력일 경우
                        else
                        {
                            if (status.setArmor)
                            {
                                for (int i = 0; i < items.Length; i++)
                                {
                                    if (items[i].Equals(default(Item)))
                                        break;

                                    if (items[i].isEquip && !items[i].ability)
                                    {
                                        items[i].isEquip = false;
                                        Uninstall(i, status);
                                        break;
                                    }
                                }

                            }
                            status.defense += items[input - 1].value;
                            status.setArmor = true;
                        }
                        // 장착 [E] 표시 추가
                        items[input - 1].isEquip = true;
                        Console.WriteLine($"{items[input - 1].itemName}을(를) 장착하였습니다.");
                    }
                    else
                    {
                        //// 장착 [E] 표시 빼기
                        //items[input - 1].isEquip = false;
                        //Console.WriteLine($"{items[input - 1].itemName}을(를) 장착 해제하였습니다.");
                        //// 능력치 반영 (빼기)
                        //if (items[input - 1].ability)
                        //{
                        //    status.offense -= items[input - 1].value;
                        //}
                        //else
                        //{
                        //    status.defense -= items[input - 1].value;
                        //}

                        Uninstall(input - 1, status);
                    }
                }
            }
            else
            {
                Console.Clear();
                Console.WriteLine("잘못된 입력입니다.");
            }
        }  
        
        // 장착 해제 함수
        public void Uninstall(int input, Status status)
        {
            // 장착 [E] 표시 빼기
            items[input].isEquip = false;
            Console.WriteLine($"{items[input].itemName}을(를) 장착 해제하였습니다.");
            // 능력치 반영 (빼기)
            if (items[input].ability)
            {
                status.offense -= items[input].value;
                status.setWeapon = false;
            }
            else
            {
                status.defense -= items[input].value;
                status.setArmor = false;
            }
        }
    }

    public class Store
    {
        bool isBuy = false;     // 구매 관리
        bool isSell = false;    // 판매 관리
        bool isSuccess = false; // 입력 관리

        public struct Product
        {
            public bool isSoldout;   // 팔렸는가?
            public string pName;
            public bool pAbility;   // 공격력이면 true 방어력이면 false
            public int pValue;
            public string pDescription;
            public int price;

            public void ProductInfo()
            {
                string abilityName;
                if (pAbility)
                    abilityName = "공격력";
                else
                    abilityName = "방어력";

                Console.Write($"{pName}\t\\ {abilityName} +{pValue}\t\\ {pDescription}\t\t\\ ");
                if (isSoldout)
                {
                    Console.WriteLine("구매 완료");
                }
                else
                {
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
                pAbility = false,
                pValue = 5,
                pDescription = "수련에 도움을 주는 갑옷입니다.",
                price = 1000
            },
            new Product
            {
                isSoldout = false,
                pName = "무쇠갑옷",
                pAbility = false,
                pValue = 9,
                pDescription = "무쇠로 만들어져 튼튼한 갑옷입니다.",
                price = 1500
            },
            new Product
            {
                isSoldout = false,
                pName = "스파르타 갑옷",
                pAbility = false,
                pValue = 15,
                pDescription = "스파르타 전사들이 사용한 갑옷입니다.",
                price = 3500
            },
            new Product
            {
                isSoldout = false,
                pName = "많이 낡은 검",
                pAbility = true,
                pValue = 2,
                pDescription = "쉽게 볼 수 있는 낡은 검입니다.",
                price = 600
            },
            new Product
            {
                isSoldout = false,
                pName = "청동 도끼",
                pAbility = true,
                pValue = 5,
                pDescription = "어디선가 사용됐던 것 같은 도끼입니다.",
                price = 1500
            },
            new Product
            {
                isSoldout = false,
                pName = "스파르타 창",
                pAbility = true,
                pValue = 7,
                pDescription = "스파르타 전사들이 사용한 창입니다.",
                price = 2500
            },
            new Product
            {
                isSoldout = false,
                pName = "광선검",
                pAbility = true,
                pValue = 20,
                pDescription = "전설에 나온다는 전설의 검입니다.",
                price = 2500
            }
        };

        public void DisplayStore(Status status, Inventory inventory)
        {
            Console.Clear();
            Console.Write("상점 ");
            if (isBuy)
                Console.Write("- 아이템 구매");
            if (isSell)
                Console.Write("- 아이템 판매");

            Console.WriteLine("\n필요한 아이템을 얻을 수 있는 상점입니다.\n");

            Console.WriteLine($"[보유 골드]\n{status.gold} G\n");

            Console.WriteLine("[아이템 목록]");
            if (isSell)
            {
                inventory.PrintItem(true);
            }
            else
            {
                PrintProduct(isBuy);
            }

            if (isBuy || isSell)
            {
                Console.WriteLine("0. 나가기\n");
            }
            else
            {
                Console.WriteLine("1. 아이템 구매");
                Console.WriteLine("2. 아이템 판매");
                Console.WriteLine("0. 나가기\n");
            }

            Console.WriteLine("원하시는 행동을 입력해 주세요.");
            isSuccess = int.TryParse(Console.ReadLine(), out int input);

            if (isSuccess)
            {
                if (isBuy)
                {
                    isBuy = false;
                    switch (input)
                    {
                        case 0:
                            Console.Clear();
                            return;

                        default:
                            Console.Clear();
                            BuyProduct(input, status, ref inventory, ref products); 
                            break;
                    }
                }
                else if (isSell)
                {
                    isSell = false;
                    switch (input)
                    {
                        case 0:
                            Console.Clear();
                            return;

                        default:
                            Console.Clear();
                            SellItem(input, status, ref inventory, ref products);
                            break;
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
                        case 2:
                            isSell = true;
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

        public void PrintProduct(bool isBuy)
        {
            for (int i = 0; i < products.Length; i++)
            {
                if (isBuy)
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
            if (input > 0 && input < products.Length + 1)
            {
                if (!products[input - 1].isSoldout) // 안 팔린 경우
                {
                    // 보유 금액이 충분하다면
                    if (products[input - 1].price <= status.gold)
                    {
                        // 문구 출력
                        Console.Clear();
                        Console.WriteLine("구매를 완료하였습니다.\n");
                        // 재화 감소
                        status.gold = status.gold - products[input - 1].price;
                        // 인벤토리에 아이템 추가
                        for (int i = 0; i < inv.items.Length; i++)
                        {
                            if (inv.items[i].Equals(default(Item)))
                            {
                                inv.items[i].itemName = products[input - 1].pName;
                                inv.items[i].ability = products[input - 1].pAbility;
                                inv.items[i].value = products[input - 1].pValue;
                                inv.items[i].description = products[input - 1].pDescription;
                                inv.items[i].itemPrice = products[input - 1].price;

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

        public void SellItem(int input, Status status, ref Inventory inventory, ref Product[] products)
        {
            Item[] tempItems = new Item[10];
            bool flag = false;

            // 입력이 인벤토리의 0보다 크고 아이템의 num보다 작다면
            if (input > 0 && input < inventory.count + 1)
            {
                Console.Clear();
                Console.WriteLine("판매가 완료되었습니다.\n");

                // 85%의 재화 돌려 주고
                float resellPrice = inventory.items[input - 1].itemPrice * 0.85f;
                status.gold += (int)resellPrice;

                // 장착하고 있었다면 장착 해제
                if (inventory.items[input - 1].isEquip)
                {
                    inventory.items[input - 1].isEquip = false;

                    // 능력치 제거
                    if (inventory.items[input - 1].ability)
                    {
                        status.offense -= inventory.items[input - 1].value;
                    }
                    else
                    {
                        status.defense -= inventory.items[input - 1].value;
                    }
                }

                // 상점 입점
                for (int i = 0; i <= products.Length; i++)
                {
                    if (inventory.items[input - 1].itemName == products[i].pName)
                    {
                        products[i].isSoldout = false;
                        break;
                    }
                }

                // 인벤토리에서 제거
                for (int i = 0; i < inventory.items.Length; i++)
                {
                    if (inventory.items[i].Equals(default(Item)))
                    {
                        break;
                    }

                    // 조건 설정 잘할 것
                    if (i >= (input - 1))
                    { 
                        tempItems[i] = inventory.items[i + 1];
                    }
                    else
                    {
                        tempItems[i] = inventory.items[i];
                    }  
                }
                inventory.items = tempItems;
            }
            else
            {
                Console.Clear();
                Console.WriteLine("잘못된 입력입니다.");
            }
        }
    }

    public class Dungeon
    {
        public int countClear = 0;

        public void DisplayDungeon(Status status)
        {
            Console.Clear();
            Console.WriteLine("던전 입장");
            Console.WriteLine("이곳에서 던전의 난이도를 선택할 수 있습니다.");
            Console.WriteLine();

            Console.WriteLine("1. 쉬운 던전\t\\ 방어력 5 이상 권장");
            Console.WriteLine("2. 일반 던전\t\\ 방어력 11 이상 권장");
            Console.WriteLine("3. 어려운 던전\t\\ 방어력 17 이상 권장");
            Console.WriteLine("0. 나가기");
            Console.WriteLine();

            Console.WriteLine("원하시는 행동을 입력해 주세요.");
            bool isSuccess = int.TryParse(Console.ReadLine(), out int input);

            if (isSuccess) { 
                switch(input)
                {
                    case 0:
                        Console.Clear();
                        break;

                    case 1: 
                        Console.Clear(); 
                        EnterDungeon(input, 5, status);
                        break;

                    case 2:
                        Console.Clear();
                        EnterDungeon(input, 11, status);
                        break;

                    case 3:
                        Console.Clear();
                        EnterDungeon(input, 17, status);
                        break;

                    default:
                        Console.Clear();
                        Console.WriteLine("잘못된 입력입니다.");
                        break;
                }
            }
            else
            {
                Console.Clear();
                Console.WriteLine("잘못된 입력입니다.");
            }
        }

        void EnterDungeon(int input, int recommend, Status status)
        {
            int random;
            int result = status.defense - recommend;
            int failStamina = 0;

            if (status.defense < recommend)
            {
                random = new Random().Next(0, 101);

                // 40%의 확률로 던전 실패
                // 보상 없고 체력 절반 감소
                if (random < 40)
                {
                    failStamina = status.stamina - (int)(status.stamina * 0.5);

                    Console.WriteLine("던전 실패");
                    Console.WriteLine($"아쉽습니다. 던전을 실패하였습니다.");
                    Console.WriteLine();

                    Console.WriteLine("[탐험 결과]");
                    Console.WriteLine($"체력 {status.stamina} -> {failStamina}");
                    Console.WriteLine();

                    status.stamina = failStamina;
                    if (status.stamina <= 0)
                    {
                        Console.WriteLine("Game Over");
                        Environment.Exit(0);
                    }
                        
                    return;
                }
                else
                {
                    countClear++;
                    DungeonClear(input, result, status);
                }
            }
            else
            {
                countClear++;
                DungeonClear(input, result, status);

            }
        }

        void DungeonClear(int input, int result, Status status)
        {
            int rand;
            int addReward;
            float temp;
            string dungeonName = "";
            // 던전 클리어 시 받을 보상 계산
            int reward = 0;

            if (input == 1)
            {
                reward = 1000;
                dungeonName = "쉬운";
            }
            else if (input == 2)
            {
                reward = 1700;
                dungeonName = "일반";
            }
            else if (input == 3)
            {
                reward = 2500;
                dungeonName = "어려운";
            }

            addReward = new Random().Next((int)status.offense, (int)status.offense * 2 + 1);

            temp = reward * addReward * 0.01f;
            status.gold += (int)temp + reward;

            // 방어력에 따른 체력 소모 반영
            rand = new Random().Next(20, 36);
            status.stamina -= rand - result;

            if (status.stamina > 0)
            {
                Console.WriteLine("던전 클리어");
                Console.WriteLine($"축하합니다!!\n{dungeonName} 던전을 클리어 하셨습니다.");
                LevelUp(status);
                Console.WriteLine();
            }
            else
            {
                Console.WriteLine("Game Over");
                Console.WriteLine("기력이 다하였습니다.\n");
            }

            Console.WriteLine("[탐험 결과]");
            Console.WriteLine($"체력 {status.stamina + rand - result} -> {status.stamina}");
            Console.WriteLine($"Gold {status.gold - (int)temp - reward} -> {status.gold}");
            Console.WriteLine();

            if (status.stamina <= 0)
            {
                Environment.Exit(0);
            }

            Console.WriteLine("0. 나가기");

            Console.WriteLine("원하시는 행동을 입력해 주세요.");
            bool isSuccess = int.TryParse(Console.ReadLine(), out int a);
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
                    return;
                }
            }
            else
            {
                Console.Clear();
                Console.WriteLine("잘못된 입력입니다.");
            }
        }

        public void LevelUp(Status status)
        {
            if (status.level == countClear)
            {
                status.level++;
                countClear = 0;
                status.offense = status.offense + 0.5f;
                status.defense++;
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
            Rest rest = new Rest(); 
            Dungeon dungeon = new Dungeon();    

            Console.WriteLine("스파르타 마을에 오신 여러분 환영합니다.");
            Console.WriteLine("이곳에서 던전으로 들어가기 전 활동을 할 수 있습니다.\n");

            Console.WriteLine("이름을 입력해 주세요");
            status.name = Console.ReadLine();

            Console.WriteLine($"입력하신 이름은 {status.name}입니다.");
            Console.WriteLine();

            do
            {
                StartGame(p, status, inventory, store, rest, dungeon);
            } while (!p.isEnd);

        }

        static void StartGame(Program p, Status status, Inventory inventory, Store store, Rest rest, Dungeon dungeon)
        {
            bool isSuccess = false;

            Console.WriteLine("1. 상태 보기 \n2. 인벤토리 \n3. 상점\n4. 던전 입장\n5. 휴식하기\n0. 끝내기\n");
            Console.WriteLine("원하시는 행동을 입력해 주세요.");

            string input = Console.ReadLine();

            isSuccess = int.TryParse(input, out int select);

            if (isSuccess)
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

                    case 4:
                        dungeon.DisplayDungeon(status);
                        break;

                    case 5:
                        rest.DisplayRest(status); 
                        break;

                    default:
                        Console.Clear();
                        Console.WriteLine("잘못된 입력입니다.\n");
                        break;
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
