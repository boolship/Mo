// --------------------------------------------------------------------------------------------------------------------
// <copyright file="UnitTestMode.cs" company="boolship">
//   Copyright © boolship@gmail.com 2011
// </copyright>
// <summary>
//   Unit tests for MOde (MO) console options. Program name MO pronounced "moe" as in gitmo.
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

namespace TestMo
{
    using System;
    using System.Collections.Generic;
    using System.IO;
    using Microsoft.VisualStudio.TestTools.UnitTesting;
    using mo;

    /// <summary>
    /// Simple Mode unit tests.
    /// </summary>
    [TestClass]
    public class UnitTestMode
    {
        #region :: UnitTestMode class fields

        /// <summary>
        /// String handling helper.
        /// </summary>
        private const string EndTag = "\r\n";

        #endregion

        /// <summary>
        /// Test Console Status
        /// </summary>
        [TestMethod]
        public void TestArgumentNullShowStatus()
        {
            using (StringWriter sw = new StringWriter())
            {
                Console.SetOut(sw);

                // mo
                Mode.Main(null);

                // use defaults in Setting class properties
                string expected = String.Format(Setting.Status + EndTag, 50, 100, 500, 100, false, false);
                string actual = sw.ToString();

                Assert.AreEqual(expected.Length, actual.Length);
                Assert.AreEqual(expected, actual);
            }
        }

        /// <summary>
        /// Test Show Usage
        /// </summary>
        [TestMethod]
        public void TestArgumentErrorShowUsage()
        {
            List<object[]> argList =
                new List<object[]>
                    {
                        new[] { "0", "error" },
                        new[] { "0", "W=error" }
                    };
            foreach (var arg in argList)
            {
                using (StringWriter sw = new StringWriter())
                {
                    Console.SetOut(sw);

                    // mo 0 error
                    Mode.Main((string[])arg);

                    // use defaults in Preset class properties
                    object[] presets =
                        new object[]
                            {
                                "mo",
                                Preset.WinCol0, Preset.WinCol1, Preset.WinCol2, Preset.WinCol3,
                                Preset.WinLin0, Preset.WinLin1, Preset.WinLin2, Preset.WinLin3,
                                Preset.BufCol0, Preset.BufCol1, Preset.BufCol2, Preset.BufCol3,
                                Preset.BufLin0, Preset.BufLin1, Preset.BufLin2, Preset.BufLin3,
                                true, true
                            };
                    string expected = String.Format(Preset.Usage + EndTag, presets);
                    string actual = sw.ToString();

                    Assert.AreEqual(expected.Length, actual.Length);
                    Assert.AreEqual(expected, actual);
                }
            }
        }

        /// <summary>
        /// Test preset commands 0|1|2|3, QE, IN
        /// </summary>
        [TestMethod]
        public void TestPresetCommands()
        {
            // 1920x1080 works on my pc
            List<object[]> presets =
                new List<object[]> 
                        {
                          new object[] { 600, 166, 29, 78, true, true },
                          new object[] { 2400, 250, 46, 120, true, true },
                          new object[] { 10000, 333, 62, 161, true, true },
                          new object[] { 32766, 32766, 96, 245, true, true }
                        };
            List<string> arguments = new List<string> { "0", "1", "2", "3" };
            int current = 0;

            foreach (var argVal in arguments)
            {                
                using (StringWriter sw = new StringWriter())
                {
                    Console.SetOut(sw);

                    // mo 0, mo 1, mo 2, mo 3
                    Mode.Main(new[] { argVal });
                    
                    // use defaults in Preset class properties
                    const string Command =
                        "buf lin set {0}" + EndTag
                        + "buf col set {1}" + EndTag
                        + "win lin set {2}" + EndTag
                        + "win col set {3}" + EndTag
                        + "quick edit set {4}" + EndTag
                        + "insert set {5}" + EndTag;

                    // (presets.ToArray())[current++]
                    string expected = String.Format(Command, presets[current++]);
                    string actual = sw.ToString();

                    Assert.AreEqual(expected.Length, actual.Length);
                    Assert.AreEqual(expected, actual);
                }
            }
        }

