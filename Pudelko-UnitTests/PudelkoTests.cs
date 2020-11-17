using System;
using System.Collections.Generic;
using System.Globalization;
using System.Security.Cryptography;
using System.Threading;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using PudelkoLib;

namespace Pudelko_UnitTests
{
    [TestClass]
    public static class InitializeCulture
    {
        [AssemblyInitialize]
        public static void SetEnglishCultureOnAllUnitTest(TestContext context)
        {
            Thread.CurrentThread.CurrentCulture = new CultureInfo("en-US");
            Thread.CurrentThread.CurrentUICulture = new CultureInfo("en-US");
        }
    }

    // ========================================

    [TestClass]
    public class PudelkoConstructorsTests
    {
        private static double defaultSize = 0.1; // w metrach
        private static double accuracy = 0.001; //dok³adnoœæ 3 miejsca po przecinku

        private void AssertPudelko(Pudelko p, double expectedA, double expectedB, double expectedC)
        {
            Assert.AreEqual(expectedA, p.A, delta: accuracy);
            Assert.AreEqual(expectedB, p.B, delta: accuracy);
            Assert.AreEqual(expectedC, p.C, delta: accuracy);
        }

        [DataTestMethod, TestCategory("Constructors")]
        [DataRow(1.0, 2.543, 3.1,
            1.0, 2.543, 3.1)]
        [DataRow(1.0001, 2.54387, 3.1005,
            1.0, 2.543, 3.1)] // dla metrów licz¹ siê 3 miejsca po przecinku
        public void Constructor_3params_DefaultMeters(double a, double b, double c,
            double expectedA, double expectedB, double expectedC)
        {
            Pudelko p = new Pudelko(a, b, c);

            AssertPudelko(p, expectedA, expectedB, expectedC);
        }


        [DataTestMethod, TestCategory("Constructors")]
        [DataRow(1.0, 2.543, 3.1,
            1.0, 2.543, 3.1)]
        [DataRow(1.0001, 2.54387, 3.1005,
            1.0, 2.543, 3.1)] // dla metrów licz¹ siê 3 miejsca po przecinku
        public void Constructor_3params_InMeters(double a, double b, double c,
            double expectedA, double expectedB, double expectedC)
        {
            Pudelko p = new Pudelko(a, b, c, UnitOfMeasure.Meter);

            AssertPudelko(p, expectedA, expectedB, expectedC);
        }


        [DataTestMethod, TestCategory("Constructors")]
        [DataRow(100.0, 25.5, 3.1,
                 1.0, 0.255, 0.031)]
        [DataRow(100.0, 25.58, 3.13,
                 1.0, 0.255, 0.031)] // dla centymertów liczy siê tylko 1 miejsce po przecinku
        public void Constructor_3params_InCentimeters(double a, double b, double c,
                                                      double expectedA, double expectedB, double expectedC)
        {
            Pudelko p = new Pudelko(length: a, width: b, height: c, unitOfMeasure: UnitOfMeasure.Centimeter);

            AssertPudelko(p, expectedA, expectedB, expectedC);
        }

        [DataTestMethod, TestCategory("Constructors")]
        [DataRow(100, 255, 3,
                 0.1, 0.255, 0.003)]
        [DataRow(100.0, 25.58, 3.13,
                 0.1, 0.025, 0.003)] // dla milimetrów nie licz¹ siê miejsca po przecinku
        public void Constructor_3params_InMilimeters(double a, double b, double c,
                                                     double expectedA, double expectedB, double expectedC)
        {
            Pudelko p = new Pudelko(unitOfMeasure: UnitOfMeasure.Millimeter, length: a, width: b, height: c);

            AssertPudelko(p, expectedA, expectedB, expectedC);
        }


        // ----

        [DataTestMethod, TestCategory("Constructors")]
        [DataRow(1.0, 2.5, 1.0, 2.5)]
        [DataRow(1.001, 2.599, 1.001, 2.599)]
        [DataRow(1.0019, 2.5999, 1.001, 2.599)]
        public void Constructor_2params_DefaultMeters(double a, double b, double expectedA, double expectedB)
        {
            Pudelko p = new Pudelko(a, b);

            AssertPudelko(p, expectedA, expectedB, expectedC: 0.1);
        }

