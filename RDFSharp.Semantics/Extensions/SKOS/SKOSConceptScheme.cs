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
    /// SKOSConceptScheme represents an organized collection of skos:Concept individuals
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
        /// Gets the enumerator on the concepts for iteration
        /// </summary>
        public IEnumerator<RDFResource> ConceptsEnumerator
            => Concepts.Values.GetEnumerator();

        /// <summary>
        /// Collection of concepts
        /// </summary>
        internal Dictionary<long, RDFResource> Concepts { get; set; }

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
            Ontology = new OWLOntology(conceptSchemeURI) {
                Model = SKOSOntology.Instance.Model,
                Data = SKOSOntology.Instance.Data
            };

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

            //Declare individual to the concept scheme
            if (!Concepts.ContainsKey(skosConcept.PatternMemberID))
                Concepts.Add(skosConcept.PatternMemberID, skosConcept);

            //Add knowledge to the A-BOX
            Ontology.Data.DeclareIndividual(skosConcept);
            Ontology.Data.DeclareIndividualType(skosConcept, RDFVocabulary.SKOS.CONCEPT);
            Ontology.Data.DeclareObjectAssertion(skosConcept, RDFVocabulary.SKOS.IN_SCHEME, this);

            return this;
        }
        #endregion
    }
}