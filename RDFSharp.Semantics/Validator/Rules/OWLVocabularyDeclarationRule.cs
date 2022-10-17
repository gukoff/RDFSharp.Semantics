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

namespace RDFSharp.Semantics
{
    /// <summary>
    /// OWL-DL validator rule checking for explicit declaration of classes, properties and individuals
    /// </summary>
    internal class OWLVocabularyDeclarationRule
    {
        internal static OWLValidatorReport ExecuteRule(OWLOntology ontology)
        {
            OWLValidatorReport validatorRuleReport = new OWLValidatorReport();

            #region ClassModel
            //rdfs:subClassOf
            foreach (RDFTriple subClassOfTriple in ontology.Model.ClassModel.TBoxGraph[null, RDFVocabulary.RDFS.SUB_CLASS_OF, null, null])
            {
                if (!ontology.Model.ClassModel.CheckHasClass((RDFResource)subClassOfTriple.Subject))
                    validatorRuleReport.AddEvidence(new OWLValidatorEvidence(
                        OWLSemanticsEnums.OWLValidatorEvidenceCategory.Warning,
                        nameof(OWLVocabularyDeclarationRule),
                        $"Declaration of class '{subClassOfTriple.Subject}' is not found in the model: it is required as subject of a 'rdfs:subClassOf' relation",
                        $"Declare '{subClassOfTriple.Subject}' class or restriction to the class model"));

                if (!ontology.Model.ClassModel.CheckHasClass((RDFResource)subClassOfTriple.Object))
                    validatorRuleReport.AddEvidence(new OWLValidatorEvidence(
                        OWLSemanticsEnums.OWLValidatorEvidenceCategory.Warning,
                        nameof(OWLVocabularyDeclarationRule),
                        $"Declaration of class '{subClassOfTriple.Object}' is not found in the model: it is required as object of a 'rdfs:subClassOf' relation",
                        $"Declare '{subClassOfTriple.Object}' class or restriction to the class model"));
            }
            //owl:equivalentClass
            foreach (RDFTriple equivalentClassTriple in ontology.Model.ClassModel.TBoxGraph[null, RDFVocabulary.OWL.EQUIVALENT_CLASS, null, null])
            {
                if (!ontology.Model.ClassModel.CheckHasClass((RDFResource)equivalentClassTriple.Subject))
                    validatorRuleReport.AddEvidence(new OWLValidatorEvidence(
                        OWLSemanticsEnums.OWLValidatorEvidenceCategory.Warning,
                        nameof(OWLVocabularyDeclarationRule),
                        $"Declaration of class '{equivalentClassTriple.Subject}' is not found in the model: it is required as subject of an 'owl:equivalentClass' relation",
                        $"Declare '{equivalentClassTriple.Subject}' class or restriction to the class model"));

                if (!ontology.Model.ClassModel.CheckHasClass((RDFResource)equivalentClassTriple.Object))
                    validatorRuleReport.AddEvidence(new OWLValidatorEvidence(
                        OWLSemanticsEnums.OWLValidatorEvidenceCategory.Warning,
                        nameof(OWLVocabularyDeclarationRule),
                        $"Declaration of class '{equivalentClassTriple.Object}' is not found in the model: it is required as object of an 'owl:equivalentClass' relation",
                        $"Declare '{equivalentClassTriple.Object}' class or restriction to the class model"));
            }
            //owl:disjointWith
            foreach (RDFTriple disjointWithTriple in ontology.Model.ClassModel.TBoxGraph[null, RDFVocabulary.OWL.DISJOINT_WITH, null, null])
            {
                if (!ontology.Model.ClassModel.CheckHasClass((RDFResource)disjointWithTriple.Subject))
                    validatorRuleReport.AddEvidence(new OWLValidatorEvidence(
                        OWLSemanticsEnums.OWLValidatorEvidenceCategory.Warning,
                        nameof(OWLVocabularyDeclarationRule),
                        $"Declaration of class '{disjointWithTriple.Subject}' is not found in the model: it is required as subject of an 'owl:disjointWith' relation",
                        $"Declare '{disjointWithTriple.Subject}' class or restriction to the class model"));

                if (!ontology.Model.ClassModel.CheckHasClass((RDFResource)disjointWithTriple.Object))
                    validatorRuleReport.AddEvidence(new OWLValidatorEvidence(
                        OWLSemanticsEnums.OWLValidatorEvidenceCategory.Warning,
                        nameof(OWLVocabularyDeclarationRule),
                        $"Declaration of class '{disjointWithTriple.Object}' is not found in the model: it is required as object of an 'owl:disjointWith' relation",
                        $"Declare '{disjointWithTriple.Object}' class or restriction to the class model"));
            }
            //owl:oneOf
            foreach (RDFTriple oneOfTriple in ontology.Model.ClassModel.TBoxGraph[null, RDFVocabulary.OWL.ONE_OF, null, null])
            {
                if (!ontology.Model.ClassModel.CheckHasClass((RDFResource)oneOfTriple.Subject))
                    validatorRuleReport.AddEvidence(new OWLValidatorEvidence(
                        OWLSemanticsEnums.OWLValidatorEvidenceCategory.Warning,
                        nameof(OWLVocabularyDeclarationRule),
                        $"Declaration of enumerate class '{oneOfTriple.Subject}' is not found in the model: it is required as subject of an 'owl:oneOf' relation",
                        $"Declare '{oneOfTriple.Subject}' enumerate class to the class model"));

                RDFCollection oneOfMembersCollection = RDFModelUtilities.DeserializeCollectionFromGraph(ontology.Model.ClassModel.TBoxGraph, (RDFResource)oneOfTriple.Object, RDFModelEnums.RDFTripleFlavors.SPO);
                foreach (RDFPatternMember oneOfMember in oneOfMembersCollection)
                {
                    if (!ontology.Data.CheckHasIndividual((RDFResource)oneOfMember))
                        validatorRuleReport.AddEvidence(new OWLValidatorEvidence(
                            OWLSemanticsEnums.OWLValidatorEvidenceCategory.Warning,
                            nameof(OWLVocabularyDeclarationRule),
                            $"Declaration of individual '{oneOfMember}' is not found in the data: it is required by 'owl:oneOf' relation of '{(RDFResource)oneOfTriple.Subject}' enumerate class",
                            $"Declare '{oneOfMember}' individual to the data"));
                }
            }
            //owl:unionOf
            foreach (RDFTriple unionOfTriple in ontology.Model.ClassModel.TBoxGraph[null, RDFVocabulary.OWL.UNION_OF, null, null])
            {
                if (!ontology.Model.ClassModel.CheckHasClass((RDFResource)unionOfTriple.Subject))
                    validatorRuleReport.AddEvidence(new OWLValidatorEvidence(
                        OWLSemanticsEnums.OWLValidatorEvidenceCategory.Warning,
                        nameof(OWLVocabularyDeclarationRule),
                        $"Declaration of union class '{unionOfTriple.Subject}' is not found in the model: it is required as subject of an 'owl:unionOf' relation",
                        $"Declare '{unionOfTriple.Subject}' union class to the class model"));

                RDFCollection unionMembersCollection = RDFModelUtilities.DeserializeCollectionFromGraph(ontology.Model.ClassModel.TBoxGraph, (RDFResource)unionOfTriple.Object, RDFModelEnums.RDFTripleFlavors.SPO);
                foreach (RDFPatternMember unionMember in unionMembersCollection)
                {
                    if (!ontology.Model.ClassModel.CheckHasClass((RDFResource)unionMember))
                        validatorRuleReport.AddEvidence(new OWLValidatorEvidence(
                            OWLSemanticsEnums.OWLValidatorEvidenceCategory.Warning,
                            nameof(OWLVocabularyDeclarationRule),
                            $"Declaration of class '{unionMember}' is not found in the model: it is required by 'owl:unionOf' relation of '{(RDFResource)unionOfTriple.Subject}' union class",
                            $"Declare '{unionMember}' class or restriction to the class model"));
                }
            }
            //owl:intersectionOf
            foreach (RDFTriple intersectionOfTriple in ontology.Model.ClassModel.TBoxGraph[null, RDFVocabulary.OWL.INTERSECTION_OF, null, null])
            {
                if (!ontology.Model.ClassModel.CheckHasClass((RDFResource)intersectionOfTriple.Subject))
                    validatorRuleReport.AddEvidence(new OWLValidatorEvidence(
                        OWLSemanticsEnums.OWLValidatorEvidenceCategory.Warning,
                        nameof(OWLVocabularyDeclarationRule),
                        $"Declaration of intersection class '{intersectionOfTriple.Subject}' is not found in the model: it is required as subject of an 'owl:intersectionOf' relation",
                        $"Declare '{intersectionOfTriple.Subject}' intersection class to the class model"));

                RDFCollection intersectionMembersCollection = RDFModelUtilities.DeserializeCollectionFromGraph(ontology.Model.ClassModel.TBoxGraph, (RDFResource)intersectionOfTriple.Object, RDFModelEnums.RDFTripleFlavors.SPO);
                foreach (RDFPatternMember intersectionMember in intersectionMembersCollection)
                {
                    if (!ontology.Model.ClassModel.CheckHasClass((RDFResource)intersectionMember))
                        validatorRuleReport.AddEvidence(new OWLValidatorEvidence(
                            OWLSemanticsEnums.OWLValidatorEvidenceCategory.Warning,
                            nameof(OWLVocabularyDeclarationRule),
                            $"Declaration of class '{intersectionMember}' is not found in the model: it is required by 'owl:intersectionOf' relation of '{(RDFResource)intersectionOfTriple.Subject}' intersection class",
                            $"Declare '{intersectionMember}' class or restriction to the class model"));
                }
            }
            //owl:complementOf
            foreach (RDFTriple complementOfTriple in ontology.Model.ClassModel.TBoxGraph[null, RDFVocabulary.OWL.COMPLEMENT_OF, null, null])
            {
                if (!ontology.Model.ClassModel.CheckHasClass((RDFResource)complementOfTriple.Subject))
                    validatorRuleReport.AddEvidence(new OWLValidatorEvidence(
                        OWLSemanticsEnums.OWLValidatorEvidenceCategory.Warning,
                        nameof(OWLVocabularyDeclarationRule),
                        $"Declaration of complement class '{complementOfTriple.Subject}' is not found in the model: it is required as subject of an 'owl:complementOf' relation",
                        $"Declare '{complementOfTriple.Subject}' complement class to the class model"));

                if (!ontology.Model.ClassModel.CheckHasClass((RDFResource)complementOfTriple.Object))
                    validatorRuleReport.AddEvidence(new OWLValidatorEvidence(
                        OWLSemanticsEnums.OWLValidatorEvidenceCategory.Warning,
                        nameof(OWLVocabularyDeclarationRule),
                        $"Declaration of class '{complementOfTriple.Object}' is not found in the model: it is required by 'owl:complementOf' relation of '{(RDFResource)complementOfTriple.Subject}' complement class",
                        $"Declare '{complementOfTriple.Object}' class or restriction to the class model"));
            }
            //owl:hasKey [OWL2]
            foreach (RDFTriple hasKeyTriple in ontology.Model.ClassModel.TBoxGraph[null, RDFVocabulary.OWL.HAS_KEY, null, null])
            {
                if (!ontology.Model.ClassModel.CheckHasClass((RDFResource)hasKeyTriple.Subject))
                    validatorRuleReport.AddEvidence(new OWLValidatorEvidence(
                        OWLSemanticsEnums.OWLValidatorEvidenceCategory.Warning,
                        nameof(OWLVocabularyDeclarationRule),
                        $"Declaration of class '{hasKeyTriple.Subject}' is not found in the model: it is required as subject of an 'owl:hasKey' relation",
                        $"Declare '{hasKeyTriple.Subject}' class to the class model"));

                RDFCollection hasKeyMembersCollection = RDFModelUtilities.DeserializeCollectionFromGraph(ontology.Model.ClassModel.TBoxGraph, (RDFResource)hasKeyTriple.Object, RDFModelEnums.RDFTripleFlavors.SPO);
                foreach (RDFPatternMember hasKeyMember in hasKeyMembersCollection)
                {
                    if (!ontology.Model.PropertyModel.CheckHasProperty((RDFResource)hasKeyMember))
                        validatorRuleReport.AddEvidence(new OWLValidatorEvidence(
                            OWLSemanticsEnums.OWLValidatorEvidenceCategory.Warning,
                            nameof(OWLVocabularyDeclarationRule),
                            $"Declaration of property '{hasKeyMember}' is not found in the model: it is required by 'owl:hasKey' relation of '{(RDFResource)hasKeyTriple.Subject}' class",
                            $"Declare '{hasKeyMember}' property to the property model"));
                }
            }
            //owl:disjointUnionOf [OWL2]
            foreach (RDFTriple disjointUnionOfTriple in ontology.Model.ClassModel.TBoxGraph[null, RDFVocabulary.OWL.DISJOINT_UNION_OF, null, null])
            {
                if (!ontology.Model.ClassModel.CheckHasClass((RDFResource)disjointUnionOfTriple.Subject))
                    validatorRuleReport.AddEvidence(new OWLValidatorEvidence(
                        OWLSemanticsEnums.OWLValidatorEvidenceCategory.Warning,
                        nameof(OWLVocabularyDeclarationRule),
                        $"Declaration of disjoint union class '{disjointUnionOfTriple.Subject}' is not found in the model: it is required as subject of an 'owl:disjointUnionOf' relation",
                        $"Declare '{disjointUnionOfTriple.Subject}' disjoint union class to the class model"));

                RDFCollection disjointUnionMembersCollection = RDFModelUtilities.DeserializeCollectionFromGraph(ontology.Model.ClassModel.TBoxGraph, (RDFResource)disjointUnionOfTriple.Object, RDFModelEnums.RDFTripleFlavors.SPO);
                foreach (RDFPatternMember disjointUnionMember in disjointUnionMembersCollection)
                {
                    if (!ontology.Model.ClassModel.CheckHasClass((RDFResource)disjointUnionMember))
                        validatorRuleReport.AddEvidence(new OWLValidatorEvidence(
                            OWLSemanticsEnums.OWLValidatorEvidenceCategory.Warning,
                            nameof(OWLVocabularyDeclarationRule),
                            $"Declaration of class '{disjointUnionMember}' is not found in the model: it is required by 'owl:disjointUnionOf' relation of '{(RDFResource)disjointUnionOfTriple.Subject}' disjoint union class",
                            $"Declare '{disjointUnionMember}' class or restriction to the class model"));
                }
            }
            //owl:AllDisjointClasses [OWL2]
            foreach (RDFTriple allDisjointClassesTriple in ontology.Model.ClassModel.TBoxGraph[null, RDFVocabulary.RDF.TYPE, RDFVocabulary.OWL.ALL_DISJOINT_CLASSES, null])
            {
                if (!ontology.Model.ClassModel.CheckHasClass((RDFResource)allDisjointClassesTriple.Subject))
                    validatorRuleReport.AddEvidence(new OWLValidatorEvidence(
                        OWLSemanticsEnums.OWLValidatorEvidenceCategory.Warning,
                        nameof(OWLVocabularyDeclarationRule),
                        $"Declaration of AllDisjointClass class '{allDisjointClassesTriple.Subject}' is not found in the model: it is required as subject of an 'owl:members' relation",
                        $"Declare '{allDisjointClassesTriple.Subject}' AllDisjointClass class to the class model"));

                foreach (RDFTriple allDisjointClassesMembersTriple in ontology.Model.ClassModel.TBoxGraph[(RDFResource)allDisjointClassesTriple.Subject, RDFVocabulary.OWL.MEMBERS, null, null])
                {
                    RDFCollection allDisjointClassesMembersCollection = RDFModelUtilities.DeserializeCollectionFromGraph(ontology.Model.ClassModel.TBoxGraph, (RDFResource)allDisjointClassesMembersTriple.Object, RDFModelEnums.RDFTripleFlavors.SPO);
                    foreach (RDFPatternMember allDisjointClassesMember in allDisjointClassesMembersCollection)
                    {
                        if (!ontology.Model.ClassModel.CheckHasClass((RDFResource)allDisjointClassesMember))
                            validatorRuleReport.AddEvidence(new OWLValidatorEvidence(
                                OWLSemanticsEnums.OWLValidatorEvidenceCategory.Warning,
                                nameof(OWLVocabularyDeclarationRule),
                                $"Declaration of class '{allDisjointClassesMember}' is not found in the model: it is required by 'owl:members' relation of '{(RDFResource)allDisjointClassesTriple.Subject}' AllDisjointClasses class",
                                $"Declare '{allDisjointClassesMember}' class or restriction to the class model"));
                    }
                }
            }
            //owl:allValuesFrom
            foreach (RDFTriple allValuesFromTriple in ontology.Model.ClassModel.TBoxGraph[null, RDFVocabulary.OWL.ALL_VALUES_FROM, null, null])
            {
                if (!ontology.Model.ClassModel.CheckHasRestrictionClass((RDFResource)allValuesFromTriple.Subject))
                    validatorRuleReport.AddEvidence(new OWLValidatorEvidence(
                        OWLSemanticsEnums.OWLValidatorEvidenceCategory.Warning,
                        nameof(OWLVocabularyDeclarationRule),
                        $"Declaration of restriction '{allValuesFromTriple.Subject}' is not found in the model: it is required as subject of an 'owl:allValuesFrom' relation",
                        $"Declare '{allValuesFromTriple.Subject}' restriction to the class model"));

                if (!ontology.Model.ClassModel.CheckHasClass((RDFResource)allValuesFromTriple.Object))
                    validatorRuleReport.AddEvidence(new OWLValidatorEvidence(
                        OWLSemanticsEnums.OWLValidatorEvidenceCategory.Warning,
                        nameof(OWLVocabularyDeclarationRule),
                        $"Declaration of class '{allValuesFromTriple.Object}' is not found in the model: it is required by 'owl:allValuesFrom' relation of '{allValuesFromTriple.Subject}' restriction",
                        $"Declare '{allValuesFromTriple.Object}' class or restriction to the class model"));
            }
            //owl:someValuesFrom
            foreach (RDFTriple someValuesFromTriple in ontology.Model.ClassModel.TBoxGraph[null, RDFVocabulary.OWL.SOME_VALUES_FROM, null, null])
            {
                if (!ontology.Model.ClassModel.CheckHasRestrictionClass((RDFResource)someValuesFromTriple.Subject))
                    validatorRuleReport.AddEvidence(new OWLValidatorEvidence(
                        OWLSemanticsEnums.OWLValidatorEvidenceCategory.Warning,
                        nameof(OWLVocabularyDeclarationRule),
                        $"Declaration of restriction '{someValuesFromTriple.Subject}' is not found in the model: it is required as subject of an 'owl:someValuesFrom' relation",
                        $"Declare '{someValuesFromTriple.Subject}' restriction to the class model"));

                if (!ontology.Model.ClassModel.CheckHasClass((RDFResource)someValuesFromTriple.Object))
                    validatorRuleReport.AddEvidence(new OWLValidatorEvidence(
                        OWLSemanticsEnums.OWLValidatorEvidenceCategory.Warning,
                        nameof(OWLVocabularyDeclarationRule),
                        $"Declaration of class '{someValuesFromTriple.Object}' is not found in the model: it is required by 'owl:someValuesFrom' relation of '{someValuesFromTriple.Subject}' restriction",
                        $"Declare '{someValuesFromTriple.Object}' class or restriction to the class model"));
            }
            //owl:hasValue
            foreach (RDFTriple hasValueTriple in ontology.Model.ClassModel.TBoxGraph[null, RDFVocabulary.OWL.HAS_VALUE, null, null])
            {
                if (!ontology.Model.ClassModel.CheckHasRestrictionClass((RDFResource)hasValueTriple.Subject))
                    validatorRuleReport.AddEvidence(new OWLValidatorEvidence(
                        OWLSemanticsEnums.OWLValidatorEvidenceCategory.Warning,
                        nameof(OWLVocabularyDeclarationRule),
                        $"Declaration of restriction '{hasValueTriple.Subject}' is not found in the model: it is required as subject of an 'owl:hasValue' relation",
                        $"Declare '{hasValueTriple.Subject}' restriction to the class model"));

                if (hasValueTriple.TripleFlavor == RDFModelEnums.RDFTripleFlavors.SPO && !ontology.Data.CheckHasIndividual((RDFResource)hasValueTriple.Object))
                    validatorRuleReport.AddEvidence(new OWLValidatorEvidence(
                        OWLSemanticsEnums.OWLValidatorEvidenceCategory.Warning,
                        nameof(OWLVocabularyDeclarationRule),
                        $"Declaration of individual '{hasValueTriple.Object}' is not found in the data: it is required by 'owl:hasValue' relation of '{hasValueTriple.Subject}' restriction",
                        $"Declare '{hasValueTriple.Object}' individual to the data"));
            }
            //owl:hasSelf [OWL2]
            foreach (RDFTriple hasSelfTriple in ontology.Model.ClassModel.TBoxGraph[null, RDFVocabulary.OWL.HAS_SELF, null, null])
            {
                if (!ontology.Model.ClassModel.CheckHasRestrictionClass((RDFResource)hasSelfTriple.Subject))
                    validatorRuleReport.AddEvidence(new OWLValidatorEvidence(
                        OWLSemanticsEnums.OWLValidatorEvidenceCategory.Warning,
                        nameof(OWLVocabularyDeclarationRule),
                        $"Declaration of restriction '{hasSelfTriple.Subject}' is not found in the model: it is required as subject of an 'owl:hasSelf' relation",
                        $"Declare '{hasSelfTriple.Subject}' restriction to the class model"));
            }
            //owl:onClass [OWL2]
            foreach (RDFTriple onClassTriple in ontology.Model.ClassModel.TBoxGraph[null, RDFVocabulary.OWL.ON_CLASS, null, null])
            {
                if (!ontology.Model.ClassModel.CheckHasRestrictionClass((RDFResource)onClassTriple.Subject))
                    validatorRuleReport.AddEvidence(new OWLValidatorEvidence(
                        OWLSemanticsEnums.OWLValidatorEvidenceCategory.Warning,
                        nameof(OWLVocabularyDeclarationRule),
                        $"Declaration of restriction '{onClassTriple.Subject}' is not found in the model: it is required as subject of an 'owl:hasValue' relation",
                        $"Declare '{onClassTriple.Subject}' restriction to the class model"));

                if (!ontology.Model.ClassModel.CheckHasClass((RDFResource)onClassTriple.Object))
                    validatorRuleReport.AddEvidence(new OWLValidatorEvidence(
                        OWLSemanticsEnums.OWLValidatorEvidenceCategory.Warning,
                        nameof(OWLVocabularyDeclarationRule),
                        $"Declaration of class '{onClassTriple.Object}' is not found in the data: it is required by 'owl:onClass' relation of '{onClassTriple.Subject}' qualified restriction",
                        $"Declare '{onClassTriple.Object}' class or restriction to the class model"));
            }
            //owl:onProperty
            foreach (RDFTriple onPropertyTriple in ontology.Model.ClassModel.TBoxGraph[null, RDFVocabulary.OWL.ON_PROPERTY, null, null])
            {
                if (!ontology.Model.ClassModel.CheckHasRestrictionClass((RDFResource)onPropertyTriple.Subject))
                    validatorRuleReport.AddEvidence(new OWLValidatorEvidence(
                        OWLSemanticsEnums.OWLValidatorEvidenceCategory.Warning,
                        nameof(OWLVocabularyDeclarationRule),
                        $"Declaration of restriction '{onPropertyTriple.Subject}' is not found in the model: it is required by 'owl:onProperty' relation on '{onPropertyTriple.Object}' property",
                        $"Declare '{onPropertyTriple.Subject}' restriction to the class model"));

                if (!ontology.Model.PropertyModel.CheckHasProperty((RDFResource)onPropertyTriple.Object))
                    validatorRuleReport.AddEvidence(new OWLValidatorEvidence(
                        OWLSemanticsEnums.OWLValidatorEvidenceCategory.Warning,
                        nameof(OWLVocabularyDeclarationRule),
                        $"Declaration of property '{onPropertyTriple.Object}' is not found in the model: it is required as object of an 'owl:onProperty' relation",
                        $"Declare '{onPropertyTriple.Object}' property to the property model"));
            }
            #endregion

            #region PropertyModel
            //rdfs:subPropertyOf
            foreach (RDFTriple subPropertyOfTriple in ontology.Model.PropertyModel.TBoxGraph[null, RDFVocabulary.RDFS.SUB_PROPERTY_OF, null, null])
            {
                if (!ontology.Model.PropertyModel.CheckHasProperty((RDFResource)subPropertyOfTriple.Subject))
                    validatorRuleReport.AddEvidence(new OWLValidatorEvidence(
                        OWLSemanticsEnums.OWLValidatorEvidenceCategory.Warning,
                        nameof(OWLVocabularyDeclarationRule),
                        $"Declaration of property '{subPropertyOfTriple.Subject}' is not found in the model: it is required as subject of a 'rdfs:subPropertyOf' relation",
                        $"Declare '{subPropertyOfTriple.Subject}' property to the property model"));

                if (!ontology.Model.PropertyModel.CheckHasProperty((RDFResource)subPropertyOfTriple.Object))
                    validatorRuleReport.AddEvidence(new OWLValidatorEvidence(
                        OWLSemanticsEnums.OWLValidatorEvidenceCategory.Warning,
                        nameof(OWLVocabularyDeclarationRule),
                        $"Declaration of property '{subPropertyOfTriple.Object}' is not found in the model: it is required as object of a 'rdfs:subPropertyOf' relation",
                        $"Declare '{subPropertyOfTriple.Object}' property to the property model"));
            }
            //owl:equivalentProperty
            foreach (RDFTriple equivalentPropertyTriple in ontology.Model.PropertyModel.TBoxGraph[null, RDFVocabulary.OWL.EQUIVALENT_PROPERTY, null, null])
            {
                if (!ontology.Model.PropertyModel.CheckHasProperty((RDFResource)equivalentPropertyTriple.Subject))
                    validatorRuleReport.AddEvidence(new OWLValidatorEvidence(
                        OWLSemanticsEnums.OWLValidatorEvidenceCategory.Warning,
                        nameof(OWLVocabularyDeclarationRule),
                        $"Declaration of property '{equivalentPropertyTriple.Subject}' is not found in the model: it is required as subject of an 'owl:equivalentProperty' relation",
                        $"Declare '{equivalentPropertyTriple.Subject}' property to the property model"));

                if (!ontology.Model.PropertyModel.CheckHasProperty((RDFResource)equivalentPropertyTriple.Object))
                    validatorRuleReport.AddEvidence(new OWLValidatorEvidence(
                        OWLSemanticsEnums.OWLValidatorEvidenceCategory.Warning,
                        nameof(OWLVocabularyDeclarationRule),
                        $"Declaration of property '{equivalentPropertyTriple.Object}' is not found in the model: it is required as object of an 'owl:equivalentProperty' relation",
                        $"Declare '{equivalentPropertyTriple.Object}' property to the property model"));
            }
            //owl:propertyDisjointWith [OWL2]
            foreach (RDFTriple propertyDisjointWithTriple in ontology.Model.PropertyModel.TBoxGraph[null, RDFVocabulary.OWL.PROPERTY_DISJOINT_WITH, null, null])
            {
                if (!ontology.Model.PropertyModel.CheckHasProperty((RDFResource)propertyDisjointWithTriple.Subject))
                    validatorRuleReport.AddEvidence(new OWLValidatorEvidence(
                        OWLSemanticsEnums.OWLValidatorEvidenceCategory.Warning,
                        nameof(OWLVocabularyDeclarationRule),
                        $"Declaration of property '{propertyDisjointWithTriple.Subject}' is not found in the model: it is required as subject of an 'owl:propertyDisjointWith' relation",
                        $"Declare '{propertyDisjointWithTriple.Subject}' property to the property model"));

                if (!ontology.Model.PropertyModel.CheckHasProperty((RDFResource)propertyDisjointWithTriple.Object))
                    validatorRuleReport.AddEvidence(new OWLValidatorEvidence(
                        OWLSemanticsEnums.OWLValidatorEvidenceCategory.Warning,
                        nameof(OWLVocabularyDeclarationRule),
                        $"Declaration of property '{propertyDisjointWithTriple.Object}' is not found in the model: it is required as object of an 'owl:propertyDisjointWith' relation",
                        $"Declare '{propertyDisjointWithTriple.Object}' property to the property model"));
            }
            //owl:inverseOf
            foreach (RDFTriple inverseOfTriple in ontology.Model.PropertyModel.TBoxGraph[null, RDFVocabulary.OWL.INVERSE_OF, null, null])
            {
                if (!ontology.Model.PropertyModel.CheckHasProperty((RDFResource)inverseOfTriple.Subject))
                    validatorRuleReport.AddEvidence(new OWLValidatorEvidence(
                        OWLSemanticsEnums.OWLValidatorEvidenceCategory.Warning,
                        nameof(OWLVocabularyDeclarationRule),
                        $"Declaration of property '{inverseOfTriple.Subject}' is not found in the model: it is required as subject of an 'owl:inverseOf' relation",
                        $"Declare '{inverseOfTriple.Subject}' property to the property model"));

                if (!ontology.Model.PropertyModel.CheckHasProperty((RDFResource)inverseOfTriple.Object))
                    validatorRuleReport.AddEvidence(new OWLValidatorEvidence(
                        OWLSemanticsEnums.OWLValidatorEvidenceCategory.Warning,
                        nameof(OWLVocabularyDeclarationRule),
                        $"Declaration of property '{inverseOfTriple.Object}' is not found in the model: it is required as object of an 'owl:inverseOf' relation",
                        $"Declare '{inverseOfTriple.Object}' property to the property model"));
            }
            //owl:propertyChainAxiom [OWL2]
            foreach (RDFTriple propertyChainAxiomTriple in ontology.Model.PropertyModel.TBoxGraph[null, RDFVocabulary.OWL.PROPERTY_CHAIN_AXIOM, null, null])
            {
                if (!ontology.Model.PropertyModel.CheckHasProperty((RDFResource)propertyChainAxiomTriple.Subject))
                    validatorRuleReport.AddEvidence(new OWLValidatorEvidence(
                        OWLSemanticsEnums.OWLValidatorEvidenceCategory.Warning,
                        nameof(OWLVocabularyDeclarationRule),
                        $"Declaration of property '{propertyChainAxiomTriple.Subject}' is not found in the model: it is required as subject of an 'owl:propertyChainAxiom' relation",
                        $"Declare '{propertyChainAxiomTriple.Subject}' property to the property model"));

                RDFCollection propertyChainAxiomMembersCollection = RDFModelUtilities.DeserializeCollectionFromGraph(ontology.Model.PropertyModel.TBoxGraph, (RDFResource)propertyChainAxiomTriple.Object, RDFModelEnums.RDFTripleFlavors.SPO);
                foreach (RDFPatternMember propertyChainAxiomMember in propertyChainAxiomMembersCollection)
                {
                    if (!ontology.Model.PropertyModel.CheckHasProperty((RDFResource)propertyChainAxiomMember))
                        validatorRuleReport.AddEvidence(new OWLValidatorEvidence(
                            OWLSemanticsEnums.OWLValidatorEvidenceCategory.Warning,
                            nameof(OWLVocabularyDeclarationRule),
                            $"Declaration of property '{propertyChainAxiomMember}' is not found in the model: it is required by 'owl:propertyChainAxiom' relation of '{(RDFResource)propertyChainAxiomTriple.Subject}' property",
                            $"Declare '{propertyChainAxiomMember}' property to the property model"));
                }
            }
            //owl:AllDisjointProperties [OWL2]
            foreach (RDFTriple allDisjointPropertiesTriple in ontology.Model.PropertyModel.TBoxGraph[null, RDFVocabulary.RDF.TYPE, RDFVocabulary.OWL.ALL_DISJOINT_PROPERTIES, null])
            {
                if (!ontology.Model.PropertyModel.CheckHasProperty((RDFResource)allDisjointPropertiesTriple.Subject))
                    validatorRuleReport.AddEvidence(new OWLValidatorEvidence(
                        OWLSemanticsEnums.OWLValidatorEvidenceCategory.Warning,
                        nameof(OWLVocabularyDeclarationRule),
                        $"Declaration of AllDisjointProperties class '{allDisjointPropertiesTriple.Subject}' is not found in the model: it is required as subject of an 'owl:members' relation",
                        $"Declare '{allDisjointPropertiesTriple.Subject}' AllDisjointProperties class to the property model"));

                foreach (RDFTriple allDisjointPropertiesMembersTriple in ontology.Model.PropertyModel.TBoxGraph[(RDFResource)allDisjointPropertiesTriple.Subject, RDFVocabulary.OWL.MEMBERS, null, null])
                {
                    RDFCollection allDisjointPropertiesMembersCollection = RDFModelUtilities.DeserializeCollectionFromGraph(ontology.Model.PropertyModel.TBoxGraph, (RDFResource)allDisjointPropertiesMembersTriple.Object, RDFModelEnums.RDFTripleFlavors.SPO);
                    foreach (RDFPatternMember allDisjointPropertiesMember in allDisjointPropertiesMembersCollection)
                    {
                        if (!ontology.Model.PropertyModel.CheckHasProperty((RDFResource)allDisjointPropertiesMember))
                            validatorRuleReport.AddEvidence(new OWLValidatorEvidence(
                                OWLSemanticsEnums.OWLValidatorEvidenceCategory.Warning,
                                nameof(OWLVocabularyDeclarationRule),
                                $"Declaration of property '{allDisjointPropertiesMember}' is not found in the model: it is required by 'owl:members' relation of '{(RDFResource)allDisjointPropertiesTriple.Subject}' AllDisjointProperties class",
                                $"Declare '{allDisjointPropertiesMember}' property to the property model"));
                    }
                }
            }
            //rdfs:domain
            foreach (RDFTriple domainTriple in ontology.Model.PropertyModel.TBoxGraph[null, RDFVocabulary.RDFS.DOMAIN, null, null])
            {
                if (!ontology.Model.PropertyModel.CheckHasProperty((RDFResource)domainTriple.Subject))
                    validatorRuleReport.AddEvidence(new OWLValidatorEvidence(
                        OWLSemanticsEnums.OWLValidatorEvidenceCategory.Warning,
                        nameof(OWLVocabularyDeclarationRule),
                        $"Declaration of property '{domainTriple.Subject}' is not found in the model: it is required as subject of a 'rdfs:domain' relation",
                        $"Declare '{domainTriple.Subject}' property to the property model"));

                if (!ontology.Model.ClassModel.CheckHasClass((RDFResource)domainTriple.Object))
                    validatorRuleReport.AddEvidence(new OWLValidatorEvidence(
                        OWLSemanticsEnums.OWLValidatorEvidenceCategory.Warning,
                        nameof(OWLVocabularyDeclarationRule),
                        $"Declaration of class '{domainTriple.Object}' is not found in the model: it is required by 'rdfs:domain' relation of '{(RDFResource)domainTriple.Subject}' property",
                        $"Declare '{domainTriple.Object}' class or restriction to the class model"));
            }
            //rdfs:range
            foreach (RDFTriple rangeTriple in ontology.Model.PropertyModel.TBoxGraph[null, RDFVocabulary.RDFS.RANGE, null, null])
            {
                if (!ontology.Model.PropertyModel.CheckHasProperty((RDFResource)rangeTriple.Subject))
                    validatorRuleReport.AddEvidence(new OWLValidatorEvidence(
                        OWLSemanticsEnums.OWLValidatorEvidenceCategory.Warning,
                        nameof(OWLVocabularyDeclarationRule),
                        $"Declaration of property '{rangeTriple.Subject}' is not found in the model: it is required as subject of a 'rdfs:range' relation",
                        $"Declare '{rangeTriple.Subject}' property to the property model"));

                if (!ontology.Model.ClassModel.CheckHasClass((RDFResource)rangeTriple.Object))
                    validatorRuleReport.AddEvidence(new OWLValidatorEvidence(
                        OWLSemanticsEnums.OWLValidatorEvidenceCategory.Warning,
                        nameof(OWLVocabularyDeclarationRule),
                        $"Declaration of class '{rangeTriple.Object}' is not found in the model: it is required by 'rdfs:range' relation of '{(RDFResource)rangeTriple.Subject}' property",
                        $"Declare '{rangeTriple.Object}' class or restriction to the class model"));
            }
            #endregion

            #region Data

            #endregion

            return validatorRuleReport;
        }
    }
}