        [DataTestMethod, TestCategory("Constructors")]
        [DataRow(1.0, 2.5, 1.0, 2.5)]
        [DataRow(1.001, 2.599, 1.001, 2.599)]
        [DataRow(1.0019, 2.5999, 1.001, 2.599)]
        public void Constructor_2params_InMeters(double a, double b, double expectedA, double expectedB)
        {
            Pudelko p = new Pudelko(length: a, width: b, unitOfMeasure: UnitOfMeasure.Meter);

            AssertPudelko(p, expectedA, expectedB, expectedC: 0.1);
        }

        [DataTestMethod, TestCategory("Constructors")]
        [DataRow(11.0, 2.5, 0.11, 0.025)]
        [DataRow(100.1, 2.599, 1.001, 0.025)]
        [DataRow(2.0019, 0.25999, 0.02, 0.002)]
        public void Constructor_2params_InCentimeters(double a, double b, double expectedA, double expectedB)
        {
            Pudelko p = new Pudelko(unitOfMeasure: UnitOfMeasure.Centimeter, length: a, width: b);

            AssertPudelko(p, expectedA, expectedB, expectedC: 0.1);
        }

        [DataTestMethod, TestCategory("Constructors")]
        [DataRow(11, 2.0, 0.011, 0.002)]
        [DataRow(100.1, 2599, 0.1, 2.599)]
        [DataRow(200.19, 2.5999, 0.2, 0.002)]
        public void Constructor_2params_InMilimeters(double a, double b, double expectedA, double expectedB)
        {
            Pudelko p = new Pudelko(unitOfMeasure: UnitOfMeasure.Millimeter, length: a, width: b);
            //a: 11, expectedA: 0.011, b: 2599 => 2.599
            AssertPudelko(p, expectedA, expectedB, expectedC: 0.1);
        }

        // -------

        [DataTestMethod, TestCategory("Constructors")]
        [DataRow(2.5)]
        public void Constructor_1param_DefaultMeters(double a)
        {
            Pudelko p = new Pudelko(a);

            Assert.AreEqual(a, p.A);
            Assert.AreEqual(0.1, p.B);
            Assert.AreEqual(0.1, p.C);
        }

        [DataTestMethod, TestCategory("Constructors")]
        [DataRow(2.5)]
        public void Constructor_1param_InMeters(double a)
        {
            Pudelko p = new Pudelko(a);

            Assert.AreEqual(a, p.A);
            Assert.AreEqual(0.1, p.B);
            Assert.AreEqual(0.1, p.C);
        }

        [DataTestMethod, TestCategory("Constructors")]
        [DataRow(11.0, 0.11)]
        [DataRow(100.1, 1.001)]
        [DataRow(2.0019, 0.02)]
        public void Constructor_1param_InCentimeters(double a, double expectedA)
        {
            Pudelko p = new Pudelko(unitOfMeasure: UnitOfMeasure.Centimeter, length: a);

            AssertPudelko(p, expectedA, expectedB: 0.1, expectedC: 0.1);
        }

        [DataTestMethod, TestCategory("Constructors")]
        [DataRow(11, 0.011)]
        [DataRow(100.1, 0.1)]
        [DataRow(200.19, 0.2)]
        public void Constructor_1param_InMilimeters(double a, double expectedA)
        {
            Pudelko p = new Pudelko(unitOfMeasure: UnitOfMeasure.Millimeter, length: a);

            AssertPudelko(p, expectedA, expectedB: 0.1, expectedC: 0.1);
        }


        // ---

