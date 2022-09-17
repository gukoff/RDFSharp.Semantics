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

using System.Collections.Generic;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using RDFSharp.Model;

namespace RDFSharp.Semantics.Test
{
    [TestClass]
    public class RDFOntologyDataHelperTest
    {
        #region Analyzer
        [TestMethod]
        public void ShouldCheckHasIndividual()
        {
            RDFOntologyData data = new RDFOntologyData();
            data.DeclareIndividual(new RDFResource("ex:indivA"));

            Assert.IsTrue(data.CheckHasIndividual(new RDFResource("ex:indivA")));
        }

        [TestMethod]
        public void ShouldCheckHasNotIndividual()
        {
            RDFOntologyData data = new RDFOntologyData();
            data.DeclareIndividual(new RDFResource("ex:indivA"));

            Assert.IsFalse(data.CheckHasIndividual(new RDFResource("ex:indivB")));
            Assert.IsFalse(data.CheckHasIndividual(null));
            Assert.IsFalse(new RDFOntologyData().CheckHasIndividual(new RDFResource("ex:indivA")));
        }

        [TestMethod]
        public void ShouldCheckHasResourceAnnotation()
        {
            RDFOntologyData data = new RDFOntologyData();
            data.DeclareIndividual(new RDFResource("ex:indivA"));
            data.AnnotateIndividual(new RDFResource("ex:indivA"), RDFVocabulary.RDFS.SEE_ALSO, new RDFResource("ex:seealso"));

            Assert.IsTrue(data.CheckHasAnnotation(new RDFResource("ex:indivA"), RDFVocabulary.RDFS.SEE_ALSO, new RDFResource("ex:seealso")));
        }

        [TestMethod]
        public void ShouldCheckHasNotResourceAnnotation()
        {
            RDFOntologyData data = new RDFOntologyData();
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
            RDFOntologyData data = new RDFOntologyData();
            data.DeclareIndividual(new RDFResource("ex:indivA"));
            data.AnnotateIndividual(new RDFResource("ex:indivA"), RDFVocabulary.RDFS.LABEL, new RDFPlainLiteral("label"));

            Assert.IsTrue(data.CheckHasAnnotation(new RDFResource("ex:indivA"), RDFVocabulary.RDFS.LABEL, new RDFPlainLiteral("label")));
        }

        [TestMethod]
        public void ShouldCheckHasNotLiteralAnnotation()
        {
            RDFOntologyData data = new RDFOntologyData();
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
            RDFOntologyData data = new RDFOntologyData();
            data.DeclareIndividual(new RDFResource("ex:indivA"));
            data.DeclareIndividual(new RDFResource("ex:indivB"));
            data.DeclareObjectAssertion(new RDFResource("ex:indivA"), RDFVocabulary.FOAF.KNOWS, new RDFResource("ex:indivB"));

            Assert.IsTrue(data.CheckHasObjectAssertion(new RDFResource("ex:indivA"), RDFVocabulary.FOAF.KNOWS, new RDFResource("ex:indivB")));
        }

        [TestMethod]
        public void ShouldCheckHasNotObjectAssertion()
        {
            RDFOntologyData data = new RDFOntologyData();
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
            RDFOntologyData data = new RDFOntologyData();
            data.DeclareIndividual(new RDFResource("ex:indivA"));
            data.DeclareDatatypeAssertion(new RDFResource("ex:indivA"), RDFVocabulary.FOAF.AGE, new RDFTypedLiteral("25", RDFModelEnums.RDFDatatypes.XSD_INTEGER));

            Assert.IsTrue(data.CheckHasDatatypeAssertion(new RDFResource("ex:indivA"), RDFVocabulary.FOAF.AGE, new RDFTypedLiteral("25", RDFModelEnums.RDFDatatypes.XSD_INTEGER)));
        }

        [TestMethod]
        public void ShouldCheckHasNotDatatypeAssertion()
        {
            RDFOntologyData data = new RDFOntologyData();
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
            RDFOntologyData data = new RDFOntologyData();
            data.DeclareIndividual(new RDFResource("ex:indivA"));
            data.DeclareIndividual(new RDFResource("ex:indivB"));
            data.DeclareNegativeObjectAssertion(new RDFResource("ex:indivA"), RDFVocabulary.FOAF.KNOWS, new RDFResource("ex:indivB"));

            Assert.IsTrue(data.CheckHasNegativeObjectAssertion(new RDFResource("ex:indivA"), RDFVocabulary.FOAF.KNOWS, new RDFResource("ex:indivB")));
        }

        [TestMethod]
        public void ShouldCheckHasNotNegativeObjectAssertion()
        {
            RDFOntologyData data = new RDFOntologyData();
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
            RDFOntologyData data = new RDFOntologyData();
            data.DeclareIndividual(new RDFResource("ex:indivA"));
            data.DeclareNegativeDatatypeAssertion(new RDFResource("ex:indivA"), RDFVocabulary.FOAF.AGE, new RDFTypedLiteral("25", RDFModelEnums.RDFDatatypes.XSD_INTEGER));

            Assert.IsTrue(data.CheckHasNegativeDatatypeAssertion(new RDFResource("ex:indivA"), RDFVocabulary.FOAF.AGE, new RDFTypedLiteral("25", RDFModelEnums.RDFDatatypes.XSD_INTEGER)));
        }

        [TestMethod]
        public void ShouldCheckHasNotNegativeDatatypeAssertion()
        {
            RDFOntologyData data = new RDFOntologyData();
            data.DeclareIndividual(new RDFResource("ex:indivA"));
            data.DeclareNegativeDatatypeAssertion(new RDFResource("ex:indivA"), RDFVocabulary.FOAF.AGE, new RDFTypedLiteral("25", RDFModelEnums.RDFDatatypes.XSD_INTEGER));

            Assert.IsFalse(data.CheckHasNegativeDatatypeAssertion(new RDFResource("ex:indivA"), RDFVocabulary.FOAF.AGE, new RDFTypedLiteral("27", RDFModelEnums.RDFDatatypes.XSD_INTEGER)));
            Assert.IsFalse(data.CheckHasNegativeDatatypeAssertion(null, RDFVocabulary.FOAF.AGE, new RDFTypedLiteral("25", RDFModelEnums.RDFDatatypes.XSD_INTEGER)));
            Assert.IsFalse(data.CheckHasNegativeDatatypeAssertion(new RDFResource("ex:indivA"), null, new RDFTypedLiteral("25", RDFModelEnums.RDFDatatypes.XSD_INTEGER)));
            Assert.IsFalse(data.CheckHasNegativeDatatypeAssertion(new RDFResource("ex:indivA"), RDFVocabulary.FOAF.AGE, null));
        }

