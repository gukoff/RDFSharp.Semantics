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
        #endregion
    }
}