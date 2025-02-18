// Copyright 2015-2025 Rik Essenius
// 
//   Licensed under the Apache License, Version 2.0 (the "License"); you may not use this file
//   except in compliance with the License. You may obtain a copy of the License at
// 
//       http://www.apache.org/licenses/LICENSE-2.0
// 
//   Unless required by applicable law or agreed to in writing, software distributed under the License
//   is distributed on an "AS IS" BASIS WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, either express or implied.
//   See the License for the specific language governing permissions and limitations under the License.

using SkiaSharp;

namespace MoveImagesTest;

[TestClass, SupportedOSPlatform("windows")]
public class FilterTest
{

    private const string Landscape = "landscape";
    private const string Portrait = "portrait";

    [TestMethod]
    public void TestFilterOrientationLandscapeMatch()
    {

        var filter = new OrientationFilter(Landscape);
        var image = new ImageMetaData(new SKImageInfo(4, 2));
        Assert.IsTrue(filter.Match(image));
    }

    [TestMethod]
    public void TestFilterOrientationLandscapeNoMatch()
    {

        var filter = new OrientationFilter(Landscape);
        var image = new ImageMetaData(new SKImageInfo(2, 4));
        Assert.IsFalse(filter.Match(image));
    }

    [TestMethod]
    public void TestFilterOrientationPortraitMatch()
    {

        var filter = new OrientationFilter(Portrait);
        var image = new ImageMetaData(new SKImageInfo(2, 4));
        Assert.IsTrue(filter.Match(image));
    }

    [TestMethod]
    public void TestFilterOrientationPortraitNoMatch()
    {
        var filter = new OrientationFilter(Portrait);
        var image = new ImageMetaData(new SKImageInfo(4, 2));
        Assert.IsFalse(filter.Match(image));
    }

    [TestMethod]
    [ExpectedException(typeof (ArgumentException))]
    public void TestFilterOrientationWrong()
    {

        _ = new OrientationFilter("bogus");
    }

    [TestMethod]
    public void TestFilterMinSizeMatch()
    {
        var filter = new MinSizeFilter(10);
        var image = new ImageMetaData(new SKImageInfo(12, 10));
        Assert.IsTrue(filter.Match(image));
    }

    [TestMethod]
    public void TestFilterMinSizeNoMatch()
    {
        var filter = new MinSizeFilter(10);
        var image = new ImageMetaData(new SKImageInfo(9, 8));
        Assert.IsFalse(filter.Match(image));
    }

    [TestMethod]
    public void TestFilterMinMegaPixelMatch()
    {
        var filter1 = new MegaPixelFilter("0.5");
        var image1 = new ImageMetaData(new SKImageInfo(720, 720));
        var image2 = new ImageMetaData(new SKImageInfo(640, 480));
        Assert.IsTrue(filter1.Match(image1));
        Assert.IsFalse(filter1.Match(image2));
        var filter2 = new MegaPixelFilter(">0.5");
        Assert.IsTrue(filter2.Match(image1));
        Assert.IsFalse(filter2.Match(image2));
    }

    [TestMethod]
    public void TestFilterMaxMegaPixelMatch()
    {
        var filter = new MegaPixelFilter("<0.5");
        var image1 = new ImageMetaData(new SKImageInfo(640, 480));
        var image2 = new ImageMetaData(new SKImageInfo(720, 720));
        Assert.IsTrue(filter.Match(image1));
        Assert.IsFalse(filter.Match(image2));
    }

    [TestMethod]
    public void TestCreateOrientationFilter()
    {
        var filter = FilterFactory.Create("orientation", Landscape);
        Assert.IsTrue(filter.GetType() == typeof (OrientationFilter));
    }

    [TestMethod]
    public void TestCreateMinSizeFilter()
    {
        var filter = FilterFactory.Create("minsize", "100");
        Assert.IsTrue(filter.GetType() == typeof (MinSizeFilter));
    }

    [TestMethod]
    [ExpectedException(typeof (FormatException))]
    public void TestCreateMinSizeWrongFilter()
    {
        FilterFactory.Create("minsize", "bogus");
    }

    [TestMethod]
    [ExpectedException(typeof (ArgumentException))]
    public void TestCreateWrongFilter()
    {
        FilterFactory.Create("bogus", string.Empty);
    }

    [TestMethod]
    public void TestParseFilterArgument()
    {
        const string argument = "/orientation:landscape";
        var parser = new ArgumentParser(argument);
        Assert.IsTrue(parser.IsFilter);
        Assert.AreEqual("orientation", parser.Filter);
        Assert.AreEqual("landscape", parser.Value);
    }

    [TestMethod]
    public void TestParseNonFilterArgument()
    {
        const string argument = "path";
        var parser = new ArgumentParser(argument);
        Assert.IsFalse(parser.IsFilter);
        Assert.AreEqual(string.Empty, parser.Filter);
        Assert.AreEqual("path", parser.Value);
    }

    [TestMethod]
    public void TestFilterCollectionNoDefault()
    {
        var arguments = new[] {"/orientation:landscape", "/minsize:100", ".."};
        var fc = new FilterCollection(arguments);
        Assert.IsTrue(fc.Filters.Count == 2);
        Assert.AreEqual(typeof (OrientationFilter), fc.Filters[0].GetType());
        Assert.AreEqual(typeof (MinSizeFilter), fc.Filters[1].GetType());
        Assert.AreEqual("..", fc.TargetFolder);
    }

    [TestMethod]
    public void TestFilterCollectionDefault()
    {
        var arguments = new[] {"/minsize:100", "/orientation:landscape"};
        var fc = new FilterCollection(arguments);
        Assert.IsTrue(fc.Filters.Count == 2);
        Assert.AreEqual(typeof (MinSizeFilter), fc.Filters[0].GetType());
        Assert.AreEqual(typeof (OrientationFilter), fc.Filters[1].GetType());
        Assert.AreEqual("landscape", fc.TargetFolder);
    }

    [TestMethod]
    public void TestFilterCollectionMegaPixel()
    {
        var arguments = new[] { "/orientation:landscape", "/megapixels:0.5" };
        var fc = new FilterCollection(arguments);
        Assert.IsTrue(fc.Filters.Count == 2);
        Assert.AreEqual(typeof(OrientationFilter), fc.Filters[0].GetType());
        Assert.AreEqual(typeof(MegaPixelFilter), fc.Filters[1].GetType());
        Assert.AreEqual("landscape", fc.TargetFolder);
    }

    [TestMethod]
    public void MainHelperTestNoArgs()
    {
        Assert.AreEqual(1, MainHelper.Run(Array.Empty<string>(), null!));
    }
}