        [TestMethod]
        public void ShouldCheckAreSameIndividuals()
        {
            RDFOntologyData data = new RDFOntologyData();
            data.DeclareIndividual(new RDFResource("ex:indivA"));
            data.DeclareIndividual(new RDFResource("ex:indivB"));
            data.DeclareIndividual(new RDFResource("ex:indivC"));
            data.DeclareSameIndividuals(new RDFResource("ex:indivA"), new RDFResource("ex:indivB"));
            data.DeclareSameIndividuals(new RDFResource("ex:indivB"), new RDFResource("ex:indivC"));

            Assert.IsTrue(data.CheckAreSameIndividuals(new RDFResource("ex:indivA"), new RDFResource("ex:indivB")));
            Assert.IsTrue(data.CheckAreSameIndividuals(new RDFResource("ex:indivB"), new RDFResource("ex:indivA"))); //Inferred
            Assert.IsTrue(data.CheckAreSameIndividuals(new RDFResource("ex:indivB"), new RDFResource("ex:indivC")));
            Assert.IsTrue(data.CheckAreSameIndividuals(new RDFResource("ex:indivC"), new RDFResource("ex:indivB"))); //Inferred
            Assert.IsTrue(data.CheckAreSameIndividuals(new RDFResource("ex:indivA"), new RDFResource("ex:indivC"))); //Inferred
            Assert.IsTrue(data.CheckAreSameIndividuals(new RDFResource("ex:indivC"), new RDFResource("ex:indivA"))); //Inferred
        }

        [TestMethod]
        public void ShouldCheckAreNotSameIndividuals()
        {
            RDFOntologyData data = new RDFOntologyData();
            data.DeclareIndividual(new RDFResource("ex:indivA"));
            data.DeclareIndividual(new RDFResource("ex:indivB"));
            data.DeclareIndividual(new RDFResource("ex:indivC"));
            data.DeclareSameIndividuals(new RDFResource("ex:indivA"), new RDFResource("ex:indivB"));

            Assert.IsFalse(data.CheckAreSameIndividuals(new RDFResource("ex:indivA"), new RDFResource("ex:indivC")));
            Assert.IsFalse(data.CheckAreSameIndividuals(new RDFResource("ex:indivB"), new RDFResource("ex:indivC")));
            Assert.IsFalse(data.CheckAreSameIndividuals(null, new RDFResource("ex:indivB")));
            Assert.IsFalse(data.CheckAreSameIndividuals(new RDFResource("ex:indivA"), null));
        }

        [TestMethod]
        public void ShouldCheckAreDifferentIndividuals()
        {
            RDFOntologyData data = new RDFOntologyData();
            data.DeclareIndividual(new RDFResource("ex:indivA"));
            data.DeclareIndividual(new RDFResource("ex:indivB"));
            data.DeclareIndividual(new RDFResource("ex:indivC"));
            data.DeclareIndividual(new RDFResource("ex:indivD"));
            data.DeclareIndividual(new RDFResource("ex:indivE"));
            data.DeclareDifferentIndividuals(new RDFResource("ex:indivA"), new RDFResource("ex:indivB"));
            data.DeclareSameIndividuals(new RDFResource("ex:indivB"), new RDFResource("ex:indivC"));
            data.DeclareDifferentIndividuals(new RDFResource("ex:indivC"), new RDFResource("ex:indivD"));
            data.DeclareSameIndividuals(new RDFResource("ex:indivC"), new RDFResource("ex:indivE"));

            Assert.IsTrue(data.CheckAreDifferentIndividuals(new RDFResource("ex:indivA"), new RDFResource("ex:indivB")));
            Assert.IsTrue(data.CheckAreDifferentIndividuals(new RDFResource("ex:indivB"), new RDFResource("ex:indivA"))); //Inferred
            Assert.IsTrue(data.CheckAreDifferentIndividuals(new RDFResource("ex:indivA"), new RDFResource("ex:indivC"))); //Inferred
            Assert.IsTrue(data.CheckAreDifferentIndividuals(new RDFResource("ex:indivC"), new RDFResource("ex:indivA"))); //Inferred
            Assert.IsTrue(data.CheckAreDifferentIndividuals(new RDFResource("ex:indivA"), new RDFResource("ex:indivE"))); //Inferred
            Assert.IsTrue(data.CheckAreDifferentIndividuals(new RDFResource("ex:indivE"), new RDFResource("ex:indivA"))); //Inferred
            Assert.IsTrue(data.CheckAreDifferentIndividuals(new RDFResource("ex:indivB"), new RDFResource("ex:indivD"))); //Inferred
            Assert.IsTrue(data.CheckAreDifferentIndividuals(new RDFResource("ex:indivD"), new RDFResource("ex:indivB"))); //Inferred
            Assert.IsTrue(data.CheckAreDifferentIndividuals(new RDFResource("ex:indivD"), new RDFResource("ex:indivE"))); //Inferred
            Assert.IsTrue(data.CheckAreDifferentIndividuals(new RDFResource("ex:indivE"), new RDFResource("ex:indivD"))); //Inferred
        }

        [TestMethod]
        public void ShouldCheckAreNotDifferentIndividuals()
        {
            RDFOntologyData data = new RDFOntologyData();
            data.DeclareIndividual(new RDFResource("ex:indivA"));
            data.DeclareIndividual(new RDFResource("ex:indivB"));
            data.DeclareIndividual(new RDFResource("ex:indivC"));
            data.DeclareDifferentIndividuals(new RDFResource("ex:indivA"), new RDFResource("ex:indivB"));

            Assert.IsFalse(data.CheckAreDifferentIndividuals(new RDFResource("ex:indivA"), new RDFResource("ex:indivC")));
            Assert.IsFalse(data.CheckAreDifferentIndividuals(null, new RDFResource("ex:indivC")));
            Assert.IsFalse(data.CheckAreDifferentIndividuals(new RDFResource("ex:indivB"), null));
        }

