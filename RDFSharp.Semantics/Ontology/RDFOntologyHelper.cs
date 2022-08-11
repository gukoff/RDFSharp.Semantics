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
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Threading.Tasks;

namespace RDFSharp.Semantics
{
    /// <summary>
    /// RDFOntologyHelper contains utility methods supporting RDFS/OWL-DL modeling, validation and reasoning
    /// </summary>
    public static class RDFOntologyHelper
    {
        #region Inferences

        #region GetInferences
        /// <summary>
        /// Gets an ontology made by semantic inferences found in the given one
        /// </summary>
        public static RDFOntology GetInferences(this RDFOntology ontology)
        {
            if (ontology == null)
                return null;

            RDFOntology result = new RDFOntology((RDFResource)ontology.Value)
            {
                Model = ontology.Model.GetInferences(),
                Data = ontology.Data.GetInferences()
            };
            return result;
        }

        /// <summary>
        /// Gets an ontology model made by semantic inferences found in the given one
        /// </summary>
        public static RDFOntologyModel GetInferences(this RDFOntologyModel ontologyModel)
        {
            if (ontologyModel == null)
                return null;

            RDFOntologyModel result = new RDFOntologyModel()
            {
                ClassModel = ontologyModel.ClassModel.GetInferences(),
                PropertyModel = ontologyModel.PropertyModel.GetInferences()
            };
            return result;
        }

        /// <summary>
        /// Gets an ontology class model made by semantic inferences found in the given one
        /// </summary>
        public static RDFOntologyClassModel GetInferences(this RDFOntologyClassModel ontologyClassModel)
        {
            if (ontologyClassModel == null)
                return null;

            RDFOntologyClassModel result = new RDFOntologyClassModel();
            
            //SubClassOf
            foreach (var entry in ontologyClassModel.Relations.SubClassOf.Where(tEntry => tEntry.InferenceType == RDFSemanticsEnums.RDFOntologyInferenceType.API || tEntry.InferenceType == RDFSemanticsEnums.RDFOntologyInferenceType.Reasoner))
                result.Relations.SubClassOf.AddEntry(entry);

            //EquivalentClass
            foreach (var entry in ontologyClassModel.Relations.EquivalentClass.Where(tEntry => tEntry.InferenceType == RDFSemanticsEnums.RDFOntologyInferenceType.API || tEntry.InferenceType == RDFSemanticsEnums.RDFOntologyInferenceType.Reasoner))
                result.Relations.EquivalentClass.AddEntry(entry);

            //DisjointWith
            foreach (var entry in ontologyClassModel.Relations.DisjointWith.Where(tEntry => tEntry.InferenceType == RDFSemanticsEnums.RDFOntologyInferenceType.API || tEntry.InferenceType == RDFSemanticsEnums.RDFOntologyInferenceType.Reasoner))
                result.Relations.DisjointWith.AddEntry(entry);

            //UnionOf
            foreach (var entry in ontologyClassModel.Relations.UnionOf.Where(tEntry => tEntry.InferenceType == RDFSemanticsEnums.RDFOntologyInferenceType.API || tEntry.InferenceType == RDFSemanticsEnums.RDFOntologyInferenceType.Reasoner))
                result.Relations.UnionOf.AddEntry(entry);

            //IntersectionOf
            foreach (var entry in ontologyClassModel.Relations.IntersectionOf.Where(tEntry => tEntry.InferenceType == RDFSemanticsEnums.RDFOntologyInferenceType.API || tEntry.InferenceType == RDFSemanticsEnums.RDFOntologyInferenceType.Reasoner))
                result.Relations.IntersectionOf.AddEntry(entry);

            //OneOf
            foreach (var entry in ontologyClassModel.Relations.OneOf.Where(tEntry => tEntry.InferenceType == RDFSemanticsEnums.RDFOntologyInferenceType.API || tEntry.InferenceType == RDFSemanticsEnums.RDFOntologyInferenceType.Reasoner))
                result.Relations.OneOf.AddEntry(entry);

            //HasKey [OWL2]
            foreach (var entry in ontologyClassModel.Relations.HasKey.Where(tEntry => tEntry.InferenceType == RDFSemanticsEnums.RDFOntologyInferenceType.API || tEntry.InferenceType == RDFSemanticsEnums.RDFOntologyInferenceType.Reasoner))
                result.Relations.HasKey.AddEntry(entry);

            return result;
        }

