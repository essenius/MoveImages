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
public class FilterCollectionTest
{
    private readonly ImageCollection _imageCollection =
        [
            new ImageMetaData(new SKImageInfo(10, 12 )),
            new ImageMetaData(new SKImageInfo(12, 10)),
            new ImageMetaData(new SKImageInfo(2, 4)),
            new ImageMetaData(new SKImageInfo(11, 9)),
            new ImageMetaData(new SKImageInfo(9, 9))
        ];
    
    [TestMethod]
    public void TestFilterCollectionOrientationLandscape()
    {
        _imageCollection.Filter(new FilterCollection(["/orientation:landscape"]));
        Assert.AreEqual(3, _imageCollection.Count);
        Assert.AreEqual(12, _imageCollection[0].Width);
        Assert.AreEqual(10, _imageCollection[0].Height);
        Assert.AreEqual(11, _imageCollection[1].Width);
        Assert.AreEqual(9, _imageCollection[1].Height);
        Assert.AreEqual(9, _imageCollection[2].Width);
        Assert.AreEqual(9, _imageCollection[2].Height);
    }

    [TestMethod]
    public void TestFilterCollectionMinSize10()
    {
        _imageCollection.Filter(new FilterCollection(["/minsize:10"]));
        Assert.AreEqual(3, _imageCollection.Count);
        Assert.AreEqual(10, _imageCollection[0].Width);
        Assert.AreEqual(12, _imageCollection[0].Height);
        Assert.AreEqual(12, _imageCollection[1].Width);
        Assert.AreEqual(10, _imageCollection[1].Height);
        Assert.AreEqual(11, _imageCollection[2].Width);
        Assert.AreEqual(9, _imageCollection[2].Height);
    }

    [TestMethod]
    public void TestFilterCollectionOrientationPortraitMinSize10()
    {
        _imageCollection.Filter(new FilterCollection(["/orientation:portrait", "/minsize:10"]));
        Assert.AreEqual(1, _imageCollection.Count);
        Assert.AreEqual(_imageCollection[0].Width, 10);
        Assert.AreEqual(_imageCollection[0].Height, 12);
    }

}