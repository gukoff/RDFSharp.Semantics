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
    /// OWLSemanticsOptions represents a collector of admin options for tuning specific aspects of the ontology engine
    /// </summary>
    public static class OWLSemanticsOptions
    {
        /// <summary>
        /// This option permits to switch off the realtime protection of ontology taxonomies: this delivers a general performance boost,<br/>
        /// but may expose the ontology T-BOX / A-BOX to potential knowledge tampering risks (default: False) 
        /// </summary>
        public static bool DisableOntologyProtection { get; set; }

        /// <summary>
        /// This option permits to switch off the advanced features of the reasoner engine: this delivers a general performance boost,<br/>
        /// but may reduce the sensibility of detecting complex OWL-DL correlations between classes, properties and individuals (default: False)
        /// </summary>
        public static bool DisableAdvancedReasoner { get; set; }

        /// <summary>
        /// This option permits the automatic declaration of classes inside subClass, equivalentClass, disjointWith model relations.<br/>
        /// This may accomodate ontologies which are not OWL-DL compliant because not respecting mandatory class declaration (default: False) 
        /// </summary>
        public static bool EnableAutomaticClassDeclaration { get; set; }

        /// <summary>
        /// This option permits the automatic declaration of properties inside subProperty, equivalentProperty, propertyDisjointWith, inverseOf model relations.<br/>
        /// This may accomodate ontologies which are not OWL-DL compliant because not respecting mandatory property declaration (default: False) 
        /// </summary>
        public static bool EnableAutomaticPropertyDeclaration { get; set; }

        /// <summary>
        /// This option permits the automatic declaration of individuals inside sameAs, differentFrom, assertion, negativeAssertion data relations.<br/>
        /// This may accomodate ontologies which are not OWL-DL compliant because not respecting mandatory individual declaration (default: False) 
        /// </summary>
        public static bool EnableAutomaticIndividualDeclaration { get; set; }
    }
}