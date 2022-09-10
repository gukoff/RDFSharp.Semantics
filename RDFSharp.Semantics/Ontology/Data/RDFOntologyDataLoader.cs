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

using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using RDFSharp.Model;
using RDFSharp.Query;

namespace RDFSharp.Semantics
{
    /// <summary>
    /// RDFOntologyDataLoader is responsible for loading ontology datas from remote sources or alternative representations
    /// </summary>
    internal static class RDFOntologyDataLoader
    {
        #region Methods
        /// <summary>
        /// Gets an ontology data representation of the given graph
        /// </summary>
        internal static void LoadData(this RDFOntology ontology, RDFGraph graph)
        {
            if (graph == null)
                throw new RDFSemanticsException("Cannot get ontology data from RDFGraph because given \"graph\" parameter is null");

            RDFSemanticsEvents.RaiseSemanticsInfo(string.Format("Graph '{0}' is going to be parsed as Data...", graph.Context));

            #region Declarations
            HashSet<long> annotationProperties = graph.GetAnnotationPropertyHashes();
            annotationProperties.UnionWith(RDFSemanticsUtilities.StandardResourceAnnotations);

            //Individuals (instance of owl:Individual)
            foreach (RDFResource owlIndividual in GetIndividualDeclarations(graph))
                ontology.Data.DeclareIndividual(owlIndividual);

            //Individuals (instance of owl:Class)
            foreach (RDFResource owlClass in ontology.Model.ClassModel.Where(cls => ontology.Model.ClassModel.CheckHasSimpleClass(cls)))
                foreach (RDFTriple type in graph[null, RDFVocabulary.RDF.TYPE, owlClass, null])
                    ontology.Data.DeclareIndividualType((RDFResource)type.Subject, owlClass);
            #endregion

            #region Taxonomies
            foreach (RDFResource owlIndividual in ontology.Data)
            {
                //Annotations
                foreach (RDFTriple individualAnnotation in graph[owlIndividual, null, null, null].Where(t => annotationProperties.Contains(t.Predicate.PatternMemberID)))
                {
                    if (individualAnnotation.TripleFlavor == RDFModelEnums.RDFTripleFlavors.SPO)
                        ontology.Model.ClassModel.AnnotateClass(owlIndividual, (RDFResource)individualAnnotation.Predicate, (RDFResource)individualAnnotation.Object);
                    else
                        ontology.Model.ClassModel.AnnotateClass(owlIndividual, (RDFResource)individualAnnotation.Predicate, (RDFLiteral)individualAnnotation.Object);
                }

                //Relations
                foreach (RDFTriple sameAsRelation in graph[owlIndividual, RDFVocabulary.OWL.SAME_AS, null, null])
                    ontology.Data.DeclareSameIndividuals(owlIndividual, (RDFResource)sameAsRelation.Object);
                foreach (RDFTriple differentFromRelation in graph[owlIndividual, RDFVocabulary.OWL.DIFFERENT_FROM, null, null])
                    ontology.Data.DeclareDifferentIndividuals(owlIndividual, (RDFResource)differentFromRelation.Object);
                foreach (RDFTriple negativeObjectAssertion in GetNegativeObjectAssertions(graph, owlIndividual))
                    ontology.Data.DeclareNegativeObjectAssertion(owlIndividual, (RDFResource)negativeObjectAssertion.Predicate, (RDFResource)negativeObjectAssertion.Object);
                foreach (RDFTriple negativeDatatypeAssertion in GetNegativeDatatypeAssertions(graph, owlIndividual))
                    ontology.Data.DeclareNegativeDatatypeAssertion(owlIndividual, (RDFResource)negativeDatatypeAssertion.Predicate, (RDFLiteral)negativeDatatypeAssertion.Object);
                foreach (RDFTriple objectAssertion in GetObjectAssertions(ontology, graph, owlIndividual))
                    ontology.Data.DeclareObjectAssertion(owlIndividual, (RDFResource)objectAssertion.Predicate, (RDFResource)objectAssertion.Object);
                foreach (RDFTriple datatypeAssertion in GetDatatypeAssertions(ontology, graph, owlIndividual))
                    ontology.Data.DeclareDatatypeAssertion(owlIndividual, (RDFResource)datatypeAssertion.Predicate, (RDFLiteral)datatypeAssertion.Object);
            }

            //owl:AllDifferent [OWL2]
            IEnumerator<RDFResource> allDifferent = graph[null, RDFVocabulary.RDF.TYPE, RDFVocabulary.OWL.ALL_DIFFERENT, null]
                                                      .Select(t => (RDFResource)t.Subject)
                                                      .GetEnumerator();
            while (allDifferent.MoveNext())
                foreach (RDFTriple allDifferentMembers in graph[allDifferent.Current, RDFVocabulary.OWL.DISTINCT_MEMBERS, null, null])
                {
                    List<RDFResource> differentIndividuals = new List<RDFResource>();
                    RDFCollection differentIndividualsCollection = RDFModelUtilities.DeserializeCollectionFromGraph(graph, (RDFResource)allDifferentMembers.Object, RDFModelEnums.RDFTripleFlavors.SPO);
                    foreach (RDFPatternMember differentIndividual in differentIndividualsCollection)
                        differentIndividuals.Add((RDFResource)differentIndividual);
                    ontology.Data.DeclareAllDifferentIndividuals(differentIndividuals);
                }
            #endregion

            RDFSemanticsEvents.RaiseSemanticsInfo(string.Format("Graph '{0}' has been parsed as Data", graph.Context));
        }
        #endregion

