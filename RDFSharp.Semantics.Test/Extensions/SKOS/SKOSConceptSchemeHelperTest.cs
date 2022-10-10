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
using RDFSharp.Semantics.Extensions.SKOS;
using System.Collections.Generic;

namespace RDFSharp.Semantics.Test
{
    [TestClass]
    public class SKOSConceptSchemeHelperTest
    {
        #region Tests
        [TestMethod]
        public void ShouldCheckHasConcept()
        {
            SKOSConceptScheme conceptScheme = new SKOSConceptScheme("ex:conceptScheme");
            conceptScheme.DeclareConcept(new RDFResource("ex:concept1"));
            conceptScheme.DeclareConcept(new RDFResource("ex:concept2"));

            Assert.IsTrue(conceptScheme.CheckHasConcept(new RDFResource("ex:concept1")));
            Assert.IsTrue(conceptScheme.CheckHasConcept(new RDFResource("ex:concept2")));
        }

        [TestMethod]
        public void ShouldCheckHasNotConcept()
        {
            SKOSConceptScheme conceptSchemeNULL = null;
            SKOSConceptScheme conceptSchemeEMPTY = new SKOSConceptScheme("ex:conceptSchemeEmpty");
            SKOSConceptScheme conceptScheme = new SKOSConceptScheme("ex:conceptScheme");
            conceptScheme.DeclareConcept(new RDFResource("ex:concept1"));
            conceptScheme.DeclareConcept(new RDFResource("ex:concept2"));

            Assert.IsFalse(conceptScheme.CheckHasConcept(new RDFResource("ex:concept3")));
            Assert.IsFalse(conceptScheme.CheckHasConcept(null));            
            Assert.IsFalse(conceptSchemeNULL.CheckHasConcept(new RDFResource("ex:concept1")));
            Assert.IsFalse(conceptSchemeEMPTY.CheckHasConcept(new RDFResource("ex:concept1")));
        }

        [TestMethod]
        public void ShouldCheckHasCollection()
        {
            SKOSConceptScheme conceptScheme = new SKOSConceptScheme("ex:conceptScheme");
            conceptScheme.DeclareCollection(new RDFResource("ex:collection"), new List<RDFResource>() { 
                new RDFResource("ex:concept1"), new RDFResource("ex:concept2") });

            Assert.IsTrue(conceptScheme.CheckHasCollection(new RDFResource("ex:collection")));
        }

        [TestMethod]
        public void ShouldCheckHasNotCollection()
        {
            SKOSConceptScheme conceptSchemeNULL = null;
            SKOSConceptScheme conceptSchemeEMPTY = new SKOSConceptScheme("ex:conceptSchemeEmpty");
            SKOSConceptScheme conceptScheme = new SKOSConceptScheme("ex:conceptScheme");
            conceptScheme.DeclareCollection(new RDFResource("ex:collection"), new List<RDFResource>() {
                new RDFResource("ex:concept1"), new RDFResource("ex:concept2") });

            Assert.IsFalse(conceptScheme.CheckHasCollection(new RDFResource("ex:collection2")));
            Assert.IsFalse(conceptScheme.CheckHasCollection(null));
            Assert.IsFalse(conceptSchemeNULL.CheckHasCollection(new RDFResource("ex:concept1")));
            Assert.IsFalse(conceptSchemeEMPTY.CheckHasCollection(new RDFResource("ex:concept1")));
        }
        #endregion
    }
}