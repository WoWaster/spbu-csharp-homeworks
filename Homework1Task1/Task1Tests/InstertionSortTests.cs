using System;
using System.Collections.Generic;
using NUnit.Framework;
using Task1;

namespace Task1Tests;

public class Tests
{
    [SetUp]
    public void Setup()
    {
    }

    [Test]
    [Repeat(10)]
    public void SortIntTest()
    {
        var rand = new Random();
        var amount = rand.Next(10, 100);

        var numbersDefaultSort = new List<int>();
        for (var i = 0; i < amount; i++) numbersDefaultSort.Add(rand.Next(int.MaxValue, int.MaxValue));

        var numbersInsertionSort = new List<int>(numbersDefaultSort);

        numbersDefaultSort.Sort();
        numbersInsertionSort.InsertionSort();
        Assert.That(numbersInsertionSort, Is.EquivalentTo(numbersDefaultSort));
    }

    [Test]
    [Repeat(10)]
    public void SortDoubleTest()
    {
        var rand = new Random();
        var amount = rand.Next(10, 100);

        var numbersDefaultSort = new List<double>();
        for (var i = 0; i < amount; i++) numbersDefaultSort.Add(rand.NextDouble());

        var numbersInsertionSort = new List<double>(numbersDefaultSort);

        numbersDefaultSort.Sort();
        numbersInsertionSort.InsertionSort();
        Assert.That(numbersInsertionSort, Is.EquivalentTo(numbersDefaultSort));
    }

    [Test]
    public void SortStringTest()
    {
        var stringsDefaultSort = new List<string>
            { "eager", "arrange", "root", "fright", "pad", "justice", "loyal", "thunder", "excite", "behavior" };
        var stringsInsertionSort = new List<string>(stringsDefaultSort);
        stringsDefaultSort.Sort();
        stringsInsertionSort.InsertionSort();
        Assert.That(stringsInsertionSort, Is.EquivalentTo(stringsDefaultSort));
    }
}