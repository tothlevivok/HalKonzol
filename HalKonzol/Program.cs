using System.Data;

namespace HalKonzol
{
    internal class Program
    {
        static string command = "run";
        static bool loggedIn = false;
        static ServerConnection connection = new ServerConnection("http://localhost:3000");
        
        static async Task Main(string[] args)
        {
            while (command == "run")
            {
                int number = WriteMenu();
                await DoStg(number);
            }
        }

        static async Task DoStg(int number)
        {
            if (!loggedIn)
            {
                switch (number)
                {
                    case 1:
                        await ListFish();
                        break;
                    case 2:
                        await FilterFish();
                        break;
                    case 3:
                        await SearchFish();
                        break;
                    case 4:
                        await GroupFish();
                        break;
                    case 5:
                        await Login();
                        break;
                    case 6:
                        command = "exit";
                        break;
                }
            }
            else
            {
                switch (number)
                {
                    case 1:
                        await ListMyFish();
                        break;
                    case 2:
                        await AddFish();
                        break;
                    case 3:
                        await Logout();
                        break;
                }
            }
            
        }

        static async Task Logout()
        {
            loggedIn = false;
            WriteMenu();
        }

        static async Task AddFish()
        {
            try
            {
                Console.WriteLine("Hal neve: ");
                string name = Console.ReadLine().Trim();
                Console.WriteLine("Hal súlya: ");
                string weightString = Console.ReadLine().Trim();
                if(double.TryParse(weightString, out double weight))
                {
                    await connection.PostFish(name, weight);
                }
                else
                {
                    Console.WriteLine("Érvénytelen súly!");
                }
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
            }
        }

        static async Task ListMyFish()
        {
            List<Fish> allFish = await connection.GetMyFish();
            allFish.ForEach(fish => Console.WriteLine($"Név: {fish.name}, súly: {fish.weight * 100} dkg"));
            Console.ReadKey();
        }

        static async Task GroupFish()
        {
            List<Fish> allFish = await connection.GetFish();
            int under100 = allFish.Where(fish => fish.weight < 100).Count();
            int under200 = allFish.Where(fish => fish.weight >= 100 && fish.weight <= 200).Count();
            int over200 = allFish.Where(fish => fish.weight > 200).Count();
            Console.WriteLine("Statisztika");
            Console.WriteLine("Halak súlya csoportosítva");
            Console.WriteLine($"100kg alatt: {under100}");
            Console.WriteLine($"100kg - 200kg: {under200}");
            Console.WriteLine($"200kg felett: {over200}");
        }

        static async Task SearchFish()
        {
            List<Fish> allFish = await connection.GetFish();
            Console.WriteLine("Add meg a keresett szórészletet: ");
            string name = Console.ReadLine().Trim();
            allFish.Where(fish => fish.name.Contains(name)).ToList().ForEach(fish => Console.WriteLine($"Név: {fish.name}, súly: {fish.weight * 100} dkg"));
            Console.ReadKey();
        }

        static async Task FilterFish()
        {
            List<Fish> allFish = await connection.GetFish();
            Console.WriteLine("Add meg a minimum súlyt (dkg):");
            int number = ValidateNumber(0, int.MaxValue);
            allFish.Where(fish => fish.weight * 100 >= number).ToList().ForEach(fish => Console.WriteLine($"Név: {fish.name}, súly: {fish.weight * 100} dkg"));
            Console.ReadKey();
        }

        static async Task ListFish()
        {
            List<Fish> allFish = await connection.GetFish();
            allFish.ForEach(fish => Console.WriteLine($"Név: {fish.name}, súly: {fish.weight * 100} dkg"));
            Console.ReadKey();
        }

        static int WriteMenu()
        {
            Console.Clear();
            Console.WriteLine(loggedIn);
            if (!loggedIn)
            {
                Console.WriteLine("[1] - Halak listázása");
                Console.WriteLine("[2] - Szűrés megadott tömeg felett");
                Console.WriteLine("[3] - Név szerinti keresés rész-illesztéssel");
                Console.WriteLine("[4] - Halak csoportosítása súly alapján");
                Console.WriteLine("[5] - Bejelentkezés");
                Console.WriteLine("[6] - Kilépés");
                Console.WriteLine("");
                Console.WriteLine("Kérlek válassz egy opciót");
                return ValidateNumber(1,6);
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
            Console.WriteLine($"Add meg a számot {min} és {max} között: ");
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

        static async Task Login()
        {
            try
            {
                Console.Write("Felhasználónév: ");
                string username = Console.ReadLine().Trim();
                Console.Write("Jelszó: ");
                string password = Console.ReadLine().Trim();
                if(await connection.Login(username, password))
                {
                    loggedIn = true;
                }
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
            }
        }
    }
}
