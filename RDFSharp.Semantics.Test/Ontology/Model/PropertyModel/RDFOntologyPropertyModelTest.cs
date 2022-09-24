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
    public class RDFOntologyPropertyModelTest
    {
        #region Test
        [TestMethod]
        public void ShouldCreatePropertyModel()
        {
            RDFOntologyPropertyModel propertyModel = new RDFOntologyPropertyModel();

            Assert.IsNotNull(propertyModel);
            Assert.IsNotNull(propertyModel.Properties);
            Assert.IsTrue(propertyModel.PropertiesCount == 0);
            Assert.IsTrue(propertyModel.AllDisjointPropertiesCount == 0);
            Assert.IsTrue(propertyModel.AnnotationPropertiesCount == 0);
            Assert.IsTrue(propertyModel.AsymmetricPropertiesCount == 0);
            Assert.IsTrue(propertyModel.DatatypePropertiesCount == 0);
            Assert.IsTrue(propertyModel.DeprecatedPropertiesCount == 0);
            Assert.IsTrue(propertyModel.FunctionalPropertiesCount == 0);
            Assert.IsTrue(propertyModel.InverseFunctionalPropertiesCount == 0);
            Assert.IsTrue(propertyModel.IrreflexivePropertiesCount == 0);
            Assert.IsTrue(propertyModel.ObjectPropertiesCount == 0);
            Assert.IsTrue(propertyModel.ReflexivePropertiesCount == 0);
            Assert.IsTrue(propertyModel.SymmetricPropertiesCount == 0);
            Assert.IsTrue(propertyModel.TransitivePropertiesCount == 0);
            Assert.IsNotNull(propertyModel.TBoxGraph);
            Assert.IsTrue(propertyModel.TBoxGraph.TriplesCount == 0);
            Assert.IsNotNull(propertyModel.TBoxInferenceGraph);
            Assert.IsTrue(propertyModel.TBoxInferenceGraph.TriplesCount == 0);
            Assert.IsNotNull(propertyModel.TBoxVirtualGraph);
            Assert.IsTrue(propertyModel.TBoxVirtualGraph.TriplesCount == 0);

            int p = 0;
            IEnumerator<RDFResource> propertiesEnumerator = propertyModel.PropertiesEnumerator;
            while (propertiesEnumerator.MoveNext())
                p++;
            Assert.IsTrue(p == 0);

            int adjp = 0;
            IEnumerator<RDFResource> allDisjointPropertiesEnumerator = propertyModel.AllDisjointPropertiesEnumerator;
            while (allDisjointPropertiesEnumerator.MoveNext())
                adjp++;
            Assert.IsTrue(adjp == 0);

            int anp = 0;
            IEnumerator<RDFResource> annotationPropertiesEnumerator = propertyModel.AnnotationPropertiesEnumerator;
            while (annotationPropertiesEnumerator.MoveNext())
                anp++;
            Assert.IsTrue(anp == 0);

            int asp = 0;
            IEnumerator<RDFResource> asymmetricPropertiesEnumerator = propertyModel.AsymmetricPropertiesEnumerator;
            while (asymmetricPropertiesEnumerator.MoveNext())
                asp++;
            Assert.IsTrue(asp == 0);

            int dtp = 0;
            IEnumerator<RDFResource> datatypePropertiesEnumerator = propertyModel.DatatypePropertiesEnumerator;
            while (datatypePropertiesEnumerator.MoveNext())
                dtp++;
            Assert.IsTrue(dtp == 0);

            int dp = 0;
            IEnumerator<RDFResource> deprecatedPropertiesEnumerator = propertyModel.DeprecatedPropertiesEnumerator;
            while (deprecatedPropertiesEnumerator.MoveNext())
                dp++;
            Assert.IsTrue(dp == 0);

            int fp = 0;
            IEnumerator<RDFResource> functionalPropertiesEnumerator = propertyModel.FunctionalPropertiesEnumerator;
            while (functionalPropertiesEnumerator.MoveNext())
                fp++;
            Assert.IsTrue(fp == 0);

            int ifp = 0;
            IEnumerator<RDFResource> inverseFunctionaPropertiesEnumerator = propertyModel.InverseFunctionalPropertiesEnumerator;
            while (inverseFunctionaPropertiesEnumerator.MoveNext())
                ifp++;
            Assert.IsTrue(ifp == 0);

            int ip = 0;
            IEnumerator<RDFResource> irreflexivePropertiesEnumerator = propertyModel.IrreflexivePropertiesEnumerator;
            while (irreflexivePropertiesEnumerator.MoveNext())
                ip++;
            Assert.IsTrue(ip == 0);

            int obp = 0;
            IEnumerator<RDFResource> objectPropertiesEnumerator = propertyModel.ObjectPropertiesEnumerator;
            while (objectPropertiesEnumerator.MoveNext())
                obp++;
            Assert.IsTrue(obp == 0);

            int rp = 0;
            IEnumerator<RDFResource> reflexivePropertiesEnumerator = propertyModel.ReflexivePropertiesEnumerator;
            while (reflexivePropertiesEnumerator.MoveNext())
                rp++;
            Assert.IsTrue(rp == 0);

            int sp = 0;
            IEnumerator<RDFResource> symmetricPropertiesEnumerator = propertyModel.SymmetricPropertiesEnumerator;
            while (symmetricPropertiesEnumerator.MoveNext())
                sp++;
            Assert.IsTrue(sp == 0);

            int tp = 0;
            IEnumerator<RDFResource> transitivePropertiesEnumerator = propertyModel.TransitivePropertiesEnumerator;
            while (transitivePropertiesEnumerator.MoveNext())
                tp++;
            Assert.IsTrue(tp == 0);
        }

        [TestMethod]
        public void ShouldDeclareAnnotationProperty()
        {
            RDFOntologyPropertyModel propertyModel = new RDFOntologyPropertyModel();
            propertyModel.DeclareAnnotationProperty(new RDFResource("ex:annprop"));

            Assert.IsTrue(propertyModel.PropertiesCount == 1);
            Assert.IsTrue(propertyModel.AllDisjointPropertiesCount == 0);
            Assert.IsTrue(propertyModel.AnnotationPropertiesCount == 1);
            Assert.IsTrue(propertyModel.AsymmetricPropertiesCount == 0);
            Assert.IsTrue(propertyModel.DatatypePropertiesCount == 0);
            Assert.IsTrue(propertyModel.DeprecatedPropertiesCount == 0);
            Assert.IsTrue(propertyModel.FunctionalPropertiesCount == 0);
            Assert.IsTrue(propertyModel.InverseFunctionalPropertiesCount == 0);
            Assert.IsTrue(propertyModel.IrreflexivePropertiesCount == 0);
            Assert.IsTrue(propertyModel.ObjectPropertiesCount == 0);
            Assert.IsTrue(propertyModel.ReflexivePropertiesCount == 0);
            Assert.IsTrue(propertyModel.SymmetricPropertiesCount == 0);
            Assert.IsTrue(propertyModel.TransitivePropertiesCount == 0);
            Assert.IsTrue(propertyModel.TBoxGraph.TriplesCount == 1);
            Assert.IsTrue(propertyModel.TBoxGraph.ContainsTriple(new RDFTriple(new RDFResource("ex:annprop"), RDFVocabulary.RDF.TYPE, RDFVocabulary.OWL.ANNOTATION_PROPERTY)));
            Assert.IsTrue(propertyModel.TBoxInferenceGraph.TriplesCount == 0);
            Assert.IsTrue(propertyModel.TBoxVirtualGraph.TriplesCount == 1);
            Assert.IsTrue(propertyModel.TBoxVirtualGraph.ContainsTriple(new RDFTriple(new RDFResource("ex:annprop"), RDFVocabulary.RDF.TYPE, RDFVocabulary.OWL.ANNOTATION_PROPERTY)));

            int p = 0;
            IEnumerator<RDFResource> propertiesEnumerator = propertyModel.PropertiesEnumerator;
            while (propertiesEnumerator.MoveNext())
            {
                Assert.IsTrue(propertiesEnumerator.Current.Equals(new RDFResource("ex:annprop")));
                p++;
            }
            Assert.IsTrue(p == 1);

            int anp = 0;
            IEnumerator<RDFResource> annotationPropertiesEnumerator = propertyModel.AnnotationPropertiesEnumerator;
            while (annotationPropertiesEnumerator.MoveNext())
            {
                Assert.IsTrue(annotationPropertiesEnumerator.Current.Equals(new RDFResource("ex:annprop")));
                anp++;
            }
            Assert.IsTrue(anp == 1);
        }

        [TestMethod]
        public void ShouldDeclareObjectProperty()
        {
            RDFOntologyPropertyModel propertyModel = new RDFOntologyPropertyModel();
            propertyModel.DeclareObjectProperty(new RDFResource("ex:objprop"));

            Assert.IsTrue(propertyModel.PropertiesCount == 1);
            Assert.IsTrue(propertyModel.AllDisjointPropertiesCount == 0);
            Assert.IsTrue(propertyModel.AnnotationPropertiesCount == 0);
            Assert.IsTrue(propertyModel.AsymmetricPropertiesCount == 0);
            Assert.IsTrue(propertyModel.DatatypePropertiesCount == 0);
            Assert.IsTrue(propertyModel.DeprecatedPropertiesCount == 0);
            Assert.IsTrue(propertyModel.FunctionalPropertiesCount == 0);
            Assert.IsTrue(propertyModel.InverseFunctionalPropertiesCount == 0);
            Assert.IsTrue(propertyModel.IrreflexivePropertiesCount == 0);
            Assert.IsTrue(propertyModel.ObjectPropertiesCount == 1);
            Assert.IsTrue(propertyModel.ReflexivePropertiesCount == 0);
            Assert.IsTrue(propertyModel.SymmetricPropertiesCount == 0);
            Assert.IsTrue(propertyModel.TransitivePropertiesCount == 0);
            Assert.IsTrue(propertyModel.TBoxGraph.TriplesCount == 1);
            Assert.IsTrue(propertyModel.TBoxGraph.ContainsTriple(new RDFTriple(new RDFResource("ex:objprop"), RDFVocabulary.RDF.TYPE, RDFVocabulary.OWL.OBJECT_PROPERTY)));
            Assert.IsTrue(propertyModel.TBoxInferenceGraph.TriplesCount == 0);
            Assert.IsTrue(propertyModel.TBoxVirtualGraph.TriplesCount == 1);
            Assert.IsTrue(propertyModel.TBoxVirtualGraph.ContainsTriple(new RDFTriple(new RDFResource("ex:objprop"), RDFVocabulary.RDF.TYPE, RDFVocabulary.OWL.OBJECT_PROPERTY)));

            int p = 0;
            IEnumerator<RDFResource> propertiesEnumerator = propertyModel.PropertiesEnumerator;
            while (propertiesEnumerator.MoveNext())
            {
                Assert.IsTrue(propertiesEnumerator.Current.Equals(new RDFResource("ex:objprop")));
                p++;
            }
            Assert.IsTrue(p == 1);

            int obp = 0;
            IEnumerator<RDFResource> objectPropertiesEnumerator = propertyModel.ObjectPropertiesEnumerator;
            while (objectPropertiesEnumerator.MoveNext())
            {
                Assert.IsTrue(objectPropertiesEnumerator.Current.Equals(new RDFResource("ex:objprop")));
                obp++;
            }
            Assert.IsTrue(obp == 1);
        }

        [TestMethod]
        public void ShouldDeclareAsymmetricObjectProperty()
        {
            RDFOntologyPropertyModel propertyModel = new RDFOntologyPropertyModel();
            propertyModel.DeclareObjectProperty(new RDFResource("ex:objprop"), new RDFOntologyObjectPropertyBehavior() { Asymmetric = true });

            Assert.IsTrue(propertyModel.PropertiesCount == 1);
            Assert.IsTrue(propertyModel.AllDisjointPropertiesCount == 0);
            Assert.IsTrue(propertyModel.AnnotationPropertiesCount == 0);
            Assert.IsTrue(propertyModel.AsymmetricPropertiesCount == 1);
            Assert.IsTrue(propertyModel.DatatypePropertiesCount == 0);
            Assert.IsTrue(propertyModel.DeprecatedPropertiesCount == 0);
            Assert.IsTrue(propertyModel.FunctionalPropertiesCount == 0);
            Assert.IsTrue(propertyModel.InverseFunctionalPropertiesCount == 0);
            Assert.IsTrue(propertyModel.IrreflexivePropertiesCount == 0);
            Assert.IsTrue(propertyModel.ObjectPropertiesCount == 1);
            Assert.IsTrue(propertyModel.ReflexivePropertiesCount == 0);
            Assert.IsTrue(propertyModel.SymmetricPropertiesCount == 0);
            Assert.IsTrue(propertyModel.TransitivePropertiesCount == 0);
            Assert.IsTrue(propertyModel.TBoxGraph.TriplesCount == 2);
            Assert.IsTrue(propertyModel.TBoxGraph.ContainsTriple(new RDFTriple(new RDFResource("ex:objprop"), RDFVocabulary.RDF.TYPE, RDFVocabulary.OWL.OBJECT_PROPERTY)));
            Assert.IsTrue(propertyModel.TBoxGraph.ContainsTriple(new RDFTriple(new RDFResource("ex:objprop"), RDFVocabulary.RDF.TYPE, RDFVocabulary.OWL.ASYMMETRIC_PROPERTY)));
            Assert.IsTrue(propertyModel.TBoxInferenceGraph.TriplesCount == 0);
            Assert.IsTrue(propertyModel.TBoxVirtualGraph.TriplesCount == 2);
            Assert.IsTrue(propertyModel.TBoxVirtualGraph.ContainsTriple(new RDFTriple(new RDFResource("ex:objprop"), RDFVocabulary.RDF.TYPE, RDFVocabulary.OWL.OBJECT_PROPERTY)));
            Assert.IsTrue(propertyModel.TBoxVirtualGraph.ContainsTriple(new RDFTriple(new RDFResource("ex:objprop"), RDFVocabulary.RDF.TYPE, RDFVocabulary.OWL.ASYMMETRIC_PROPERTY)));

            int p = 0;
            IEnumerator<RDFResource> propertiesEnumerator = propertyModel.PropertiesEnumerator;
            while (propertiesEnumerator.MoveNext())
            {
                Assert.IsTrue(propertiesEnumerator.Current.Equals(new RDFResource("ex:objprop")));
                p++;
            }
            Assert.IsTrue(p == 1);

            int obp = 0;
            IEnumerator<RDFResource> objectPropertiesEnumerator = propertyModel.ObjectPropertiesEnumerator;
            while (objectPropertiesEnumerator.MoveNext())
            {
                Assert.IsTrue(objectPropertiesEnumerator.Current.Equals(new RDFResource("ex:objprop")));
                obp++;
            }
            Assert.IsTrue(obp == 1);

            int ap = 0;
            IEnumerator<RDFResource> asymmetricPropertiesEnumerator = propertyModel.AsymmetricPropertiesEnumerator;
            while (asymmetricPropertiesEnumerator.MoveNext())
            {
                Assert.IsTrue(asymmetricPropertiesEnumerator.Current.Equals(new RDFResource("ex:objprop")));
                ap++;
            }
            Assert.IsTrue(ap == 1);
        }

        [TestMethod]
        public void ShouldDeclareFunctionalObjectProperty()
        {
            RDFOntologyPropertyModel propertyModel = new RDFOntologyPropertyModel();
            propertyModel.DeclareObjectProperty(new RDFResource("ex:objprop"), new RDFOntologyObjectPropertyBehavior() { Functional = true });

            Assert.IsTrue(propertyModel.PropertiesCount == 1);
            Assert.IsTrue(propertyModel.AllDisjointPropertiesCount == 0);
            Assert.IsTrue(propertyModel.AnnotationPropertiesCount == 0);
            Assert.IsTrue(propertyModel.AsymmetricPropertiesCount == 0);
            Assert.IsTrue(propertyModel.DatatypePropertiesCount == 0);
            Assert.IsTrue(propertyModel.DeprecatedPropertiesCount == 0);
            Assert.IsTrue(propertyModel.FunctionalPropertiesCount == 1);
            Assert.IsTrue(propertyModel.InverseFunctionalPropertiesCount == 0);
            Assert.IsTrue(propertyModel.IrreflexivePropertiesCount == 0);
            Assert.IsTrue(propertyModel.ObjectPropertiesCount == 1);
            Assert.IsTrue(propertyModel.ReflexivePropertiesCount == 0);
            Assert.IsTrue(propertyModel.SymmetricPropertiesCount == 0);
            Assert.IsTrue(propertyModel.TransitivePropertiesCount == 0);
            Assert.IsTrue(propertyModel.TBoxGraph.TriplesCount == 2);
            Assert.IsTrue(propertyModel.TBoxGraph.ContainsTriple(new RDFTriple(new RDFResource("ex:objprop"), RDFVocabulary.RDF.TYPE, RDFVocabulary.OWL.OBJECT_PROPERTY)));
            Assert.IsTrue(propertyModel.TBoxGraph.ContainsTriple(new RDFTriple(new RDFResource("ex:objprop"), RDFVocabulary.RDF.TYPE, RDFVocabulary.OWL.FUNCTIONAL_PROPERTY)));
            Assert.IsTrue(propertyModel.TBoxInferenceGraph.TriplesCount == 0);
            Assert.IsTrue(propertyModel.TBoxVirtualGraph.TriplesCount == 2);
            Assert.IsTrue(propertyModel.TBoxVirtualGraph.ContainsTriple(new RDFTriple(new RDFResource("ex:objprop"), RDFVocabulary.RDF.TYPE, RDFVocabulary.OWL.OBJECT_PROPERTY)));
            Assert.IsTrue(propertyModel.TBoxVirtualGraph.ContainsTriple(new RDFTriple(new RDFResource("ex:objprop"), RDFVocabulary.RDF.TYPE, RDFVocabulary.OWL.FUNCTIONAL_PROPERTY)));

            int p = 0;
            IEnumerator<RDFResource> propertiesEnumerator = propertyModel.PropertiesEnumerator;
            while (propertiesEnumerator.MoveNext())
            {
                Assert.IsTrue(propertiesEnumerator.Current.Equals(new RDFResource("ex:objprop")));
                p++;
            }
            Assert.IsTrue(p == 1);

            int obp = 0;
            IEnumerator<RDFResource> objectPropertiesEnumerator = propertyModel.ObjectPropertiesEnumerator;
            while (objectPropertiesEnumerator.MoveNext())
            {
                Assert.IsTrue(objectPropertiesEnumerator.Current.Equals(new RDFResource("ex:objprop")));
                obp++;
            }
            Assert.IsTrue(obp == 1);

            int fp = 0;
            IEnumerator<RDFResource> functionalPropertiesEnumerator = propertyModel.FunctionalPropertiesEnumerator;
            while (functionalPropertiesEnumerator.MoveNext())
            {
                Assert.IsTrue(functionalPropertiesEnumerator.Current.Equals(new RDFResource("ex:objprop")));
                fp++;
            }
            Assert.IsTrue(fp == 1);
        }

        [TestMethod]
        public void ShouldDeclareInverseFunctionalObjectProperty()
        {
            RDFOntologyPropertyModel propertyModel = new RDFOntologyPropertyModel();
            propertyModel.DeclareObjectProperty(new RDFResource("ex:objprop"), new RDFOntologyObjectPropertyBehavior() { InverseFunctional = true });

            Assert.IsTrue(propertyModel.PropertiesCount == 1);
            Assert.IsTrue(propertyModel.AllDisjointPropertiesCount == 0);
            Assert.IsTrue(propertyModel.AnnotationPropertiesCount == 0);
            Assert.IsTrue(propertyModel.AsymmetricPropertiesCount == 0);
            Assert.IsTrue(propertyModel.DatatypePropertiesCount == 0);
            Assert.IsTrue(propertyModel.DeprecatedPropertiesCount == 0);
            Assert.IsTrue(propertyModel.FunctionalPropertiesCount == 0);
            Assert.IsTrue(propertyModel.InverseFunctionalPropertiesCount == 1);
            Assert.IsTrue(propertyModel.IrreflexivePropertiesCount == 0);
            Assert.IsTrue(propertyModel.ObjectPropertiesCount == 1);
            Assert.IsTrue(propertyModel.ReflexivePropertiesCount == 0);
            Assert.IsTrue(propertyModel.SymmetricPropertiesCount == 0);
            Assert.IsTrue(propertyModel.TransitivePropertiesCount == 0);
            Assert.IsTrue(propertyModel.TBoxGraph.TriplesCount == 2);
            Assert.IsTrue(propertyModel.TBoxGraph.ContainsTriple(new RDFTriple(new RDFResource("ex:objprop"), RDFVocabulary.RDF.TYPE, RDFVocabulary.OWL.OBJECT_PROPERTY)));
            Assert.IsTrue(propertyModel.TBoxGraph.ContainsTriple(new RDFTriple(new RDFResource("ex:objprop"), RDFVocabulary.RDF.TYPE, RDFVocabulary.OWL.INVERSE_FUNCTIONAL_PROPERTY)));
            Assert.IsTrue(propertyModel.TBoxInferenceGraph.TriplesCount == 0);
            Assert.IsTrue(propertyModel.TBoxVirtualGraph.TriplesCount == 2);
            Assert.IsTrue(propertyModel.TBoxVirtualGraph.ContainsTriple(new RDFTriple(new RDFResource("ex:objprop"), RDFVocabulary.RDF.TYPE, RDFVocabulary.OWL.OBJECT_PROPERTY)));
            Assert.IsTrue(propertyModel.TBoxVirtualGraph.ContainsTriple(new RDFTriple(new RDFResource("ex:objprop"), RDFVocabulary.RDF.TYPE, RDFVocabulary.OWL.INVERSE_FUNCTIONAL_PROPERTY)));

            int p = 0;
            IEnumerator<RDFResource> propertiesEnumerator = propertyModel.PropertiesEnumerator;
            while (propertiesEnumerator.MoveNext())
            {
                Assert.IsTrue(propertiesEnumerator.Current.Equals(new RDFResource("ex:objprop")));
                p++;
            }
            Assert.IsTrue(p == 1);

            int obp = 0;
            IEnumerator<RDFResource> objectPropertiesEnumerator = propertyModel.ObjectPropertiesEnumerator;
            while (objectPropertiesEnumerator.MoveNext())
            {
                Assert.IsTrue(objectPropertiesEnumerator.Current.Equals(new RDFResource("ex:objprop")));
                obp++;
            }
            Assert.IsTrue(obp == 1);

            int ifp = 0;
            IEnumerator<RDFResource> inverseFunctionalPropertiesEnumerator = propertyModel.InverseFunctionalPropertiesEnumerator;
            while (inverseFunctionalPropertiesEnumerator.MoveNext())
            {
                Assert.IsTrue(inverseFunctionalPropertiesEnumerator.Current.Equals(new RDFResource("ex:objprop")));
                ifp++;
            }
            Assert.IsTrue(ifp == 1);
        }

        [TestMethod]
        public void ShouldDeclareIrreflexiveObjectProperty()
        {
            RDFOntologyPropertyModel propertyModel = new RDFOntologyPropertyModel();
            propertyModel.DeclareObjectProperty(new RDFResource("ex:objprop"), new RDFOntologyObjectPropertyBehavior() { Irreflexive = true });

            Assert.IsTrue(propertyModel.PropertiesCount == 1);
            Assert.IsTrue(propertyModel.AllDisjointPropertiesCount == 0);
            Assert.IsTrue(propertyModel.AnnotationPropertiesCount == 0);
            Assert.IsTrue(propertyModel.AsymmetricPropertiesCount == 0);
            Assert.IsTrue(propertyModel.DatatypePropertiesCount == 0);
            Assert.IsTrue(propertyModel.DeprecatedPropertiesCount == 0);
            Assert.IsTrue(propertyModel.FunctionalPropertiesCount == 0);
            Assert.IsTrue(propertyModel.InverseFunctionalPropertiesCount == 0);
            Assert.IsTrue(propertyModel.IrreflexivePropertiesCount == 1);
            Assert.IsTrue(propertyModel.ObjectPropertiesCount == 1);
            Assert.IsTrue(propertyModel.ReflexivePropertiesCount == 0);
            Assert.IsTrue(propertyModel.SymmetricPropertiesCount == 0);
            Assert.IsTrue(propertyModel.TransitivePropertiesCount == 0);
            Assert.IsTrue(propertyModel.TBoxGraph.TriplesCount == 2);
            Assert.IsTrue(propertyModel.TBoxGraph.ContainsTriple(new RDFTriple(new RDFResource("ex:objprop"), RDFVocabulary.RDF.TYPE, RDFVocabulary.OWL.OBJECT_PROPERTY)));
            Assert.IsTrue(propertyModel.TBoxGraph.ContainsTriple(new RDFTriple(new RDFResource("ex:objprop"), RDFVocabulary.RDF.TYPE, RDFVocabulary.OWL.IRREFLEXIVE_PROPERTY)));
            Assert.IsTrue(propertyModel.TBoxInferenceGraph.TriplesCount == 0);
            Assert.IsTrue(propertyModel.TBoxVirtualGraph.TriplesCount == 2);
            Assert.IsTrue(propertyModel.TBoxVirtualGraph.ContainsTriple(new RDFTriple(new RDFResource("ex:objprop"), RDFVocabulary.RDF.TYPE, RDFVocabulary.OWL.OBJECT_PROPERTY)));
            Assert.IsTrue(propertyModel.TBoxVirtualGraph.ContainsTriple(new RDFTriple(new RDFResource("ex:objprop"), RDFVocabulary.RDF.TYPE, RDFVocabulary.OWL.IRREFLEXIVE_PROPERTY)));

            int p = 0;
            IEnumerator<RDFResource> propertiesEnumerator = propertyModel.PropertiesEnumerator;
            while (propertiesEnumerator.MoveNext())
            {
                Assert.IsTrue(propertiesEnumerator.Current.Equals(new RDFResource("ex:objprop")));
                p++;
            }
            Assert.IsTrue(p == 1);

            int obp = 0;
            IEnumerator<RDFResource> objectPropertiesEnumerator = propertyModel.ObjectPropertiesEnumerator;
            while (objectPropertiesEnumerator.MoveNext())
            {
                Assert.IsTrue(objectPropertiesEnumerator.Current.Equals(new RDFResource("ex:objprop")));
                obp++;
            }
            Assert.IsTrue(obp == 1);

            int ip = 0;
            IEnumerator<RDFResource> irreflexivePropertiesEnumerator = propertyModel.IrreflexivePropertiesEnumerator;
            while (irreflexivePropertiesEnumerator.MoveNext())
            {
                Assert.IsTrue(irreflexivePropertiesEnumerator.Current.Equals(new RDFResource("ex:objprop")));
                ip++;
            }
            Assert.IsTrue(ip == 1);
        }

        [TestMethod]
        public void ShouldDeclareReflexiveObjectProperty()
        {
            RDFOntologyPropertyModel propertyModel = new RDFOntologyPropertyModel();
            propertyModel.DeclareObjectProperty(new RDFResource("ex:objprop"), new RDFOntologyObjectPropertyBehavior() { Reflexive = true });

            Assert.IsTrue(propertyModel.PropertiesCount == 1);
            Assert.IsTrue(propertyModel.AllDisjointPropertiesCount == 0);
            Assert.IsTrue(propertyModel.AnnotationPropertiesCount == 0);
            Assert.IsTrue(propertyModel.AsymmetricPropertiesCount == 0);
            Assert.IsTrue(propertyModel.DatatypePropertiesCount == 0);
            Assert.IsTrue(propertyModel.DeprecatedPropertiesCount == 0);
            Assert.IsTrue(propertyModel.FunctionalPropertiesCount == 0);
            Assert.IsTrue(propertyModel.InverseFunctionalPropertiesCount == 0);
            Assert.IsTrue(propertyModel.IrreflexivePropertiesCount == 0);
            Assert.IsTrue(propertyModel.ObjectPropertiesCount == 1);
            Assert.IsTrue(propertyModel.ReflexivePropertiesCount == 1);
            Assert.IsTrue(propertyModel.SymmetricPropertiesCount == 0);
            Assert.IsTrue(propertyModel.TransitivePropertiesCount == 0);
            Assert.IsTrue(propertyModel.TBoxGraph.TriplesCount == 2);
            Assert.IsTrue(propertyModel.TBoxGraph.ContainsTriple(new RDFTriple(new RDFResource("ex:objprop"), RDFVocabulary.RDF.TYPE, RDFVocabulary.OWL.OBJECT_PROPERTY)));
            Assert.IsTrue(propertyModel.TBoxGraph.ContainsTriple(new RDFTriple(new RDFResource("ex:objprop"), RDFVocabulary.RDF.TYPE, RDFVocabulary.OWL.REFLEXIVE_PROPERTY)));
            Assert.IsTrue(propertyModel.TBoxInferenceGraph.TriplesCount == 0);
            Assert.IsTrue(propertyModel.TBoxVirtualGraph.TriplesCount == 2);
            Assert.IsTrue(propertyModel.TBoxVirtualGraph.ContainsTriple(new RDFTriple(new RDFResource("ex:objprop"), RDFVocabulary.RDF.TYPE, RDFVocabulary.OWL.OBJECT_PROPERTY)));
            Assert.IsTrue(propertyModel.TBoxVirtualGraph.ContainsTriple(new RDFTriple(new RDFResource("ex:objprop"), RDFVocabulary.RDF.TYPE, RDFVocabulary.OWL.REFLEXIVE_PROPERTY)));

            int p = 0;
            IEnumerator<RDFResource> propertiesEnumerator = propertyModel.PropertiesEnumerator;
            while (propertiesEnumerator.MoveNext())
            {
                Assert.IsTrue(propertiesEnumerator.Current.Equals(new RDFResource("ex:objprop")));
                p++;
            }
            Assert.IsTrue(p == 1);

            int obp = 0;
            IEnumerator<RDFResource> objectPropertiesEnumerator = propertyModel.ObjectPropertiesEnumerator;
            while (objectPropertiesEnumerator.MoveNext())
            {
                Assert.IsTrue(objectPropertiesEnumerator.Current.Equals(new RDFResource("ex:objprop")));
                obp++;
            }
            Assert.IsTrue(obp == 1);

            int rp = 0;
            IEnumerator<RDFResource> reflexivePropertiesEnumerator = propertyModel.ReflexivePropertiesEnumerator;
            while (reflexivePropertiesEnumerator.MoveNext())
            {
                Assert.IsTrue(reflexivePropertiesEnumerator.Current.Equals(new RDFResource("ex:objprop")));
                rp++;
            }
            Assert.IsTrue(rp == 1);
        }

        [TestMethod]
        public void ShouldDeclareSymmetricObjectProperty()
        {
            RDFOntologyPropertyModel propertyModel = new RDFOntologyPropertyModel();
            propertyModel.DeclareObjectProperty(new RDFResource("ex:objprop"), new RDFOntologyObjectPropertyBehavior() { Symmetric = true });

            Assert.IsTrue(propertyModel.PropertiesCount == 1);
            Assert.IsTrue(propertyModel.AllDisjointPropertiesCount == 0);
            Assert.IsTrue(propertyModel.AnnotationPropertiesCount == 0);
            Assert.IsTrue(propertyModel.AsymmetricPropertiesCount == 0);
            Assert.IsTrue(propertyModel.DatatypePropertiesCount == 0);
            Assert.IsTrue(propertyModel.DeprecatedPropertiesCount == 0);
            Assert.IsTrue(propertyModel.FunctionalPropertiesCount == 0);
            Assert.IsTrue(propertyModel.InverseFunctionalPropertiesCount == 0);
            Assert.IsTrue(propertyModel.IrreflexivePropertiesCount == 0);
            Assert.IsTrue(propertyModel.ObjectPropertiesCount == 1);
            Assert.IsTrue(propertyModel.ReflexivePropertiesCount == 0);
            Assert.IsTrue(propertyModel.SymmetricPropertiesCount == 1);
            Assert.IsTrue(propertyModel.TransitivePropertiesCount == 0);
            Assert.IsTrue(propertyModel.TBoxGraph.TriplesCount == 2);
            Assert.IsTrue(propertyModel.TBoxGraph.ContainsTriple(new RDFTriple(new RDFResource("ex:objprop"), RDFVocabulary.RDF.TYPE, RDFVocabulary.OWL.OBJECT_PROPERTY)));
            Assert.IsTrue(propertyModel.TBoxGraph.ContainsTriple(new RDFTriple(new RDFResource("ex:objprop"), RDFVocabulary.RDF.TYPE, RDFVocabulary.OWL.SYMMETRIC_PROPERTY)));
            Assert.IsTrue(propertyModel.TBoxInferenceGraph.TriplesCount == 0);
            Assert.IsTrue(propertyModel.TBoxVirtualGraph.TriplesCount == 2);
            Assert.IsTrue(propertyModel.TBoxVirtualGraph.ContainsTriple(new RDFTriple(new RDFResource("ex:objprop"), RDFVocabulary.RDF.TYPE, RDFVocabulary.OWL.OBJECT_PROPERTY)));
            Assert.IsTrue(propertyModel.TBoxVirtualGraph.ContainsTriple(new RDFTriple(new RDFResource("ex:objprop"), RDFVocabulary.RDF.TYPE, RDFVocabulary.OWL.SYMMETRIC_PROPERTY)));

            int p = 0;
            IEnumerator<RDFResource> propertiesEnumerator = propertyModel.PropertiesEnumerator;
            while (propertiesEnumerator.MoveNext())
            {
                Assert.IsTrue(propertiesEnumerator.Current.Equals(new RDFResource("ex:objprop")));
                p++;
            }
            Assert.IsTrue(p == 1);

            int obp = 0;
            IEnumerator<RDFResource> objectPropertiesEnumerator = propertyModel.ObjectPropertiesEnumerator;
            while (objectPropertiesEnumerator.MoveNext())
            {
                Assert.IsTrue(objectPropertiesEnumerator.Current.Equals(new RDFResource("ex:objprop")));
                obp++;
            }
            Assert.IsTrue(obp == 1);

            int sp = 0;
            IEnumerator<RDFResource> symmetricPropertiesEnumerator = propertyModel.SymmetricPropertiesEnumerator;
            while (symmetricPropertiesEnumerator.MoveNext())
            {
                Assert.IsTrue(symmetricPropertiesEnumerator.Current.Equals(new RDFResource("ex:objprop")));
                sp++;
            }
            Assert.IsTrue(sp == 1);
        }

        [TestMethod]
        public void ShouldDeclareTransitiveObjectProperty()
        {
            RDFOntologyPropertyModel propertyModel = new RDFOntologyPropertyModel();
            propertyModel.DeclareObjectProperty(new RDFResource("ex:objprop"), new RDFOntologyObjectPropertyBehavior() { Transitive = true });

            Assert.IsTrue(propertyModel.PropertiesCount == 1);
            Assert.IsTrue(propertyModel.AllDisjointPropertiesCount == 0);
            Assert.IsTrue(propertyModel.AnnotationPropertiesCount == 0);
            Assert.IsTrue(propertyModel.AsymmetricPropertiesCount == 0);
            Assert.IsTrue(propertyModel.DatatypePropertiesCount == 0);
            Assert.IsTrue(propertyModel.DeprecatedPropertiesCount == 0);
            Assert.IsTrue(propertyModel.FunctionalPropertiesCount == 0);
            Assert.IsTrue(propertyModel.InverseFunctionalPropertiesCount == 0);
            Assert.IsTrue(propertyModel.IrreflexivePropertiesCount == 0);
            Assert.IsTrue(propertyModel.ObjectPropertiesCount == 1);
            Assert.IsTrue(propertyModel.ReflexivePropertiesCount == 0);
            Assert.IsTrue(propertyModel.SymmetricPropertiesCount == 0);
            Assert.IsTrue(propertyModel.TransitivePropertiesCount == 1);
            Assert.IsTrue(propertyModel.TBoxGraph.TriplesCount == 2);
            Assert.IsTrue(propertyModel.TBoxGraph.ContainsTriple(new RDFTriple(new RDFResource("ex:objprop"), RDFVocabulary.RDF.TYPE, RDFVocabulary.OWL.OBJECT_PROPERTY)));
            Assert.IsTrue(propertyModel.TBoxGraph.ContainsTriple(new RDFTriple(new RDFResource("ex:objprop"), RDFVocabulary.RDF.TYPE, RDFVocabulary.OWL.TRANSITIVE_PROPERTY)));
            Assert.IsTrue(propertyModel.TBoxInferenceGraph.TriplesCount == 0);
            Assert.IsTrue(propertyModel.TBoxVirtualGraph.TriplesCount == 2);
            Assert.IsTrue(propertyModel.TBoxVirtualGraph.ContainsTriple(new RDFTriple(new RDFResource("ex:objprop"), RDFVocabulary.RDF.TYPE, RDFVocabulary.OWL.OBJECT_PROPERTY)));
            Assert.IsTrue(propertyModel.TBoxVirtualGraph.ContainsTriple(new RDFTriple(new RDFResource("ex:objprop"), RDFVocabulary.RDF.TYPE, RDFVocabulary.OWL.TRANSITIVE_PROPERTY)));

            int p = 0;
            IEnumerator<RDFResource> propertiesEnumerator = propertyModel.PropertiesEnumerator;
            while (propertiesEnumerator.MoveNext())
            {
                Assert.IsTrue(propertiesEnumerator.Current.Equals(new RDFResource("ex:objprop")));
                p++;
            }
            Assert.IsTrue(p == 1);

            int obp = 0;
            IEnumerator<RDFResource> objectPropertiesEnumerator = propertyModel.ObjectPropertiesEnumerator;
            while (objectPropertiesEnumerator.MoveNext())
            {
                Assert.IsTrue(objectPropertiesEnumerator.Current.Equals(new RDFResource("ex:objprop")));
                obp++;
            }
            Assert.IsTrue(obp == 1);

            int tp = 0;
            IEnumerator<RDFResource> transitivePropertiesEnumerator = propertyModel.TransitivePropertiesEnumerator;
            while (transitivePropertiesEnumerator.MoveNext())
            {
                Assert.IsTrue(transitivePropertiesEnumerator.Current.Equals(new RDFResource("ex:objprop")));
                tp++;
            }
            Assert.IsTrue(tp == 1);
        }

        [TestMethod]
        public void ShouldDeclareAllDisjointProperties()
        {
            RDFOntologyPropertyModel propertyModel = new RDFOntologyPropertyModel();
            propertyModel.DeclareAllDisjointProperties(new RDFResource("ex:allDisjointProperties"), new List<RDFResource>() { new RDFResource("ex:objprop1"), new RDFResource("ex:objprop2") });

            Assert.IsTrue(propertyModel.PropertiesCount == 0); //owl:AllDisjointProperties is not considered a property
            Assert.IsTrue(propertyModel.AllDisjointPropertiesCount == 1);
            Assert.IsTrue(propertyModel.AnnotationPropertiesCount == 0);
            Assert.IsTrue(propertyModel.AsymmetricPropertiesCount == 0);
            Assert.IsTrue(propertyModel.DatatypePropertiesCount == 0);
            Assert.IsTrue(propertyModel.DeprecatedPropertiesCount == 0);
            Assert.IsTrue(propertyModel.FunctionalPropertiesCount == 0);
            Assert.IsTrue(propertyModel.InverseFunctionalPropertiesCount == 0);
            Assert.IsTrue(propertyModel.IrreflexivePropertiesCount == 0);
            Assert.IsTrue(propertyModel.ObjectPropertiesCount == 0);
            Assert.IsTrue(propertyModel.ReflexivePropertiesCount == 0);
            Assert.IsTrue(propertyModel.SymmetricPropertiesCount == 0);
            Assert.IsTrue(propertyModel.TransitivePropertiesCount == 0);
            Assert.IsTrue(propertyModel.TBoxGraph.TriplesCount == 8);
            Assert.IsTrue(propertyModel.TBoxGraph[new RDFResource("ex:allDisjointProperties"), RDFVocabulary.RDF.TYPE, RDFVocabulary.OWL.ALL_DISJOINT_PROPERTIES, null].TriplesCount == 1);
            Assert.IsTrue(propertyModel.TBoxGraph[new RDFResource("ex:allDisjointProperties"), RDFVocabulary.OWL.MEMBERS, null, null].TriplesCount == 1);
            Assert.IsTrue(propertyModel.TBoxGraph[null, RDFVocabulary.RDF.TYPE, RDFVocabulary.RDF.LIST, null].TriplesCount == 2);
            Assert.IsTrue(propertyModel.TBoxGraph[null, RDFVocabulary.RDF.FIRST, new RDFResource("ex:objprop1"), null].TriplesCount == 1);
            Assert.IsTrue(propertyModel.TBoxGraph[null, RDFVocabulary.RDF.FIRST, new RDFResource("ex:objprop2"), null].TriplesCount == 1);
            Assert.IsTrue(propertyModel.TBoxGraph[null, RDFVocabulary.RDF.REST, null, null].TriplesCount == 2);
            Assert.IsTrue(propertyModel.TBoxInferenceGraph.TriplesCount == 0);
            Assert.IsTrue(propertyModel.TBoxVirtualGraph.TriplesCount == 8);
            Assert.IsTrue(propertyModel.TBoxVirtualGraph[new RDFResource("ex:allDisjointProperties"), RDFVocabulary.RDF.TYPE, RDFVocabulary.OWL.ALL_DISJOINT_PROPERTIES, null].TriplesCount == 1);
            Assert.IsTrue(propertyModel.TBoxVirtualGraph[new RDFResource("ex:allDisjointProperties"), RDFVocabulary.OWL.MEMBERS, null, null].TriplesCount == 1);
            Assert.IsTrue(propertyModel.TBoxVirtualGraph[null, RDFVocabulary.RDF.TYPE, RDFVocabulary.RDF.LIST, null].TriplesCount == 2);
            Assert.IsTrue(propertyModel.TBoxVirtualGraph[null, RDFVocabulary.RDF.FIRST, new RDFResource("ex:objprop1"), null].TriplesCount == 1);
            Assert.IsTrue(propertyModel.TBoxVirtualGraph[null, RDFVocabulary.RDF.FIRST, new RDFResource("ex:objprop2"), null].TriplesCount == 1);
            Assert.IsTrue(propertyModel.TBoxVirtualGraph[null, RDFVocabulary.RDF.REST, null, null].TriplesCount == 2);

            int adjp = 0;
            IEnumerator<RDFResource> allDisjointPropertiesEnumerator = propertyModel.AllDisjointPropertiesEnumerator;
            while (allDisjointPropertiesEnumerator.MoveNext())
            {
                Assert.IsTrue(allDisjointPropertiesEnumerator.Current.Equals(new RDFResource("ex:allDisjointProperties")));
                adjp++;
            }
            Assert.IsTrue(adjp == 1);
        }

        [TestMethod]
        public void ShouldThrowExceptionOnDeclaringAllDisjointPropertiesBecauseNullProperty()
            => Assert.ThrowsException<RDFSemanticsException>(() => new RDFOntologyPropertyModel().DeclareAllDisjointProperties(null, new List<RDFResource>() { new RDFResource("ex:objprop1") }));

        [TestMethod]
        public void ShouldThrowExceptionOnDeclaringAllDisjointPropertiesBecauseNullProperties()
            => Assert.ThrowsException<RDFSemanticsException>(() => new RDFOntologyPropertyModel().DeclareAllDisjointProperties(new RDFResource("ex:allDisjointProperties"), null));

        [TestMethod]
        public void ShouldThrowExceptionOnDeclaringAllDisjointPropertiesBecauseEmptyProperties()
            => Assert.ThrowsException<RDFSemanticsException>(() => new RDFOntologyPropertyModel().DeclareAllDisjointProperties(new RDFResource("ex:allDisjointProperties"), new List<RDFResource>()));

        [TestMethod]
        public void ShouldDeclareDeprecatedProperty()
        {
            RDFOntologyPropertyModel propertyModel = new RDFOntologyPropertyModel();
            propertyModel.DeclareObjectProperty(new RDFResource("ex:objprop"), new RDFOntologyObjectPropertyBehavior() { Deprecated = true });

            Assert.IsTrue(propertyModel.PropertiesCount == 1);
            Assert.IsTrue(propertyModel.AllDisjointPropertiesCount == 0);
            Assert.IsTrue(propertyModel.AnnotationPropertiesCount == 0);
            Assert.IsTrue(propertyModel.AsymmetricPropertiesCount == 0);
            Assert.IsTrue(propertyModel.DatatypePropertiesCount == 0);
            Assert.IsTrue(propertyModel.DeprecatedPropertiesCount == 1);
            Assert.IsTrue(propertyModel.FunctionalPropertiesCount == 0);
            Assert.IsTrue(propertyModel.InverseFunctionalPropertiesCount == 0);
            Assert.IsTrue(propertyModel.IrreflexivePropertiesCount == 0);
            Assert.IsTrue(propertyModel.ObjectPropertiesCount == 1);
            Assert.IsTrue(propertyModel.ReflexivePropertiesCount == 0);
            Assert.IsTrue(propertyModel.SymmetricPropertiesCount == 0);
            Assert.IsTrue(propertyModel.TransitivePropertiesCount == 0);
            Assert.IsTrue(propertyModel.TBoxGraph.TriplesCount == 2);
            Assert.IsTrue(propertyModel.TBoxGraph.ContainsTriple(new RDFTriple(new RDFResource("ex:objprop"), RDFVocabulary.RDF.TYPE, RDFVocabulary.OWL.OBJECT_PROPERTY)));
            Assert.IsTrue(propertyModel.TBoxGraph.ContainsTriple(new RDFTriple(new RDFResource("ex:objprop"), RDFVocabulary.RDF.TYPE, RDFVocabulary.OWL.DEPRECATED_PROPERTY)));
            Assert.IsTrue(propertyModel.TBoxInferenceGraph.TriplesCount == 0);
            Assert.IsTrue(propertyModel.TBoxVirtualGraph.TriplesCount == 2);
            Assert.IsTrue(propertyModel.TBoxVirtualGraph.ContainsTriple(new RDFTriple(new RDFResource("ex:objprop"), RDFVocabulary.RDF.TYPE, RDFVocabulary.OWL.OBJECT_PROPERTY)));
            Assert.IsTrue(propertyModel.TBoxVirtualGraph.ContainsTriple(new RDFTriple(new RDFResource("ex:objprop"), RDFVocabulary.RDF.TYPE, RDFVocabulary.OWL.DEPRECATED_PROPERTY)));

            int p = 0;
            IEnumerator<RDFResource> propertiesEnumerator = propertyModel.PropertiesEnumerator;
            while (propertiesEnumerator.MoveNext())
            {
                Assert.IsTrue(propertiesEnumerator.Current.Equals(new RDFResource("ex:objprop")));
                p++;
            }
            Assert.IsTrue(p == 1);

            int dp = 0;
            IEnumerator<RDFResource> deprecatedPropertiesEnumerator = propertyModel.DeprecatedPropertiesEnumerator;
            while (deprecatedPropertiesEnumerator.MoveNext())
            {
                Assert.IsTrue(deprecatedPropertiesEnumerator.Current.Equals(new RDFResource("ex:objprop")));
                dp++;
            }
            Assert.IsTrue(dp == 1);

            int obp = 0;
            IEnumerator<RDFResource> objectPropertiesEnumerator = propertyModel.ObjectPropertiesEnumerator;
            while (objectPropertiesEnumerator.MoveNext())
            {
                Assert.IsTrue(objectPropertiesEnumerator.Current.Equals(new RDFResource("ex:objprop")));
                obp++;
            }
            Assert.IsTrue(obp == 1);
        }

        [TestMethod]
        public void ShouldDeclareDatatypeProperty()
        {
            RDFOntologyPropertyModel propertyModel = new RDFOntologyPropertyModel();
            propertyModel.DeclareDatatypeProperty(new RDFResource("ex:dtprop"));

            Assert.IsTrue(propertyModel.PropertiesCount == 1);
            Assert.IsTrue(propertyModel.AllDisjointPropertiesCount == 0);
            Assert.IsTrue(propertyModel.AnnotationPropertiesCount == 0);
            Assert.IsTrue(propertyModel.AsymmetricPropertiesCount == 0);
            Assert.IsTrue(propertyModel.DatatypePropertiesCount == 1);
            Assert.IsTrue(propertyModel.DeprecatedPropertiesCount == 0);
            Assert.IsTrue(propertyModel.FunctionalPropertiesCount == 0);
            Assert.IsTrue(propertyModel.InverseFunctionalPropertiesCount == 0);
            Assert.IsTrue(propertyModel.IrreflexivePropertiesCount == 0);
            Assert.IsTrue(propertyModel.ObjectPropertiesCount == 0);
            Assert.IsTrue(propertyModel.ReflexivePropertiesCount == 0);
            Assert.IsTrue(propertyModel.SymmetricPropertiesCount == 0);
            Assert.IsTrue(propertyModel.TransitivePropertiesCount == 0);
            Assert.IsTrue(propertyModel.TBoxGraph.TriplesCount == 1);
            Assert.IsTrue(propertyModel.TBoxGraph.ContainsTriple(new RDFTriple(new RDFResource("ex:dtprop"), RDFVocabulary.RDF.TYPE, RDFVocabulary.OWL.DATATYPE_PROPERTY)));
            Assert.IsTrue(propertyModel.TBoxInferenceGraph.TriplesCount == 0);
            Assert.IsTrue(propertyModel.TBoxVirtualGraph.TriplesCount == 1);
            Assert.IsTrue(propertyModel.TBoxVirtualGraph.ContainsTriple(new RDFTriple(new RDFResource("ex:dtprop"), RDFVocabulary.RDF.TYPE, RDFVocabulary.OWL.DATATYPE_PROPERTY)));

            int p = 0;
            IEnumerator<RDFResource> propertiesEnumerator = propertyModel.PropertiesEnumerator;
            while (propertiesEnumerator.MoveNext())
            {
                Assert.IsTrue(propertiesEnumerator.Current.Equals(new RDFResource("ex:dtprop")));
                p++;
            }
            Assert.IsTrue(p == 1);

            int obp = 0;
            IEnumerator<RDFResource> datatypePropertiesEnumerator = propertyModel.DatatypePropertiesEnumerator;
            while (datatypePropertiesEnumerator.MoveNext())
            {
                Assert.IsTrue(datatypePropertiesEnumerator.Current.Equals(new RDFResource("ex:dtprop")));
                obp++;
            }
            Assert.IsTrue(obp == 1);
        }     

        [TestMethod]
        public void ShouldThrowExceptionOnDeclaringAnnotationPropertyBecauseNull()
            => Assert.ThrowsException<RDFSemanticsException>(() => new RDFOntologyPropertyModel().DeclareAnnotationProperty(null));

        [TestMethod]
        public void ShouldThrowExceptionOnDeclaringDatatypePropertyBecauseNull()
            => Assert.ThrowsException<RDFSemanticsException>(() => new RDFOntologyPropertyModel().DeclareDatatypeProperty(null));

        [TestMethod]
        public void ShouldThrowExceptionOnDeclaringObjectPropertyBecauseNull()
            => Assert.ThrowsException<RDFSemanticsException>(() => new RDFOntologyPropertyModel().DeclareObjectProperty(null));

        [TestMethod]
        public void ShouldThrowExceptionOnDeclaringObjectPropertyBecauseInvalidBehavior1()
            => Assert.ThrowsException<RDFSemanticsException>(() => new RDFOntologyPropertyModel().DeclareObjectProperty(new RDFResource("ex:objprop"), 
                new RDFOntologyObjectPropertyBehavior() { Symmetric = true, Asymmetric = true }));

        [TestMethod]
        public void ShouldThrowExceptionOnDeclaringObjectPropertyBecauseInvalidBehavior2()
           => Assert.ThrowsException<RDFSemanticsException>(() => new RDFOntologyPropertyModel().DeclareObjectProperty(new RDFResource("ex:objprop"),
               new RDFOntologyObjectPropertyBehavior() { Reflexive = true, Irreflexive = true }));
        #endregion
    }
}