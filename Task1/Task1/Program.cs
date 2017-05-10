using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml.Serialization;
using System.IO;

namespace Task1
{
    public class Product
    {
        public string type;
        public string name;
        public double price;
        public int qty;
        public bool valid;

        public Product() {
            type = "";
            name = "";
            price = 0.0;
            qty = 0;
            valid = true;
        }

        public void modify(string type, string name, double price, int qty, bool valid = true)
        {
            this.type = type;
            this.name = name;
            this.price = price;
            this.qty = qty;
            this.valid = valid;
        }

        public Product(string type, string name, double price, int qty, bool valid = true)
        {
            modify(type, name, price, qty, valid);
        }

        public Product clone()
        {
            return new Product(type, name, price, qty, valid);
        }

        public void setValid(bool valid)
        {
            this.valid = valid;
        }

        public void print()
        {
            Console.Write("Type : " + type);
            Console.Write("\tName : " + name);
            Console.Write("\tPrice : " + price);
            Console.Write("\tQty : " + qty);
            Console.WriteLine("");
        }
    }

    public class Program
    {
        static List<Product> inv = new List<Product>();

        static void menu()
        {
            Console.WriteLine("1. Add Inventory");
            Console.WriteLine("2. Modify Inventory");
            Console.WriteLine("3. Show All");
            Console.WriteLine("4. Make Bill");
            Console.WriteLine("5. Store XML");
            Console.WriteLine("6. Load XML");
            Console.WriteLine("9. Exit");
        }

        static void showAll()
        {
            foreach (Product i in inv)
            {
                if (i.valid == true)
                {
                    i.print();
                }
            }
        }

        static void add(string type, string name, double price, int qty, bool valid = true){
            Product o = new Product(type, name, price, qty, valid);
            inv.Add(o);
        }

        static void modify(string kname, int qty)
        {
            Product rem = new Product(), neww = new Product();

            bool flag = false;

            foreach(Product i in inv){
                
                if (i.name == kname)
                {
                    rem = neww = i;
                    neww.qty = qty;
                }
                flag = true;
            }

            if (flag)
            {
                inv.Remove(rem);
                inv.Add(neww);
            }
        }

        static void makeBill()
        {
            string input = "p";
            double total = 0.0;
            List<Product> cart = new List<Product>();

            while (input != "@")
            {
                Console.WriteLine("@ to end searching and make bill, Search element : ");
                input = Console.ReadLine();

                if(input == "@")
                    break;

                Product old = new Product(), neww = new Product(), o = new Product();
                bool flag = false;

                foreach (Product i in inv)
                {
                    if (i.name == input)
                    {
                        Console.WriteLine("Enter quantity : ");
                        int qty;
                        qty = int.Parse(Console.ReadLine());
                        if (i.qty < qty)
                        {
                            Console.WriteLine("Only " + i.qty + " available");
                        }
                        else
                        {
                            flag = true;

                            o = i.clone();
                            o.qty = qty;
                            old = i;
                            flag = true;
                            neww = i.clone();
                            neww.qty -= qty;

                            if (neww.qty == 0)
                                neww.valid = false;

                            total = total + o.qty * o.price;
                        }
                    }
                }

                if (flag)
                {
                    cart.Add(o);
                    Console.Write("Product found, ");
                    inv.Remove(old);
                    Console.WriteLine("Qty : " + neww.qty);
                    inv.Add(neww);
                }
            }


            Console.WriteLine("Your Cart\n");

            foreach (Product i in cart)
            {
                i.print();
            }

            Console.WriteLine("Total : Rs." + total);
        }

        static void storeXML(string fname)
        {
            XmlSerializer x = new XmlSerializer(typeof(List<Product>));
            TextWriter tw = new StreamWriter(fname);
            x.Serialize(tw, inv);
            tw.Close();
        }

        static void loadXML(string fname)
        {
            XmlSerializer x = new XmlSerializer(typeof(List<Product>));
            TextReader tr = new StreamReader(fname);
            inv = (List<Product>)x.Deserialize(tr);

            tr.Close();
        }

        static void Main(string[] args)
        {
            string fname = "";
            string input = "";

            string type;
            string name;
            double price;
            int qty;
            bool valid;


            while (input != "9")
            {
                menu();
                input = Console.ReadLine();
                switch (input)
                {
                    case "1":
                        Console.WriteLine("Enter type : ");
                        type = Console.ReadLine();
                        Console.WriteLine("Enter name : ");
                        name = Console.ReadLine();
                        Console.WriteLine("Enter price : ");
                        price = double.Parse(Console.ReadLine());
                        Console.WriteLine("Enter quantity : ");
                        qty = int.Parse(Console.ReadLine());
                        valid = true;

                        add(type, name, price, qty);

                        break;
                    case "2":
                        Console.WriteLine("Enter name : ");
                        name = Console.ReadLine();
                        Console.WriteLine("Enter quantity : ");
                        qty = int.Parse(Console.ReadLine());
                        modify(name, qty);

                        break;
                    case "3":
                        showAll();

                        break;

                    case "4":
                        makeBill();

                        break;

                    case "5":
                        Console.WriteLine("Enter filename : ");
                        fname = Console.ReadLine();
                        storeXML(fname);

                        break;

                    case "6":
                        Console.WriteLine("Enter filename : ");
                        fname = Console.ReadLine();
                        loadXML(fname);

                        break;
                }
            }
        }
    }
}
