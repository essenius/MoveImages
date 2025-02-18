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
public class MainHelper
{
    public static int Run(IEnumerable<string> arguments, string sourceFolder)
    {
        var filterCollection = new FilterCollection(arguments);
        if (string.IsNullOrEmpty(filterCollection.TargetFolder))
        {
            Console.WriteLine("Usage: MoveImages [/orientation:[portrait|landscape]] [\"/megapixels:<0.5\"] [/minsize:100] [targetFolder]\n" +
                              "Moves selected images from the current folder to the target folder.\n" +
                              "if orientation is specified, default target is the orientation. Otherwise, targetFolder is mandatory.");
            return 1;
        }
        var imageCollection = new ImageCollection(sourceFolder);
        Console.Write("Total files: " + imageCollection.Count + "; ");
        imageCollection.Filter(filterCollection);
        Console.WriteLine("Selected: " + imageCollection.Count);
        var targetFolder = Path.IsPathRooted(filterCollection.TargetFolder) ? filterCollection.TargetFolder: Path.Combine(sourceFolder, filterCollection.TargetFolder) ;
        imageCollection.MoveToFolder(targetFolder);

        return 0;
    } 
}