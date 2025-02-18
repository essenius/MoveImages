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

using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Runtime.Versioning;

namespace MoveImages;

[SupportedOSPlatform("windows")]
public class ImageCollection: Collection<ImageMetaData>
{
    public ImageCollection()
    {
    }

    public ImageCollection(string path)
    {
        Load(path);
    }

    public static void MoveCursorBack(int positions)
    {
        try
        {
            var currentLeft = Console.CursorLeft;
            var currentTop = Console.CursorTop;

            var newLeft = currentLeft - positions;
            if (newLeft < 0)
            {
                newLeft = 0;
            }

            Console.SetCursorPosition(newLeft, currentTop);
        } catch(IOException)
        {
            // ignore, happens with testing (no console)
        }
    }

    public void Load(string path)
    {
        Clear();
        var files = Directory.GetFiles(path);
        Console.WriteLine($"Adding {files.Length} images...");
        var count = 0;
        var previousPercentage = 0;
        Console.Write("  0% done");
        MoveCursorBack(9);

        foreach (var imagePath in Directory.GetFiles(path))
        {
            count++;
            var image = new ImageMetaData(imagePath);
            if (!image.IsImage) continue;
            Add(image);
            var percentage = 100 * count / files.Length;
            if (percentage == previousPercentage) continue;
            var formattedPercentage = percentage.ToString().PadLeft(3);
            Console.Write($"{formattedPercentage}");
            MoveCursorBack(3);
            previousPercentage = percentage;
        }

        Console.WriteLine();
    }

    public void Filter(FilterCollection filterCollection)
    {
        var selectedItems = this.Where(image =>
            filterCollection.Filters.Aggregate(true, (current, filter) => current && filter.Match(image))
        ).ToList();

        var selectedCount = selectedItems.Count;
        Console.WriteLine($"Number of selected items: {selectedCount}");

        foreach (var image in this.Except(selectedItems).ToList())
        {
            Remove(image);
        }

        /* foreach (var image in from image in this.Reverse() 
                 let selected = filterCollection.Filters.Aggregate(true, (current, filter) => current && filter.Match(image)) 
                 where !selected select image)
        {
            Remove(image);
        } */
    }

    public void MoveToFolder(string targetFolder)
    {
        Directory.CreateDirectory(targetFolder);
        foreach (var image in this)
        {
             Debug.Assert(image.Path != null, "image.Path != null");
            var newPath = Path.Combine(targetFolder, Path.GetFileName(image.Path));
            Console.WriteLine("move " + image.Path + " to " + newPath);
            try
            {
                File.Move(image.Path, newPath);
            }
            catch (IOException ioe)
            {
                Console.WriteLine($"{image.Path}: {ioe.Message}");
            }
        }
    }
}