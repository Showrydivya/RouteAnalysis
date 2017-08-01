using System;
using System.Text;
using System.Collections.Generic;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Jeff.Vessels.BL.RouteAnalysis;

namespace Jeff.Vessels.BL.RouteAnalysis.Test
{
    /// <summary>
    /// Summary description for PathsIntersection_Test
    /// </summary>
    [TestClass]
    public class PathsIntersectionTest
    {
        public PathsIntersectionTest()
        {
            //
            // TODO: Add constructor logic here
            //
        }

        private TestContext testContextInstance;

        /// <summary>
        ///Gets or sets the test context which provides
        ///information about and functionality for the current test run.
        ///</summary>
        public TestContext TestContext
        {
            get
            {
                return testContextInstance;
            }
            set
            {
                testContextInstance = value;
            }
        }

        #region Additional test attributes
        //
        // You can use the following additional attributes as you write your tests:
        //
        // Use ClassInitialize to run code before running the first test in the class
        // [ClassInitialize()]
        // public static void MyClassInitialize(TestContext testContext) { }
        //
        // Use ClassCleanup to run code after all tests in a class have run
        // [ClassCleanup()]
        // public static void MyClassCleanup() { }
        //
        // Use TestInitialize to run code before running each test 
        // [TestInitialize()]
        // public void MyTestInitialize() { }
        //
        // Use TestCleanup to run code after each test has run
        // [TestCleanup()]
        // public void MyTestCleanup() { }
        //
        #endregion

        [TestMethod]
        public void LineSegmentsIntersect()
        {
            Vector intersection;
            var actual = LineSegment.LineSegementsIntersect(
                new Vector(0, 0),
                new Vector(5, 5),
                new Vector(0, 5),
                new Vector(5, 0),
                out intersection);

            Assert.IsTrue(actual);
            Assert.AreEqual(new Vector(2.5, 2.5), intersection);
        }

        [TestMethod]
        public void LineSegmentsDoNotIntersect()
        {
            Vector intersection;
            var actual = LineSegment.LineSegementsIntersect(
                new Vector(3, 0),
                new Vector(3, 4),
                new Vector(0, 5),
                new Vector(5, 5),
                out intersection);

            Assert.IsFalse(actual);
        }

        [TestMethod]
        public void LineSegmentsAreCollinearAndOverlapping()
        {
            Vector intersection;
            var actual = LineSegment.LineSegementsIntersect(
                new Vector(0, 0),
                new Vector(2, 0),
                new Vector(1, 0),
                new Vector(3, 0),
                out intersection,
                considerOverlapAsIntersect: true);

            Assert.IsTrue(actual);
            Assert.AreEqual(double.NaN, intersection.X);
            Assert.AreEqual(double.NaN, intersection.Y);
        }
    }
}
