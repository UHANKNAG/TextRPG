using System.Reflection.Emit;
using System.Xml.Linq;

namespace TextRPG
{
    public class Status
    {
        int level = 1;
        string name = "Chad";
        string job = "전사";
        int offense = 10;
        int defense = 5;
        int stamina = 100;
        int gold = 1500;

        public void DisplayStatus()
        {
            Console.WriteLine("\n상태 보기 \n캐릭터의 정보가 표시됩니다.\n");

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

        public void DisplayIventory()
        {
            List<Item> items = new List<Item>();

            Console.WriteLine("\n인벤토리 \n보유 중인 아이템을 관리할 수 있습니다.\n");

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

    internal class Program
    {

        static void Main(string[] args)
        {
            Status status = new Status();
            Inventory inventory = new Inventory();
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
