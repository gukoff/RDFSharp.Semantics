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
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace RDFSharp.Semantics
{
    /// <summary>
    /// OWLValidatorRuleset implements a subset of RDFS/OWL-DL/OWL2 validator rules
    /// </summary>
    internal static class OWLValidatorRuleset
    {
        #region VocabularyDisjointness
        /// <summary>
        /// OWL-DL validator rule checking for vocabulary disjointness of classes, properties and individuals
        /// </summary>
        internal static OWLValidatorReport VocabularyDisjointness(OWLOntology ontology)
        {
            OWLValidatorReport validatorRuleReport = new OWLValidatorReport();

            #region ClassModel
            foreach (RDFResource owlClass in ontology.Model.ClassModel)
            {
                //ClassModel vs PropertyModel
                if (ontology.Model.PropertyModel.Properties.ContainsKey(owlClass.PatternMemberID))
                    validatorRuleReport.AddEvidence(new OWLValidatorEvidence(
                        OWLSemanticsEnums.OWLValidatorEvidenceCategory.Error,
                        nameof(VocabularyDisjointness),
                        $"Disjointess of class model and property model is violated because the name '{owlClass}' refers both to a class and a property",
                        "Remove or rename one of the two entities: at the moment the ontology is OWL Full!"));

                //ClassModel vs Data
                if (ontology.Data.Individuals.ContainsKey(owlClass.PatternMemberID))
                    validatorRuleReport.AddEvidence(new OWLValidatorEvidence(
                        OWLSemanticsEnums.OWLValidatorEvidenceCategory.Error,
                        nameof(VocabularyDisjointness),
                        $"Disjointess of class model and data is violated because the name '{owlClass}' refers both to a class and an individual",
                        "Remove or rename one of the two entities: at the moment the ontology is OWL Full!"));
            }
            #endregion

            #region PropertyModel
            //PropertyModel vs Data
            foreach (RDFResource owlProperty in ontology.Model.PropertyModel)
                if (ontology.Data.Individuals.ContainsKey(owlProperty.PatternMemberID))
                    validatorRuleReport.AddEvidence(new OWLValidatorEvidence(
                        OWLSemanticsEnums.OWLValidatorEvidenceCategory.Error,
                        nameof(VocabularyDisjointness),
                        $"Disjointess of property model and data is violated because the name '{owlProperty}' refers both to a property and an individual",
                        "Remove or rename one of the two entities: at the moment the ontology is OWL Full!"));
            #endregion

            return validatorRuleReport;
        }

        /// <summary>
        /// OWL-DL validator rule checking for explicit declaration of classes, properties and individuals
        /// </summary>
        internal static OWLValidatorReport VocabularyDeclaration(OWLOntology ontology)
        {
            OWLValidatorReport validatorRuleReport = new OWLValidatorReport();

            #region ClassModel
            //rdfs:subClassOf
            foreach (RDFTriple subClassOfTriple in ontology.Model.ClassModel.TBoxGraph[null, RDFVocabulary.RDFS.SUB_CLASS_OF, null, null])
            {
                if (!ontology.Model.ClassModel.CheckHasClass((RDFResource)subClassOfTriple.Subject))
                    validatorRuleReport.AddEvidence(new OWLValidatorEvidence(
                        OWLSemanticsEnums.OWLValidatorEvidenceCategory.Warning,
                        nameof(VocabularyDeclaration),
                        $"Declaration of class '{subClassOfTriple.Subject}' is not found in the model: it is required as subject of a rdfs:subClassOf relation",
                        $"Declare '{subClassOfTriple.Subject}' class or restriction to the class model"));
                if (!ontology.Model.ClassModel.CheckHasClass((RDFResource)subClassOfTriple.Object))
                    validatorRuleReport.AddEvidence(new OWLValidatorEvidence(
                        OWLSemanticsEnums.OWLValidatorEvidenceCategory.Warning,
                        nameof(VocabularyDeclaration),
                        $"Declaration of class '{subClassOfTriple.Object}' is not found in the model: it is required as object of a rdfs:subClassOf relation",
                        $"Declare '{subClassOfTriple.Object}' class or restriction to the class model"));
            }
            //owl:equivalentClass
            foreach (RDFTriple equivalentClassTriple in ontology.Model.ClassModel.TBoxGraph[null, RDFVocabulary.OWL.EQUIVALENT_CLASS, null, null])
            {
                if (!ontology.Model.ClassModel.CheckHasClass((RDFResource)equivalentClassTriple.Subject))
                    validatorRuleReport.AddEvidence(new OWLValidatorEvidence(
                        OWLSemanticsEnums.OWLValidatorEvidenceCategory.Warning,
                        nameof(VocabularyDeclaration),
                        $"Declaration of class '{equivalentClassTriple.Subject}' is not found in the model: it is required as subject of an owl:equivalentClass relation",
                        $"Declare '{equivalentClassTriple.Subject}' class or restriction to the class model"));
                if (!ontology.Model.ClassModel.CheckHasClass((RDFResource)equivalentClassTriple.Object))
                    validatorRuleReport.AddEvidence(new OWLValidatorEvidence(
                        OWLSemanticsEnums.OWLValidatorEvidenceCategory.Warning,
                        nameof(VocabularyDeclaration),
                        $"Declaration of class '{equivalentClassTriple.Object}' is not found in the model: it is required as object of an owl:equivalentClass relation",
                        $"Declare '{equivalentClassTriple.Object}' class or restriction to the class model"));
            }
            //owl:disjointWith
            foreach (RDFTriple disjointWithTriple in ontology.Model.ClassModel.TBoxGraph[null, RDFVocabulary.OWL.DISJOINT_WITH, null, null])
            {
                if (!ontology.Model.ClassModel.CheckHasClass((RDFResource)disjointWithTriple.Subject))
                    validatorRuleReport.AddEvidence(new OWLValidatorEvidence(
                        OWLSemanticsEnums.OWLValidatorEvidenceCategory.Warning,
                        nameof(VocabularyDeclaration),
                        $"Declaration of class '{disjointWithTriple.Subject}' is not found in the model: it is required as subject of an owl:disjointWith relation",
                        $"Declare '{disjointWithTriple.Subject}' class or restriction to the class model"));
                if (!ontology.Model.ClassModel.CheckHasClass((RDFResource)disjointWithTriple.Object))
                    validatorRuleReport.AddEvidence(new OWLValidatorEvidence(
                        OWLSemanticsEnums.OWLValidatorEvidenceCategory.Warning,
                        nameof(VocabularyDeclaration),
                        $"Declaration of class '{disjointWithTriple.Object}' is not found in the model: it is required as object of an owl:disjointWith relation",
                        $"Declare '{disjointWithTriple.Object}' class or restriction to the class model"));
            }
            //owl:oneOf
            foreach (RDFTriple oneOfTriple in ontology.Model.ClassModel.TBoxGraph[null, RDFVocabulary.OWL.ONE_OF, null, null])
            {
                if (!ontology.Model.ClassModel.CheckHasEnumerateClass((RDFResource)oneOfTriple.Subject))
                    validatorRuleReport.AddEvidence(new OWLValidatorEvidence(
                        OWLSemanticsEnums.OWLValidatorEvidenceCategory.Warning,
                        nameof(VocabularyDeclaration),
                        $"Declaration of enumerate class '{oneOfTriple.Subject}' is not found in the model: it is required as subject of an owl:oneOf relation",
                        $"Declare '{oneOfTriple.Subject}' enumerate class to the class model"));
                else
                {
                    List<RDFResource> oneOfMembers = ontology.Data.GetIndividualsOf(ontology.Model, (RDFResource)oneOfTriple.Subject);
                    foreach (RDFResource oneOfMember in oneOfMembers)
                    {
                        if (!ontology.Data.CheckHasIndividual(oneOfMember))
                            validatorRuleReport.AddEvidence(new OWLValidatorEvidence(
                                OWLSemanticsEnums.OWLValidatorEvidenceCategory.Warning,
                                nameof(VocabularyDeclaration),
                                $"Declaration of individual '{oneOfMember}' is not found in the data: it is required by owl:oneOf relation of '{(RDFResource)oneOfTriple.Subject}' enumerate class",
                                $"Declare '{oneOfMember}' individual to the data"));
                    }
                }
            }
            #endregion

            #region PropertyModel

            #endregion

            #region Data

            #endregion

            return validatorRuleReport;
        }
        #endregion
    }
}