        /// <summary>
        /// Gets an ontology property model made by semantic inferences found in the given one
        /// </summary>
        public static RDFOntologyPropertyModel GetInferences(this RDFOntologyPropertyModel ontologyPropertyModel)
        {
            if (ontologyPropertyModel == null)
                return null;

            RDFOntologyPropertyModel result = new RDFOntologyPropertyModel();

            //SubPropertyOf
            foreach (var entry in ontologyPropertyModel.Relations.SubPropertyOf.Where(tEntry => tEntry.InferenceType == RDFSemanticsEnums.RDFOntologyInferenceType.API || tEntry.InferenceType == RDFSemanticsEnums.RDFOntologyInferenceType.Reasoner))
                result.Relations.SubPropertyOf.AddEntry(entry);

            //EquivalentProperty
            foreach (var entry in ontologyPropertyModel.Relations.EquivalentProperty.Where(tEntry => tEntry.InferenceType == RDFSemanticsEnums.RDFOntologyInferenceType.API || tEntry.InferenceType == RDFSemanticsEnums.RDFOntologyInferenceType.Reasoner))
                result.Relations.EquivalentProperty.AddEntry(entry);

            //InverseOf
            foreach (var entry in ontologyPropertyModel.Relations.InverseOf.Where(tEntry => tEntry.InferenceType == RDFSemanticsEnums.RDFOntologyInferenceType.API || tEntry.InferenceType == RDFSemanticsEnums.RDFOntologyInferenceType.Reasoner))
                result.Relations.InverseOf.AddEntry(entry);

            //PropertyDisjointWith [OWL2]
            foreach (var entry in ontologyPropertyModel.Relations.PropertyDisjointWith.Where(tEntry => tEntry.InferenceType == RDFSemanticsEnums.RDFOntologyInferenceType.API || tEntry.InferenceType == RDFSemanticsEnums.RDFOntologyInferenceType.Reasoner))
                result.Relations.PropertyDisjointWith.AddEntry(entry);

            //PropertyChainAxiom [OWL2]
            foreach (var entry in ontologyPropertyModel.Relations.PropertyChainAxiom.Where(tEntry => tEntry.InferenceType == RDFSemanticsEnums.RDFOntologyInferenceType.API || tEntry.InferenceType == RDFSemanticsEnums.RDFOntologyInferenceType.Reasoner))
                result.Relations.PropertyChainAxiom.AddEntry(entry);

            return result;
        }

        /// <summary>
        /// Gets an ontology data made by semantic inferences found in the given one
        /// </summary>
        public static RDFOntologyData GetInferences(this RDFOntologyData ontologyData)
        {
            if (ontologyData == null)
                return null;

            RDFOntologyData result = new RDFOntologyData();

            //ClassType
            foreach (var entry in ontologyData.Relations.ClassType.Where(tEntry => tEntry.InferenceType == RDFSemanticsEnums.RDFOntologyInferenceType.API || tEntry.InferenceType == RDFSemanticsEnums.RDFOntologyInferenceType.Reasoner))
                result.Relations.ClassType.AddEntry(entry);

            //SameAs
            foreach (var entry in ontologyData.Relations.SameAs.Where(tEntry => tEntry.InferenceType == RDFSemanticsEnums.RDFOntologyInferenceType.API || tEntry.InferenceType == RDFSemanticsEnums.RDFOntologyInferenceType.Reasoner))
                result.Relations.SameAs.AddEntry(entry);

            //DifferentFrom
            foreach (var entry in ontologyData.Relations.DifferentFrom.Where(tEntry => tEntry.InferenceType == RDFSemanticsEnums.RDFOntologyInferenceType.API || tEntry.InferenceType == RDFSemanticsEnums.RDFOntologyInferenceType.Reasoner))
                result.Relations.DifferentFrom.AddEntry(entry);

            //Assertions
            foreach (var entry in ontologyData.Relations.Assertions.Where(tEntry => tEntry.InferenceType == RDFSemanticsEnums.RDFOntologyInferenceType.API || tEntry.InferenceType == RDFSemanticsEnums.RDFOntologyInferenceType.Reasoner))
                result.Relations.Assertions.AddEntry(entry);

            //NegativeAssertions [OWL2]
            foreach (var entry in ontologyData.Relations.NegativeAssertions.Where(tEntry => tEntry.InferenceType == RDFSemanticsEnums.RDFOntologyInferenceType.API || tEntry.InferenceType == RDFSemanticsEnums.RDFOntologyInferenceType.Reasoner))
                result.Relations.NegativeAssertions.AddEntry(entry);

            return result;
        }
        #endregion

