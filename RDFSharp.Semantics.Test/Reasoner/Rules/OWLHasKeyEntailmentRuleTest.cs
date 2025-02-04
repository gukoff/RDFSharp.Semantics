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

using Microsoft.VisualStudio.TestTools.UnitTesting;
using RDFSharp.Model;
using System.Collections.Generic;

namespace RDFSharp.Semantics.Reasoner.Test
{
    [TestClass]
    public class OWLHasKeyEntailmentRuleTest
    {
        #region Tests
        [TestMethod]
        public void ShouldExecuteHasKeyEntailmentWithSingleObjectProperty()
        {
            OWLOntology ontology = new OWLOntology("ex:ont");
            ontology.Model.ClassModel.DeclareClass(new RDFResource("ex:class"));
            ontology.Model.ClassModel.DeclareHasKey(new RDFResource("ex:class"), new List<RDFResource>() { new RDFResource("ex:objprop") });
            ontology.Model.PropertyModel.DeclareObjectProperty(new RDFResource("ex:objprop"));
            ontology.Data.DeclareIndividual(new RDFResource("ex:indiv1"));
            ontology.Data.DeclareIndividualType(new RDFResource("ex:indiv1"), new RDFResource("ex:class"));
            ontology.Data.DeclareIndividual(new RDFResource("ex:indiv2"));
            ontology.Data.DeclareIndividualType(new RDFResource("ex:indiv2"), new RDFResource("ex:class"));
            ontology.Data.DeclareIndividual(new RDFResource("ex:indiv3"));
            ontology.Data.DeclareIndividualType(new RDFResource("ex:indiv3"), new RDFResource("ex:class"));
            ontology.Data.DeclareObjectAssertion(new RDFResource("ex:indiv1"), new RDFResource("ex:objprop"), new RDFResource("ex:keyval"));
            ontology.Data.DeclareObjectAssertion(new RDFResource("ex:indiv2"), new RDFResource("ex:objprop"), new RDFResource("ex:keyval"));
            ontology.Data.DeclareObjectAssertion(new RDFResource("ex:indiv3"), new RDFResource("ex:objprop"), new RDFResource("ex:keyval2"));

            OWLReasonerReport reasonerReport = OWLHasKeyEntailmentRule.ExecuteRule(ontology);

            Assert.IsNotNull(reasonerReport);
            Assert.IsTrue(reasonerReport.EvidencesCount == 2);
        }

        [TestMethod]
        public void ShouldExecuteHasKeyEntailmentWithMultipleObjectProperties()
        {
            OWLOntology ontology = new OWLOntology("ex:ont");
            ontology.Model.ClassModel.DeclareClass(new RDFResource("ex:class"));
            ontology.Model.ClassModel.DeclareHasKey(new RDFResource("ex:class"), new List<RDFResource>() { new RDFResource("ex:objprop"), new RDFResource("ex:objprop2") });
            ontology.Model.PropertyModel.DeclareObjectProperty(new RDFResource("ex:objprop"));
            ontology.Model.PropertyModel.DeclareObjectProperty(new RDFResource("ex:objprop2"));
            ontology.Data.DeclareIndividual(new RDFResource("ex:indiv1"));
            ontology.Data.DeclareIndividualType(new RDFResource("ex:indiv1"), new RDFResource("ex:class"));
            ontology.Data.DeclareIndividual(new RDFResource("ex:indiv2"));
            ontology.Data.DeclareIndividualType(new RDFResource("ex:indiv2"), new RDFResource("ex:class"));
            ontology.Data.DeclareIndividual(new RDFResource("ex:indiv3"));
            ontology.Data.DeclareIndividualType(new RDFResource("ex:indiv3"), new RDFResource("ex:class"));
            ontology.Data.DeclareObjectAssertion(new RDFResource("ex:indiv1"), new RDFResource("ex:objprop"), new RDFResource("ex:keyval"));
            ontology.Data.DeclareObjectAssertion(new RDFResource("ex:indiv1"), new RDFResource("ex:objprop2"), new RDFResource("ex:keyval2"));
            ontology.Data.DeclareObjectAssertion(new RDFResource("ex:indiv2"), new RDFResource("ex:objprop"), new RDFResource("ex:keyval"));
            ontology.Data.DeclareObjectAssertion(new RDFResource("ex:indiv2"), new RDFResource("ex:objprop2"), new RDFResource("ex:keyval2"));
            ontology.Data.DeclareObjectAssertion(new RDFResource("ex:indiv3"), new RDFResource("ex:objprop"), new RDFResource("ex:keyval"));

            OWLReasonerReport reasonerReport = OWLHasKeyEntailmentRule.ExecuteRule(ontology);

            Assert.IsNotNull(reasonerReport);
            Assert.IsTrue(reasonerReport.EvidencesCount == 2);
        }

