﻿/*
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
using System.Collections.Generic;
using System.Linq;

namespace RDFSharp.Semantics.Extensions.SKOS.Test
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

        [TestMethod]
        public void ShouldLiteralAnnotate()
        {
            SKOSConceptScheme conceptScheme = new SKOSConceptScheme("ex:conceptScheme");
            conceptScheme.Annotate(RDFVocabulary.RDFS.COMMENT, new RDFPlainLiteral("This is a skos:conceptScheme!"));

            //Test evolution of SKOS knowledge
            Assert.IsTrue(conceptScheme.Ontology.URI.Equals(conceptScheme.URI));
            Assert.IsTrue(conceptScheme.Ontology.Model.ClassModel.ClassesCount == 8);
            Assert.IsTrue(conceptScheme.Ontology.Model.PropertyModel.PropertiesCount == 33);
            Assert.IsTrue(conceptScheme.Ontology.Data.IndividualsCount == 1);
            Assert.IsTrue(conceptScheme.Ontology.Data.CheckHasAnnotation(new RDFResource("ex:conceptScheme"), RDFVocabulary.RDFS.COMMENT, new RDFPlainLiteral("This is a skos:conceptScheme!")));
        }

        [TestMethod]
        public void ShouldThrowExceptionOnLiteralAnnotatingBecauseNullProperty()
            => Assert.ThrowsException<OWLSemanticsException>(() => new SKOSConceptScheme("ex:conceptScheme").Annotate(null, new RDFPlainLiteral("This is a skos:conceptScheme!")));

        [TestMethod]
        public void ShouldThrowExceptionOnLiteralAnnotatingBecauseBlankProperty()
            => Assert.ThrowsException<OWLSemanticsException>(() => new SKOSConceptScheme("ex:conceptScheme").Annotate(new RDFResource(), new RDFPlainLiteral("This is a skos:conceptScheme!")));

        [TestMethod]
        public void ShouldThrowExceptionOnLiteralAnnotatingBecauseNullValue()
            => Assert.ThrowsException<OWLSemanticsException>(() => new SKOSConceptScheme("ex:conceptScheme").Annotate(RDFVocabulary.RDFS.COMMENT, null as RDFLiteral));

        [TestMethod]
        public void ShouldResourceAnnotate()
        {
            SKOSConceptScheme conceptScheme = new SKOSConceptScheme("ex:conceptScheme");
            conceptScheme.Annotate(RDFVocabulary.RDFS.SEE_ALSO, new RDFResource("ex:seeAlso"));

            //Test evolution of SKOS knowledge
            Assert.IsTrue(conceptScheme.Ontology.URI.Equals(conceptScheme.URI));
            Assert.IsTrue(conceptScheme.Ontology.Model.ClassModel.ClassesCount == 8);
            Assert.IsTrue(conceptScheme.Ontology.Model.PropertyModel.PropertiesCount == 33);
            Assert.IsTrue(conceptScheme.Ontology.Data.IndividualsCount == 1);
            Assert.IsTrue(conceptScheme.Ontology.Data.CheckHasAnnotation(new RDFResource("ex:conceptScheme"), RDFVocabulary.RDFS.SEE_ALSO, new RDFResource("ex:seeAlso")));
        }

        [TestMethod]
        public void ShouldThrowExceptionOnResourceAnnotatingBecauseNullProperty()
            => Assert.ThrowsException<OWLSemanticsException>(() => new SKOSConceptScheme("ex:conceptScheme").Annotate(null, new RDFResource("ex:seeAlso")));

        [TestMethod]
        public void ShouldThrowExceptionOnResourceAnnotatingBecauseBlankProperty()
            => Assert.ThrowsException<OWLSemanticsException>(() => new SKOSConceptScheme("ex:conceptScheme").Annotate(new RDFResource(), new RDFResource("ex:seeAlso")));

        [TestMethod]
        public void ShouldThrowExceptionOnResourceAnnotatingBecauseNullValue()
            => Assert.ThrowsException<OWLSemanticsException>(() => new SKOSConceptScheme("ex:conceptScheme").Annotate(RDFVocabulary.RDFS.COMMENT, null as RDFResource));

        [TestMethod]
        public void ShouldLiteralAnnotateConcept()
        {
            SKOSConceptScheme conceptScheme = new SKOSConceptScheme("ex:conceptScheme");
            conceptScheme.AnnotateConcept(new RDFResource("ex:concept"), RDFVocabulary.RDFS.COMMENT, new RDFPlainLiteral("This is a skos:concept!"));

            //Test evolution of SKOS knowledge
            Assert.IsTrue(conceptScheme.Ontology.URI.Equals(conceptScheme.URI));
            Assert.IsTrue(conceptScheme.Ontology.Model.ClassModel.ClassesCount == 8);
            Assert.IsTrue(conceptScheme.Ontology.Model.PropertyModel.PropertiesCount == 33);
            Assert.IsTrue(conceptScheme.Ontology.Data.IndividualsCount == 1);
            Assert.IsTrue(conceptScheme.Ontology.Data.CheckHasAnnotation(new RDFResource("ex:concept"), RDFVocabulary.RDFS.COMMENT, new RDFPlainLiteral("This is a skos:concept!")));
        }

        [TestMethod]
        public void ShouldThrowExceptionOnLiteralAnnotatingConceptBecauseNullConcept()
            => Assert.ThrowsException<OWLSemanticsException>(() => new SKOSConceptScheme("ex:conceptScheme").AnnotateConcept(null, RDFVocabulary.RDFS.COMMENT, new RDFPlainLiteral("This is a skos:concept!")));

        [TestMethod]
        public void ShouldThrowExceptionOnLiteralAnnotatingConceptBecauseNullProperty()
            => Assert.ThrowsException<OWLSemanticsException>(() => new SKOSConceptScheme("ex:conceptScheme").AnnotateConcept(new RDFResource("ex:concept"), null, new RDFPlainLiteral("This is a skos:concept!")));

        [TestMethod]
        public void ShouldThrowExceptionOnLiteralAnnotatingConceptBecauseBlankProperty()
            => Assert.ThrowsException<OWLSemanticsException>(() => new SKOSConceptScheme("ex:conceptScheme").AnnotateConcept(new RDFResource("ex:concept"), new RDFResource(), new RDFPlainLiteral("This is a skos:concept!")));

        [TestMethod]
        public void ShouldThrowExceptionOnLiteralAnnotatingConceptBecauseNullValue()
            => Assert.ThrowsException<OWLSemanticsException>(() => new SKOSConceptScheme("ex:conceptScheme").AnnotateConcept(new RDFResource("ex:concept"), RDFVocabulary.RDFS.COMMENT, null as RDFLiteral));

        [TestMethod]
        public void ShouldResourceAnnotateConcept()
        {
            SKOSConceptScheme conceptScheme = new SKOSConceptScheme("ex:conceptScheme");
            conceptScheme.AnnotateConcept(new RDFResource("ex:concept"), RDFVocabulary.RDFS.SEE_ALSO, new RDFResource("ex:seeAlso"));

            //Test evolution of SKOS knowledge
            Assert.IsTrue(conceptScheme.Ontology.URI.Equals(conceptScheme.URI));
            Assert.IsTrue(conceptScheme.Ontology.Model.ClassModel.ClassesCount == 8);
            Assert.IsTrue(conceptScheme.Ontology.Model.PropertyModel.PropertiesCount == 33);
            Assert.IsTrue(conceptScheme.Ontology.Data.IndividualsCount == 1);
            Assert.IsTrue(conceptScheme.Ontology.Data.CheckHasAnnotation(new RDFResource("ex:concept"), RDFVocabulary.RDFS.SEE_ALSO, new RDFResource("ex:seeAlso")));
        }

        [TestMethod]
        public void ShouldThrowExceptionOnResourceAnnotatingConceptBecauseNullConcept()
            => Assert.ThrowsException<OWLSemanticsException>(() => new SKOSConceptScheme("ex:conceptScheme").AnnotateConcept(null, RDFVocabulary.RDFS.COMMENT, new RDFResource("ex:seeAlso")));

        [TestMethod]
        public void ShouldThrowExceptionOnResourceAnnotatingConceptBecauseNullProperty()
            => Assert.ThrowsException<OWLSemanticsException>(() => new SKOSConceptScheme("ex:conceptScheme").AnnotateConcept(new RDFResource("ex:concept"), null, new RDFResource("ex:seeAlso")));

        [TestMethod]
        public void ShouldThrowExceptionOnResourceAnnotatingConceptBecauseBlankProperty()
            => Assert.ThrowsException<OWLSemanticsException>(() => new SKOSConceptScheme("ex:conceptScheme").AnnotateConcept(new RDFResource("ex:concept"), new RDFResource(), new RDFResource("ex:seeAlso")));

        [TestMethod]
        public void ShouldThrowExceptionOnResourceAnnotatingConceptBecauseNullValue()
            => Assert.ThrowsException<OWLSemanticsException>(() => new SKOSConceptScheme("ex:conceptScheme").AnnotateConcept(new RDFResource("ex:concept"), RDFVocabulary.RDFS.COMMENT, null as RDFResource));

        [TestMethod]
        public void ShouldLiteralAnnotateCollection()
        {
            SKOSConceptScheme conceptScheme = new SKOSConceptScheme("ex:conceptScheme");
            conceptScheme.AnnotateCollection(new RDFResource("ex:collection"), RDFVocabulary.RDFS.COMMENT, new RDFPlainLiteral("This is a skos:Collection!"));

            //Test evolution of SKOS knowledge
            Assert.IsTrue(conceptScheme.Ontology.URI.Equals(conceptScheme.URI));
            Assert.IsTrue(conceptScheme.Ontology.Model.ClassModel.ClassesCount == 8);
            Assert.IsTrue(conceptScheme.Ontology.Model.PropertyModel.PropertiesCount == 33);
            Assert.IsTrue(conceptScheme.Ontology.Data.IndividualsCount == 1);
            Assert.IsTrue(conceptScheme.Ontology.Data.CheckHasAnnotation(new RDFResource("ex:collection"), RDFVocabulary.RDFS.COMMENT, new RDFPlainLiteral("This is a skos:Collection!")));
        }

        [TestMethod]
        public void ShouldThrowExceptionOnLiteralAnnotatingCollectionBecauseNullCollection()
            => Assert.ThrowsException<OWLSemanticsException>(() => new SKOSConceptScheme("ex:conceptScheme").AnnotateCollection(null, RDFVocabulary.RDFS.COMMENT, new RDFPlainLiteral("This is a skos:Collection!")));

        [TestMethod]
        public void ShouldThrowExceptionOnLiteralAnnotatingCollectionBecauseNullProperty()
            => Assert.ThrowsException<OWLSemanticsException>(() => new SKOSConceptScheme("ex:conceptScheme").AnnotateCollection(new RDFResource("ex:collection"), null, new RDFPlainLiteral("This is a skos:Collection!")));

        [TestMethod]
        public void ShouldThrowExceptionOnLiteralAnnotatingCollectionBecauseBlankProperty()
            => Assert.ThrowsException<OWLSemanticsException>(() => new SKOSConceptScheme("ex:conceptScheme").AnnotateCollection(new RDFResource("ex:collection"), new RDFResource(), new RDFPlainLiteral("This is a skos:Collection!")));

        [TestMethod]
        public void ShouldThrowExceptionOnLiteralAnnotatingCollectionBecauseNullValue()
            => Assert.ThrowsException<OWLSemanticsException>(() => new SKOSConceptScheme("ex:conceptScheme").AnnotateCollection(new RDFResource("ex:collection"), RDFVocabulary.RDFS.COMMENT, null as RDFLiteral));

        [TestMethod]
        public void ShouldResourceAnnotateCollection()
        {
            SKOSConceptScheme conceptScheme = new SKOSConceptScheme("ex:conceptScheme");
            conceptScheme.AnnotateCollection(new RDFResource("ex:collection"), RDFVocabulary.RDFS.SEE_ALSO, new RDFResource("ex:seeAlso"));

            //Test evolution of SKOS knowledge
            Assert.IsTrue(conceptScheme.Ontology.URI.Equals(conceptScheme.URI));
            Assert.IsTrue(conceptScheme.Ontology.Model.ClassModel.ClassesCount == 8);
            Assert.IsTrue(conceptScheme.Ontology.Model.PropertyModel.PropertiesCount == 33);
            Assert.IsTrue(conceptScheme.Ontology.Data.IndividualsCount == 1);
            Assert.IsTrue(conceptScheme.Ontology.Data.CheckHasAnnotation(new RDFResource("ex:collection"), RDFVocabulary.RDFS.SEE_ALSO, new RDFResource("ex:seeAlso")));
        }

        [TestMethod]
        public void ShouldThrowExceptionOnResourceAnnotatingCollectionBecauseNullCollection()
            => Assert.ThrowsException<OWLSemanticsException>(() => new SKOSConceptScheme("ex:conceptScheme").AnnotateCollection(null, RDFVocabulary.RDFS.COMMENT, new RDFResource("ex:seeAlso")));

        [TestMethod]
        public void ShouldThrowExceptionOnResourceAnnotatingCollectionBecauseNullProperty()
            => Assert.ThrowsException<OWLSemanticsException>(() => new SKOSConceptScheme("ex:conceptScheme").AnnotateCollection(new RDFResource("ex:collection"), null, new RDFResource("ex:seeAlso")));

        [TestMethod]
        public void ShouldThrowExceptionOnResourceAnnotatingCollectionBecauseBlankProperty()
            => Assert.ThrowsException<OWLSemanticsException>(() => new SKOSConceptScheme("ex:conceptScheme").AnnotateCollection(new RDFResource("ex:collection"), new RDFResource(), new RDFResource("ex:seeAlso")));

        [TestMethod]
        public void ShouldThrowExceptionOnResourceAnnotatingCollectionBecauseNullValue()
            => Assert.ThrowsException<OWLSemanticsException>(() => new SKOSConceptScheme("ex:conceptScheme").AnnotateCollection(new RDFResource("ex:collection"), RDFVocabulary.RDFS.COMMENT, null as RDFResource));

        [TestMethod]
        public void ShouldLiteralAnnotateOrderedCollection()
        {
            SKOSConceptScheme conceptScheme = new SKOSConceptScheme("ex:conceptScheme");
            conceptScheme.AnnotateOrderedCollection(new RDFResource("ex:orderedCollection"), RDFVocabulary.RDFS.COMMENT, new RDFPlainLiteral("This is a skos:OrderedCollection!"));

            //Test evolution of SKOS knowledge
            Assert.IsTrue(conceptScheme.Ontology.URI.Equals(conceptScheme.URI));
            Assert.IsTrue(conceptScheme.Ontology.Model.ClassModel.ClassesCount == 8);
            Assert.IsTrue(conceptScheme.Ontology.Model.PropertyModel.PropertiesCount == 33);
            Assert.IsTrue(conceptScheme.Ontology.Data.IndividualsCount == 1);
            Assert.IsTrue(conceptScheme.Ontology.Data.CheckHasAnnotation(new RDFResource("ex:orderedCollection"), RDFVocabulary.RDFS.COMMENT, new RDFPlainLiteral("This is a skos:OrderedCollection!")));
        }

        [TestMethod]
        public void ShouldThrowExceptionOnLiteralAnnotatingOrderedCollectionBecauseNullOrderedCollection()
            => Assert.ThrowsException<OWLSemanticsException>(() => new SKOSConceptScheme("ex:conceptScheme").AnnotateOrderedCollection(null, RDFVocabulary.RDFS.COMMENT, new RDFPlainLiteral("This is a skos:OrderedCollection!")));

        [TestMethod]
        public void ShouldThrowExceptionOnLiteralAnnotatingOrderedCollectionBecauseNullProperty()
            => Assert.ThrowsException<OWLSemanticsException>(() => new SKOSConceptScheme("ex:conceptScheme").AnnotateOrderedCollection(new RDFResource("ex:orderedCollection"), null, new RDFPlainLiteral("This is a skos:OrderedCollection!")));

        [TestMethod]
        public void ShouldThrowExceptionOnLiteralAnnotatingOrderedCollectionBecauseBlankProperty()
            => Assert.ThrowsException<OWLSemanticsException>(() => new SKOSConceptScheme("ex:conceptScheme").AnnotateOrderedCollection(new RDFResource("ex:orderedCollection"), new RDFResource(), new RDFPlainLiteral("This is a skos:OrderedCollection!")));

        [TestMethod]
        public void ShouldThrowExceptionOnLiteralAnnotatingOrderedCollectionBecauseNullValue()
            => Assert.ThrowsException<OWLSemanticsException>(() => new SKOSConceptScheme("ex:conceptScheme").AnnotateOrderedCollection(new RDFResource("ex:orderedCollection"), RDFVocabulary.RDFS.COMMENT, null as RDFLiteral));

        [TestMethod]
        public void ShouldResourceAnnotateOrderedCollection()
        {
            SKOSConceptScheme conceptScheme = new SKOSConceptScheme("ex:conceptScheme");
            conceptScheme.AnnotateOrderedCollection(new RDFResource("ex:orderedCollection"), RDFVocabulary.RDFS.SEE_ALSO, new RDFResource("ex:seeAlso"));

            //Test evolution of SKOS knowledge
            Assert.IsTrue(conceptScheme.Ontology.URI.Equals(conceptScheme.URI));
            Assert.IsTrue(conceptScheme.Ontology.Model.ClassModel.ClassesCount == 8);
            Assert.IsTrue(conceptScheme.Ontology.Model.PropertyModel.PropertiesCount == 33);
            Assert.IsTrue(conceptScheme.Ontology.Data.IndividualsCount == 1);
            Assert.IsTrue(conceptScheme.Ontology.Data.CheckHasAnnotation(new RDFResource("ex:orderedCollection"), RDFVocabulary.RDFS.SEE_ALSO, new RDFResource("ex:seeAlso")));
        }

        [TestMethod]
        public void ShouldThrowExceptionOnResourceAnnotatingOrderedCollectionBecauseNullOrderedCollection()
            => Assert.ThrowsException<OWLSemanticsException>(() => new SKOSConceptScheme("ex:conceptScheme").AnnotateOrderedCollection(null, RDFVocabulary.RDFS.COMMENT, new RDFResource("ex:seeAlso")));

        [TestMethod]
        public void ShouldThrowExceptionOnResourceAnnotatingOrderedCollectionBecauseNullProperty()
            => Assert.ThrowsException<OWLSemanticsException>(() => new SKOSConceptScheme("ex:conceptScheme").AnnotateOrderedCollection(new RDFResource("ex:orderedCollection"), null, new RDFResource("ex:seeAlso")));

        [TestMethod]
        public void ShouldThrowExceptionOnResourceAnnotatingOrderedCollectionBecauseBlankProperty()
            => Assert.ThrowsException<OWLSemanticsException>(() => new SKOSConceptScheme("ex:conceptScheme").AnnotateOrderedCollection(new RDFResource("ex:orderedCollection"), new RDFResource(), new RDFResource("ex:seeAlso")));

        [TestMethod]
        public void ShouldThrowExceptionOnResourceAnnotatingOrderedCollectionBecauseNullValue()
            => Assert.ThrowsException<OWLSemanticsException>(() => new SKOSConceptScheme("ex:conceptScheme").AnnotateOrderedCollection(new RDFResource("ex:orderedCollection"), RDFVocabulary.RDFS.COMMENT, null as RDFResource));

        [TestMethod]
        public void ShouldLiteralAnnotateLabel()
        {
            SKOSConceptScheme conceptScheme = new SKOSConceptScheme("ex:conceptScheme");
            conceptScheme.AnnotateLabel(new RDFResource("ex:label"), RDFVocabulary.RDFS.COMMENT, new RDFPlainLiteral("This is a skos:Label!"));

            //Test evolution of SKOS knowledge
            Assert.IsTrue(conceptScheme.Ontology.URI.Equals(conceptScheme.URI));
            Assert.IsTrue(conceptScheme.Ontology.Model.ClassModel.ClassesCount == 8);
            Assert.IsTrue(conceptScheme.Ontology.Model.PropertyModel.PropertiesCount == 33);
            Assert.IsTrue(conceptScheme.Ontology.Data.IndividualsCount == 1);
            Assert.IsTrue(conceptScheme.Ontology.Data.CheckHasAnnotation(new RDFResource("ex:label"), RDFVocabulary.RDFS.COMMENT, new RDFPlainLiteral("This is a skos:Label!")));
        }

        [TestMethod]
        public void ShouldThrowExceptionOnLiteralAnnotatingLabelBecauseNullLabel()
            => Assert.ThrowsException<OWLSemanticsException>(() => new SKOSConceptScheme("ex:conceptScheme").AnnotateLabel(null, RDFVocabulary.RDFS.COMMENT, new RDFPlainLiteral("This is a skos:Label!")));

        [TestMethod]
        public void ShouldThrowExceptionOnLiteralAnnotatingLabelBecauseNullProperty()
            => Assert.ThrowsException<OWLSemanticsException>(() => new SKOSConceptScheme("ex:conceptScheme").AnnotateLabel(new RDFResource("ex:label"), null, new RDFPlainLiteral("This is a skos:Label!")));

        [TestMethod]
        public void ShouldThrowExceptionOnLiteralAnnotatingLabelBecauseBlankProperty()
            => Assert.ThrowsException<OWLSemanticsException>(() => new SKOSConceptScheme("ex:conceptScheme").AnnotateLabel(new RDFResource("ex:label"), new RDFResource(), new RDFPlainLiteral("This is a skos:Label!")));

        [TestMethod]
        public void ShouldThrowExceptionOnLiteralAnnotatingLabelBecauseNullValue()
            => Assert.ThrowsException<OWLSemanticsException>(() => new SKOSConceptScheme("ex:conceptScheme").AnnotateLabel(new RDFResource("ex:label"), RDFVocabulary.RDFS.COMMENT, null as RDFLiteral));

        [TestMethod]
        public void ShouldResourceAnnotateLabel()
        {
            SKOSConceptScheme conceptScheme = new SKOSConceptScheme("ex:conceptScheme");
            conceptScheme.AnnotateLabel(new RDFResource("ex:label"), RDFVocabulary.RDFS.SEE_ALSO, new RDFResource("ex:seeAlso"));

            //Test evolution of SKOS knowledge
            Assert.IsTrue(conceptScheme.Ontology.URI.Equals(conceptScheme.URI));
            Assert.IsTrue(conceptScheme.Ontology.Model.ClassModel.ClassesCount == 8);
            Assert.IsTrue(conceptScheme.Ontology.Model.PropertyModel.PropertiesCount == 33);
            Assert.IsTrue(conceptScheme.Ontology.Data.IndividualsCount == 1);
            Assert.IsTrue(conceptScheme.Ontology.Data.CheckHasAnnotation(new RDFResource("ex:label"), RDFVocabulary.RDFS.SEE_ALSO, new RDFResource("ex:seeAlso")));
        }

        [TestMethod]
        public void ShouldThrowExceptionOnResourceAnnotatingLabelBecauseNullLabel()
            => Assert.ThrowsException<OWLSemanticsException>(() => new SKOSConceptScheme("ex:conceptScheme").AnnotateLabel(null, RDFVocabulary.RDFS.COMMENT, new RDFResource("ex:seeAlso")));

        [TestMethod]
        public void ShouldThrowExceptionOnResourceAnnotatingLabelBecauseNullProperty()
            => Assert.ThrowsException<OWLSemanticsException>(() => new SKOSConceptScheme("ex:conceptScheme").AnnotateLabel(new RDFResource("ex:label"), null, new RDFResource("ex:seeAlso")));

        [TestMethod]
        public void ShouldThrowExceptionOnResourceAnnotatingLabelBecauseBlankProperty()
            => Assert.ThrowsException<OWLSemanticsException>(() => new SKOSConceptScheme("ex:conceptScheme").AnnotateLabel(new RDFResource("ex:label"), new RDFResource(), new RDFResource("ex:seeAlso")));

        [TestMethod]
        public void ShouldThrowExceptionOnResourceAnnotatingLabelBecauseNullValue()
            => Assert.ThrowsException<OWLSemanticsException>(() => new SKOSConceptScheme("ex:conceptScheme").AnnotateLabel(new RDFResource("ex:label"), RDFVocabulary.RDFS.COMMENT, null as RDFResource));

        [TestMethod]
        public void ShouldDeclareConceptNote()
        {
            SKOSConceptScheme conceptScheme = new SKOSConceptScheme("ex:conceptScheme");
            conceptScheme.DeclareConceptNote(new RDFResource("ex:concept"), new RDFPlainLiteral("This is a note!"));

            //Test evolution of SKOS knowledge
            Assert.IsTrue(conceptScheme.Ontology.URI.Equals(conceptScheme.URI));
            Assert.IsTrue(conceptScheme.Ontology.Model.ClassModel.ClassesCount == 8);
            Assert.IsTrue(conceptScheme.Ontology.Model.PropertyModel.PropertiesCount == 33);
            Assert.IsTrue(conceptScheme.Ontology.Data.IndividualsCount == 1);
            Assert.IsTrue(conceptScheme.Ontology.Data.CheckHasAnnotation(new RDFResource("ex:concept"), RDFVocabulary.SKOS.NOTE, new RDFPlainLiteral("This is a note!")));
        }

        [TestMethod]
        public void ShouldThrowExceptionOnDeclaringConceptNoteBecauseNullConcept()
            => Assert.ThrowsException<OWLSemanticsException>(() => new SKOSConceptScheme("ex:conceptScheme").DeclareConceptNote(null, new RDFPlainLiteral("This is a note!")));

        [TestMethod]
        public void ShouldThrowExceptionOnDeclaringConceptNoteBecauseNullValue()
            => Assert.ThrowsException<OWLSemanticsException>(() => new SKOSConceptScheme("ex:conceptScheme").DeclareConceptNote(new RDFResource("ex:concept"), null));

        [TestMethod]
        public void ShouldDeclareConceptChangeNote()
        {
            SKOSConceptScheme conceptScheme = new SKOSConceptScheme("ex:conceptScheme");
            conceptScheme.DeclareConceptChangeNote(new RDFResource("ex:concept"), new RDFPlainLiteral("This is a note!"));

            //Test evolution of SKOS knowledge
            Assert.IsTrue(conceptScheme.Ontology.URI.Equals(conceptScheme.URI));
            Assert.IsTrue(conceptScheme.Ontology.Model.ClassModel.ClassesCount == 8);
            Assert.IsTrue(conceptScheme.Ontology.Model.PropertyModel.PropertiesCount == 33);
            Assert.IsTrue(conceptScheme.Ontology.Data.IndividualsCount == 1);
            Assert.IsTrue(conceptScheme.Ontology.Data.CheckHasAnnotation(new RDFResource("ex:concept"), RDFVocabulary.SKOS.CHANGE_NOTE, new RDFPlainLiteral("This is a note!")));
        }

        [TestMethod]
        public void ShouldThrowExceptionOnDeclaringConceptChangeNoteBecauseNullConcept()
            => Assert.ThrowsException<OWLSemanticsException>(() => new SKOSConceptScheme("ex:conceptScheme").DeclareConceptChangeNote(null, new RDFPlainLiteral("This is a note!")));

        [TestMethod]
        public void ShouldThrowExceptionOnDeclaringConceptChangeNoteBecauseNullValue()
            => Assert.ThrowsException<OWLSemanticsException>(() => new SKOSConceptScheme("ex:conceptScheme").DeclareConceptChangeNote(new RDFResource("ex:concept"), null));

        [TestMethod]
        public void ShouldDeclareConceptEditorialNote()
        {
            SKOSConceptScheme conceptScheme = new SKOSConceptScheme("ex:conceptScheme");
            conceptScheme.DeclareConceptEditorialNote(new RDFResource("ex:concept"), new RDFPlainLiteral("This is a note!"));

            //Test evolution of SKOS knowledge
            Assert.IsTrue(conceptScheme.Ontology.URI.Equals(conceptScheme.URI));
            Assert.IsTrue(conceptScheme.Ontology.Model.ClassModel.ClassesCount == 8);
            Assert.IsTrue(conceptScheme.Ontology.Model.PropertyModel.PropertiesCount == 33);
            Assert.IsTrue(conceptScheme.Ontology.Data.IndividualsCount == 1);
            Assert.IsTrue(conceptScheme.Ontology.Data.CheckHasAnnotation(new RDFResource("ex:concept"), RDFVocabulary.SKOS.EDITORIAL_NOTE, new RDFPlainLiteral("This is a note!")));
        }

        [TestMethod]
        public void ShouldThrowExceptionOnDeclaringConceptEditorialNoteBecauseNullConcept()
            => Assert.ThrowsException<OWLSemanticsException>(() => new SKOSConceptScheme("ex:conceptScheme").DeclareConceptEditorialNote(null, new RDFPlainLiteral("This is a note!")));

        [TestMethod]
        public void ShouldThrowExceptionOnDeclaringConceptEditorialNoteBecauseNullValue()
            => Assert.ThrowsException<OWLSemanticsException>(() => new SKOSConceptScheme("ex:conceptScheme").DeclareConceptEditorialNote(new RDFResource("ex:concept"), null));

        [TestMethod]
        public void ShouldDeclareConceptHistoryNote()
        {
            SKOSConceptScheme conceptScheme = new SKOSConceptScheme("ex:conceptScheme");
            conceptScheme.DeclareConceptHistoryNote(new RDFResource("ex:concept"), new RDFPlainLiteral("This is a note!"));

            //Test evolution of SKOS knowledge
            Assert.IsTrue(conceptScheme.Ontology.URI.Equals(conceptScheme.URI));
            Assert.IsTrue(conceptScheme.Ontology.Model.ClassModel.ClassesCount == 8);
            Assert.IsTrue(conceptScheme.Ontology.Model.PropertyModel.PropertiesCount == 33);
            Assert.IsTrue(conceptScheme.Ontology.Data.IndividualsCount == 1);
            Assert.IsTrue(conceptScheme.Ontology.Data.CheckHasAnnotation(new RDFResource("ex:concept"), RDFVocabulary.SKOS.HISTORY_NOTE, new RDFPlainLiteral("This is a note!")));
        }

        [TestMethod]
        public void ShouldThrowExceptionOnDeclaringConceptHistoryNoteBecauseNullConcept()
            => Assert.ThrowsException<OWLSemanticsException>(() => new SKOSConceptScheme("ex:conceptScheme").DeclareConceptHistoryNote(null, new RDFPlainLiteral("This is a note!")));

        [TestMethod]
        public void ShouldThrowExceptionOnDeclaringConceptHistoryNoteBecauseNullValue()
            => Assert.ThrowsException<OWLSemanticsException>(() => new SKOSConceptScheme("ex:conceptScheme").DeclareConceptHistoryNote(new RDFResource("ex:concept"), null));

        [TestMethod]
        public void ShouldDeclareConceptScopeNote()
        {
            SKOSConceptScheme conceptScheme = new SKOSConceptScheme("ex:conceptScheme");
            conceptScheme.DeclareConceptScopeNote(new RDFResource("ex:concept"), new RDFPlainLiteral("This is a note!"));

            //Test evolution of SKOS knowledge
            Assert.IsTrue(conceptScheme.Ontology.URI.Equals(conceptScheme.URI));
            Assert.IsTrue(conceptScheme.Ontology.Model.ClassModel.ClassesCount == 8);
            Assert.IsTrue(conceptScheme.Ontology.Model.PropertyModel.PropertiesCount == 33);
            Assert.IsTrue(conceptScheme.Ontology.Data.IndividualsCount == 1);
            Assert.IsTrue(conceptScheme.Ontology.Data.CheckHasAnnotation(new RDFResource("ex:concept"), RDFVocabulary.SKOS.SCOPE_NOTE, new RDFPlainLiteral("This is a note!")));
        }

        [TestMethod]
        public void ShouldThrowExceptionOnDeclaringConceptScopeNoteBecauseNullConcept()
            => Assert.ThrowsException<OWLSemanticsException>(() => new SKOSConceptScheme("ex:conceptScheme").DeclareConceptScopeNote(null, new RDFPlainLiteral("This is a note!")));

        [TestMethod]
        public void ShouldThrowExceptionOnDeclaringConceptScopeNoteBecauseNullValue()
            => Assert.ThrowsException<OWLSemanticsException>(() => new SKOSConceptScheme("ex:conceptScheme").DeclareConceptScopeNote(new RDFResource("ex:concept"), null));

        [TestMethod]
        public void ShouldDeclareConceptExample()
        {
            SKOSConceptScheme conceptScheme = new SKOSConceptScheme("ex:conceptScheme");
            conceptScheme.DeclareConceptExample(new RDFResource("ex:concept"), new RDFPlainLiteral("This is a note!"));

            //Test evolution of SKOS knowledge
            Assert.IsTrue(conceptScheme.Ontology.URI.Equals(conceptScheme.URI));
            Assert.IsTrue(conceptScheme.Ontology.Model.ClassModel.ClassesCount == 8);
            Assert.IsTrue(conceptScheme.Ontology.Model.PropertyModel.PropertiesCount == 33);
            Assert.IsTrue(conceptScheme.Ontology.Data.IndividualsCount == 1);
            Assert.IsTrue(conceptScheme.Ontology.Data.CheckHasAnnotation(new RDFResource("ex:concept"), RDFVocabulary.SKOS.EXAMPLE, new RDFPlainLiteral("This is a note!")));
        }

        [TestMethod]
        public void ShouldThrowExceptionOnDeclaringConceptExampleBecauseNullConcept()
            => Assert.ThrowsException<OWLSemanticsException>(() => new SKOSConceptScheme("ex:conceptScheme").DeclareConceptExample(null, new RDFPlainLiteral("This is a note!")));

        [TestMethod]
        public void ShouldThrowExceptionOnDeclaringConceptExampleBecauseNullValue()
            => Assert.ThrowsException<OWLSemanticsException>(() => new SKOSConceptScheme("ex:conceptScheme").DeclareConceptExample(new RDFResource("ex:concept"), null));

        [TestMethod]
        public void ShouldDeclareConceptDefinition()
        {
            SKOSConceptScheme conceptScheme = new SKOSConceptScheme("ex:conceptScheme");
            conceptScheme.DeclareConceptDefinition(new RDFResource("ex:concept"), new RDFPlainLiteral("This is a note!"));

            //Test evolution of SKOS knowledge
            Assert.IsTrue(conceptScheme.Ontology.URI.Equals(conceptScheme.URI));
            Assert.IsTrue(conceptScheme.Ontology.Model.ClassModel.ClassesCount == 8);
            Assert.IsTrue(conceptScheme.Ontology.Model.PropertyModel.PropertiesCount == 33);
            Assert.IsTrue(conceptScheme.Ontology.Data.IndividualsCount == 1);
            Assert.IsTrue(conceptScheme.Ontology.Data.CheckHasAnnotation(new RDFResource("ex:concept"), RDFVocabulary.SKOS.DEFINITION, new RDFPlainLiteral("This is a note!")));
        }

        [TestMethod]
        public void ShouldThrowExceptionOnDeclaringConceptDefinitionBecauseNullConcept()
            => Assert.ThrowsException<OWLSemanticsException>(() => new SKOSConceptScheme("ex:conceptScheme").DeclareConceptDefinition(null, new RDFPlainLiteral("This is a note!")));

        [TestMethod]
        public void ShouldThrowExceptionOnDeclaringConceptDefinitionBecauseNullValue()
            => Assert.ThrowsException<OWLSemanticsException>(() => new SKOSConceptScheme("ex:conceptScheme").DeclareConceptDefinition(new RDFResource("ex:concept"), null));

        //RELATIONS

        [TestMethod]
        public void ShouldDeclareTopConcept()
        {
            SKOSConceptScheme conceptScheme = new SKOSConceptScheme("ex:conceptScheme");
            conceptScheme.DeclareTopConcept(new RDFResource("ex:concept"));

            //Test evolution of SKOS knowledge
            Assert.IsTrue(conceptScheme.Ontology.URI.Equals(conceptScheme.URI));
            Assert.IsTrue(conceptScheme.Ontology.Model.ClassModel.ClassesCount == 8);
            Assert.IsTrue(conceptScheme.Ontology.Model.PropertyModel.PropertiesCount == 33);
            Assert.IsTrue(conceptScheme.Ontology.Data.IndividualsCount == 1);
            Assert.IsTrue(conceptScheme.Ontology.Data.CheckHasObjectAssertion(new RDFResource("ex:conceptScheme"), RDFVocabulary.SKOS.HAS_TOP_CONCEPT, new RDFResource("ex:concept")));
            Assert.IsTrue(conceptScheme.Ontology.Data.CheckHasObjectAssertion(new RDFResource("ex:concept"), RDFVocabulary.SKOS.TOP_CONCEPT_OF, new RDFResource("ex:conceptScheme")));
        }

        [TestMethod]
        public void ShouldThrowExceptionOnDeclaringTopConceptBecauseNullConcept()
            => Assert.ThrowsException<OWLSemanticsException>(() => new SKOSConceptScheme("ex:conceptScheme").DeclareTopConcept(null));

        [TestMethod]
        public void ShouldDeclareSemanticRelatedConcepts()
        {
            SKOSConceptScheme conceptScheme = new SKOSConceptScheme("ex:conceptScheme");
            conceptScheme.DeclareSemanticRelatedConcepts(new RDFResource("ex:concept1"), new RDFResource("ex:concept2"));

            //Test evolution of SKOS knowledge
            Assert.IsTrue(conceptScheme.Ontology.URI.Equals(conceptScheme.URI));
            Assert.IsTrue(conceptScheme.Ontology.Model.ClassModel.ClassesCount == 8);
            Assert.IsTrue(conceptScheme.Ontology.Model.PropertyModel.PropertiesCount == 33);
            Assert.IsTrue(conceptScheme.Ontology.Data.IndividualsCount == 1);
            Assert.IsTrue(conceptScheme.Ontology.Data.CheckHasObjectAssertion(new RDFResource("ex:concept1"), RDFVocabulary.SKOS.SEMANTIC_RELATION, new RDFResource("ex:concept2")));
        }

        [TestMethod]
        public void ShouldThrowExceptionOnDeclaringSemanticRelatedConceptsBecauseNullLeftConcept()
            => Assert.ThrowsException<OWLSemanticsException>(() => new SKOSConceptScheme("ex:conceptScheme").DeclareSemanticRelatedConcepts(null, new RDFResource("ex:concept2")));

        [TestMethod]
        public void ShouldThrowExceptionOnDeclaringSemanticRelatedConceptsBecauseNullRightConcept()
            => Assert.ThrowsException<OWLSemanticsException>(() => new SKOSConceptScheme("ex:conceptScheme").DeclareSemanticRelatedConcepts(new RDFResource("ex:concept1"), null));

        [TestMethod]
        public void ShouldDeclareMappingRelatedConcepts()
        {
            SKOSConceptScheme conceptScheme = new SKOSConceptScheme("ex:conceptScheme");
            conceptScheme.DeclareMappingRelatedConcepts(new RDFResource("ex:concept1"), new RDFResource("ex:concept2"));

            //Test evolution of SKOS knowledge
            Assert.IsTrue(conceptScheme.Ontology.URI.Equals(conceptScheme.URI));
            Assert.IsTrue(conceptScheme.Ontology.Model.ClassModel.ClassesCount == 8);
            Assert.IsTrue(conceptScheme.Ontology.Model.PropertyModel.PropertiesCount == 33);
            Assert.IsTrue(conceptScheme.Ontology.Data.IndividualsCount == 1);
            Assert.IsTrue(conceptScheme.Ontology.Data.CheckHasObjectAssertion(new RDFResource("ex:concept1"), RDFVocabulary.SKOS.MAPPING_RELATION, new RDFResource("ex:concept2")));
        }

        [TestMethod]
        public void ShouldThrowExceptionOnDeclaringMappingRelatedConceptsBecauseNullLeftConcept()
            => Assert.ThrowsException<OWLSemanticsException>(() => new SKOSConceptScheme("ex:conceptScheme").DeclareMappingRelatedConcepts(null, new RDFResource("ex:concept2")));

        [TestMethod]
        public void ShouldThrowExceptionOnDeclaringMappingRelatedConceptsBecauseNullRightConcept()
            => Assert.ThrowsException<OWLSemanticsException>(() => new SKOSConceptScheme("ex:conceptScheme").DeclareMappingRelatedConcepts(new RDFResource("ex:concept1"), null));

        [TestMethod]
        public void ShouldDeclareRelatedConcepts()
        {
            SKOSConceptScheme conceptScheme = new SKOSConceptScheme("ex:conceptScheme");
            conceptScheme.DeclareRelatedConcepts(new RDFResource("ex:concept1"), new RDFResource("ex:concept2"));

            //Test evolution of SKOS knowledge
            Assert.IsTrue(conceptScheme.Ontology.URI.Equals(conceptScheme.URI));
            Assert.IsTrue(conceptScheme.Ontology.Model.ClassModel.ClassesCount == 8);
            Assert.IsTrue(conceptScheme.Ontology.Model.PropertyModel.PropertiesCount == 33);
            Assert.IsTrue(conceptScheme.Ontology.Data.IndividualsCount == 1);
            Assert.IsTrue(conceptScheme.Ontology.Data.CheckHasObjectAssertion(new RDFResource("ex:concept1"), RDFVocabulary.SKOS.RELATED, new RDFResource("ex:concept2")));
            Assert.IsTrue(conceptScheme.Ontology.Data.CheckHasObjectAssertion(new RDFResource("ex:concept2"), RDFVocabulary.SKOS.RELATED, new RDFResource("ex:concept1")));
        }

        [TestMethod]
        public void ShouldThrowExceptionOnDeclaringRelatedConceptsBecauseNullLeftConcept()
            => Assert.ThrowsException<OWLSemanticsException>(() => new SKOSConceptScheme("ex:conceptScheme").DeclareRelatedConcepts(null, new RDFResource("ex:concept2")));

        [TestMethod]
        public void ShouldThrowExceptionOnDeclaringRelatedConceptsBecauseNullRightConcept()
            => Assert.ThrowsException<OWLSemanticsException>(() => new SKOSConceptScheme("ex:conceptScheme").DeclareRelatedConcepts(new RDFResource("ex:concept1"), null));

        [TestMethod]
        public void ShouldDeclareBroaderConcepts()
        {
            SKOSConceptScheme conceptScheme = new SKOSConceptScheme("ex:conceptScheme");
            conceptScheme.DeclareBroaderConcepts(new RDFResource("ex:childConcept"), new RDFResource("ex:motherConcept"));

            //Test evolution of SKOS knowledge
            Assert.IsTrue(conceptScheme.Ontology.URI.Equals(conceptScheme.URI));
            Assert.IsTrue(conceptScheme.Ontology.Model.ClassModel.ClassesCount == 8);
            Assert.IsTrue(conceptScheme.Ontology.Model.PropertyModel.PropertiesCount == 33);
            Assert.IsTrue(conceptScheme.Ontology.Data.IndividualsCount == 1);
            Assert.IsTrue(conceptScheme.Ontology.Data.CheckHasObjectAssertion(new RDFResource("ex:childConcept"), RDFVocabulary.SKOS.BROADER, new RDFResource("ex:motherConcept")));
            Assert.IsTrue(conceptScheme.Ontology.Data.CheckHasObjectAssertion(new RDFResource("ex:motherConcept"), RDFVocabulary.SKOS.NARROWER, new RDFResource("ex:childConcept")));
        }

        [TestMethod]
        public void ShouldEmitWarningOnDeclaringBroaderConceptsBecauseHierarchicallyRelatedConcepts()
        {
            string warningMsg = null;
            OWLSemanticsEvents.OnSemanticsWarning += (string msg) => { warningMsg = msg; };

            SKOSConceptScheme conceptScheme = new SKOSConceptScheme("ex:conceptScheme");
            conceptScheme.DeclareNarrowerConcepts(new RDFResource("ex:childConcept"), new RDFResource("ex:motherConcept"));
            conceptScheme.DeclareBroaderConcepts(new RDFResource("ex:childConcept"), new RDFResource("ex:motherConcept")); //SKOS violation

            //Test evolution of SKOS knowledge
            Assert.IsTrue(conceptScheme.Ontology.URI.Equals(conceptScheme.URI));
            Assert.IsTrue(conceptScheme.Ontology.Model.ClassModel.ClassesCount == 8);
            Assert.IsTrue(conceptScheme.Ontology.Model.PropertyModel.PropertiesCount == 33);
            Assert.IsTrue(conceptScheme.Ontology.Data.IndividualsCount == 1);
            Assert.IsTrue(conceptScheme.Ontology.Data.CheckHasObjectAssertion(new RDFResource("ex:childConcept"), RDFVocabulary.SKOS.NARROWER, new RDFResource("ex:motherConcept")));
            Assert.IsNotNull(warningMsg);
            Assert.IsTrue(warningMsg.IndexOf("Broader relation between concept 'ex:childConcept' and concept 'ex:motherConcept' cannot be declared to the concept scheme because it would violate SKOS integrity") > -1);
        }

        [TestMethod]
        public void ShouldEmitWarningOnDeclaringBroaderConceptsBecauseAssociativeRelatedConcepts()
        {
            string warningMsg = null;
            OWLSemanticsEvents.OnSemanticsWarning += (string msg) => { warningMsg = msg; };

            SKOSConceptScheme conceptScheme = new SKOSConceptScheme("ex:conceptScheme");
            conceptScheme.DeclareRelatedConcepts(new RDFResource("ex:childConcept"), new RDFResource("ex:motherConcept"));
            conceptScheme.DeclareBroaderConcepts(new RDFResource("ex:childConcept"), new RDFResource("ex:motherConcept")); //SKOS violation

            //Test evolution of SKOS knowledge
            Assert.IsTrue(conceptScheme.Ontology.URI.Equals(conceptScheme.URI));
            Assert.IsTrue(conceptScheme.Ontology.Model.ClassModel.ClassesCount == 8);
            Assert.IsTrue(conceptScheme.Ontology.Model.PropertyModel.PropertiesCount == 33);
            Assert.IsTrue(conceptScheme.Ontology.Data.IndividualsCount == 1);
            Assert.IsTrue(conceptScheme.Ontology.Data.CheckHasObjectAssertion(new RDFResource("ex:childConcept"), RDFVocabulary.SKOS.RELATED, new RDFResource("ex:motherConcept")));
            Assert.IsNotNull(warningMsg);
            Assert.IsTrue(warningMsg.IndexOf("Broader relation between concept 'ex:childConcept' and concept 'ex:motherConcept' cannot be declared to the concept scheme because it would violate SKOS integrity") > -1);
        }

        [TestMethod]
        public void ShouldEmitWarningOnDeclaringBroaderConceptsBecauseMappingRelatedConcepts()
        {
            string warningMsg = null;
            OWLSemanticsEvents.OnSemanticsWarning += (string msg) => { warningMsg = msg; };

            SKOSConceptScheme conceptScheme = new SKOSConceptScheme("ex:conceptScheme");
            conceptScheme.DeclareCloseMatchConcepts(new RDFResource("ex:childConcept"), new RDFResource("ex:motherConcept"));
            conceptScheme.DeclareBroaderConcepts(new RDFResource("ex:childConcept"), new RDFResource("ex:motherConcept")); //SKOS violation

            //Test evolution of SKOS knowledge
            Assert.IsTrue(conceptScheme.Ontology.URI.Equals(conceptScheme.URI));
            Assert.IsTrue(conceptScheme.Ontology.Model.ClassModel.ClassesCount == 8);
            Assert.IsTrue(conceptScheme.Ontology.Model.PropertyModel.PropertiesCount == 33);
            Assert.IsTrue(conceptScheme.Ontology.Data.IndividualsCount == 1);
            Assert.IsTrue(conceptScheme.Ontology.Data.CheckHasObjectAssertion(new RDFResource("ex:childConcept"), RDFVocabulary.SKOS.CLOSE_MATCH, new RDFResource("ex:motherConcept")));
            Assert.IsNotNull(warningMsg);
            Assert.IsTrue(warningMsg.IndexOf("Broader relation between concept 'ex:childConcept' and concept 'ex:motherConcept' cannot be declared to the concept scheme because it would violate SKOS integrity") > -1);
        }

        [TestMethod]
        public void ShouldThrowExceptionOnDeclaringBroaderConceptsBecauseNullChildConcept()
            => Assert.ThrowsException<OWLSemanticsException>(() => new SKOSConceptScheme("ex:conceptScheme")
                        .DeclareBroaderConcepts(null, new RDFResource("ex:motherConcept")));

        [TestMethod]
        public void ShouldThrowExceptionOnDeclaringBroaderConceptsBecauseNullMotherConcept()
            => Assert.ThrowsException<OWLSemanticsException>(() => new SKOSConceptScheme("ex:conceptScheme")
                        .DeclareBroaderConcepts(new RDFResource("ex:childConcept"), null));

        [TestMethod]
        public void ShouldThrowExceptionOnDeclaringBroaderConceptsBecauseSelfConcept()
            => Assert.ThrowsException<OWLSemanticsException>(() => new SKOSConceptScheme("ex:conceptScheme")
                        .DeclareBroaderConcepts(new RDFResource("ex:childConcept"), new RDFResource("ex:childConcept")));

        [TestMethod]
        public void ShouldDeclareBroaderTransitiveConcepts()
        {
            SKOSConceptScheme conceptScheme = new SKOSConceptScheme("ex:conceptScheme");
            conceptScheme.DeclareBroaderTransitiveConcepts(new RDFResource("ex:childConcept"), new RDFResource("ex:motherConcept"));

            //Test evolution of SKOS knowledge
            Assert.IsTrue(conceptScheme.Ontology.URI.Equals(conceptScheme.URI));
            Assert.IsTrue(conceptScheme.Ontology.Model.ClassModel.ClassesCount == 8);
            Assert.IsTrue(conceptScheme.Ontology.Model.PropertyModel.PropertiesCount == 33);
            Assert.IsTrue(conceptScheme.Ontology.Data.IndividualsCount == 1);
            Assert.IsTrue(conceptScheme.Ontology.Data.CheckHasObjectAssertion(new RDFResource("ex:childConcept"), RDFVocabulary.SKOS.BROADER_TRANSITIVE, new RDFResource("ex:motherConcept")));
            Assert.IsTrue(conceptScheme.Ontology.Data.CheckHasObjectAssertion(new RDFResource("ex:motherConcept"), RDFVocabulary.SKOS.NARROWER_TRANSITIVE, new RDFResource("ex:childConcept")));
        }

        [TestMethod]
        public void ShouldEmitWarningOnDeclaringBroaderTransitiveConceptsBecauseHierarchicallyRelatedConcepts()
        {
            string warningMsg = null;
            OWLSemanticsEvents.OnSemanticsWarning += (string msg) => { warningMsg = msg; };

            SKOSConceptScheme conceptScheme = new SKOSConceptScheme("ex:conceptScheme");
            conceptScheme.DeclareNarrowerTransitiveConcepts(new RDFResource("ex:childConcept"), new RDFResource("ex:motherConcept"));
            conceptScheme.DeclareBroaderTransitiveConcepts(new RDFResource("ex:childConcept"), new RDFResource("ex:motherConcept")); //SKOS violation

            //Test evolution of SKOS knowledge
            Assert.IsTrue(conceptScheme.Ontology.URI.Equals(conceptScheme.URI));
            Assert.IsTrue(conceptScheme.Ontology.Model.ClassModel.ClassesCount == 8);
            Assert.IsTrue(conceptScheme.Ontology.Model.PropertyModel.PropertiesCount == 33);
            Assert.IsTrue(conceptScheme.Ontology.Data.IndividualsCount == 1);
            Assert.IsTrue(conceptScheme.Ontology.Data.CheckHasObjectAssertion(new RDFResource("ex:childConcept"), RDFVocabulary.SKOS.NARROWER_TRANSITIVE, new RDFResource("ex:motherConcept")));
            Assert.IsNotNull(warningMsg);
            Assert.IsTrue(warningMsg.IndexOf("BroaderTransitive relation between concept 'ex:childConcept' and concept 'ex:motherConcept' cannot be declared to the concept scheme because it would violate SKOS integrity") > -1);
        }

        [TestMethod]
        public void ShouldEmitWarningOnDeclaringBroaderTransitiveConceptsBecauseAssociativeRelatedConcepts()
        {
            string warningMsg = null;
            OWLSemanticsEvents.OnSemanticsWarning += (string msg) => { warningMsg = msg; };

            SKOSConceptScheme conceptScheme = new SKOSConceptScheme("ex:conceptScheme");
            conceptScheme.DeclareRelatedConcepts(new RDFResource("ex:childConcept"), new RDFResource("ex:motherConcept"));
            conceptScheme.DeclareBroaderTransitiveConcepts(new RDFResource("ex:childConcept"), new RDFResource("ex:motherConcept")); //SKOS violation

            //Test evolution of SKOS knowledge
            Assert.IsTrue(conceptScheme.Ontology.URI.Equals(conceptScheme.URI));
            Assert.IsTrue(conceptScheme.Ontology.Model.ClassModel.ClassesCount == 8);
            Assert.IsTrue(conceptScheme.Ontology.Model.PropertyModel.PropertiesCount == 33);
            Assert.IsTrue(conceptScheme.Ontology.Data.IndividualsCount == 1);
            Assert.IsTrue(conceptScheme.Ontology.Data.CheckHasObjectAssertion(new RDFResource("ex:childConcept"), RDFVocabulary.SKOS.RELATED, new RDFResource("ex:motherConcept")));
            Assert.IsNotNull(warningMsg);
            Assert.IsTrue(warningMsg.IndexOf("BroaderTransitive relation between concept 'ex:childConcept' and concept 'ex:motherConcept' cannot be declared to the concept scheme because it would violate SKOS integrity") > -1);
        }

        [TestMethod]
        public void ShouldEmitWarningOnDeclaringBroaderTransitiveConceptsBecauseMappingRelatedConcepts()
        {
            string warningMsg = null;
            OWLSemanticsEvents.OnSemanticsWarning += (string msg) => { warningMsg = msg; };

            SKOSConceptScheme conceptScheme = new SKOSConceptScheme("ex:conceptScheme");
            conceptScheme.DeclareCloseMatchConcepts(new RDFResource("ex:childConcept"), new RDFResource("ex:motherConcept"));
            conceptScheme.DeclareBroaderTransitiveConcepts(new RDFResource("ex:childConcept"), new RDFResource("ex:motherConcept")); //SKOS violation

            //Test evolution of SKOS knowledge
            Assert.IsTrue(conceptScheme.Ontology.URI.Equals(conceptScheme.URI));
            Assert.IsTrue(conceptScheme.Ontology.Model.ClassModel.ClassesCount == 8);
            Assert.IsTrue(conceptScheme.Ontology.Model.PropertyModel.PropertiesCount == 33);
            Assert.IsTrue(conceptScheme.Ontology.Data.IndividualsCount == 1);
            Assert.IsTrue(conceptScheme.Ontology.Data.CheckHasObjectAssertion(new RDFResource("ex:childConcept"), RDFVocabulary.SKOS.CLOSE_MATCH, new RDFResource("ex:motherConcept")));
            Assert.IsNotNull(warningMsg);
            Assert.IsTrue(warningMsg.IndexOf("BroaderTransitive relation between concept 'ex:childConcept' and concept 'ex:motherConcept' cannot be declared to the concept scheme because it would violate SKOS integrity") > -1);
        }

        [TestMethod]
        public void ShouldThrowExceptionOnDeclaringBroaderTransitiveConceptsBecauseNullChildConcept()
            => Assert.ThrowsException<OWLSemanticsException>(() => new SKOSConceptScheme("ex:conceptScheme")
                        .DeclareBroaderTransitiveConcepts(null, new RDFResource("ex:motherConcept")));

        [TestMethod]
        public void ShouldThrowExceptionOnDeclaringBroaderTransitiveConceptsBecauseNullMotherConcept()
            => Assert.ThrowsException<OWLSemanticsException>(() => new SKOSConceptScheme("ex:conceptScheme")
                        .DeclareBroaderTransitiveConcepts(new RDFResource("ex:childConcept"), null));

        [TestMethod]
        public void ShouldThrowExceptionOnDeclaringBroaderTransitiveConceptsBecauseSelfConcept()
            => Assert.ThrowsException<OWLSemanticsException>(() => new SKOSConceptScheme("ex:conceptScheme")
                        .DeclareBroaderTransitiveConcepts(new RDFResource("ex:childConcept"), new RDFResource("ex:childConcept")));

        [TestMethod]
        public void ShouldDeclareNarrowerConcepts()
        {
            SKOSConceptScheme conceptScheme = new SKOSConceptScheme("ex:conceptScheme");
            conceptScheme.DeclareNarrowerConcepts(new RDFResource("ex:motherConcept"), new RDFResource("ex:childConcept"));

            //Test evolution of SKOS knowledge
            Assert.IsTrue(conceptScheme.Ontology.URI.Equals(conceptScheme.URI));
            Assert.IsTrue(conceptScheme.Ontology.Model.ClassModel.ClassesCount == 8);
            Assert.IsTrue(conceptScheme.Ontology.Model.PropertyModel.PropertiesCount == 33);
            Assert.IsTrue(conceptScheme.Ontology.Data.IndividualsCount == 1);
            Assert.IsTrue(conceptScheme.Ontology.Data.CheckHasObjectAssertion(new RDFResource("ex:motherConcept"), RDFVocabulary.SKOS.NARROWER, new RDFResource("ex:childConcept")));
            Assert.IsTrue(conceptScheme.Ontology.Data.CheckHasObjectAssertion(new RDFResource("ex:childConcept"), RDFVocabulary.SKOS.BROADER, new RDFResource("ex:motherConcept")));
        }

        [TestMethod]
        public void ShouldEmitWarningOnDeclaringNarrowerConceptsBecauseHierarchicallyRelatedConcepts()
        {
            string warningMsg = null;
            OWLSemanticsEvents.OnSemanticsWarning += (string msg) => { warningMsg = msg; };

            SKOSConceptScheme conceptScheme = new SKOSConceptScheme("ex:conceptScheme");
            conceptScheme.DeclareBroaderConcepts(new RDFResource("ex:motherConcept"), new RDFResource("ex:childConcept"));
            conceptScheme.DeclareNarrowerConcepts(new RDFResource("ex:motherConcept"), new RDFResource("ex:childConcept")); //SKOS violation

            //Test evolution of SKOS knowledge
            Assert.IsTrue(conceptScheme.Ontology.URI.Equals(conceptScheme.URI));
            Assert.IsTrue(conceptScheme.Ontology.Model.ClassModel.ClassesCount == 8);
            Assert.IsTrue(conceptScheme.Ontology.Model.PropertyModel.PropertiesCount == 33);
            Assert.IsTrue(conceptScheme.Ontology.Data.IndividualsCount == 1);
            Assert.IsTrue(conceptScheme.Ontology.Data.CheckHasObjectAssertion(new RDFResource("ex:motherConcept"), RDFVocabulary.SKOS.BROADER, new RDFResource("ex:childConcept")));
            Assert.IsNotNull(warningMsg);
            Assert.IsTrue(warningMsg.IndexOf("Narrower relation between concept 'ex:motherConcept' and concept 'ex:childConcept' cannot be declared to the concept scheme because it would violate SKOS integrity") > -1);
        }

        [TestMethod]
        public void ShouldEmitWarningOnDeclaringNarrowerConceptsBecauseAssociativeRelatedConcepts()
        {
            string warningMsg = null;
            OWLSemanticsEvents.OnSemanticsWarning += (string msg) => { warningMsg = msg; };

            SKOSConceptScheme conceptScheme = new SKOSConceptScheme("ex:conceptScheme");
            conceptScheme.DeclareRelatedConcepts(new RDFResource("ex:motherConcept"), new RDFResource("ex:childConcept"));
            conceptScheme.DeclareNarrowerConcepts(new RDFResource("ex:motherConcept"), new RDFResource("ex:childConcept")); //SKOS violation

            //Test evolution of SKOS knowledge
            Assert.IsTrue(conceptScheme.Ontology.URI.Equals(conceptScheme.URI));
            Assert.IsTrue(conceptScheme.Ontology.Model.ClassModel.ClassesCount == 8);
            Assert.IsTrue(conceptScheme.Ontology.Model.PropertyModel.PropertiesCount == 33);
            Assert.IsTrue(conceptScheme.Ontology.Data.IndividualsCount == 1);
            Assert.IsTrue(conceptScheme.Ontology.Data.CheckHasObjectAssertion(new RDFResource("ex:motherConcept"), RDFVocabulary.SKOS.RELATED, new RDFResource("ex:childConcept")));
            Assert.IsNotNull(warningMsg);
            Assert.IsTrue(warningMsg.IndexOf("Narrower relation between concept 'ex:motherConcept' and concept 'ex:childConcept' cannot be declared to the concept scheme because it would violate SKOS integrity") > -1);
        }

        [TestMethod]
        public void ShouldEmitWarningOnDeclaringNarrowerConceptsBecauseMappingRelatedConcepts()
        {
            string warningMsg = null;
            OWLSemanticsEvents.OnSemanticsWarning += (string msg) => { warningMsg = msg; };

            SKOSConceptScheme conceptScheme = new SKOSConceptScheme("ex:conceptScheme");
            conceptScheme.DeclareCloseMatchConcepts(new RDFResource("ex:motherConcept"), new RDFResource("ex:childConcept"));
            conceptScheme.DeclareNarrowerConcepts(new RDFResource("ex:motherConcept"), new RDFResource("ex:childConcept")); //SKOS violation

            //Test evolution of SKOS knowledge
            Assert.IsTrue(conceptScheme.Ontology.URI.Equals(conceptScheme.URI));
            Assert.IsTrue(conceptScheme.Ontology.Model.ClassModel.ClassesCount == 8);
            Assert.IsTrue(conceptScheme.Ontology.Model.PropertyModel.PropertiesCount == 33);
            Assert.IsTrue(conceptScheme.Ontology.Data.IndividualsCount == 1);
            Assert.IsTrue(conceptScheme.Ontology.Data.CheckHasObjectAssertion(new RDFResource("ex:motherConcept"), RDFVocabulary.SKOS.CLOSE_MATCH, new RDFResource("ex:childConcept")));
            Assert.IsNotNull(warningMsg);
            Assert.IsTrue(warningMsg.IndexOf("Narrower relation between concept 'ex:motherConcept' and concept 'ex:childConcept' cannot be declared to the concept scheme because it would violate SKOS integrity") > -1);
        }

        [TestMethod]
        public void ShouldThrowExceptionOnDeclaringNarrowerConceptsBecauseNullChildConcept()
            => Assert.ThrowsException<OWLSemanticsException>(() => new SKOSConceptScheme("ex:conceptScheme")
                        .DeclareNarrowerConcepts(null, new RDFResource("ex:childConcept")));

        [TestMethod]
        public void ShouldThrowExceptionOnDeclaringNarrowerConceptsBecauseNullMotherConcept()
            => Assert.ThrowsException<OWLSemanticsException>(() => new SKOSConceptScheme("ex:conceptScheme")
                        .DeclareNarrowerConcepts(new RDFResource("ex:motherConcept"), null));

        [TestMethod]
        public void ShouldThrowExceptionOnDeclaringNarrowerConceptsBecauseSelfConcept()
            => Assert.ThrowsException<OWLSemanticsException>(() => new SKOSConceptScheme("ex:conceptScheme")
                        .DeclareNarrowerConcepts(new RDFResource("ex:motherConcept"), new RDFResource("ex:motherConcept")));

        [TestMethod]
        public void ShouldDeclareNarrowerTransitiveConcepts()
        {
            SKOSConceptScheme conceptScheme = new SKOSConceptScheme("ex:conceptScheme");
            conceptScheme.DeclareNarrowerTransitiveConcepts(new RDFResource("ex:motherConcept"), new RDFResource("ex:childConcept"));

            //Test evolution of SKOS knowledge
            Assert.IsTrue(conceptScheme.Ontology.URI.Equals(conceptScheme.URI));
            Assert.IsTrue(conceptScheme.Ontology.Model.ClassModel.ClassesCount == 8);
            Assert.IsTrue(conceptScheme.Ontology.Model.PropertyModel.PropertiesCount == 33);
            Assert.IsTrue(conceptScheme.Ontology.Data.IndividualsCount == 1);
            Assert.IsTrue(conceptScheme.Ontology.Data.CheckHasObjectAssertion(new RDFResource("ex:motherConcept"), RDFVocabulary.SKOS.NARROWER_TRANSITIVE, new RDFResource("ex:childConcept")));
            Assert.IsTrue(conceptScheme.Ontology.Data.CheckHasObjectAssertion(new RDFResource("ex:childConcept"), RDFVocabulary.SKOS.BROADER_TRANSITIVE, new RDFResource("ex:motherConcept")));
        }

        [TestMethod]
        public void ShouldEmitWarningOnDeclaringNarrowerTransitiveConceptsBecauseHierarchicallyRelatedConcepts()
        {
            string warningMsg = null;
            OWLSemanticsEvents.OnSemanticsWarning += (string msg) => { warningMsg = msg; };

            SKOSConceptScheme conceptScheme = new SKOSConceptScheme("ex:conceptScheme");
            conceptScheme.DeclareBroaderTransitiveConcepts(new RDFResource("ex:motherConcept"), new RDFResource("ex:childConcept"));
            conceptScheme.DeclareNarrowerTransitiveConcepts(new RDFResource("ex:motherConcept"), new RDFResource("ex:childConcept")); //SKOS violation

            //Test evolution of SKOS knowledge
            Assert.IsTrue(conceptScheme.Ontology.URI.Equals(conceptScheme.URI));
            Assert.IsTrue(conceptScheme.Ontology.Model.ClassModel.ClassesCount == 8);
            Assert.IsTrue(conceptScheme.Ontology.Model.PropertyModel.PropertiesCount == 33);
            Assert.IsTrue(conceptScheme.Ontology.Data.IndividualsCount == 1);
            Assert.IsTrue(conceptScheme.Ontology.Data.CheckHasObjectAssertion(new RDFResource("ex:motherConcept"), RDFVocabulary.SKOS.BROADER_TRANSITIVE, new RDFResource("ex:childConcept")));
            Assert.IsNotNull(warningMsg);
            Assert.IsTrue(warningMsg.IndexOf("NarrowerTransitive relation between concept 'ex:motherConcept' and concept 'ex:childConcept' cannot be declared to the concept scheme because it would violate SKOS integrity") > -1);
        }

        [TestMethod]
        public void ShouldEmitWarningOnDeclaringNarrowerTransitiveConceptsBecauseAssociativeRelatedConcepts()
        {
            string warningMsg = null;
            OWLSemanticsEvents.OnSemanticsWarning += (string msg) => { warningMsg = msg; };

            SKOSConceptScheme conceptScheme = new SKOSConceptScheme("ex:conceptScheme");
            conceptScheme.DeclareRelatedConcepts(new RDFResource("ex:motherConcept"), new RDFResource("ex:childConcept"));
            conceptScheme.DeclareNarrowerTransitiveConcepts(new RDFResource("ex:motherConcept"), new RDFResource("ex:childConcept")); //SKOS violation

            //Test evolution of SKOS knowledge
            Assert.IsTrue(conceptScheme.Ontology.URI.Equals(conceptScheme.URI));
            Assert.IsTrue(conceptScheme.Ontology.Model.ClassModel.ClassesCount == 8);
            Assert.IsTrue(conceptScheme.Ontology.Model.PropertyModel.PropertiesCount == 33);
            Assert.IsTrue(conceptScheme.Ontology.Data.IndividualsCount == 1);
            Assert.IsTrue(conceptScheme.Ontology.Data.CheckHasObjectAssertion(new RDFResource("ex:motherConcept"), RDFVocabulary.SKOS.RELATED, new RDFResource("ex:childConcept")));
            Assert.IsNotNull(warningMsg);
            Assert.IsTrue(warningMsg.IndexOf("NarrowerTransitive relation between concept 'ex:motherConcept' and concept 'ex:childConcept' cannot be declared to the concept scheme because it would violate SKOS integrity") > -1);
        }

        [TestMethod]
        public void ShouldEmitWarningOnDeclaringNarrowerTransitiveConceptsBecauseMappingRelatedConcepts()
        {
            string warningMsg = null;
            OWLSemanticsEvents.OnSemanticsWarning += (string msg) => { warningMsg = msg; };

            SKOSConceptScheme conceptScheme = new SKOSConceptScheme("ex:conceptScheme");
            conceptScheme.DeclareCloseMatchConcepts(new RDFResource("ex:motherConcept"), new RDFResource("ex:childConcept"));
            conceptScheme.DeclareNarrowerTransitiveConcepts(new RDFResource("ex:motherConcept"), new RDFResource("ex:childConcept")); //SKOS violation

            //Test evolution of SKOS knowledge
            Assert.IsTrue(conceptScheme.Ontology.URI.Equals(conceptScheme.URI));
            Assert.IsTrue(conceptScheme.Ontology.Model.ClassModel.ClassesCount == 8);
            Assert.IsTrue(conceptScheme.Ontology.Model.PropertyModel.PropertiesCount == 33);
            Assert.IsTrue(conceptScheme.Ontology.Data.IndividualsCount == 1);
            Assert.IsTrue(conceptScheme.Ontology.Data.CheckHasObjectAssertion(new RDFResource("ex:motherConcept"), RDFVocabulary.SKOS.CLOSE_MATCH, new RDFResource("ex:childConcept")));
            Assert.IsNotNull(warningMsg);
            Assert.IsTrue(warningMsg.IndexOf("NarrowerTransitive relation between concept 'ex:motherConcept' and concept 'ex:childConcept' cannot be declared to the concept scheme because it would violate SKOS integrity") > -1);
        }

        [TestMethod]
        public void ShouldThrowExceptionOnDeclaringNarrowerTransitiveConceptsBecauseNullChildConcept()
            => Assert.ThrowsException<OWLSemanticsException>(() => new SKOSConceptScheme("ex:conceptScheme")
                        .DeclareNarrowerTransitiveConcepts(null, new RDFResource("ex:childConcept")));

        [TestMethod]
        public void ShouldThrowExceptionOnDeclaringNarrowerTransitiveConceptsBecauseNullMotherConcept()
            => Assert.ThrowsException<OWLSemanticsException>(() => new SKOSConceptScheme("ex:conceptScheme")
                        .DeclareNarrowerTransitiveConcepts(new RDFResource("ex:motherConcept"), null));

        [TestMethod]
        public void ShouldThrowExceptionOnDeclaringNarrowerTransitiveConceptsBecauseSelfConcept()
            => Assert.ThrowsException<OWLSemanticsException>(() => new SKOSConceptScheme("ex:conceptScheme")
                        .DeclareNarrowerTransitiveConcepts(new RDFResource("ex:motherConcept"), new RDFResource("ex:motherConcept")));

        [TestMethod]
        public void ShouldDeclareCloseMatchConcepts()
        {
            SKOSConceptScheme conceptScheme = new SKOSConceptScheme("ex:conceptScheme");
            conceptScheme.DeclareCloseMatchConcepts(new RDFResource("ex:leftConcept"), new RDFResource("ex:rightConcept"));

            //Test evolution of SKOS knowledge
            Assert.IsTrue(conceptScheme.Ontology.URI.Equals(conceptScheme.URI));
            Assert.IsTrue(conceptScheme.Ontology.Model.ClassModel.ClassesCount == 8);
            Assert.IsTrue(conceptScheme.Ontology.Model.PropertyModel.PropertiesCount == 33);
            Assert.IsTrue(conceptScheme.Ontology.Data.IndividualsCount == 1);
            Assert.IsTrue(conceptScheme.Ontology.Data.CheckHasObjectAssertion(new RDFResource("ex:leftConcept"), RDFVocabulary.SKOS.CLOSE_MATCH, new RDFResource("ex:rightConcept")));
            Assert.IsTrue(conceptScheme.Ontology.Data.CheckHasObjectAssertion(new RDFResource("ex:rightConcept"), RDFVocabulary.SKOS.CLOSE_MATCH, new RDFResource("ex:leftConcept")));
        }

        [TestMethod]
        public void ShouldEmitWarningOnDeclaringCloseMatchConceptsBecauseHierarchicallyRelatedConcepts()
        {
            string warningMsg = null;
            OWLSemanticsEvents.OnSemanticsWarning += (string msg) => { warningMsg = msg; };

            SKOSConceptScheme conceptScheme = new SKOSConceptScheme("ex:conceptScheme");
            conceptScheme.DeclareBroaderConcepts(new RDFResource("ex:leftConcept"), new RDFResource("ex:rightConcept"));
            conceptScheme.DeclareCloseMatchConcepts(new RDFResource("ex:leftConcept"), new RDFResource("ex:rightConcept")); //SKOS violation

            //Test evolution of SKOS knowledge
            Assert.IsTrue(conceptScheme.Ontology.URI.Equals(conceptScheme.URI));
            Assert.IsTrue(conceptScheme.Ontology.Model.ClassModel.ClassesCount == 8);
            Assert.IsTrue(conceptScheme.Ontology.Model.PropertyModel.PropertiesCount == 33);
            Assert.IsTrue(conceptScheme.Ontology.Data.IndividualsCount == 1);
            Assert.IsTrue(conceptScheme.Ontology.Data.CheckHasObjectAssertion(new RDFResource("ex:leftConcept"), RDFVocabulary.SKOS.BROADER, new RDFResource("ex:rightConcept")));
            Assert.IsNotNull(warningMsg);
            Assert.IsTrue(warningMsg.IndexOf("CloseMatch relation between concept 'ex:leftConcept' and concept 'ex:rightConcept' cannot be declared to the concept scheme because it would violate SKOS integrity") > -1);
        }

        [TestMethod]
        public void ShouldEmitWarningOnDeclaringCloseMatchConceptsBecauseMappingRelatedConcepts()
        {
            string warningMsg = null;
            OWLSemanticsEvents.OnSemanticsWarning += (string msg) => { warningMsg = msg; };

            SKOSConceptScheme conceptScheme = new SKOSConceptScheme("ex:conceptScheme");
            conceptScheme.DeclareBroadMatchConcepts(new RDFResource("ex:leftConcept"), new RDFResource("ex:rightConcept"));
            conceptScheme.DeclareCloseMatchConcepts(new RDFResource("ex:leftConcept"), new RDFResource("ex:rightConcept")); //SKOS violation

            //Test evolution of SKOS knowledge
            Assert.IsTrue(conceptScheme.Ontology.URI.Equals(conceptScheme.URI));
            Assert.IsTrue(conceptScheme.Ontology.Model.ClassModel.ClassesCount == 8);
            Assert.IsTrue(conceptScheme.Ontology.Model.PropertyModel.PropertiesCount == 33);
            Assert.IsTrue(conceptScheme.Ontology.Data.IndividualsCount == 1);
            Assert.IsTrue(conceptScheme.Ontology.Data.CheckHasObjectAssertion(new RDFResource("ex:leftConcept"), RDFVocabulary.SKOS.BROAD_MATCH, new RDFResource("ex:rightConcept")));
            Assert.IsNotNull(warningMsg);
            Assert.IsTrue(warningMsg.IndexOf("CloseMatch relation between concept 'ex:leftConcept' and concept 'ex:rightConcept' cannot be declared to the concept scheme because it would violate SKOS integrity") > -1);
        }

        [TestMethod]
        public void ShouldThrowExceptionOnDeclaringCloseMatchConceptsBecauseNullLeftConcept()
            => Assert.ThrowsException<OWLSemanticsException>(() => new SKOSConceptScheme("ex:conceptScheme")
                        .DeclareCloseMatchConcepts(null, new RDFResource("ex:rightConcept")));

        [TestMethod]
        public void ShouldThrowExceptionOnDeclaringCloseMatchConceptsBecauseNullRightConcept()
            => Assert.ThrowsException<OWLSemanticsException>(() => new SKOSConceptScheme("ex:conceptScheme")
                        .DeclareCloseMatchConcepts(new RDFResource("ex:leftConcept"), null));

        [TestMethod]
        public void ShouldThrowExceptionOnDeclaringCloseMatchConceptsBecauseSelfConcept()
            => Assert.ThrowsException<OWLSemanticsException>(() => new SKOSConceptScheme("ex:conceptScheme")
                        .DeclareCloseMatchConcepts(new RDFResource("ex:leftConcept"), new RDFResource("ex:leftConcept")));

        [TestMethod]
        public void ShouldDeclareExactMatchConcepts()
        {
            SKOSConceptScheme conceptScheme = new SKOSConceptScheme("ex:conceptScheme");
            conceptScheme.DeclareExactMatchConcepts(new RDFResource("ex:leftConcept"), new RDFResource("ex:rightConcept"));

            //Test evolution of SKOS knowledge
            Assert.IsTrue(conceptScheme.Ontology.URI.Equals(conceptScheme.URI));
            Assert.IsTrue(conceptScheme.Ontology.Model.ClassModel.ClassesCount == 8);
            Assert.IsTrue(conceptScheme.Ontology.Model.PropertyModel.PropertiesCount == 33);
            Assert.IsTrue(conceptScheme.Ontology.Data.IndividualsCount == 1);
            Assert.IsTrue(conceptScheme.Ontology.Data.CheckHasObjectAssertion(new RDFResource("ex:leftConcept"), RDFVocabulary.SKOS.EXACT_MATCH, new RDFResource("ex:rightConcept")));
            Assert.IsTrue(conceptScheme.Ontology.Data.CheckHasObjectAssertion(new RDFResource("ex:rightConcept"), RDFVocabulary.SKOS.EXACT_MATCH, new RDFResource("ex:leftConcept")));
        }

        [TestMethod]
        public void ShouldEmitWarningOnDeclaringExactMatchConceptsBecauseHierarchicallyRelatedConcepts()
        {
            string warningMsg = null;
            OWLSemanticsEvents.OnSemanticsWarning += (string msg) => { warningMsg = msg; };

            SKOSConceptScheme conceptScheme = new SKOSConceptScheme("ex:conceptScheme");
            conceptScheme.DeclareNarrowerTransitiveConcepts(new RDFResource("ex:leftConcept"), new RDFResource("ex:rightConcept"));
            conceptScheme.DeclareExactMatchConcepts(new RDFResource("ex:leftConcept"), new RDFResource("ex:rightConcept")); //SKOS violation

            //Test evolution of SKOS knowledge
            Assert.IsTrue(conceptScheme.Ontology.URI.Equals(conceptScheme.URI));
            Assert.IsTrue(conceptScheme.Ontology.Model.ClassModel.ClassesCount == 8);
            Assert.IsTrue(conceptScheme.Ontology.Model.PropertyModel.PropertiesCount == 33);
            Assert.IsTrue(conceptScheme.Ontology.Data.IndividualsCount == 1);
            Assert.IsTrue(conceptScheme.Ontology.Data.CheckHasObjectAssertion(new RDFResource("ex:leftConcept"), RDFVocabulary.SKOS.NARROWER_TRANSITIVE, new RDFResource("ex:rightConcept")));
            Assert.IsNotNull(warningMsg);
            Assert.IsTrue(warningMsg.IndexOf("ExactMatch relation between concept 'ex:leftConcept' and concept 'ex:rightConcept' cannot be declared to the concept scheme because it would violate SKOS integrity") > -1);
        }

        [TestMethod]
        public void ShouldEmitWarningOnDeclaringExactMatchConceptsBecauseMappingRelatedConcepts()
        {
            string warningMsg = null;
            OWLSemanticsEvents.OnSemanticsWarning += (string msg) => { warningMsg = msg; };

            SKOSConceptScheme conceptScheme = new SKOSConceptScheme("ex:conceptScheme");
            conceptScheme.DeclareRelatedMatchConcepts(new RDFResource("ex:leftConcept"), new RDFResource("ex:rightConcept"));
            conceptScheme.DeclareExactMatchConcepts(new RDFResource("ex:leftConcept"), new RDFResource("ex:rightConcept")); //SKOS violation

            //Test evolution of SKOS knowledge
            Assert.IsTrue(conceptScheme.Ontology.URI.Equals(conceptScheme.URI));
            Assert.IsTrue(conceptScheme.Ontology.Model.ClassModel.ClassesCount == 8);
            Assert.IsTrue(conceptScheme.Ontology.Model.PropertyModel.PropertiesCount == 33);
            Assert.IsTrue(conceptScheme.Ontology.Data.IndividualsCount == 1);
            Assert.IsTrue(conceptScheme.Ontology.Data.CheckHasObjectAssertion(new RDFResource("ex:leftConcept"), RDFVocabulary.SKOS.RELATED_MATCH, new RDFResource("ex:rightConcept")));
            Assert.IsNotNull(warningMsg);
            Assert.IsTrue(warningMsg.IndexOf("ExactMatch relation between concept 'ex:leftConcept' and concept 'ex:rightConcept' cannot be declared to the concept scheme because it would violate SKOS integrity") > -1);
        }

        [TestMethod]
        public void ShouldThrowExceptionOnDeclaringExactMatchConceptsBecauseNullLeftConcept()
            => Assert.ThrowsException<OWLSemanticsException>(() => new SKOSConceptScheme("ex:conceptScheme")
                        .DeclareExactMatchConcepts(null, new RDFResource("ex:rightConcept")));

        [TestMethod]
        public void ShouldThrowExceptionOnDeclaringExactMatchConceptsBecauseNullRightConcept()
            => Assert.ThrowsException<OWLSemanticsException>(() => new SKOSConceptScheme("ex:conceptScheme")
                        .DeclareExactMatchConcepts(new RDFResource("ex:leftConcept"), null));

        [TestMethod]
        public void ShouldThrowExceptionOnDeclaringExactMatchConceptsBecauseSelfConcept()
            => Assert.ThrowsException<OWLSemanticsException>(() => new SKOSConceptScheme("ex:conceptScheme")
                        .DeclareExactMatchConcepts(new RDFResource("ex:leftConcept"), new RDFResource("ex:leftConcept")));

        [TestMethod]
        public void ShouldDeclareBroadMatchConcepts()
        {
            SKOSConceptScheme conceptScheme = new SKOSConceptScheme("ex:conceptScheme");
            conceptScheme.DeclareBroadMatchConcepts(new RDFResource("ex:childConcept"), new RDFResource("ex:motherConcept"));

            //Test evolution of SKOS knowledge
            Assert.IsTrue(conceptScheme.Ontology.URI.Equals(conceptScheme.URI));
            Assert.IsTrue(conceptScheme.Ontology.Model.ClassModel.ClassesCount == 8);
            Assert.IsTrue(conceptScheme.Ontology.Model.PropertyModel.PropertiesCount == 33);
            Assert.IsTrue(conceptScheme.Ontology.Data.IndividualsCount == 1);
            Assert.IsTrue(conceptScheme.Ontology.Data.CheckHasObjectAssertion(new RDFResource("ex:childConcept"), RDFVocabulary.SKOS.BROAD_MATCH, new RDFResource("ex:motherConcept")));
            Assert.IsTrue(conceptScheme.Ontology.Data.CheckHasObjectAssertion(new RDFResource("ex:motherConcept"), RDFVocabulary.SKOS.NARROW_MATCH, new RDFResource("ex:childConcept")));
        }

        [TestMethod]
        public void ShouldEmitWarningOnDeclaringBroadMatchConceptsBecauseHierarchicallyRelatedConcepts()
        {
            string warningMsg = null;
            OWLSemanticsEvents.OnSemanticsWarning += (string msg) => { warningMsg = msg; };

            SKOSConceptScheme conceptScheme = new SKOSConceptScheme("ex:conceptScheme");
            conceptScheme.DeclareNarrowerConcepts(new RDFResource("ex:childConcept"), new RDFResource("ex:motherConcept"));
            conceptScheme.DeclareBroadMatchConcepts(new RDFResource("ex:childConcept"), new RDFResource("ex:motherConcept")); //SKOS violation

            //Test evolution of SKOS knowledge
            Assert.IsTrue(conceptScheme.Ontology.URI.Equals(conceptScheme.URI));
            Assert.IsTrue(conceptScheme.Ontology.Model.ClassModel.ClassesCount == 8);
            Assert.IsTrue(conceptScheme.Ontology.Model.PropertyModel.PropertiesCount == 33);
            Assert.IsTrue(conceptScheme.Ontology.Data.IndividualsCount == 1);
            Assert.IsTrue(conceptScheme.Ontology.Data.CheckHasObjectAssertion(new RDFResource("ex:childConcept"), RDFVocabulary.SKOS.NARROWER, new RDFResource("ex:motherConcept")));
            Assert.IsNotNull(warningMsg);
            Assert.IsTrue(warningMsg.IndexOf("BroadMatch relation between concept 'ex:childConcept' and concept 'ex:motherConcept' cannot be declared to the concept scheme because it would violate SKOS integrity") > -1);
        }

        [TestMethod]
        public void ShouldEmitWarningOnDeclaringBroadMatchConceptsBecauseAssociativeRelatedConcepts()
        {
            string warningMsg = null;
            OWLSemanticsEvents.OnSemanticsWarning += (string msg) => { warningMsg = msg; };

            SKOSConceptScheme conceptScheme = new SKOSConceptScheme("ex:conceptScheme");
            conceptScheme.DeclareRelatedConcepts(new RDFResource("ex:childConcept"), new RDFResource("ex:motherConcept"));
            conceptScheme.DeclareBroadMatchConcepts(new RDFResource("ex:childConcept"), new RDFResource("ex:motherConcept")); //SKOS violation

            //Test evolution of SKOS knowledge
            Assert.IsTrue(conceptScheme.Ontology.URI.Equals(conceptScheme.URI));
            Assert.IsTrue(conceptScheme.Ontology.Model.ClassModel.ClassesCount == 8);
            Assert.IsTrue(conceptScheme.Ontology.Model.PropertyModel.PropertiesCount == 33);
            Assert.IsTrue(conceptScheme.Ontology.Data.IndividualsCount == 1);
            Assert.IsTrue(conceptScheme.Ontology.Data.CheckHasObjectAssertion(new RDFResource("ex:childConcept"), RDFVocabulary.SKOS.RELATED, new RDFResource("ex:motherConcept")));
            Assert.IsNotNull(warningMsg);
            Assert.IsTrue(warningMsg.IndexOf("BroadMatch relation between concept 'ex:childConcept' and concept 'ex:motherConcept' cannot be declared to the concept scheme because it would violate SKOS integrity") > -1);
        }

        [TestMethod]
        public void ShouldEmitWarningOnDeclaringBroadMatchConceptsBecauseMappingRelatedConcepts()
        {
            string warningMsg = null;
            OWLSemanticsEvents.OnSemanticsWarning += (string msg) => { warningMsg = msg; };

            SKOSConceptScheme conceptScheme = new SKOSConceptScheme("ex:conceptScheme");
            conceptScheme.DeclareCloseMatchConcepts(new RDFResource("ex:childConcept"), new RDFResource("ex:motherConcept"));
            conceptScheme.DeclareBroadMatchConcepts(new RDFResource("ex:childConcept"), new RDFResource("ex:motherConcept")); //SKOS violation

            //Test evolution of SKOS knowledge
            Assert.IsTrue(conceptScheme.Ontology.URI.Equals(conceptScheme.URI));
            Assert.IsTrue(conceptScheme.Ontology.Model.ClassModel.ClassesCount == 8);
            Assert.IsTrue(conceptScheme.Ontology.Model.PropertyModel.PropertiesCount == 33);
            Assert.IsTrue(conceptScheme.Ontology.Data.IndividualsCount == 1);
            Assert.IsTrue(conceptScheme.Ontology.Data.CheckHasObjectAssertion(new RDFResource("ex:childConcept"), RDFVocabulary.SKOS.CLOSE_MATCH, new RDFResource("ex:motherConcept")));
            Assert.IsNotNull(warningMsg);
            Assert.IsTrue(warningMsg.IndexOf("BroadMatch relation between concept 'ex:childConcept' and concept 'ex:motherConcept' cannot be declared to the concept scheme because it would violate SKOS integrity") > -1);
        }

        [TestMethod]
        public void ShouldThrowExceptionOnDeclaringBroadMatchConceptsBecauseNullChildConcept()
            => Assert.ThrowsException<OWLSemanticsException>(() => new SKOSConceptScheme("ex:conceptScheme")
                        .DeclareBroadMatchConcepts(null, new RDFResource("ex:motherConcept")));

        [TestMethod]
        public void ShouldThrowExceptionOnDeclaringBroadMatchConceptsBecauseNullMotherConcept()
            => Assert.ThrowsException<OWLSemanticsException>(() => new SKOSConceptScheme("ex:conceptScheme")
                        .DeclareBroadMatchConcepts(new RDFResource("ex:childConcept"), null));

        [TestMethod]
        public void ShouldThrowExceptionOnDeclaringBroadMatchConceptsBecauseSelfConcept()
            => Assert.ThrowsException<OWLSemanticsException>(() => new SKOSConceptScheme("ex:conceptScheme")
                        .DeclareBroadMatchConcepts(new RDFResource("ex:childConcept"), new RDFResource("ex:childConcept")));

        [TestMethod]
        public void ShouldDeclareNarrowMatchConcepts()
        {
            SKOSConceptScheme conceptScheme = new SKOSConceptScheme("ex:conceptScheme");
            conceptScheme.DeclareNarrowMatchConcepts(new RDFResource("ex:motherConcept"), new RDFResource("ex:childConcept"));

            //Test evolution of SKOS knowledge
            Assert.IsTrue(conceptScheme.Ontology.URI.Equals(conceptScheme.URI));
            Assert.IsTrue(conceptScheme.Ontology.Model.ClassModel.ClassesCount == 8);
            Assert.IsTrue(conceptScheme.Ontology.Model.PropertyModel.PropertiesCount == 33);
            Assert.IsTrue(conceptScheme.Ontology.Data.IndividualsCount == 1);
            Assert.IsTrue(conceptScheme.Ontology.Data.CheckHasObjectAssertion(new RDFResource("ex:motherConcept"), RDFVocabulary.SKOS.NARROW_MATCH, new RDFResource("ex:childConcept")));
            Assert.IsTrue(conceptScheme.Ontology.Data.CheckHasObjectAssertion(new RDFResource("ex:childConcept"), RDFVocabulary.SKOS.BROAD_MATCH, new RDFResource("ex:motherConcept")));
        }

        [TestMethod]
        public void ShouldEmitWarningOnDeclaringNarrowMatchConceptsBecauseHierarchicallyRelatedConcepts()
        {
            string warningMsg = null;
            OWLSemanticsEvents.OnSemanticsWarning += (string msg) => { warningMsg = msg; };

            SKOSConceptScheme conceptScheme = new SKOSConceptScheme("ex:conceptScheme");
            conceptScheme.DeclareBroadMatchConcepts(new RDFResource("ex:motherConcept"), new RDFResource("ex:childConcept"));
            conceptScheme.DeclareNarrowMatchConcepts(new RDFResource("ex:motherConcept"), new RDFResource("ex:childConcept")); //SKOS violation

            //Test evolution of SKOS knowledge
            Assert.IsTrue(conceptScheme.Ontology.URI.Equals(conceptScheme.URI));
            Assert.IsTrue(conceptScheme.Ontology.Model.ClassModel.ClassesCount == 8);
            Assert.IsTrue(conceptScheme.Ontology.Model.PropertyModel.PropertiesCount == 33);
            Assert.IsTrue(conceptScheme.Ontology.Data.IndividualsCount == 1);
            Assert.IsTrue(conceptScheme.Ontology.Data.CheckHasObjectAssertion(new RDFResource("ex:motherConcept"), RDFVocabulary.SKOS.BROAD_MATCH, new RDFResource("ex:childConcept")));
            Assert.IsNotNull(warningMsg);
            Assert.IsTrue(warningMsg.IndexOf("NarrowMatch relation between concept 'ex:motherConcept' and concept 'ex:childConcept' cannot be declared to the concept scheme because it would violate SKOS integrity") > -1);
        }

        [TestMethod]
        public void ShouldEmitWarningOnDeclaringNarrowMatchConceptsBecauseAssociativeRelatedConcepts()
        {
            string warningMsg = null;
            OWLSemanticsEvents.OnSemanticsWarning += (string msg) => { warningMsg = msg; };

            SKOSConceptScheme conceptScheme = new SKOSConceptScheme("ex:conceptScheme");
            conceptScheme.DeclareRelatedConcepts(new RDFResource("ex:motherConcept"), new RDFResource("ex:childConcept"));
            conceptScheme.DeclareNarrowMatchConcepts(new RDFResource("ex:motherConcept"), new RDFResource("ex:childConcept")); //SKOS violation

            //Test evolution of SKOS knowledge
            Assert.IsTrue(conceptScheme.Ontology.URI.Equals(conceptScheme.URI));
            Assert.IsTrue(conceptScheme.Ontology.Model.ClassModel.ClassesCount == 8);
            Assert.IsTrue(conceptScheme.Ontology.Model.PropertyModel.PropertiesCount == 33);
            Assert.IsTrue(conceptScheme.Ontology.Data.IndividualsCount == 1);
            Assert.IsTrue(conceptScheme.Ontology.Data.CheckHasObjectAssertion(new RDFResource("ex:motherConcept"), RDFVocabulary.SKOS.RELATED, new RDFResource("ex:childConcept")));
            Assert.IsNotNull(warningMsg);
            Assert.IsTrue(warningMsg.IndexOf("NarrowMatch relation between concept 'ex:motherConcept' and concept 'ex:childConcept' cannot be declared to the concept scheme because it would violate SKOS integrity") > -1);
        }

        [TestMethod]
        public void ShouldEmitWarningOnDeclaringNarrowMatchConceptsBecauseMappingRelatedConcepts()
        {
            string warningMsg = null;
            OWLSemanticsEvents.OnSemanticsWarning += (string msg) => { warningMsg = msg; };

            SKOSConceptScheme conceptScheme = new SKOSConceptScheme("ex:conceptScheme");
            conceptScheme.DeclareCloseMatchConcepts(new RDFResource("ex:motherConcept"), new RDFResource("ex:childConcept"));
            conceptScheme.DeclareNarrowMatchConcepts(new RDFResource("ex:motherConcept"), new RDFResource("ex:childConcept")); //SKOS violation

            //Test evolution of SKOS knowledge
            Assert.IsTrue(conceptScheme.Ontology.URI.Equals(conceptScheme.URI));
            Assert.IsTrue(conceptScheme.Ontology.Model.ClassModel.ClassesCount == 8);
            Assert.IsTrue(conceptScheme.Ontology.Model.PropertyModel.PropertiesCount == 33);
            Assert.IsTrue(conceptScheme.Ontology.Data.IndividualsCount == 1);
            Assert.IsTrue(conceptScheme.Ontology.Data.CheckHasObjectAssertion(new RDFResource("ex:motherConcept"), RDFVocabulary.SKOS.CLOSE_MATCH, new RDFResource("ex:childConcept")));
            Assert.IsNotNull(warningMsg);
            Assert.IsTrue(warningMsg.IndexOf("NarrowMatch relation between concept 'ex:motherConcept' and concept 'ex:childConcept' cannot be declared to the concept scheme because it would violate SKOS integrity") > -1);
        }

        [TestMethod]
        public void ShouldThrowExceptionOnDeclaringNarrowMatchConceptsBecauseNullChildConcept()
            => Assert.ThrowsException<OWLSemanticsException>(() => new SKOSConceptScheme("ex:conceptScheme")
                        .DeclareNarrowMatchConcepts(null, new RDFResource("ex:childConcept")));

        [TestMethod]
        public void ShouldThrowExceptionOnDeclaringNarrowMatchConceptsBecauseNullMotherConcept()
            => Assert.ThrowsException<OWLSemanticsException>(() => new SKOSConceptScheme("ex:conceptScheme")
                        .DeclareNarrowMatchConcepts(new RDFResource("ex:motherConcept"), null));

        [TestMethod]
        public void ShouldThrowExceptionOnDeclaringNarrowMatchConceptsBecauseSelfConcept()
            => Assert.ThrowsException<OWLSemanticsException>(() => new SKOSConceptScheme("ex:conceptScheme")
                        .DeclareNarrowMatchConcepts(new RDFResource("ex:motherConcept"), new RDFResource("ex:motherConcept")));

        [TestMethod]
        public void ShouldDeclareRelatedMatchConcepts()
        {
            SKOSConceptScheme conceptScheme = new SKOSConceptScheme("ex:conceptScheme");
            conceptScheme.DeclareRelatedMatchConcepts(new RDFResource("ex:leftConcept"), new RDFResource("ex:rightConcept"));

            //Test evolution of SKOS knowledge
            Assert.IsTrue(conceptScheme.Ontology.URI.Equals(conceptScheme.URI));
            Assert.IsTrue(conceptScheme.Ontology.Model.ClassModel.ClassesCount == 8);
            Assert.IsTrue(conceptScheme.Ontology.Model.PropertyModel.PropertiesCount == 33);
            Assert.IsTrue(conceptScheme.Ontology.Data.IndividualsCount == 1);
            Assert.IsTrue(conceptScheme.Ontology.Data.CheckHasObjectAssertion(new RDFResource("ex:leftConcept"), RDFVocabulary.SKOS.RELATED_MATCH, new RDFResource("ex:rightConcept")));
            Assert.IsTrue(conceptScheme.Ontology.Data.CheckHasObjectAssertion(new RDFResource("ex:rightConcept"), RDFVocabulary.SKOS.RELATED_MATCH, new RDFResource("ex:leftConcept")));
        }

        [TestMethod]
        public void ShouldEmitWarningOnDeclaringRelatedMatchConceptsBecauseHierarchicallyRelatedConcepts()
        {
            string warningMsg = null;
            OWLSemanticsEvents.OnSemanticsWarning += (string msg) => { warningMsg = msg; };

            SKOSConceptScheme conceptScheme = new SKOSConceptScheme("ex:conceptScheme");
            conceptScheme.DeclareNarrowerTransitiveConcepts(new RDFResource("ex:leftConcept"), new RDFResource("ex:rightConcept"));
            conceptScheme.DeclareRelatedMatchConcepts(new RDFResource("ex:leftConcept"), new RDFResource("ex:rightConcept")); //SKOS violation

            //Test evolution of SKOS knowledge
            Assert.IsTrue(conceptScheme.Ontology.URI.Equals(conceptScheme.URI));
            Assert.IsTrue(conceptScheme.Ontology.Model.ClassModel.ClassesCount == 8);
            Assert.IsTrue(conceptScheme.Ontology.Model.PropertyModel.PropertiesCount == 33);
            Assert.IsTrue(conceptScheme.Ontology.Data.IndividualsCount == 1);
            Assert.IsTrue(conceptScheme.Ontology.Data.CheckHasObjectAssertion(new RDFResource("ex:leftConcept"), RDFVocabulary.SKOS.NARROWER_TRANSITIVE, new RDFResource("ex:rightConcept")));
            Assert.IsNotNull(warningMsg);
            Assert.IsTrue(warningMsg.IndexOf("RelatedMatch relation between concept 'ex:leftConcept' and concept 'ex:rightConcept' cannot be declared to the concept scheme because it would violate SKOS integrity") > -1);
        }

        [TestMethod]
        public void ShouldEmitWarningOnDeclaringRelatedMatchConceptsBecauseMappingRelatedConcepts()
        {
            string warningMsg = null;
            OWLSemanticsEvents.OnSemanticsWarning += (string msg) => { warningMsg = msg; };

            SKOSConceptScheme conceptScheme = new SKOSConceptScheme("ex:conceptScheme");
            conceptScheme.DeclareExactMatchConcepts(new RDFResource("ex:leftConcept"), new RDFResource("ex:rightConcept"));
            conceptScheme.DeclareRelatedMatchConcepts(new RDFResource("ex:leftConcept"), new RDFResource("ex:rightConcept")); //SKOS violation

            //Test evolution of SKOS knowledge
            Assert.IsTrue(conceptScheme.Ontology.URI.Equals(conceptScheme.URI));
            Assert.IsTrue(conceptScheme.Ontology.Model.ClassModel.ClassesCount == 8);
            Assert.IsTrue(conceptScheme.Ontology.Model.PropertyModel.PropertiesCount == 33);
            Assert.IsTrue(conceptScheme.Ontology.Data.IndividualsCount == 1);
            Assert.IsTrue(conceptScheme.Ontology.Data.CheckHasObjectAssertion(new RDFResource("ex:leftConcept"), RDFVocabulary.SKOS.EXACT_MATCH, new RDFResource("ex:rightConcept")));
            Assert.IsNotNull(warningMsg);
            Assert.IsTrue(warningMsg.IndexOf("RelatedMatch relation between concept 'ex:leftConcept' and concept 'ex:rightConcept' cannot be declared to the concept scheme because it would violate SKOS integrity") > -1);
        }

        [TestMethod]
        public void ShouldThrowExceptionOnDeclaringRelatedMatchConceptsBecauseNullLeftConcept()
            => Assert.ThrowsException<OWLSemanticsException>(() => new SKOSConceptScheme("ex:conceptScheme")
                        .DeclareRelatedMatchConcepts(null, new RDFResource("ex:rightConcept")));

        [TestMethod]
        public void ShouldThrowExceptionOnDeclaringRelatedMatchConceptsBecauseNullRightConcept()
            => Assert.ThrowsException<OWLSemanticsException>(() => new SKOSConceptScheme("ex:conceptScheme")
                        .DeclareRelatedMatchConcepts(new RDFResource("ex:leftConcept"), null));

        [TestMethod]
        public void ShouldThrowExceptionOnDeclaringRelatedMatchConceptsBecauseSelfConcept()
            => Assert.ThrowsException<OWLSemanticsException>(() => new SKOSConceptScheme("ex:conceptScheme")
                        .DeclareRelatedMatchConcepts(new RDFResource("ex:leftConcept"), new RDFResource("ex:leftConcept")));

        [TestMethod]
        public void ShouldDeclarePreferredLabel()
        {
            SKOSConceptScheme conceptScheme = new SKOSConceptScheme("ex:conceptScheme");
            conceptScheme.DeclarePreferredLabel(new RDFResource("ex:concept"), new RDFPlainLiteral("label"));
            conceptScheme.DeclarePreferredLabel(new RDFResource("ex:concept"), new RDFPlainLiteral("label", "en-US"));
            conceptScheme.DeclarePreferredLabel(new RDFResource("ex:concept"), new RDFPlainLiteral("etichetta", "it-IT"));

            //Test evolution of SKOS knowledge
            Assert.IsTrue(conceptScheme.Ontology.URI.Equals(conceptScheme.URI));
            Assert.IsTrue(conceptScheme.Ontology.Model.ClassModel.ClassesCount == 8);
            Assert.IsTrue(conceptScheme.Ontology.Model.PropertyModel.PropertiesCount == 33);
            Assert.IsTrue(conceptScheme.Ontology.Data.IndividualsCount == 1);
            Assert.IsTrue(conceptScheme.Ontology.Data.CheckHasAnnotation(new RDFResource("ex:concept"), RDFVocabulary.SKOS.PREF_LABEL, new RDFPlainLiteral("label")));
            Assert.IsTrue(conceptScheme.Ontology.Data.CheckHasAnnotation(new RDFResource("ex:concept"), RDFVocabulary.SKOS.PREF_LABEL, new RDFPlainLiteral("label", "en-US")));
            Assert.IsTrue(conceptScheme.Ontology.Data.CheckHasAnnotation(new RDFResource("ex:concept"), RDFVocabulary.SKOS.PREF_LABEL, new RDFPlainLiteral("etichetta", "it-IT")));
        }

        [TestMethod]
        public void ShouldEmitWarningOnDeclaringPreferredLabelBecauseAlreadyExistingUnlanguagedLabel()
        {
            string warningMsg = null;
            OWLSemanticsEvents.OnSemanticsWarning += (string msg) => { warningMsg = msg; };

            SKOSConceptScheme conceptScheme = new SKOSConceptScheme("ex:conceptScheme");
            conceptScheme.DeclarePreferredLabel(new RDFResource("ex:concept"), new RDFPlainLiteral("label"));
            conceptScheme.DeclarePreferredLabel(new RDFResource("ex:concept"), new RDFPlainLiteral("label2")); //SKOS violation

            //Test evolution of SKOS knowledge
            Assert.IsTrue(conceptScheme.Ontology.URI.Equals(conceptScheme.URI));
            Assert.IsTrue(conceptScheme.Ontology.Model.ClassModel.ClassesCount == 8);
            Assert.IsTrue(conceptScheme.Ontology.Model.PropertyModel.PropertiesCount == 33);
            Assert.IsTrue(conceptScheme.Ontology.Data.IndividualsCount == 1);
            Assert.IsTrue(conceptScheme.Ontology.Data.CheckHasAnnotation(new RDFResource("ex:concept"), RDFVocabulary.SKOS.PREF_LABEL, new RDFPlainLiteral("label")));
            Assert.IsNotNull(warningMsg);
            Assert.IsTrue(warningMsg.IndexOf("PrefLabel relation between concept 'ex:concept' and value 'label2' cannot be declared to the concept scheme because it would violate SKOS integrity") > -1);
        }

        [TestMethod]
        public void ShouldEmitWarningOnDeclaringPreferredLabelBecauseAlreadyExistingLanguagedLabel()
        {
            string warningMsg = null;
            OWLSemanticsEvents.OnSemanticsWarning += (string msg) => { warningMsg = msg; };

            SKOSConceptScheme conceptScheme = new SKOSConceptScheme("ex:conceptScheme");
            conceptScheme.DeclarePreferredLabel(new RDFResource("ex:concept"), new RDFPlainLiteral("label", "en-US"));
            conceptScheme.DeclarePreferredLabel(new RDFResource("ex:concept"), new RDFPlainLiteral("label2", "en-US")); //SKOS violation

            //Test evolution of SKOS knowledge
            Assert.IsTrue(conceptScheme.Ontology.URI.Equals(conceptScheme.URI));
            Assert.IsTrue(conceptScheme.Ontology.Model.ClassModel.ClassesCount == 8);
            Assert.IsTrue(conceptScheme.Ontology.Model.PropertyModel.PropertiesCount == 33);
            Assert.IsTrue(conceptScheme.Ontology.Data.IndividualsCount == 1);
            Assert.IsTrue(conceptScheme.Ontology.Data.CheckHasAnnotation(new RDFResource("ex:concept"), RDFVocabulary.SKOS.PREF_LABEL, new RDFPlainLiteral("label", "en-US")));
            Assert.IsNotNull(warningMsg);
            Assert.IsTrue(warningMsg.IndexOf("PrefLabel relation between concept 'ex:concept' and value 'label2@EN-US' cannot be declared to the concept scheme because it would violate SKOS integrity") > -1);
        }

        [TestMethod]
        public void ShouldEmitWarningOnDeclaringPreferredLabelBecauseClashWithHiddenLabel()
        {
            string warningMsg = null;
            OWLSemanticsEvents.OnSemanticsWarning += (string msg) => { warningMsg = msg; };

            SKOSConceptScheme conceptScheme = new SKOSConceptScheme("ex:conceptScheme");
            conceptScheme.DeclareHiddenLabel(new RDFResource("ex:concept"), new RDFPlainLiteral("label"));
            conceptScheme.DeclarePreferredLabel(new RDFResource("ex:concept"), new RDFPlainLiteral("label")); //SKOS violation

            //Test evolution of SKOS knowledge
            Assert.IsTrue(conceptScheme.Ontology.URI.Equals(conceptScheme.URI));
            Assert.IsTrue(conceptScheme.Ontology.Model.ClassModel.ClassesCount == 8);
            Assert.IsTrue(conceptScheme.Ontology.Model.PropertyModel.PropertiesCount == 33);
            Assert.IsTrue(conceptScheme.Ontology.Data.IndividualsCount == 1);
            Assert.IsTrue(conceptScheme.Ontology.Data.CheckHasAnnotation(new RDFResource("ex:concept"), RDFVocabulary.SKOS.HIDDEN_LABEL, new RDFPlainLiteral("label")));
            Assert.IsNotNull(warningMsg);
            Assert.IsTrue(warningMsg.IndexOf("PrefLabel relation between concept 'ex:concept' and value 'label' cannot be declared to the concept scheme because it would violate SKOS integrity") > -1);
        }

        [TestMethod]
        public void ShouldEmitWarningOnDeclaringPreferredLabelBecauseClashWithExtendedHiddenLabel()
        {
            string warningMsg = null;
            OWLSemanticsEvents.OnSemanticsWarning += (string msg) => { warningMsg = msg; };

            SKOSConceptScheme conceptScheme = new SKOSConceptScheme("ex:conceptScheme");
            conceptScheme.DeclareHiddenLabel(new RDFResource("ex:concept"), new RDFResource("ex:label"), new RDFPlainLiteral("label"));
            conceptScheme.DeclarePreferredLabel(new RDFResource("ex:concept"), new RDFPlainLiteral("label")); //SKOS violation

            //Test evolution of SKOS knowledge
            Assert.IsTrue(conceptScheme.Ontology.URI.Equals(conceptScheme.URI));
            Assert.IsTrue(conceptScheme.Ontology.Model.ClassModel.ClassesCount == 8);
            Assert.IsTrue(conceptScheme.Ontology.Model.PropertyModel.PropertiesCount == 33);
            Assert.IsTrue(conceptScheme.Ontology.Data.IndividualsCount == 1);
            Assert.IsTrue(conceptScheme.Ontology.Data.CheckHasObjectAssertion(new RDFResource("ex:concept"), RDFVocabulary.SKOS.SKOSXL.HIDDEN_LABEL, new RDFResource("ex:label")));
            Assert.IsTrue(conceptScheme.Ontology.Data.CheckHasDatatypeAssertion(new RDFResource("ex:label"), RDFVocabulary.SKOS.SKOSXL.LITERAL_FORM, new RDFPlainLiteral("label")));
            Assert.IsNotNull(warningMsg);
            Assert.IsTrue(warningMsg.IndexOf("PrefLabel relation between concept 'ex:concept' and value 'label' cannot be declared to the concept scheme because it would violate SKOS integrity") > -1);
        }

        [TestMethod]
        public void ShouldThrowExceptionOnDeclaringPreferredLabelBecauseNullConcept()
            => Assert.ThrowsException<OWLSemanticsException>(() => new SKOSConceptScheme("ex:conceptScheme")
                        .DeclarePreferredLabel(null, new RDFPlainLiteral("label")));

        [TestMethod]
        public void ShouldThrowExceptionOnDeclaringPreferredLabelBecauseNullValue()
            => Assert.ThrowsException<OWLSemanticsException>(() => new SKOSConceptScheme("ex:conceptScheme")
                        .DeclarePreferredLabel(new RDFResource("ex:concept"), null));

        [TestMethod]
        public void ShouldDeclareExtendedPreferredLabel()
        {
            SKOSConceptScheme conceptScheme = new SKOSConceptScheme("ex:conceptScheme");
            conceptScheme.DeclarePreferredLabel(new RDFResource("ex:concept"), new RDFResource("ex:label"), new RDFPlainLiteral("label"));
            conceptScheme.DeclarePreferredLabel(new RDFResource("ex:concept"), new RDFResource("ex:label"), new RDFPlainLiteral("label", "en-US"));
            conceptScheme.DeclarePreferredLabel(new RDFResource("ex:concept"), new RDFResource("ex:label"), new RDFPlainLiteral("etichetta", "it-IT"));

            //Test evolution of SKOS knowledge
            Assert.IsTrue(conceptScheme.Ontology.URI.Equals(conceptScheme.URI));
            Assert.IsTrue(conceptScheme.Ontology.Model.ClassModel.ClassesCount == 8);
            Assert.IsTrue(conceptScheme.Ontology.Model.PropertyModel.PropertiesCount == 33);
            Assert.IsTrue(conceptScheme.Ontology.Data.IndividualsCount == 1);
            Assert.IsTrue(conceptScheme.Ontology.Data.CheckHasObjectAssertion(new RDFResource("ex:concept"), RDFVocabulary.SKOS.SKOSXL.PREF_LABEL, new RDFResource("ex:label")));
            Assert.IsTrue(conceptScheme.Ontology.Data.CheckHasDatatypeAssertion(new RDFResource("ex:label"), RDFVocabulary.SKOS.SKOSXL.LITERAL_FORM, new RDFPlainLiteral("label")));
            Assert.IsTrue(conceptScheme.Ontology.Data.CheckHasDatatypeAssertion(new RDFResource("ex:label"), RDFVocabulary.SKOS.SKOSXL.LITERAL_FORM, new RDFPlainLiteral("label", "en-US")));
            Assert.IsTrue(conceptScheme.Ontology.Data.CheckHasDatatypeAssertion(new RDFResource("ex:label"), RDFVocabulary.SKOS.SKOSXL.LITERAL_FORM, new RDFPlainLiteral("etichetta", "it-IT")));
        }

        [TestMethod]
        public void ShouldEmitWarningOnDeclaringExtendedPreferredLabelBecauseAlreadyExistingUnlanguagedLabel()
        {
            string warningMsg = null;
            OWLSemanticsEvents.OnSemanticsWarning += (string msg) => { warningMsg = msg; };

            SKOSConceptScheme conceptScheme = new SKOSConceptScheme("ex:conceptScheme");
            conceptScheme.DeclarePreferredLabel(new RDFResource("ex:concept"), new RDFPlainLiteral("label"));
            conceptScheme.DeclarePreferredLabel(new RDFResource("ex:concept"), new RDFResource("ex:label"), new RDFPlainLiteral("label2")); //SKOS violation

            //Test evolution of SKOS knowledge
            Assert.IsTrue(conceptScheme.Ontology.URI.Equals(conceptScheme.URI));
            Assert.IsTrue(conceptScheme.Ontology.Model.ClassModel.ClassesCount == 8);
            Assert.IsTrue(conceptScheme.Ontology.Model.PropertyModel.PropertiesCount == 33);
            Assert.IsTrue(conceptScheme.Ontology.Data.IndividualsCount == 1);
            Assert.IsTrue(conceptScheme.Ontology.Data.CheckHasAnnotation(new RDFResource("ex:concept"), RDFVocabulary.SKOS.PREF_LABEL, new RDFPlainLiteral("label")));
            Assert.IsNotNull(warningMsg);
            Assert.IsTrue(warningMsg.IndexOf("PrefLabel relation between concept 'ex:concept' and label 'ex:label' with value 'label2' cannot be declared to the concept scheme because it would violate SKOS integrity") > -1);
        }

        [TestMethod]
        public void ShouldEmitWarningOnDeclaringExtendedPreferredLabelBecauseAlreadyExistingLanguagedLabel()
        {
            string warningMsg = null;
            OWLSemanticsEvents.OnSemanticsWarning += (string msg) => { warningMsg = msg; };

            SKOSConceptScheme conceptScheme = new SKOSConceptScheme("ex:conceptScheme");
            conceptScheme.DeclarePreferredLabel(new RDFResource("ex:concept"), new RDFPlainLiteral("label", "en-US"));
            conceptScheme.DeclarePreferredLabel(new RDFResource("ex:concept"), new RDFResource("ex:label"), new RDFPlainLiteral("label2", "en-US")); //SKOS violation

            //Test evolution of SKOS knowledge
            Assert.IsTrue(conceptScheme.Ontology.URI.Equals(conceptScheme.URI));
            Assert.IsTrue(conceptScheme.Ontology.Model.ClassModel.ClassesCount == 8);
            Assert.IsTrue(conceptScheme.Ontology.Model.PropertyModel.PropertiesCount == 33);
            Assert.IsTrue(conceptScheme.Ontology.Data.IndividualsCount == 1);
            Assert.IsTrue(conceptScheme.Ontology.Data.CheckHasAnnotation(new RDFResource("ex:concept"), RDFVocabulary.SKOS.PREF_LABEL, new RDFPlainLiteral("label", "en-US")));
            Assert.IsNotNull(warningMsg);
            Assert.IsTrue(warningMsg.IndexOf("PrefLabel relation between concept 'ex:concept' and label 'ex:label' with value 'label2@EN-US' cannot be declared to the concept scheme because it would violate SKOS integrity") > -1);
        }

        [TestMethod]
        public void ShouldEmitWarningOnDeclaringExtendedPreferredLabelBecauseClashWithHiddenLabel()
        {
            string warningMsg = null;
            OWLSemanticsEvents.OnSemanticsWarning += (string msg) => { warningMsg = msg; };

            SKOSConceptScheme conceptScheme = new SKOSConceptScheme("ex:conceptScheme");
            conceptScheme.DeclareHiddenLabel(new RDFResource("ex:concept"), new RDFPlainLiteral("label"));
            conceptScheme.DeclarePreferredLabel(new RDFResource("ex:concept"), new RDFResource("ex:label"), new RDFPlainLiteral("label")); //SKOS violation

            //Test evolution of SKOS knowledge
            Assert.IsTrue(conceptScheme.Ontology.URI.Equals(conceptScheme.URI));
            Assert.IsTrue(conceptScheme.Ontology.Model.ClassModel.ClassesCount == 8);
            Assert.IsTrue(conceptScheme.Ontology.Model.PropertyModel.PropertiesCount == 33);
            Assert.IsTrue(conceptScheme.Ontology.Data.IndividualsCount == 1);
            Assert.IsTrue(conceptScheme.Ontology.Data.CheckHasAnnotation(new RDFResource("ex:concept"), RDFVocabulary.SKOS.HIDDEN_LABEL, new RDFPlainLiteral("label")));
            Assert.IsNotNull(warningMsg);
            Assert.IsTrue(warningMsg.IndexOf("PrefLabel relation between concept 'ex:concept' and label 'ex:label' with value 'label' cannot be declared to the concept scheme because it would violate SKOS integrity") > -1);
        }

        [TestMethod]
        public void ShouldEmitWarningOnDeclaringExtendedPreferredLabelBecauseClashWithExtendedHiddenLabel()
        {
            string warningMsg = null;
            OWLSemanticsEvents.OnSemanticsWarning += (string msg) => { warningMsg = msg; };

            SKOSConceptScheme conceptScheme = new SKOSConceptScheme("ex:conceptScheme");
            conceptScheme.DeclareHiddenLabel(new RDFResource("ex:concept"), new RDFResource("ex:label"), new RDFPlainLiteral("label"));
            conceptScheme.DeclarePreferredLabel(new RDFResource("ex:concept"), new RDFResource("ex:label"), new RDFPlainLiteral("label")); //SKOS violation

            //Test evolution of SKOS knowledge
            Assert.IsTrue(conceptScheme.Ontology.URI.Equals(conceptScheme.URI));
            Assert.IsTrue(conceptScheme.Ontology.Model.ClassModel.ClassesCount == 8);
            Assert.IsTrue(conceptScheme.Ontology.Model.PropertyModel.PropertiesCount == 33);
            Assert.IsTrue(conceptScheme.Ontology.Data.IndividualsCount == 1);
            Assert.IsTrue(conceptScheme.Ontology.Data.CheckHasObjectAssertion(new RDFResource("ex:concept"), RDFVocabulary.SKOS.SKOSXL.HIDDEN_LABEL, new RDFResource("ex:label")));
            Assert.IsTrue(conceptScheme.Ontology.Data.CheckHasDatatypeAssertion(new RDFResource("ex:label"), RDFVocabulary.SKOS.SKOSXL.LITERAL_FORM, new RDFPlainLiteral("label")));
            Assert.IsNotNull(warningMsg);
            Assert.IsTrue(warningMsg.IndexOf("PrefLabel relation between concept 'ex:concept' and label 'ex:label' with value 'label' cannot be declared to the concept scheme because it would violate SKOS integrity") > -1);
        }

        [TestMethod]
        public void ShouldThrowExceptionOnDeclaringExtendedPreferredLabelBecauseNullConcept()
            => Assert.ThrowsException<OWLSemanticsException>(() => new SKOSConceptScheme("ex:conceptScheme")
                        .DeclarePreferredLabel(null, new RDFResource("ex:label"), new RDFPlainLiteral("label")));

        [TestMethod]
        public void ShouldThrowExceptionOnDeclaringExtendedPreferredLabelBecauseNullLabel()
            => Assert.ThrowsException<OWLSemanticsException>(() => new SKOSConceptScheme("ex:conceptScheme")
                        .DeclarePreferredLabel(new RDFResource("ex:concept"), null, new RDFPlainLiteral("label")));

        [TestMethod]
        public void ShouldThrowExceptionOnDeclaringExtendedPreferredLabelBecauseNullValue()
            => Assert.ThrowsException<OWLSemanticsException>(() => new SKOSConceptScheme("ex:conceptScheme")
                        .DeclarePreferredLabel(new RDFResource("ex:concept"), new RDFResource("ex:label"), null));

        [TestMethod]
        public void ShouldDeclareAlternativeLabel()
        {
            SKOSConceptScheme conceptScheme = new SKOSConceptScheme("ex:conceptScheme");
            conceptScheme.DeclareAlternativeLabel(new RDFResource("ex:concept"), new RDFPlainLiteral("label"));
            conceptScheme.DeclareAlternativeLabel(new RDFResource("ex:concept"), new RDFPlainLiteral("label", "en-US"));
            conceptScheme.DeclareAlternativeLabel(new RDFResource("ex:concept"), new RDFPlainLiteral("etichetta", "it-IT"));

            //Test evolution of SKOS knowledge
            Assert.IsTrue(conceptScheme.Ontology.URI.Equals(conceptScheme.URI));
            Assert.IsTrue(conceptScheme.Ontology.Model.ClassModel.ClassesCount == 8);
            Assert.IsTrue(conceptScheme.Ontology.Model.PropertyModel.PropertiesCount == 33);
            Assert.IsTrue(conceptScheme.Ontology.Data.IndividualsCount == 1);
            Assert.IsTrue(conceptScheme.Ontology.Data.CheckHasAnnotation(new RDFResource("ex:concept"), RDFVocabulary.SKOS.ALT_LABEL, new RDFPlainLiteral("label")));
            Assert.IsTrue(conceptScheme.Ontology.Data.CheckHasAnnotation(new RDFResource("ex:concept"), RDFVocabulary.SKOS.ALT_LABEL, new RDFPlainLiteral("label", "en-US")));
            Assert.IsTrue(conceptScheme.Ontology.Data.CheckHasAnnotation(new RDFResource("ex:concept"), RDFVocabulary.SKOS.ALT_LABEL, new RDFPlainLiteral("etichetta", "it-IT")));
        }

        [TestMethod]
        public void ShouldEmitWarningOnDeclaringAlternativeLabelBecauseClashWithHiddenLabel()
        {
            string warningMsg = null;
            OWLSemanticsEvents.OnSemanticsWarning += (string msg) => { warningMsg = msg; };

            SKOSConceptScheme conceptScheme = new SKOSConceptScheme("ex:conceptScheme");
            conceptScheme.DeclareHiddenLabel(new RDFResource("ex:concept"), new RDFPlainLiteral("label"));
            conceptScheme.DeclareAlternativeLabel(new RDFResource("ex:concept"), new RDFPlainLiteral("label")); //SKOS violation

            //Test evolution of SKOS knowledge
            Assert.IsTrue(conceptScheme.Ontology.URI.Equals(conceptScheme.URI));
            Assert.IsTrue(conceptScheme.Ontology.Model.ClassModel.ClassesCount == 8);
            Assert.IsTrue(conceptScheme.Ontology.Model.PropertyModel.PropertiesCount == 33);
            Assert.IsTrue(conceptScheme.Ontology.Data.IndividualsCount == 1);
            Assert.IsTrue(conceptScheme.Ontology.Data.CheckHasAnnotation(new RDFResource("ex:concept"), RDFVocabulary.SKOS.HIDDEN_LABEL, new RDFPlainLiteral("label")));
            Assert.IsNotNull(warningMsg);
            Assert.IsTrue(warningMsg.IndexOf("AltLabel relation between concept 'ex:concept' and value 'label' cannot be declared to the concept scheme because it would violate SKOS integrity") > -1);
        }

        [TestMethod]
        public void ShouldEmitWarningOnDeclaringAlternativeLabelBecauseClashWithExtendedHiddenLabel()
        {
            string warningMsg = null;
            OWLSemanticsEvents.OnSemanticsWarning += (string msg) => { warningMsg = msg; };

            SKOSConceptScheme conceptScheme = new SKOSConceptScheme("ex:conceptScheme");
            conceptScheme.DeclareHiddenLabel(new RDFResource("ex:concept"), new RDFResource("ex:label"), new RDFPlainLiteral("label"));
            conceptScheme.DeclareAlternativeLabel(new RDFResource("ex:concept"), new RDFPlainLiteral("label")); //SKOS violation

            //Test evolution of SKOS knowledge
            Assert.IsTrue(conceptScheme.Ontology.URI.Equals(conceptScheme.URI));
            Assert.IsTrue(conceptScheme.Ontology.Model.ClassModel.ClassesCount == 8);
            Assert.IsTrue(conceptScheme.Ontology.Model.PropertyModel.PropertiesCount == 33);
            Assert.IsTrue(conceptScheme.Ontology.Data.IndividualsCount == 1);
            Assert.IsTrue(conceptScheme.Ontology.Data.CheckHasObjectAssertion(new RDFResource("ex:concept"), RDFVocabulary.SKOS.SKOSXL.HIDDEN_LABEL, new RDFResource("ex:label")));
            Assert.IsTrue(conceptScheme.Ontology.Data.CheckHasDatatypeAssertion(new RDFResource("ex:label"), RDFVocabulary.SKOS.SKOSXL.LITERAL_FORM, new RDFPlainLiteral("label")));
            Assert.IsNotNull(warningMsg);
            Assert.IsTrue(warningMsg.IndexOf("AltLabel relation between concept 'ex:concept' and value 'label' cannot be declared to the concept scheme because it would violate SKOS integrity") > -1);
        }

        [TestMethod]
        public void ShouldEmitWarningOnDeclaringAlternativeLabelBecauseClashWithPreferredLabel()
        {
            string warningMsg = null;
            OWLSemanticsEvents.OnSemanticsWarning += (string msg) => { warningMsg = msg; };

            SKOSConceptScheme conceptScheme = new SKOSConceptScheme("ex:conceptScheme");
            conceptScheme.DeclarePreferredLabel(new RDFResource("ex:concept"), new RDFPlainLiteral("label"));
            conceptScheme.DeclareAlternativeLabel(new RDFResource("ex:concept"), new RDFPlainLiteral("label")); //SKOS violation

            //Test evolution of SKOS knowledge
            Assert.IsTrue(conceptScheme.Ontology.URI.Equals(conceptScheme.URI));
            Assert.IsTrue(conceptScheme.Ontology.Model.ClassModel.ClassesCount == 8);
            Assert.IsTrue(conceptScheme.Ontology.Model.PropertyModel.PropertiesCount == 33);
            Assert.IsTrue(conceptScheme.Ontology.Data.IndividualsCount == 1);
            Assert.IsTrue(conceptScheme.Ontology.Data.CheckHasAnnotation(new RDFResource("ex:concept"), RDFVocabulary.SKOS.PREF_LABEL, new RDFPlainLiteral("label")));
            Assert.IsNotNull(warningMsg);
            Assert.IsTrue(warningMsg.IndexOf("AltLabel relation between concept 'ex:concept' and value 'label' cannot be declared to the concept scheme because it would violate SKOS integrity") > -1);
        }

        [TestMethod]
        public void ShouldEmitWarningOnDeclaringAlternativeLabelBecauseClashWithExtendedPreferredLabel()
        {
            string warningMsg = null;
            OWLSemanticsEvents.OnSemanticsWarning += (string msg) => { warningMsg = msg; };

            SKOSConceptScheme conceptScheme = new SKOSConceptScheme("ex:conceptScheme");
            conceptScheme.DeclarePreferredLabel(new RDFResource("ex:concept"), new RDFResource("ex:label"), new RDFPlainLiteral("label"));
            conceptScheme.DeclareAlternativeLabel(new RDFResource("ex:concept"), new RDFPlainLiteral("label")); //SKOS violation

            //Test evolution of SKOS knowledge
            Assert.IsTrue(conceptScheme.Ontology.URI.Equals(conceptScheme.URI));
            Assert.IsTrue(conceptScheme.Ontology.Model.ClassModel.ClassesCount == 8);
            Assert.IsTrue(conceptScheme.Ontology.Model.PropertyModel.PropertiesCount == 33);
            Assert.IsTrue(conceptScheme.Ontology.Data.IndividualsCount == 1);
            Assert.IsTrue(conceptScheme.Ontology.Data.CheckHasObjectAssertion(new RDFResource("ex:concept"), RDFVocabulary.SKOS.SKOSXL.PREF_LABEL, new RDFResource("ex:label")));
            Assert.IsTrue(conceptScheme.Ontology.Data.CheckHasDatatypeAssertion(new RDFResource("ex:label"), RDFVocabulary.SKOS.SKOSXL.LITERAL_FORM, new RDFPlainLiteral("label")));
            Assert.IsNotNull(warningMsg);
            Assert.IsTrue(warningMsg.IndexOf("AltLabel relation between concept 'ex:concept' and value 'label' cannot be declared to the concept scheme because it would violate SKOS integrity") > -1);
        }

        [TestMethod]
        public void ShouldThrowExceptionOnDeclaringAlternativeLabelBecauseNullLabel()
            => Assert.ThrowsException<OWLSemanticsException>(() => new SKOSConceptScheme("ex:conceptScheme")
                        .DeclareAlternativeLabel(null, new RDFPlainLiteral("label")));

        [TestMethod]
        public void ShouldThrowExceptionOnDeclaringAlternativeLabelBecauseNullValue()
            => Assert.ThrowsException<OWLSemanticsException>(() => new SKOSConceptScheme("ex:conceptScheme")
                        .DeclareAlternativeLabel(new RDFResource("ex:concept"), null));

        [TestMethod]
        public void ShouldDeclareExtendedAlternativeLabel()
        {
            SKOSConceptScheme conceptScheme = new SKOSConceptScheme("ex:conceptScheme");
            conceptScheme.DeclareAlternativeLabel(new RDFResource("ex:concept"), new RDFResource("ex:label"), new RDFPlainLiteral("label"));
            conceptScheme.DeclareAlternativeLabel(new RDFResource("ex:concept"), new RDFResource("ex:label"), new RDFPlainLiteral("label", "en-US"));
            conceptScheme.DeclareAlternativeLabel(new RDFResource("ex:concept"), new RDFResource("ex:label"), new RDFPlainLiteral("etichetta", "it-IT"));

            //Test evolution of SKOS knowledge
            Assert.IsTrue(conceptScheme.Ontology.URI.Equals(conceptScheme.URI));
            Assert.IsTrue(conceptScheme.Ontology.Model.ClassModel.ClassesCount == 8);
            Assert.IsTrue(conceptScheme.Ontology.Model.PropertyModel.PropertiesCount == 33);
            Assert.IsTrue(conceptScheme.Ontology.Data.IndividualsCount == 1);
            Assert.IsTrue(conceptScheme.Ontology.Data.CheckHasObjectAssertion(new RDFResource("ex:concept"), RDFVocabulary.SKOS.SKOSXL.ALT_LABEL, new RDFResource("ex:label")));
            Assert.IsTrue(conceptScheme.Ontology.Data.CheckHasDatatypeAssertion(new RDFResource("ex:label"), RDFVocabulary.SKOS.SKOSXL.LITERAL_FORM, new RDFPlainLiteral("label", "en-US")));
            Assert.IsTrue(conceptScheme.Ontology.Data.CheckHasDatatypeAssertion(new RDFResource("ex:label"), RDFVocabulary.SKOS.SKOSXL.LITERAL_FORM, new RDFPlainLiteral("label", "en-US")));
            Assert.IsTrue(conceptScheme.Ontology.Data.CheckHasDatatypeAssertion(new RDFResource("ex:label"), RDFVocabulary.SKOS.SKOSXL.LITERAL_FORM, new RDFPlainLiteral("etichetta", "it-IT")));
        }

        [TestMethod]
        public void ShouldEmitWarningOnDeclaringExtendedAlternativeLabelBecauseClashWithHiddenLabel()
        {
            string warningMsg = null;
            OWLSemanticsEvents.OnSemanticsWarning += (string msg) => { warningMsg = msg; };

            SKOSConceptScheme conceptScheme = new SKOSConceptScheme("ex:conceptScheme");
            conceptScheme.DeclareHiddenLabel(new RDFResource("ex:concept"), new RDFPlainLiteral("label"));
            conceptScheme.DeclareAlternativeLabel(new RDFResource("ex:concept"), new RDFResource("ex:label"), new RDFPlainLiteral("label")); //SKOS violation

            //Test evolution of SKOS knowledge
            Assert.IsTrue(conceptScheme.Ontology.URI.Equals(conceptScheme.URI));
            Assert.IsTrue(conceptScheme.Ontology.Model.ClassModel.ClassesCount == 8);
            Assert.IsTrue(conceptScheme.Ontology.Model.PropertyModel.PropertiesCount == 33);
            Assert.IsTrue(conceptScheme.Ontology.Data.IndividualsCount == 1);
            Assert.IsTrue(conceptScheme.Ontology.Data.CheckHasAnnotation(new RDFResource("ex:concept"), RDFVocabulary.SKOS.HIDDEN_LABEL, new RDFPlainLiteral("label")));
            Assert.IsNotNull(warningMsg);
            Assert.IsTrue(warningMsg.IndexOf("AltLabel relation between concept 'ex:concept' and label 'ex:label' with value 'label' cannot be declared to the concept scheme because it would violate SKOS integrity") > -1);
        }

        [TestMethod]
        public void ShouldEmitWarningOnDeclaringExtendedAlternativeLabelBecauseClashWithExtendedHiddenLabel()
        {
            string warningMsg = null;
            OWLSemanticsEvents.OnSemanticsWarning += (string msg) => { warningMsg = msg; };

            SKOSConceptScheme conceptScheme = new SKOSConceptScheme("ex:conceptScheme");
            conceptScheme.DeclareHiddenLabel(new RDFResource("ex:concept"), new RDFResource("ex:label"), new RDFPlainLiteral("label"));
            conceptScheme.DeclareAlternativeLabel(new RDFResource("ex:concept"), new RDFResource("ex:label"), new RDFPlainLiteral("label")); //SKOS violation

            //Test evolution of SKOS knowledge
            Assert.IsTrue(conceptScheme.Ontology.URI.Equals(conceptScheme.URI));
            Assert.IsTrue(conceptScheme.Ontology.Model.ClassModel.ClassesCount == 8);
            Assert.IsTrue(conceptScheme.Ontology.Model.PropertyModel.PropertiesCount == 33);
            Assert.IsTrue(conceptScheme.Ontology.Data.IndividualsCount == 1);
            Assert.IsTrue(conceptScheme.Ontology.Data.CheckHasObjectAssertion(new RDFResource("ex:concept"), RDFVocabulary.SKOS.SKOSXL.HIDDEN_LABEL, new RDFResource("ex:label")));
            Assert.IsTrue(conceptScheme.Ontology.Data.CheckHasDatatypeAssertion(new RDFResource("ex:label"), RDFVocabulary.SKOS.SKOSXL.LITERAL_FORM, new RDFPlainLiteral("label")));
            Assert.IsNotNull(warningMsg);
            Assert.IsTrue(warningMsg.IndexOf("AltLabel relation between concept 'ex:concept' and label 'ex:label' with value 'label' cannot be declared to the concept scheme because it would violate SKOS integrity") > -1);
        }

        [TestMethod]
        public void ShouldEmitWarningOnDeclaringExtendedAlternativeLabelBecauseClashWithPreferredLabel()
        {
            string warningMsg = null;
            OWLSemanticsEvents.OnSemanticsWarning += (string msg) => { warningMsg = msg; };

            SKOSConceptScheme conceptScheme = new SKOSConceptScheme("ex:conceptScheme");
            conceptScheme.DeclarePreferredLabel(new RDFResource("ex:concept"), new RDFPlainLiteral("label"));
            conceptScheme.DeclareAlternativeLabel(new RDFResource("ex:concept"), new RDFResource("ex:label"), new RDFPlainLiteral("label")); //SKOS violation

            //Test evolution of SKOS knowledge
            Assert.IsTrue(conceptScheme.Ontology.URI.Equals(conceptScheme.URI));
            Assert.IsTrue(conceptScheme.Ontology.Model.ClassModel.ClassesCount == 8);
            Assert.IsTrue(conceptScheme.Ontology.Model.PropertyModel.PropertiesCount == 33);
            Assert.IsTrue(conceptScheme.Ontology.Data.IndividualsCount == 1);
            Assert.IsTrue(conceptScheme.Ontology.Data.CheckHasAnnotation(new RDFResource("ex:concept"), RDFVocabulary.SKOS.PREF_LABEL, new RDFPlainLiteral("label")));
            Assert.IsNotNull(warningMsg);
            Assert.IsTrue(warningMsg.IndexOf("AltLabel relation between concept 'ex:concept' and label 'ex:label' with value 'label' cannot be declared to the concept scheme because it would violate SKOS integrity") > -1);
        }

        [TestMethod]
        public void ShouldEmitWarningOnDeclaringExtendedAlternativeLabelBecauseClashWithExtendedPreferredLabel()
        {
            string warningMsg = null;
            OWLSemanticsEvents.OnSemanticsWarning += (string msg) => { warningMsg = msg; };

            SKOSConceptScheme conceptScheme = new SKOSConceptScheme("ex:conceptScheme");
            conceptScheme.DeclarePreferredLabel(new RDFResource("ex:concept"), new RDFResource("ex:label"), new RDFPlainLiteral("label"));
            conceptScheme.DeclareAlternativeLabel(new RDFResource("ex:concept"), new RDFResource("ex:label"), new RDFPlainLiteral("label")); //SKOS violation

            //Test evolution of SKOS knowledge
            Assert.IsTrue(conceptScheme.Ontology.URI.Equals(conceptScheme.URI));
            Assert.IsTrue(conceptScheme.Ontology.Model.ClassModel.ClassesCount == 8);
            Assert.IsTrue(conceptScheme.Ontology.Model.PropertyModel.PropertiesCount == 33);
            Assert.IsTrue(conceptScheme.Ontology.Data.IndividualsCount == 1);
            Assert.IsTrue(conceptScheme.Ontology.Data.CheckHasObjectAssertion(new RDFResource("ex:concept"), RDFVocabulary.SKOS.SKOSXL.PREF_LABEL, new RDFResource("ex:label")));
            Assert.IsTrue(conceptScheme.Ontology.Data.CheckHasDatatypeAssertion(new RDFResource("ex:label"), RDFVocabulary.SKOS.SKOSXL.LITERAL_FORM, new RDFPlainLiteral("label")));
            Assert.IsNotNull(warningMsg);
            Assert.IsTrue(warningMsg.IndexOf("AltLabel relation between concept 'ex:concept' and label 'ex:label' with value 'label' cannot be declared to the concept scheme because it would violate SKOS integrity") > -1);
        }

        [TestMethod]
        public void ShouldThrowExceptionOnDeclaringExtendedAlternativeLabelBecauseNullConcept()
            => Assert.ThrowsException<OWLSemanticsException>(() => new SKOSConceptScheme("ex:conceptScheme")
                        .DeclareAlternativeLabel(null, new RDFResource("ex:label"), new RDFPlainLiteral("label")));

        [TestMethod]
        public void ShouldThrowExceptionOnDeclaringExtendedAlternativeLabelBecauseNullLabel()
            => Assert.ThrowsException<OWLSemanticsException>(() => new SKOSConceptScheme("ex:conceptScheme")
                        .DeclareAlternativeLabel(new RDFResource("ex:concept"), null, new RDFPlainLiteral("label")));

        [TestMethod]
        public void ShouldThrowExceptionOnDeclaringExtendedAlternativeLabelBecauseNullValue()
            => Assert.ThrowsException<OWLSemanticsException>(() => new SKOSConceptScheme("ex:conceptScheme")
                        .DeclareAlternativeLabel(new RDFResource("ex:concept"), new RDFResource("ex:label"), null));
        #endregion
    }
}