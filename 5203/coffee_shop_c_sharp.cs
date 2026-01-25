using System;
using System.Collections.Generic;
using System.IO;
using System.Threading;

namespace CoffeeShopSystem
{
    class Node
    {
        public int Serial;
        public string FoodName;
        public int Quantity;
        public float Price;
        public Node Next;

        public Node(int serial, string name, int quantity, float price)
        {
            Serial = serial;
            FoodName = name;
            Quantity = quantity;
            Price = price;
            Next = null;
        }
    }

    class Program
    {
        static Node head = null;

        // ---------------- Utility ----------------
        static void Cls() => Console.Clear();

        static void Br(int lines)
        {
            for (int i = 0; i < lines; i++)
                Console.WriteLine();
        }

        static void LoadingBar(string msg = "Loading")
        {
            for (int i = 0; i <= 100; i += 5)
            {
                Cls();
                Console.WriteLine($"\n\n\t\t{i}% {msg}...");
                Console.Write("\t\t");
                for (int j = 0; j < i / 2; j++)
                    Console.Write("#");
                Thread.Sleep(50);
            }
        }

        static void WelcomeMessage()
        {
            Cls();
            string msg = "WELCOME TO COFFEE SHOP SYSTEM";
            Console.Write("\n\n\t\t");
            foreach (char c in msg)
            {
                Console.Write(c + " ");
                Thread.Sleep(50);
            }
            Br(2);
        }

        // ---------------- Insert ----------------
        static void InsertFirst(int serial, string name, int qty, float price)
        {
            Node temp = new Node(serial, name, qty, price);
            temp.Next = head;
            head = temp;
        }

        static void InsertEnd(int serial, string name, int qty, float price)
        {
            Node temp = new Node(serial, name, qty, price);
            if (head == null)
            {
                head = temp;
                return;
            }

            Node current = head;
            while (current.Next != null)
                current = current.Next;

            current.Next = temp;
        }

        // ---------------- Display ----------------
        static void FoodList()
        {
            Cls();
            Console.WriteLine("{0,-10}{1,-20}{2,-10}{3,-10}",
                "Item No", "Item Name", "Price", "Stock");
            Console.WriteLine("-----------------------------------------------");

            Node current = head;
            while (current != null)
            {
                Console.WriteLine("{0,-10}{1,-20}{2,-10}{3,-10}",
                    current.Serial, current.FoodName, current.Price, current.Quantity);
                current = current.Next;
                Thread.Sleep(50);
            }
            Br(1);
        }

        // ---------------- Update & Delete ----------------
        static void UpdateFood(int serial, int newQty)
        {
            Node current = head;
            while (current != null && current.Serial != serial)
                current = current.Next;

            if (current != null)
                current.Quantity = newQty;
        }

        static void DeleteFood(int serial)
        {
            if (head == null) return;

            if (head.Serial == serial)
            {
                head = head.Next;
                return;
            }

            Node current = head;
            while (current.Next != null && current.Next.Serial != serial)
                current = current.Next;

            if (current.Next != null)
                current.Next = current.Next.Next;
        }

        static int CountItem()
        {
            int count = 0;
            Node current = head;
            while (current != null)
            {
                count++;
                current = current.Next;
            }
            return count;
        }

        // ---------------- Backup ----------------
        static void BackupSystem(float totalCash, List<int> cardNos, List<float> cardMoneys)
        {
            string filename = DateTime.Now.ToString("dd-MMM-yyyy") + ".txt";
            using (StreamWriter sw = new StreamWriter(filename))
            {
                sw.WriteLine($"Total Cash Today: {totalCash}");
                sw.WriteLine("Card No ------ Money");
                for (int i = 0; i < cardNos.Count; i++)
                    sw.WriteLine($"{cardNos[i]} ------- {cardMoneys[i]}");
            }

            Console.WriteLine("\nBackup Successful: " + filename);
            Thread.Sleep(1000);
        }