        public static IEnumerable<object[]> DataSet1Meters_ArgumentOutOfRangeEx => new List<object[]>
        {
            new object[] {-1.0, 2.5, 3.1},
            new object[] {1.0, -2.5, 3.1},
            new object[] {1.0, 2.5, -3.1},
            new object[] {-1.0, -2.5, 3.1},
            new object[] {-1.0, 2.5, -3.1},
            new object[] {1.0, -2.5, -3.1},
            new object[] {-1.0, -2.5, -3.1},
            new object[] {0, 2.5, 3.1},
            new object[] {1.0, 0, 3.1},
            new object[] {1.0, 2.5, 0},
            new object[] {1.0, 0, 0},
            new object[] {0, 2.5, 0},
            new object[] {0, 0, 3.1},
            new object[] {0, 0, 0},
            new object[] {10.1, 2.5, 3.1},
            new object[] {10, 10.1, 3.1},
            new object[] {10, 10, 10.1},
            new object[] {10.1, 10.1, 3.1},
            new object[] {10.1, 10, 10.1},
            new object[] {10, 10.1, 10.1},
            new object[] {10.1, 10.1, 10.1}
        };

        [DataTestMethod, TestCategory("Constructors")]
        [DynamicData(nameof(DataSet1Meters_ArgumentOutOfRangeEx))]
        [ExpectedException(typeof(ArgumentOutOfRangeException))]
        public void Constructor_3params_DefaultMeters_ArgumentOutOfRangeException(double a, double b, double c)
        {
            Pudelko p = new Pudelko(a, b, c);
        }

        [DataTestMethod, TestCategory("Constructors")]
        [DynamicData(nameof(DataSet1Meters_ArgumentOutOfRangeEx))]
        [ExpectedException(typeof(ArgumentOutOfRangeException))]
        public void Constructor_3params_InMeters_ArgumentOutOfRangeException(double a, double b, double c)
        {
            Pudelko p = new Pudelko(a, b, c,  UnitOfMeasure.Meter);
        }

        [DataTestMethod, TestCategory("Constructors")]
        [DataRow(-1, 1, 1)]
        [DataRow(1, -1, 1)]
        [DataRow(1, 1, -1)]
        [DataRow(-1, -1, 1)]
        [DataRow(-1, 1, -1)]
        [DataRow(1, -1, -1)]
        [DataRow(-1, -1, -1)]
        [DataRow(0, 1, 1)]
        [DataRow(1, 0, 1)]
        [DataRow(1, 1, 0)]
        [DataRow(0, 0, 1)]
        [DataRow(0, 1, 0)]
        [DataRow(1, 0, 0)]
        [DataRow(0, 0, 0)]
        [DataRow(0.01, 0.1, 1)]
        [DataRow(0.1, 0.01, 1)]
        [DataRow(0.1, 0.1, 0.01)]
        [DataRow(1001, 1, 1)]
        [DataRow(1, 1001, 1)]
        [DataRow(1, 1, 1001)]
        [DataRow(1001, 1, 1001)]
        [DataRow(1, 1001, 1001)]
        [DataRow(1001, 1001, 1)]
        [DataRow(1001, 1001, 1001)]
        [ExpectedException(typeof(ArgumentOutOfRangeException))]
        public void Constructor_3params_InCentimeters_ArgumentOutOfRangeException(double a, double b, double c)
        {
            Pudelko p = new Pudelko(a, b, c, UnitOfMeasure.Centimeter);
        }


        [DataTestMethod, TestCategory("Constructors")]
        [DataRow(-1, 1, 1)]
        [DataRow(1, -1, 1)]
        [DataRow(1, 1, -1)]
        [DataRow(-1, -1, 1)]
        [DataRow(-1, 1, -1)]
        [DataRow(1, -1, -1)]
        [DataRow(-1, -1, -1)]
        [DataRow(0, 1, 1)]
        [DataRow(1, 0, 1)]
        [DataRow(1, 1, 0)]
        [DataRow(0, 0, 1)]
        [DataRow(0, 1, 0)]
        [DataRow(1, 0, 0)]
        [DataRow(0, 0, 0)]
        [DataRow(0.1, 1, 1)]
        [DataRow(1, 0.1, 1)]
        [DataRow(1, 1, 0.1)]
        [DataRow(10001, 1, 1)]
        [DataRow(1, 10001, 1)]
        [DataRow(1, 1, 10001)]
        [DataRow(10001, 10001, 1)]
        [DataRow(10001, 1, 10001)]
        [DataRow(1, 10001, 10001)]
        [DataRow(10001, 10001, 10001)]
        [ExpectedException(typeof(ArgumentOutOfRangeException))]
        public void Constructor_3params_InMiliimeters_ArgumentOutOfRangeException(double a, double b, double c)
        {
            Pudelko p = new Pudelko(a, b, c, UnitOfMeasure.Millimeter);
        }


