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
using System.Collections.Generic;
using System.Linq;

namespace RDFSharp.Semantics.Test
{
    [TestClass]
    public class SKOSConceptSchemeTest
    {
        #region Tests
        [TestMethod]
        public void ShouldCreateConceptScheme()
        {
            SKOSConceptScheme conceptScheme = new SKOSConceptScheme("ex:conceptScheme");

            Assert.IsNotNull(conceptScheme);
            Assert.IsNotNull(conceptScheme.Ontology);

            //Test initialization of SKOS knowledge
            Assert.IsTrue(conceptScheme.Ontology.URI.Equals(conceptScheme.URI));
            Assert.IsTrue(conceptScheme.Ontology.Model.ClassModel.ClassesCount == 8);
            Assert.IsTrue(conceptScheme.Ontology.Model.PropertyModel.PropertiesCount == 33);
            Assert.IsTrue(conceptScheme.Ontology.Data.IndividualsCount == 1);

            //Test counters and enumerators
            Assert.IsTrue(conceptScheme.ConceptsCount == 0);
            int i1 = 0;
            IEnumerator<RDFResource> conceptsEnumerator = conceptScheme.ConceptsEnumerator;
            while (conceptsEnumerator.MoveNext()) i1++;
            Assert.IsTrue(i1 == 0);

            int i2 = 0;
            foreach (RDFResource skosConcept in conceptScheme) i2++;
            Assert.IsTrue(i2 == 0);

            Assert.IsTrue(conceptScheme.CollectionsCount == 0);
            int j = 0;
            IEnumerator<RDFResource> collectionsEnumerator = conceptScheme.CollectionsEnumerator;
            while (collectionsEnumerator.MoveNext()) j++;
            Assert.IsTrue(j == 0);
            
            Assert.IsTrue(conceptScheme.OrderedCollectionsCount == 0);
            int k = 0;
            IEnumerator<RDFResource> orderedCollectionsEnumerator = conceptScheme.OrderedCollectionsEnumerator;
            while (orderedCollectionsEnumerator.MoveNext()) k++;
            Assert.IsTrue(k == 0);
            
            Assert.IsTrue(conceptScheme.LabelsCount == 0);
            int l = 0;
            IEnumerator<RDFResource> labelsEnumerator = conceptScheme.LabelsEnumerator;
            while (labelsEnumerator.MoveNext()) l++;
            Assert.IsTrue(l == 0);
        }

        [TestMethod]
        public void ShouldDeclareConcept()
        {
            SKOSConceptScheme conceptScheme = new SKOSConceptScheme("ex:conceptScheme");
            conceptScheme.DeclareConcept(new RDFResource("ex:concept"));

            //Test evolution of SKOS knowledge
            Assert.IsTrue(conceptScheme.Ontology.URI.Equals(conceptScheme.URI));
            Assert.IsTrue(conceptScheme.Ontology.Model.ClassModel.ClassesCount == 8);
            Assert.IsTrue(conceptScheme.Ontology.Model.PropertyModel.PropertiesCount == 33);
            Assert.IsTrue(conceptScheme.Ontology.Data.IndividualsCount == 2);
            Assert.IsTrue(conceptScheme.Ontology.Data.CheckHasIndividual(new RDFResource("ex:concept")));
            Assert.IsTrue(conceptScheme.Ontology.Data.CheckIsIndividualOf(conceptScheme.Ontology.Model, new RDFResource("ex:concept"), RDFVocabulary.SKOS.CONCEPT));
            Assert.IsTrue(conceptScheme.Ontology.Data.CheckHasObjectAssertion(new RDFResource("ex:concept"), RDFVocabulary.SKOS.IN_SCHEME, new RDFResource("ex:conceptScheme")));

            //Test counters and enumerators
            Assert.IsTrue(conceptScheme.ConceptsCount == 1);
            int i1 = 0;
            IEnumerator<RDFResource> conceptsEnumerator = conceptScheme.ConceptsEnumerator;
            while (conceptsEnumerator.MoveNext()) i1++;
            Assert.IsTrue(i1 == 1);

            int i2 = 0;
            foreach (RDFResource skosConcept in conceptScheme) i2++;
            Assert.IsTrue(i2 == 1);
        }

        [TestMethod]
        public void ShouldThrowExceptionOnDeclaringConceptBecauseNull()
            => Assert.ThrowsException<OWLSemanticsException>(() => new SKOSConceptScheme("ex:conceptScheme").DeclareConcept(null));

