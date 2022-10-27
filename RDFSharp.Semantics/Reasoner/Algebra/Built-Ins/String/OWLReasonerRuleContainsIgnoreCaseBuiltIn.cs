/*
   Copyright 2012-2022 Marco De Salvo
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
    /// OWLReasonerRuleContainsIgnoreCaseBuiltIn represents a built-in of type swrlb:containsIgnoreCase
    /// </summary>
    public class OWLReasonerRuleContainsIgnoreCaseBuiltIn : OWLReasonerRuleFilterBuiltIn
    {
        #region Properties
        /// <summary>
        /// Represents the Uri of the built-in (swrlb:containsIgnoreCase)
        /// </summary>
        private static RDFResource BuiltInUri = new RDFResource($"swrlb:containsIgnoreCase");
        #endregion

        #region Ctors
        /// <summary>
        /// Default-ctor to build a swrlb:containsIgnoreCase built-in with given arguments
        /// </summary>
        public OWLReasonerRuleContainsIgnoreCaseBuiltIn(RDFVariable leftArgument, string containString)
            : base(new OWLOntologyResource() { Value = BuiltInUri }, leftArgument, null)
        {
            if (containString == null)
                throw new OWLSemanticsException("Cannot create built-in because given \"endString\" parameter is null.");

            //For printing, this built-in requires simulation of the right argument as plain literal
            this.RightArgument = new OWLOntologyLiteral(new RDFPlainLiteral(containString));

            this.BuiltInFilter = new RDFRegexFilter(leftArgument, new Regex($"{containString}", RegexOptions.IgnoreCase));
        }
        #endregion
    }
}