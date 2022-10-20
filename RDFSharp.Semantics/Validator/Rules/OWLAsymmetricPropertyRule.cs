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
    /// OWL2 validator rule checking for consistency of assertions using asymmetric properties [OWL2]
    /// </summary>
    internal static class OWLAsymmetricPropertyRule
    {
        internal static OWLValidatorReport ExecuteRule(OWLOntology ontology)
        {
            OWLValidatorReport validatorRuleReport = new OWLValidatorReport();

            //owl:AsymmetricProperty
            IEnumerator<RDFResource> asymmetricPropertiesEnumerator = ontology.Model.PropertyModel.AsymmetricPropertiesEnumerator;
            while (asymmetricPropertiesEnumerator.MoveNext())
            {
                if (ontology.Data.ABoxGraph[null, asymmetricPropertiesEnumerator.Current, null, null].Any(t => t.Subject.Equals(t.Object)))
                    validatorRuleReport.AddEvidence(new OWLValidatorEvidence(
                        OWLSemanticsEnums.OWLValidatorEvidenceCategory.Error,
                        nameof(OWLAsymmetricPropertyRule),
                        $"Asymmetric property '{asymmetricPropertiesEnumerator.Current}' is used in an object assertion having the same subject and object term: this violates OWL2 integrity",
                        "Revise your object assertions: correct asymmetric property usage in order to not have the same subject and object term"));

            }

            return validatorRuleReport;
        }
    }
}