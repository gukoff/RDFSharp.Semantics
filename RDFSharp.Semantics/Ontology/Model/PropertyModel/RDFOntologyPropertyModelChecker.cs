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
using System.Collections.Generic;
using System.Linq;

namespace RDFSharp.Semantics
{
    /// <summary>
    /// RDFOntologyPropertyModelChecker is responsible for real-time OWL-DL validation of ontologies during T-BOX property modeling
    /// </summary>
    public static class RDFOntologyPropertyModelChecker
    {
        #region Methods
        /// <summary>
        /// Checks if the given owl:Property is a reserved ontology property
        /// </summary>
        internal static bool CheckReservedProperty(this RDFResource owlProperty) =>
            RDFSemanticsUtilities.ReservedProperties.Contains(owlProperty.PatternMemberID);

        /// <summary>
        /// Checks if the given childProperty can be subProperty of the given motherProperty without tampering OWL-DL integrity<br/>
        /// Does not accept property chain definitions (OWL2-DL decidability)
        /// </summary>
        internal static bool CheckSubPropertyCompatibility(this RDFOntologyPropertyModel propertyModel, RDFResource childProperty, RDFResource motherProperty)
            => !propertyModel.CheckAreSubProperties(motherProperty, childProperty)
                  && !propertyModel.CheckAreEquivalentProperties(motherProperty, childProperty)
                    && !propertyModel.CheckAreDisjointProperties(motherProperty, childProperty)
                      //OWL2-DL decidability
                      && !propertyModel.CheckHasPropertyChainAxiom(childProperty)
                        && !propertyModel.CheckHasPropertyChainAxiom(motherProperty);

        /// <summary>
        /// Checks if the given leftProperty can be equivalentProperty of the given rightProperty without tampering OWL-DL integrity<br/>
        /// Does not accept property chain definitions (OWL2-DL decidability)
        /// </summary>
        internal static bool CheckEquivalentPropertyCompatibility(this RDFOntologyPropertyModel propertyModel, RDFResource leftProperty, RDFResource rightProperty)
            => !propertyModel.CheckAreSubProperties(leftProperty, rightProperty)
                  && !propertyModel.CheckAreSuperPropertes(leftProperty, rightProperty)
                    && !propertyModel.CheckAreDisjointProperties(leftProperty, rightProperty)
                      //OWL2-DL decidability
                      && !propertyModel.CheckHasPropertyChainAxiom(leftProperty)
                        && !propertyModel.CheckHasPropertyChainAxiom(rightProperty);

        /// <summary>
        /// Checks if the given leftProperty can be propertyDisjointWith of the given rightProperty without tampering OWL-DL integrity [OWL2]
        /// </summary>
        internal static bool CheckDisjointPropertyCompatibility(this RDFOntologyPropertyModel propertyModel, RDFResource leftProperty, RDFResource rightProperty)
            => !propertyModel.CheckAreSubProperties(leftProperty, rightProperty)
                  && !propertyModel.CheckAreSuperPropertes(leftProperty, rightProperty)
                    && !propertyModel.CheckAreEquivalentProperties(leftProperty, rightProperty);

        /// <summary>
        /// Checks if the given leftProperty can be inverse of the given rightProperty without tampering OWL-DL integrity [OWL2]
        /// </summary>
        internal static bool CheckInversePropertyCompatibility(this RDFOntologyPropertyModel propertyModel, RDFResource leftProperty, RDFResource rightProperty)
            => !propertyModel.CheckAreSubProperties(leftProperty, rightProperty)
                  && !propertyModel.CheckAreSuperPropertes(leftProperty, rightProperty)
                    && !propertyModel.CheckAreEquivalentProperties(leftProperty, rightProperty);
        #endregion
    }
}