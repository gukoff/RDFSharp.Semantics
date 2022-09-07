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
using System.Threading.Tasks;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using RDFSharp.Model;

namespace RDFSharp.Semantics.Test
{
    [TestClass]
    public class RDFOntologyTest
    {
        #region Test
        [TestMethod]
        public void ShouldCreateOntology()
        {
            RDFOntology ontology = new RDFOntology("ex:ont");

            Assert.IsNotNull(ontology);
            Assert.IsNotNull(ontology.Model);
            Assert.IsNotNull(ontology.Data);
            Assert.IsNotNull(ontology.OBoxGraph);
            Assert.IsTrue(ontology.OBoxGraph.Context.Equals(new Uri("ex:ont")));
        }

        [TestMethod]
        public void ShouldCreateOntologyFromModelAndData()
        {
            RDFOntology ontology = new RDFOntology("ex:ont", new RDFOntologyModel(), new RDFOntologyData());

            Assert.IsNotNull(ontology);
            Assert.IsNotNull(ontology.Model);
            Assert.IsNotNull(ontology.Data);
            Assert.IsNotNull(ontology.OBoxGraph);
            Assert.IsTrue(ontology.OBoxGraph.Context.Equals(new Uri("ex:ont")));
        }

        [TestMethod]
        public void ShouldAnnotateOntologyWithResource()
        {
            RDFOntology ontology = new RDFOntology("ex:ont");
            ontology.Annotate(RDFVocabulary.RDFS.SEE_ALSO, new RDFResource("ex:seealso"));

            Assert.IsTrue(ontology.OBoxGraph.TriplesCount == 1);
        }

        [TestMethod]
        public void ShouldAnnotateOntologyWithLiteral()
        {
            RDFOntology ontology = new RDFOntology("ex:ont");
            ontology.Annotate(RDFVocabulary.RDFS.COMMENT, new RDFPlainLiteral("This is a test ontology"));

            Assert.IsTrue(ontology.OBoxGraph.TriplesCount == 1);
        }

        [TestMethod]
        public void ShouldThrowExceptionOnResourceAnnotatingOntologyBecauseNullProperty()
            => Assert.ThrowsException<RDFSemanticsException>(() => new RDFOntology("ex:ont").Annotate(null, new RDFResource("ex:org")));

        [TestMethod]
        public void ShouldThrowExceptionOnResourceAnnotatingOntologyBecauseBlankProperty()
            => Assert.ThrowsException<RDFSemanticsException>(() => new RDFOntology("ex:ont").Annotate(new RDFResource(), new RDFResource("ex:org")));

        [TestMethod]
        public void ShouldThrowExceptionOnResourceAnnotatingOntologyBecauseNullValue()
            => Assert.ThrowsException<RDFSemanticsException>(() => new RDFOntology("ex:ont").Annotate(RDFVocabulary.RDFS.LABEL, null as RDFResource));

        [TestMethod]
        public void ShouldThrowExceptionOnLiteralAnnotatingOntologyBecauseNullProperty()
            => Assert.ThrowsException<RDFSemanticsException>(() => new RDFOntology("ex:ont").Annotate(null, new RDFPlainLiteral("This is a test ontology")));

        [TestMethod]
        public void ShouldThrowExceptionOnLiteralAnnotatingOntologyBecauseBlankProperty()
            => Assert.ThrowsException<RDFSemanticsException>(() => new RDFOntology("ex:ont").Annotate(new RDFResource(), new RDFPlainLiteral("This is a test ontology")));

        [TestMethod]
        public void ShouldThrowExceptionOnLiteralAnnotatingOntologyBecauseNullValue()
            => Assert.ThrowsException<RDFSemanticsException>(() => new RDFOntology("ex:ont").Annotate(RDFVocabulary.RDFS.LABEL, null as RDFLiteral));
        #endregion
    }
}