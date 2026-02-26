using System;
using System.IO;
using System.Net.NetworkInformation;
using System.Threading;

namespace Environment_Pulse
{
    internal class Program
    {
        // Global State - driven by individual check methods
        static float currentFreeSpace;
        static IPStatus currentIPStatus;
        static long currentLatency;

        static void Main(string[] args)
        {
            while (true)
            {
                Console.Clear();
                Console.WriteLine("Welcome to Environment Pulse - Your Environment Health Monitor!");
                Console.WriteLine("Please choose from the following options: ");
                Console.WriteLine("1. Check Disk Health");
                Console.WriteLine("2. Check Network Connectivity");
                Console.WriteLine("3. Check Overall System Pulse");
                Console.WriteLine("4. Exit");

                string choice = Console.ReadLine();

                switch (choice)
                {
                    case "1":
                        CheckDiskHealth();
                        break;
                    case "2":
                        CheckNetworkConnectivity();
                        break;
                    case "3":
                        GetSystemPulse();
                        break;
                    case "4":
                        return;
                    default:
                        Console.WriteLine("Invalid choice. Please select 1, 2, or 3.");
                        Thread.Sleep(4000);
                        break;
                }
            }
        }

        // ===============================================================================
        // METHOD: CheckDiskHealth
        // GOAL: Verify Disk Health and update global state.
        // ===============================================================================
        public static void CheckDiskHealth()
        {
            Console.WriteLine("Checking Disk Health. Please Wait...");
            string drive = "C:";
            var driveInfo = new DriveInfo(drive);

            // Calculate percentage
            float freeSpacePercent = ((float)driveInfo.AvailableFreeSpace / (float)driveInfo.TotalSize) * 100;
            currentFreeSpace = freeSpacePercent;

            if (freeSpacePercent <= 30)
            {
                Console.BackgroundColor = ConsoleColor.Red;
                Console.WriteLine($"Free Space as a Percentage: {freeSpacePercent.ToString("F2")}%");
            }
            else
            {
                Console.BackgroundColor = ConsoleColor.Green;
                Console.ForegroundColor = ConsoleColor.Black;
                Console.WriteLine($"Free Space as a Percentage: {freeSpacePercent.ToString("F2")}%");
            }

            Thread.Sleep(3000);
            Console.ResetColor();
        }

        // ===============================================================================
        // METHOD: CheckNetworkConnectivity
        // GOAL: Verify internet access and update global state.
        // ===============================================================================
        public static void CheckNetworkConnectivity()
        {
            Console.WriteLine("Running connectivity check...");
            Thread.Sleep(2000);

            var targetIP = "8.8.8.8";
            Ping myPinger = new Ping();

            try
            {
                PingReply myPingReply = myPinger.Send(targetIP, 3000);
                currentIPStatus = myPingReply.Status;
                currentLatency = myPingReply.RoundtripTime;

                if (myPingReply.Status == IPStatus.Success)
                {
                    Console.BackgroundColor = ConsoleColor.Green;
                    Console.ForegroundColor = ConsoleColor.Black;
                    Console.WriteLine($"[SUCCESS] Connection to {myPingReply.Address} is Active! Latency: {currentLatency} ms");
                }
                else
                {
                    // Catch-all for specific IPStatus failures (TimedOut, etc.)
                    Console.BackgroundColor = ConsoleColor.Magenta;
                    Console.ForegroundColor = ConsoleColor.White;
                    Console.WriteLine($"[SNAG] Status: {myPingReply.Status}. Better look it up.");
                }
            }
            catch (Exception ex)
            {
                currentIPStatus = IPStatus.Unknown; // Defensive: Reset status on hard failure
                Console.Beep();
                Console.WriteLine($"[CRITICAL ERROR] {ex.Message}");
            }
            finally
            {
                Thread.Sleep(5000);
                Console.ResetColor();
            }
        }

        // ===============================================================================
        // METHOD: GetSystemPulse
        // GOAL: Perform all checks and provide a correlated health report.
        // ===============================================================================
        public static void GetSystemPulse()
        {
            // Defensive Programming: Reset to 'Unknowns' before checking
            currentIPStatus = IPStatus.Unknown;
            currentFreeSpace = -1;

            CheckDiskHealth();
            CheckNetworkConnectivity();

            Console.Clear();
            Console.WriteLine("--- CORRELATED SYSTEM PULSE REPORT ---");

            if (currentIPStatus != IPStatus.Success && (currentFreeSpace <= 30 && currentFreeSpace != -1))
            {
                Console.BackgroundColor = ConsoleColor.Red;
                Console.ForegroundColor = ConsoleColor.White;
                Console.WriteLine($"[CRITICAL] Storage Low & Connectivity Issues! Storage: {currentFreeSpace:F2}% | Status: {currentIPStatus}");
            }
            else if (currentIPStatus != IPStatus.Success)
            {
                Console.BackgroundColor = ConsoleColor.Yellow;
                Console.ForegroundColor = ConsoleColor.Black;
                Console.WriteLine($"[OFFLINE] Connectivity Issues | Status: {currentIPStatus}");
            }
            else if (currentFreeSpace <= 30 && currentFreeSpace != -1)
            {
                Console.BackgroundColor = ConsoleColor.Magenta;
                Console.ForegroundColor = ConsoleColor.White;
                Console.WriteLine($"[STORAGE LOW] Disk health at {currentFreeSpace:F2}%");
            }
            else
            {
                Console.BackgroundColor = ConsoleColor.Green;
                Console.ForegroundColor = ConsoleColor.Black;
                Console.WriteLine($"[PASS] System Healthy | Storage: {currentFreeSpace:F2}% | Latency: {currentLatency}ms");
            }

            Thread.Sleep(5000);
            Console.ResetColor();
        }
    }
}