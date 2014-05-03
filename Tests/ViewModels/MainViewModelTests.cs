using FileCreator.Properties;
using FileCreator.ViewModels;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.ComponentModel.DataAnnotations;

namespace Tests.ViewModels
{
    [TestClass]
    public class MainViewModelTests
    {

        [TestMethod]
        public void MainViewModelTests_FileProperty_ValidTest()
        {
            var vm = new MainViewModel() { FilePath = @"C:\validfilename.exe" }; // TODO: Make more robust - find valid drive letter and path
            Assert.IsTrue(Validator.TryValidateProperty(vm.FilePath, new ValidationContext(vm, null, null) { MemberName = "FilePath" }, null));
        }

        [TestMethod]
        public void MainViewModelTests_FileProperty_NonExistentPathTest()
        {
            var vm = new MainViewModel() { FilePath = @"C:\path\that\is\formatted\correctly\but\hopefully\doesnt\exist\validfilename.exe" };
            Assert.IsFalse(Validator.TryValidateProperty(vm.FilePath, new ValidationContext(vm, null, null) { MemberName = "FilePath" }, null));
        }

        [TestMethod]
        public void MainViewModelTests_FileProperty_ValidRelativePathTest()
        {
            var vm = new MainViewModel() { FilePath = "validfilename.exe" };
            Assert.IsTrue(Validator.TryValidateProperty(vm.FilePath, new ValidationContext(vm, null, null) { MemberName = "FilePath" }, null));
        }

        [TestMethod]
        public void MainViewModelTests_FileProperty_PathTooLongTest()
        {
            var vm = new MainViewModel() { FilePath = new string('x', 1001) };
            Assert.IsFalse(Validator.TryValidateProperty(vm.FilePath, new ValidationContext(vm, null, null) { MemberName = "FilePath" }, null));
        }

        [TestMethod]
        public void MainViewModelTests_FileProperty_PathMaxLengthTest()
        {
            var vm = new MainViewModel() { FilePath = new string('x', 1000) };
            Assert.IsTrue(Validator.TryValidateProperty(vm.FilePath, new ValidationContext(vm, null, null) { MemberName = "FilePath" }, null));
        }

        [TestMethod]
        public void MainViewModelTests_FileProperty_InvalidFileNameTest()
        {
            var vm = new MainViewModel() { FilePath = @"C:\dirpath\that\is\formatted\correctly\invalid/file>name.exe" };
            Assert.IsFalse(Validator.TryValidateProperty(vm.FilePath, new ValidationContext(vm, null, null) { MemberName = "FilePath" }, null));
        }

        [TestMethod]
        public void MainViewModelTests_FileProperty_InvalidPathTest()
        {
            var vm = new MainViewModel() { FilePath = @"C:\invali|d\dirpath\validfilename.exe" };
            Assert.IsFalse(Validator.TryValidateProperty(vm.FilePath, new ValidationContext(vm, null, null) { MemberName = "FilePath" }, null));
        }

        [TestMethod]
        public void MainViewModelTests_FileProperty_NullTest()
        {
            var vm = new MainViewModel() { FilePath = null };
            Assert.IsFalse(Validator.TryValidateProperty(vm.FilePath, new ValidationContext(vm, null, null) { MemberName = "FilePath" }, null));
        }

        [TestMethod]
        public void MainViewModelTests_FileProperty_EmptyTest()
        {
            var vm = new MainViewModel() { FilePath = string.Empty };
            Assert.IsFalse(Validator.TryValidateProperty(vm.FilePath, new ValidationContext(vm, null, null) { MemberName = "FilePath" }, null));
        }

        [TestMethod]
        public void MainViewModelTests_FileProperty_WhitespaceTest()
        {
            var vm = new MainViewModel() { FilePath = " " };
            Assert.IsFalse(Validator.TryValidateProperty(vm.FilePath, new ValidationContext(vm, null, null) { MemberName = "FilePath" }, null));
        }

        [TestMethod]
        public void MainViewModelTests_FileProperty_NotSuppliedTest()
        {
            Settings.Default.Reset();
            Settings.Default.Reload();
            var vm = new MainViewModel(); // File should be populated with default value
            Assert.IsTrue(Validator.TryValidateProperty(vm.FilePath, new ValidationContext(vm, null, null) { MemberName = "FilePath" }, null));
        }