        /// <summary>
        /// Test set group of uppercase commands Preset0, QE, IN
        /// </summary>
        [TestMethod]
        public void TestGroupUppercasePreset0Commands()
        {
            using (StringWriter sw = new StringWriter())
            {
                Console.SetOut(sw);

                // mo WL=0 WC=0 BL=0 BC=0 QE=1 IN=1
                Mode.Main(new[] { "WL=0", "WC=0", "BL=0", "BC=0", "QE=true", "IN=true" });

                // use defaults in Preset class properties
                const string Command =
                    "buf lin set {0}" + EndTag
                    + "buf col set {1}" + EndTag
                    + "win lin set {2}" + EndTag
                    + "win col set {3}" + EndTag
                    + "quick edit set {4}" + EndTag
                    + "insert set {5}" + EndTag;

                string expected = String.Format(Command, 600, 166, 29, 78, true, true);
                string actual = sw.ToString();

                Assert.AreEqual(expected.Length, actual.Length);
                Assert.AreEqual(expected, actual);
            }
        }

        /// <summary>
        /// Test set group of lowercase commands Preset0, QE, IN
        /// </summary>
        [TestMethod]
        public void TestGroupLowercasePreset0Commands()
        {
            using (StringWriter sw = new StringWriter())
            {
                Console.SetOut(sw);

                // mo wl=0 wc=0 bl=0 bc=0 qe=1 in=1
                Mode.Main(new[] { "wl=0", "wc=0", "bl=0", "bc=0", "qe=true", "in=true" });

                // use defaults in Preset class properties
                const string Command =
                    "buf lin set {0}" + EndTag
                    + "buf col set {1}" + EndTag
                    + "win lin set {2}" + EndTag
                    + "win col set {3}" + EndTag
                    + "quick edit set {4}" + EndTag
                    + "insert set {5}" + EndTag;

                string expected = String.Format(Command, 600, 166, 29, 78, true, true);
                string actual = sw.ToString();

                Assert.AreEqual(expected.Length, actual.Length);
                Assert.AreEqual(expected, actual);
            }
        }

        /// <summary>
        /// Test set W= commands (1) alone, (2) mixed
        /// </summary>
        [TestMethod]
        public void TestWindowCommands()
        {
            // 1920x1080 works on my pc
            List<object[]> presets1 =
                new List<object[]> 
                        {
                          new object[] { 29, 78 },
                          new object[] { 46, 120 },
                          new object[] { 62, 161 },
                          new object[] { 96, 245 },
                          new object[] { 29, 78 },
                          new object[] { 46, 120 },
                          new object[] { 62, 161 },
                          new object[] { 96, 245 }
                        };
            List<object[]> presets2 =
                new List<object[]> 
                        {
                          new object[] { 29, 120 },
                          new object[] { 46, 120 },
                          new object[] { 62, 120 },
                          new object[] { 96, 120 },
                          new object[] { 29, 120 },
                          new object[] { 46, 120 },
                          new object[] { 62, 120 },
                          new object[] { 96, 120 }
                        };
            List<string> arguments = new List<string> { "W=0", "W=1", "W=2", "W=3", "w=0", "w=1", "w=2", "w=3" };
            int current1 = 0, current2 = 0;

            foreach (var argVal in arguments)
            {
                // (1) alone
                using (StringWriter sw = new StringWriter())
                {
                    Console.SetOut(sw);

                    // mo W=0, mo W=1, ...
                    Mode.Main(new[] { argVal });

                    // use defaults in Preset class properties
                    const string Command =
                        "win lin set {0}" + EndTag
                        + "win col set {1}" + EndTag;

                    string expected = String.Format(Command, presets1[current1++]);
                    string actual = sw.ToString();

                    Assert.AreEqual(expected.Length, actual.Length);
                    Assert.AreEqual(expected, actual);
                }

                // (2) mixed, last argument wins
                using (StringWriter sw = new StringWriter())
                {
                    Console.SetOut(sw);

                    // mo W=0 WC=1, mo W=1 WC=1, ...
                    Mode.Main(new[] { argVal, "WC=1" });

                    // use defaults in Preset class properties
                    const string Command =
                        "win lin set {0}" + EndTag
                        + "win col set {1}" + EndTag;

                    string expected = String.Format(Command, presets2[current2++]);
                    string actual = sw.ToString();

                    Assert.AreEqual(expected.Length, actual.Length);
                    Assert.AreEqual(expected, actual);
                }
            }
        }

