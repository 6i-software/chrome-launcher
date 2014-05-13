using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ChromeLauncher.Model
{
    /// <summary>
    /// {
    ///  "actions": [
    ///    {
    ///      "url":"http://20100.lescigales.org/FF1/",
    ///      "arguments":"--new-window --incognito",
    ///      "fullscreen": true,
    ///      "indexscreen": 0
    ///    },
    ///    {
    ///      "url":"http://20100.lescigales.org/FF1/solution/intro.php",
    ///      "arguments":"--new-window --incognito",
    ///      "fullscreen": true,
    ///      "indexscreen": 1
    ///    }
    ///  ]
    ///}
    /// </summary>
    public class ChromeActions
    {
        public bool Kill_all_chromes { get; set; }
        public int Delay_before_launch { get; set; }
        
        public List<ChromeAction> Actions { get; set; }
    }

    /// <summary>
    ///     ///    {
    ///      "url":"http://20100.lescigales.org/FF1/solution/intro.php",
    ///      "arguments":"--new-window --incognito",
    ///      "fullscreen": true,
    ///      "indexscreen": 1
    ///    }
    /// </summary>
    public class ChromeAction
    {
        /// <summary>
        /// The url to open with chrome
        /// </summary>
        public string Url { get; set; }

        /// <summary>
        /// The argument to pass with the chrome (ex --new-window --incognito")
        /// </summary>
        public string Arguments { get; set; }

        /// <summary>
        /// Used in order to active the fullscreen mode with chrome (like we send the F11 keys to chrome)
        /// </summary>
        public bool Fullscreen { get; set; }

        /// <summary>
        /// Indicates in wicht number screen to launch chrome
        /// </summary>
        public int Indexscreen { get; set; }
    }
}
