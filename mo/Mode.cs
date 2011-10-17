// --------------------------------------------------------------------------------------------------------------------
// <copyright file="Mode.cs" company="boolship">
//   Copyright © boolship@gmail.com 2011
// </copyright>
// <summary>
//   Set MOde (MO) console options. Program name MO pronounced "moe" as in gitmo.
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

    /// <summary>
    /// Class Mode configures Console devices.
    /// </summary>
    public class Mode
    {
        /// <summary>
        /// Console program main method
        /// </summary>
        /// <param name="args">
        /// The program arguments enter by user.
        /// </param>
        public static void Main(string[] args)
        {
            Mode mode = new Mode();
            mode.Run(args);
        }

        /// <summary>
        /// Console run() method
        /// </summary>
        /// <param name="args">
        /// The program arguments.
        /// </param>
        /// <exception cref="ArgumentException">
        /// Some error in program arguments.
        /// </exception>
        /// <returns>
        /// Success returns true, otherwise false.
        /// </returns>
        public bool Run(string[] args)
        {
            bool result = false;
            try
            {
                result = (args == null || args.Length == 0)
                        ? new Setting().ShowStatus()
                        : this.ParseCommands(args);
            }
            catch (ArgumentException)
            {
                Preset.ShowUsage();
            }
            catch (Exception e)
            {
                Console.WriteLine(e.GetType() + ", " + e.Message);
            }

            return result;
        }

        /// <summary>
        /// Parse program arguments.
        /// </summary>
        /// <param name="args">
        /// The program arguments.
        /// </param>
        /// <returns>
        /// True if successful, false if error in program arguments.
        /// </returns>
        public bool ParseCommands(ICollection<string> args)
        {
            bool includeCommand = false;
            bool includePreset = false;
            bool result = false;

            Setting console = new Setting();

            foreach (var cliArg in args)
            {
                int arg;
                bool isInt = Int32.TryParse(cliArg, out arg);

                if (isInt)
                {
                    // preset values or error
                    if (console.SettingPreset(arg))
                    {
                        includePreset = true;

                        // skip to NEXT cliArg
                        continue;
                    }

                    throw new ArgumentException("Unknown argument, use 0-3");
                }

                // command option, cliArg not Int32 (see continue)
                bool currentArgCommand = false;
                
                // commands like W=, B=, WC=, BC=, WL=, BL=
                List<string> commands = console.Commands;
                foreach (var command in commands)
                {
                    if (cliArg.IndexOf(command, 0, command.Length, StringComparison.OrdinalIgnoreCase) == 0)
                    {
                        string value = cliArg.Substring(command.Length);
                        int cmdArg;
                        bool isCmdArg;

                        if (command.Equals("QE=", StringComparison.OrdinalIgnoreCase) |
                            command.Equals("IN=", StringComparison.OrdinalIgnoreCase))
                        {
                            // test value is true/false
                            isCmdArg = Setting.TrueParse(value, out cmdArg);
                        }
                        else
                        {
                            // test value is Int32
                            isCmdArg = Int32.TryParse(value, out cmdArg);
                        }

                        if (String.IsNullOrEmpty(value) || !isCmdArg || cmdArg < 0)
                        {
                            throw new ArgumentException("Unknown argument value, see usage.");
                        }

                        // cmdArg is int >= 0, true==1, false==0
                        if (console.SettingCommand(command, cmdArg))
                        {
                            currentArgCommand = true;
                            includeCommand = true;

                            // break from command matching
                            break;
                        }
                    }
                }

                // not Int32 (see continue), not in commands
                if (!currentArgCommand)
                {
                    throw new ArgumentException("Unknown argument option, see usage.");
                }
            }

            // set console values or error
            console.SetConsole();

            // argument list has preset or command, error free
            if ((includePreset | includeCommand) && console.ErrorFreeSetting())
            {
                result = true;
            }

            // parsable setting
            return result;
        }
    }
}