        /// <summary>
        /// Test set B= commands (1) alone, (2) mixed
        /// </summary>
        [TestMethod]
        public void TestBufferCommands()
        {
            // 1920x1080 works on my pc
            List<object[]> presets1 =
                new List<object[]> 
                        {
                          new object[] { 600, 166 },
                          new object[] { 2400, 250 },
                          new object[] { 10000, 333 },
                          new object[] { 32766, 32766 },
                          new object[] { 600, 166 },
                          new object[] { 2400, 250 },
                          new object[] { 10000, 333 },
                          new object[] { 32766, 32766 }
                        };
            List<object[]> presets2 =
                new List<object[]> 
                        {
                          new object[] { 600, 32766 },
                          new object[] { 2400, 32766 },
                          new object[] { 10000, 32766 },
                          new object[] { 32766, 32766 },
                          new object[] { 600, 32766 },
                          new object[] { 2400, 32766 },
                          new object[] { 10000, 32766 },
                          new object[] { 32766, 32766 }
                        };
            List<string> arguments = new List<string> { "B=0", "B=1", "B=2", "B=3", "b=0", "b=1", "b=2", "b=3" };
            int current1 = 0, current2 = 0;

            foreach (var argVal in arguments)
            {
                // (1) alone
                using (StringWriter sw = new StringWriter())
                {
                    Console.SetOut(sw);

                    // mo 0, mo 1, mo 2, mo 3
                    Mode.Main(new[] { argVal });

                    // use defaults in Preset class properties
                    const string Command =
                        "buf lin set {0}" + EndTag
                        + "buf col set {1}" + EndTag;

                    string expected = String.Format(Command, presets1[current1++]);
                    string actual = sw.ToString();

                    Assert.AreEqual(expected.Length, actual.Length);
                    Assert.AreEqual(expected, actual);
                }

                // (2) mixed, last argument wins
                using (StringWriter sw = new StringWriter())
                {
                    Console.SetOut(sw);

                    // mo 0, mo 1, mo 2, mo 3
                    Mode.Main(new[] { argVal, "BC=3" });

                    // use defaults in Preset class properties
                    const string Command =
                        "buf lin set {0}" + EndTag
                        + "buf col set {1}" + EndTag;

                    string expected = String.Format(Command, presets2[current2++]);
                    string actual = sw.ToString();

                    Assert.AreEqual(expected.Length, actual.Length);
                    Assert.AreEqual(expected, actual);
                }
            }
        }

        /// <summary>
        /// Test set QE= and IN= commands
        /// </summary>
        [TestMethod]
        public void TestTrueFalseCommands()
        {
            List<object[]> argList =
                new List<object[]>
                    {
                        new[] { "QE=false", "IN=false" },
                        new[] { "QE=true", "IN=true" },
                        new[] { "QE=FALSE", "IN=FALSE" },
                        new[] { "QE=TRUE", "IN=TRUE" },
                        new[] { "QE=f", "IN=f" },
                        new[] { "QE=t", "IN=t" }
                    };
            List<object[]> presets =
                new List<object[]>
                    {
                        new object[] { false, false },
                        new object[] { true, true },
                        new object[] { false, false },
                        new object[] { true, true },
                        new object[] { false, false },
                        new object[] { true, true }
                    };
            int current = 0;
            foreach (var arg in argList)
            {
                using (StringWriter sw = new StringWriter())
                {
                    Console.SetOut(sw);

                    // mo 0 error
                    Mode.Main((string[])arg);

                    // use defaults in Preset class properties
                    const string Command =
                        "quick edit set {0}" + EndTag
                        + "insert set {1}" + EndTag;

                    string expected = String.Format(Command, presets[current++]);
                    string actual = sw.ToString();

                    Assert.AreEqual(expected.Length, actual.Length);
                    Assert.AreEqual(expected, actual);
                }
            }         
        }

