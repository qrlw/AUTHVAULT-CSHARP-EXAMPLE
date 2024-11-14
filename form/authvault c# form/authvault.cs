using System;
using System.IO;
using System.Linq;
using System.Net;
using System.Threading;
using System.Management;
using System.Diagnostics;
using System.Runtime.InteropServices;
using System.Collections.Generic;
using System.Text;

namespace authvault_c__form
{
    public static class AuthVault
    {
        private static bool isInitialized = false;

        static void Error(string message)
        {
            string command = $"start cmd /C \"color c && title Error && echo {message} && timeout /t 5\"";
            System.Diagnostics.Process.Start("cmd.exe", command);
            Environment.Exit(0);
        }

        static string GetUsername()
        {
            if (CheckSectionIntegrity(".text"))
            {
                Console.Clear();
                Error("Integrity check failed during initial check. Don't tamper with the program!");
            }

            try
            {
                string username = Environment.UserName;
                return username;
            }
            catch
            {
                Console.WriteLine("Failed to get username.");
                Environment.Exit(0);
            }

            return string.Empty;
        }

        static void CheckInitialization()
        {
            if (CheckSectionIntegrity(".text"))
            {
                Console.Clear();
                Error("Integrity check failed during initial check. Don't tamper with the program!");
            }

            if (!isInitialized)
            {
                Console.ForegroundColor = ConsoleColor.Red;
                Error("[Error] AuthVault is not initialized! Call AuthVault::setup() before any other function.");
            }
        }

        static void CreateAppDataFolder()
        {
            if (CheckSectionIntegrity(".text"))
            {
                Console.Clear();
                Error("Integrity check failed during initial check. Don't tamper with the program!");
            }

            string path = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.UserProfile), "AppData", "Roaming", "authvault");

