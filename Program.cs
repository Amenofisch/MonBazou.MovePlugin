using System;
using System.Threading;
using System.IO;
using Microsoft.Win32;

namespace MovePlugin
{
    internal class Program
    {
        enum ExitCodes : int
        {
            Success = 0,
            WrongFileName = 1,
            WrongFileExtension = 2,
            FileMissing = 4,
            ConfigEmpty = 8,
            CopyError = 16,
            GamePathMissing = 32,
        }

        private static string _dllName;
        private static string _confFile = "movePlugin.conf";
        private static string _gamePath;
        private static string _bepInExPath;
        private static string _binFolder = null;

        static void Main(string[] args)
        {
            Console.WriteLine("MonBazou.MovePlugin Tool coded by Amenofisch#5368\n\n");

            if(!File.Exists("movePlugin.conf"))
            {
                Console.WriteLine("Enter .dll name:");
                string input = Console.ReadLine();
                _dllName = input;
                if (!File.Exists(AppDomain.CurrentDomain.BaseDirectory + "/bin/Debug/" + input) && !File.Exists(AppDomain.CurrentDomain.BaseDirectory + "/bin/Release/" + input)) Environment.Exit((int)ExitCodes.WrongFileName);
                File.WriteAllText(_confFile, input);
            }

            _dllName = File.ReadAllText(_confFile).Trim();
            if (String.IsNullOrEmpty(_dllName)) Environment.Exit((int)ExitCodes.ConfigEmpty);
            if (!File.Exists(AppDomain.CurrentDomain.BaseDirectory + "/bin/Debug/" + _dllName) && !File.Exists(AppDomain.CurrentDomain.BaseDirectory + "/bin/Release/" + _dllName)) Environment.Exit((int)ExitCodes.FileMissing);

            if(_binFolder == null && File.Exists(AppDomain.CurrentDomain.BaseDirectory + "/bin/Debug/" + _dllName))
            {
                _binFolder = "Debug";
            }
            if(_binFolder == null && File.Exists(AppDomain.CurrentDomain.BaseDirectory + "/bin/Release/" + _dllName))
            {
                _binFolder = "Release";
            }

            if(_binFolder == null)
            {
                Console.WriteLine("Couldn't find .dll in any of the bin folders");
                Environment.Exit((int)ExitCodes.FileMissing);
            }          

            _gamePath = GetGameFolder();
            if (String.IsNullOrEmpty(_gamePath)) Environment.Exit((int)ExitCodes.GamePathMissing);
            _bepInExPath = _gamePath + "/BepInEx/plugins/";

            try
            {
                File.Copy(AppDomain.CurrentDomain.BaseDirectory + $"/bin/{_binFolder}/" + _dllName, _bepInExPath + _dllName, true);
            } catch (Exception ex)
            {
                Console.WriteLine(ex.ToString());
                Environment.Exit((int)ExitCodes.CopyError);
            }

            Console.WriteLine("Succesfully copied " + _dllName + " to " + _bepInExPath);
            Thread.Sleep(2000);
            Environment.Exit((int)ExitCodes.Success);
        }

        #region GetGameFolder Method
        /// <summary>
        /// Gets the game folder from registry.
        /// Returns string with path to game root folder.
        /// </summary>
        static string GetGameFolder()
        {
            try
            {
                using (RegistryKey registryKey = RegistryKey.OpenBaseKey(RegistryHive.LocalMachine, RegistryView.Registry64))
                {
                    RegistryKey registryKey2;
                    try
                    {
                        registryKey2 = registryKey.OpenSubKey(@"SOFTWARE\Microsoft\Windows\CurrentVersion\Uninstall\Steam App 1520370");
                        object value = registryKey2.GetValue("InstallLocation");
                        return value.ToString();
                    }
                    catch (Exception ex)
                    {
                        return null;
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.ToString());
                return null;
            }
        }
        #endregion

    }
}