        /// <summary>
        /// Test set command to bad value not int, e.g. W=2.5, B=.10, WC=1E10, BC=0xffffff, WL=, BL=-1, QE=BAD, IN=bad
        /// </summary>
        [TestMethod]
        public void TestCommandBadArgumentValue()
        {
            List<object[]> argList =
                new List<object[]>
                    {
                        new[] { "W=2.5" },
                        new[] { "B=.10" },
                        new[] { "C=1.0" },
                        new[] { "WC=1E10" },
                        new[] { "BC=0xffffff" },
                        new[] { "WL=" },
                        new[] { "BL=-1" },
                        new[] { "QE=" },
                        new[] { "QE=0" },
                        new[] { "QE=1" },
                        new[] { "QE=2" },
                        new[] { "QE=off" },
                        new[] { "QE=on" },
                        new[] { "QE=error" },
                        new[] { "IN=" },
                        new[] { "IN=0" },
                        new[] { "IN=1" },
                        new[] { "IN=2" },
                        new[] { "IN=off" },
                        new[] { "IN=on" },
                        new[] { "IN=error" }
                    };
            foreach (var arg in argList)
            {
                using (StringWriter sw = new StringWriter())
                {
                    Console.SetOut(sw);

                    // mo 0 error
                    Mode.Main((string[])arg);

                    // use defaults in Preset class properties
                    object[] presets =
                        new object[]
                            {
                                "mo",
                                Preset.WinCol0, Preset.WinCol1, Preset.WinCol2, Preset.WinCol3,
                                Preset.WinLin0, Preset.WinLin1, Preset.WinLin2, Preset.WinLin3,
                                Preset.BufCol0, Preset.BufCol1, Preset.BufCol2, Preset.BufCol3,
                                Preset.BufLin0, Preset.BufLin1, Preset.BufLin2, Preset.BufLin3,
                                true, true
                            };
                    string expected = String.Format(Preset.Usage + EndTag, presets);
                    string actual = sw.ToString();

                    Assert.AreEqual(expected.Length, actual.Length);
                    Assert.AreEqual(expected, actual);
                }
            }
        }

        /// <summary>
        /// Test set command to isInt out of range (W=4, B=10, WL=-1, QE=1000000, IN=32767)
        /// </summary>
        [TestMethod]
        public void TestCommandBadArgumentValueIntOutOfRange()
        {
            List<object[]> argList =
                new List<object[]>
                    {
                        new[] { "W=4" },
                        new[] { "B=10" },
                        new[] { "WC=32767" },
                        new[] { "BC=32767" },
                        new[] { "WL=32767" },
                        new[] { "BL=32767" }
                    };
            List<string> outLine =
                new List<string>
                    {
                       "err: minimum win lin {0}" + EndTag + "err: minimum win col {1}" + EndTag,
                       "err: minimum buf lin {0}" + EndTag + "err: minimum buf col {1}" + EndTag,
                       "err: maximum win col {0}" + EndTag,
                       "err: maximum buf col {0}" + EndTag,
                       "err: maximum win lin {0}" + EndTag,
                       "err: maximum buf lin {0}" + EndTag
                    };
            List<object[]> outValue =
                new List<object[]>
                    {
                        new[] { Preset.WinLin0.ToString(), Preset.WinCol0.ToString() },
                        new[] { Preset.BufLin0.ToString(), Preset.WinCol0.ToString() },
                        new[] { Preset.WinCol3.ToString() },
                        new[] { Preset.BufCol3.ToString() },
                        new[] { Preset.WinLin3.ToString() },
                        new[] { Preset.BufLin3.ToString() }
                    };
            int current = 0;
            foreach (var arg in argList)
            {
                using (StringWriter sw = new StringWriter())
                {
                    Console.SetOut(sw);

                    // mo W=4, mo B=10, etc...
                    Mode.Main((string[])arg);

                    string expected = String.Format(outLine[current], outValue[current]);
                    current++;
                    string actual = sw.ToString();

                    Assert.AreEqual(expected.Length, actual.Length);
                    Assert.AreEqual(expected, actual);
                }
            }
        }
    }
}
