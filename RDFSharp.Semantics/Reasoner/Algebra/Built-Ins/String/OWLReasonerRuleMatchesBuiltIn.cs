/*
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

using RDFSharp.Model;
using RDFSharp.Query;
using System.Text;
using System.Text.RegularExpressions;

namespace RDFSharp.Semantics
{
    /// <summary>
    /// OWLReasonerRuleMatchesBuiltIn represents a SWRL built-in filtering inferences of a rule's antecedent on a swrlb:matches basis
    /// </summary>
    public class OWLReasonerRuleMatchesBuiltIn : OWLReasonerRuleFilterBuiltIn
    {
        #region Properties
        /// <summary>
        /// Represents the Uri of the built-in (swrlb:matches)
        /// </summary>
        private static readonly RDFResource BuiltInUri = new RDFResource("swrlb:matches");
        #endregion

        #region Ctors
        /// <summary>
        /// Default-ctor to build a swrlb:matches built-in with given arguments
        /// </summary>
        public OWLReasonerRuleMatchesBuiltIn(RDFVariable leftArgument, Regex matchesRegex)
            : base(BuiltInUri, leftArgument, null)
        {
            if (matchesRegex == null)
                throw new OWLSemanticsException("Cannot create built-in because given \"matchesRegex\" parameter is null");

            StringBuilder regexFlags = new StringBuilder();
            if (matchesRegex.Options.HasFlag(RegexOptions.IgnoreCase))
                regexFlags.Append('i');
            if (matchesRegex.Options.HasFlag(RegexOptions.Singleline))
                regexFlags.Append('s');
            if (matchesRegex.Options.HasFlag(RegexOptions.Multiline))
                regexFlags.Append('m');
            if (matchesRegex.Options.HasFlag(RegexOptions.IgnorePatternWhitespace))
                regexFlags.Append('x');
            RightArgument = string.IsNullOrEmpty(regexFlags.ToString()) ? new RDFPlainLiteral($"{matchesRegex}") 
                                                                        : new RDFPlainLiteral($"{matchesRegex}\",\"{regexFlags}");
            BuiltInFilter = new RDFRegexFilter(leftArgument, matchesRegex);
        }
        #endregion
    }
}