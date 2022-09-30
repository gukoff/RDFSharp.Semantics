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

using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using RDFSharp.Model;

namespace RDFSharp.Semantics.Test
{
    [TestClass]
    public class RDFOntologyModelTest
    {
        #region Test
        [TestMethod]
        public void ShouldCreateModel()
        {
            RDFOntologyModel model = new RDFOntologyModel();

            Assert.IsNotNull(model);
            Assert.IsNotNull(model.ClassModel);
            Assert.IsNotNull(model.PropertyModel);
        }

        [TestMethod]
        public void ShouldCreateModelFromClassAndProperty()
        {
            RDFOntologyModel model = new RDFOntologyModel(new RDFOntologyClassModel(), new RDFOntologyPropertyModel());

            Assert.IsNotNull(model);
            Assert.IsNotNull(model.ClassModel);
            Assert.IsNotNull(model.PropertyModel);
        }

        [TestMethod]
        public void ShouldExportToGraph()
        {
            RDFOntologyModel model = new RDFOntologyModel();

            //ClassModel definition from tests
            model.ClassModel.DeclareClass(new RDFResource("ex:classA"));
            model.ClassModel.DeclareClass(new RDFResource("ex:classB"));
            model.ClassModel.DeclareClass(new RDFResource("ex:classC"));
            model.ClassModel.DeclareClass(new RDFResource("ex:classD"));
            model.ClassModel.DeclareClass(new RDFResource("ex:classE"), new RDFOntologyClassBehavior() { Deprecated = true });
            model.ClassModel.DeclareSubClasses(new RDFResource("ex:indivB"), new RDFResource("ex:classA"));
            model.ClassModel.DeclareEquivalentClasses(new RDFResource("ex:indivA"), new RDFResource("ex:classC"));
            model.ClassModel.DeclareDisjointClasses(new RDFResource("ex:indivC"), new RDFResource("ex:classD"));
            model.ClassModel.DeclareHasKey(new RDFResource("ex:classA"), new List<RDFResource>() { RDFVocabulary.FOAF.ACCOUNT });
            model.ClassModel.DeclareAllDisjointClasses(new RDFResource("ex:allDisjointClasses"), new List<RDFResource>() { new RDFResource("ex:classD"), new RDFResource("ex:classE") });
            model.ClassModel.DeclareDisjointUnionClass(new RDFResource("ex:disjointUnionClass"), new List<RDFResource>() { new RDFResource("ex:classD"), new RDFResource("ex:classE") });
            model.ClassModel.AnnotateClass(new RDFResource("ex:classA"), RDFVocabulary.RDFS.COMMENT, new RDFPlainLiteral("comment"));
            model.ClassModel.AnnotateClass(new RDFResource("ex:classB"), RDFVocabulary.DC.DESCRIPTION, new RDFPlainLiteral("title"));

            //PropertyModel definition from tests
            model.PropertyModel.DeclareObjectProperty(new RDFResource("ex:propertyA"), new RDFOntologyObjectPropertyBehavior() { Symmetric = true });
            model.PropertyModel.DeclareObjectProperty(new RDFResource("ex:propertyB"), new RDFOntologyObjectPropertyBehavior() { Asymmetric = true });
            model.PropertyModel.DeclareObjectProperty(new RDFResource("ex:propertyC"), new RDFOntologyObjectPropertyBehavior() { Transitive = true });
            model.PropertyModel.DeclareObjectProperty(new RDFResource("ex:propertyD"), new RDFOntologyObjectPropertyBehavior() { Reflexive = true });
            model.PropertyModel.DeclareDatatypeProperty(new RDFResource("ex:propertyE"), new RDFOntologyObjectPropertyBehavior() { Domain = RDFVocabulary.RDFS.RESOURCE, Range = RDFVocabulary.RDFS.RESOURCE });
            model.PropertyModel.DeclareAnnotationProperty(new RDFResource("ex:propertyF"));
            model.PropertyModel.DeclareObjectProperty(new RDFResource("ex:propertyG"), new RDFOntologyObjectPropertyBehavior() { Deprecated = true });
            model.PropertyModel.DeclareObjectProperty(new RDFResource("ex:propertyH"), new RDFOntologyObjectPropertyBehavior() { Irreflexive = true });
            model.PropertyModel.DeclareObjectProperty(new RDFResource("ex:propertyI"), new RDFOntologyObjectPropertyBehavior() { Functional = true });
            model.PropertyModel.DeclareObjectProperty(new RDFResource("ex:propertyJ"), new RDFOntologyObjectPropertyBehavior() { InverseFunctional = true });
            model.PropertyModel.DeclareSubProperties(new RDFResource("ex:propertyB"), new RDFResource("ex:propertyA"));
            model.PropertyModel.DeclareEquivalentProperties(new RDFResource("ex:propertyA"), new RDFResource("ex:propertyC"));
            model.PropertyModel.DeclareDisjointProperties(new RDFResource("ex:propertyC"), new RDFResource("ex:propertyD"));
            model.PropertyModel.DeclarePropertyChainAxiom(new RDFResource("ex:propertyChainAxiom"), new List<RDFResource>() { new RDFResource("ex:propertyA") });
            model.PropertyModel.DeclareAllDisjointProperties(new RDFResource("ex:allDisjointProperties"), new List<RDFResource>() { new RDFResource("ex:propertyH"), new RDFResource("ex:propertyI") });
            model.PropertyModel.AnnotateProperty(new RDFResource("ex:propertyA"), new RDFResource("ex:propertyF"), new RDFPlainLiteral("comment"));
            model.PropertyModel.AnnotateProperty(new RDFResource("ex:propertyB"), new RDFResource("ex:propertyF"), new RDFPlainLiteral("title"));

            RDFGraph graph = model.ToRDFGraph();

            Assert.IsNotNull(graph);
            Assert.IsTrue(graph.TriplesCount == 74);
        }

