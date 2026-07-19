using System.Globalization;
using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Bardasoft.VisualStudio.SourcePath.Tests;

[TestClass]
public sealed class SourcePathTextTests
{
    [TestMethod]
    public void Get_WhenCultureIsEnglish_ReturnsEnglishText()
    {
        string result = SourcePathText.Get(
            SourcePathText.CopyFullPath,
            CultureInfo.GetCultureInfo("en-US"));

        Assert.AreEqual("Copy full path", result);
    }

    [TestMethod]
    public void Get_WhenCultureIsSpanish_ReturnsSpanishText()
    {
        string result = SourcePathText.Get(
            SourcePathText.CopyFullPath,
            CultureInfo.GetCultureInfo("es-ES"));

        Assert.AreEqual("Copiar ruta completa", result);
    }

    [TestMethod]
    public void Get_WhenCultureIsPortugueseBrazil_UsesPortugueseText()
    {
        string result = SourcePathText.Get(
            SourcePathText.OpenLocationInExplorer,
            CultureInfo.GetCultureInfo("pt-BR"));

        Assert.AreEqual("Abrir local no Explorer", result);
    }

    [TestMethod]
    public void Get_WhenCultureIsChineseTraditional_UsesChineseText()
    {
        string result = SourcePathText.Get(
            SourcePathText.CopyFileName,
            CultureInfo.GetCultureInfo("zh-TW"));

        Assert.AreEqual("复制文件名", result);
    }

    [TestMethod]
    public void Get_WhenCultureIsUnsupported_FallsBackToEnglish()
    {
        string result = SourcePathText.Get(
            SourcePathText.CopyFolderPath,
            CultureInfo.GetCultureInfo("tr-TR"));

        Assert.AreEqual("Copy folder path", result);
    }

    [TestMethod]
    public void Get_WhenKeyIsUnknown_ReturnsKey()
    {
        const string unknownKey = "UnknownSourcePathText";

        string result = SourcePathText.Get(
            unknownKey,
            CultureInfo.GetCultureInfo("ja-JP"));

        Assert.AreEqual(unknownKey, result);
    }

    [TestMethod]
    public void EverySupportedLanguage_HasEveryRequiredKey()
    {
        foreach (string language in SourcePathText.GetSupportedLanguages())
        {
            string[] missingKeys = SourcePathText
                .GetRequiredKeys()
                .Where(key => !SourcePathText.HasText(language, key))
                .ToArray();

            CollectionAssert.AreEqual(
                new string[0],
                missingKeys,
                "Missing localized SourcePath text for language '" + language + "'.");
        }
    }
}
