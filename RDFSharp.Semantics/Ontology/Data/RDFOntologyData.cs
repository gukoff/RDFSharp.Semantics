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
using System.Threading.Tasks;

namespace RDFSharp.Semantics
{
    /// <summary>
    /// RDFOntologyData represents the data component (A-BOX) of an ontology.
    /// </summary>
    public class RDFOntologyData : IEnumerable<RDFOntologyIndividual>
    {
        #region Properties
        /// <summary>
        /// Count of the individuals contained in the ontology data
        /// </summary>
        public long IndividualsCount => this.Individuals.Count;

        /// <summary>
        /// Count of the literals contained in the ontology data
        /// </summary>
        public long LiteralsCount => this.Literals.Count;

        /// <summary>
        /// Gets the enumerator on the individuals of the ontology data for iteration
        /// </summary>
        public IEnumerator<RDFOntologyIndividual> IndividualsEnumerator => this.Individuals.Values.GetEnumerator();

        /// <summary>
        /// Gets the enumerator on the literals of the ontology data for iteration
        /// </summary>
        public IEnumerator<RDFOntologyLiteral> LiteralsEnumerator => this.Literals.Values.GetEnumerator();

        /// <summary>
        /// Annotations describing individuals of the ontology data
        /// </summary>
        public RDFOntologyAnnotations Annotations { get; internal set; }

        /// <summary>
        /// Relations describing individuals of the ontology data
        /// </summary>
        public RDFOntologyDataMetadata Relations { get; internal set; }

        /// <summary>
        /// Individuals contained in the ontology data
        /// </summary>
        internal Dictionary<long, RDFOntologyIndividual> Individuals { get; set; }

        /// <summary>
        /// Literals contained in the ontology data
        /// </summary>
        internal Dictionary<long, RDFOntologyLiteral> Literals { get; set; }
        #endregion

        #region Ctors
        /// <summary>
        /// Default-ctor to build an empty ontology data
        /// </summary>
        public RDFOntologyData()
        {
            this.Individuals = new Dictionary<long, RDFOntologyIndividual>();
            this.Literals = new Dictionary<long, RDFOntologyLiteral>();
            this.Annotations = new RDFOntologyAnnotations();
            this.Relations = new RDFOntologyDataMetadata();
        }
        #endregion

        #region Interfaces
        /// <summary>
        /// Exposes a typed enumerator on the ontology data's individuals
        /// </summary>
        IEnumerator<RDFOntologyIndividual> IEnumerable<RDFOntologyIndividual>.GetEnumerator() => this.IndividualsEnumerator;

        /// <summary>
        /// Exposes an untyped enumerator on the data's individuals
        /// </summary>
        IEnumerator IEnumerable.GetEnumerator() => this.IndividualsEnumerator;
        #endregion

        #region Methods

        #region Add
        /// <summary>
        /// Adds the given individual to the ontology data
        /// </summary>
        public RDFOntologyData AddIndividual(RDFOntologyIndividual ontologyIndividual)
        {
            if (ontologyIndividual != null)
            {
                if (!this.Individuals.ContainsKey(ontologyIndividual.PatternMemberID))
                    this.Individuals.Add(ontologyIndividual.PatternMemberID, ontologyIndividual);
            }
            return this;
        }

        /// <summary>
        /// Adds the given literal to the ontology data
        /// </summary>
        public RDFOntologyData AddLiteral(RDFOntologyLiteral ontologyLiteral)
        {
            if (ontologyLiteral != null)
            {
                if (!this.Literals.ContainsKey(ontologyLiteral.PatternMemberID))
                    this.Literals.Add(ontologyLiteral.PatternMemberID, ontologyLiteral);
            }
            return this;
        }

        /// <summary>
        /// Adds the given standard annotation to the given ontology individual
        /// </summary>
        public RDFOntologyData AddStandardAnnotation(RDFSemanticsEnums.RDFOntologyStandardAnnotation standardAnnotation, RDFOntologyIndividual ontologyIndividual, RDFOntologyResource annotationValue)
        {
            if (ontologyIndividual != null && annotationValue != null)
            {
                switch (standardAnnotation)
                {
                    //owl:versionInfo
                    case RDFSemanticsEnums.RDFOntologyStandardAnnotation.VersionInfo:
                        if (annotationValue.IsLiteral())
                            this.Annotations.VersionInfo.AddEntry(new RDFOntologyTaxonomyEntry(ontologyIndividual, RDFVocabulary.OWL.VERSION_INFO.ToRDFOntologyAnnotationProperty(), annotationValue));
                        else
                            RDFSemanticsEvents.RaiseSemanticsInfo(string.Format("Cannot annotate ontology individual with owl:versionInfo value '{0}' because it is not an ontology literal", annotationValue));
                        break;

                    //rdfs:comment
                    case RDFSemanticsEnums.RDFOntologyStandardAnnotation.Comment:
                        if (annotationValue.IsLiteral())
                            this.Annotations.Comment.AddEntry(new RDFOntologyTaxonomyEntry(ontologyIndividual, RDFVocabulary.RDFS.COMMENT.ToRDFOntologyAnnotationProperty(), annotationValue));
                        else
                            RDFSemanticsEvents.RaiseSemanticsInfo(string.Format("Cannot annotate ontology individual with rdfs:comment value '{0}' because it is not an ontology literal", annotationValue));
                        break;

                    //rdfs:label
                    case RDFSemanticsEnums.RDFOntologyStandardAnnotation.Label:
                        if (annotationValue.IsLiteral())
                            this.Annotations.Label.AddEntry(new RDFOntologyTaxonomyEntry(ontologyIndividual, RDFVocabulary.RDFS.LABEL.ToRDFOntologyAnnotationProperty(), annotationValue));
                        else
                            RDFSemanticsEvents.RaiseSemanticsInfo(string.Format("Cannot annotate ontology individual with rdfs:label value '{0}' because it is not an ontology literal", annotationValue));
                        break;

                    //rdfs:seeAlso
                    case RDFSemanticsEnums.RDFOntologyStandardAnnotation.SeeAlso:
                        this.Annotations.SeeAlso.AddEntry(new RDFOntologyTaxonomyEntry(ontologyIndividual, RDFVocabulary.RDFS.SEE_ALSO.ToRDFOntologyAnnotationProperty(), annotationValue));
                        break;

                    //rdfs:isDefinedBy
                    case RDFSemanticsEnums.RDFOntologyStandardAnnotation.IsDefinedBy:
                        this.Annotations.IsDefinedBy.AddEntry(new RDFOntologyTaxonomyEntry(ontologyIndividual, RDFVocabulary.RDFS.IS_DEFINED_BY.ToRDFOntologyAnnotationProperty(), annotationValue));
                        break;

                    #region Unsupported
                    //owl:versionIRI
                    case RDFSemanticsEnums.RDFOntologyStandardAnnotation.VersionIRI:
                        RDFSemanticsEvents.RaiseSemanticsInfo("Cannot annotate ontology individual with owl:versionIRI because it is reserved for ontologies");
                        break;

                    //owl:priorVersion
                    case RDFSemanticsEnums.RDFOntologyStandardAnnotation.PriorVersion:
                        RDFSemanticsEvents.RaiseSemanticsInfo("Cannot annotate ontology individual with owl:priorVersion because it is reserved for ontologies");
                        break;

                    //owl:imports
                    case RDFSemanticsEnums.RDFOntologyStandardAnnotation.Imports:
                        RDFSemanticsEvents.RaiseSemanticsInfo("Cannot annotate ontology individual with owl:imports because it is reserved for ontologies");
                        break;

                    //owl:backwardCompatibleWith
                    case RDFSemanticsEnums.RDFOntologyStandardAnnotation.BackwardCompatibleWith:
                        RDFSemanticsEvents.RaiseSemanticsInfo("Cannot annotate ontology individual with owl:backwardCompatibleWith because it is reserved for ontologies");
                        break;

                    //owl:incompatibleWith
                    case RDFSemanticsEnums.RDFOntologyStandardAnnotation.IncompatibleWith:
                        RDFSemanticsEvents.RaiseSemanticsInfo("Cannot annotate ontology individual with owl:incompatibleWith because it is reserved for ontologies");
                        break;
                    #endregion
                }
            }
            return this;
        }