        public static IEnumerable<object[]> DataSet2Meters_ArgumentOutOfRangeEx => new List<object[]>
        {
            new object[] {-1.0, 2.5},
            new object[] {1.0, -2.5},
            new object[] {-1.0, -2.5},
            new object[] {0, 2.5},
            new object[] {1.0, 0},
            new object[] {0, 0},
            new object[] {10.1, 10},
            new object[] {10, 10.1},
            new object[] {10.1, 10.1}
        };

        [DataTestMethod, TestCategory("Constructors")]
        [DynamicData(nameof(DataSet2Meters_ArgumentOutOfRangeEx))]
        [ExpectedException(typeof(ArgumentOutOfRangeException))]
        public void Constructor_2params_DefaultMeters_ArgumentOutOfRangeException(double a, double b)
        {
            Pudelko p = new Pudelko(a, b);
        }

        [DataTestMethod, TestCategory("Constructors")]
        [DynamicData(nameof(DataSet2Meters_ArgumentOutOfRangeEx))]
        [ExpectedException(typeof(ArgumentOutOfRangeException))]
        public void Constructor_2params_InMeters_ArgumentOutOfRangeException(double a, double b)
        {
            Pudelko p = new Pudelko(a, b, UnitOfMeasure.Meter);
        }

        [DataTestMethod, TestCategory("Constructors")]
        [DataRow(-1, 1)]
        [DataRow(1, -1)]
        [DataRow(-1, -1)]
        [DataRow(0, 1)]
        [DataRow(1, 0)]
        [DataRow(0, 0)]
        [DataRow(0.01, 1)]
        [DataRow(1, 0.01)]
        [DataRow(0.01, 0.01)]
        [DataRow(1001, 1)]
        [DataRow(1, 1001)]
        [DataRow(1001, 1001)]
        [ExpectedException(typeof(ArgumentOutOfRangeException))]
        public void Constructor_2params_InCentimeters_ArgumentOutOfRangeException(double a, double b)
        {
            Pudelko p = new Pudelko(a, b, UnitOfMeasure.Centimeter);
        }

        [DataTestMethod, TestCategory("Constructors")]
        [DataRow(-1, 1)]
        [DataRow(1, -1)]
        [DataRow(-1, -1)]
        [DataRow(0, 1)]
        [DataRow(1, 0)]
        [DataRow(0, 0)]
        [DataRow(0.1, 1)]
        [DataRow(1, 0.1)]
        [DataRow(0.1, 0.1)]
        [DataRow(10001, 1)]
        [DataRow(1, 10001)]
        [DataRow(10001, 10001)]
        [ExpectedException(typeof(ArgumentOutOfRangeException))]
        public void Constructor_2params_InMilimeters_ArgumentOutOfRangeException(double a, double b)
        {
            Pudelko p = new Pudelko(a, b, UnitOfMeasure.Millimeter);
        }




        [DataTestMethod, TestCategory("Constructors")]
        [DataRow(-1.0)]
        [DataRow(0)]
        [DataRow(10.1)]
        [ExpectedException(typeof(ArgumentOutOfRangeException))]
        public void Constructor_1param_DefaultMeters_ArgumentOutOfRangeException(double a)
        {
            Pudelko p = new Pudelko(a);
        }

        [DataTestMethod, TestCategory("Constructors")]
        [DataRow(-1.0)]
        [DataRow(0)]
        [DataRow(10.1)]
        [ExpectedException(typeof(ArgumentOutOfRangeException))]
        public void Constructor_1param_InMeters_ArgumentOutOfRangeException(double a)
        {
            Pudelko p = new Pudelko(a, unitOfMeasure: UnitOfMeasure.Meter);
        }