        [TestMethod]
        public void ShouldExecuteHasKeyEntailmentWithSingleDatatypeProperty()
        {
            OWLOntology ontology = new OWLOntology("ex:ont");
            ontology.Model.ClassModel.DeclareClass(new RDFResource("ex:class"));
            ontology.Model.ClassModel.DeclareHasKey(new RDFResource("ex:class"), new List<RDFResource>() { new RDFResource("ex:dtprop") });
            ontology.Model.PropertyModel.DeclareDatatypeProperty(new RDFResource("ex:dtprop"));
            ontology.Data.DeclareIndividual(new RDFResource("ex:indiv1"));
            ontology.Data.DeclareIndividualType(new RDFResource("ex:indiv1"), new RDFResource("ex:class"));
            ontology.Data.DeclareIndividual(new RDFResource("ex:indiv2"));
            ontology.Data.DeclareIndividualType(new RDFResource("ex:indiv2"), new RDFResource("ex:class"));
            ontology.Data.DeclareIndividual(new RDFResource("ex:indiv3"));
            ontology.Data.DeclareIndividualType(new RDFResource("ex:indiv3"), new RDFResource("ex:class"));
            ontology.Data.DeclareDatatypeAssertion(new RDFResource("ex:indiv1"), new RDFResource("ex:dtprop"), new RDFPlainLiteral("keyval"));
            ontology.Data.DeclareDatatypeAssertion(new RDFResource("ex:indiv2"), new RDFResource("ex:dtprop"), new RDFPlainLiteral("keyval"));
            ontology.Data.DeclareDatatypeAssertion(new RDFResource("ex:indiv3"), new RDFResource("ex:dtprop"), new RDFPlainLiteral("keyval2"));

            OWLReasonerReport reasonerReport = OWLHasKeyEntailmentRule.ExecuteRule(ontology);

            Assert.IsNotNull(reasonerReport);
            Assert.IsTrue(reasonerReport.EvidencesCount == 2);
        }

        [TestMethod]
        public void ShouldExecuteHasKeyEntailmentWithMultipleDatatypeProperties()
        {
            OWLOntology ontology = new OWLOntology("ex:ont");
            ontology.Model.ClassModel.DeclareClass(new RDFResource("ex:class"));
            ontology.Model.ClassModel.DeclareHasKey(new RDFResource("ex:class"), new List<RDFResource>() { new RDFResource("ex:dtprop"), new RDFResource("ex:dtprop2") });
            ontology.Model.PropertyModel.DeclareDatatypeProperty(new RDFResource("ex:dtprop"));
            ontology.Model.PropertyModel.DeclareDatatypeProperty(new RDFResource("ex:dtprop2"));
            ontology.Data.DeclareIndividual(new RDFResource("ex:indiv1"));
            ontology.Data.DeclareIndividualType(new RDFResource("ex:indiv1"), new RDFResource("ex:class"));
            ontology.Data.DeclareIndividual(new RDFResource("ex:indiv2"));
            ontology.Data.DeclareIndividualType(new RDFResource("ex:indiv2"), new RDFResource("ex:class"));
            ontology.Data.DeclareIndividual(new RDFResource("ex:indiv3"));
            ontology.Data.DeclareIndividualType(new RDFResource("ex:indiv3"), new RDFResource("ex:class"));
            ontology.Data.DeclareDatatypeAssertion(new RDFResource("ex:indiv1"), new RDFResource("ex:dtprop"), new RDFPlainLiteral("keyval"));
            ontology.Data.DeclareDatatypeAssertion(new RDFResource("ex:indiv1"), new RDFResource("ex:dtprop2"), new RDFPlainLiteral("keyval2"));
            ontology.Data.DeclareDatatypeAssertion(new RDFResource("ex:indiv2"), new RDFResource("ex:dtprop"), new RDFPlainLiteral("keyval"));
            ontology.Data.DeclareDatatypeAssertion(new RDFResource("ex:indiv2"), new RDFResource("ex:dtprop2"), new RDFPlainLiteral("keyval2"));
            ontology.Data.DeclareDatatypeAssertion(new RDFResource("ex:indiv3"), new RDFResource("ex:dtprop"), new RDFPlainLiteral("keyval"));

            OWLReasonerReport reasonerReport = OWLHasKeyEntailmentRule.ExecuteRule(ontology);

            Assert.IsNotNull(reasonerReport);
            Assert.IsTrue(reasonerReport.EvidencesCount == 2);
        }

