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

namespace RDFSharp.Semantics
{
    /// <summary>
    /// OWLReasonerRuleLessOrEqualThanBuiltIn represents a built-in of type swrlb:lessThanOrEqual
    /// </summary>
    public class OWLOntologyReasonerRuleLessOrEqualThanBuiltIn : OWLReasonerRuleFilterBuiltIn
    {
        #region Properties
        /// <summary>
        /// Represents the Uri of the built-in (swrlb:lessThanOrEqual)
        /// </summary>
        private static RDFResource BuiltInUri = new RDFResource($"swrlb:lessThanOrEqual");
        #endregion

        #region Ctors
        /// <summary>
        /// Default-ctor to build a swrlb:lessThanOrEqual built-in with given arguments
        /// </summary>
        public OWLOntologyReasonerRuleLessOrEqualThanBuiltIn(RDFVariable leftArgument, RDFVariable rightArgument)
            : this(leftArgument, rightArgument as RDFPatternMember) { }

        /// <summary>
        /// Default-ctor to build a swrlb:lessThanOrEqual built-in with given arguments
        /// </summary>
        public OWLOntologyReasonerRuleLessOrEqualThanBuiltIn(RDFVariable leftArgument, OWLOntologyFact rightArgument)
            : this(leftArgument, rightArgument?.Value as RDFPatternMember) { }

        /// <summary>
        /// Default-ctor to build a swrlb:lessThanOrEqual built-in with given arguments
        /// </summary>
        public OWLOntologyReasonerRuleLessOrEqualThanBuiltIn(RDFVariable leftArgument, OWLOntologyLiteral rightArgument)
            : this(leftArgument, rightArgument?.Value as RDFPatternMember) { }

        /// <summary>
        /// Internal-ctor to build a swrlb:lessThanOrEqual built-in with given arguments
        /// </summary>
        internal OWLOntologyReasonerRuleLessOrEqualThanBuiltIn(RDFVariable leftArgument, RDFPatternMember rightArgument)
            : base(new OWLOntologyResource() { Value = BuiltInUri }, leftArgument, rightArgument)
                => this.BuiltInFilter = new RDFComparisonFilter(RDFQueryEnums.RDFComparisonFlavors.LessOrEqualThan, leftArgument, rightArgument);
        #endregion
    }
}