        [TestMethod]
        public void ShouldDeclareCollection()
        {
            SKOSConceptScheme conceptScheme = new SKOSConceptScheme("ex:conceptScheme");
            conceptScheme.DeclareCollection(new RDFResource("ex:collection"), new List<RDFResource>()
                { new RDFResource("ex:concept1"), new RDFResource("ex:concept2") });

            //Test evolution of SKOS knowledge
            Assert.IsTrue(conceptScheme.Ontology.URI.Equals(conceptScheme.URI));
            Assert.IsTrue(conceptScheme.Ontology.Model.ClassModel.ClassesCount == 8);
            Assert.IsTrue(conceptScheme.Ontology.Model.PropertyModel.PropertiesCount == 33);
            Assert.IsTrue(conceptScheme.Ontology.Data.IndividualsCount == 4);
            Assert.IsTrue(conceptScheme.Ontology.Data.CheckHasIndividual(new RDFResource("ex:collection")));
            Assert.IsTrue(conceptScheme.Ontology.Data.CheckIsIndividualOf(conceptScheme.Ontology.Model, new RDFResource("ex:collection"), RDFVocabulary.SKOS.COLLECTION));
            Assert.IsTrue(conceptScheme.Ontology.Data.CheckHasObjectAssertion(new RDFResource("ex:collection"), RDFVocabulary.SKOS.IN_SCHEME, new RDFResource("ex:conceptScheme")));
            Assert.IsTrue(conceptScheme.Ontology.Data.CheckHasIndividual(new RDFResource("ex:concept1")));
            Assert.IsTrue(conceptScheme.Ontology.Data.CheckIsIndividualOf(conceptScheme.Ontology.Model, new RDFResource("ex:concept1"), RDFVocabulary.SKOS.CONCEPT));
            Assert.IsTrue(conceptScheme.Ontology.Data.CheckHasObjectAssertion(new RDFResource("ex:collection"), RDFVocabulary.SKOS.MEMBER, new RDFResource("ex:concept1")));
            Assert.IsTrue(conceptScheme.Ontology.Data.CheckHasIndividual(new RDFResource("ex:concept2")));
            Assert.IsTrue(conceptScheme.Ontology.Data.CheckIsIndividualOf(conceptScheme.Ontology.Model, new RDFResource("ex:concept2"), RDFVocabulary.SKOS.CONCEPT));
            Assert.IsTrue(conceptScheme.Ontology.Data.CheckHasObjectAssertion(new RDFResource("ex:collection"), RDFVocabulary.SKOS.MEMBER, new RDFResource("ex:concept2")));
            
            //Test counters and enumerators
            Assert.IsTrue(conceptScheme.ConceptsCount == 2);
            int i1 = 0;
            IEnumerator<RDFResource> conceptsEnumerator = conceptScheme.ConceptsEnumerator;
            while (conceptsEnumerator.MoveNext()) i1++;
            Assert.IsTrue(i1 == 2);

            int i2 = 0;
            foreach (RDFResource skosConcept in conceptScheme) i2++;
            Assert.IsTrue(i2 == 2);

            Assert.IsTrue(conceptScheme.CollectionsCount == 1);
            int j1 = 0;
            IEnumerator<RDFResource> collectionsEnumerator = conceptScheme.CollectionsEnumerator;
            while (collectionsEnumerator.MoveNext()) j1++;
            Assert.IsTrue(j1 == 1);
        }

        [TestMethod]
        public void ShouldThrowExceptionOnDeclaringCollectionBecauseNull()
            => Assert.ThrowsException<OWLSemanticsException>(() => new SKOSConceptScheme("ex:conceptScheme").DeclareCollection(null, new List<RDFResource>() { new RDFResource("ex:concept") }));

        [TestMethod]
        public void ShouldThrowExceptionOnDeclaringCollectionBecauseNullList()
            => Assert.ThrowsException<OWLSemanticsException>(() => new SKOSConceptScheme("ex:conceptScheme").DeclareCollection(new RDFResource("ex:collection"), null));

        [TestMethod]
        public void ShouldThrowExceptionOnDeclaringCollectionBecauseEmptyList()
            => Assert.ThrowsException<OWLSemanticsException>(() => new SKOSConceptScheme("ex:conceptScheme").DeclareCollection(new RDFResource("ex:collection"), new List<RDFResource>()));