        [TestMethod]
        public void ShouldExecuteHasKeyEntailmentViaReasoner()
        {
            OWLOntology ontology = new OWLOntology("ex:ont");
            ontology.Model.ClassModel.DeclareClass(new RDFResource("ex:class"));
            ontology.Model.ClassModel.DeclareHasKey(new RDFResource("ex:class"), new List<RDFResource>() { new RDFResource("ex:objprop") });
            ontology.Model.PropertyModel.DeclareObjectProperty(new RDFResource("ex:objprop"));
            ontology.Data.DeclareIndividual(new RDFResource("ex:indiv1"));
            ontology.Data.DeclareIndividualType(new RDFResource("ex:indiv1"), new RDFResource("ex:class"));
            ontology.Data.DeclareIndividual(new RDFResource("ex:indiv2"));
            ontology.Data.DeclareIndividualType(new RDFResource("ex:indiv2"), new RDFResource("ex:class"));
            ontology.Data.DeclareIndividual(new RDFResource("ex:indiv3"));
            ontology.Data.DeclareIndividualType(new RDFResource("ex:indiv3"), new RDFResource("ex:class"));
            ontology.Data.DeclareObjectAssertion(new RDFResource("ex:indiv1"), new RDFResource("ex:objprop"), new RDFResource("ex:keyval"));
            ontology.Data.DeclareObjectAssertion(new RDFResource("ex:indiv2"), new RDFResource("ex:objprop"), new RDFResource("ex:keyval"));
            ontology.Data.DeclareObjectAssertion(new RDFResource("ex:indiv3"), new RDFResource("ex:objprop"), new RDFResource("ex:keyval2"));

            OWLReasoner reasoner = new OWLReasoner().AddStandardRule(OWLSemanticsEnums.OWLReasonerStandardRules.HasKeyEntailment);
            OWLReasonerReport reasonerReport = reasoner.ApplyToOntology(ontology);

            Assert.IsNotNull(reasonerReport);
            Assert.IsTrue(reasonerReport.EvidencesCount == 2);
        }

        [TestMethod]
        public void ShouldExecuteHasKeyEntailmentViaReasonerAndNotEntailBecauseDifferentIndividuals()
        {
            OWLOntology ontology = new OWLOntology("ex:ont");
            ontology.Model.ClassModel.DeclareClass(new RDFResource("ex:class"));
            ontology.Model.ClassModel.DeclareHasKey(new RDFResource("ex:class"), new List<RDFResource>() { new RDFResource("ex:objprop") });
            ontology.Model.PropertyModel.DeclareObjectProperty(new RDFResource("ex:objprop"));
            ontology.Data.DeclareIndividual(new RDFResource("ex:indiv1"));
            ontology.Data.DeclareIndividualType(new RDFResource("ex:indiv1"), new RDFResource("ex:class"));
            ontology.Data.DeclareIndividual(new RDFResource("ex:indiv2"));
            ontology.Data.DeclareIndividualType(new RDFResource("ex:indiv2"), new RDFResource("ex:class"));
            ontology.Data.DeclareAllDifferentIndividuals(new RDFResource("ex:alldiffIndivs"), new List<RDFResource>() { new RDFResource("ex:indiv1"), new RDFResource("ex:indiv2") });
            ontology.Data.DeclareIndividual(new RDFResource("ex:indiv3"));
            ontology.Data.DeclareIndividualType(new RDFResource("ex:indiv3"), new RDFResource("ex:class"));
            ontology.Data.DeclareObjectAssertion(new RDFResource("ex:indiv1"), new RDFResource("ex:objprop"), new RDFResource("ex:keyval"));
            ontology.Data.DeclareObjectAssertion(new RDFResource("ex:indiv2"), new RDFResource("ex:objprop"), new RDFResource("ex:keyval"));
            ontology.Data.DeclareObjectAssertion(new RDFResource("ex:indiv3"), new RDFResource("ex:objprop"), new RDFResource("ex:keyval2"));

            OWLReasoner reasoner = new OWLReasoner().AddStandardRule(OWLSemanticsEnums.OWLReasonerStandardRules.HasKeyEntailment);
            OWLReasonerReport reasonerReport = reasoner.ApplyToOntology(ontology);

            Assert.IsNotNull(reasonerReport);
            Assert.IsTrue(reasonerReport.EvidencesCount == 0);
        }
        #endregion
    }
}