        [TestMethod]
        public void ShouldCheckAreDifferentIndividualsWithAllDifferentShortcut()
        {
            RDFOntologyData data = new RDFOntologyData();
            data.DeclareIndividual(new RDFResource("ex:indivA"));
            data.DeclareIndividual(new RDFResource("ex:indivB"));
            data.DeclareIndividual(new RDFResource("ex:indivC"));
            data.DeclareIndividual(new RDFResource("ex:indivD"));
            data.DeclareIndividual(new RDFResource("ex:indivE"));
            data.DeclareAllDifferentIndividuals(new RDFResource("ex:alldiff"), new List<RDFResource>() {
                new RDFResource("ex:indivA"), new RDFResource("ex:indivB"), new RDFResource("ex:indivD") });
            data.DeclareSameIndividuals(new RDFResource("ex:indivC"), new RDFResource("ex:indivD"));
            data.DeclareSameIndividuals(new RDFResource("ex:indivE"), new RDFResource("ex:indivD"));

            Assert.IsTrue(data.CheckAreDifferentIndividuals(new RDFResource("ex:indivA"), new RDFResource("ex:indivB")));
            Assert.IsTrue(data.CheckAreDifferentIndividuals(new RDFResource("ex:indivB"), new RDFResource("ex:indivA"))); //Inferred
            Assert.IsTrue(data.CheckAreDifferentIndividuals(new RDFResource("ex:indivA"), new RDFResource("ex:indivD")));
            Assert.IsTrue(data.CheckAreDifferentIndividuals(new RDFResource("ex:indivD"), new RDFResource("ex:indivA"))); //Inferred
            Assert.IsTrue(data.CheckAreDifferentIndividuals(new RDFResource("ex:indivB"), new RDFResource("ex:indivD")));
            Assert.IsTrue(data.CheckAreDifferentIndividuals(new RDFResource("ex:indivD"), new RDFResource("ex:indivB"))); //Inferred
            Assert.IsTrue(data.CheckAreDifferentIndividuals(new RDFResource("ex:indivA"), new RDFResource("ex:indivC"))); //Inferred
            Assert.IsTrue(data.CheckAreDifferentIndividuals(new RDFResource("ex:indivC"), new RDFResource("ex:indivA"))); //Inferred
            Assert.IsTrue(data.CheckAreDifferentIndividuals(new RDFResource("ex:indivA"), new RDFResource("ex:indivE"))); //Inferred
            Assert.IsTrue(data.CheckAreDifferentIndividuals(new RDFResource("ex:indivE"), new RDFResource("ex:indivA"))); //Inferred
            Assert.IsTrue(data.CheckAreDifferentIndividuals(new RDFResource("ex:indivB"), new RDFResource("ex:indivC"))); //Inferred
            Assert.IsTrue(data.CheckAreDifferentIndividuals(new RDFResource("ex:indivC"), new RDFResource("ex:indivB"))); //Inferred
            Assert.IsTrue(data.CheckAreDifferentIndividuals(new RDFResource("ex:indivB"), new RDFResource("ex:indivE"))); //Inferred
            Assert.IsTrue(data.CheckAreDifferentIndividuals(new RDFResource("ex:indivE"), new RDFResource("ex:indivB"))); //Inferred
        }

        [TestMethod]
        public void ShouldCheckAreTransitiveRelatedIndividuals()
        {
            RDFOntologyData data = new RDFOntologyData();
            data.DeclareIndividual(new RDFResource("ex:indivA"));
            data.DeclareIndividual(new RDFResource("ex:indivB"));
            data.DeclareIndividual(new RDFResource("ex:indivC"));
            data.DeclareIndividual(new RDFResource("ex:indivD"));
            data.DeclareObjectAssertion(new RDFResource("ex:indivA"), new RDFResource("ex:genderOf"), new RDFResource("ex:indivB"));
            data.DeclareObjectAssertion(new RDFResource("ex:indivB"), new RDFResource("ex:genderOf"), new RDFResource("ex:indivC"));
            data.DeclareObjectAssertion(new RDFResource("ex:indivC"), new RDFResource("ex:genderOf"), new RDFResource("ex:indivD"));

            Assert.IsTrue(data.CheckAreTransitiveRelatedIndividuals(new RDFResource("ex:indivA"), new RDFResource("ex:genderOf"), new RDFResource("ex:indivC")));
            Assert.IsTrue(data.CheckAreTransitiveRelatedIndividuals(new RDFResource("ex:indivA"), new RDFResource("ex:genderOf"), new RDFResource("ex:indivD")));
            Assert.IsTrue(data.CheckAreTransitiveRelatedIndividuals(new RDFResource("ex:indivB"), new RDFResource("ex:genderOf"), new RDFResource("ex:indivD")));
        }

        [TestMethod]
        public void ShouldCheckAreNotTransitiveRelatedIndividuals()
        {
            RDFOntologyData data = new RDFOntologyData();
            data.DeclareIndividual(new RDFResource("ex:indivA"));
            data.DeclareIndividual(new RDFResource("ex:indivB"));
            data.DeclareIndividual(new RDFResource("ex:indivC"));
            data.DeclareIndividual(new RDFResource("ex:indivD"));
            data.DeclareObjectAssertion(new RDFResource("ex:indivA"), new RDFResource("ex:genderOf"), new RDFResource("ex:indivB"));
            data.DeclareObjectAssertion(new RDFResource("ex:indivC"), new RDFResource("ex:genderOf"), new RDFResource("ex:indivD"));

            Assert.IsFalse(data.CheckAreTransitiveRelatedIndividuals(new RDFResource("ex:indivA"), new RDFResource("ex:genderOf"), new RDFResource("ex:indivC")));
            Assert.IsFalse(data.CheckAreTransitiveRelatedIndividuals(null, new RDFResource("ex:genderOf"), new RDFResource("ex:indivD")));
            Assert.IsFalse(data.CheckAreTransitiveRelatedIndividuals(new RDFResource("ex:indivB"), null, new RDFResource("ex:indivD")));
            Assert.IsFalse(data.CheckAreTransitiveRelatedIndividuals(new RDFResource("ex:indivB"), new RDFResource("ex:genderOf"), null));
        }

