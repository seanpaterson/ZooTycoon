using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ZooTycoon
{
    //Were all UI interations will take place
    class Menu
    {
        public static List<string>[] animals;
        DataBaseConnection myConnection;
        public Menu()
        {
            myConnection = null;
            animals = null;
        }

        //Place where the user can search for animal inhabitants or create a new one
        public void MainMenu()
        {
            myConnection = new DataBaseConnection();
            bool exit = false;
            String answer;
            while (exit != true)
            {
                Console.Write("Hello zookeeper! Please choose an option! \n"
                    + "0) EXIT \n"
                    + "1) Check out all animals at a particular zoo. \n"
                    + "2) Add a new animal to a particular zoo. \n"
                    + "Enter number here:  ");
                answer = Console.ReadLine();
                switch (answer)
                {
                    case "0":
                        exit = true;
                        break;
                    case "1":
                        Select();
                        break;
                    case "2":
                        Insert();
                        break;

                }
            }
        }

        private void Insert()
        {
            bool success;
            String species;
            String name;
            String location;
            Console.Write("Enter the species of the animal:  ");
            species = Console.ReadLine();
            Console.Write("Enter the name of said animal:  ");
            name = Console.ReadLine();
            Console.Write("Enter the location of the animal:  ");
            location = Console.ReadLine();
            success = myConnection.Insert(species, name, location);
            if (success == false)
                Console.WriteLine("Something went wrong...");
        }
        private void Select()
        {
            String location;
            Console.Write("Enter the location of the zoo to see all of the animals:  ");
            location = Console.ReadLine();
            animals = myConnection.Select(location);
            for (int i = 0; i < animals[0].Count; ++i)
                Console.WriteLine("Species: " + animals[0].ElementAt(i) + "\nName: " + animals[1].ElementAt(i) + "\nLocation: " + animals[2].ElementAt(i) + "\n");
        }
    }
}
