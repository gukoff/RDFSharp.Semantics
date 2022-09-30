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

        [TestMethod]
        public void ShouldAnnotateResourceProperty()
        {
            RDFOntologyPropertyModel propertyModel = new RDFOntologyPropertyModel();
            propertyModel.DeclareObjectProperty(new RDFResource("ex:propertyA"));
            propertyModel.AnnotateProperty(new RDFResource("ex:propertyA"), RDFVocabulary.RDFS.SEE_ALSO, new RDFResource("ex:seealso"));
            propertyModel.AnnotateProperty(new RDFResource("ex:propertyA"), RDFVocabulary.RDFS.SEE_ALSO, new RDFResource("ex:seealso")); //Will be discarded, since duplicate annotations are not allowed

            Assert.IsTrue(propertyModel.PropertiesCount == 1);
            Assert.IsTrue(propertyModel.TBoxGraph.TriplesCount == 2);
            Assert.IsTrue(propertyModel.TBoxGraph.ContainsTriple(new RDFTriple(new RDFResource("ex:propertyA"), RDFVocabulary.RDF.TYPE, RDFVocabulary.OWL.OBJECT_PROPERTY)));
            Assert.IsTrue(propertyModel.TBoxGraph.ContainsTriple(new RDFTriple(new RDFResource("ex:propertyA"), RDFVocabulary.RDFS.SEE_ALSO, new RDFResource("ex:seealso"))));
        }

        [TestMethod]
        public void ShouldAnnotateLiteralProperty()
        {
            RDFOntologyPropertyModel propertyModel = new RDFOntologyPropertyModel();
            propertyModel.DeclareObjectProperty(new RDFResource("ex:propertyA"));
            propertyModel.AnnotateProperty(new RDFResource("ex:propertyA"), RDFVocabulary.RDFS.LABEL, new RDFPlainLiteral("label"));
            propertyModel.AnnotateProperty(new RDFResource("ex:propertyA"), RDFVocabulary.RDFS.LABEL, new RDFPlainLiteral("label")); //Will be discarded, since duplicate annotations are not allowed

            Assert.IsTrue(propertyModel.PropertiesCount == 1);
            Assert.IsTrue(propertyModel.TBoxGraph.TriplesCount == 2);
            Assert.IsTrue(propertyModel.TBoxGraph.ContainsTriple(new RDFTriple(new RDFResource("ex:propertyA"), RDFVocabulary.RDF.TYPE, RDFVocabulary.OWL.OBJECT_PROPERTY)));
            Assert.IsTrue(propertyModel.TBoxGraph.ContainsTriple(new RDFTriple(new RDFResource("ex:propertyA"), RDFVocabulary.RDFS.LABEL, new RDFPlainLiteral("label"))));
        }

        [TestMethod]
        public void ShouldThrowExceptionOnAnnotatingResourcePropertyBecauseNullSubject()
            => Assert.ThrowsException<RDFSemanticsException>(() => new RDFOntologyPropertyModel()
                        .DeclareObjectProperty(new RDFResource("ex:property1"))
                        .AnnotateProperty(null, RDFVocabulary.RDFS.SEE_ALSO, new RDFResource("ex:seealso")));

        [TestMethod]
        public void ShouldThrowExceptionOnAnnotatingResourcePropertyBecauseNullPredicate()
            => Assert.ThrowsException<RDFSemanticsException>(() => new RDFOntologyPropertyModel()
                        .DeclareObjectProperty(new RDFResource("ex:property1"))
                        .AnnotateProperty(new RDFResource("ex:property1"), null, new RDFResource("ex:seealso")));

        [TestMethod]
        public void ShouldThrowExceptionOnAnnotatingResourcePropertyBecauseBlankPredicate()
           => Assert.ThrowsException<RDFSemanticsException>(() => new RDFOntologyPropertyModel()
                       .DeclareObjectProperty(new RDFResource("ex:property1"))
                       .AnnotateProperty(new RDFResource("ex:property1"), new RDFResource(), new RDFResource("ex:seealso")));

        [TestMethod]
        public void ShouldThrowExceptionOnAnnotatingResourcePropertyBecauseNullObject()
            => Assert.ThrowsException<RDFSemanticsException>(() => new RDFOntologyPropertyModel()
                        .DeclareObjectProperty(new RDFResource("ex:property1"))
                        .AnnotateProperty(new RDFResource("ex:property1"), RDFVocabulary.RDFS.SEE_ALSO, null as RDFResource));

        [TestMethod]
        public void ShouldThrowExceptionOnAnnotatingLiteralPropertyBecauseNullSubject()
            => Assert.ThrowsException<RDFSemanticsException>(() => new RDFOntologyPropertyModel()
                        .DeclareObjectProperty(new RDFResource("ex:property1"))
                        .AnnotateProperty(null, RDFVocabulary.RDFS.LABEL, new RDFPlainLiteral("label")));

        [TestMethod]
        public void ShouldThrowExceptionOnAnnotatingLiteralPropertyBecauseNullPredicate()
            => Assert.ThrowsException<RDFSemanticsException>(() => new RDFOntologyPropertyModel()
                        .DeclareObjectProperty(new RDFResource("ex:property1"))
                        .AnnotateProperty(new RDFResource("ex:property1"), null, new RDFPlainLiteral("label")));

        [TestMethod]
        public void ShouldThrowExceptionOnAnnotatingLiteralPropertyBecauseBlankPredicate()
           => Assert.ThrowsException<RDFSemanticsException>(() => new RDFOntologyPropertyModel()
                       .DeclareObjectProperty(new RDFResource("ex:property1"))
                       .AnnotateProperty(new RDFResource("ex:property1"), new RDFResource(), new RDFPlainLiteral("label")));
        [TestMethod]
        public void ShouldThrowExceptionOnAnnotatingLiteralPropertyBecauseNullObject()
            => Assert.ThrowsException<RDFSemanticsException>(() => new RDFOntologyPropertyModel()
                        .DeclareObjectProperty(new RDFResource("ex:property1"))
                        .AnnotateProperty(new RDFResource("ex:property1"), RDFVocabulary.RDFS.LABEL, null as RDFLiteral));

        [TestMethod]
        public void ShouldDeclareSubProperties()
        {
            RDFOntologyPropertyModel propertyModel = new RDFOntologyPropertyModel();
            propertyModel.DeclareObjectProperty(new RDFResource("ex:propertyA"));
            propertyModel.DeclareObjectProperty(new RDFResource("ex:propertyB"));
            propertyModel.DeclareSubProperties(new RDFResource("ex:propertyA"), new RDFResource("ex:propertyB"));

            Assert.IsTrue(propertyModel.TBoxGraph.TriplesCount == 3);
            Assert.IsTrue(propertyModel.TBoxGraph.ContainsTriple(new RDFTriple(new RDFResource("ex:propertyA"), RDFVocabulary.RDF.TYPE, RDFVocabulary.OWL.OBJECT_PROPERTY)));
            Assert.IsTrue(propertyModel.TBoxGraph.ContainsTriple(new RDFTriple(new RDFResource("ex:propertyB"), RDFVocabulary.RDF.TYPE, RDFVocabulary.OWL.OBJECT_PROPERTY)));
            Assert.IsTrue(propertyModel.TBoxGraph.ContainsTriple(new RDFTriple(new RDFResource("ex:propertyA"), RDFVocabulary.RDFS.SUB_PROPERTY_OF, new RDFResource("ex:propertyB"))));
        }

        [TestMethod]
        public void ShouldEmitWarningOnDeclaringSubPropertiesBecauseIncompatibleProperties()
        {
            string warningMsg = null;
            RDFSemanticsEvents.OnSemanticsWarning += (string msg) => { warningMsg = msg; };

            RDFOntologyPropertyModel propertyModel = new RDFOntologyPropertyModel();
            propertyModel.DeclareObjectProperty(new RDFResource("ex:propertyA"));
            propertyModel.DeclareObjectProperty(new RDFResource("ex:propertyB"));
            propertyModel.DeclareSubProperties(new RDFResource("ex:propertyB"), new RDFResource("ex:propertyA"));
            propertyModel.DeclareSubProperties(new RDFResource("ex:propertyA"), new RDFResource("ex:propertyB"));  //OWL-DL contraddiction (enforced by policy)

            Assert.IsNotNull(warningMsg);
            Assert.IsTrue(warningMsg.IndexOf("SubProperty relation between property 'ex:propertyA' and property 'ex:propertyB' cannot be declared to the model because it would violate OWL-DL integrity") > -1);
            Assert.IsTrue(propertyModel.TBoxGraph.ContainsTriple(new RDFTriple(new RDFResource("ex:propertyA"), RDFVocabulary.RDF.TYPE, RDFVocabulary.OWL.OBJECT_PROPERTY)));
            Assert.IsTrue(propertyModel.TBoxGraph.ContainsTriple(new RDFTriple(new RDFResource("ex:propertyB"), RDFVocabulary.RDF.TYPE, RDFVocabulary.OWL.OBJECT_PROPERTY)));
            Assert.IsTrue(propertyModel.TBoxGraph.ContainsTriple(new RDFTriple(new RDFResource("ex:propertyB"), RDFVocabulary.RDFS.SUB_PROPERTY_OF, new RDFResource("ex:propertyA"))));
        }

        [TestMethod]
        public void ShouldThrowExceptionOnDeclaringSubPropertiesBecauseNullChildProperty()
            => Assert.ThrowsException<RDFSemanticsException>(() => new RDFOntologyPropertyModel()
                        .DeclareObjectProperty(new RDFResource("ex:propertyA"))
                        .DeclareObjectProperty(new RDFResource("ex:propertyB"))
                        .DeclareSubProperties(null, new RDFResource("ex:propertyB")));

        [TestMethod]
        public void ShouldThrowExceptionOnDeclaringSubPropertiesBecauseNullMotherProperty()
            => Assert.ThrowsException<RDFSemanticsException>(() => new RDFOntologyPropertyModel()
                        .DeclareObjectProperty(new RDFResource("ex:propertyA"))
                        .DeclareObjectProperty(new RDFResource("ex:propertyB"))
                        .DeclareSubProperties(new RDFResource("ex:propertyA"), null));

        [TestMethod]
        public void ShouldThrowExceptionOnDeclaringSubPropertiesBecauseSelfProperty()
            => Assert.ThrowsException<RDFSemanticsException>(() => new RDFOntologyPropertyModel()
                        .DeclareObjectProperty(new RDFResource("ex:propertyA"))
                        .DeclareSubProperties(new RDFResource("ex:propertyA"), new RDFResource("ex:propertyA")));

        [TestMethod]
        public void ShouldDeclareEquivalentProperties()
        {
            RDFOntologyPropertyModel propertyModel = new RDFOntologyPropertyModel();
            propertyModel.DeclareObjectProperty(new RDFResource("ex:propertyA"));
            propertyModel.DeclareObjectProperty(new RDFResource("ex:propertyB"));
            propertyModel.DeclareEquivalentProperties(new RDFResource("ex:propertyA"), new RDFResource("ex:propertyB"));

            Assert.IsTrue(propertyModel.TBoxGraph.TriplesCount == 3);
            Assert.IsTrue(propertyModel.TBoxGraph.ContainsTriple(new RDFTriple(new RDFResource("ex:propertyA"), RDFVocabulary.RDF.TYPE, RDFVocabulary.OWL.OBJECT_PROPERTY)));
            Assert.IsTrue(propertyModel.TBoxGraph.ContainsTriple(new RDFTriple(new RDFResource("ex:propertyB"), RDFVocabulary.RDF.TYPE, RDFVocabulary.OWL.OBJECT_PROPERTY)));
            Assert.IsTrue(propertyModel.TBoxGraph.ContainsTriple(new RDFTriple(new RDFResource("ex:propertyA"), RDFVocabulary.OWL.EQUIVALENT_PROPERTY, new RDFResource("ex:propertyB"))));
        }

        [TestMethod]
        public void ShouldEmitWarningOnDeclaringEquivalentPropertiesBecauseIncompatibleProperties()
        {
            string warningMsg = null;
            RDFSemanticsEvents.OnSemanticsWarning += (string msg) => { warningMsg = msg; };

            RDFOntologyPropertyModel propertyModel = new RDFOntologyPropertyModel();
            propertyModel.DeclareObjectProperty(new RDFResource("ex:propertyA"));
            propertyModel.DeclareObjectProperty(new RDFResource("ex:propertyB"));
            propertyModel.DeclareSubProperties(new RDFResource("ex:propertyA"), new RDFResource("ex:propertyB"));
            propertyModel.DeclareEquivalentProperties(new RDFResource("ex:propertyA"), new RDFResource("ex:propertyB"));  //OWL-DL contraddiction (enforced by policy)

            Assert.IsNotNull(warningMsg);
            Assert.IsTrue(warningMsg.IndexOf("EquivalentProperty relation between property 'ex:propertyA' and property 'ex:propertyB' cannot be declared to the model because it would violate OWL-DL integrity") > -1);
            Assert.IsTrue(propertyModel.TBoxGraph.ContainsTriple(new RDFTriple(new RDFResource("ex:propertyA"), RDFVocabulary.RDF.TYPE, RDFVocabulary.OWL.OBJECT_PROPERTY)));
            Assert.IsTrue(propertyModel.TBoxGraph.ContainsTriple(new RDFTriple(new RDFResource("ex:propertyB"), RDFVocabulary.RDF.TYPE, RDFVocabulary.OWL.OBJECT_PROPERTY)));
            Assert.IsTrue(propertyModel.TBoxGraph.ContainsTriple(new RDFTriple(new RDFResource("ex:propertyA"), RDFVocabulary.RDFS.SUB_PROPERTY_OF, new RDFResource("ex:propertyB"))));
        }

        [TestMethod]
        public void ShouldThrowExceptionOnDeclaringEquivalentPropertiesBecauseNullLeftProperty()
            => Assert.ThrowsException<RDFSemanticsException>(() => new RDFOntologyPropertyModel()
                        .DeclareObjectProperty(new RDFResource("ex:propertyA"))
                        .DeclareObjectProperty(new RDFResource("ex:propertyB"))
                        .DeclareEquivalentProperties(null, new RDFResource("ex:propertyB")));

        [TestMethod]
        public void ShouldThrowExceptionOnDeclaringEquivalentPropertiesBecauseNullRightProperty()
            => Assert.ThrowsException<RDFSemanticsException>(() => new RDFOntologyPropertyModel()
                        .DeclareObjectProperty(new RDFResource("ex:propertyA"))
                        .DeclareObjectProperty(new RDFResource("ex:propertyB"))
                        .DeclareEquivalentProperties(new RDFResource("ex:propertyA"), null));

        [TestMethod]
        public void ShouldThrowExceptionOnDeclaringEquivalentPropertiesBecauseSelfProperty()
            => Assert.ThrowsException<RDFSemanticsException>(() => new RDFOntologyPropertyModel()
                        .DeclareObjectProperty(new RDFResource("ex:propertyA"))
                        .DeclareEquivalentProperties(new RDFResource("ex:propertyA"), new RDFResource("ex:propertyA")));

        [TestMethod]
        public void ShouldDeclareDisjointProperties()
        {
            RDFOntologyPropertyModel propertyModel = new RDFOntologyPropertyModel();
            propertyModel.DeclareObjectProperty(new RDFResource("ex:propertyA"));
            propertyModel.DeclareObjectProperty(new RDFResource("ex:propertyB"));
            propertyModel.DeclareDisjointProperties(new RDFResource("ex:propertyA"), new RDFResource("ex:propertyB"));

            Assert.IsTrue(propertyModel.TBoxGraph.TriplesCount == 3);
            Assert.IsTrue(propertyModel.TBoxGraph.ContainsTriple(new RDFTriple(new RDFResource("ex:propertyA"), RDFVocabulary.RDF.TYPE, RDFVocabulary.OWL.OBJECT_PROPERTY)));
            Assert.IsTrue(propertyModel.TBoxGraph.ContainsTriple(new RDFTriple(new RDFResource("ex:propertyB"), RDFVocabulary.RDF.TYPE, RDFVocabulary.OWL.OBJECT_PROPERTY)));
            Assert.IsTrue(propertyModel.TBoxGraph.ContainsTriple(new RDFTriple(new RDFResource("ex:propertyA"), RDFVocabulary.OWL.PROPERTY_DISJOINT_WITH, new RDFResource("ex:propertyB"))));
        }

        [TestMethod]
        public void ShouldEmitWarningOnDeclaringDisjointPropertiesBecauseIncompatibleProperties()
        {
            string warningMsg = null;
            RDFSemanticsEvents.OnSemanticsWarning += (string msg) => { warningMsg = msg; };

            RDFOntologyPropertyModel propertyModel = new RDFOntologyPropertyModel();
            propertyModel.DeclareObjectProperty(new RDFResource("ex:propertyA"));
            propertyModel.DeclareObjectProperty(new RDFResource("ex:propertyB"));
            propertyModel.DeclareSubProperties(new RDFResource("ex:propertyA"), new RDFResource("ex:propertyB"));
            propertyModel.DeclareDisjointProperties(new RDFResource("ex:propertyA"), new RDFResource("ex:propertyB"));  //OWL-DL contraddiction (enforced by policy)

            Assert.IsNotNull(warningMsg);
            Assert.IsTrue(warningMsg.IndexOf("PropertyDisjointWith relation between property 'ex:propertyA' and property 'ex:propertyB' cannot be declared to the model because it would violate OWL-DL integrity") > -1);
            Assert.IsTrue(propertyModel.TBoxGraph.ContainsTriple(new RDFTriple(new RDFResource("ex:propertyA"), RDFVocabulary.RDF.TYPE, RDFVocabulary.OWL.OBJECT_PROPERTY)));
            Assert.IsTrue(propertyModel.TBoxGraph.ContainsTriple(new RDFTriple(new RDFResource("ex:propertyB"), RDFVocabulary.RDF.TYPE, RDFVocabulary.OWL.OBJECT_PROPERTY)));
            Assert.IsTrue(propertyModel.TBoxGraph.ContainsTriple(new RDFTriple(new RDFResource("ex:propertyA"), RDFVocabulary.RDFS.SUB_PROPERTY_OF, new RDFResource("ex:propertyB"))));
        }

        [TestMethod]
        public void ShouldThrowExceptionOnDeclaringDisjointPropertiesBecauseNullLeftProperty()
            => Assert.ThrowsException<RDFSemanticsException>(() => new RDFOntologyPropertyModel()
                        .DeclareObjectProperty(new RDFResource("ex:propertyA"))
                        .DeclareObjectProperty(new RDFResource("ex:propertyB"))
                        .DeclareDisjointProperties(null, new RDFResource("ex:propertyB")));

        [TestMethod]
        public void ShouldThrowExceptionOnDeclaringDisjointPropertiesBecauseNullRightProperty()
            => Assert.ThrowsException<RDFSemanticsException>(() => new RDFOntologyPropertyModel()
                        .DeclareObjectProperty(new RDFResource("ex:propertyA"))
                        .DeclareObjectProperty(new RDFResource("ex:propertyB"))
                        .DeclareDisjointProperties(new RDFResource("ex:propertyA"), null));

        [TestMethod]
        public void ShouldThrowExceptionOnDeclaringDisjointPropertiesBecauseSelfProperty()
            => Assert.ThrowsException<RDFSemanticsException>(() => new RDFOntologyPropertyModel()
                        .DeclareObjectProperty(new RDFResource("ex:propertyA"))
                        .DeclareDisjointProperties(new RDFResource("ex:propertyA"), new RDFResource("ex:propertyA")));

        [TestMethod]
        public void ShouldDeclareInverseProperties()
        {
            RDFOntologyPropertyModel propertyModel = new RDFOntologyPropertyModel();
            propertyModel.DeclareObjectProperty(new RDFResource("ex:propertyA"));
            propertyModel.DeclareObjectProperty(new RDFResource("ex:propertyB"));
            propertyModel.DeclareInverseProperties(new RDFResource("ex:propertyA"), new RDFResource("ex:propertyB"));

            Assert.IsTrue(propertyModel.TBoxGraph.TriplesCount == 3);
            Assert.IsTrue(propertyModel.TBoxGraph.ContainsTriple(new RDFTriple(new RDFResource("ex:propertyA"), RDFVocabulary.RDF.TYPE, RDFVocabulary.OWL.OBJECT_PROPERTY)));
            Assert.IsTrue(propertyModel.TBoxGraph.ContainsTriple(new RDFTriple(new RDFResource("ex:propertyB"), RDFVocabulary.RDF.TYPE, RDFVocabulary.OWL.OBJECT_PROPERTY)));
            Assert.IsTrue(propertyModel.TBoxGraph.ContainsTriple(new RDFTriple(new RDFResource("ex:propertyA"), RDFVocabulary.OWL.INVERSE_OF, new RDFResource("ex:propertyB"))));
        }

        [TestMethod]
        public void ShouldEmitWarningOnDeclaringInversePropertiesBecauseIncompatibleProperties()
        {
            string warningMsg = null;
            RDFSemanticsEvents.OnSemanticsWarning += (string msg) => { warningMsg = msg; };

            RDFOntologyPropertyModel propertyModel = new RDFOntologyPropertyModel();
            propertyModel.DeclareObjectProperty(new RDFResource("ex:propertyA"));
            propertyModel.DeclareObjectProperty(new RDFResource("ex:propertyB"));
            propertyModel.DeclareSubProperties(new RDFResource("ex:propertyA"), new RDFResource("ex:propertyB"));
            propertyModel.DeclareInverseProperties(new RDFResource("ex:propertyA"), new RDFResource("ex:propertyB"));  //OWL-DL contraddiction (enforced by policy)

            Assert.IsNotNull(warningMsg);
            Assert.IsTrue(warningMsg.IndexOf("Inverse relation between property 'ex:propertyA' and property 'ex:propertyB' cannot be declared to the model because it would violate OWL-DL integrity") > -1);
            Assert.IsTrue(propertyModel.TBoxGraph.ContainsTriple(new RDFTriple(new RDFResource("ex:propertyA"), RDFVocabulary.RDF.TYPE, RDFVocabulary.OWL.OBJECT_PROPERTY)));
            Assert.IsTrue(propertyModel.TBoxGraph.ContainsTriple(new RDFTriple(new RDFResource("ex:propertyB"), RDFVocabulary.RDF.TYPE, RDFVocabulary.OWL.OBJECT_PROPERTY)));
            Assert.IsTrue(propertyModel.TBoxGraph.ContainsTriple(new RDFTriple(new RDFResource("ex:propertyA"), RDFVocabulary.RDFS.SUB_PROPERTY_OF, new RDFResource("ex:propertyB"))));
        }

        [TestMethod]
        public void ShouldThrowExceptionOnDeclaringInversePropertiesBecauseNullLeftProperty()
            => Assert.ThrowsException<RDFSemanticsException>(() => new RDFOntologyPropertyModel()
                        .DeclareObjectProperty(new RDFResource("ex:propertyA"))
                        .DeclareObjectProperty(new RDFResource("ex:propertyB"))
                        .DeclareInverseProperties(null, new RDFResource("ex:propertyB")));

        [TestMethod]
        public void ShouldThrowExceptionOnDeclaringInversePropertiesBecauseNullRightProperty()
            => Assert.ThrowsException<RDFSemanticsException>(() => new RDFOntologyPropertyModel()
                        .DeclareObjectProperty(new RDFResource("ex:propertyA"))
                        .DeclareObjectProperty(new RDFResource("ex:propertyB"))
                        .DeclareInverseProperties(new RDFResource("ex:propertyA"), null));

        [TestMethod]
        public void ShouldThrowExceptionOnDeclaringInversePropertiesBecauseSelfProperty()
            => Assert.ThrowsException<RDFSemanticsException>(() => new RDFOntologyPropertyModel()
                        .DeclareObjectProperty(new RDFResource("ex:propertyA"))
                        .DeclareInverseProperties(new RDFResource("ex:propertyA"), new RDFResource("ex:propertyA")));

        [TestMethod]
        public void ShouldDeclarePropertyChainAxiom()
        {
            RDFOntologyPropertyModel propertyModel = new RDFOntologyPropertyModel();
            propertyModel.DeclareObjectProperty(new RDFResource("ex:objprop1"));
            propertyModel.DeclareObjectProperty(new RDFResource("ex:objprop2"));
            propertyModel.DeclarePropertyChainAxiom(new RDFResource("ex:propertyChainAxiom"), new List<RDFResource>() { new RDFResource("ex:objprop1"), new RDFResource("ex:objprop2") });

            Assert.IsTrue(propertyModel.TBoxGraph.TriplesCount == 10);
            Assert.IsTrue(propertyModel.TBoxGraph[null, RDFVocabulary.RDF.TYPE, RDFVocabulary.OWL.OBJECT_PROPERTY, null].TriplesCount == 3);
            Assert.IsTrue(propertyModel.TBoxGraph[new RDFResource("ex:propertyChainAxiom"), RDFVocabulary.OWL.PROPERTY_CHAIN_AXIOM, null, null].TriplesCount == 1);
            Assert.IsTrue(propertyModel.TBoxGraph[null, RDFVocabulary.RDF.TYPE, RDFVocabulary.RDF.LIST, null].TriplesCount == 2);
            Assert.IsTrue(propertyModel.TBoxGraph[null, RDFVocabulary.RDF.FIRST, new RDFResource("ex:objprop1"), null].TriplesCount == 1);
            Assert.IsTrue(propertyModel.TBoxGraph[null, RDFVocabulary.RDF.FIRST, new RDFResource("ex:objprop2"), null].TriplesCount == 1);
            Assert.IsTrue(propertyModel.TBoxGraph[null, RDFVocabulary.RDF.REST, null, null].TriplesCount == 2);
        }

        [TestMethod]
        public void ShouldThrowExceptionOnDeclaringPropertyChainAxiomBecauseNullProperty()
            => Assert.ThrowsException<RDFSemanticsException>(() => new RDFOntologyPropertyModel().DeclarePropertyChainAxiom(null, new List<RDFResource>() { new RDFResource("ex:objprop1") }));

        [TestMethod]
        public void ShouldThrowExceptionOnDeclaringPropertyChainAxiomBecauseNullProperties()
            => Assert.ThrowsException<RDFSemanticsException>(() => new RDFOntologyPropertyModel().DeclarePropertyChainAxiom(new RDFResource("ex:propertyChainAxiom"), null));

        [TestMethod]
        public void ShouldThrowExceptionOnDeclaringPropertyChainAxiomBecauseEmptyProperties()
            => Assert.ThrowsException<RDFSemanticsException>(() => new RDFOntologyPropertyModel().DeclarePropertyChainAxiom(new RDFResource("ex:propertyChainAxiom"), new List<RDFResource>()));

        [TestMethod]
        public void ShouldThrowExceptionOnDeclaringPropertyChainAxiomBecauseSelfProperty()
            => Assert.ThrowsException<RDFSemanticsException>(() => new RDFOntologyPropertyModel().DeclarePropertyChainAxiom(new RDFResource("ex:propertyChainAxiom"), new List<RDFResource>() { new RDFResource("ex:propertyChainAxiom") }));

        [TestMethod]
        public void ShouldExportToGraph()
        {
            RDFOntologyPropertyModel propertyModel = new RDFOntologyPropertyModel();
            propertyModel.DeclareObjectProperty(new RDFResource("ex:propertyA"), new RDFOntologyObjectPropertyBehavior() { Symmetric = true });
            propertyModel.DeclareObjectProperty(new RDFResource("ex:propertyB"), new RDFOntologyObjectPropertyBehavior() { Asymmetric = true });
            propertyModel.DeclareObjectProperty(new RDFResource("ex:propertyC"), new RDFOntologyObjectPropertyBehavior() { Transitive = true });
            propertyModel.DeclareObjectProperty(new RDFResource("ex:propertyD"), new RDFOntologyObjectPropertyBehavior() {  Reflexive = true });
            propertyModel.DeclareDatatypeProperty(new RDFResource("ex:propertyE"), new RDFOntologyObjectPropertyBehavior() { Domain = RDFVocabulary.RDFS.RESOURCE, Range = RDFVocabulary.RDFS.RESOURCE });
            propertyModel.DeclareAnnotationProperty(new RDFResource("ex:propertyF"));
            propertyModel.DeclareObjectProperty(new RDFResource("ex:propertyG"), new RDFOntologyObjectPropertyBehavior() { Deprecated = true });
            propertyModel.DeclareObjectProperty(new RDFResource("ex:propertyH"), new RDFOntologyObjectPropertyBehavior() { Irreflexive = true });
            propertyModel.DeclareObjectProperty(new RDFResource("ex:propertyI"), new RDFOntologyObjectPropertyBehavior() { Functional = true });
            propertyModel.DeclareObjectProperty(new RDFResource("ex:propertyJ"), new RDFOntologyObjectPropertyBehavior() { InverseFunctional = true });
            propertyModel.DeclareSubProperties(new RDFResource("ex:propertyB"), new RDFResource("ex:propertyA"));
            propertyModel.DeclareEquivalentProperties(new RDFResource("ex:propertyA"), new RDFResource("ex:propertyC"));
            propertyModel.DeclareDisjointProperties(new RDFResource("ex:propertyC"), new RDFResource("ex:propertyD"));
            propertyModel.DeclarePropertyChainAxiom(new RDFResource("ex:propertyChainAxiom"), new List<RDFResource>() { new RDFResource("ex:propertyA") });
            propertyModel.DeclareAllDisjointProperties(new RDFResource("ex:allDisjointProperties"), new List<RDFResource>() { new RDFResource("ex:propertyH"), new RDFResource("ex:propertyI") });
            propertyModel.AnnotateProperty(new RDFResource("ex:propertyA"), new RDFResource("ex:propertyF"), new RDFPlainLiteral("comment"));
            propertyModel.AnnotateProperty(new RDFResource("ex:propertyB"), new RDFResource("ex:propertyF"), new RDFPlainLiteral("title"));
            RDFGraph graph = propertyModel.ToRDFGraph();

            Assert.IsNotNull(graph);
            Assert.IsTrue(graph.TriplesCount == 40);
        }

        [TestMethod]
        public async Task ShouldExportToGraphAsync()
        {
            RDFOntologyPropertyModel propertyModel = new RDFOntologyPropertyModel();
            propertyModel.DeclareObjectProperty(new RDFResource("ex:propertyA"), new RDFOntologyObjectPropertyBehavior() { Symmetric = true });
            propertyModel.DeclareObjectProperty(new RDFResource("ex:propertyB"), new RDFOntologyObjectPropertyBehavior() { Asymmetric = true });
            propertyModel.DeclareObjectProperty(new RDFResource("ex:propertyC"), new RDFOntologyObjectPropertyBehavior() { Transitive = true });
            propertyModel.DeclareObjectProperty(new RDFResource("ex:propertyD"), new RDFOntologyObjectPropertyBehavior() { Reflexive = true });
            propertyModel.DeclareDatatypeProperty(new RDFResource("ex:propertyE"), new RDFOntologyObjectPropertyBehavior() { Domain = RDFVocabulary.RDFS.RESOURCE, Range = RDFVocabulary.RDFS.RESOURCE });
            propertyModel.DeclareAnnotationProperty(new RDFResource("ex:propertyF"));
            propertyModel.DeclareObjectProperty(new RDFResource("ex:propertyG"), new RDFOntologyObjectPropertyBehavior() { Deprecated = true });
            propertyModel.DeclareObjectProperty(new RDFResource("ex:propertyH"), new RDFOntologyObjectPropertyBehavior() { Irreflexive = true });
            propertyModel.DeclareObjectProperty(new RDFResource("ex:propertyI"), new RDFOntologyObjectPropertyBehavior() { Functional = true });
            propertyModel.DeclareObjectProperty(new RDFResource("ex:propertyJ"), new RDFOntologyObjectPropertyBehavior() { InverseFunctional = true });
            propertyModel.DeclareSubProperties(new RDFResource("ex:propertyB"), new RDFResource("ex:propertyA"));
            propertyModel.DeclareEquivalentProperties(new RDFResource("ex:propertyA"), new RDFResource("ex:propertyC"));
            propertyModel.DeclareDisjointProperties(new RDFResource("ex:propertyC"), new RDFResource("ex:propertyD"));
            propertyModel.DeclarePropertyChainAxiom(new RDFResource("ex:propertyChainAxiom"), new List<RDFResource>() { new RDFResource("ex:propertyA") });
            propertyModel.DeclareAllDisjointProperties(new RDFResource("ex:allDisjointProperties"), new List<RDFResource>() { new RDFResource("ex:propertyH"), new RDFResource("ex:propertyI") });
            propertyModel.AnnotateProperty(new RDFResource("ex:propertyA"), new RDFResource("ex:propertyF"), new RDFPlainLiteral("comment"));
            propertyModel.AnnotateProperty(new RDFResource("ex:propertyB"), new RDFResource("ex:propertyF"), new RDFPlainLiteral("title"));
            RDFGraph graph = await propertyModel.ToRDFGraphAsync();

            Assert.IsNotNull(graph);
            Assert.IsTrue(graph.TriplesCount == 40);
        }
        #endregion
    }
}