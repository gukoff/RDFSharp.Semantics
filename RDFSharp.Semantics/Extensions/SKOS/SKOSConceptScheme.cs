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
            => Concepts.Count;

        /// <summary>
        /// Count of the collections
        /// </summary>
        public long CollectionsCount
            => Collections.Count;

        /// <summary>
        /// Count of the labels [SKOS-XL]
        /// </summary>
        public long LabelsCount
            => Labels.Count;

        /// <summary>
        /// Gets the enumerator on the concepts for iteration
        /// </summary>
        public IEnumerator<RDFResource> ConceptsEnumerator
            => Concepts.Values.GetEnumerator();

        /// <summary>
        /// Gets the enumerator on the collections for iteration
        /// </summary>
        public IEnumerator<RDFResource> CollectionsEnumerator
            => Collections.Values.GetEnumerator();

        /// <summary>
        /// Gets the enumerator on the labels for iteration [SKOS-XL]
        /// </summary>
        public IEnumerator<RDFResource> LabelsEnumerator
            => Labels.Values.GetEnumerator();

        /// <summary>
        /// Collection of concepts
        /// </summary>
        internal Dictionary<long, RDFResource> Concepts { get; set; }

        /// <summary>
        /// Collection of collections
        /// </summary>
        internal Dictionary<long, SKOSCollection> Collections { get; set; }

        /// <summary>
        /// Collection of labels [SKOS-XL]
        /// </summary>
        internal Dictionary<long, RDFResource> Labels { get; set; }

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
            Concepts = new Dictionary<long, RDFResource>();
            Collections = new Dictionary<long, SKOSCollection>();
            Labels = new Dictionary<long, RDFResource>();
            Ontology = new OWLOntology(conceptSchemeURI) { Model = SKOSOntology.Instance.Model, Data = SKOSOntology.Instance.Data };

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

            //Declare concept to the concept scheme
            if (!Concepts.ContainsKey(skosConcept.PatternMemberID))
                Concepts.Add(skosConcept.PatternMemberID, skosConcept);

            //Add knowledge to the A-BOX
            Ontology.Data.DeclareIndividual(skosConcept);
            Ontology.Data.DeclareIndividualType(skosConcept, RDFVocabulary.SKOS.CONCEPT);
            Ontology.Data.DeclareObjectAssertion(skosConcept, RDFVocabulary.SKOS.IN_SCHEME, this);

            return this;
        }

        /// <summary>
        /// Declares the given skos:Collection instance to the concept scheme
        /// </summary>
        public SKOSConceptScheme DeclareCollection(SKOSCollection skosCollection)
        {
            if (skosCollection == null)
                throw new OWLSemanticsException("Cannot declare skos:Collection instance to the concept scheme because given \"skosCollection\" parameter is null");

            //Declare collection to the concept scheme
            if (!Collections.ContainsKey(skosCollection.PatternMemberID))
                Collections.Add(skosCollection.PatternMemberID, skosCollection);

            //Add knowledge to the A-BOX
            Ontology.Data.DeclareObjectAssertion(skosCollection, RDFVocabulary.SKOS.IN_SCHEME, this);
            foreach (RDFTriple skosCollectionTriple in skosCollection.Ontology.Data.ABoxGraph)
                Ontology.Data.ABoxGraph.AddTriple(skosCollectionTriple);

            return this;
        }

        /// <summary>
        /// Declares the given skosxl:Label instance to the concept scheme
        /// </summary>
        public SKOSConceptScheme DeclareLabel(RDFResource skosLabel)
        {
            if (skosLabel == null)
                throw new OWLSemanticsException("Cannot declare skosxl:Label instance to the concept scheme because given \"skosLabel\" parameter is null");

            //Declare label to the concept scheme
            if (!Labels.ContainsKey(skosLabel.PatternMemberID))
                Labels.Add(skosLabel.PatternMemberID, skosLabel);

            //Add knowledge to the A-BOX
            Ontology.Data.DeclareIndividual(skosLabel);
            Ontology.Data.DeclareIndividualType(skosLabel, RDFVocabulary.SKOS.SKOSXL.LABEL);
            Ontology.Data.DeclareObjectAssertion(skosLabel, RDFVocabulary.SKOS.IN_SCHEME, this);

            return this;
        }
        #endregion
    }
}