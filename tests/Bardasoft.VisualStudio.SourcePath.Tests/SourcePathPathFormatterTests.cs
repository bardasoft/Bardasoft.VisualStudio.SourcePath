using System.IO;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Bardasoft.VisualStudio.SourcePath.Tests;

[TestClass]
public sealed class SourcePathPathFormatterTests
{
    [TestMethod]
    public void Format_WhenDisplayModeIsFullPath_ReturnsOriginalPath()
    {
        string filePath = Path.Combine("E:\\", "Work", "App", "Program.cs");

        string result = SourcePathPathFormatter.Format(
            filePath,
            SourcePathDisplayMode.FullPath,
            Path.Combine("E:\\", "Work"));

        Assert.AreEqual(filePath, result);
    }

    [TestMethod]
    public void Format_WhenDisplayModeIsFileName_ReturnsOnlyFileName()
    {
        string filePath = Path.Combine("E:\\", "Work", "App", "Services", "UserService.cs");

        string result = SourcePathPathFormatter.Format(
            filePath,
            SourcePathDisplayMode.FileName,
            Path.Combine("E:\\", "Work"));

        Assert.AreEqual("UserService.cs", result);
    }

    [TestMethod]
    public void Format_WhenDisplayModeIsRelativeToSolution_ReturnsSolutionRelativePath()
    {
        string solutionDirectory = Path.Combine("E:\\", "Work", "App");
        string filePath = Path.Combine(solutionDirectory, "Services", "UserService.cs");
        string expected = Path.Combine("Services", "UserService.cs");

        string result = SourcePathPathFormatter.Format(
            filePath,
            SourcePathDisplayMode.RelativeToSolution,
            solutionDirectory);

        Assert.AreEqual(expected, result);
    }

    [TestMethod]
    public void Format_WhenFileIsOutsideSolution_ReturnsOriginalPath()
    {
        string solutionDirectory = Path.Combine("E:\\", "Work", "App");
        string filePath = Path.Combine("E:\\", "Other", "UserService.cs");

        string result = SourcePathPathFormatter.Format(
            filePath,
            SourcePathDisplayMode.RelativeToSolution,
            solutionDirectory);

        Assert.AreEqual(filePath, result);
    }

    [TestMethod]
    public void Format_WhenFilePathIsEmpty_ReturnsEmptyString()
    {
        string result = SourcePathPathFormatter.Format(
            " ",
            SourcePathDisplayMode.FullPath,
            Path.Combine("E:\\", "Work"));

        Assert.AreEqual(string.Empty, result);
    }

    [TestMethod]
    public void Format_WhenSolutionDirectoryHasTrailingSeparator_ReturnsRelativePath()
    {
        string solutionDirectory = Path.Combine("E:\\", "Work", "App") + Path.DirectorySeparatorChar;
        string filePath = Path.Combine(solutionDirectory, "Controllers", "HomeController.cs");
        string expected = Path.Combine("Controllers", "HomeController.cs");

        string result = SourcePathPathFormatter.Format(
            filePath,
            SourcePathDisplayMode.RelativeToSolution,
            solutionDirectory);

        Assert.AreEqual(expected, result);
    }
}
