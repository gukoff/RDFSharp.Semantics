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
using System.Linq;
using RDFSharp.Model;
using RDFSharp.Query;

namespace RDFSharp.Semantics
{
    /// <summary>
    /// RDFOntologyLoader is responsible for loading ontologies from remote sources or alternative representations
    /// </summary>
    internal static class RDFOntologyLoader
    {
        #region Methods
        /// <summary>
        /// Gets an ontology representation of the given graph
        /// </summary>
        internal static RDFOntology FromRDFGraph(RDFGraph graph, Action<RDFOntology,RDFGraph> classModelExtensionPoint=null, Action<RDFOntology,RDFGraph> propertyModelExtensionPoint=null, Action<RDFOntology,RDFGraph> dataExtensionPoint=null)
        {
            if (graph == null)
                throw new RDFSemanticsException("Cannot get ontology from RDFGraph because given \"graph\" parameter is null");

            RDFSemanticsEvents.RaiseSemanticsInfo(string.Format("Graph '{0}' is going to be parsed as Ontology...", graph.Context));
            LoadOntology(graph, out RDFOntology ontology);
            ontology.LoadModel(graph, classModelExtensionPoint, propertyModelExtensionPoint);
            ontology.LoadData(graph, dataExtensionPoint);
            RDFSemanticsEvents.RaiseSemanticsInfo(string.Format("Graph '{0}' has been parsed as Ontology", graph.Context));

            return ontology;
        }

        /// <summary>
        /// Gets the hashes of owl:AnnotationProperty declarations
        /// </summary>
        internal static HashSet<long> GetAnnotationPropertyHashes(this RDFGraph graph)
            => new HashSet<long>(graph[null, RDFVocabulary.RDF.TYPE, RDFVocabulary.OWL.ANNOTATION_PROPERTY, null]
                                    .Select(t => t.Subject.PatternMemberID));
        #endregion

        #region Utilities
        /// <summary>
        /// Parses the ontology URI and annotations
        /// </summary>
        private static void LoadOntology(RDFGraph graph, out RDFOntology ontology)
        {
            //Load ontology URI
            ontology = new RDFOntology(graph.Context.ToString());
            if (!graph.ContainsTriple(new RDFTriple(ontology, RDFVocabulary.RDF.TYPE, RDFVocabulary.OWL.ONTOLOGY)))
            {
                RDFTriple ontologyDeclaration = graph[null, RDFVocabulary.RDF.TYPE, RDFVocabulary.OWL.ONTOLOGY, null].FirstOrDefault();
                if (ontologyDeclaration != null)
                    ontology = new RDFOntology(ontologyDeclaration.Subject.ToString());
            }

            //Load ontology annotations
            HashSet<long> annotationProperties = GetAnnotationPropertyHashes(graph);
            annotationProperties.UnionWith(RDFSemanticsUtilities.StandardOntologyAnnotations);

            RDFGraph ontologyGraph = graph[ontology, null, null, null];
            foreach (RDFTriple ontologyAnnotation in ontologyGraph.Where(t => annotationProperties.Contains(t.Predicate.PatternMemberID)))
            {
                if (ontologyAnnotation.TripleFlavor == RDFModelEnums.RDFTripleFlavors.SPO)
                    ontology.Annotate((RDFResource)ontologyAnnotation.Predicate, (RDFResource)ontologyAnnotation.Object);
                else
                    ontology.Annotate((RDFResource)ontologyAnnotation.Predicate, (RDFLiteral)ontologyAnnotation.Object);
            }
        }
        #endregion
    }
}