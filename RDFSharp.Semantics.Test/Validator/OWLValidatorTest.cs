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

namespace RDFSharp.Semantics.Validator.Test
{
    [TestClass]
    public class OWLValidatorTest
    {
        #region Tests
        [TestMethod]
        public void ShouldCreateValidator()
        {
            OWLValidator validator = new OWLValidator();

            Assert.IsNotNull(validator);
            Assert.IsNotNull(validator.StandardRules);
            Assert.IsTrue(validator.StandardRules.Count == 0);
            Assert.IsNotNull(validator.CustomRules);
            Assert.IsTrue(validator.CustomRules.Count == 0);
        }

        [TestMethod]
        public void ShouldAddStandardValidatorRule()
        {
            OWLValidator validator = new OWLValidator();
            validator.AddStandardRule(OWLSemanticsEnums.OWLValidatorStandardRules.Vocabulary_Disjointness);
            validator.AddStandardRule(OWLSemanticsEnums.OWLValidatorStandardRules.Vocabulary_Disjointness); //Will be discarded, since duplicate standard rules are not allowed

            Assert.IsNotNull(validator);
            Assert.IsNotNull(validator.StandardRules);
            Assert.IsTrue(validator.StandardRules.Count == 1);
            Assert.IsNotNull(validator.CustomRules);
            Assert.IsTrue(validator.CustomRules.Count == 0);
        }

        [TestMethod]
        public void ShouldAddCustomValidatorRule()
        {
            OWLValidatorReport CustomValidatorRule(OWLOntology ontology)
                => new OWLValidatorReport().AddEvidence(new OWLValidatorEvidence(OWLSemanticsEnums.OWLValidatorEvidenceCategory.Warning, nameof(CustomValidatorRule), "test message", "test suggestion"));

            OWLValidator validator = new OWLValidator();
            validator.AddCustomRule(new OWLValidatorRule("testRule", "this is test rule", CustomValidatorRule));

            Assert.IsNotNull(validator);
            Assert.IsNotNull(validator.StandardRules);
            Assert.IsTrue(validator.StandardRules.Count == 0);
            Assert.IsNotNull(validator.CustomRules);
            Assert.IsTrue(validator.CustomRules.Count == 1);
        }

        [TestMethod]
        public void ShouldThrowExceptionOnCreatingCUstomValidatorRuleBecauseNull()
            => Assert.ThrowsException<OWLSemanticsException>(() => new OWLValidator().AddCustomRule(null));
        #endregion
    }
}