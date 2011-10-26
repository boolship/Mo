// --------------------------------------------------------------------------------------------------------------------
// <copyright file="Preset.cs" company="boolship">
//   Copyright © boolship@gmail.com 2011
// </copyright>
// <summary>
//   Console preset options.
//
//   Copyright @2011 by boolship@gmail.com
//
//   This program is free software: you can redistribute it and/or modify
//   it under the terms of the GNU General Public License as published by
//   the Free Software Foundation, either version 3 of the License, or
//   (at your option) any later version.
//
//   This program is distributed in the hope that it will be useful,
//   but WITHOUT ANY WARRANTY; without even the implied warranty of
//   MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the
//   GNU General Public License for more details.
//
//   You should have received a copy of the GNU General Public License
//   along with this program.  If not, see http://www.gnu.org/licenses/.
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace mo
{
    using System;

    /// <summary>
    /// Console Preset options.
    /// </summary>
    public static class Preset
    {
        #region :: Preset class fields

        /// <summary>
        /// Program help message with format arguments 0-18
        /// </summary>
        public const string Usage =
            "Set Console Window & Buffer sizes, QuickEdit, other options from command line."
            + "\n"
            + "\n{0} [0|1|2|3] [[W|B|C]=n] [[WC|BC]=n] [[WL|BL]=n] [QE=[true|false]] \n\t[IN=[true|false]]"
            + "\n"
            + "\n              No arguement will display Console status."
            + "\n  0           Small  window ~1/3 screen. W: {1,3}/{5,-3} B: {9,3}/{13,-3} col/lin"
            + "\n  1           Medium window ~1/2 screen. W: {2,3}/{6,-3} B: {10,3}/{14,-3} col/lin"
            + "\n  2           Large  window ~2/3 screen. W: {3,3}/{7,-3} B: {11,3}/{15,-3} col/lin"
            + "\n  3           Max    window ~max screen. W: {4,3}/{8,-3} B: {12,3}/{16,-3} col/lin"
            + "\n              Preset 0|1|2|3 also sets QE={17}, IN={18}"
            + "\n"
            + "\n  W=n         Window=n, where n=specify|0|1|2|3 (Small, Medium, Large, Max)"
            + "\n  B=n         Buffer=n, where n=specify|0|1|2|3 (Small, Medium, Large, Max)"
            + "\n  C=n         Column=n, where n=specify|0|1|2|3 (Small, Medium, Large, Max)"
            + "\n  WC=n        Window Columns=n, where n=specify|0|1|2|3"
            + "\n  BC=n        Buffer Columns=n, where n=specify|0|1|2|3"
            + "\n  WL=n        Window Lines=n, where n=specify|0|1|2|3"
            + "\n  BL=n        Buffer Lines=n, where n=specify|0|1|2|3"
            + "\n  QE=true     QuickEdit Mode set true"
            + "\n  QE=false    QuickEdit Mode set false"
            + "\n  IN=true     Insert Mode set true"
            + "\n  IN=false    Insert Mode set false"
            + "\n"
            + "\nCommand line (CLI) arguments are processed in order. This allows general"
            + "\noptions, such as presets, to be overridden with subsequent details."
            + "\nOverride QE and IN with preset 0|1|2|3 using QE=false, IN=false. The"
            + "\nactual buffer maximum size might be limited to less than 10000."
            + "\nPreset sizes are based on screen resolution."
            + "\n"
            + "\nSimple and quick usage examples include: \"mo\", \"mo 1\", \"mo 3 b=2\","
            + "\n\"mo 2 qe=f in=f\", \"mo 1 c=100\"";

        /// <summary>
        /// command line name
        /// </summary>
        private const string CliName = "mo";

        #endregion

        /// <summary>
        /// Initializes static members of the <see cref="Preset"/> class. 
        /// </summary>
        static Preset()
        {
            // preset BUFFER LINES arguments 0, 1, 2, 3
            BufLin0 = 600;
            BufLin1 = 2400;
            BufLin2 = 10000;
            BufLin3 = Int16.MaxValue - 1;

            // preset BUFFER COLS arguments 0, 1, 2, 3
            BufCol0 = ConsoleLargestWindowWidth * 2 / 3;
            BufCol1 = ConsoleLargestWindowWidth;
            BufCol2 = ConsoleLargestWindowWidth * 4 / 3;
            BufCol3 = Int16.MaxValue - 1;

            // preset WINDOW LINES arguments 0, 1, 2, 3
            WinLin0 = (ConsoleLargestWindowHeight / 3) - 4;
            WinLin1 = (ConsoleLargestWindowHeight / 2) - 4;
            WinLin2 = (ConsoleLargestWindowHeight * 2 / 3) - 4;
            WinLin3 = ConsoleLargestWindowHeight - 4;

            // preset WINDOW COLS arguments 0, 1, 2, 3
            WinCol0 = (ConsoleLargestWindowWidth / 3) - 5;
            WinCol1 = (ConsoleLargestWindowWidth / 2) - 5;
            WinCol2 = (ConsoleLargestWindowWidth * 2 / 3) - 5;
            WinCol3 = ConsoleLargestWindowWidth - 5;

            // other options
            SettingError = -1;
        }

        #region :: Preset class properties

        /// <summary>
        /// Gets or sets static BufLin0.
        /// </summary>
        public static int BufLin0 { get; set; }

        /// <summary>
        /// Gets or sets static BufLin1.
        /// </summary>
        public static int BufLin1 { get; set; }

        /// <summary>
        /// Gets or sets static BufLin2.
        /// </summary>
        public static int BufLin2 { get; set; }

        /// <summary>
        /// Gets or sets static BufLin3.
        /// </summary>
        public static int BufLin3 { get; set; }

        /// <summary>
        /// Gets or sets static BufCol0.
        /// </summary>
        public static int BufCol0 { get; set; }

        /// <summary>
        /// Gets or sets static BufCol1.
        /// </summary>
        public static int BufCol1 { get; set; }

        /// <summary>
        /// Gets or sets static BufCol2.
        /// </summary>
        public static int BufCol2 { get; set; }

        /// <summary>
        /// Gets or sets static BufCol3.
        /// </summary>
        public static int BufCol3 { get; set; }

        /// <summary>
        /// Gets or sets static WinLin0.
        /// </summary>
        public static int WinLin0 { get; set; }

        /// <summary>
        /// Gets or sets static WinLin1.
        /// </summary>
        public static int WinLin1 { get; set; }

        /// <summary>
        /// Gets or sets static WinLin2.
        /// </summary>
        public static int WinLin2 { get; set; }

        /// <summary>
        /// Gets or sets static WinLin3.
        /// </summary>
        public static int WinLin3 { get; set; }

        /// <summary>
        /// Gets or sets static WinCol0.
        /// </summary>
        public static int WinCol0 { get; set; }

        /// <summary>
        /// Gets or sets static WinCol1.
        /// </summary>
        public static int WinCol1 { get; set; }

        /// <summary>
        /// Gets or sets static WinCol2.
        /// </summary>
        public static int WinCol2 { get; set; }

        /// <summary>
        /// Gets or sets static WinCol3.
        /// </summary>
        public static int WinCol3 { get; set; }

        /// <summary>
        /// Gets or sets value error code.
        /// </summary>
        internal static int SettingError { get; set; }

        /// <summary>
        /// Gets unit test workaround when Console.LargestWindowHeight == 0.
        /// </summary>
        internal static int ConsoleLargestWindowHeight
        {
            get
            {
                int def;
                try
                {
                    def = Console.LargestWindowHeight == 0 ? 100 : Console.LargestWindowHeight;
                }
                catch (System.IO.IOException)
                {
                    def = 100;
                }

                return def;
            }
        }

        /// <summary>
        /// Gets unit test workaround when Console.LargestWindowWidth == 0.
        /// </summary>
        internal static int ConsoleLargestWindowWidth
        {
            get
            {
                int def;
                try
                {
                    def = Console.LargestWindowWidth == 0 ? 250 : Console.LargestWindowWidth;
                }
                catch (System.IO.IOException)
                {
                    def = 250;
                }

                return def;
            }
        }
        
        #endregion

        /// <summary>
        /// Show program usage/help based on screen resolution.
        /// </summary>
        public static void ShowUsage()
        {
            Console.WriteLine(String.Format(
                Usage,
                CliName,
                WinCol0,
                WinCol1,
                WinCol2,
                WinCol3,
                WinLin0,
                WinLin1,
                WinLin2,
                WinLin3,
                BufCol0,
                BufCol1,
                BufCol2,
                BufCol3,
                BufLin0,
                BufLin1,
                BufLin2,
                BufLin3,
                true,
                true));
        }
    }
}