        [TestMethod]
        public void ShouldCheckIsIndividualOfClass()
        {
            RDFOntology ontology = new RDFOntology("ex:ont");
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

            Assert.IsTrue(ontology.Data.CheckIsIndividualOfClass(ontology.Model, new RDFResource("ex:indiv1"), new RDFResource("ex:classA")));
            Assert.IsTrue(ontology.Data.CheckIsIndividualOfClass(ontology.Model, new RDFResource("ex:indiv2"), new RDFResource("ex:classB")));
            Assert.IsTrue(ontology.Data.CheckIsIndividualOfClass(ontology.Model, new RDFResource("ex:indiv3"), new RDFResource("ex:classC")));
            Assert.IsTrue(ontology.Data.CheckIsIndividualOfClass(ontology.Model, new RDFResource("ex:indiv2"), new RDFResource("ex:classA"))); //Inference
            Assert.IsTrue(ontology.Data.CheckIsIndividualOfClass(ontology.Model, new RDFResource("ex:indiv2"), new RDFResource("ex:classC"))); //Inference
            Assert.IsTrue(ontology.Data.CheckIsIndividualOfClass(ontology.Model, new RDFResource("ex:indiv3"), new RDFResource("ex:classA"))); //Inference
            Assert.IsTrue(ontology.Data.CheckIsIndividualOfClass(ontology.Model, new RDFResource("ex:indiv3"), new RDFResource("ex:classB"))); //Inference
        }

        [TestMethod]
        public void ShouldCheckIsIndividualOfEnumerate()
        {
            RDFOntology ontology = new RDFOntology("ex:ont");
            ontology.Model.ClassModel.DeclareEnumerateClass(new RDFResource("ex:enumClass"), new List<RDFResource>() { 
                new RDFResource("ex:indiv1"), new RDFResource("ex:indiv2") });
            ontology.Data.DeclareIndividual(new RDFResource("ex:indiv1"));
            ontology.Data.DeclareIndividual(new RDFResource("ex:indiv2"));

            Assert.IsTrue(ontology.Data.CheckIsIndividualOfClass(ontology.Model, new RDFResource("ex:indiv1"), new RDFResource("ex:enumClass")));
            Assert.IsTrue(ontology.Data.CheckIsIndividualOfClass(ontology.Model, new RDFResource("ex:indiv2"), new RDFResource("ex:enumClass")));
        }

        [TestMethod]
        public void ShouldCheckIsIndividualOfCardinalityRestriction()
        {
            RDFOntology ontology = new RDFOntology("ex:ont");
            ontology.Model.ClassModel.DeclareCardinalityRestriction(new RDFResource("ex:cardRest"), new RDFResource("ex:objProp"), 2);
            ontology.Model.PropertyModel.DeclareObjectProperty(new RDFResource("ex:objProp"));
            ontology.Data.DeclareIndividual(new RDFResource("ex:indiv1"));
            ontology.Data.DeclareIndividual(new RDFResource("ex:indiv2"));
            ontology.Data.DeclareIndividual(new RDFResource("ex:indiv3"));
            ontology.Data.DeclareObjectAssertion(new RDFResource("ex:indiv1"), new RDFResource("ex:objProp"), new RDFResource("ex:indiv2"));
            ontology.Data.DeclareObjectAssertion(new RDFResource("ex:indiv1"), new RDFResource("ex:objProp"), new RDFResource("ex:indiv3"));
            ontology.Data.DeclareObjectAssertion(new RDFResource("ex:indiv2"), new RDFResource("ex:objProp"), new RDFResource("ex:indiv3"));

            Assert.IsTrue(ontology.Data.CheckIsIndividualOfClass(ontology.Model, new RDFResource("ex:indiv1"), new RDFResource("ex:cardRest")));
            Assert.IsFalse(ontology.Data.CheckIsIndividualOfClass(ontology.Model, new RDFResource("ex:indiv2"), new RDFResource("ex:cardRest")));
            Assert.IsFalse(ontology.Data.CheckIsIndividualOfClass(ontology.Model, new RDFResource("ex:indiv3"), new RDFResource("ex:cardRest")));
        }

        [TestMethod]
        public void ShouldCheckIsIndividualOfCardinalityRestrictionWithReasoningOnProperty()
        {
            RDFOntology ontology = new RDFOntology("ex:ont");
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

            Assert.IsTrue(ontology.Data.CheckIsIndividualOfClass(ontology.Model, new RDFResource("ex:indiv1"), new RDFResource("ex:cardRest")));
            Assert.IsFalse(ontology.Data.CheckIsIndividualOfClass(ontology.Model, new RDFResource("ex:indiv2"), new RDFResource("ex:cardRest")));
            Assert.IsFalse(ontology.Data.CheckIsIndividualOfClass(ontology.Model, new RDFResource("ex:indiv3"), new RDFResource("ex:cardRest")));
        }

        [TestMethod]
        public void ShouldCheckIsIndividualOfQualifiedCardinalityRestriction()
        {
            RDFOntology ontology = new RDFOntology("ex:ont");
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

            Assert.IsTrue(ontology.Data.CheckIsIndividualOfClass(ontology.Model, new RDFResource("ex:indiv1"), new RDFResource("ex:qCardRest")));
            Assert.IsFalse(ontology.Data.CheckIsIndividualOfClass(ontology.Model, new RDFResource("ex:indiv2"), new RDFResource("ex:qCardRest")));
            Assert.IsFalse(ontology.Data.CheckIsIndividualOfClass(ontology.Model, new RDFResource("ex:indiv3"), new RDFResource("ex:qCardRest")));
            Assert.IsFalse(ontology.Data.CheckIsIndividualOfClass(ontology.Model, new RDFResource("ex:indiv4"), new RDFResource("ex:qCardRest")));
        }

        [TestMethod]
        public void ShouldCheckIsIndividualOfQualifiedCardinalityRestrictionWithReasoningOnProperty()
        {
            RDFOntology ontology = new RDFOntology("ex:ont");
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

            Assert.IsTrue(ontology.Data.CheckIsIndividualOfClass(ontology.Model, new RDFResource("ex:indiv1"), new RDFResource("ex:qCardRest")));
            Assert.IsFalse(ontology.Data.CheckIsIndividualOfClass(ontology.Model, new RDFResource("ex:indiv2"), new RDFResource("ex:qCardRest")));
            Assert.IsFalse(ontology.Data.CheckIsIndividualOfClass(ontology.Model, new RDFResource("ex:indiv3"), new RDFResource("ex:qCardRest")));
            Assert.IsFalse(ontology.Data.CheckIsIndividualOfClass(ontology.Model, new RDFResource("ex:indiv4"), new RDFResource("ex:qCardRest")));
        }