        /// <summary>
        /// Adds the given custom annotation to the given ontology individual
        /// </summary>
        public RDFOntologyData AddCustomAnnotation(RDFOntologyAnnotationProperty ontologyAnnotationProperty, RDFOntologyIndividual ontologyIndividual, RDFOntologyResource annotationValue)
        {
            if (ontologyAnnotationProperty != null && ontologyIndividual != null && annotationValue != null)
            {
                //standard annotation
                if (RDFSemanticsUtilities.StandardAnnotationProperties.Contains(ontologyAnnotationProperty.PatternMemberID))
                {
                    //owl:versionInfo
                    if (ontologyAnnotationProperty.Equals(RDFVocabulary.OWL.VERSION_INFO))
                        this.AddStandardAnnotation(RDFSemanticsEnums.RDFOntologyStandardAnnotation.VersionInfo, ontologyIndividual, annotationValue);

                    //rdfs:comment
                    else if (ontologyAnnotationProperty.Equals(RDFVocabulary.RDFS.COMMENT))
                        this.AddStandardAnnotation(RDFSemanticsEnums.RDFOntologyStandardAnnotation.Comment, ontologyIndividual, annotationValue);

                    //rdfs:label
                    else if (ontologyAnnotationProperty.Equals(RDFVocabulary.RDFS.LABEL))
                        this.AddStandardAnnotation(RDFSemanticsEnums.RDFOntologyStandardAnnotation.Label, ontologyIndividual, annotationValue);

                    //rdfs:seeAlso
                    else if (ontologyAnnotationProperty.Equals(RDFVocabulary.RDFS.SEE_ALSO))
                        this.AddStandardAnnotation(RDFSemanticsEnums.RDFOntologyStandardAnnotation.SeeAlso, ontologyIndividual, annotationValue);

                    //rdfs:isDefinedBy
                    else if (ontologyAnnotationProperty.Equals(RDFVocabulary.RDFS.IS_DEFINED_BY))
                        this.AddStandardAnnotation(RDFSemanticsEnums.RDFOntologyStandardAnnotation.IsDefinedBy, ontologyIndividual, annotationValue);

                    #region Unsupported
                    //owl:versionIRI
                    else if (ontologyAnnotationProperty.Equals(RDFVocabulary.OWL.VERSION_IRI))
                        this.AddStandardAnnotation(RDFSemanticsEnums.RDFOntologyStandardAnnotation.VersionIRI, ontologyIndividual, annotationValue);

                    //owl:imports
                    else if (ontologyAnnotationProperty.Equals(RDFVocabulary.OWL.IMPORTS))
                        this.AddStandardAnnotation(RDFSemanticsEnums.RDFOntologyStandardAnnotation.Imports, ontologyIndividual, annotationValue);

                    //owl:backwardCompatibleWith
                    else if (ontologyAnnotationProperty.Equals(RDFVocabulary.OWL.BACKWARD_COMPATIBLE_WITH))
                        this.AddStandardAnnotation(RDFSemanticsEnums.RDFOntologyStandardAnnotation.BackwardCompatibleWith, ontologyIndividual, annotationValue);

                    //owl:incompatibleWith
                    else if (ontologyAnnotationProperty.Equals(RDFVocabulary.OWL.INCOMPATIBLE_WITH))
                        this.AddStandardAnnotation(RDFSemanticsEnums.RDFOntologyStandardAnnotation.IncompatibleWith, ontologyIndividual, annotationValue);

                    //owl:priorVersion
                    else if (ontologyAnnotationProperty.Equals(RDFVocabulary.OWL.PRIOR_VERSION))
                        this.AddStandardAnnotation(RDFSemanticsEnums.RDFOntologyStandardAnnotation.PriorVersion, ontologyIndividual, annotationValue);
                    #endregion
                }

                //custom annotation
                else
                    this.Annotations.CustomAnnotations.AddEntry(new RDFOntologyTaxonomyEntry(ontologyIndividual, ontologyAnnotationProperty, annotationValue));
            }
            return this;
        }

        /// <summary>
        /// Adds the "ontologyIndividual -> rdf:type -> ontologyClass" relation to the data (and links the given axiom annotation if provided)
        /// </summary>
        public RDFOntologyData AddClassTypeRelation(RDFOntologyIndividual ontologyIndividual, RDFOntologyClass ontologyClass, RDFOntologyAxiomAnnotation axiomAnnotation=null)
        {
            if (ontologyIndividual != null && ontologyClass != null)
            {
                //Enforce preliminary check on usage of BASE classes
                if (!RDFOntologyChecker.CheckReservedClass(ontologyClass))
                {
                    //Enforce taxonomy checks before adding the ClassType relation
                    if (RDFOntologyChecker.CheckClassTypeCompatibility(ontologyClass))
                    {
                        RDFOntologyTaxonomyEntry taxonomyEntry = new RDFOntologyTaxonomyEntry(ontologyIndividual, RDFVocabulary.RDF.TYPE.ToRDFOntologyObjectProperty(), ontologyClass);
                        this.Relations.ClassType.AddEntry(taxonomyEntry);
                        this.AddIndividual(ontologyIndividual);

                        //Link owl:Axiom annotation
                        this.AddAxiomAnnotation(taxonomyEntry, axiomAnnotation, nameof(RDFOntologyDataMetadata.ClassType));
                    }
                    else
                        RDFSemanticsEvents.RaiseSemanticsWarning(string.Format("ClassType relation between individual '{0}' and class '{1}' cannot be added to the data because only plain classes can be explicitly assigned as class types of individuals.", ontologyIndividual, ontologyClass));
                }
                else
                    RDFSemanticsEvents.RaiseSemanticsWarning(string.Format("ClassType relation between individual '{0}' and class '{1}' cannot be added to the data because because usage of BASE reserved classes compromises the taxonomy consistency.", ontologyIndividual, ontologyClass));
            }
            return this;
        }

        /// <summary>
        /// Adds the "aIndividual -> owl:sameAs -> bIndividual" relation to the data (and links the given axiom annotation if provided)
        /// </summary>
        public RDFOntologyData AddSameAsRelation(RDFOntologyIndividual aIndividual, RDFOntologyIndividual bIndividual, RDFOntologyAxiomAnnotation axiomAnnotation=null)
        {
            if (aIndividual != null && bIndividual != null && !aIndividual.Equals(bIndividual))
            {
                //Enforce taxonomy checks before adding the SameAs relation
                if (RDFOntologyChecker.CheckSameAsCompatibility(this, aIndividual, bIndividual))
                {
                    RDFOntologyTaxonomyEntry sameAsLeft = new RDFOntologyTaxonomyEntry(aIndividual, RDFVocabulary.OWL.SAME_AS.ToRDFOntologyObjectProperty(), bIndividual);
                    this.Relations.SameAs.AddEntry(sameAsLeft);
                    RDFOntologyTaxonomyEntry sameAsRight = new RDFOntologyTaxonomyEntry(bIndividual, RDFVocabulary.OWL.SAME_AS.ToRDFOntologyObjectProperty(), aIndividual).SetInference(RDFSemanticsEnums.RDFOntologyInferenceType.API);
                    this.Relations.SameAs.AddEntry(sameAsRight);

                    //Link owl:Axiom annotation
                    this.AddAxiomAnnotation(sameAsLeft, axiomAnnotation, nameof(RDFOntologyDataMetadata.SameAs));
                }
                else
                    RDFSemanticsEvents.RaiseSemanticsWarning(string.Format("SameAs relation between individual '{0}' and individual '{1}' cannot be added to the data because it violates the taxonomy consistency.", aIndividual, bIndividual));
            }
            return this;
        }

        /// <summary>
        /// Adds the "aIndividual -> owl:differentFrom -> bIndividual" relation to the data (and links the given axiom annotation if provided)
        /// </summary>
        public RDFOntologyData AddDifferentFromRelation(RDFOntologyIndividual aIndividual, RDFOntologyIndividual bIndividual, RDFOntologyAxiomAnnotation axiomAnnotation=null)
        {
            if (aIndividual != null && bIndividual != null && !aIndividual.Equals(bIndividual))
            {
                //Enforce taxonomy checks before adding the DifferentFrom relation
                if (RDFOntologyChecker.CheckDifferentFromCompatibility(this, aIndividual, bIndividual))
                {
                    RDFOntologyTaxonomyEntry differentFromLeft = new RDFOntologyTaxonomyEntry(aIndividual, RDFVocabulary.OWL.DIFFERENT_FROM.ToRDFOntologyObjectProperty(), bIndividual);
                    this.Relations.DifferentFrom.AddEntry(differentFromLeft);
                    RDFOntologyTaxonomyEntry differentFromRight = new RDFOntologyTaxonomyEntry(bIndividual, RDFVocabulary.OWL.DIFFERENT_FROM.ToRDFOntologyObjectProperty(), aIndividual).SetInference(RDFSemanticsEnums.RDFOntologyInferenceType.API);
                    this.Relations.DifferentFrom.AddEntry(differentFromRight);

                    //Link owl:Axiom annotation
                    this.AddAxiomAnnotation(differentFromLeft, axiomAnnotation, nameof(RDFOntologyDataMetadata.DifferentFrom));
                }
                else
                    RDFSemanticsEvents.RaiseSemanticsWarning(string.Format("DifferentFrom relation between individual '{0}' and individual '{1}' cannot be added to the data because it violates the taxonomy consistency.", aIndividual, bIndividual));
            }
            return this;
        }

        /// <summary>
        /// Foreach of the given list of individuals, adds the "aIndividual -> owl:differentFrom -> bIndividual" relation to the data [OWL2]
        /// </summary>
        public RDFOntologyData AddAllDifferentRelation(List<RDFOntologyIndividual> ontologyIndividuals)
        {
            ontologyIndividuals?.ForEach(outerIndividual =>
                ontologyIndividuals?.ForEach(innerIndividual => 
                    this.AddDifferentFromRelation(innerIndividual, outerIndividual)));
            return this;
        }

        /// <summary>
        /// Adds the "aIndividual -> objectProperty -> bIndividual" relation to the data (and links the given axiom annotation if provided)
        /// </summary>
        public RDFOntologyData AddObjectAssertion(RDFOntologyIndividual aIndividual, RDFOntologyObjectProperty objectProperty, RDFOntologyIndividual bIndividual, RDFOntologyAxiomAnnotation axiomAnnotation=null)
        {
            if (aIndividual != null && objectProperty != null && bIndividual != null)
            {
                //Enforce preliminary check on usage of BASE properties
                if (!RDFOntologyChecker.CheckReservedProperty(objectProperty))
                {
                    //Enforce taxonomy checks before adding the assertion
                    //Creation of transitive cycles is not allowed [OWL-DL]
                    if (RDFOntologyChecker.CheckTransitiveAssertionCompatibility(this, aIndividual, objectProperty, bIndividual))
                    {
                        //Collision with negative assertions must be avoided [OWL2]
                        if (RDFOntologyChecker.CheckAssertionCompatibility(this, aIndividual, objectProperty, bIndividual))
                        {
                            if (objectProperty.Equals(RDFVocabulary.SKOS.MEMBER))
                                this.AddMemberRelation(aIndividual, bIndividual, axiomAnnotation);
                            else if (objectProperty.Equals(RDFVocabulary.SKOS.MEMBER_LIST))
                                this.AddMemberListRelation(aIndividual, bIndividual, axiomAnnotation);
                            else
                            {
                                RDFOntologyTaxonomyEntry taxonomyEntry = new RDFOntologyTaxonomyEntry(aIndividual, objectProperty, bIndividual);
                                this.Relations.Assertions.AddEntry(taxonomyEntry);

                                //Link owl:Axiom annotation
                                this.AddAxiomAnnotation(taxonomyEntry, axiomAnnotation, nameof(RDFOntologyDataMetadata.Assertions));
                            }
                        }
                        else
                            RDFSemanticsEvents.RaiseSemanticsWarning(string.Format("Assertion relation between individual '{0}' and individual '{1}' with property '{2}' cannot be added to the data because it would violate the taxonomy consistency (negative assertion detected).", aIndividual, bIndividual, objectProperty));
                    }
                    else
                        RDFSemanticsEvents.RaiseSemanticsWarning(string.Format("Assertion relation between individual '{0}' and individual '{1}' with transitive property '{2}' cannot be added to the data because it would violate the taxonomy consistency (transitive cycle detected).", aIndividual, bIndividual, objectProperty));
                }
                else
                    RDFSemanticsEvents.RaiseSemanticsWarning(string.Format("Assertion relation between individual '{0}' and individual '{1}' cannot be added to the data because usage of BASE reserved properties compromises the taxonomy consistency.", aIndividual, bIndividual));
            }
            return this;
        }

