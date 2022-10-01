namespace Task2Tests;

using System.IO;
using NUnit.Framework;
using Task2;

public class BwtTests
{
    [SetUp]
    public void Setup()
    {
    }

    [Test]
    [TestCaseSource(nameof(_shortStrings))]
    public void ShortStringsTest(string text, string bwtTextExpected)
    {
        var (bwtTextActual, position) = Bwt.ForwardBwt(text);
        Assert.That(bwtTextActual, Is.EqualTo(bwtTextExpected));
        Assert.That(Bwt.ReverseBwt(bwtTextExpected, position),Is.EqualTo(text));
    }

    [Test]
    public void BigStringTest()
    {
        var bigText = File.ReadAllText("bigTextInput.txt");
        var bwtBigTextExpected = File.ReadAllText("bigTextOutput.txt");
        var (bwtBigText, position) = Bwt.ForwardBwt(bigText);
        Assert.That(bwtBigText, Is.EqualTo(bwtBigTextExpected));
        Assert.That(Bwt.ReverseBwt(bwtBigText, position),Is.EqualTo(bigText));
    }

    private static object[] _shortStrings =
    {
        new object[] {"l#p%dMvt0EugX2H@U12jaz@$", "l@ptUX1zH02d@gj%u2$#vEMa"},
        new object[] {"catanddoglovethefrog", "tcgndhveootgardlfaeo"},
        new object[] {"5884654654132123633", "2431163255866334485"},
        new object[] {"Old orange otters obey noodles.", "ysdes.rologtlbnOda  on  oeretoe"},
        new object[] {"31311235435", "31311542533"}
    }; 
}