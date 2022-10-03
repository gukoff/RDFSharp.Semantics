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

            //Get OWL ontology with SKOS extensions
            OWLOntology ontology = OWLOntologyLoader.FromRDFGraph(graph, dataExtensionPoint: SKOSDataExtensionPoint);

            //Wrap OWL ontology into SKOS concept scheme
            SKOSConceptScheme conceptScheme = new SKOSConceptScheme(ontology.URI.ToString()) { Ontology = ontology };

            return conceptScheme;
        }
        #endregion

        #region Utilities
        /// <summary>
        /// Extends OWL data loading with support for SKOS artifacts
        /// </summary>
        internal static void SKOSDataExtensionPoint(OWLOntology ontology, RDFGraph graph)
        {
            //skos:OrderedCollection
            foreach (RDFTriple typeOrderedCollection in graph[null, RDFVocabulary.RDF.TYPE, RDFVocabulary.SKOS.ORDERED_COLLECTION, null])
                foreach (RDFTriple memberListRelation in graph[(RDFResource)typeOrderedCollection.Subject, RDFVocabulary.SKOS.MEMBER_LIST, null, null])
                {
                    RDFCollection skosOrderedCollection = RDFModelUtilities.DeserializeCollectionFromGraph(graph, (RDFResource)memberListRelation.Object, RDFModelEnums.RDFTripleFlavors.SPO);
                    if (skosOrderedCollection.ItemsCount > 0)
                    {
                        //Also declare concepts of the ordred collection
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