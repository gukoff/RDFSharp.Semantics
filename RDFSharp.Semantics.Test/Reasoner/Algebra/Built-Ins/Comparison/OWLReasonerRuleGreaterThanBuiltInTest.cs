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
using RDFSharp.Query;
using System.Data;

namespace RDFSharp.Semantics.Reasoner.Test
{
    [TestClass]
    public class OWLReasonerRuleGreaterThanBuiltInTest
    {
        #region Tests
        [TestMethod]
        public void ShouldCreateGreaterThanBuiltInWithVariableRightArgument()
        {
            OWLReasonerRuleGreaterThanBuiltIn builtin = new OWLReasonerRuleGreaterThanBuiltIn(new RDFVariable("?L"), new RDFVariable("?R"));

            Assert.IsNotNull(builtin);
            Assert.IsNotNull(builtin.BuiltInFilter);
            Assert.IsNotNull(builtin.Predicate);
            Assert.IsTrue(builtin.Predicate.Equals(new RDFResource("swrlb:greaterThan")));
            Assert.IsNotNull(builtin.LeftArgument);
            Assert.IsTrue(builtin.LeftArgument.Equals(new RDFVariable("?L")));
            Assert.IsNotNull(builtin.RightArgument);
            Assert.IsTrue(builtin.RightArgument.Equals(new RDFVariable("?R")));
            Assert.IsTrue(builtin.IsBuiltIn);
            Assert.IsTrue(string.Equals("swrlb:greaterThan(?L,?R)", builtin.ToString()));
        }

        [TestMethod]
        public void ShouldCreateGreaterThanBuiltInWithResourceRightArgument()
        {
            OWLReasonerRuleGreaterThanBuiltIn builtin = new OWLReasonerRuleGreaterThanBuiltIn(new RDFVariable("?L"), new RDFResource("ex:res"));

            Assert.IsNotNull(builtin);
            Assert.IsNotNull(builtin.BuiltInFilter);
            Assert.IsNotNull(builtin.Predicate);
            Assert.IsTrue(builtin.Predicate.Equals(new RDFResource("swrlb:greaterThan")));
            Assert.IsNotNull(builtin.LeftArgument);
            Assert.IsTrue(builtin.LeftArgument.Equals(new RDFVariable("?L")));
            Assert.IsNotNull(builtin.RightArgument);
            Assert.IsTrue(builtin.RightArgument.Equals(new RDFResource("ex:res")));
            Assert.IsTrue(builtin.IsBuiltIn);
            Assert.IsTrue(string.Equals("swrlb:greaterThan(?L,ex:res)", builtin.ToString()));
        }

        [TestMethod]
        public void ShouldCreateGreaterThanBuiltInWithLiteralRightArgument()
        {
            OWLReasonerRuleGreaterThanBuiltIn builtin = new OWLReasonerRuleGreaterThanBuiltIn(new RDFVariable("?L"), new RDFTypedLiteral("25", RDFModelEnums.RDFDatatypes.XSD_INTEGER));

            Assert.IsNotNull(builtin);
            Assert.IsNotNull(builtin.BuiltInFilter);
            Assert.IsNotNull(builtin.Predicate);
            Assert.IsTrue(builtin.Predicate.Equals(new RDFResource("swrlb:greaterThan")));
            Assert.IsNotNull(builtin.LeftArgument);
            Assert.IsTrue(builtin.LeftArgument.Equals(new RDFVariable("?L")));
            Assert.IsNotNull(builtin.RightArgument);
            Assert.IsTrue(builtin.RightArgument.Equals(new RDFTypedLiteral("25", RDFModelEnums.RDFDatatypes.XSD_INTEGER)));
            Assert.IsTrue(builtin.IsBuiltIn);
            Assert.IsTrue(string.Equals($"swrlb:greaterThan(?L,\"25\"^^xsd:integer)", builtin.ToString()));
        }

        [TestMethod]
        public void ShouldThrowExceptionOnCreatingGreaterThanBuiltInBecauseNullLeftArgument()
            => Assert.ThrowsException<OWLSemanticsException>(() => new OWLReasonerRuleGreaterThanBuiltIn(null, new RDFVariable("?R")));

        [TestMethod]
        public void ShouldThrowExceptionOnCreatingGreaterThanBuiltInBecauseNullRightArgumentVariable()
            => Assert.ThrowsException<OWLSemanticsException>(() => new OWLReasonerRuleGreaterThanBuiltIn(new RDFVariable("?L"), null as RDFVariable));

        [TestMethod]
        public void ShouldThrowExceptionOnCreatingGreaterThanBuiltInBecauseNullRightArgumentResource()
            => Assert.ThrowsException<OWLSemanticsException>(() => new OWLReasonerRuleGreaterThanBuiltIn(new RDFVariable("?L"), null as RDFResource));

        [TestMethod]
        public void ShouldThrowExceptionOnCreatingGreaterThanBuiltInBecauseNullRightArgumentLiteral()
            => Assert.ThrowsException<OWLSemanticsException>(() => new OWLReasonerRuleGreaterThanBuiltIn(new RDFVariable("?L"), null as RDFLiteral));

        [TestMethod]
        public void ShouldEvaluateGreaterThanEqualBuiltIn()
        {
            DataTable antecedentTable = new DataTable();
            antecedentTable.Columns.Add("?C");
            antecedentTable.Rows.Add("ex:indiv1");
            antecedentTable.Rows.Add("ex:indiv2");

            OWLReasonerRuleGreaterThanBuiltIn builtin = new OWLReasonerRuleGreaterThanBuiltIn(new RDFVariable("?C"), new RDFResource("ex:indiv1"));
            DataTable result = builtin.Evaluate(antecedentTable, null);

            Assert.IsNotNull(result);
            Assert.IsTrue(result.Rows.Count == 1);
        }

        #endregion
    }
}