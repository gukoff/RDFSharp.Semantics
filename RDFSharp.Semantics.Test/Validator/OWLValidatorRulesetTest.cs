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

using Microsoft.VisualStudio.TestTools.UnitTesting;
using RDFSharp.Model;
using System.Collections.Generic;

namespace RDFSharp.Semantics.Validator.Test
{
    [TestClass]
    public class OWLValidatorRulesetTest
    {
        #region Tests
        [TestMethod]
        public void ShouldValidateVocabularyDisjointness()
        {
            OWLOntology ontology = new OWLOntology("ex:ont");
            ontology.Model.ClassModel.DeclareClass(new RDFResource("ex:entity"));
            ontology.Model.PropertyModel.DeclareObjectProperty(new RDFResource("ex:entity"));
            ontology.Data.DeclareIndividual(new RDFResource("ex:entity"));

            OWLValidatorReport validatorReport = OWLValidatorRuleset.VocabularyDisjointness(ontology);

            Assert.IsNotNull(validatorReport);
            Assert.IsTrue(validatorReport.EvidencesCount == 3);
            Assert.IsTrue(validatorReport.SelectErrors().Count == 3);
            Assert.IsTrue(validatorReport.SelectWarnings().Count == 0);
        }

        [TestMethod]
        public void ShouldValidateVocabularyDeclaration_SubClassOf()
        {
            OWLOntology ontology = new OWLOntology("ex:ont");
            ontology.Model.ClassModel.DeclareSubClasses(new RDFResource("ex:class1"), new RDFResource("ex:class2"));

            OWLValidatorReport validatorReport = OWLValidatorRuleset.VocabularyDeclaration(ontology);

            Assert.IsNotNull(validatorReport);
            Assert.IsTrue(validatorReport.EvidencesCount == 2);
            Assert.IsTrue(validatorReport.SelectErrors().Count == 0);
            Assert.IsTrue(validatorReport.SelectWarnings().Count == 2);
        }

        [TestMethod]
        public void ShouldValidateVocabularyDeclaration_EquivalentClass()
        {
            OWLOntology ontology = new OWLOntology("ex:ont");
            ontology.Model.ClassModel.DeclareEquivalentClasses(new RDFResource("ex:class1"), new RDFResource("ex:class2"));

            OWLValidatorReport validatorReport = OWLValidatorRuleset.VocabularyDeclaration(ontology);

            Assert.IsNotNull(validatorReport);
            Assert.IsTrue(validatorReport.EvidencesCount == 4); //Consider also the automatic inferences
            Assert.IsTrue(validatorReport.SelectErrors().Count == 0);
            Assert.IsTrue(validatorReport.SelectWarnings().Count == 4);
        }

        [TestMethod]
        public void ShouldValidateVocabularyDeclaration_DisjointWith()
        {
            OWLOntology ontology = new OWLOntology("ex:ont");
            ontology.Model.ClassModel.DeclareDisjointClasses(new RDFResource("ex:class1"), new RDFResource("ex:class2"));

            OWLValidatorReport validatorReport = OWLValidatorRuleset.VocabularyDeclaration(ontology);

            Assert.IsNotNull(validatorReport);
            Assert.IsTrue(validatorReport.EvidencesCount == 4); //Consider also the automatic inferences
            Assert.IsTrue(validatorReport.SelectErrors().Count == 0);
            Assert.IsTrue(validatorReport.SelectWarnings().Count == 4);
        }

        [TestMethod]
        public void ShouldValidateVocabularyDeclaration_OneOf()
        {
            OWLOntology ontology = new OWLOntology("ex:ont");
            ontology.Model.ClassModel.TBoxGraph.AddTriple(new RDFTriple(new RDFResource("ex:enumerateClass"), RDFVocabulary.OWL.ONE_OF, new RDFResource("bnode:representative")));
            ontology.Model.ClassModel.TBoxGraph.AddTriple(new RDFTriple(new RDFResource("bnode:representative"), RDFVocabulary.RDF.TYPE, RDFVocabulary.RDF.LIST));
            ontology.Model.ClassModel.TBoxGraph.AddTriple(new RDFTriple(new RDFResource("bnode:representative"), RDFVocabulary.RDF.FIRST, new RDFResource("ex:individual1")));
            ontology.Model.ClassModel.TBoxGraph.AddTriple(new RDFTriple(new RDFResource("bnode:representative"), RDFVocabulary.RDF.REST, RDFVocabulary.RDF.NIL));

            OWLValidatorReport validatorReport = OWLValidatorRuleset.VocabularyDeclaration(ontology);

            Assert.IsNotNull(validatorReport);
            Assert.IsTrue(validatorReport.EvidencesCount == 2);
            Assert.IsTrue(validatorReport.SelectErrors().Count == 0);
            Assert.IsTrue(validatorReport.SelectWarnings().Count == 2);
        }