        [TestMethod]
        public void MainViewModelTests_FileSizeUnitsProperty_ValidTest()
        {
            var vm = new MainViewModel() { FileSizeUnits = FileCreator.Size.B};
            Assert.IsTrue(Validator.TryValidateProperty(vm.FileSizeUnits, new ValidationContext(vm, null, null) { MemberName = "FileSizeUnits" }, null));
        }

        [TestMethod]
        public void MainViewModelTests_FileSizeUnitsProperty_NotSuppliedTest()
        {
            var vm = new MainViewModel();
            Assert.IsTrue(Validator.TryValidateProperty(vm.FileSizeUnits, new ValidationContext(vm, null, null) { MemberName = "FileSizeUnits" }, null));
        }

        [TestMethod]
        public void MainViewModelTests_FileSizeProperty_NotSuppliedTest()
        {
            var vm = new MainViewModel();
            Assert.IsTrue(Validator.TryValidateProperty(vm.FileSize, new ValidationContext(vm, null, null) { MemberName = "FileSize" }, null));
        }

        [TestMethod]
        public void MainViewModelTests_FileSizeProperty_NullTest()
        {
            var vm = new MainViewModel() { FileSize = null };
            Assert.IsFalse(Validator.TryValidateProperty(vm.FileSize, new ValidationContext(vm, null, null) { MemberName = "FileSize" }, null));
        }

        [TestMethod]
        public void MainViewModelTests_FileSizeProperty_EmptyTest()
        {
            var vm = new MainViewModel() { FileSize = string.Empty };
            Assert.IsFalse(Validator.TryValidateProperty(vm.FileSize, new ValidationContext(vm, null, null) { MemberName = "FileSize" }, null));
        }

        [TestMethod]
        public void MainViewModelTests_FileSizeProperty_WhiteSpaceTest()
        {
            var vm = new MainViewModel() { FileSize = " " };
            Assert.IsFalse(Validator.TryValidateProperty(vm.FileSize, new ValidationContext(vm, null, null) { MemberName = "FileSize" }, null));
        }
    
        // TODO:
        // randomly pick a size unit
        // randomly pick a value between long.min and long.max
        // determine locally if the value is valid or not
        // run test and make sure it appropriately fails or passes
        // test similarly with ChunkSizeUnits and ChunkSize

        [TestMethod]
        public void MainViewModelTests_ChunkSizeProperty_NotSuppliedTest()
        {
            Settings.Default.Reset();
            Settings.Default.Reload();
            var vm = new MainViewModel();
            Assert.IsTrue(Validator.TryValidateProperty(vm.ChunkSize, new ValidationContext(vm, null, null) { MemberName = "ChunkSize" }, null));
        }

        [TestMethod]
        public void MainViewModelTests_ChunkSizeProperty_NullTest()
        {
            var vm = new MainViewModel() { ChunkSize = null };
            Assert.IsFalse(Validator.TryValidateProperty(vm.ChunkSize, new ValidationContext(vm, null, null) { MemberName = "ChunkSize" }, null));
        }

        [TestMethod]
        public void MainViewModelTests_ChunkSizeProperty_EmptyTest()
        {
            var vm = new MainViewModel() { ChunkSize = string.Empty };
            Assert.IsFalse(Validator.TryValidateProperty(vm.ChunkSize, new ValidationContext(vm, null, null) { MemberName = "ChunkSize" }, null));
        }

        [TestMethod]
        public void MainViewModelTests_ChunkSizeProperty_WhiteSpaceTest()
        {
            var vm = new MainViewModel() { ChunkSize = " " };
            Assert.IsFalse(Validator.TryValidateProperty(vm.ChunkSize, new ValidationContext(vm, null, null) { MemberName = "ChunkSize" }, null));
        }

        [TestMethod]
        public void MainViewModelTests_ChunkDataProperty_NotSuppliedTest()
        {
            Settings.Default.Reset();
            Settings.Default.Reload();
            var vm = new MainViewModel();
            Assert.IsTrue(Validator.TryValidateProperty(vm.ChunkDataType, new ValidationContext(vm, null, null) { MemberName = "ChunkDataType" }, null));
        }

        [TestMethod]
        public void MainViewModelTests_ChunkDataProperty_ValueSpecifiedTest()
        {
            var vm = new MainViewModel() { ChunkDataType = FileCreator.ChunkData.Randoms };
            Assert.IsTrue(Validator.TryValidateProperty(vm.ChunkDataType, new ValidationContext(vm, null, null) { MemberName = "ChunkDataType" }, null));
        }
    }
}
