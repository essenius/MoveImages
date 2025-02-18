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
public class ImageTest
{
    [TestMethod]
    public void TestImageMetaDataFromImage()
    {
        var image = new SKImageInfo(9, 11);
        var imageMetaData = new ImageMetaData(image);
        Assert.IsTrue(imageMetaData.IsImage);
        Assert.AreEqual(9, imageMetaData.Width);
        Assert.AreEqual(11, imageMetaData.Height);
        Assert.IsTrue(string.IsNullOrEmpty(imageMetaData.Path));
    }

    [TestMethod]
    public void TestImageMetaDataFromFile()
    {
        var filename = Path.GetTempFileName();
        var image = new SKBitmap(9, 11);
        using (var data = image.Encode(SKEncodedImageFormat.Png, 80)) {
            using var stream = File.OpenWrite(filename);
            data.SaveTo(stream);
        }

        var imageMetaData = new ImageMetaData(filename);
        Assert.IsTrue(imageMetaData.IsImage);
        Assert.AreEqual(9, imageMetaData.Width);
        Assert.AreEqual(11, imageMetaData.Height);
        Assert.AreEqual(filename, imageMetaData.Path);
        File.Delete(filename);
    }

    [TestMethod]
    public void TestImageMetaDataWrongFile()
    {
        var filename = Path.GetTempFileName();
        var imageMetaData = new ImageMetaData(filename);
        Assert.IsFalse(imageMetaData.IsImage);
        Assert.AreEqual(filename, imageMetaData.Path);
    }
}