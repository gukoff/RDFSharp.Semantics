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
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;

namespace RDFSharp.Semantics.Extensions.SKOS
{
    /// <summary>
    /// SKOSOntology represents an OWL-DL ontology implementation of W3C SKOS vocabulary
    /// </summary>
    [ExcludeFromCodeCoverage]
    internal static class SKOSOntology
    {
        #region Properties
        /// <summary>
        /// Singleton instance of the SKOS ontology
        /// </summary>
        internal static OWLOntology Instance { get; set; }
        #endregion

        #region Ctors
        /// <summary>
        /// Default-ctor to initialize the SKOS ontology
        /// </summary>
        static SKOSOntology()
        {
            Instance = new OWLOntology(RDFVocabulary.SKOS.BASE_URI);

            #region Classes
            Instance.Model.ClassModel.DeclareClass(RDFVocabulary.SKOS.COLLECTION);
            Instance.Model.ClassModel.DeclareClass(RDFVocabulary.SKOS.CONCEPT);
            Instance.Model.ClassModel.DeclareClass(RDFVocabulary.SKOS.CONCEPT_SCHEME);
            Instance.Model.ClassModel.DeclareClass(RDFVocabulary.SKOS.ORDERED_COLLECTION);
            Instance.Model.ClassModel.DeclareUnionClass(new RDFResource("bnode:ConceptCollection"), new List<RDFResource>() { RDFVocabulary.SKOS.CONCEPT, RDFVocabulary.SKOS.COLLECTION });
            Instance.Model.ClassModel.DeclareCardinalityRestriction(new RDFResource("bnode:ExactlyOneLiteralForm"), RDFVocabulary.SKOS.SKOSXL.LITERAL_FORM, 1);
            Instance.Model.ClassModel.DeclareAllDisjointClasses(new RDFResource("bnode:AllDisjointSKOSClasses"), new List<RDFResource>() { RDFVocabulary.SKOS.COLLECTION, RDFVocabulary.SKOS.CONCEPT, RDFVocabulary.SKOS.CONCEPT_SCHEME, RDFVocabulary.SKOS.SKOSXL.LABEL });
            Instance.Model.ClassModel.DeclareSubClasses(RDFVocabulary.SKOS.ORDERED_COLLECTION, RDFVocabulary.SKOS.COLLECTION);            
            //SKOS-XL
            Instance.Model.ClassModel.DeclareClass(RDFVocabulary.SKOS.SKOSXL.LABEL);
            Instance.Model.ClassModel.DeclareSubClasses(RDFVocabulary.SKOS.SKOSXL.LABEL, new RDFResource("bnode:ExactlyOneLiteralForm"));
            #endregion

            #region Properties
            Instance.Model.PropertyModel.DeclareAnnotationProperty(RDFVocabulary.SKOS.ALT_LABEL);
            Instance.Model.PropertyModel.DeclareObjectProperty(RDFVocabulary.SKOS.BROAD_MATCH);
            Instance.Model.PropertyModel.DeclareObjectProperty(RDFVocabulary.SKOS.BROADER);
            Instance.Model.PropertyModel.DeclareObjectProperty(RDFVocabulary.SKOS.BROADER_TRANSITIVE, new OWLOntologyObjectPropertyBehavior() { Transitive = true });
            Instance.Model.PropertyModel.DeclareAnnotationProperty(RDFVocabulary.SKOS.CHANGE_NOTE);
            Instance.Model.PropertyModel.DeclareObjectProperty(RDFVocabulary.SKOS.CLOSE_MATCH, new OWLOntologyObjectPropertyBehavior() { Symmetric = true });
            Instance.Model.PropertyModel.DeclareAnnotationProperty(RDFVocabulary.SKOS.DEFINITION);
            Instance.Model.PropertyModel.DeclareAnnotationProperty(RDFVocabulary.SKOS.EDITORIAL_NOTE);
            Instance.Model.PropertyModel.DeclareObjectProperty(RDFVocabulary.SKOS.EXACT_MATCH, new OWLOntologyObjectPropertyBehavior() { Symmetric = true, Transitive = true });
            Instance.Model.PropertyModel.DeclareAnnotationProperty(RDFVocabulary.SKOS.EXAMPLE);
            Instance.Model.PropertyModel.DeclareObjectProperty(RDFVocabulary.SKOS.HAS_TOP_CONCEPT, new OWLOntologyObjectPropertyBehavior() { Domain = RDFVocabulary.SKOS.CONCEPT_SCHEME, Range = RDFVocabulary.SKOS.CONCEPT });
            Instance.Model.PropertyModel.DeclareAnnotationProperty(RDFVocabulary.SKOS.HIDDEN_LABEL);
            Instance.Model.PropertyModel.DeclareAnnotationProperty(RDFVocabulary.SKOS.HISTORY_NOTE);
            Instance.Model.PropertyModel.DeclareObjectProperty(RDFVocabulary.SKOS.NARROW_MATCH);
            Instance.Model.PropertyModel.DeclareObjectProperty(RDFVocabulary.SKOS.NARROWER);
            Instance.Model.PropertyModel.DeclareObjectProperty(RDFVocabulary.SKOS.NARROWER_TRANSITIVE, new OWLOntologyObjectPropertyBehavior() { Transitive = true });
            Instance.Model.PropertyModel.DeclareObjectProperty(RDFVocabulary.SKOS.IN_SCHEME, new OWLOntologyObjectPropertyBehavior() { Range = RDFVocabulary.SKOS.CONCEPT_SCHEME });
            Instance.Model.PropertyModel.DeclareObjectProperty(RDFVocabulary.SKOS.MAPPING_RELATION);
            Instance.Model.PropertyModel.DeclareObjectProperty(RDFVocabulary.SKOS.MEMBER, new OWLOntologyObjectPropertyBehavior() { Domain = RDFVocabulary.SKOS.COLLECTION, Range = new RDFResource("bnode:ConceptCollection") });
            Instance.Model.PropertyModel.DeclareObjectProperty(RDFVocabulary.SKOS.MEMBER_LIST, new OWLOntologyObjectPropertyBehavior() { Functional = true, Domain = RDFVocabulary.SKOS.ORDERED_COLLECTION });
            Instance.Model.PropertyModel.DeclareDatatypeProperty(RDFVocabulary.SKOS.NOTATION);
            Instance.Model.PropertyModel.DeclareAnnotationProperty(RDFVocabulary.SKOS.NOTE);
            Instance.Model.PropertyModel.DeclareAnnotationProperty(RDFVocabulary.SKOS.PREF_LABEL);
            Instance.Model.PropertyModel.DeclareObjectProperty(RDFVocabulary.SKOS.RELATED_MATCH, new OWLOntologyObjectPropertyBehavior() { Symmetric = true });
            Instance.Model.PropertyModel.DeclareObjectProperty(RDFVocabulary.SKOS.RELATED, new OWLOntologyObjectPropertyBehavior() { Symmetric = true });
            Instance.Model.PropertyModel.DeclareAnnotationProperty(RDFVocabulary.SKOS.SCOPE_NOTE);
            Instance.Model.PropertyModel.DeclareObjectProperty(RDFVocabulary.SKOS.SEMANTIC_RELATION, new OWLOntologyObjectPropertyBehavior() { Domain = RDFVocabulary.SKOS.CONCEPT, Range = RDFVocabulary.SKOS.CONCEPT });
            Instance.Model.PropertyModel.DeclareObjectProperty(RDFVocabulary.SKOS.TOP_CONCEPT_OF, new OWLOntologyObjectPropertyBehavior() { Domain = RDFVocabulary.SKOS.CONCEPT, Range = RDFVocabulary.SKOS.CONCEPT_SCHEME });
            Instance.Model.PropertyModel.DeclareSubProperties(RDFVocabulary.SKOS.BROAD_MATCH, RDFVocabulary.SKOS.BROADER);
            Instance.Model.PropertyModel.DeclareSubProperties(RDFVocabulary.SKOS.BROAD_MATCH, RDFVocabulary.SKOS.MAPPING_RELATION);
            Instance.Model.PropertyModel.DeclareSubProperties(RDFVocabulary.SKOS.BROADER, RDFVocabulary.SKOS.BROADER_TRANSITIVE);
            Instance.Model.PropertyModel.DeclareSubProperties(RDFVocabulary.SKOS.BROADER_TRANSITIVE, RDFVocabulary.SKOS.SEMANTIC_RELATION);
            Instance.Model.PropertyModel.DeclareSubProperties(RDFVocabulary.SKOS.CLOSE_MATCH, RDFVocabulary.SKOS.MAPPING_RELATION);
            Instance.Model.PropertyModel.DeclareSubProperties(RDFVocabulary.SKOS.EXACT_MATCH, RDFVocabulary.SKOS.CLOSE_MATCH);
            Instance.Model.PropertyModel.DeclareSubProperties(RDFVocabulary.SKOS.MAPPING_RELATION, RDFVocabulary.SKOS.SEMANTIC_RELATION);
            Instance.Model.PropertyModel.DeclareSubProperties(RDFVocabulary.SKOS.NARROW_MATCH, RDFVocabulary.SKOS.NARROWER);
            Instance.Model.PropertyModel.DeclareSubProperties(RDFVocabulary.SKOS.NARROW_MATCH, RDFVocabulary.SKOS.MAPPING_RELATION);
            Instance.Model.PropertyModel.DeclareSubProperties(RDFVocabulary.SKOS.NARROWER, RDFVocabulary.SKOS.NARROWER_TRANSITIVE);
            Instance.Model.PropertyModel.DeclareSubProperties(RDFVocabulary.SKOS.NARROWER_TRANSITIVE,RDFVocabulary.SKOS.SEMANTIC_RELATION);
            Instance.Model.PropertyModel.DeclareSubProperties(RDFVocabulary.SKOS.TOP_CONCEPT_OF, RDFVocabulary.SKOS.IN_SCHEME);
            Instance.Model.PropertyModel.DeclareSubProperties(RDFVocabulary.SKOS.RELATED_MATCH, RDFVocabulary.SKOS.RELATED);
            Instance.Model.PropertyModel.DeclareSubProperties(RDFVocabulary.SKOS.RELATED_MATCH, RDFVocabulary.SKOS.MAPPING_RELATION);
            Instance.Model.PropertyModel.DeclareSubProperties(RDFVocabulary.SKOS.RELATED, RDFVocabulary.SKOS.SEMANTIC_RELATION);
            Instance.Model.PropertyModel.DeclareInverseProperties(RDFVocabulary.SKOS.BROAD_MATCH, RDFVocabulary.SKOS.NARROW_MATCH);
            Instance.Model.PropertyModel.DeclareInverseProperties(RDFVocabulary.SKOS.BROADER, RDFVocabulary.SKOS.NARROWER);
            Instance.Model.PropertyModel.DeclareInverseProperties(RDFVocabulary.SKOS.BROADER_TRANSITIVE, RDFVocabulary.SKOS.NARROWER_TRANSITIVE);
            Instance.Model.PropertyModel.DeclareInverseProperties(RDFVocabulary.SKOS.HAS_TOP_CONCEPT, RDFVocabulary.SKOS.TOP_CONCEPT_OF);
            //SKOS-XL
            Instance.Model.PropertyModel.DeclareDatatypeProperty(RDFVocabulary.SKOS.SKOSXL.LITERAL_FORM, new OWLOntologyDatatypePropertyBehavior() {  Domain = RDFVocabulary.SKOS.SKOSXL.LABEL });
            Instance.Model.PropertyModel.DeclareObjectProperty(RDFVocabulary.SKOS.SKOSXL.PREF_LABEL, new OWLOntologyObjectPropertyBehavior() { Range = RDFVocabulary.SKOS.SKOSXL.LABEL });
            Instance.Model.PropertyModel.DeclareObjectProperty(RDFVocabulary.SKOS.SKOSXL.ALT_LABEL, new OWLOntologyObjectPropertyBehavior() { Range = RDFVocabulary.SKOS.SKOSXL.LABEL });
            Instance.Model.PropertyModel.DeclareObjectProperty(RDFVocabulary.SKOS.SKOSXL.HIDDEN_LABEL, new OWLOntologyObjectPropertyBehavior() { Range = RDFVocabulary.SKOS.SKOSXL.LABEL });
            Instance.Model.PropertyModel.DeclareObjectProperty(RDFVocabulary.SKOS.SKOSXL.LABEL_RELATION, new OWLOntologyObjectPropertyBehavior() { Symmetric = true, Domain = RDFVocabulary.SKOS.SKOSXL.LABEL, Range = RDFVocabulary.SKOS.SKOSXL.LABEL });
            #endregion
        }
        #endregion
    }
}