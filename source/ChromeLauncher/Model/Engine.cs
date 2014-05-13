using System;
using System.Diagnostics;
using System.IO;
using System.Text;
using System.Threading;
using System.Linq;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace ChromeLauncher.Model
{
    public class Engine
    {
        #region Attributes
        /// <summary>
        /// The process of chrome launch
        /// </summary>
        public Process Process_chrome_launch { get; set; }

        /// <summary>
        /// The start url to go when chrome is launch
        /// </summary>
        public String Chrome_Url_start { get; set; }

        /// <summary>
        /// Argument give to chrome to start
        /// </summary>
        public String Chrome_Arguments { get; set; }

        /// <summary>
        /// Get the StartInfo.Arguments to pass before start the process
        /// ex : @"http://20100.lescigales.org/FF1/" + " --new-window --incognito --start-maximized --start-fullscreen";
        /// </summary>
        public String Chrome_StartInfo_Arguments {
            get 
            {
                return this.Chrome_Url_start + " " + this.Chrome_Arguments;
            }
        }

        /// <summary>
        /// The process associate with the tab url launch with chrome
        /// </summary>
        public Process Current_chrome_process { get; set; }

        /// <summary>
        /// The window handle associate with the Current_chrome_process
        /// </summary>
        public IntPtr Current_chrome_hWnd { get; set; }

        /// <summary>
        /// The id associate with the Current_chrome_process
        /// </summary>
        public int Current_chrome_id { get ; set ; }

        /// <summary>
        /// Information about sreean and the Current_chrome_process
        /// </summary>
        public string Current_chrome_information { get; set; }

        /// <summary>
        /// Store if chrome launch in fullscreen mode (send keys F11)
        /// </summary>
        public bool Opt_fullscreen { get; set; }

        /// <summary>
        /// The index of screen where to launch chrome
        /// </summary>
        public int Index_screen { get; set; }

        /// <summary>
        /// Use to handle screen in this engine
        /// </summary>
        public ScreensHandler ScreensHandler = new ScreensHandler();
        
        /// <summary>
        /// Passed in constructor in order to activate the debug mode
        /// </summary>
        public bool Opt_debug_mode { get; set; }
        #endregion

        #region Construtor
        public Engine(string p_url, string p_chrome_arguments, bool opt_debug_mode)
        {
            this.set_Engine(p_url, p_chrome_arguments, opt_debug_mode);
            this.launch_Process_chrome();
        }

        public Engine(string p_url, string p_chrome_arguments, bool p_opt_fullscreen, bool opt_debug_mode)
        {
            this.set_Engine(p_url, p_chrome_arguments, opt_debug_mode);
            this.Opt_fullscreen = p_opt_fullscreen;
            this.launch_Process_chrome();
        }

        public Engine(string p_url, string p_chrome_arguments, int p_index_screen, bool opt_debug_mode)
        {
            this.set_Engine(p_url, p_chrome_arguments, opt_debug_mode);
            this.Index_screen = p_index_screen;
            this.launch_Process_chrome();
        }

        public Engine(string p_url, string p_chrome_arguments, bool p_opt_fullscreen, int p_index_screen, bool opt_debug_mode)
        {
            this.set_Engine(p_url, p_chrome_arguments, opt_debug_mode);
            this.Opt_fullscreen = p_opt_fullscreen;
            this.Index_screen = p_index_screen;
            this.launch_Process_chrome();
        }
        #endregion

        #region Help private methods
        /// <summary>
        /// Set attributes to null value
        /// </summary>
        private void set_instance_nulls()
        {
            this.Current_chrome_process = null;
            this.Current_chrome_hWnd = IntPtr.Zero;
            this.Current_chrome_id = 0;
            this.Current_chrome_information = null;
            this.Opt_fullscreen = false;
            this.Index_screen = -1; // -1 = not defined
        }

        /// <summary>
        /// Instruciton commom ise in all constructors
        /// </summary>
        /// <param name="p_url"></param>
        /// <param name="p_chrome_arguments"></param>
        private void set_Engine(string p_url, string p_chrome_arguments, bool p_opt_debug_mode)
        {
            this.Opt_debug_mode = p_opt_debug_mode;
            this.set_instance_nulls();
            this.set_window_placement_in_Preferences_Chrome_File();
            this.Chrome_Url_start = p_url;
            this.Chrome_Arguments = p_chrome_arguments;
        }

        /// <summary>
        /// Use to kill all chrome's isntances
        /// </summary>
        public static void kill_all_chromes()
        {
            Process[] all_chromes = Process.GetProcessesByName("chrome");
            foreach (Process item in all_chromes)
            {
                item.Kill();
            }
        }

        /// <summary>
        /// Launch the process chrome and set 
        ///  -> this.Process_chrome 
        ///  -> this.Current_chrome_process
        ///  -> this.Current_chrome_hWnd
        ///  -> this.Current_chrome_id
        ///  -> this.Current_chrome_information
        /// </summary>
        private void launch_Process_chrome()
        {
            Process process_launch = new Process();
            process_launch.StartInfo.FileName = "chrome.exe";
            process_launch.StartInfo.Arguments = this.Chrome_StartInfo_Arguments;
            process_launch.Start();
            
            this.Process_chrome_launch = process_launch;
            Thread.Sleep(1000);

            Process[] processes_chrome = Process.GetProcessesByName("chrome");
            int current_chrome_id = 0;
            IntPtr current_chrome_hWnd = IntPtr.Zero;
            Process current_chrome_process;
            bool isSuccess_place_onscreen = true;
            StringBuilder current_chrome_informations = new StringBuilder();
            foreach (Process item in processes_chrome)
            {
                if (item.MainWindowTitle.Length > 0)
                {
                    current_chrome_informations.Append("   | Process launch = [" + item.ProcessName + " - ID " + item.Id + "]\n");
                    current_chrome_informations.Append("   | MainWindowHandle = " + item.MainWindowHandle + "\n");
                    current_chrome_informations.Append("   | MainWindowTitle = " + item.MainWindowTitle + "\n");
                    current_chrome_informations.Append("   | Handle = " + item.Handle + "\n");
                    current_chrome_informations.Append("   | HandleCount = " + item.HandleCount + "\n");
                    
                    current_chrome_process = item;
                    current_chrome_hWnd = item.MainWindowHandle;
                    current_chrome_id = item.Id;

                    current_chrome_informations.Append("   | Fullscreen mode = " + this.Opt_fullscreen + "\n");
                    if (this.Index_screen >= 0)
                    {
                        current_chrome_informations.Append("   | Try to place on screen " + this.Index_screen + "\n");
                        isSuccess_place_onscreen = this.ScreensHandler.place_onscreen(current_chrome_hWnd, this.Index_screen);
                        current_chrome_informations.Append(this.ScreensHandler.Log_place_onscreen);
                    }

                    if (this.Opt_fullscreen)
                    {
                        this.ScreensHandler.chrome_on_fullscreen(current_chrome_hWnd);
                    }

                    this.Current_chrome_process = current_chrome_process;
                    this.Current_chrome_hWnd = current_chrome_hWnd;
                    this.Current_chrome_id = current_chrome_id;
                    this.Current_chrome_information = current_chrome_informations.ToString();

                    if (this.Opt_debug_mode)
                    {
                        Console.WriteLine(this.Current_chrome_information);
                    }

                    if (isSuccess_place_onscreen == false)
                    {
                        Console.ForegroundColor = ConsoleColor.Red;
                        Console.WriteLine("[ERREUR] Impossible de lancer chrome sur l'écran " + (this.Index_screen + 1) + " car il n'existe pas. Ainsi"); 
                        Console.WriteLine("ce dernier est lancé sur le dernier écran disponible, à savoir l'écran " + this.ScreensHandler.ScreensInformation.NumberScreen);
                        Console.ResetColor();
                    }
                    break;
                }
            }
        }
        #endregion

        #region Methods : change window_placement in the Preferences file of chrome
        /// <summary>
        /// Before we launch chrome, we set the left monitor to default. We use the attribute "window_placement" in Preferences Chrome File to do it. This File is a Json.
        /// And the attribute window_placement have this structure :
        ///   "window_placement": {
        ///      "bottom": 779,
        ///      "left": 230,
        ///      "maximized": true,
        ///      "right": 1030,
        ///      "top": 179,
        ///      "work_area_bottom": 984,
        ///      "work_area_left": 0,
        ///      "work_area_right": 1280,
        ///      "work_area_top": 0
        ///   }
        /// </summary>
        private void set_window_placement_in_Preferences_Chrome_File()
        {
            string pathfile_Chrome_Preferences = this.get_chrome_preferences_file();
            string data_pathfile_Chrome_Preferences = System.IO.File.ReadAllText(pathfile_Chrome_Preferences);

            if (this.Opt_debug_mode)
            {
                Console.WriteLine("   | Try to deserialize the json chrome preferences");
            }
            try
            {
                dynamic Chrome_Preferences = JsonConvert.DeserializeObject(data_pathfile_Chrome_Preferences);

                // Set chrome into left monitor by default
                Chrome_Preferences["browser"]["window_placement"]["bottom"] = 600;
                Chrome_Preferences["browser"]["window_placement"]["left"] = 0;
                Chrome_Preferences["browser"]["window_placement"]["maximized"] = false;
                Chrome_Preferences["browser"]["window_placement"]["right"] = 800;
                Chrome_Preferences["browser"]["window_placement"]["top"] = 0;

                // Serialize JSON directly into the pathfile_Chrome_Preferences
                using (StreamWriter file = File.CreateText(pathfile_Chrome_Preferences))
                {
                    JsonSerializer serializer = new JsonSerializer();
                    serializer.Serialize(file, Chrome_Preferences);
                    if (this.Opt_debug_mode)
                    {
                        Console.WriteLine("   | Sucess to (deserialize / change / serialize) chrome preferences");
                    }
                }
            }
            catch (Exception e)
            {
                if (this.Opt_debug_mode)
                {
                    Console.ForegroundColor = ConsoleColor.Red;
                    Console.WriteLine("[ERREUR] Unable to change chrome Preferences cause of : " + e.Message);
                    Console.ResetColor();
                }
            }            
        }
        
        /// <summary>
        /// Get the true folder of Chrome User Data (in order to access the file Preference)
        /// </summary>
        /// <returns>Path of Chrome User Data</returns>
        private string get_chrome_user_data_folder()
        {
            // Default Location are :
            // 
            // -- Windows XP --
            // Google Chrome: C:\Documents and Settings\%USERNAME%\Local Settings\Application Data\Google\Chrome\User Data\Default
            // Chromium: C:\Documents and Settings\%USERNAME%\Local Settings\Application Data\Chromium\User Data\Default
            //
            // -- Windows 7 or Vista --
            // Google Chrome: C:\Users\%USERNAME%\AppData\Local\Google\Chrome\User Data\Default
            // Chromium: C:\Users\%USERNAME%\AppData\Local\Chromium\User Data\Default
            //
            // -- Mac OS X --
            // Google Chrome: ~/Library/Application Support/Google/Chrome/Default
            // Chromium: ~/Library/Application Support/Chromium/Default
            //
            // -- Linux --
            // Google Chrome: ~/.config/google-chrome/Default
            // Chromium: ~/.config/chromium/Default
            //
            // -- Chrome OS --
            // /home/chronos/              
            return Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData) + @"\Google\Chrome\User Data\";
        }

        /// <summary>
        /// Get the path file of the chrome Prefereces
        /// </summary>
        /// <returns></returns>
        private string get_chrome_preferences_file()
        {
            String pathfile_Chrome_Preferences = null;
            String temp_pathfile_Chrome_Preferences = this.get_chrome_user_data_folder() + @"\Default\Preferences";

            if (File.Exists(temp_pathfile_Chrome_Preferences))
            {
                pathfile_Chrome_Preferences = temp_pathfile_Chrome_Preferences;
            }
            else
            {
                // Try another method to get the Preference file
                const int LikeWin7 = 6;
                OperatingSystem osInfo = Environment.OSVersion;
                DirectoryInfo strDirectory;
                String path = null;
                String file = null;

                if (osInfo.Platform.Equals(System.PlatformID.Win32NT))
                {
                    if (osInfo.Version.Major == LikeWin7)
                    {
                        path = Environment.GetEnvironmentVariable("LocalAppData") + @"\Google\Chrome\User Data\Default";
                    }
                }

                if (path == null || path.Length == 0)
                    throw new ArgumentNullException("Fail. Bad OS");
                if (!(strDirectory = new DirectoryInfo(path)).Exists)
                    throw new DirectoryNotFoundException("Fail. The directory (" + path + ") was not found");
                if (!new FileInfo(file = Directory.GetFiles(strDirectory.FullName, "Preferences*")[0]).Exists)
                    throw new FileNotFoundException("Fail. The file Preferences in (" + path + ") was not found.", file);
                
                pathfile_Chrome_Preferences = file;
            }
            return pathfile_Chrome_Preferences;
        }
        #endregion

        #region Static methods : Use it in order to test
        /// <summary>
        /// Use to launch number_test_to_launch tests on number_test_to_launch screens.
        /// </summary>
        /// <param name="number_test_to_launch"></param>
        public static void launch_test(int number_test_to_launch, bool opt_debug_mode = false, bool opt_kill_all_chrome = false, int opt_delay=0)
        {
            if (opt_debug_mode)
            {
                Console.WriteLine(" > Options");
                Console.WriteLine("   | -> Kill all chrome before the launching = " + opt_kill_all_chrome);
                Console.WriteLine("   | -> Delay before the launching = " + opt_delay);
                Console.WriteLine("");
            }
            if (opt_kill_all_chrome)
            {
                Engine.kill_all_chromes();
            }
            
            if (number_test_to_launch > 0)
            {
                if (number_test_to_launch > 6)
                {
                    if (opt_debug_mode)
                        Console.WriteLine(" Too many tests on screen, 5 screens maximum");
                }
                else
                {
                    String[] urls = new String[] {
                        @"http://20100.lescigales.org/FF1/", 
                        @"http://20100.lescigales.org/FF1/solution/intro.php",
                        @"http://google.fr",
                        @"http://www.impsandmonsters.com/",
                        @"http://www.yodablog.net/"
                    };
                    Engine chrome_engine;
                    for (int i = 0; i < number_test_to_launch; i++)
                    {
                        if (opt_debug_mode)
                        {
                            Console.WriteLine(" > Test " + (i + 1));
                            Console.WriteLine("   ----------------------------------------------------");
                            Console.WriteLine("   | Lancer sur url = " + urls[i]);
                            Console.WriteLine("   | Options chrome = " + "--new-window --incognito");
                        }

                        if (opt_delay > 0)
                        {
                            Thread.Sleep(opt_delay);
                        }                        
                        chrome_engine = new Engine(urls[i], "--new-window --incognito", true, i, opt_debug_mode);
                    }
                }
            }
            else
            {
                if (opt_debug_mode)
                    Console.WriteLine(" Minimum is 1 screen...");
            }
        }
        #endregion
    }
}