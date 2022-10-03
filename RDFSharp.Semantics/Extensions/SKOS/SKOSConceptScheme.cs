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

using RDFSharp.Model;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace RDFSharp.Semantics.Extensions.SKOS
{
    /// <summary>
    /// SKOSConceptScheme represents an organized taxonomy describing relations between skos:Concept individuals
    /// </summary>
    public class SKOSConceptScheme : RDFResource, IEnumerable<RDFResource>
    {
        #region Properties
        /// <summary>
        /// Count of the concepts
        /// </summary>
        public long ConceptsCount
        {
            get
            {
                long count = 0;
                IEnumerator<RDFResource> concepts = ConceptsEnumerator;
                while (concepts.MoveNext())
                    count++;
                return count;
            }
        }

        /// <summary>
        /// Count of the collections
        /// </summary>
        public long CollectionsCount
        {
            get
            {
                long count = 0;
                IEnumerator<RDFResource> collections = CollectionsEnumerator;
                while (collections.MoveNext())
                    count++;
                return count;
            }
        }

        /// <summary>
        /// Count of the ordered collections
        /// </summary>
        public long OrderedCollectionsCount
        {
            get
            {
                long count = 0;
                IEnumerator<RDFResource> orderedCollections = OrderedCollectionsEnumerator;
                while (orderedCollections.MoveNext())
                    count++;
                return count;
            }
        }

        /// <summary>
        /// Count of the labels [SKOS-XL]
        /// </summary>
        public long LabelsCount
        {
            get
            {
                long count = 0;
                IEnumerator<RDFResource> labels = LabelsEnumerator;
                while (labels.MoveNext())
                    count++;
                return count;
            }
        }

        /// <summary>
        /// Gets the enumerator on the concepts for iteration
        /// </summary>
        public IEnumerator<RDFResource> ConceptsEnumerator
            => Ontology.Data.ABoxGraph[null, RDFVocabulary.RDF.TYPE, RDFVocabulary.SKOS.CONCEPT, null]
                            .Select(t => t.Subject)
                            .OfType<RDFResource>()
                            .GetEnumerator();

        /// <summary>
        /// Gets the enumerator on the collections for iteration
        /// </summary>
        public IEnumerator<RDFResource> CollectionsEnumerator
            => Ontology.Data.ABoxGraph[null, RDFVocabulary.RDF.TYPE, RDFVocabulary.SKOS.COLLECTION, null]
                            .Select(t => t.Subject)
                            .OfType<RDFResource>()
                            .GetEnumerator();

        /// <summary>
        /// Gets the enumerator on the ordered collections for iteration
        /// </summary>
        public IEnumerator<RDFResource> OrderedCollectionsEnumerator
            => Ontology.Data.ABoxGraph[null, RDFVocabulary.RDF.TYPE, RDFVocabulary.SKOS.ORDERED_COLLECTION, null]
                            .Select(t => t.Subject)
                            .OfType<RDFResource>()
                            .GetEnumerator();

        /// <summary>
        /// Gets the enumerator on the labels for iteration [SKOS-XL]
        /// </summary>
        public IEnumerator<RDFResource> LabelsEnumerator
            => Ontology.Data.ABoxGraph[null, RDFVocabulary.RDF.TYPE, RDFVocabulary.SKOS.SKOSXL.LABEL, null]
                            .Select(t => t.Subject)
                            .OfType<RDFResource>()
                            .GetEnumerator();

        /// <summary>
        /// Knowledge describing the concept scheme (always initialized with SKOS ontology)
        /// </summary>
        internal OWLOntology Ontology { get; set; }
        #endregion

        #region Ctors
        /// <summary>
        /// Builds an empty concept scheme with the given URI
        /// </summary>
        public SKOSConceptScheme(string conceptSchemeURI) : base(conceptSchemeURI)
        {
            Ontology = new OWLOntology(conceptSchemeURI) { Model = BuildSKOSModel() };

            //Declare concept scheme to the data
            Ontology.Data.DeclareIndividual(this);
            Ontology.Data.DeclareIndividualType(this, RDFVocabulary.SKOS.CONCEPT_SCHEME);
        }
        #endregion

        #region Interfaces
        /// <summary>
        /// Exposes a typed enumerator on the concepts for iteration
        /// </summary>
        IEnumerator<RDFResource> IEnumerable<RDFResource>.GetEnumerator()
            => ConceptsEnumerator;

        /// <summary>
        /// Exposes an untyped enumerator on the concepts for iteration
        /// </summary>
        IEnumerator IEnumerable.GetEnumerator()
            => ConceptsEnumerator;
        #endregion

        #region Methods
        /// <summary>
        /// Declares the given skos:Concept instance to the concept scheme
        /// </summary>
        public SKOSConceptScheme DeclareConcept(RDFResource skosConcept)
        {
            if (skosConcept == null)
                throw new OWLSemanticsException("Cannot declare skos:Concept instance to the concept scheme because given \"skosConcept\" parameter is null");

            //Add knowledge to the A-BOX
            Ontology.Data.DeclareIndividual(skosConcept);
            Ontology.Data.DeclareIndividualType(skosConcept, RDFVocabulary.SKOS.CONCEPT);
            Ontology.Data.DeclareObjectAssertion(skosConcept, RDFVocabulary.SKOS.IN_SCHEME, this);

            return this;
        }

        /// <summary>
        /// Declares the given skos:Collection instance to the concept scheme
        /// </summary>
        public SKOSConceptScheme DeclareCollection(RDFResource skosCollection, List<RDFResource> skosConcepts)
        {
            if (skosCollection == null)
                throw new OWLSemanticsException("Cannot declare skos:Collection instance to the concept scheme because given \"skosCollection\" parameter is null");
            if (skosConcepts == null)
                throw new OWLSemanticsException("Cannot declare skos:Collection instance to the concept scheme because given \"skosConcepts\" parameter is null");
            if (skosConcepts.Count == 0)
                throw new OWLSemanticsException("Cannot declare skos:Collection instance to the concept scheme because given \"skosConcepts\" parameter is an empty list");

            //Add knowledge to the A-BOX (collection)
            Ontology.Data.DeclareIndividual(skosCollection);
            Ontology.Data.DeclareIndividualType(skosCollection, RDFVocabulary.SKOS.COLLECTION);
            Ontology.Data.DeclareObjectAssertion(skosCollection, RDFVocabulary.SKOS.IN_SCHEME, this);

            //Add knowledge to the A-BOX (concepts)
            foreach (RDFResource skosConcept in skosConcepts)
            {
                Ontology.Data.DeclareIndividual(skosConcept);
                Ontology.Data.DeclareIndividualType(skosConcept, RDFVocabulary.SKOS.CONCEPT);
                Ontology.Data.DeclareObjectAssertion(skosCollection, RDFVocabulary.SKOS.MEMBER, skosConcept);
            }

            return this;
        }

        /// <summary>
        /// Declares the given skos:OrderedCollection instance to the concept scheme
        /// </summary>
        public SKOSConceptScheme DeclareOrderedCollection(RDFResource skosOrderedCollection, List<RDFResource> skosConcepts)
        {
            if (skosOrderedCollection == null)
                throw new OWLSemanticsException("Cannot declare skos:OrderedCollection instance to the concept scheme because given \"skosOrderedCollection\" parameter is null");
            if (skosConcepts == null)
                throw new OWLSemanticsException("Cannot declare skos:OrderedCollection instance to the concept scheme because given \"skosConcepts\" parameter is null");
            if (skosConcepts.Count == 0)
                throw new OWLSemanticsException("Cannot declare skos:OrderedCollection instance to the concept scheme because given \"skosConcepts\" parameter is an empty list");

            //Add knowledge to the A-BOX (ordered collection)
            Ontology.Data.DeclareIndividual(skosOrderedCollection);
            Ontology.Data.DeclareIndividualType(skosOrderedCollection, RDFVocabulary.SKOS.ORDERED_COLLECTION);
            Ontology.Data.DeclareObjectAssertion(skosOrderedCollection, RDFVocabulary.SKOS.IN_SCHEME, this);

            //Add knowledge to the A-BOX (concepts)
            RDFCollection rdfCollection = new RDFCollection(RDFModelEnums.RDFItemTypes.Resource);
            foreach (RDFResource skosConcept in skosConcepts)
            {
                Ontology.Data.DeclareIndividual(skosConcept);
                Ontology.Data.DeclareIndividualType(skosConcept, RDFVocabulary.SKOS.CONCEPT);
                rdfCollection.AddItem(skosConcept);
            }
            Ontology.Data.ABoxGraph.AddCollection(rdfCollection);
            Ontology.Data.DeclareObjectAssertion(skosOrderedCollection, RDFVocabulary.SKOS.MEMBER_LIST, rdfCollection.ReificationSubject);

            return this;
        }

        /// <summary>
        /// Declares the given skosxl:Label instance to the concept scheme
        /// </summary>
        public SKOSConceptScheme DeclareLabel(RDFResource skosLabel)
        {
            if (skosLabel == null)
                throw new OWLSemanticsException("Cannot declare skosxl:Label instance to the concept scheme because given \"skosLabel\" parameter is null");

            //Add knowledge to the A-BOX
            Ontology.Data.DeclareIndividual(skosLabel);
            Ontology.Data.DeclareIndividualType(skosLabel, RDFVocabulary.SKOS.SKOSXL.LABEL);
            Ontology.Data.DeclareObjectAssertion(skosLabel, RDFVocabulary.SKOS.IN_SCHEME, this);

            return this;
        }

        //ANNOTATIONS

        //RELATIONS

        //EXPORT

        /// <summary>
        /// Gets a graph representation of the concept scheme
        /// </summary>
        public RDFGraph ToRDFGraph()
            => Ontology.ToRDFGraph();

        /// <summary>
        /// Asynchronously gets a graph representation of the concept scheme
        /// </summary>
        public Task<RDFGraph> ToRDFGraphAsync()
            => Task.Run(() => ToRDFGraph());

        //IMPORT

        /// <summary>
        /// Gets a concept scheme representation from the given graph
        /// </summary>
        public static SKOSConceptScheme FromRDFGraph(RDFGraph graph)
            => SKOSConceptSchemeLoader.FromRDFGraph(graph);

        /// <summary>
        /// Asynchronously gets a concept scheme representation from the given graph
        /// </summary>
        public static Task<SKOSConceptScheme> FromRDFGraphAsync(RDFGraph graph)
            => Task.Run(() => FromRDFGraph(graph));
        #endregion

        #region Utilities
        /// <summary>
        /// Builds a reference SKOS model
        /// </summary>
        internal static OWLOntologyModel BuildSKOSModel()
            => new OWLOntologyModel() { ClassModel = BuildSKOSClassModel(), PropertyModel = BuildSKOSPropertyModel() };

        /// <summary>
        /// Builds a reference SKOS class model
        /// </summary>
        internal static OWLOntologyClassModel BuildSKOSClassModel()
        {
            OWLOntologyClassModel classModel = new OWLOntologyClassModel();

            //SKOS
            classModel.DeclareClass(RDFVocabulary.SKOS.COLLECTION);
            classModel.DeclareClass(RDFVocabulary.SKOS.CONCEPT);
            classModel.DeclareClass(RDFVocabulary.SKOS.CONCEPT_SCHEME);
            classModel.DeclareClass(RDFVocabulary.SKOS.ORDERED_COLLECTION);
            classModel.DeclareUnionClass(new RDFResource("bnode:ConceptCollection"), new List<RDFResource>() { RDFVocabulary.SKOS.CONCEPT, RDFVocabulary.SKOS.COLLECTION });
            classModel.DeclareCardinalityRestriction(new RDFResource("bnode:ExactlyOneLiteralForm"), RDFVocabulary.SKOS.SKOSXL.LITERAL_FORM, 1);
            classModel.DeclareAllDisjointClasses(new RDFResource("bnode:AllDisjointSKOSClasses"), new List<RDFResource>() { RDFVocabulary.SKOS.COLLECTION, RDFVocabulary.SKOS.CONCEPT, RDFVocabulary.SKOS.CONCEPT_SCHEME, RDFVocabulary.SKOS.SKOSXL.LABEL });
            classModel.DeclareSubClasses(RDFVocabulary.SKOS.ORDERED_COLLECTION, RDFVocabulary.SKOS.COLLECTION);

            //SKOS-XL
            classModel.DeclareClass(RDFVocabulary.SKOS.SKOSXL.LABEL);
            classModel.DeclareSubClasses(RDFVocabulary.SKOS.SKOSXL.LABEL, new RDFResource("bnode:ExactlyOneLiteralForm"));

            return classModel;
        }

        /// <summary>
        /// Builds a reference SKOS property model
        /// </summary>
        internal static OWLOntologyPropertyModel BuildSKOSPropertyModel()
        {
            OWLOntologyPropertyModel propertyModel = new OWLOntologyPropertyModel();

            //SKOS
            propertyModel.DeclareAnnotationProperty(RDFVocabulary.SKOS.ALT_LABEL);
            propertyModel.DeclareObjectProperty(RDFVocabulary.SKOS.BROAD_MATCH);
            propertyModel.DeclareObjectProperty(RDFVocabulary.SKOS.BROADER);
            propertyModel.DeclareObjectProperty(RDFVocabulary.SKOS.BROADER_TRANSITIVE, new OWLOntologyObjectPropertyBehavior() { Transitive = true });
            propertyModel.DeclareAnnotationProperty(RDFVocabulary.SKOS.CHANGE_NOTE);
            propertyModel.DeclareObjectProperty(RDFVocabulary.SKOS.CLOSE_MATCH, new OWLOntologyObjectPropertyBehavior() { Symmetric = true });
            propertyModel.DeclareAnnotationProperty(RDFVocabulary.SKOS.DEFINITION);
            propertyModel.DeclareAnnotationProperty(RDFVocabulary.SKOS.EDITORIAL_NOTE);
            propertyModel.DeclareObjectProperty(RDFVocabulary.SKOS.EXACT_MATCH, new OWLOntologyObjectPropertyBehavior() { Symmetric = true, Transitive = true });
            propertyModel.DeclareAnnotationProperty(RDFVocabulary.SKOS.EXAMPLE);
            propertyModel.DeclareObjectProperty(RDFVocabulary.SKOS.HAS_TOP_CONCEPT, new OWLOntologyObjectPropertyBehavior() { Domain = RDFVocabulary.SKOS.CONCEPT_SCHEME, Range = RDFVocabulary.SKOS.CONCEPT });
            propertyModel.DeclareAnnotationProperty(RDFVocabulary.SKOS.HIDDEN_LABEL);
            propertyModel.DeclareAnnotationProperty(RDFVocabulary.SKOS.HISTORY_NOTE);
            propertyModel.DeclareObjectProperty(RDFVocabulary.SKOS.NARROW_MATCH);
            propertyModel.DeclareObjectProperty(RDFVocabulary.SKOS.NARROWER);
            propertyModel.DeclareObjectProperty(RDFVocabulary.SKOS.NARROWER_TRANSITIVE, new OWLOntologyObjectPropertyBehavior() { Transitive = true });
            propertyModel.DeclareObjectProperty(RDFVocabulary.SKOS.IN_SCHEME, new OWLOntologyObjectPropertyBehavior() { Range = RDFVocabulary.SKOS.CONCEPT_SCHEME });
            propertyModel.DeclareObjectProperty(RDFVocabulary.SKOS.MAPPING_RELATION);
            propertyModel.DeclareObjectProperty(RDFVocabulary.SKOS.MEMBER, new OWLOntologyObjectPropertyBehavior() { Domain = RDFVocabulary.SKOS.COLLECTION, Range = new RDFResource("bnode:ConceptCollection") });
            propertyModel.DeclareObjectProperty(RDFVocabulary.SKOS.MEMBER_LIST, new OWLOntologyObjectPropertyBehavior() { Functional = true, Domain = RDFVocabulary.SKOS.ORDERED_COLLECTION });
            propertyModel.DeclareDatatypeProperty(RDFVocabulary.SKOS.NOTATION);
            propertyModel.DeclareAnnotationProperty(RDFVocabulary.SKOS.NOTE);
            propertyModel.DeclareAnnotationProperty(RDFVocabulary.SKOS.PREF_LABEL);
            propertyModel.DeclareObjectProperty(RDFVocabulary.SKOS.RELATED_MATCH, new OWLOntologyObjectPropertyBehavior() { Symmetric = true });
            propertyModel.DeclareObjectProperty(RDFVocabulary.SKOS.RELATED, new OWLOntologyObjectPropertyBehavior() { Symmetric = true });
            propertyModel.DeclareAnnotationProperty(RDFVocabulary.SKOS.SCOPE_NOTE);
            propertyModel.DeclareObjectProperty(RDFVocabulary.SKOS.SEMANTIC_RELATION, new OWLOntologyObjectPropertyBehavior() { Domain = RDFVocabulary.SKOS.CONCEPT, Range = RDFVocabulary.SKOS.CONCEPT });
            propertyModel.DeclareObjectProperty(RDFVocabulary.SKOS.TOP_CONCEPT_OF, new OWLOntologyObjectPropertyBehavior() { Domain = RDFVocabulary.SKOS.CONCEPT, Range = RDFVocabulary.SKOS.CONCEPT_SCHEME });
            propertyModel.DeclareSubProperties(RDFVocabulary.SKOS.BROAD_MATCH, RDFVocabulary.SKOS.BROADER);
            propertyModel.DeclareSubProperties(RDFVocabulary.SKOS.BROAD_MATCH, RDFVocabulary.SKOS.MAPPING_RELATION);
            propertyModel.DeclareSubProperties(RDFVocabulary.SKOS.BROADER, RDFVocabulary.SKOS.BROADER_TRANSITIVE);
            propertyModel.DeclareSubProperties(RDFVocabulary.SKOS.BROADER_TRANSITIVE, RDFVocabulary.SKOS.SEMANTIC_RELATION);
            propertyModel.DeclareSubProperties(RDFVocabulary.SKOS.CLOSE_MATCH, RDFVocabulary.SKOS.MAPPING_RELATION);
            propertyModel.DeclareSubProperties(RDFVocabulary.SKOS.EXACT_MATCH, RDFVocabulary.SKOS.CLOSE_MATCH);
            propertyModel.DeclareSubProperties(RDFVocabulary.SKOS.MAPPING_RELATION, RDFVocabulary.SKOS.SEMANTIC_RELATION);
            propertyModel.DeclareSubProperties(RDFVocabulary.SKOS.NARROW_MATCH, RDFVocabulary.SKOS.NARROWER);
            propertyModel.DeclareSubProperties(RDFVocabulary.SKOS.NARROW_MATCH, RDFVocabulary.SKOS.MAPPING_RELATION);
            propertyModel.DeclareSubProperties(RDFVocabulary.SKOS.NARROWER, RDFVocabulary.SKOS.NARROWER_TRANSITIVE);
            propertyModel.DeclareSubProperties(RDFVocabulary.SKOS.NARROWER_TRANSITIVE, RDFVocabulary.SKOS.SEMANTIC_RELATION);
            propertyModel.DeclareSubProperties(RDFVocabulary.SKOS.TOP_CONCEPT_OF, RDFVocabulary.SKOS.IN_SCHEME);
            propertyModel.DeclareSubProperties(RDFVocabulary.SKOS.RELATED_MATCH, RDFVocabulary.SKOS.RELATED);
            propertyModel.DeclareSubProperties(RDFVocabulary.SKOS.RELATED_MATCH, RDFVocabulary.SKOS.MAPPING_RELATION);
            propertyModel.DeclareSubProperties(RDFVocabulary.SKOS.RELATED, RDFVocabulary.SKOS.SEMANTIC_RELATION);
            propertyModel.DeclareInverseProperties(RDFVocabulary.SKOS.BROAD_MATCH, RDFVocabulary.SKOS.NARROW_MATCH);
            propertyModel.DeclareInverseProperties(RDFVocabulary.SKOS.BROADER, RDFVocabulary.SKOS.NARROWER);
            propertyModel.DeclareInverseProperties(RDFVocabulary.SKOS.BROADER_TRANSITIVE, RDFVocabulary.SKOS.NARROWER_TRANSITIVE);
            propertyModel.DeclareInverseProperties(RDFVocabulary.SKOS.HAS_TOP_CONCEPT, RDFVocabulary.SKOS.TOP_CONCEPT_OF);

            //SKOS-XL
            propertyModel.DeclareDatatypeProperty(RDFVocabulary.SKOS.SKOSXL.LITERAL_FORM, new OWLOntologyDatatypePropertyBehavior() { Domain = RDFVocabulary.SKOS.SKOSXL.LABEL });
            propertyModel.DeclareObjectProperty(RDFVocabulary.SKOS.SKOSXL.PREF_LABEL, new OWLOntologyObjectPropertyBehavior() { Range = RDFVocabulary.SKOS.SKOSXL.LABEL });
            propertyModel.DeclareObjectProperty(RDFVocabulary.SKOS.SKOSXL.ALT_LABEL, new OWLOntologyObjectPropertyBehavior() { Range = RDFVocabulary.SKOS.SKOSXL.LABEL });
            propertyModel.DeclareObjectProperty(RDFVocabulary.SKOS.SKOSXL.HIDDEN_LABEL, new OWLOntologyObjectPropertyBehavior() { Range = RDFVocabulary.SKOS.SKOSXL.LABEL });
            propertyModel.DeclareObjectProperty(RDFVocabulary.SKOS.SKOSXL.LABEL_RELATION, new OWLOntologyObjectPropertyBehavior() { Symmetric = true, Domain = RDFVocabulary.SKOS.SKOSXL.LABEL, Range = RDFVocabulary.SKOS.SKOSXL.LABEL });

            return propertyModel;
        }
        #endregion
    }
}