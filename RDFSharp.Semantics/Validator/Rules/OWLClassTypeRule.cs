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
    /// OWL-DL validator rule checking for consistency of rdf:type relations characterizing individuals
    /// </summary>
    internal static class OWLClassTypeRule
    {
        internal static OWLValidatorReport ExecuteRule(OWLOntology ontology)
        {
            #region CheckAreDisjointClassTypes
            bool CheckAreDisjointClassTypes(RDFResource outerClassType, RDFResource innerClassType)
                => !outerClassType.Equals(innerClassType) && ontology.Model.ClassModel.CheckIsDisjointClassWith(outerClassType, innerClassType);
            #endregion

            OWLValidatorReport validatorRuleReport = new OWLValidatorReport();
            OWLOntologyDataLens dataLens = new OWLOntologyDataLens(new RDFResource("ex:fakeIndividual"), ontology);

            //rdf:type
            IEnumerator<RDFResource> individualsEnumerator = ontology.Data.IndividualsEnumerator;
            while (individualsEnumerator.MoveNext())
            {
                //Switch data lens to current individual and calculate its class types
                dataLens.Individual = individualsEnumerator.Current;
                List<RDFResource> classTypes = dataLens.ClassTypes();

                //There should not be disjoint classes assigned as class types of the same individual
                if (classTypes.Any(outerClassType => classTypes.Any(innerClassType => CheckAreDisjointClassTypes(outerClassType, innerClassType))))
                    validatorRuleReport.AddEvidence(new OWLValidatorEvidence(
                        OWLSemanticsEnums.OWLValidatorEvidenceCategory.Error,
                        nameof(OWLClassTypeRule),
                        $"Violation of 'rdf:type' relations on individual '{individualsEnumerator.Current}'",
                        "Revise your class model: you have disjoint classes which are "));
            }

            return validatorRuleReport;
        }
    }
}