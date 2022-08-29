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
    /// RDFOntologyDataChecker is responsible for real-time OWL-DL validation of ontologies during A-BOX individual modeling
    /// </summary>
    public static class RDFOntologyDataChecker
    {
        #region Methods
        /// <summary>
        /// Checks if the given leftIndividual can be same as the given rightIndividual without tampering OWL-DL integrity
        /// </summary>
        internal static bool CheckSameAsCompatibility(this RDFOntologyData data, RDFResource leftIndividual, RDFResource rightIndividual)
            => !data.CheckAreDifferentIndividuals(rightIndividual, leftIndividual);

        /// <summary>
        /// Checks if the given leftIndividual can be different from the given rightIndividual without tampering OWL-DL integrity
        /// </summary>
        internal static bool CheckDifferentFromCompatibility(this RDFOntologyData data, RDFResource leftIndividual, RDFResource rightIndividual)
            => !data.CheckAreSameIndividuals(rightIndividual, leftIndividual);

        /// <summary>
        /// Checks if the given leftIndividual can be linked to the given rightIndividual though the given objectProperty without tampering OWL-DL integrity
        /// </summary>
        internal static bool CheckObjectAssertionCompatibility(this RDFOntologyData data, RDFResource leftIndividual, RDFResource objectProperty, RDFResource rightIndividual)
            => !data.CheckHasNegativeObjectAssertion(leftIndividual, objectProperty, rightIndividual);

        /// <summary>
        /// Checks if the given leftIndividual can be linked to the given value though the given datatypeProperty without tampering OWL-DL integrity
        /// </summary>
        internal static bool CheckDatatypeAssertionCompatibility(this RDFOntologyData data, RDFResource individual, RDFResource datatypeProperty, RDFLiteral value)
            => !data.CheckHasNegativeDatatypeAssertion(individual, datatypeProperty, value);

        /// <summary>
        /// Checks if the given leftIndividual can be linked to the given rightIndividual though the given negative objectProperty without tampering OWL-DL integrity [OWL2]
        /// </summary>
        internal static bool CheckNegativeObjectAssertionCompatibility(this RDFOntologyData data, RDFResource leftIndividual, RDFResource objectProperty, RDFResource rightIndividual)
            => !data.CheckHasObjectAssertion(leftIndividual, objectProperty, rightIndividual);

        /// <summary>
        /// Checks if the given leftIndividual can be linked to the given value though the given negative datatypeProperty without tampering OWL-DL integrity [OWL2]
        /// </summary>
        internal static bool CheckNegativeDatatypeAssertionCompatibility(this RDFOntologyData data, RDFResource individual, RDFResource datatypeProperty, RDFLiteral value)
            => !data.CheckHasDatatypeAssertion(individual, datatypeProperty, value);
        #endregion
    }
}