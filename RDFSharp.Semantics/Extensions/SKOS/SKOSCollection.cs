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
using System;
using System.Collections.Generic;

namespace RDFSharp.Semantics.Extensions.SKOS
{
    /// <summary>
    /// SKOSCollection represents an organized list of skos:Concept individuals
    /// </summary>
    public class SKOSCollection : RDFResource
    {
        #region Properties
        /// <summary>
        /// Count of the concepts of the collection
        /// </summary>
        public long ConceptsCount
            => this.Concepts.Count;

        /// <summary>
        /// Count of the collections of the collection
        /// </summary>
        public long CollectionsCount
            => this.Collections.Count;

        /// <summary>
        /// Gets the enumerator on the concepts of the collection
        /// </summary>
        public IEnumerator<RDFResource> ConceptsEnumerator
            => this.Concepts.Values.GetEnumerator();

        /// <summary>
        /// Gets the enumerator on the collections of the collection
        /// </summary>
        public IEnumerator<SKOSCollection> CollectionsEnumerator
            => this.Collections.Values.GetEnumerator();

        /// <summary>
        /// Dictionary of concepts contained in the collection
        /// </summary>
        internal Dictionary<long, RDFResource> Concepts { get; set; }

        /// <summary>
        /// Dictionary of collections contained in the collection
        /// </summary>
        internal Dictionary<long, SKOSCollection> Collections { get; set; }

        /// <summary>
        /// Knowledge describing the collection
        /// </summary>
        internal OWLOntology Ontology { get; set; }
        #endregion

        #region Ctors
        /// <summary>
        /// Builds an empty collection with the given URI
        /// </summary>
        public SKOSCollection(string collectionURI) : base(collectionURI)
        {
            Concepts = new Dictionary<long, RDFResource>();
            Collections = new Dictionary<long, SKOSCollection>();
            Ontology = new OWLOntology(collectionURI);

            //Declare collection to the data
            Ontology.Data.DeclareIndividual(this);
            Ontology.Data.DeclareIndividualType(this, RDFVocabulary.SKOS.COLLECTION);
        }
        #endregion

        #region Methods
        /// <summary>
        /// Declares the given skos:Concept to the collection
        /// </summary>
        public SKOSCollection DeclareConcept(RDFResource skosConcept)
        {
            if (skosConcept == null)
                throw new OWLSemanticsException("Cannot declare skos:Concept instance to the collection because given \"skosConcept\" parameter is null");

            //Declare concept to the collection
            if (!Concepts.ContainsKey(skosConcept.PatternMemberID))
                Concepts.Add(skosConcept.PatternMemberID, skosConcept);

            //Add knowledge to the A-BOX
            Ontology.Data.DeclareIndividual(skosConcept);
            Ontology.Data.DeclareIndividualType(skosConcept, RDFVocabulary.SKOS.CONCEPT);
            Ontology.Data.DeclareObjectAssertion(this, RDFVocabulary.SKOS.MEMBER, skosConcept);

            return this;
        }

        /// <summary>
        /// Declares the given skos:Collection to the collection
        /// </summary>
        public SKOSCollection DeclareCollection(SKOSCollection skosCollection)
        {
            if (skosCollection == null)
                throw new OWLSemanticsException("Cannot declare skos:Collection instance to the collection because given \"skosCollection\" parameter is null");
            if (skosCollection.GetMembers().Count == 0)
                throw new OWLSemanticsException("Cannot declare skos:Collection instance to the collection because given \"skosCollection\" parameter does not contain concepts");

            //Declare collection to the collection
            if (!Collections.ContainsKey(skosCollection.PatternMemberID))
                Collections.Add(skosCollection.PatternMemberID, skosCollection);

            //Add knowledge to the A-BOX
            Ontology.Data.DeclareObjectAssertion(this, RDFVocabulary.SKOS.MEMBER, skosCollection);

            return this;
        }

        /// <summary>
        /// Gets the complete list of concepts contained in the collection
        /// </summary>
        public List<RDFResource> GetMembers()
        {
            List<RDFResource> concepts = new List<RDFResource>();

            //Concepts
            concepts.AddRange(Concepts.Values);

            //Collections
            foreach (SKOSCollection collection in Collections.Values)
                concepts.AddRange(collection.GetMembers());

            return concepts;
        }
        #endregion
    }
}