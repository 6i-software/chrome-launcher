using System;
using CLAP;
using System.Text;
using CLAP.Validation;
using ChromeLauncher.Model;
using Newtonsoft.Json;
using System.Collections.Generic;
using System.Threading;

namespace ChromeLauncher.CommandLineApplication
{
    class ChromeLauncher_Commands
    {
        bool _debug_mode = false;
        bool _kill_all_chrome = false;
        int _delay = 0;
        
        #region CALP commands
        [Verb(Aliases="-v,-version", Description = "Donne la version & les copyrights de l'application.")]
        public void Version()
        {
            Console.WriteLine(GetVersionAndCopyright());
        }

        /// <summary>
        /// Show help message in console
        /// </summary>
        [Empty]
        [Verb(Aliases = "-h,-help,-?,?", Description = "Affiche l'aide.")]
        public void Help()
        {
            Console.WriteLine(GetVersionAndCopyright());

            console_write_title_color("---- Usages ----");
            Console.WriteLine("");
            Console.WriteLine("");
            Console.WriteLine(@"  ChromeLauncher screens");
            Console.WriteLine(@"  ChromeLauncher tests");
            Console.WriteLine(@"  ChromeLauncher tests -nombre=3 -debug -kill-all-chromes -delay=2500 -clear");
            Console.WriteLine(@"  ChromeLauncher t -n=3 -d -kill -delay=2500 -c");
            Console.WriteLine(@"  ChromeLauncher load -filepath=c:\foo\actions.json");
            Console.WriteLine(@"  ChromeLauncher -l -f=actions.json -d -c");

            Console.WriteLine("");
            console_write_title_color("---- Commandes ----");
            Console.WriteLine("");
            Console.WriteLine("");

            console_write_commands_color("    load");
            Console.Write(" : Charge un fichier json d'actions à passer aux lanceurs de chrome, " + Environment.NewLine);
            Console.Write("           avec en option le chemin du fichier. [-l, -load [-f=, -filepath=]]" + Environment.NewLine);

            console_write_commands_color("   tests");
            Console.Write(" : Lance un nombre donné de chromes sur autant d'écran, avec en option " + Environment.NewLine);
            Console.Write("           le nombre d'instance. [-t, -tests [-n=, -nombre= (default=1, min=1," + Environment.NewLine);
            Console.Write("           max=5)]]" + Environment.NewLine);

            console_write_commands_color(" screens");
            Console.Write(" : Donne des informations sur les écrans disponibles. [-s, -screens]" + Environment.NewLine);

            console_write_commands_color("    kill");
            Console.Write(" : Tue tous les processus chromes en cours d'éxecution avant le " + Environment.NewLine);
            Console.Write("           lancement. [-kill, -kill-all-chromes]" + Environment.NewLine);

            console_write_commands_color("   delay");
            Console.Write(" : Ajoute un délai d'attente, exprimé en milisecondes avant le" + Environment.NewLine);
            Console.Write("           lancement. [-delay=2500]" + Environment.NewLine);
            
            console_write_commands_color("   debug");
            Console.WriteLine(" : Active le debug lors de l'éxecution. [-d, -debug]");
            console_write_commands_color("   clear");
            Console.WriteLine(" : Efface la sortie console. [-c, -clear]");
            console_write_commands_color(" version");
            Console.WriteLine(" : Donne la version & copyright de l'application. [-v, -version]");
            console_write_commands_color("    help");
            Console.WriteLine(" : Affiche cette aide. [-h, -? -help]");
        }

        [Verb(Aliases = "-t,-tests", Description = "Lance des tests")]
        public void Tests(
            [MoreOrEqualTo(1), LessThan(5), DefaultValue(1)]
            [Description("Nombre de chrome à lancer sur autant d'écrans")]
            int nombre)
        {
            // Console.WriteLine("nombre de tests à lancer = {0}", nombre);

            if (this._debug_mode)
            {
                Console.WriteLine(GetVersionAndCopyright());
                console_write_title_color("---- DEBUG Screens informations ----");
                Console.WriteLine(Environment.NewLine + "");
                ScreensInformations screens = new ScreensInformations();
                Console.WriteLine("");
                Console.Write(screens.ToString());
                Console.WriteLine("");                
                console_write_title_color("---- DEBUG Launch tests ----");
                Console.WriteLine(Environment.NewLine + "");
                Console.WriteLine(" Nombre de tests à lancer sur autant d'écran : " + nombre);
                Console.WriteLine("");
            }

            Engine.launch_test(nombre, _debug_mode, _kill_all_chrome, _delay);

            if (this._debug_mode)
            {
                console_write_title_color("---- END ----");
                Console.WriteLine("");
            }
        }

