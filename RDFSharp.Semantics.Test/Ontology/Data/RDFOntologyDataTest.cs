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
    public class RDFOntologyDataTest
    {
        #region Test
        [TestMethod]
        public void ShouldCreateData()
        {
            RDFOntologyData data = new RDFOntologyData();

            Assert.IsNotNull(data);
            Assert.IsNotNull(data.Individuals);
            Assert.IsTrue(data.IndividualsCount == 0);
            Assert.IsTrue(data.AllDifferentCount == 0);
            Assert.IsNotNull(data.ABoxGraph);
            Assert.IsTrue(data.ABoxGraph.TriplesCount == 0);
            Assert.IsNotNull(data.ABoxInferenceGraph);
            Assert.IsTrue(data.ABoxInferenceGraph.TriplesCount == 0);
            Assert.IsNotNull(data.ABoxVirtualGraph);
            Assert.IsTrue(data.ABoxVirtualGraph.TriplesCount == 0);

            int i = 0;
            IEnumerator<RDFResource> individualsEnumerator = data.IndividualsEnumerator;
            while (individualsEnumerator.MoveNext()) 
                i++;
            Assert.IsTrue(i == 0);

            int j = 0;
            IEnumerator<RDFResource> allDifferentEnumerator = data.AllDifferentEnumerator;
            while (allDifferentEnumerator.MoveNext()) 
                j++;
            Assert.IsTrue(j == 0);
        }

        [TestMethod]
        public void ShouldDeclareIndividual()
        {
            RDFOntologyData data = new RDFOntologyData();
            data.DeclareIndividual(new RDFResource("ex:indivA"));
            data.DeclareIndividual(new RDFResource("ex:indivA")); //Will be discarded since duplicate individuals are not allowed

            Assert.IsTrue(data.IndividualsCount == 1);
            Assert.IsTrue(data.AllDifferentCount == 0);
            Assert.IsTrue(data.ABoxGraph.TriplesCount == 1);
            Assert.IsTrue(data.ABoxGraph.ContainsTriple(new RDFTriple(new RDFResource("ex:indivA"), RDFVocabulary.RDF.TYPE, RDFVocabulary.OWL.NAMED_INDIVIDUAL)));

            int i = 0;
            IEnumerator<RDFResource> individualsEnumerator = data.IndividualsEnumerator;
            while (individualsEnumerator.MoveNext())
                i++;
            Assert.IsTrue(i == 1);

            int j = 0;
            IEnumerator<RDFResource> allDifferentEnumerator = data.AllDifferentEnumerator;
            while (allDifferentEnumerator.MoveNext())
                j++;
            Assert.IsTrue(j == 0);
        }

        [TestMethod]
        public void ShouldThrowExceptionOnDeclaringIndividualBecauseNull()
            => Assert.ThrowsException<RDFSemanticsException>(() => new RDFOntologyData().DeclareIndividual(null));

        [TestMethod]
        public void ShouldAnnotateResourceIndividual()
        {
            RDFOntologyData data = new RDFOntologyData();
            data.DeclareIndividual(new RDFResource("ex:indivA"));
            data.AnnotateIndividual(new RDFResource("ex:indivA"), RDFVocabulary.RDFS.SEE_ALSO, new RDFResource("ex:seealso"));
            data.AnnotateIndividual(new RDFResource("ex:indivA"), RDFVocabulary.RDFS.SEE_ALSO, new RDFResource("ex:seealso")); //Will be discarded, since duplicate annotations are not allowed

            Assert.IsTrue(data.IndividualsCount == 1);
            Assert.IsTrue(data.AllDifferentCount == 0);
            Assert.IsTrue(data.ABoxGraph.TriplesCount == 2);
            Assert.IsTrue(data.ABoxGraph.ContainsTriple(new RDFTriple(new RDFResource("ex:indivA"), RDFVocabulary.RDF.TYPE, RDFVocabulary.OWL.NAMED_INDIVIDUAL)));
            Assert.IsTrue(data.ABoxGraph.ContainsTriple(new RDFTriple(new RDFResource("ex:indivA"), RDFVocabulary.RDFS.SEE_ALSO, new RDFResource("ex:seealso"))));
        }

        [TestMethod]
        public void ShouldAnnotateLiteralIndividual()
        {
            RDFOntologyData data = new RDFOntologyData();
            data.DeclareIndividual(new RDFResource("ex:indivA"));
            data.AnnotateIndividual(new RDFResource("ex:indivA"), RDFVocabulary.RDFS.LABEL, new RDFPlainLiteral("label"));
            data.AnnotateIndividual(new RDFResource("ex:indivA"), RDFVocabulary.RDFS.LABEL, new RDFPlainLiteral("label")); //Will be discarded, since duplicate annotations are not allowed

            Assert.IsTrue(data.IndividualsCount == 1);
            Assert.IsTrue(data.AllDifferentCount == 0);
            Assert.IsTrue(data.ABoxGraph.TriplesCount == 2);
            Assert.IsTrue(data.ABoxGraph.ContainsTriple(new RDFTriple(new RDFResource("ex:indivA"), RDFVocabulary.RDF.TYPE, RDFVocabulary.OWL.NAMED_INDIVIDUAL)));
            Assert.IsTrue(data.ABoxGraph.ContainsTriple(new RDFTriple(new RDFResource("ex:indivA"), RDFVocabulary.RDFS.LABEL, new RDFPlainLiteral("label"))));
        }

        [TestMethod]
        public void ShouldThrowExceptionOnAnnotatingResourceIndividualBecauseNullSubject()
            => Assert.ThrowsException<RDFSemanticsException>(() => new RDFOntologyData()
                        .DeclareIndividual(new RDFResource("ex:indiv1"))
                        .AnnotateIndividual(null, RDFVocabulary.RDFS.SEE_ALSO, new RDFResource("ex:seealso")));

        [TestMethod]
        public void ShouldThrowExceptionOnAnnotatingResourceIndividualBecauseNullPredicate()
            => Assert.ThrowsException<RDFSemanticsException>(() => new RDFOntologyData()
                        .DeclareIndividual(new RDFResource("ex:indiv1"))
                        .AnnotateIndividual(new RDFResource("ex:indiv1"), null, new RDFResource("ex:seealso")));

        [TestMethod]
        public void ShouldThrowExceptionOnAnnotatingResourceIndividualBecauseBlankPredicate()
           => Assert.ThrowsException<RDFSemanticsException>(() => new RDFOntologyData()
                       .DeclareIndividual(new RDFResource("ex:indiv1"))
                       .AnnotateIndividual(new RDFResource("ex:indiv1"), new RDFResource(), new RDFResource("ex:seealso")));

        [TestMethod]
        public void ShouldThrowExceptionOnAnnotatingResourceIndividualBecauseNullObject()
            => Assert.ThrowsException<RDFSemanticsException>(() => new RDFOntologyData()
                        .DeclareIndividual(new RDFResource("ex:indiv1"))
                        .AnnotateIndividual(new RDFResource("ex:indiv1"), RDFVocabulary.RDFS.SEE_ALSO, null as RDFResource));

        [TestMethod]
        public void ShouldThrowExceptionOnAnnotatingLiteralIndividualBecauseNullSubject()
            => Assert.ThrowsException<RDFSemanticsException>(() => new RDFOntologyData()
                        .DeclareIndividual(new RDFResource("ex:indiv1"))
                        .AnnotateIndividual(null, RDFVocabulary.RDFS.LABEL, new RDFPlainLiteral("label")));

        [TestMethod]
        public void ShouldThrowExceptionOnAnnotatingLiteralIndividualBecauseNullPredicate()
            => Assert.ThrowsException<RDFSemanticsException>(() => new RDFOntologyData()
                        .DeclareIndividual(new RDFResource("ex:indiv1"))
                        .AnnotateIndividual(new RDFResource("ex:indiv1"), null, new RDFPlainLiteral("label")));

        [TestMethod]
        public void ShouldThrowExceptionOnAnnotatingLiteralIndividualBecauseBlankPredicate()
           => Assert.ThrowsException<RDFSemanticsException>(() => new RDFOntologyData()
                       .DeclareIndividual(new RDFResource("ex:indiv1"))
                       .AnnotateIndividual(new RDFResource("ex:indiv1"), new RDFResource(), new RDFPlainLiteral("label")));

        [TestMethod]
        public void ShouldThrowExceptionOnAnnotatingLiteralIndividualBecauseNullObject()
            => Assert.ThrowsException<RDFSemanticsException>(() => new RDFOntologyData()
                        .DeclareIndividual(new RDFResource("ex:indiv1"))
                        .AnnotateIndividual(new RDFResource("ex:indiv1"), RDFVocabulary.RDFS.LABEL, null as RDFLiteral));
        #endregion
    }
}