        #region ClearInferences
        /// <summary>
        /// Clears all the taxonomy entries marked as semantic inferences generated by a reasoner
        /// </summary>
        public static void ClearInferences(this RDFOntology ontology)
        {
            ontology?.Model.ClearInferences();
            ontology?.Data.ClearInferences();
        }

        /// <summary>
        /// Clears all the taxonomy entries marked as semantic inferences generated by a reasoner
        /// </summary>
        public static void ClearInferences(this RDFOntologyModel ontologyModel)
        {
            ontologyModel?.ClassModel.ClearInferences();
            ontologyModel?.PropertyModel.ClearInferences();
        }

        /// <summary>
        /// Clears all the taxonomy entries marked as semantic inferences generated by a reasoner
        /// </summary>
        public static void ClearInferences(this RDFOntologyClassModel ontologyClassModel)
        {
            //SubClassOf
            ontologyClassModel?.Relations.SubClassOf.Entries.RemoveAll(tEntry => tEntry.InferenceType == RDFSemanticsEnums.RDFOntologyInferenceType.Reasoner);

            //EquivalentClass
            ontologyClassModel?.Relations.EquivalentClass.Entries.RemoveAll(tEntry => tEntry.InferenceType == RDFSemanticsEnums.RDFOntologyInferenceType.Reasoner);

            //DisjointWith
            ontologyClassModel?.Relations.DisjointWith.Entries.RemoveAll(tEntry => tEntry.InferenceType == RDFSemanticsEnums.RDFOntologyInferenceType.Reasoner);

            //UnionOf
            ontologyClassModel?.Relations.UnionOf.Entries.RemoveAll(tEntry => tEntry.InferenceType == RDFSemanticsEnums.RDFOntologyInferenceType.Reasoner);

            //IntersectionOf
            ontologyClassModel?.Relations.IntersectionOf.Entries.RemoveAll(tEntry => tEntry.InferenceType == RDFSemanticsEnums.RDFOntologyInferenceType.Reasoner);

            //OneOf
            ontologyClassModel?.Relations.OneOf.Entries.RemoveAll(tEntry => tEntry.InferenceType == RDFSemanticsEnums.RDFOntologyInferenceType.Reasoner);

            //HasKey [OWL2]
            ontologyClassModel?.Relations.HasKey.Entries.RemoveAll(tEntry => tEntry.InferenceType == RDFSemanticsEnums.RDFOntologyInferenceType.Reasoner);
        }

        /// <summary>
        /// Clears all the taxonomy entries marked as semantic inferences generated by a reasoner
        /// </summary>
        public static void ClearInferences(this RDFOntologyPropertyModel ontologyPropertyModel)
        {
            //SubPropertyOf
            ontologyPropertyModel?.Relations.SubPropertyOf.Entries.RemoveAll(tEntry => tEntry.InferenceType == RDFSemanticsEnums.RDFOntologyInferenceType.Reasoner);

            //EquivalentProperty
            ontologyPropertyModel?.Relations.EquivalentProperty.Entries.RemoveAll(tEntry => tEntry.InferenceType == RDFSemanticsEnums.RDFOntologyInferenceType.Reasoner);

            //InverseOf
            ontologyPropertyModel?.Relations.InverseOf.Entries.RemoveAll(tEntry => tEntry.InferenceType == RDFSemanticsEnums.RDFOntologyInferenceType.Reasoner);

            //PropertyDisjointWith [OWL2]
            ontologyPropertyModel?.Relations.PropertyDisjointWith.Entries.RemoveAll(tEntry => tEntry.InferenceType == RDFSemanticsEnums.RDFOntologyInferenceType.Reasoner);

            //PropertyChainAxiom [OWL2]
            ontologyPropertyModel?.Relations.PropertyChainAxiom.Entries.RemoveAll(tEntry => tEntry.InferenceType == RDFSemanticsEnums.RDFOntologyInferenceType.Reasoner);
        }