        [TestMethod]
        public void ShouldDeclareOrderedCollection()
        {
            SKOSConceptScheme conceptScheme = new SKOSConceptScheme("ex:conceptScheme");
            conceptScheme.DeclareOrderedCollection(new RDFResource("ex:orderedCollection"), new List<RDFResource>()
                { new RDFResource("ex:concept1"), new RDFResource("ex:concept2") });

            //Test evolution of SKOS knowledge
            Assert.IsTrue(conceptScheme.Ontology.URI.Equals(conceptScheme.URI));
            Assert.IsTrue(conceptScheme.Ontology.Model.ClassModel.ClassesCount == 8);
            Assert.IsTrue(conceptScheme.Ontology.Model.PropertyModel.PropertiesCount == 33);
            Assert.IsTrue(conceptScheme.Ontology.Data.IndividualsCount == 4);
            Assert.IsTrue(conceptScheme.Ontology.Data.CheckHasIndividual(new RDFResource("ex:orderedCollection")));
            Assert.IsTrue(conceptScheme.Ontology.Data.CheckIsIndividualOf(conceptScheme.Ontology.Model, new RDFResource("ex:orderedCollection"), RDFVocabulary.SKOS.ORDERED_COLLECTION));
            Assert.IsTrue(conceptScheme.Ontology.Data.CheckHasObjectAssertion(new RDFResource("ex:orderedCollection"), RDFVocabulary.SKOS.IN_SCHEME, new RDFResource("ex:conceptScheme")));
            Assert.IsTrue(conceptScheme.Ontology.Data.CheckHasIndividual(new RDFResource("ex:concept1")));
            Assert.IsTrue(conceptScheme.Ontology.Data.CheckIsIndividualOf(conceptScheme.Ontology.Model, new RDFResource("ex:concept1"), RDFVocabulary.SKOS.CONCEPT));
            Assert.IsTrue(conceptScheme.Ontology.Data.CheckHasIndividual(new RDFResource("ex:concept2")));
            Assert.IsTrue(conceptScheme.Ontology.Data.CheckIsIndividualOf(conceptScheme.Ontology.Model, new RDFResource("ex:concept2"), RDFVocabulary.SKOS.CONCEPT));
            Assert.IsTrue(conceptScheme.Ontology.Data.ABoxGraph[new RDFResource("ex:orderedCollection"), RDFVocabulary.SKOS.MEMBER_LIST, null, null].Any());
            Assert.IsTrue(conceptScheme.Ontology.Data.ABoxGraph[null, RDFVocabulary.RDF.FIRST, new RDFResource("ex:concept1"), null].Any());
            Assert.IsTrue(conceptScheme.Ontology.Data.ABoxGraph[null, RDFVocabulary.RDF.FIRST, new RDFResource("ex:concept2"), null].Any());

            //Test counters and enumerators
            Assert.IsTrue(conceptScheme.ConceptsCount == 2);
            int i1 = 0;
            IEnumerator<RDFResource> conceptsEnumerator = conceptScheme.ConceptsEnumerator;
            while (conceptsEnumerator.MoveNext()) i1++;
            Assert.IsTrue(i1 == 2);

            int i2 = 0;
            foreach (RDFResource skosConcept in conceptScheme) i2++;
            Assert.IsTrue(i2 == 2);

            Assert.IsTrue(conceptScheme.OrderedCollectionsCount == 1);
            int j1 = 0;
            IEnumerator<RDFResource> orderedCollectionsEnumerator = conceptScheme.OrderedCollectionsEnumerator;
            while (orderedCollectionsEnumerator.MoveNext()) j1++;
            Assert.IsTrue(j1 == 1);
        }

        [TestMethod]
        public void ShouldThrowExceptionOnDeclaringOrderedCollectionBecauseNull()
            => Assert.ThrowsException<OWLSemanticsException>(() => new SKOSConceptScheme("ex:conceptScheme").DeclareOrderedCollection(null, new List<RDFResource>() { new RDFResource("ex:concept") }));

        [TestMethod]
        public void ShouldThrowExceptionOnDeclaringOrderedCollectionBecauseNullList()
            => Assert.ThrowsException<OWLSemanticsException>(() => new SKOSConceptScheme("ex:conceptScheme").DeclareOrderedCollection(new RDFResource("ex:orderedCollection"), null));

        [TestMethod]
        public void ShouldThrowExceptionOnDeclaringOrderedCollectionBecauseEmptyList()
            => Assert.ThrowsException<OWLSemanticsException>(() => new SKOSConceptScheme("ex:conceptScheme").DeclareOrderedCollection(new RDFResource("ex:orderedCollection"), new List<RDFResource>()));

        [TestMethod]
        public void ShouldDeclareLabel()
        {
            SKOSConceptScheme conceptScheme = new SKOSConceptScheme("ex:conceptScheme");
            conceptScheme.DeclareLabel(new RDFResource("ex:label"));

            //Test evolution of SKOS knowledge
            Assert.IsTrue(conceptScheme.Ontology.URI.Equals(conceptScheme.URI));
            Assert.IsTrue(conceptScheme.Ontology.Model.ClassModel.ClassesCount == 8);
            Assert.IsTrue(conceptScheme.Ontology.Model.PropertyModel.PropertiesCount == 33);
            Assert.IsTrue(conceptScheme.Ontology.Data.IndividualsCount == 2);
            Assert.IsTrue(conceptScheme.Ontology.Data.CheckHasIndividual(new RDFResource("ex:label")));
            Assert.IsTrue(conceptScheme.Ontology.Data.CheckIsIndividualOf(conceptScheme.Ontology.Model, new RDFResource("ex:label"), RDFVocabulary.SKOS.SKOSXL.LABEL));
            Assert.IsTrue(conceptScheme.Ontology.Data.CheckHasObjectAssertion(new RDFResource("ex:label"), RDFVocabulary.SKOS.IN_SCHEME, new RDFResource("ex:conceptScheme")));

            //Test counters and enumerators
            Assert.IsTrue(conceptScheme.LabelsCount == 1);
            int i1 = 0;
            IEnumerator<RDFResource> labelsEnumerator = conceptScheme.LabelsEnumerator;
            while (labelsEnumerator.MoveNext()) i1++;
            Assert.IsTrue(i1 == 1);
        }

        [TestMethod]
        public void ShouldThrowExceptionOnDeclaringLabelBecauseNull()
            => Assert.ThrowsException<OWLSemanticsException>(() => new SKOSConceptScheme("ex:conceptScheme").DeclareLabel(null));
        
        //ANNOTATIONS


        #endregion
    }
}