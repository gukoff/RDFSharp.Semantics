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
using System.Text.RegularExpressions;

namespace RDFSharp.Semantics
{
    /// <summary>
    /// OWLReasonerRuleStartsWithBuiltIn represents a SWRL built-in filtering inferences of a rule's antecedent on a swrlb:startsWith basis
    /// </summary>
    public class OWLReasonerRuleStartsWithBuiltIn : OWLReasonerRuleFilterBuiltIn
    {
        #region Properties
        /// <summary>
        /// Represents the Uri of the built-in (swrlb:startsWith)
        /// </summary>
        private static readonly RDFResource BuiltInUri = new RDFResource("swrlb:startsWith");
        #endregion

        #region Ctors
        /// <summary>
        /// Default-ctor to build a swrlb:startsWith built-in with given arguments
        /// </summary>
        public OWLReasonerRuleStartsWithBuiltIn(RDFVariable leftArgument, string startString)
            : base(BuiltInUri, leftArgument, null)
        {
            if (startString == null)
                throw new OWLSemanticsException("Cannot create built-in because given \"startString\" parameter is null");

            RightArgument = new RDFPlainLiteral(startString);
            BuiltInFilter = new RDFRegexFilter(leftArgument, new Regex($"^{startString}"));
        }
        #endregion
    }
}