            try
            {
                if (!Directory.Exists(path))
                {
                    Directory.CreateDirectory(path);
                }
            }
            catch (Exception e)
            {
                Console.WriteLine($"Error when connecting to local system: {e.Message}");
            }
        }

        static bool IsPythonInstalled()
        {
            if (CheckSectionIntegrity(".text"))
            {
                Console.Clear();
                Error("Integrity check failed during initial check. Don't tamper with the program!");
            }

            try
            {
                return System.Diagnostics.Process.Start("python", "--version") != null;
            }
            catch
            {
                return false;
            }
        }

        static bool IsSeleniumInstalled()
        {
            if (CheckSectionIntegrity(".text"))
            {
                Console.Clear();
                Error("Integrity check failed during initial check. Don't tamper with the program!");
            }

            string path = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.UserProfile), "AppData", "Roaming", "Python", "Python39", "site-packages", "selenium");
            return Directory.Exists(path);
        }



        static bool ConstantTimeStringCompare(string str1, string str2, int length)
        {
            if (CheckSectionIntegrity(".text"))
            {
                Console.Clear();
                Error("Integrity check failed during initial check. Don't tamper with the program!");
            }

            int result = 0;
            for (int i = 0; i < length; i++)
            {
                result |= str1[i] ^ str2[i];
            }

            return result == 0;
        }

        static void RemoveJSFile()
        {
            if (CheckSectionIntegrity(".text"))
            {
                Console.Clear();
                Error("Integrity check failed during initial check. Don't tamper with the program!");
            }

            string username = GetUsername();
            string jsFilePath = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.UserProfile), "AppData", "Roaming", "authvault", "AuthVault.py");

            if (File.Exists(jsFilePath))
            {
                File.Delete(jsFilePath);
            }
        }

        static string GetHWID()
        {
            if (CheckSectionIntegrity(".text"))
            {
                Console.Clear();
                Error("Integrity check failed during initial check. Don't tamper with the program!");
            }

            string hwid = string.Empty;
            try
            {
                hwid = GetCpuInfo() + GetVolumeSerialNumber();
            }
            catch
            {
             
            }

            return hwid;
        }

        public static string GetCpuInfo()
        {
            string cpuInfo = string.Empty;

            ManagementObjectSearcher searcher = new ManagementObjectSearcher("select * from Win32_Processor");

            foreach (ManagementObject obj in searcher.Get())
            {
                cpuInfo = obj["ProcessorId"].ToString();
                break; 
            }

            return cpuInfo;
        }
        static string GetVolumeSerialNumber()
        {
            uint serialNumber = 0;
            var driveInfo = new DriveInfo("C");
            return serialNumber.ToString();
        }

        static void WriteSessionLog(string sessionId, string message)
        {
            if (CheckSectionIntegrity(".text"))
            {
                Console.Clear();
                Error("Integrity check failed during initial check. Don't tamper with the program!");
            }

            string username = GetUsername();
            string logFilePath = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.UserProfile), "AppData", "Roaming", "authvault", $"session_log_{sessionId}.txt");

            try
            {
                using (StreamWriter sw = new StreamWriter(logFilePath, true))
                {
                    sw.WriteLine($"Session ID: {sessionId}");
                    sw.WriteLine($"Message: {message}");
                }
            }
            catch (Exception)
            {
                Console.WriteLine("Failed to open log file for writing.");
                Environment.Exit(0);
            }
        }

        static bool CheckSectionIntegrity(string section)
        {
           
            return false;
        }

        static bool IsOnline()
        {
            if (CheckSectionIntegrity(".text"))
            {
                Console.Clear();
                Error("Integrity check failed during initial check. Don't tamper with the program!");
            }

            try
            {
                using (var client = new WebClient())
                {
                    client.DownloadString("http://google.com");
                    return true;
                }
            }
            catch
            {
                return false;
            }
        }


        public static int ValidateLicenseKey(string licenseKey, int applicationId, string secret)
        {
            CheckInitialization();

            if (IsOnline())
            {
              
            }
            else
            {
                MessageBox.Show("There is no stable connection to the internet.");
                Thread.Sleep(2500);
                Environment.Exit(0);
            }

            if (CheckSectionIntegrity(".text"))
            {
                MessageBox.Show("Integrity check failed during initial check. Don't tamper with the program!");
                Environment.Exit(0);
            }

            string hwid = GetHWID();
            string url = $"https://authvault-api.tiiny.io/?license_key={licenseKey}&application_id={applicationId}&secret={secret}&hwid={hwid}";

            try
            {
                using (WebClient client = new WebClient())
                {
                    string response = client.DownloadString(url);




                    if (response.Contains("License key is valid", StringComparison.OrdinalIgnoreCase) ||
                        response.Contains("License key activated and HWID stored", StringComparison.OrdinalIgnoreCase))
                    {
                        return 0;
                    }


                    if (response.Contains("expired", StringComparison.OrdinalIgnoreCase))
                    {
                        MessageBox.Show("License key has expired!");
                        Thread.Sleep(500);
                        Environment.Exit(0);
                    }


                    else if (response.Contains("HWID mismatch", StringComparison.OrdinalIgnoreCase))
                    {
                        MessageBox.Show("HWID Doesn't match the key!");
                        Thread.Sleep(500);
                        Environment.Exit(0);
                    }


                    else if (response.Contains("License key not found", StringComparison.OrdinalIgnoreCase))
                    {
                        MessageBox.Show("Invalid key!");
                        Thread.Sleep(500);
                        Environment.Exit(0);
                    }


                    else if (response.Contains("License key has exceeded its allowed uses.", StringComparison.OrdinalIgnoreCase))
                    {
                        MessageBox.Show("All uses have been used for this key!");
                        Thread.Sleep(500);
                        Environment.Exit(0);
                    }

                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Failed to connect to API: " + ex.Message);
                Environment.Exit(0);
            }

            return -1;
        }



        public static void Setup()
        {
            isInitialized = true;
            CreateAppDataFolder();

            // Run threads in parallel
            var threads = new List<Thread>();
            for (int i = 0; i < 1200; i++)
            {
                var thread = new Thread(ValidateLicenseKeyThread);
                thread.Start();
                threads.Add(thread);
            }

            // Wait for all threads to complete
            foreach (var thread in threads)
            {
                thread.Join();
            }
        }

        static void ValidateLicenseKeyThread()
        {
            // Threaded validation code here
        }
    }
}
