using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace EpubSpellChecker
{
    /// <summary>
    /// Represents the possible settings
    /// </summary>
    class Settings
    {

        /// <summary>
        /// If true, OCR problems will also be checked on valid words
        /// </summary>
        public bool OCRWarnings { get; set; }
        
        /// <summary>
        /// If true, the check of OCR errors on valid words will only use OCR patterns that have fixed words in the  book
        /// </summary>
        public bool OnlyUseAppliedOCRPatternsForWarnings { get; set; }

        /// <summary>
        /// A list of all the tests that need to be executed
        /// </summary>
        public string[] EnabledTests { get; set; }


        /// <summary>
        /// Returns a default settings object
        /// </summary>
        /// <returns></returns>
        public static Settings GetDefault()
        {
            Settings settings = new Settings();
            settings.EnabledTests = System.Reflection.Assembly.GetExecutingAssembly().GetTypes()
                                                 .Where(t => t.GetInterfaces().Contains(typeof(ITest)))
                                                 .Select(t => t.Name)
                                                 .ToArray();
            settings.OCRWarnings = true;
            settings.OnlyUseAppliedOCRPatternsForWarnings = false;
            return settings;
        }

        
    }
}