        /// <summary>
        /// Adds the "ontologyIndividual -> datatypeProperty -> ontologyLiteral" relation to the data (and links the given axiom annotation if provided)
        /// </summary>
        public RDFOntologyData AddDataAssertion(RDFOntologyIndividual ontologyIndividual, RDFOntologyDatatypeProperty datatypeProperty, RDFOntologyLiteral ontologyLiteral, RDFOntologyAxiomAnnotation axiomAnnotation = null)
        {
            if (ontologyIndividual != null && datatypeProperty != null && ontologyLiteral != null)
            {
                //Enforce preliminary check on usage of BASE properties
                if (!RDFOntologyChecker.CheckReservedProperty(datatypeProperty))
                {
                    //Collision with negative assertions must be avoided [OWL2]
                    if (RDFOntologyChecker.CheckAssertionCompatibility(this, ontologyIndividual, datatypeProperty, ontologyLiteral))
                    {
                        //Cannot accept assertion in case of SKOS collection predicates
                        if (!datatypeProperty.Equals(RDFVocabulary.SKOS.MEMBER)
                                && !datatypeProperty.Equals(RDFVocabulary.SKOS.MEMBER_LIST))
                        {
                            RDFOntologyTaxonomyEntry taxonomyEntry = new RDFOntologyTaxonomyEntry(ontologyIndividual, datatypeProperty, ontologyLiteral);
                            this.Relations.Assertions.AddEntry(taxonomyEntry);
                            this.AddLiteral(ontologyLiteral);

                            //Link owl:Axiom annotation
                            this.AddAxiomAnnotation(taxonomyEntry, axiomAnnotation, nameof(RDFOntologyDataMetadata.Assertions));
                        }
                        else
                            RDFSemanticsEvents.RaiseSemanticsWarning(string.Format("Assertion relation between individual '{0}' and literal '{1}' with property '{2}' cannot be added to the data because it would violate the taxonomy consistency (SKOS collection detected).", ontologyIndividual, ontologyLiteral, datatypeProperty));
                    }
                    else
                        RDFSemanticsEvents.RaiseSemanticsWarning(string.Format("Assertion relation between individual '{0}' and literal '{1}' with property '{2}' cannot be added to the data because it would violate the taxonomy consistency (negative assertion detected).", ontologyIndividual, ontologyLiteral, datatypeProperty));
                }
                else
                    RDFSemanticsEvents.RaiseSemanticsWarning(string.Format("Assertion relation between individual '{0}' and literal '{1}' cannot be added to the data because usage of BASE reserved properties compromises the taxonomy consistency.", ontologyIndividual, ontologyLiteral));
            }
            return this;
        }

        /// <summary>
        /// Adds the "aIndividual -> objectProperty -> bIndividual" negative relation to the data [OWL2]
        /// </summary>
        public RDFOntologyData AddNegativeObjectAssertion(RDFOntologyIndividual aIndividual, RDFOntologyObjectProperty objectProperty, RDFOntologyIndividual bIndividual)
        {
            if (aIndividual != null && objectProperty != null && bIndividual != null)
            {
                //Enforce preliminary check on usage of BASE properties
                if (!RDFOntologyChecker.CheckReservedProperty(objectProperty))
                {
                    //Collision with assertions must be avoided [OWL2]
                    if (RDFOntologyChecker.CheckNegativeAssertionCompatibility(this, aIndividual, objectProperty, bIndividual))
                    {
                        //Cannot accept negative assertion in case of SKOS collection predicates
                        if (!objectProperty.Equals(RDFVocabulary.SKOS.MEMBER)
                                && !objectProperty.Equals(RDFVocabulary.SKOS.MEMBER_LIST))
                            this.Relations.NegativeAssertions.AddEntry(new RDFOntologyTaxonomyEntry(aIndividual, objectProperty, bIndividual));
                        else
                            RDFSemanticsEvents.RaiseSemanticsWarning(string.Format("NegativeAssertion relation between individual '{0}' and individual '{1}' with property '{2}' cannot be added to the data because it would violate the taxonomy consistency (SKOS collection detected).", aIndividual, bIndividual, objectProperty));
                    }
                    else
                        RDFSemanticsEvents.RaiseSemanticsWarning(string.Format("NegativeAssertion relation between individual '{0}' and individual '{1}' with property '{2}' cannot be added to the data because it would violate the taxonomy consistency (assertion detected).", aIndividual, bIndividual, objectProperty));
                }
                else
                    RDFSemanticsEvents.RaiseSemanticsWarning(string.Format("NegativeAssertion relation between individual '{0}' and individual '{1}' cannot be added to the data because usage of BASE reserved properties compromises the taxonomy consistency.", aIndividual, bIndividual));
            }
            return this;
        }

        /// <summary>
        /// Adds the "ontologyIndividual -> datatypeProperty -> ontologyLiteral" negative relation to the data [OWL2]
        /// </summary>
        public RDFOntologyData AddNegativeDataAssertion(RDFOntologyIndividual ontologyIndividual, RDFOntologyDatatypeProperty datatypeProperty, RDFOntologyLiteral ontologyLiteral)
        {
            if (ontologyIndividual != null && datatypeProperty != null && ontologyLiteral != null)
            {
                //Enforce preliminary check on usage of BASE properties
                if (!RDFOntologyChecker.CheckReservedProperty(datatypeProperty))
                {
                    //Collision with assertions must be avoided [OWL2]
                    if (RDFOntologyChecker.CheckNegativeAssertionCompatibility(this, ontologyIndividual, datatypeProperty, ontologyLiteral))
                    {
                        //Cannot accept negative assertion in case of SKOS collection predicates
                        if (!datatypeProperty.Equals(RDFVocabulary.SKOS.MEMBER)
                                && !datatypeProperty.Equals(RDFVocabulary.SKOS.MEMBER_LIST))
                        {
                            this.Relations.NegativeAssertions.AddEntry(new RDFOntologyTaxonomyEntry(ontologyIndividual, datatypeProperty, ontologyLiteral));
                            this.AddLiteral(ontologyLiteral);
                        }
                        else
                            RDFSemanticsEvents.RaiseSemanticsWarning(string.Format("NegativeAssertion relation between individual '{0}' and literal '{1}' with property '{2}' cannot be added to the data because it would violate the taxonomy consistency (SKOS collection detected).", ontologyIndividual, ontologyLiteral, datatypeProperty));
                    }
                    else
                        RDFSemanticsEvents.RaiseSemanticsWarning(string.Format("NegativeAssertion relation between individual '{0}' and literal '{1}' with property '{2}' cannot be added to the data because it would violate the taxonomy consistency (assertion detected).", ontologyIndividual, ontologyLiteral, datatypeProperty));
                }
                else
                    RDFSemanticsEvents.RaiseSemanticsWarning(string.Format("NegativeAssertion relation between individual '{0}' and literal '{1}' cannot be added to the data because usage of BASE reserved properties compromises the taxonomy consistency.", ontologyIndividual, ontologyLiteral));
            }
            return this;
        }

        /// <summary>
        /// Adds the given owl:Axiom annotation to the given taxonomy entry
        /// </summary>
        internal RDFOntologyData AddAxiomAnnotation(RDFOntologyTaxonomyEntry taxonomyEntry, RDFOntologyAxiomAnnotation axiomAnnotation, string targetTaxonomyName)
        {
            #region DetectTargetTaxonomy
            RDFOntologyTaxonomy DetectTargetTaxonomy()
            {
                RDFOntologyTaxonomy targetTaxonomy = null;
                switch (targetTaxonomyName)
                {
                    case nameof(RDFOntologyDataMetadata.ClassType):
                        targetTaxonomy = this.Relations.ClassType;
                        break;
                    case nameof(RDFOntologyDataMetadata.SameAs):
                        targetTaxonomy = this.Relations.SameAs;
                        break;
                    case nameof(RDFOntologyDataMetadata.DifferentFrom):
                        targetTaxonomy = this.Relations.DifferentFrom;
                        break;
                    case nameof(RDFOntologyDataMetadata.Assertions):
                        targetTaxonomy = this.Relations.Assertions;
                        break;
                    case nameof(RDFOntologyDataMetadata.Member):
                        targetTaxonomy = this.Relations.Member;
                        break;
                    case nameof(RDFOntologyDataMetadata.MemberList):
                        targetTaxonomy = this.Relations.MemberList;
                        break;
                }
                return targetTaxonomy;
            }
            #endregion

            RDFOntologyTaxonomy taxonomy = DetectTargetTaxonomy();
            if (axiomAnnotation != null && taxonomy != null && taxonomy.ContainsEntry(taxonomyEntry))
                this.Annotations.AxiomAnnotations.AddEntry(new RDFOntologyTaxonomyEntry(this.GetTaxonomyEntryRepresentative(taxonomyEntry), axiomAnnotation.Property, axiomAnnotation.Value));
            return this;
        }
        #endregion

