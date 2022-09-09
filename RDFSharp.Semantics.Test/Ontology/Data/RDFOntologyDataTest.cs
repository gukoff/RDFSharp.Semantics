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
        public void ShouldNotEmitWarningOnDeclaringIndividualTypeEvenWhenReservedClass()
        {
            string warningMsg = null;
            RDFSemanticsEvents.OnSemanticsWarning += (string msg) => { warningMsg = msg; };

            //Permissive policy avoids most of real-time OWL-DL safety checks (at the cost of ontology integrity)
            RDFSemanticsOptions.OWLDLIntegrityPolicy = RDFSemanticsEnums.RDFOntologyOWLDLIntegrityPolicy.Permissive;

            RDFOntologyData data = new RDFOntologyData();
            data.DeclareIndividual(new RDFResource("ex:indivA"));
            data.DeclareIndividualType(new RDFResource("ex:indivA"), RDFVocabulary.RDFS.RESOURCE);

            Assert.IsNull(warningMsg);
            Assert.IsTrue(data.ABoxGraph.TriplesCount == 2);
            Assert.IsTrue(data.ABoxGraph.ContainsTriple(new RDFTriple(new RDFResource("ex:indivA"), RDFVocabulary.RDF.TYPE, RDFVocabulary.OWL.NAMED_INDIVIDUAL)));
            Assert.IsTrue(data.ABoxGraph.ContainsTriple(new RDFTriple(new RDFResource("ex:indivA"), RDFVocabulary.RDF.TYPE, RDFVocabulary.RDFS.RESOURCE)));

        }

        [TestMethod]
        public void ShouldEmitWarningOnDeclaringIndividualTypeBecauseReservedClass()
        {
            string warningMsg = null;
            RDFSemanticsEvents.OnSemanticsWarning += (string msg) => { warningMsg = msg; };

            //Strict policy executes most of real-time OWL-DL safety checks (at the cost of performances)
            RDFSemanticsOptions.OWLDLIntegrityPolicy = RDFSemanticsEnums.RDFOntologyOWLDLIntegrityPolicy.Strict;

            RDFOntologyData data = new RDFOntologyData();
            data.DeclareIndividual(new RDFResource("ex:indivA"));
            data.DeclareIndividualType(new RDFResource("ex:indivA"), RDFVocabulary.RDFS.RESOURCE);

            Assert.IsNotNull(warningMsg);
            Assert.IsTrue(warningMsg.IndexOf("Type relation between individual 'ex:indivA' and class 'http://www.w3.org/2000/01/rdf-schema#Resource' cannot be added to the data because it would violate OWL-DL integrity") > -1);
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

            Assert.IsTrue(data.ABoxGraph.TriplesCount == 3);
            Assert.IsTrue(data.ABoxGraph.ContainsTriple(new RDFTriple(new RDFResource("ex:indivA"), RDFVocabulary.RDF.TYPE, RDFVocabulary.OWL.NAMED_INDIVIDUAL)));
            Assert.IsTrue(data.ABoxGraph.ContainsTriple(new RDFTriple(new RDFResource("ex:indivB"), RDFVocabulary.RDF.TYPE, RDFVocabulary.OWL.NAMED_INDIVIDUAL)));
            Assert.IsTrue(data.ABoxGraph.ContainsTriple(new RDFTriple(new RDFResource("ex:indivA"), RDFVocabulary.OWL.SAME_AS, new RDFResource("ex:indivB"))));
            Assert.IsTrue(data.ABoxInferenceGraph.TriplesCount == 1);
            Assert.IsTrue(data.ABoxInferenceGraph.ContainsTriple(new RDFTriple(new RDFResource("ex:indivB"), RDFVocabulary.OWL.SAME_AS, new RDFResource("ex:indivA"))));
        }

        [TestMethod]
        public void ShouldNotEmitWarningOnDeclaringSameIndividualsEvenWhenIncompatibleIndividuals()
        {
            string warningMsg = null;
            RDFSemanticsEvents.OnSemanticsWarning += (string msg) => { warningMsg = msg; };

            //Permissive policy avoids most of real-time OWL-DL safety checks (at the cost of ontology integrity)
            RDFSemanticsOptions.OWLDLIntegrityPolicy = RDFSemanticsEnums.RDFOntologyOWLDLIntegrityPolicy.Permissive;

            RDFOntologyData data = new RDFOntologyData();
            data.DeclareIndividual(new RDFResource("ex:indivA"));
            data.DeclareIndividual(new RDFResource("ex:indivB"));
            data.DeclareDifferentIndividuals(new RDFResource("ex:indivA"), new RDFResource("ex:indivB"));
            data.DeclareSameIndividuals(new RDFResource("ex:indivA"), new RDFResource("ex:indivB")); //OWL-DL contraddiction (permitted by policy)

            Assert.IsNull(warningMsg);
            Assert.IsTrue(data.ABoxGraph.TriplesCount == 4);
            Assert.IsTrue(data.ABoxGraph.ContainsTriple(new RDFTriple(new RDFResource("ex:indivA"), RDFVocabulary.RDF.TYPE, RDFVocabulary.OWL.NAMED_INDIVIDUAL)));
            Assert.IsTrue(data.ABoxGraph.ContainsTriple(new RDFTriple(new RDFResource("ex:indivB"), RDFVocabulary.RDF.TYPE, RDFVocabulary.OWL.NAMED_INDIVIDUAL)));
            Assert.IsTrue(data.ABoxGraph.ContainsTriple(new RDFTriple(new RDFResource("ex:indivA"), RDFVocabulary.OWL.DIFFERENT_FROM, new RDFResource("ex:indivB"))));
            Assert.IsTrue(data.ABoxGraph.ContainsTriple(new RDFTriple(new RDFResource("ex:indivA"), RDFVocabulary.OWL.SAME_AS, new RDFResource("ex:indivB"))));
            Assert.IsTrue(data.ABoxInferenceGraph.TriplesCount == 2);
            Assert.IsTrue(data.ABoxInferenceGraph.ContainsTriple(new RDFTriple(new RDFResource("ex:indivB"), RDFVocabulary.OWL.DIFFERENT_FROM, new RDFResource("ex:indivA"))));
            Assert.IsTrue(data.ABoxInferenceGraph.ContainsTriple(new RDFTriple(new RDFResource("ex:indivB"), RDFVocabulary.OWL.SAME_AS, new RDFResource("ex:indivA"))));
        }

        [TestMethod]
        public void ShouldEmitWarningOnDeclaringSameIndividualsBecauseIncompatibleIndividuals()
        {
            string warningMsg = null;
            RDFSemanticsEvents.OnSemanticsWarning += (string msg) => { warningMsg = msg; };

            //Strict policy executes most of real-time OWL-DL safety checks (at the cost of performances)
            RDFSemanticsOptions.OWLDLIntegrityPolicy = RDFSemanticsEnums.RDFOntologyOWLDLIntegrityPolicy.Strict;

            RDFOntologyData data = new RDFOntologyData();
            data.DeclareIndividual(new RDFResource("ex:indivA"));
            data.DeclareIndividual(new RDFResource("ex:indivB"));
            data.DeclareDifferentIndividuals(new RDFResource("ex:indivA"), new RDFResource("ex:indivB"));
            data.DeclareSameIndividuals(new RDFResource("ex:indivA"), new RDFResource("ex:indivB"));  //OWL-DL contraddiction (enforced by policy)

            Assert.IsNotNull(warningMsg);
            Assert.IsTrue(warningMsg.IndexOf("SameAs relation between individual 'ex:indivA' and individual 'ex:indivB' cannot be added to the data because it would violate OWL-DL integrity") > -1);
            Assert.IsTrue(data.ABoxGraph.ContainsTriple(new RDFTriple(new RDFResource("ex:indivA"), RDFVocabulary.RDF.TYPE, RDFVocabulary.OWL.NAMED_INDIVIDUAL)));
            Assert.IsTrue(data.ABoxGraph.ContainsTriple(new RDFTriple(new RDFResource("ex:indivB"), RDFVocabulary.RDF.TYPE, RDFVocabulary.OWL.NAMED_INDIVIDUAL)));
            Assert.IsTrue(data.ABoxGraph.ContainsTriple(new RDFTriple(new RDFResource("ex:indivA"), RDFVocabulary.OWL.DIFFERENT_FROM, new RDFResource("ex:indivB"))));
            Assert.IsTrue(data.ABoxInferenceGraph.TriplesCount == 1);
            Assert.IsTrue(data.ABoxInferenceGraph.ContainsTriple(new RDFTriple(new RDFResource("ex:indivB"), RDFVocabulary.OWL.DIFFERENT_FROM, new RDFResource("ex:indivA"))));
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

            Assert.IsTrue(data.ABoxGraph.TriplesCount == 3);
            Assert.IsTrue(data.ABoxGraph.ContainsTriple(new RDFTriple(new RDFResource("ex:indivA"), RDFVocabulary.RDF.TYPE, RDFVocabulary.OWL.NAMED_INDIVIDUAL)));
            Assert.IsTrue(data.ABoxGraph.ContainsTriple(new RDFTriple(new RDFResource("ex:indivB"), RDFVocabulary.RDF.TYPE, RDFVocabulary.OWL.NAMED_INDIVIDUAL)));
            Assert.IsTrue(data.ABoxGraph.ContainsTriple(new RDFTriple(new RDFResource("ex:indivA"), RDFVocabulary.OWL.DIFFERENT_FROM, new RDFResource("ex:indivB"))));
            Assert.IsTrue(data.ABoxInferenceGraph.TriplesCount == 1);
            Assert.IsTrue(data.ABoxInferenceGraph.ContainsTriple(new RDFTriple(new RDFResource("ex:indivB"), RDFVocabulary.OWL.DIFFERENT_FROM, new RDFResource("ex:indivA"))));
        }

        [TestMethod]
        public void ShouldNotEmitWarningOnDeclaringDifferentIndividualsEvenWhenIncompatibleIndividuals()
        {
            string warningMsg = null;
            RDFSemanticsEvents.OnSemanticsWarning += (string msg) => { warningMsg = msg; };

            //Permissive policy avoids most of real-time OWL-DL safety checks (at the cost of ontology integrity)
            RDFSemanticsOptions.OWLDLIntegrityPolicy = RDFSemanticsEnums.RDFOntologyOWLDLIntegrityPolicy.Permissive;

            RDFOntologyData data = new RDFOntologyData();
            data.DeclareIndividual(new RDFResource("ex:indivA"));
            data.DeclareIndividual(new RDFResource("ex:indivB"));
            data.DeclareSameIndividuals(new RDFResource("ex:indivA"), new RDFResource("ex:indivB"));
            data.DeclareDifferentIndividuals(new RDFResource("ex:indivA"), new RDFResource("ex:indivB")); //OWL-DL contraddiction (permitted by policy)

            Assert.IsNull(warningMsg);
            Assert.IsTrue(data.ABoxGraph.TriplesCount == 4);
            Assert.IsTrue(data.ABoxGraph.ContainsTriple(new RDFTriple(new RDFResource("ex:indivA"), RDFVocabulary.RDF.TYPE, RDFVocabulary.OWL.NAMED_INDIVIDUAL)));
            Assert.IsTrue(data.ABoxGraph.ContainsTriple(new RDFTriple(new RDFResource("ex:indivB"), RDFVocabulary.RDF.TYPE, RDFVocabulary.OWL.NAMED_INDIVIDUAL)));
            Assert.IsTrue(data.ABoxGraph.ContainsTriple(new RDFTriple(new RDFResource("ex:indivA"), RDFVocabulary.OWL.SAME_AS, new RDFResource("ex:indivB"))));
            Assert.IsTrue(data.ABoxGraph.ContainsTriple(new RDFTriple(new RDFResource("ex:indivA"), RDFVocabulary.OWL.DIFFERENT_FROM, new RDFResource("ex:indivB"))));
            Assert.IsTrue(data.ABoxInferenceGraph.TriplesCount == 2);
            Assert.IsTrue(data.ABoxInferenceGraph.ContainsTriple(new RDFTriple(new RDFResource("ex:indivB"), RDFVocabulary.OWL.SAME_AS, new RDFResource("ex:indivA"))));
            Assert.IsTrue(data.ABoxInferenceGraph.ContainsTriple(new RDFTriple(new RDFResource("ex:indivB"), RDFVocabulary.OWL.DIFFERENT_FROM, new RDFResource("ex:indivA"))));
        }

        [TestMethod]
        public void ShouldEmitWarningOnDeclaringDifferentIndividualsBecauseIncompatibleIndividuals()
        {
            string warningMsg = null;
            RDFSemanticsEvents.OnSemanticsWarning += (string msg) => { warningMsg = msg; };

            //Strict policy executes most of real-time OWL-DL safety checks (at the cost of performances)
            RDFSemanticsOptions.OWLDLIntegrityPolicy = RDFSemanticsEnums.RDFOntologyOWLDLIntegrityPolicy.Strict;

            RDFOntologyData data = new RDFOntologyData();
            data.DeclareIndividual(new RDFResource("ex:indivA"));
            data.DeclareIndividual(new RDFResource("ex:indivB"));
            data.DeclareSameIndividuals(new RDFResource("ex:indivA"), new RDFResource("ex:indivB"));
            data.DeclareDifferentIndividuals(new RDFResource("ex:indivA"), new RDFResource("ex:indivB"));  //OWL-DL contraddiction (enforced by policy)

            Assert.IsNotNull(warningMsg);
            Assert.IsTrue(warningMsg.IndexOf("DifferentFrom relation between individual 'ex:indivA' and individual 'ex:indivB' cannot be added to the data because it would violate OWL-DL integrity") > -1);
            Assert.IsTrue(data.ABoxGraph.ContainsTriple(new RDFTriple(new RDFResource("ex:indivA"), RDFVocabulary.RDF.TYPE, RDFVocabulary.OWL.NAMED_INDIVIDUAL)));
            Assert.IsTrue(data.ABoxGraph.ContainsTriple(new RDFTriple(new RDFResource("ex:indivB"), RDFVocabulary.RDF.TYPE, RDFVocabulary.OWL.NAMED_INDIVIDUAL)));
            Assert.IsTrue(data.ABoxGraph.ContainsTriple(new RDFTriple(new RDFResource("ex:indivA"), RDFVocabulary.OWL.SAME_AS, new RDFResource("ex:indivB"))));
            Assert.IsTrue(data.ABoxInferenceGraph.TriplesCount == 1);
            Assert.IsTrue(data.ABoxInferenceGraph.ContainsTriple(new RDFTriple(new RDFResource("ex:indivB"), RDFVocabulary.OWL.SAME_AS, new RDFResource("ex:indivA"))));
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
        #endregion
    }
}