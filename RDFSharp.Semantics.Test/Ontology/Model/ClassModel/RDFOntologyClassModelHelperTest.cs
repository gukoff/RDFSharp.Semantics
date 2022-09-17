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
    public class RDFOntologyClassModelHelperTest
    {
        #region Declarer
        [TestMethod]
        public void ShouldCheckHasClass()
        {
            RDFOntologyClassModel classModel = new RDFOntologyClassModel();
            classModel.DeclareClass(new RDFResource("ex:classA"));

            Assert.IsTrue(classModel.CheckHasClass(new RDFResource("ex:classA")));
        }

        [TestMethod]
        public void ShouldCheckHasNotClass()
        {
            RDFOntologyClassModel classModel = new RDFOntologyClassModel();
            classModel.DeclareClass(new RDFResource("ex:classA"));

            Assert.IsFalse(classModel.CheckHasClass(new RDFResource("ex:classB")));
            Assert.IsFalse(classModel.CheckHasClass(null));
            Assert.IsFalse(new RDFOntologyClassModel().CheckHasClass(new RDFResource("ex:classA")));
        }

        [TestMethod]
        public void ShouldCheckHasDeprecatedClass()
        {
            RDFOntologyClassModel classModel = new RDFOntologyClassModel();
            classModel.DeclareClass(new RDFResource("ex:classA"), new RDFOntologyClassBehavior() { Deprecated = true });

            Assert.IsTrue(classModel.CheckHasDeprecatedClass(new RDFResource("ex:classA")));
        }

        [TestMethod]
        public void ShouldCheckHasNotDeprecatedClass()
        {
            RDFOntologyClassModel classModel = new RDFOntologyClassModel();
            classModel.DeclareClass(new RDFResource("ex:classA"), new RDFOntologyClassBehavior() { Deprecated = false });
            classModel.DeclareClass(new RDFResource("ex:classB"));

            Assert.IsFalse(classModel.CheckHasDeprecatedClass(new RDFResource("ex:classA")));
            Assert.IsFalse(classModel.CheckHasDeprecatedClass(new RDFResource("ex:classB")));
            Assert.IsFalse(classModel.CheckHasDeprecatedClass(null));
        }

        [TestMethod]
        public void ShouldCheckHasRestrictionClass()
        {
            RDFOntologyClassModel classModel = new RDFOntologyClassModel();
            classModel.DeclareRestriction(new RDFResource("ex:restr"), new RDFResource("ex:onprop"));

            Assert.IsTrue(classModel.CheckHasRestrictionClass(new RDFResource("ex:restr")));
        }

        [TestMethod]
        public void ShouldCheckHasNotRestrictionClass()
        {
            RDFOntologyClassModel classModel = new RDFOntologyClassModel();
            classModel.DeclareRestriction(new RDFResource("ex:restr"), new RDFResource("ex:onprop"));

            Assert.IsFalse(classModel.CheckHasRestrictionClass(new RDFResource("ex:restr2")));
            Assert.IsFalse(classModel.CheckHasRestrictionClass(null));
        }

        [TestMethod]
        public void ShouldCheckHasEnumerateClass()
        {
            RDFOntologyClassModel classModel = new RDFOntologyClassModel();
            classModel.DeclareEnumerateClass(new RDFResource("ex:enumClass"), new List<RDFResource>() { new RDFResource("ex:onprop") });

            Assert.IsTrue(classModel.CheckHasEnumerateClass(new RDFResource("ex:enumClass")));
        }

        [TestMethod]
        public void ShouldCheckHasNotEnumerateClass()
        {
            RDFOntologyClassModel classModel = new RDFOntologyClassModel();
            classModel.DeclareEnumerateClass(new RDFResource("ex:enumClass"), new List<RDFResource>() { new RDFResource("ex:onprop") });

            Assert.IsFalse(classModel.CheckHasEnumerateClass(new RDFResource("ex:enumClass2")));
            Assert.IsFalse(classModel.CheckHasEnumerateClass(null));
        }

        [TestMethod]
        public void ShouldCheckHasCompositeClass()
        {
            RDFOntologyClassModel classModel = new RDFOntologyClassModel();
            classModel.DeclareUnionClass(new RDFResource("ex:unionClass"), new List<RDFResource>() { new RDFResource("ex:classA") });

            Assert.IsTrue(classModel.CheckHasCompositeClass(new RDFResource("ex:unionClass")));
        }

        [TestMethod]
        public void ShouldCheckHasNotCompositeClass()
        {
            RDFOntologyClassModel classModel = new RDFOntologyClassModel();
            classModel.DeclareUnionClass(new RDFResource("ex:unionClass"), new List<RDFResource>() { new RDFResource("ex:classA") });

            Assert.IsFalse(classModel.CheckHasCompositeClass(new RDFResource("ex:unionClass2")));
            Assert.IsFalse(classModel.CheckHasCompositeClass(null));
        }

        [TestMethod]
        public void ShouldCheckHasCompositeUnionClass()
        {
            RDFOntologyClassModel classModel = new RDFOntologyClassModel();
            classModel.DeclareUnionClass(new RDFResource("ex:unionClass"), new List<RDFResource>() { new RDFResource("ex:classA") });

            Assert.IsTrue(classModel.CheckHasCompositeUnionClass(new RDFResource("ex:unionClass")));
        }

        [TestMethod]
        public void ShouldCheckHasNotCompositeUnionClass()
        {
            RDFOntologyClassModel classModel = new RDFOntologyClassModel();
            classModel.DeclareUnionClass(new RDFResource("ex:unionClass"), new List<RDFResource>() { new RDFResource("ex:classA") });

            Assert.IsFalse(classModel.CheckHasCompositeUnionClass(new RDFResource("ex:unionClass2")));
            Assert.IsFalse(classModel.CheckHasCompositeUnionClass(null));
        }

        [TestMethod]
        public void ShouldCheckHasCompositeIntersectionClass()
        {
            RDFOntologyClassModel classModel = new RDFOntologyClassModel();
            classModel.DeclareIntersectionClass(new RDFResource("ex:intersectionClass"), new List<RDFResource>() { new RDFResource("ex:classA") });

            Assert.IsTrue(classModel.CheckHasCompositeIntersectionClass(new RDFResource("ex:intersectionClass")));
        }

        [TestMethod]
        public void ShouldCheckHasNotCompositeIntersectionClass()
        {
            RDFOntologyClassModel classModel = new RDFOntologyClassModel();
            classModel.DeclareIntersectionClass(new RDFResource("ex:intersectionClass"), new List<RDFResource>() { new RDFResource("ex:classA") });

            Assert.IsFalse(classModel.CheckHasCompositeIntersectionClass(new RDFResource("ex:intersectionClass2")));
            Assert.IsFalse(classModel.CheckHasCompositeIntersectionClass(null));
        }

        [TestMethod]
        public void ShouldCheckHasCompositeComplementClass()
        {
            RDFOntologyClassModel classModel = new RDFOntologyClassModel();
            classModel.DeclareComplementClass(new RDFResource("ex:complementClass"), new RDFResource("ex:classA"));

            Assert.IsTrue(classModel.CheckHasCompositeComplementClass(new RDFResource("ex:complementClass")));
        }

        [TestMethod]
        public void ShouldCheckHasNotCompositeComplementClass()
        {
            RDFOntologyClassModel classModel = new RDFOntologyClassModel();
            classModel.DeclareComplementClass(new RDFResource("ex:complementClass"), new RDFResource("ex:classA"));

            Assert.IsFalse(classModel.CheckHasCompositeComplementClass(new RDFResource("ex:complementClass2")));
            Assert.IsFalse(classModel.CheckHasCompositeComplementClass(null));
        }

        [TestMethod]
        public void ShouldCheckHasAllValuesFromRestrictionClass()
        {
            RDFOntologyClassModel classModel = new RDFOntologyClassModel();
            classModel.DeclareAllValuesFromRestriction(new RDFResource("ex:restr"), new RDFResource("ex:onprop"), new RDFResource("ex:onclass"));

            Assert.IsTrue(classModel.CheckHasAllValuesFromRestrictionClass(new RDFResource("ex:restr")));
        }

        [TestMethod]
        public void ShouldCheckHasNotAllValuesFromRestrictionClass()
        {
            RDFOntologyClassModel classModel = new RDFOntologyClassModel();
            classModel.DeclareAllValuesFromRestriction(new RDFResource("ex:restr"), new RDFResource("ex:onprop"), new RDFResource("ex:onclass"));

            Assert.IsFalse(classModel.CheckHasAllValuesFromRestrictionClass(new RDFResource("ex:restr2")));
            Assert.IsFalse(classModel.CheckHasAllValuesFromRestrictionClass(null));
        }

        [TestMethod]
        public void ShouldCheckHasSomeValuesFromRestrictionClass()
        {
            RDFOntologyClassModel classModel = new RDFOntologyClassModel();
            classModel.DeclareSomeValuesFromRestriction(new RDFResource("ex:restr"), new RDFResource("ex:onprop"), new RDFResource("ex:onclass"));

            Assert.IsTrue(classModel.CheckHasSomeValuesFromRestrictionClass(new RDFResource("ex:restr")));
        }

        [TestMethod]
        public void ShouldCheckHasNotSomeValuesFromRestrictionClass()
        {
            RDFOntologyClassModel classModel = new RDFOntologyClassModel();
            classModel.DeclareSomeValuesFromRestriction(new RDFResource("ex:restr"), new RDFResource("ex:onprop"), new RDFResource("ex:onclass"));

            Assert.IsFalse(classModel.CheckHasSomeValuesFromRestrictionClass(new RDFResource("ex:restr2")));
            Assert.IsFalse(classModel.CheckHasSomeValuesFromRestrictionClass(null));
        }

        [TestMethod]
        public void ShouldCheckHasValueRestrictionClass()
        {
            RDFOntologyClassModel classModel = new RDFOntologyClassModel();
            classModel.DeclareHasValueRestriction(new RDFResource("ex:restr"), new RDFResource("ex:onprop"), new RDFResource("ex:indiv1"));

            Assert.IsTrue(classModel.CheckHasValueRestrictionClass(new RDFResource("ex:restr")));
        }

        [TestMethod]
        public void ShouldCheckHasNotValueRestrictionClass()
        {
            RDFOntologyClassModel classModel = new RDFOntologyClassModel();
            classModel.DeclareHasValueRestriction(new RDFResource("ex:restr"), new RDFResource("ex:onprop"), new RDFResource("ex:indiv1"));

            Assert.IsFalse(classModel.CheckHasValueRestrictionClass(new RDFResource("ex:restr2")));
            Assert.IsFalse(classModel.CheckHasValueRestrictionClass(null));
        }

        [TestMethod]
        public void ShouldCheckHasSelfRestrictionClass()
        {
            RDFOntologyClassModel classModel = new RDFOntologyClassModel();
            classModel.DeclareHasSelfRestriction(new RDFResource("ex:restr"), new RDFResource("ex:onprop"), true);

            Assert.IsTrue(classModel.CheckHasSelfRestrictionClass(new RDFResource("ex:restr")));
        }

        [TestMethod]
        public void ShouldCheckHasNotSelfRestrictionClass()
        {
            RDFOntologyClassModel classModel = new RDFOntologyClassModel();
            classModel.DeclareHasSelfRestriction(new RDFResource("ex:restr"), new RDFResource("ex:onprop"), true);

            Assert.IsFalse(classModel.CheckHasSelfRestrictionClass(new RDFResource("ex:restr2")));
            Assert.IsFalse(classModel.CheckHasSelfRestrictionClass(null));
        }

        [TestMethod]
        public void ShouldCheckHasCardinalityRestrictionClass()
        {
            RDFOntologyClassModel classModel = new RDFOntologyClassModel();
            classModel.DeclareCardinalityRestriction(new RDFResource("ex:restr"), new RDFResource("ex:onprop"), 1);

            Assert.IsTrue(classModel.CheckHasCardinalityRestrictionClass(new RDFResource("ex:restr")));
        }

        [TestMethod]
        public void ShouldCheckHasNotCardinalityRestrictionClass()
        {
            RDFOntologyClassModel classModel = new RDFOntologyClassModel();
            classModel.DeclareCardinalityRestriction(new RDFResource("ex:restr"), new RDFResource("ex:onprop"), 1);

            Assert.IsFalse(classModel.CheckHasCardinalityRestrictionClass(new RDFResource("ex:restr2")));
            Assert.IsFalse(classModel.CheckHasCardinalityRestrictionClass(null));
        }

        [TestMethod]
        public void ShouldCheckHasMinCardinalityRestrictionClass()
        {
            RDFOntologyClassModel classModel = new RDFOntologyClassModel();
            classModel.DeclareMinCardinalityRestriction(new RDFResource("ex:restr"), new RDFResource("ex:onprop"), 1);

            Assert.IsTrue(classModel.CheckHasMinCardinalityRestrictionClass(new RDFResource("ex:restr")));
        }

        [TestMethod]
        public void ShouldCheckHasNotMinCardinalityRestrictionClass()
        {
            RDFOntologyClassModel classModel = new RDFOntologyClassModel();
            classModel.DeclareMinCardinalityRestriction(new RDFResource("ex:restr"), new RDFResource("ex:onprop"), 1);

            Assert.IsFalse(classModel.CheckHasMinCardinalityRestrictionClass(new RDFResource("ex:restr2")));
            Assert.IsFalse(classModel.CheckHasMinCardinalityRestrictionClass(null));
        }

        [TestMethod]
        public void ShouldCheckHasNotMinCardinalityRestrictionClassBecauseAlsoMax()
        {
            RDFOntologyClassModel classModel = new RDFOntologyClassModel();
            classModel.DeclareMinMaxCardinalityRestriction(new RDFResource("ex:restr"), new RDFResource("ex:onprop"), 1, 2);

            Assert.IsFalse(classModel.CheckHasMinCardinalityRestrictionClass(new RDFResource("ex:restr")));
        }

        [TestMethod]
        public void ShouldCheckHasMaxCardinalityRestrictionClass()
        {
            RDFOntologyClassModel classModel = new RDFOntologyClassModel();
            classModel.DeclareMaxCardinalityRestriction(new RDFResource("ex:restr"), new RDFResource("ex:onprop"), 1);

            Assert.IsTrue(classModel.CheckHasMaxCardinalityRestrictionClass(new RDFResource("ex:restr")));
        }

        [TestMethod]
        public void ShouldCheckHasNotMaxCardinalityRestrictionClass()
        {
            RDFOntologyClassModel classModel = new RDFOntologyClassModel();
            classModel.DeclareMaxCardinalityRestriction(new RDFResource("ex:restr"), new RDFResource("ex:onprop"), 1);

            Assert.IsFalse(classModel.CheckHasMaxCardinalityRestrictionClass(new RDFResource("ex:restr2")));
            Assert.IsFalse(classModel.CheckHasMaxCardinalityRestrictionClass(null));
        }

        [TestMethod]
        public void ShouldCheckHasNotMaxCardinalityRestrictionClassBecauseAlsoMin()
        {
            RDFOntologyClassModel classModel = new RDFOntologyClassModel();
            classModel.DeclareMinMaxCardinalityRestriction(new RDFResource("ex:restr"), new RDFResource("ex:onprop"), 1, 2);

            Assert.IsFalse(classModel.CheckHasMaxCardinalityRestrictionClass(new RDFResource("ex:restr")));
        }

        [TestMethod]
        public void ShouldCheckHasMinMaxCardinalityRestrictionClass()
        {
            RDFOntologyClassModel classModel = new RDFOntologyClassModel();
            classModel.DeclareMinMaxCardinalityRestriction(new RDFResource("ex:restr"), new RDFResource("ex:onprop"), 1, 2);

            Assert.IsTrue(classModel.CheckHasMinMaxCardinalityRestrictionClass(new RDFResource("ex:restr")));
        }

        [TestMethod]
        public void ShouldCheckHasNotMinMaxCardinalityRestrictionClass()
        {
            RDFOntologyClassModel classModel = new RDFOntologyClassModel();
            classModel.DeclareMinMaxCardinalityRestriction(new RDFResource("ex:restr"), new RDFResource("ex:onprop"), 1, 2);

            Assert.IsFalse(classModel.CheckHasMinMaxCardinalityRestrictionClass(new RDFResource("ex:restr2")));
            Assert.IsFalse(classModel.CheckHasMinMaxCardinalityRestrictionClass(null));
        }

        [TestMethod]
        public void ShouldCheckHasNotMinMaxCardinalityRestrictionClassBecauseNotMax()
        {
            RDFOntologyClassModel classModel = new RDFOntologyClassModel();
            classModel.DeclareMinCardinalityRestriction(new RDFResource("ex:restr"), new RDFResource("ex:onprop"), 1);

            Assert.IsFalse(classModel.CheckHasMinMaxCardinalityRestrictionClass(new RDFResource("ex:restr")));
        }

        [TestMethod]
        public void ShouldCheckHasNotMinMaxCardinalityRestrictionClassBecauseNotMin()
        {
            RDFOntologyClassModel classModel = new RDFOntologyClassModel();
            classModel.DeclareMaxCardinalityRestriction(new RDFResource("ex:restr"), new RDFResource("ex:onprop"), 1);

            Assert.IsFalse(classModel.CheckHasMinMaxCardinalityRestrictionClass(new RDFResource("ex:restr")));
        }

        [TestMethod]
        public void ShouldCheckHasQualifiedCardinalityRestrictionClass()
        {
            RDFOntologyClassModel classModel = new RDFOntologyClassModel();
            classModel.DeclareQualifiedCardinalityRestriction(new RDFResource("ex:restr"), new RDFResource("ex:onprop"), 1, new RDFResource("ex:onclass"));

            Assert.IsTrue(classModel.CheckHasQualifiedCardinalityRestrictionClass(new RDFResource("ex:restr")));
        }

        [TestMethod]
        public void ShouldCheckHasNotQualifiedCardinalityRestrictionClass()
        {
            RDFOntologyClassModel classModel = new RDFOntologyClassModel();
            classModel.DeclareQualifiedCardinalityRestriction(new RDFResource("ex:restr"), new RDFResource("ex:onprop"), 1, new RDFResource("ex:onclass"));

            Assert.IsFalse(classModel.CheckHasQualifiedCardinalityRestrictionClass(new RDFResource("ex:restr2")));
            Assert.IsFalse(classModel.CheckHasQualifiedCardinalityRestrictionClass(null));
        }

        [TestMethod]
        public void ShouldCheckHasMinQualifiedCardinalityRestrictionClass()
        {
            RDFOntologyClassModel classModel = new RDFOntologyClassModel();
            classModel.DeclareMinQualifiedCardinalityRestriction(new RDFResource("ex:restr"), new RDFResource("ex:onprop"), 1, new RDFResource("ex:onclass"));

            Assert.IsTrue(classModel.CheckHasMinQualifiedCardinalityRestrictionClass(new RDFResource("ex:restr")));
        }

        [TestMethod]
        public void ShouldCheckHasNotMinQualifiedCardinalityRestrictionClass()
        {
            RDFOntologyClassModel classModel = new RDFOntologyClassModel();
            classModel.DeclareMinQualifiedCardinalityRestriction(new RDFResource("ex:restr"), new RDFResource("ex:onprop"), 1, new RDFResource("ex:onclass"));

            Assert.IsFalse(classModel.CheckHasMinQualifiedCardinalityRestrictionClass(new RDFResource("ex:restr2")));
            Assert.IsFalse(classModel.CheckHasMinQualifiedCardinalityRestrictionClass(null));
        }

        [TestMethod]
        public void ShouldCheckHasNotMinQualifiedCardinalityRestrictionClassBecauseAlsoMax()
        {
            RDFOntologyClassModel classModel = new RDFOntologyClassModel();
            classModel.DeclareMinMaxQualifiedCardinalityRestriction(new RDFResource("ex:restr"), new RDFResource("ex:onprop"), 1, 2, new RDFResource("ex:onclass"));

            Assert.IsFalse(classModel.CheckHasMinQualifiedCardinalityRestrictionClass(new RDFResource("ex:restr")));
        }

        [TestMethod]
        public void ShouldCheckHasMaxQualifiedCardinalityRestrictionClass()
        {
            RDFOntologyClassModel classModel = new RDFOntologyClassModel();
            classModel.DeclareMaxQualifiedCardinalityRestriction(new RDFResource("ex:restr"), new RDFResource("ex:onprop"), 1, new RDFResource("ex:onclass"));

            Assert.IsTrue(classModel.CheckHasMaxQualifiedCardinalityRestrictionClass(new RDFResource("ex:restr")));
        }

        [TestMethod]
        public void ShouldCheckHasNotMaxQualifiedCardinalityRestrictionClass()
        {
            RDFOntologyClassModel classModel = new RDFOntologyClassModel();
            classModel.DeclareMaxQualifiedCardinalityRestriction(new RDFResource("ex:restr"), new RDFResource("ex:onprop"), 1, new RDFResource("ex:onclass"));

            Assert.IsFalse(classModel.CheckHasMaxQualifiedCardinalityRestrictionClass(new RDFResource("ex:restr2")));
            Assert.IsFalse(classModel.CheckHasMaxQualifiedCardinalityRestrictionClass(null));
        }

        [TestMethod]
        public void ShouldCheckHasNotMaxQualifiedCardinalityRestrictionClassBecauseAlsoMin()
        {
            RDFOntologyClassModel classModel = new RDFOntologyClassModel();
            classModel.DeclareMinMaxQualifiedCardinalityRestriction(new RDFResource("ex:restr"), new RDFResource("ex:onprop"), 1, 2, new RDFResource("ex:onclass"));

            Assert.IsFalse(classModel.CheckHasMaxQualifiedCardinalityRestrictionClass(new RDFResource("ex:restr")));
        }

        [TestMethod]
        public void ShouldCheckHasMinMaxQualifiedCardinalityRestrictionClass()
        {
            RDFOntologyClassModel classModel = new RDFOntologyClassModel();
            classModel.DeclareMinMaxQualifiedCardinalityRestriction(new RDFResource("ex:restr"), new RDFResource("ex:onprop"), 1, 2, new RDFResource("ex:onclass"));

            Assert.IsTrue(classModel.CheckHasMinMaxQualifiedCardinalityRestrictionClass(new RDFResource("ex:restr")));
        }

        [TestMethod]
        public void ShouldCheckHasNotMinMaxQualifiedCardinalityRestrictionClass()
        {
            RDFOntologyClassModel classModel = new RDFOntologyClassModel();
            classModel.DeclareMinMaxQualifiedCardinalityRestriction(new RDFResource("ex:restr"), new RDFResource("ex:onprop"), 1, 2, new RDFResource("ex:onclass"));

            Assert.IsFalse(classModel.CheckHasMinMaxQualifiedCardinalityRestrictionClass(new RDFResource("ex:restr2")));
            Assert.IsFalse(classModel.CheckHasMinMaxQualifiedCardinalityRestrictionClass(null));
        }

        [TestMethod]
        public void ShouldCheckHasNotMinMaxQualifiedCardinalityRestrictionClassBecauseNotMax()
        {
            RDFOntologyClassModel classModel = new RDFOntologyClassModel();
            classModel.DeclareMinQualifiedCardinalityRestriction(new RDFResource("ex:restr"), new RDFResource("ex:onprop"), 1, new RDFResource("ex:onclass"));

            Assert.IsFalse(classModel.CheckHasMinMaxQualifiedCardinalityRestrictionClass(new RDFResource("ex:restr")));
        }

        [TestMethod]
        public void ShouldCheckHasNotMinMaxQualifiedCardinalityRestrictionClassBecauseNotMin()
        {
            RDFOntologyClassModel classModel = new RDFOntologyClassModel();
            classModel.DeclareMaxQualifiedCardinalityRestriction(new RDFResource("ex:restr"), new RDFResource("ex:onprop"), 1, new RDFResource("ex:onclass"));

            Assert.IsFalse(classModel.CheckHasMinMaxQualifiedCardinalityRestrictionClass(new RDFResource("ex:restr")));
        }

        [TestMethod]
        public void ShouldCheckHasDisjointUnionClass()
        {
            RDFOntologyClassModel classModel = new RDFOntologyClassModel();
            classModel.DeclareDisjointUnionClass(new RDFResource("ex:disjointUnionClass"), new List<RDFResource>() { new RDFResource("ex:classA"), new RDFResource("ex:classB") });

            Assert.IsTrue(classModel.CheckHasDisjointUnionClass(new RDFResource("ex:disjointUnionClass")));
        }

        [TestMethod]
        public void ShouldCheckHasNotDisjointUnionClass()
        {
            RDFOntologyClassModel classModel = new RDFOntologyClassModel();
            classModel.DeclareDisjointUnionClass(new RDFResource("ex:disjointUnionClass"), new List<RDFResource>() { new RDFResource("ex:classA"), new RDFResource("ex:classB") });

            Assert.IsFalse(classModel.CheckHasDisjointUnionClass(new RDFResource("ex:disjointUnionClass2")));
            Assert.IsFalse(classModel.CheckHasDisjointUnionClass(null));
        }

        [TestMethod]
        public void ShouldCheckHasAllDisjointClasses()
        {
            RDFOntologyClassModel classModel = new RDFOntologyClassModel();
            classModel.DeclareAllDisjointClasses(new RDFResource("ex:allDisjointClasses"), new List<RDFResource>() { new RDFResource("ex:classA"), new RDFResource("ex:classB") });

            Assert.IsTrue(classModel.CheckHasAllDisjointClasses(new RDFResource("ex:allDisjointClasses")));
        }

        [TestMethod]
        public void ShouldCheckHasNotAllDisjointClasses()
        {
            RDFOntologyClassModel classModel = new RDFOntologyClassModel();
            classModel.DeclareAllDisjointClasses(new RDFResource("ex:allDisjointClasses"), new List<RDFResource>() { new RDFResource("ex:classA"), new RDFResource("ex:classB") });

            Assert.IsFalse(classModel.CheckHasAllDisjointClasses(new RDFResource("ex:allDisjointClasses2")));
            Assert.IsFalse(classModel.CheckHasAllDisjointClasses(null));
        }

        [TestMethod]
        public void ShouldCheckHasSimpleClass()
        {
            RDFOntologyClassModel classModel = new RDFOntologyClassModel();
            classModel.DeclareClass(new RDFResource("ex:classA"));

            Assert.IsTrue(classModel.CheckHasSimpleClass(new RDFResource("ex:classA")));
        }

        [TestMethod]
        public void ShouldCheckHasNotSimpleClassBecauseRestriction()
        {
            RDFOntologyClassModel classModel = new RDFOntologyClassModel();
            classModel.DeclareRestriction(new RDFResource("ex:classA"), new RDFResource("ex:onprop"));

            Assert.IsFalse(classModel.CheckHasSimpleClass(new RDFResource("ex:classA")));
        }

        [TestMethod]
        public void ShouldCheckHasNotSimpleClassBecauseEnumerate()
        {
            RDFOntologyClassModel classModel = new RDFOntologyClassModel();
            classModel.DeclareEnumerateClass(new RDFResource("ex:classA"), new List<RDFResource>() { new RDFResource("ex:indiv1") });

            Assert.IsFalse(classModel.CheckHasSimpleClass(new RDFResource("ex:classA")));
        }

        [TestMethod]
        public void ShouldCheckHasNotSimpleClassBecauseComposite()
        {
            RDFOntologyClassModel classModel = new RDFOntologyClassModel();
            classModel.DeclareIntersectionClass(new RDFResource("ex:classA"), new List<RDFResource>() { new RDFResource("ex:class1") });

            Assert.IsFalse(classModel.CheckHasSimpleClass(new RDFResource("ex:classA")));
        }

        [TestMethod]
        public void ShouldCheckHasNotSimpleClassBecauseDisjointUnion()
        {
            RDFOntologyClassModel classModel = new RDFOntologyClassModel();
            classModel.DeclareDisjointUnionClass(new RDFResource("ex:classA"), new List<RDFResource>() { new RDFResource("ex:class1") });

            Assert.IsFalse(classModel.CheckHasSimpleClass(new RDFResource("ex:classA")));
        }

        [TestMethod]
        public void ShouldCheckHasNotSimpleClassBecauseAllDisjointClasses()
        {
            RDFOntologyClassModel classModel = new RDFOntologyClassModel();
            classModel.DeclareAllDisjointClasses(new RDFResource("ex:classA"), new List<RDFResource>() { new RDFResource("ex:class1") });

            Assert.IsFalse(classModel.CheckHasSimpleClass(new RDFResource("ex:classA")));
        }

        [TestMethod]
        public void ShouldCheckHasAnnotationLiteral()
        {
            RDFOntologyClassModel classModel = new RDFOntologyClassModel();
            classModel.DeclareClass(new RDFResource("ex:classA"));
            classModel.AnnotateClass(new RDFResource("ex:classA"), RDFVocabulary.RDFS.COMMENT, new RDFPlainLiteral("comment"));

            Assert.IsTrue(classModel.CheckHasAnnotation(new RDFResource("ex:classA"), RDFVocabulary.RDFS.COMMENT, new RDFPlainLiteral("comment")));
        }

        [TestMethod]
        public void ShouldCheckHasAnnotationResource()
        {
            RDFOntologyClassModel classModel = new RDFOntologyClassModel();
            classModel.DeclareClass(new RDFResource("ex:classA"));
            classModel.AnnotateClass(new RDFResource("ex:classA"), RDFVocabulary.RDFS.SEE_ALSO, new RDFResource("ex:seealso"));

            Assert.IsTrue(classModel.CheckHasAnnotation(new RDFResource("ex:classA"), RDFVocabulary.RDFS.SEE_ALSO, new RDFResource("ex:seealso")));
        }

        [TestMethod]
        public void ShouldCheckHasNotAnnotationLiteral()
        {
            RDFOntologyClassModel classModel = new RDFOntologyClassModel();
            classModel.DeclareClass(new RDFResource("ex:classA"));
            classModel.AnnotateClass(new RDFResource("ex:classA"), RDFVocabulary.RDFS.COMMENT, new RDFPlainLiteral("comment"));

            Assert.IsFalse(classModel.CheckHasAnnotation(new RDFResource("ex:classA"), RDFVocabulary.RDFS.COMMENT, new RDFPlainLiteral("comment", "en")));
        }

        [TestMethod]
        public void ShouldCheckHasNotAnnotationResource()
        {
            RDFOntologyClassModel classModel = new RDFOntologyClassModel();
            classModel.DeclareClass(new RDFResource("ex:classA"));
            classModel.AnnotateClass(new RDFResource("ex:classA"), RDFVocabulary.RDFS.SEE_ALSO, new RDFResource("ex:seealso"));

            Assert.IsFalse(classModel.CheckHasAnnotation(new RDFResource("ex:classA"), RDFVocabulary.RDFS.SEE_ALSO, new RDFResource("ex:seealso2")));
        }
        #endregion

        #region Analyzer
        [TestMethod]
        public void ShouldCheckAreSubClasses()
        {
            RDFOntologyClassModel classModel = new RDFOntologyClassModel();
            classModel.DeclareClass(new RDFResource("ex:classA"));
            classModel.DeclareClass(new RDFResource("ex:classB"));
            classModel.DeclareClass(new RDFResource("ex:classC"));
            classModel.DeclareClass(new RDFResource("ex:classD"));
            classModel.DeclareSubClasses(new RDFResource("ex:classB"), new RDFResource("ex:classA"));
            classModel.DeclareSubClasses(new RDFResource("ex:classC"), new RDFResource("ex:classB"));
            classModel.DeclareEquivalentClasses(new RDFResource("ex:classC"), new RDFResource("ex:classD"));

            Assert.IsTrue(classModel.CheckAreSubClasses(new RDFResource("ex:classB"), new RDFResource("ex:classA")));
            Assert.IsTrue(classModel.CheckAreSubClasses(new RDFResource("ex:classC"), new RDFResource("ex:classB")));
            Assert.IsTrue(classModel.CheckAreSubClasses(new RDFResource("ex:classC"), new RDFResource("ex:classA"))); //Inferred
            Assert.IsTrue(classModel.CheckAreSubClasses(new RDFResource("ex:classD"), new RDFResource("ex:classB"))); //Inferred
            Assert.IsTrue(classModel.CheckAreSubClasses(new RDFResource("ex:classD"), new RDFResource("ex:classA"))); //Inferred
        }

        [TestMethod]
        public void ShouldCheckAreSuperClasses()
        {
            RDFOntologyClassModel classModel = new RDFOntologyClassModel();
            classModel.DeclareClass(new RDFResource("ex:classA"));
            classModel.DeclareClass(new RDFResource("ex:classB"));
            classModel.DeclareClass(new RDFResource("ex:classC"));
            classModel.DeclareClass(new RDFResource("ex:classD"));
            classModel.DeclareSubClasses(new RDFResource("ex:classB"), new RDFResource("ex:classA"));
            classModel.DeclareSubClasses(new RDFResource("ex:classC"), new RDFResource("ex:classB"));
            classModel.DeclareEquivalentClasses(new RDFResource("ex:classC"), new RDFResource("ex:classD"));

            Assert.IsTrue(classModel.CheckAreSuperClasses(new RDFResource("ex:classA"), new RDFResource("ex:classB")));
            Assert.IsTrue(classModel.CheckAreSuperClasses(new RDFResource("ex:classB"), new RDFResource("ex:classC")));
            Assert.IsTrue(classModel.CheckAreSuperClasses(new RDFResource("ex:classA"), new RDFResource("ex:classC"))); //Inferred
            Assert.IsTrue(classModel.CheckAreSuperClasses(new RDFResource("ex:classB"), new RDFResource("ex:classD"))); //Inferred
            Assert.IsTrue(classModel.CheckAreSuperClasses(new RDFResource("ex:classA"), new RDFResource("ex:classD"))); //Inferred
        }
        #endregion

        #region Checker

        #endregion
    }
}