﻿/*
   Copyright 2012-2023 Marco De Salvo

   Licensed under the Apache License, Version 2.0 (the "License");
   you may not use this file except in compliance with the License.
   You may obtain a copy of the License at

     http://www.apache.org/licenses/LICENSE-2.0

   Unless required by applicable law or agreed to in writing, software
   distributed under the License is distributed on an "AS IS" BASIS,
   WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, either express or implied.
   See the License for the specific language governing permissions and
   limitations under the License.
*/

using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
[assembly: ComVisible(false)]
[assembly: Guid("10E8013A-CE1C-4641-B1BF-27AB111A54D6")]
//Internals
[assembly: InternalsVisibleTo("RDFSharp.Semantics.Test")]
//Internals (RDFSharp.Semantics.Extensions)
[assembly: InternalsVisibleTo("RDFSharp.Semantics.Extensions.SKOS")]
[assembly: InternalsVisibleTo("RDFSharp.Semantics.Extensions.SKOS.Test")]
[assembly: InternalsVisibleTo("RDFSharp.Semantics.Extensions.GEO")]
[assembly: InternalsVisibleTo("RDFSharp.Semantics.Extensions.GEO.Test")]
[assembly: InternalsVisibleTo("RDFSharp.Semantics.Extensions.TIME")]
[assembly: InternalsVisibleTo("RDFSharp.Semantics.Extensions.TIME.Test")]