using System.Linq;

namespace ArktinMonitor.MessageBox
{
    internal class Program
    {
        private static void Main(string[] args)
        {
            System.Windows.Forms.MessageBox.Show(args.FirstOrDefault());
        }
    }
}