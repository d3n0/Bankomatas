using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Bankomatas
{
    internal class Card
    {
        private int Id;
        private int Code;
        private double Money;
        private List<Transaction> Transactions = new List<Transaction>();
        private double transactedToday = 0;
        private bool logged = false;
        private int passwordIncorect = 0;
        private String filePath = @"C:\Users\User\source\repos\Bankomatas\Bankomatas\cards\";

        public Card(int id)
        {
            filePath += (id + ".txt");
            if (cardExists()) letMeIn();
            else Console.WriteLine("Kortele nerasta.");
        }
        private void letMeIn()
        {
            if (getCardData())
            while (!logged && passwordIncorect < 3)
            {
                Console.Clear();
                Console.WriteLine($"Jus turite dar {3 - passwordIncorect} bandymus. Iveskite korteles koda:");
                int passTry = Convert.ToInt32(Console.ReadLine());
                if (passCorrect(passTry))
                {
                    logged = true;
                    Console.WriteLine($"Slaptazodis teisingas. Sveiki, prisijunge!");
                    showMenu();
                        
                } else
                {
                    passwordIncorect++;
                }
            }
            if (passwordIncorect >= 3) Console.WriteLine("Kortele uzblokuota.");
        }
        private bool getCardData()
        {
            try
            {
                String[] lines = File.ReadAllLines(filePath);
                if (lines.Length < 3) throw new Exception();
                Id = Convert.ToInt32(lines[0]);
                Code = Convert.ToInt32(lines[1]);
                Money = Convert.ToDouble(lines[2]);
            } catch(Exception e)
            {
                Console.WriteLine("Nepavyko Perskaityti duomenu is failo..");
                return false;
            }
            return true;
        }
        private bool cardExists()
        {
            if (File.Exists(filePath))
                if (getCardData()) return true;
            return false;
        }
        private bool passCorrect(int input)
        {
            if (Code == input)
                return true;
            return false;
        }
        private void showMenu(int menuItem = 0)
        {
            Console.Clear();
            switch (menuItem)
            {
                case 1:
                    showLastTransactions();
                    break;
                case 2:
                    Console.WriteLine("Irasykite suma, kuria norite issiimti:");
                    withdraw(Convert.ToDouble(Console.ReadLine()));
                    break;
                case 3:
                    Console.WriteLine("Taikomas limitas | Isnaudotas limitas | Limito likutis:");
                    Console.WriteLine($"$1000 | ${transactedToday} | {limitLeft()}");
                    Console.WriteLine($"10 transakciju | {Transactions.Count} | {10-Transactions.Count}");
                    break;
                default:
                    Console.WriteLine($"Jusu korteles balansas: ${Money}\n");
                    break;
            }
            Console.WriteLine("0 : Korteles balansas");
            Console.WriteLine("1 : Paskutines 5 transakcijos");
            Console.WriteLine("2 : Issiimti pinigus");
            Console.WriteLine("3 : Korteles limitai");
            showMenu(Convert.ToInt32(Console.ReadLine()));
        }
        private void showLastTransactions()
        {
            if(Transactions == null)
            {
                Console.WriteLine("Transakciju neuzfiksuota.");
            } else
            {
                foreach(Transaction transaction in Transactions)
                {
                    Console.WriteLine(transaction.ToString());
                }
            }
        }
        private void withdraw(double howMuch)
        {
            if (howMuch > 0 && limitLeft() >= howMuch)
            {
                Money -= howMuch;
                transactedToday += howMuch;
                Transactions.Add(new Transaction(howMuch));
                Console.WriteLine("Operacija sekminga.");
            } else
                Console.WriteLine("Operacija nesekminga. Korteleje nera pakankamai pinigu arba isnaudojote dienos limita.");
        }
        private double limitLeft()
        {
            if(Transactions != null && Transactions.Count >= 10)
            {
                return 0;
            }
            return (1000 - transactedToday > Money ? Money : 1000 - transactedToday);
        }
    }
}
