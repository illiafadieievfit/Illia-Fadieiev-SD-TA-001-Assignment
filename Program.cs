using System.Diagnostics;
using System.IO;
namespace Illia_Fadieiev_SD_TA_001_Assignment
{
    //Class declaration
    class Car
    {
        //Properties declaration
        public string Registration { get; set; }
        public string Make { get; set; }
        public string Model { get; set; }
        public int Mileage { get; set; }
        public int Price { get; set; }

        //Constructor: runs when a new Car object is created
        public Car(string registration, string make, string model, int mileage, int price)
        {
            //Assignation of parameters to properties
            Registration = registration;
            Make = make;
            Model = model;
            Mileage = mileage;
            Price = price;
        }
        internal class Program
        {
            static void Main(string[] args)
            {
                //Path definition
                string filePath = Path.Combine(AppContext.BaseDirectory, "SD-TA-001-B_DealershipStockList.csv");
                
                //Check if file exists
                if (File.Exists(filePath)) {
                    
                    //If exists, read it as an array of strings
                    string[] fileLines = File.ReadAllLines(filePath);
                    //Create a list of Car objects
                    List<Car> cars = new List<Car>();
                    
                    //Parse all lines but first
                    foreach (string line in fileLines.Skip(1))
                    {
                        string[] car = line.Split(',');
                        //Check data parses to required data types
                        if (int.TryParse(car[3], out int mileage) && int.TryParse(car[4], out int price))
                        {
                            Car currentCar = new Car(car[0], car[1], car[2], mileage, price);
                            //Add a new element to the list of Cars
                            cars.Add(currentCar);
                        }
                        else
                        {
                            //If data doesn't parse, show an error explaining what went wrong
                            Console.WriteLine("Error: failed to parse the data. Please check the data is valid and try again");
                            //Exit the app gracefully
                            return;
                        }
                    }
                    //Call a method to make calculations and assign a result to calculations named tuple
                    var calculcations = Calculations(cars);
                    //Report creation
                    string report = "Car Stock – Weekly Report";
                    report += "\nTotal Cars in Stock: " + cars.Count + " Cars";
                    report += "\nTotal Value of Cars in Stock: €" + calculcations.sumPrice;
                    report += "\nAverage Price of Cars: €" + GetAverage(calculcations.sumPrice, cars.Count);
                    report += "\nAverage Mileage of Cars: " + GetAverage(calculcations.sumMil, cars.Count);
                    report += $"\nCheapest Car Available: {cars[calculcations.minPriceIndex].Registration}, {cars[calculcations.minPriceIndex].Make}, {cars[calculcations.minPriceIndex].Model} €{cars[calculcations.minPriceIndex].Price}";
                    
                    //Write report to the file, the Path is combined inside the method WriteAllText
                    File.WriteAllText(Path.Combine(AppContext.BaseDirectory, AppContext.BaseDirectory, "report.txt"), report);
                    //Show success message and the report
                    Console.WriteLine($"Report is done and written to ${AppContext.BaseDirectory}report.txt\n" + report);
                }
                else {
                    //If file doesn't exist, show an error explaining what went wrong
                    Console.WriteLine($"Error: File not found. Please place the CSV file with data to {AppContext.BaseDirectory} and try again"); 
                }  
            }
            static int GetAverage(int total, int amount)
            {
                return total / amount;
            }
            static (int sumPrice, int sumMil, int minPrice, int minPriceIndex) Calculations(List<Car> cars)
            {
                //Define variables required to make calculations
                int minPrice = cars[0].Price;
                int minPriceIndex = 0;
                int sumPrice = 0;
                int sumMil = 0;
                //Loop with necessary calculations
                for (int i = 0; i < cars.Count; i++)
                {
                    if (cars[i].Price < minPrice)
                    {
                        minPrice = cars[i].Price;
                        minPriceIndex = i;
                    }

                    sumPrice += cars[i].Price;
                    sumMil += cars[i].Mileage;
                }
                //Return values with field names as a named tuple
                return (sumPrice, sumMil, minPrice, minPriceIndex);
            }
        }
    }
}