        #region Remove
        /// <summary>
        /// Removes the given individual from the data
        /// </summary>
        public RDFOntologyData RemoveIndividual(RDFOntologyIndividual ontologyIndividual)
        {
            if (ontologyIndividual != null)
            {
                //Declaration
                if (this.Individuals.ContainsKey(ontologyIndividual.PatternMemberID))
                    this.Individuals.Remove(ontologyIndividual.PatternMemberID);

                //ClassType
                foreach (RDFOntologyTaxonomyEntry taxonomyEntry in this.Relations.ClassType.SelectEntriesBySubject(ontologyIndividual))
                {
                    this.Relations.ClassType.RemoveEntry(taxonomyEntry);

                    //Unlink owl:Axiom annotation
                    this.RemoveAxiomAnnotation(taxonomyEntry);
                }   

                //SameAs
                foreach (RDFOntologyTaxonomyEntry taxonomyEntry in this.Relations.SameAs.SelectEntriesBySubject(ontologyIndividual))
                {
                    this.Relations.SameAs.RemoveEntry(taxonomyEntry);

                    //Unlink owl:Axiom annotation
                    this.RemoveAxiomAnnotation(taxonomyEntry);
                }
                foreach (RDFOntologyTaxonomyEntry taxonomyEntry in this.Relations.SameAs.SelectEntriesByObject(ontologyIndividual))
                { 
                    this.Relations.SameAs.RemoveEntry(taxonomyEntry);

                    //Unlink owl:Axiom annotation
                    this.RemoveAxiomAnnotation(taxonomyEntry);
                }

                //DifferentFrom
                foreach (RDFOntologyTaxonomyEntry taxonomyEntry in this.Relations.DifferentFrom.SelectEntriesBySubject(ontologyIndividual))
                {
                    this.Relations.DifferentFrom.RemoveEntry(taxonomyEntry);

                    //Unlink owl:Axiom annotation
                    this.RemoveAxiomAnnotation(taxonomyEntry);
                }                    
                foreach (RDFOntologyTaxonomyEntry taxonomyEntry in this.Relations.DifferentFrom.SelectEntriesByObject(ontologyIndividual))
                {
                    this.Relations.DifferentFrom.RemoveEntry(taxonomyEntry);

                    //Unlink owl:Axiom annotation
                    this.RemoveAxiomAnnotation(taxonomyEntry);
                }   

                //Assertions
                foreach (RDFOntologyTaxonomyEntry taxonomyEntry in this.Relations.Assertions.SelectEntriesBySubject(ontologyIndividual))
                { 
                    this.Relations.Assertions.RemoveEntry(taxonomyEntry);

                    //Unlink owl:Axiom annotation
                    this.RemoveAxiomAnnotation(taxonomyEntry);
                }
                foreach (RDFOntologyTaxonomyEntry taxonomyEntry in this.Relations.Assertions.SelectEntriesByObject(ontologyIndividual))
                { 
                    this.Relations.Assertions.RemoveEntry(taxonomyEntry);

                    //Unlink owl:Axiom annotation
                    this.RemoveAxiomAnnotation(taxonomyEntry);
                }

                //Member [SKOS]
                foreach (RDFOntologyTaxonomyEntry taxonomyEntry in this.Relations.Member.SelectEntriesBySubject(ontologyIndividual))
                {
                    this.Relations.Member.RemoveEntry(taxonomyEntry);

                    //Unlink owl:Axiom annotation
                    this.RemoveAxiomAnnotation(taxonomyEntry);
                }                    
                foreach (RDFOntologyTaxonomyEntry taxonomyEntry in this.Relations.Member.SelectEntriesByObject(ontologyIndividual))
                {
                    this.Relations.Member.RemoveEntry(taxonomyEntry);

                    //Unlink owl:Axiom annotation
                    this.RemoveAxiomAnnotation(taxonomyEntry);
                }                    

                //MemberList [SKOS]
                foreach (RDFOntologyTaxonomyEntry taxonomyEntry in this.Relations.MemberList.SelectEntriesBySubject(ontologyIndividual))
                {
                    this.Relations.MemberList.RemoveEntry(taxonomyEntry);

                    //Unlink owl:Axiom annotation
                    this.RemoveAxiomAnnotation(taxonomyEntry);
                }   
                foreach (RDFOntologyTaxonomyEntry taxonomyEntry in this.Relations.MemberList.SelectEntriesByObject(ontologyIndividual))
                {
                    this.Relations.MemberList.RemoveEntry(taxonomyEntry);

                    //Unlink owl:Axiom annotation
                    this.RemoveAxiomAnnotation(taxonomyEntry);
                }   

                //NegativeAssertions [OWL2]
                foreach (RDFOntologyTaxonomyEntry taxonomyEntry in this.Relations.NegativeAssertions.SelectEntriesBySubject(ontologyIndividual))
                    this.Relations.NegativeAssertions.RemoveEntry(taxonomyEntry);
                foreach (RDFOntologyTaxonomyEntry taxonomyEntry in this.Relations.NegativeAssertions.SelectEntriesByObject(ontologyIndividual))
                    this.Relations.NegativeAssertions.RemoveEntry(taxonomyEntry);

                //Annotations
                foreach (RDFOntologyTaxonomyEntry taxonomyEntry in this.Annotations.VersionInfo.SelectEntriesBySubject(ontologyIndividual))
                    this.Annotations.VersionInfo.RemoveEntry(taxonomyEntry);
                foreach (RDFOntologyTaxonomyEntry taxonomyEntry in this.Annotations.Comment.SelectEntriesBySubject(ontologyIndividual))
                    this.Annotations.Comment.RemoveEntry(taxonomyEntry);
                foreach (RDFOntologyTaxonomyEntry taxonomyEntry in this.Annotations.Label.SelectEntriesBySubject(ontologyIndividual))
                    this.Annotations.Label.RemoveEntry(taxonomyEntry);
                foreach (RDFOntologyTaxonomyEntry taxonomyEntry in this.Annotations.SeeAlso.SelectEntriesBySubject(ontologyIndividual))
                    this.Annotations.SeeAlso.RemoveEntry(taxonomyEntry);
                foreach (RDFOntologyTaxonomyEntry taxonomyEntry in this.Annotations.IsDefinedBy.SelectEntriesBySubject(ontologyIndividual))
                    this.Annotations.IsDefinedBy.RemoveEntry(taxonomyEntry);
                foreach (RDFOntologyTaxonomyEntry taxonomyEntry in this.Annotations.CustomAnnotations.SelectEntriesBySubject(ontologyIndividual))
                    this.Annotations.CustomAnnotations.RemoveEntry(taxonomyEntry);
                foreach (RDFOntologyTaxonomyEntry taxonomyEntry in this.Annotations.CustomAnnotations.SelectEntriesByObject(ontologyIndividual))
                    this.Annotations.CustomAnnotations.RemoveEntry(taxonomyEntry);
            }
            return this;
        }