        [TestMethod]
        public void ShouldValidateVocabularyDeclaration_UnionOf()
        {
            OWLOntology ontology = new OWLOntology("ex:ont");
            ontology.Model.ClassModel.TBoxGraph.AddTriple(new RDFTriple(new RDFResource("ex:unionClass"), RDFVocabulary.OWL.UNION_OF, new RDFResource("bnode:representative")));
            ontology.Model.ClassModel.TBoxGraph.AddTriple(new RDFTriple(new RDFResource("bnode:representative"), RDFVocabulary.RDF.TYPE, RDFVocabulary.RDF.LIST));
            ontology.Model.ClassModel.TBoxGraph.AddTriple(new RDFTriple(new RDFResource("bnode:representative"), RDFVocabulary.RDF.FIRST, new RDFResource("ex:class1")));
            ontology.Model.ClassModel.TBoxGraph.AddTriple(new RDFTriple(new RDFResource("bnode:representative"), RDFVocabulary.RDF.REST, RDFVocabulary.RDF.NIL));

            OWLValidatorReport validatorReport = OWLValidatorRuleset.VocabularyDeclaration(ontology);

            Assert.IsNotNull(validatorReport);
            Assert.IsTrue(validatorReport.EvidencesCount == 2);
            Assert.IsTrue(validatorReport.SelectErrors().Count == 0);
            Assert.IsTrue(validatorReport.SelectWarnings().Count == 2);
        }

        [TestMethod]
        public void ShouldValidateVocabularyDeclaration_IntersectionOf()
        {
            OWLOntology ontology = new OWLOntology("ex:ont");
            ontology.Model.ClassModel.TBoxGraph.AddTriple(new RDFTriple(new RDFResource("ex:intersectionClass"), RDFVocabulary.OWL.INTERSECTION_OF, new RDFResource("bnode:representative")));
            ontology.Model.ClassModel.TBoxGraph.AddTriple(new RDFTriple(new RDFResource("bnode:representative"), RDFVocabulary.RDF.TYPE, RDFVocabulary.RDF.LIST));
            ontology.Model.ClassModel.TBoxGraph.AddTriple(new RDFTriple(new RDFResource("bnode:representative"), RDFVocabulary.RDF.FIRST, new RDFResource("ex:class1")));
            ontology.Model.ClassModel.TBoxGraph.AddTriple(new RDFTriple(new RDFResource("bnode:representative"), RDFVocabulary.RDF.REST, RDFVocabulary.RDF.NIL));

            OWLValidatorReport validatorReport = OWLValidatorRuleset.VocabularyDeclaration(ontology);

            Assert.IsNotNull(validatorReport);
            Assert.IsTrue(validatorReport.EvidencesCount == 2);
            Assert.IsTrue(validatorReport.SelectErrors().Count == 0);
            Assert.IsTrue(validatorReport.SelectWarnings().Count == 2);
        }

        [TestMethod]
        public void ShouldValidateVocabularyDeclaration_ComplementOf()
        {
            OWLOntology ontology = new OWLOntology("ex:ont");
            ontology.Model.ClassModel.TBoxGraph.AddTriple(new RDFTriple(new RDFResource("ex:class1"), RDFVocabulary.OWL.COMPLEMENT_OF, new RDFResource("ex:class2")));

            OWLValidatorReport validatorReport = OWLValidatorRuleset.VocabularyDeclaration(ontology);

            Assert.IsNotNull(validatorReport);
            Assert.IsTrue(validatorReport.EvidencesCount == 2);
            Assert.IsTrue(validatorReport.SelectErrors().Count == 0);
            Assert.IsTrue(validatorReport.SelectWarnings().Count == 2);
        }

