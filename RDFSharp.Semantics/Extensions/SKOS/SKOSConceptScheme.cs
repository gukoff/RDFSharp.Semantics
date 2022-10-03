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
    /// SKOSConceptScheme represents an OWL ontology specialized in describing relations between skos:Concept individuals
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

        /// <summary>
        /// Annotates the concept scheme with the given "annotationProperty -> annotationValue"
        /// </summary>
        public SKOSConceptScheme Annotate(RDFResource annotationProperty, RDFResource annotationValue)
        {
            if (annotationProperty == null)
                throw new OWLSemanticsException("Cannot annotate concept scheme because given \"annotationProperty\" parameter is null");
            if (annotationProperty.IsBlank)
                throw new OWLSemanticsException("Cannot annotate concept scheme because given \"annotationProperty\" parameter is a blank predicate");
            if (annotationValue == null)
                throw new OWLSemanticsException("Cannot annotate concept scheme because given \"annotationValue\" parameter is null");

            //Add knowledge to the A-BOX
            Ontology.Data.ABoxGraph.AddTriple(new RDFTriple(this, annotationProperty, annotationValue));

            return this;
        }

        /// <summary>
        /// Annotates the concept scheme with the given "annotationProperty -> annotationValue"
        /// </summary>
        public SKOSConceptScheme Annotate(RDFResource annotationProperty, RDFLiteral annotationValue)
        {
            if (annotationProperty == null)
                throw new OWLSemanticsException("Cannot annotate concept scheme because given \"annotationProperty\" parameter is null");
            if (annotationProperty.IsBlank)
                throw new OWLSemanticsException("Cannot annotate concept scheme because given \"annotationProperty\" parameter is a blank predicate");
            if (annotationValue == null)
                throw new OWLSemanticsException("Cannot annotate concept scheme because given \"annotationValue\" parameter is null");

            //Add knowledge to the A-BOX
            Ontology.Data.ABoxGraph.AddTriple(new RDFTriple(this, annotationProperty, annotationValue));

            return this;
        }

        /// <summary>
        /// Annotates the given skos:Concept with the given "annotationProperty -> annotationValue"
        /// </summary>
        public SKOSConceptScheme AnnotateConcept(RDFResource skosConcept, RDFResource annotationProperty, RDFResource annotationValue)
        {
            if (skosConcept == null)
                throw new OWLSemanticsException("Cannot annotate concept because given \"skosConcept\" parameter is null");
            if (annotationProperty == null)
                throw new OWLSemanticsException("Cannot annotate concept because given \"annotationProperty\" parameter is null");
            if (annotationProperty.IsBlank)
                throw new OWLSemanticsException("Cannot annotate concept because given \"annotationProperty\" parameter is a blank predicate");
            if (annotationValue == null)
                throw new OWLSemanticsException("Cannot annotate concept because given \"annotationValue\" parameter is null");

            //Add knowledge to the A-BOX
            Ontology.Data.ABoxGraph.AddTriple(new RDFTriple(skosConcept, annotationProperty, annotationValue));

            return this;
        }

        /// <summary>
        /// Annotates the given skos:Concept with the given "annotationProperty -> annotationValue"
        /// </summary>
        public SKOSConceptScheme AnnotateConcept(RDFResource skosConcept, RDFResource annotationProperty, RDFLiteral annotationValue)
        {
            if (skosConcept == null)
                throw new OWLSemanticsException("Cannot annotate concept because given \"skosConcept\" parameter is null");
            if (annotationProperty == null)
                throw new OWLSemanticsException("Cannot annotate concept because given \"annotationProperty\" parameter is null");
            if (annotationProperty.IsBlank)
                throw new OWLSemanticsException("Cannot annotate concept because given \"annotationProperty\" parameter is a blank predicate");
            if (annotationValue == null)
                throw new OWLSemanticsException("Cannot annotate concept because given \"annotationValue\" parameter is null");

            //Add knowledge to the A-BOX
            Ontology.Data.ABoxGraph.AddTriple(new RDFTriple(skosConcept, annotationProperty, annotationValue));

            return this;
        }

        /// <summary>
        /// Annotates the given skosxl:Label with the given "annotationProperty -> annotationValue"
        /// </summary>
        public SKOSConceptScheme AnnotateLabel(RDFResource skosxlLabel, RDFResource annotationProperty, RDFResource annotationValue)
        {
            if (skosxlLabel == null)
                throw new OWLSemanticsException("Cannot annotate label because given \"skosxlLabel\" parameter is null");
            if (annotationProperty == null)
                throw new OWLSemanticsException("Cannot annotate label because given \"annotationProperty\" parameter is null");
            if (annotationProperty.IsBlank)
                throw new OWLSemanticsException("Cannot annotate label because given \"annotationProperty\" parameter is a blank predicate");
            if (annotationValue == null)
                throw new OWLSemanticsException("Cannot annotate label because given \"annotationValue\" parameter is null");

            //Add knowledge to the A-BOX
            Ontology.Data.ABoxGraph.AddTriple(new RDFTriple(skosxlLabel, annotationProperty, annotationValue));

            return this;
        }

        /// <summary>
        /// Annotates the given skosxl:Label with the given "annotationProperty -> annotationValue"
        /// </summary>
        public SKOSConceptScheme AnnotateLabel(RDFResource skosxlLabel, RDFResource annotationProperty, RDFLiteral annotationValue)
        {
            if (skosxlLabel == null)
                throw new OWLSemanticsException("Cannot annotate label because given \"skosxlLabel\" parameter is null");
            if (annotationProperty == null)
                throw new OWLSemanticsException("Cannot annotate label because given \"annotationProperty\" parameter is null");
            if (annotationProperty.IsBlank)
                throw new OWLSemanticsException("Cannot annotate label because given \"annotationProperty\" parameter is a blank predicate");
            if (annotationValue == null)
                throw new OWLSemanticsException("Cannot annotate label because given \"annotationValue\" parameter is null");

            //Add knowledge to the A-BOX
            Ontology.Data.ABoxGraph.AddTriple(new RDFTriple(skosxlLabel, annotationProperty, annotationValue));

            return this;
        }

        /// <summary>
        /// Annotates the given skos:Collection with the given "annotationProperty -> annotationValue"
        /// </summary>
        public SKOSConceptScheme AnnotateCollection(RDFResource skosCollection, RDFResource annotationProperty, RDFResource annotationValue)
        {
            if (skosCollection == null)
                throw new OWLSemanticsException("Cannot annotate collection because given \"skosCollection\" parameter is null");
            if (annotationProperty == null)
                throw new OWLSemanticsException("Cannot annotate collection because given \"annotationProperty\" parameter is null");
            if (annotationProperty.IsBlank)
                throw new OWLSemanticsException("Cannot annotate collection because given \"annotationProperty\" parameter is a blank predicate");
            if (annotationValue == null)
                throw new OWLSemanticsException("Cannot annotate collection because given \"annotationValue\" parameter is null");

            //Add knowledge to the A-BOX
            Ontology.Data.ABoxGraph.AddTriple(new RDFTriple(skosCollection, annotationProperty, annotationValue));

            return this;
        }

        /// <summary>
        /// Annotates the given skos:Collection with the given "annotationProperty -> annotationValue"
        /// </summary>
        public SKOSConceptScheme AnnotateCollection(RDFResource skosCollection, RDFResource annotationProperty, RDFLiteral annotationValue)
        {
            if (skosCollection == null)
                throw new OWLSemanticsException("Cannot annotate collection because given \"skosCollection\" parameter is null");
            if (annotationProperty == null)
                throw new OWLSemanticsException("Cannot annotate collection because given \"annotationProperty\" parameter is null");
            if (annotationProperty.IsBlank)
                throw new OWLSemanticsException("Cannot annotate collection because given \"annotationProperty\" parameter is a blank predicate");
            if (annotationValue == null)
                throw new OWLSemanticsException("Cannot annotate collection because given \"annotationValue\" parameter is null");

            //Add knowledge to the A-BOX
            Ontology.Data.ABoxGraph.AddTriple(new RDFTriple(skosCollection, annotationProperty, annotationValue));

            return this;
        }

        /// <summary>
        /// Annotates the given skos:OrderedCollection with the given "annotationProperty -> annotationValue"
        /// </summary>
        public SKOSConceptScheme AnnotateOrderedCollection(RDFResource skosOrderedCollection, RDFResource annotationProperty, RDFResource annotationValue)
        {
            if (skosOrderedCollection == null)
                throw new OWLSemanticsException("Cannot annotate ordered collection because given \"skosOrderedCollection\" parameter is null");
            if (annotationProperty == null)
                throw new OWLSemanticsException("Cannot annotate ordered collection because given \"annotationProperty\" parameter is null");
            if (annotationProperty.IsBlank)
                throw new OWLSemanticsException("Cannot annotate ordered collection because given \"annotationProperty\" parameter is a blank predicate");
            if (annotationValue == null)
                throw new OWLSemanticsException("Cannot annotate ordered collection because given \"annotationValue\" parameter is null");

            //Add knowledge to the A-BOX
            Ontology.Data.ABoxGraph.AddTriple(new RDFTriple(skosOrderedCollection, annotationProperty, annotationValue));

            return this;
        }

        /// <summary>
        /// Annotates the given skos:OrderedCollection with the given "annotationProperty -> annotationValue"
        /// </summary>
        public SKOSConceptScheme AnnotateOrderedCollection(RDFResource skosOrderedCollection, RDFResource annotationProperty, RDFLiteral annotationValue)
        {
            if (skosOrderedCollection == null)
                throw new OWLSemanticsException("Cannot annotate ordered collection because given \"skosOrderedCollection\" parameter is null");
            if (annotationProperty == null)
                throw new OWLSemanticsException("Cannot annotate ordered collection because given \"annotationProperty\" parameter is null");
            if (annotationProperty.IsBlank)
                throw new OWLSemanticsException("Cannot annotate ordered collection because given \"annotationProperty\" parameter is a blank predicate");
            if (annotationValue == null)
                throw new OWLSemanticsException("Cannot annotate ordered collection because given \"annotationValue\" parameter is null");

            //Add knowledge to the A-BOX
            Ontology.Data.ABoxGraph.AddTriple(new RDFTriple(skosOrderedCollection, annotationProperty, annotationValue));

            return this;
        }

        /// <summary>
        ///  Declares the existence of the given "Note(skosConcept,noteValue)" annotation to the concept scheme
        /// </summary>
        public SKOSConceptScheme DeclareNote(RDFResource skosConcept, RDFLiteral noteValue)
        {
            if (skosConcept == null)
                throw new OWLSemanticsException("Cannot declare skos:note annotation to the concept scheme because given \"skosConcept\" parameter is null");
            if (noteValue == null)
                throw new OWLSemanticsException("Cannot declare skos:note annotation to the concept scheme because given \"noteValue\" parameter is null");

            //Add knowledge to the A-BOX
            DeclareConcept(skosConcept);
            Ontology.Data.ABoxGraph.AddTriple(new RDFTriple(skosConcept, RDFVocabulary.SKOS.NOTE, noteValue));

            return this;
        }

        /// <summary>
        ///  Declares the existence of the given "ChangeNote(skosConcept,changeNoteValue)" annotation to the concept scheme
        /// </summary>
        public SKOSConceptScheme DeclareChangeNote(RDFResource skosConcept, RDFLiteral changeNoteValue)
        {
            if (skosConcept == null)
                throw new OWLSemanticsException("Cannot declare skos:changeNote annotation to the concept scheme because given \"skosConcept\" parameter is null");
            if (changeNoteValue == null)
                throw new OWLSemanticsException("Cannot declare skos:changeNote annotation to the concept scheme because given \"changeNoteValue\" parameter is null");

            //Add knowledge to the A-BOX
            DeclareConcept(skosConcept);
            Ontology.Data.ABoxGraph.AddTriple(new RDFTriple(skosConcept, RDFVocabulary.SKOS.CHANGE_NOTE, changeNoteValue));

            return this;
        }

        /// <summary>
        ///  Declares the existence of the given "EditorialNote(skosConcept,editorialNoteValue)" annotation to the concept scheme
        /// </summary>
        public SKOSConceptScheme DeclareEditorialNote(RDFResource skosConcept, RDFLiteral editorialNoteValue)
        {
            if (skosConcept == null)
                throw new OWLSemanticsException("Cannot declare skos:editorialNote annotation to the concept scheme because given \"skosConcept\" parameter is null");
            if (editorialNoteValue == null)
                throw new OWLSemanticsException("Cannot declare skos:editorialNote annotation to the concept scheme because given \"editorialNoteValue\" parameter is null");

            //Add knowledge to the A-BOX
            DeclareConcept(skosConcept);
            Ontology.Data.ABoxGraph.AddTriple(new RDFTriple(skosConcept, RDFVocabulary.SKOS.EDITORIAL_NOTE, editorialNoteValue));

            return this;
        }

        /// <summary>
        ///  Declares the existence of the given "HistoryNote(skosConcept,historyNoteValue)" annotation to the concept scheme
        /// </summary>
        public SKOSConceptScheme DeclareHistoryNote(RDFResource skosConcept, RDFLiteral historyNoteValue)
        {
            if (skosConcept == null)
                throw new OWLSemanticsException("Cannot declare skos:historyNote annotation to the concept scheme because given \"skosConcept\" parameter is null");
            if (historyNoteValue == null)
                throw new OWLSemanticsException("Cannot declare skos:historyNote annotation to the concept scheme because given \"historyNoteValue\" parameter is null");

            //Add knowledge to the A-BOX
            DeclareConcept(skosConcept);
            Ontology.Data.ABoxGraph.AddTriple(new RDFTriple(skosConcept, RDFVocabulary.SKOS.HISTORY_NOTE, historyNoteValue));

            return this;
        }

        /// <summary>
        ///  Declares the existence of the given "ScopeNote(skosConcept,scopeNoteValue)" annotation to the concept scheme
        /// </summary>
        public SKOSConceptScheme DeclareScopeNote(RDFResource skosConcept, RDFLiteral scopeNoteValue)
        {
            if (skosConcept == null)
                throw new OWLSemanticsException("Cannot declare skos:scopeNote annotation to the concept scheme because given \"skosConcept\" parameter is null");
            if (scopeNoteValue == null)
                throw new OWLSemanticsException("Cannot declare skos:scopeNote annotation to the concept scheme because given \"scopeNoteValue\" parameter is null");

            //Add knowledge to the A-BOX
            DeclareConcept(skosConcept);
            Ontology.Data.ABoxGraph.AddTriple(new RDFTriple(skosConcept, RDFVocabulary.SKOS.SCOPE_NOTE, scopeNoteValue));

            return this;
        }

        /// <summary>
        ///  Declares the existence of the given "Definition(skosConcept,definitionValue)" annotation to the concept scheme
        /// </summary>
        public SKOSConceptScheme DeclareDefinition(RDFResource skosConcept, RDFLiteral definitionValue)
        {
            if (skosConcept == null)
                throw new OWLSemanticsException("Cannot declare skos:definition annotation to the concept scheme because given \"skosConcept\" parameter is null");
            if (definitionValue == null)
                throw new OWLSemanticsException("Cannot declare skos:definition annotation to the concept scheme because given \"definitionValue\" parameter is null");

            //Add knowledge to the A-BOX
            DeclareConcept(skosConcept);
            Ontology.Data.ABoxGraph.AddTriple(new RDFTriple(skosConcept, RDFVocabulary.SKOS.DEFINITION, definitionValue));

            return this;
        }

        /// <summary>
        ///  Declares the existence of the given "Example(skosConcept,exampleValue)" annotation to the concept scheme
        /// </summary>
        public SKOSConceptScheme DeclareExample(RDFResource skosConcept, RDFLiteral exampleValue)
        {
            if (skosConcept == null)
                throw new OWLSemanticsException("Cannot declare skos:example annotation to the concept scheme because given \"skosConcept\" parameter is null");
            if (exampleValue == null)
                throw new OWLSemanticsException("Cannot declare skos:example annotation to the concept scheme because given \"exampleValue\" parameter is null");

            //Add knowledge to the A-BOX
            DeclareConcept(skosConcept);
            Ontology.Data.ABoxGraph.AddTriple(new RDFTriple(skosConcept, RDFVocabulary.SKOS.EXAMPLE, exampleValue));

            return this;
        }

        //RELATIONS

        /// <summary>
        /// Declares the existence of the given "TopConceptOf(skosConcept,skosConceptScheme)" relation to the concept scheme
        /// </summary>
        public SKOSConceptScheme DeclareTopConcept(RDFResource skosConcept)
        {
            if (skosConcept == null)
                throw new OWLSemanticsException("Cannot declare skos:topConceptOf relation to the concept scheme because given \"skosConcept\" parameter is null");

            //Add knowledge to the A-BOX
            DeclareConcept(skosConcept);
            Ontology.Data.ABoxGraph.AddTriple(new RDFTriple(skosConcept, RDFVocabulary.SKOS.TOP_CONCEPT_OF, this));

            //Also add an automatic A-BOX inference exploiting owl:inverseOf relation
            Ontology.Data.ABoxGraph.AddTriple(new RDFTriple(this, RDFVocabulary.SKOS.HAS_TOP_CONCEPT, skosConcept));

            return this;
        }

        /// <summary>
        /// Declares the existence of the given "SemanticRelation(leftConcept,rightConcept)" relation to the concept scheme
        /// </summary>
        public SKOSConceptScheme DeclareSemanticRelatedConcepts(RDFResource leftConcept, RDFResource rightConcept)
        {
            if (leftConcept == null)
                throw new OWLSemanticsException("Cannot declare skos:semanticRelation relation to the concept scheme because given \"leftConcept\" parameter is null");
            if (rightConcept == null)
                throw new OWLSemanticsException("Cannot declare skos:semanticRelation relation to the concept scheme because given \"rightConcept\" parameter is null");

            //Add knowledge to the A-BOX
            DeclareConcept(leftConcept);
            DeclareConcept(rightConcept);
            Ontology.Data.ABoxGraph.AddTriple(new RDFTriple(leftConcept, RDFVocabulary.SKOS.SEMANTIC_RELATION, rightConcept));

            return this;
        }

        /// <summary>
        /// Declares the existence of the given "Related(leftConcept,rightConcept)" relation to the concept scheme
        /// </summary>
        public SKOSConceptScheme DeclareRelatedConcepts(RDFResource leftConcept, RDFResource rightConcept)
        {
            if (leftConcept == null)
                throw new OWLSemanticsException("Cannot declare skos:related relation to the concept scheme because given \"leftConcept\" parameter is null");
            if (rightConcept == null)
                throw new OWLSemanticsException("Cannot declare skos:related relation to the concept scheme because given \"rightConcept\" parameter is null");

            //Add knowledge to the A-BOX
            DeclareConcept(leftConcept);
            DeclareConcept(rightConcept);
            Ontology.Data.ABoxGraph.AddTriple(new RDFTriple(leftConcept, RDFVocabulary.SKOS.RELATED, rightConcept));

            //Also add an automatic A-BOX inference exploiting symmetry of skos:related relation
            Ontology.Data.ABoxGraph.AddTriple(new RDFTriple(rightConcept, RDFVocabulary.SKOS.RELATED, leftConcept));

            return this;
        }

        /// <summary>
        /// Declares the existence of the given "Broader(childConcept,motherConcept)" relation to the concept scheme
        /// </summary>
        public SKOSConceptScheme DeclareBroaderConcepts(RDFResource childConcept, RDFResource motherConcept)
        {
            #region SKOS Integrity Checks
            bool SKOSIntegrityChecks()
                => this.CheckBroaderCompatibility(childConcept, motherConcept);
            #endregion

            if (childConcept == null)
                throw new OWLSemanticsException("Cannot declare skos:broader relation to the concept scheme because given \"childConcept\" parameter is null");
            if (motherConcept == null)
                throw new OWLSemanticsException("Cannot declare skos:broader relation to the concept scheme because given \"motherConcept\" parameter is null");
            if (childConcept.Equals(motherConcept))
                throw new OWLSemanticsException("Cannot declare skos:broader relation to the concept scheme because given \"childConcept\" parameter refers to the same concept as the given \"motherConcept\" parameter");

            //Add knowledge to the A-BOX (or raise warning if violations are detected)
            if (SKOSIntegrityChecks())
            {
                //Add knowledge to the A-BOX
                DeclareConcept(childConcept);
                DeclareConcept(motherConcept);
                Ontology.Data.ABoxGraph.AddTriple(new RDFTriple(childConcept, RDFVocabulary.SKOS.BROADER, motherConcept));

                //Also add an automatic A-BOX inference exploiting owl:inverseOf relation
                Ontology.Data.ABoxGraph.AddTriple(new RDFTriple(motherConcept, RDFVocabulary.SKOS.NARROWER, childConcept));
            }
            else
                OWLSemanticsEvents.RaiseSemanticsWarning(string.Format("Broader relation between concept '{0}' and concept '{1}' cannot be declared to the concept scheme because it would violate SKOS integrity", childConcept, motherConcept));

            return this;
        }

        /// <summary>
        /// Declares the existence of the given "BroaderTransitive(childConcept,motherConcept)" relation to the concept scheme
        /// </summary>
        public SKOSConceptScheme DeclareBroaderTransitiveConcepts(RDFResource childConcept, RDFResource motherConcept)
        {
            #region SKOS Integrity Checks
            bool SKOSIntegrityChecks()
                => this.CheckBroaderCompatibility(childConcept, motherConcept);
            #endregion

            if (childConcept == null)
                throw new OWLSemanticsException("Cannot declare skos:broaderTransitive relation to the concept scheme because given \"childConcept\" parameter is null");
            if (motherConcept == null)
                throw new OWLSemanticsException("Cannot declare skos:broaderTransitive relation to the concept scheme because given \"motherConcept\" parameter is null");
            if (childConcept.Equals(motherConcept))
                throw new OWLSemanticsException("Cannot declare skos:broaderTransitive relation to the concept scheme because given \"childConcept\" parameter refers to the same concept as the given \"motherConcept\" parameter");

            //Add knowledge to the A-BOX (or raise warning if violations are detected)
            if (SKOSIntegrityChecks())
            {
                //Add knowledge to the A-BOX
                DeclareConcept(childConcept);
                DeclareConcept(motherConcept);
                Ontology.Data.ABoxGraph.AddTriple(new RDFTriple(childConcept, RDFVocabulary.SKOS.BROADER_TRANSITIVE, motherConcept));

                //Also add an automatic A-BOX inference exploiting owl:inverseOf relation
                Ontology.Data.ABoxGraph.AddTriple(new RDFTriple(motherConcept, RDFVocabulary.SKOS.NARROWER_TRANSITIVE, childConcept));
            }
            else
                OWLSemanticsEvents.RaiseSemanticsWarning(string.Format("BroaderTransitive relation between concept '{0}' and concept '{1}' cannot be declared to the concept scheme because it would violate SKOS integrity", childConcept, motherConcept));

            return this;
        }

        /// <summary>
        /// Declares the existence of the given "Narrower(motherConcept,childConcept)" relation to the concept scheme
        /// </summary>
        public SKOSConceptScheme DeclareNarrowerConcepts(RDFResource motherConcept, RDFResource childConcept)
        {
            #region SKOS Integrity Checks
            bool SKOSIntegrityChecks()
                => this.CheckNarrowerCompatibility(motherConcept, childConcept);
            #endregion

            if (childConcept == null)
                throw new OWLSemanticsException("Cannot declare skos:narrower relation to the concept scheme because given \"childConcept\" parameter is null");
            if (motherConcept == null)
                throw new OWLSemanticsException("Cannot declare skos:narrower relation to the concept scheme because given \"motherConcept\" parameter is null");
            if (childConcept.Equals(motherConcept))
                throw new OWLSemanticsException("Cannot declare skos:narrower relation to the concept scheme because given \"childConcept\" parameter refers to the same concept as the given \"motherConcept\" parameter");

            //Add knowledge to the A-BOX (or raise warning if violations are detected)
            if (SKOSIntegrityChecks())
            {
                //Add knowledge to the A-BOX
                DeclareConcept(childConcept);
                DeclareConcept(motherConcept);
                Ontology.Data.ABoxGraph.AddTriple(new RDFTriple(motherConcept, RDFVocabulary.SKOS.NARROWER, childConcept));

                //Also add an automatic A-BOX inference exploiting owl:inverseOf relation
                Ontology.Data.ABoxGraph.AddTriple(new RDFTriple(childConcept, RDFVocabulary.SKOS.BROADER, motherConcept));
            }
            else
                OWLSemanticsEvents.RaiseSemanticsWarning(string.Format("Narrower relation between concept '{0}' and concept '{1}' cannot be declared to the concept scheme because it would violate SKOS integrity", motherConcept, childConcept));

            return this;
        }

        /// <summary>
        /// Declares the existence of the given "NarrowerTransitive(motherConcept,childConcept)" relation to the concept scheme
        /// </summary>
        public SKOSConceptScheme DeclareNarrowerTransitiveConcepts(RDFResource motherConcept, RDFResource childConcept)
        {
            #region SKOS Integrity Checks
            bool SKOSIntegrityChecks()
                => this.CheckNarrowerCompatibility(motherConcept, childConcept);
            #endregion

            if (childConcept == null)
                throw new OWLSemanticsException("Cannot declare skos:narrowerTransitive relation to the concept scheme because given \"childConcept\" parameter is null");
            if (motherConcept == null)
                throw new OWLSemanticsException("Cannot declare skos:narrowerTransitive relation to the concept scheme because given \"motherConcept\" parameter is null");
            if (childConcept.Equals(motherConcept))
                throw new OWLSemanticsException("Cannot declare skos:narrowerTransitive relation to the concept scheme because given \"childConcept\" parameter refers to the same concept as the given \"motherConcept\" parameter");

            //Add knowledge to the A-BOX (or raise warning if violations are detected)
            if (SKOSIntegrityChecks())
            {
                //Add knowledge to the A-BOX
                DeclareConcept(childConcept);
                DeclareConcept(motherConcept);
                Ontology.Data.ABoxGraph.AddTriple(new RDFTriple(motherConcept, RDFVocabulary.SKOS.NARROWER_TRANSITIVE, childConcept));

                //Also add an automatic A-BOX inference exploiting owl:inverseOf relation
                Ontology.Data.ABoxGraph.AddTriple(new RDFTriple(childConcept, RDFVocabulary.SKOS.BROADER_TRANSITIVE, motherConcept));
            }
            else
                OWLSemanticsEvents.RaiseSemanticsWarning(string.Format("NarrowerTransitive relation between concept '{0}' and concept '{1}' cannot be declared to the concept scheme because it would violate SKOS integrity", motherConcept, childConcept));

            return this;
        }

        /// <summary>
        ///  Declares the existence of the given "Notation(skosConcept,notationValue)" relation to the concept scheme
        /// </summary>
        public SKOSConceptScheme DeclareNotation(RDFResource skosConcept, RDFLiteral notationValue)
        {
            if (skosConcept == null)
                throw new OWLSemanticsException("Cannot declare skos:notation relation to the concept scheme because given \"skosConcept\" parameter is null");
            if (notationValue == null)
                throw new OWLSemanticsException("Cannot declare skos:notation relation to the concept scheme because given \"notationValue\" parameter is null");

            //Add knowledge to the A-BOX
            DeclareConcept(skosConcept);
            Ontology.Data.ABoxGraph.AddTriple(new RDFTriple(skosConcept, RDFVocabulary.SKOS.NOTATION, notationValue));

            return this;
        }

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