        [DataTestMethod, TestCategory("Constructors")]
        [DataRow(-1.0)]
        [DataRow(0)]
        [DataRow(0.01)]
        [DataRow(1001)]
        [ExpectedException(typeof(ArgumentOutOfRangeException))]
        public void Constructor_1param_InCentimeters_ArgumentOutOfRangeException(double a)
        {
            Pudelko p = new Pudelko(a, unitOfMeasure: UnitOfMeasure.Centimeter);
        }

        [DataTestMethod, TestCategory("Constructors")]
        [DataRow(-1)]
        [DataRow(0)]
        [DataRow(0.1)]
        [DataRow(10001)]
        [ExpectedException(typeof(ArgumentOutOfRangeException))]
        public void Constructor_1param_InMilimeters_ArgumentOutOfRangeException(double a)
        {
            Pudelko p = new Pudelko(a,UnitOfMeasure.Millimeter);
        }

        #region ToString tests ===================================

        [TestMethod, TestCategory("String representation")]
        public void ToString_Default_Culture_EN()
        {
            var p = new Pudelko(2.5, 9.321);
            string expectedStringEN = "2.500 m × 9.321 m × 0.100 m";

            Assert.AreEqual(expectedStringEN, p.ToString());
        }

        [DataTestMethod, TestCategory("String representation")]
        [DataRow("m", 2.5, 9.321, 0.1, "2.500 m × 9.321 m × 0.100 m")]
        [DataRow("m", 2.5, 9.321, 0.1, "2.500 m × 9.321 m × 0.100 m")]
        [DataRow("cm", 2.5, 9.321, 0.1, "250.0 cm × 932.1 cm × 10.0 cm")]
        [DataRow("mm", 2.5, 9.321, 0.1, "2500 mm × 9321 mm × 100 mm")]
        public void ToString_Formattable_Culture_EN(string format, double a, double b, double c, string expectedStringRepresentation)
        {
            var p = new Pudelko(a, b, c, unitOfMeasure: UnitOfMeasure.Meter);
            Assert.AreEqual(expectedStringRepresentation, p.ToString(format));
        }

        [TestMethod, TestCategory("String representation")]
        [ExpectedException(typeof(FormatException))]
        public void ToString_Formattable_WrongFormat_FormatException()
        {
            var p = new Pudelko(1);
            var stringformatedrepreentation = p.ToString("wrong code");
        }

        #endregion


        #region Pole, Objêtoœæ ===================================
        [DataTestMethod, TestCategory("Calculated properties")]
        [DataRow(1.0, 1.0, 1.0, 6.0)]
        [DataRow(2.5, 1.2, 2.3, 23.02)]
        [DataRow(1.3, 9.92, 3.32, 100.2928)]
        public void Area_InMeters_ReturnsCalculatedAreaInMeters(double a, double b, double c, double expectedArea)
        {
            var p = new Pudelko(a, b, c);

            Assert.AreEqual(p.Area, expectedArea, delta: accuracy);
        }

        [DataTestMethod, TestCategory("Calculated properties")]
        [DataRow(100.0, 200.0, 300.0, 22.0)]
        [DataRow(250.0, 120.0, 230.0, 23.02)]
        [DataRow(130.0, 992.0, 332.0, 100.2928)]
        public void Area_InCentimeters_ReturnsCalculatedAreaInMeters(double a, double b, double c,
            double expectedArea)
        {
            var p = new Pudelko(a, b, c, UnitOfMeasure.Centimeter);

            Assert.AreEqual(p.Area, expectedArea, delta: accuracy);
        }

        [DataTestMethod, TestCategory("Calculated properties")]
        [DataRow(1000.0, 2000.0, 3000.0, 22.0)]
        [DataRow(2500.0, 1200.0, 2300.0, 23.02)]
        [DataRow(1300.0, 9920.0, 3320.0, 100.2928)]
        public void Area_InMillimeters_ReturnsCalculatedAreaInMeters(double a, double b, double c,
            double expectedArea)
        {
            var p = new Pudelko(a, b, c, UnitOfMeasure.Millimeter);

            Assert.AreEqual(p.Area, expectedArea, delta: accuracy);
        }

