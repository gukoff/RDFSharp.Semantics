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

namespace RDFSharp.Semantics.Extensions.SKOS.Test
{
    [TestClass]
    public class SKOSConceptSchemeLoaderTest
    {
        #region Tests
        [TestMethod]
        public void ShouldLoadFromGraph()
        {
            RDFGraph graph = new RDFGraph();

            //OWL knowledge
            graph.AddTriple(new RDFTriple(new RDFResource("ex:class1"), RDFVocabulary.RDF.TYPE, RDFVocabulary.OWL.CLASS));
            graph.AddTriple(new RDFTriple(new RDFResource("ex:class1"), RDFVocabulary.DC.DCTERMS.DESCRIPTION, new RDFPlainLiteral("this is a class")));
            graph.AddTriple(new RDFTriple(new RDFResource("ex:objprop1"), RDFVocabulary.RDF.TYPE, RDFVocabulary.OWL.OBJECT_PROPERTY));
            graph.AddTriple(new RDFTriple(new RDFResource("ex:objprop1"), RDFVocabulary.RDF.TYPE, RDFVocabulary.OWL.REFLEXIVE_PROPERTY));
            graph.AddTriple(new RDFTriple(new RDFResource("ex:objprop1"), RDFVocabulary.RDFS.COMMENT, new RDFPlainLiteral("this is an object property")));
            graph.AddTriple(new RDFTriple(new RDFResource("ex:indiv1"), RDFVocabulary.RDF.TYPE, new RDFResource("ex:class1")));
            graph.AddTriple(new RDFTriple(new RDFResource("ex:indiv1"), RDFVocabulary.DC.DESCRIPTION, new RDFPlainLiteral("this is an individual")));
            graph.AddTriple(new RDFTriple(new RDFResource("ex:indiv1"), new RDFResource("ex:objprop1"), new RDFResource("ex:indiv1")));

            //SKOS knowledge
            graph.AddTriple(new RDFTriple(new RDFResource("ex:conceptScheme"), RDFVocabulary.RDF.TYPE, RDFVocabulary.SKOS.CONCEPT_SCHEME));
            graph.AddTriple(new RDFTriple(new RDFResource("ex:conceptScheme"), RDFVocabulary.RDFS.COMMENT, new RDFPlainLiteral("This is a test concept scheme")));
            graph.AddTriple(new RDFTriple(new RDFResource("ex:concept1"), RDFVocabulary.RDF.TYPE, RDFVocabulary.SKOS.CONCEPT));
            graph.AddTriple(new RDFTriple(new RDFResource("ex:concept1"), RDFVocabulary.SKOS.IN_SCHEME, new RDFResource("ex:conceptScheme")));
            graph.AddTriple(new RDFTriple(new RDFResource("ex:concept1"), RDFVocabulary.RDFS.COMMENT, new RDFPlainLiteral("This is a test concept")));
            graph.AddTriple(new RDFTriple(new RDFResource("ex:concept1"), RDFVocabulary.SKOS.EXACT_MATCH, new RDFResource("ex:concept2")));
            graph.AddTriple(new RDFTriple(new RDFResource("ex:concept1"), RDFVocabulary.SKOS.SKOSXL.PREF_LABEL, new RDFPlainLiteral("concept1", "en-US")));
            graph.AddTriple(new RDFTriple(new RDFResource("ex:concept2"), RDFVocabulary.RDF.TYPE, RDFVocabulary.SKOS.CONCEPT));
            graph.AddTriple(new RDFTriple(new RDFResource("ex:concept2"), RDFVocabulary.SKOS.IN_SCHEME, new RDFResource("ex:conceptScheme")));
            graph.AddTriple(new RDFTriple(new RDFResource("ex:collection1"), RDFVocabulary.RDF.TYPE, RDFVocabulary.SKOS.COLLECTION));
            graph.AddTriple(new RDFTriple(new RDFResource("ex:collection1"), RDFVocabulary.SKOS.IN_SCHEME, new RDFResource("ex:conceptScheme")));
            graph.AddTriple(new RDFTriple(new RDFResource("ex:collection1"), RDFVocabulary.SKOS.MEMBER, new RDFResource("ex:concept1")));
            graph.AddTriple(new RDFTriple(new RDFResource("ex:collection1"), RDFVocabulary.SKOS.MEMBER, new RDFResource("ex:collection2")));
            graph.AddTriple(new RDFTriple(new RDFResource("ex:collection1"), RDFVocabulary.RDFS.COMMENT, new RDFPlainLiteral("This is a test collection")));
            graph.AddTriple(new RDFTriple(new RDFResource("ex:collection1"), RDFVocabulary.SKOS.PREF_LABEL, new RDFPlainLiteral("collection1")));
            graph.AddTriple(new RDFTriple(new RDFResource("ex:collection2"), RDFVocabulary.RDF.TYPE, RDFVocabulary.SKOS.COLLECTION));
            graph.AddTriple(new RDFTriple(new RDFResource("ex:collection2"), RDFVocabulary.SKOS.IN_SCHEME, new RDFResource("ex:conceptScheme")));
            graph.AddTriple(new RDFTriple(new RDFResource("ex:collection2"), RDFVocabulary.SKOS.MEMBER, new RDFResource("ex:concept2")));
            graph.AddTriple(new RDFTriple(new RDFResource("ex:collection2"), RDFVocabulary.SKOS.MEMBER, new RDFResource("ex:orderedCollection")));
            graph.AddTriple(new RDFTriple(new RDFResource("ex:orderedCollection"), RDFVocabulary.RDF.TYPE, RDFVocabulary.SKOS.ORDERED_COLLECTION));
            graph.AddTriple(new RDFTriple(new RDFResource("ex:orderedCollection"), RDFVocabulary.SKOS.IN_SCHEME, new RDFResource("ex:conceptScheme")));
            graph.AddTriple(new RDFTriple(new RDFResource("ex:orderedCollection"), RDFVocabulary.RDFS.COMMENT, new RDFPlainLiteral("This is a test ordered collection")));
            graph.AddTriple(new RDFTriple(new RDFResource("ex:orderedCollection"), RDFVocabulary.SKOS.SKOSXL.PREF_LABEL, new RDFResource("ex:label1")));
            graph.AddTriple(new RDFTriple(new RDFResource("ex:orderedCollection"), RDFVocabulary.SKOS.MEMBER_LIST, new RDFResource("bnode:ordcollRepresentative")));
            graph.AddTriple(new RDFTriple(new RDFResource("bnode:ordcollRepresentative"), RDFVocabulary.RDF.TYPE, RDFVocabulary.RDF.LIST));
            graph.AddTriple(new RDFTriple(new RDFResource("bnode:ordcollRepresentative"), RDFVocabulary.RDF.FIRST, new RDFResource("ex:concept3")));
            graph.AddTriple(new RDFTriple(new RDFResource("bnode:ordcollRepresentative"), RDFVocabulary.RDF.REST, RDFVocabulary.RDF.NIL));
            graph.AddTriple(new RDFTriple(new RDFResource("ex:label1"), RDFVocabulary.RDF.TYPE, RDFVocabulary.SKOS.SKOSXL.LABEL));
            graph.AddTriple(new RDFTriple(new RDFResource("ex:label1"), RDFVocabulary.SKOS.SKOSXL.LITERAL_FORM, new RDFPlainLiteral("label1")));

            //LOAD OWL+SKOS
            SKOSConceptScheme conceptScheme = SKOSConceptSchemeLoader.FromRDFGraph(graph);

            //Test persistence of OWL+SKOS knowledge
            Assert.IsNotNull(conceptScheme);
            Assert.IsNotNull(conceptScheme.Ontology);
            Assert.IsTrue(conceptScheme.Ontology.URI.Equals(RDFNamespaceRegister.DefaultNamespace.NamespaceUri));
            Assert.IsTrue(conceptScheme.Ontology.Model.ClassModel.ClassesCount == 9);
            Assert.IsTrue(conceptScheme.Ontology.Model.PropertyModel.PropertiesCount == 34);
            Assert.IsTrue(conceptScheme.Ontology.Data.IndividualsCount == 8);
        }
        #endregion
    }
}