using SimpleServer_Test.Handler;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace SimpleServer_Test
{
    static class Program
    {
        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        [STAThread]
        static void Main()
        {
            //Application.EnableVisualStyles();
            //Application.SetCompatibleTextRenderingDefault(false);
            //Application.Run(new Form1());

            Console.WriteLine(ByteArrayToString(Encoding.ASCII.GetBytes("SSMSGHEADER")));
            Console.WriteLine(Encoding.ASCII.GetBytes("SSMSGHEADER").Length);

            Server server = new Server("0.0.0.0",8080);
            server.Start<Listener>();
        }

        public static string ByteArrayToString(byte[] bytes)
        {
            if (bytes == null) return "null";
            string joinedBytes = string.Join(", ", bytes.Select(b => b.ToString()));
            return $"new byte[] {{ {joinedBytes} }}";
        }
    }
}
