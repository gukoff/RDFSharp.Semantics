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
using System.Collections.Generic;

namespace RDFSharp.Semantics
{
    /// <summary>
    /// OWL-DL validator rule checking for usage of deprecated classes and properties
    /// </summary>
    internal static class OWLDeprecationRule
    {
        internal static OWLValidatorReport ExecuteRule(OWLOntology ontology)
        {
            OWLValidatorReport validatorRuleReport = new OWLValidatorReport();

            //owl:DeprecatedClass
            IEnumerator<RDFResource> deprecatedClassesEnumerator = ontology.Model.ClassModel.DeprecatedClassesEnumerator;
            while (deprecatedClassesEnumerator.MoveNext())
            {
                if (ontology.Data.ABoxGraph[null, RDFVocabulary.RDF.TYPE, deprecatedClassesEnumerator.Current, null].TriplesCount > 0)
                    validatorRuleReport.AddEvidence(new OWLValidatorEvidence(
                        OWLSemanticsEnums.OWLValidatorEvidenceCategory.Warning,
                        nameof(OWLDeprecationRule),
                        $"Deprecated class '{deprecatedClassesEnumerator.Current}' is used by individuals through 'rdf:type' relation",
                        $"Revise your 'rdf:type' relations: abandon active usage of deprecated classes (which may be removed in future ontology editions)"));
            }

            //owl:DeprecatedProperty
            IEnumerator<RDFResource> deprecatedPropertiesEnumerator = ontology.Model.PropertyModel.DeprecatedPropertiesEnumerator;
            while (deprecatedPropertiesEnumerator.MoveNext())
            {
                if (ontology.Data.ABoxGraph[null, deprecatedPropertiesEnumerator.Current, null, null].TriplesCount > 0)
                    validatorRuleReport.AddEvidence(new OWLValidatorEvidence(
                        OWLSemanticsEnums.OWLValidatorEvidenceCategory.Warning,
                        nameof(OWLDeprecationRule),
                        $"Deprecated property '{deprecatedPropertiesEnumerator.Current}' is used by individuals through object or datatype assertions",
                        $"Revise your object or datatype assertions: abandon active usage of deprecated properties (which may be removed in future ontology editions)"));
            }

            return validatorRuleReport;
        }
    }
}