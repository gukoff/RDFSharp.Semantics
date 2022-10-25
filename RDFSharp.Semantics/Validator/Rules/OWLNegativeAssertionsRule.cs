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
    /// OWL2 validator rule checking for consistency of negative assertions [OWL2]
    /// </summary>
    internal static class OWLNegativeAssertionsRule
    {
        internal static OWLValidatorReport ExecuteRule(OWLOntology ontology)
        {
            OWLValidatorReport validatorRuleReport = new OWLValidatorReport();

            //owl:NegativeObjectProperty
            RDFGraph objectAssertions = OWLOntologyDataLoader.GetObjectAssertions(ontology, ontology.Data.ABoxGraph);
            foreach (RDFTriple negativeObjectAssertion in OWLOntologyDataLoader.GetNegativeObjectAssertions(ontology.Data.ABoxGraph))
            {
                //Enlist the individuals which are compatible with negative assertion subject
                List<RDFResource> compatibleSubjects = ontology.Data.GetSameIndividuals((RDFResource)negativeObjectAssertion.Subject)
                                                        .Union(new List<RDFResource>() { (RDFResource)negativeObjectAssertion.Subject }).ToList();

                //Enlist the object properties which are compatible with negative assertion predicate
                List<RDFResource> compatibleProperties = ontology.Model.PropertyModel.GetEquivalentPropertiesOf((RDFResource)negativeObjectAssertion.Predicate)
                                                          .Union(ontology.Model.PropertyModel.GetSubPropertiesOf((RDFResource)negativeObjectAssertion.Predicate))
                                                           .Union(new List<RDFResource>() { (RDFResource)negativeObjectAssertion.Predicate })
                                                            .ToList();

                //Enlist the individuals which are compatible with negative assertion object
                List<RDFResource> compatibleObjects = ontology.Data.GetSameIndividuals((RDFResource)negativeObjectAssertion.Object)
                                                       .Union(new List<RDFResource>() { (RDFResource)negativeObjectAssertion.Object }).ToList();

                //There should not be any object assertion conflicting with negative object assertions
                if (objectAssertions.Any(objAsn => compatibleSubjects.Any(subj => subj.Equals(objAsn.Subject))
                                                     && compatibleProperties.Any(pred => pred.Equals(objAsn.Predicate))
                                                       && compatibleObjects.Any(obj => obj.Equals(objAsn.Object))))
                    validatorRuleReport.AddEvidence(new OWLValidatorEvidence(
                        OWLSemanticsEnums.OWLValidatorEvidenceCategory.Error,
                        nameof(OWLNegativeAssertionsRule),
                        $"Violation of negative object assertion '{negativeObjectAssertion}'",
                        "Revise your object assertions: there should not be any object assertion conflicting with negative object assertions!"));
            }

            //owl:NegativeDatatypeProperty
            RDFGraph datatypeAssertions = OWLOntologyDataLoader.GetDatatypeAssertions(ontology, ontology.Data.ABoxGraph);
            foreach (RDFTriple negativeDatatypeAssertion in OWLOntologyDataLoader.GetNegativeDatatypeAssertions(ontology.Data.ABoxGraph))
            {
                //Enlist the individuals which are compatible with negative assertion subject
                List<RDFResource> compatibleSubjects = ontology.Data.GetSameIndividuals((RDFResource)negativeDatatypeAssertion.Subject)
                                                        .Union(new List<RDFResource>() { (RDFResource)negativeDatatypeAssertion.Subject }).ToList();

                //Enlist the object properties which are compatible with negative assertion predicate
                List<RDFResource> compatibleProperties = ontology.Model.PropertyModel.GetEquivalentPropertiesOf((RDFResource)negativeDatatypeAssertion.Predicate)
                                                          .Union(ontology.Model.PropertyModel.GetSubPropertiesOf((RDFResource)negativeDatatypeAssertion.Predicate))
                                                           .Union(new List<RDFResource>() { (RDFResource)negativeDatatypeAssertion.Predicate }).ToList();

                //There should not be any datatype assertion conflicting with negative datatype assertions
                if (datatypeAssertions.Any(dtAsn => compatibleSubjects.Any(subj => subj.Equals(dtAsn.Subject))
                                                      && compatibleProperties.Any(pred => pred.Equals(dtAsn.Predicate))
                                                        && negativeDatatypeAssertion.Object.Equals(dtAsn.Object)))
                    validatorRuleReport.AddEvidence(new OWLValidatorEvidence(
                        OWLSemanticsEnums.OWLValidatorEvidenceCategory.Error,
                        nameof(OWLNegativeAssertionsRule),
                        $"Violation of negative datatype assertion '{negativeDatatypeAssertion}'",
                        "Revise your datatype assertions: there should not be any datatype assertion conflicting with negative datatype assertions!"));
            }

            return validatorRuleReport;
        }
    }
}