        /// <summary>
        /// Replaces all the occurrences of the given individual with the given new individual in the data
        /// </summary>
        public RDFOntologyData ReplaceIndividual(RDFOntologyIndividual ontologyIndividual, RDFOntologyIndividual newOntologyIndividual)
        {
            if (ontologyIndividual != null && newOntologyIndividual != null && !ontologyIndividual.Equals(newOntologyIndividual))
            {
                //Declarations
                this.AddIndividual(newOntologyIndividual);

                //ClassType
                foreach (RDFOntologyTaxonomyEntry taxonomyEntry in this.Relations.ClassType.SelectEntriesBySubject(ontologyIndividual))
                    this.AddClassTypeRelation(newOntologyIndividual, (RDFOntologyClass)taxonomyEntry.TaxonomyObject);

                //SameAs
                foreach (RDFOntologyTaxonomyEntry taxonomyEntry in this.Relations.SameAs.SelectEntriesBySubject(ontologyIndividual))
                    this.AddSameAsRelation(newOntologyIndividual, (RDFOntologyIndividual)taxonomyEntry.TaxonomyObject);
                foreach (RDFOntologyTaxonomyEntry taxonomyEntry in this.Relations.SameAs.SelectEntriesByObject(ontologyIndividual))
                    this.AddSameAsRelation((RDFOntologyIndividual)taxonomyEntry.TaxonomySubject, newOntologyIndividual);

                //DifferentFrom
                foreach (RDFOntologyTaxonomyEntry taxonomyEntry in this.Relations.DifferentFrom.SelectEntriesBySubject(ontologyIndividual))
                    this.AddDifferentFromRelation(newOntologyIndividual, (RDFOntologyIndividual)taxonomyEntry.TaxonomyObject);
                foreach (RDFOntologyTaxonomyEntry taxonomyEntry in this.Relations.DifferentFrom.SelectEntriesByObject(ontologyIndividual))
                    this.AddDifferentFromRelation((RDFOntologyIndividual)taxonomyEntry.TaxonomySubject, newOntologyIndividual);

                //Assertions
                foreach (RDFOntologyTaxonomyEntry taxonomyEntry in this.Relations.Assertions.SelectEntriesBySubject(ontologyIndividual))
                {
                    if (taxonomyEntry.TaxonomyObject.Value is RDFLiteral)
                        this.AddDataAssertion(newOntologyIndividual, (RDFOntologyDatatypeProperty)taxonomyEntry.TaxonomyPredicate, (RDFOntologyLiteral)taxonomyEntry.TaxonomyObject);
                    else
                        this.AddObjectAssertion(newOntologyIndividual, (RDFOntologyObjectProperty)taxonomyEntry.TaxonomyPredicate, (RDFOntologyIndividual)taxonomyEntry.TaxonomyObject);
                }
                foreach (RDFOntologyTaxonomyEntry taxonomyEntry in this.Relations.Assertions.SelectEntriesByObject(ontologyIndividual))
                    this.AddObjectAssertion((RDFOntologyIndividual)taxonomyEntry.TaxonomySubject, (RDFOntologyObjectProperty)taxonomyEntry.TaxonomyPredicate, newOntologyIndividual);

                //Member [SKOS]
                foreach (RDFOntologyTaxonomyEntry taxonomyEntry in this.Relations.Member.SelectEntriesBySubject(ontologyIndividual))
                    this.AddMemberRelation(newOntologyIndividual, (RDFOntologyIndividual)taxonomyEntry.TaxonomyObject);
                foreach (RDFOntologyTaxonomyEntry taxonomyEntry in this.Relations.Member.SelectEntriesByObject(ontologyIndividual))
                    this.AddMemberRelation((RDFOntologyIndividual)taxonomyEntry.TaxonomySubject, newOntologyIndividual);

                //MemberList [SKOS]
                foreach (RDFOntologyTaxonomyEntry taxonomyEntry in this.Relations.MemberList.SelectEntriesBySubject(ontologyIndividual))
                    this.AddMemberListRelation(newOntologyIndividual, (RDFOntologyIndividual)taxonomyEntry.TaxonomyObject);
                foreach (RDFOntologyTaxonomyEntry taxonomyEntry in this.Relations.MemberList.SelectEntriesByObject(ontologyIndividual))
                    this.AddMemberListRelation((RDFOntologyIndividual)taxonomyEntry.TaxonomySubject, newOntologyIndividual);

                //NegativeAssertions [OWL2]
                foreach (RDFOntologyTaxonomyEntry taxonomyEntry in this.Relations.NegativeAssertions.SelectEntriesBySubject(ontologyIndividual))
                {
                    if (taxonomyEntry.TaxonomyObject.Value is RDFLiteral)
                        this.AddNegativeDataAssertion(newOntologyIndividual, (RDFOntologyDatatypeProperty)taxonomyEntry.TaxonomyPredicate, (RDFOntologyLiteral)taxonomyEntry.TaxonomyObject);
                    else
                        this.AddNegativeObjectAssertion(newOntologyIndividual, (RDFOntologyObjectProperty)taxonomyEntry.TaxonomyPredicate, (RDFOntologyIndividual)taxonomyEntry.TaxonomyObject);
                }
                foreach (RDFOntologyTaxonomyEntry taxonomyEntry in this.Relations.NegativeAssertions.SelectEntriesByObject(ontologyIndividual))
                    this.AddNegativeObjectAssertion((RDFOntologyIndividual)taxonomyEntry.TaxonomySubject, (RDFOntologyObjectProperty)taxonomyEntry.TaxonomyPredicate, newOntologyIndividual);

                //Annotations
                foreach (RDFOntologyTaxonomyEntry taxonomyEntry in this.Annotations.VersionInfo.SelectEntriesBySubject(ontologyIndividual))
                    this.AddStandardAnnotation(RDFSemanticsEnums.RDFOntologyStandardAnnotation.VersionInfo, newOntologyIndividual, taxonomyEntry.TaxonomyObject);
                foreach (RDFOntologyTaxonomyEntry taxonomyEntry in this.Annotations.Comment.SelectEntriesBySubject(ontologyIndividual))
                    this.AddStandardAnnotation(RDFSemanticsEnums.RDFOntologyStandardAnnotation.Comment, newOntologyIndividual, taxonomyEntry.TaxonomyObject);
                foreach (RDFOntologyTaxonomyEntry taxonomyEntry in this.Annotations.Label.SelectEntriesBySubject(ontologyIndividual))
                    this.AddStandardAnnotation(RDFSemanticsEnums.RDFOntologyStandardAnnotation.Label, newOntologyIndividual, taxonomyEntry.TaxonomyObject);
                foreach (RDFOntologyTaxonomyEntry taxonomyEntry in this.Annotations.SeeAlso.SelectEntriesBySubject(ontologyIndividual))
                    this.AddStandardAnnotation(RDFSemanticsEnums.RDFOntologyStandardAnnotation.SeeAlso, newOntologyIndividual, taxonomyEntry.TaxonomyObject);
                foreach (RDFOntologyTaxonomyEntry taxonomyEntry in this.Annotations.IsDefinedBy.SelectEntriesBySubject(ontologyIndividual))
                    this.AddStandardAnnotation(RDFSemanticsEnums.RDFOntologyStandardAnnotation.IsDefinedBy, newOntologyIndividual, taxonomyEntry.TaxonomyObject);
                foreach (RDFOntologyTaxonomyEntry taxonomyEntry in this.Annotations.CustomAnnotations.SelectEntriesBySubject(ontologyIndividual))
                    this.AddCustomAnnotation((RDFOntologyAnnotationProperty)taxonomyEntry.TaxonomyPredicate, newOntologyIndividual, taxonomyEntry.TaxonomyObject);
                foreach (RDFOntologyTaxonomyEntry taxonomyEntry in this.Annotations.CustomAnnotations.SelectEntriesByObject(ontologyIndividual))
                    this.AddCustomAnnotation((RDFOntologyAnnotationProperty)taxonomyEntry.TaxonomyPredicate, (RDFOntologyIndividual)taxonomyEntry.TaxonomySubject, newOntologyIndividual);

                //Drop replaced individual
                this.RemoveIndividual(ontologyIndividual);                
            }
            return this;
        }

        /// <summary>
        /// Removes the given literal from the data
        /// </summary>
        public RDFOntologyData RemoveLiteral(RDFOntologyLiteral ontologyLiteral)
        {
            if (ontologyLiteral != null)
            {
                if (this.Literals.ContainsKey(ontologyLiteral.PatternMemberID))
                    this.Literals.Remove(ontologyLiteral.PatternMemberID);
            }
            return this;
        }

        /// <summary>
        /// Replaces all the occurrences of the given literal with the given new literal in the data
        /// </summary>
        public RDFOntologyData ReplaceLiteral(RDFOntologyLiteral ontologyLiteral, RDFOntologyLiteral newOntologyLiteral)
        {
            if (ontologyLiteral != null && newOntologyLiteral != null && !ontologyLiteral.Equals(newOntologyLiteral))
            {
                //Declarations
                this.RemoveLiteral(ontologyLiteral);
                this.AddLiteral(newOntologyLiteral);

                //Assertions
                foreach (RDFOntologyTaxonomyEntry taxonomyEntry in this.Relations.Assertions.SelectEntriesByObject(ontologyLiteral))
                {
                    this.Relations.Assertions.RemoveEntry(taxonomyEntry);

                    //Unlink owl:Axiom annotation
                    this.RemoveAxiomAnnotation(taxonomyEntry);

                    this.AddDataAssertion((RDFOntologyIndividual)taxonomyEntry.TaxonomySubject, (RDFOntologyDatatypeProperty)taxonomyEntry.TaxonomyPredicate, newOntologyLiteral);
                }

                //NegativeAssertions [OWL2]
                foreach (RDFOntologyTaxonomyEntry taxonomyEntry in this.Relations.NegativeAssertions.SelectEntriesByObject(ontologyLiteral))
                {
                    this.Relations.NegativeAssertions.RemoveEntry(taxonomyEntry);
                    this.AddNegativeDataAssertion((RDFOntologyIndividual)taxonomyEntry.TaxonomySubject, (RDFOntologyDatatypeProperty)taxonomyEntry.TaxonomyPredicate, newOntologyLiteral);
                }

                //Annotations
                foreach (RDFOntologyTaxonomyEntry taxonomyEntry in this.Annotations.VersionInfo.SelectEntriesByObject(ontologyLiteral))
                {
                    this.Annotations.VersionInfo.RemoveEntry(taxonomyEntry);
                    this.AddStandardAnnotation(RDFSemanticsEnums.RDFOntologyStandardAnnotation.VersionInfo, (RDFOntologyIndividual)taxonomyEntry.TaxonomySubject, newOntologyLiteral);
                }
                foreach (RDFOntologyTaxonomyEntry taxonomyEntry in this.Annotations.Comment.SelectEntriesByObject(ontologyLiteral))
                {
                    this.Annotations.Comment.RemoveEntry(taxonomyEntry);
                    this.AddStandardAnnotation(RDFSemanticsEnums.RDFOntologyStandardAnnotation.Comment, (RDFOntologyIndividual)taxonomyEntry.TaxonomySubject, newOntologyLiteral);
                }
                foreach (RDFOntologyTaxonomyEntry taxonomyEntry in this.Annotations.Label.SelectEntriesByObject(ontologyLiteral))
                {
                    this.Annotations.Label.RemoveEntry(taxonomyEntry);
                    this.AddStandardAnnotation(RDFSemanticsEnums.RDFOntologyStandardAnnotation.Label, (RDFOntologyIndividual)taxonomyEntry.TaxonomySubject, newOntologyLiteral);
                }   
                foreach (RDFOntologyTaxonomyEntry taxonomyEntry in this.Annotations.SeeAlso.SelectEntriesByObject(ontologyLiteral))
                {
                    this.Annotations.SeeAlso.RemoveEntry(taxonomyEntry);
                    this.AddStandardAnnotation(RDFSemanticsEnums.RDFOntologyStandardAnnotation.SeeAlso, (RDFOntologyIndividual)taxonomyEntry.TaxonomySubject, newOntologyLiteral);
                }
                foreach (RDFOntologyTaxonomyEntry taxonomyEntry in this.Annotations.IsDefinedBy.SelectEntriesByObject(ontologyLiteral))
                {
                    this.Annotations.IsDefinedBy.RemoveEntry(taxonomyEntry);
                    this.AddStandardAnnotation(RDFSemanticsEnums.RDFOntologyStandardAnnotation.IsDefinedBy, (RDFOntologyIndividual)taxonomyEntry.TaxonomySubject, newOntologyLiteral);
                }
                foreach (RDFOntologyTaxonomyEntry taxonomyEntry in this.Annotations.CustomAnnotations.SelectEntriesByObject(ontologyLiteral))
                {
                    this.Annotations.CustomAnnotations.RemoveEntry(taxonomyEntry);
                    this.AddCustomAnnotation((RDFOntologyAnnotationProperty)taxonomyEntry.TaxonomyPredicate, (RDFOntologyIndividual)taxonomyEntry.TaxonomySubject, newOntologyLiteral);
                }
            }
            return this;
        }