        [Verb(Aliases = "-l,-load", Description = "Charge un fichier json")]
        public void Load(
            [FileExists]
            [RequiredAttribute]
            string filepath)
        {
            if (this._debug_mode)
            {
                ScreensInformations screens = new ScreensInformations();

                Console.WriteLine(GetVersionAndCopyright());
                console_write_title_color("---- DEBUG Screens informations ----");
                Console.WriteLine("");
                Console.WriteLine("");
                Console.Write(screens.ToString());
                Console.WriteLine("");
                console_write_title_color("---- DEBUG Load json file ----");
                Console.WriteLine(Environment.NewLine + "");
                Console.WriteLine(" > Fichier json chargé = " + filepath);
                Console.WriteLine("");
            }

            string data_load_json = "";
            try
            {
                data_load_json = System.IO.File.ReadAllText(filepath);
                if (this._debug_mode)
                {
                    Console.WriteLine("   | Now the json is stored into a string");
                }
            }
            catch (Exception e)
            {
                Console.ForegroundColor = ConsoleColor.Red;
                Console.WriteLine("[ERREUR] Unable to read the data into the loaded json, because : " + e.Message);
                Console.ResetColor();
            }

            try
            {
                ChromeActions chrome_actions = JsonConvert.DeserializeObject<ChromeActions>(data_load_json);
                bool p_kill_all_chromes = chrome_actions.Kill_all_chromes;
                int p_opt_Delay_milliseconds = chrome_actions.Delay_before_launch;

                if (this._debug_mode)
                {
                    Console.WriteLine("   | Now the json is deserialize into object type ChromeActions");
                    Console.WriteLine("   | Reading the ChromeActions and launch the "+chrome_actions.Actions.Count+" actions.");
                    Console.WriteLine("");

                    Console.WriteLine(" > Options global for all actions");
                    if (p_opt_Delay_milliseconds > 0)
                    {
                        Console.WriteLine("   | -> Delay before the launching = " + p_opt_Delay_milliseconds);
                    }
                    else
                    {
                        Console.WriteLine("   | -> Delay before the launching = disabled");
                    }
                    Console.WriteLine("   | -> Kill all chrome before the launching = " + p_kill_all_chromes);
                    Console.WriteLine("");
                }

                if (p_kill_all_chromes == true)
                {
                    Engine.kill_all_chromes();
                }
                
                int idx = 1;
                foreach (ChromeAction action in chrome_actions.Actions)
                {
                    if (this._debug_mode)
                    {
                        Console.WriteLine(" > Action " + idx);
                        Console.WriteLine("   | -> Url= " + action.Url);
                        Console.WriteLine("   | -> Arguments = " + action.Arguments);
                        Console.WriteLine("   | -> Fullscreen = " + action.Fullscreen);
                        Console.WriteLine("   | -> Indexscreen = " + action.Indexscreen);
                        Console.WriteLine("   | -> Launch now.");
                    }

                    if (p_opt_Delay_milliseconds > 0)
                    {
                        Thread.Sleep(p_opt_Delay_milliseconds);
                    }
                    Engine chrome_engine = new Engine(action.Url, action.Arguments, action.Fullscreen, action.Indexscreen, this._debug_mode);
                    idx++;
                }
            }
            catch (Exception e)
            {
                Console.ForegroundColor = ConsoleColor.Red;
                Console.WriteLine("[ERREUR] Unable to deserialize the load json "+filepath+", because: " + e.Message);
                Console.ResetColor();
            }  

            if (this._debug_mode)
            {
                console_write_title_color("---- END ----");
                Console.WriteLine("");
            }
        }

        [Verb(Aliases = "-s,-screens", Description = "Informations sur les écrans")]
        public void Screens()
        {
            ScreensInformations screens = new ScreensInformations();
            Console.WriteLine("");
            console_write_title_color("---- Screens informations ----");
            Console.WriteLine("");
            Console.WriteLine("");
            Console.Write(screens.ToString());
            Console.WriteLine("");
            console_write_title_color("---- END ----" + Environment.NewLine);
        }

        [Verb(Aliases = "-t,-title", Description = "Titre en couleur")]
        public void Title()
        {
            console_write_title_application_color();
        }

        [Global(Aliases = "c,cls", Description = "Efface la sortie console.")]
        public void Clear()
        {
            Console.Clear();
        }

        [Global(Aliases = "d", Description = "Active le mode debug")]
        public void Debug()
        {
            this._debug_mode = true;
        }

        [Global(Aliases = "kill,kill-all-chromes", Description = "Stop tous les processus chrome avant le lancement.")]
        public void Opt_Kill_all_chromes_options()
        {
            this._kill_all_chrome = true;
        }

        [Global(Aliases = "delay", Description = "Amorce un délai d'attente avant le lancement.")]
        public void Opt_Delay([RequiredAttribute] int delay_miliseconds)
        {
            this._delay = delay_miliseconds;
        }
        #endregion

