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
                            //Add a new element to the list of Cars
                            cars.Add(new Car(car[0], car[1], car[2], mileage, price));
                        }
                        else
                        {
                            //If data doesn't parse, show an error explaining what went wrong
                            Console.WriteLine("Error: failed to parse the data. Please check the data is valid and try again");
                            //Exit the app gracefully
                            return;
                        }
                    }

                    //Define variables required to make a report
                    int minPrice = cars[0].Price;
                    int minPriceIndex = 0;
                    int sumPrice = 0;
                    int sumMil = 0;

                    //Loop with necessary calculations
                    for (int i = 0; i < cars.Count; i++)
                    {
                        if (cars[i].Price < minPrice) { minPrice = cars[i].Price; minPriceIndex = i; }
                        sumPrice += cars[i].Price;
                        sumMil += cars[i].Mileage;
                    }
                    
                    //Report creation
                    string report = "Car Stock – Weekly Report";
                    report += "\nTotal Cars in Stock: " + cars.Count + " Cars";
                    report += "\nTotal Value of Cars in Stock: €" + sumPrice;
                    report += "\nAverage Price of Cars: €" + sumPrice / cars.Count;
                    report += "\nAverage Mileage of Cars: " + sumMil / cars.Count;
                    report += $"\nCheapest Car Available: {cars[minPriceIndex].Registration}, {cars[minPriceIndex].Make}, {cars[minPriceIndex].Model} €{cars[minPriceIndex].Price}";
                    
                    //Write report to the file, Path combined inside the method WriteAllText
                    File.WriteAllText(Path.Combine(AppContext.BaseDirectory, AppContext.BaseDirectory, "report.txt"), report);
                    //Show success message and the report
                    Console.WriteLine("Report is done and written to report.txt\n" + report);
                }
                else {
                    //If file doesn't exist, show an error explaining what went wrong
                    Console.WriteLine("Error: File not found. Please place the CSV file with data to bin\\Debug\\net8.0 and try again"); 
                }  
            }
        }
    }
}