        /// <summary>
        /// Removes the given standard annotation from the given ontology individual
        /// </summary>
        public RDFOntologyData RemoveStandardAnnotation(RDFSemanticsEnums.RDFOntologyStandardAnnotation standardAnnotation, RDFOntologyIndividual ontologyIndividual, RDFOntologyResource annotationValue)
        {
            if (ontologyIndividual != null && annotationValue != null)
            {
                switch (standardAnnotation)
                {
                    //owl:versionInfo
                    case RDFSemanticsEnums.RDFOntologyStandardAnnotation.VersionInfo:
                        this.Annotations.VersionInfo.RemoveEntry(new RDFOntologyTaxonomyEntry(ontologyIndividual, RDFVocabulary.OWL.VERSION_INFO.ToRDFOntologyAnnotationProperty(), annotationValue));
                        break;

                    //rdfs:comment
                    case RDFSemanticsEnums.RDFOntologyStandardAnnotation.Comment:
                        this.Annotations.Comment.RemoveEntry(new RDFOntologyTaxonomyEntry(ontologyIndividual, RDFVocabulary.RDFS.COMMENT.ToRDFOntologyAnnotationProperty(), annotationValue));
                        break;

                    //rdfs:label
                    case RDFSemanticsEnums.RDFOntologyStandardAnnotation.Label:
                        this.Annotations.Label.RemoveEntry(new RDFOntologyTaxonomyEntry(ontologyIndividual, RDFVocabulary.RDFS.LABEL.ToRDFOntologyAnnotationProperty(), annotationValue));
                        break;

                    //rdfs:seeAlso
                    case RDFSemanticsEnums.RDFOntologyStandardAnnotation.SeeAlso:
                        this.Annotations.SeeAlso.RemoveEntry(new RDFOntologyTaxonomyEntry(ontologyIndividual, RDFVocabulary.RDFS.SEE_ALSO.ToRDFOntologyAnnotationProperty(), annotationValue));
                        break;

                    //rdfs:isDefinedBy
                    case RDFSemanticsEnums.RDFOntologyStandardAnnotation.IsDefinedBy:
                        this.Annotations.IsDefinedBy.RemoveEntry(new RDFOntologyTaxonomyEntry(ontologyIndividual, RDFVocabulary.RDFS.IS_DEFINED_BY.ToRDFOntologyAnnotationProperty(), annotationValue));
                        break;
                }
            }
            return this;
        }

        /// <summary>
        /// Removes the given custom annotation from the given ontology individual
        /// </summary>
        public RDFOntologyData RemoveCustomAnnotation(RDFOntologyAnnotationProperty ontologyAnnotationProperty, RDFOntologyIndividual ontologyIndividual, RDFOntologyResource annotationValue)
        {
            if (ontologyAnnotationProperty != null && ontologyIndividual != null && annotationValue != null)
            {
                //owl:versionInfo
                if (ontologyAnnotationProperty.Equals(RDFVocabulary.OWL.VERSION_INFO.ToRDFOntologyAnnotationProperty()))
                    this.RemoveStandardAnnotation(RDFSemanticsEnums.RDFOntologyStandardAnnotation.VersionInfo, ontologyIndividual, annotationValue);
                
                //rdfs:comment
                else if (ontologyAnnotationProperty.Equals(RDFVocabulary.RDFS.COMMENT.ToRDFOntologyAnnotationProperty()))
                    this.RemoveStandardAnnotation(RDFSemanticsEnums.RDFOntologyStandardAnnotation.Comment, ontologyIndividual, annotationValue);
                
                //rdfs:label
                else if (ontologyAnnotationProperty.Equals(RDFVocabulary.RDFS.LABEL.ToRDFOntologyAnnotationProperty()))
                    this.RemoveStandardAnnotation(RDFSemanticsEnums.RDFOntologyStandardAnnotation.Label, ontologyIndividual, annotationValue);
                
                //rdfs:seeAlso
                else if (ontologyAnnotationProperty.Equals(RDFVocabulary.RDFS.SEE_ALSO.ToRDFOntologyAnnotationProperty()))
                    this.RemoveStandardAnnotation(RDFSemanticsEnums.RDFOntologyStandardAnnotation.SeeAlso, ontologyIndividual, annotationValue);
                
                //rdfs:isDefinedBy
                else if (ontologyAnnotationProperty.Equals(RDFVocabulary.RDFS.IS_DEFINED_BY.ToRDFOntologyAnnotationProperty()))
                    this.RemoveStandardAnnotation(RDFSemanticsEnums.RDFOntologyStandardAnnotation.IsDefinedBy, ontologyIndividual, annotationValue);

                #region Unsupported
                //owl:versionIRI
                else if (ontologyAnnotationProperty.Equals(RDFVocabulary.OWL.VERSION_IRI.ToRDFOntologyAnnotationProperty()))
                    this.RemoveStandardAnnotation(RDFSemanticsEnums.RDFOntologyStandardAnnotation.VersionIRI, ontologyIndividual, annotationValue);

                //owl:imports
                else if (ontologyAnnotationProperty.Equals(RDFVocabulary.OWL.IMPORTS.ToRDFOntologyAnnotationProperty()))
                    this.RemoveStandardAnnotation(RDFSemanticsEnums.RDFOntologyStandardAnnotation.Imports, ontologyIndividual, annotationValue);
                
                //owl:backwardCompatibleWith
                else if (ontologyAnnotationProperty.Equals(RDFVocabulary.OWL.BACKWARD_COMPATIBLE_WITH.ToRDFOntologyAnnotationProperty()))
                    this.RemoveStandardAnnotation(RDFSemanticsEnums.RDFOntologyStandardAnnotation.BackwardCompatibleWith, ontologyIndividual, annotationValue);
                
                //owl:incompatibleWith
                else if (ontologyAnnotationProperty.Equals(RDFVocabulary.OWL.INCOMPATIBLE_WITH.ToRDFOntologyAnnotationProperty()))
                    this.RemoveStandardAnnotation(RDFSemanticsEnums.RDFOntologyStandardAnnotation.IncompatibleWith, ontologyIndividual, annotationValue);
                
                //owl:priorVersion
                else if (ontologyAnnotationProperty.Equals(RDFVocabulary.OWL.PRIOR_VERSION.ToRDFOntologyAnnotationProperty()))
                    this.RemoveStandardAnnotation(RDFSemanticsEnums.RDFOntologyStandardAnnotation.PriorVersion, ontologyIndividual, annotationValue);
                #endregion

                //custom annotation
                else
                    this.Annotations.CustomAnnotations.RemoveEntry(new RDFOntologyTaxonomyEntry(ontologyIndividual, ontologyAnnotationProperty, annotationValue));
            }
            return this;
        }

        /// <summary>
        /// Removes the "ontologyIndividual -> rdf:type -> ontologyClass" relation from the data
        /// </summary>
        public RDFOntologyData RemoveClassTypeRelation(RDFOntologyIndividual ontologyIndividual, RDFOntologyClass ontologyClass)
        {
            if (ontologyIndividual != null && ontologyClass != null)
            {
                RDFOntologyTaxonomyEntry taxonomyEntry = new RDFOntologyTaxonomyEntry(ontologyIndividual, RDFVocabulary.RDF.TYPE.ToRDFOntologyObjectProperty(), ontologyClass);
                this.Relations.ClassType.RemoveEntry(taxonomyEntry);

                //Unlink owl:Axiom annotation
                this.RemoveAxiomAnnotation(taxonomyEntry);
            }                
            return this;
        }

        /// <summary>
        /// Removes the "aIndividual -> owl:sameAs -> bIndividual" relation from the data
        /// </summary>
        public RDFOntologyData RemoveSameAsRelation(RDFOntologyIndividual aIndividual, RDFOntologyIndividual bIndividual)
        {
            if (aIndividual != null && bIndividual != null)
            {
                RDFOntologyTaxonomyEntry sameAsLeft = new RDFOntologyTaxonomyEntry(aIndividual, RDFVocabulary.OWL.SAME_AS.ToRDFOntologyObjectProperty(), bIndividual);
                this.Relations.SameAs.RemoveEntry(sameAsLeft);
                RDFOntologyTaxonomyEntry sameAsRight = new RDFOntologyTaxonomyEntry(bIndividual, RDFVocabulary.OWL.SAME_AS.ToRDFOntologyObjectProperty(), aIndividual);
                this.Relations.SameAs.RemoveEntry(sameAsRight);

                //Unlink owl:Axiom annotations
                this.RemoveAxiomAnnotation(sameAsLeft);
                this.RemoveAxiomAnnotation(sameAsRight);
            }
            return this;
        }

        /// <summary>
        /// Removes the "aIndividual -> owl:differentFrom -> bIndividual" relation from the data
        /// </summary>
        public RDFOntologyData RemoveDifferentFromRelation(RDFOntologyIndividual aIndividual, RDFOntologyIndividual bIndividual)
        {
            if (aIndividual != null && bIndividual != null)
            {
                RDFOntologyTaxonomyEntry differentFromLeft = new RDFOntologyTaxonomyEntry(aIndividual, RDFVocabulary.OWL.DIFFERENT_FROM.ToRDFOntologyObjectProperty(), bIndividual);
                this.Relations.DifferentFrom.RemoveEntry(differentFromLeft);
                RDFOntologyTaxonomyEntry differentFromRight = new RDFOntologyTaxonomyEntry(bIndividual, RDFVocabulary.OWL.DIFFERENT_FROM.ToRDFOntologyObjectProperty(), aIndividual);
                this.Relations.DifferentFrom.RemoveEntry(differentFromRight);

                //Unlink owl:Axiom annotations
                this.RemoveAxiomAnnotation(differentFromLeft);
                this.RemoveAxiomAnnotation(differentFromRight);
            }
            return this;
        }

        /// <summary>
        /// Foreach of the given list of individuals, removes the "aIndividual -> owl:differentFrom -> bIndividual" relation from the data [OWL2]
        /// </summary>
        public RDFOntologyData RemoveAllDifferentRelation(List<RDFOntologyIndividual> ontologyIndividuals)
        {
            ontologyIndividuals?.ForEach(outerIndividual =>
                ontologyIndividuals?.ForEach(innerIndividual =>
                    this.RemoveDifferentFromRelation(innerIndividual, outerIndividual)));
            return this;
        }

