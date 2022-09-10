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
using System.Threading.Tasks;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using RDFSharp.Model;

namespace RDFSharp.Semantics.Test
{
    [TestClass]
    public class RDFOntologyDataAnalyzerTest
    {
        #region Test
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
            Assert.IsTrue(data.CheckAreDifferentIndividuals(new RDFResource("ex:indivB"), new RDFResource("ex:indivD"))); //Inferred
            Assert.IsTrue(data.CheckAreDifferentIndividuals(new RDFResource("ex:indivD"), new RDFResource("ex:indivB"))); //Inferred
            Assert.IsTrue(data.CheckAreDifferentIndividuals(new RDFResource("ex:indivA"), new RDFResource("ex:indivD"))); //Inferred
            Assert.IsTrue(data.CheckAreDifferentIndividuals(new RDFResource("ex:indivD"), new RDFResource("ex:indivA"))); //Inferred
            Assert.IsTrue(data.CheckAreDifferentIndividuals(new RDFResource("ex:indivA"), new RDFResource("ex:indivE"))); //Inferred
            Assert.IsTrue(data.CheckAreDifferentIndividuals(new RDFResource("ex:indivE"), new RDFResource("ex:indivA"))); //Inferred
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
        #endregion
    }
}