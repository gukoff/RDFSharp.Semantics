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
using System.Collections;
using System.Collections.Generic;
using System.Xml.Serialization;

namespace RDFSharp.Semantics.Extensions.SKOS
{
    /// <summary>
    /// SKOSConceptSchemeLoader is responsible for loading SKOS concept schemes from remote sources or alternative representations
    /// </summary>
    internal static class SKOSConceptSchemeLoader
    {
        #region Methods
        /// <summary>
        /// Gets a concept scheme representation of the given graph
        /// </summary>
        internal static SKOSConceptScheme FromRDFGraph(RDFGraph graph)
        {
            if (graph == null)
                throw new OWLSemanticsException("Cannot get concept scheme from RDFGraph because given \"graph\" parameter is null");

            //Get OWL ontology with SKOS extension points
            OWLOntology ontology = OWLOntologyLoader.FromRDFGraph(graph,
               classModelExtensionPoint: SKOSClassModelExtensionPoint,
               propertyModelExtensionPoint: SKOSPropertyModelExtensionPoint,
               dataExtensionPoint: SKOSDataExtensionPoint);

            //Build SKOS concept scheme from ontology
            SKOSConceptScheme conceptScheme = new SKOSConceptScheme(ontology.URI.ToString()) { Ontology = ontology };
            conceptScheme.Ontology.Data.DeclareIndividual(conceptScheme);
            conceptScheme.Ontology.Data.DeclareIndividualType(conceptScheme, RDFVocabulary.SKOS.CONCEPT_SCHEME);

            return conceptScheme;
        }
        #endregion

        #region Utilities
        /// <summary>
        /// Extends OWL class model loading with support for SKOS artifacts
        /// </summary>
        internal static void SKOSClassModelExtensionPoint(OWLOntology ontology, RDFGraph graph)
            => ontology.Model.ClassModel = SKOSConceptScheme.BuildSKOSClassModel();

        /// <summary>
        /// Extends OWL property model loading with support for SKOS artifacts
        /// </summary>
        internal static void SKOSPropertyModelExtensionPoint(OWLOntology ontology, RDFGraph graph)
            => ontology.Model.PropertyModel = SKOSConceptScheme.BuildSKOSPropertyModel();

        /// <summary>
        /// Extends OWL data loading with support for SKOS artifacts
        /// </summary>
        internal static void SKOSDataExtensionPoint(OWLOntology ontology, RDFGraph graph)
        {
            //skos:Collection
            foreach (RDFTriple typeCollection in graph[null, RDFVocabulary.RDF.TYPE, RDFVocabulary.SKOS.COLLECTION, null])
                foreach (RDFTriple memberRelation in graph[(RDFResource)typeCollection.Subject, RDFVocabulary.SKOS.MEMBER, null, null])
                {
                    //Also declare concepts of the collection
                    ontology.Data.DeclareIndividual((RDFResource)memberRelation.Object);
                    ontology.Data.DeclareIndividualType((RDFResource)memberRelation.Object, RDFVocabulary.SKOS.CONCEPT);
                    ontology.Data.DeclareObjectAssertion((RDFResource)typeCollection.Subject, RDFVocabulary.SKOS.MEMBER, (RDFResource)memberRelation.Object);
                }

            //skos:OrderedCollection
            foreach (RDFTriple typeOrderedCollection in graph[null, RDFVocabulary.RDF.TYPE, RDFVocabulary.SKOS.ORDERED_COLLECTION, null])
                foreach (RDFTriple memberListRelation in graph[(RDFResource)typeOrderedCollection.Subject, RDFVocabulary.SKOS.MEMBER_LIST, null, null])
                {
                    RDFCollection skosOrderedCollection = RDFModelUtilities.DeserializeCollectionFromGraph(graph, (RDFResource)memberListRelation.Object, RDFModelEnums.RDFTripleFlavors.SPO);
                    if (skosOrderedCollection.ItemsCount > 0)
                    {
                        //Also declare concepts of the ordered collection
                        foreach (RDFPatternMember skosConcept in skosOrderedCollection)
                        {
                            ontology.Data.DeclareIndividual((RDFResource)skosConcept);
                            ontology.Data.DeclareIndividualType((RDFResource)skosConcept, RDFVocabulary.SKOS.CONCEPT);
                        }
                        ontology.Data.ABoxGraph.AddCollection(skosOrderedCollection);
                        ontology.Data.DeclareObjectAssertion((RDFResource)typeOrderedCollection.Subject, RDFVocabulary.SKOS.MEMBER_LIST, skosOrderedCollection.ReificationSubject);
                    }
                }
        }
        #endregion
    }
}