        /// <summary>
        /// Removes the "aIndividual -> objectProperty -> bIndividual" relation from the data
        /// </summary>
        public RDFOntologyData RemoveObjectAssertion(RDFOntologyIndividual aIndividual, RDFOntologyObjectProperty objectProperty, RDFOntologyIndividual bIndividual)
        {
            if (aIndividual != null && objectProperty != null && bIndividual != null)
            {
                if (objectProperty.Equals(RDFVocabulary.SKOS.MEMBER))
                    this.RemoveMemberRelation(aIndividual, bIndividual);
                else if (objectProperty.Equals(RDFVocabulary.SKOS.MEMBER_LIST))
                    this.RemoveMemberListRelation(aIndividual, bIndividual);
                else
                {
                    RDFOntologyTaxonomyEntry taxonomyEntry = new RDFOntologyTaxonomyEntry(aIndividual, objectProperty, bIndividual);
                    this.Relations.Assertions.RemoveEntry(taxonomyEntry);

                    //Unlink owl:Axiom annotation
                    this.RemoveAxiomAnnotation(taxonomyEntry);
                }
            }
            return this;
        }

        /// <summary>
        /// Removes the "ontologyIndividual -> datatypeProperty -> ontologyLiteral" relation from the data
        /// </summary>
        public RDFOntologyData RemoveDataAssertion(RDFOntologyIndividual ontologyIndividual, RDFOntologyDatatypeProperty datatypeProperty, RDFOntologyLiteral ontologyLiteral)
        {
            if (ontologyIndividual != null && datatypeProperty != null && ontologyLiteral != null)
            {
                RDFOntologyTaxonomyEntry taxonomyEntry = new RDFOntologyTaxonomyEntry(ontologyIndividual, datatypeProperty, ontologyLiteral);
                this.Relations.Assertions.RemoveEntry(taxonomyEntry);

                //Unlink owl:Axiom annotation
                this.RemoveAxiomAnnotation(taxonomyEntry);
            }
            return this;
        }

        /// <summary>
        /// Removes the "aIndividual -> objectProperty -> bIndividual" negative relation from the data [OWL2]
        /// </summary>
        public RDFOntologyData RemoveNegativeObjectAssertion(RDFOntologyIndividual aIndividual, RDFOntologyObjectProperty objectProperty, RDFOntologyIndividual bIndividual)
        {
            if (aIndividual != null && objectProperty != null && bIndividual != null)
                this.Relations.NegativeAssertions.RemoveEntry(new RDFOntologyTaxonomyEntry(aIndividual, objectProperty, bIndividual));
            return this;
        }

        /// <summary>
        /// Removes the "ontologyIndividual -> datatypeProperty -> ontologyLiteral" negative relation from the data [OWL2]
        /// </summary>
        public RDFOntologyData RemoveNegativeDataAssertion(RDFOntologyIndividual ontologyIndividual, RDFOntologyDatatypeProperty datatypeProperty, RDFOntologyLiteral ontologyLiteral)
        {
            if (ontologyIndividual != null && datatypeProperty != null && ontologyLiteral != null)
                this.Relations.NegativeAssertions.RemoveEntry(new RDFOntologyTaxonomyEntry(ontologyIndividual, datatypeProperty, ontologyLiteral));
            return this;
        }

        /// <summary>
        /// Removes the given owl:Axiom annotation [OWL2]
        /// </summary>
        internal RDFOntologyData RemoveAxiomAnnotation(RDFOntologyTaxonomyEntry taxonomyEntry)
        {
            foreach (RDFOntologyTaxonomyEntry axnTaxonomyEntry in this.Annotations.AxiomAnnotations.SelectEntriesBySubject(this.GetTaxonomyEntryRepresentative(taxonomyEntry)))
                this.Annotations.AxiomAnnotations.RemoveEntry(axnTaxonomyEntry);
            return this;
        }
        #endregion

        #region Select
        /// <summary>
        /// Selects the individual represented by the given identifier
        /// </summary>
        public RDFOntologyIndividual SelectIndividual(long individualID)
            => this.Individuals.ContainsKey(individualID) ? this.Individuals[individualID] : null;

        /// <summary>
        /// Selects the individual represented by the given string
        /// </summary>
        public RDFOntologyIndividual SelectIndividual(string individual)
            => individual != null ? SelectIndividual(RDFModelUtilities.CreateHash(individual)) : null;

        /// <summary>
        /// Selects the literal represented by the given identifier
        /// </summary>
        public RDFOntologyLiteral SelectLiteral(long literalID)
            => this.Literals.ContainsKey(literalID) ? this.Literals[literalID] : null;

        /// <summary>
        /// Selects the literal represented by the given string
        /// </summary>
        public RDFOntologyLiteral SelectLiteral(string literal)
            => literal != null ? SelectLiteral(RDFModelUtilities.CreateHash(literal)) : null;

        /// <summary>
        /// Gets the representative of the given taxonomy entry
        /// </summary>
        internal RDFOntologyIndividual GetTaxonomyEntryRepresentative(RDFOntologyTaxonomyEntry taxonomyEntry)
            => new RDFOntologyIndividual(new RDFResource($"bnode:semref{taxonomyEntry.TaxonomyEntryID}"));
        #endregion

        #region Set
        /// <summary>
        /// Builds a new intersection data from this data and a given one
        /// </summary>
        public RDFOntologyData IntersectWith(RDFOntologyData ontologyData)
        {
            RDFOntologyData result = new RDFOntologyData();

            if (ontologyData != null)
            {
                //Add intersection individuals
                foreach (RDFOntologyIndividual individual in this)
                    if (ontologyData.Individuals.ContainsKey(individual.PatternMemberID))
                        result.AddIndividual(individual);

                //Add intersection literals
                foreach (RDFOntologyLiteral literal in this.Literals.Values)
                    if (ontologyData.Literals.ContainsKey(literal.PatternMemberID))
                        result.AddLiteral(literal);

                //Add intersection relations
                result.Relations.ClassType = this.Relations.ClassType.IntersectWith(ontologyData.Relations.ClassType);
                result.Relations.SameAs = this.Relations.SameAs.IntersectWith(ontologyData.Relations.SameAs);
                result.Relations.DifferentFrom = this.Relations.DifferentFrom.IntersectWith(ontologyData.Relations.DifferentFrom);
                result.Relations.Assertions = this.Relations.Assertions.IntersectWith(ontologyData.Relations.Assertions);
                result.Relations.NegativeAssertions = this.Relations.NegativeAssertions.IntersectWith(ontologyData.Relations.NegativeAssertions); //OWL2
                result.Relations.Member = this.Relations.Member.IntersectWith(ontologyData.Relations.Member); //SKOS
                result.Relations.MemberList = this.Relations.MemberList.IntersectWith(ontologyData.Relations.MemberList); //SKOS

                //Add intersection annotations
                result.Annotations.VersionInfo = this.Annotations.VersionInfo.IntersectWith(ontologyData.Annotations.VersionInfo);
                result.Annotations.Comment = this.Annotations.Comment.IntersectWith(ontologyData.Annotations.Comment);
                result.Annotations.Label = this.Annotations.Label.IntersectWith(ontologyData.Annotations.Label);
                result.Annotations.SeeAlso = this.Annotations.SeeAlso.IntersectWith(ontologyData.Annotations.SeeAlso);
                result.Annotations.IsDefinedBy = this.Annotations.IsDefinedBy.IntersectWith(ontologyData.Annotations.IsDefinedBy);
                result.Annotations.CustomAnnotations = this.Annotations.CustomAnnotations.IntersectWith(ontologyData.Annotations.CustomAnnotations);
                result.Annotations.AxiomAnnotations = this.Annotations.AxiomAnnotations.IntersectWith(ontologyData.Annotations.AxiomAnnotations); //OWL2
            }

            return result;
        }

