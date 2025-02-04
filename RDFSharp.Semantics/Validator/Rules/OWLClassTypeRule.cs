﻿/*
   Copyright 2012-2023 Marco De Salvo
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
using System.Security.Claims;

namespace RDFSharp.Semantics
{
    /// <summary>
    /// OWL-DL validator rule checking for consistency of rdf:type relations characterizing individuals
    /// </summary>
    internal static class OWLClassTypeRule
    {
        internal static OWLValidatorReport ExecuteRule(OWLOntology ontology)
        {
            OWLValidatorReport validatorRuleReport = new OWLValidatorReport();
            Dictionary<long, HashSet<long>> disjointWithCache = new Dictionary<long, HashSet<long>>();

            // Precompute the graph with all triples that have predicate rdf:type
            // to efficiently slice it by subject in the loop.
            RDFGraph withTypePredicate = ontology.Data.ABoxGraph.SelectTriplesByPredicate(RDFVocabulary.RDF.TYPE);

            IEnumerator<RDFResource> individualsEnumerator = ontology.Data.IndividualsEnumerator;
            while (individualsEnumerator.MoveNext())
            {
                //Extract classes assigned to the current individual
                List<RDFResource> individualClasses = withTypePredicate.SelectTriplesBySubject(individualsEnumerator.Current)
                                                                   .Select(t => t.Object)
                                                                   .OfType<RDFResource>()
                                                                   .ToList();

                //Iterate discovered classes to check for eventual 'owl:disjointWith' clashes
                foreach (RDFResource individualClass in individualClasses)
                {
                    //Calculate disjoint classes of the current class
                    if (!disjointWithCache.ContainsKey(individualClass.PatternMemberID))
                        disjointWithCache.Add(individualClass.PatternMemberID, new HashSet<long>(ontology.Model.ClassModel.GetDisjointClassesWith(individualClass).Select(cls => cls.PatternMemberID)));
                
                    //There should not be disjoint classes assigned as class types of the same individual
                    if (individualClasses.Any(idvClass => !idvClass.Equals(individualClass) && disjointWithCache[individualClass.PatternMemberID].Contains(idvClass.PatternMemberID)))
                        validatorRuleReport.AddEvidence(new OWLValidatorEvidence(
                            OWLSemanticsEnums.OWLValidatorEvidenceCategory.Error,
                            nameof(OWLClassTypeRule),
                            $"Violation of 'rdf:type' relations on individual '{individualsEnumerator.Current}'",
                            "Revise your class model: you have disjoint classes to which this individual belongs at the same time!"));
                }                
            }

            return validatorRuleReport;
        }
    }
}