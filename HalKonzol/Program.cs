using System.Data;

namespace HalKonzol
{
    internal class Program
    {
        static string command = "run";
        static bool loggedIn = false;
        static ServerConnection connection = new("http://localhost:3000");
        
        static async Task Main(string[] args)
        {
            while (command == "run")
            {
                DoStg(WriteMenu());
            }
        }

        static void DoStg(int number)
        {
            if (!loggedIn)
            {
                switch (number)
                {
                    case 1:
                        ListFish();
                        break;
                    case 2:
                        FilterFish();
                        break;
                    case 3:
                        SearchFish();
                        break;
                    case 4:
                        GroupFish();
                        break;
                    case 5:
                        ListFish();
                        break;
                }
            }
            else
            {
                switch (number)
                {
                    case 1:
                        ListMyFish();
                        break;
                    case 2:
                        AddFish();
                        break;
                    case 3:
                        Logout();
                        break;
                }
            }
            
        }

        private static void Logout()
        {
            
        }

        private static void AddFish()
        {
            
        }

        private static void ListMyFish()
        {
            
        }

        private static void GroupFish()
        {
            
        }

        private static void SearchFish()
        {
            
        }

        static async void FilterFish()
        {
            List<Fish> allFish = await connection.GetFish();
            int number = ValidateNumber(0, int.MaxValue);
            allFish.Where(fish => fish.weight * 100 >= number).ToList().ForEach(fish => Console.WriteLine($"Név: {fish.name}, súly: {fish.weight * 100} dkg"));
            Console.ReadKey();
        }

        static async void ListFish()
        {
            List<Fish> allFish = await connection.GetFish();
            allFish.ForEach(fish => Console.WriteLine($"Név: {fish.name}, súly: {fish.weight * 100} dkg"));
            Console.ReadKey();
        }

        static int WriteMenu()
        {
            Console.Clear();
            if (!loggedIn)
            {
                Console.WriteLine("[1] - Halak listázása");
                Console.WriteLine("[2] - Szűrés megadott tömeg felett");
                Console.WriteLine("[3] - Név szerinti keresés rész-illesztéssel");
                Console.WriteLine("[4] - Halak csoportosítása súly alapján");
                Console.WriteLine("[5] - Bejelentkezés");
                Console.WriteLine("");
                Console.WriteLine("Kérlek válassz egy opciót");
                return ValidateNumber(1,5);
            }
            else
            {
                Console.WriteLine("[1] - Saját hal listázása");
                Console.WriteLine("[2] - Hal hozzáadása");
                Console.WriteLine("[3] - Kijelentkezés");
                return ValidateNumber(1, 3);
            }
        }

        static int ValidateNumber(int min, int max)
        {
            Console.Write($"Add meg a számot {min} és {max} között: ");
            string userInput = Console.ReadLine().Trim();
            bool valid = int.TryParse(userInput, out int result);

            if (valid)
            {
                if (result >= min && result <= max) return result;
                Console.WriteLine("A határon kívül esett a szám!");
            }
            Console.WriteLine("Számot kell megadni!");
            return ValidateNumber(min, max);
        }

        static void login()
        {
            
        }
    }
}