        [TestMethod]
        public void ShouldCheckIsIndividualOfMinCardinalityRestriction()
        {
            RDFOntology ontology = new RDFOntology("ex:ont");
            ontology.Model.ClassModel.DeclareMinCardinalityRestriction(new RDFResource("ex:cardRest"), new RDFResource("ex:objProp"), 1);
            ontology.Model.PropertyModel.DeclareObjectProperty(new RDFResource("ex:objProp"));
            ontology.Data.DeclareIndividual(new RDFResource("ex:indiv1"));
            ontology.Data.DeclareIndividual(new RDFResource("ex:indiv2"));
            ontology.Data.DeclareIndividual(new RDFResource("ex:indiv3"));
            ontology.Data.DeclareObjectAssertion(new RDFResource("ex:indiv1"), new RDFResource("ex:objProp"), new RDFResource("ex:indiv2"));
            ontology.Data.DeclareObjectAssertion(new RDFResource("ex:indiv1"), new RDFResource("ex:objProp"), new RDFResource("ex:indiv3"));
            ontology.Data.DeclareObjectAssertion(new RDFResource("ex:indiv2"), new RDFResource("ex:objProp"), new RDFResource("ex:indiv3"));

            Assert.IsTrue(ontology.Data.CheckIsIndividualOfClass(ontology.Model, new RDFResource("ex:indiv1"), new RDFResource("ex:cardRest")));
            Assert.IsTrue(ontology.Data.CheckIsIndividualOfClass(ontology.Model, new RDFResource("ex:indiv2"), new RDFResource("ex:cardRest")));
            Assert.IsFalse(ontology.Data.CheckIsIndividualOfClass(ontology.Model, new RDFResource("ex:indiv3"), new RDFResource("ex:cardRest")));
        }

        [TestMethod]
        public void ShouldCheckIsIndividualOfMinQualifiedCardinalityRestriction()
        {
            RDFOntology ontology = new RDFOntology("ex:ont");
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

            Assert.IsTrue(ontology.Data.CheckIsIndividualOfClass(ontology.Model, new RDFResource("ex:indiv1"), new RDFResource("ex:qCardRest")));
            Assert.IsTrue(ontology.Data.CheckIsIndividualOfClass(ontology.Model, new RDFResource("ex:indiv2"), new RDFResource("ex:qCardRest")));
            Assert.IsTrue(ontology.Data.CheckIsIndividualOfClass(ontology.Model, new RDFResource("ex:indiv3"), new RDFResource("ex:qCardRest")));
            Assert.IsFalse(ontology.Data.CheckIsIndividualOfClass(ontology.Model, new RDFResource("ex:indiv4"), new RDFResource("ex:qCardRest")));
        }

        [TestMethod]
        public void ShouldCheckIsIndividualOfMaxCardinalityRestriction()
        {
            RDFOntology ontology = new RDFOntology("ex:ont");
            ontology.Model.ClassModel.DeclareMaxCardinalityRestriction(new RDFResource("ex:cardRest"), new RDFResource("ex:objProp"), 1);
            ontology.Model.PropertyModel.DeclareObjectProperty(new RDFResource("ex:objProp"));
            ontology.Data.DeclareIndividual(new RDFResource("ex:indiv1"));
            ontology.Data.DeclareIndividual(new RDFResource("ex:indiv2"));
            ontology.Data.DeclareIndividual(new RDFResource("ex:indiv3"));
            ontology.Data.DeclareObjectAssertion(new RDFResource("ex:indiv1"), new RDFResource("ex:objProp"), new RDFResource("ex:indiv2"));
            ontology.Data.DeclareObjectAssertion(new RDFResource("ex:indiv1"), new RDFResource("ex:objProp"), new RDFResource("ex:indiv3"));
            ontology.Data.DeclareObjectAssertion(new RDFResource("ex:indiv2"), new RDFResource("ex:objProp"), new RDFResource("ex:indiv3"));

            Assert.IsFalse(ontology.Data.CheckIsIndividualOfClass(ontology.Model, new RDFResource("ex:indiv1"), new RDFResource("ex:cardRest")));
            Assert.IsTrue(ontology.Data.CheckIsIndividualOfClass(ontology.Model, new RDFResource("ex:indiv2"), new RDFResource("ex:cardRest")));
            Assert.IsFalse(ontology.Data.CheckIsIndividualOfClass(ontology.Model, new RDFResource("ex:indiv3"), new RDFResource("ex:cardRest")));
        }

        [TestMethod]
        public void ShouldCheckIsIndividualOfMaxQualifiedCardinalityRestriction()
        {
            RDFOntology ontology = new RDFOntology("ex:ont");
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

            Assert.IsFalse(ontology.Data.CheckIsIndividualOfClass(ontology.Model, new RDFResource("ex:indiv1"), new RDFResource("ex:qCardRest")));
            Assert.IsFalse(ontology.Data.CheckIsIndividualOfClass(ontology.Model, new RDFResource("ex:indiv2"), new RDFResource("ex:qCardRest")));
            Assert.IsTrue(ontology.Data.CheckIsIndividualOfClass(ontology.Model, new RDFResource("ex:indiv3"), new RDFResource("ex:qCardRest")));
            Assert.IsFalse(ontology.Data.CheckIsIndividualOfClass(ontology.Model, new RDFResource("ex:indiv4"), new RDFResource("ex:qCardRest")));
        }

        [TestMethod]
        public void ShouldCheckIsIndividualOfMinMaxCardinalityRestriction()
        {
            RDFOntology ontology = new RDFOntology("ex:ont");
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

            Assert.IsTrue(ontology.Data.CheckIsIndividualOfClass(ontology.Model, new RDFResource("ex:indiv1"), new RDFResource("ex:cardRest")));
            Assert.IsTrue(ontology.Data.CheckIsIndividualOfClass(ontology.Model, new RDFResource("ex:indiv2"), new RDFResource("ex:cardRest")));
            Assert.IsFalse(ontology.Data.CheckIsIndividualOfClass(ontology.Model, new RDFResource("ex:indiv3"), new RDFResource("ex:cardRest")));
            Assert.IsFalse(ontology.Data.CheckIsIndividualOfClass(ontology.Model, new RDFResource("ex:indiv4"), new RDFResource("ex:cardRest")));
        }

        [TestMethod]
        public void ShouldCheckIsIndividualOfMinMaxQualifiedCardinalityRestriction()
        {
            RDFOntology ontology = new RDFOntology("ex:ont");
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

            Assert.IsTrue(ontology.Data.CheckIsIndividualOfClass(ontology.Model, new RDFResource("ex:indiv1"), new RDFResource("ex:qCardRest")));
            Assert.IsTrue(ontology.Data.CheckIsIndividualOfClass(ontology.Model, new RDFResource("ex:indiv2"), new RDFResource("ex:qCardRest")));
            Assert.IsFalse(ontology.Data.CheckIsIndividualOfClass(ontology.Model, new RDFResource("ex:indiv3"), new RDFResource("ex:qCardRest")));
            Assert.IsFalse(ontology.Data.CheckIsIndividualOfClass(ontology.Model, new RDFResource("ex:indiv4"), new RDFResource("ex:qCardRest")));
        }
        
