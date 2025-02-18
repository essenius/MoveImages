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

namespace MoveImages;

[SupportedOSPlatform("windows")]
public class OrientationFilter(string orientationString) : IFilter
{
    private enum Orientation
    {
        Portrait,
        // ReSharper disable once UnusedMember.Local
        // this is used via the Enum.Parse function below
        Landscape
    }

    private readonly Orientation _orientation = (Orientation)Enum.Parse(typeof(Orientation), orientationString, IgnoreCase);
    private const bool IgnoreCase = true;

    public bool ProvidesDefault => true;

    public bool Match(ImageMetaData image)
    {
        if (_orientation == Orientation.Portrait)
        {
            return image.Height > image.Width;
        }
        return image.Width >= image.Height;
    }

}