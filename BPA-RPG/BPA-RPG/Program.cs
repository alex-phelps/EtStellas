using System;
using System.IO;
using System.Net;
using System.Net.Mail;

namespace EtStellas
{
#if WINDOWS || LINUX
    /// <summary>
    /// The main public class.
    /// </summary>
    public static class Program
    {
        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        [STAThread]
        static void Main()
        {
            using (var game = new MainGame())
                game.Run();

            try
            {
                using (var game = new MainGame())
                    game.Run();
            }
            //Process any exceptions raised during the main program
            catch (Exception e)
            {
                //Make sure directories exist
                if (!Directory.Exists("Logs"))
                    Directory.CreateDirectory("Logs");
                if (!Directory.Exists("Logs/CrashReports"))
                    Directory.CreateDirectory("Logs/CrashReports");

                //Print exception info in crash file
                using (StreamWriter writer = File.CreateText("Logs/CrashReports/crash_" + string.Format("{0:yyyy-MM-dd_HH-mm-ss}", DateTime.Now) + ".txt"))
                    writer.WriteLine(e.ToString());

                //Get user input from input box
                string userMessage = Microsoft.VisualBasic.Interaction.InputBox(
                    "Unfortunately, Et Stellas has stopped running.\n\n" +
                    "To help us resolve this issue and improve the game, please provide " +
                    "information on the situation at the time of the crash.",
                    "Crash Report");

                //Prepare information
                var fromAddress = new MailAddress("etstellas@gmail.com", "User");
                var toAddress = new MailAddress("etstellas@gmail.com", "Me");
                string subject = "Crash Report";
                string body = userMessage + "\n\n" +
                    "Crash Info:\n\n" + e.ToString();

                //Create smtp client
                var smtp = new SmtpClient
                {
                    Host = "smtp.gmail.com",
                    Port = 587,
                    EnableSsl = true,
                    DeliveryMethod = SmtpDeliveryMethod.Network,
                    Timeout = 20000,

                    //Unfortunately, without a personal smtp server, I have to go through authentication and hardcode the email password here
                    Credentials = new NetworkCredential(fromAddress.Address, "monogame4life")
                };

                //Send E-Mail
                using (var message = new MailMessage(fromAddress, toAddress)
                {
                    Subject = subject,
                    Body = body
                })
                    smtp.Send(message);
            }
        }
    }
#endif
}