        #region Utilities
        /// <summary>
        /// Gets the owl:NamedIndividual declarations
        /// </summary>
        private static HashSet<RDFResource> GetIndividualDeclarations(RDFGraph graph)
            => new HashSet<RDFResource>(graph[null, RDFVocabulary.RDF.TYPE, RDFVocabulary.OWL.NAMED_INDIVIDUAL, null]
                                           .Select(t => t.Subject)
                                           .OfType<RDFResource>());

        /// <summary>
        /// Gets the negative object assertions of the given owl:Individual (same algorythm of DataLens)
        /// </summary>
        private static RDFGraph GetNegativeObjectAssertions(RDFGraph graph, RDFResource owlIndividual)
        {
            //Perform a SPARQL query to fetch all negative object assertions of the given owl:Individual
            RDFSelectQuery negativeObjectAssertionQuery = new RDFSelectQuery()
                .AddPatternGroup(new RDFPatternGroup()
                    .AddPattern(new RDFPattern(new RDFVariable("?NASN_REPRESENTATIVE"), RDFVocabulary.RDF.TYPE, RDFVocabulary.OWL.NEGATIVE_PROPERTY_ASSERTION))
                    .AddPattern(new RDFPattern(new RDFVariable("?NASN_REPRESENTATIVE"), RDFVocabulary.OWL.SOURCE_INDIVIDUAL, owlIndividual))
                    .AddPattern(new RDFPattern(new RDFVariable("?NASN_REPRESENTATIVE"), RDFVocabulary.OWL.ASSERTION_PROPERTY, new RDFVariable("?NASN_PROPERTY")))
                    .AddPattern(new RDFPattern(new RDFVariable("?NASN_REPRESENTATIVE"), RDFVocabulary.OWL.TARGET_INDIVIDUAL, new RDFVariable("?NASN_TARGET")))
                    .AddFilter(new RDFIsUriFilter(new RDFVariable("?NASN_PROPERTY")))
                    .AddFilter(new RDFIsUriFilter(new RDFVariable("?NASN_TARGET"))))
                .AddProjectionVariable(new RDFVariable("?NASN_PROPERTY"))
                .AddProjectionVariable(new RDFVariable("?NASN_TARGET"));
            RDFSelectQueryResult negativeObjectAssertionQueryResult = negativeObjectAssertionQuery.ApplyToGraph(graph);

            //Merge them into the results graph
            RDFGraph negativeObjectAssertionsGraph = new RDFGraph();
            foreach (DataRow negativeObjectAssertion in negativeObjectAssertionQueryResult.SelectResults.Rows)
                negativeObjectAssertionsGraph.AddTriple(new RDFTriple(owlIndividual, new RDFResource(negativeObjectAssertion["?NASN_PROPERTY"].ToString()), new RDFResource(negativeObjectAssertion["?NASN_TARGET"].ToString())));

            return negativeObjectAssertionsGraph;
        }