        [DataTestMethod, TestCategory("Calculated properties")]
        [DataRow(2.0, 2.0, 2.0, 8.0)]
        [DataRow(1.5, 2.9, 3.1, 13.485)]
        [DataRow(9.9, 1.23, 0.1, 1.2177)]
        public void Volume_InMeters_ReturnsCalculatedVolumeInMeters(double a, double b, double c, double expectedVolume)
        {
            var p = new Pudelko(a, b, c);

            Assert.AreEqual(p.Volume, expectedVolume, delta: accuracy);
        }

        [DataTestMethod, TestCategory("Calculated properties")]
        [DataRow(200, 200, 200, 8.0)]
        [DataRow(150, 290, 310, 13.485)]
        [DataRow(990, 123, 10, 1.2177)]
        public void Volume_InCentimeters_ReturnsCalculatedVolumeInMeters(double a, double b, double c,
            double expectedVolume)
        {
            var p = new Pudelko(a, b, c, UnitOfMeasure.Centimeter);

            Assert.AreEqual(p.Volume, expectedVolume, delta: accuracy);
        }

        [DataTestMethod, TestCategory("Calculated properties")]
        [DataRow(2000, 2000, 2000, 8.0)]
        [DataRow(1500, 2900, 3100, 13.485)]
        [DataRow(9900, 1230, 100, 1.2177)]
        public void Volume_InMillimeters_ReturnsCalculatedVolumeInMeters(double a, double b, double c,
            double expectedVolume)
        {
            var p = new Pudelko(a, b, c, UnitOfMeasure.Millimeter);

            Assert.AreEqual(p.Volume, expectedVolume, delta: accuracy);
        }
        #endregion



        #region Equals ===========================================

        [TestMethod, TestCategory("Equals")]
        public void Equals_OtherIsNull_ReturnsFalse()
        {
            var p1 = new Pudelko();
            Pudelko p2 = null;

            Assert.AreEqual(p1.Equals(p2), false);
        }

        [TestMethod, TestCategory("Equals")]
        public void Equals_OtherIsReferenceToSameObject_ReturnsTrue()
        {
            var p1 = new Pudelko();
            var p2 = p1;

            Assert.AreEqual(p1.Equals(p2), true);
        }

        [DataTestMethod, TestCategory("Equals")]
        [DataRow(1.0, 1.0, 1.0)]
        [DataRow(3.1, 2.0, 5.6)]
        [DataRow(3.0, 2.1, 5.56)]
        [DataRow(2.98, 2.1, 5.5)]
        [DataRow(3.0, 2.09, 5.5)]
        [DataRow(3.1, 3.1, 3.1)]
        public void Equals_OtherHasDifferentEdges_ReturnsFalse(double otherA, double otherB, double otherC)
        {
            var p1 = new Pudelko(3.0, 2.1, 5.5);
            var p2 = new Pudelko(otherA, otherB, otherC);
            

            Assert.AreEqual(p1.Equals(p2), false);
        }

        [DataTestMethod, TestCategory("Equals")]
        [DataRow(1.0, 2.0, 3.0)]
        [DataRow(2.223, 1.999, 3.92)]
        [DataRow(0.1, 0.12321, 0.92103)]
        public void Equals_OtherHasSameEdges_ReturnsTrue(double a, double b, double c)
        {
            var p1 = new Pudelko(a, b, c);
            var p2 = new Pudelko(a, b, c);

            Assert.AreEqual(p1.Equals(p2), true);
        }

        #endregion

        #region Operators overloading ===========================

        [TestMethod, TestCategory("Operators")]
        public void EqualsOperator_OtherIsNull_ReturnsFalse()
        {
            var p1 = new Pudelko();
            Pudelko p2 = null;

            Assert.AreEqual(p1 == p2, false);
        }

