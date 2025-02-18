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

namespace MoveImagesTest;

[TestClass, SupportedOSPlatform("windows")]
public class FolderTest
{
    private static readonly string TempFolder = Path.GetTempPath();
    private readonly string _folder = Path.Combine(TempFolder, Path.GetRandomFileName());
    private readonly ImageCollection _imageCollection = [];

    [TestInitialize]
    public void Initialize()
    {
        Console.WriteLine(_folder);
        Directory.CreateDirectory(_folder);
        var bitmap = new Bitmap(10, 12);
        bitmap.Save(Path.Combine(_folder, "A" + Path.GetRandomFileName()));
        bitmap = new Bitmap(12, 9);
        bitmap.Save(Path.Combine(_folder, "B" + Path.GetRandomFileName()));
        _imageCollection.Load(_folder);
    }

    [TestCleanup]
    public void Cleanup()
    {
        const bool recursive = true; 
        Directory.Delete(_folder, recursive);            
    }

    [TestMethod]
    public void TestReadFromFolder()
    {
        Assert.AreEqual(2, _imageCollection.Count);
        Assert.IsTrue(_imageCollection[0].IsImage);
        Assert.AreEqual(10, _imageCollection[0].Width);
        Assert.AreEqual(12, _imageCollection[0].Height);
        Assert.IsFalse(string.IsNullOrEmpty(_imageCollection[0].Path));
        Assert.IsTrue(_imageCollection[1].IsImage);
        Assert.AreEqual(12, _imageCollection[1].Width);
        Assert.AreEqual(9, _imageCollection[1].Height);
        Assert.IsFalse(string.IsNullOrEmpty(_imageCollection[1].Path));
    }

    [TestMethod]
    public void TestMove()
    {
        var newFolder = Path.Combine(_folder, "movedTo");
        _imageCollection.MoveToFolder(newFolder);

        var filesAtOldLocation = Directory.GetFiles(_folder);
        Assert.AreEqual(0, filesAtOldLocation.Length);
        var filesAtNewLocation = Directory.GetFiles(newFolder);
        Assert.AreEqual(2, filesAtNewLocation.Length);
    }

    [TestMethod]
    public void MainHelperTestRelativeFolder()
    {
        Assert.AreEqual(0, MainHelper.Run( ["/orientation:portrait"], _folder));
        var targetFolder = Path.Combine(_folder, "portrait");
        var filesAtOldLocation = Directory.GetFiles(_folder);
        Assert.AreEqual(1, filesAtOldLocation.Length);
        var filesAtNewLocation = Directory.GetFiles(targetFolder);
        Assert.AreEqual(1, filesAtNewLocation.Length);
    }

    [TestMethod]
    public void MainHelperTestAbsoluteFolder()
    {
        var targetFolder = Path.Combine(Path.GetTempPath(), Path.GetRandomFileName());
        Assert.AreEqual(0, MainHelper.Run(["/orientation:portrait", targetFolder], _folder));
        var filesAtOldLocation = Directory.GetFiles(_folder);
        Assert.AreEqual(1, filesAtOldLocation.Length);
        var filesAtNewLocation = Directory.GetFiles(targetFolder);
        Assert.AreEqual(1, filesAtNewLocation.Length);
    }
    
}