        /// <summary>
        /// Gets the negative data assertions of the given owl:Individual (same algorythm of DataLens)
        /// </summary>
        private static RDFGraph GetNegativeDatatypeAssertions(RDFGraph graph, RDFResource owlIndividual)
        {
            //Perform a SPARQL query to fetch all negative datatype assertions of the given owl:Individual
            RDFSelectQuery negativeDatatypeAssertionQuery = new RDFSelectQuery()
                .AddPatternGroup(new RDFPatternGroup()
                    .AddPattern(new RDFPattern(new RDFVariable("?NASN_REPRESENTATIVE"), RDFVocabulary.RDF.TYPE, RDFVocabulary.OWL.NEGATIVE_PROPERTY_ASSERTION))
                    .AddPattern(new RDFPattern(new RDFVariable("?NASN_REPRESENTATIVE"), RDFVocabulary.OWL.SOURCE_INDIVIDUAL, owlIndividual))
                    .AddPattern(new RDFPattern(new RDFVariable("?NASN_REPRESENTATIVE"), RDFVocabulary.OWL.ASSERTION_PROPERTY, new RDFVariable("?NASN_PROPERTY")))
                    .AddPattern(new RDFPattern(new RDFVariable("?NASN_REPRESENTATIVE"), RDFVocabulary.OWL.TARGET_VALUE, new RDFVariable("?NASN_TARGET")))
                    .AddFilter(new RDFIsUriFilter(new RDFVariable("?NASN_PROPERTY")))
                    .AddFilter(new RDFIsLiteralFilter(new RDFVariable("?NASN_TARGET"))))
                .AddProjectionVariable(new RDFVariable("?NASN_PROPERTY"))
                .AddProjectionVariable(new RDFVariable("?NASN_TARGET"));
            RDFSelectQueryResult negativeDatatypeAssertionQueryResult = negativeDatatypeAssertionQuery.ApplyToGraph(graph);

            //Merge them into the results graph
            RDFGraph negativeDatatypeAssertionsGraph = new RDFGraph();
            foreach (DataRow negativeDatatypeAssertion in negativeDatatypeAssertionQueryResult.SelectResults.Rows)
                negativeDatatypeAssertionsGraph.AddTriple(new RDFTriple(owlIndividual, new RDFResource(negativeDatatypeAssertion["?NASN_PROPERTY"].ToString()), (RDFLiteral)RDFQueryUtilities.ParseRDFPatternMember(negativeDatatypeAssertion["?NASN_TARGET"].ToString())));

            return negativeDatatypeAssertionsGraph;
        }

        /// <summary>
        /// Gets the object relations of the given owl:Individual
        /// </summary>
        private static RDFGraph GetObjectAssertions(RDFOntology ontology, RDFGraph graph, RDFResource owlIndividual)
        {
            RDFGraph objectAssertions = new RDFGraph();

            foreach (RDFResource owlProperty in ontology.Model.PropertyModel.Where(objProp => ontology.Model.PropertyModel.CheckHasObjectProperty(objProp)))
                foreach (RDFTriple objectAssertion in graph[owlIndividual, owlProperty, null, null].Where(asn => asn.TripleFlavor == RDFModelEnums.RDFTripleFlavors.SPO))
                    objectAssertions.AddTriple(objectAssertion);

            return objectAssertions;
        }

        /// <summary>
        /// Gets the data relations of the given owl:Individual
        /// </summary>
        private static RDFGraph GetDatatypeAssertions(RDFOntology ontology, RDFGraph graph, RDFResource owlIndividual)
        {
            RDFGraph datatypeAssertions = new RDFGraph();

            foreach (RDFResource owlProperty in ontology.Model.PropertyModel.Where(dtProp => ontology.Model.PropertyModel.CheckHasDatatypeProperty(dtProp)))
                foreach (RDFTriple datatypeAssertion in graph[owlIndividual, owlProperty, null, null].Where(asn => asn.TripleFlavor == RDFModelEnums.RDFTripleFlavors.SPL))
                    datatypeAssertions.AddTriple(datatypeAssertion);

            return datatypeAssertions;
        }
        #endregion
    }
}