        [TestMethod]
        public void ShouldCheckIsIndividualOfSomeValuesFromRestriction()
        {
            RDFOntology ontology = new RDFOntology("ex:ont");
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

            Assert.IsTrue(ontology.Data.CheckIsIndividualOfClass(ontology.Model, new RDFResource("ex:indiv1"), new RDFResource("ex:svFromRest")));
            Assert.IsTrue(ontology.Data.CheckIsIndividualOfClass(ontology.Model, new RDFResource("ex:indiv2"), new RDFResource("ex:svFromRest")));
            Assert.IsFalse(ontology.Data.CheckIsIndividualOfClass(ontology.Model, new RDFResource("ex:indiv3"), new RDFResource("ex:svFromRest")));
            Assert.IsFalse(ontology.Data.CheckIsIndividualOfClass(ontology.Model, new RDFResource("ex:indiv4"), new RDFResource("ex:svFromRest")));
        }

        [TestMethod]
        public void ShouldCheckIsIndividualOfSomeValuesFromRestrictionWithReasoning()
        {
            RDFOntology ontology = new RDFOntology("ex:ont");
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

            Assert.IsTrue(ontology.Data.CheckIsIndividualOfClass(ontology.Model, new RDFResource("ex:indiv1"), new RDFResource("ex:svFromRest")));
            Assert.IsTrue(ontology.Data.CheckIsIndividualOfClass(ontology.Model, new RDFResource("ex:indiv2"), new RDFResource("ex:svFromRest")));
            Assert.IsFalse(ontology.Data.CheckIsIndividualOfClass(ontology.Model, new RDFResource("ex:indiv3"), new RDFResource("ex:svFromRest")));
            Assert.IsFalse(ontology.Data.CheckIsIndividualOfClass(ontology.Model, new RDFResource("ex:indiv4"), new RDFResource("ex:svFromRest")));
        }

        [TestMethod]
        public void ShouldCheckIsIndividualOfAllValuesFromRestriction()
        {
            RDFOntology ontology = new RDFOntology("ex:ont");
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

            Assert.IsTrue(ontology.Data.CheckIsIndividualOfClass(ontology.Model, new RDFResource("ex:indiv1"), new RDFResource("ex:avFromRest")));
            Assert.IsTrue(ontology.Data.CheckIsIndividualOfClass(ontology.Model, new RDFResource("ex:indiv2"), new RDFResource("ex:avFromRest")));
            Assert.IsFalse(ontology.Data.CheckIsIndividualOfClass(ontology.Model, new RDFResource("ex:indiv3"), new RDFResource("ex:avFromRest")));
            Assert.IsFalse(ontology.Data.CheckIsIndividualOfClass(ontology.Model, new RDFResource("ex:indiv4"), new RDFResource("ex:avFromRest")));
        }

        [TestMethod]
        public void ShouldCheckIsIndividualOfAllValuesFromRestrictionWithReasoning()
        {
            RDFOntology ontology = new RDFOntology("ex:ont");
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

            Assert.IsTrue(ontology.Data.CheckIsIndividualOfClass(ontology.Model, new RDFResource("ex:indiv1"), new RDFResource("ex:avFromRest")));
            Assert.IsTrue(ontology.Data.CheckIsIndividualOfClass(ontology.Model, new RDFResource("ex:indiv2"), new RDFResource("ex:avFromRest")));
            Assert.IsFalse(ontology.Data.CheckIsIndividualOfClass(ontology.Model, new RDFResource("ex:indiv3"), new RDFResource("ex:avFromRest")));
            Assert.IsFalse(ontology.Data.CheckIsIndividualOfClass(ontology.Model, new RDFResource("ex:indiv4"), new RDFResource("ex:avFromRest")));
        }

        [TestMethod]
        public void ShouldCheckIsIndividualOfHasValueResourceRestriction()
        {
            RDFOntology ontology = new RDFOntology("ex:ont");
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

            Assert.IsTrue(ontology.Data.CheckIsIndividualOfClass(ontology.Model, new RDFResource("ex:indiv1"), new RDFResource("ex:hvRest")));
            Assert.IsFalse(ontology.Data.CheckIsIndividualOfClass(ontology.Model, new RDFResource("ex:indiv2"), new RDFResource("ex:hvRest")));
            Assert.IsTrue(ontology.Data.CheckIsIndividualOfClass(ontology.Model, new RDFResource("ex:indiv3"), new RDFResource("ex:hvRest")));
            Assert.IsFalse(ontology.Data.CheckIsIndividualOfClass(ontology.Model, new RDFResource("ex:indiv4"), new RDFResource("ex:hvRest")));
        }

        [TestMethod]
        public void ShouldCheckIsIndividualOfHasValueResourceRestrictionWithReasoning()
        {
            RDFOntology ontology = new RDFOntology("ex:ont");
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

            Assert.IsTrue(ontology.Data.CheckIsIndividualOfClass(ontology.Model, new RDFResource("ex:indiv1"), new RDFResource("ex:hvRest")));
            Assert.IsTrue(ontology.Data.CheckIsIndividualOfClass(ontology.Model, new RDFResource("ex:indiv2"), new RDFResource("ex:hvRest")));
            Assert.IsTrue(ontology.Data.CheckIsIndividualOfClass(ontology.Model, new RDFResource("ex:indiv3"), new RDFResource("ex:hvRest")));
            Assert.IsFalse(ontology.Data.CheckIsIndividualOfClass(ontology.Model, new RDFResource("ex:indiv4"), new RDFResource("ex:hvRest")));
        }

