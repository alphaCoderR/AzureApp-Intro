using System;
namespace RandConsoleApp
{
    public class GFG
    {
        public static void sampleFunc()
        {
            Console.WriteLine("This is a random function");
        }

        static public void Main()
        {

            // Get the Background and foreground  
            // color of Console Using BackgroundColor 
            // and ForegroundColor property of Console 
            Console.WriteLine("Background color  :{0}",
                            Console.BackgroundColor);

            Console.WriteLine("Foreground color : {0}",
                            Console.ForegroundColor);
        }
    }

}
