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
    /// OWLReasonerRuleAddBuiltIn represents a SWRL built-in filtering inferences of a rule's antecedent on a swrlb:add basis
    /// </summary>
    public class OWLReasonerRuleAddBuiltIn : OWLReasonerRuleMathBuiltIn
    {
        #region Properties
        /// <summary>
        /// Represents the Uri of the built-in (swrlb:add)
        /// </summary>
        private static readonly RDFResource BuiltInUri = new RDFResource("swrlb:add");
        #endregion

        #region Ctors
        /// <summary>
        /// Default-ctor to build a swrlb:add built-in with given arguments
        /// </summary>
        public OWLReasonerRuleAddBuiltIn(RDFVariable leftArgument, RDFVariable rightArgument, double addValue)
            : base(BuiltInUri, leftArgument, rightArgument, addValue)
        {
            if (rightArgument == null)
                throw new OWLSemanticsException("Cannot create built-in because given \"rightArgument\" parameter is null");
        }
        #endregion
    }
}