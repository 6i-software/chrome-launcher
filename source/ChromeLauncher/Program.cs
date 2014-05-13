#region License
//
// ChromeLauncher
//
// Author:
//   Vincent Blain (vincent.blain@ac-orleans-tours.fr)
//
// -----------------------------------------------------------
//    Copyright © 2014, 
//    Rectorat de l'académie d'Orléans-Tours 
//    Division de l'évaluation et de la prospective
//    Pôle analyse et développement informatique
// -----------------------------------------------------------
// 
// En utilisant tout ou partie de l'application, vous acceptez toutes 
// les dispositions de cette licence.
//
// Ce logiciel est libre d’utilisation ou redistribution sous la condition de prise 
// de contact avec la Division de l’évaluation et de la prospective (DEP) de 
// l'académie d'Orléans-Tours. (ce.dep@ac-orleans-tours.fr)
// Ce logiciel est distribué car potentiellement utile, mais sans aucune garantie, 
// ni explicite ni implicite, y compris les garanties d'évolution ou d'adaptation 
// dans un but spécifique. 
// Vous n'avez pas la liberté de modifier/publier des améliorations du logiciel sans 
// contact avec la Division de l'évaluation et de la prospective (DEP) de l'académie 
// d'Orléans-Tours.
//
#endregion

using System;
using ChromeLauncher.CommandLineApplication;
using CLAP;
using System.Reflection;
using System.Collections.Generic;
using System.IO;

namespace ChromeLauncher
{
    class Program
    {
        #region Attributes : CLI application
        /// <summary>
        /// The version of this CLI application
        /// </summary>
        public static readonly String Version = "v0.0.2";
        
        /// <summary>
        /// The name of this CLI application
        /// </summary>
        public static readonly String NameApplication = "ChromeLauncher";

        /// <summary>
        /// Who contact?
        /// </summary>
        public static readonly String Contact = "ce.dep@ac-orleans-tours.fr";

        /// <summary>
        /// Date last major version
        /// </summary>
        public static readonly String Date = "avril 2014";
        #endregion

        /// <summary>
        /// The entry of this CLI application
        /// </summary>
        /// <param name="args"></param>
        /// <returns></returns>
        static void Main(string[] arg)
        {
            ChromeLauncher_Commands commands = new ChromeLauncher_Commands();
            Parser<ChromeLauncher_Commands> parser = new Parser<ChromeLauncher_Commands>()
            {
                HelpGenerator = new ChromeLauncher_HelpGenerator()
            };
            parser.Run(arg, commands);
        }
    }
}