        [TestMethod]
        public async Task ShouldExportToGraphAsync()
        {
            RDFOntologyModel model = new RDFOntologyModel();

            //ClassModel definition from tests
            model.ClassModel.DeclareClass(new RDFResource("ex:classA"));
            model.ClassModel.DeclareClass(new RDFResource("ex:classB"));
            model.ClassModel.DeclareClass(new RDFResource("ex:classC"));
            model.ClassModel.DeclareClass(new RDFResource("ex:classD"));
            model.ClassModel.DeclareClass(new RDFResource("ex:classE"), new RDFOntologyClassBehavior() { Deprecated = true });
            model.ClassModel.DeclareSubClasses(new RDFResource("ex:indivB"), new RDFResource("ex:classA"));
            model.ClassModel.DeclareEquivalentClasses(new RDFResource("ex:indivA"), new RDFResource("ex:classC"));
            model.ClassModel.DeclareDisjointClasses(new RDFResource("ex:indivC"), new RDFResource("ex:classD"));
            model.ClassModel.DeclareHasKey(new RDFResource("ex:classA"), new List<RDFResource>() { RDFVocabulary.FOAF.ACCOUNT });
            model.ClassModel.DeclareAllDisjointClasses(new RDFResource("ex:allDisjointClasses"), new List<RDFResource>() { new RDFResource("ex:classD"), new RDFResource("ex:classE") });
            model.ClassModel.DeclareDisjointUnionClass(new RDFResource("ex:disjointUnionClass"), new List<RDFResource>() { new RDFResource("ex:classD"), new RDFResource("ex:classE") });
            model.ClassModel.AnnotateClass(new RDFResource("ex:classA"), RDFVocabulary.RDFS.COMMENT, new RDFPlainLiteral("comment"));
            model.ClassModel.AnnotateClass(new RDFResource("ex:classB"), RDFVocabulary.DC.DESCRIPTION, new RDFPlainLiteral("title"));

            //PropertyModel definition from tests
            model.PropertyModel.DeclareObjectProperty(new RDFResource("ex:propertyA"), new RDFOntologyObjectPropertyBehavior() { Symmetric = true });
            model.PropertyModel.DeclareObjectProperty(new RDFResource("ex:propertyB"), new RDFOntologyObjectPropertyBehavior() { Asymmetric = true });
            model.PropertyModel.DeclareObjectProperty(new RDFResource("ex:propertyC"), new RDFOntologyObjectPropertyBehavior() { Transitive = true });
            model.PropertyModel.DeclareObjectProperty(new RDFResource("ex:propertyD"), new RDFOntologyObjectPropertyBehavior() { Reflexive = true });
            model.PropertyModel.DeclareDatatypeProperty(new RDFResource("ex:propertyE"), new RDFOntologyObjectPropertyBehavior() { Domain = RDFVocabulary.RDFS.RESOURCE, Range = RDFVocabulary.RDFS.RESOURCE });
            model.PropertyModel.DeclareAnnotationProperty(new RDFResource("ex:propertyF"));
            model.PropertyModel.DeclareObjectProperty(new RDFResource("ex:propertyG"), new RDFOntologyObjectPropertyBehavior() { Deprecated = true });
            model.PropertyModel.DeclareObjectProperty(new RDFResource("ex:propertyH"), new RDFOntologyObjectPropertyBehavior() { Irreflexive = true });
            model.PropertyModel.DeclareObjectProperty(new RDFResource("ex:propertyI"), new RDFOntologyObjectPropertyBehavior() { Functional = true });
            model.PropertyModel.DeclareObjectProperty(new RDFResource("ex:propertyJ"), new RDFOntologyObjectPropertyBehavior() { InverseFunctional = true });
            model.PropertyModel.DeclareSubProperties(new RDFResource("ex:propertyB"), new RDFResource("ex:propertyA"));
            model.PropertyModel.DeclareEquivalentProperties(new RDFResource("ex:propertyA"), new RDFResource("ex:propertyC"));
            model.PropertyModel.DeclareDisjointProperties(new RDFResource("ex:propertyC"), new RDFResource("ex:propertyD"));
            model.PropertyModel.DeclarePropertyChainAxiom(new RDFResource("ex:propertyChainAxiom"), new List<RDFResource>() { new RDFResource("ex:propertyA") });
            model.PropertyModel.DeclareAllDisjointProperties(new RDFResource("ex:allDisjointProperties"), new List<RDFResource>() { new RDFResource("ex:propertyH"), new RDFResource("ex:propertyI") });
            model.PropertyModel.AnnotateProperty(new RDFResource("ex:propertyA"), new RDFResource("ex:propertyF"), new RDFPlainLiteral("comment"));
            model.PropertyModel.AnnotateProperty(new RDFResource("ex:propertyB"), new RDFResource("ex:propertyF"), new RDFPlainLiteral("title"));

            RDFGraph graph = await model.ToRDFGraphAsync();

            Assert.IsNotNull(graph);
            Assert.IsTrue(graph.TriplesCount == 74);
        }
        #endregion
    }
}