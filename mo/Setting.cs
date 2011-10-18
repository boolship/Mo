﻿// --------------------------------------------------------------------------------------------------------------------
// <copyright file="Setting.cs" company="boolship">
//   Copyright © boolship@gmail.com 2011
// </copyright>
// <summary>
//   Console setting commands. 
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
    using System.Collections.Generic;
    using System.ComponentModel;
    using System.IO;
    using System.Runtime.InteropServices;

    /// <summary>
    /// Console Setting options.
    /// </summary>
    public class Setting
    {
        #region :: Setting class fields

        /// <summary>
        /// Console status message with format arguments 0-5
        /// </summary>
        public const string Status =
                "Console Status:"
                + "\n---------------"
                + "\n\tWindow Size        = Lines: {0,4}, Columns: {1,4}"
                + "\n\tScreen Buffer Size = Lines: {2,4}, Columns: {3,4}"
                + "\n"
                + "\nEdit Options:"
                + "\n-------------"
                + "\n\tQuickEdit Mode: {4,3}, Insert Mode: {5,3}";

        /// <summary>
        /// Available program commands with int and true|false values.
        /// </summary>
        private readonly List<string> availableCommands = new List<string> { "W=", "B=", "WC=", "BC=", "WL=", "BL=", "QE=", "IN=" };

        /// <summary>
        /// To disable QuickEdit or Insert mode, use ENABLE_EXTENDED_FLAGS without other flag.
        /// </summary>
        private const int EnableExtendedFlags = 0x0080;

        /// <summary>
        /// To enable QuickEdit mode, use ENABLE_QUICK_EDIT_MODE | ENABLE_EXTENDED_FLAGS. 
        /// </summary>
        private const int EnableQuickEditMode = 0x0040;

        /// <summary>
        /// To enable Insert mode, use ENABLE_INSERT_MODE | ENABLE_EXTENDED_FLAGS. 
        /// </summary>
        private const int EnableInsertMode = 0x0020;

        /// <summary>
        /// Command console window columns.
        /// </summary>
        private int windowColumns;

        /// <summary>
        /// Command console window lines.
        /// </summary>
        private int windowLines;

        /// <summary>
        /// Command console buffer columns.
        /// </summary>
        private int bufferColumns;

        /// <summary>
        /// Command console buffer lines.
        /// </summary>
        private int bufferLines;

        /// <summary>
        /// Command console insert mode.
        /// </summary>
        private int insertMode;

        /// <summary>
        /// Command console quick edit.
        /// </summary>
        private int quickEdit;
   
        #endregion

        #region :: Setting class properties

        /// <summary>
        /// Gets Commands list.
        /// </summary>
        public List<string> Commands
        {
            get { return availableCommands; }
        }

        /// <summary>
        /// Gets or sets WindowColumns with presets 0|1|2|3.
        /// </summary>
        internal int WindowColumns
        {
            private get
            {
                return this.windowColumns;
            }

            set
            {
                this.windowColumns =
                    (value == 0)
                        ? Preset.WinCol0
                        : (value == 1)
                              ? Preset.WinCol1
                              : (value == 2) ? Preset.WinCol2 : (value == 3) ? Preset.WinCol3 : value;
            }
        }

        /// <summary>
        /// Gets or sets WindowLines with presets 0|1|2|3.
        /// </summary>
        internal int WindowLines
        {
            private get
            {
                return this.windowLines;
            }

            set
            {
                this.windowLines =
                    (value == 0)
                        ? Preset.WinLin0
                        : (value == 1)
                              ? Preset.WinLin1
                              : (value == 2) ? Preset.WinLin2 : (value == 3) ? Preset.WinLin3 : value;
            }
        }

        /// <summary>
        /// Gets or sets BufferColumns with presets 0|1|2|3.
        /// </summary>
        internal int BufferColumns
        {
            private get
            {
                return this.bufferColumns;
            }

            set
            {
                bufferColumns =
                    (value == 0)
                        ? Preset.BufCol0
                        : (value == 1)
                              ? Preset.BufCol1
                              : (value == 2) ? Preset.BufCol2 : (value == 3) ? Preset.BufCol3 : value;
            }
        }

        /// <summary>
        /// Gets or sets BufferLines with presets 0|1|2|3.
        /// </summary>
        internal int BufferLines
        {
            private get
            {
                return this.bufferLines;
            }

            set
            {
                bufferLines =
                    (value == 0)
                        ? Preset.BufLin0
                        : (value == 1)
                              ? Preset.BufLin1
                              : (value == 2) ? Preset.BufLin2 : (value == 3) ? Preset.BufLin3 : value;
            }
        }

        /// <summary>
        /// Gets or sets InsertMode with presets 0|1|2|3 true.
        /// True = 1, False = Int16.MaxVal
        /// </summary>
        internal int InsertMode
        {
            private get { return this.insertMode; }
            set { insertMode = (value == 0 || value == 1 || value == 2 || value == 3) ? 1 : value; }
        }

        /// <summary>
        /// Gets or sets QuickEdit with presets 0|1|2|3 true.
        /// True = 1, False = Int16.MaxVal
        /// </summary>
        internal int QuickEdit
        {
            private get { return this.quickEdit; }
            set { quickEdit = (value == 0 || value == 1 || value == 2 || value == 3) ? 1 : value; }
        }

        /// <summary>
        /// Gets unit test workaround when Console.WindowHeight throws IOException.
        /// </summary>
        private static int ConsoleWindowHeight
        {
            get
            {
                int def;
                try
                {
                    def = Console.WindowHeight;
                }
                catch (IOException)
                {
                    def = 50;
                }

                return def;
            }
        }

        /// <summary>
        /// Gets unit test workaround when Console.WindowWidth throws IOException.
        /// </summary>
        private static int ConsoleWindowWidth
        {
            get
            {
                int def;
                try
                {
                    def = Console.WindowWidth;
                }
                catch (IOException)
                {
                    def = 100;
                }

                return def;
            }
        }

        /// <summary>
        /// Gets unit test workaround when Console.BufferHeight throws IOException.
        /// </summary>
        private static int ConsoleBufferHeight
        {
            get
            {
                int def;
                try
                {
                    def = Console.BufferHeight;
                }
                catch (IOException)
                {
                    def = 500;
                }

                return def;
            }
        }

        /// <summary>
        /// Gets unit test workaround when Console.BufferWidth throws IOException.
        /// </summary>
        private static int ConsoleBufferWidth
        {
            get
            {
                int def;
                try
                {
                    def = Console.BufferWidth;
                }
                catch (IOException)
                {
                    def = 100;
                }

                return def;
            }
        }

        /// <summary>
        /// Gets a value indicating whether ConsoleQuickEdit set.
        /// </summary>
        private bool ConsoleQuickEdit
        {
            get
            {
                return IsSet(EnableQuickEditMode) ? true : false;
            }
        }

        /// <summary>
        /// Gets a value indicating whether ConsoleInsertMode set.
        /// </summary>
        private bool ConsoleInsertMode
        {
            get
            {
                return IsSet(EnableInsertMode) ? true : false;
            }
        }

        #endregion

        /// <summary>
        /// Parse true and false to int.
        /// </summary>
        /// <param name="s">
        /// Parse the string "true" or "t" or "false" or "f", case insensitive.
        /// </param>
        /// <param name="result">
        /// Set result equal to 1 if true, or result equal to Int16.MaxValue otherwise.
        /// </param>
        /// <returns>
        /// Parsing "true" or "t" or "false" or "f" returns true, otherwise false.
        /// </returns>
        public static bool TrueParse(string s, out int result)
        {
            if (s.Equals("true", StringComparison.OrdinalIgnoreCase) ||
                s.Equals("t", StringComparison.OrdinalIgnoreCase))
            {
                result = 1;
                return true;
            }

            result = Int16.MaxValue;
            if (s.Equals("false", StringComparison.OrdinalIgnoreCase) ||
                s.Equals("f", StringComparison.OrdinalIgnoreCase))
            {
                return true;
            }

            return false;
        }

        /// <summary>
        /// Check if flag set in Current Mode.
        /// </summary>
        /// <param name="flag">
        /// The console flag, e.g. ENABLE_QUICK_EDIT_MODE, ENABLE_INSERT_MODE.
        /// See http://msdn.microsoft.com/en-us/library/ms686033%28VS.85%29.aspx
        /// </param>
        /// <returns>
        /// True if flag is set, else false.
        /// </returns>
        public bool IsSet(int flag)
        {
            const int StdInputHandle = -10;
            var handleConsole = GetStdHandle(StdInputHandle);

            // #TODO# handleConsole not working in unit test, returns false
            if (String.Equals(handleConsole.ToString(), "0"))
            {
            }

            int currentMode;
            GetConsoleMode(handleConsole, out currentMode);

            return (currentMode | flag) == currentMode;
        }

        /// <summary>
        /// Show console status.
        /// </summary>
        /// <returns>
        /// Success returns true, otherwise false.
        /// </returns>
        public bool ShowStatus()
        {
            Console.WriteLine(String.Format(
                Status,
                ConsoleWindowHeight,
                ConsoleWindowWidth,
                ConsoleBufferHeight,
                ConsoleBufferWidth,
                ConsoleQuickEdit,
                ConsoleInsertMode));
            return true;
        }

        /// <summary>
        /// Check if error in setting.
        /// </summary>
        /// <returns>
        /// Success returns true, otherwise false.
        /// </returns>
        public bool ErrorFreeSetting()
        {
            return (WindowColumns < 0 || WindowLines < 0 || BufferColumns < 0 || BufferLines < 0 ||
                QuickEdit < 0 || InsertMode < 0)
                      ? false
                      : true;
        }

        /// <summary>
        /// Apply console settings in specific order.
        /// </summary>
        /// <returns>
        /// Success returns true, otherwise false
        /// </returns>
        public bool SetConsole()
        {
            if (BufferLines > 0)
            {
                int bl = (WindowLines > 0 && BufferLines < WindowLines) ? WindowLines : BufferLines;
                string msg = "buf lin set " + bl;
                try
                {
                    if (bl < Preset.BufLin0 | bl > Preset.BufLin3)
                    {
                        throw new ArgumentOutOfRangeException();
                    }

                    Console.Clear(); // set window position = 0
                    Console.BufferHeight = bl;
                }
                catch (IOException)
                {
                    // when unit test Console.<Property> throws IOException.
                }
                catch (ArgumentOutOfRangeException)
                {
                    if (bl < Preset.BufLin0)
                    {
                        msg = String.Format("err: minimum buf lin {0}", Preset.BufLin0);                        
                    }
                    else if (bl > Preset.BufLin3)
                    {
                        msg = String.Format("err: maximum buf lin {0}", Preset.BufLin3);
                    }
                }

                Console.WriteLine(msg);
            }

            if (BufferColumns > 0)
            {
                int bc = (WindowColumns > 0 && BufferColumns < WindowColumns) ? WindowColumns : BufferColumns;
                string msg = "buf col set " + bc;
                try
                {
                    if (bc < Preset.BufCol0 | bc > Preset.BufCol3)
                    {
                        throw new ArgumentOutOfRangeException();
                    }

                    Console.BufferWidth = bc;
                }
                catch (IOException)
                {
                    // when unit test Console.<Property> throws IOException.
                }
                catch (ArgumentOutOfRangeException)
                {
                    if (bc < Preset.BufCol0)
                    {
                        msg = String.Format("err: minimum buf col {0}", Preset.BufCol0);
                    }
                    else if (bc > Preset.BufCol3)
                    {
                        msg = String.Format("err: maximum buf col {0}", Preset.BufCol3);
                    }
                }

                Console.WriteLine(msg);
            }

            if (WindowLines > 0)
            {
                string msg = "win lin set " + WindowLines;
                try
                {
                    if (WindowLines < Preset.WinLin0 | WindowLines > Preset.WinLin3)
                    {
                        throw new ArgumentOutOfRangeException();
                    }

                    Console.WindowHeight = WindowLines;
                }
                catch (IOException)
                {
                    // when unit test Console.<Property> throws IOException.
                }
                catch (ArgumentOutOfRangeException)
                {
                    if (WindowLines < Preset.WinLin0)
                    {
                        msg = String.Format("err: minimum win lin {0}", Preset.WinLin0);                        
                    }
                    else if (WindowLines > Preset.WinLin3)
                    {
                        msg = String.Format("err: maximum win lin {0}", Preset.WinLin3);
                    }
                }

                Console.WriteLine(msg);
            }

            if (WindowColumns > 0)
            {
                string msg = "win col set " + WindowColumns;
                try
                {
                    if (WindowColumns < Preset.WinCol0 | WindowColumns > Preset.WinCol3)
                    {
                        throw new ArgumentOutOfRangeException();
                    }

                    Console.WindowWidth = WindowColumns;
                }
                catch (IOException)
                {
                    // when unit test Console.<Property> throws IOException.
                }
                catch (ArgumentOutOfRangeException)
                {
                    if (WindowColumns < Preset.WinCol0)
                    {
                        msg = String.Format("err: minimum win col {0}", Preset.WinCol0);                                            
                    }
                    else if (WindowColumns > Preset.WinCol3)
                    {
                        msg = String.Format("err: maximum win col {0}", Preset.WinCol3);
                    }
                }

                Console.WriteLine(msg);
            }

            if (QuickEdit > 0)
            {
                try
                {
                    const int StdInputHandle = -10;
                    var handleConsole = GetStdHandle(StdInputHandle);
                    int currentMode;
                    GetConsoleMode(handleConsole, out currentMode);

                    if (QuickEdit == 1 && !IsSet(EnableQuickEditMode))
                    {
                        int result = SetConsoleMode(handleConsole, currentMode | EnableQuickEditMode | EnableExtendedFlags);
                        if (result == 0)
                        {
                            throw new Win32Exception(Marshal.GetLastWin32Error());
                        }                        
                    }
                    else if (QuickEdit == Int16.MaxValue && IsSet(EnableQuickEditMode))
                    {
                        int currentModeOff = currentMode - EnableQuickEditMode;
                        int result = SetConsoleMode(handleConsole, currentModeOff | EnableExtendedFlags);
                        if (result == 0)
                        {
                            throw new Win32Exception(Marshal.GetLastWin32Error());
                        }                                                
                    }
                }
                catch (Win32Exception w)
                {
                    // more info using: w.StackTrace, Exception e = w.GetBaseException();
                    TextWriter ew = Console.Error;
                    ew.WriteLine("win32 err: " + w.ErrorCode + ": " + w.Message);
                }

                Console.WriteLine("quick edit set " + (QuickEdit == 1 ? true : false));
            }

            if (InsertMode > 0)
            {
                try
                {
                    const int StdInputHandle = -10;
                    var handleConsole = GetStdHandle(StdInputHandle);
                    int currentMode;
                    GetConsoleMode(handleConsole, out currentMode);

                    if (InsertMode == 1 && !IsSet(EnableInsertMode))
                    {
                        int result = SetConsoleMode(handleConsole, currentMode | EnableInsertMode | EnableExtendedFlags);
                        if (result == 0)
                        {
                            throw new Win32Exception(Marshal.GetLastWin32Error());
                        }
                    }
                    else if (InsertMode == Int16.MaxValue && IsSet(EnableInsertMode))
                    {
                        int currentModeOff = currentMode - EnableInsertMode;
                        int result = SetConsoleMode(handleConsole, currentModeOff | EnableExtendedFlags);
                        if (result == 0)
                        {
                            throw new Win32Exception(Marshal.GetLastWin32Error());
                        }
                    }
                }
                catch (Win32Exception w)
                {
                    // more info using: w.StackTrace, Exception e = w.GetBaseException();
                    TextWriter ew = Console.Error;
                    ew.WriteLine("win32 err: " + w.ErrorCode + ": " + w.Message);
                }

                Console.WriteLine("insert set " + (InsertMode == 1 ? true : false));
            }

            return false;
        }

        /// <summary>
        /// Set Console command preset option or specified values.
        /// </summary>
        /// <param name="command">
        /// The command.
        /// </param>
        /// <param name="option">
        /// The option.
        /// </param>
        /// <returns>
        /// Success returns true, otherwise false.
        /// </returns>
        public bool SettingCommand(string command, int option)
        {
            if (String.Compare(command, "W=", StringComparison.OrdinalIgnoreCase) == 0)
            {
                WindowColumns = option;
                WindowLines = option;
            }
            else if (String.Compare(command, "B=", StringComparison.OrdinalIgnoreCase) == 0)
            {
                BufferColumns = option;
                BufferLines = option;
            }
            else if (String.Compare(command, "WC=", StringComparison.OrdinalIgnoreCase) == 0)
            {
                WindowColumns = option;
            }
            else if (String.Compare(command, "BC=", StringComparison.OrdinalIgnoreCase) == 0)
            {
                BufferColumns = option;
            }
            else if (String.Compare(command, "WL=", StringComparison.OrdinalIgnoreCase) == 0)
            {
                WindowLines = option;
            }
            else if (String.Compare(command, "BL=", StringComparison.OrdinalIgnoreCase) == 0)
            {
                BufferLines = option;
            }
            else if (String.Compare(command, "QE=", StringComparison.OrdinalIgnoreCase) == 0)
            {
                QuickEdit = option;
            }
            else if (String.Compare(command, "IN=", StringComparison.OrdinalIgnoreCase) == 0)
            {
                InsertMode = option;
            }

            return ErrorFreeSetting();
        }

        /// <summary>
        /// Set Preset option for window, buffer and other values.
        /// </summary>
        /// <param name="option">
        /// preset option 0|1|2|3
        /// </param>
        /// <returns>
        /// Success returns true, otherwise false.
        /// </returns>
        public bool SettingPreset(int option)
        {
            WindowColumns = option;
            WindowLines = option;
            BufferColumns = option;
            BufferLines = option;

            // Preset 0|1|2|3 is true, Int16.MaxVal is false
            QuickEdit = option;
            InsertMode = option;

            return ErrorFreeSetting();
        }

        [DllImport("kernel32.dll", SetLastError = true)]
        internal static extern IntPtr GetStdHandle(int stdHandle);

        [DllImport("kernel32.dll", SetLastError = true)]
        internal static extern int GetConsoleMode(IntPtr consoleHandle, out int mode);

        [DllImport("kernel32.dll", SetLastError = true)]
        internal static extern int SetConsoleMode(IntPtr consoleHandle, int mode);
    }
}
