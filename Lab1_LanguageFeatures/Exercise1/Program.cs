using System.Diagnostics;

namespace Exercise1
{
    public class Program
    {
        public static void Main(string[] args)
        {
            WriteDelegate writeDelegate = Console.WriteLine;
            writeDelegate += message => Debug.WriteLine(message.ToUpper());

            BalloonProgram program = new BalloonProgram(writeDelegate);
            program.Run();

            Console.WriteLine();
            Console.WriteLine("Press enter to close...");
            Console.Read();
        }
    }
}
