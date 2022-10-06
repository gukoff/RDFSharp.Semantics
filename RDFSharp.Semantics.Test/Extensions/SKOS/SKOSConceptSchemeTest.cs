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
    public class SKOSConceptSchemeTest
    {
        #region Tests
        [TestMethod]
        public void ShouldCreateConceptScheme()
        {
            SKOSConceptScheme conceptScheme = new SKOSConceptScheme("ex:conceptScheme");

            Assert.IsNotNull(conceptScheme);
            Assert.IsNotNull(conceptScheme.Ontology);

            //Test initialization of SKOS knowledge
            Assert.IsTrue(conceptScheme.Ontology.URI.Equals(conceptScheme.URI));
            Assert.IsTrue(conceptScheme.Ontology.Model.ClassModel.ClassesCount == 8);
            Assert.IsTrue(conceptScheme.Ontology.Model.PropertyModel.PropertiesCount == 33);
            Assert.IsTrue(conceptScheme.Ontology.Data.IndividualsCount == 1);

            //Test counters and enumerators
            Assert.IsTrue(conceptScheme.ConceptsCount == 0);
            int i = 0;
            IEnumerator<RDFResource> conceptsEnumerator = conceptScheme.ConceptsEnumerator;
            while (conceptsEnumerator.MoveNext()) i++;
            Assert.IsTrue(i == 0);
            Assert.IsTrue(conceptScheme.CollectionsCount == 0);
            int j = 0;
            IEnumerator<RDFResource> collectionsEnumerator = conceptScheme.CollectionsEnumerator;
            while (collectionsEnumerator.MoveNext()) j++;
            Assert.IsTrue(j == 0);
            Assert.IsTrue(conceptScheme.OrderedCollectionsCount == 0);
            int k = 0;
            IEnumerator<RDFResource> orderedCollectionsEnumerator = conceptScheme.OrderedCollectionsEnumerator;
            while (orderedCollectionsEnumerator.MoveNext()) k++;
            Assert.IsTrue(k == 0);
            Assert.IsTrue(conceptScheme.LabelsCount == 0);
            int l = 0;
            IEnumerator<RDFResource> labelsEnumerator = conceptScheme.LabelsEnumerator;
            while (labelsEnumerator.MoveNext()) l++;
            Assert.IsTrue(l == 0);
        }
        #endregion
    }
}