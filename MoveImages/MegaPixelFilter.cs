// Copyright 2015-2024 Rik Essenius
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

namespace MoveImages;

[SupportedOSPlatform("windows")]
public class MegaPixelFilter : IFilter
{
    private readonly double _minMegaPixels;
    private readonly bool _lower;

    public MegaPixelFilter(string parameter)
    {
        _lower = false;
        var start = 0;
        if (parameter[0] == '<' || parameter[0] == '>') 
        {
            _lower = parameter[0] == '<';
            start = 1;
        }
        _minMegaPixels = double.Parse(parameter[start..]);
    }

    public bool Match(ImageMetaData image)
    {
        var megapixels = image.Width / 1000000.0 * image.Height;

        var criterion = megapixels >= _minMegaPixels;
        return _lower ? !criterion : criterion;
    }

    public bool ProvidesDefault => false;
}