        #region Get error parsing
        [Error]
        void HandleError(ExceptionContext context)
        {
            Console.ForegroundColor = ConsoleColor.Red;
            string current_type_exception = context.Exception.GetType().Name;
            string begin = "L'instruction précédente n'a pas été comrpise par l'application.";
            switch (current_type_exception)
            {
                case "MissingDefaultVerbException":
                    Console.WriteLine("[ERREUR] "+begin+" Aucune commande par defaut trouvé");
                    break;
                case "VerbNotFoundException":
                    Console.WriteLine("[ERREUR] "+begin+" La commande '" + ((VerbNotFoundException)context.Exception).Verb + "' n'existe pas");
                    break;
                case "MissingRequiredArgumentException":
                    Console.WriteLine("[ERREUR] " + begin + " La \n" +
                        "commande précedente requiert obligatoirement que le paramètre "
                        + "'-" + ((MissingRequiredArgumentException)context.Exception).ParameterName + "'"
                        + " soit \nrenseigné.");
                    break;
                default:
                    Console.WriteLine("[" + current_type_exception + "] " + context.Exception.Message);
                    break;
            }
            Console.ResetColor();
        }
        #endregion

        #region Get information about CLI Application
        /// <summary>
        /// Get the version & copyright of this CLI application
        /// </summary>
        /// <returns></returns>
        public string GetVersionAndCopyright()
        {
            return 
            @"_______________________________________________________________________________" + Environment.NewLine +
            @"                                                                               " + Environment.NewLine +
            @"    ___  _                            _                        _               " + Environment.NewLine +
            @"   |  _>| |_  _ _  ___ ._ _ _  ___   | |   ___  _ _ ._ _  ___ | |_  ___  _ _   " + Environment.NewLine +
            @"   | <__| . || '_|/ . \| ' ' |/ ._>  | |_ <_> || | || ' |/ | '| . |/ ._>| '_|  " + Environment.NewLine +
            @"   `___/|_|_||_|  \___/|_|_|_|\___.  |___|<___|`___||_|_|\_|_.|_|_|\___.|_|    " + Environment.NewLine +
            @"                                                                               " + Environment.NewLine +
            @"  Version : " + Program.Version + Environment.NewLine +
            @"  Contact : " + Program.Contact + Environment.NewLine +
            @"     Date : " + Program.Date + Environment.NewLine +
            @"                                                                               " + Environment.NewLine +
            @"  Copyright © 2014, Académie d'Orléans-Tours / Division de l'évaluation et de " + Environment.NewLine +
            @"  la prospective / Pôle analyse et développement informatique. " + Environment.NewLine +
            @"_______________________________________________________________________________" + Environment.NewLine;
        }

        public void console_write_title_application_color()
        {
            Console.Write(@"_______________________________________________________________________________" + Environment.NewLine);
            Console.Write(@"                                                                               " + Environment.NewLine); 
            c(@"     ");c2(@"  ___  _                            ___                               " + Environment.NewLine);
            c(@"     ");c2(@" |  _>| |_  _ _  ___ ._ _ _  ___   / __> ___  _ _  ___  ___ ._ _  ___ " + Environment.NewLine);
            c(@"     ");c2(@" | <__| . || '_>/ . \| ' ' |/ ._>  \__ \/ | '| '_>/ ._>/ ._>| ' |<_-< " + Environment.NewLine);
            c(@"     ");c2(@" `___/|_|_||_|  \___/|_|_|_|\___.  <___/\_|_.|_|  \___.\___.|_|_|/__/ " + Environment.NewLine);
            c(@"     ");c2(@"                                                                      " + Environment.NewLine);
            Console.Write(@"                                                                               " + Environment.NewLine);
            Console.Write(@"  Version : " + Program.Version + Environment.NewLine +
                          @"  Contact : " + Program.Contact + Environment.NewLine +
                          @"     Date : " + Program.Date + Environment.NewLine +
                          @"                                                                               " + Environment.NewLine +
                          @"  Copyright © 2014, Académie d'Orléans-Tours / Division de l'évaluation et de " + Environment.NewLine +
                          @"  la prospective / Pôle analyse et développement informatique. " + Environment.NewLine +
                          @"_______________________________________________________________________________" + Environment.NewLine);

            Console.ResetColor();
        }
        private void c(string str)
        {
            Console.ForegroundColor = ConsoleColor.Cyan;
            Console.Write(str);
            Console.ResetColor();
        }
        private void c2(string str)
        {
            Console.ForegroundColor = ConsoleColor.Cyan;
            Console.BackgroundColor = ConsoleColor.DarkCyan;
            Console.Write(str);
            Console.ResetColor();
        }

        private void console_write_title_color(string str)
        {
            Console.ForegroundColor = ConsoleColor.Cyan;
            Console.BackgroundColor = ConsoleColor.DarkCyan;
            Console.Write(str);
            Console.ResetColor();
        }

        private void console_write_commands_color(string str)
        {
            Console.ForegroundColor = ConsoleColor.Cyan;
            Console.Write(str);
            Console.ResetColor();
        }
        #endregion
    }
}