        /// <summary>
        /// Clears all the taxonomy entries marked as semantic inferences generated by a reasoner
        /// </summary>
        public static void ClearInferences(this RDFOntologyData ontologyData)
        {
            //ClassType
            ontologyData?.Relations.ClassType.Entries.RemoveAll(tEntry => tEntry.InferenceType == RDFSemanticsEnums.RDFOntologyInferenceType.Reasoner);

            //SameAs
            ontologyData?.Relations.SameAs.Entries.RemoveAll(tEntry => tEntry.InferenceType == RDFSemanticsEnums.RDFOntologyInferenceType.Reasoner);

            //DifferentFrom
            ontologyData?.Relations.DifferentFrom.Entries.RemoveAll(tEntry => tEntry.InferenceType == RDFSemanticsEnums.RDFOntologyInferenceType.Reasoner);

            //Assertions
            ontologyData?.Relations.Assertions.Entries.RemoveAll(tEntry => tEntry.InferenceType == RDFSemanticsEnums.RDFOntologyInferenceType.Reasoner);

            //NegativeAssertions [OWL2]
            ontologyData?.Relations.NegativeAssertions.Entries.RemoveAll(tEntry => tEntry.InferenceType == RDFSemanticsEnums.RDFOntologyInferenceType.Reasoner);
        }
        #endregion

        #endregion

        #region Extensions

        #region Model Extensions
        /// <summary>
        /// Gets an ontology class of the given nature from the given RDF resource
        /// </summary>
        public static RDFOntologyClass ToRDFOntologyClass(this RDFResource ontResource, RDFSemanticsEnums.RDFOntologyClassNature nature = RDFSemanticsEnums.RDFOntologyClassNature.OWL)
            => new RDFOntologyClass(ontResource, nature);

        /// <summary>
        /// Gets an ontology property from the given RDF resource
        /// </summary>
        public static RDFOntologyProperty ToRDFOntologyProperty(this RDFResource ontResource)
            => new RDFOntologyProperty(ontResource);

        /// <summary>
        /// Gets an ontology object property from the given RDF resource
        /// </summary>
        public static RDFOntologyObjectProperty ToRDFOntologyObjectProperty(this RDFResource ontResource)
            => new RDFOntologyObjectProperty(ontResource);

        /// <summary>
        /// Gets an ontology datatype property from the given RDF resource
        /// </summary>
        public static RDFOntologyDatatypeProperty ToRDFOntologyDatatypeProperty(this RDFResource ontResource)
            => new RDFOntologyDatatypeProperty(ontResource);

        /// <summary>
        /// Gets an ontology annotation property from the given RDF resource
        /// </summary>
        public static RDFOntologyAnnotationProperty ToRDFOntologyAnnotationProperty(this RDFResource ontResource)
            => new RDFOntologyAnnotationProperty(ontResource);

        /// <summary>
        /// Gets an ontology individual from the given RDF resource
        /// </summary>
        public static RDFOntologyIndividual ToRDFOntologyIndividual(this RDFResource ontResource)
            => new RDFOntologyIndividual(ontResource);

        /// <summary>
        /// Gets an ontology literal from the given RDF literal
        /// </summary>
        public static RDFOntologyLiteral ToRDFOntologyLiteral(this RDFLiteral ontLiteral)
            => new RDFOntologyLiteral(ontLiteral);
        #endregion

        #region Query Extensions
        /// <summary>
        /// Applies the given SPARQL SELECT query to the given ontology (which is converted into
        /// a RDF graph including semantic inferences in respect of the given export behavior)
        /// </summary>
        public static RDFSelectQueryResult ApplyToOntology(this RDFSelectQuery selectQuery,
                                                           RDFOntology ontology,
                                                           RDFSemanticsEnums.RDFOntologyInferenceExportBehavior ontologyInferenceExportBehavior = RDFSemanticsEnums.RDFOntologyInferenceExportBehavior.ModelAndData)
        {
            if (selectQuery != null && ontology != null)
                return selectQuery.ApplyToGraph(ontology.ToRDFGraph(ontologyInferenceExportBehavior));

            return new RDFSelectQueryResult();
        }

        /// <summary>
        /// Asynchronously applies the given SPARQL SELECT query to the given ontology (which is converted into
        /// a RDF graph including semantic inferences in respect of the given export behavior)
        /// </summary>
        public static Task<RDFSelectQueryResult> ApplyToOntologyAsync(this RDFSelectQuery selectQuery,
                                                                      RDFOntology ontology,
                                                                      RDFSemanticsEnums.RDFOntologyInferenceExportBehavior ontologyInferenceExportBehavior = RDFSemanticsEnums.RDFOntologyInferenceExportBehavior.ModelAndData)
            => Task.Run(() => ApplyToOntology(selectQuery, ontology, ontologyInferenceExportBehavior));

