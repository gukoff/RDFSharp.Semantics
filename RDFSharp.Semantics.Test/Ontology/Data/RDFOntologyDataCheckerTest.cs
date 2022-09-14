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
    public class RDFOntologyDataCheckerTest
    {
        #region Test
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