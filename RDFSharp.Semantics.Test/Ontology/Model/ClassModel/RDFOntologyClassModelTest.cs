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

        [TestMethod]
        public void ShouldDeclareAllValuesFromRestriction()
        {
            RDFOntologyClassModel classModel = new RDFOntologyClassModel();
            classModel.DeclareAllValuesFromRestriction(new RDFResource("ex:avRestr"), new RDFResource("ex:onProp"), new RDFResource("ex:avClass"));

            Assert.IsTrue(classModel.ClassesCount == 1);
            Assert.IsTrue(classModel.AllDisjointClassesCount == 0);
            Assert.IsTrue(classModel.CompositesCount == 0);
            Assert.IsTrue(classModel.DeprecatedClassesCount == 0);
            Assert.IsTrue(classModel.EnumeratesCount == 0);
            Assert.IsTrue(classModel.RestrictionsCount == 1);
            Assert.IsTrue(classModel.TBoxGraph.TriplesCount == 4);
            Assert.IsTrue(classModel.TBoxGraph.ContainsTriple(new RDFTriple(new RDFResource("ex:avRestr"), RDFVocabulary.RDF.TYPE, RDFVocabulary.OWL.CLASS)));
            Assert.IsTrue(classModel.TBoxGraph.ContainsTriple(new RDFTriple(new RDFResource("ex:avRestr"), RDFVocabulary.RDF.TYPE, RDFVocabulary.OWL.RESTRICTION)));
            Assert.IsTrue(classModel.TBoxGraph.ContainsTriple(new RDFTriple(new RDFResource("ex:avRestr"), RDFVocabulary.OWL.ON_PROPERTY, new RDFResource("ex:onProp"))));
            Assert.IsTrue(classModel.TBoxGraph.ContainsTriple(new RDFTriple(new RDFResource("ex:avRestr"), RDFVocabulary.OWL.ALL_VALUES_FROM, new RDFResource("ex:avClass"))));
            Assert.IsTrue(classModel.TBoxInferenceGraph.TriplesCount == 0);
            Assert.IsTrue(classModel.TBoxVirtualGraph.TriplesCount == 4);
            Assert.IsTrue(classModel.TBoxVirtualGraph.ContainsTriple(new RDFTriple(new RDFResource("ex:avRestr"), RDFVocabulary.RDF.TYPE, RDFVocabulary.OWL.CLASS)));
            Assert.IsTrue(classModel.TBoxVirtualGraph.ContainsTriple(new RDFTriple(new RDFResource("ex:avRestr"), RDFVocabulary.RDF.TYPE, RDFVocabulary.OWL.RESTRICTION)));
            Assert.IsTrue(classModel.TBoxVirtualGraph.ContainsTriple(new RDFTriple(new RDFResource("ex:avRestr"), RDFVocabulary.OWL.ON_PROPERTY, new RDFResource("ex:onProp"))));
            Assert.IsTrue(classModel.TBoxVirtualGraph.ContainsTriple(new RDFTriple(new RDFResource("ex:avRestr"), RDFVocabulary.OWL.ALL_VALUES_FROM, new RDFResource("ex:avClass"))));

            int i = 0;
            IEnumerator<RDFResource> restrictionsEnumerator = classModel.RestrictionsEnumerator;
            while (restrictionsEnumerator.MoveNext())
            {
                Assert.IsTrue(restrictionsEnumerator.Current.Equals(new RDFResource("ex:avRestr")));
                i++;
            }
            Assert.IsTrue(i == 1);
        }

        [TestMethod]
        public void ShouldThrowExceptionOnDeclaringAllValuesFromRestrictionBecauseNull()
            => Assert.ThrowsException<RDFSemanticsException>(() => new RDFOntologyClassModel().DeclareAllValuesFromRestriction(null, new RDFResource("ex:onProp"), new RDFResource("ex:avClass")));

        [TestMethod]
        public void ShouldDeclareSomeValuesFromRestriction()
        {
            RDFOntologyClassModel classModel = new RDFOntologyClassModel();
            classModel.DeclareSomeValuesFromRestriction(new RDFResource("ex:svRestr"), new RDFResource("ex:onProp"), new RDFResource("ex:svClass"));

            Assert.IsTrue(classModel.ClassesCount == 1);
            Assert.IsTrue(classModel.AllDisjointClassesCount == 0);
            Assert.IsTrue(classModel.CompositesCount == 0);
            Assert.IsTrue(classModel.DeprecatedClassesCount == 0);
            Assert.IsTrue(classModel.EnumeratesCount == 0);
            Assert.IsTrue(classModel.RestrictionsCount == 1);
            Assert.IsTrue(classModel.TBoxGraph.TriplesCount == 4);
            Assert.IsTrue(classModel.TBoxGraph.ContainsTriple(new RDFTriple(new RDFResource("ex:svRestr"), RDFVocabulary.RDF.TYPE, RDFVocabulary.OWL.CLASS)));
            Assert.IsTrue(classModel.TBoxGraph.ContainsTriple(new RDFTriple(new RDFResource("ex:svRestr"), RDFVocabulary.RDF.TYPE, RDFVocabulary.OWL.RESTRICTION)));
            Assert.IsTrue(classModel.TBoxGraph.ContainsTriple(new RDFTriple(new RDFResource("ex:svRestr"), RDFVocabulary.OWL.ON_PROPERTY, new RDFResource("ex:onProp"))));
            Assert.IsTrue(classModel.TBoxGraph.ContainsTriple(new RDFTriple(new RDFResource("ex:svRestr"), RDFVocabulary.OWL.SOME_VALUES_FROM, new RDFResource("ex:svClass"))));
            Assert.IsTrue(classModel.TBoxInferenceGraph.TriplesCount == 0);
            Assert.IsTrue(classModel.TBoxVirtualGraph.TriplesCount == 4);
            Assert.IsTrue(classModel.TBoxVirtualGraph.ContainsTriple(new RDFTriple(new RDFResource("ex:svRestr"), RDFVocabulary.RDF.TYPE, RDFVocabulary.OWL.CLASS)));
            Assert.IsTrue(classModel.TBoxVirtualGraph.ContainsTriple(new RDFTriple(new RDFResource("ex:svRestr"), RDFVocabulary.RDF.TYPE, RDFVocabulary.OWL.RESTRICTION)));
            Assert.IsTrue(classModel.TBoxVirtualGraph.ContainsTriple(new RDFTriple(new RDFResource("ex:svRestr"), RDFVocabulary.OWL.ON_PROPERTY, new RDFResource("ex:onProp"))));
            Assert.IsTrue(classModel.TBoxVirtualGraph.ContainsTriple(new RDFTriple(new RDFResource("ex:svRestr"), RDFVocabulary.OWL.SOME_VALUES_FROM, new RDFResource("ex:svClass"))));

            int i = 0;
            IEnumerator<RDFResource> restrictionsEnumerator = classModel.RestrictionsEnumerator;
            while (restrictionsEnumerator.MoveNext())
            {
                Assert.IsTrue(restrictionsEnumerator.Current.Equals(new RDFResource("ex:svRestr")));
                i++;
            }
            Assert.IsTrue(i == 1);
        }

        [TestMethod]
        public void ShouldThrowExceptionOnDeclaringSomeValuesFromRestrictionBecauseNull()
            => Assert.ThrowsException<RDFSemanticsException>(() => new RDFOntologyClassModel().DeclareSomeValuesFromRestriction(null, new RDFResource("ex:onProp"), new RDFResource("ex:svClass")));

        [TestMethod]
        public void ShouldDeclareHasSelfTrueRestriction()
        {
            RDFOntologyClassModel classModel = new RDFOntologyClassModel();
            classModel.DeclareHasSelfRestriction(new RDFResource("ex:hsRestr"), new RDFResource("ex:onProp"), true);

            Assert.IsTrue(classModel.ClassesCount == 1);
            Assert.IsTrue(classModel.AllDisjointClassesCount == 0);
            Assert.IsTrue(classModel.CompositesCount == 0);
            Assert.IsTrue(classModel.DeprecatedClassesCount == 0);
            Assert.IsTrue(classModel.EnumeratesCount == 0);
            Assert.IsTrue(classModel.RestrictionsCount == 1);
            Assert.IsTrue(classModel.TBoxGraph.TriplesCount == 4);
            Assert.IsTrue(classModel.TBoxGraph.ContainsTriple(new RDFTriple(new RDFResource("ex:hsRestr"), RDFVocabulary.RDF.TYPE, RDFVocabulary.OWL.CLASS)));
            Assert.IsTrue(classModel.TBoxGraph.ContainsTriple(new RDFTriple(new RDFResource("ex:hsRestr"), RDFVocabulary.RDF.TYPE, RDFVocabulary.OWL.RESTRICTION)));
            Assert.IsTrue(classModel.TBoxGraph.ContainsTriple(new RDFTriple(new RDFResource("ex:hsRestr"), RDFVocabulary.OWL.ON_PROPERTY, new RDFResource("ex:onProp"))));
            Assert.IsTrue(classModel.TBoxGraph.ContainsTriple(new RDFTriple(new RDFResource("ex:hsRestr"), RDFVocabulary.OWL.HAS_SELF, RDFTypedLiteral.True)));
            Assert.IsTrue(classModel.TBoxInferenceGraph.TriplesCount == 0);
            Assert.IsTrue(classModel.TBoxVirtualGraph.TriplesCount == 4);
            Assert.IsTrue(classModel.TBoxVirtualGraph.ContainsTriple(new RDFTriple(new RDFResource("ex:hsRestr"), RDFVocabulary.RDF.TYPE, RDFVocabulary.OWL.CLASS)));
            Assert.IsTrue(classModel.TBoxVirtualGraph.ContainsTriple(new RDFTriple(new RDFResource("ex:hsRestr"), RDFVocabulary.RDF.TYPE, RDFVocabulary.OWL.RESTRICTION)));
            Assert.IsTrue(classModel.TBoxVirtualGraph.ContainsTriple(new RDFTriple(new RDFResource("ex:hsRestr"), RDFVocabulary.OWL.ON_PROPERTY, new RDFResource("ex:onProp"))));
            Assert.IsTrue(classModel.TBoxVirtualGraph.ContainsTriple(new RDFTriple(new RDFResource("ex:hsRestr"), RDFVocabulary.OWL.HAS_SELF, RDFTypedLiteral.True)));

            int i = 0;
            IEnumerator<RDFResource> restrictionsEnumerator = classModel.RestrictionsEnumerator;
            while (restrictionsEnumerator.MoveNext())
            {
                Assert.IsTrue(restrictionsEnumerator.Current.Equals(new RDFResource("ex:hsRestr")));
                i++;
            }
            Assert.IsTrue(i == 1);
        }

        [TestMethod]
        public void ShouldDeclareHasSelfFalseRestriction()
        {
            RDFOntologyClassModel classModel = new RDFOntologyClassModel();
            classModel.DeclareHasSelfRestriction(new RDFResource("ex:hsRestr"), new RDFResource("ex:onProp"), false);

            Assert.IsTrue(classModel.ClassesCount == 1);
            Assert.IsTrue(classModel.AllDisjointClassesCount == 0);
            Assert.IsTrue(classModel.CompositesCount == 0);
            Assert.IsTrue(classModel.DeprecatedClassesCount == 0);
            Assert.IsTrue(classModel.EnumeratesCount == 0);
            Assert.IsTrue(classModel.RestrictionsCount == 1);
            Assert.IsTrue(classModel.TBoxGraph.TriplesCount == 4);
            Assert.IsTrue(classModel.TBoxGraph.ContainsTriple(new RDFTriple(new RDFResource("ex:hsRestr"), RDFVocabulary.RDF.TYPE, RDFVocabulary.OWL.CLASS)));
            Assert.IsTrue(classModel.TBoxGraph.ContainsTriple(new RDFTriple(new RDFResource("ex:hsRestr"), RDFVocabulary.RDF.TYPE, RDFVocabulary.OWL.RESTRICTION)));
            Assert.IsTrue(classModel.TBoxGraph.ContainsTriple(new RDFTriple(new RDFResource("ex:hsRestr"), RDFVocabulary.OWL.ON_PROPERTY, new RDFResource("ex:onProp"))));
            Assert.IsTrue(classModel.TBoxGraph.ContainsTriple(new RDFTriple(new RDFResource("ex:hsRestr"), RDFVocabulary.OWL.HAS_SELF, RDFTypedLiteral.False)));
            Assert.IsTrue(classModel.TBoxInferenceGraph.TriplesCount == 0);
            Assert.IsTrue(classModel.TBoxVirtualGraph.TriplesCount == 4);
            Assert.IsTrue(classModel.TBoxVirtualGraph.ContainsTriple(new RDFTriple(new RDFResource("ex:hsRestr"), RDFVocabulary.RDF.TYPE, RDFVocabulary.OWL.CLASS)));
            Assert.IsTrue(classModel.TBoxVirtualGraph.ContainsTriple(new RDFTriple(new RDFResource("ex:hsRestr"), RDFVocabulary.RDF.TYPE, RDFVocabulary.OWL.RESTRICTION)));
            Assert.IsTrue(classModel.TBoxVirtualGraph.ContainsTriple(new RDFTriple(new RDFResource("ex:hsRestr"), RDFVocabulary.OWL.ON_PROPERTY, new RDFResource("ex:onProp"))));
            Assert.IsTrue(classModel.TBoxVirtualGraph.ContainsTriple(new RDFTriple(new RDFResource("ex:hsRestr"), RDFVocabulary.OWL.HAS_SELF, RDFTypedLiteral.False)));

            int i = 0;
            IEnumerator<RDFResource> restrictionsEnumerator = classModel.RestrictionsEnumerator;
            while (restrictionsEnumerator.MoveNext())
            {
                Assert.IsTrue(restrictionsEnumerator.Current.Equals(new RDFResource("ex:hsRestr")));
                i++;
            }
            Assert.IsTrue(i == 1);
        }

        [TestMethod]
        public void ShouldDeclareHasValueResourceRestriction()
        {
            RDFOntologyClassModel classModel = new RDFOntologyClassModel();
            classModel.DeclareHasValueRestriction(new RDFResource("ex:hvRestr"), new RDFResource("ex:onProp"), new RDFResource("ex:val"));

            Assert.IsTrue(classModel.ClassesCount == 1);
            Assert.IsTrue(classModel.AllDisjointClassesCount == 0);
            Assert.IsTrue(classModel.CompositesCount == 0);
            Assert.IsTrue(classModel.DeprecatedClassesCount == 0);
            Assert.IsTrue(classModel.EnumeratesCount == 0);
            Assert.IsTrue(classModel.RestrictionsCount == 1);
            Assert.IsTrue(classModel.TBoxGraph.TriplesCount == 4);
            Assert.IsTrue(classModel.TBoxGraph.ContainsTriple(new RDFTriple(new RDFResource("ex:hvRestr"), RDFVocabulary.RDF.TYPE, RDFVocabulary.OWL.CLASS)));
            Assert.IsTrue(classModel.TBoxGraph.ContainsTriple(new RDFTriple(new RDFResource("ex:hvRestr"), RDFVocabulary.RDF.TYPE, RDFVocabulary.OWL.RESTRICTION)));
            Assert.IsTrue(classModel.TBoxGraph.ContainsTriple(new RDFTriple(new RDFResource("ex:hvRestr"), RDFVocabulary.OWL.ON_PROPERTY, new RDFResource("ex:onProp"))));
            Assert.IsTrue(classModel.TBoxGraph.ContainsTriple(new RDFTriple(new RDFResource("ex:hvRestr"), RDFVocabulary.OWL.HAS_VALUE, new RDFResource("ex:val"))));
            Assert.IsTrue(classModel.TBoxInferenceGraph.TriplesCount == 0);
            Assert.IsTrue(classModel.TBoxVirtualGraph.TriplesCount == 4);
            Assert.IsTrue(classModel.TBoxVirtualGraph.ContainsTriple(new RDFTriple(new RDFResource("ex:hvRestr"), RDFVocabulary.RDF.TYPE, RDFVocabulary.OWL.CLASS)));
            Assert.IsTrue(classModel.TBoxVirtualGraph.ContainsTriple(new RDFTriple(new RDFResource("ex:hvRestr"), RDFVocabulary.RDF.TYPE, RDFVocabulary.OWL.RESTRICTION)));
            Assert.IsTrue(classModel.TBoxVirtualGraph.ContainsTriple(new RDFTriple(new RDFResource("ex:hvRestr"), RDFVocabulary.OWL.ON_PROPERTY, new RDFResource("ex:onProp"))));
            Assert.IsTrue(classModel.TBoxVirtualGraph.ContainsTriple(new RDFTriple(new RDFResource("ex:hvRestr"), RDFVocabulary.OWL.HAS_VALUE, new RDFResource("ex:val"))));

            int i = 0;
            IEnumerator<RDFResource> restrictionsEnumerator = classModel.RestrictionsEnumerator;
            while (restrictionsEnumerator.MoveNext())
            {
                Assert.IsTrue(restrictionsEnumerator.Current.Equals(new RDFResource("ex:hvRestr")));
                i++;
            }
            Assert.IsTrue(i == 1);
        }

        [TestMethod]
        public void ShouldThrowExceptionOnDeclaringHasValueRestrictionBecauseNull()
            => Assert.ThrowsException<RDFSemanticsException>(() => new RDFOntologyClassModel().DeclareHasValueRestriction(new RDFResource("ex:hvRestr"), new RDFResource("ex:onProp"), null as RDFResource));

        [TestMethod]
        public void ShouldDeclareHasValueLiteralRestriction()
        {
            RDFOntologyClassModel classModel = new RDFOntologyClassModel();
            classModel.DeclareHasValueRestriction(new RDFResource("ex:hvRestr"), new RDFResource("ex:onProp"), new RDFPlainLiteral("val"));

            Assert.IsTrue(classModel.ClassesCount == 1);
            Assert.IsTrue(classModel.AllDisjointClassesCount == 0);
            Assert.IsTrue(classModel.CompositesCount == 0);
            Assert.IsTrue(classModel.DeprecatedClassesCount == 0);
            Assert.IsTrue(classModel.EnumeratesCount == 0);
            Assert.IsTrue(classModel.RestrictionsCount == 1);
            Assert.IsTrue(classModel.TBoxGraph.TriplesCount == 4);
            Assert.IsTrue(classModel.TBoxGraph.ContainsTriple(new RDFTriple(new RDFResource("ex:hvRestr"), RDFVocabulary.RDF.TYPE, RDFVocabulary.OWL.CLASS)));
            Assert.IsTrue(classModel.TBoxGraph.ContainsTriple(new RDFTriple(new RDFResource("ex:hvRestr"), RDFVocabulary.RDF.TYPE, RDFVocabulary.OWL.RESTRICTION)));
            Assert.IsTrue(classModel.TBoxGraph.ContainsTriple(new RDFTriple(new RDFResource("ex:hvRestr"), RDFVocabulary.OWL.ON_PROPERTY, new RDFResource("ex:onProp"))));
            Assert.IsTrue(classModel.TBoxGraph.ContainsTriple(new RDFTriple(new RDFResource("ex:hvRestr"), RDFVocabulary.OWL.HAS_VALUE, new RDFPlainLiteral("val"))));
            Assert.IsTrue(classModel.TBoxInferenceGraph.TriplesCount == 0);
            Assert.IsTrue(classModel.TBoxVirtualGraph.TriplesCount == 4);
            Assert.IsTrue(classModel.TBoxVirtualGraph.ContainsTriple(new RDFTriple(new RDFResource("ex:hvRestr"), RDFVocabulary.RDF.TYPE, RDFVocabulary.OWL.CLASS)));
            Assert.IsTrue(classModel.TBoxVirtualGraph.ContainsTriple(new RDFTriple(new RDFResource("ex:hvRestr"), RDFVocabulary.RDF.TYPE, RDFVocabulary.OWL.RESTRICTION)));
            Assert.IsTrue(classModel.TBoxVirtualGraph.ContainsTriple(new RDFTriple(new RDFResource("ex:hvRestr"), RDFVocabulary.OWL.ON_PROPERTY, new RDFResource("ex:onProp"))));
            Assert.IsTrue(classModel.TBoxVirtualGraph.ContainsTriple(new RDFTriple(new RDFResource("ex:hvRestr"), RDFVocabulary.OWL.HAS_VALUE, new RDFPlainLiteral("val"))));

            int i = 0;
            IEnumerator<RDFResource> restrictionsEnumerator = classModel.RestrictionsEnumerator;
            while (restrictionsEnumerator.MoveNext())
            {
                Assert.IsTrue(restrictionsEnumerator.Current.Equals(new RDFResource("ex:hvRestr")));
                i++;
            }
            Assert.IsTrue(i == 1);
        }

        [TestMethod]
        public void ShouldThrowExceptionOnDeclaringHasValueLiteralRestrictionBecauseNull()
            => Assert.ThrowsException<RDFSemanticsException>(() => new RDFOntologyClassModel().DeclareHasValueRestriction(new RDFResource("ex:hvRestr"), new RDFResource("ex:onProp"), null as RDFLiteral));

        [TestMethod]
        public void ShouldDeclareCardinalityRestriction()
        {
            RDFOntologyClassModel classModel = new RDFOntologyClassModel();
            classModel.DeclareCardinalityRestriction(new RDFResource("ex:cRestr"), new RDFResource("ex:onProp"), 1);

            Assert.IsTrue(classModel.ClassesCount == 1);
            Assert.IsTrue(classModel.AllDisjointClassesCount == 0);
            Assert.IsTrue(classModel.CompositesCount == 0);
            Assert.IsTrue(classModel.DeprecatedClassesCount == 0);
            Assert.IsTrue(classModel.EnumeratesCount == 0);
            Assert.IsTrue(classModel.RestrictionsCount == 1);
            Assert.IsTrue(classModel.TBoxGraph.TriplesCount == 4);
            Assert.IsTrue(classModel.TBoxGraph.ContainsTriple(new RDFTriple(new RDFResource("ex:cRestr"), RDFVocabulary.RDF.TYPE, RDFVocabulary.OWL.CLASS)));
            Assert.IsTrue(classModel.TBoxGraph.ContainsTriple(new RDFTriple(new RDFResource("ex:cRestr"), RDFVocabulary.RDF.TYPE, RDFVocabulary.OWL.RESTRICTION)));
            Assert.IsTrue(classModel.TBoxGraph.ContainsTriple(new RDFTriple(new RDFResource("ex:cRestr"), RDFVocabulary.OWL.ON_PROPERTY, new RDFResource("ex:onProp"))));
            Assert.IsTrue(classModel.TBoxGraph.ContainsTriple(new RDFTriple(new RDFResource("ex:cRestr"), RDFVocabulary.OWL.CARDINALITY, new RDFTypedLiteral("1", RDFModelEnums.RDFDatatypes.XSD_NONNEGATIVEINTEGER))));
            Assert.IsTrue(classModel.TBoxInferenceGraph.TriplesCount == 0);
            Assert.IsTrue(classModel.TBoxVirtualGraph.TriplesCount == 4);
            Assert.IsTrue(classModel.TBoxVirtualGraph.ContainsTriple(new RDFTriple(new RDFResource("ex:cRestr"), RDFVocabulary.RDF.TYPE, RDFVocabulary.OWL.CLASS)));
            Assert.IsTrue(classModel.TBoxVirtualGraph.ContainsTriple(new RDFTriple(new RDFResource("ex:cRestr"), RDFVocabulary.RDF.TYPE, RDFVocabulary.OWL.RESTRICTION)));
            Assert.IsTrue(classModel.TBoxVirtualGraph.ContainsTriple(new RDFTriple(new RDFResource("ex:cRestr"), RDFVocabulary.OWL.ON_PROPERTY, new RDFResource("ex:onProp"))));
            Assert.IsTrue(classModel.TBoxVirtualGraph.ContainsTriple(new RDFTriple(new RDFResource("ex:cRestr"), RDFVocabulary.OWL.CARDINALITY, new RDFTypedLiteral("1", RDFModelEnums.RDFDatatypes.XSD_NONNEGATIVEINTEGER))));

            int i = 0;
            IEnumerator<RDFResource> restrictionsEnumerator = classModel.RestrictionsEnumerator;
            while (restrictionsEnumerator.MoveNext())
            {
                Assert.IsTrue(restrictionsEnumerator.Current.Equals(new RDFResource("ex:cRestr")));
                i++;
            }
            Assert.IsTrue(i == 1);
        }

        [TestMethod]
        public void ShouldThrowExceptionOnDeclaringCardinalityRestrictionBecauseZero()
            => Assert.ThrowsException<RDFSemanticsException>(() => new RDFOntologyClassModel().DeclareCardinalityRestriction(new RDFResource("ex:cRestr"), new RDFResource("ex:onProp"), 0));

        [TestMethod]
        public void ShouldDeclareMinCardinalityRestriction()
        {
            RDFOntologyClassModel classModel = new RDFOntologyClassModel();
            classModel.DeclareMinCardinalityRestriction(new RDFResource("ex:cRestr"), new RDFResource("ex:onProp"), 1);

            Assert.IsTrue(classModel.ClassesCount == 1);
            Assert.IsTrue(classModel.AllDisjointClassesCount == 0);
            Assert.IsTrue(classModel.CompositesCount == 0);
            Assert.IsTrue(classModel.DeprecatedClassesCount == 0);
            Assert.IsTrue(classModel.EnumeratesCount == 0);
            Assert.IsTrue(classModel.RestrictionsCount == 1);
            Assert.IsTrue(classModel.TBoxGraph.TriplesCount == 4);
            Assert.IsTrue(classModel.TBoxGraph.ContainsTriple(new RDFTriple(new RDFResource("ex:cRestr"), RDFVocabulary.RDF.TYPE, RDFVocabulary.OWL.CLASS)));
            Assert.IsTrue(classModel.TBoxGraph.ContainsTriple(new RDFTriple(new RDFResource("ex:cRestr"), RDFVocabulary.RDF.TYPE, RDFVocabulary.OWL.RESTRICTION)));
            Assert.IsTrue(classModel.TBoxGraph.ContainsTriple(new RDFTriple(new RDFResource("ex:cRestr"), RDFVocabulary.OWL.ON_PROPERTY, new RDFResource("ex:onProp"))));
            Assert.IsTrue(classModel.TBoxGraph.ContainsTriple(new RDFTriple(new RDFResource("ex:cRestr"), RDFVocabulary.OWL.MIN_CARDINALITY, new RDFTypedLiteral("1", RDFModelEnums.RDFDatatypes.XSD_NONNEGATIVEINTEGER))));
            Assert.IsTrue(classModel.TBoxInferenceGraph.TriplesCount == 0);
            Assert.IsTrue(classModel.TBoxVirtualGraph.TriplesCount == 4);
            Assert.IsTrue(classModel.TBoxVirtualGraph.ContainsTriple(new RDFTriple(new RDFResource("ex:cRestr"), RDFVocabulary.RDF.TYPE, RDFVocabulary.OWL.CLASS)));
            Assert.IsTrue(classModel.TBoxVirtualGraph.ContainsTriple(new RDFTriple(new RDFResource("ex:cRestr"), RDFVocabulary.RDF.TYPE, RDFVocabulary.OWL.RESTRICTION)));
            Assert.IsTrue(classModel.TBoxVirtualGraph.ContainsTriple(new RDFTriple(new RDFResource("ex:cRestr"), RDFVocabulary.OWL.ON_PROPERTY, new RDFResource("ex:onProp"))));
            Assert.IsTrue(classModel.TBoxVirtualGraph.ContainsTriple(new RDFTriple(new RDFResource("ex:cRestr"), RDFVocabulary.OWL.MIN_CARDINALITY, new RDFTypedLiteral("1", RDFModelEnums.RDFDatatypes.XSD_NONNEGATIVEINTEGER))));

            int i = 0;
            IEnumerator<RDFResource> restrictionsEnumerator = classModel.RestrictionsEnumerator;
            while (restrictionsEnumerator.MoveNext())
            {
                Assert.IsTrue(restrictionsEnumerator.Current.Equals(new RDFResource("ex:cRestr")));
                i++;
            }
            Assert.IsTrue(i == 1);
        }

        [TestMethod]
        public void ShouldThrowExceptionOnDeclaringMinCardinalityRestrictionBecauseZero()
            => Assert.ThrowsException<RDFSemanticsException>(() => new RDFOntologyClassModel().DeclareMinCardinalityRestriction(new RDFResource("ex:cRestr"), new RDFResource("ex:onProp"), 0));

        [TestMethod]
        public void ShouldDeclareMaxCardinalityRestriction()
        {
            RDFOntologyClassModel classModel = new RDFOntologyClassModel();
            classModel.DeclareMaxCardinalityRestriction(new RDFResource("ex:cRestr"), new RDFResource("ex:onProp"), 1);

            Assert.IsTrue(classModel.ClassesCount == 1);
            Assert.IsTrue(classModel.AllDisjointClassesCount == 0);
            Assert.IsTrue(classModel.CompositesCount == 0);
            Assert.IsTrue(classModel.DeprecatedClassesCount == 0);
            Assert.IsTrue(classModel.EnumeratesCount == 0);
            Assert.IsTrue(classModel.RestrictionsCount == 1);
            Assert.IsTrue(classModel.TBoxGraph.TriplesCount == 4);
            Assert.IsTrue(classModel.TBoxGraph.ContainsTriple(new RDFTriple(new RDFResource("ex:cRestr"), RDFVocabulary.RDF.TYPE, RDFVocabulary.OWL.CLASS)));
            Assert.IsTrue(classModel.TBoxGraph.ContainsTriple(new RDFTriple(new RDFResource("ex:cRestr"), RDFVocabulary.RDF.TYPE, RDFVocabulary.OWL.RESTRICTION)));
            Assert.IsTrue(classModel.TBoxGraph.ContainsTriple(new RDFTriple(new RDFResource("ex:cRestr"), RDFVocabulary.OWL.ON_PROPERTY, new RDFResource("ex:onProp"))));
            Assert.IsTrue(classModel.TBoxGraph.ContainsTriple(new RDFTriple(new RDFResource("ex:cRestr"), RDFVocabulary.OWL.MAX_CARDINALITY, new RDFTypedLiteral("1", RDFModelEnums.RDFDatatypes.XSD_NONNEGATIVEINTEGER))));
            Assert.IsTrue(classModel.TBoxInferenceGraph.TriplesCount == 0);
            Assert.IsTrue(classModel.TBoxVirtualGraph.TriplesCount == 4);
            Assert.IsTrue(classModel.TBoxVirtualGraph.ContainsTriple(new RDFTriple(new RDFResource("ex:cRestr"), RDFVocabulary.RDF.TYPE, RDFVocabulary.OWL.CLASS)));
            Assert.IsTrue(classModel.TBoxVirtualGraph.ContainsTriple(new RDFTriple(new RDFResource("ex:cRestr"), RDFVocabulary.RDF.TYPE, RDFVocabulary.OWL.RESTRICTION)));
            Assert.IsTrue(classModel.TBoxVirtualGraph.ContainsTriple(new RDFTriple(new RDFResource("ex:cRestr"), RDFVocabulary.OWL.ON_PROPERTY, new RDFResource("ex:onProp"))));
            Assert.IsTrue(classModel.TBoxVirtualGraph.ContainsTriple(new RDFTriple(new RDFResource("ex:cRestr"), RDFVocabulary.OWL.MAX_CARDINALITY, new RDFTypedLiteral("1", RDFModelEnums.RDFDatatypes.XSD_NONNEGATIVEINTEGER))));

            int i = 0;
            IEnumerator<RDFResource> restrictionsEnumerator = classModel.RestrictionsEnumerator;
            while (restrictionsEnumerator.MoveNext())
            {
                Assert.IsTrue(restrictionsEnumerator.Current.Equals(new RDFResource("ex:cRestr")));
                i++;
            }
            Assert.IsTrue(i == 1);
        }

        [TestMethod]
        public void ShouldThrowExceptionOnDeclaringMaxCardinalityRestrictionBecauseZero()
            => Assert.ThrowsException<RDFSemanticsException>(() => new RDFOntologyClassModel().DeclareMaxCardinalityRestriction(new RDFResource("ex:cRestr"), new RDFResource("ex:onProp"), 0));

        [TestMethod]
        public void ShouldDeclareMinMaxCardinalityRestriction()
        {
            RDFOntologyClassModel classModel = new RDFOntologyClassModel();
            classModel.DeclareMinMaxCardinalityRestriction(new RDFResource("ex:cRestr"), new RDFResource("ex:onProp"), 1, 2);

            Assert.IsTrue(classModel.ClassesCount == 1);
            Assert.IsTrue(classModel.AllDisjointClassesCount == 0);
            Assert.IsTrue(classModel.CompositesCount == 0);
            Assert.IsTrue(classModel.DeprecatedClassesCount == 0);
            Assert.IsTrue(classModel.EnumeratesCount == 0);
            Assert.IsTrue(classModel.RestrictionsCount == 1);
            Assert.IsTrue(classModel.TBoxGraph.TriplesCount == 5);
            Assert.IsTrue(classModel.TBoxGraph.ContainsTriple(new RDFTriple(new RDFResource("ex:cRestr"), RDFVocabulary.RDF.TYPE, RDFVocabulary.OWL.CLASS)));
            Assert.IsTrue(classModel.TBoxGraph.ContainsTriple(new RDFTriple(new RDFResource("ex:cRestr"), RDFVocabulary.RDF.TYPE, RDFVocabulary.OWL.RESTRICTION)));
            Assert.IsTrue(classModel.TBoxGraph.ContainsTriple(new RDFTriple(new RDFResource("ex:cRestr"), RDFVocabulary.OWL.ON_PROPERTY, new RDFResource("ex:onProp"))));
            Assert.IsTrue(classModel.TBoxGraph.ContainsTriple(new RDFTriple(new RDFResource("ex:cRestr"), RDFVocabulary.OWL.MIN_CARDINALITY, new RDFTypedLiteral("1", RDFModelEnums.RDFDatatypes.XSD_NONNEGATIVEINTEGER))));
            Assert.IsTrue(classModel.TBoxGraph.ContainsTriple(new RDFTriple(new RDFResource("ex:cRestr"), RDFVocabulary.OWL.MAX_CARDINALITY, new RDFTypedLiteral("2", RDFModelEnums.RDFDatatypes.XSD_NONNEGATIVEINTEGER))));
            Assert.IsTrue(classModel.TBoxInferenceGraph.TriplesCount == 0);
            Assert.IsTrue(classModel.TBoxVirtualGraph.TriplesCount == 5);
            Assert.IsTrue(classModel.TBoxVirtualGraph.ContainsTriple(new RDFTriple(new RDFResource("ex:cRestr"), RDFVocabulary.RDF.TYPE, RDFVocabulary.OWL.CLASS)));
            Assert.IsTrue(classModel.TBoxVirtualGraph.ContainsTriple(new RDFTriple(new RDFResource("ex:cRestr"), RDFVocabulary.RDF.TYPE, RDFVocabulary.OWL.RESTRICTION)));
            Assert.IsTrue(classModel.TBoxVirtualGraph.ContainsTriple(new RDFTriple(new RDFResource("ex:cRestr"), RDFVocabulary.OWL.ON_PROPERTY, new RDFResource("ex:onProp"))));
            Assert.IsTrue(classModel.TBoxVirtualGraph.ContainsTriple(new RDFTriple(new RDFResource("ex:cRestr"), RDFVocabulary.OWL.MIN_CARDINALITY, new RDFTypedLiteral("1", RDFModelEnums.RDFDatatypes.XSD_NONNEGATIVEINTEGER))));
            Assert.IsTrue(classModel.TBoxVirtualGraph.ContainsTriple(new RDFTriple(new RDFResource("ex:cRestr"), RDFVocabulary.OWL.MAX_CARDINALITY, new RDFTypedLiteral("2", RDFModelEnums.RDFDatatypes.XSD_NONNEGATIVEINTEGER))));

            int i = 0;
            IEnumerator<RDFResource> restrictionsEnumerator = classModel.RestrictionsEnumerator;
            while (restrictionsEnumerator.MoveNext())
            {
                Assert.IsTrue(restrictionsEnumerator.Current.Equals(new RDFResource("ex:cRestr")));
                i++;
            }
            Assert.IsTrue(i == 1);
        }

        [TestMethod]
        public void ShouldThrowExceptionOnDeclaringMinMaxCardinalityRestrictionBecauseZeroMin()
            => Assert.ThrowsException<RDFSemanticsException>(() => new RDFOntologyClassModel().DeclareMinMaxCardinalityRestriction(new RDFResource("ex:cRestr"), new RDFResource("ex:onProp"), 0, 2));

        [TestMethod]
        public void ShouldThrowExceptionOnDeclaringMinMaxCardinalityRestrictionBecauseZeroMax()
            => Assert.ThrowsException<RDFSemanticsException>(() => new RDFOntologyClassModel().DeclareMinMaxCardinalityRestriction(new RDFResource("ex:cRestr"), new RDFResource("ex:onProp"), 1, 0));

        [TestMethod]
        public void ShouldThrowExceptionOnDeclaringMinMaxCardinalityRestrictionBecauseInvalidMax()
            => Assert.ThrowsException<RDFSemanticsException>(() => new RDFOntologyClassModel().DeclareMinMaxCardinalityRestriction(new RDFResource("ex:cRestr"), new RDFResource("ex:onProp"), 2, 1));

        [TestMethod]
        public void ShouldDeclareQualifiedCardinalityRestriction()
        {
            RDFOntologyClassModel classModel = new RDFOntologyClassModel();
            classModel.DeclareQualifiedCardinalityRestriction(new RDFResource("ex:cRestr"), new RDFResource("ex:onProp"), 1, new RDFResource("ex:onClass"));

            Assert.IsTrue(classModel.ClassesCount == 1);
            Assert.IsTrue(classModel.AllDisjointClassesCount == 0);
            Assert.IsTrue(classModel.CompositesCount == 0);
            Assert.IsTrue(classModel.DeprecatedClassesCount == 0);
            Assert.IsTrue(classModel.EnumeratesCount == 0);
            Assert.IsTrue(classModel.RestrictionsCount == 1);
            Assert.IsTrue(classModel.TBoxGraph.TriplesCount == 5);
            Assert.IsTrue(classModel.TBoxGraph.ContainsTriple(new RDFTriple(new RDFResource("ex:cRestr"), RDFVocabulary.RDF.TYPE, RDFVocabulary.OWL.CLASS)));
            Assert.IsTrue(classModel.TBoxGraph.ContainsTriple(new RDFTriple(new RDFResource("ex:cRestr"), RDFVocabulary.RDF.TYPE, RDFVocabulary.OWL.RESTRICTION)));
            Assert.IsTrue(classModel.TBoxGraph.ContainsTriple(new RDFTriple(new RDFResource("ex:cRestr"), RDFVocabulary.OWL.ON_PROPERTY, new RDFResource("ex:onProp"))));
            Assert.IsTrue(classModel.TBoxGraph.ContainsTriple(new RDFTriple(new RDFResource("ex:cRestr"), RDFVocabulary.OWL.QUALIFIED_CARDINALITY, new RDFTypedLiteral("1", RDFModelEnums.RDFDatatypes.XSD_NONNEGATIVEINTEGER))));
            Assert.IsTrue(classModel.TBoxGraph.ContainsTriple(new RDFTriple(new RDFResource("ex:cRestr"), RDFVocabulary.OWL.ON_CLASS, new RDFResource("ex:onClass"))));
            Assert.IsTrue(classModel.TBoxInferenceGraph.TriplesCount == 0);
            Assert.IsTrue(classModel.TBoxVirtualGraph.TriplesCount == 5);
            Assert.IsTrue(classModel.TBoxVirtualGraph.ContainsTriple(new RDFTriple(new RDFResource("ex:cRestr"), RDFVocabulary.RDF.TYPE, RDFVocabulary.OWL.CLASS)));
            Assert.IsTrue(classModel.TBoxVirtualGraph.ContainsTriple(new RDFTriple(new RDFResource("ex:cRestr"), RDFVocabulary.RDF.TYPE, RDFVocabulary.OWL.RESTRICTION)));
            Assert.IsTrue(classModel.TBoxVirtualGraph.ContainsTriple(new RDFTriple(new RDFResource("ex:cRestr"), RDFVocabulary.OWL.ON_PROPERTY, new RDFResource("ex:onProp"))));
            Assert.IsTrue(classModel.TBoxVirtualGraph.ContainsTriple(new RDFTriple(new RDFResource("ex:cRestr"), RDFVocabulary.OWL.QUALIFIED_CARDINALITY, new RDFTypedLiteral("1", RDFModelEnums.RDFDatatypes.XSD_NONNEGATIVEINTEGER))));
            Assert.IsTrue(classModel.TBoxVirtualGraph.ContainsTriple(new RDFTriple(new RDFResource("ex:cRestr"), RDFVocabulary.OWL.ON_CLASS, new RDFResource("ex:onClass"))));

            int i = 0;
            IEnumerator<RDFResource> restrictionsEnumerator = classModel.RestrictionsEnumerator;
            while (restrictionsEnumerator.MoveNext())
            {
                Assert.IsTrue(restrictionsEnumerator.Current.Equals(new RDFResource("ex:cRestr")));
                i++;
            }
            Assert.IsTrue(i == 1);
        }

        [TestMethod]
        public void ShouldThrowExceptionOnDeclaringQualifiedCardinalityRestrictionBecauseZero()
            => Assert.ThrowsException<RDFSemanticsException>(() => new RDFOntologyClassModel().DeclareQualifiedCardinalityRestriction(new RDFResource("ex:cRestr"), new RDFResource("ex:onProp"), 0, new RDFResource("ex:onClass")));

        [TestMethod]
        public void ShouldDeclareMinQualifiedCardinalityRestriction()
        {
            RDFOntologyClassModel classModel = new RDFOntologyClassModel();
            classModel.DeclareMinQualifiedCardinalityRestriction(new RDFResource("ex:cRestr"), new RDFResource("ex:onProp"), 1, new RDFResource("ex:onClass"));

            Assert.IsTrue(classModel.ClassesCount == 1);
            Assert.IsTrue(classModel.AllDisjointClassesCount == 0);
            Assert.IsTrue(classModel.CompositesCount == 0);
            Assert.IsTrue(classModel.DeprecatedClassesCount == 0);
            Assert.IsTrue(classModel.EnumeratesCount == 0);
            Assert.IsTrue(classModel.RestrictionsCount == 1);
            Assert.IsTrue(classModel.TBoxGraph.TriplesCount == 5);
            Assert.IsTrue(classModel.TBoxGraph.ContainsTriple(new RDFTriple(new RDFResource("ex:cRestr"), RDFVocabulary.RDF.TYPE, RDFVocabulary.OWL.CLASS)));
            Assert.IsTrue(classModel.TBoxGraph.ContainsTriple(new RDFTriple(new RDFResource("ex:cRestr"), RDFVocabulary.RDF.TYPE, RDFVocabulary.OWL.RESTRICTION)));
            Assert.IsTrue(classModel.TBoxGraph.ContainsTriple(new RDFTriple(new RDFResource("ex:cRestr"), RDFVocabulary.OWL.ON_PROPERTY, new RDFResource("ex:onProp"))));
            Assert.IsTrue(classModel.TBoxGraph.ContainsTriple(new RDFTriple(new RDFResource("ex:cRestr"), RDFVocabulary.OWL.MIN_QUALIFIED_CARDINALITY, new RDFTypedLiteral("1", RDFModelEnums.RDFDatatypes.XSD_NONNEGATIVEINTEGER))));
            Assert.IsTrue(classModel.TBoxGraph.ContainsTriple(new RDFTriple(new RDFResource("ex:cRestr"), RDFVocabulary.OWL.ON_CLASS, new RDFResource("ex:onClass"))));
            Assert.IsTrue(classModel.TBoxInferenceGraph.TriplesCount == 0);
            Assert.IsTrue(classModel.TBoxVirtualGraph.TriplesCount == 5);
            Assert.IsTrue(classModel.TBoxVirtualGraph.ContainsTriple(new RDFTriple(new RDFResource("ex:cRestr"), RDFVocabulary.RDF.TYPE, RDFVocabulary.OWL.CLASS)));
            Assert.IsTrue(classModel.TBoxVirtualGraph.ContainsTriple(new RDFTriple(new RDFResource("ex:cRestr"), RDFVocabulary.RDF.TYPE, RDFVocabulary.OWL.RESTRICTION)));
            Assert.IsTrue(classModel.TBoxVirtualGraph.ContainsTriple(new RDFTriple(new RDFResource("ex:cRestr"), RDFVocabulary.OWL.ON_PROPERTY, new RDFResource("ex:onProp"))));
            Assert.IsTrue(classModel.TBoxVirtualGraph.ContainsTriple(new RDFTriple(new RDFResource("ex:cRestr"), RDFVocabulary.OWL.MIN_QUALIFIED_CARDINALITY, new RDFTypedLiteral("1", RDFModelEnums.RDFDatatypes.XSD_NONNEGATIVEINTEGER))));
            Assert.IsTrue(classModel.TBoxVirtualGraph.ContainsTriple(new RDFTriple(new RDFResource("ex:cRestr"), RDFVocabulary.OWL.ON_CLASS, new RDFResource("ex:onClass"))));

            int i = 0;
            IEnumerator<RDFResource> restrictionsEnumerator = classModel.RestrictionsEnumerator;
            while (restrictionsEnumerator.MoveNext())
            {
                Assert.IsTrue(restrictionsEnumerator.Current.Equals(new RDFResource("ex:cRestr")));
                i++;
            }
            Assert.IsTrue(i == 1);
        }

        [TestMethod]
        public void ShouldThrowExceptionOnDeclaringMinQualifiedCardinalityRestrictionBecauseZero()
            => Assert.ThrowsException<RDFSemanticsException>(() => new RDFOntologyClassModel().DeclareMinQualifiedCardinalityRestriction(new RDFResource("ex:cRestr"), new RDFResource("ex:onProp"), 0, new RDFResource("ex:onClass")));

        [TestMethod]
        public void ShouldDeclareMaxQualifiedCardinalityRestriction()
        {
            RDFOntologyClassModel classModel = new RDFOntologyClassModel();
            classModel.DeclareMaxQualifiedCardinalityRestriction(new RDFResource("ex:cRestr"), new RDFResource("ex:onProp"), 1, new RDFResource("ex:onClass"));

            Assert.IsTrue(classModel.ClassesCount == 1);
            Assert.IsTrue(classModel.AllDisjointClassesCount == 0);
            Assert.IsTrue(classModel.CompositesCount == 0);
            Assert.IsTrue(classModel.DeprecatedClassesCount == 0);
            Assert.IsTrue(classModel.EnumeratesCount == 0);
            Assert.IsTrue(classModel.RestrictionsCount == 1);
            Assert.IsTrue(classModel.TBoxGraph.TriplesCount == 5);
            Assert.IsTrue(classModel.TBoxGraph.ContainsTriple(new RDFTriple(new RDFResource("ex:cRestr"), RDFVocabulary.RDF.TYPE, RDFVocabulary.OWL.CLASS)));
            Assert.IsTrue(classModel.TBoxGraph.ContainsTriple(new RDFTriple(new RDFResource("ex:cRestr"), RDFVocabulary.RDF.TYPE, RDFVocabulary.OWL.RESTRICTION)));
            Assert.IsTrue(classModel.TBoxGraph.ContainsTriple(new RDFTriple(new RDFResource("ex:cRestr"), RDFVocabulary.OWL.ON_PROPERTY, new RDFResource("ex:onProp"))));
            Assert.IsTrue(classModel.TBoxGraph.ContainsTriple(new RDFTriple(new RDFResource("ex:cRestr"), RDFVocabulary.OWL.MAX_QUALIFIED_CARDINALITY, new RDFTypedLiteral("1", RDFModelEnums.RDFDatatypes.XSD_NONNEGATIVEINTEGER))));
            Assert.IsTrue(classModel.TBoxGraph.ContainsTriple(new RDFTriple(new RDFResource("ex:cRestr"), RDFVocabulary.OWL.ON_CLASS, new RDFResource("ex:onClass"))));
            Assert.IsTrue(classModel.TBoxInferenceGraph.TriplesCount == 0);
            Assert.IsTrue(classModel.TBoxVirtualGraph.TriplesCount == 5);
            Assert.IsTrue(classModel.TBoxVirtualGraph.ContainsTriple(new RDFTriple(new RDFResource("ex:cRestr"), RDFVocabulary.RDF.TYPE, RDFVocabulary.OWL.CLASS)));
            Assert.IsTrue(classModel.TBoxVirtualGraph.ContainsTriple(new RDFTriple(new RDFResource("ex:cRestr"), RDFVocabulary.RDF.TYPE, RDFVocabulary.OWL.RESTRICTION)));
            Assert.IsTrue(classModel.TBoxVirtualGraph.ContainsTriple(new RDFTriple(new RDFResource("ex:cRestr"), RDFVocabulary.OWL.ON_PROPERTY, new RDFResource("ex:onProp"))));
            Assert.IsTrue(classModel.TBoxVirtualGraph.ContainsTriple(new RDFTriple(new RDFResource("ex:cRestr"), RDFVocabulary.OWL.MAX_QUALIFIED_CARDINALITY, new RDFTypedLiteral("1", RDFModelEnums.RDFDatatypes.XSD_NONNEGATIVEINTEGER))));
            Assert.IsTrue(classModel.TBoxVirtualGraph.ContainsTriple(new RDFTriple(new RDFResource("ex:cRestr"), RDFVocabulary.OWL.ON_CLASS, new RDFResource("ex:onClass"))));

            int i = 0;
            IEnumerator<RDFResource> restrictionsEnumerator = classModel.RestrictionsEnumerator;
            while (restrictionsEnumerator.MoveNext())
            {
                Assert.IsTrue(restrictionsEnumerator.Current.Equals(new RDFResource("ex:cRestr")));
                i++;
            }
            Assert.IsTrue(i == 1);
        }

        [TestMethod]
        public void ShouldThrowExceptionOnDeclaringMaxQualifiedCardinalityRestrictionBecauseZero()
            => Assert.ThrowsException<RDFSemanticsException>(() => new RDFOntologyClassModel().DeclareMaxQualifiedCardinalityRestriction(new RDFResource("ex:cRestr"), new RDFResource("ex:onProp"), 0, new RDFResource("ex:onClass")));

        [TestMethod]
        public void ShouldDeclareMinMaxQualifiedCardinalityRestriction()
        {
            RDFOntologyClassModel classModel = new RDFOntologyClassModel();
            classModel.DeclareMinMaxQualifiedCardinalityRestriction(new RDFResource("ex:cRestr"), new RDFResource("ex:onProp"), 1, 2, new RDFResource("ex:onClass"));

            Assert.IsTrue(classModel.ClassesCount == 1);
            Assert.IsTrue(classModel.AllDisjointClassesCount == 0);
            Assert.IsTrue(classModel.CompositesCount == 0);
            Assert.IsTrue(classModel.DeprecatedClassesCount == 0);
            Assert.IsTrue(classModel.EnumeratesCount == 0);
            Assert.IsTrue(classModel.RestrictionsCount == 1);
            Assert.IsTrue(classModel.TBoxGraph.TriplesCount == 6);
            Assert.IsTrue(classModel.TBoxGraph.ContainsTriple(new RDFTriple(new RDFResource("ex:cRestr"), RDFVocabulary.RDF.TYPE, RDFVocabulary.OWL.CLASS)));
            Assert.IsTrue(classModel.TBoxGraph.ContainsTriple(new RDFTriple(new RDFResource("ex:cRestr"), RDFVocabulary.RDF.TYPE, RDFVocabulary.OWL.RESTRICTION)));
            Assert.IsTrue(classModel.TBoxGraph.ContainsTriple(new RDFTriple(new RDFResource("ex:cRestr"), RDFVocabulary.OWL.ON_PROPERTY, new RDFResource("ex:onProp"))));
            Assert.IsTrue(classModel.TBoxGraph.ContainsTriple(new RDFTriple(new RDFResource("ex:cRestr"), RDFVocabulary.OWL.MIN_QUALIFIED_CARDINALITY, new RDFTypedLiteral("1", RDFModelEnums.RDFDatatypes.XSD_NONNEGATIVEINTEGER))));
            Assert.IsTrue(classModel.TBoxGraph.ContainsTriple(new RDFTriple(new RDFResource("ex:cRestr"), RDFVocabulary.OWL.MAX_QUALIFIED_CARDINALITY, new RDFTypedLiteral("2", RDFModelEnums.RDFDatatypes.XSD_NONNEGATIVEINTEGER))));
            Assert.IsTrue(classModel.TBoxGraph.ContainsTriple(new RDFTriple(new RDFResource("ex:cRestr"), RDFVocabulary.OWL.ON_CLASS, new RDFResource("ex:onClass"))));
            Assert.IsTrue(classModel.TBoxInferenceGraph.TriplesCount == 0);
            Assert.IsTrue(classModel.TBoxVirtualGraph.TriplesCount == 6);
            Assert.IsTrue(classModel.TBoxVirtualGraph.ContainsTriple(new RDFTriple(new RDFResource("ex:cRestr"), RDFVocabulary.RDF.TYPE, RDFVocabulary.OWL.CLASS)));
            Assert.IsTrue(classModel.TBoxVirtualGraph.ContainsTriple(new RDFTriple(new RDFResource("ex:cRestr"), RDFVocabulary.RDF.TYPE, RDFVocabulary.OWL.RESTRICTION)));
            Assert.IsTrue(classModel.TBoxVirtualGraph.ContainsTriple(new RDFTriple(new RDFResource("ex:cRestr"), RDFVocabulary.OWL.ON_PROPERTY, new RDFResource("ex:onProp"))));
            Assert.IsTrue(classModel.TBoxVirtualGraph.ContainsTriple(new RDFTriple(new RDFResource("ex:cRestr"), RDFVocabulary.OWL.MIN_QUALIFIED_CARDINALITY, new RDFTypedLiteral("1", RDFModelEnums.RDFDatatypes.XSD_NONNEGATIVEINTEGER))));
            Assert.IsTrue(classModel.TBoxVirtualGraph.ContainsTriple(new RDFTriple(new RDFResource("ex:cRestr"), RDFVocabulary.OWL.MAX_QUALIFIED_CARDINALITY, new RDFTypedLiteral("2", RDFModelEnums.RDFDatatypes.XSD_NONNEGATIVEINTEGER))));
            Assert.IsTrue(classModel.TBoxVirtualGraph.ContainsTriple(new RDFTriple(new RDFResource("ex:cRestr"), RDFVocabulary.OWL.ON_CLASS, new RDFResource("ex:onClass"))));

            int i = 0;
            IEnumerator<RDFResource> restrictionsEnumerator = classModel.RestrictionsEnumerator;
            while (restrictionsEnumerator.MoveNext())
            {
                Assert.IsTrue(restrictionsEnumerator.Current.Equals(new RDFResource("ex:cRestr")));
                i++;
            }
            Assert.IsTrue(i == 1);
        }

        [TestMethod]
        public void ShouldThrowExceptionOnDeclaringMinMaxQualifiedCardinalityRestrictionBecauseNullClass()
            => Assert.ThrowsException<RDFSemanticsException>(() => new RDFOntologyClassModel().DeclareMinMaxQualifiedCardinalityRestriction(new RDFResource("ex:cRestr"), new RDFResource("ex:onProp"), 1, 2, null));

        [TestMethod]
        public void ShouldThrowExceptionOnDeclaringMinMaxQualifiedCardinalityRestrictionBecauseZeroMin()
            => Assert.ThrowsException<RDFSemanticsException>(() => new RDFOntologyClassModel().DeclareMinMaxQualifiedCardinalityRestriction(new RDFResource("ex:cRestr"), new RDFResource("ex:onProp"), 0, 2, new RDFResource("ex:onClass")));

        [TestMethod]
        public void ShouldThrowExceptionOnDeclaringMinMaxQualifiedCardinalityRestrictionBecauseZeroMax()
            => Assert.ThrowsException<RDFSemanticsException>(() => new RDFOntologyClassModel().DeclareMinMaxQualifiedCardinalityRestriction(new RDFResource("ex:cRestr"), new RDFResource("ex:onProp"), 1, 0, new RDFResource("ex:onClass")));

        [TestMethod]
        public void ShouldThrowExceptionOnDeclaringMinMaxQualifiedCardinalityRestrictionBecauseInvalidMax()
            => Assert.ThrowsException<RDFSemanticsException>(() => new RDFOntologyClassModel().DeclareMinMaxQualifiedCardinalityRestriction(new RDFResource("ex:cRestr"), new RDFResource("ex:onProp"), 2, 1, new RDFResource("ex:onClass")));

        [TestMethod]
        public void ShouldDeclareEnumerateClass()
        {
            RDFOntologyClassModel classModel = new RDFOntologyClassModel();
            classModel.DeclareEnumerateClass(new RDFResource("ex:enumClass"), new List<RDFResource>() { new RDFResource("ex:indiv1"), new RDFResource("ex:indiv2") });

            Assert.IsTrue(classModel.ClassesCount == 1);
            Assert.IsTrue(classModel.AllDisjointClassesCount == 0);
            Assert.IsTrue(classModel.CompositesCount == 0);
            Assert.IsTrue(classModel.DeprecatedClassesCount == 0);
            Assert.IsTrue(classModel.EnumeratesCount == 1);
            Assert.IsTrue(classModel.RestrictionsCount == 0);
            Assert.IsTrue(classModel.TBoxGraph.TriplesCount == 8);
            Assert.IsTrue(classModel.TBoxGraph[new RDFResource("ex:enumClass"), RDFVocabulary.RDF.TYPE, RDFVocabulary.OWL.CLASS, null].TriplesCount == 1);
            Assert.IsTrue(classModel.TBoxGraph[new RDFResource("ex:enumClass"), RDFVocabulary.OWL.ONE_OF, null, null].TriplesCount == 1);
            Assert.IsTrue(classModel.TBoxGraph[null, RDFVocabulary.RDF.TYPE, RDFVocabulary.RDF.LIST, null].TriplesCount == 2);
            Assert.IsTrue(classModel.TBoxGraph[null, RDFVocabulary.RDF.FIRST, new RDFResource("ex:indiv1"), null].TriplesCount == 1);
            Assert.IsTrue(classModel.TBoxGraph[null, RDFVocabulary.RDF.FIRST, new RDFResource("ex:indiv2"), null].TriplesCount == 1);
            Assert.IsTrue(classModel.TBoxGraph[null, RDFVocabulary.RDF.REST, null, null].TriplesCount == 2);
            Assert.IsTrue(classModel.TBoxInferenceGraph.TriplesCount == 0);
            Assert.IsTrue(classModel.TBoxVirtualGraph.TriplesCount == 8);
            Assert.IsTrue(classModel.TBoxVirtualGraph[new RDFResource("ex:enumClass"), RDFVocabulary.RDF.TYPE, RDFVocabulary.OWL.CLASS, null].TriplesCount == 1);
            Assert.IsTrue(classModel.TBoxVirtualGraph[new RDFResource("ex:enumClass"), RDFVocabulary.OWL.ONE_OF, null, null].TriplesCount == 1);
            Assert.IsTrue(classModel.TBoxVirtualGraph[null, RDFVocabulary.RDF.TYPE, RDFVocabulary.RDF.LIST, null].TriplesCount == 2);
            Assert.IsTrue(classModel.TBoxVirtualGraph[null, RDFVocabulary.RDF.FIRST, new RDFResource("ex:indiv1"), null].TriplesCount == 1);
            Assert.IsTrue(classModel.TBoxVirtualGraph[null, RDFVocabulary.RDF.FIRST, new RDFResource("ex:indiv2"), null].TriplesCount == 1);
            Assert.IsTrue(classModel.TBoxVirtualGraph[null, RDFVocabulary.RDF.REST, null, null].TriplesCount == 2);

            int i = 0;
            IEnumerator<RDFResource> enumeratesEnumerator = classModel.EnumeratesEnumerator;
            while (enumeratesEnumerator.MoveNext())
            {
                Assert.IsTrue(enumeratesEnumerator.Current.Equals(new RDFResource("ex:enumClass")));
                i++;
            }
            Assert.IsTrue(i == 1);
        }

        [TestMethod]
        public void ShouldThrowExceptionOnDeclaringEnumerateClassBecauseNullClass()
            => Assert.ThrowsException<RDFSemanticsException>(() => new RDFOntologyClassModel().DeclareEnumerateClass(null, new List<RDFResource>() { new RDFResource("ex:indiv1") }));

        [TestMethod]
        public void ShouldThrowExceptionOnDeclaringEnumerateClassBecauseNullIndividuals()
            => Assert.ThrowsException<RDFSemanticsException>(() => new RDFOntologyClassModel().DeclareEnumerateClass(new RDFResource("ex:enumClass"), null));

        [TestMethod]
        public void ShouldThrowExceptionOnDeclaringEnumerateClassBecauseEmptyIndividuals()
            => Assert.ThrowsException<RDFSemanticsException>(() => new RDFOntologyClassModel().DeclareEnumerateClass(new RDFResource("ex:enumClass"), new List<RDFResource>()));

        [TestMethod]
        public void ShouldDeclareUnionClass()
        {
            RDFOntologyClassModel classModel = new RDFOntologyClassModel();
            classModel.DeclareUnionClass(new RDFResource("ex:unionClass"), new List<RDFResource>() { new RDFResource("ex:class1"), new RDFResource("ex:class2") });

            Assert.IsTrue(classModel.ClassesCount == 1);
            Assert.IsTrue(classModel.AllDisjointClassesCount == 0);
            Assert.IsTrue(classModel.CompositesCount == 1);
            Assert.IsTrue(classModel.DeprecatedClassesCount == 0);
            Assert.IsTrue(classModel.EnumeratesCount == 0);
            Assert.IsTrue(classModel.RestrictionsCount == 0);
            Assert.IsTrue(classModel.TBoxGraph.TriplesCount == 8);
            Assert.IsTrue(classModel.TBoxGraph[new RDFResource("ex:unionClass"), RDFVocabulary.RDF.TYPE, RDFVocabulary.OWL.CLASS, null].TriplesCount == 1);
            Assert.IsTrue(classModel.TBoxGraph[new RDFResource("ex:unionClass"), RDFVocabulary.OWL.UNION_OF, null, null].TriplesCount == 1);
            Assert.IsTrue(classModel.TBoxGraph[null, RDFVocabulary.RDF.TYPE, RDFVocabulary.RDF.LIST, null].TriplesCount == 2);
            Assert.IsTrue(classModel.TBoxGraph[null, RDFVocabulary.RDF.FIRST, new RDFResource("ex:class1"), null].TriplesCount == 1);
            Assert.IsTrue(classModel.TBoxGraph[null, RDFVocabulary.RDF.FIRST, new RDFResource("ex:class2"), null].TriplesCount == 1);
            Assert.IsTrue(classModel.TBoxGraph[null, RDFVocabulary.RDF.REST, null, null].TriplesCount == 2);
            Assert.IsTrue(classModel.TBoxInferenceGraph.TriplesCount == 0);
            Assert.IsTrue(classModel.TBoxVirtualGraph.TriplesCount == 8);
            Assert.IsTrue(classModel.TBoxVirtualGraph[new RDFResource("ex:unionClass"), RDFVocabulary.RDF.TYPE, RDFVocabulary.OWL.CLASS, null].TriplesCount == 1);
            Assert.IsTrue(classModel.TBoxVirtualGraph[new RDFResource("ex:unionClass"), RDFVocabulary.OWL.UNION_OF, null, null].TriplesCount == 1);
            Assert.IsTrue(classModel.TBoxVirtualGraph[null, RDFVocabulary.RDF.TYPE, RDFVocabulary.RDF.LIST, null].TriplesCount == 2);
            Assert.IsTrue(classModel.TBoxVirtualGraph[null, RDFVocabulary.RDF.FIRST, new RDFResource("ex:class1"), null].TriplesCount == 1);
            Assert.IsTrue(classModel.TBoxVirtualGraph[null, RDFVocabulary.RDF.FIRST, new RDFResource("ex:class2"), null].TriplesCount == 1);
            Assert.IsTrue(classModel.TBoxVirtualGraph[null, RDFVocabulary.RDF.REST, null, null].TriplesCount == 2);

            int i = 0;
            IEnumerator<RDFResource> compositesEnumerator = classModel.CompositesEnumerator;
            while (compositesEnumerator.MoveNext())
            {
                Assert.IsTrue(compositesEnumerator.Current.Equals(new RDFResource("ex:unionClass")));
                i++;
            }
            Assert.IsTrue(i == 1);
        }

        [TestMethod]
        public void ShouldThrowExceptionOnDeclaringUnionClassBecauseNullClass()
            => Assert.ThrowsException<RDFSemanticsException>(() => new RDFOntologyClassModel().DeclareUnionClass(null, new List<RDFResource>() { new RDFResource("ex:class1") }));

        [TestMethod]
        public void ShouldThrowExceptionOnDeclaringUnionClassBecauseNullClasses()
            => Assert.ThrowsException<RDFSemanticsException>(() => new RDFOntologyClassModel().DeclareUnionClass(new RDFResource("ex:unionClass"), null));

        [TestMethod]
        public void ShouldThrowExceptionOnDeclaringUnionClassBecauseEmptyClasses()
            => Assert.ThrowsException<RDFSemanticsException>(() => new RDFOntologyClassModel().DeclareUnionClass(new RDFResource("ex:unionClass"), new List<RDFResource>()));

        [TestMethod]
        public void ShouldDeclareIntersectionClass()
        {
            RDFOntologyClassModel classModel = new RDFOntologyClassModel();
            classModel.DeclareIntersectionClass(new RDFResource("ex:intersectionClass"), new List<RDFResource>() { new RDFResource("ex:class1"), new RDFResource("ex:class2") });

            Assert.IsTrue(classModel.ClassesCount == 1);
            Assert.IsTrue(classModel.AllDisjointClassesCount == 0);
            Assert.IsTrue(classModel.CompositesCount == 1);
            Assert.IsTrue(classModel.DeprecatedClassesCount == 0);
            Assert.IsTrue(classModel.EnumeratesCount == 0);
            Assert.IsTrue(classModel.RestrictionsCount == 0);
            Assert.IsTrue(classModel.TBoxGraph.TriplesCount == 8);
            Assert.IsTrue(classModel.TBoxGraph[new RDFResource("ex:intersectionClass"), RDFVocabulary.RDF.TYPE, RDFVocabulary.OWL.CLASS, null].TriplesCount == 1);
            Assert.IsTrue(classModel.TBoxGraph[new RDFResource("ex:intersectionClass"), RDFVocabulary.OWL.INTERSECTION_OF, null, null].TriplesCount == 1);
            Assert.IsTrue(classModel.TBoxGraph[null, RDFVocabulary.RDF.TYPE, RDFVocabulary.RDF.LIST, null].TriplesCount == 2);
            Assert.IsTrue(classModel.TBoxGraph[null, RDFVocabulary.RDF.FIRST, new RDFResource("ex:class1"), null].TriplesCount == 1);
            Assert.IsTrue(classModel.TBoxGraph[null, RDFVocabulary.RDF.FIRST, new RDFResource("ex:class2"), null].TriplesCount == 1);
            Assert.IsTrue(classModel.TBoxGraph[null, RDFVocabulary.RDF.REST, null, null].TriplesCount == 2);
            Assert.IsTrue(classModel.TBoxInferenceGraph.TriplesCount == 0);
            Assert.IsTrue(classModel.TBoxVirtualGraph.TriplesCount == 8);
            Assert.IsTrue(classModel.TBoxVirtualGraph[new RDFResource("ex:intersectionClass"), RDFVocabulary.RDF.TYPE, RDFVocabulary.OWL.CLASS, null].TriplesCount == 1);
            Assert.IsTrue(classModel.TBoxVirtualGraph[new RDFResource("ex:intersectionClass"), RDFVocabulary.OWL.INTERSECTION_OF, null, null].TriplesCount == 1);
            Assert.IsTrue(classModel.TBoxVirtualGraph[null, RDFVocabulary.RDF.TYPE, RDFVocabulary.RDF.LIST, null].TriplesCount == 2);
            Assert.IsTrue(classModel.TBoxVirtualGraph[null, RDFVocabulary.RDF.FIRST, new RDFResource("ex:class1"), null].TriplesCount == 1);
            Assert.IsTrue(classModel.TBoxVirtualGraph[null, RDFVocabulary.RDF.FIRST, new RDFResource("ex:class2"), null].TriplesCount == 1);
            Assert.IsTrue(classModel.TBoxVirtualGraph[null, RDFVocabulary.RDF.REST, null, null].TriplesCount == 2);

            int i = 0;
            IEnumerator<RDFResource> compositesEnumerator = classModel.CompositesEnumerator;
            while (compositesEnumerator.MoveNext())
            {
                Assert.IsTrue(compositesEnumerator.Current.Equals(new RDFResource("ex:intersectionClass")));
                i++;
            }
            Assert.IsTrue(i == 1);
        }

        [TestMethod]
        public void ShouldThrowExceptionOnDeclaringIntersectionClassBecauseNullClass()
            => Assert.ThrowsException<RDFSemanticsException>(() => new RDFOntologyClassModel().DeclareIntersectionClass(null, new List<RDFResource>() { new RDFResource("ex:class1") }));

        [TestMethod]
        public void ShouldThrowExceptionOnDeclaringIntersectionClassBecauseNullClasses()
            => Assert.ThrowsException<RDFSemanticsException>(() => new RDFOntologyClassModel().DeclareIntersectionClass(new RDFResource("ex:intersectionClass"), null));

        [TestMethod]
        public void ShouldThrowExceptionOnDeclaringIntersectionClassBecauseEmptyClasses()
            => Assert.ThrowsException<RDFSemanticsException>(() => new RDFOntologyClassModel().DeclareIntersectionClass(new RDFResource("ex:intersectionClass"), new List<RDFResource>()));

        [TestMethod]
        public void ShouldDeclareComplementClass()
        {
            RDFOntologyClassModel classModel = new RDFOntologyClassModel();
            classModel.DeclareComplementClass(new RDFResource("ex:complementClass"), new RDFResource("ex:class"));

            Assert.IsTrue(classModel.ClassesCount == 1);
            Assert.IsTrue(classModel.AllDisjointClassesCount == 0);
            Assert.IsTrue(classModel.CompositesCount == 1);
            Assert.IsTrue(classModel.DeprecatedClassesCount == 0);
            Assert.IsTrue(classModel.EnumeratesCount == 0);
            Assert.IsTrue(classModel.RestrictionsCount == 0);
            Assert.IsTrue(classModel.TBoxGraph.TriplesCount == 2);
            Assert.IsTrue(classModel.TBoxGraph[new RDFResource("ex:complementClass"), RDFVocabulary.RDF.TYPE, RDFVocabulary.OWL.CLASS, null].TriplesCount == 1);
            Assert.IsTrue(classModel.TBoxGraph[new RDFResource("ex:complementClass"), RDFVocabulary.OWL.COMPLEMENT_OF, new RDFResource("ex:class"), null].TriplesCount == 1);
            Assert.IsTrue(classModel.TBoxInferenceGraph.TriplesCount == 0);
            Assert.IsTrue(classModel.TBoxVirtualGraph.TriplesCount == 2);
            Assert.IsTrue(classModel.TBoxVirtualGraph[new RDFResource("ex:complementClass"), RDFVocabulary.RDF.TYPE, RDFVocabulary.OWL.CLASS, null].TriplesCount == 1);
            Assert.IsTrue(classModel.TBoxVirtualGraph[new RDFResource("ex:complementClass"), RDFVocabulary.OWL.COMPLEMENT_OF, new RDFResource("ex:class"), null].TriplesCount == 1);

            int i = 0;
            IEnumerator<RDFResource> compositesEnumerator = classModel.CompositesEnumerator;
            while (compositesEnumerator.MoveNext())
            {
                Assert.IsTrue(compositesEnumerator.Current.Equals(new RDFResource("ex:complementClass")));
                i++;
            }
            Assert.IsTrue(i == 1);
        }

        [TestMethod]
        public void ShouldThrowExceptionOnDeclaringComplementClassBecauseNullClass()
            => Assert.ThrowsException<RDFSemanticsException>(() => new RDFOntologyClassModel().DeclareComplementClass(null, new RDFResource("ex:class1")));

        [TestMethod]
        public void ShouldThrowExceptionOnDeclaringComplementClassBecauseNullComplementClass()
            => Assert.ThrowsException<RDFSemanticsException>(() => new RDFOntologyClassModel().DeclareComplementClass(new RDFResource("ex:complementClass"), null));

        [TestMethod]
        public void ShouldDeclareDisjointUnionClass()
        {
            RDFOntologyClassModel classModel = new RDFOntologyClassModel();
            classModel.DeclareDisjointUnionClass(new RDFResource("ex:disjointUnionClass"), new List<RDFResource>() { new RDFResource("ex:class1"), new RDFResource("ex:class2") });

            Assert.IsTrue(classModel.ClassesCount == 1);
            Assert.IsTrue(classModel.AllDisjointClassesCount == 0);
            Assert.IsTrue(classModel.CompositesCount == 0);
            Assert.IsTrue(classModel.DeprecatedClassesCount == 0);
            Assert.IsTrue(classModel.EnumeratesCount == 0);
            Assert.IsTrue(classModel.RestrictionsCount == 0);
            Assert.IsTrue(classModel.TBoxGraph.TriplesCount == 8);
            Assert.IsTrue(classModel.TBoxGraph[new RDFResource("ex:disjointUnionClass"), RDFVocabulary.RDF.TYPE, RDFVocabulary.OWL.CLASS, null].TriplesCount == 1);
            Assert.IsTrue(classModel.TBoxGraph[new RDFResource("ex:disjointUnionClass"), RDFVocabulary.OWL.DISJOINT_UNION_OF, null, null].TriplesCount == 1);
            Assert.IsTrue(classModel.TBoxGraph[null, RDFVocabulary.RDF.TYPE, RDFVocabulary.RDF.LIST, null].TriplesCount == 2);
            Assert.IsTrue(classModel.TBoxGraph[null, RDFVocabulary.RDF.FIRST, new RDFResource("ex:class1"), null].TriplesCount == 1);
            Assert.IsTrue(classModel.TBoxGraph[null, RDFVocabulary.RDF.FIRST, new RDFResource("ex:class2"), null].TriplesCount == 1);
            Assert.IsTrue(classModel.TBoxGraph[null, RDFVocabulary.RDF.REST, null, null].TriplesCount == 2);
            Assert.IsTrue(classModel.TBoxInferenceGraph.TriplesCount == 0);
            Assert.IsTrue(classModel.TBoxVirtualGraph.TriplesCount == 8);
            Assert.IsTrue(classModel.TBoxVirtualGraph[new RDFResource("ex:disjointUnionClass"), RDFVocabulary.RDF.TYPE, RDFVocabulary.OWL.CLASS, null].TriplesCount == 1);
            Assert.IsTrue(classModel.TBoxVirtualGraph[new RDFResource("ex:disjointUnionClass"), RDFVocabulary.OWL.DISJOINT_UNION_OF, null, null].TriplesCount == 1);
            Assert.IsTrue(classModel.TBoxVirtualGraph[null, RDFVocabulary.RDF.TYPE, RDFVocabulary.RDF.LIST, null].TriplesCount == 2);
            Assert.IsTrue(classModel.TBoxVirtualGraph[null, RDFVocabulary.RDF.FIRST, new RDFResource("ex:class1"), null].TriplesCount == 1);
            Assert.IsTrue(classModel.TBoxVirtualGraph[null, RDFVocabulary.RDF.FIRST, new RDFResource("ex:class2"), null].TriplesCount == 1);
            Assert.IsTrue(classModel.TBoxVirtualGraph[null, RDFVocabulary.RDF.REST, null, null].TriplesCount == 2);
        }

        [TestMethod]
        public void ShouldThrowExceptionOnDeclaringDisjointUnionClassBecauseNullClass()
            => Assert.ThrowsException<RDFSemanticsException>(() => new RDFOntologyClassModel().DeclareDisjointUnionClass(null, new List<RDFResource>() { new RDFResource("ex:class1") }));

        [TestMethod]
        public void ShouldThrowExceptionOnDeclaringDisjointUnionClassBecauseNullClasses()
            => Assert.ThrowsException<RDFSemanticsException>(() => new RDFOntologyClassModel().DeclareDisjointUnionClass(new RDFResource("ex:disjointUnionClass"), null));

        [TestMethod]
        public void ShouldThrowExceptionOnDeclaringDisjointUnionClassBecauseEmptyClasses()
            => Assert.ThrowsException<RDFSemanticsException>(() => new RDFOntologyClassModel().DeclareDisjointUnionClass(new RDFResource("ex:disjointUnionClass"), new List<RDFResource>()));

        [TestMethod]
        public void ShouldDeclareAllDisjointClasses()
        {
            RDFOntologyClassModel classModel = new RDFOntologyClassModel();
            classModel.DeclareAllDisjointClasses(new RDFResource("ex:allDisjointClasses"), new List<RDFResource>() { new RDFResource("ex:class1"), new RDFResource("ex:class2") });

            Assert.IsTrue(classModel.ClassesCount == 1);
            Assert.IsTrue(classModel.AllDisjointClassesCount == 1);
            Assert.IsTrue(classModel.CompositesCount == 0);
            Assert.IsTrue(classModel.DeprecatedClassesCount == 0);
            Assert.IsTrue(classModel.EnumeratesCount == 0);
            Assert.IsTrue(classModel.RestrictionsCount == 0);
            Assert.IsTrue(classModel.TBoxGraph.TriplesCount == 9);
            Assert.IsTrue(classModel.TBoxGraph[new RDFResource("ex:allDisjointClasses"), RDFVocabulary.RDF.TYPE, RDFVocabulary.OWL.CLASS, null].TriplesCount == 1);
            Assert.IsTrue(classModel.TBoxGraph[new RDFResource("ex:allDisjointClasses"), RDFVocabulary.RDF.TYPE, RDFVocabulary.OWL.ALL_DISJOINT_CLASSES, null].TriplesCount == 1);
            Assert.IsTrue(classModel.TBoxGraph[new RDFResource("ex:allDisjointClasses"), RDFVocabulary.OWL.MEMBERS, null, null].TriplesCount == 1);
            Assert.IsTrue(classModel.TBoxGraph[null, RDFVocabulary.RDF.TYPE, RDFVocabulary.RDF.LIST, null].TriplesCount == 2);
            Assert.IsTrue(classModel.TBoxGraph[null, RDFVocabulary.RDF.FIRST, new RDFResource("ex:class1"), null].TriplesCount == 1);
            Assert.IsTrue(classModel.TBoxGraph[null, RDFVocabulary.RDF.FIRST, new RDFResource("ex:class2"), null].TriplesCount == 1);
            Assert.IsTrue(classModel.TBoxGraph[null, RDFVocabulary.RDF.REST, null, null].TriplesCount == 2);
            Assert.IsTrue(classModel.TBoxInferenceGraph.TriplesCount == 0);
            Assert.IsTrue(classModel.TBoxVirtualGraph.TriplesCount == 9);
            Assert.IsTrue(classModel.TBoxVirtualGraph[new RDFResource("ex:allDisjointClasses"), RDFVocabulary.RDF.TYPE, RDFVocabulary.OWL.CLASS, null].TriplesCount == 1);
            Assert.IsTrue(classModel.TBoxVirtualGraph[new RDFResource("ex:allDisjointClasses"), RDFVocabulary.RDF.TYPE, RDFVocabulary.OWL.ALL_DISJOINT_CLASSES, null].TriplesCount == 1);
            Assert.IsTrue(classModel.TBoxVirtualGraph[new RDFResource("ex:allDisjointClasses"), RDFVocabulary.OWL.MEMBERS, null, null].TriplesCount == 1);
            Assert.IsTrue(classModel.TBoxVirtualGraph[null, RDFVocabulary.RDF.TYPE, RDFVocabulary.RDF.LIST, null].TriplesCount == 2);
            Assert.IsTrue(classModel.TBoxVirtualGraph[null, RDFVocabulary.RDF.FIRST, new RDFResource("ex:class1"), null].TriplesCount == 1);
            Assert.IsTrue(classModel.TBoxVirtualGraph[null, RDFVocabulary.RDF.FIRST, new RDFResource("ex:class2"), null].TriplesCount == 1);
            Assert.IsTrue(classModel.TBoxVirtualGraph[null, RDFVocabulary.RDF.REST, null, null].TriplesCount == 2);

            int i = 0;
            IEnumerator<RDFResource> allDisjointClassesEnumerator = classModel.AllDisjointClassesEnumerator;
            while (allDisjointClassesEnumerator.MoveNext())
            {
                Assert.IsTrue(allDisjointClassesEnumerator.Current.Equals(new RDFResource("ex:allDisjointClasses")));
                i++;
            }
            Assert.IsTrue(i == 1);
        }

        [TestMethod]
        public void ShouldThrowExceptionOnDeclaringAllDisjointClassesBecauseNullClass()
            => Assert.ThrowsException<RDFSemanticsException>(() => new RDFOntologyClassModel().DeclareAllDisjointClasses(null, new List<RDFResource>() { new RDFResource("ex:class1") }));

        [TestMethod]
        public void ShouldThrowExceptionOnDeclaringAllDisjointClassesBecauseNullClasses()
            => Assert.ThrowsException<RDFSemanticsException>(() => new RDFOntologyClassModel().DeclareAllDisjointClasses(new RDFResource("ex:allDisjointClasses"), null));

        [TestMethod]
        public void ShouldThrowExceptionOnDeclaringAllDisjointClassesBecauseEmptyClasses()
            => Assert.ThrowsException<RDFSemanticsException>(() => new RDFOntologyClassModel().DeclareAllDisjointClasses(new RDFResource("ex:allDisjointClasses"), new List<RDFResource>()));

        [TestMethod]
        public void ShouldAnnotateResourceClass()
        {
            RDFOntologyClassModel classModel = new RDFOntologyClassModel();
            classModel.DeclareClass(new RDFResource("ex:classA"));
            classModel.AnnotateClass(new RDFResource("ex:classA"), RDFVocabulary.RDFS.SEE_ALSO, new RDFResource("ex:seealso"));
            classModel.AnnotateClass(new RDFResource("ex:classA"), RDFVocabulary.RDFS.SEE_ALSO, new RDFResource("ex:seealso")); //Will be discarded, since duplicate annotations are not allowed

            Assert.IsTrue(classModel.ClassesCount == 1);
            Assert.IsTrue(classModel.TBoxGraph.TriplesCount == 2);
            Assert.IsTrue(classModel.TBoxGraph.ContainsTriple(new RDFTriple(new RDFResource("ex:classA"), RDFVocabulary.RDF.TYPE, RDFVocabulary.OWL.CLASS)));
            Assert.IsTrue(classModel.TBoxGraph.ContainsTriple(new RDFTriple(new RDFResource("ex:classA"), RDFVocabulary.RDFS.SEE_ALSO, new RDFResource("ex:seealso"))));
        }

        [TestMethod]
        public void ShouldAnnotateLiteralClass()
        {
            RDFOntologyClassModel classModel = new RDFOntologyClassModel();
            classModel.DeclareClass(new RDFResource("ex:classA"));
            classModel.AnnotateClass(new RDFResource("ex:classA"), RDFVocabulary.RDFS.LABEL, new RDFPlainLiteral("label"));
            classModel.AnnotateClass(new RDFResource("ex:classA"), RDFVocabulary.RDFS.LABEL, new RDFPlainLiteral("label")); //Will be discarded, since duplicate annotations are not allowed

            Assert.IsTrue(classModel.ClassesCount == 1);
            Assert.IsTrue(classModel.TBoxGraph.TriplesCount == 2);
            Assert.IsTrue(classModel.TBoxGraph.ContainsTriple(new RDFTriple(new RDFResource("ex:classA"), RDFVocabulary.RDF.TYPE, RDFVocabulary.OWL.CLASS)));
            Assert.IsTrue(classModel.TBoxGraph.ContainsTriple(new RDFTriple(new RDFResource("ex:classA"), RDFVocabulary.RDFS.LABEL, new RDFPlainLiteral("label"))));
        }

        [TestMethod]
        public void ShouldThrowExceptionOnAnnotatingResourceClassBecauseNullSubject()
            => Assert.ThrowsException<RDFSemanticsException>(() => new RDFOntologyClassModel()
                        .DeclareClass(new RDFResource("ex:class1"))
                        .AnnotateClass(null, RDFVocabulary.RDFS.SEE_ALSO, new RDFResource("ex:seealso")));

        [TestMethod]
        public void ShouldThrowExceptionOnAnnotatingResourceClassBecauseNullPredicate()
            => Assert.ThrowsException<RDFSemanticsException>(() => new RDFOntologyClassModel()
                        .DeclareClass(new RDFResource("ex:class1"))
                        .AnnotateClass(new RDFResource("ex:class1"), null, new RDFResource("ex:seealso")));

        [TestMethod]
        public void ShouldThrowExceptionOnAnnotatingResourceClassBecauseBlankPredicate()
           => Assert.ThrowsException<RDFSemanticsException>(() => new RDFOntologyClassModel()
                       .DeclareClass(new RDFResource("ex:class1"))
                       .AnnotateClass(new RDFResource("ex:class1"), new RDFResource(), new RDFResource("ex:seealso")));

        [TestMethod]
        public void ShouldThrowExceptionOnAnnotatingResourceClassBecauseNullObject()
            => Assert.ThrowsException<RDFSemanticsException>(() => new RDFOntologyClassModel()
                        .DeclareClass(new RDFResource("ex:class1"))
                        .AnnotateClass(new RDFResource("ex:class1"), RDFVocabulary.RDFS.SEE_ALSO, null as RDFResource));

        [TestMethod]
        public void ShouldThrowExceptionOnAnnotatingLiteralClassBecauseNullSubject()
            => Assert.ThrowsException<RDFSemanticsException>(() => new RDFOntologyClassModel()
                        .DeclareClass(new RDFResource("ex:class1"))
                        .AnnotateClass(null, RDFVocabulary.RDFS.LABEL, new RDFPlainLiteral("label")));

        [TestMethod]
        public void ShouldThrowExceptionOnAnnotatingLiteralClassBecauseNullPredicate()
            => Assert.ThrowsException<RDFSemanticsException>(() => new RDFOntologyClassModel()
                        .DeclareClass(new RDFResource("ex:class1"))
                        .AnnotateClass(new RDFResource("ex:class1"), null, new RDFPlainLiteral("label")));

        [TestMethod]
        public void ShouldThrowExceptionOnAnnotatingLiteralClassBecauseBlankPredicate()
           => Assert.ThrowsException<RDFSemanticsException>(() => new RDFOntologyClassModel()
                       .DeclareClass(new RDFResource("ex:class1"))
                       .AnnotateClass(new RDFResource("ex:class1"), new RDFResource(), new RDFPlainLiteral("label")));

        [TestMethod]
        public void ShouldThrowExceptionOnAnnotatingLiteralClassBecauseNullObject()
            => Assert.ThrowsException<RDFSemanticsException>(() => new RDFOntologyClassModel()
                        .DeclareClass(new RDFResource("ex:class1"))
                        .AnnotateClass(new RDFResource("ex:class1"), RDFVocabulary.RDFS.LABEL, null as RDFLiteral));
        #endregion
    }
}