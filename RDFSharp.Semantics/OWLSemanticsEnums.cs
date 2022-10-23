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

namespace RDFSharp.Semantics
{
    /// <summary>
    /// OWLSemanticsEnums represents a collector for all the enumerations used by the "RDFSharp.Semantics" namespace
    /// </summary>
    public static class OWLSemanticsEnums
    {
        /// <summary>
        /// Represents an enumeration for possible categories of ontology validator evidence
        /// </summary>
        public enum OWLValidatorEvidenceCategory
        {
            /// <summary>
            /// Specifications have not been violated: ontology may contain semantic inconsistencies
            /// </summary>
            Warning = 1,
            /// <summary>
            /// Specifications have been violated: ontology will contain semantic inconsistencies
            /// </summary>
            Error = 2
        };

        /// <summary>
        /// Represents an enumeration for available standard RDFS/OWL-DL/OWL2 validator rules
        /// </summary>
        public enum OWLValidatorStandardRules
        {
            /// <summary>
            /// This OWL-DL rule checks for vocabulary disjointness of classes, properties and individuals
            /// </summary>
            TermDisjointness = 1,
            /// <summary>
            /// This OWL-DL rule checks for explicit declaration of classes, properties and individuals
            /// </summary>
            TermDeclaration = 2,
            /// <summary>
            /// This OWL-DL rule checks for usage of deprecated classes and properties
            /// </summary>
            TermDeprecation = 3,
            /// <summary>
            /// This RDFS rule checks for consistency of rdfs:domain and rdfs:range knowledge
            /// </summary>
            DomainRange = 4,
            /// <summary>
            /// This OWL-DL rule checks for consistency of owl:inverseOf knowledge
            /// </summary>
            InverseOf = 5,
            /// <summary>
            /// This OWL-DL rule checks for consistency of owl:SymmetricProperty knowledge
            /// </summary>
            SymmetricProperty = 6,
            /// <summary>
            /// This OWL2 rule checks for consistency of owl:AsymmetricProperty knowledge
            /// </summary>
            AsymmetricProperty = 7,
            /// <summary>
            /// This OWL2 rule checks for consistency of owl:IrreflexiveProperty knowledge
            /// </summary>
            IrreflexiveProperty = 8,
            /// <summary>
            /// This OWL2 rule checks for consistency of owl:propertyDisjointWith knowledge
            /// </summary>
            PropertyDisjoint = 9,
            /// <summary>
            /// This OWL2 rule checks for consistency of owl:NegativePropertyAssertion knowledge
            /// </summary>
            NegativeAssertions = 10,
            /// <summary>
            /// This OWL2 rule checks for consistency of owl:hasKey knowledge
            /// </summary>
            ClassKey = 11,
            /// <summary>
            /// This OWL2 rule checks for consistency of owl:propertyChainAxiom knowledge
            /// </summary>
            PropertyChainAxiom = 12,
            /// <summary>
            /// This OWL-DL rule checks for consistency of rdf:type knowledge
            /// </summary>
            ClassType = 13,
            /// <summary>
            /// This OWL-DL rule checks for consistency of global cardinality constraints
            /// </summary>
            GlobalCardinalityConstraint = 14,
            /// <summary>
            /// This OWL-DL rule checks for consistency of local cardinality constraints
            /// </summary>
            LocalCardinalityConstraint = 15            
        };
    }
}