        [TestMethod]
        public void ShouldValidateVocabularyDeclaration_HasKey()
        {
            OWLOntology ontology = new OWLOntology("ex:ont");
            ontology.Model.ClassModel.TBoxGraph.AddTriple(new RDFTriple(new RDFResource("ex:class"), RDFVocabulary.OWL.HAS_KEY, new RDFResource("bnode:representative")));
            ontology.Model.ClassModel.TBoxGraph.AddTriple(new RDFTriple(new RDFResource("bnode:representative"), RDFVocabulary.RDF.TYPE, RDFVocabulary.RDF.LIST));
            ontology.Model.ClassModel.TBoxGraph.AddTriple(new RDFTriple(new RDFResource("bnode:representative"), RDFVocabulary.RDF.FIRST, new RDFResource("ex:objprop1")));
            ontology.Model.ClassModel.TBoxGraph.AddTriple(new RDFTriple(new RDFResource("bnode:representative"), RDFVocabulary.RDF.REST, RDFVocabulary.RDF.NIL));

            OWLValidatorReport validatorReport = OWLValidatorRuleset.VocabularyDeclaration(ontology);

            Assert.IsNotNull(validatorReport);
            Assert.IsTrue(validatorReport.EvidencesCount == 2);
            Assert.IsTrue(validatorReport.SelectErrors().Count == 0);
            Assert.IsTrue(validatorReport.SelectWarnings().Count == 2);
        }

        [TestMethod]
        public void ShouldValidateVocabularyDeclaration_DisjointUnionOf()
        {
            OWLOntology ontology = new OWLOntology("ex:ont");
            ontology.Model.ClassModel.TBoxGraph.AddTriple(new RDFTriple(new RDFResource("ex:disjointUnionClass"), RDFVocabulary.OWL.DISJOINT_UNION_OF, new RDFResource("bnode:representative")));
            ontology.Model.ClassModel.TBoxGraph.AddTriple(new RDFTriple(new RDFResource("bnode:representative"), RDFVocabulary.RDF.TYPE, RDFVocabulary.RDF.LIST));
            ontology.Model.ClassModel.TBoxGraph.AddTriple(new RDFTriple(new RDFResource("bnode:representative"), RDFVocabulary.RDF.FIRST, new RDFResource("ex:class1")));
            ontology.Model.ClassModel.TBoxGraph.AddTriple(new RDFTriple(new RDFResource("bnode:representative"), RDFVocabulary.RDF.REST, RDFVocabulary.RDF.NIL));

            OWLValidatorReport validatorReport = OWLValidatorRuleset.VocabularyDeclaration(ontology);

            Assert.IsNotNull(validatorReport);
            Assert.IsTrue(validatorReport.EvidencesCount == 2);
            Assert.IsTrue(validatorReport.SelectErrors().Count == 0);
            Assert.IsTrue(validatorReport.SelectWarnings().Count == 2);
        }

        [TestMethod]
        public void ShouldValidateVocabularyDeclaration_AllDisjointClasses()
        {
            OWLOntology ontology = new OWLOntology("ex:ont");
            ontology.Model.ClassModel.TBoxGraph.AddTriple(new RDFTriple(new RDFResource("ex:allDisjointClasses"), RDFVocabulary.RDF.TYPE, RDFVocabulary.OWL.ALL_DISJOINT_CLASSES));
            ontology.Model.ClassModel.TBoxGraph.AddTriple(new RDFTriple(new RDFResource("ex:allDisjointClasses"), RDFVocabulary.OWL.MEMBERS, new RDFResource("bnode:representative")));
            ontology.Model.ClassModel.TBoxGraph.AddTriple(new RDFTriple(new RDFResource("bnode:representative"), RDFVocabulary.RDF.TYPE, RDFVocabulary.RDF.LIST));
            ontology.Model.ClassModel.TBoxGraph.AddTriple(new RDFTriple(new RDFResource("bnode:representative"), RDFVocabulary.RDF.FIRST, new RDFResource("ex:class1")));
            ontology.Model.ClassModel.TBoxGraph.AddTriple(new RDFTriple(new RDFResource("bnode:representative"), RDFVocabulary.RDF.REST, RDFVocabulary.RDF.NIL));

            OWLValidatorReport validatorReport = OWLValidatorRuleset.VocabularyDeclaration(ontology);

            Assert.IsNotNull(validatorReport);
            Assert.IsTrue(validatorReport.EvidencesCount == 2);
            Assert.IsTrue(validatorReport.SelectErrors().Count == 0);
            Assert.IsTrue(validatorReport.SelectWarnings().Count == 2);
        }