        [TestMethod]
        public void ShouldCheckIsIndividualOfHasValueLiteralRestriction()
        {
            RDFOntology ontology = new RDFOntology("ex:ont");
            ontology.Model.ClassModel.DeclareHasValueRestriction(new RDFResource("ex:hvRest"), new RDFResource("ex:dtProp"), new RDFTypedLiteral("25", RDFModelEnums.RDFDatatypes.XSD_POSITIVEINTEGER));
            ontology.Model.PropertyModel.DeclareDatatypeProperty(new RDFResource("ex:dtProp"));
            ontology.Data.DeclareDatatypeAssertion(new RDFResource("ex:indiv1"), new RDFResource("ex:dtProp"), new RDFTypedLiteral("25.0", RDFModelEnums.RDFDatatypes.XSD_FLOAT));
            ontology.Data.DeclareDatatypeAssertion(new RDFResource("ex:indiv1"), new RDFResource("ex:dtProp"), new RDFTypedLiteral("26", RDFModelEnums.RDFDatatypes.XSD_DECIMAL));
            ontology.Data.DeclareDatatypeAssertion(new RDFResource("ex:indiv2"), new RDFResource("ex:dtProp"), new RDFPlainLiteral("25"));
            ontology.Data.DeclareDatatypeAssertion(new RDFResource("ex:indiv3"), new RDFResource("ex:dtProp"), new RDFTypedLiteral("25", RDFModelEnums.RDFDatatypes.XSD_NONNEGATIVEINTEGER));
            ontology.Data.DeclareDatatypeAssertion(new RDFResource("ex:indiv3"), new RDFResource("ex:dtProp"), new RDFTypedLiteral("25", RDFModelEnums.RDFDatatypes.XSD_GDAY));

            Assert.IsTrue(ontology.Data.CheckIsIndividualOfClass(ontology.Model, new RDFResource("ex:indiv1"), new RDFResource("ex:hvRest")));
            Assert.IsFalse(ontology.Data.CheckIsIndividualOfClass(ontology.Model, new RDFResource("ex:indiv2"), new RDFResource("ex:hvRest")));
            Assert.IsTrue(ontology.Data.CheckIsIndividualOfClass(ontology.Model, new RDFResource("ex:indiv3"), new RDFResource("ex:hvRest")));
            Assert.IsFalse(ontology.Data.CheckIsIndividualOfClass(ontology.Model, new RDFResource("ex:indiv4"), new RDFResource("ex:hvRest")));
        }

        [TestMethod]
        public void ShouldCheckIsIndividualOfHasSelfTrueRestriction()
        {
            RDFOntology ontology = new RDFOntology("ex:ont");
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

            Assert.IsTrue(ontology.Data.CheckIsIndividualOfClass(ontology.Model, new RDFResource("ex:indiv1"), new RDFResource("ex:hsRest")));
            Assert.IsFalse(ontology.Data.CheckIsIndividualOfClass(ontology.Model, new RDFResource("ex:indiv2"), new RDFResource("ex:hsRest")));
            Assert.IsTrue(ontology.Data.CheckIsIndividualOfClass(ontology.Model, new RDFResource("ex:indiv3"), new RDFResource("ex:hsRest")));
            Assert.IsFalse(ontology.Data.CheckIsIndividualOfClass(ontology.Model, new RDFResource("ex:indiv4"), new RDFResource("ex:hsRest")));
        }

        [TestMethod]
        public void ShouldCheckIsIndividualOfHasSelfFalseRestriction()
        {
            RDFOntology ontology = new RDFOntology("ex:ont");
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

            Assert.IsFalse(ontology.Data.CheckIsIndividualOfClass(ontology.Model, new RDFResource("ex:indiv1"), new RDFResource("ex:hsRest")));
            Assert.IsTrue(ontology.Data.CheckIsIndividualOfClass(ontology.Model, new RDFResource("ex:indiv2"), new RDFResource("ex:hsRest")));
            Assert.IsFalse(ontology.Data.CheckIsIndividualOfClass(ontology.Model, new RDFResource("ex:indiv3"), new RDFResource("ex:hsRest")));
            Assert.IsFalse(ontology.Data.CheckIsIndividualOfClass(ontology.Model, new RDFResource("ex:indiv4"), new RDFResource("ex:hsRest")));
        }

        [TestMethod]
        public void ShouldCheckIsIndividualsOfCompositeUnion()
        {
            RDFOntology ontology = new RDFOntology("ex:ont");
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

            Assert.IsTrue(ontology.Data.CheckIsIndividualOfClass(ontology.Model, new RDFResource("ex:indiv1"), new RDFResource("ex:unionClass")));
            Assert.IsTrue(ontology.Data.CheckIsIndividualOfClass(ontology.Model, new RDFResource("ex:indiv2"), new RDFResource("ex:unionClass")));
            Assert.IsFalse(ontology.Data.CheckIsIndividualOfClass(ontology.Model, new RDFResource("ex:indiv3"), new RDFResource("ex:unionClass")));
        }

        [TestMethod]
        public void ShouldCheckIsIndividualsOfCompositeIntersection()
        {
            RDFOntology ontology = new RDFOntology("ex:ont");
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

            Assert.IsFalse(ontology.Data.CheckIsIndividualOfClass(ontology.Model, new RDFResource("ex:indiv1"), new RDFResource("ex:intersectionClass")));
            Assert.IsFalse(ontology.Data.CheckIsIndividualOfClass(ontology.Model, new RDFResource("ex:indiv2"), new RDFResource("ex:intersectionClass")));
            Assert.IsTrue(ontology.Data.CheckIsIndividualOfClass(ontology.Model, new RDFResource("ex:indiv3"), new RDFResource("ex:intersectionClass")));
        }

        [TestMethod]
        public void ShouldCheckIsIndividualsOfCompositeComplement()
        {
            RDFOntology ontology = new RDFOntology("ex:ont");
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

            Assert.IsFalse(ontology.Data.CheckIsIndividualOfClass(ontology.Model, new RDFResource("ex:indiv1"), new RDFResource("ex:complementClass")));
            Assert.IsTrue(ontology.Data.CheckIsIndividualOfClass(ontology.Model, new RDFResource("ex:indiv2"), new RDFResource("ex:complementClass")));
            Assert.IsFalse(ontology.Data.CheckIsIndividualOfClass(ontology.Model, new RDFResource("ex:indiv3"), new RDFResource("ex:complementClass")));
        }
        #endregion

        #region Checker
        [TestMethod]
        public void ShouldCheckSameAsCompatibility()
        {
            RDFOntologyData data = new RDFOntologyData();
            data.DeclareIndividual(new RDFResource("ex:indiv1"));
            data.DeclareIndividual(new RDFResource("ex:indiv2"));

            Assert.IsTrue(data.CheckSameAsCompatibility(new RDFResource("ex:indiv1"), new RDFResource("ex:indiv2")));
            Assert.IsTrue(data.CheckSameAsCompatibility(new RDFResource("ex:indiv2"), new RDFResource("ex:indiv1")));
        }

