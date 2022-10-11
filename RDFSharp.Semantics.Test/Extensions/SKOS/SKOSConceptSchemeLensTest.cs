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

using RDFSharp.Model;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Linq;

namespace RDFSharp.Semantics.Extensions.SKOS.Test
{
    [TestClass]
    public class SKOSConceptSchemeLensTest
    {
        #region Initialize
        private SKOSConceptScheme ConceptScheme { get; set; }
        private SKOSConceptSchemeLens ConceptSchemeLens { get; set; }

        [TestInitialize]
        public void Initialize()
        {
            ConceptScheme = new SKOSConceptScheme("ex:conceptScheme")
                .Annotate(RDFVocabulary.RDFS.COMMENT, new RDFPlainLiteral("This is a test concept scheme"))
                .DeclareConcept(new RDFResource("ex:concept1"))
                .AnnotateConcept(new RDFResource("ex:concept1"), RDFVocabulary.RDFS.COMMENT, new RDFPlainLiteral("This is a test concept"))
                .DeclarePreferredLabel(new RDFResource("ex:concept1"), new RDFResource("ex:label1"), new RDFPlainLiteral("concept1", "en-US"))
                .DeclareConcept(new RDFResource("ex:concept2"))
                .DeclareConceptDefinition(new RDFResource("ex:concept2"), new RDFTypedLiteral("this is concept 2", RDFModelEnums.RDFDatatypes.RDFS_LITERAL))
                .DeclareNarrowMatchConcepts(new RDFResource("ex:concept1"), new RDFResource("ex:concept2"))
                .DeclareCollection(new RDFResource("ex:collection1"), new List<RDFResource>() { new RDFResource("ex:concept1"), new RDFResource("ex:collection2") })
                .AnnotateCollection(new RDFResource("ex:collection1"), RDFVocabulary.RDFS.COMMENT, new RDFPlainLiteral("This is a test collection"))
                .DeclareCollection(new RDFResource("ex:collection2"), new List<RDFResource>() { new RDFResource("ex:concept2"), new RDFResource("ex:collection1") })
                .DeclareConcept(new RDFResource("ex:concept3"))
                .DeclareBroaderConcepts(new RDFResource("ex:concept1"), new RDFResource("ex:concept3"))
                .DeclareConcept(new RDFResource("ex:concept4"))
                .DeclareConcept(new RDFResource("ex:concept5"))
                .DeclareBroaderTransitiveConcepts(new RDFResource("ex:concept1"), new RDFResource("ex:concept4"))
                .DeclareBroaderTransitiveConcepts(new RDFResource("ex:concept4"), new RDFResource("ex:concept5"))
                .DeclareConcept(new RDFResource("ex:concept6"))
                .DeclareNarrowerConcepts(new RDFResource("ex:concept1"), new RDFResource("ex:concept6"))
                .DeclareConcept(new RDFResource("ex:concept7"))
                .DeclareConcept(new RDFResource("ex:concept8"))
                .DeclareNarrowerTransitiveConcepts(new RDFResource("ex:concept1"), new RDFResource("ex:concept7"))
                .DeclareNarrowerTransitiveConcepts(new RDFResource("ex:concept7"), new RDFResource("ex:concept8"))
                .DeclareConcept(new RDFResource("ex:concept9"))
                .DeclareBroadMatchConcepts(new RDFResource("ex:concept1"), new RDFResource("ex:concept9"))
                .DeclareConcept(new RDFResource("ex:concept10"))
                .DeclareCloseMatchConcepts(new RDFResource("ex:concept1"), new RDFResource("ex:concept10"))
                .DeclareConcept(new RDFResource("ex:concept11"))
                .DeclareConcept(new RDFResource("ex:concept12"))
                .DeclareExactMatchConcepts(new RDFResource("ex:concept1"), new RDFResource("ex:concept11"))
                .DeclareExactMatchConcepts(new RDFResource("ex:concept11"), new RDFResource("ex:concept12"))
                .DeclareTopConcept(new RDFResource("ex:concept1"));

            ConceptSchemeLens = new SKOSConceptSchemeLens(new RDFResource("ex:concept1"), ConceptScheme);
        }
        #endregion

        #region Tests
        [TestMethod]
        public void ShouldBeTopConcept()
            => Assert.IsTrue(ConceptSchemeLens.IsTopConcept());

        [TestMethod]
        public async Task ShouldBeTopConceptAsync()
            => Assert.IsTrue(await ConceptSchemeLens.IsTopConceptAsync());

        [TestMethod]
        public void ShouldGetBroaderConcepts()
            => Assert.IsTrue(ConceptSchemeLens.BroaderConcepts().Count == 3);

        [TestMethod]
        public async Task ShouldGetBroaderConceptsAsync()
            => Assert.IsTrue((await ConceptSchemeLens.BroaderConceptsAsync()).Count == 3);

        [TestMethod]
        public void ShouldGetNarrowerConcepts()
            => Assert.IsTrue(ConceptSchemeLens.NarrowerConcepts().Count == 3);

        [TestMethod]
        public async Task ShouldGetNarrowerConceptsAsync()
            => Assert.IsTrue((await ConceptSchemeLens.NarrowerConceptsAsync()).Count == 3);

        [TestMethod]
        public void ShouldGetBroadMatchConcepts()
           => Assert.IsTrue(ConceptSchemeLens.BroadMatchConcepts().Count == 1);

        [TestMethod]
        public async Task ShouldGetBroadMatchConceptsAsync()
            => Assert.IsTrue((await ConceptSchemeLens.BroadMatchConceptsAsync()).Count == 1);

        [TestMethod]
        public void ShouldGetNarrowMatchConcepts()
            => Assert.IsTrue(ConceptSchemeLens.NarrowMatchConcepts().Count == 1);

        [TestMethod]
        public async Task ShouldGetNarrowMatchConceptsAsync()
            => Assert.IsTrue((await ConceptSchemeLens.NarrowMatchConceptsAsync()).Count == 1);

        [TestMethod]
        public void ShouldGetCloseMatchConcepts()
          => Assert.IsTrue(ConceptSchemeLens.CloseMatchConcepts().Count == 1);

        [TestMethod]
        public async Task ShouldGetCloseMatchConceptsAsync()
            => Assert.IsTrue((await ConceptSchemeLens.CloseMatchConceptsAsync()).Count == 1);

        [TestMethod]
        public void ShouldGetExactMatchConcepts()
            => Assert.IsTrue(ConceptSchemeLens.ExactMatchConcepts().Count == 2);

        [TestMethod]
        public async Task ShouldGetExactMatchConceptsAsync()
            => Assert.IsTrue((await ConceptSchemeLens.ExactMatchConceptsAsync()).Count == 2);
        #endregion
    }
}