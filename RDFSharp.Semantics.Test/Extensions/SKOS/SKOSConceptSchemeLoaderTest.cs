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

using Microsoft.VisualStudio.TestTools.UnitTesting;
using RDFSharp.Model;
using RDFSharp.Semantics.Extensions.SKOS;

namespace RDFSharp.Semantics.Test
{
    [TestClass]
    public class SKOSConceptSchemeLoaderTest
    {
        #region Initialize
        private SKOSConceptScheme ConceptScheme { get; set; }

        [TestInitialize]
        public void Initialize()
        {
            RDFGraph graph = new RDFGraph();

            //Declarations (Data)
            graph.AddTriple(new RDFTriple(new RDFResource("ex:conceptscheme"), RDFVocabulary.RDF.TYPE, RDFVocabulary.SKOS.CONCEPT_SCHEME));
            graph.AddTriple(new RDFTriple(new RDFResource("ex:concept1"), RDFVocabulary.RDF.TYPE, RDFVocabulary.SKOS.CONCEPT));
            graph.AddTriple(new RDFTriple(new RDFResource("ex:concept1"), RDFVocabulary.SKOS.IN_SCHEME, new RDFResource("ex:conceptscheme")));
            graph.AddTriple(new RDFTriple(new RDFResource("ex:concept2"), RDFVocabulary.RDF.TYPE, RDFVocabulary.SKOS.CONCEPT));
            graph.AddTriple(new RDFTriple(new RDFResource("ex:concept2"), RDFVocabulary.SKOS.IN_SCHEME, new RDFResource("ex:conceptscheme")));
            graph.AddTriple(new RDFTriple(new RDFResource("ex:concept3"), RDFVocabulary.RDF.TYPE, RDFVocabulary.SKOS.CONCEPT));
            graph.AddTriple(new RDFTriple(new RDFResource("ex:concept3"), RDFVocabulary.SKOS.IN_SCHEME, new RDFResource("ex:conceptscheme")));
            graph.AddTriple(new RDFTriple(new RDFResource("ex:concept4"), RDFVocabulary.RDF.TYPE, RDFVocabulary.SKOS.CONCEPT));
            graph.AddTriple(new RDFTriple(new RDFResource("ex:concept4"), RDFVocabulary.SKOS.IN_SCHEME, new RDFResource("ex:conceptscheme")));
            graph.AddTriple(new RDFTriple(new RDFResource("ex:collection"), RDFVocabulary.RDF.TYPE, RDFVocabulary.SKOS.COLLECTION));
            graph.AddTriple(new RDFTriple(new RDFResource("ex:collection"), RDFVocabulary.SKOS.MEMBER, new RDFResource("ex:concept1")));
            graph.AddTriple(new RDFTriple(new RDFResource("ex:collection"), RDFVocabulary.SKOS.MEMBER, new RDFResource("ex:concept2")));
            graph.AddTriple(new RDFTriple(new RDFResource("ex:collection"), RDFVocabulary.SKOS.MEMBER, new RDFResource("ex:concept3")));
            graph.AddTriple(new RDFTriple(new RDFResource("ex:collection"), RDFVocabulary.SKOS.IN_SCHEME, new RDFResource("ex:conceptscheme")));
            graph.AddTriple(new RDFTriple(new RDFResource("ex:orderedcollection"), RDFVocabulary.RDF.TYPE, RDFVocabulary.SKOS.ORDERED_COLLECTION));
            graph.AddTriple(new RDFTriple(new RDFResource("ex:orderedcollection"), RDFVocabulary.SKOS.MEMBER_LIST, new RDFResource("bnode:orderedcollectionMembers")));
            graph.AddTriple(new RDFTriple(new RDFResource("bnode:orderedcollectionMembers"), RDFVocabulary.RDF.TYPE, RDFVocabulary.RDF.LIST));
            graph.AddTriple(new RDFTriple(new RDFResource("bnode:orderedcollectionMembers"), RDFVocabulary.RDF.FIRST, new RDFResource("ex:concept3")));
            graph.AddTriple(new RDFTriple(new RDFResource("bnode:orderedcollectionMembers"), RDFVocabulary.RDF.REST, new RDFResource("bnode:orderedcollectionMembers2")));
            graph.AddTriple(new RDFTriple(new RDFResource("bnode:orderedcollectionMembers2"), RDFVocabulary.RDF.TYPE, RDFVocabulary.RDF.LIST));
            graph.AddTriple(new RDFTriple(new RDFResource("bnode:orderedcollectionMembers2"), RDFVocabulary.RDF.FIRST, new RDFResource("ex:concept4")));
            graph.AddTriple(new RDFTriple(new RDFResource("bnode:orderedcollectionMembers2"), RDFVocabulary.RDF.REST, RDFVocabulary.RDF.NIL));
            graph.AddTriple(new RDFTriple(new RDFResource("ex:orderedcollection"), RDFVocabulary.SKOS.IN_SCHEME, new RDFResource("ex:conceptscheme")));
            graph.AddTriple(new RDFTriple(new RDFResource("ex:label1"), RDFVocabulary.RDF.TYPE, RDFVocabulary.SKOS.SKOSXL.LABEL));
            graph.AddTriple(new RDFTriple(new RDFResource("ex:label1"), RDFVocabulary.SKOS.IN_SCHEME, new RDFResource("ex:conceptscheme")));
            graph.AddTriple(new RDFTriple(new RDFResource("ex:label2"), RDFVocabulary.RDF.TYPE, RDFVocabulary.SKOS.SKOSXL.LABEL));
            graph.AddTriple(new RDFTriple(new RDFResource("ex:label2"), RDFVocabulary.SKOS.IN_SCHEME, new RDFResource("ex:conceptscheme")));

            //Load
            ConceptScheme = SKOSConceptSchemeLoader.FromRDFGraph(graph);

            Assert.IsNotNull(ConceptScheme);
        }
        #endregion

        #region Tests
        [TestMethod]
        public void ShouldLoadConceptScheme()
        {

        }
        #endregion
    }
}