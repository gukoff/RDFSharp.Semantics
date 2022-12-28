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

namespace RDFSharp.Semantics.Reasoner.Test
{
    [TestClass]
    public class OWLIndividualTypeEntailmentRuleTest
    {
        #region Tests
        [TestMethod]
        public void ShouldExecuteIndividualTypeEntailmentOnSimpleClass()
        {
            OWLOntology ontology = new OWLOntology("ex:ont");
            ontology.Model.ClassModel.DeclareClass(new RDFResource("ex:class1"));
            ontology.Model.ClassModel.DeclareClass(new RDFResource("ex:class2"));
            ontology.Model.ClassModel.DeclareClass(new RDFResource("ex:class3"));
            ontology.Model.ClassModel.DeclareSubClasses(new RDFResource("ex:class2"), new RDFResource("ex:class1"));
            ontology.Model.ClassModel.DeclareEquivalentClasses(new RDFResource("ex:class2"), new RDFResource("ex:class3"));
            ontology.Data.DeclareIndividual(new RDFResource("ex:indiv"));
            ontology.Data.DeclareIndividualType(new RDFResource("ex:indiv"), new RDFResource("ex:class3"));

            OWLReasonerReport reasonerReport = OWLIndividualTypeEntailmentRule.ExecuteRule(ontology, OWLOntologyLoaderOptions.DefaultOptions);

            Assert.IsNotNull(reasonerReport);
            Assert.IsTrue(reasonerReport.EvidencesCount == 2);
        }

        [TestMethod]
        public void ShouldExecuteIndividualTypeEntailmentOnRestrictionClass()
        {
            OWLOntology ontology = new OWLOntology("ex:ont");
            ontology.Model.ClassModel.DeclareHasValueRestriction(new RDFResource("ex:hvRestriction"), new RDFResource("ex:objprop"), new RDFResource("ex:indiv2"));
            ontology.Model.PropertyModel.DeclareObjectProperty(new RDFResource("ex:objprop"));
            ontology.Data.DeclareIndividual(new RDFResource("ex:indiv1"));
            ontology.Data.DeclareIndividual(new RDFResource("ex:indiv2"));
            ontology.Data.DeclareObjectAssertion(new RDFResource("ex:indiv1"), new RDFResource("ex:objprop"), new RDFResource("ex:indiv2"));

            OWLReasonerReport reasonerReport = OWLIndividualTypeEntailmentRule.ExecuteRule(ontology, OWLOntologyLoaderOptions.DefaultOptions);

            Assert.IsNotNull(reasonerReport);
            Assert.IsTrue(reasonerReport.EvidencesCount == 1);
        }

        [TestMethod]
        public void ShouldExecuteIndividualTypeEntailmentOnCompositeClass()
        {
            OWLOntology ontology = new OWLOntology("ex:ont");
            ontology.Model.ClassModel.DeclareClass(new RDFResource("ex:simpleClass"));
            ontology.Model.ClassModel.DeclareHasValueRestriction(new RDFResource("ex:hvRestriction"), new RDFResource("ex:objprop"), new RDFResource("ex:indiv2"));
            ontology.Model.ClassModel.DeclareUnionClass(new RDFResource("ex:unionClass"), new List<RDFResource>() { new RDFResource("ex:simpleClass"), new RDFResource("ex:hvRestriction") });
            ontology.Model.PropertyModel.DeclareObjectProperty(new RDFResource("ex:objprop"));
            ontology.Data.DeclareIndividual(new RDFResource("ex:indiv1"));
            ontology.Data.DeclareIndividual(new RDFResource("ex:indiv2"));
            ontology.Data.DeclareIndividualType(new RDFResource("ex:indiv2"), new RDFResource("ex:simpleClass"));
            ontology.Data.DeclareObjectAssertion(new RDFResource("ex:indiv1"), new RDFResource("ex:objprop"), new RDFResource("ex:indiv2"));

            OWLReasonerReport reasonerReport = OWLIndividualTypeEntailmentRule.ExecuteRule(ontology, OWLOntologyLoaderOptions.DefaultOptions);

            Assert.IsNotNull(reasonerReport);
            Assert.IsTrue(reasonerReport.EvidencesCount == 3); // 2 evidences from union class, 1 evidence from restriction class
        }

        [TestMethod]
        public void ShouldExecuteIndividualTypeEntailmentOnEnumerateClass()
        {
            OWLOntology ontology = new OWLOntology("ex:ont");
            ontology.Model.ClassModel.DeclareEnumerateClass(new RDFResource("ex:enumerateClass"), new List<RDFResource>() { new RDFResource("ex:indiv1"), new RDFResource("ex:indiv2") });
            ontology.Data.DeclareIndividual(new RDFResource("ex:indiv1"));
            ontology.Data.DeclareIndividual(new RDFResource("ex:indiv2"));

            OWLReasonerReport reasonerReport = OWLIndividualTypeEntailmentRule.ExecuteRule(ontology, OWLOntologyLoaderOptions.DefaultOptions);

            Assert.IsNotNull(reasonerReport);
            Assert.IsTrue(reasonerReport.EvidencesCount == 2);
        }

        [TestMethod]
        public void ShouldExecuteIndividualTypeEntailmentViaReasoner()
        {
            OWLOntology ontology = new OWLOntology("ex:ont");
            ontology.Model.ClassModel.DeclareClass(new RDFResource("ex:class1"));
            ontology.Model.ClassModel.DeclareClass(new RDFResource("ex:class2"));
            ontology.Model.ClassModel.DeclareClass(new RDFResource("ex:class3"));
            ontology.Model.ClassModel.DeclareSubClasses(new RDFResource("ex:class2"), new RDFResource("ex:class1"));
            ontology.Model.ClassModel.DeclareEquivalentClasses(new RDFResource("ex:class2"), new RDFResource("ex:class3"));
            ontology.Data.DeclareIndividual(new RDFResource("ex:indiv"));
            ontology.Data.DeclareIndividualType(new RDFResource("ex:indiv"), new RDFResource("ex:class3"));

            OWLReasoner reasoner = new OWLReasoner().AddStandardRule(OWLSemanticsEnums.OWLReasonerStandardRules.IndividualTypeEntailment);
            OWLReasonerReport reasonerReport = reasoner.ApplyToOntology(ontology);

            Assert.IsNotNull(reasonerReport);
            Assert.IsTrue(reasonerReport.EvidencesCount == 2);
        }
        #endregion
    }
}