        [TestMethod]
        public void ShouldValidateVocabularyDeclaration_AllValuesFrom()
        {
            OWLOntology ontology = new OWLOntology("ex:ont");
            ontology.Model.ClassModel.TBoxGraph.AddTriple(new RDFTriple(new RDFResource("ex:class1"), RDFVocabulary.OWL.ALL_VALUES_FROM, new RDFResource("ex:class2")));

            OWLValidatorReport validatorReport = OWLValidatorRuleset.VocabularyDeclaration(ontology);

            Assert.IsNotNull(validatorReport);
            Assert.IsTrue(validatorReport.EvidencesCount == 2);
            Assert.IsTrue(validatorReport.SelectErrors().Count == 0);
            Assert.IsTrue(validatorReport.SelectWarnings().Count == 2);
        }

        [TestMethod]
        public void ShouldValidateVocabularyDeclaration_SomeValuesFrom()
        {
            OWLOntology ontology = new OWLOntology("ex:ont");
            ontology.Model.ClassModel.TBoxGraph.AddTriple(new RDFTriple(new RDFResource("ex:class1"), RDFVocabulary.OWL.SOME_VALUES_FROM, new RDFResource("ex:class2")));

            OWLValidatorReport validatorReport = OWLValidatorRuleset.VocabularyDeclaration(ontology);

            Assert.IsNotNull(validatorReport);
            Assert.IsTrue(validatorReport.EvidencesCount == 2);
            Assert.IsTrue(validatorReport.SelectErrors().Count == 0);
            Assert.IsTrue(validatorReport.SelectWarnings().Count == 2);
        }

        [TestMethod]
        public void ShouldValidateVocabularyDeclaration_HasValue()
        {
            OWLOntology ontology = new OWLOntology("ex:ont");
            ontology.Model.ClassModel.TBoxGraph.AddTriple(new RDFTriple(new RDFResource("ex:class"), RDFVocabulary.OWL.HAS_VALUE, new RDFResource("ex:individual")));

            OWLValidatorReport validatorReport = OWLValidatorRuleset.VocabularyDeclaration(ontology);

            Assert.IsNotNull(validatorReport);
            Assert.IsTrue(validatorReport.EvidencesCount == 2);
            Assert.IsTrue(validatorReport.SelectErrors().Count == 0);
            Assert.IsTrue(validatorReport.SelectWarnings().Count == 2);
        }

        [TestMethod]
        public void ShouldValidateVocabularyDeclaration_HasSelf()
        {
            OWLOntology ontology = new OWLOntology("ex:ont");
            ontology.Model.ClassModel.TBoxGraph.AddTriple(new RDFTriple(new RDFResource("ex:class"), RDFVocabulary.OWL.HAS_SELF, RDFTypedLiteral.True));

            OWLValidatorReport validatorReport = OWLValidatorRuleset.VocabularyDeclaration(ontology);

            Assert.IsNotNull(validatorReport);
            Assert.IsTrue(validatorReport.EvidencesCount == 1);
            Assert.IsTrue(validatorReport.SelectErrors().Count == 0);
            Assert.IsTrue(validatorReport.SelectWarnings().Count == 1);
        }

        [TestMethod]
        public void ShouldValidateVocabularyDeclaration_OnClass()
        {
            OWLOntology ontology = new OWLOntology("ex:ont");
            ontology.Model.ClassModel.TBoxGraph.AddTriple(new RDFTriple(new RDFResource("ex:qRestr"), RDFVocabulary.OWL.ON_CLASS, new RDFResource("ex:class")));

            OWLValidatorReport validatorReport = OWLValidatorRuleset.VocabularyDeclaration(ontology);

            Assert.IsNotNull(validatorReport);
            Assert.IsTrue(validatorReport.EvidencesCount == 2);
            Assert.IsTrue(validatorReport.SelectErrors().Count == 0);
            Assert.IsTrue(validatorReport.SelectWarnings().Count == 2);
        }
        #endregion
    }
}