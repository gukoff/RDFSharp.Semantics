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
    public class RDFOntologyPropertyModelHelperTest
    {
        #region Declarer
        [TestMethod]
        public void ShouldCheckHasAnnotationProperty()
        {
            RDFOntologyPropertyModel propertyModel = new RDFOntologyPropertyModel();
            propertyModel.DeclareAnnotationProperty(new RDFResource("ex:annprop"));

            Assert.IsTrue(propertyModel.CheckHasProperty(new RDFResource("ex:annprop")));
            Assert.IsTrue(propertyModel.CheckHasAnnotationProperty(new RDFResource("ex:annprop")));
        }

        [TestMethod]
        public void ShouldCheckHasNotAnnotationProperty()
        {
            RDFOntologyPropertyModel propertyModel = new RDFOntologyPropertyModel();
            propertyModel.DeclareDatatypeProperty(new RDFResource("ex:dtprop"));

            Assert.IsFalse(propertyModel.CheckHasProperty(new RDFResource("ex:dtprop2")));
            Assert.IsFalse(propertyModel.CheckHasProperty(null));
            Assert.IsFalse(propertyModel.CheckHasAnnotationProperty(new RDFResource("ex:dtprop")));
            Assert.IsFalse(new RDFOntologyPropertyModel().CheckHasProperty(new RDFResource("ex:dtprop")));
        }

        [TestMethod]
        public void ShouldCheckHasDatatypeProperty()
        {
            RDFOntologyPropertyModel propertyModel = new RDFOntologyPropertyModel();
            propertyModel.DeclareDatatypeProperty(new RDFResource("ex:dtprop"));

            Assert.IsTrue(propertyModel.CheckHasProperty(new RDFResource("ex:dtprop")));
            Assert.IsTrue(propertyModel.CheckHasDatatypeProperty(new RDFResource("ex:dtprop")));
        }

        [TestMethod]
        public void ShouldCheckHasNotDatatypeProperty()
        {
            RDFOntologyPropertyModel propertyModel = new RDFOntologyPropertyModel();
            propertyModel.DeclareObjectProperty(new RDFResource("ex:objprop"));

            Assert.IsFalse(propertyModel.CheckHasProperty(new RDFResource("ex:objprop2")));
            Assert.IsFalse(propertyModel.CheckHasProperty(null));
            Assert.IsFalse(propertyModel.CheckHasDatatypeProperty(new RDFResource("ex:objprop")));
            Assert.IsFalse(new RDFOntologyPropertyModel().CheckHasProperty(new RDFResource("ex:objprop")));
        }

        [TestMethod]
        public void ShouldCheckHasObjectProperty()
        {
            RDFOntologyPropertyModel propertyModel = new RDFOntologyPropertyModel();
            propertyModel.DeclareObjectProperty(new RDFResource("ex:objprop"));

            Assert.IsTrue(propertyModel.CheckHasProperty(new RDFResource("ex:objprop")));
            Assert.IsTrue(propertyModel.CheckHasObjectProperty(new RDFResource("ex:objprop")));
        }

        [TestMethod]
        public void ShouldCheckHasNotObjectProperty()
        {
            RDFOntologyPropertyModel propertyModel = new RDFOntologyPropertyModel();
            propertyModel.DeclareDatatypeProperty(new RDFResource("ex:dtprop"));

            Assert.IsFalse(propertyModel.CheckHasProperty(new RDFResource("ex:dtprop2")));
            Assert.IsFalse(propertyModel.CheckHasProperty(null));
            Assert.IsFalse(propertyModel.CheckHasObjectProperty(new RDFResource("ex:dtprop")));
            Assert.IsFalse(new RDFOntologyPropertyModel().CheckHasProperty(new RDFResource("ex:dtprop")));
        }

        [TestMethod]
        public void ShouldCheckHasSymmetricObjectProperty()
        {
            RDFOntologyPropertyModel propertyModel = new RDFOntologyPropertyModel();
            propertyModel.DeclareObjectProperty(new RDFResource("ex:symobjprop"), new RDFOntologyObjectPropertyBehavior() { Symmetric = true });

            Assert.IsTrue(propertyModel.CheckHasProperty(new RDFResource("ex:symobjprop")));
            Assert.IsTrue(propertyModel.CheckHasObjectProperty(new RDFResource("ex:symobjprop")));
            Assert.IsTrue(propertyModel.CheckHasSymmetricProperty(new RDFResource("ex:symobjprop")));
        }

        [TestMethod]
        public void ShouldCheckHasNotSymmetricObjectProperty()
        {
            RDFOntologyPropertyModel propertyModel = new RDFOntologyPropertyModel();
            propertyModel.DeclareObjectProperty(new RDFResource("ex:objprop"), new RDFOntologyObjectPropertyBehavior() { Symmetric = false });

            Assert.IsTrue(propertyModel.CheckHasObjectProperty(new RDFResource("ex:objprop")));
            Assert.IsFalse(propertyModel.CheckHasSymmetricProperty(new RDFResource("ex:objprop")));
            Assert.IsFalse(propertyModel.CheckHasSymmetricProperty(null));
        }

        [TestMethod]
        public void ShouldCheckHasAsymmetricObjectProperty()
        {
            RDFOntologyPropertyModel propertyModel = new RDFOntologyPropertyModel();
            propertyModel.DeclareObjectProperty(new RDFResource("ex:asymobjprop"), new RDFOntologyObjectPropertyBehavior() { Asymmetric = true });

            Assert.IsTrue(propertyModel.CheckHasProperty(new RDFResource("ex:asymobjprop")));
            Assert.IsTrue(propertyModel.CheckHasObjectProperty(new RDFResource("ex:asymobjprop")));
            Assert.IsTrue(propertyModel.CheckHasAsymmetricProperty(new RDFResource("ex:asymobjprop")));
        }

        [TestMethod]
        public void ShouldCheckHasNotAsymmetricObjectProperty()
        {
            RDFOntologyPropertyModel propertyModel = new RDFOntologyPropertyModel();
            propertyModel.DeclareObjectProperty(new RDFResource("ex:objprop"), new RDFOntologyObjectPropertyBehavior() { Asymmetric = false });

            Assert.IsTrue(propertyModel.CheckHasObjectProperty(new RDFResource("ex:objprop")));
            Assert.IsFalse(propertyModel.CheckHasAsymmetricProperty(new RDFResource("ex:objprop")));
            Assert.IsFalse(propertyModel.CheckHasAsymmetricProperty(null));
        }

        [TestMethod]
        public void ShouldCheckHasTransitiveObjectProperty()
        {
            RDFOntologyPropertyModel propertyModel = new RDFOntologyPropertyModel();
            propertyModel.DeclareObjectProperty(new RDFResource("ex:trobjprop"), new RDFOntologyObjectPropertyBehavior() { Transitive = true });

            Assert.IsTrue(propertyModel.CheckHasProperty(new RDFResource("ex:trobjprop")));
            Assert.IsTrue(propertyModel.CheckHasObjectProperty(new RDFResource("ex:trobjprop")));
            Assert.IsTrue(propertyModel.CheckHasTransitiveProperty(new RDFResource("ex:trobjprop")));
        }

        [TestMethod]
        public void ShouldCheckHasNotTransitiveObjectProperty()
        {
            RDFOntologyPropertyModel propertyModel = new RDFOntologyPropertyModel();
            propertyModel.DeclareObjectProperty(new RDFResource("ex:objprop"), new RDFOntologyObjectPropertyBehavior() { Transitive = false });

            Assert.IsTrue(propertyModel.CheckHasObjectProperty(new RDFResource("ex:objprop")));
            Assert.IsFalse(propertyModel.CheckHasTransitiveProperty(new RDFResource("ex:objprop")));
            Assert.IsFalse(propertyModel.CheckHasTransitiveProperty(null));
        }

        [TestMethod]
        public void ShouldCheckHasFunctionalObjectProperty()
        {
            RDFOntologyPropertyModel propertyModel = new RDFOntologyPropertyModel();
            propertyModel.DeclareObjectProperty(new RDFResource("ex:fobjprop"), new RDFOntologyObjectPropertyBehavior() { Functional = true });

            Assert.IsTrue(propertyModel.CheckHasProperty(new RDFResource("ex:fobjprop")));
            Assert.IsTrue(propertyModel.CheckHasObjectProperty(new RDFResource("ex:fobjprop")));
            Assert.IsTrue(propertyModel.CheckHasFunctionalProperty(new RDFResource("ex:fobjprop")));
        }

        [TestMethod]
        public void ShouldCheckHasNotFunctionalObjectProperty()
        {
            RDFOntologyPropertyModel propertyModel = new RDFOntologyPropertyModel();
            propertyModel.DeclareObjectProperty(new RDFResource("ex:objprop"), new RDFOntologyObjectPropertyBehavior() { Functional = false });

            Assert.IsTrue(propertyModel.CheckHasObjectProperty(new RDFResource("ex:objprop")));
            Assert.IsFalse(propertyModel.CheckHasFunctionalProperty(new RDFResource("ex:objprop")));
            Assert.IsFalse(propertyModel.CheckHasFunctionalProperty(null));
        }

        [TestMethod]
        public void ShouldCheckHasInverseFunctionalObjectProperty()
        {
            RDFOntologyPropertyModel propertyModel = new RDFOntologyPropertyModel();
            propertyModel.DeclareObjectProperty(new RDFResource("ex:ifobjprop"), new RDFOntologyObjectPropertyBehavior() { InverseFunctional = true });

            Assert.IsTrue(propertyModel.CheckHasProperty(new RDFResource("ex:ifobjprop")));
            Assert.IsTrue(propertyModel.CheckHasObjectProperty(new RDFResource("ex:ifobjprop")));
            Assert.IsTrue(propertyModel.CheckHasInverseFunctionalProperty(new RDFResource("ex:ifobjprop")));
        }

        [TestMethod]
        public void ShouldCheckHasNotInverseFunctionalObjectProperty()
        {
            RDFOntologyPropertyModel propertyModel = new RDFOntologyPropertyModel();
            propertyModel.DeclareObjectProperty(new RDFResource("ex:objprop"), new RDFOntologyObjectPropertyBehavior() { InverseFunctional = false });

            Assert.IsTrue(propertyModel.CheckHasObjectProperty(new RDFResource("ex:objprop")));
            Assert.IsFalse(propertyModel.CheckHasInverseFunctionalProperty(new RDFResource("ex:objprop")));
            Assert.IsFalse(propertyModel.CheckHasInverseFunctionalProperty(null));
        }

        [TestMethod]
        public void ShouldCheckHasReflexiveObjectProperty()
        {
            RDFOntologyPropertyModel propertyModel = new RDFOntologyPropertyModel();
            propertyModel.DeclareObjectProperty(new RDFResource("ex:robjprop"), new RDFOntologyObjectPropertyBehavior() { Reflexive = true });

            Assert.IsTrue(propertyModel.CheckHasProperty(new RDFResource("ex:robjprop")));
            Assert.IsTrue(propertyModel.CheckHasObjectProperty(new RDFResource("ex:robjprop")));
            Assert.IsTrue(propertyModel.CheckHasReflexiveProperty(new RDFResource("ex:robjprop")));
        }

        [TestMethod]
        public void ShouldCheckHasNotReflexiveObjectProperty()
        {
            RDFOntologyPropertyModel propertyModel = new RDFOntologyPropertyModel();
            propertyModel.DeclareObjectProperty(new RDFResource("ex:objprop"), new RDFOntologyObjectPropertyBehavior() { Reflexive = false });

            Assert.IsTrue(propertyModel.CheckHasObjectProperty(new RDFResource("ex:objprop")));
            Assert.IsFalse(propertyModel.CheckHasReflexiveProperty(new RDFResource("ex:objprop")));
            Assert.IsFalse(propertyModel.CheckHasReflexiveProperty(null));
        }

        [TestMethod]
        public void ShouldCheckHasIrreflexiveObjectProperty()
        {
            RDFOntologyPropertyModel propertyModel = new RDFOntologyPropertyModel();
            propertyModel.DeclareObjectProperty(new RDFResource("ex:irobjprop"), new RDFOntologyObjectPropertyBehavior() { Irreflexive = true });

            Assert.IsTrue(propertyModel.CheckHasProperty(new RDFResource("ex:irobjprop")));
            Assert.IsTrue(propertyModel.CheckHasObjectProperty(new RDFResource("ex:irobjprop")));
            Assert.IsTrue(propertyModel.CheckHasIrreflexiveProperty(new RDFResource("ex:irobjprop")));
        }

        [TestMethod]
        public void ShouldCheckHasNotIrreflexiveObjectProperty()
        {
            RDFOntologyPropertyModel propertyModel = new RDFOntologyPropertyModel();
            propertyModel.DeclareObjectProperty(new RDFResource("ex:objprop"), new RDFOntologyObjectPropertyBehavior() { Irreflexive = false });

            Assert.IsTrue(propertyModel.CheckHasObjectProperty(new RDFResource("ex:objprop")));
            Assert.IsFalse(propertyModel.CheckHasIrreflexiveProperty(new RDFResource("ex:objprop")));
            Assert.IsFalse(propertyModel.CheckHasIrreflexiveProperty(null));
        }

        [TestMethod]
        public void ShouldCheckHasDeprecatedProperty()
        {
            RDFOntologyPropertyModel propertyModel = new RDFOntologyPropertyModel();
            propertyModel.DeclareAnnotationProperty(new RDFResource("ex:annprop"), new RDFOntologyAnnotationPropertyBehavior() { Deprecated = true });

            Assert.IsTrue(propertyModel.CheckHasProperty(new RDFResource("ex:annprop")));
            Assert.IsTrue(propertyModel.CheckHasAnnotationProperty(new RDFResource("ex:annprop")));
            Assert.IsTrue(propertyModel.CheckHasDeprecatedProperty(new RDFResource("ex:annprop")));
        }

        [TestMethod]
        public void ShouldCheckHasNotDeprecatedProperty()
        {
            RDFOntologyPropertyModel propertyModel = new RDFOntologyPropertyModel();
            propertyModel.DeclareAnnotationProperty(new RDFResource("ex:annprop"), new RDFOntologyAnnotationPropertyBehavior() { Deprecated = false });

            Assert.IsTrue(propertyModel.CheckHasAnnotationProperty(new RDFResource("ex:annprop")));
            Assert.IsFalse(propertyModel.CheckHasDeprecatedProperty(new RDFResource("ex:annprop")));
            Assert.IsFalse(propertyModel.CheckHasDeprecatedProperty(null));
        }

        [TestMethod]
        public void ShouldCheckHasAnnotationLiteral()
        {
            RDFOntologyPropertyModel propertyModel = new RDFOntologyPropertyModel();
            propertyModel.DeclareObjectProperty(new RDFResource("ex:propertyA"));
            propertyModel.AnnotateProperty(new RDFResource("ex:propertyA"), RDFVocabulary.RDFS.COMMENT, new RDFPlainLiteral("comment"));

            Assert.IsTrue(propertyModel.CheckHasAnnotation(new RDFResource("ex:propertyA"), RDFVocabulary.RDFS.COMMENT, new RDFPlainLiteral("comment")));
        }

        [TestMethod]
        public void ShouldCheckHasAnnotationResource()
        {
            RDFOntologyPropertyModel propertyModel = new RDFOntologyPropertyModel();
            propertyModel.DeclareObjectProperty(new RDFResource("ex:propertyA"));
            propertyModel.AnnotateProperty(new RDFResource("ex:propertyA"), RDFVocabulary.RDFS.SEE_ALSO, new RDFResource("ex:seealso"));

            Assert.IsTrue(propertyModel.CheckHasAnnotation(new RDFResource("ex:propertyA"), RDFVocabulary.RDFS.SEE_ALSO, new RDFResource("ex:seealso")));
        }

        [TestMethod]
        public void ShouldCheckHasNotAnnotationLiteral()
        {
            RDFOntologyPropertyModel propertyModel = new RDFOntologyPropertyModel();
            propertyModel.DeclareObjectProperty(new RDFResource("ex:propertyA"));
            propertyModel.AnnotateProperty(new RDFResource("ex:propertyA"), RDFVocabulary.RDFS.COMMENT, new RDFPlainLiteral("comment"));

            Assert.IsFalse(propertyModel.CheckHasAnnotation(new RDFResource("ex:propertyA"), RDFVocabulary.RDFS.COMMENT, new RDFPlainLiteral("comment", "en")));
        }

        [TestMethod]
        public void ShouldCheckHasNotAnnotationResource()
        {
            RDFOntologyPropertyModel propertyModel = new RDFOntologyPropertyModel();
            propertyModel.DeclareObjectProperty(new RDFResource("ex:propertyA"));
            propertyModel.AnnotateProperty(new RDFResource("ex:propertyA"), RDFVocabulary.RDFS.SEE_ALSO, new RDFResource("ex:seealso"));

            Assert.IsFalse(propertyModel.CheckHasAnnotation(new RDFResource("ex:propertyA"), RDFVocabulary.RDFS.SEE_ALSO, new RDFResource("ex:seealso2")));
        }

        [TestMethod]
        public void ShouldCheckHasPropertyChainAxiom()
        {
            RDFOntologyPropertyModel propertyModel = new RDFOntologyPropertyModel();
            propertyModel.DeclareObjectProperty(new RDFResource("ex:propertyA"));
            propertyModel.DeclareObjectProperty(new RDFResource("ex:propertyB"));
            propertyModel.DeclarePropertyChainAxiom(new RDFResource("ex:propertyChainAxiom"), new List<RDFResource>() { new RDFResource("ex:propertyA"), new RDFResource("ex:propertyB") });

            Assert.IsTrue(propertyModel.CheckHasPropertyChainAxiom(new RDFResource("ex:propertyChainAxiom")));
            Assert.IsFalse(propertyModel.CheckHasPropertyChainAxiom(null));
        }
        #endregion

        #region Analyzer
        [TestMethod]
        public void ShouldCheckAreSubProperties()
        {
            RDFOntologyPropertyModel propertyModel = new RDFOntologyPropertyModel();
            propertyModel.DeclareObjectProperty(new RDFResource("ex:propertyA"));
            propertyModel.DeclareObjectProperty(new RDFResource("ex:propertyB"));
            propertyModel.DeclareObjectProperty(new RDFResource("ex:propertyC"));
            propertyModel.DeclareObjectProperty(new RDFResource("ex:propertyD"));
            propertyModel.DeclareSubProperties(new RDFResource("ex:propertyB"), new RDFResource("ex:propertyA"));
            propertyModel.DeclareSubProperties(new RDFResource("ex:propertyC"), new RDFResource("ex:propertyB"));
            propertyModel.DeclareEquivalentProperties(new RDFResource("ex:propertyC"), new RDFResource("ex:propertyD"));

            Assert.IsTrue(propertyModel.CheckAreSubProperties(new RDFResource("ex:propertyB"), new RDFResource("ex:propertyA")));
            Assert.IsTrue(propertyModel.CheckAreSubProperties(new RDFResource("ex:propertyC"), new RDFResource("ex:propertyB")));
            Assert.IsTrue(propertyModel.CheckAreSubProperties(new RDFResource("ex:propertyC"), new RDFResource("ex:propertyA"))); //Inferred
            Assert.IsTrue(propertyModel.CheckAreSubProperties(new RDFResource("ex:propertyD"), new RDFResource("ex:propertyB"))); //Inferred
            Assert.IsTrue(propertyModel.CheckAreSubProperties(new RDFResource("ex:propertyD"), new RDFResource("ex:propertyA"))); //Inferred
        }

        [TestMethod]
        public void ShouldAnswerSubProperties()
        {
            RDFOntologyPropertyModel propertyModel = new RDFOntologyPropertyModel();
            propertyModel.DeclareObjectProperty(new RDFResource("ex:propertyA"));
            propertyModel.DeclareObjectProperty(new RDFResource("ex:propertyB"));
            propertyModel.DeclareObjectProperty(new RDFResource("ex:propertyC"));
            propertyModel.DeclareObjectProperty(new RDFResource("ex:propertyD"));
            propertyModel.DeclareSubProperties(new RDFResource("ex:propertyB"), new RDFResource("ex:propertyA"));
            propertyModel.DeclareSubProperties(new RDFResource("ex:propertyC"), new RDFResource("ex:propertyB"));
            propertyModel.DeclareEquivalentProperties(new RDFResource("ex:propertyC"), new RDFResource("ex:propertyD"));

            Assert.IsTrue(propertyModel.AnswerSubProperties(new RDFResource("ex:propertyA")).Any(sp => sp.Equals(new RDFResource("ex:propertyB"))));
            Assert.IsTrue(propertyModel.AnswerSubProperties(new RDFResource("ex:propertyA")).Any(sp => sp.Equals(new RDFResource("ex:propertyC")))); //Inferred
            Assert.IsTrue(propertyModel.AnswerSubProperties(new RDFResource("ex:propertyA")).Any(sp => sp.Equals(new RDFResource("ex:propertyD")))); //Inferred
            Assert.IsTrue(propertyModel.AnswerSubProperties(new RDFResource("ex:propertyB")).Any(sp => sp.Equals(new RDFResource("ex:propertyC"))));
            Assert.IsTrue(propertyModel.AnswerSubProperties(new RDFResource("ex:propertyB")).Any(sp => sp.Equals(new RDFResource("ex:propertyD")))); //Inferred
        }

        [TestMethod]
        public void ShouldCheckAreSuperProperties()
        {
            RDFOntologyPropertyModel propertyModel = new RDFOntologyPropertyModel();
            propertyModel.DeclareObjectProperty(new RDFResource("ex:propertyA"));
            propertyModel.DeclareObjectProperty(new RDFResource("ex:propertyB"));
            propertyModel.DeclareObjectProperty(new RDFResource("ex:propertyC"));
            propertyModel.DeclareObjectProperty(new RDFResource("ex:propertyD"));
            propertyModel.DeclareSubProperties(new RDFResource("ex:propertyB"), new RDFResource("ex:propertyA"));
            propertyModel.DeclareSubProperties(new RDFResource("ex:propertyC"), new RDFResource("ex:propertyB"));
            propertyModel.DeclareEquivalentProperties(new RDFResource("ex:propertyC"), new RDFResource("ex:propertyD"));

            Assert.IsTrue(propertyModel.CheckAreSuperProperties(new RDFResource("ex:propertyA"), new RDFResource("ex:propertyB")));
            Assert.IsTrue(propertyModel.CheckAreSuperProperties(new RDFResource("ex:propertyB"), new RDFResource("ex:propertyC")));
            Assert.IsTrue(propertyModel.CheckAreSuperProperties(new RDFResource("ex:propertyA"), new RDFResource("ex:propertyC"))); //Inferred
            Assert.IsTrue(propertyModel.CheckAreSuperProperties(new RDFResource("ex:propertyB"), new RDFResource("ex:propertyD"))); //Inferred
            Assert.IsTrue(propertyModel.CheckAreSuperProperties(new RDFResource("ex:propertyA"), new RDFResource("ex:propertyD"))); //Inferred
        }

        [TestMethod]
        public void ShouldAnswerSuperProperties()
        {
            RDFOntologyPropertyModel propertyModel = new RDFOntologyPropertyModel();
            propertyModel.DeclareObjectProperty(new RDFResource("ex:propertyA"));
            propertyModel.DeclareObjectProperty(new RDFResource("ex:propertyB"));
            propertyModel.DeclareObjectProperty(new RDFResource("ex:propertyC"));
            propertyModel.DeclareObjectProperty(new RDFResource("ex:propertyD"));
            propertyModel.DeclareSubProperties(new RDFResource("ex:propertyB"), new RDFResource("ex:propertyA"));
            propertyModel.DeclareSubProperties(new RDFResource("ex:propertyC"), new RDFResource("ex:propertyB"));
            propertyModel.DeclareEquivalentProperties(new RDFResource("ex:propertyC"), new RDFResource("ex:propertyD"));

            Assert.IsTrue(propertyModel.AnswerSuperProperties(new RDFResource("ex:propertyB")).Any(sp => sp.Equals(new RDFResource("ex:propertyA"))));
            Assert.IsTrue(propertyModel.AnswerSuperProperties(new RDFResource("ex:propertyC")).Any(sp => sp.Equals(new RDFResource("ex:propertyB"))));
            Assert.IsTrue(propertyModel.AnswerSuperProperties(new RDFResource("ex:propertyC")).Any(sp => sp.Equals(new RDFResource("ex:propertyA")))); //Inferred
            Assert.IsTrue(propertyModel.AnswerSuperProperties(new RDFResource("ex:propertyD")).Any(sp => sp.Equals(new RDFResource("ex:propertyB")))); //Inferred
            Assert.IsTrue(propertyModel.AnswerSuperProperties(new RDFResource("ex:propertyD")).Any(sp => sp.Equals(new RDFResource("ex:propertyA")))); //Inferred
        }

        [TestMethod]
        public void ShouldCheckAreEquivalentProperties()
        {
            RDFOntologyPropertyModel propertyModel = new RDFOntologyPropertyModel();
            propertyModel.DeclareObjectProperty(new RDFResource("ex:propertyA"));
            propertyModel.DeclareObjectProperty(new RDFResource("ex:propertyB"));
            propertyModel.DeclareObjectProperty(new RDFResource("ex:propertyC"));
            propertyModel.DeclareEquivalentProperties(new RDFResource("ex:propertyA"), new RDFResource("ex:propertyB"));
            propertyModel.DeclareEquivalentProperties(new RDFResource("ex:propertyB"), new RDFResource("ex:propertyC"));

            Assert.IsTrue(propertyModel.CheckAreEquivalentProperties(new RDFResource("ex:propertyA"), new RDFResource("ex:propertyB")));
            Assert.IsTrue(propertyModel.CheckAreEquivalentProperties(new RDFResource("ex:propertyA"), new RDFResource("ex:propertyC"))); //Inferred
            Assert.IsTrue(propertyModel.CheckAreEquivalentProperties(new RDFResource("ex:propertyB"), new RDFResource("ex:propertyA"))); //Inferred            
            Assert.IsTrue(propertyModel.CheckAreEquivalentProperties(new RDFResource("ex:propertyB"), new RDFResource("ex:propertyC")));
            Assert.IsTrue(propertyModel.CheckAreEquivalentProperties(new RDFResource("ex:propertyC"), new RDFResource("ex:propertyB"))); //Inferred
            Assert.IsTrue(propertyModel.CheckAreEquivalentProperties(new RDFResource("ex:propertyC"), new RDFResource("ex:propertyA"))); //Inferred
        }

        [TestMethod]
        public void ShouldAnswerEquivalentProperties()
        {
            RDFOntologyPropertyModel propertyModel = new RDFOntologyPropertyModel();
            propertyModel.DeclareObjectProperty(new RDFResource("ex:propertyA"));
            propertyModel.DeclareObjectProperty(new RDFResource("ex:propertyB"));
            propertyModel.DeclareObjectProperty(new RDFResource("ex:propertyC"));
            propertyModel.DeclareEquivalentProperties(new RDFResource("ex:propertyA"), new RDFResource("ex:propertyB"));
            propertyModel.DeclareEquivalentProperties(new RDFResource("ex:propertyB"), new RDFResource("ex:propertyC"));

            Assert.IsTrue(propertyModel.AnswerEquivalentProperties(new RDFResource("ex:propertyA")).Any(sp => sp.Equals(new RDFResource("ex:propertyB"))));
            Assert.IsTrue(propertyModel.AnswerEquivalentProperties(new RDFResource("ex:propertyA")).Any(sp => sp.Equals(new RDFResource("ex:propertyC")))); //Inferred
            Assert.IsTrue(propertyModel.AnswerEquivalentProperties(new RDFResource("ex:propertyB")).Any(sp => sp.Equals(new RDFResource("ex:propertyA")))); //Inferred
            Assert.IsTrue(propertyModel.AnswerEquivalentProperties(new RDFResource("ex:propertyB")).Any(sp => sp.Equals(new RDFResource("ex:propertyC"))));
            Assert.IsTrue(propertyModel.AnswerEquivalentProperties(new RDFResource("ex:propertyC")).Any(sp => sp.Equals(new RDFResource("ex:propertyA")))); //Inferred
            Assert.IsTrue(propertyModel.AnswerEquivalentProperties(new RDFResource("ex:propertyC")).Any(sp => sp.Equals(new RDFResource("ex:propertyB")))); //Inferred
        }

        [TestMethod]
        public void ShouldCheckAreDisjointProperties()
        {
            RDFOntologyPropertyModel propertyModel = new RDFOntologyPropertyModel();
            propertyModel.DeclareObjectProperty(new RDFResource("ex:propertyA"));
            propertyModel.DeclareObjectProperty(new RDFResource("ex:propertyB"));
            propertyModel.DeclareObjectProperty(new RDFResource("ex:propertyC"));
            propertyModel.DeclareObjectProperty(new RDFResource("ex:propertyD"));
            propertyModel.DeclareEquivalentProperties(new RDFResource("ex:propertyA"), new RDFResource("ex:propertyB"));
            propertyModel.DeclareDisjointProperties(new RDFResource("ex:propertyB"), new RDFResource("ex:propertyC"));
            propertyModel.DeclareSubProperties(new RDFResource("ex:propertyD"), new RDFResource("ex:propertyC"));

            Assert.IsTrue(propertyModel.CheckAreDisjointProperties(new RDFResource("ex:propertyA"), new RDFResource("ex:propertyC"))); //Inferred
            Assert.IsTrue(propertyModel.CheckAreDisjointProperties(new RDFResource("ex:propertyB"), new RDFResource("ex:propertyC")));
            Assert.IsTrue(propertyModel.CheckAreDisjointProperties(new RDFResource("ex:propertyB"), new RDFResource("ex:propertyD"))); //Inferred
            Assert.IsTrue(propertyModel.CheckAreDisjointProperties(new RDFResource("ex:propertyA"), new RDFResource("ex:propertyD"))); //Inferred
        }

        [TestMethod]
        public void ShouldAnswerDisjointProperties()
        {
            RDFOntologyPropertyModel propertyModel = new RDFOntologyPropertyModel();
            propertyModel.DeclareObjectProperty(new RDFResource("ex:propertyA"));
            propertyModel.DeclareObjectProperty(new RDFResource("ex:propertyB"));
            propertyModel.DeclareObjectProperty(new RDFResource("ex:propertyC"));
            propertyModel.DeclareObjectProperty(new RDFResource("ex:propertyD"));
            propertyModel.DeclareEquivalentProperties(new RDFResource("ex:propertyA"), new RDFResource("ex:propertyB"));
            propertyModel.DeclareDisjointProperties(new RDFResource("ex:propertyB"), new RDFResource("ex:propertyC"));
            propertyModel.DeclareSubProperties(new RDFResource("ex:propertyD"), new RDFResource("ex:propertyC"));

            Assert.IsTrue(propertyModel.AnswerDisjointProperties(new RDFResource("ex:propertyA")).Any(sp => sp.Equals(new RDFResource("ex:propertyC")))); //Inferred
            Assert.IsTrue(propertyModel.AnswerDisjointProperties(new RDFResource("ex:propertyA")).Any(sp => sp.Equals(new RDFResource("ex:propertyD")))); //Inferred
            Assert.IsTrue(propertyModel.AnswerDisjointProperties(new RDFResource("ex:propertyB")).Any(sp => sp.Equals(new RDFResource("ex:propertyC"))));
            Assert.IsTrue(propertyModel.AnswerDisjointProperties(new RDFResource("ex:propertyB")).Any(sp => sp.Equals(new RDFResource("ex:propertyD")))); //Inferred
            Assert.IsTrue(propertyModel.AnswerDisjointProperties(new RDFResource("ex:propertyC")).Any(sp => sp.Equals(new RDFResource("ex:propertyA")))); //Inferred
            Assert.IsTrue(propertyModel.AnswerDisjointProperties(new RDFResource("ex:propertyC")).Any(sp => sp.Equals(new RDFResource("ex:propertyB")))); //Inferred
            Assert.IsTrue(propertyModel.AnswerDisjointProperties(new RDFResource("ex:propertyD")).Any(sp => sp.Equals(new RDFResource("ex:propertyA")))); //Inferred
            Assert.IsTrue(propertyModel.AnswerDisjointProperties(new RDFResource("ex:propertyD")).Any(sp => sp.Equals(new RDFResource("ex:propertyB")))); //Inferred
        }

        [TestMethod]
        public void ShouldCheckAreInverseProperties()
        {
            RDFOntologyPropertyModel propertyModel = new RDFOntologyPropertyModel();
            propertyModel.DeclareObjectProperty(new RDFResource("ex:propertyA"));
            propertyModel.DeclareObjectProperty(new RDFResource("ex:propertyB"));
            propertyModel.DeclareInverseProperties(new RDFResource("ex:propertyA"), new RDFResource("ex:propertyB"));

            Assert.IsTrue(propertyModel.CheckAreInverseProperties(new RDFResource("ex:propertyA"), new RDFResource("ex:propertyB")));
            Assert.IsTrue(propertyModel.CheckAreInverseProperties(new RDFResource("ex:propertyB"), new RDFResource("ex:propertyA"))); //Inferred
        }

        [TestMethod]
        public void ShouldAnswerInverseProperties()
        {
            RDFOntologyPropertyModel propertyModel = new RDFOntologyPropertyModel();
            propertyModel.DeclareObjectProperty(new RDFResource("ex:propertyA"));
            propertyModel.DeclareObjectProperty(new RDFResource("ex:propertyB"));
            propertyModel.DeclareInverseProperties(new RDFResource("ex:propertyA"), new RDFResource("ex:propertyB"));

            Assert.IsTrue(propertyModel.AnswerInverseProperties(new RDFResource("ex:propertyA")).Any(sp => sp.Equals(new RDFResource("ex:propertyB"))));
            Assert.IsTrue(propertyModel.AnswerInverseProperties(new RDFResource("ex:propertyB")).Any(sp => sp.Equals(new RDFResource("ex:propertyA")))); //Inferred
        }

        [TestMethod]
        public void ShouldAnswerChainAxiomProperties()
        {
            RDFOntologyPropertyModel propertyModel = new RDFOntologyPropertyModel();
            propertyModel.DeclareObjectProperty(new RDFResource("ex:propertyA"));
            propertyModel.DeclareObjectProperty(new RDFResource("ex:propertyB"));
            propertyModel.DeclarePropertyChainAxiom(new RDFResource("ex:propertyChainAxiom"), new List<RDFResource>() { new RDFResource("ex:propertyA"), new RDFResource("ex:propertyB") });

            Assert.IsTrue(propertyModel.AnswerChainAxiomProperties(new RDFResource("ex:propertyChainAxiom")).Any(sp => sp.Equals(new RDFResource("ex:propertyA"))));
            Assert.IsTrue(propertyModel.AnswerChainAxiomProperties(new RDFResource("ex:propertyChainAxiom")).Any(sp => sp.Equals(new RDFResource("ex:propertyB"))));
        }
        #endregion

        #region Checker

        #endregion
    }
}