        [TestMethod, TestCategory("Operators")]
        public void EqualsOperator_BothAreNulls_ReturnsTrue()
        {
            Pudelko p1 = null;
            Pudelko p2 = null;

            Assert.AreEqual(p1 == p2, true);
        }

        [TestMethod, TestCategory("Operators")]
        public void EqualsOperator_FirstIsNull_ReturnsFalse()
        {
            Pudelko p1 = null;
            var p2 = new Pudelko();

            Assert.AreEqual(p1 == p2, false);
        }

        [TestMethod, TestCategory("Operators")]
        public void EqualsOperator_OtherIsNotObjectOfTypePudelko_ReturnsFalse()
        {
            var p1 = new Pudelko();
            var p2 = new object();

            Assert.AreEqual(p1 == p2, false);
        }

        [TestMethod, TestCategory("Operators")]
        public void EqualsOperator_OtherIsReferenceToSameObject_ReturnsTrue()
        {
            var p1 = new Pudelko();
            var p2 = p1;

            Assert.AreEqual(p1 == p2, true);
        }

        [TestMethod, TestCategory("Operators")]
        public void EqualsOperator_EdgesAreTheSame_ReturnsTrue()
        {
            var p1 = new Pudelko(1.5, 2.2, 3.2);
            var p2 = new Pudelko(1.5, 2.2, 3.2);

            Assert.AreEqual(p1 == p2, true);
        }

        [TestMethod, TestCategory("Operators")]
        public void EqualsOperator_EdgesAreNotTheSame_ReturnsFalse()
        {
            var p1 = new Pudelko(1.5, 2.2, 3.2);
            var p2 = new Pudelko(2, 2, 2);

            Assert.AreEqual(p1 == p2, false);
        }

        [TestMethod, TestCategory("Operators")]
        public void NotEqualsOperator_EdgesAreTheSame_ReturnsFalse()
        {
            var p1 = new Pudelko(1.5, 2.2, 3.2);
            var p2 = new Pudelko(1.5, 2.2, 3.2);

            Assert.AreEqual(p1 != p2, false);
        }

        [TestMethod, TestCategory("Operators")]
        public void NotEqualsOperator_EdgesAreNotTheSame_ReturnsTrue()
        {
            var p1 = new Pudelko(1.5, 2.2, 3.2);
            var p2 = new Pudelko(2, 2, 2);

            Assert.AreEqual(p1 != p2, true);
        }

        [DataTestMethod, TestCategory("Operators")]
        [DataRow(2,2, 2, 4, 2, 2)]
        [DataRow(1, 5, 5, 3, 5, 5)]
        [DataRow(0.1, 1.99, 2.01, 2.1, 2, 2.01)]
        [DataRow(2.01, 2.01, 2.01, 4.01, 2.01, 2.01)]
        [DataRow(0.1, 0.1, 0.1, 2.1, 2, 2)]
        public void PlusOperator_InMeters_ReturnsSmallestBoxThatCanContainBoth(double otherA, double otherB, double otherC,
            double expectedA, double expectedB, double expectedC)
        {
            var p1 = new Pudelko(2, 2, 2);
            var p2 = new Pudelko(otherA, otherB, otherC);

            var boxForBoth = p1 + p2;

            AssertPudelko(boxForBoth, expectedA, expectedB, expectedC);
        }

        [DataTestMethod, TestCategory("Operators")]
        [DataRow(200, 200, 200, 4, 2, 2)]
        [DataRow(100, 500, 500, 3, 5, 5)]
        [DataRow(10, 199, 201, 2.1, 2, 2.01)]
        [DataRow(201, 201, 201, 4.01, 2.01, 2.01)]
        [DataRow(10, 10, 10, 2.1, 2, 2)]
        public void PlusOperator_InCentimeters_ReturnsSmallestBoxThatCanContainBoth(double otherA, double otherB, double otherC,
            double expectedA, double expectedB, double expectedC)
        {
            var p1 = new Pudelko(200, 200, 200, UnitOfMeasure.Centimeter);
            var p2 = new Pudelko(otherA, otherB, otherC, UnitOfMeasure.Centimeter);

            var boxForBoth = p1 + p2;

            AssertPudelko(boxForBoth, expectedA, expectedB, expectedC);
        }