        /// <summary>
        /// Applies the given SPARQL ASK query to the given ontology (which is converted into
        /// a RDF graph including semantic inferences in respect of the given export behavior)
        /// </summary>
        public static RDFAskQueryResult ApplyToOntology(this RDFAskQuery askQuery,
                                                        RDFOntology ontology,
                                                        RDFSemanticsEnums.RDFOntologyInferenceExportBehavior ontologyInferenceExportBehavior = RDFSemanticsEnums.RDFOntologyInferenceExportBehavior.ModelAndData)
        {
            if (askQuery != null && ontology != null)
                return askQuery.ApplyToGraph(ontology.ToRDFGraph(ontologyInferenceExportBehavior));

            return new RDFAskQueryResult();
        }

        /// <summary>
        /// Asynchronously applies the given SPARQL ASK query to the given ontology (which is converted into
        /// a RDF graph including semantic inferences in respect of the given export behavior)
        /// </summary>
        public static Task<RDFAskQueryResult> ApplyToOntologyAsync(this RDFAskQuery askQuery,
                                                                   RDFOntology ontology,
                                                                   RDFSemanticsEnums.RDFOntologyInferenceExportBehavior ontologyInferenceExportBehavior = RDFSemanticsEnums.RDFOntologyInferenceExportBehavior.ModelAndData)
            => Task.Run(() => ApplyToOntology(askQuery, ontology, ontologyInferenceExportBehavior));

        /// <summary>
        /// Applies the given SPARQL CONSTRUCT query to the given ontology (which is converted into
        /// a RDF graph including semantic inferences in respect of the given export behavior)
        /// </summary>
        public static RDFConstructQueryResult ApplyToOntology(this RDFConstructQuery constructQuery,
                                                              RDFOntology ontology,
                                                              RDFSemanticsEnums.RDFOntologyInferenceExportBehavior ontologyInferenceExportBehavior = RDFSemanticsEnums.RDFOntologyInferenceExportBehavior.ModelAndData)
        {
            if (constructQuery != null && ontology != null)
                return constructQuery.ApplyToGraph(ontology.ToRDFGraph(ontologyInferenceExportBehavior));

            return new RDFConstructQueryResult();
        }

        /// <summary>
        /// Asynchronously applies the given SPARQL CONSTRUCT query to the given ontology (which is converted into
        /// a RDF graph including semantic inferences in respect of the given export behavior)
        /// </summary>
        public static Task<RDFConstructQueryResult> ApplyToOntologyAsync(this RDFConstructQuery constructQuery,
                                                                         RDFOntology ontology,
                                                                         RDFSemanticsEnums.RDFOntologyInferenceExportBehavior ontologyInferenceExportBehavior = RDFSemanticsEnums.RDFOntologyInferenceExportBehavior.ModelAndData)
            => Task.Run(() => ApplyToOntology(constructQuery, ontology, ontologyInferenceExportBehavior));

        /// <summary>
        /// Applies the given SPARQL DESCRIBE query to the given ontology (which is converted into
        /// a RDF graph including semantic inferences in respect of the given export behavior)
        /// </summary>
        public static RDFDescribeQueryResult ApplyToOntology(this RDFDescribeQuery describeQuery,
                                                             RDFOntology ontology,
                                                             RDFSemanticsEnums.RDFOntologyInferenceExportBehavior ontologyInferenceExportBehavior = RDFSemanticsEnums.RDFOntologyInferenceExportBehavior.ModelAndData)
        {
            if (describeQuery != null && ontology != null)
                return describeQuery.ApplyToGraph(ontology.ToRDFGraph(ontologyInferenceExportBehavior));

            return new RDFDescribeQueryResult();
        }

        /// <summary>
        /// Asynchronously applies the given SPARQL DESCRIBE query to the given ontology (which is converted into
        /// a RDF graph including semantic inferences in respect of the given export behavior)
        /// </summary>
        public static Task<RDFDescribeQueryResult> ApplyToOntologyAsync(this RDFDescribeQuery describeQuery,
                                                                        RDFOntology ontology,
                                                                        RDFSemanticsEnums.RDFOntologyInferenceExportBehavior ontologyInferenceExportBehavior = RDFSemanticsEnums.RDFOntologyInferenceExportBehavior.ModelAndData)
            => Task.Run(() => ApplyToOntology(describeQuery, ontology, ontologyInferenceExportBehavior));
        #endregion

