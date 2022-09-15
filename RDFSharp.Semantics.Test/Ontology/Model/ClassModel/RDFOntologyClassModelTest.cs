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
    public class RDFOntologyClassModelTest
    {
        #region Test
        [TestMethod]
        public void ShouldCreateClassModel()
        {
            RDFOntologyClassModel classModel = new RDFOntologyClassModel();

            Assert.IsNotNull(classModel);
            Assert.IsNotNull(classModel.Classes);
            Assert.IsTrue(classModel.ClassesCount == 0);
            Assert.IsTrue(classModel.AllDisjointClassesCount == 0);
            Assert.IsTrue(classModel.CompositesCount == 0);
            Assert.IsTrue(classModel.DeprecatedClassesCount == 0);
            Assert.IsTrue(classModel.EnumeratesCount == 0);
            Assert.IsTrue(classModel.RestrictionsCount == 0);
            Assert.IsNotNull(classModel.TBoxGraph);
            Assert.IsTrue(classModel.TBoxGraph.TriplesCount == 0);
            Assert.IsNotNull(classModel.TBoxInferenceGraph);
            Assert.IsTrue(classModel.TBoxInferenceGraph.TriplesCount == 0);
            Assert.IsNotNull(classModel.TBoxVirtualGraph);
            Assert.IsTrue(classModel.TBoxVirtualGraph.TriplesCount == 0);

            int i = 0;
            IEnumerator<RDFResource> classesEnumerator = classModel.ClassesEnumerator;
            while (classesEnumerator.MoveNext())
                i++;
            Assert.IsTrue(i == 0);

            int j = 0;
            IEnumerator<RDFResource> allDisjointClassesEnumerator = classModel.AllDisjointClassesEnumerator;
            while (allDisjointClassesEnumerator.MoveNext())
                j++;
            Assert.IsTrue(j == 0);

            int m = 0;
            IEnumerator<RDFResource> compositeClassesEnumerator = classModel.CompositesEnumerator;
            while (compositeClassesEnumerator.MoveNext())
                m++;
            Assert.IsTrue(m == 0);

            int n = 0;
            IEnumerator<RDFResource> deprecatedClassesEnumerator = classModel.DeprecatedClassesEnumerator;
            while (deprecatedClassesEnumerator.MoveNext())
                n++;
            Assert.IsTrue(n == 0);

            int k = 0;
            IEnumerator<RDFResource> enumerateClassesEnumerator = classModel.EnumeratesEnumerator;
            while (enumerateClassesEnumerator.MoveNext())
                k++;
            Assert.IsTrue(k == 0);

            int r = 0;
            IEnumerator<RDFResource> restrictionClassesEnumerator = classModel.RestrictionsEnumerator;
            while (restrictionClassesEnumerator.MoveNext())
                r++;
            Assert.IsTrue(r == 0);
        }

        [TestMethod]
        public void ShouldDeclareClass()
        {
            RDFOntologyClassModel classModel = new RDFOntologyClassModel();
            classModel.DeclareClass(new RDFResource("ex:class1"));

            Assert.IsTrue(classModel.ClassesCount == 1);
            Assert.IsTrue(classModel.AllDisjointClassesCount == 0);
            Assert.IsTrue(classModel.CompositesCount == 0);
            Assert.IsTrue(classModel.DeprecatedClassesCount == 0);
            Assert.IsTrue(classModel.EnumeratesCount == 0);
            Assert.IsTrue(classModel.RestrictionsCount == 0);
            Assert.IsTrue(classModel.TBoxGraph.TriplesCount == 1);
            Assert.IsTrue(classModel.TBoxGraph.ContainsTriple(new RDFTriple(new RDFResource("ex:class1"), RDFVocabulary.RDF.TYPE, RDFVocabulary.OWL.CLASS)));
            Assert.IsTrue(classModel.TBoxInferenceGraph.TriplesCount == 0);
            Assert.IsTrue(classModel.TBoxVirtualGraph.TriplesCount == 1);
            Assert.IsTrue(classModel.TBoxVirtualGraph.ContainsTriple(new RDFTriple(new RDFResource("ex:class1"), RDFVocabulary.RDF.TYPE, RDFVocabulary.OWL.CLASS)));

            int i = 0;
            IEnumerator<RDFResource> classesEnumerator = classModel.ClassesEnumerator;
            while (classesEnumerator.MoveNext())
            {
                Assert.IsTrue(classesEnumerator.Current.Equals(new RDFResource("ex:class1")));
                i++;
            }                
            Assert.IsTrue(i == 1);
        }

        [TestMethod]
        public void ShouldDeclareDeprecatedClass()
        {
            RDFOntologyClassModel classModel = new RDFOntologyClassModel();
            classModel.DeclareClass(new RDFResource("ex:class1"), new RDFOntologyClassBehavior() { Deprecated = true });

            Assert.IsTrue(classModel.ClassesCount == 1);
            Assert.IsTrue(classModel.AllDisjointClassesCount == 0);
            Assert.IsTrue(classModel.CompositesCount == 0);
            Assert.IsTrue(classModel.DeprecatedClassesCount == 1);
            Assert.IsTrue(classModel.EnumeratesCount == 0);
            Assert.IsTrue(classModel.RestrictionsCount == 0);
            Assert.IsTrue(classModel.TBoxGraph.TriplesCount == 2);
            Assert.IsTrue(classModel.TBoxGraph.ContainsTriple(new RDFTriple(new RDFResource("ex:class1"), RDFVocabulary.RDF.TYPE, RDFVocabulary.OWL.CLASS)));
            Assert.IsTrue(classModel.TBoxGraph.ContainsTriple(new RDFTriple(new RDFResource("ex:class1"), RDFVocabulary.RDF.TYPE, RDFVocabulary.OWL.DEPRECATED_CLASS)));
            Assert.IsTrue(classModel.TBoxInferenceGraph.TriplesCount == 0);
            Assert.IsTrue(classModel.TBoxVirtualGraph.TriplesCount == 2);
            Assert.IsTrue(classModel.TBoxVirtualGraph.ContainsTriple(new RDFTriple(new RDFResource("ex:class1"), RDFVocabulary.RDF.TYPE, RDFVocabulary.OWL.CLASS)));
            Assert.IsTrue(classModel.TBoxVirtualGraph.ContainsTriple(new RDFTriple(new RDFResource("ex:class1"), RDFVocabulary.RDF.TYPE, RDFVocabulary.OWL.DEPRECATED_CLASS)));

            int i = 0;
            IEnumerator<RDFResource> classesEnumerator = classModel.ClassesEnumerator;
            while (classesEnumerator.MoveNext())
            {
                Assert.IsTrue(classesEnumerator.Current.Equals(new RDFResource("ex:class1")));
                i++;
            }
            Assert.IsTrue(i == 1);

            int j = 0;
            IEnumerator<RDFResource> deprecatedClassesEnumerator = classModel.DeprecatedClassesEnumerator;
            while (deprecatedClassesEnumerator.MoveNext())
            {
                Assert.IsTrue(deprecatedClassesEnumerator.Current.Equals(new RDFResource("ex:class1")));
                j++;
            }
            Assert.IsTrue(j == 1);
        }

        [TestMethod]
        public void ShouldThrowExceptionOnDeclaringClassBecauseNull()
            => Assert.ThrowsException<RDFSemanticsException>(() => new RDFOntologyClassModel().DeclareClass(null));
        #endregion
    }
}