        [DataTestMethod, TestCategory("Operators")]
        [DataRow(2000, 2000, 2000, 4, 2, 2)]
        [DataRow(1000, 5000, 5000, 3, 5, 5)]
        [DataRow(100, 1990, 2010, 2.1, 2, 2.01)]
        [DataRow(2010, 2010, 2010, 4.01, 2.01, 2.01)]
        [DataRow(100, 100, 100, 2.1, 2, 2)]
        public void PlusOperator_InMillimeters_ReturnsSmallestBoxThatCanContainBoth(double otherA, double otherB, double otherC,
            double expectedA, double expectedB, double expectedC)
        {
            var p1 = new Pudelko(2000, 2000, 2000, UnitOfMeasure.Millimeter);
            var p2 = new Pudelko(otherA, otherB, otherC, UnitOfMeasure.Millimeter);

            var boxForBoth = p1 + p2;

            AssertPudelko(boxForBoth, expectedA, expectedB, expectedC);
        }

        #endregion

        #region Conversions =====================================
        [TestMethod]
        public void ExplicitConversion_ToDoubleArray_AsMeters()
        {
            var p = new Pudelko(1, 2.1, 3.231);
            double[] tab = (double[])p;
            Assert.AreEqual(3, tab.Length);
            Assert.AreEqual(p.A, tab[0]);
            Assert.AreEqual(p.B, tab[1]);
            Assert.AreEqual(p.C, tab[2]);
        }

        [TestMethod]
        public void ImplicitConversion_FromAalueTuple_As_Pudelko_InMilimeters()
        {
            var (a, b, c) = (2500, 9321, 100); // in milimeters, ValueTuple
            Pudelko p =(Pudelko)(a, b, c);
            Assert.AreEqual((int)(p.A * 1000), a);
            Assert.AreEqual((int)(p.B * 1000), b);
            Assert.AreEqual((int)(p.C * 1000), c);
        }

        #endregion

        #region Indexer, enumeration ============================
        [TestMethod]
        public void Indexer_ReadFrom()
        {
            var p = new Pudelko(1, 2.1, 3.231);
            Assert.AreEqual(p.A, p[0]);
            Assert.AreEqual(p.B, p[1]);
            Assert.AreEqual(p.C, p[2]);
        }

        [TestMethod]
        public void ForEach_Test()
        {
            var p = new Pudelko(1, 2.1, 3.231);
            var tab = new[] { p.A, p.B, p.C };
            int i = 0;
            foreach (double x in p)
            {
                Assert.AreEqual(x, tab[i]);
                i++;
            }
        }

        #endregion

        #region Parsing =========================================

        [DataTestMethod, TestCategory("Parsing")]
        [DataRow("2.0 m × 1.0 m3.0 m")]
        [DataRow("2 3 2")]
        [DataRow("2 m x 3 m x 2 m")]
        [DataRow("2.0 m × 1.0 mm × 3.0 cm")]
        [ExpectedException(typeof(FormatException))]
        public void Parse_InvalidInput_ThrowsFormatException(string input)
        {
            var p = Pudelko.Parse(input);
        }

        [TestMethod, TestCategory("Parsing")]
        public void Parse_InMeters_ReturnsNewPudelkoInMeters()
        {
            var p = Pudelko.Parse("2.0 m × 1.0 m × 3.0 m");

            AssertPudelko(p, 2, 1, 3);
        }

        [TestMethod, TestCategory("Parsing")]
        public void Parse_InCentimeters_ReturnsNewPudelkoInCentimeters()
        {
            var p = Pudelko.Parse("200 cm × 100 cm × 300 cm");

            AssertPudelko(p, 2, 1, 3);
        }

        [TestMethod, TestCategory("Parsing")]
        public void Parse_InMillimeters_ReturnsNewPudelkoInMillimeters()
        {
            var p = Pudelko.Parse("2000 mm × 1000 mm × 3000 mm");

            AssertPudelko(p, 2, 1, 3);
        }

        #endregion

    }
}
