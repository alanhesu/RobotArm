using Leap;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace GestureControls
{
    class GestureListener
    {
        public void OnServiceConnect(object sender, ConnectionEventArgs args)
        {
            Console.WriteLine("Service Connected");
        }

        public void OnConnect(object sender, DeviceEventArgs args)
        {
            Console.WriteLine("Connected");
        }

        public void OnFrame(object sender, FrameEventArgs args)
        {
            Console.WriteLine("Frame Available");
        }
    }

    class Locate
    {
        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        // [STAThread]
        public static void Main()
        {
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            Application.Run(new Form1());          

            Controller controller = new Controller();
            GestureListener listener = new GestureListener();
            controller.Connect += listener.OnServiceConnect;
            controller.Device += listener.OnConnect;
            controller.FrameReady += listener.OnFrame;
            Console.WriteLine("Press enter to quit");
            Console.ReadLine();

            // controller.RemoveListener(listener);
            controller.Dispose();
        }
    }
}
