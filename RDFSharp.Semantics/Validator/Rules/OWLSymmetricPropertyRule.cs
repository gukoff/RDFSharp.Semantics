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
using System.Linq;

namespace RDFSharp.Semantics
{
    /// <summary>
    /// OWL-DL validator rule checking for consistency of assertions using symmetric properties
    /// </summary>
    internal static class OWLSymmetricPropertyRule
    {
        internal static OWLValidatorReport ExecuteRule(OWLOntology ontology)
        {
            OWLValidatorReport validatorRuleReport = new OWLValidatorReport();

            //owl:SymmetricProperty
            IEnumerator<RDFResource> symmetricPropertiesEnumerator = ontology.Model.PropertyModel.SymmetricPropertiesEnumerator;
            while (symmetricPropertiesEnumerator.MoveNext())
            {
                List<RDFResource> domainClasses = ontology.Model.PropertyModel.GetDomainOf(symmetricPropertiesEnumerator.Current);
                List<RDFResource> rangeClasses = ontology.Model.PropertyModel.GetRangeOf(symmetricPropertiesEnumerator.Current);
                if (domainClasses.Any() || rangeClasses.Any())
                {
                    RDFGraph symmetricObjectAssertions = ontology.Data.ABoxGraph[null, symmetricPropertiesEnumerator.Current, null, null];
                    foreach (RDFTriple symmetricObjectAssertion in symmetricObjectAssertions)
                    {
                        //rdfs:domain => object of the symmetric assertion should be compatible with these classes
                        if (domainClasses.Any(domainClass => !ontology.Data.CheckIsIndividualOf(ontology.Model, (RDFResource)symmetricObjectAssertion.Object, domainClass)))
                            validatorRuleReport.AddEvidence(new OWLValidatorEvidence(
                                OWLSemanticsEnums.OWLValidatorEvidenceCategory.Error,
                                nameof(OWLSymmetricPropertyRule),
                                $"Violation of 'owl:SymmetricProperty' behavior on property '{symmetricPropertiesEnumerator.Current}'",
                                "Revise your object assertions: fix symmetric property usage in order to not tamper domain/range constraints of the property"));

                        //rdfs:range => subject of the symmetric assertion should be compatible with these classes
                        if (rangeClasses.Any(rangeClass => !ontology.Data.CheckIsIndividualOf(ontology.Model, (RDFResource)symmetricObjectAssertion.Subject, rangeClass)))
                            validatorRuleReport.AddEvidence(new OWLValidatorEvidence(
                                OWLSemanticsEnums.OWLValidatorEvidenceCategory.Error,
                                nameof(OWLSymmetricPropertyRule),
                                $"Violation of 'owl:SymmetricProperty' behavior on property '{symmetricPropertiesEnumerator.Current}'",
                                "Revise your object assertions: fix symmetric property usage in order to not tamper domain/range constraints of the property"));
                    }
                }
            }

            return validatorRuleReport;
        }
    }
}