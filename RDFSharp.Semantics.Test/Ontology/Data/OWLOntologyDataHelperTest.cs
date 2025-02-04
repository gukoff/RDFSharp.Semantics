﻿/*
   Copyright 2012-2023 Marco De Salvo

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

using System.Collections.Generic;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using RDFSharp.Model;

namespace RDFSharp.Semantics.Test
{
    [TestClass]
    public class OWLOntologyDataHelperTest
    {
        #region Declarer
        [TestMethod]
        public void ShouldCheckHasIndividual()
        {
            OWLOntologyData data = new OWLOntologyData();
            data.DeclareIndividual(new RDFResource("ex:indivA"));

            Assert.IsTrue(data.CheckHasIndividual(new RDFResource("ex:indivA")));
        }

        [TestMethod]
        public void ShouldCheckHasNotIndividual()
        {
            OWLOntologyData data = new OWLOntologyData();
            data.DeclareIndividual(new RDFResource("ex:indivA"));

            Assert.IsFalse(data.CheckHasIndividual(new RDFResource("ex:indivB")));
            Assert.IsFalse(data.CheckHasIndividual(null));
            Assert.IsFalse(new OWLOntologyData().CheckHasIndividual(new RDFResource("ex:indivA")));
        }

        [TestMethod]
        public void ShouldCheckHasResourceAnnotation()
        {
            OWLOntologyData data = new OWLOntologyData();
            data.DeclareIndividual(new RDFResource("ex:indivA"));
            data.AnnotateIndividual(new RDFResource("ex:indivA"), RDFVocabulary.RDFS.SEE_ALSO, new RDFResource("ex:seealso"));

            Assert.IsTrue(data.CheckHasAnnotation(new RDFResource("ex:indivA"), RDFVocabulary.RDFS.SEE_ALSO, new RDFResource("ex:seealso")));
        }

        [TestMethod]
        public void ShouldCheckHasNotResourceAnnotation()
        {
            OWLOntologyData data = new OWLOntologyData();
            data.DeclareIndividual(new RDFResource("ex:indivA"));
            data.AnnotateIndividual(new RDFResource("ex:indivA"), RDFVocabulary.RDFS.SEE_ALSO, new RDFResource("ex:seealso"));

            Assert.IsFalse(data.CheckHasAnnotation(new RDFResource("ex:indivB"), RDFVocabulary.RDFS.SEE_ALSO, new RDFResource("ex:seealso")));
            Assert.IsFalse(data.CheckHasAnnotation(null, RDFVocabulary.RDFS.SEE_ALSO, new RDFResource("ex:seealso")));
            Assert.IsFalse(data.CheckHasAnnotation(new RDFResource("ex:indivA"), null, new RDFResource("ex:seealso")));
            Assert.IsFalse(data.CheckHasAnnotation(new RDFResource("ex:indivA"), RDFVocabulary.RDFS.SEE_ALSO, null as RDFResource));
        }

        [TestMethod]
        public void ShouldCheckHasLiteralAnnotation()
        {
            OWLOntologyData data = new OWLOntologyData();
            data.DeclareIndividual(new RDFResource("ex:indivA"));
            data.AnnotateIndividual(new RDFResource("ex:indivA"), RDFVocabulary.RDFS.LABEL, new RDFPlainLiteral("label"));

            Assert.IsTrue(data.CheckHasAnnotation(new RDFResource("ex:indivA"), RDFVocabulary.RDFS.LABEL, new RDFPlainLiteral("label")));
        }

        [TestMethod]
        public void ShouldCheckHasNotLiteralAnnotation()
        {
            OWLOntologyData data = new OWLOntologyData();
            data.DeclareIndividual(new RDFResource("ex:indivA"));
            data.AnnotateIndividual(new RDFResource("ex:indivA"), RDFVocabulary.RDFS.LABEL, new RDFPlainLiteral("label"));

            Assert.IsFalse(data.CheckHasAnnotation(new RDFResource("ex:indivB"), RDFVocabulary.RDFS.LABEL, new RDFPlainLiteral("label")));
            Assert.IsFalse(data.CheckHasAnnotation(null, RDFVocabulary.RDFS.LABEL, new RDFPlainLiteral("label")));
            Assert.IsFalse(data.CheckHasAnnotation(new RDFResource("ex:indivA"), null, new RDFPlainLiteral("label")));
            Assert.IsFalse(data.CheckHasAnnotation(new RDFResource("ex:indivA"), RDFVocabulary.RDFS.LABEL, null as RDFLiteral));
        }

        [TestMethod]
        public void ShouldCheckHasObjectAssertion()
        {
            OWLOntologyData data = new OWLOntologyData();
            data.DeclareIndividual(new RDFResource("ex:indivA"));
            data.DeclareIndividual(new RDFResource("ex:indivB"));
            data.DeclareObjectAssertion(new RDFResource("ex:indivA"), RDFVocabulary.FOAF.KNOWS, new RDFResource("ex:indivB"));

            Assert.IsTrue(data.CheckHasObjectAssertion(new RDFResource("ex:indivA"), RDFVocabulary.FOAF.KNOWS, new RDFResource("ex:indivB")));
        }

        [TestMethod]
        public void ShouldCheckHasNotObjectAssertion()
        {
            OWLOntologyData data = new OWLOntologyData();
            data.DeclareIndividual(new RDFResource("ex:indivA"));
            data.DeclareIndividual(new RDFResource("ex:indivB"));
            data.DeclareObjectAssertion(new RDFResource("ex:indivA"), RDFVocabulary.FOAF.KNOWS, new RDFResource("ex:indivB"));

            Assert.IsFalse(data.CheckHasObjectAssertion(new RDFResource("ex:indivB"), RDFVocabulary.FOAF.KNOWS, new RDFResource("ex:indivA")));
            Assert.IsFalse(data.CheckHasObjectAssertion(null, RDFVocabulary.FOAF.KNOWS, new RDFResource("ex:indivB")));
            Assert.IsFalse(data.CheckHasObjectAssertion(new RDFResource("ex:indivA"), null, new RDFResource("ex:indivB")));
            Assert.IsFalse(data.CheckHasObjectAssertion(new RDFResource("ex:indivA"), RDFVocabulary.FOAF.KNOWS, null));
        }

        [TestMethod]
        public void ShouldCheckHasDatatypeAssertion()
        {
            OWLOntologyData data = new OWLOntologyData();
            data.DeclareIndividual(new RDFResource("ex:indivA"));
            data.DeclareDatatypeAssertion(new RDFResource("ex:indivA"), RDFVocabulary.FOAF.AGE, new RDFTypedLiteral("25", RDFModelEnums.RDFDatatypes.XSD_INTEGER));

            Assert.IsTrue(data.CheckHasDatatypeAssertion(new RDFResource("ex:indivA"), RDFVocabulary.FOAF.AGE, new RDFTypedLiteral("25", RDFModelEnums.RDFDatatypes.XSD_INTEGER)));
        }

        [TestMethod]
        public void ShouldCheckHasNotDatatypeAssertion()
        {
            OWLOntologyData data = new OWLOntologyData();
            data.DeclareIndividual(new RDFResource("ex:indivA"));
            data.DeclareDatatypeAssertion(new RDFResource("ex:indivA"), RDFVocabulary.FOAF.AGE, new RDFTypedLiteral("25", RDFModelEnums.RDFDatatypes.XSD_INTEGER));

            Assert.IsFalse(data.CheckHasDatatypeAssertion(new RDFResource("ex:indivA"), RDFVocabulary.FOAF.AGE, new RDFTypedLiteral("27", RDFModelEnums.RDFDatatypes.XSD_INTEGER)));
            Assert.IsFalse(data.CheckHasDatatypeAssertion(null, RDFVocabulary.FOAF.AGE, new RDFTypedLiteral("25", RDFModelEnums.RDFDatatypes.XSD_INTEGER)));
            Assert.IsFalse(data.CheckHasDatatypeAssertion(new RDFResource("ex:indivA"), null, new RDFTypedLiteral("25", RDFModelEnums.RDFDatatypes.XSD_INTEGER)));
            Assert.IsFalse(data.CheckHasDatatypeAssertion(new RDFResource("ex:indivA"), RDFVocabulary.FOAF.AGE, null));
        }

        [TestMethod]
        public void ShouldCheckHasNegativeObjectAssertion()
        {
            OWLOntologyData data = new OWLOntologyData();
            data.DeclareIndividual(new RDFResource("ex:indivA"));
            data.DeclareIndividual(new RDFResource("ex:indivB"));
            data.DeclareNegativeObjectAssertion(new RDFResource("ex:indivA"), RDFVocabulary.FOAF.KNOWS, new RDFResource("ex:indivB"));

            Assert.IsTrue(data.CheckHasNegativeObjectAssertion(new RDFResource("ex:indivA"), RDFVocabulary.FOAF.KNOWS, new RDFResource("ex:indivB")));
        }

        [TestMethod]
        public void ShouldCheckHasNotNegativeObjectAssertion()
        {
            OWLOntologyData data = new OWLOntologyData();
            data.DeclareIndividual(new RDFResource("ex:indivA"));
            data.DeclareIndividual(new RDFResource("ex:indivB"));
            data.DeclareNegativeObjectAssertion(new RDFResource("ex:indivA"), RDFVocabulary.FOAF.KNOWS, new RDFResource("ex:indivB"));

            Assert.IsFalse(data.CheckHasNegativeObjectAssertion(new RDFResource("ex:indivB"), RDFVocabulary.FOAF.KNOWS, new RDFResource("ex:indivA")));
            Assert.IsFalse(data.CheckHasNegativeObjectAssertion(null, RDFVocabulary.FOAF.KNOWS, new RDFResource("ex:indivB")));
            Assert.IsFalse(data.CheckHasNegativeObjectAssertion(new RDFResource("ex:indivA"), null, new RDFResource("ex:indivB")));
            Assert.IsFalse(data.CheckHasNegativeObjectAssertion(new RDFResource("ex:indivA"), RDFVocabulary.FOAF.KNOWS, null));
        }

        [TestMethod]
        public void ShouldCheckHasNegativeDatatypeAssertion()
        {
            OWLOntologyData data = new OWLOntologyData();
            data.DeclareIndividual(new RDFResource("ex:indivA"));
            data.DeclareNegativeDatatypeAssertion(new RDFResource("ex:indivA"), RDFVocabulary.FOAF.AGE, new RDFTypedLiteral("25", RDFModelEnums.RDFDatatypes.XSD_INTEGER));

            Assert.IsTrue(data.CheckHasNegativeDatatypeAssertion(new RDFResource("ex:indivA"), RDFVocabulary.FOAF.AGE, new RDFTypedLiteral("25", RDFModelEnums.RDFDatatypes.XSD_INTEGER)));
        }

        [TestMethod]
        public void ShouldCheckHasNotNegativeDatatypeAssertion()
        {
            OWLOntologyData data = new OWLOntologyData();
            data.DeclareIndividual(new RDFResource("ex:indivA"));
            data.DeclareNegativeDatatypeAssertion(new RDFResource("ex:indivA"), RDFVocabulary.FOAF.AGE, new RDFTypedLiteral("25", RDFModelEnums.RDFDatatypes.XSD_INTEGER));

            Assert.IsFalse(data.CheckHasNegativeDatatypeAssertion(new RDFResource("ex:indivA"), RDFVocabulary.FOAF.AGE, new RDFTypedLiteral("27", RDFModelEnums.RDFDatatypes.XSD_INTEGER)));
            Assert.IsFalse(data.CheckHasNegativeDatatypeAssertion(null, RDFVocabulary.FOAF.AGE, new RDFTypedLiteral("25", RDFModelEnums.RDFDatatypes.XSD_INTEGER)));
            Assert.IsFalse(data.CheckHasNegativeDatatypeAssertion(new RDFResource("ex:indivA"), null, new RDFTypedLiteral("25", RDFModelEnums.RDFDatatypes.XSD_INTEGER)));
            Assert.IsFalse(data.CheckHasNegativeDatatypeAssertion(new RDFResource("ex:indivA"), RDFVocabulary.FOAF.AGE, null));
        }
        #endregion

        #region Analyzer
        [TestMethod]
        public void ShouldCheckAreSameIndividuals()
        {
            OWLOntologyData data = new OWLOntologyData();
            data.DeclareIndividual(new RDFResource("ex:indivA"));
            data.DeclareIndividual(new RDFResource("ex:indivB"));
            data.DeclareIndividual(new RDFResource("ex:indivC"));
            data.DeclareSameIndividuals(new RDFResource("ex:indivA"), new RDFResource("ex:indivB"));
            data.DeclareSameIndividuals(new RDFResource("ex:indivB"), new RDFResource("ex:indivC"));

            Assert.IsTrue(data.CheckIsSameIndividual(new RDFResource("ex:indivA"), new RDFResource("ex:indivB")));
            Assert.IsTrue(data.CheckIsSameIndividual(new RDFResource("ex:indivB"), new RDFResource("ex:indivA"))); //Inferred
            Assert.IsTrue(data.CheckIsSameIndividual(new RDFResource("ex:indivB"), new RDFResource("ex:indivC")));
            Assert.IsTrue(data.CheckIsSameIndividual(new RDFResource("ex:indivC"), new RDFResource("ex:indivB"))); //Inferred
            Assert.IsTrue(data.CheckIsSameIndividual(new RDFResource("ex:indivA"), new RDFResource("ex:indivC"))); //Inferred
            Assert.IsTrue(data.CheckIsSameIndividual(new RDFResource("ex:indivC"), new RDFResource("ex:indivA"))); //Inferred
        }

        [TestMethod]
        public void ShouldCheckAreNotSameIndividuals()
        {
            OWLOntologyData data = new OWLOntologyData();
            data.DeclareIndividual(new RDFResource("ex:indivA"));
            data.DeclareIndividual(new RDFResource("ex:indivB"));
            data.DeclareIndividual(new RDFResource("ex:indivC"));
            data.DeclareSameIndividuals(new RDFResource("ex:indivA"), new RDFResource("ex:indivB"));

            Assert.IsFalse(data.CheckIsSameIndividual(new RDFResource("ex:indivA"), new RDFResource("ex:indivC")));
            Assert.IsFalse(data.CheckIsSameIndividual(new RDFResource("ex:indivB"), new RDFResource("ex:indivC")));
            Assert.IsFalse(data.CheckIsSameIndividual(null, new RDFResource("ex:indivB")));
            Assert.IsFalse(data.CheckIsSameIndividual(new RDFResource("ex:indivA"), null));
        }

        [TestMethod]
        public void ShouldCheckAreDifferentIndividuals()
        {
            OWLOntologyData data = new OWLOntologyData();
            data.DeclareIndividual(new RDFResource("ex:indivA"));
            data.DeclareIndividual(new RDFResource("ex:indivB"));
            data.DeclareIndividual(new RDFResource("ex:indivC"));
            data.DeclareIndividual(new RDFResource("ex:indivD"));
            data.DeclareIndividual(new RDFResource("ex:indivE"));
            data.DeclareDifferentIndividuals(new RDFResource("ex:indivA"), new RDFResource("ex:indivB"));
            data.DeclareSameIndividuals(new RDFResource("ex:indivB"), new RDFResource("ex:indivC"));
            data.DeclareDifferentIndividuals(new RDFResource("ex:indivC"), new RDFResource("ex:indivD"));
            data.DeclareSameIndividuals(new RDFResource("ex:indivC"), new RDFResource("ex:indivE"));

            Assert.IsTrue(data.CheckIsDifferentIndividual(new RDFResource("ex:indivA"), new RDFResource("ex:indivB")));
            Assert.IsTrue(data.CheckIsDifferentIndividual(new RDFResource("ex:indivB"), new RDFResource("ex:indivA"))); //Inferred
            Assert.IsTrue(data.CheckIsDifferentIndividual(new RDFResource("ex:indivA"), new RDFResource("ex:indivC"))); //Inferred
            Assert.IsTrue(data.CheckIsDifferentIndividual(new RDFResource("ex:indivC"), new RDFResource("ex:indivA"))); //Inferred
            Assert.IsTrue(data.CheckIsDifferentIndividual(new RDFResource("ex:indivA"), new RDFResource("ex:indivE"))); //Inferred
            Assert.IsTrue(data.CheckIsDifferentIndividual(new RDFResource("ex:indivE"), new RDFResource("ex:indivA"))); //Inferred
            Assert.IsTrue(data.CheckIsDifferentIndividual(new RDFResource("ex:indivB"), new RDFResource("ex:indivD"))); //Inferred
            Assert.IsTrue(data.CheckIsDifferentIndividual(new RDFResource("ex:indivD"), new RDFResource("ex:indivB"))); //Inferred
            Assert.IsTrue(data.CheckIsDifferentIndividual(new RDFResource("ex:indivD"), new RDFResource("ex:indivE"))); //Inferred
            Assert.IsTrue(data.CheckIsDifferentIndividual(new RDFResource("ex:indivE"), new RDFResource("ex:indivD"))); //Inferred
        }

        [TestMethod]
        public void ShouldCheckAreNotDifferentIndividuals()
        {
            OWLOntologyData data = new OWLOntologyData();
            data.DeclareIndividual(new RDFResource("ex:indivA"));
            data.DeclareIndividual(new RDFResource("ex:indivB"));
            data.DeclareIndividual(new RDFResource("ex:indivC"));
            data.DeclareDifferentIndividuals(new RDFResource("ex:indivA"), new RDFResource("ex:indivB"));

            Assert.IsFalse(data.CheckIsDifferentIndividual(new RDFResource("ex:indivA"), new RDFResource("ex:indivC")));
            Assert.IsFalse(data.CheckIsDifferentIndividual(null, new RDFResource("ex:indivC")));
            Assert.IsFalse(data.CheckIsDifferentIndividual(new RDFResource("ex:indivB"), null));
        }

        [TestMethod]
        public void ShouldCheckAreDifferentIndividualsWithAllDifferentShortcut()
        {
            OWLOntologyData data = new OWLOntologyData();
            data.DeclareIndividual(new RDFResource("ex:indivA"));
            data.DeclareIndividual(new RDFResource("ex:indivB"));
            data.DeclareIndividual(new RDFResource("ex:indivC"));
            data.DeclareIndividual(new RDFResource("ex:indivD"));
            data.DeclareIndividual(new RDFResource("ex:indivE"));
            data.DeclareAllDifferentIndividuals(new RDFResource("ex:alldiff"), new List<RDFResource>() {
                new RDFResource("ex:indivA"), new RDFResource("ex:indivB"), new RDFResource("ex:indivD") });
            data.DeclareSameIndividuals(new RDFResource("ex:indivC"), new RDFResource("ex:indivD"));
            data.DeclareSameIndividuals(new RDFResource("ex:indivE"), new RDFResource("ex:indivD"));

            Assert.IsTrue(data.CheckIsDifferentIndividual(new RDFResource("ex:indivA"), new RDFResource("ex:indivB")));
            Assert.IsTrue(data.CheckIsDifferentIndividual(new RDFResource("ex:indivB"), new RDFResource("ex:indivA"))); //Inferred
            Assert.IsTrue(data.CheckIsDifferentIndividual(new RDFResource("ex:indivA"), new RDFResource("ex:indivD")));
            Assert.IsTrue(data.CheckIsDifferentIndividual(new RDFResource("ex:indivD"), new RDFResource("ex:indivA"))); //Inferred
            Assert.IsTrue(data.CheckIsDifferentIndividual(new RDFResource("ex:indivB"), new RDFResource("ex:indivD")));
            Assert.IsTrue(data.CheckIsDifferentIndividual(new RDFResource("ex:indivD"), new RDFResource("ex:indivB"))); //Inferred
            Assert.IsTrue(data.CheckIsDifferentIndividual(new RDFResource("ex:indivA"), new RDFResource("ex:indivC"))); //Inferred
            Assert.IsTrue(data.CheckIsDifferentIndividual(new RDFResource("ex:indivC"), new RDFResource("ex:indivA"))); //Inferred
            Assert.IsTrue(data.CheckIsDifferentIndividual(new RDFResource("ex:indivA"), new RDFResource("ex:indivE"))); //Inferred
            Assert.IsTrue(data.CheckIsDifferentIndividual(new RDFResource("ex:indivE"), new RDFResource("ex:indivA"))); //Inferred
            Assert.IsTrue(data.CheckIsDifferentIndividual(new RDFResource("ex:indivB"), new RDFResource("ex:indivC"))); //Inferred
            Assert.IsTrue(data.CheckIsDifferentIndividual(new RDFResource("ex:indivC"), new RDFResource("ex:indivB"))); //Inferred
            Assert.IsTrue(data.CheckIsDifferentIndividual(new RDFResource("ex:indivB"), new RDFResource("ex:indivE"))); //Inferred
            Assert.IsTrue(data.CheckIsDifferentIndividual(new RDFResource("ex:indivE"), new RDFResource("ex:indivB"))); //Inferred
        }

        [TestMethod]
        public void ShouldCheckAreTransitiveRelatedIndividuals()
        {
            OWLOntologyData data = new OWLOntologyData();
            data.DeclareIndividual(new RDFResource("ex:indivA"));
            data.DeclareIndividual(new RDFResource("ex:indivB"));
            data.DeclareIndividual(new RDFResource("ex:indivC"));
            data.DeclareIndividual(new RDFResource("ex:indivD"));
            data.DeclareObjectAssertion(new RDFResource("ex:indivA"), new RDFResource("ex:genderOf"), new RDFResource("ex:indivB"));
            data.DeclareObjectAssertion(new RDFResource("ex:indivB"), new RDFResource("ex:genderOf"), new RDFResource("ex:indivC"));
            data.DeclareObjectAssertion(new RDFResource("ex:indivC"), new RDFResource("ex:genderOf"), new RDFResource("ex:indivD"));

            Assert.IsTrue(data.CheckIsTransitiveRelatedIndividual(new RDFResource("ex:indivA"), new RDFResource("ex:genderOf"), new RDFResource("ex:indivC")));
            Assert.IsTrue(data.CheckIsTransitiveRelatedIndividual(new RDFResource("ex:indivA"), new RDFResource("ex:genderOf"), new RDFResource("ex:indivD")));
            Assert.IsTrue(data.CheckIsTransitiveRelatedIndividual(new RDFResource("ex:indivB"), new RDFResource("ex:genderOf"), new RDFResource("ex:indivD")));
        }

        [TestMethod]
        public void ShouldCheckAreNotTransitiveRelatedIndividuals()
        {
            OWLOntologyData data = new OWLOntologyData();
            data.DeclareIndividual(new RDFResource("ex:indivA"));
            data.DeclareIndividual(new RDFResource("ex:indivB"));
            data.DeclareIndividual(new RDFResource("ex:indivC"));
            data.DeclareIndividual(new RDFResource("ex:indivD"));
            data.DeclareObjectAssertion(new RDFResource("ex:indivA"), new RDFResource("ex:genderOf"), new RDFResource("ex:indivB"));
            data.DeclareObjectAssertion(new RDFResource("ex:indivC"), new RDFResource("ex:genderOf"), new RDFResource("ex:indivD"));

            Assert.IsFalse(data.CheckIsTransitiveRelatedIndividual(new RDFResource("ex:indivA"), new RDFResource("ex:genderOf"), new RDFResource("ex:indivC")));
            Assert.IsFalse(data.CheckIsTransitiveRelatedIndividual(null, new RDFResource("ex:genderOf"), new RDFResource("ex:indivD")));
            Assert.IsFalse(data.CheckIsTransitiveRelatedIndividual(new RDFResource("ex:indivB"), null, new RDFResource("ex:indivD")));
            Assert.IsFalse(data.CheckIsTransitiveRelatedIndividual(new RDFResource("ex:indivB"), new RDFResource("ex:genderOf"), null));
        }

        [TestMethod]
        public void ShouldCheckIsIndividualOfClass()
        {
            OWLOntology ontology = new OWLOntology("ex:ont");
            ontology.Model.ClassModel.DeclareClass(new RDFResource("ex:classA"));
            ontology.Model.ClassModel.DeclareClass(new RDFResource("ex:classB"));
            ontology.Model.ClassModel.DeclareClass(new RDFResource("ex:classC"));
            ontology.Model.ClassModel.DeclareSubClasses(new RDFResource("ex:classB"), new RDFResource("ex:classA"));
            ontology.Model.ClassModel.DeclareEquivalentClasses(new RDFResource("ex:classC"), new RDFResource("ex:classB"));
            ontology.Data.DeclareIndividual(new RDFResource("ex:indiv1"));
            ontology.Data.DeclareIndividual(new RDFResource("ex:indiv2"));
            ontology.Data.DeclareIndividualType(new RDFResource("ex:indiv1"), new RDFResource("ex:classA"));
            ontology.Data.DeclareIndividualType(new RDFResource("ex:indiv2"), new RDFResource("ex:classB"));
            ontology.Data.DeclareIndividualType(new RDFResource("ex:indiv3"), new RDFResource("ex:classC"));

            Assert.IsTrue(ontology.Data.CheckIsIndividualOf(ontology.Model, new RDFResource("ex:indiv1"), new RDFResource("ex:classA")));
            Assert.IsTrue(ontology.Data.CheckIsIndividualOf(ontology.Model, new RDFResource("ex:indiv2"), new RDFResource("ex:classB")));
            Assert.IsTrue(ontology.Data.CheckIsIndividualOf(ontology.Model, new RDFResource("ex:indiv3"), new RDFResource("ex:classC")));
            Assert.IsTrue(ontology.Data.CheckIsIndividualOf(ontology.Model, new RDFResource("ex:indiv2"), new RDFResource("ex:classA"))); //Inference
            Assert.IsTrue(ontology.Data.CheckIsIndividualOf(ontology.Model, new RDFResource("ex:indiv2"), new RDFResource("ex:classC"))); //Inference
            Assert.IsTrue(ontology.Data.CheckIsIndividualOf(ontology.Model, new RDFResource("ex:indiv3"), new RDFResource("ex:classA"))); //Inference
            Assert.IsTrue(ontology.Data.CheckIsIndividualOf(ontology.Model, new RDFResource("ex:indiv3"), new RDFResource("ex:classB"))); //Inference
        }

        [TestMethod]
        public void ShouldCheckIsIndividualOfEnumerate()
        {
            OWLOntology ontology = new OWLOntology("ex:ont");
            ontology.Model.ClassModel.DeclareEnumerateClass(new RDFResource("ex:enumClass"), new List<RDFResource>() { 
                new RDFResource("ex:indiv1"), new RDFResource("ex:indiv2") });
            ontology.Data.DeclareIndividual(new RDFResource("ex:indiv1"));
            ontology.Data.DeclareIndividual(new RDFResource("ex:indiv2"));

            Assert.IsTrue(ontology.Data.CheckIsIndividualOf(ontology.Model, new RDFResource("ex:indiv1"), new RDFResource("ex:enumClass")));
            Assert.IsTrue(ontology.Data.CheckIsIndividualOf(ontology.Model, new RDFResource("ex:indiv2"), new RDFResource("ex:enumClass")));
        }

        [TestMethod]
        public void ShouldCheckIsIndividualOfZeroCardinalityRestriction()
        {
            OWLOntology ontology = new OWLOntology("ex:ont");
            ontology.Model.ClassModel.DeclareCardinalityRestriction(new RDFResource("ex:cardRest"), new RDFResource("ex:objProp"), 0);
            ontology.Model.PropertyModel.DeclareObjectProperty(new RDFResource("ex:objProp"));
            ontology.Data.DeclareIndividual(new RDFResource("ex:indiv1"));
            ontology.Data.DeclareIndividual(new RDFResource("ex:indiv2"));
            ontology.Data.DeclareIndividual(new RDFResource("ex:indiv3"));
            ontology.Data.DeclareObjectAssertion(new RDFResource("ex:indiv1"), new RDFResource("ex:objProp"), new RDFResource("ex:indiv2"));
            ontology.Data.DeclareObjectAssertion(new RDFResource("ex:indiv1"), new RDFResource("ex:objProp"), new RDFResource("ex:indiv3"));
            ontology.Data.DeclareObjectAssertion(new RDFResource("ex:indiv2"), new RDFResource("ex:objProp"), new RDFResource("ex:indiv3"));

            Assert.IsFalse(ontology.Data.CheckIsIndividualOf(ontology.Model, new RDFResource("ex:indiv1"), new RDFResource("ex:cardRest")));
            Assert.IsFalse(ontology.Data.CheckIsIndividualOf(ontology.Model, new RDFResource("ex:indiv2"), new RDFResource("ex:cardRest")));
            Assert.IsTrue(ontology.Data.CheckIsIndividualOf(ontology.Model, new RDFResource("ex:indiv3"), new RDFResource("ex:cardRest")));
        }

        [TestMethod]
        public void ShouldCheckIsIndividualOfNonZeroCardinalityRestriction()
        {
            OWLOntology ontology = new OWLOntology("ex:ont");
            ontology.Model.ClassModel.DeclareCardinalityRestriction(new RDFResource("ex:cardRest"), new RDFResource("ex:objProp"), 2);
            ontology.Model.PropertyModel.DeclareObjectProperty(new RDFResource("ex:objProp"));
            ontology.Data.DeclareIndividual(new RDFResource("ex:indiv1"));
            ontology.Data.DeclareIndividual(new RDFResource("ex:indiv2"));
            ontology.Data.DeclareIndividual(new RDFResource("ex:indiv3"));
            ontology.Data.DeclareObjectAssertion(new RDFResource("ex:indiv1"), new RDFResource("ex:objProp"), new RDFResource("ex:indiv2"));
            ontology.Data.DeclareObjectAssertion(new RDFResource("ex:indiv1"), new RDFResource("ex:objProp"), new RDFResource("ex:indiv3"));
            ontology.Data.DeclareObjectAssertion(new RDFResource("ex:indiv2"), new RDFResource("ex:objProp"), new RDFResource("ex:indiv3"));

            Assert.IsTrue(ontology.Data.CheckIsIndividualOf(ontology.Model, new RDFResource("ex:indiv1"), new RDFResource("ex:cardRest")));
            Assert.IsFalse(ontology.Data.CheckIsIndividualOf(ontology.Model, new RDFResource("ex:indiv2"), new RDFResource("ex:cardRest")));
            Assert.IsFalse(ontology.Data.CheckIsIndividualOf(ontology.Model, new RDFResource("ex:indiv3"), new RDFResource("ex:cardRest")));
        }

        [TestMethod]
        public void ShouldCheckIsIndividualOfZeroCardinalityRestrictionWithReasoningOnProperty()
        {
            OWLOntology ontology = new OWLOntology("ex:ont");
            ontology.Model.ClassModel.DeclareCardinalityRestriction(new RDFResource("ex:cardRest"), new RDFResource("ex:objMotherProp"), 0);
            ontology.Model.PropertyModel.DeclareObjectProperty(new RDFResource("ex:objChildProp"));
            ontology.Model.PropertyModel.DeclareObjectProperty(new RDFResource("ex:objMotherProp"));
            ontology.Model.PropertyModel.DeclareSubProperties(new RDFResource("ex:objChildProp"), new RDFResource("ex:objMotherProp"));
            ontology.Data.DeclareIndividual(new RDFResource("ex:indiv1"));
            ontology.Data.DeclareIndividual(new RDFResource("ex:indiv2"));
            ontology.Data.DeclareIndividual(new RDFResource("ex:indiv3"));
            ontology.Data.DeclareObjectAssertion(new RDFResource("ex:indiv1"), new RDFResource("ex:objChildProp"), new RDFResource("ex:indiv2"));
            ontology.Data.DeclareObjectAssertion(new RDFResource("ex:indiv1"), new RDFResource("ex:objChildProp"), new RDFResource("ex:indiv3"));
            ontology.Data.DeclareObjectAssertion(new RDFResource("ex:indiv2"), new RDFResource("ex:objChildProp"), new RDFResource("ex:indiv3"));

            Assert.IsFalse(ontology.Data.CheckIsIndividualOf(ontology.Model, new RDFResource("ex:indiv1"), new RDFResource("ex:cardRest")));
            Assert.IsFalse(ontology.Data.CheckIsIndividualOf(ontology.Model, new RDFResource("ex:indiv2"), new RDFResource("ex:cardRest")));
            Assert.IsTrue(ontology.Data.CheckIsIndividualOf(ontology.Model, new RDFResource("ex:indiv3"), new RDFResource("ex:cardRest")));
        }

        [TestMethod]
        public void ShouldCheckIsIndividualOfNonZeroCardinalityRestrictionWithReasoningOnProperty()
        {
            OWLOntology ontology = new OWLOntology("ex:ont");
            ontology.Model.ClassModel.DeclareCardinalityRestriction(new RDFResource("ex:cardRest"), new RDFResource("ex:objMotherProp"), 2);
            ontology.Model.PropertyModel.DeclareObjectProperty(new RDFResource("ex:objChildProp"));
            ontology.Model.PropertyModel.DeclareObjectProperty(new RDFResource("ex:objMotherProp"));
            ontology.Model.PropertyModel.DeclareSubProperties(new RDFResource("ex:objChildProp"), new RDFResource("ex:objMotherProp"));
            ontology.Data.DeclareIndividual(new RDFResource("ex:indiv1"));
            ontology.Data.DeclareIndividual(new RDFResource("ex:indiv2"));
            ontology.Data.DeclareIndividual(new RDFResource("ex:indiv3"));
            ontology.Data.DeclareObjectAssertion(new RDFResource("ex:indiv1"), new RDFResource("ex:objChildProp"), new RDFResource("ex:indiv2"));
            ontology.Data.DeclareObjectAssertion(new RDFResource("ex:indiv1"), new RDFResource("ex:objChildProp"), new RDFResource("ex:indiv3"));
            ontology.Data.DeclareObjectAssertion(new RDFResource("ex:indiv2"), new RDFResource("ex:objChildProp"), new RDFResource("ex:indiv3"));

            Assert.IsTrue(ontology.Data.CheckIsIndividualOf(ontology.Model, new RDFResource("ex:indiv1"), new RDFResource("ex:cardRest")));
            Assert.IsFalse(ontology.Data.CheckIsIndividualOf(ontology.Model, new RDFResource("ex:indiv2"), new RDFResource("ex:cardRest")));
            Assert.IsFalse(ontology.Data.CheckIsIndividualOf(ontology.Model, new RDFResource("ex:indiv3"), new RDFResource("ex:cardRest")));
        }

        [TestMethod]
        public void ShouldCheckIsIndividualOfZeroQualifiedCardinalityRestriction()
        {
            OWLOntology ontology = new OWLOntology("ex:ont");
            ontology.Model.ClassModel.DeclareClass(new RDFResource("ex:onClass"));
            ontology.Model.ClassModel.DeclareQualifiedCardinalityRestriction(new RDFResource("ex:qCardRest"), new RDFResource("ex:objProp"), 0, new RDFResource("ex:onClass"));
            ontology.Model.PropertyModel.DeclareObjectProperty(new RDFResource("ex:objProp"));
            ontology.Data.DeclareIndividual(new RDFResource("ex:indiv1"));
            ontology.Data.DeclareIndividual(new RDFResource("ex:indiv2"));
            ontology.Data.DeclareIndividual(new RDFResource("ex:indiv3"));
            ontology.Data.DeclareIndividual(new RDFResource("ex:indiv4"));
            ontology.Data.DeclareIndividual(new RDFResource("ex:indiv5"));
            ontology.Data.DeclareIndividual(new RDFResource("ex:indiv6"));
            ontology.Data.DeclareIndividualType(new RDFResource("ex:indiv2"), new RDFResource("ex:onClass"));
            ontology.Data.DeclareIndividualType(new RDFResource("ex:indiv3"), new RDFResource("ex:onClass"));
            ontology.Data.DeclareIndividualType(new RDFResource("ex:indiv5"), new RDFResource("ex:onClass"));
            ontology.Data.DeclareIndividualType(new RDFResource("ex:indiv6"), new RDFResource("ex:onClass"));
            ontology.Data.DeclareObjectAssertion(new RDFResource("ex:indiv1"), new RDFResource("ex:objProp"), new RDFResource("ex:indiv2"));
            ontology.Data.DeclareObjectAssertion(new RDFResource("ex:indiv1"), new RDFResource("ex:objProp"), new RDFResource("ex:indiv3"));
            ontology.Data.DeclareObjectAssertion(new RDFResource("ex:indiv2"), new RDFResource("ex:objProp"), new RDFResource("ex:indiv3"));
            ontology.Data.DeclareObjectAssertion(new RDFResource("ex:indiv2"), new RDFResource("ex:objProp"), new RDFResource("ex:indiv4"));
            ontology.Data.DeclareObjectAssertion(new RDFResource("ex:indiv3"), new RDFResource("ex:objProp"), new RDFResource("ex:indiv3"));
            ontology.Data.DeclareObjectAssertion(new RDFResource("ex:indiv3"), new RDFResource("ex:objProp"), new RDFResource("ex:indiv4"));
            ontology.Data.DeclareObjectAssertion(new RDFResource("ex:indiv3"), new RDFResource("ex:objProp"), new RDFResource("ex:indiv5"));
            ontology.Data.DeclareObjectAssertion(new RDFResource("ex:indiv3"), new RDFResource("ex:objProp"), new RDFResource("ex:indiv6"));
            ontology.Data.DeclareObjectAssertion(new RDFResource("ex:indiv4"), new RDFResource("ex:objProp"), new RDFResource("ex:indiv1"));

            Assert.IsFalse(ontology.Data.CheckIsIndividualOf(ontology.Model, new RDFResource("ex:indiv1"), new RDFResource("ex:qCardRest")));
            Assert.IsFalse(ontology.Data.CheckIsIndividualOf(ontology.Model, new RDFResource("ex:indiv2"), new RDFResource("ex:qCardRest")));
            Assert.IsFalse(ontology.Data.CheckIsIndividualOf(ontology.Model, new RDFResource("ex:indiv3"), new RDFResource("ex:qCardRest")));
            Assert.IsTrue(ontology.Data.CheckIsIndividualOf(ontology.Model, new RDFResource("ex:indiv4"), new RDFResource("ex:qCardRest")));
            Assert.IsTrue(ontology.Data.CheckIsIndividualOf(ontology.Model, new RDFResource("ex:indiv5"), new RDFResource("ex:qCardRest")));
            Assert.IsTrue(ontology.Data.CheckIsIndividualOf(ontology.Model, new RDFResource("ex:indiv6"), new RDFResource("ex:qCardRest")));
        }

        [TestMethod]
        public void ShouldCheckIsIndividualOfNonZeroQualifiedCardinalityRestriction()
        {
            OWLOntology ontology = new OWLOntology("ex:ont");
            ontology.Model.ClassModel.DeclareClass(new RDFResource("ex:onClass"));
            ontology.Model.ClassModel.DeclareQualifiedCardinalityRestriction(new RDFResource("ex:qCardRest"), new RDFResource("ex:objProp"), 2, new RDFResource("ex:onClass"));
            ontology.Model.PropertyModel.DeclareObjectProperty(new RDFResource("ex:objProp"));
            ontology.Data.DeclareIndividual(new RDFResource("ex:indiv1"));
            ontology.Data.DeclareIndividual(new RDFResource("ex:indiv2"));
            ontology.Data.DeclareIndividual(new RDFResource("ex:indiv3"));
            ontology.Data.DeclareIndividual(new RDFResource("ex:indiv4"));
            ontology.Data.DeclareIndividual(new RDFResource("ex:indiv5"));
            ontology.Data.DeclareIndividual(new RDFResource("ex:indiv6"));
            ontology.Data.DeclareIndividualType(new RDFResource("ex:indiv2"), new RDFResource("ex:onClass"));
            ontology.Data.DeclareIndividualType(new RDFResource("ex:indiv3"), new RDFResource("ex:onClass"));
            ontology.Data.DeclareIndividualType(new RDFResource("ex:indiv5"), new RDFResource("ex:onClass"));
            ontology.Data.DeclareIndividualType(new RDFResource("ex:indiv6"), new RDFResource("ex:onClass"));
            ontology.Data.DeclareObjectAssertion(new RDFResource("ex:indiv1"), new RDFResource("ex:objProp"), new RDFResource("ex:indiv2"));
            ontology.Data.DeclareObjectAssertion(new RDFResource("ex:indiv1"), new RDFResource("ex:objProp"), new RDFResource("ex:indiv3"));
            ontology.Data.DeclareObjectAssertion(new RDFResource("ex:indiv2"), new RDFResource("ex:objProp"), new RDFResource("ex:indiv3"));
            ontology.Data.DeclareObjectAssertion(new RDFResource("ex:indiv2"), new RDFResource("ex:objProp"), new RDFResource("ex:indiv4"));
            ontology.Data.DeclareObjectAssertion(new RDFResource("ex:indiv3"), new RDFResource("ex:objProp"), new RDFResource("ex:indiv3"));
            ontology.Data.DeclareObjectAssertion(new RDFResource("ex:indiv3"), new RDFResource("ex:objProp"), new RDFResource("ex:indiv4"));
            ontology.Data.DeclareObjectAssertion(new RDFResource("ex:indiv3"), new RDFResource("ex:objProp"), new RDFResource("ex:indiv5"));
            ontology.Data.DeclareObjectAssertion(new RDFResource("ex:indiv3"), new RDFResource("ex:objProp"), new RDFResource("ex:indiv6"));
            ontology.Data.DeclareObjectAssertion(new RDFResource("ex:indiv4"), new RDFResource("ex:objProp"), new RDFResource("ex:indiv1"));
            ontology.Data.DeclareObjectAssertion(new RDFResource("ex:indiv4"), new RDFResource("ex:objProp"), new RDFResource("ex:indiv4"));

            Assert.IsTrue(ontology.Data.CheckIsIndividualOf(ontology.Model, new RDFResource("ex:indiv1"), new RDFResource("ex:qCardRest")));
            Assert.IsFalse(ontology.Data.CheckIsIndividualOf(ontology.Model, new RDFResource("ex:indiv2"), new RDFResource("ex:qCardRest")));
            Assert.IsFalse(ontology.Data.CheckIsIndividualOf(ontology.Model, new RDFResource("ex:indiv3"), new RDFResource("ex:qCardRest")));
            Assert.IsFalse(ontology.Data.CheckIsIndividualOf(ontology.Model, new RDFResource("ex:indiv4"), new RDFResource("ex:qCardRest")));
            Assert.IsFalse(ontology.Data.CheckIsIndividualOf(ontology.Model, new RDFResource("ex:indiv5"), new RDFResource("ex:qCardRest")));
            Assert.IsFalse(ontology.Data.CheckIsIndividualOf(ontology.Model, new RDFResource("ex:indiv6"), new RDFResource("ex:qCardRest")));
        }

        [TestMethod]
        public void ShouldCheckIsIndividualOfZeroQualifiedCardinalityRestrictionWithReasoningOnProperty()
        {
            OWLOntology ontology = new OWLOntology("ex:ont");
            ontology.Model.ClassModel.DeclareClass(new RDFResource("ex:onClass"));
            ontology.Model.ClassModel.DeclareQualifiedCardinalityRestriction(new RDFResource("ex:qCardRest"), new RDFResource("ex:objProp"), 0, new RDFResource("ex:onClass"));
            ontology.Model.PropertyModel.DeclareObjectProperty(new RDFResource("ex:objProp"));
            ontology.Model.PropertyModel.DeclareObjectProperty(new RDFResource("ex:objPropEquiv"));
            ontology.Model.PropertyModel.DeclareEquivalentProperties(new RDFResource("ex:objProp"), new RDFResource("ex:objPropEquiv"));
            ontology.Data.DeclareIndividual(new RDFResource("ex:indiv1"));
            ontology.Data.DeclareIndividual(new RDFResource("ex:indiv2"));
            ontology.Data.DeclareIndividual(new RDFResource("ex:indiv3"));
            ontology.Data.DeclareIndividual(new RDFResource("ex:indiv4"));
            ontology.Data.DeclareIndividual(new RDFResource("ex:indiv5"));
            ontology.Data.DeclareIndividual(new RDFResource("ex:indiv6"));
            ontology.Data.DeclareIndividualType(new RDFResource("ex:indiv2"), new RDFResource("ex:onClass"));
            ontology.Data.DeclareIndividualType(new RDFResource("ex:indiv3"), new RDFResource("ex:onClass"));
            ontology.Data.DeclareIndividualType(new RDFResource("ex:indiv5"), new RDFResource("ex:onClass"));
            ontology.Data.DeclareIndividualType(new RDFResource("ex:indiv6"), new RDFResource("ex:onClass"));
            ontology.Data.DeclareObjectAssertion(new RDFResource("ex:indiv1"), new RDFResource("ex:objPropEquiv"), new RDFResource("ex:indiv2"));
            ontology.Data.DeclareObjectAssertion(new RDFResource("ex:indiv1"), new RDFResource("ex:objPropEquiv"), new RDFResource("ex:indiv3"));
            ontology.Data.DeclareObjectAssertion(new RDFResource("ex:indiv2"), new RDFResource("ex:objPropEquiv"), new RDFResource("ex:indiv3"));
            ontology.Data.DeclareObjectAssertion(new RDFResource("ex:indiv2"), new RDFResource("ex:objPropEquiv"), new RDFResource("ex:indiv4"));
            ontology.Data.DeclareObjectAssertion(new RDFResource("ex:indiv3"), new RDFResource("ex:objPropEquiv"), new RDFResource("ex:indiv3"));
            ontology.Data.DeclareObjectAssertion(new RDFResource("ex:indiv3"), new RDFResource("ex:objPropEquiv"), new RDFResource("ex:indiv4"));
            ontology.Data.DeclareObjectAssertion(new RDFResource("ex:indiv3"), new RDFResource("ex:objPropEquiv"), new RDFResource("ex:indiv5"));
            ontology.Data.DeclareObjectAssertion(new RDFResource("ex:indiv3"), new RDFResource("ex:objPropEquiv"), new RDFResource("ex:indiv6"));

            Assert.IsFalse(ontology.Data.CheckIsIndividualOf(ontology.Model, new RDFResource("ex:indiv1"), new RDFResource("ex:qCardRest")));
            Assert.IsFalse(ontology.Data.CheckIsIndividualOf(ontology.Model, new RDFResource("ex:indiv2"), new RDFResource("ex:qCardRest")));
            Assert.IsFalse(ontology.Data.CheckIsIndividualOf(ontology.Model, new RDFResource("ex:indiv3"), new RDFResource("ex:qCardRest")));
            Assert.IsTrue(ontology.Data.CheckIsIndividualOf(ontology.Model, new RDFResource("ex:indiv4"), new RDFResource("ex:qCardRest")));
            Assert.IsTrue(ontology.Data.CheckIsIndividualOf(ontology.Model, new RDFResource("ex:indiv5"), new RDFResource("ex:qCardRest")));
            Assert.IsTrue(ontology.Data.CheckIsIndividualOf(ontology.Model, new RDFResource("ex:indiv6"), new RDFResource("ex:qCardRest")));
        }

        [TestMethod]
        public void ShouldCheckIsIndividualOfNonZeroQualifiedCardinalityRestrictionWithReasoningOnProperty()
        {
            OWLOntology ontology = new OWLOntology("ex:ont");
            ontology.Model.ClassModel.DeclareClass(new RDFResource("ex:onClass"));
            ontology.Model.ClassModel.DeclareQualifiedCardinalityRestriction(new RDFResource("ex:qCardRest"), new RDFResource("ex:objProp"), 2, new RDFResource("ex:onClass"));
            ontology.Model.PropertyModel.DeclareObjectProperty(new RDFResource("ex:objProp"));
            ontology.Model.PropertyModel.DeclareObjectProperty(new RDFResource("ex:objPropEquiv"));
            ontology.Model.PropertyModel.DeclareEquivalentProperties(new RDFResource("ex:objProp"), new RDFResource("ex:objPropEquiv"));
            ontology.Data.DeclareIndividual(new RDFResource("ex:indiv1"));
            ontology.Data.DeclareIndividual(new RDFResource("ex:indiv2"));
            ontology.Data.DeclareIndividual(new RDFResource("ex:indiv3"));
            ontology.Data.DeclareIndividual(new RDFResource("ex:indiv4"));
            ontology.Data.DeclareIndividual(new RDFResource("ex:indiv5"));
            ontology.Data.DeclareIndividual(new RDFResource("ex:indiv6"));
            ontology.Data.DeclareIndividualType(new RDFResource("ex:indiv2"), new RDFResource("ex:onClass"));
            ontology.Data.DeclareIndividualType(new RDFResource("ex:indiv3"), new RDFResource("ex:onClass"));
            ontology.Data.DeclareIndividualType(new RDFResource("ex:indiv5"), new RDFResource("ex:onClass"));
            ontology.Data.DeclareIndividualType(new RDFResource("ex:indiv6"), new RDFResource("ex:onClass"));
            ontology.Data.DeclareObjectAssertion(new RDFResource("ex:indiv1"), new RDFResource("ex:objPropEquiv"), new RDFResource("ex:indiv2"));
            ontology.Data.DeclareObjectAssertion(new RDFResource("ex:indiv1"), new RDFResource("ex:objPropEquiv"), new RDFResource("ex:indiv3"));
            ontology.Data.DeclareObjectAssertion(new RDFResource("ex:indiv2"), new RDFResource("ex:objPropEquiv"), new RDFResource("ex:indiv3"));
            ontology.Data.DeclareObjectAssertion(new RDFResource("ex:indiv2"), new RDFResource("ex:objPropEquiv"), new RDFResource("ex:indiv4"));
            ontology.Data.DeclareObjectAssertion(new RDFResource("ex:indiv3"), new RDFResource("ex:objPropEquiv"), new RDFResource("ex:indiv3"));
            ontology.Data.DeclareObjectAssertion(new RDFResource("ex:indiv3"), new RDFResource("ex:objPropEquiv"), new RDFResource("ex:indiv4"));
            ontology.Data.DeclareObjectAssertion(new RDFResource("ex:indiv3"), new RDFResource("ex:objPropEquiv"), new RDFResource("ex:indiv5"));
            ontology.Data.DeclareObjectAssertion(new RDFResource("ex:indiv3"), new RDFResource("ex:objPropEquiv"), new RDFResource("ex:indiv6"));

            Assert.IsTrue(ontology.Data.CheckIsIndividualOf(ontology.Model, new RDFResource("ex:indiv1"), new RDFResource("ex:qCardRest")));
            Assert.IsFalse(ontology.Data.CheckIsIndividualOf(ontology.Model, new RDFResource("ex:indiv2"), new RDFResource("ex:qCardRest")));
            Assert.IsFalse(ontology.Data.CheckIsIndividualOf(ontology.Model, new RDFResource("ex:indiv3"), new RDFResource("ex:qCardRest")));
            Assert.IsFalse(ontology.Data.CheckIsIndividualOf(ontology.Model, new RDFResource("ex:indiv4"), new RDFResource("ex:qCardRest")));
            Assert.IsFalse(ontology.Data.CheckIsIndividualOf(ontology.Model, new RDFResource("ex:indiv5"), new RDFResource("ex:qCardRest")));
            Assert.IsFalse(ontology.Data.CheckIsIndividualOf(ontology.Model, new RDFResource("ex:indiv6"), new RDFResource("ex:qCardRest")));
        }

        [TestMethod]
        public void ShouldCheckIsIndividualOfZeroMinCardinalityRestriction()
        {
            OWLOntology ontology = new OWLOntology("ex:ont");
            ontology.Model.ClassModel.DeclareMinCardinalityRestriction(new RDFResource("ex:cardRest"), new RDFResource("ex:objProp"), 0);
            ontology.Model.PropertyModel.DeclareObjectProperty(new RDFResource("ex:objProp"));
            ontology.Data.DeclareIndividual(new RDFResource("ex:indiv1"));
            ontology.Data.DeclareIndividual(new RDFResource("ex:indiv2"));
            ontology.Data.DeclareIndividual(new RDFResource("ex:indiv3"));
            ontology.Data.DeclareObjectAssertion(new RDFResource("ex:indiv1"), new RDFResource("ex:objProp"), new RDFResource("ex:indiv2"));
            ontology.Data.DeclareObjectAssertion(new RDFResource("ex:indiv1"), new RDFResource("ex:objProp"), new RDFResource("ex:indiv3"));
            ontology.Data.DeclareObjectAssertion(new RDFResource("ex:indiv2"), new RDFResource("ex:objProp"), new RDFResource("ex:indiv3"));

            Assert.IsTrue(ontology.Data.CheckIsIndividualOf(ontology.Model, new RDFResource("ex:indiv1"), new RDFResource("ex:cardRest")));
            Assert.IsTrue(ontology.Data.CheckIsIndividualOf(ontology.Model, new RDFResource("ex:indiv2"), new RDFResource("ex:cardRest")));
            Assert.IsTrue(ontology.Data.CheckIsIndividualOf(ontology.Model, new RDFResource("ex:indiv3"), new RDFResource("ex:cardRest")));
        }

        [TestMethod]
        public void ShouldCheckIsIndividualOfNonZeroMinCardinalityRestriction()
        {
            OWLOntology ontology = new OWLOntology("ex:ont");
            ontology.Model.ClassModel.DeclareMinCardinalityRestriction(new RDFResource("ex:cardRest"), new RDFResource("ex:objProp"), 1);
            ontology.Model.PropertyModel.DeclareObjectProperty(new RDFResource("ex:objProp"));
            ontology.Data.DeclareIndividual(new RDFResource("ex:indiv1"));
            ontology.Data.DeclareIndividual(new RDFResource("ex:indiv2"));
            ontology.Data.DeclareIndividual(new RDFResource("ex:indiv3"));
            ontology.Data.DeclareObjectAssertion(new RDFResource("ex:indiv1"), new RDFResource("ex:objProp"), new RDFResource("ex:indiv2"));
            ontology.Data.DeclareObjectAssertion(new RDFResource("ex:indiv1"), new RDFResource("ex:objProp"), new RDFResource("ex:indiv3"));
            ontology.Data.DeclareObjectAssertion(new RDFResource("ex:indiv2"), new RDFResource("ex:objProp"), new RDFResource("ex:indiv3"));

            Assert.IsTrue(ontology.Data.CheckIsIndividualOf(ontology.Model, new RDFResource("ex:indiv1"), new RDFResource("ex:cardRest")));
            Assert.IsTrue(ontology.Data.CheckIsIndividualOf(ontology.Model, new RDFResource("ex:indiv2"), new RDFResource("ex:cardRest")));
            Assert.IsFalse(ontology.Data.CheckIsIndividualOf(ontology.Model, new RDFResource("ex:indiv3"), new RDFResource("ex:cardRest")));
        }

        [TestMethod]
        public void ShouldCheckIsIndividualOfZeroMinQualifiedCardinalityRestriction()
        {
            OWLOntology ontology = new OWLOntology("ex:ont");
            ontology.Model.ClassModel.DeclareClass(new RDFResource("ex:onClass"));
            ontology.Model.ClassModel.DeclareMinQualifiedCardinalityRestriction(new RDFResource("ex:qCardRest"), new RDFResource("ex:objProp"), 0, new RDFResource("ex:onClass"));
            ontology.Model.PropertyModel.DeclareObjectProperty(new RDFResource("ex:objProp"));
            ontology.Data.DeclareIndividual(new RDFResource("ex:indiv1"));
            ontology.Data.DeclareIndividual(new RDFResource("ex:indiv2"));
            ontology.Data.DeclareIndividual(new RDFResource("ex:indiv3"));
            ontology.Data.DeclareIndividual(new RDFResource("ex:indiv4"));
            ontology.Data.DeclareIndividual(new RDFResource("ex:indiv5"));
            ontology.Data.DeclareIndividual(new RDFResource("ex:indiv6"));
            ontology.Data.DeclareIndividualType(new RDFResource("ex:indiv2"), new RDFResource("ex:onClass"));
            ontology.Data.DeclareIndividualType(new RDFResource("ex:indiv3"), new RDFResource("ex:onClass"));
            ontology.Data.DeclareIndividualType(new RDFResource("ex:indiv5"), new RDFResource("ex:onClass"));
            ontology.Data.DeclareIndividualType(new RDFResource("ex:indiv6"), new RDFResource("ex:onClass"));
            ontology.Data.DeclareObjectAssertion(new RDFResource("ex:indiv1"), new RDFResource("ex:objProp"), new RDFResource("ex:indiv2"));
            ontology.Data.DeclareObjectAssertion(new RDFResource("ex:indiv1"), new RDFResource("ex:objProp"), new RDFResource("ex:indiv3"));
            ontology.Data.DeclareObjectAssertion(new RDFResource("ex:indiv2"), new RDFResource("ex:objProp"), new RDFResource("ex:indiv3"));
            ontology.Data.DeclareObjectAssertion(new RDFResource("ex:indiv2"), new RDFResource("ex:objProp"), new RDFResource("ex:indiv4"));
            ontology.Data.DeclareObjectAssertion(new RDFResource("ex:indiv3"), new RDFResource("ex:objProp"), new RDFResource("ex:indiv3"));
            ontology.Data.DeclareObjectAssertion(new RDFResource("ex:indiv3"), new RDFResource("ex:objProp"), new RDFResource("ex:indiv4"));
            ontology.Data.DeclareObjectAssertion(new RDFResource("ex:indiv3"), new RDFResource("ex:objProp"), new RDFResource("ex:indiv5"));
            ontology.Data.DeclareObjectAssertion(new RDFResource("ex:indiv3"), new RDFResource("ex:objProp"), new RDFResource("ex:indiv6"));

            Assert.IsTrue(ontology.Data.CheckIsIndividualOf(ontology.Model, new RDFResource("ex:indiv1"), new RDFResource("ex:qCardRest")));
            Assert.IsTrue(ontology.Data.CheckIsIndividualOf(ontology.Model, new RDFResource("ex:indiv2"), new RDFResource("ex:qCardRest")));
            Assert.IsTrue(ontology.Data.CheckIsIndividualOf(ontology.Model, new RDFResource("ex:indiv3"), new RDFResource("ex:qCardRest")));
            Assert.IsTrue(ontology.Data.CheckIsIndividualOf(ontology.Model, new RDFResource("ex:indiv4"), new RDFResource("ex:qCardRest")));
            Assert.IsTrue(ontology.Data.CheckIsIndividualOf(ontology.Model, new RDFResource("ex:indiv5"), new RDFResource("ex:qCardRest")));
            Assert.IsTrue(ontology.Data.CheckIsIndividualOf(ontology.Model, new RDFResource("ex:indiv6"), new RDFResource("ex:qCardRest")));
        }

        [TestMethod]
        public void ShouldCheckIsIndividualOfNonZeroMinQualifiedCardinalityRestriction()
        {
            OWLOntology ontology = new OWLOntology("ex:ont");
            ontology.Model.ClassModel.DeclareClass(new RDFResource("ex:onClass"));
            ontology.Model.ClassModel.DeclareMinQualifiedCardinalityRestriction(new RDFResource("ex:qCardRest"), new RDFResource("ex:objProp"), 1, new RDFResource("ex:onClass"));
            ontology.Model.PropertyModel.DeclareObjectProperty(new RDFResource("ex:objProp"));
            ontology.Data.DeclareIndividual(new RDFResource("ex:indiv1"));
            ontology.Data.DeclareIndividual(new RDFResource("ex:indiv2"));
            ontology.Data.DeclareIndividual(new RDFResource("ex:indiv3"));
            ontology.Data.DeclareIndividual(new RDFResource("ex:indiv4"));
            ontology.Data.DeclareIndividual(new RDFResource("ex:indiv5"));
            ontology.Data.DeclareIndividual(new RDFResource("ex:indiv6"));
            ontology.Data.DeclareIndividualType(new RDFResource("ex:indiv2"), new RDFResource("ex:onClass"));
            ontology.Data.DeclareIndividualType(new RDFResource("ex:indiv3"), new RDFResource("ex:onClass"));
            ontology.Data.DeclareIndividualType(new RDFResource("ex:indiv5"), new RDFResource("ex:onClass"));
            ontology.Data.DeclareIndividualType(new RDFResource("ex:indiv6"), new RDFResource("ex:onClass"));
            ontology.Data.DeclareObjectAssertion(new RDFResource("ex:indiv1"), new RDFResource("ex:objProp"), new RDFResource("ex:indiv2"));
            ontology.Data.DeclareObjectAssertion(new RDFResource("ex:indiv1"), new RDFResource("ex:objProp"), new RDFResource("ex:indiv3"));
            ontology.Data.DeclareObjectAssertion(new RDFResource("ex:indiv2"), new RDFResource("ex:objProp"), new RDFResource("ex:indiv3"));
            ontology.Data.DeclareObjectAssertion(new RDFResource("ex:indiv2"), new RDFResource("ex:objProp"), new RDFResource("ex:indiv4"));
            ontology.Data.DeclareObjectAssertion(new RDFResource("ex:indiv3"), new RDFResource("ex:objProp"), new RDFResource("ex:indiv3"));
            ontology.Data.DeclareObjectAssertion(new RDFResource("ex:indiv3"), new RDFResource("ex:objProp"), new RDFResource("ex:indiv4"));
            ontology.Data.DeclareObjectAssertion(new RDFResource("ex:indiv3"), new RDFResource("ex:objProp"), new RDFResource("ex:indiv5"));
            ontology.Data.DeclareObjectAssertion(new RDFResource("ex:indiv3"), new RDFResource("ex:objProp"), new RDFResource("ex:indiv6"));
            ontology.Data.DeclareObjectAssertion(new RDFResource("ex:indiv4"), new RDFResource("ex:objProp"), new RDFResource("ex:indiv1"));

            Assert.IsTrue(ontology.Data.CheckIsIndividualOf(ontology.Model, new RDFResource("ex:indiv1"), new RDFResource("ex:qCardRest")));
            Assert.IsTrue(ontology.Data.CheckIsIndividualOf(ontology.Model, new RDFResource("ex:indiv2"), new RDFResource("ex:qCardRest")));
            Assert.IsTrue(ontology.Data.CheckIsIndividualOf(ontology.Model, new RDFResource("ex:indiv3"), new RDFResource("ex:qCardRest")));
            Assert.IsFalse(ontology.Data.CheckIsIndividualOf(ontology.Model, new RDFResource("ex:indiv4"), new RDFResource("ex:qCardRest")));
            Assert.IsFalse(ontology.Data.CheckIsIndividualOf(ontology.Model, new RDFResource("ex:indiv5"), new RDFResource("ex:qCardRest")));
            Assert.IsFalse(ontology.Data.CheckIsIndividualOf(ontology.Model, new RDFResource("ex:indiv6"), new RDFResource("ex:qCardRest")));
        }

        [TestMethod]
        public void ShouldCheckIsIndividualOfZeroMaxCardinalityRestriction()
        {
            OWLOntology ontology = new OWLOntology("ex:ont");
            ontology.Model.ClassModel.DeclareMaxCardinalityRestriction(new RDFResource("ex:cardRest"), new RDFResource("ex:objProp"), 0);
            ontology.Model.PropertyModel.DeclareObjectProperty(new RDFResource("ex:objProp"));
            ontology.Model.PropertyModel.DeclareObjectProperty(new RDFResource("ex:objProp2"));
            ontology.Data.DeclareIndividual(new RDFResource("ex:indiv1"));
            ontology.Data.DeclareIndividual(new RDFResource("ex:indiv2"));
            ontology.Data.DeclareIndividual(new RDFResource("ex:indiv3"));
            ontology.Data.DeclareObjectAssertion(new RDFResource("ex:indiv1"), new RDFResource("ex:objProp"), new RDFResource("ex:indiv2"));
            ontology.Data.DeclareObjectAssertion(new RDFResource("ex:indiv1"), new RDFResource("ex:objProp2"), new RDFResource("ex:indiv3"));
            ontology.Data.DeclareObjectAssertion(new RDFResource("ex:indiv2"), new RDFResource("ex:objProp2"), new RDFResource("ex:indiv3"));

            Assert.IsFalse(ontology.Data.CheckIsIndividualOf(ontology.Model, new RDFResource("ex:indiv1"), new RDFResource("ex:cardRest")));
            Assert.IsTrue(ontology.Data.CheckIsIndividualOf(ontology.Model, new RDFResource("ex:indiv2"), new RDFResource("ex:cardRest")));
            Assert.IsTrue(ontology.Data.CheckIsIndividualOf(ontology.Model, new RDFResource("ex:indiv3"), new RDFResource("ex:cardRest")));
        }

        [TestMethod]
        public void ShouldCheckIsIndividualOfNonZeroMaxCardinalityRestriction()
        {
            OWLOntology ontology = new OWLOntology("ex:ont");
            ontology.Model.ClassModel.DeclareMaxCardinalityRestriction(new RDFResource("ex:cardRest"), new RDFResource("ex:objProp"), 1);
            ontology.Model.PropertyModel.DeclareObjectProperty(new RDFResource("ex:objProp"));
            ontology.Data.DeclareIndividual(new RDFResource("ex:indiv1"));
            ontology.Data.DeclareIndividual(new RDFResource("ex:indiv2"));
            ontology.Data.DeclareIndividual(new RDFResource("ex:indiv3"));
            ontology.Data.DeclareObjectAssertion(new RDFResource("ex:indiv1"), new RDFResource("ex:objProp"), new RDFResource("ex:indiv2"));
            ontology.Data.DeclareObjectAssertion(new RDFResource("ex:indiv1"), new RDFResource("ex:objProp"), new RDFResource("ex:indiv3"));
            ontology.Data.DeclareObjectAssertion(new RDFResource("ex:indiv2"), new RDFResource("ex:objProp"), new RDFResource("ex:indiv3"));

            Assert.IsFalse(ontology.Data.CheckIsIndividualOf(ontology.Model, new RDFResource("ex:indiv1"), new RDFResource("ex:cardRest")));
            Assert.IsTrue(ontology.Data.CheckIsIndividualOf(ontology.Model, new RDFResource("ex:indiv2"), new RDFResource("ex:cardRest")));
            Assert.IsTrue(ontology.Data.CheckIsIndividualOf(ontology.Model, new RDFResource("ex:indiv3"), new RDFResource("ex:cardRest")));
        }

        [TestMethod]
        public void ShouldCheckIsIndividualOfZeroMaxQualifiedCardinalityRestriction()
        {
            OWLOntology ontology = new OWLOntology("ex:ont");
            ontology.Model.ClassModel.DeclareClass(new RDFResource("ex:onClass"));
            ontology.Model.ClassModel.DeclareMaxQualifiedCardinalityRestriction(new RDFResource("ex:qCardRest"), new RDFResource("ex:objProp"), 0, new RDFResource("ex:onClass"));
            ontology.Model.PropertyModel.DeclareObjectProperty(new RDFResource("ex:objProp"));
            ontology.Model.PropertyModel.DeclareObjectProperty(new RDFResource("ex:objProp2"));
            ontology.Data.DeclareIndividual(new RDFResource("ex:indiv1"));
            ontology.Data.DeclareIndividual(new RDFResource("ex:indiv2"));
            ontology.Data.DeclareIndividual(new RDFResource("ex:indiv3"));
            ontology.Data.DeclareIndividual(new RDFResource("ex:indiv4"));
            ontology.Data.DeclareIndividual(new RDFResource("ex:indiv5"));
            ontology.Data.DeclareIndividual(new RDFResource("ex:indiv6"));
            ontology.Data.DeclareIndividualType(new RDFResource("ex:indiv2"), new RDFResource("ex:onClass"));
            ontology.Data.DeclareIndividualType(new RDFResource("ex:indiv3"), new RDFResource("ex:onClass"));
            ontology.Data.DeclareIndividualType(new RDFResource("ex:indiv5"), new RDFResource("ex:onClass"));
            ontology.Data.DeclareIndividualType(new RDFResource("ex:indiv6"), new RDFResource("ex:onClass"));
            ontology.Data.DeclareObjectAssertion(new RDFResource("ex:indiv1"), new RDFResource("ex:objProp"), new RDFResource("ex:indiv4"));
            ontology.Data.DeclareObjectAssertion(new RDFResource("ex:indiv2"), new RDFResource("ex:objProp"), new RDFResource("ex:indiv3"));
            ontology.Data.DeclareObjectAssertion(new RDFResource("ex:indiv2"), new RDFResource("ex:objProp"), new RDFResource("ex:indiv5"));
            ontology.Data.DeclareObjectAssertion(new RDFResource("ex:indiv3"), new RDFResource("ex:objProp"), new RDFResource("ex:indiv5"));

            Assert.IsTrue(ontology.Data.CheckIsIndividualOf(ontology.Model, new RDFResource("ex:indiv1"), new RDFResource("ex:qCardRest")));
            Assert.IsFalse(ontology.Data.CheckIsIndividualOf(ontology.Model, new RDFResource("ex:indiv2"), new RDFResource("ex:qCardRest")));
            Assert.IsFalse(ontology.Data.CheckIsIndividualOf(ontology.Model, new RDFResource("ex:indiv3"), new RDFResource("ex:qCardRest")));
            Assert.IsTrue(ontology.Data.CheckIsIndividualOf(ontology.Model, new RDFResource("ex:indiv4"), new RDFResource("ex:qCardRest")));
            Assert.IsTrue(ontology.Data.CheckIsIndividualOf(ontology.Model, new RDFResource("ex:indiv5"), new RDFResource("ex:qCardRest")));
            Assert.IsTrue(ontology.Data.CheckIsIndividualOf(ontology.Model, new RDFResource("ex:indiv6"), new RDFResource("ex:qCardRest")));
        }

        [TestMethod]
        public void ShouldCheckIsIndividualOfNonZeroMaxQualifiedCardinalityRestriction()
        {
            OWLOntology ontology = new OWLOntology("ex:ont");
            ontology.Model.ClassModel.DeclareClass(new RDFResource("ex:onClass"));
            ontology.Model.ClassModel.DeclareMaxQualifiedCardinalityRestriction(new RDFResource("ex:qCardRest"), new RDFResource("ex:objProp"), 1, new RDFResource("ex:onClass"));
            ontology.Model.PropertyModel.DeclareObjectProperty(new RDFResource("ex:objProp"));
            ontology.Data.DeclareIndividual(new RDFResource("ex:indiv1"));
            ontology.Data.DeclareIndividual(new RDFResource("ex:indiv2"));
            ontology.Data.DeclareIndividual(new RDFResource("ex:indiv3"));
            ontology.Data.DeclareIndividual(new RDFResource("ex:indiv4"));
            ontology.Data.DeclareIndividual(new RDFResource("ex:indiv5"));
            ontology.Data.DeclareIndividual(new RDFResource("ex:indiv6"));
            ontology.Data.DeclareIndividualType(new RDFResource("ex:indiv2"), new RDFResource("ex:onClass"));
            ontology.Data.DeclareIndividualType(new RDFResource("ex:indiv3"), new RDFResource("ex:onClass"));
            ontology.Data.DeclareIndividualType(new RDFResource("ex:indiv5"), new RDFResource("ex:onClass"));
            ontology.Data.DeclareIndividualType(new RDFResource("ex:indiv6"), new RDFResource("ex:onClass"));
            ontology.Data.DeclareObjectAssertion(new RDFResource("ex:indiv1"), new RDFResource("ex:objProp"), new RDFResource("ex:indiv2"));
            ontology.Data.DeclareObjectAssertion(new RDFResource("ex:indiv1"), new RDFResource("ex:objProp"), new RDFResource("ex:indiv3"));
            ontology.Data.DeclareObjectAssertion(new RDFResource("ex:indiv2"), new RDFResource("ex:objProp"), new RDFResource("ex:indiv3"));
            ontology.Data.DeclareObjectAssertion(new RDFResource("ex:indiv2"), new RDFResource("ex:objProp"), new RDFResource("ex:indiv5"));
            ontology.Data.DeclareObjectAssertion(new RDFResource("ex:indiv3"), new RDFResource("ex:objProp"), new RDFResource("ex:indiv5"));
            ontology.Data.DeclareObjectAssertion(new RDFResource("ex:indiv4"), new RDFResource("ex:objProp"), new RDFResource("ex:indiv1"));

            Assert.IsFalse(ontology.Data.CheckIsIndividualOf(ontology.Model, new RDFResource("ex:indiv1"), new RDFResource("ex:qCardRest")));
            Assert.IsFalse(ontology.Data.CheckIsIndividualOf(ontology.Model, new RDFResource("ex:indiv2"), new RDFResource("ex:qCardRest")));
            Assert.IsTrue(ontology.Data.CheckIsIndividualOf(ontology.Model, new RDFResource("ex:indiv3"), new RDFResource("ex:qCardRest")));
            Assert.IsTrue(ontology.Data.CheckIsIndividualOf(ontology.Model, new RDFResource("ex:indiv4"), new RDFResource("ex:qCardRest")));
            Assert.IsTrue(ontology.Data.CheckIsIndividualOf(ontology.Model, new RDFResource("ex:indiv5"), new RDFResource("ex:qCardRest")));
            Assert.IsTrue(ontology.Data.CheckIsIndividualOf(ontology.Model, new RDFResource("ex:indiv6"), new RDFResource("ex:qCardRest")));
        }

        [TestMethod]
        public void ShouldCheckIsIndividualOfZeroMinZeroMaxCardinalityRestriction()
        {
            OWLOntology ontology = new OWLOntology("ex:ont");
            ontology.Model.ClassModel.DeclareMinMaxCardinalityRestriction(new RDFResource("ex:cardRest"), new RDFResource("ex:objProp"), 0, 0);
            ontology.Model.PropertyModel.DeclareObjectProperty(new RDFResource("ex:objProp"));
            ontology.Data.DeclareIndividual(new RDFResource("ex:indiv1"));
            ontology.Data.DeclareIndividual(new RDFResource("ex:indiv2"));
            ontology.Data.DeclareIndividual(new RDFResource("ex:indiv3"));
            ontology.Data.DeclareIndividual(new RDFResource("ex:indiv4"));
            ontology.Data.DeclareObjectAssertion(new RDFResource("ex:indiv1"), new RDFResource("ex:objProp"), new RDFResource("ex:indiv2"));
            ontology.Data.DeclareObjectAssertion(new RDFResource("ex:indiv1"), new RDFResource("ex:objProp"), new RDFResource("ex:indiv3"));
            ontology.Data.DeclareObjectAssertion(new RDFResource("ex:indiv2"), new RDFResource("ex:objProp"), new RDFResource("ex:indiv3"));
            ontology.Data.DeclareObjectAssertion(new RDFResource("ex:indiv4"), new RDFResource("ex:objProp"), new RDFResource("ex:indiv1"));
            ontology.Data.DeclareObjectAssertion(new RDFResource("ex:indiv4"), new RDFResource("ex:objProp"), new RDFResource("ex:indiv2"));
            ontology.Data.DeclareObjectAssertion(new RDFResource("ex:indiv4"), new RDFResource("ex:objProp"), new RDFResource("ex:indiv3"));

            Assert.IsFalse(ontology.Data.CheckIsIndividualOf(ontology.Model, new RDFResource("ex:indiv1"), new RDFResource("ex:cardRest")));
            Assert.IsFalse(ontology.Data.CheckIsIndividualOf(ontology.Model, new RDFResource("ex:indiv2"), new RDFResource("ex:cardRest")));
            Assert.IsTrue(ontology.Data.CheckIsIndividualOf(ontology.Model, new RDFResource("ex:indiv3"), new RDFResource("ex:cardRest")));
            Assert.IsFalse(ontology.Data.CheckIsIndividualOf(ontology.Model, new RDFResource("ex:indiv4"), new RDFResource("ex:cardRest")));
        }

        [TestMethod]
        public void ShouldCheckIsIndividualOfZeroMinNonZeroMaxCardinalityRestriction()
        {
            OWLOntology ontology = new OWLOntology("ex:ont");
            ontology.Model.ClassModel.DeclareMinMaxCardinalityRestriction(new RDFResource("ex:cardRest"), new RDFResource("ex:objProp"), 0, 1);
            ontology.Model.PropertyModel.DeclareObjectProperty(new RDFResource("ex:objProp"));
            ontology.Data.DeclareIndividual(new RDFResource("ex:indiv1"));
            ontology.Data.DeclareIndividual(new RDFResource("ex:indiv2"));
            ontology.Data.DeclareIndividual(new RDFResource("ex:indiv3"));
            ontology.Data.DeclareIndividual(new RDFResource("ex:indiv4"));
            ontology.Data.DeclareObjectAssertion(new RDFResource("ex:indiv1"), new RDFResource("ex:objProp"), new RDFResource("ex:indiv2"));
            ontology.Data.DeclareObjectAssertion(new RDFResource("ex:indiv1"), new RDFResource("ex:objProp"), new RDFResource("ex:indiv3"));
            ontology.Data.DeclareObjectAssertion(new RDFResource("ex:indiv2"), new RDFResource("ex:objProp"), new RDFResource("ex:indiv3"));
            ontology.Data.DeclareObjectAssertion(new RDFResource("ex:indiv4"), new RDFResource("ex:objProp"), new RDFResource("ex:indiv1"));
            ontology.Data.DeclareObjectAssertion(new RDFResource("ex:indiv4"), new RDFResource("ex:objProp"), new RDFResource("ex:indiv2"));
            ontology.Data.DeclareObjectAssertion(new RDFResource("ex:indiv4"), new RDFResource("ex:objProp"), new RDFResource("ex:indiv3"));

            Assert.IsFalse(ontology.Data.CheckIsIndividualOf(ontology.Model, new RDFResource("ex:indiv1"), new RDFResource("ex:cardRest")));
            Assert.IsTrue(ontology.Data.CheckIsIndividualOf(ontology.Model, new RDFResource("ex:indiv2"), new RDFResource("ex:cardRest")));
            Assert.IsFalse(ontology.Data.CheckIsIndividualOf(ontology.Model, new RDFResource("ex:indiv3"), new RDFResource("ex:cardRest")));
            Assert.IsFalse(ontology.Data.CheckIsIndividualOf(ontology.Model, new RDFResource("ex:indiv4"), new RDFResource("ex:cardRest")));
        }

        [TestMethod]
        public void ShouldCheckIsIndividualOfNonZeroMinNonZeroMaxCardinalityRestriction()
        {
            OWLOntology ontology = new OWLOntology("ex:ont");
            ontology.Model.ClassModel.DeclareMinMaxCardinalityRestriction(new RDFResource("ex:cardRest"), new RDFResource("ex:objProp"), 1, 2);
            ontology.Model.PropertyModel.DeclareObjectProperty(new RDFResource("ex:objProp"));
            ontology.Data.DeclareIndividual(new RDFResource("ex:indiv1"));
            ontology.Data.DeclareIndividual(new RDFResource("ex:indiv2"));
            ontology.Data.DeclareIndividual(new RDFResource("ex:indiv3"));
            ontology.Data.DeclareIndividual(new RDFResource("ex:indiv4"));
            ontology.Data.DeclareObjectAssertion(new RDFResource("ex:indiv1"), new RDFResource("ex:objProp"), new RDFResource("ex:indiv2"));
            ontology.Data.DeclareObjectAssertion(new RDFResource("ex:indiv1"), new RDFResource("ex:objProp"), new RDFResource("ex:indiv3"));
            ontology.Data.DeclareObjectAssertion(new RDFResource("ex:indiv2"), new RDFResource("ex:objProp"), new RDFResource("ex:indiv3"));
            ontology.Data.DeclareObjectAssertion(new RDFResource("ex:indiv4"), new RDFResource("ex:objProp"), new RDFResource("ex:indiv1"));
            ontology.Data.DeclareObjectAssertion(new RDFResource("ex:indiv4"), new RDFResource("ex:objProp"), new RDFResource("ex:indiv2"));
            ontology.Data.DeclareObjectAssertion(new RDFResource("ex:indiv4"), new RDFResource("ex:objProp"), new RDFResource("ex:indiv3"));

            Assert.IsTrue(ontology.Data.CheckIsIndividualOf(ontology.Model, new RDFResource("ex:indiv1"), new RDFResource("ex:cardRest")));
            Assert.IsTrue(ontology.Data.CheckIsIndividualOf(ontology.Model, new RDFResource("ex:indiv2"), new RDFResource("ex:cardRest")));
            Assert.IsFalse(ontology.Data.CheckIsIndividualOf(ontology.Model, new RDFResource("ex:indiv3"), new RDFResource("ex:cardRest")));
            Assert.IsFalse(ontology.Data.CheckIsIndividualOf(ontology.Model, new RDFResource("ex:indiv4"), new RDFResource("ex:cardRest")));
        }

        [TestMethod]
        public void ShouldThrowExceptionOnDeclaringNonZeroMinZeroMaxCardinalityRestriction()
            => Assert.ThrowsException<OWLSemanticsException>(() => new OWLOntology("ex:ont").Model.ClassModel.DeclareMinMaxCardinalityRestriction(new RDFResource("ex:cardRest"), new RDFResource("ex:objProp"), 1, 0));

        [TestMethod]
        public void ShouldCheckIsIndividualOfZeroMinZeroMaxQualifiedCardinalityRestriction()
        {
            OWLOntology ontology = new OWLOntology("ex:ont");
            ontology.Model.ClassModel.DeclareClass(new RDFResource("ex:onClass"));
            ontology.Model.ClassModel.DeclareMinMaxQualifiedCardinalityRestriction(new RDFResource("ex:qCardRest"), new RDFResource("ex:objProp"), 0, 0, new RDFResource("ex:onClass"));
            ontology.Model.PropertyModel.DeclareObjectProperty(new RDFResource("ex:objProp"));
            ontology.Data.DeclareIndividual(new RDFResource("ex:indiv1"));
            ontology.Data.DeclareIndividual(new RDFResource("ex:indiv2"));
            ontology.Data.DeclareIndividual(new RDFResource("ex:indiv3"));
            ontology.Data.DeclareIndividual(new RDFResource("ex:indiv4"));
            ontology.Data.DeclareIndividual(new RDFResource("ex:indiv5"));
            ontology.Data.DeclareIndividual(new RDFResource("ex:indiv6"));
            ontology.Data.DeclareIndividualType(new RDFResource("ex:indiv2"), new RDFResource("ex:onClass"));
            ontology.Data.DeclareIndividualType(new RDFResource("ex:indiv3"), new RDFResource("ex:onClass"));
            ontology.Data.DeclareIndividualType(new RDFResource("ex:indiv5"), new RDFResource("ex:onClass"));
            ontology.Data.DeclareIndividualType(new RDFResource("ex:indiv6"), new RDFResource("ex:onClass"));
            ontology.Data.DeclareObjectAssertion(new RDFResource("ex:indiv1"), new RDFResource("ex:objProp"), new RDFResource("ex:indiv2"));
            ontology.Data.DeclareObjectAssertion(new RDFResource("ex:indiv1"), new RDFResource("ex:objProp"), new RDFResource("ex:indiv3"));
            ontology.Data.DeclareObjectAssertion(new RDFResource("ex:indiv2"), new RDFResource("ex:objProp"), new RDFResource("ex:indiv3"));
            ontology.Data.DeclareObjectAssertion(new RDFResource("ex:indiv3"), new RDFResource("ex:objProp"), new RDFResource("ex:indiv3"));
            ontology.Data.DeclareObjectAssertion(new RDFResource("ex:indiv3"), new RDFResource("ex:objProp"), new RDFResource("ex:indiv5"));
            ontology.Data.DeclareObjectAssertion(new RDFResource("ex:indiv3"), new RDFResource("ex:objProp"), new RDFResource("ex:indiv6"));

            Assert.IsFalse(ontology.Data.CheckIsIndividualOf(ontology.Model, new RDFResource("ex:indiv1"), new RDFResource("ex:qCardRest")));
            Assert.IsFalse(ontology.Data.CheckIsIndividualOf(ontology.Model, new RDFResource("ex:indiv2"), new RDFResource("ex:qCardRest")));
            Assert.IsFalse(ontology.Data.CheckIsIndividualOf(ontology.Model, new RDFResource("ex:indiv3"), new RDFResource("ex:qCardRest")));
            Assert.IsTrue(ontology.Data.CheckIsIndividualOf(ontology.Model, new RDFResource("ex:indiv4"), new RDFResource("ex:qCardRest")));
            Assert.IsTrue(ontology.Data.CheckIsIndividualOf(ontology.Model, new RDFResource("ex:indiv5"), new RDFResource("ex:qCardRest")));
            Assert.IsTrue(ontology.Data.CheckIsIndividualOf(ontology.Model, new RDFResource("ex:indiv6"), new RDFResource("ex:qCardRest")));
        }

        [TestMethod]
        public void ShouldCheckIsIndividualOfZeroMinNonZeroMaxQualifiedCardinalityRestriction()
        {
            OWLOntology ontology = new OWLOntology("ex:ont");
            ontology.Model.ClassModel.DeclareClass(new RDFResource("ex:onClass"));
            ontology.Model.ClassModel.DeclareMinMaxQualifiedCardinalityRestriction(new RDFResource("ex:qCardRest"), new RDFResource("ex:objProp"), 0, 2, new RDFResource("ex:onClass"));
            ontology.Model.PropertyModel.DeclareObjectProperty(new RDFResource("ex:objProp"));
            ontology.Data.DeclareIndividual(new RDFResource("ex:indiv1"));
            ontology.Data.DeclareIndividual(new RDFResource("ex:indiv2"));
            ontology.Data.DeclareIndividual(new RDFResource("ex:indiv3"));
            ontology.Data.DeclareIndividual(new RDFResource("ex:indiv4"));
            ontology.Data.DeclareIndividual(new RDFResource("ex:indiv5"));
            ontology.Data.DeclareIndividual(new RDFResource("ex:indiv6"));
            ontology.Data.DeclareIndividualType(new RDFResource("ex:indiv2"), new RDFResource("ex:onClass"));
            ontology.Data.DeclareIndividualType(new RDFResource("ex:indiv3"), new RDFResource("ex:onClass"));
            ontology.Data.DeclareIndividualType(new RDFResource("ex:indiv5"), new RDFResource("ex:onClass"));
            ontology.Data.DeclareIndividualType(new RDFResource("ex:indiv6"), new RDFResource("ex:onClass"));
            ontology.Data.DeclareObjectAssertion(new RDFResource("ex:indiv1"), new RDFResource("ex:objProp"), new RDFResource("ex:indiv2"));
            ontology.Data.DeclareObjectAssertion(new RDFResource("ex:indiv1"), new RDFResource("ex:objProp"), new RDFResource("ex:indiv3"));
            ontology.Data.DeclareObjectAssertion(new RDFResource("ex:indiv2"), new RDFResource("ex:objProp"), new RDFResource("ex:indiv3"));
            ontology.Data.DeclareObjectAssertion(new RDFResource("ex:indiv3"), new RDFResource("ex:objProp"), new RDFResource("ex:indiv3"));
            ontology.Data.DeclareObjectAssertion(new RDFResource("ex:indiv3"), new RDFResource("ex:objProp"), new RDFResource("ex:indiv5"));
            ontology.Data.DeclareObjectAssertion(new RDFResource("ex:indiv3"), new RDFResource("ex:objProp"), new RDFResource("ex:indiv6"));

            Assert.IsTrue(ontology.Data.CheckIsIndividualOf(ontology.Model, new RDFResource("ex:indiv1"), new RDFResource("ex:qCardRest")));
            Assert.IsTrue(ontology.Data.CheckIsIndividualOf(ontology.Model, new RDFResource("ex:indiv2"), new RDFResource("ex:qCardRest")));
            Assert.IsFalse(ontology.Data.CheckIsIndividualOf(ontology.Model, new RDFResource("ex:indiv3"), new RDFResource("ex:qCardRest")));
            Assert.IsFalse(ontology.Data.CheckIsIndividualOf(ontology.Model, new RDFResource("ex:indiv4"), new RDFResource("ex:qCardRest")));
            Assert.IsFalse(ontology.Data.CheckIsIndividualOf(ontology.Model, new RDFResource("ex:indiv5"), new RDFResource("ex:qCardRest")));
            Assert.IsFalse(ontology.Data.CheckIsIndividualOf(ontology.Model, new RDFResource("ex:indiv6"), new RDFResource("ex:qCardRest")));
        }

        [TestMethod]
        public void ShouldThrowExceptionOnDeclaringNonZeroMinZeroMaxQualifiedCardinalityRestriction()
            => Assert.ThrowsException<OWLSemanticsException>(() => new OWLOntology("ex:ont").Model.ClassModel.DeclareMinMaxQualifiedCardinalityRestriction(new RDFResource("ex:cardRest"), new RDFResource("ex:objProp"), 1, 0, new RDFResource("ex:onClass")));

        [TestMethod]
        public void ShouldCheckIsIndividualOfNonZeroMinNonZeroMaxQualifiedCardinalityRestriction()
        {
            OWLOntology ontology = new OWLOntology("ex:ont");
            ontology.Model.ClassModel.DeclareClass(new RDFResource("ex:onClass"));
            ontology.Model.ClassModel.DeclareMinMaxQualifiedCardinalityRestriction(new RDFResource("ex:qCardRest"), new RDFResource("ex:objProp"), 1, 2, new RDFResource("ex:onClass"));
            ontology.Model.PropertyModel.DeclareObjectProperty(new RDFResource("ex:objProp"));
            ontology.Data.DeclareIndividual(new RDFResource("ex:indiv1"));
            ontology.Data.DeclareIndividual(new RDFResource("ex:indiv2"));
            ontology.Data.DeclareIndividual(new RDFResource("ex:indiv3"));
            ontology.Data.DeclareIndividual(new RDFResource("ex:indiv4"));
            ontology.Data.DeclareIndividual(new RDFResource("ex:indiv5"));
            ontology.Data.DeclareIndividual(new RDFResource("ex:indiv6"));
            ontology.Data.DeclareIndividualType(new RDFResource("ex:indiv2"), new RDFResource("ex:onClass"));
            ontology.Data.DeclareIndividualType(new RDFResource("ex:indiv3"), new RDFResource("ex:onClass"));
            ontology.Data.DeclareIndividualType(new RDFResource("ex:indiv5"), new RDFResource("ex:onClass"));
            ontology.Data.DeclareIndividualType(new RDFResource("ex:indiv6"), new RDFResource("ex:onClass"));
            ontology.Data.DeclareObjectAssertion(new RDFResource("ex:indiv1"), new RDFResource("ex:objProp"), new RDFResource("ex:indiv2"));
            ontology.Data.DeclareObjectAssertion(new RDFResource("ex:indiv1"), new RDFResource("ex:objProp"), new RDFResource("ex:indiv3"));
            ontology.Data.DeclareObjectAssertion(new RDFResource("ex:indiv2"), new RDFResource("ex:objProp"), new RDFResource("ex:indiv3"));
            ontology.Data.DeclareObjectAssertion(new RDFResource("ex:indiv3"), new RDFResource("ex:objProp"), new RDFResource("ex:indiv3"));
            ontology.Data.DeclareObjectAssertion(new RDFResource("ex:indiv3"), new RDFResource("ex:objProp"), new RDFResource("ex:indiv5"));
            ontology.Data.DeclareObjectAssertion(new RDFResource("ex:indiv3"), new RDFResource("ex:objProp"), new RDFResource("ex:indiv6"));

            Assert.IsTrue(ontology.Data.CheckIsIndividualOf(ontology.Model, new RDFResource("ex:indiv1"), new RDFResource("ex:qCardRest")));
            Assert.IsTrue(ontology.Data.CheckIsIndividualOf(ontology.Model, new RDFResource("ex:indiv2"), new RDFResource("ex:qCardRest")));
            Assert.IsFalse(ontology.Data.CheckIsIndividualOf(ontology.Model, new RDFResource("ex:indiv3"), new RDFResource("ex:qCardRest")));
            Assert.IsFalse(ontology.Data.CheckIsIndividualOf(ontology.Model, new RDFResource("ex:indiv4"), new RDFResource("ex:qCardRest")));
            Assert.IsFalse(ontology.Data.CheckIsIndividualOf(ontology.Model, new RDFResource("ex:indiv5"), new RDFResource("ex:qCardRest")));
            Assert.IsFalse(ontology.Data.CheckIsIndividualOf(ontology.Model, new RDFResource("ex:indiv6"), new RDFResource("ex:qCardRest")));
        }
        
        [TestMethod]
        public void ShouldCheckIsIndividualOfSomeValuesFromRestriction()
        {
            OWLOntology ontology = new OWLOntology("ex:ont");
            ontology.Model.ClassModel.DeclareClass(new RDFResource("ex:onClass"));
            ontology.Model.ClassModel.DeclareSomeValuesFromRestriction(new RDFResource("ex:svFromRest"), new RDFResource("ex:objProp"), new RDFResource("ex:onClass"));
            ontology.Model.PropertyModel.DeclareObjectProperty(new RDFResource("ex:objProp"));
            ontology.Data.DeclareIndividual(new RDFResource("ex:indiv1"));
            ontology.Data.DeclareIndividual(new RDFResource("ex:indiv2"));
            ontology.Data.DeclareIndividual(new RDFResource("ex:indiv3"));
            ontology.Data.DeclareIndividual(new RDFResource("ex:indiv4"));
            ontology.Data.DeclareIndividualType(new RDFResource("ex:indiv2"), new RDFResource("ex:onClass"));
            ontology.Data.DeclareIndividualType(new RDFResource("ex:indiv3"), new RDFResource("ex:onClass"));
            ontology.Data.DeclareObjectAssertion(new RDFResource("ex:indiv1"), new RDFResource("ex:objProp"), new RDFResource("ex:indiv2"));
            ontology.Data.DeclareObjectAssertion(new RDFResource("ex:indiv1"), new RDFResource("ex:objProp"), new RDFResource("ex:indiv3"));
            ontology.Data.DeclareObjectAssertion(new RDFResource("ex:indiv2"), new RDFResource("ex:objProp"), new RDFResource("ex:indiv3"));
            ontology.Data.DeclareObjectAssertion(new RDFResource("ex:indiv3"), new RDFResource("ex:objProp"), new RDFResource("ex:indiv1"));
            ontology.Data.DeclareObjectAssertion(new RDFResource("ex:indiv3"), new RDFResource("ex:objProp"), new RDFResource("ex:indiv4"));

            Assert.IsTrue(ontology.Data.CheckIsIndividualOf(ontology.Model, new RDFResource("ex:indiv1"), new RDFResource("ex:svFromRest")));
            Assert.IsTrue(ontology.Data.CheckIsIndividualOf(ontology.Model, new RDFResource("ex:indiv2"), new RDFResource("ex:svFromRest")));
            Assert.IsFalse(ontology.Data.CheckIsIndividualOf(ontology.Model, new RDFResource("ex:indiv3"), new RDFResource("ex:svFromRest")));
            Assert.IsFalse(ontology.Data.CheckIsIndividualOf(ontology.Model, new RDFResource("ex:indiv4"), new RDFResource("ex:svFromRest")));
        }

        [TestMethod]
        public void ShouldCheckIsIndividualOfSomeValuesFromRestrictionWithReasoning()
        {
            OWLOntology ontology = new OWLOntology("ex:ont");
            ontology.Model.ClassModel.DeclareClass(new RDFResource("ex:onClass"));
            ontology.Model.ClassModel.DeclareClass(new RDFResource("ex:subClass"));
            ontology.Model.ClassModel.DeclareSubClasses(new RDFResource("ex:subClass"), new RDFResource("ex:onClass"));
            ontology.Model.ClassModel.DeclareSomeValuesFromRestriction(new RDFResource("ex:svFromRest"), new RDFResource("ex:objProp"), new RDFResource("ex:onClass"));
            ontology.Model.PropertyModel.DeclareObjectProperty(new RDFResource("ex:objProp"));
            ontology.Model.PropertyModel.DeclareObjectProperty(new RDFResource("ex:objPropEquiv"));
            ontology.Model.PropertyModel.DeclareObjectProperty(new RDFResource("ex:objPropSub"));
            ontology.Model.PropertyModel.DeclareEquivalentProperties(new RDFResource("ex:objProp"), new RDFResource("ex:objPropEquiv"));
            ontology.Model.PropertyModel.DeclareSubProperties(new RDFResource("ex:objPropSub"), new RDFResource("ex:objProp"));
            ontology.Data.DeclareIndividual(new RDFResource("ex:indiv1"));
            ontology.Data.DeclareIndividual(new RDFResource("ex:indiv2"));
            ontology.Data.DeclareIndividual(new RDFResource("ex:indiv3"));
            ontology.Data.DeclareIndividual(new RDFResource("ex:indiv4"));
            ontology.Data.DeclareIndividualType(new RDFResource("ex:indiv2"), new RDFResource("ex:subClass"));
            ontology.Data.DeclareIndividualType(new RDFResource("ex:indiv3"), new RDFResource("ex:subClass"));
            ontology.Data.DeclareObjectAssertion(new RDFResource("ex:indiv1"), new RDFResource("ex:objPropEquiv"), new RDFResource("ex:indiv2"));
            ontology.Data.DeclareObjectAssertion(new RDFResource("ex:indiv1"), new RDFResource("ex:objPropSub"), new RDFResource("ex:indiv3"));
            ontology.Data.DeclareObjectAssertion(new RDFResource("ex:indiv2"), new RDFResource("ex:objPropSub"), new RDFResource("ex:indiv3"));
            ontology.Data.DeclareObjectAssertion(new RDFResource("ex:indiv3"), new RDFResource("ex:objPropEquiv"), new RDFResource("ex:indiv1"));
            ontology.Data.DeclareObjectAssertion(new RDFResource("ex:indiv3"), new RDFResource("ex:objPropEquiv"), new RDFResource("ex:indiv4"));

            Assert.IsTrue(ontology.Data.CheckIsIndividualOf(ontology.Model, new RDFResource("ex:indiv1"), new RDFResource("ex:svFromRest")));
            Assert.IsTrue(ontology.Data.CheckIsIndividualOf(ontology.Model, new RDFResource("ex:indiv2"), new RDFResource("ex:svFromRest")));
            Assert.IsFalse(ontology.Data.CheckIsIndividualOf(ontology.Model, new RDFResource("ex:indiv3"), new RDFResource("ex:svFromRest")));
            Assert.IsFalse(ontology.Data.CheckIsIndividualOf(ontology.Model, new RDFResource("ex:indiv4"), new RDFResource("ex:svFromRest")));
        }

        [TestMethod]
        public void ShouldCheckIsIndividualOfAllValuesFromRestriction()
        {
            OWLOntology ontology = new OWLOntology("ex:ont");
            ontology.Model.ClassModel.DeclareClass(new RDFResource("ex:onClass"));
            ontology.Model.ClassModel.DeclareAllValuesFromRestriction(new RDFResource("ex:avFromRest"), new RDFResource("ex:objProp"), new RDFResource("ex:onClass"));
            ontology.Model.PropertyModel.DeclareObjectProperty(new RDFResource("ex:objProp"));
            ontology.Data.DeclareIndividual(new RDFResource("ex:indiv1"));
            ontology.Data.DeclareIndividual(new RDFResource("ex:indiv2"));
            ontology.Data.DeclareIndividual(new RDFResource("ex:indiv3"));
            ontology.Data.DeclareIndividual(new RDFResource("ex:indiv4"));
            ontology.Data.DeclareIndividualType(new RDFResource("ex:indiv2"), new RDFResource("ex:onClass"));
            ontology.Data.DeclareIndividualType(new RDFResource("ex:indiv3"), new RDFResource("ex:onClass"));
            ontology.Data.DeclareObjectAssertion(new RDFResource("ex:indiv1"), new RDFResource("ex:objProp"), new RDFResource("ex:indiv2"));
            ontology.Data.DeclareObjectAssertion(new RDFResource("ex:indiv1"), new RDFResource("ex:objProp"), new RDFResource("ex:indiv3"));
            ontology.Data.DeclareObjectAssertion(new RDFResource("ex:indiv2"), new RDFResource("ex:objProp"), new RDFResource("ex:indiv3"));
            ontology.Data.DeclareObjectAssertion(new RDFResource("ex:indiv3"), new RDFResource("ex:objProp"), new RDFResource("ex:indiv2"));
            ontology.Data.DeclareObjectAssertion(new RDFResource("ex:indiv3"), new RDFResource("ex:objProp"), new RDFResource("ex:indiv4"));

            Assert.IsTrue(ontology.Data.CheckIsIndividualOf(ontology.Model, new RDFResource("ex:indiv1"), new RDFResource("ex:avFromRest")));
            Assert.IsTrue(ontology.Data.CheckIsIndividualOf(ontology.Model, new RDFResource("ex:indiv2"), new RDFResource("ex:avFromRest")));
            Assert.IsFalse(ontology.Data.CheckIsIndividualOf(ontology.Model, new RDFResource("ex:indiv3"), new RDFResource("ex:avFromRest")));
            Assert.IsFalse(ontology.Data.CheckIsIndividualOf(ontology.Model, new RDFResource("ex:indiv4"), new RDFResource("ex:avFromRest")));
        }

        [TestMethod]
        public void ShouldCheckIsIndividualOfAllValuesFromRestrictionWithReasoning()
        {
            OWLOntology ontology = new OWLOntology("ex:ont");
            ontology.Model.ClassModel.DeclareClass(new RDFResource("ex:onClass"));
            ontology.Model.ClassModel.DeclareClass(new RDFResource("ex:subClass"));
            ontology.Model.ClassModel.DeclareSubClasses(new RDFResource("ex:subClass"), new RDFResource("ex:onClass"));
            ontology.Model.ClassModel.DeclareAllValuesFromRestriction(new RDFResource("ex:avFromRest"), new RDFResource("ex:objProp"), new RDFResource("ex:onClass"));
            ontology.Model.PropertyModel.DeclareObjectProperty(new RDFResource("ex:objProp"));
            ontology.Model.PropertyModel.DeclareObjectProperty(new RDFResource("ex:objPropEquiv"));
            ontology.Model.PropertyModel.DeclareObjectProperty(new RDFResource("ex:objPropSub"));
            ontology.Model.PropertyModel.DeclareEquivalentProperties(new RDFResource("ex:objProp"), new RDFResource("ex:objPropEquiv"));
            ontology.Model.PropertyModel.DeclareSubProperties(new RDFResource("ex:objPropSub"), new RDFResource("ex:objProp"));
            ontology.Data.DeclareIndividual(new RDFResource("ex:indiv1"));
            ontology.Data.DeclareIndividual(new RDFResource("ex:indiv2"));
            ontology.Data.DeclareIndividual(new RDFResource("ex:indiv3"));
            ontology.Data.DeclareIndividual(new RDFResource("ex:indiv4"));
            ontology.Data.DeclareIndividualType(new RDFResource("ex:indiv2"), new RDFResource("ex:subClass"));
            ontology.Data.DeclareIndividualType(new RDFResource("ex:indiv3"), new RDFResource("ex:subClass"));
            ontology.Data.DeclareObjectAssertion(new RDFResource("ex:indiv1"), new RDFResource("ex:objPropEquiv"), new RDFResource("ex:indiv2"));
            ontology.Data.DeclareObjectAssertion(new RDFResource("ex:indiv1"), new RDFResource("ex:objPropSub"), new RDFResource("ex:indiv3"));
            ontology.Data.DeclareObjectAssertion(new RDFResource("ex:indiv2"), new RDFResource("ex:objPropSub"), new RDFResource("ex:indiv3"));
            ontology.Data.DeclareObjectAssertion(new RDFResource("ex:indiv3"), new RDFResource("ex:objPropEquiv"), new RDFResource("ex:indiv2"));
            ontology.Data.DeclareObjectAssertion(new RDFResource("ex:indiv3"), new RDFResource("ex:objPropEquiv"), new RDFResource("ex:indiv4"));

            Assert.IsTrue(ontology.Data.CheckIsIndividualOf(ontology.Model, new RDFResource("ex:indiv1"), new RDFResource("ex:avFromRest")));
            Assert.IsTrue(ontology.Data.CheckIsIndividualOf(ontology.Model, new RDFResource("ex:indiv2"), new RDFResource("ex:avFromRest")));
            Assert.IsFalse(ontology.Data.CheckIsIndividualOf(ontology.Model, new RDFResource("ex:indiv3"), new RDFResource("ex:avFromRest")));
            Assert.IsFalse(ontology.Data.CheckIsIndividualOf(ontology.Model, new RDFResource("ex:indiv4"), new RDFResource("ex:avFromRest")));
        }

        [TestMethod]
        public void ShouldCheckIsIndividualOfHasValueResourceRestriction()
        {
            OWLOntology ontology = new OWLOntology("ex:ont");
            ontology.Model.ClassModel.DeclareHasValueRestriction(new RDFResource("ex:hvRest"), new RDFResource("ex:objProp"), new RDFResource("ex:indiv2"));
            ontology.Model.PropertyModel.DeclareObjectProperty(new RDFResource("ex:objProp"));
            ontology.Data.DeclareIndividual(new RDFResource("ex:indiv1"));
            ontology.Data.DeclareIndividual(new RDFResource("ex:indiv2"));
            ontology.Data.DeclareIndividual(new RDFResource("ex:indiv3"));
            ontology.Data.DeclareIndividual(new RDFResource("ex:indiv4"));
            ontology.Data.DeclareObjectAssertion(new RDFResource("ex:indiv1"), new RDFResource("ex:objProp"), new RDFResource("ex:indiv2"));
            ontology.Data.DeclareObjectAssertion(new RDFResource("ex:indiv1"), new RDFResource("ex:objProp"), new RDFResource("ex:indiv3"));
            ontology.Data.DeclareObjectAssertion(new RDFResource("ex:indiv2"), new RDFResource("ex:objProp"), new RDFResource("ex:indiv3"));
            ontology.Data.DeclareObjectAssertion(new RDFResource("ex:indiv3"), new RDFResource("ex:objProp"), new RDFResource("ex:indiv2"));
            ontology.Data.DeclareObjectAssertion(new RDFResource("ex:indiv3"), new RDFResource("ex:objProp"), new RDFResource("ex:indiv4"));

            Assert.IsTrue(ontology.Data.CheckIsIndividualOf(ontology.Model, new RDFResource("ex:indiv1"), new RDFResource("ex:hvRest")));
            Assert.IsFalse(ontology.Data.CheckIsIndividualOf(ontology.Model, new RDFResource("ex:indiv2"), new RDFResource("ex:hvRest")));
            Assert.IsTrue(ontology.Data.CheckIsIndividualOf(ontology.Model, new RDFResource("ex:indiv3"), new RDFResource("ex:hvRest")));
            Assert.IsFalse(ontology.Data.CheckIsIndividualOf(ontology.Model, new RDFResource("ex:indiv4"), new RDFResource("ex:hvRest")));
        }

        [TestMethod]
        public void ShouldCheckIsIndividualOfHasValueResourceRestrictionWithReasoning()
        {
            OWLOntology ontology = new OWLOntology("ex:ont");
            ontology.Model.ClassModel.DeclareHasValueRestriction(new RDFResource("ex:hvRest"), new RDFResource("ex:objProp"), new RDFResource("ex:indiv2"));
            ontology.Model.PropertyModel.DeclareObjectProperty(new RDFResource("ex:objProp"));
            ontology.Data.DeclareIndividual(new RDFResource("ex:indiv1"));
            ontology.Data.DeclareIndividual(new RDFResource("ex:indiv2"));
            ontology.Data.DeclareIndividual(new RDFResource("ex:indiv3"));
            ontology.Data.DeclareIndividual(new RDFResource("ex:indiv4"));
            ontology.Data.DeclareSameIndividuals(new RDFResource("ex:indiv2"), new RDFResource("ex:indiv3"));
            ontology.Data.DeclareObjectAssertion(new RDFResource("ex:indiv1"), new RDFResource("ex:objProp"), new RDFResource("ex:indiv2"));
            ontology.Data.DeclareObjectAssertion(new RDFResource("ex:indiv1"), new RDFResource("ex:objProp"), new RDFResource("ex:indiv3"));
            ontology.Data.DeclareObjectAssertion(new RDFResource("ex:indiv2"), new RDFResource("ex:objProp"), new RDFResource("ex:indiv3"));
            ontology.Data.DeclareObjectAssertion(new RDFResource("ex:indiv3"), new RDFResource("ex:objProp"), new RDFResource("ex:indiv2"));
            ontology.Data.DeclareObjectAssertion(new RDFResource("ex:indiv3"), new RDFResource("ex:objProp"), new RDFResource("ex:indiv4"));

            Assert.IsTrue(ontology.Data.CheckIsIndividualOf(ontology.Model, new RDFResource("ex:indiv1"), new RDFResource("ex:hvRest")));
            Assert.IsTrue(ontology.Data.CheckIsIndividualOf(ontology.Model, new RDFResource("ex:indiv2"), new RDFResource("ex:hvRest")));
            Assert.IsTrue(ontology.Data.CheckIsIndividualOf(ontology.Model, new RDFResource("ex:indiv3"), new RDFResource("ex:hvRest")));
            Assert.IsFalse(ontology.Data.CheckIsIndividualOf(ontology.Model, new RDFResource("ex:indiv4"), new RDFResource("ex:hvRest")));
        }

        [TestMethod]
        public void ShouldCheckIsIndividualOfHasValueLiteralRestriction()
        {
            OWLOntology ontology = new OWLOntology("ex:ont");
            ontology.Model.ClassModel.DeclareHasValueRestriction(new RDFResource("ex:hvRest"), new RDFResource("ex:dtProp"), new RDFTypedLiteral("25", RDFModelEnums.RDFDatatypes.XSD_POSITIVEINTEGER));
            ontology.Model.PropertyModel.DeclareDatatypeProperty(new RDFResource("ex:dtProp"));
            ontology.Data.DeclareDatatypeAssertion(new RDFResource("ex:indiv1"), new RDFResource("ex:dtProp"), new RDFTypedLiteral("25.0", RDFModelEnums.RDFDatatypes.XSD_FLOAT));
            ontology.Data.DeclareDatatypeAssertion(new RDFResource("ex:indiv1"), new RDFResource("ex:dtProp"), new RDFTypedLiteral("26", RDFModelEnums.RDFDatatypes.XSD_DECIMAL));
            ontology.Data.DeclareDatatypeAssertion(new RDFResource("ex:indiv2"), new RDFResource("ex:dtProp"), new RDFPlainLiteral("25"));
            ontology.Data.DeclareDatatypeAssertion(new RDFResource("ex:indiv3"), new RDFResource("ex:dtProp"), new RDFTypedLiteral("25", RDFModelEnums.RDFDatatypes.XSD_NONNEGATIVEINTEGER));
            ontology.Data.DeclareDatatypeAssertion(new RDFResource("ex:indiv3"), new RDFResource("ex:dtProp"), new RDFTypedLiteral("---25", RDFModelEnums.RDFDatatypes.XSD_GDAY));

            Assert.IsTrue(ontology.Data.CheckIsIndividualOf(ontology.Model, new RDFResource("ex:indiv1"), new RDFResource("ex:hvRest")));
            Assert.IsFalse(ontology.Data.CheckIsIndividualOf(ontology.Model, new RDFResource("ex:indiv2"), new RDFResource("ex:hvRest")));
            Assert.IsTrue(ontology.Data.CheckIsIndividualOf(ontology.Model, new RDFResource("ex:indiv3"), new RDFResource("ex:hvRest")));
            Assert.IsFalse(ontology.Data.CheckIsIndividualOf(ontology.Model, new RDFResource("ex:indiv4"), new RDFResource("ex:hvRest")));
        }

        [TestMethod]
        public void ShouldCheckIsIndividualOfHasSelfTrueRestriction()
        {
            OWLOntology ontology = new OWLOntology("ex:ont");
            ontology.Model.ClassModel.DeclareHasSelfRestriction(new RDFResource("ex:hsRest"), new RDFResource("ex:objProp"), true);
            ontology.Model.PropertyModel.DeclareObjectProperty(new RDFResource("ex:objProp"));
            ontology.Data.DeclareIndividual(new RDFResource("ex:indiv1"));
            ontology.Data.DeclareIndividual(new RDFResource("ex:indiv2"));
            ontology.Data.DeclareIndividual(new RDFResource("ex:indiv3"));
            ontology.Data.DeclareIndividual(new RDFResource("ex:indiv4"));
            ontology.Data.DeclareObjectAssertion(new RDFResource("ex:indiv1"), new RDFResource("ex:objProp"), new RDFResource("ex:indiv1"));
            ontology.Data.DeclareObjectAssertion(new RDFResource("ex:indiv1"), new RDFResource("ex:objProp"), new RDFResource("ex:indiv3"));
            ontology.Data.DeclareObjectAssertion(new RDFResource("ex:indiv2"), new RDFResource("ex:objProp"), new RDFResource("ex:indiv3"));
            ontology.Data.DeclareObjectAssertion(new RDFResource("ex:indiv3"), new RDFResource("ex:objProp"), new RDFResource("ex:indiv3"));
            ontology.Data.DeclareObjectAssertion(new RDFResource("ex:indiv3"), new RDFResource("ex:objProp"), new RDFResource("ex:indiv4"));

            Assert.IsTrue(ontology.Data.CheckIsIndividualOf(ontology.Model, new RDFResource("ex:indiv1"), new RDFResource("ex:hsRest")));
            Assert.IsFalse(ontology.Data.CheckIsIndividualOf(ontology.Model, new RDFResource("ex:indiv2"), new RDFResource("ex:hsRest")));
            Assert.IsTrue(ontology.Data.CheckIsIndividualOf(ontology.Model, new RDFResource("ex:indiv3"), new RDFResource("ex:hsRest")));
            Assert.IsFalse(ontology.Data.CheckIsIndividualOf(ontology.Model, new RDFResource("ex:indiv4"), new RDFResource("ex:hsRest")));
        }

        [TestMethod]
        public void ShouldCheckIsIndividualOfHasSelfFalseRestriction()
        {
            OWLOntology ontology = new OWLOntology("ex:ont");
            ontology.Model.ClassModel.DeclareHasSelfRestriction(new RDFResource("ex:hsRest"), new RDFResource("ex:objProp"), false);
            ontology.Model.PropertyModel.DeclareObjectProperty(new RDFResource("ex:objProp"));
            ontology.Data.DeclareIndividual(new RDFResource("ex:indiv1"));
            ontology.Data.DeclareIndividual(new RDFResource("ex:indiv2"));
            ontology.Data.DeclareIndividual(new RDFResource("ex:indiv3"));
            ontology.Data.DeclareIndividual(new RDFResource("ex:indiv4"));
            ontology.Data.DeclareObjectAssertion(new RDFResource("ex:indiv1"), new RDFResource("ex:objProp"), new RDFResource("ex:indiv1"));
            ontology.Data.DeclareObjectAssertion(new RDFResource("ex:indiv1"), new RDFResource("ex:objProp"), new RDFResource("ex:indiv3"));
            ontology.Data.DeclareObjectAssertion(new RDFResource("ex:indiv2"), new RDFResource("ex:objProp"), new RDFResource("ex:indiv3"));
            ontology.Data.DeclareObjectAssertion(new RDFResource("ex:indiv3"), new RDFResource("ex:objProp"), new RDFResource("ex:indiv3"));
            ontology.Data.DeclareObjectAssertion(new RDFResource("ex:indiv3"), new RDFResource("ex:objProp"), new RDFResource("ex:indiv4"));

            Assert.IsFalse(ontology.Data.CheckIsIndividualOf(ontology.Model, new RDFResource("ex:indiv1"), new RDFResource("ex:hsRest")));
            Assert.IsTrue(ontology.Data.CheckIsIndividualOf(ontology.Model, new RDFResource("ex:indiv2"), new RDFResource("ex:hsRest")));
            Assert.IsFalse(ontology.Data.CheckIsIndividualOf(ontology.Model, new RDFResource("ex:indiv3"), new RDFResource("ex:hsRest")));
            Assert.IsFalse(ontology.Data.CheckIsIndividualOf(ontology.Model, new RDFResource("ex:indiv4"), new RDFResource("ex:hsRest")));
        }

        [TestMethod]
        public void ShouldCheckIsIndividualsOfCompositeUnion()
        {
            OWLOntology ontology = new OWLOntology("ex:ont");
            ontology.Model.ClassModel.DeclareClass(new RDFResource("ex:class1"));
            ontology.Model.ClassModel.DeclareClass(new RDFResource("ex:class2"));
            ontology.Model.ClassModel.DeclareClass(new RDFResource("ex:class3"));
            ontology.Model.ClassModel.DeclareUnionClass(new RDFResource("ex:unionClass"), new List<RDFResource>() {
              new RDFResource("ex:class1"), new RDFResource("ex:class2") });
            ontology.Data.DeclareIndividual(new RDFResource("ex:indiv1"));
            ontology.Data.DeclareIndividual(new RDFResource("ex:indiv2"));
            ontology.Data.DeclareIndividual(new RDFResource("ex:indiv3"));
            ontology.Data.DeclareIndividualType(new RDFResource("ex:indiv1"), new RDFResource("ex:class1"));
            ontology.Data.DeclareIndividualType(new RDFResource("ex:indiv2"), new RDFResource("ex:class2"));
            ontology.Data.DeclareIndividualType(new RDFResource("ex:indiv3"), new RDFResource("ex:class3"));

            Assert.IsTrue(ontology.Data.CheckIsIndividualOf(ontology.Model, new RDFResource("ex:indiv1"), new RDFResource("ex:unionClass")));
            Assert.IsTrue(ontology.Data.CheckIsIndividualOf(ontology.Model, new RDFResource("ex:indiv2"), new RDFResource("ex:unionClass")));
            Assert.IsFalse(ontology.Data.CheckIsIndividualOf(ontology.Model, new RDFResource("ex:indiv3"), new RDFResource("ex:unionClass")));
        }

        [TestMethod]
        public void ShouldCheckIsIndividualsOfCompositeIntersection()
        {
            OWLOntology ontology = new OWLOntology("ex:ont");
            ontology.Model.ClassModel.DeclareClass(new RDFResource("ex:class1"));
            ontology.Model.ClassModel.DeclareClass(new RDFResource("ex:class2"));
            ontology.Model.ClassModel.DeclareClass(new RDFResource("ex:class3"));
            ontology.Model.ClassModel.DeclareIntersectionClass(new RDFResource("ex:intersectionClass"), new List<RDFResource>() {
              new RDFResource("ex:class1"), new RDFResource("ex:class2") });
            ontology.Data.DeclareIndividual(new RDFResource("ex:indiv1"));
            ontology.Data.DeclareIndividual(new RDFResource("ex:indiv2"));
            ontology.Data.DeclareIndividual(new RDFResource("ex:indiv3"));
            ontology.Data.DeclareIndividualType(new RDFResource("ex:indiv1"), new RDFResource("ex:class1"));
            ontology.Data.DeclareIndividualType(new RDFResource("ex:indiv2"), new RDFResource("ex:class2"));
            ontology.Data.DeclareIndividualType(new RDFResource("ex:indiv3"), new RDFResource("ex:class1"));
            ontology.Data.DeclareIndividualType(new RDFResource("ex:indiv3"), new RDFResource("ex:class2"));
            ontology.Data.DeclareIndividualType(new RDFResource("ex:indiv3"), new RDFResource("ex:class3"));

            Assert.IsFalse(ontology.Data.CheckIsIndividualOf(ontology.Model, new RDFResource("ex:indiv1"), new RDFResource("ex:intersectionClass")));
            Assert.IsFalse(ontology.Data.CheckIsIndividualOf(ontology.Model, new RDFResource("ex:indiv2"), new RDFResource("ex:intersectionClass")));
            Assert.IsTrue(ontology.Data.CheckIsIndividualOf(ontology.Model, new RDFResource("ex:indiv3"), new RDFResource("ex:intersectionClass")));
        }

        [TestMethod]
        public void ShouldCheckIsIndividualsOfCompositeComplement()
        {
            OWLOntology ontology = new OWLOntology("ex:ont");
            ontology.Model.ClassModel.DeclareClass(new RDFResource("ex:class1"));
            ontology.Model.ClassModel.DeclareClass(new RDFResource("ex:class2"));
            ontology.Model.ClassModel.DeclareClass(new RDFResource("ex:class3"));
            ontology.Model.ClassModel.DeclareComplementClass(new RDFResource("ex:complementClass"), new RDFResource("ex:class1"));
            ontology.Data.DeclareIndividual(new RDFResource("ex:indiv1"));
            ontology.Data.DeclareIndividual(new RDFResource("ex:indiv2"));
            ontology.Data.DeclareIndividual(new RDFResource("ex:indiv3"));
            ontology.Data.DeclareIndividualType(new RDFResource("ex:indiv1"), new RDFResource("ex:class1"));
            ontology.Data.DeclareIndividualType(new RDFResource("ex:indiv2"), new RDFResource("ex:class2"));
            ontology.Data.DeclareIndividualType(new RDFResource("ex:indiv3"), new RDFResource("ex:class1"));
            ontology.Data.DeclareIndividualType(new RDFResource("ex:indiv3"), new RDFResource("ex:class2"));
            ontology.Data.DeclareIndividualType(new RDFResource("ex:indiv3"), new RDFResource("ex:class3"));

            Assert.IsFalse(ontology.Data.CheckIsIndividualOf(ontology.Model, new RDFResource("ex:indiv1"), new RDFResource("ex:complementClass")));
            Assert.IsTrue(ontology.Data.CheckIsIndividualOf(ontology.Model, new RDFResource("ex:indiv2"), new RDFResource("ex:complementClass")));
            Assert.IsFalse(ontology.Data.CheckIsIndividualOf(ontology.Model, new RDFResource("ex:indiv3"), new RDFResource("ex:complementClass")));
        }
        #endregion

        #region Checker
        [TestMethod]
        public void ShouldCheckSameAsCompatibility()
        {
            OWLOntologyData data = new OWLOntologyData();
            data.DeclareIndividual(new RDFResource("ex:indiv1"));
            data.DeclareIndividual(new RDFResource("ex:indiv2"));

            Assert.IsTrue(data.CheckSameAsCompatibility(new RDFResource("ex:indiv1"), new RDFResource("ex:indiv2")));
            Assert.IsTrue(data.CheckSameAsCompatibility(new RDFResource("ex:indiv2"), new RDFResource("ex:indiv1")));
        }

        [TestMethod]
        public void ShouldCheckSameAsIncompatibility()
        {
            OWLOntologyData data = new OWLOntologyData();
            data.DeclareIndividual(new RDFResource("ex:indiv1"));
            data.DeclareIndividual(new RDFResource("ex:indiv2"));
            data.DeclareDifferentIndividuals(new RDFResource("ex:indiv1"), new RDFResource("ex:indiv2"));

            Assert.IsFalse(data.CheckSameAsCompatibility(new RDFResource("ex:indiv1"), new RDFResource("ex:indiv2")));
            Assert.IsFalse(data.CheckSameAsCompatibility(new RDFResource("ex:indiv2"), new RDFResource("ex:indiv1")));
        }

        [TestMethod]
        public void ShouldCheckSameAsIncompatibilityWithReasoning()
        {
            OWLOntologyData data = new OWLOntologyData();
            data.DeclareIndividual(new RDFResource("ex:indiv1"));
            data.DeclareIndividual(new RDFResource("ex:indiv2"));
            data.DeclareIndividual(new RDFResource("ex:indiv2"));
            data.DeclareDifferentIndividuals(new RDFResource("ex:indiv1"), new RDFResource("ex:indiv2"));
            data.DeclareSameIndividuals(new RDFResource("ex:indiv2"), new RDFResource("ex:indiv3"));

            Assert.IsFalse(data.CheckSameAsCompatibility(new RDFResource("ex:indiv1"), new RDFResource("ex:indiv2")));
            Assert.IsFalse(data.CheckSameAsCompatibility(new RDFResource("ex:indiv2"), new RDFResource("ex:indiv1")));
            Assert.IsFalse(data.CheckSameAsCompatibility(new RDFResource("ex:indiv1"), new RDFResource("ex:indiv3")));
            Assert.IsFalse(data.CheckSameAsCompatibility(new RDFResource("ex:indiv3"), new RDFResource("ex:indiv1")));
        }

        [TestMethod]
        public void ShouldCheckDifferentFromCompatibility()
        {
            OWLOntologyData data = new OWLOntologyData();
            data.DeclareIndividual(new RDFResource("ex:indiv1"));
            data.DeclareIndividual(new RDFResource("ex:indiv2"));

            Assert.IsTrue(data.CheckDifferentFromCompatibility(new RDFResource("ex:indiv1"), new RDFResource("ex:indiv2")));
            Assert.IsTrue(data.CheckDifferentFromCompatibility(new RDFResource("ex:indiv2"), new RDFResource("ex:indiv1")));
        }

        [TestMethod]
        public void ShouldCheckDifferentFromIncompatibility()
        {
            OWLOntologyData data = new OWLOntologyData();
            data.DeclareIndividual(new RDFResource("ex:indiv1"));
            data.DeclareIndividual(new RDFResource("ex:indiv2"));
            data.DeclareSameIndividuals(new RDFResource("ex:indiv1"), new RDFResource("ex:indiv2"));

            Assert.IsFalse(data.CheckDifferentFromCompatibility(new RDFResource("ex:indiv1"), new RDFResource("ex:indiv2")));
            Assert.IsFalse(data.CheckDifferentFromCompatibility(new RDFResource("ex:indiv2"), new RDFResource("ex:indiv1")));
        }

        [TestMethod]
        public void ShouldCheckDifferentFromIncompatibilityWithReasoning()
        {
            OWLOntologyData data = new OWLOntologyData();
            data.DeclareIndividual(new RDFResource("ex:indiv1"));
            data.DeclareIndividual(new RDFResource("ex:indiv2"));
            data.DeclareIndividual(new RDFResource("ex:indiv2"));
            data.DeclareSameIndividuals(new RDFResource("ex:indiv1"), new RDFResource("ex:indiv2"));
            data.DeclareSameIndividuals(new RDFResource("ex:indiv2"), new RDFResource("ex:indiv3"));

            Assert.IsFalse(data.CheckDifferentFromCompatibility(new RDFResource("ex:indiv1"), new RDFResource("ex:indiv2")));
            Assert.IsFalse(data.CheckDifferentFromCompatibility(new RDFResource("ex:indiv2"), new RDFResource("ex:indiv1")));
            Assert.IsFalse(data.CheckDifferentFromCompatibility(new RDFResource("ex:indiv1"), new RDFResource("ex:indiv3")));
            Assert.IsFalse(data.CheckDifferentFromCompatibility(new RDFResource("ex:indiv3"), new RDFResource("ex:indiv1")));
            Assert.IsFalse(data.CheckDifferentFromCompatibility(new RDFResource("ex:indiv2"), new RDFResource("ex:indiv3")));
            Assert.IsFalse(data.CheckDifferentFromCompatibility(new RDFResource("ex:indiv3"), new RDFResource("ex:indiv2")));
        }

        [TestMethod]
        public void ShouldCheckObjectAssertionCompatibility()
        {
            OWLOntologyData data = new OWLOntologyData();
            data.DeclareIndividual(new RDFResource("ex:indiv1"));
            data.DeclareIndividual(new RDFResource("ex:indiv2"));

            Assert.IsTrue(data.CheckObjectAssertionCompatibility(new RDFResource("ex:indiv1"), new RDFResource("ex:objProp"), new RDFResource("ex:indiv2")));
        }

        [TestMethod]
        public void ShouldCheckObjectAssertionIncompatibility()
        {
            OWLOntologyData data = new OWLOntologyData();
            data.DeclareIndividual(new RDFResource("ex:indiv1"));
            data.DeclareIndividual(new RDFResource("ex:indiv2"));
            data.DeclareNegativeObjectAssertion(new RDFResource("ex:indiv1"), new RDFResource("ex:objProp"), new RDFResource("ex:indiv2"));

            Assert.IsFalse(data.CheckObjectAssertionCompatibility(new RDFResource("ex:indiv1"), new RDFResource("ex:objProp"), new RDFResource("ex:indiv2")));
        }

        [TestMethod]
        public void ShouldCheckNegativeObjectAssertionCompatibility()
        {
            OWLOntologyData data = new OWLOntologyData();
            data.DeclareIndividual(new RDFResource("ex:indiv1"));
            data.DeclareIndividual(new RDFResource("ex:indiv2"));

            Assert.IsTrue(data.CheckNegativeObjectAssertionCompatibility(new RDFResource("ex:indiv1"), new RDFResource("ex:objProp"), new RDFResource("ex:indiv2")));
        }

        [TestMethod]
        public void ShouldCheckNegativeObjectAssertionIncompatibility()
        {
            OWLOntologyData data = new OWLOntologyData();
            data.DeclareIndividual(new RDFResource("ex:indiv1"));
            data.DeclareIndividual(new RDFResource("ex:indiv2"));
            data.DeclareObjectAssertion(new RDFResource("ex:indiv1"), new RDFResource("ex:objProp"), new RDFResource("ex:indiv2"));

            Assert.IsFalse(data.CheckNegativeObjectAssertionCompatibility(new RDFResource("ex:indiv1"), new RDFResource("ex:objProp"), new RDFResource("ex:indiv2")));
        }

        [TestMethod]
        public void ShouldCheckDatatypeAssertionCompatibility()
        {
            OWLOntologyData data = new OWLOntologyData();
            data.DeclareIndividual(new RDFResource("ex:indiv1"));

            Assert.IsTrue(data.CheckDatatypeAssertionCompatibility(new RDFResource("ex:indiv1"), new RDFResource("ex:dtProp"), new RDFPlainLiteral("hello")));
        }

        [TestMethod]
        public void ShouldCheckDatatypeAssertionIncompatibility()
        {
            OWLOntologyData data = new OWLOntologyData();
            data.DeclareIndividual(new RDFResource("ex:indiv1"));
            data.DeclareNegativeDatatypeAssertion(new RDFResource("ex:indiv1"), new RDFResource("ex:dtProp"), new RDFPlainLiteral("hello"));

            Assert.IsFalse(data.CheckDatatypeAssertionCompatibility(new RDFResource("ex:indiv1"), new RDFResource("ex:dtProp"), new RDFPlainLiteral("hello")));
        }

        [TestMethod]
        public void ShouldCheckNegativeDatatypeAssertionCompatibility()
        {
            OWLOntologyData data = new OWLOntologyData();
            data.DeclareIndividual(new RDFResource("ex:indiv1"));

            Assert.IsTrue(data.CheckNegativeDatatypeAssertionCompatibility(new RDFResource("ex:indiv1"), new RDFResource("ex:dtProp"), new RDFPlainLiteral("hello")));
        }

        [TestMethod]
        public void ShouldCheckNegativeDatatypeAssertionIncompatibility()
        {
            OWLOntologyData data = new OWLOntologyData();
            data.DeclareIndividual(new RDFResource("ex:indiv1"));
            data.DeclareDatatypeAssertion(new RDFResource("ex:indiv1"), new RDFResource("ex:dtProp"), new RDFPlainLiteral("hello"));

            Assert.IsFalse(data.CheckNegativeDatatypeAssertionCompatibility(new RDFResource("ex:indiv1"), new RDFResource("ex:dtProp"), new RDFPlainLiteral("hello")));
        }
        #endregion
    }
}