        /// <summary>
        /// Builds a new union data from this data and a given one
        /// </summary>
        public RDFOntologyData UnionWith(RDFOntologyData ontologyData)
        {
            RDFOntologyData result = new RDFOntologyData();

            //Add individuals from this data
            foreach (RDFOntologyIndividual individual in this)
                result.AddIndividual(individual);

            //Add literals from this data
            foreach (RDFOntologyLiteral literal in this.Literals.Values)
                result.AddLiteral(literal);

            //Add relations from this data
            result.Relations.ClassType = result.Relations.ClassType.UnionWith(this.Relations.ClassType);
            result.Relations.SameAs = result.Relations.SameAs.UnionWith(this.Relations.SameAs);
            result.Relations.DifferentFrom = result.Relations.DifferentFrom.UnionWith(this.Relations.DifferentFrom);
            result.Relations.Assertions = result.Relations.Assertions.UnionWith(this.Relations.Assertions);
            result.Relations.NegativeAssertions = result.Relations.NegativeAssertions.UnionWith(this.Relations.NegativeAssertions); //OWL2
            result.Relations.Member = result.Relations.Member.UnionWith(this.Relations.Member); //SKOS
            result.Relations.MemberList = result.Relations.MemberList.UnionWith(this.Relations.MemberList); //SKOS

            //Add annotations from this data
            result.Annotations.VersionInfo = result.Annotations.VersionInfo.UnionWith(this.Annotations.VersionInfo);
            result.Annotations.Comment = result.Annotations.Comment.UnionWith(this.Annotations.Comment);
            result.Annotations.Label = result.Annotations.Label.UnionWith(this.Annotations.Label);
            result.Annotations.SeeAlso = result.Annotations.SeeAlso.UnionWith(this.Annotations.SeeAlso);
            result.Annotations.IsDefinedBy = result.Annotations.IsDefinedBy.UnionWith(this.Annotations.IsDefinedBy);
            result.Annotations.CustomAnnotations = result.Annotations.CustomAnnotations.UnionWith(this.Annotations.CustomAnnotations);
            result.Annotations.AxiomAnnotations = result.Annotations.AxiomAnnotations.UnionWith(this.Annotations.AxiomAnnotations); //OWL2

            //Manage the given data
            if (ontologyData != null)
            {
                //Add individuals from the given data
                foreach (RDFOntologyIndividual individual in ontologyData)
                    result.AddIndividual(individual);

                //Add literals from the given data
                foreach (RDFOntologyLiteral literal in ontologyData.Literals.Values)
                    result.AddLiteral(literal);

                //Add relations from the given data
                result.Relations.ClassType = result.Relations.ClassType.UnionWith(ontologyData.Relations.ClassType);
                result.Relations.SameAs = result.Relations.SameAs.UnionWith(ontologyData.Relations.SameAs);
                result.Relations.DifferentFrom = result.Relations.DifferentFrom.UnionWith(ontologyData.Relations.DifferentFrom);
                result.Relations.Assertions = result.Relations.Assertions.UnionWith(ontologyData.Relations.Assertions);
                result.Relations.NegativeAssertions = result.Relations.NegativeAssertions.UnionWith(ontologyData.Relations.NegativeAssertions); //OWL2
                result.Relations.Member = result.Relations.Member.UnionWith(ontologyData.Relations.Member); //SKOS
                result.Relations.MemberList = result.Relations.MemberList.UnionWith(ontologyData.Relations.MemberList); //SKOS

                //Add annotations from the given data
                result.Annotations.VersionInfo = result.Annotations.VersionInfo.UnionWith(ontologyData.Annotations.VersionInfo);
                result.Annotations.Comment = result.Annotations.Comment.UnionWith(ontologyData.Annotations.Comment);
                result.Annotations.Label = result.Annotations.Label.UnionWith(ontologyData.Annotations.Label);
                result.Annotations.SeeAlso = result.Annotations.SeeAlso.UnionWith(ontologyData.Annotations.SeeAlso);
                result.Annotations.IsDefinedBy = result.Annotations.IsDefinedBy.UnionWith(ontologyData.Annotations.IsDefinedBy);
                result.Annotations.CustomAnnotations = result.Annotations.CustomAnnotations.UnionWith(ontologyData.Annotations.CustomAnnotations);
                result.Annotations.AxiomAnnotations = result.Annotations.AxiomAnnotations.UnionWith(ontologyData.Annotations.AxiomAnnotations); //OWL2
            }

            return result;
        }

        /// <summary>
        /// Builds a new difference data from this data and a given one
        /// </summary>
        public RDFOntologyData DifferenceWith(RDFOntologyData ontologyData)
        {
            RDFOntologyData result = new RDFOntologyData();

            if (ontologyData != null)
            {
                //Add difference individuals
                foreach (RDFOntologyIndividual individual in this)
                    if (!ontologyData.Individuals.ContainsKey(individual.PatternMemberID))
                        result.AddIndividual(individual);

                //Add difference literals
                foreach (RDFOntologyLiteral literal in this.Literals.Values)
                    if (!ontologyData.Literals.ContainsKey(literal.PatternMemberID))
                        result.AddLiteral(literal);

                //Add difference relations
                result.Relations.ClassType = this.Relations.ClassType.DifferenceWith(ontologyData.Relations.ClassType);
                result.Relations.SameAs = this.Relations.SameAs.DifferenceWith(ontologyData.Relations.SameAs);
                result.Relations.DifferentFrom = this.Relations.DifferentFrom.DifferenceWith(ontologyData.Relations.DifferentFrom);
                result.Relations.Assertions = this.Relations.Assertions.DifferenceWith(ontologyData.Relations.Assertions);
                result.Relations.NegativeAssertions = this.Relations.NegativeAssertions.DifferenceWith(ontologyData.Relations.NegativeAssertions); //OWL2
                result.Relations.Member = this.Relations.Member.DifferenceWith(ontologyData.Relations.Member); //SKOS
                result.Relations.MemberList = this.Relations.MemberList.DifferenceWith(ontologyData.Relations.MemberList); //SKOS

                //Add difference annotations
                result.Annotations.VersionInfo = this.Annotations.VersionInfo.DifferenceWith(ontologyData.Annotations.VersionInfo);
                result.Annotations.Comment = this.Annotations.Comment.DifferenceWith(ontologyData.Annotations.Comment);
                result.Annotations.Label = this.Annotations.Label.DifferenceWith(ontologyData.Annotations.Label);
                result.Annotations.SeeAlso = this.Annotations.SeeAlso.DifferenceWith(ontologyData.Annotations.SeeAlso);
                result.Annotations.IsDefinedBy = this.Annotations.IsDefinedBy.DifferenceWith(ontologyData.Annotations.IsDefinedBy);
                result.Annotations.CustomAnnotations = this.Annotations.CustomAnnotations.DifferenceWith(ontologyData.Annotations.CustomAnnotations);
                result.Annotations.AxiomAnnotations = this.Annotations.AxiomAnnotations.DifferenceWith(ontologyData.Annotations.AxiomAnnotations); //OWL2
            }
            else
            {
                //Add individuals from this data
                foreach (RDFOntologyIndividual individual in this)
                    result.AddIndividual(individual);

                //Add literals from this data
                foreach (RDFOntologyLiteral literal in this.Literals.Values)
                    result.AddLiteral(literal);

                //Add relations from this data
                result.Relations.ClassType = result.Relations.ClassType.UnionWith(this.Relations.ClassType);
                result.Relations.SameAs = result.Relations.SameAs.UnionWith(this.Relations.SameAs);
                result.Relations.DifferentFrom = result.Relations.DifferentFrom.UnionWith(this.Relations.DifferentFrom);
                result.Relations.Assertions = result.Relations.Assertions.UnionWith(this.Relations.Assertions);
                result.Relations.NegativeAssertions = result.Relations.NegativeAssertions.UnionWith(this.Relations.NegativeAssertions); //OWL2
                result.Relations.Member = result.Relations.Member.UnionWith(this.Relations.Member); //SKOS
                result.Relations.MemberList = result.Relations.MemberList.UnionWith(this.Relations.MemberList); //SKOS

                //Add annotations from this data
                result.Annotations.VersionInfo = result.Annotations.VersionInfo.UnionWith(this.Annotations.VersionInfo);
                result.Annotations.Comment = result.Annotations.Comment.UnionWith(this.Annotations.Comment);
                result.Annotations.Label = result.Annotations.Label.UnionWith(this.Annotations.Label);
                result.Annotations.SeeAlso = result.Annotations.SeeAlso.UnionWith(this.Annotations.SeeAlso);
                result.Annotations.IsDefinedBy = result.Annotations.IsDefinedBy.UnionWith(this.Annotations.IsDefinedBy);
                result.Annotations.CustomAnnotations = result.Annotations.CustomAnnotations.UnionWith(this.Annotations.CustomAnnotations);
                result.Annotations.AxiomAnnotations = result.Annotations.AxiomAnnotations.UnionWith(this.Annotations.AxiomAnnotations); //OWL2
            }

            return result;
        }
        #endregion

        #region Convert
        /// <summary>
        /// Gets a graph representation of this ontology data, exporting inferences according to the selected behavior
        /// </summary>
        public RDFGraph ToRDFGraph(RDFSemanticsEnums.RDFOntologyInferenceExportBehavior infexpBehavior)
        {
            RDFGraph result = new RDFGraph();

            //Relations
            result = result.UnionWith(this.Relations.SameAs.ReifyToRDFGraph(infexpBehavior, nameof(this.Relations.SameAs)))
                           .UnionWith(this.Relations.DifferentFrom.ReifyToRDFGraph(infexpBehavior, nameof(this.Relations.DifferentFrom)))
                           .UnionWith(this.Relations.ClassType.ReifyToRDFGraph(infexpBehavior, nameof(this.Relations.ClassType)))
                           .UnionWith(this.Relations.Assertions.ReifyToRDFGraph(infexpBehavior, nameof(this.Relations.Assertions)))
                           .UnionWith(this.Relations.NegativeAssertions.ReifyToRDFGraph(infexpBehavior, nameof(this.Relations.NegativeAssertions))) //OWL2
                           .UnionWith(this.Relations.Member.ReifyToRDFGraph(infexpBehavior, nameof(this.Relations.Member))) //SKOS
                           .UnionWith(this.Relations.MemberList.ReifyToRDFGraph(infexpBehavior, nameof(this.Relations.MemberList))); //SKOS

            //Annotations
            result = result.UnionWith(this.Annotations.VersionInfo.ReifyToRDFGraph(infexpBehavior, nameof(this.Annotations.VersionInfo)))
                           .UnionWith(this.Annotations.Comment.ReifyToRDFGraph(infexpBehavior, nameof(this.Annotations.Comment)))
                           .UnionWith(this.Annotations.Label.ReifyToRDFGraph(infexpBehavior, nameof(this.Annotations.Label)))
                           .UnionWith(this.Annotations.SeeAlso.ReifyToRDFGraph(infexpBehavior, nameof(this.Annotations.SeeAlso)))
                           .UnionWith(this.Annotations.IsDefinedBy.ReifyToRDFGraph(infexpBehavior, nameof(this.Annotations.IsDefinedBy)))
                           .UnionWith(this.Annotations.CustomAnnotations.ReifyToRDFGraph(infexpBehavior, nameof(this.Annotations.CustomAnnotations)))
                           .UnionWith(this.Annotations.AxiomAnnotations.ReifyToRDFGraph(infexpBehavior, nameof(this.Annotations.AxiomAnnotations), null, null, this)); //OWL2

            return result;
        }

        /// <summary>
        /// Asynchronously gets a graph representation of this ontology data, exporting inferences according to the selected behavior
        /// </summary>
        public Task<RDFGraph> ToRDFGraphAsync(RDFSemanticsEnums.RDFOntologyInferenceExportBehavior infexpBehavior)
            => Task.Run(() => ToRDFGraph(infexpBehavior));
        #endregion

        #endregion
    }
}