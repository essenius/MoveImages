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

using System.Runtime.Versioning;
using SkiaSharp;

namespace MoveImages;

[SupportedOSPlatform("windows")]
public class ImageMetaData
{
    private void SetProperties(SKImageInfo image)
    {
        Width = image.Width;
        Height = image.Height;
        IsImage = true;                        
    }
    public ImageMetaData(SKImageInfo image)
    {
        SetProperties(image);
        Path = string.Empty;
    }

    public ImageMetaData(string imagePath)
    {
        Path = imagePath;
        using var image = SKImage.FromEncodedData(imagePath);
        if (image != null)
        {
            SetProperties(image.Info);
        }
    }
    public string Path { get; }
    public int Height { get; private set; }
    public int Width { get; private set; }
    public bool IsImage { get; private set; }
}