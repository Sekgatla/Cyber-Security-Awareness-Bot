using System;
using System.Windows;

namespace CybersecurityChatbot
{
    // Application entry point
    // WPF projects use App.xaml as the startup definition,
    // but this class can be used for any pre-launch configuration
    public class Program
    {
        [STAThread]
        public static void Main(string[] args)
        {
            App app = new App();
            app.InitializeComponent();
            app.Run();
        }
    }
}
