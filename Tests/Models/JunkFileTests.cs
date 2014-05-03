using FileCreator.Models;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Diagnostics;
using System.IO;
using System.Threading;

namespace Tests.Models
{
    [TestClass]
    public class JunkFileTests
    {
        //TODO: Consider including progress changed event in tests

        public TestContext TestContext { get; set; }

        private static DirectoryInfo TempTestFileLocation = new DirectoryInfo(Path.GetTempPath());

        [TestMethod]
        public void JunkFile_1ByteTest()
        {
            var tempFile = new FileInfo(Path.Combine(TempTestFileLocation.FullName, TestHelperMethods.RandomCharString(TestHelperMethods.GetRandomIntFromRange(2, 200))));
            var fillWithJunk = TestHelperMethods.GetRandomBool();

            TestContext.WriteLine("Writing: {0}", tempFile);
            TestContext.WriteLine("Fill with junk: {0}", fillWithJunk);

            var jf = new JunkFile();
            var result = jf.Create(tempFile, 1, 1, fillWithJunk, new CancellationTokenSource());
            result.Wait();

            Assert.IsTrue(tempFile.Length == 1, "Actual file size '{0}' differs from expected size '{1}'.", tempFile.Length, 1);

            File.Delete(tempFile.FullName);
        }

        [TestMethod]
        public void JunkFile_MaxFileSizeTest()
        {
            var maxAllowableTimeDifference = new TimeSpan(0, 0, 0, 1, 0);
            var tempFile = new FileInfo(Path.Combine(TempTestFileLocation.FullName, TestHelperMethods.RandomCharString(TestHelperMethods.GetRandomIntFromRange(2, 200))));
            var fillWithJunk = TestHelperMethods.GetRandomBool();

            TestContext.WriteLine("Writing: {0}", tempFile);
            TestContext.WriteLine("Fill with junk: {0}", fillWithJunk);

            var jf = new JunkFile();
            var cs = new CancellationTokenSource();

            var sw = new Stopwatch();
            sw.Start();

            var result = jf.Create(tempFile, long.MaxValue, 1024, fillWithJunk, cs);
            result.Wait(5000);
            cs.Cancel();
            result.Wait();

            sw.Stop();

            // Confirm time taken is appropriate
            Assert.IsTrue(TestHelperMethods.TimeSpansAreSimilar(result.Result, sw.Elapsed, maxAllowableTimeDifference), "Timespans differ by too much!");

            File.Delete(tempFile.FullName);
        }

        [TestMethod]
        public void JunkFile_RandomTest()
        {
            var maxAllowableTimeDifference = new TimeSpan(0, 0, 0, 1, 0);
            var tempFile = new FileInfo(Path.Combine(TempTestFileLocation.FullName, TestHelperMethods.RandomCharString(TestHelperMethods.GetRandomIntFromRange(2, 200))));
            var size = TestHelperMethods.GetRandomIntFromRange(1, 1024 * 1024 * 1024); // Max 1 GB
            var blockSize = TestHelperMethods.GetRandomIntFromRange(1, size);
            var fillWithJunk = TestHelperMethods.GetRandomBool();

            TestContext.WriteLine("Writing: {0}", tempFile);
            TestContext.WriteLine("Size: {0}", size);
            TestContext.WriteLine("Block size: {0}", size);
            TestContext.WriteLine("Fill with junk: {0}", fillWithJunk);

            var jf = new JunkFile();
            var sw = new Stopwatch();

            sw.Start();
            var result = jf.Create(tempFile, size, blockSize, fillWithJunk, new CancellationTokenSource());
            result.Wait();
            sw.Stop();

            TestContext.WriteLine("Reported time taken: {0}", result.Result);
            TestContext.WriteLine("Measured time taken: {0}", sw.Elapsed);

            // Confirm time taken is resonably accurate
            Assert.IsTrue(TestHelperMethods.TimeSpansAreSimilar(result.Result, sw.Elapsed, maxAllowableTimeDifference), "Timespans differ by too much!");

            // Test file size is correct
            Assert.IsTrue(tempFile.Length == size, "Actual file size '{0}' differs from expected size '{1}'.", tempFile.Length, size);

            File.Delete(tempFile.FullName);
        }
    }
}