        #region SemanticsExtensions
        /// <summary>
        /// Gets a graph representation of the given taxonomy, exporting inferences according to the selected behavior [OWL2]
        /// </summary>
        internal static RDFGraph ReifyToRDFGraph(this RDFOntologyTaxonomy taxonomy, RDFSemanticsEnums.RDFOntologyInferenceExportBehavior infexpBehavior,
            string taxonomyName, RDFOntologyClassModel ontologyClassModel = null, RDFOntologyPropertyModel ontologyPropertyModel = null, RDFOntologyData ontologyData=null)
        {
            switch (taxonomyName)
            {
                //Semantic-based reification
                case nameof(RDFOntologyDataMetadata.NegativeAssertions): //OWL2
                case nameof(RDFOntologyAnnotations.AxiomAnnotations): //OWL2
                    return ReifySemanticTaxonomyToGraph(taxonomy, taxonomyName, infexpBehavior, ontologyClassModel, ontologyPropertyModel, ontologyData);

                //List-based reification
                case nameof(RDFOntologyClassModelMetadata.HasKey): //OWL2
                case nameof(RDFOntologyPropertyModelMetadata.PropertyChainAxiom): //OWL2
                case nameof(RDFOntologyDataMetadata.MemberList): //SKOS
                    return ReifyListTaxonomyToGraph(taxonomy, taxonomyName, infexpBehavior);

                //Triple-based reification
                default:
                    return ReifyTripleTaxonomyToGraph(taxonomy, infexpBehavior);
            }
        }
        private static RDFGraph ReifySemanticTaxonomyToGraph(RDFOntologyTaxonomy taxonomy, string taxonomyName, RDFSemanticsEnums.RDFOntologyInferenceExportBehavior infexpBehavior,
            RDFOntologyClassModel ontologyClassModel = null, RDFOntologyPropertyModel ontologyPropertyModel = null, RDFOntologyData ontologyData = null)
        {
            RDFGraph result = new RDFGraph();

            #region Utilities
            //Executes the semantic reification of the given taxonomy entry
            void BuildSemanticReification(bool isAxiomAnn, RDFOntologyTaxonomyEntry te, RDFTriple asnTriple,
                RDFResource type, RDFResource subjProp, RDFResource predProp, RDFResource objProp, RDFResource litProp)
            {
                //Reification
                RDFResource axiomRepresentative = new RDFResource(asnTriple.ReificationSubject.ToString().Replace("bnode:", "bnode:semref"));
                result.AddTriple(new RDFTriple(axiomRepresentative, RDFVocabulary.RDF.TYPE, type));
                result.AddTriple(new RDFTriple(axiomRepresentative, subjProp, (RDFResource)asnTriple.Subject));
                result.AddTriple(new RDFTriple(axiomRepresentative, predProp, (RDFResource)asnTriple.Predicate));
                if (asnTriple.TripleFlavor == RDFModelEnums.RDFTripleFlavors.SPL)
                    result.AddTriple(new RDFTriple(axiomRepresentative, litProp, (RDFLiteral)asnTriple.Object));
                else
                    result.AddTriple(new RDFTriple(axiomRepresentative, objProp, (RDFResource)asnTriple.Object));

                //AxiomAnnotation
                if (isAxiomAnn)
                    result.AddTriple(new RDFTriple(axiomRepresentative, (RDFResource)te.TaxonomyPredicate.Value, (RDFLiteral)te.TaxonomyObject.Value));
            };

            //Finds the taxonomy entry represented by the given ID in the ontology taxonomies
            RDFOntologyTaxonomyEntry FindTaxonomyEntry(long teID)
                => //ClassModel
                   ontologyClassModel?.Relations.SubClassOf.SelectEntryByID(teID) ?? 
                   ontologyClassModel?.Relations.EquivalentClass.SelectEntryByID(teID) ??
                   ontologyClassModel?.Relations.DisjointWith.SelectEntryByID(teID) ??
                   //PropertyModel
                   ontologyPropertyModel?.Relations.SubPropertyOf.SelectEntryByID(teID) ??
                   ontologyPropertyModel?.Relations.EquivalentProperty.SelectEntryByID(teID) ??
                   ontologyPropertyModel?.Relations.InverseOf.SelectEntryByID(teID) ??
                   ontologyPropertyModel?.Relations.PropertyDisjointWith.SelectEntryByID(teID) ??
                   //Data
                   ontologyData?.Relations.ClassType.SelectEntryByID(teID) ??
                   ontologyData?.Relations.SameAs.SelectEntryByID(teID) ??
                   ontologyData?.Relations.DifferentFrom.SelectEntryByID(teID) ??
                   ontologyData?.Relations.Member.SelectEntryByID(teID) ??
                   ontologyData?.Relations.MemberList.SelectEntryByID(teID) ??
                   ontologyData?.Relations.Assertions.SelectEntryByID(teID) ??
                   null;
            #endregion

            //Determine the semantic reification vocabulary to be used, depending on the working taxonomy
            bool isAxiomAnnotation = false;
            RDFResource rdfType = new RDFResource();
            RDFResource subjectProperty = new RDFResource();
            RDFResource predicateProperty = new RDFResource();
            RDFResource objectProperty = new RDFResource();
            RDFResource literalProperty = new RDFResource();
            switch (taxonomyName)
            {
                //NegativeAssertions [OWL2]
                case nameof(RDFOntologyDataMetadata.NegativeAssertions):
                    isAxiomAnnotation = false;
                    rdfType = RDFVocabulary.OWL.NEGATIVE_PROPERTY_ASSERTION;
                    subjectProperty = RDFVocabulary.OWL.SOURCE_INDIVIDUAL;
                    predicateProperty = RDFVocabulary.OWL.ASSERTION_PROPERTY;
                    objectProperty = RDFVocabulary.OWL.TARGET_INDIVIDUAL;
                    literalProperty = RDFVocabulary.OWL.TARGET_VALUE;
                    break;

                //Axiom Annotations [OWL2]
                case nameof(RDFOntologyAnnotations.AxiomAnnotations):
                    isAxiomAnnotation = true;
                    rdfType = RDFVocabulary.OWL.AXIOM;
                    subjectProperty = RDFVocabulary.OWL.ANNOTATED_SOURCE;
                    predicateProperty = RDFVocabulary.OWL.ANNOTATED_PROPERTY;
                    objectProperty = RDFVocabulary.OWL.ANNOTATED_TARGET;
                    literalProperty = RDFVocabulary.OWL.ANNOTATED_TARGET;
                    break;
            }            

            foreach (RDFOntologyTaxonomyEntry te in taxonomy)
            {
                RDFTriple asn = te.ToRDFTriple();

                //In case of owl:Axiom, we have to lookup the linked taxonomy entry by ID
                if (isAxiomAnnotation)
                {
                    string teID = te.TaxonomySubject.ToString().Replace("bnode:semref", string.Empty);
                    RDFOntologyTaxonomyEntry axiomAsn = FindTaxonomyEntry(long.Parse(teID));
                    if (axiomAsn == null)
                        continue;

                    asn = axiomAsn.ToRDFTriple();
                }

                //Do not export semantic inferences
                if (infexpBehavior == RDFSemanticsEnums.RDFOntologyInferenceExportBehavior.None)
                {
                    if (te.InferenceType == RDFSemanticsEnums.RDFOntologyInferenceType.None)
                        BuildSemanticReification(isAxiomAnnotation, te, asn, rdfType, subjectProperty, predicateProperty, objectProperty, literalProperty);
                }

                //Export semantic inferences related only to ontology model
                else if (infexpBehavior == RDFSemanticsEnums.RDFOntologyInferenceExportBehavior.OnlyModel)
                {
                    if (taxonomy.Category == RDFSemanticsEnums.RDFOntologyTaxonomyCategory.Model ||
                            taxonomy.Category == RDFSemanticsEnums.RDFOntologyTaxonomyCategory.Annotation)
                    {
                        BuildSemanticReification(isAxiomAnnotation, te, asn, rdfType, subjectProperty, predicateProperty, objectProperty, literalProperty);
                    }
                    else
                    {
                        if (te.InferenceType == RDFSemanticsEnums.RDFOntologyInferenceType.None)
                            BuildSemanticReification(isAxiomAnnotation, te, asn, rdfType, subjectProperty, predicateProperty, objectProperty, literalProperty);
                    }
                }

                //Export semantic inferences related only to ontology data
                else if (infexpBehavior == RDFSemanticsEnums.RDFOntologyInferenceExportBehavior.OnlyData)
                {
                    if (taxonomy.Category == RDFSemanticsEnums.RDFOntologyTaxonomyCategory.Data ||
                            taxonomy.Category == RDFSemanticsEnums.RDFOntologyTaxonomyCategory.Annotation)
                    {
                        BuildSemanticReification(isAxiomAnnotation, te, asn, rdfType, subjectProperty, predicateProperty, objectProperty, literalProperty);
                    }
                    else
                    {
                        if (te.InferenceType == RDFSemanticsEnums.RDFOntologyInferenceType.None)
                            BuildSemanticReification(isAxiomAnnotation, te, asn, rdfType, subjectProperty, predicateProperty, objectProperty, literalProperty);
                    }
                }

                //Export semantic inferences related both to ontology model and data
                else
                    BuildSemanticReification(isAxiomAnnotation, te, asn, rdfType, subjectProperty, predicateProperty, objectProperty, literalProperty);
            }

            return result;
        }
        private static RDFGraph ReifyListTaxonomyToGraph(RDFOntologyTaxonomy taxonomy, string taxonomyName, RDFSemanticsEnums.RDFOntologyInferenceExportBehavior infexpBehavior)
        {
            RDFGraph result = new RDFGraph();

            RDFResource taxonomyPredicate = taxonomyName.Equals(nameof(RDFOntologyClassModelMetadata.HasKey)) ? RDFVocabulary.OWL.HAS_KEY :
                                                taxonomyName.Equals(nameof(RDFOntologyPropertyModelMetadata.PropertyChainAxiom)) ? RDFVocabulary.OWL.PROPERTY_CHAIN_AXIOM :
                                                    taxonomyName.Equals(nameof(RDFOntologyDataMetadata.MemberList)) ? RDFVocabulary.SKOS.MEMBER_LIST :
                                                        null; //Unrecognized taxonomy predicates will not be handled
            if (taxonomyPredicate != null)
            {
                foreach (IGrouping<RDFOntologyResource, RDFOntologyTaxonomyEntry> tgroup in taxonomy.GroupBy(t => t.TaxonomySubject))
                {
                    //Build collection corresponding to the current subject of the given taxonomy
                    RDFCollection tgroupColl = new RDFCollection(RDFModelEnums.RDFItemTypes.Resource, taxonomy.AcceptDuplicates);
                    foreach (RDFOntologyTaxonomyEntry tgroupEntry in tgroup.ToList())
                        tgroupColl.AddItem((RDFResource)tgroupEntry.TaxonomyObject.Value);
                    result.AddCollection(tgroupColl);

                    //Attach collection with taxonomy-specific predicate
                    result.AddTriple(new RDFTriple((RDFResource)tgroup.Key.Value, taxonomyPredicate, tgroupColl.ReificationSubject));
                }
            }

            return result;
        }
        private static RDFGraph ReifyTripleTaxonomyToGraph(RDFOntologyTaxonomy taxonomy, RDFSemanticsEnums.RDFOntologyInferenceExportBehavior infexpBehavior)
        {
            RDFGraph result = new RDFGraph();

            foreach (RDFOntologyTaxonomyEntry te in taxonomy)
            {
                //Do not export semantic inferences
                if (infexpBehavior == RDFSemanticsEnums.RDFOntologyInferenceExportBehavior.None)
                {
                    if (te.InferenceType == RDFSemanticsEnums.RDFOntologyInferenceType.None)
                        result.AddTriple(te.ToRDFTriple());
                }

                //Export semantic inferences related only to ontology model
                else if (infexpBehavior == RDFSemanticsEnums.RDFOntologyInferenceExportBehavior.OnlyModel)
                {
                    if (taxonomy.Category == RDFSemanticsEnums.RDFOntologyTaxonomyCategory.Model
                            || taxonomy.Category == RDFSemanticsEnums.RDFOntologyTaxonomyCategory.Annotation)
                    {
                        result.AddTriple(te.ToRDFTriple());
                    }
                    else
                    {
                        if (te.InferenceType == RDFSemanticsEnums.RDFOntologyInferenceType.None)
                            result.AddTriple(te.ToRDFTriple());
                    }
                }

                //Export semantic inferences related only to ontology data
                else if (infexpBehavior == RDFSemanticsEnums.RDFOntologyInferenceExportBehavior.OnlyData)
                {
                    if (taxonomy.Category == RDFSemanticsEnums.RDFOntologyTaxonomyCategory.Data
                            || taxonomy.Category == RDFSemanticsEnums.RDFOntologyTaxonomyCategory.Annotation)
                    {
                        result.AddTriple(te.ToRDFTriple());
                    }
                    else
                    {
                        if (te.InferenceType == RDFSemanticsEnums.RDFOntologyInferenceType.None)
                            result.AddTriple(te.ToRDFTriple());
                    }
                }

                //Export semantic inferences related both to ontology model and data
                else
                    result.AddTriple(te.ToRDFTriple());
            }

            return result;
        }
        #endregion

        #endregion
    }
}