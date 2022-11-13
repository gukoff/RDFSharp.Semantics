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
    /// OWL-DL reasoner rule targeting data knowledge (A-BOX) to reason over rdf:type relations
    /// </summary>
    internal static class OWLIndividualTypeEntailmentRule
    {
        internal static OWLReasonerReport ExecuteRule(OWLOntology ontology)
        {
            #region RuleBody
            void InferClassIndividuals(RDFResource currentClass, List<RDFResource> classIndividuals, OWLReasonerReport report)
            {
                foreach (RDFResource classIndividual in classIndividuals)
                {
                    //Create the inferences
                    OWLReasonerEvidence evidence = new OWLReasonerEvidence(OWLSemanticsEnums.OWLReasonerEvidenceCategory.Data,
                        nameof(OWLIndividualTypeEntailmentRule), new RDFTriple(classIndividual, RDFVocabulary.RDF.TYPE, currentClass));

                    //Add the inferences to the report
                    if (!ontology.Data.ABoxGraph.ContainsTriple(evidence.EvidenceContent))
                        report.AddEvidence(evidence);
                }
            }
            #endregion

            OWLReasonerReport reasonerRuleReport = new OWLReasonerReport();

            IEnumerator<RDFResource> classesEnumerator = ontology.Model.ClassModel.ClassesEnumerator;
            while (classesEnumerator.MoveNext())
            {
                //SimpleClass
                if (ontology.Model.ClassModel.CheckHasSimpleClass(classesEnumerator.Current))
                {
                    OWLSemanticsEvents.RaiseSemanticsInfo($"SimpleClass:{classesEnumerator.Current}");
                    List<RDFResource> simpleClassIndividuals = ontology.Data.FindIndividualsOfClass(ontology.Model, classesEnumerator.Current);
                    InferClassIndividuals(classesEnumerator.Current, simpleClassIndividuals, reasonerRuleReport);
                    continue;
                }

                //EnumerateClass
                if (ontology.Model.ClassModel.CheckHasEnumerateClass(classesEnumerator.Current))
                {
                    OWLSemanticsEvents.RaiseSemanticsInfo($"EnumerateClass:{classesEnumerator.Current}");
                    List<RDFResource> enumerateClassIndividuals = ontology.Data.FindIndividualsOfEnumerate(ontology.Model, classesEnumerator.Current);
                    InferClassIndividuals(classesEnumerator.Current, enumerateClassIndividuals, reasonerRuleReport);
                    continue;
                }

                //RestrictionClass
                if (ontology.Model.ClassModel.CheckHasRestrictionClass(classesEnumerator.Current))
                {
                    OWLSemanticsEvents.RaiseSemanticsInfo($"RestrictionClass:{classesEnumerator.Current}");
                    List<RDFResource> restrictionClassIndividuals = ontology.Data.FindIndividualsOfRestriction(ontology.Model, classesEnumerator.Current);
                    InferClassIndividuals(classesEnumerator.Current, restrictionClassIndividuals, reasonerRuleReport);
                    continue;
                }

                //CompositeClass
                if (ontology.Model.ClassModel.CheckHasCompositeClass(classesEnumerator.Current))
                {
                    OWLSemanticsEvents.RaiseSemanticsInfo($"RestrictionClass:{classesEnumerator.Current}");
                    List<RDFResource> compositeClassIndividuals = ontology.Data.FindIndividualsOfComposite(ontology.Model, classesEnumerator.Current);
                    InferClassIndividuals(classesEnumerator.Current, compositeClassIndividuals, reasonerRuleReport);
                    continue;
                }
            }

            return reasonerRuleReport;
        }
    }
}