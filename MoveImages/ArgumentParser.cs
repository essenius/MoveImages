﻿// Copyright 2015-2024 Rik Essenius
// 
//   Licensed under the Apache License, Version 2.0 (the "License"); you may not use this file
//   except in compliance with the License. You may obtain a copy of the License at
// 
//       http://www.apache.org/licenses/LICENSE-2.0
// 
//   Unless required by applicable law or agreed to in writing, software distributed under the License
//   is distributed on an "AS IS" BASIS WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, either express or implied.
//   See the License for the specific language governing permissions and limitations under the License.

namespace MoveImages;

public class ArgumentParser(string argument)
{
    public bool IsFilter => argument[0] == '/';

    public string Filter => IsFilter ? argument.Split(':')[0][1..] : string.Empty;

    public string Value => IsFilter ? argument.Split(':')[1] : argument;
}