        // ---------------- Menus ----------------
        static void MainMenu()
        {
            Console.WriteLine("\n1. Coffee List");
            Console.WriteLine("2. Admin Panel");
            Console.WriteLine("3. Exit\n");
        }

        static void AdminPanel(ref float totalMoney, List<int> cardNos, List<float> cardMoneys)
        {
            while (true)
            {
                Cls();
                Console.WriteLine("Admin Panel");
                Console.WriteLine("1. Total Cash Today");
                Console.WriteLine("2. View Card Payments");
                Console.WriteLine("3. Add Coffee Item");
                Console.WriteLine("4. Delete Item");
                Console.WriteLine("5. Item List");
                Console.WriteLine("6. Item Counter");
                Console.WriteLine("7. Backup System");
                Console.WriteLine("0. Main Menu");
                Console.Write("Enter Choice: ");

                if (!int.TryParse(Console.ReadLine(), out var choice))
                {
                    continue;
                }

                if (choice == 1)
                {
                    Console.WriteLine($"Today's Total Cash: {totalMoney}");
                    Thread.Sleep(1000);
                }
                else if (choice == 2)
                {
                    if (cardNos.Count == 0)
                        Console.WriteLine("No Card History.");
                    else
                    {
                        Console.WriteLine("{0,-10}{1,-10}", "Card No", "Money");
                        Console.WriteLine("--------------------");
                        for (int i = 0; i < cardNos.Count; i++)
                            Console.WriteLine("{0,-10}{1,-10}", cardNos[i], cardMoneys[i]);
                    }
                    Thread.Sleep(1000);
                }
                else if (choice == 3)
                {
                    Console.Write("Enter Item Serial: ");
                    int s = int.Parse(Console.ReadLine());
                    Console.Write("Enter Item Name: ");
                    string name = Console.ReadLine();
                    Console.Write("Enter Quantity: ");
                    int q = int.Parse(Console.ReadLine());
                    Console.Write("Enter Price: ");
                    float p = float.Parse(Console.ReadLine());
                    InsertEnd(s, name, q, p);
                }
                else if (choice == 4)
                {
                    Console.Write("Enter Serial to Delete: ");
                    DeleteFood(int.Parse(Console.ReadLine()));
                }
                else if (choice == 5)
                {
                    FoodList();
                }
                else if (choice == 6)
                {
                    Console.WriteLine("Total Items: " + CountItem());
                    Thread.Sleep(1000);
                }
                else if (choice == 7)
                {
                    BackupSystem(totalMoney, cardNos, cardMoneys);
                }
                else if (choice == 0)
                    break;
            }
        }

        // ---------------- Main ----------------
        static void Main()
        {
            Console.Title = "Coffee Shop Management System";

            LoadingBar();
            WelcomeMessage();

            InsertFirst(1, "Americano", 23, 2.23f);
            InsertEnd(2, "Latte", 13, 1.67f);
            InsertEnd(3, "Cappuccino", 8, 3.83f);
            InsertEnd(4, "Espresso", 46, 5.23f);
            InsertEnd(5, "Irish", 46, 5.23f);

            float totalMoney = 0;
            List<int> cardNos = new List<int>();
            List<float> cardMoneys = new List<float>();

            while (true)
            {
                MainMenu();
                Console.Write("Enter Choice: ");
                int choice = int.Parse(Console.ReadLine());

                if (choice == 1)
                    FoodList();
                else if (choice == 2)
                {
                    Console.Write("Enter Admin Password: ");
                    int pass = int.Parse(Console.ReadLine());
                    if (pass == 12345)
                        AdminPanel(ref totalMoney, cardNos, cardMoneys);
                    else
                    {
                        Console.WriteLine("Wrong Password.");
                        Thread.Sleep(1000);
                    }
                }
                else if (choice == 3)
                {
                    Console.WriteLine("Thank You For Using Our System.");
                    break;
                }
                else
                {
                    Console.WriteLine("Invalid Choice!");
                    Thread.Sleep(1000);
                }
            }
        }
    }
}
