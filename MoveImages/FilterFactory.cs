﻿// Copyright 2015-2025 Rik Essenius
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
public static class FilterFactory
{
    public static IFilter Create(string method, string parameter)
    {
        return method.ToUpperInvariant() switch
        {
            "ORIENTATION" => new OrientationFilter(parameter),
            "MINSIZE" => new MinSizeFilter(int.Parse(parameter)),
            "MEGAPIXELS" => new MinMegaPixelFilter(parameter),
            _ => throw new ArgumentException("Unrecognized filter")
        };
    }
}