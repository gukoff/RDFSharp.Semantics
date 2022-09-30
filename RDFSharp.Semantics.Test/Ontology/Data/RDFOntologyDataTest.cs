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

        [TestMethod]
        public void ShouldDeclareIndividualType()
        {
            RDFOntologyData data = new RDFOntologyData();
            data.DeclareIndividual(new RDFResource("ex:indivA"));
            data.DeclareIndividualType(new RDFResource("ex:indivA"), new RDFResource("ex:classA"));

            Assert.IsTrue(data.ABoxGraph.TriplesCount == 2);
            Assert.IsTrue(data.ABoxGraph.ContainsTriple(new RDFTriple(new RDFResource("ex:indivA"), RDFVocabulary.RDF.TYPE, RDFVocabulary.OWL.NAMED_INDIVIDUAL)));
            Assert.IsTrue(data.ABoxGraph.ContainsTriple(new RDFTriple(new RDFResource("ex:indivA"), RDFVocabulary.RDF.TYPE, new RDFResource("ex:classA"))));
        }

        [TestMethod]
        public void ShouldEmitWarningOnDeclaringIndividualTypeBecauseReservedClass()
        {
            string warningMsg = null;
            RDFSemanticsEvents.OnSemanticsWarning += (string msg) => { warningMsg = msg; };

            RDFOntologyData data = new RDFOntologyData();
            data.DeclareIndividual(new RDFResource("ex:indivA"));
            data.DeclareIndividualType(new RDFResource("ex:indivA"), RDFVocabulary.RDFS.RESOURCE);

            Assert.IsNotNull(warningMsg);
            Assert.IsTrue(warningMsg.IndexOf("Type relation between individual 'ex:indivA' and class 'http://www.w3.org/2000/01/rdf-schema#Resource' cannot be declared to the data because it would violate OWL-DL integrity") > -1);
            Assert.IsTrue(data.ABoxGraph.TriplesCount == 1);
            Assert.IsTrue(data.ABoxGraph.ContainsTriple(new RDFTriple(new RDFResource("ex:indivA"), RDFVocabulary.RDF.TYPE, RDFVocabulary.OWL.NAMED_INDIVIDUAL)));
        }

        [TestMethod]
        public void ShouldThrowExceptionOnDeclaringIndividualTypeBecauseNullIndividual()
            => Assert.ThrowsException<RDFSemanticsException>(() => new RDFOntologyData()
                        .DeclareIndividual(new RDFResource("ex:indivA"))
                        .DeclareIndividualType(null, new RDFResource("ex:classA")));

        [TestMethod]
        public void ShouldThrowExceptionOnDeclaringIndividualTypeBecauseNullClass()
            => Assert.ThrowsException<RDFSemanticsException>(() => new RDFOntologyData()
                        .DeclareIndividual(new RDFResource("ex:indivA"))
                        .DeclareIndividualType(new RDFResource("ex:indivA"), null));

        [TestMethod]
        public void ShouldDeclareSameIndividuals()
        {
            RDFOntologyData data = new RDFOntologyData();
            data.DeclareIndividual(new RDFResource("ex:indivA"));
            data.DeclareIndividual(new RDFResource("ex:indivB"));
            data.DeclareSameIndividuals(new RDFResource("ex:indivA"), new RDFResource("ex:indivB"));

            Assert.IsTrue(data.ABoxGraph.TriplesCount == 4);
            Assert.IsTrue(data.ABoxGraph.ContainsTriple(new RDFTriple(new RDFResource("ex:indivA"), RDFVocabulary.RDF.TYPE, RDFVocabulary.OWL.NAMED_INDIVIDUAL)));
            Assert.IsTrue(data.ABoxGraph.ContainsTriple(new RDFTriple(new RDFResource("ex:indivB"), RDFVocabulary.RDF.TYPE, RDFVocabulary.OWL.NAMED_INDIVIDUAL)));
            Assert.IsTrue(data.ABoxGraph.ContainsTriple(new RDFTriple(new RDFResource("ex:indivA"), RDFVocabulary.OWL.SAME_AS, new RDFResource("ex:indivB"))));
            Assert.IsTrue(data.ABoxGraph.ContainsTriple(new RDFTriple(new RDFResource("ex:indivB"), RDFVocabulary.OWL.SAME_AS, new RDFResource("ex:indivA"))));
        }

        [TestMethod]
        public void ShouldEmitWarningOnDeclaringSameIndividualsBecauseIncompatibleIndividuals()
        {
            string warningMsg = null;
            RDFSemanticsEvents.OnSemanticsWarning += (string msg) => { warningMsg = msg; };

            RDFOntologyData data = new RDFOntologyData();
            data.DeclareIndividual(new RDFResource("ex:indivA"));
            data.DeclareIndividual(new RDFResource("ex:indivB"));
            data.DeclareDifferentIndividuals(new RDFResource("ex:indivA"), new RDFResource("ex:indivB"));
            data.DeclareSameIndividuals(new RDFResource("ex:indivA"), new RDFResource("ex:indivB"));  //OWL-DL contraddiction (enforced by policy)

            Assert.IsNotNull(warningMsg);
            Assert.IsTrue(warningMsg.IndexOf("SameAs relation between individual 'ex:indivA' and individual 'ex:indivB' cannot be declared to the data because it would violate OWL-DL integrity") > -1);
            Assert.IsTrue(data.ABoxGraph.ContainsTriple(new RDFTriple(new RDFResource("ex:indivA"), RDFVocabulary.RDF.TYPE, RDFVocabulary.OWL.NAMED_INDIVIDUAL)));
            Assert.IsTrue(data.ABoxGraph.ContainsTriple(new RDFTriple(new RDFResource("ex:indivB"), RDFVocabulary.RDF.TYPE, RDFVocabulary.OWL.NAMED_INDIVIDUAL)));
            Assert.IsTrue(data.ABoxGraph.ContainsTriple(new RDFTriple(new RDFResource("ex:indivA"), RDFVocabulary.OWL.DIFFERENT_FROM, new RDFResource("ex:indivB"))));
        }

        [TestMethod]
        public void ShouldThrowExceptionOnDeclaringSameIndividualsBecauseNullLeftIndividual()
            => Assert.ThrowsException<RDFSemanticsException>(() => new RDFOntologyData()
                        .DeclareIndividual(new RDFResource("ex:indivA"))
                        .DeclareIndividual(new RDFResource("ex:indivB"))
                        .DeclareSameIndividuals(null, new RDFResource("ex:indivB")));

        [TestMethod]
        public void ShouldThrowExceptionOnDeclaringSameIndividualsBecauseNullRightIndividual()
            => Assert.ThrowsException<RDFSemanticsException>(() => new RDFOntologyData()
                        .DeclareIndividual(new RDFResource("ex:indivA"))
                        .DeclareIndividual(new RDFResource("ex:indivB"))
                        .DeclareSameIndividuals(new RDFResource("ex:indivA"), null));
						
		[TestMethod]
        public void ShouldThrowExceptionOnDeclaringSameIndividualsBecauseSelfIndividual()
            => Assert.ThrowsException<RDFSemanticsException>(() => new RDFOntologyData()
                        .DeclareIndividual(new RDFResource("ex:indivA"))
                        .DeclareSameIndividuals(new RDFResource("ex:indivA"), new RDFResource("ex:indivA")));

        [TestMethod]
        public void ShouldDeclareDifferentIndividuals()
        {
            RDFOntologyData data = new RDFOntologyData();
            data.DeclareIndividual(new RDFResource("ex:indivA"));
            data.DeclareIndividual(new RDFResource("ex:indivB"));
            data.DeclareDifferentIndividuals(new RDFResource("ex:indivA"), new RDFResource("ex:indivB"));

            Assert.IsTrue(data.ABoxGraph.TriplesCount == 4);
            Assert.IsTrue(data.ABoxGraph.ContainsTriple(new RDFTriple(new RDFResource("ex:indivA"), RDFVocabulary.RDF.TYPE, RDFVocabulary.OWL.NAMED_INDIVIDUAL)));
            Assert.IsTrue(data.ABoxGraph.ContainsTriple(new RDFTriple(new RDFResource("ex:indivB"), RDFVocabulary.RDF.TYPE, RDFVocabulary.OWL.NAMED_INDIVIDUAL)));
            Assert.IsTrue(data.ABoxGraph.ContainsTriple(new RDFTriple(new RDFResource("ex:indivA"), RDFVocabulary.OWL.DIFFERENT_FROM, new RDFResource("ex:indivB"))));
            Assert.IsTrue(data.ABoxGraph.ContainsTriple(new RDFTriple(new RDFResource("ex:indivB"), RDFVocabulary.OWL.DIFFERENT_FROM, new RDFResource("ex:indivA"))));
        }

        [TestMethod]
        public void ShouldEmitWarningOnDeclaringDifferentIndividualsBecauseIncompatibleIndividuals()
        {
            string warningMsg = null;
            RDFSemanticsEvents.OnSemanticsWarning += (string msg) => { warningMsg = msg; };

            RDFOntologyData data = new RDFOntologyData();
            data.DeclareIndividual(new RDFResource("ex:indivA"));
            data.DeclareIndividual(new RDFResource("ex:indivB"));
            data.DeclareSameIndividuals(new RDFResource("ex:indivA"), new RDFResource("ex:indivB"));
            data.DeclareDifferentIndividuals(new RDFResource("ex:indivA"), new RDFResource("ex:indivB"));  //OWL-DL contraddiction (enforced by policy)

            Assert.IsNotNull(warningMsg);
            Assert.IsTrue(warningMsg.IndexOf("DifferentFrom relation between individual 'ex:indivA' and individual 'ex:indivB' cannot be declared to the data because it would violate OWL-DL integrity") > -1);
            Assert.IsTrue(data.ABoxGraph.ContainsTriple(new RDFTriple(new RDFResource("ex:indivA"), RDFVocabulary.RDF.TYPE, RDFVocabulary.OWL.NAMED_INDIVIDUAL)));
            Assert.IsTrue(data.ABoxGraph.ContainsTriple(new RDFTriple(new RDFResource("ex:indivB"), RDFVocabulary.RDF.TYPE, RDFVocabulary.OWL.NAMED_INDIVIDUAL)));
            Assert.IsTrue(data.ABoxGraph.ContainsTriple(new RDFTriple(new RDFResource("ex:indivA"), RDFVocabulary.OWL.SAME_AS, new RDFResource("ex:indivB"))));
        }

        [TestMethod]
        public void ShouldThrowExceptionOnDeclaringDifferentIndividualsBecauseNullLeftIndividual()
            => Assert.ThrowsException<RDFSemanticsException>(() => new RDFOntologyData()
                        .DeclareIndividual(new RDFResource("ex:indivA"))
                        .DeclareIndividual(new RDFResource("ex:indivB"))
                        .DeclareDifferentIndividuals(null, new RDFResource("ex:indivB")));

        [TestMethod]
        public void ShouldThrowExceptionOnDeclaringDifferentIndividualsBecauseNullRightIndividual()
            => Assert.ThrowsException<RDFSemanticsException>(() => new RDFOntologyData()
                        .DeclareIndividual(new RDFResource("ex:indivA"))
                        .DeclareIndividual(new RDFResource("ex:indivB"))
                        .DeclareDifferentIndividuals(new RDFResource("ex:indivA"), null));

        [TestMethod]
        public void ShouldThrowExceptionOnDeclaringDifferentIndividualsBecauseSelfIndividual()
            => Assert.ThrowsException<RDFSemanticsException>(() => new RDFOntologyData()
                        .DeclareIndividual(new RDFResource("ex:indivA"))
                        .DeclareDifferentIndividuals(new RDFResource("ex:indivA"), new RDFResource("ex:indivA")));

        [TestMethod]
        public void ShouldDeclareAllDifferentIndividuals()
        {
            RDFOntologyData data = new RDFOntologyData();
            data.DeclareAllDifferentIndividuals(new RDFResource("ex:allDiff"), new List<RDFResource>() {
                new RDFResource("ex:indivA"), new RDFResource("ex:indivB"), new RDFResource("ex:indivC") });

            Assert.IsTrue(data.AllDifferentCount == 1);
            Assert.IsTrue(data.ABoxGraph.TriplesCount == 11);
            Assert.IsTrue(data.ABoxGraph[new RDFResource("ex:allDiff"), RDFVocabulary.RDF.TYPE, RDFVocabulary.OWL.ALL_DIFFERENT, null].Any());
            Assert.IsTrue(data.ABoxGraph[new RDFResource("ex:allDiff"), RDFVocabulary.OWL.DISTINCT_MEMBERS, null, null].Any());
            Assert.IsTrue(data.ABoxGraph[null, RDFVocabulary.RDF.TYPE, RDFVocabulary.RDF.LIST, null].TriplesCount == 3);
            Assert.IsTrue(data.ABoxGraph[null, RDFVocabulary.RDF.FIRST, new RDFResource("ex:indivA"), null].Any());
            Assert.IsTrue(data.ABoxGraph[null, RDFVocabulary.RDF.FIRST, new RDFResource("ex:indivB"), null].Any());
            Assert.IsTrue(data.ABoxGraph[null, RDFVocabulary.RDF.FIRST, new RDFResource("ex:indivC"), null].Any());
            Assert.IsTrue(data.ABoxGraph[null, RDFVocabulary.RDF.REST, null, null].TriplesCount == 3);

            int j = 0;
            IEnumerator<RDFResource> allDifferentEnumerator = data.AllDifferentEnumerator;
            while (allDifferentEnumerator.MoveNext())
                j++;
            Assert.IsTrue(j == 1);
        }

        [TestMethod]
        public void ShouldThrowExceptionOnDeclaringAllDifferentIndividualsBecauseNullClass()
            => Assert.ThrowsException<RDFSemanticsException>(() => new RDFOntologyData()
                        .DeclareIndividual(new RDFResource("ex:indivA"))
                        .DeclareAllDifferentIndividuals(null, new List<RDFResource>() { new RDFResource("ex:indivA") }));

        [TestMethod]
        public void ShouldThrowExceptionOnDeclaringAllDifferentIndividualsBecauseNullIndividuals()
            => Assert.ThrowsException<RDFSemanticsException>(() => new RDFOntologyData()
                        .DeclareIndividual(new RDFResource("ex:indivA"))
                        .DeclareAllDifferentIndividuals(new RDFResource("ex:diffClass"), null));

        [TestMethod]
        public void ShouldThrowExceptionOnDeclaringAllDifferentIndividualsBecauseEmptyIndividuals()
            => Assert.ThrowsException<RDFSemanticsException>(() => new RDFOntologyData()
                        .DeclareIndividual(new RDFResource("ex:indivA"))
                        .DeclareAllDifferentIndividuals(new RDFResource("ex:diffClass"), new List<RDFResource>()));

        [TestMethod]
        public void ShouldDeclareObjectAssertion()
        {
            RDFOntologyData data = new RDFOntologyData();
            data.DeclareObjectAssertion(new RDFResource("ex:indivA"), RDFVocabulary.FOAF.KNOWS, new RDFResource("ex:indivB"));

            Assert.IsTrue(data.ABoxGraph.TriplesCount == 1);
            Assert.IsTrue(data.ABoxGraph[new RDFResource("ex:indivA"), RDFVocabulary.FOAF.KNOWS, new RDFResource("ex:indivB"), null].Any());
        }

        [TestMethod]
        public void ShouldThrowExceptionOnDeclaringObjectAssertionBecauseNullLeftIndividual()
            => Assert.ThrowsException<RDFSemanticsException>(() => new RDFOntologyData()
                        .DeclareObjectAssertion(null, RDFVocabulary.FOAF.KNOWS, new RDFResource("ex:indivB")));

        [TestMethod]
        public void ShouldThrowExceptionOnDeclaringObjectAssertionBecauseNullPredicate()
            => Assert.ThrowsException<RDFSemanticsException>(() => new RDFOntologyData()
                        .DeclareObjectAssertion(new RDFResource("ex:indivB"), null, new RDFResource("ex:indivB")));

        [TestMethod]
        public void ShouldThrowExceptionOnDeclaringObjectAssertionBecauseBlankPredicate()
            => Assert.ThrowsException<RDFSemanticsException>(() => new RDFOntologyData()
                        .DeclareObjectAssertion(new RDFResource("ex:indivB"), new RDFResource(), new RDFResource("ex:indivB")));

        [TestMethod]
        public void ShouldThrowExceptionOnDeclaringObjectAssertionBecauseNullRightIndividual()
            => Assert.ThrowsException<RDFSemanticsException>(() => new RDFOntologyData()
                        .DeclareObjectAssertion(new RDFResource("ex:indivA"), RDFVocabulary.FOAF.KNOWS, null));

        [TestMethod]
        public void ShouldEmitWarningOnDeclaringObjectAssertionBecauseReservedProperty()
        {
            string warningMsg = null;
            RDFSemanticsEvents.OnSemanticsWarning += (string msg) => { warningMsg = msg; };

            RDFOntologyData data = new RDFOntologyData();
            data.DeclareObjectAssertion(new RDFResource("ex:indivA"), RDFVocabulary.RDFS.SEE_ALSO, new RDFResource("ex:indivB")); //Reserved annotation property (not allowed by policy)

            Assert.IsNotNull(warningMsg);
            Assert.IsTrue(warningMsg.IndexOf("ObjectAssertion relation between individual 'ex:indivA' and individual 'ex:indivB' through property 'http://www.w3.org/2000/01/rdf-schema#seeAlso' cannot be declared to the data because it would violate OWL-DL integrity") > -1);
            Assert.IsTrue(data.ABoxGraph.TriplesCount == 0);
        }

        [TestMethod]
        public void ShouldEmitWarningOnDeclaringObjectAssertionBecauseIncompatibleObjectAssertion()
        {
            string warningMsg = null;
            RDFSemanticsEvents.OnSemanticsWarning += (string msg) => { warningMsg = msg; };

            RDFOntologyData data = new RDFOntologyData();
            data.DeclareNegativeObjectAssertion(new RDFResource("ex:indivA"), new RDFResource("ex:objProp"), new RDFResource("ex:indivB"));
            data.DeclareObjectAssertion(new RDFResource("ex:indivA"), new RDFResource("ex:objProp"), new RDFResource("ex:indivB")); //OWL-DL contraddiction (not allowed by policy)

            Assert.IsNotNull(warningMsg);
            Assert.IsTrue(warningMsg.IndexOf("ObjectAssertion relation between individual 'ex:indivA' and individual 'ex:indivB' through property 'ex:objProp' cannot be declared to the data because it would violate OWL-DL integrity") > -1);
            Assert.IsTrue(data.ABoxGraph.TriplesCount == 4);
            Assert.IsTrue(data.ABoxGraph[null, RDFVocabulary.RDF.TYPE, RDFVocabulary.OWL.NEGATIVE_PROPERTY_ASSERTION, null].Any());
            Assert.IsTrue(data.ABoxGraph[null, RDFVocabulary.OWL.SOURCE_INDIVIDUAL, new RDFResource("ex:indivA"), null].Any());
            Assert.IsTrue(data.ABoxGraph[null, RDFVocabulary.OWL.ASSERTION_PROPERTY, new RDFResource("ex:objProp"), null].Any());
            Assert.IsTrue(data.ABoxGraph[null, RDFVocabulary.OWL.TARGET_INDIVIDUAL, new RDFResource("ex:indivB"), null].Any());
        }

        [TestMethod]
        public void ShouldDeclareDatatypeAssertion()
        {
            RDFOntologyData data = new RDFOntologyData();
            data.DeclareDatatypeAssertion(new RDFResource("ex:indivA"), RDFVocabulary.FOAF.NAME, new RDFPlainLiteral("name"));

            Assert.IsTrue(data.ABoxGraph.TriplesCount == 1);
            Assert.IsTrue(data.ABoxGraph[new RDFResource("ex:indivA"), RDFVocabulary.FOAF.NAME, null, new RDFPlainLiteral("name")].Any());
        }

        [TestMethod]
        public void ShouldThrowExceptionOnDeclaringDatatypeAssertionBecauseNullIndividual()
            => Assert.ThrowsException<RDFSemanticsException>(() => new RDFOntologyData()
                        .DeclareDatatypeAssertion(null, RDFVocabulary.FOAF.NAME, new RDFPlainLiteral("name")));

        [TestMethod]
        public void ShouldThrowExceptionOnDeclaringDatatypeAssertionBecauseNullPredicate()
            => Assert.ThrowsException<RDFSemanticsException>(() => new RDFOntologyData()
                        .DeclareDatatypeAssertion(new RDFResource("ex:indivA"), null, new RDFPlainLiteral("name")));

        [TestMethod]
        public void ShouldThrowExceptionOnDeclaringDatatypeAssertionBecauseBlankPredicate()
            => Assert.ThrowsException<RDFSemanticsException>(() => new RDFOntologyData()
                        .DeclareDatatypeAssertion(new RDFResource("ex:indivA"), new RDFResource(), new RDFPlainLiteral("name")));

        [TestMethod]
        public void ShouldThrowExceptionOnDeclaringDatatypeAssertionBecauseNullValue()
            => Assert.ThrowsException<RDFSemanticsException>(() => new RDFOntologyData()
                        .DeclareDatatypeAssertion(new RDFResource("ex:indivA"), RDFVocabulary.FOAF.NAME, null));

        [TestMethod]
        public void ShouldEmitWarningOnDeclaringDatatypeAssertionBecauseReservedProperty()
        {
            string warningMsg = null;
            RDFSemanticsEvents.OnSemanticsWarning += (string msg) => { warningMsg = msg; };

            RDFOntologyData data = new RDFOntologyData();
            data.DeclareDatatypeAssertion(new RDFResource("ex:indivA"), RDFVocabulary.RDFS.SEE_ALSO, new RDFPlainLiteral("name")); //Reserved annotation property (not allowed by policy)

            Assert.IsNotNull(warningMsg);
            Assert.IsTrue(warningMsg.IndexOf("DatatypeAssertion relation between individual 'ex:indivA' and value 'name' through property 'http://www.w3.org/2000/01/rdf-schema#seeAlso' cannot be declared to the data because it would violate OWL-DL integrity") > -1);
            Assert.IsTrue(data.ABoxGraph.TriplesCount == 0);
        }

        [TestMethod]
        public void ShouldEmitWarningOnDeclaringDatatypeAssertionBecauseIncompatibleDatatypeAssertion()
        {
            string warningMsg = null;
            RDFSemanticsEvents.OnSemanticsWarning += (string msg) => { warningMsg = msg; };

            RDFOntologyData data = new RDFOntologyData();
            data.DeclareNegativeDatatypeAssertion(new RDFResource("ex:indivA"), new RDFResource("ex:dtProp"), new RDFPlainLiteral("name"));
            data.DeclareDatatypeAssertion(new RDFResource("ex:indivA"), new RDFResource("ex:dtProp"), new RDFPlainLiteral("name")); //OWL-DL contraddiction (not allowed by policy)

            Assert.IsNotNull(warningMsg);
            Assert.IsTrue(warningMsg.IndexOf("DatatypeAssertion relation between individual 'ex:indivA' and value 'name' through property 'ex:dtProp' cannot be declared to the data because it would violate OWL-DL integrity") > -1);
            Assert.IsTrue(data.ABoxGraph.TriplesCount == 4);
            Assert.IsTrue(data.ABoxGraph[null, RDFVocabulary.RDF.TYPE, RDFVocabulary.OWL.NEGATIVE_PROPERTY_ASSERTION, null].Any());
            Assert.IsTrue(data.ABoxGraph[null, RDFVocabulary.OWL.SOURCE_INDIVIDUAL, new RDFResource("ex:indivA"), null].Any());
            Assert.IsTrue(data.ABoxGraph[null, RDFVocabulary.OWL.ASSERTION_PROPERTY, new RDFResource("ex:dtProp"), null].Any());
            Assert.IsTrue(data.ABoxGraph[null, RDFVocabulary.OWL.TARGET_VALUE, null, new RDFPlainLiteral("name")].Any());
        }

        [TestMethod]
        public void ShouldDeclareNegativeObjectAssertion()
        {
            RDFOntologyData data = new RDFOntologyData();
            data.DeclareNegativeObjectAssertion(new RDFResource("ex:indivA"), RDFVocabulary.FOAF.KNOWS, new RDFResource("ex:indivB"));

            Assert.IsTrue(data.ABoxGraph.TriplesCount == 4);
            Assert.IsTrue(data.ABoxGraph[null, RDFVocabulary.RDF.TYPE, RDFVocabulary.OWL.NEGATIVE_PROPERTY_ASSERTION, null].Any());
            Assert.IsTrue(data.ABoxGraph[null, RDFVocabulary.OWL.SOURCE_INDIVIDUAL, new RDFResource("ex:indivA"), null].Any());
            Assert.IsTrue(data.ABoxGraph[null, RDFVocabulary.OWL.ASSERTION_PROPERTY, RDFVocabulary.FOAF.KNOWS, null].Any());
            Assert.IsTrue(data.ABoxGraph[null, RDFVocabulary.OWL.TARGET_INDIVIDUAL, new RDFResource("ex:indivB"), null].Any());
        }

        [TestMethod]
        public void ShouldThrowExceptionOnDeclaringNegativeObjectAssertionBecauseNullLeftIndividual()
            => Assert.ThrowsException<RDFSemanticsException>(() => new RDFOntologyData()
                        .DeclareNegativeObjectAssertion(null, RDFVocabulary.FOAF.KNOWS, new RDFResource("ex:indivB")));

        [TestMethod]
        public void ShouldThrowExceptionOnDeclaringNegativeObjectAssertionBecauseNullPredicate()
            => Assert.ThrowsException<RDFSemanticsException>(() => new RDFOntologyData()
                        .DeclareNegativeObjectAssertion(new RDFResource("ex:indivB"), null, new RDFResource("ex:indivB")));

        [TestMethod]
        public void ShouldThrowExceptionOnDeclaringNegativeObjectAssertionBecauseBlankPredicate()
            => Assert.ThrowsException<RDFSemanticsException>(() => new RDFOntologyData()
                        .DeclareNegativeObjectAssertion(new RDFResource("ex:indivB"), new RDFResource(), new RDFResource("ex:indivB")));

        [TestMethod]
        public void ShouldThrowExceptionOnDeclaringNegativeObjectAssertionBecauseNullRightIndividual()
            => Assert.ThrowsException<RDFSemanticsException>(() => new RDFOntologyData()
                        .DeclareNegativeObjectAssertion(new RDFResource("ex:indivA"), RDFVocabulary.FOAF.KNOWS, null));

        [TestMethod]
        public void ShouldEmitWarningOnDeclaringNegativeObjectAssertionBecauseReservedProperty()
        {
            string warningMsg = null;
            RDFSemanticsEvents.OnSemanticsWarning += (string msg) => { warningMsg = msg; };

            RDFOntologyData data = new RDFOntologyData();
            data.DeclareNegativeObjectAssertion(new RDFResource("ex:indivA"), RDFVocabulary.RDFS.SEE_ALSO, new RDFResource("ex:indivB")); //Reserved annotation property (not allowed by policy)

            Assert.IsNotNull(warningMsg);
            Assert.IsTrue(warningMsg.IndexOf("NegativeObjectAssertion relation between individual 'ex:indivA' and individual 'ex:indivB' through property 'http://www.w3.org/2000/01/rdf-schema#seeAlso' cannot be declared to the data because it would violate OWL-DL integrity") > -1);
            Assert.IsTrue(data.ABoxGraph.TriplesCount == 0);
        }

        [TestMethod]
        public void ShouldEmitWarningOnDeclaringNegativeObjectAssertionBecauseIncompatibleNegativeObjectAssertion()
        {
            string warningMsg = null;
            RDFSemanticsEvents.OnSemanticsWarning += (string msg) => { warningMsg = msg; };

            RDFOntologyData data = new RDFOntologyData();
            data.DeclareObjectAssertion(new RDFResource("ex:indivA"), new RDFResource("ex:objProp"), new RDFResource("ex:indivB"));
            data.DeclareNegativeObjectAssertion(new RDFResource("ex:indivA"), new RDFResource("ex:objProp"), new RDFResource("ex:indivB")); //OWL-DL contraddiction (not allowed by policy)

            Assert.IsNotNull(warningMsg);
            Assert.IsTrue(warningMsg.IndexOf("NegativeObjectAssertion relation between individual 'ex:indivA' and individual 'ex:indivB' through property 'ex:objProp' cannot be declared to the data because it would violate OWL-DL integrity") > -1);
            Assert.IsTrue(data.ABoxGraph.TriplesCount == 1);
            Assert.IsTrue(data.ABoxGraph[new RDFResource("ex:indivA"), new RDFResource("ex:objProp"), new RDFResource("ex:indivB"), null].Any());
        }

        [TestMethod]
        public void ShouldDeclareNegativeDatatypeAssertion()
        {
            RDFOntologyData data = new RDFOntologyData();
            data.DeclareNegativeDatatypeAssertion(new RDFResource("ex:indivA"), RDFVocabulary.FOAF.NAME, new RDFPlainLiteral("name"));

            Assert.IsTrue(data.ABoxGraph.TriplesCount == 4);
            Assert.IsTrue(data.ABoxGraph[null, RDFVocabulary.RDF.TYPE, RDFVocabulary.OWL.NEGATIVE_PROPERTY_ASSERTION, null].Any());
            Assert.IsTrue(data.ABoxGraph[null, RDFVocabulary.OWL.SOURCE_INDIVIDUAL, new RDFResource("ex:indivA"), null].Any());
            Assert.IsTrue(data.ABoxGraph[null, RDFVocabulary.OWL.ASSERTION_PROPERTY, RDFVocabulary.FOAF.NAME, null].Any());
            Assert.IsTrue(data.ABoxGraph[null, RDFVocabulary.OWL.TARGET_VALUE, null, new RDFPlainLiteral("name")].Any());
        }

        [TestMethod]
        public void ShouldThrowExceptionOnDeclaringNegativeDatatypeAssertionBecauseNullIndividual()
            => Assert.ThrowsException<RDFSemanticsException>(() => new RDFOntologyData()
                        .DeclareNegativeDatatypeAssertion(null, RDFVocabulary.FOAF.NAME, new RDFPlainLiteral("name")));

        [TestMethod]
        public void ShouldThrowExceptionOnDeclaringNegativeDatatypeAssertionBecauseNullPredicate()
            => Assert.ThrowsException<RDFSemanticsException>(() => new RDFOntologyData()
                        .DeclareNegativeDatatypeAssertion(new RDFResource("ex:indivA"), null, new RDFPlainLiteral("name")));

        [TestMethod]
        public void ShouldThrowExceptionOnDeclaringNegativeDatatypeAssertionBecauseBlankPredicate()
            => Assert.ThrowsException<RDFSemanticsException>(() => new RDFOntologyData()
                        .DeclareNegativeDatatypeAssertion(new RDFResource("ex:indivA"), new RDFResource(), new RDFPlainLiteral("name")));

        [TestMethod]
        public void ShouldThrowExceptionOnDeclaringNegativeDatatypeAssertionBecauseNullValue()
            => Assert.ThrowsException<RDFSemanticsException>(() => new RDFOntologyData()
                        .DeclareNegativeDatatypeAssertion(new RDFResource("ex:indivA"), RDFVocabulary.FOAF.NAME, null));

        [TestMethod]
        public void ShouldEmitWarningOnDeclaringNegativeDatatypeAssertionBecauseReservedProperty()
        {
            string warningMsg = null;
            RDFSemanticsEvents.OnSemanticsWarning += (string msg) => { warningMsg = msg; };

            RDFOntologyData data = new RDFOntologyData();
            data.DeclareNegativeDatatypeAssertion(new RDFResource("ex:indivA"), RDFVocabulary.RDFS.SEE_ALSO, new RDFPlainLiteral("name")); //Reserved annotation property (not allowed by policy)

            Assert.IsNotNull(warningMsg);
            Assert.IsTrue(warningMsg.IndexOf("NegativeDatatypeAssertion relation between individual 'ex:indivA' and value 'name' through property 'http://www.w3.org/2000/01/rdf-schema#seeAlso' cannot be declared to the data because it would violate OWL-DL integrity") > -1);
            Assert.IsTrue(data.ABoxGraph.TriplesCount == 0);
        }

        [TestMethod]
        public void ShouldEmitWarningOnDeclaringNegativeDatatypeAssertionBecauseIncompatibleNegativeDatatypeAssertion()
        {
            string warningMsg = null;
            RDFSemanticsEvents.OnSemanticsWarning += (string msg) => { warningMsg = msg; };

            RDFOntologyData data = new RDFOntologyData();
            data.DeclareDatatypeAssertion(new RDFResource("ex:indivA"), new RDFResource("ex:dtProp"), new RDFPlainLiteral("name"));
            data.DeclareNegativeDatatypeAssertion(new RDFResource("ex:indivA"), new RDFResource("ex:dtProp"), new RDFPlainLiteral("name")); //OWL-DL contraddiction (not allowed by policy)

            Assert.IsNotNull(warningMsg);
            Assert.IsTrue(warningMsg.IndexOf("NegativeDatatypeAssertion relation between individual 'ex:indivA' and value 'name' through property 'ex:dtProp' cannot be declared to the data because it would violate OWL-DL integrity") > -1);
            Assert.IsTrue(data.ABoxGraph.TriplesCount == 1);
            Assert.IsTrue(data.ABoxGraph[new RDFResource("ex:indivA"), new RDFResource("ex:dtProp"), null, new RDFPlainLiteral("name")].Any());
        }
        
        [TestMethod]
        public void ShouldExportToGraph()
        {
            RDFOntologyData data = new RDFOntologyData();
            data.DeclareIndividual(new RDFResource("ex:indivA"));
            data.DeclareIndividual(new RDFResource("ex:indivB"));
            data.DeclareIndividualType(new RDFResource("ex:indivA"), new RDFResource("ex:classA"));
            data.DeclareIndividualType(new RDFResource("ex:indivB"), new RDFResource("ex:classB"));
            data.AnnotateIndividual(new RDFResource("ex:indivA"), RDFVocabulary.RDFS.COMMENT, new RDFPlainLiteral("comment"));
            data.AnnotateIndividual(new RDFResource("ex:indivB"), RDFVocabulary.DC.DESCRIPTION, new RDFPlainLiteral("title"));
            data.DeclareSameIndividuals(new RDFResource("ex:indivA"), new RDFResource("ex:indivB"));
            RDFGraph graph = data.ToRDFGraph();

            Assert.IsNotNull(graph);
            Assert.IsTrue(graph.TriplesCount == 8);
        }

        [TestMethod]
        public async Task ShouldExportToGraphAsync()
        {
            RDFOntologyData data = new RDFOntologyData();
            data.DeclareIndividual(new RDFResource("ex:indivA"));
            data.DeclareIndividual(new RDFResource("ex:indivB"));
            data.DeclareIndividualType(new RDFResource("ex:indivA"), new RDFResource("ex:classA"));
            data.DeclareIndividualType(new RDFResource("ex:indivB"), new RDFResource("ex:classB"));
            data.AnnotateIndividual(new RDFResource("ex:indivA"), RDFVocabulary.RDFS.COMMENT, new RDFPlainLiteral("comment"));
            data.AnnotateIndividual(new RDFResource("ex:indivB"), RDFVocabulary.DC.DESCRIPTION, new RDFPlainLiteral("title"));
            data.DeclareSameIndividuals(new RDFResource("ex:indivA"), new RDFResource("ex:indivB"));
            RDFGraph graph = await data.ToRDFGraphAsync();

            Assert.IsNotNull(graph);
            Assert.IsTrue(graph.TriplesCount == 8);
        }
        #endregion
    }
}