        [TestMethod]
        public void ShouldCheckSameAsIncompatibility()
        {
            RDFOntologyData data = new RDFOntologyData();
            data.DeclareIndividual(new RDFResource("ex:indiv1"));
            data.DeclareIndividual(new RDFResource("ex:indiv2"));
            data.DeclareDifferentIndividuals(new RDFResource("ex:indiv1"), new RDFResource("ex:indiv2"));

            Assert.IsFalse(data.CheckSameAsCompatibility(new RDFResource("ex:indiv1"), new RDFResource("ex:indiv2")));
            Assert.IsFalse(data.CheckSameAsCompatibility(new RDFResource("ex:indiv2"), new RDFResource("ex:indiv1")));
        }

        [TestMethod]
        public void ShouldCheckSameAsIncompatibilityWithReasoning()
        {
            RDFOntologyData data = new RDFOntologyData();
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
            RDFOntologyData data = new RDFOntologyData();
            data.DeclareIndividual(new RDFResource("ex:indiv1"));
            data.DeclareIndividual(new RDFResource("ex:indiv2"));

            Assert.IsTrue(data.CheckDifferentFromCompatibility(new RDFResource("ex:indiv1"), new RDFResource("ex:indiv2")));
            Assert.IsTrue(data.CheckDifferentFromCompatibility(new RDFResource("ex:indiv2"), new RDFResource("ex:indiv1")));
        }

        [TestMethod]
        public void ShouldCheckDifferentFromIncompatibility()
        {
            RDFOntologyData data = new RDFOntologyData();
            data.DeclareIndividual(new RDFResource("ex:indiv1"));
            data.DeclareIndividual(new RDFResource("ex:indiv2"));
            data.DeclareSameIndividuals(new RDFResource("ex:indiv1"), new RDFResource("ex:indiv2"));

            Assert.IsFalse(data.CheckDifferentFromCompatibility(new RDFResource("ex:indiv1"), new RDFResource("ex:indiv2")));
            Assert.IsFalse(data.CheckDifferentFromCompatibility(new RDFResource("ex:indiv2"), new RDFResource("ex:indiv1")));
        }

        [TestMethod]
        public void ShouldCheckDifferentFromIncompatibilityWithReasoning()
        {
            RDFOntologyData data = new RDFOntologyData();
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
            RDFOntologyData data = new RDFOntologyData();
            data.DeclareIndividual(new RDFResource("ex:indiv1"));
            data.DeclareIndividual(new RDFResource("ex:indiv2"));

            Assert.IsTrue(data.CheckObjectAssertionCompatibility(new RDFResource("ex:indiv1"), new RDFResource("ex:objProp"), new RDFResource("ex:indiv2")));
        }

        [TestMethod]
        public void ShouldCheckObjectAssertionIncompatibility()
        {
            RDFOntologyData data = new RDFOntologyData();
            data.DeclareIndividual(new RDFResource("ex:indiv1"));
            data.DeclareIndividual(new RDFResource("ex:indiv2"));
            data.DeclareNegativeObjectAssertion(new RDFResource("ex:indiv1"), new RDFResource("ex:objProp"), new RDFResource("ex:indiv2"));

            Assert.IsFalse(data.CheckObjectAssertionCompatibility(new RDFResource("ex:indiv1"), new RDFResource("ex:objProp"), new RDFResource("ex:indiv2")));
        }

        [TestMethod]
        public void ShouldCheckNegativeObjectAssertionCompatibility()
        {
            RDFOntologyData data = new RDFOntologyData();
            data.DeclareIndividual(new RDFResource("ex:indiv1"));
            data.DeclareIndividual(new RDFResource("ex:indiv2"));

            Assert.IsTrue(data.CheckNegativeObjectAssertionCompatibility(new RDFResource("ex:indiv1"), new RDFResource("ex:objProp"), new RDFResource("ex:indiv2")));
        }

        [TestMethod]
        public void ShouldCheckNegativeObjectAssertionIncompatibility()
        {
            RDFOntologyData data = new RDFOntologyData();
            data.DeclareIndividual(new RDFResource("ex:indiv1"));
            data.DeclareIndividual(new RDFResource("ex:indiv2"));
            data.DeclareObjectAssertion(new RDFResource("ex:indiv1"), new RDFResource("ex:objProp"), new RDFResource("ex:indiv2"));

            Assert.IsFalse(data.CheckNegativeObjectAssertionCompatibility(new RDFResource("ex:indiv1"), new RDFResource("ex:objProp"), new RDFResource("ex:indiv2")));
        }

        [TestMethod]
        public void ShouldCheckDatatypeAssertionCompatibility()
        {
            RDFOntologyData data = new RDFOntologyData();
            data.DeclareIndividual(new RDFResource("ex:indiv1"));

            Assert.IsTrue(data.CheckDatatypeAssertionCompatibility(new RDFResource("ex:indiv1"), new RDFResource("ex:dtProp"), new RDFPlainLiteral("hello")));
        }

        [TestMethod]
        public void ShouldCheckDatatypeAssertionIncompatibility()
        {
            RDFOntologyData data = new RDFOntologyData();
            data.DeclareIndividual(new RDFResource("ex:indiv1"));
            data.DeclareNegativeDatatypeAssertion(new RDFResource("ex:indiv1"), new RDFResource("ex:dtProp"), new RDFPlainLiteral("hello"));

            Assert.IsFalse(data.CheckDatatypeAssertionCompatibility(new RDFResource("ex:indiv1"), new RDFResource("ex:dtProp"), new RDFPlainLiteral("hello")));
        }

        [TestMethod]
        public void ShouldCheckNegativeDatatypeAssertionCompatibility()
        {
            RDFOntologyData data = new RDFOntologyData();
            data.DeclareIndividual(new RDFResource("ex:indiv1"));

            Assert.IsTrue(data.CheckNegativeDatatypeAssertionCompatibility(new RDFResource("ex:indiv1"), new RDFResource("ex:dtProp"), new RDFPlainLiteral("hello")));
        }

        [TestMethod]
        public void ShouldCheckNegativeDatatypeAssertionIncompatibility()
        {
            RDFOntologyData data = new RDFOntologyData();
            data.DeclareIndividual(new RDFResource("ex:indiv1"));
            data.DeclareDatatypeAssertion(new RDFResource("ex:indiv1"), new RDFResource("ex:dtProp"), new RDFPlainLiteral("hello"));

            Assert.IsFalse(data.CheckNegativeDatatypeAssertionCompatibility(new RDFResource("ex:indiv1"), new RDFResource("ex:dtProp"), new RDFPlainLiteral("hello")));
        }
        #endregion
    }
}