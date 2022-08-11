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
using RDFSharp.Query;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;

namespace RDFSharp.Semantics
{
    /// <summary>
    /// RDFSemanticsUtilities is a collector of reusable utility methods for RDF ontology management.
    /// </summary>
    internal static class RDFSemanticsUtilities
    {
        internal static readonly Lazy<Regex> NumberRegex = new Lazy<Regex>(() => new Regex(@"^[0-9]+$", RegexOptions.Compiled));
        internal static readonly HashSet<long> StandardAnnotationProperties = new HashSet<long>()
        {
            { RDFVocabulary.OWL.VERSION_INFO.PatternMemberID },
            { RDFVocabulary.OWL.VERSION_IRI.PatternMemberID },
            { RDFVocabulary.RDFS.COMMENT.PatternMemberID },
            { RDFVocabulary.RDFS.LABEL.PatternMemberID },
            { RDFVocabulary.RDFS.SEE_ALSO.PatternMemberID },
            { RDFVocabulary.RDFS.IS_DEFINED_BY.PatternMemberID },
            { RDFVocabulary.OWL.PRIOR_VERSION.PatternMemberID },
            { RDFVocabulary.OWL.BACKWARD_COMPATIBLE_WITH.PatternMemberID },
            { RDFVocabulary.OWL.INCOMPATIBLE_WITH.PatternMemberID },
            { RDFVocabulary.OWL.IMPORTS.PatternMemberID }
        };

        #region Convert
        /// <summary>
        /// Gets an ontology representation of the given graph
        /// </summary>
        internal static RDFOntology FromRDFGraph(RDFGraph ontGraph)
        {
            if (ontGraph == null)
                return null;

            RDFSemanticsEvents.RaiseSemanticsInfo(string.Format("Graph '{0}' is going to be parsed as Ontology (triples not having supported ontology semantics may be discarded!)", ontGraph.Context));

            //Step 1: start the graph->ontology process
            StartProcess(ontGraph, out Dictionary<string, RDFGraph> prefetchContext);

            #region Initialization

            //Step 2: initialize ontology (declaration)
            InitializeOntology(ontGraph, prefetchContext, out RDFOntology ontology);

            //Step 3: initialize property model (property declarations)
            InitializePropertyModel(ontology, ontGraph, prefetchContext);

            //Step 4: initialize class model (class declarations)
            InitializeClassModel(ontology, ontGraph, prefetchContext);

            //Step 5: initialize data (individual declarations)
            InitializeData(ontology, ontGraph, prefetchContext);

            #endregion

            #region Finalization

            //Step 6.1: finalize restrictions (specializations, attributes and relations)
            FinalizeRestrictions(ontology, ontGraph, prefetchContext);

            //Step 6.2: finalize enumerate/datarange classes (specializations, attributes and relations)
            FinalizeEnumeratesAndDataRanges(ontology, ontGraph, prefetchContext);

            //Step 6.3: finalize property model (specializations, attributes and relations)
            FinalizePropertyModel(ontology, ontGraph, prefetchContext);

            //Step 6.4: finalize class model (specializations, attributes and relations)
            FinalizeClassModel(ontology, ontGraph, prefetchContext);

            //Step 6.5: finalize data (attributes and relations)
            FinalizeData(ontology, ontGraph, prefetchContext);

            //Step 6.6: finalize annotations (ontology, class model, property model, data, axioms)
            FinalizeOntologyAnnotations(ontology, ontGraph, prefetchContext);
            FinalizeClassModelAnnotations(ontology, ontGraph, prefetchContext);
            FinalizePropertyModelAnnotations(ontology, ontGraph, prefetchContext);
            FinalizeDataAnnotations(ontology, ontGraph, prefetchContext);
            FinalizeAxiomAnnotations(ontology, ontGraph, prefetchContext);

            #endregion

            //Step 6.7: end the graph->ontology process
            EndProcess(ref ontology);

            RDFSemanticsEvents.RaiseSemanticsInfo(string.Format("Graph '{0}' has been parsed as Ontology.", ontGraph.Context));
            return ontology;
        }

        /// <summary>
        /// Prefetches the context cache from the given RDF graph
        /// </summary>
        private static void StartProcess(RDFGraph ontGraph, out Dictionary<string, RDFGraph> prefetchContext)
            => prefetchContext = new Dictionary<string, RDFGraph>()
                {
                    { nameof(RDFVocabulary.RDF.TYPE), ontGraph.SelectTriplesByPredicate(RDFVocabulary.RDF.TYPE) },
                    { nameof(RDFVocabulary.RDF.FIRST), ontGraph.SelectTriplesByPredicate(RDFVocabulary.RDF.FIRST) },
                    { nameof(RDFVocabulary.RDF.REST), ontGraph.SelectTriplesByPredicate(RDFVocabulary.RDF.REST) },
                    { nameof(RDFVocabulary.SKOS.MEMBER), ontGraph.SelectTriplesByPredicate(RDFVocabulary.SKOS.MEMBER) }, //SKOS
                    { nameof(RDFVocabulary.SKOS.MEMBER_LIST), ontGraph.SelectTriplesByPredicate(RDFVocabulary.SKOS.MEMBER_LIST) }, //SKOS
                    { nameof(RDFVocabulary.RDFS.SUB_CLASS_OF), ontGraph.SelectTriplesByPredicate(RDFVocabulary.RDFS.SUB_CLASS_OF) },
                    { nameof(RDFVocabulary.RDFS.SUB_PROPERTY_OF), ontGraph.SelectTriplesByPredicate(RDFVocabulary.RDFS.SUB_PROPERTY_OF) },
                    { nameof(RDFVocabulary.RDFS.DOMAIN), ontGraph.SelectTriplesByPredicate(RDFVocabulary.RDFS.DOMAIN) },
                    { nameof(RDFVocabulary.RDFS.RANGE), ontGraph.SelectTriplesByPredicate(RDFVocabulary.RDFS.RANGE) },
                    { nameof(RDFVocabulary.OWL.EQUIVALENT_CLASS), ontGraph.SelectTriplesByPredicate(RDFVocabulary.OWL.EQUIVALENT_CLASS) },
                    { nameof(RDFVocabulary.OWL.DISJOINT_WITH), ontGraph.SelectTriplesByPredicate(RDFVocabulary.OWL.DISJOINT_WITH) },
                    { nameof(RDFVocabulary.OWL.ALL_DISJOINT_CLASSES), ontGraph.SelectTriplesByObject(RDFVocabulary.OWL.ALL_DISJOINT_CLASSES).SelectTriplesByPredicate(RDFVocabulary.RDF.TYPE)}, //OWL2
                    { nameof(RDFVocabulary.OWL.HAS_KEY), ontGraph.SelectTriplesByPredicate(RDFVocabulary.OWL.HAS_KEY) }, //OWL2
                    { nameof(RDFVocabulary.OWL.PROPERTY_CHAIN_AXIOM), ontGraph.SelectTriplesByPredicate(RDFVocabulary.OWL.PROPERTY_CHAIN_AXIOM) }, //OWL2
                    { nameof(RDFVocabulary.OWL.EQUIVALENT_PROPERTY), ontGraph.SelectTriplesByPredicate(RDFVocabulary.OWL.EQUIVALENT_PROPERTY) },
                    { nameof(RDFVocabulary.OWL.PROPERTY_DISJOINT_WITH), ontGraph.SelectTriplesByPredicate(RDFVocabulary.OWL.PROPERTY_DISJOINT_WITH) }, //OWL2
                    { nameof(RDFVocabulary.OWL.ALL_DISJOINT_PROPERTIES), ontGraph.SelectTriplesByObject(RDFVocabulary.OWL.ALL_DISJOINT_PROPERTIES).SelectTriplesByPredicate(RDFVocabulary.RDF.TYPE)}, //OWL2
                    { nameof(RDFVocabulary.OWL.INVERSE_OF), ontGraph.SelectTriplesByPredicate(RDFVocabulary.OWL.INVERSE_OF) },
                    { nameof(RDFVocabulary.OWL.ON_CLASS), ontGraph.SelectTriplesByPredicate(RDFVocabulary.OWL.ON_CLASS) }, //OWL2
                    { nameof(RDFVocabulary.OWL.ON_DATARANGE), ontGraph.SelectTriplesByPredicate(RDFVocabulary.OWL.ON_DATARANGE) }, //OWL2
                    { nameof(RDFVocabulary.OWL.ON_PROPERTY), ontGraph.SelectTriplesByPredicate(RDFVocabulary.OWL.ON_PROPERTY) },
                    { nameof(RDFVocabulary.OWL.ONE_OF), ontGraph.SelectTriplesByPredicate(RDFVocabulary.OWL.ONE_OF) },
                    { nameof(RDFVocabulary.OWL.UNION_OF), ontGraph.SelectTriplesByPredicate(RDFVocabulary.OWL.UNION_OF) },
                    { nameof(RDFVocabulary.OWL.DISJOINT_UNION_OF), ontGraph.SelectTriplesByPredicate(RDFVocabulary.OWL.DISJOINT_UNION_OF) }, //OWL2
                    { nameof(RDFVocabulary.OWL.INTERSECTION_OF), ontGraph.SelectTriplesByPredicate(RDFVocabulary.OWL.INTERSECTION_OF) },
                    { nameof(RDFVocabulary.OWL.COMPLEMENT_OF), ontGraph.SelectTriplesByPredicate(RDFVocabulary.OWL.COMPLEMENT_OF) },
                    { nameof(RDFVocabulary.OWL.ALL_VALUES_FROM), ontGraph.SelectTriplesByPredicate(RDFVocabulary.OWL.ALL_VALUES_FROM) },
                    { nameof(RDFVocabulary.OWL.SOME_VALUES_FROM), ontGraph.SelectTriplesByPredicate(RDFVocabulary.OWL.SOME_VALUES_FROM) },
                    { nameof(RDFVocabulary.OWL.HAS_SELF), ontGraph.SelectTriplesByPredicate(RDFVocabulary.OWL.HAS_SELF) }, //OWL2
                    { nameof(RDFVocabulary.OWL.HAS_VALUE), ontGraph.SelectTriplesByPredicate(RDFVocabulary.OWL.HAS_VALUE) },
                    { nameof(RDFVocabulary.OWL.CARDINALITY), ontGraph.SelectTriplesByPredicate(RDFVocabulary.OWL.CARDINALITY) },
                    { nameof(RDFVocabulary.OWL.MIN_CARDINALITY), ontGraph.SelectTriplesByPredicate(RDFVocabulary.OWL.MIN_CARDINALITY) },
                    { nameof(RDFVocabulary.OWL.MAX_CARDINALITY), ontGraph.SelectTriplesByPredicate(RDFVocabulary.OWL.MAX_CARDINALITY) },
                    { nameof(RDFVocabulary.OWL.QUALIFIED_CARDINALITY), ontGraph.SelectTriplesByPredicate(RDFVocabulary.OWL.QUALIFIED_CARDINALITY) }, //OWL2
                    { nameof(RDFVocabulary.OWL.MIN_QUALIFIED_CARDINALITY), ontGraph.SelectTriplesByPredicate(RDFVocabulary.OWL.MIN_QUALIFIED_CARDINALITY) }, //OWL2
                    { nameof(RDFVocabulary.OWL.MAX_QUALIFIED_CARDINALITY), ontGraph.SelectTriplesByPredicate(RDFVocabulary.OWL.MAX_QUALIFIED_CARDINALITY) }, //OWL2
                    { nameof(RDFVocabulary.OWL.SAME_AS), ontGraph.SelectTriplesByPredicate(RDFVocabulary.OWL.SAME_AS) },
                    { nameof(RDFVocabulary.OWL.DIFFERENT_FROM), ontGraph.SelectTriplesByPredicate(RDFVocabulary.OWL.DIFFERENT_FROM) },
                    { nameof(RDFVocabulary.OWL.ALL_DIFFERENT), ontGraph.SelectTriplesByObject(RDFVocabulary.OWL.ALL_DIFFERENT).SelectTriplesByPredicate(RDFVocabulary.RDF.TYPE)}, //OWL2
                    { nameof(RDFVocabulary.OWL.MEMBERS), ontGraph.SelectTriplesByPredicate(RDFVocabulary.OWL.MEMBERS) },
                    { nameof(RDFVocabulary.OWL.DISTINCT_MEMBERS), ontGraph.SelectTriplesByPredicate(RDFVocabulary.OWL.DISTINCT_MEMBERS) }, //OWL2
                    { nameof(RDFVocabulary.OWL.NEGATIVE_PROPERTY_ASSERTION), ontGraph.SelectTriplesByObject(RDFVocabulary.OWL.NEGATIVE_PROPERTY_ASSERTION).SelectTriplesByPredicate(RDFVocabulary.RDF.TYPE)}, //OWL2
                    { nameof(RDFVocabulary.OWL.SOURCE_INDIVIDUAL), ontGraph.SelectTriplesByPredicate(RDFVocabulary.OWL.SOURCE_INDIVIDUAL) }, //OWL2
                    { nameof(RDFVocabulary.OWL.ASSERTION_PROPERTY), ontGraph.SelectTriplesByPredicate(RDFVocabulary.OWL.ASSERTION_PROPERTY) }, //OWL2
                    { nameof(RDFVocabulary.OWL.TARGET_VALUE), ontGraph.SelectTriplesByPredicate(RDFVocabulary.OWL.TARGET_VALUE) }, //OWL2
                    { nameof(RDFVocabulary.OWL.TARGET_INDIVIDUAL), ontGraph.SelectTriplesByPredicate(RDFVocabulary.OWL.TARGET_INDIVIDUAL) }, //OWL2
                    { nameof(RDFVocabulary.OWL.AXIOM), ontGraph.SelectTriplesByObject(RDFVocabulary.OWL.AXIOM).SelectTriplesByPredicate(RDFVocabulary.RDF.TYPE)}, //OWL2
                    { nameof(RDFVocabulary.OWL.ANNOTATED_SOURCE), ontGraph.SelectTriplesByPredicate(RDFVocabulary.OWL.ANNOTATED_SOURCE) }, //OWL2
                    { nameof(RDFVocabulary.OWL.ANNOTATED_PROPERTY), ontGraph.SelectTriplesByPredicate(RDFVocabulary.OWL.ANNOTATED_PROPERTY) }, //OWL2
                    { nameof(RDFVocabulary.OWL.ANNOTATED_TARGET), ontGraph.SelectTriplesByPredicate(RDFVocabulary.OWL.ANNOTATED_TARGET) }, //OWL2
                    { nameof(RDFVocabulary.OWL.NAMED_INDIVIDUAL), ontGraph.SelectTriplesByObject(RDFVocabulary.OWL.NAMED_INDIVIDUAL).SelectTriplesByPredicate(RDFVocabulary.RDF.TYPE)}, //OWL2
                    //Annotations
                    { nameof(RDFVocabulary.OWL.VERSION_INFO), ontGraph.SelectTriplesByPredicate(RDFVocabulary.OWL.VERSION_INFO) },
                    { nameof(RDFVocabulary.OWL.VERSION_IRI), ontGraph.SelectTriplesByPredicate(RDFVocabulary.OWL.VERSION_IRI) },
                    { nameof(RDFVocabulary.RDFS.COMMENT), ontGraph.SelectTriplesByPredicate(RDFVocabulary.RDFS.COMMENT) },
                    { nameof(RDFVocabulary.RDFS.LABEL), ontGraph.SelectTriplesByPredicate(RDFVocabulary.RDFS.LABEL) },
                    { nameof(RDFVocabulary.RDFS.SEE_ALSO), ontGraph.SelectTriplesByPredicate(RDFVocabulary.RDFS.SEE_ALSO) },
                    { nameof(RDFVocabulary.RDFS.IS_DEFINED_BY), ontGraph.SelectTriplesByPredicate(RDFVocabulary.RDFS.IS_DEFINED_BY) },
                    { nameof(RDFVocabulary.OWL.IMPORTS), ontGraph.SelectTriplesByPredicate(RDFVocabulary.OWL.IMPORTS) },
                    { nameof(RDFVocabulary.OWL.BACKWARD_COMPATIBLE_WITH), ontGraph.SelectTriplesByPredicate(RDFVocabulary.OWL.BACKWARD_COMPATIBLE_WITH) },
                    { nameof(RDFVocabulary.OWL.INCOMPATIBLE_WITH), ontGraph.SelectTriplesByPredicate(RDFVocabulary.OWL.INCOMPATIBLE_WITH) },
                    { nameof(RDFVocabulary.OWL.PRIOR_VERSION), ontGraph.SelectTriplesByPredicate(RDFVocabulary.OWL.PRIOR_VERSION) }
                };

        /// <summary>
        /// Parses the ontology definition from the given RDF graph
        /// </summary>
        private static void InitializeOntology(RDFGraph ontGraph, Dictionary<string, RDFGraph> prefetchContext, out RDFOntology ontology)
        {
            //Expand with BASE ontology
            ontology = new RDFOntology(new RDFResource(ontGraph.Context.ToString()))
                            .UnionWith(RDFBASEOntology.Instance);
            ontology.Value = new RDFResource(ontGraph.Context.ToString());

            if (!prefetchContext[nameof(RDFVocabulary.RDF.TYPE)].ContainsTriple(new RDFTriple((RDFResource)ontology.Value, RDFVocabulary.RDF.TYPE, RDFVocabulary.OWL.ONTOLOGY)))
            {
                RDFTriple ontologyTriple = prefetchContext[nameof(RDFVocabulary.RDF.TYPE)].SelectTriplesByObject(RDFVocabulary.OWL.ONTOLOGY).FirstOrDefault();
                if (ontologyTriple != null)
                    ontology.Value = ontologyTriple.Subject;
            }
        }

        /// <summary>
        /// Parses the property model definitions from the given RDF graph
        /// </summary>
        private static void InitializePropertyModel(RDFOntology ontology, RDFGraph ontGraph, Dictionary<string, RDFGraph> prefetchContext)
        {
            #region Load RDF:Property
            foreach (RDFTriple typeProperty in prefetchContext[nameof(RDFVocabulary.RDF.TYPE)].SelectTriplesByObject(RDFVocabulary.RDF.PROPERTY))
                ontology.Model.PropertyModel.AddProperty(((RDFResource)typeProperty.Subject).ToRDFOntologyProperty());
            #endregion

            #region Load OWL:AnnotationProperty
            foreach (RDFTriple typeAnnotationProperty in prefetchContext[nameof(RDFVocabulary.RDF.TYPE)].SelectTriplesByObject(RDFVocabulary.OWL.ANNOTATION_PROPERTY))
            {
                RDFOntologyAnnotationProperty annotationProperty = ((RDFResource)typeAnnotationProperty.Subject).ToRDFOntologyAnnotationProperty();
                if (!ontology.Model.PropertyModel.Properties.ContainsKey(annotationProperty.PatternMemberID))
                    ontology.Model.PropertyModel.AddProperty(annotationProperty);
                else
                    ontology.Model.PropertyModel.Properties[annotationProperty.PatternMemberID] = annotationProperty;
            }
            #endregion

            #region Load OWL:DatatypeProperty
            foreach (RDFTriple typeDatatypeProperty in prefetchContext[nameof(RDFVocabulary.RDF.TYPE)].SelectTriplesByObject(RDFVocabulary.OWL.DATATYPE_PROPERTY))
            {
                RDFOntologyDatatypeProperty datatypeProperty = ((RDFResource)typeDatatypeProperty.Subject).ToRDFOntologyDatatypeProperty();
                if (!ontology.Model.PropertyModel.Properties.ContainsKey(datatypeProperty.PatternMemberID))
                    ontology.Model.PropertyModel.AddProperty(datatypeProperty);
                else
                    ontology.Model.PropertyModel.Properties[datatypeProperty.PatternMemberID] = datatypeProperty;

                #region DeprecatedProperty
                if (prefetchContext[nameof(RDFVocabulary.RDF.TYPE)].ContainsTriple(new RDFTriple((RDFResource)datatypeProperty.Value, RDFVocabulary.RDF.TYPE, RDFVocabulary.OWL.DEPRECATED_PROPERTY)))
                    datatypeProperty.SetDeprecated(true);
                #endregion DeprecatedProperty

                #region FunctionalProperty
                if (prefetchContext[nameof(RDFVocabulary.RDF.TYPE)].ContainsTriple(new RDFTriple((RDFResource)datatypeProperty.Value, RDFVocabulary.RDF.TYPE, RDFVocabulary.OWL.FUNCTIONAL_PROPERTY)))
                    datatypeProperty.SetFunctional(true);
                #endregion FunctionalProperty
            }
            #endregion

            #region Load OWL:ObjectProperty
            foreach (RDFTriple typeObjectProperty in prefetchContext[nameof(RDFVocabulary.RDF.TYPE)].SelectTriplesByObject(RDFVocabulary.OWL.OBJECT_PROPERTY))
            {
                RDFOntologyObjectProperty objectProperty = ((RDFResource)typeObjectProperty.Subject).ToRDFOntologyObjectProperty();
                if (!ontology.Model.PropertyModel.Properties.ContainsKey(objectProperty.PatternMemberID))
                    ontology.Model.PropertyModel.AddProperty(objectProperty);
                else
                    ontology.Model.PropertyModel.Properties[objectProperty.PatternMemberID] = objectProperty;

                #region DeprecatedProperty
                if (prefetchContext[nameof(RDFVocabulary.RDF.TYPE)].ContainsTriple(new RDFTriple((RDFResource)objectProperty.Value, RDFVocabulary.RDF.TYPE, RDFVocabulary.OWL.DEPRECATED_PROPERTY)))
                    objectProperty.SetDeprecated(true);
                #endregion

                #region FunctionalProperty
                if (prefetchContext[nameof(RDFVocabulary.RDF.TYPE)].ContainsTriple(new RDFTriple((RDFResource)objectProperty.Value, RDFVocabulary.RDF.TYPE, RDFVocabulary.OWL.FUNCTIONAL_PROPERTY)))
                    objectProperty.SetFunctional(true);
                #endregion

                #region SymmetricProperty
                if (prefetchContext[nameof(RDFVocabulary.RDF.TYPE)].ContainsTriple(new RDFTriple((RDFResource)objectProperty.Value, RDFVocabulary.RDF.TYPE, RDFVocabulary.OWL.SYMMETRIC_PROPERTY)))
                    objectProperty.SetSymmetric(true);
                #endregion

                #region AsymmetricProperty [OWL2]
                if (prefetchContext[nameof(RDFVocabulary.RDF.TYPE)].ContainsTriple(new RDFTriple((RDFResource)objectProperty.Value, RDFVocabulary.RDF.TYPE, RDFVocabulary.OWL.ASYMMETRIC_PROPERTY)))
                    objectProperty.SetAsymmetric(true);
                #endregion

                #region ReflexiveProperty [OWL2]
                if (prefetchContext[nameof(RDFVocabulary.RDF.TYPE)].ContainsTriple(new RDFTriple((RDFResource)objectProperty.Value, RDFVocabulary.RDF.TYPE, RDFVocabulary.OWL.REFLEXIVE_PROPERTY)))
                    objectProperty.SetReflexive(true);
                #endregion

                #region IrreflexiveProperty [OWL2]
                if (prefetchContext[nameof(RDFVocabulary.RDF.TYPE)].ContainsTriple(new RDFTriple((RDFResource)objectProperty.Value, RDFVocabulary.RDF.TYPE, RDFVocabulary.OWL.IRREFLEXIVE_PROPERTY)))
                    objectProperty.SetIrreflexive(true);
                #endregion

                #region TransitiveProperty
                if (prefetchContext[nameof(RDFVocabulary.RDF.TYPE)].ContainsTriple(new RDFTriple((RDFResource)objectProperty.Value, RDFVocabulary.RDF.TYPE, RDFVocabulary.OWL.TRANSITIVE_PROPERTY)))
                    objectProperty.SetTransitive(true);
                #endregion

                #region InverseFunctionalProperty
                if (prefetchContext[nameof(RDFVocabulary.RDF.TYPE)].ContainsTriple(new RDFTriple((RDFResource)objectProperty.Value, RDFVocabulary.RDF.TYPE, RDFVocabulary.OWL.INVERSE_FUNCTIONAL_PROPERTY)))
                    objectProperty.SetInverseFunctional(true);
                #endregion
            }

            #region SymmetricProperty
            foreach (RDFTriple sp in prefetchContext[nameof(RDFVocabulary.RDF.TYPE)].SelectTriplesByObject(RDFVocabulary.OWL.SYMMETRIC_PROPERTY))
            {
                RDFOntologyProperty syp = ontology.Model.PropertyModel.SelectProperty(sp.Subject.ToString());
                if (syp == null)
                {
                    syp = ((RDFResource)sp.Subject).ToRDFOntologyObjectProperty();
                    ontology.Model.PropertyModel.AddProperty(syp);

                    #region DeprecatedProperty
                    if (prefetchContext[nameof(RDFVocabulary.RDF.TYPE)].ContainsTriple(new RDFTriple((RDFResource)syp.Value, RDFVocabulary.RDF.TYPE, RDFVocabulary.OWL.DEPRECATED_PROPERTY)))
                        syp.SetDeprecated(true);
                    #endregion

                    #region FunctionalProperty
                    if (prefetchContext[nameof(RDFVocabulary.RDF.TYPE)].ContainsTriple(new RDFTriple((RDFResource)syp.Value, RDFVocabulary.RDF.TYPE, RDFVocabulary.OWL.FUNCTIONAL_PROPERTY)))
                        syp.SetFunctional(true);
                    #endregion
                }
                if (syp.IsObjectProperty())
                    ((RDFOntologyObjectProperty)syp).SetSymmetric(true);
            }
            #endregion

            #region AsymmetricProperty [OWL2]
            foreach (RDFTriple ap in prefetchContext[nameof(RDFVocabulary.RDF.TYPE)].SelectTriplesByObject(RDFVocabulary.OWL.ASYMMETRIC_PROPERTY))
            {
                RDFOntologyProperty asyp = ontology.Model.PropertyModel.SelectProperty(ap.Subject.ToString());
                if (asyp == null)
                {
                    asyp = ((RDFResource)ap.Subject).ToRDFOntologyObjectProperty();
                    ontology.Model.PropertyModel.AddProperty(asyp);

                    #region DeprecatedProperty
                    if (prefetchContext[nameof(RDFVocabulary.RDF.TYPE)].ContainsTriple(new RDFTriple((RDFResource)asyp.Value, RDFVocabulary.RDF.TYPE, RDFVocabulary.OWL.DEPRECATED_PROPERTY)))
                        asyp.SetDeprecated(true);
                    #endregion

                    #region FunctionalProperty
                    if (prefetchContext[nameof(RDFVocabulary.RDF.TYPE)].ContainsTriple(new RDFTriple((RDFResource)asyp.Value, RDFVocabulary.RDF.TYPE, RDFVocabulary.OWL.FUNCTIONAL_PROPERTY)))
                        asyp.SetFunctional(true);
                    #endregion
                }
                if (asyp.IsObjectProperty())
                    ((RDFOntologyObjectProperty)asyp).SetAsymmetric(true);
            }
            #endregion

            #region ReflexiveProperty [OWL2]
            foreach (RDFTriple rp in prefetchContext[nameof(RDFVocabulary.RDF.TYPE)].SelectTriplesByObject(RDFVocabulary.OWL.REFLEXIVE_PROPERTY))
            {
                RDFOntologyProperty refp = ontology.Model.PropertyModel.SelectProperty(rp.Subject.ToString());
                if (refp == null)
                {
                    refp = ((RDFResource)rp.Subject).ToRDFOntologyObjectProperty();
                    ontology.Model.PropertyModel.AddProperty(refp);

                    #region DeprecatedProperty
                    if (prefetchContext[nameof(RDFVocabulary.RDF.TYPE)].ContainsTriple(new RDFTriple((RDFResource)refp.Value, RDFVocabulary.RDF.TYPE, RDFVocabulary.OWL.DEPRECATED_PROPERTY)))
                        refp.SetDeprecated(true);
                    #endregion

                    #region FunctionalProperty
                    if (prefetchContext[nameof(RDFVocabulary.RDF.TYPE)].ContainsTriple(new RDFTriple((RDFResource)refp.Value, RDFVocabulary.RDF.TYPE, RDFVocabulary.OWL.FUNCTIONAL_PROPERTY)))
                        refp.SetFunctional(true);
                    #endregion
                }
                if (refp.IsObjectProperty())
                    ((RDFOntologyObjectProperty)refp).SetReflexive(true);
            }
            #endregion

            #region IrreflexiveProperty [OWL2]
            foreach (RDFTriple irp in prefetchContext[nameof(RDFVocabulary.RDF.TYPE)].SelectTriplesByObject(RDFVocabulary.OWL.IRREFLEXIVE_PROPERTY))
            {
                RDFOntologyProperty irrefp = ontology.Model.PropertyModel.SelectProperty(irp.Subject.ToString());
                if (irrefp == null)
                {
                    irrefp = ((RDFResource)irp.Subject).ToRDFOntologyObjectProperty();
                    ontology.Model.PropertyModel.AddProperty(irrefp);

                    #region DeprecatedProperty
                    if (prefetchContext[nameof(RDFVocabulary.RDF.TYPE)].ContainsTriple(new RDFTriple((RDFResource)irrefp.Value, RDFVocabulary.RDF.TYPE, RDFVocabulary.OWL.DEPRECATED_PROPERTY)))
                        irrefp.SetDeprecated(true);
                    #endregion

                    #region FunctionalProperty
                    if (prefetchContext[nameof(RDFVocabulary.RDF.TYPE)].ContainsTriple(new RDFTriple((RDFResource)irrefp.Value, RDFVocabulary.RDF.TYPE, RDFVocabulary.OWL.FUNCTIONAL_PROPERTY)))
                        irrefp.SetFunctional(true);
                    #endregion
                }
                if (irrefp.IsObjectProperty())
                    ((RDFOntologyObjectProperty)irrefp).SetIrreflexive(true);
            }
            #endregion

            #region TransitiveProperty
            foreach (RDFTriple tp in prefetchContext[nameof(RDFVocabulary.RDF.TYPE)].SelectTriplesByObject(RDFVocabulary.OWL.TRANSITIVE_PROPERTY))
            {
                RDFOntologyProperty trp = ontology.Model.PropertyModel.SelectProperty(tp.Subject.ToString());
                if (trp == null)
                {
                    trp = ((RDFResource)tp.Subject).ToRDFOntologyObjectProperty();
                    ontology.Model.PropertyModel.AddProperty(trp);

                    #region DeprecatedProperty
                    if (prefetchContext[nameof(RDFVocabulary.RDF.TYPE)].ContainsTriple(new RDFTriple((RDFResource)trp.Value, RDFVocabulary.RDF.TYPE, RDFVocabulary.OWL.DEPRECATED_PROPERTY)))
                        trp.SetDeprecated(true);
                    #endregion

                    #region FunctionalProperty
                    if (prefetchContext[nameof(RDFVocabulary.RDF.TYPE)].ContainsTriple(new RDFTriple((RDFResource)trp.Value, RDFVocabulary.RDF.TYPE, RDFVocabulary.OWL.FUNCTIONAL_PROPERTY)))
                        trp.SetFunctional(true);
                    #endregion
                }
                if (trp.IsObjectProperty())
                    ((RDFOntologyObjectProperty)trp).SetTransitive(true);
            }
            #endregion

            #region InverseFunctionalProperty
            foreach (RDFTriple ip in prefetchContext[nameof(RDFVocabulary.RDF.TYPE)].SelectTriplesByObject(RDFVocabulary.OWL.INVERSE_FUNCTIONAL_PROPERTY))
            {
                RDFOntologyProperty ifp = ontology.Model.PropertyModel.SelectProperty(ip.Subject.ToString());
                if (ifp == null)
                {
                    ifp = ((RDFResource)ip.Subject).ToRDFOntologyObjectProperty();
                    ontology.Model.PropertyModel.AddProperty(ifp);

                    #region DeprecatedProperty
                    if (prefetchContext[nameof(RDFVocabulary.RDF.TYPE)].ContainsTriple(new RDFTriple((RDFResource)ifp.Value, RDFVocabulary.RDF.TYPE, RDFVocabulary.OWL.DEPRECATED_PROPERTY)))
                        ifp.SetDeprecated(true);
                    #endregion

                    #region FunctionalProperty
                    if (prefetchContext[nameof(RDFVocabulary.RDF.TYPE)].ContainsTriple(new RDFTriple((RDFResource)ifp.Value, RDFVocabulary.RDF.TYPE, RDFVocabulary.OWL.FUNCTIONAL_PROPERTY)))
                        ifp.SetFunctional(true);
                    #endregion
                }
                if (ifp.IsObjectProperty())
                    ((RDFOntologyObjectProperty)ifp).SetInverseFunctional(true);
            }
            #endregion

            #endregion
        }

        /// <summary>
        /// Parses the class model definitions from the given RDF graph
        /// </summary>
        private static void InitializeClassModel(RDFOntology ontology, RDFGraph ontGraph, Dictionary<string, RDFGraph> prefetchContext)
        {
            #region Load RDFS:Class
            foreach (RDFTriple typeRDFSClass in prefetchContext[nameof(RDFVocabulary.RDF.TYPE)].SelectTriplesByObject(RDFVocabulary.RDFS.CLASS))
            {
                RDFOntologyClass rdfsClass = ((RDFResource)typeRDFSClass.Subject).ToRDFOntologyClass(RDFSemanticsEnums.RDFOntologyClassNature.RDFS);
                ontology.Model.ClassModel.AddClass(rdfsClass);

                #region DeprecatedClass
                if (prefetchContext[nameof(RDFVocabulary.RDF.TYPE)].ContainsTriple(new RDFTriple((RDFResource)rdfsClass.Value, RDFVocabulary.RDF.TYPE, RDFVocabulary.OWL.DEPRECATED_CLASS)))
                    rdfsClass.SetDeprecated(true);
                #endregion
            }
            #endregion

            #region Load OWL:DataRange
            foreach (RDFTriple typeOWLDataRange in prefetchContext[nameof(RDFVocabulary.RDF.TYPE)].SelectTriplesByObject(RDFVocabulary.OWL.DATA_RANGE))
            {
                RDFOntologyDataRangeClass datarangeClass = new RDFOntologyDataRangeClass((RDFResource)typeOWLDataRange.Subject);
                if (!ontology.Model.ClassModel.Classes.ContainsKey(datarangeClass.PatternMemberID))
                    ontology.Model.ClassModel.AddClass(datarangeClass);
                else
                    ontology.Model.ClassModel.Classes[datarangeClass.PatternMemberID] = datarangeClass;
            }
            #endregion

            #region Load OWL:Class
            foreach (RDFTriple typeOWLClass in prefetchContext[nameof(RDFVocabulary.RDF.TYPE)].SelectTriplesByObject(RDFVocabulary.OWL.CLASS))
            {
                RDFOntologyClass owlClass = ((RDFResource)typeOWLClass.Subject).ToRDFOntologyClass();
                if (!ontology.Model.ClassModel.Classes.ContainsKey(owlClass.PatternMemberID))
                    ontology.Model.ClassModel.AddClass(owlClass);
                else
                    ontology.Model.ClassModel.Classes[owlClass.PatternMemberID] = owlClass;

                #region DeprecatedClass
                if (prefetchContext[nameof(RDFVocabulary.RDF.TYPE)].ContainsTriple(new RDFTriple((RDFResource)owlClass.Value, RDFVocabulary.RDF.TYPE, RDFVocabulary.OWL.DEPRECATED_CLASS)))
                    owlClass.SetDeprecated(true);
                #endregion
            }
            #endregion

            #region Load OWL:DeprecatedClass
            foreach (RDFTriple typeOWLDeprecatedClass in prefetchContext[nameof(RDFVocabulary.RDF.TYPE)].SelectTriplesByObject(RDFVocabulary.OWL.DEPRECATED_CLASS))
            {
                RDFOntologyClass deprecatedClass = ((RDFResource)typeOWLDeprecatedClass.Subject).ToRDFOntologyClass().SetDeprecated(true);
                if (!ontology.Model.ClassModel.Classes.ContainsKey(deprecatedClass.PatternMemberID))
                    ontology.Model.ClassModel.AddClass(deprecatedClass);
                else
                    ontology.Model.ClassModel.Classes[deprecatedClass.PatternMemberID].SetDeprecated(true);
            }
            #endregion

            #region Load OWL:Restriction
            foreach (RDFTriple typeOWLRestriction in prefetchContext[nameof(RDFVocabulary.RDF.TYPE)].SelectTriplesByObject(RDFVocabulary.OWL.RESTRICTION))
            {
                #region OnProperty
                RDFTriple restrictionOnProperty = prefetchContext[nameof(RDFVocabulary.OWL.ON_PROPERTY)].SelectTriplesBySubject((RDFResource)typeOWLRestriction.Subject).FirstOrDefault();
                if (restrictionOnProperty != null)
                {
                    RDFOntologyProperty onProperty = ontology.Model.PropertyModel.SelectProperty(restrictionOnProperty.Object.ToString());
                    if (onProperty != null)
                    {
                        //Ensure to not create a restriction over an annotation property (or a BASE reserved property)
                        if (!onProperty.IsAnnotationProperty() && !RDFOntologyChecker.CheckReservedProperty(onProperty))
                        {
                            RDFOntologyRestriction restrictionClass = new RDFOntologyRestriction((RDFResource)typeOWLRestriction.Subject, onProperty);
                            if (!ontology.Model.ClassModel.Classes.ContainsKey(restrictionClass.PatternMemberID))
                                ontology.Model.ClassModel.AddClass(restrictionClass);
                            else
                                ontology.Model.ClassModel.Classes[restrictionClass.PatternMemberID] = restrictionClass;
                        }
                        else
                            RDFSemanticsEvents.RaiseSemanticsWarning(string.Format("Restriction '{0}' cannot be imported from graph, because its applied property '{1}' represents an annotation property or is a reserved BASE property.", typeOWLRestriction.Subject, restrictionOnProperty.Object));
                    }
                    else
                        RDFSemanticsEvents.RaiseSemanticsWarning(string.Format("Restriction '{0}' cannot be imported from graph, because definition of its applied property '{1}' is not found in the model.", typeOWLRestriction.Subject, restrictionOnProperty.Object));
                }
                else
                    RDFSemanticsEvents.RaiseSemanticsWarning(string.Format("Restriction '{0}' cannot be imported from graph, because owl:OnProperty triple is not found in the graph.", typeOWLRestriction.Subject));
                #endregion
            }
            #endregion

            #region Load OWL:[UnionOf|DisjointUnionOf|IntersectionOf|ComplementOf]

            #region UnionOf
            foreach (RDFTriple unionOf in prefetchContext[nameof(RDFVocabulary.OWL.UNION_OF)].Where(t => t.TripleFlavor == RDFModelEnums.RDFTripleFlavors.SPO))
            {
                #region Initialize
                RDFOntologyClass unionOfClass = ontology.Model.ClassModel.SelectClass(unionOf.Subject.ToString());
                if (unionOfClass == null)
                {
                    unionOfClass = new RDFOntologyClass(new RDFResource(unionOf.Subject.ToString()));
                    ontology.Model.ClassModel.AddClass(unionOfClass);
                }
                #endregion

                #region ClassToUnionClass
                if (!(unionOfClass is RDFOntologyUnionClass))
                {
                    unionOfClass = new RDFOntologyUnionClass((RDFResource)unionOf.Subject);
                    ontology.Model.ClassModel.Classes[unionOfClass.PatternMemberID] = unionOfClass;
                }
                #endregion

                #region DeserializeUnionCollection
                bool nilFound = false;
                RDFResource itemRest = (RDFResource)unionOf.Object;
                HashSet<long> itemRestVisitCache = new HashSet<long>() { itemRest.PatternMemberID };
                while (!nilFound)
                {
                    #region rdf:first
                    RDFTriple first = prefetchContext[nameof(RDFVocabulary.RDF.FIRST)].SelectTriplesBySubject(itemRest).FirstOrDefault();
                    if (first != null && first.TripleFlavor == RDFModelEnums.RDFTripleFlavors.SPO)
                    {
                        RDFOntologyClass compClass = ontology.Model.ClassModel.SelectClass(first.Object.ToString());
                        if (compClass != null)
                            ontology.Model.ClassModel.AddUnionOfRelation((RDFOntologyUnionClass)unionOfClass, new List<RDFOntologyClass>() { compClass });
                        else
                            RDFSemanticsEvents.RaiseSemanticsWarning(string.Format("UnionClass '{0}' cannot be completely imported from graph, because definition of its compositing class '{1}' is not found in the model.", unionOf.Subject, first.Object));

                        #region rdf:rest
                        RDFTriple rest = prefetchContext[nameof(RDFVocabulary.RDF.REST)].SelectTriplesBySubject(itemRest).FirstOrDefault();
                        if (rest != null)
                        {
                            if (rest.Object.Equals(RDFVocabulary.RDF.NIL))
                                nilFound = true;
                            else
                            {
                                itemRest = (RDFResource)rest.Object;
                                //Avoid bad-formed cyclic lists to generate infinite loops
                                if (!itemRestVisitCache.Contains(itemRest.PatternMemberID))
                                    itemRestVisitCache.Add(itemRest.PatternMemberID);
                                else
                                    nilFound = true;
                            }
                        }
                        else
                            nilFound = true;
                        #endregion
                    }
                    else
                        nilFound = true;
                    #endregion
                }
                #endregion
            }
            #endregion

            #region DisjointUnion [OWL2]
            foreach (RDFTriple disjointUnion in prefetchContext[nameof(RDFVocabulary.OWL.DISJOINT_UNION_OF)].Where(t => t.TripleFlavor == RDFModelEnums.RDFTripleFlavors.SPO))
            {
                #region Initialize
                RDFOntologyClass disjointUnionClass = ontology.Model.ClassModel.SelectClass(disjointUnion.Subject.ToString());
                if (disjointUnionClass == null)
                {
                    disjointUnionClass = new RDFOntologyClass(new RDFResource(disjointUnion.Subject.ToString()));
                    ontology.Model.ClassModel.AddClass(disjointUnionClass);
                }
                #endregion

                #region ClassToUnionClass
                if (!(disjointUnionClass is RDFOntologyUnionClass))
                {
                    disjointUnionClass = new RDFOntologyUnionClass((RDFResource)disjointUnion.Subject);
                    ontology.Model.ClassModel.Classes[disjointUnionClass.PatternMemberID] = disjointUnionClass;
                }
                #endregion

                #region DeserializeUnionCollection
                bool nilFound = false;
                RDFResource itemRest = (RDFResource)disjointUnion.Object;
                HashSet<long> itemRestVisitCache = new HashSet<long>() { itemRest.PatternMemberID };
                List<RDFOntologyClass> disjointClasses = new List<RDFOntologyClass>();
                while (!nilFound)
                {
                    #region rdf:first
                    RDFTriple first = prefetchContext[nameof(RDFVocabulary.RDF.FIRST)].SelectTriplesBySubject(itemRest).FirstOrDefault();
                    if (first != null && first.TripleFlavor == RDFModelEnums.RDFTripleFlavors.SPO)
                    {
                        RDFOntologyClass compClass = ontology.Model.ClassModel.SelectClass(first.Object.ToString());
                        if (compClass != null)
                        {
                            ontology.Model.ClassModel.AddUnionOfRelation((RDFOntologyUnionClass)disjointUnionClass, new List<RDFOntologyClass>() { compClass });
                            disjointClasses.Add(compClass);
                        }
                        else
                            RDFSemanticsEvents.RaiseSemanticsWarning(string.Format("UnionClass '{0}' cannot be completely imported from graph, because definition of its compositing class '{1}' is not found in the model.", disjointUnion.Subject, first.Object));

                        #region rdf:rest
                        RDFTriple rest = prefetchContext[nameof(RDFVocabulary.RDF.REST)].SelectTriplesBySubject(itemRest).FirstOrDefault();
                        if (rest != null)
                        {
                            if (rest.Object.Equals(RDFVocabulary.RDF.NIL))
                                nilFound = true;
                            else
                            {
                                itemRest = (RDFResource)rest.Object;
                                //Avoid bad-formed cyclic lists to generate infinite loops
                                if (!itemRestVisitCache.Contains(itemRest.PatternMemberID))
                                    itemRestVisitCache.Add(itemRest.PatternMemberID);
                                else
                                    nilFound = true;
                            }
                        }
                        else
                            nilFound = true;
                        #endregion
                    }
                    else
                        nilFound = true;
                    #endregion
                }
                #endregion

                #region DisjointClasses
                disjointClasses.ForEach(outerClass =>
                    disjointClasses.ForEach(innerClass => ontology.Model.ClassModel.AddDisjointWithRelation(outerClass, innerClass)));
                #endregion
            }
            #endregion

            #region IntersectionOf
            foreach (RDFTriple intersectionOf in prefetchContext[nameof(RDFVocabulary.OWL.INTERSECTION_OF)].Where(t => t.TripleFlavor == RDFModelEnums.RDFTripleFlavors.SPO))
            {
                #region Initialize
                RDFOntologyClass intersectionOfClass = ontology.Model.ClassModel.SelectClass(intersectionOf.Subject.ToString());
                if (intersectionOfClass == null)
                {
                    intersectionOfClass = new RDFOntologyClass(new RDFResource(intersectionOf.Subject.ToString()));
                    ontology.Model.ClassModel.AddClass(intersectionOfClass);
                }
                #endregion

                #region ClassToIntersectionClass
                if (!(intersectionOfClass is RDFOntologyIntersectionClass))
                {
                    intersectionOfClass = new RDFOntologyIntersectionClass((RDFResource)intersectionOf.Subject);
                    ontology.Model.ClassModel.Classes[intersectionOfClass.PatternMemberID] = intersectionOfClass;
                }
                #endregion

                #region DeserializeIntersectionCollection
                bool nilFound = false;
                RDFResource itemRest = (RDFResource)intersectionOf.Object;
                HashSet<long> itemRestVisitCache = new HashSet<long>() { itemRest.PatternMemberID };
                while (!nilFound)
                {
                    #region rdf:first
                    RDFTriple first = prefetchContext[nameof(RDFVocabulary.RDF.FIRST)].SelectTriplesBySubject(itemRest).FirstOrDefault();
                    if (first != null && first.TripleFlavor == RDFModelEnums.RDFTripleFlavors.SPO)
                    {
                        RDFOntologyClass compClass = ontology.Model.ClassModel.SelectClass(first.Object.ToString());
                        if (compClass != null)
                            ontology.Model.ClassModel.AddIntersectionOfRelation((RDFOntologyIntersectionClass)intersectionOfClass, new List<RDFOntologyClass>() { compClass });
                        else
                            RDFSemanticsEvents.RaiseSemanticsWarning(string.Format("IntersectionClass '{0}' cannot be completely imported from graph, because definition of its compositing class '{1}' is not found in the model.", intersectionOf.Subject, first.Object));

                        #region rdf:rest
                        RDFTriple rest = prefetchContext[nameof(RDFVocabulary.RDF.REST)].SelectTriplesBySubject(itemRest).FirstOrDefault();
                        if (rest != null)
                        {
                            if (rest.Object.Equals(RDFVocabulary.RDF.NIL))
                                nilFound = true;
                            else
                            {
                                itemRest = (RDFResource)rest.Object;
                                //Avoid bad-formed cyclic lists to generate infinite loops
                                if (!itemRestVisitCache.Contains(itemRest.PatternMemberID))
                                    itemRestVisitCache.Add(itemRest.PatternMemberID);
                                else
                                    nilFound = true;
                            }
                        }
                        else
                            nilFound = true;
                        #endregion
                    }
                    else
                        nilFound = true;
                    #endregion
                }
                #endregion
            }
            #endregion

            #region ComplementOf
            foreach (RDFTriple complementOf in prefetchContext[nameof(RDFVocabulary.OWL.COMPLEMENT_OF)].Where(t => t.TripleFlavor == RDFModelEnums.RDFTripleFlavors.SPO))
            {
                RDFOntologyClass complementOfClass = ontology.Model.ClassModel.SelectClass(complementOf.Subject.ToString());
                if (complementOfClass != null)
                {
                    RDFOntologyClass complementedClass = ontology.Model.ClassModel.SelectClass(complementOf.Object.ToString());
                    if (complementedClass != null)
                    {
                        complementOfClass = new RDFOntologyComplementClass((RDFResource)complementOf.Subject, complementedClass);
                        ontology.Model.ClassModel.Classes[complementOfClass.PatternMemberID] = complementOfClass;
                    }
                    else
                        RDFSemanticsEvents.RaiseSemanticsWarning(string.Format("Class '{0}' cannot be imported from graph, because definition of its complement class '{1}' is not found in the model.", complementOf.Subject, complementOf.Object));
                }
                else
                    RDFSemanticsEvents.RaiseSemanticsWarning(string.Format("Class '{0}' cannot be imported from graph, because its definition is not found in the model.", complementOf.Subject));
            }
            #endregion

            #endregion
        }

        /// <summary>
        /// Parses the data definitions from the given RDF graph
        /// </summary>
        private static void InitializeData(RDFOntology ontology, RDFGraph ontGraph, Dictionary<string, RDFGraph> prefetchContext)
        {
            RDFOntologyObjectProperty rdfTypeProperty = RDFVocabulary.RDF.TYPE.ToRDFOntologyObjectProperty();

            //Detect named individuals [OWL2]
            RDFOntologyClass namedIndividualClass = RDFVocabulary.OWL.NAMED_INDIVIDUAL.ToRDFOntologyClass();
            foreach (RDFTriple namedIndividual in prefetchContext[nameof(RDFVocabulary.OWL.NAMED_INDIVIDUAL)].Where(ni => !((RDFResource)ni.Subject).IsBlank))
            {
                RDFOntologyIndividual individual = ontology.Data.SelectIndividual(namedIndividual.Subject.ToString());
                if (individual == null)
                {
                    individual = ((RDFResource)namedIndividual.Subject).ToRDFOntologyIndividual();
                    ontology.Data.AddIndividual(individual);
                }
                ontology.Data.Relations.ClassType.AddEntry(new RDFOntologyTaxonomyEntry(individual, rdfTypeProperty, namedIndividualClass));
            }

            //Detect typed individuals
            List<RDFOntologyClass> simpleClasses = ontology.Model.ClassModel.Where(cls => !RDFOntologyChecker.CheckReservedClass(cls) && cls.IsSimpleClass()).ToList();
            foreach (RDFOntologyClass simpleClass in simpleClasses)
            {
                foreach (RDFTriple classType in prefetchContext[nameof(RDFVocabulary.RDF.TYPE)].SelectTriplesByObject((RDFResource)simpleClass.Value))
                {
                    RDFOntologyIndividual individual = ontology.Data.SelectIndividual(classType.Subject.ToString());
                    if (individual == null)
                        individual = ((RDFResource)classType.Subject).ToRDFOntologyIndividual();

                    ontology.Data.AddClassTypeRelation(individual, simpleClass);
                }
            }
        }

        /// <summary>
        /// Finalizes the definition of the ontology restrictions previously detected
        /// </summary>
        private static void FinalizeRestrictions(RDFOntology ontology, RDFGraph ontGraph, Dictionary<string, RDFGraph> prefetchContext)
        {
            foreach (RDFOntologyRestriction restriction in ontology.Model.ClassModel.OfType<RDFOntologyRestriction>().ToList())
            {
                #region Cardinality
                int cardinality = 0;
                RDFTriple restrictionCardinality = prefetchContext[nameof(RDFVocabulary.OWL.CARDINALITY)].SelectTriplesBySubject((RDFResource)restriction.Value).FirstOrDefault();
                if (restrictionCardinality != null && restrictionCardinality.TripleFlavor == RDFModelEnums.RDFTripleFlavors.SPL)
                {
                    if (restrictionCardinality.Object is RDFPlainLiteral)
                    {
                        if (NumberRegex.Value.IsMatch(restrictionCardinality.Object.ToString()))
                            cardinality = int.Parse(restrictionCardinality.Object.ToString());
                    }
                    else
                    {
                        if (((RDFTypedLiteral)restrictionCardinality.Object).HasDecimalDatatype())
                        {
                            if (NumberRegex.Value.IsMatch(((RDFTypedLiteral)restrictionCardinality.Object).Value))
                                cardinality = int.Parse(((RDFTypedLiteral)restrictionCardinality.Object).Value);
                        }
                    }
                }

                if (cardinality > 0)
                {
                    RDFOntologyCardinalityRestriction cardinalityRestriction = new RDFOntologyCardinalityRestriction((RDFResource)restriction.Value, ((RDFOntologyRestriction)restriction).OnProperty, cardinality, cardinality);
                    ontology.Model.ClassModel.Classes[restriction.PatternMemberID] = cardinalityRestriction;
                    continue; //Restriction has been successfully typed
                }
                #endregion

                #region MinMaxCardinality
                int minCardinality = 0;
                RDFTriple restrictionMinCardinality = prefetchContext[nameof(RDFVocabulary.OWL.MIN_CARDINALITY)].SelectTriplesBySubject((RDFResource)restriction.Value).FirstOrDefault();
                if (restrictionMinCardinality != null && restrictionMinCardinality.TripleFlavor == RDFModelEnums.RDFTripleFlavors.SPL)
                {
                    if (restrictionMinCardinality.Object is RDFPlainLiteral)
                    {
                        if (NumberRegex.Value.IsMatch(restrictionMinCardinality.Object.ToString()))
                            minCardinality = int.Parse(restrictionMinCardinality.Object.ToString());
                    }
                    else
                    {
                        if (((RDFTypedLiteral)restrictionMinCardinality.Object).HasDecimalDatatype())
                        {
                            if (NumberRegex.Value.IsMatch(((RDFTypedLiteral)restrictionMinCardinality.Object).Value))
                                minCardinality = int.Parse(((RDFTypedLiteral)restrictionMinCardinality.Object).Value);
                        }
                    }
                }

                int maxCardinality = 0;
                RDFTriple restrictionMaxCardinality = prefetchContext[nameof(RDFVocabulary.OWL.MAX_CARDINALITY)].SelectTriplesBySubject((RDFResource)restriction.Value).FirstOrDefault();
                if (restrictionMaxCardinality != null && restrictionMaxCardinality.TripleFlavor == RDFModelEnums.RDFTripleFlavors.SPL)
                {
                    if (restrictionMaxCardinality.Object is RDFPlainLiteral)
                    {
                        if (NumberRegex.Value.IsMatch(restrictionMaxCardinality.Object.ToString()))
                            maxCardinality = int.Parse(restrictionMaxCardinality.Object.ToString());
                    }
                    else
                    {
                        if (((RDFTypedLiteral)restrictionMaxCardinality.Object).HasDecimalDatatype())
                        {
                            if (NumberRegex.Value.IsMatch(((RDFTypedLiteral)restrictionMaxCardinality.Object).Value))
                                maxCardinality = int.Parse(((RDFTypedLiteral)restrictionMaxCardinality.Object).Value);
                        }
                    }
                }

                if (minCardinality > 0 || maxCardinality > 0)
                {
                    RDFOntologyCardinalityRestriction minmaxCardinalityRestriction = new RDFOntologyCardinalityRestriction((RDFResource)restriction.Value, ((RDFOntologyRestriction)restriction).OnProperty, minCardinality, maxCardinality);
                    ontology.Model.ClassModel.Classes[restriction.PatternMemberID] = minmaxCardinalityRestriction;
                    continue; //Restriction has been successfully typed
                }
                #endregion

                #region QualifiedCardinality [OWL2]
                int qualifiedCardinality = 0;
                RDFTriple restrictionQualifiedCardinality = prefetchContext[nameof(RDFVocabulary.OWL.QUALIFIED_CARDINALITY)].SelectTriplesBySubject((RDFResource)restriction.Value).FirstOrDefault();
                if (restrictionQualifiedCardinality != null && restrictionQualifiedCardinality.TripleFlavor == RDFModelEnums.RDFTripleFlavors.SPL)
                {
                    if (restrictionQualifiedCardinality.Object is RDFPlainLiteral)
                    {
                        if (NumberRegex.Value.IsMatch(restrictionQualifiedCardinality.Object.ToString()))
                            qualifiedCardinality = int.Parse(restrictionQualifiedCardinality.Object.ToString());
                    }
                    else
                    {
                        if (((RDFTypedLiteral)restrictionQualifiedCardinality.Object).HasDecimalDatatype())
                        {
                            if (NumberRegex.Value.IsMatch(((RDFTypedLiteral)restrictionQualifiedCardinality.Object).Value))
                                qualifiedCardinality = int.Parse(((RDFTypedLiteral)restrictionQualifiedCardinality.Object).Value);
                        }
                    }
                }

                if (qualifiedCardinality > 0)
                {
                    //OnClass
                    RDFTriple restrictionOnClass = prefetchContext[nameof(RDFVocabulary.OWL.ON_CLASS)].SelectTriplesBySubject((RDFResource)restriction.Value).FirstOrDefault();
                    if (restrictionOnClass != null && restrictionOnClass.TripleFlavor == RDFModelEnums.RDFTripleFlavors.SPO)
                    {
                        RDFOntologyClass onClass = ontology.Model.ClassModel.SelectClass(restrictionOnClass.Object.ToString());
                        if (onClass != null)
                        {
                            RDFOntologyQualifiedCardinalityRestriction qualifiedCardinalityRestriction = new RDFOntologyQualifiedCardinalityRestriction((RDFResource)restriction.Value, ((RDFOntologyRestriction)restriction).OnProperty, onClass, qualifiedCardinality, qualifiedCardinality);
                            ontology.Model.ClassModel.Classes[restriction.PatternMemberID] = qualifiedCardinalityRestriction;
                            continue; //Restriction has been successfully typed
                        }
                        else
                            RDFSemanticsEvents.RaiseSemanticsWarning(string.Format("QualifiedCardinalityRestriction '{0}' cannot be imported from graph, because definition of its required onClass '{1}' is not found in the model.", restriction.Value, restrictionOnClass.Object));
                    }
                    else
                    {
                        //OnDataRange
                        RDFTriple restrictionOnDataRange = prefetchContext[nameof(RDFVocabulary.OWL.ON_DATARANGE)].SelectTriplesBySubject((RDFResource)restriction.Value).FirstOrDefault();
                        if (restrictionOnDataRange != null && restrictionOnDataRange.TripleFlavor == RDFModelEnums.RDFTripleFlavors.SPO)
                        {
                            RDFOntologyClass onDataRange = ontology.Model.ClassModel.SelectClass(restrictionOnDataRange.Object.ToString());
                            if (onDataRange != null)
                            {
                                RDFOntologyQualifiedCardinalityRestriction qualifiedCardinalityRestriction = new RDFOntologyQualifiedCardinalityRestriction((RDFResource)restriction.Value, ((RDFOntologyRestriction)restriction).OnProperty, onDataRange, qualifiedCardinality, qualifiedCardinality);
                                ontology.Model.ClassModel.Classes[restriction.PatternMemberID] = qualifiedCardinalityRestriction;
                                continue; //Restriction has been successfully typed
                            }
                            else
                                RDFSemanticsEvents.RaiseSemanticsWarning(string.Format("QualifiedCardinalityRestriction '{0}' cannot be imported from graph, because definition of its required onDataRange '{1}' is not found in the model.", restriction.Value, restrictionOnDataRange.Object));
                        }
                    }
                }
                #endregion

                #region MinMaxQualifiedCardinality [OWL2]
                int minQualifiedCardinality = 0;
                RDFTriple restrictionMinQualifiedCardinality = prefetchContext[nameof(RDFVocabulary.OWL.MIN_QUALIFIED_CARDINALITY)].SelectTriplesBySubject((RDFResource)restriction.Value).FirstOrDefault();
                if (restrictionMinQualifiedCardinality != null && restrictionMinQualifiedCardinality.TripleFlavor == RDFModelEnums.RDFTripleFlavors.SPL)
                {
                    if (restrictionMinQualifiedCardinality.Object is RDFPlainLiteral)
                    {
                        if (NumberRegex.Value.IsMatch(restrictionMinQualifiedCardinality.Object.ToString()))
                            minQualifiedCardinality = int.Parse(restrictionMinQualifiedCardinality.Object.ToString());
                    }
                    else
                    {
                        if (((RDFTypedLiteral)restrictionMinQualifiedCardinality.Object).HasDecimalDatatype())
                        {
                            if (NumberRegex.Value.IsMatch(((RDFTypedLiteral)restrictionMinQualifiedCardinality.Object).Value))
                                minQualifiedCardinality = int.Parse(((RDFTypedLiteral)restrictionMinQualifiedCardinality.Object).Value);
                        }
                    }
                }

                int maxQualifiedCardinality = 0;
                RDFTriple restrictionMaxQualifiedCardinality = prefetchContext[nameof(RDFVocabulary.OWL.MAX_QUALIFIED_CARDINALITY)].SelectTriplesBySubject((RDFResource)restriction.Value).FirstOrDefault();
                if (restrictionMaxQualifiedCardinality != null && restrictionMaxQualifiedCardinality.TripleFlavor == RDFModelEnums.RDFTripleFlavors.SPL)
                {
                    if (restrictionMaxQualifiedCardinality.Object is RDFPlainLiteral)
                    {
                        if (NumberRegex.Value.IsMatch(restrictionMaxQualifiedCardinality.Object.ToString()))
                            maxQualifiedCardinality = int.Parse(restrictionMaxQualifiedCardinality.Object.ToString());
                    }
                    else
                    {
                        if (((RDFTypedLiteral)restrictionMaxQualifiedCardinality.Object).HasDecimalDatatype())
                        {
                            if (NumberRegex.Value.IsMatch(((RDFTypedLiteral)restrictionMaxQualifiedCardinality.Object).Value))
                                maxQualifiedCardinality = int.Parse(((RDFTypedLiteral)restrictionMaxQualifiedCardinality.Object).Value);
                        }
                    }
                }

                if (minQualifiedCardinality > 0 || maxQualifiedCardinality > 0)
                {
                    //OnClass
                    RDFTriple restrictionOnClass = prefetchContext[nameof(RDFVocabulary.OWL.ON_CLASS)].SelectTriplesBySubject((RDFResource)restriction.Value).FirstOrDefault();
                    if (restrictionOnClass != null && restrictionOnClass.TripleFlavor == RDFModelEnums.RDFTripleFlavors.SPO)
                    {
                        RDFOntologyClass onClass = ontology.Model.ClassModel.SelectClass(restrictionOnClass.Object.ToString());
                        if (onClass != null)
                        {
                            RDFOntologyQualifiedCardinalityRestriction qualifiedCardinalityRestriction = new RDFOntologyQualifiedCardinalityRestriction((RDFResource)restriction.Value, ((RDFOntologyRestriction)restriction).OnProperty, onClass, minQualifiedCardinality, maxQualifiedCardinality);
                            ontology.Model.ClassModel.Classes[restriction.PatternMemberID] = qualifiedCardinalityRestriction;
                            continue; //Restriction has been successfully typed
                        }
                        else
                            RDFSemanticsEvents.RaiseSemanticsWarning(string.Format("QualifiedCardinalityRestriction '{0}' cannot be imported from graph, because definition of its required onClass '{1}' is not found in the model.", restriction.Value, restrictionOnClass.Object));
                    }
                    else
                    {
                        //OnDataRange
                        RDFTriple restrictionOnDataRange = prefetchContext[nameof(RDFVocabulary.OWL.ON_DATARANGE)].SelectTriplesBySubject((RDFResource)restriction.Value).FirstOrDefault();
                        if (restrictionOnDataRange != null && restrictionOnDataRange.TripleFlavor == RDFModelEnums.RDFTripleFlavors.SPO)
                        {
                            RDFOntologyClass onDataRange = ontology.Model.ClassModel.SelectClass(restrictionOnDataRange.Object.ToString());
                            if (onDataRange != null)
                            {
                                RDFOntologyQualifiedCardinalityRestriction qualifiedCardinalityRestriction = new RDFOntologyQualifiedCardinalityRestriction((RDFResource)restriction.Value, ((RDFOntologyRestriction)restriction).OnProperty, onDataRange, minQualifiedCardinality, maxQualifiedCardinality);
                                ontology.Model.ClassModel.Classes[restriction.PatternMemberID] = qualifiedCardinalityRestriction;
                                continue; //Restriction has been successfully typed
                            }
                            else
                                RDFSemanticsEvents.RaiseSemanticsWarning(string.Format("QualifiedCardinalityRestriction '{0}' cannot be imported from graph, because definition of its required onDataRange '{1}' is not found in the model.", restriction.Value, restrictionOnDataRange.Object));
                        }
                    }
                }
                #endregion

                #region HasSelf [OWL2]
                RDFTriple hasSelf = prefetchContext[nameof(RDFVocabulary.OWL.HAS_SELF)].SelectTriplesBySubject((RDFResource)restriction.Value).FirstOrDefault();
                if (hasSelf?.Object.Equals(RDFTypedLiteral.True) ?? false)
                {
                    RDFOntologyHasSelfRestriction hasSelfRestriction = new RDFOntologyHasSelfRestriction((RDFResource)restriction.Value, restriction.OnProperty);
                    ontology.Model.ClassModel.Classes[restriction.PatternMemberID] = hasSelfRestriction;
                    continue; //Restriction has been successfully typed
                }
                #endregion

                #region HasValue
                RDFTriple hasValue = prefetchContext[nameof(RDFVocabulary.OWL.HAS_VALUE)].SelectTriplesBySubject((RDFResource)restriction.Value).FirstOrDefault();
                if (hasValue != null)
                {
                    if (hasValue.TripleFlavor == RDFModelEnums.RDFTripleFlavors.SPO)
                    {
                        RDFOntologyHasValueRestriction hasValueRestriction = new RDFOntologyHasValueRestriction((RDFResource)restriction.Value, restriction.OnProperty, ((RDFResource)hasValue.Object).ToRDFOntologyIndividual());
                        ontology.Model.ClassModel.Classes[restriction.PatternMemberID] = hasValueRestriction;
                        continue; //Restriction has been successfully typed
                    }
                    else
                    {
                        RDFOntologyHasValueRestriction hasValueRestriction = new RDFOntologyHasValueRestriction((RDFResource)restriction.Value, restriction.OnProperty, ((RDFLiteral)hasValue.Object).ToRDFOntologyLiteral());
                        ontology.Model.ClassModel.Classes[restriction.PatternMemberID] = hasValueRestriction;
                        continue; //Restriction has been successfully typed
                    }
                }
                #endregion

                #region AllValuesFrom
                RDFTriple allValuesFrom = prefetchContext[nameof(RDFVocabulary.OWL.ALL_VALUES_FROM)].SelectTriplesBySubject((RDFResource)restriction.Value).FirstOrDefault();
                if (allValuesFrom?.TripleFlavor == RDFModelEnums.RDFTripleFlavors.SPO)
                {
                    RDFOntologyClass allValuesFromClass = ontology.Model.ClassModel.SelectClass(allValuesFrom.Object.ToString());
                    if (allValuesFromClass != null)
                    {
                        RDFOntologyAllValuesFromRestriction allValuesFromRestriction = new RDFOntologyAllValuesFromRestriction((RDFResource)restriction.Value, restriction.OnProperty, allValuesFromClass);
                        ontology.Model.ClassModel.Classes[restriction.PatternMemberID] = allValuesFromRestriction;
                        continue; //Restriction has been successfully typed
                    }
                    else
                        RDFSemanticsEvents.RaiseSemanticsWarning(string.Format("AllValuesFromRestriction '{0}' cannot be imported from graph, because definition of its required class '{1}' is not found in the model.", restriction.Value, allValuesFrom.Object));
                }
                #endregion

                #region SomeValuesFrom
                RDFTriple someValuesFrom = prefetchContext[nameof(RDFVocabulary.OWL.SOME_VALUES_FROM)].SelectTriplesBySubject((RDFResource)restriction.Value).FirstOrDefault();
                if (someValuesFrom?.TripleFlavor == RDFModelEnums.RDFTripleFlavors.SPO)
                {
                    RDFOntologyClass someValuesFromClass = ontology.Model.ClassModel.SelectClass(someValuesFrom.Object.ToString());
                    if (someValuesFromClass != null)
                    {
                        RDFOntologySomeValuesFromRestriction someValuesFromRestriction = new RDFOntologySomeValuesFromRestriction((RDFResource)restriction.Value, restriction.OnProperty, someValuesFromClass);
                        ontology.Model.ClassModel.Classes[restriction.PatternMemberID] = someValuesFromRestriction;
                        continue; //Restriction has been successfully typed
                    }
                    else
                        RDFSemanticsEvents.RaiseSemanticsWarning(string.Format("SomeValuesFromRestriction '{0}' cannot be imported from graph, because definition of its required class '{1}' is not found in the model.", restriction.Value, someValuesFrom.Object));
                }
                #endregion
            }
        }

        /// <summary>
        /// Finalizes the definition of the ontology enumerate/datarange classes previously detected
        /// </summary>
        private static void FinalizeEnumeratesAndDataRanges(RDFOntology ontology, RDFGraph ontGraph, Dictionary<string, RDFGraph> prefetchContext)
        {
            #region OWL:OneOf (Enumerate)
            foreach (RDFTriple oneOf in prefetchContext[nameof(RDFVocabulary.OWL.ONE_OF)].Where(t => t.TripleFlavor == RDFModelEnums.RDFTripleFlavors.SPO))
            {
                RDFOntologyClass oneOfClass = ontology.Model.ClassModel.SelectClass(oneOf.Subject.ToString());
                if (oneOfClass != null && !oneOfClass.IsDataRangeClass())
                {
                    #region ClassToEnumerateClass
                    if (!oneOfClass.IsEnumerateClass())
                    {
                        oneOfClass = new RDFOntologyEnumerateClass((RDFResource)oneOf.Subject);
                        ontology.Model.ClassModel.Classes[oneOfClass.PatternMemberID] = oneOfClass;
                    }
                    #endregion ClassToEnumerateClass

                    #region DeserializeEnumerateCollection
                    bool nilFound = false;
                    RDFResource itemRest = (RDFResource)oneOf.Object;
                    HashSet<long> itemRestVisitCache = new HashSet<long>() { itemRest.PatternMemberID };
                    while (!nilFound)
                    {
                        #region rdf:first
                        RDFTriple first = prefetchContext[nameof(RDFVocabulary.RDF.FIRST)].SelectTriplesBySubject(itemRest).FirstOrDefault();
                        if (first != null && first.TripleFlavor == RDFModelEnums.RDFTripleFlavors.SPO)
                        {
                            ontology.Model.ClassModel.AddOneOfRelation((RDFOntologyEnumerateClass)oneOfClass, new List<RDFOntologyIndividual>() { ((RDFResource)first.Object).ToRDFOntologyIndividual() });

                            #region rdf:rest
                            RDFTriple rest = prefetchContext[nameof(RDFVocabulary.RDF.REST)].SelectTriplesBySubject(itemRest).FirstOrDefault();
                            if (rest != null)
                            {
                                if (rest.Object.Equals(RDFVocabulary.RDF.NIL))
                                    nilFound = true;
                                else
                                {
                                    itemRest = (RDFResource)rest.Object;
                                    //Avoid bad-formed cyclic lists to generate infinite loops
                                    if (!itemRestVisitCache.Contains(itemRest.PatternMemberID))
                                        itemRestVisitCache.Add(itemRest.PatternMemberID);
                                    else
                                        nilFound = true;
                                }
                            }
                            else
                                nilFound = true;
                            #endregion
                        }
                        else
                            nilFound = true;
                        #endregion
                    }
                    #endregion
                }
                else if (oneOfClass == null)
                    RDFSemanticsEvents.RaiseSemanticsWarning(string.Format("EnumerateClass '{0}' cannot be imported from graph, because its definition is not found in the model.", oneOf.Subject));
            }
            #endregion

            #region OWL:OneOf (DataRange)
            foreach (RDFTriple oneOf in prefetchContext[nameof(RDFVocabulary.OWL.ONE_OF)].Where(t => t.TripleFlavor == RDFModelEnums.RDFTripleFlavors.SPO))
            {
                RDFOntologyClass oneOfClass = ontology.Model.ClassModel.SelectClass(oneOf.Subject.ToString());
                if (oneOfClass != null && !oneOfClass.IsEnumerateClass())
                {
                    #region ClassToDataRangeClass
                    if (!oneOfClass.IsDataRangeClass())
                    {
                        oneOfClass = new RDFOntologyDataRangeClass((RDFResource)oneOf.Subject);
                        ontology.Model.ClassModel.Classes[oneOfClass.PatternMemberID] = oneOfClass;
                    }
                    #endregion

                    #region DeserializeDataRangeCollection
                    bool nilFound = false;
                    RDFResource itemRest = (RDFResource)oneOf.Object;
                    HashSet<long> itemRestVisitCache = new HashSet<long>() { itemRest.PatternMemberID };
                    while (!nilFound)
                    {
                        #region rdf:first
                        RDFTriple first = prefetchContext[nameof(RDFVocabulary.RDF.FIRST)].SelectTriplesBySubject(itemRest).FirstOrDefault();
                        if (first != null && first.TripleFlavor == RDFModelEnums.RDFTripleFlavors.SPL)
                        {
                            ontology.Model.ClassModel.AddOneOfRelation((RDFOntologyDataRangeClass)oneOfClass, new List<RDFOntologyLiteral>() { ((RDFLiteral)first.Object).ToRDFOntologyLiteral() });

                            #region rdf:rest
                            RDFTriple rest = prefetchContext[nameof(RDFVocabulary.RDF.REST)].SelectTriplesBySubject(itemRest).FirstOrDefault();
                            if (rest != null)
                            {
                                if (rest.Object.Equals(RDFVocabulary.RDF.NIL))
                                    nilFound = true;
                                else
                                {
                                    itemRest = (RDFResource)rest.Object;
                                    //Avoid bad-formed cyclic lists to generate infinite loops
                                    if (!itemRestVisitCache.Contains(itemRest.PatternMemberID))
                                        itemRestVisitCache.Add(itemRest.PatternMemberID);
                                    else
                                        nilFound = true;
                                }
                            }
                            else
                                nilFound = true;
                            #endregion
                        }
                        else
                            nilFound = true;
                        #endregion
                    }
                    #endregion
                }
                else if (oneOfClass == null)
                    RDFSemanticsEvents.RaiseSemanticsWarning(string.Format("DataRangeClass '{0}' cannot be imported from graph, because its definition is not found in the model.", oneOf.Subject));
            }
            #endregion
        }

        /// <summary>
        /// Finalizes the property model definitions
        /// </summary>
        private static void FinalizePropertyModel(RDFOntology ontology, RDFGraph ontGraph, Dictionary<string, RDFGraph> prefetchContext)
        {
            foreach (RDFOntologyProperty property in ontology.Model.PropertyModel.Where(prop => !RDFOntologyChecker.CheckReservedProperty(prop) && !prop.IsAnnotationProperty()).ToList())
            {
                #region Domain
                RDFTriple domain = prefetchContext[nameof(RDFVocabulary.RDFS.DOMAIN)].SelectTriplesBySubject((RDFResource)property.Value).FirstOrDefault();
                if (domain != null && domain.TripleFlavor == RDFModelEnums.RDFTripleFlavors.SPO)
                {
                    RDFOntologyClass domainClass = ontology.Model.ClassModel.SelectClass(domain.Object.ToString());
                    if (domainClass != null)
                        property.SetDomain(domainClass);
                    else
                        RDFSemanticsEvents.RaiseSemanticsWarning(string.Format("Domain constraint on property '{0}' cannot be imported from graph because definition of required class '{1}' is not found in the model.", property.Value, domain.Object));
                }
                #endregion

                #region Range
                RDFTriple range = prefetchContext[nameof(RDFVocabulary.RDFS.RANGE)].SelectTriplesBySubject((RDFResource)property.Value).FirstOrDefault();
                if (range != null && range.TripleFlavor == RDFModelEnums.RDFTripleFlavors.SPO)
                {
                    RDFOntologyClass rangeClass = ontology.Model.ClassModel.SelectClass(range.Object.ToString());
                    if (rangeClass != null)
                        property.SetRange(rangeClass);
                    else
                        RDFSemanticsEvents.RaiseSemanticsWarning(string.Format("Range constraint on property '{0}' cannot be imported from graph because definition of required class '{1}' is not found in the model.", property.Value, range.Object));
                }
                #endregion

                #region SubPropertyOf
                foreach (RDFTriple subPropertyOf in prefetchContext[nameof(RDFVocabulary.RDFS.SUB_PROPERTY_OF)].SelectTriplesBySubject((RDFResource)property.Value).Where(t => t.TripleFlavor == RDFModelEnums.RDFTripleFlavors.SPO))
                {
                    RDFOntologyProperty superProperty = ontology.Model.PropertyModel.SelectProperty(subPropertyOf.Object.ToString());
                    if (superProperty != null)
                    {
                        if (property.IsObjectProperty() && superProperty.IsObjectProperty())
                            ontology.Model.PropertyModel.AddSubPropertyOfRelation((RDFOntologyObjectProperty)property, (RDFOntologyObjectProperty)superProperty);
                        else if (property.IsDatatypeProperty() && superProperty.IsDatatypeProperty())
                            ontology.Model.PropertyModel.AddSubPropertyOfRelation((RDFOntologyDatatypeProperty)property, (RDFOntologyDatatypeProperty)superProperty);
                        else
                            RDFSemanticsEvents.RaiseSemanticsWarning(string.Format("SubPropertyOf relation between properties '{0}' and '{1}' cannot be imported from graph because both properties must be explicitly typed as object or datatype properties.", property.Value, subPropertyOf.Object));
                    }
                    else
                        RDFSemanticsEvents.RaiseSemanticsWarning(string.Format("SubPropertyOf relation on property '{0}' cannot be imported from graph because definition of property '{1}' is not found in the model or represents an annotation property.", property.Value, subPropertyOf.Object));
                }
                #endregion

                #region EquivalentProperty
                foreach (RDFTriple equivalentProperty in prefetchContext[nameof(RDFVocabulary.OWL.EQUIVALENT_PROPERTY)].SelectTriplesBySubject((RDFResource)property.Value).Where(t => t.TripleFlavor == RDFModelEnums.RDFTripleFlavors.SPO))
                {
                    RDFOntologyProperty equivProp = ontology.Model.PropertyModel.SelectProperty(equivalentProperty.Object.ToString());
                    if (equivProp != null)
                    {
                        if (property.IsObjectProperty() && equivProp.IsObjectProperty())
                            ontology.Model.PropertyModel.AddEquivalentPropertyRelation((RDFOntologyObjectProperty)property, (RDFOntologyObjectProperty)equivProp);
                        else if (property.IsDatatypeProperty() && equivProp.IsDatatypeProperty())
                            ontology.Model.PropertyModel.AddEquivalentPropertyRelation((RDFOntologyDatatypeProperty)property, (RDFOntologyDatatypeProperty)equivProp);
                        else
                            RDFSemanticsEvents.RaiseSemanticsWarning(string.Format("EquivalentProperty relation between properties '{0}' and '{1}' cannot be imported from graph because both properties must be explicitly typed as object or datatype properties.", property.Value, equivalentProperty.Object));
                    }
                    else
                        RDFSemanticsEvents.RaiseSemanticsWarning(string.Format("EquivalentProperty relation on property '{0}' cannot be imported from graph because definition of property '{1}' is not found in the model.", property.Value, equivalentProperty.Object));
                }
                #endregion

                #region InverseOf
                if (property.IsObjectProperty())
                {
                    foreach (RDFTriple inverseOf in prefetchContext[nameof(RDFVocabulary.OWL.INVERSE_OF)].SelectTriplesBySubject((RDFResource)property.Value).Where(t => t.TripleFlavor == RDFModelEnums.RDFTripleFlavors.SPO))
                    {
                        RDFOntologyProperty inverseOfProperty = ontology.Model.PropertyModel.SelectProperty(inverseOf.Object.ToString());
                        if (inverseOfProperty != null && inverseOfProperty.IsObjectProperty())
                            ontology.Model.PropertyModel.AddInverseOfRelation((RDFOntologyObjectProperty)property, (RDFOntologyObjectProperty)inverseOfProperty);
                        else
                            RDFSemanticsEvents.RaiseSemanticsWarning(string.Format("InverseOf relation on property '{0}' cannot be imported from graph because definition of property '{1}' is not found in the model, or it does not represent an object property.", property.Value, inverseOf.Object));
                    }
                }
                #endregion

                #region PropertyDisjointWith [OWL2]
                foreach (RDFTriple propertyDisjointWith in prefetchContext[nameof(RDFVocabulary.OWL.PROPERTY_DISJOINT_WITH)].SelectTriplesBySubject((RDFResource)property.Value).Where(t => t.TripleFlavor == RDFModelEnums.RDFTripleFlavors.SPO))
                {
                    RDFOntologyProperty disjointProperty = ontology.Model.PropertyModel.SelectProperty(propertyDisjointWith.Object.ToString());
                    if (disjointProperty != null)
                    {
                        if (property.IsObjectProperty() && disjointProperty.IsObjectProperty())
                            ontology.Model.PropertyModel.AddPropertyDisjointWithRelation((RDFOntologyObjectProperty)property, (RDFOntologyObjectProperty)disjointProperty);
                        else if (property.IsDatatypeProperty() && disjointProperty.IsDatatypeProperty())
                            ontology.Model.PropertyModel.AddPropertyDisjointWithRelation((RDFOntologyDatatypeProperty)property, (RDFOntologyDatatypeProperty)disjointProperty);
                        else
                            RDFSemanticsEvents.RaiseSemanticsWarning(string.Format("PropertyDisjointWith relation between properties '{0}' and '{1}' cannot be imported from graph because both properties must be explicitly typed as object or datatype properties.", property.Value, propertyDisjointWith.Object));
                    }
                    else
                        RDFSemanticsEvents.RaiseSemanticsWarning(string.Format("PropertyDisjointWith relation on property '{0}' cannot be imported from graph because definition of property '{1}' is not found in the model.", property.Value, propertyDisjointWith.Object));
                }
                #endregion

                #region PropertyChainAxiom [OWL2]
                foreach (RDFTriple propertyChainAxiom in prefetchContext[nameof(RDFVocabulary.OWL.PROPERTY_CHAIN_AXIOM)].SelectTriplesBySubject((RDFResource)property.Value))
                {
                    RDFOntologyProperty propertyChainAxiomProperty = ontology.Model.PropertyModel.SelectProperty(propertyChainAxiom.Subject.ToString());
                    if (propertyChainAxiomProperty != null && propertyChainAxiomProperty is RDFOntologyObjectProperty)
                    {
                        List<RDFOntologyObjectProperty> pcaProperties = new List<RDFOntologyObjectProperty>();
                        if (propertyChainAxiom.TripleFlavor == RDFModelEnums.RDFTripleFlavors.SPO)
                        {
                            #region DeserializeCollection
                            bool nilFound = false;
                            RDFResource itemRest = (RDFResource)propertyChainAxiom.Object;
                            HashSet<long> itemRestVisitCache = new HashSet<long>() { itemRest.PatternMemberID };
                            while (!nilFound)
                            {
                                #region rdf:first
                                RDFTriple first = prefetchContext[nameof(RDFVocabulary.RDF.FIRST)].SelectTriplesBySubject(itemRest).FirstOrDefault();
                                if (first != null && first.TripleFlavor == RDFModelEnums.RDFTripleFlavors.SPO)
                                {
                                    RDFOntologyProperty pcaProp = ontology.Model.PropertyModel.SelectProperty(first.Object.ToString());
                                    if (pcaProp != null)
                                    {
                                        if (pcaProp is RDFOntologyObjectProperty)
                                            pcaProperties.Add((RDFOntologyObjectProperty)pcaProp);
                                        else
                                            RDFSemanticsEvents.RaiseSemanticsWarning(string.Format("PropertyChainAxiom relation '{0}' cannot be completely imported from graph, because chain property '{1}' is not an object property.", propertyChainAxiom, first.Object));
                                    }
                                    else
                                        RDFSemanticsEvents.RaiseSemanticsWarning(string.Format("PropertyChainAxiom relation '{0}' cannot be completely imported from graph, because definition of chain property '{1}' is not found in the model.", propertyChainAxiom, first.Object));

                                    #region rdf:rest
                                    RDFTriple rest = prefetchContext[nameof(RDFVocabulary.RDF.REST)].SelectTriplesBySubject(itemRest).FirstOrDefault();
                                    if (rest != null)
                                    {
                                        if (rest.Object.Equals(RDFVocabulary.RDF.NIL))
                                            nilFound = true;
                                        else
                                        {
                                            itemRest = (RDFResource)rest.Object;
                                            //Avoid bad-formed cyclic lists to generate infinite loops
                                            if (!itemRestVisitCache.Contains(itemRest.PatternMemberID))
                                                itemRestVisitCache.Add(itemRest.PatternMemberID);
                                            else
                                                nilFound = true;
                                        }
                                    }
                                    else
                                        nilFound = true;
                                    #endregion
                                }
                                else
                                    nilFound = true;
                                #endregion
                            }
                            #endregion
                        }
                        ontology.Model.PropertyModel.AddPropertyChainAxiomRelation((RDFOntologyObjectProperty)propertyChainAxiomProperty, pcaProperties);
                    }
                    else
                        RDFSemanticsEvents.RaiseSemanticsWarning(string.Format("PropertyChainAxiom relation on property '{0}' cannot be imported from graph, because definition of the property is not found in the model, or it does not represent an object property.", propertyChainAxiom.Subject));
                }
                #endregion
            }

            #region AllDisjointProperties [OWL2]
            foreach (RDFTriple allDisjointProperties in prefetchContext[nameof(RDFVocabulary.OWL.ALL_DISJOINT_PROPERTIES)])
            {
                List<RDFOntologyProperty> allDisjointPropertiesCache = new List<RDFOntologyProperty>();
                foreach (RDFTriple allDisjointPropertiesMembers in prefetchContext[nameof(RDFVocabulary.OWL.MEMBERS)].SelectTriplesBySubject((RDFResource)allDisjointProperties.Subject).Where(t => t.TripleFlavor == RDFModelEnums.RDFTripleFlavors.SPO))
                {
                    #region DeserializeCollection
                    bool nilFound = false;
                    RDFResource itemRest = (RDFResource)allDisjointPropertiesMembers.Object;
                    HashSet<long> itemRestVisitCache = new HashSet<long>() { itemRest.PatternMemberID };
                    while (!nilFound)
                    {
                        #region rdf:first
                        RDFTriple first = prefetchContext[nameof(RDFVocabulary.RDF.FIRST)].SelectTriplesBySubject(itemRest).FirstOrDefault();
                        if (first != null && first.TripleFlavor == RDFModelEnums.RDFTripleFlavors.SPO)
                        {
                            RDFOntologyProperty disjointProperty = ontology.Model.PropertyModel.SelectProperty(first.Object.ToString());
                            if (disjointProperty != null)
                                allDisjointPropertiesCache.Add(disjointProperty);
                            else
                                RDFSemanticsEvents.RaiseSemanticsWarning(string.Format("AllDisjointProperties '{0}' cannot be completely imported from graph because definition of property '{1}' is not found in the model.", allDisjointProperties.Subject, first.Object));

                            #region rdf:rest
                            RDFTriple rest = prefetchContext[nameof(RDFVocabulary.RDF.REST)].SelectTriplesBySubject(itemRest).FirstOrDefault();
                            if (rest != null)
                            {
                                if (rest.Object.Equals(RDFVocabulary.RDF.NIL))
                                    nilFound = true;
                                else
                                {
                                    itemRest = (RDFResource)rest.Object;
                                    //Avoid bad-formed cyclic lists to generate infinite loops
                                    if (!itemRestVisitCache.Contains(itemRest.PatternMemberID))
                                        itemRestVisitCache.Add(itemRest.PatternMemberID);
                                    else
                                        nilFound = true;
                                }
                            }
                            else
                                nilFound = true;
                            #endregion
                        }
                        else
                            nilFound = true;
                        #endregion
                    }
                    #endregion
                }

                //Add pairs of disjointPropertyWith relations
                foreach (RDFOntologyProperty outerProperty in allDisjointPropertiesCache)
                    foreach (RDFOntologyProperty innerProperty in allDisjointPropertiesCache)
                    {
                        if (outerProperty.IsObjectProperty() && innerProperty.IsObjectProperty())
                            ontology.Model.PropertyModel.AddPropertyDisjointWithRelation((RDFOntologyObjectProperty)outerProperty, (RDFOntologyObjectProperty)innerProperty);
                        else if (outerProperty.IsDatatypeProperty() && innerProperty.IsDatatypeProperty())
                            ontology.Model.PropertyModel.AddPropertyDisjointWithRelation((RDFOntologyDatatypeProperty)outerProperty, (RDFOntologyDatatypeProperty)innerProperty);
                        else
                            RDFSemanticsEvents.RaiseSemanticsWarning(string.Format("PropertyDisjointWith relation between properties '{0}' and '{1}' cannot be imported from graph because both properties must be explicitly typed as object or datatype properties.", outerProperty, innerProperty));
                    }
            }
            #endregion
        }

        /// <summary>
        /// Finalizes the class model definitions
        /// </summary>
        private static void FinalizeClassModel(RDFOntology ontology, RDFGraph ontGraph, Dictionary<string, RDFGraph> prefetchContext)
        {
            foreach (RDFOntologyClass simpleClass in ontology.Model.ClassModel.Where(cls => !RDFOntologyChecker.CheckReservedClass(cls) && cls.IsSimpleClass()).ToList())
            {
                #region SubClassOf
                foreach (RDFTriple subClassOf in prefetchContext[nameof(RDFVocabulary.RDFS.SUB_CLASS_OF)].SelectTriplesBySubject((RDFResource)simpleClass.Value).Where(t => t.TripleFlavor == RDFModelEnums.RDFTripleFlavors.SPO))
                {
                    RDFOntologyClass superClass = ontology.Model.ClassModel.SelectClass(subClassOf.Object.ToString());
                    if (superClass != null)
                        ontology.Model.ClassModel.AddSubClassOfRelation(simpleClass, superClass);
                    else
                        RDFSemanticsEvents.RaiseSemanticsWarning(string.Format("SubClassOf relation on class '{0}' cannot be imported from graph, because definition of class '{1}' is not found in the model.", simpleClass.Value, subClassOf.Object));
                }
                #endregion

                #region EquivalentClass
                foreach (RDFTriple equivalentClass in prefetchContext[nameof(RDFVocabulary.OWL.EQUIVALENT_CLASS)].SelectTriplesBySubject((RDFResource)simpleClass.Value).Where(t => t.TripleFlavor == RDFModelEnums.RDFTripleFlavors.SPO))
                {
                    RDFOntologyClass equivClass = ontology.Model.ClassModel.SelectClass(equivalentClass.Object.ToString());
                    if (equivClass != null)
                        ontology.Model.ClassModel.AddEquivalentClassRelation(simpleClass, equivClass);
                    else
                        RDFSemanticsEvents.RaiseSemanticsWarning(string.Format("EquivalentClass relation on class '{0}' cannot be imported from graph, because definition of class '{1}' is not found in the model.", simpleClass.Value, equivalentClass.Object));
                }
                #endregion

                #region DisjointWith
                foreach (RDFTriple disjointWith in prefetchContext[nameof(RDFVocabulary.OWL.DISJOINT_WITH)].SelectTriplesBySubject((RDFResource)simpleClass.Value).Where(t => t.TripleFlavor == RDFModelEnums.RDFTripleFlavors.SPO))
                {
                    RDFOntologyClass disjointWithClass = ontology.Model.ClassModel.SelectClass(disjointWith.Object.ToString());
                    if (disjointWithClass != null)
                        ontology.Model.ClassModel.AddDisjointWithRelation(simpleClass, disjointWithClass);
                    else
                        RDFSemanticsEvents.RaiseSemanticsWarning(string.Format("DisjointWith relation on class '{0}' cannot be imported from graph, because definition of class '{1}' is not found in the model.", simpleClass.Value, disjointWith.Object));
                }
                #endregion

                #region HasKey [OWL2]
                foreach (RDFTriple haskey in prefetchContext[nameof(RDFVocabulary.OWL.HAS_KEY)].SelectTriplesBySubject((RDFResource)simpleClass.Value).Where(t => t.TripleFlavor == RDFModelEnums.RDFTripleFlavors.SPO))
                {
                    RDFOntologyClass haskeyClass = ontology.Model.ClassModel.SelectClass(haskey.Subject.ToString());
                    if (haskeyClass != null)
                    {
                        List<RDFOntologyProperty> keyProps = new List<RDFOntologyProperty>();

                        #region DeserializeCollection
                        bool nilFound = false;
                        RDFResource itemRest = (RDFResource)haskey.Object;
                        HashSet<long> itemRestVisitCache = new HashSet<long>() { itemRest.PatternMemberID };
                        while (!nilFound)
                        {
                            #region rdf:first
                            RDFTriple first = prefetchContext[nameof(RDFVocabulary.RDF.FIRST)].SelectTriplesBySubject(itemRest).FirstOrDefault();
                            if (first != null && first.TripleFlavor == RDFModelEnums.RDFTripleFlavors.SPO)
                            {
                                RDFOntologyProperty keyProp = ontology.Model.PropertyModel.SelectProperty(first.Object.ToString());
                                if (keyProp != null)
                                    keyProps.Add(keyProp);
                                else
                                {
                                    //Raise warning event to inform the user: hasKey cannot be completely imported from graph because definition of property is not found in the model
                                    RDFSemanticsEvents.RaiseSemanticsWarning(string.Format("HasKey relation '{0}' cannot be completely imported from graph because definition of key property '{1}' is not found in the model.", haskey.Subject, first.Object));
                                }

                                #region rdf:rest
                                RDFTriple rest = prefetchContext[nameof(RDFVocabulary.RDF.REST)].SelectTriplesBySubject(itemRest).FirstOrDefault();
                                if (rest != null)
                                {
                                    if (rest.Object.Equals(RDFVocabulary.RDF.NIL))
                                        nilFound = true;
                                    else
                                    {
                                        itemRest = (RDFResource)rest.Object;
                                        //Avoid bad-formed cyclic lists to generate infinite loops
                                        if (!itemRestVisitCache.Contains(itemRest.PatternMemberID))
                                            itemRestVisitCache.Add(itemRest.PatternMemberID);
                                        else
                                            nilFound = true;
                                    }
                                }
                                else
                                    nilFound = true;
                                #endregion
                            }
                            else
                                nilFound = true;
                            #endregion
                        }
                        #endregion

                        ontology.Model.ClassModel.AddHasKeyRelation(haskeyClass, keyProps);
                    }
                    else
                    {
                        //Raise warning event to inform the user: hasKey relation cannot be imported from graph, because definition of class is not found in the model
                        RDFSemanticsEvents.RaiseSemanticsWarning(string.Format("HasKey relation on class '{0}' cannot be imported from graph, because definition of the class is not found in the model.", haskey.Subject));
                    }
                }
                #endregion
            }

            #region AllDisjointClasses [OWL2]
            foreach (RDFTriple allDisjointClasses in prefetchContext[nameof(RDFVocabulary.OWL.ALL_DISJOINT_CLASSES)])
            {
                List<RDFOntologyClass> allDisjointClassesCache = new List<RDFOntologyClass>();
                foreach (RDFTriple allDisjointClassesMembers in prefetchContext[nameof(RDFVocabulary.OWL.MEMBERS)].SelectTriplesBySubject((RDFResource)allDisjointClasses.Subject).Where(t => t.TripleFlavor == RDFModelEnums.RDFTripleFlavors.SPO))
                {
                    #region DeserializeCollection
                    bool nilFound = false;
                    RDFResource itemRest = (RDFResource)allDisjointClassesMembers.Object;
                    HashSet<long> itemRestVisitCache = new HashSet<long>() { itemRest.PatternMemberID };
                    while (!nilFound)
                    {
                        #region rdf:first
                        RDFTriple first = prefetchContext[nameof(RDFVocabulary.RDF.FIRST)].SelectTriplesBySubject(itemRest).FirstOrDefault();
                        if (first != null && first.TripleFlavor == RDFModelEnums.RDFTripleFlavors.SPO)
                        {
                            RDFOntologyClass disjointClass = ontology.Model.ClassModel.SelectClass(first.Object.ToString());
                            if (disjointClass != null)
                                allDisjointClassesCache.Add(disjointClass);
                            else
                                RDFSemanticsEvents.RaiseSemanticsWarning(string.Format("AllDisjointClasses '{0}' cannot be completely imported from graph because definition of class '{1}' is not found in the model.", allDisjointClasses.Subject, first.Object));

                            #region rdf:rest
                            RDFTriple rest = prefetchContext[nameof(RDFVocabulary.RDF.REST)].SelectTriplesBySubject(itemRest).FirstOrDefault();
                            if (rest != null)
                            {
                                if (rest.Object.Equals(RDFVocabulary.RDF.NIL))
                                    nilFound = true;
                                else
                                {
                                    itemRest = (RDFResource)rest.Object;
                                    //Avoid bad-formed cyclic lists to generate infinite loops
                                    if (!itemRestVisitCache.Contains(itemRest.PatternMemberID))
                                        itemRestVisitCache.Add(itemRest.PatternMemberID);
                                    else
                                        nilFound = true;
                                }
                            }
                            else
                                nilFound = true;
                            #endregion rdf:rest
                        }
                        else
                            nilFound = true;
                        #endregion
                    }
                    #endregion
                }

                ontology.Model.ClassModel.AddAllDisjointClassesRelation(allDisjointClassesCache);
            }
            #endregion
        }

        /// <summary>
        /// Finalizes the data definitions
        /// </summary>
        private static void FinalizeData(RDFOntology ontology, RDFGraph ontGraph, Dictionary<string, RDFGraph> prefetchContext)
        {
            #region SameAs
            foreach (RDFTriple sameAs in prefetchContext[nameof(RDFVocabulary.OWL.SAME_AS)].Where(t => t.TripleFlavor == RDFModelEnums.RDFTripleFlavors.SPO))
                ontology.Data.AddSameAsRelation(((RDFResource)sameAs.Subject).ToRDFOntologyIndividual(), ((RDFResource)sameAs.Object).ToRDFOntologyIndividual());
            #endregion

            #region DifferentFrom
            foreach (RDFTriple differentFrom in prefetchContext[nameof(RDFVocabulary.OWL.DIFFERENT_FROM)].Where(t => t.TripleFlavor == RDFModelEnums.RDFTripleFlavors.SPO))
                ontology.Data.AddDifferentFromRelation(((RDFResource)differentFrom.Subject).ToRDFOntologyIndividual(), ((RDFResource)differentFrom.Object).ToRDFOntologyIndividual());
            #endregion

            #region AllDifferent [OWL2]
            foreach (RDFTriple allDifferent in prefetchContext[nameof(RDFVocabulary.OWL.ALL_DIFFERENT)])
            {
                List<RDFOntologyIndividual> allDifferentIndividuals = new List<RDFOntologyIndividual>();
                foreach (RDFTriple allDifferentMembers in prefetchContext[nameof(RDFVocabulary.OWL.DISTINCT_MEMBERS)].SelectTriplesBySubject((RDFResource)allDifferent.Subject).Where(t => t.TripleFlavor == RDFModelEnums.RDFTripleFlavors.SPO))
                {
                    #region DeserializeCollection
                    bool nilFound = false;
                    RDFResource itemRest = (RDFResource)allDifferentMembers.Object;
                    HashSet<long> itemRestVisitCache = new HashSet<long>() { itemRest.PatternMemberID };
                    while (!nilFound)
                    {
                        #region rdf:first
                        RDFTriple first = prefetchContext[nameof(RDFVocabulary.RDF.FIRST)].SelectTriplesBySubject(itemRest).FirstOrDefault();
                        if (first != null && first.TripleFlavor == RDFModelEnums.RDFTripleFlavors.SPO)
                        {
                            RDFOntologyIndividual differentMember = ontology.Data.SelectIndividual(first.Object.ToString());
                            if (differentMember != null)
                                allDifferentIndividuals.Add(differentMember);
                            else
                                RDFSemanticsEvents.RaiseSemanticsWarning(string.Format("AllDifferent '{0}' cannot be completely imported from graph because definition of individual '{1}' is not found in the data.", allDifferent.Subject, first.Object));

                            #region rdf:rest
                            RDFTriple rest = prefetchContext[nameof(RDFVocabulary.RDF.REST)].SelectTriplesBySubject(itemRest).FirstOrDefault();
                            if (rest != null)
                            {
                                if (rest.Object.Equals(RDFVocabulary.RDF.NIL))
                                    nilFound = true;
                                else
                                {
                                    itemRest = (RDFResource)rest.Object;
                                    //Avoid bad-formed cyclic lists to generate infinite loops
                                    if (!itemRestVisitCache.Contains(itemRest.PatternMemberID))
                                        itemRestVisitCache.Add(itemRest.PatternMemberID);
                                    else
                                        nilFound = true;
                                }
                            }
                            else
                                nilFound = true;
                            #endregion
                        }
                        else
                            nilFound = true;
                        #endregion
                    }
                    #endregion
                }
                ontology.Data.AddAllDifferentRelation(allDifferentIndividuals);
            }
            #endregion

            #region Member [SKOS]
            foreach (RDFTriple skosMember in prefetchContext[nameof(RDFVocabulary.SKOS.MEMBER)].Where(t => t.TripleFlavor == RDFModelEnums.RDFTripleFlavors.SPO))
            {
                //skos:Collection
                RDFOntologyIndividual skosCollection = new RDFOntologyIndividual((RDFResource)skosMember.Subject);
                ontology.Data.AddIndividual(skosCollection);
                ontology.Data.AddClassTypeRelation(skosCollection, RDFVocabulary.SKOS.COLLECTION.ToRDFOntologyClass());

                //skos:Collection -> skos:member -> [skos:Concept|skos:Collection]
                ontology.Data.AddMemberRelation(skosCollection, new RDFOntologyIndividual((RDFResource)skosMember.Object));
            }
            #endregion

            #region MemberList [SKOS]
            foreach (RDFTriple skosMemberList in prefetchContext[nameof(RDFVocabulary.SKOS.MEMBER_LIST)].Where(t => t.TripleFlavor == RDFModelEnums.RDFTripleFlavors.SPO))
            {
                //skos:OrderedCollection
                RDFOntologyIndividual skosOrderedCollection = new RDFOntologyIndividual((RDFResource)skosMemberList.Subject);
                ontology.Data.AddIndividual(skosOrderedCollection);
                ontology.Data.AddClassTypeRelation(skosOrderedCollection, RDFVocabulary.SKOS.ORDERED_COLLECTION.ToRDFOntologyClass());

                #region DeserializeOrderedCollection
                bool nilFound = false;
                RDFResource itemRest = (RDFResource)skosMemberList.Object;
                HashSet<long> itemRestVisitCache = new HashSet<long>() { itemRest.PatternMemberID };
                while (!nilFound)
                {
                    #region rdf:first
                    RDFTriple first = prefetchContext[nameof(RDFVocabulary.RDF.FIRST)].SelectTriplesBySubject(itemRest).FirstOrDefault();
                    if (first != null && first.TripleFlavor == RDFModelEnums.RDFTripleFlavors.SPO)
                    {
                        //skos:OrderedCollection -> skos:memberList -> skos:Concept
                        ontology.Data.AddIndividual(((RDFResource)first.Object).ToRDFOntologyIndividual());
                        ontology.Data.AddClassTypeRelation(((RDFResource)first.Object).ToRDFOntologyIndividual(), RDFVocabulary.SKOS.CONCEPT.ToRDFOntologyClass());
                        ontology.Data.AddMemberListRelation(skosOrderedCollection, ((RDFResource)first.Object).ToRDFOntologyIndividual());

                        #region rdf:rest
                        RDFTriple rest = prefetchContext[nameof(RDFVocabulary.RDF.REST)].SelectTriplesBySubject(itemRest).FirstOrDefault();
                        if (rest != null)
                        {
                            if (rest.Object.Equals(RDFVocabulary.RDF.NIL))
                                nilFound = true;
                            else
                            {
                                itemRest = (RDFResource)rest.Object;
                                //Avoid bad-formed cyclic lists to generate infinite loops
                                if (!itemRestVisitCache.Contains(itemRest.PatternMemberID))
                                    itemRestVisitCache.Add(itemRest.PatternMemberID);
                                else
                                    nilFound = true;
                            }
                        }
                        else
                            nilFound = true;
                        #endregion
                    }
                    else
                        nilFound = true;
                    #endregion
                }
                #endregion
            }
            #endregion

            #region NegativeAssertions [OWL2]
            foreach (RDFTriple negativeAssertion in prefetchContext[nameof(RDFVocabulary.OWL.NEGATIVE_PROPERTY_ASSERTION)])
            {
                #region owl:SourceIndividual
                RDFPatternMember sourceIndividual = prefetchContext[nameof(RDFVocabulary.OWL.SOURCE_INDIVIDUAL)].SelectTriplesBySubject((RDFResource)negativeAssertion.Subject).FirstOrDefault()?.Object;
                if (sourceIndividual == null || sourceIndividual is RDFLiteral)
                {
                    RDFSemanticsEvents.RaiseSemanticsWarning(string.Format("NegativeAssertion relation '{0}' cannot be imported from graph, because owl:SourceIndividual triple is not found in the graph or it does not link a resource.", negativeAssertion.Subject));
                    continue;
                }
                #endregion

                #region owl:AssertionProperty
                RDFPatternMember assertionProperty = prefetchContext[nameof(RDFVocabulary.OWL.ASSERTION_PROPERTY)].SelectTriplesBySubject((RDFResource)negativeAssertion.Subject).FirstOrDefault()?.Object;
                if (assertionProperty == null || assertionProperty is RDFLiteral)
                {
                    RDFSemanticsEvents.RaiseSemanticsWarning(string.Format("NegativeAssertion relation '{0}' cannot be imported from graph, because owl:AssertionProperty triple is not found in the graph or it does not link a resource.", negativeAssertion.Subject));
                    continue;
                }

                //Check if property exists in the property model
                RDFOntologyProperty apProperty = ontology.Model.PropertyModel.SelectProperty(assertionProperty.ToString());
                if (apProperty == null)
                {
                    RDFSemanticsEvents.RaiseSemanticsWarning(string.Format("NegativeAssertion relation '{0}' cannot be imported from graph, because owl:AssertionProperty '{1}' is not a declared property.", negativeAssertion.Subject, assertionProperty));
                    continue;
                }
                #endregion

                #region owl:TargetIndividual
                RDFPatternMember targetIndividual = prefetchContext[nameof(RDFVocabulary.OWL.TARGET_INDIVIDUAL)].SelectTriplesBySubject((RDFResource)negativeAssertion.Subject).FirstOrDefault()?.Object;
                if (targetIndividual != null)
                {
                    //We found owl:TargetIndividual, so we can accept it only if it's a resource
                    //and the negative assertion's property is effectively an object property
                    if (targetIndividual is RDFResource && apProperty is RDFOntologyObjectProperty)
                    {
                        ontology.Data.AddNegativeObjectAssertion(((RDFResource)sourceIndividual).ToRDFOntologyIndividual(), (RDFOntologyObjectProperty)apProperty, ((RDFResource)targetIndividual).ToRDFOntologyIndividual());
                        continue;
                    }
                    else
                    {
                        
                        RDFSemanticsEvents.RaiseSemanticsWarning(string.Format("NegativeAssertion relation '{0}' cannot be imported from graph, because use of owl:TargetIndividual is not correct.", negativeAssertion.Subject));
                        continue;
                    }
                }
                #endregion

                #region owl:TargetValue
                RDFPatternMember targetValue = prefetchContext[nameof(RDFVocabulary.OWL.TARGET_VALUE)].SelectTriplesBySubject((RDFResource)negativeAssertion.Subject).FirstOrDefault()?.Object;
                if (targetValue != null)
                {
                    //We found owl:TargetValue, so we can accept it only if it's a literal
                    //and the negative assertion's property is effectively a datatype property
                    if (targetValue is RDFLiteral && apProperty is RDFOntologyDatatypeProperty)
                    {
                        ontology.Data.AddNegativeDataAssertion(((RDFResource)sourceIndividual).ToRDFOntologyIndividual(), (RDFOntologyDatatypeProperty)apProperty, ((RDFLiteral)targetValue).ToRDFOntologyLiteral());
                        continue;
                    }
                    else
                    {
                        //Raise warning event to inform the user: negative assertion relation cannot be imported from graph, because use of target value is not correct
                        RDFSemanticsEvents.RaiseSemanticsWarning(string.Format("NegativeAssertion relation '{0}' cannot be imported from graph, because use of owl:TargetValue is not correct.", negativeAssertion.Subject));
                        continue;
                    }
                }
                #endregion

                RDFSemanticsEvents.RaiseSemanticsWarning(string.Format("NegativeAssertion relation '{0}' cannot be imported from graph, because neither owl:TargetIndividual or owl:TargetValue triples are found in the graph.", negativeAssertion.Subject));
            }
            #endregion

            #region Assertions
            foreach (RDFOntologyProperty property in ontology.Model.PropertyModel.Where(prop => !RDFOntologyChecker.CheckReservedProperty(prop) && !prop.IsAnnotationProperty()).ToList())
            {
                foreach (RDFTriple assertion in ontGraph.SelectTriplesByPredicate((RDFResource)property.Value).Where(triple => !triple.Subject.Equals(ontology)
                                                                                                                                 && !ontology.Model.ClassModel.Classes.ContainsKey(triple.Subject.PatternMemberID)
                                                                                                                                   && !ontology.Model.PropertyModel.Properties.ContainsKey(triple.Subject.PatternMemberID)))
                {
                    //Check if the property is an owl:ObjectProperty
                    if (property.IsObjectProperty())
                    {
                        if (assertion.TripleFlavor == RDFModelEnums.RDFTripleFlavors.SPO)
                            ontology.Data.AddObjectAssertion(((RDFResource)assertion.Subject).ToRDFOntologyIndividual(), (RDFOntologyObjectProperty)property, ((RDFResource)assertion.Object).ToRDFOntologyIndividual());
                        else
                            RDFSemanticsEvents.RaiseSemanticsWarning(string.Format("Assertion relation on individual '{0}' cannot be imported from graph, because object property '{1}' links to a literal.", assertion.Subject, property));
                    }

                    //Check if the property is an owl:DatatypeProperty
                    else if (property.IsDatatypeProperty())
                    {
                        if (assertion.TripleFlavor == RDFModelEnums.RDFTripleFlavors.SPL)
                            ontology.Data.AddDataAssertion(((RDFResource)assertion.Subject).ToRDFOntologyIndividual(), (RDFOntologyDatatypeProperty)property, ((RDFLiteral)assertion.Object).ToRDFOntologyLiteral());
                        else
                            RDFSemanticsEvents.RaiseSemanticsWarning(string.Format("Assertion relation on individual '{0}' cannot be imported from graph, because datatype property '{1}' links to an individual.", assertion.Subject, property));
                    }
                }
            }
            #endregion
        }

        /// <summary>
        /// Finalizes the ontology annotations
        /// </summary>
        private static void FinalizeOntologyAnnotations(RDFOntology ontology, RDFGraph ontGraph, Dictionary<string, RDFGraph> prefetchContext)
        {
            #region VersionInfo
            foreach (RDFTriple versionInfo in prefetchContext[nameof(RDFVocabulary.OWL.VERSION_INFO)].SelectTriplesBySubject((RDFResource)ontology.Value))
            {
                if (versionInfo.TripleFlavor == RDFModelEnums.RDFTripleFlavors.SPL)
                    ontology.AddStandardAnnotation(RDFSemanticsEnums.RDFOntologyStandardAnnotation.VersionInfo, ((RDFLiteral)versionInfo.Object).ToRDFOntologyLiteral());
                else
                    RDFSemanticsEvents.RaiseSemanticsWarning(string.Format("VersionInfo annotation on ontology '{0}' cannot be imported from graph, because it does not link a literal.", ontology.Value));
            }
            #endregion

            #region VersionIRI
            foreach (RDFTriple versionIRI in prefetchContext[nameof(RDFVocabulary.OWL.VERSION_IRI)].SelectTriplesBySubject((RDFResource)ontology.Value))
            {
                if (versionIRI.TripleFlavor == RDFModelEnums.RDFTripleFlavors.SPO)
                    ontology.AddStandardAnnotation(RDFSemanticsEnums.RDFOntologyStandardAnnotation.VersionIRI, new RDFOntology((RDFResource)versionIRI.Object));
                else
                    RDFSemanticsEvents.RaiseSemanticsWarning(string.Format("VersionIRI annotation on ontology '{0}' cannot be imported from graph, because it does not link a resource.", ontology.Value));
            }
            #endregion

            #region Comment
            foreach (RDFTriple comment in prefetchContext[nameof(RDFVocabulary.RDFS.COMMENT)].SelectTriplesBySubject((RDFResource)ontology.Value))
            {
                if (comment.TripleFlavor == RDFModelEnums.RDFTripleFlavors.SPL)
                    ontology.AddStandardAnnotation(RDFSemanticsEnums.RDFOntologyStandardAnnotation.Comment, ((RDFLiteral)comment.Object).ToRDFOntologyLiteral());
                else
                    RDFSemanticsEvents.RaiseSemanticsWarning(string.Format("Comment annotation on ontology '{0}' cannot be imported from graph, because it does not link a literal.", ontology.Value));
            }
            #endregion

            #region Label
            foreach (RDFTriple label in prefetchContext[nameof(RDFVocabulary.RDFS.LABEL)].SelectTriplesBySubject((RDFResource)ontology.Value))
            {
                if (label.TripleFlavor == RDFModelEnums.RDFTripleFlavors.SPL)
                    ontology.AddStandardAnnotation(RDFSemanticsEnums.RDFOntologyStandardAnnotation.Label, ((RDFLiteral)label.Object).ToRDFOntologyLiteral());
                else
                    RDFSemanticsEvents.RaiseSemanticsWarning(string.Format("Label annotation on ontology '{0}' cannot be imported from graph, because it does not link a literal.", ontology.Value));
            }
            #endregion

            #region SeeAlso
            foreach (RDFTriple seeAlso in prefetchContext[nameof(RDFVocabulary.RDFS.SEE_ALSO)].SelectTriplesBySubject((RDFResource)ontology.Value))
                ontology.AddStandardAnnotation(RDFSemanticsEnums.RDFOntologyStandardAnnotation.SeeAlso, new RDFOntologyResource { Value = seeAlso.Object });
            #endregion

            #region IsDefinedBy
            foreach (RDFTriple isDefinedBy in prefetchContext[nameof(RDFVocabulary.RDFS.IS_DEFINED_BY)].SelectTriplesBySubject((RDFResource)ontology.Value))
                ontology.AddStandardAnnotation(RDFSemanticsEnums.RDFOntologyStandardAnnotation.IsDefinedBy, new RDFOntologyResource { Value = isDefinedBy.Object });
            #endregion

            #region BackwardCompatibleWith
            foreach (RDFTriple backwardCompatibleWith in prefetchContext[nameof(RDFVocabulary.OWL.BACKWARD_COMPATIBLE_WITH)].SelectTriplesBySubject((RDFResource)ontology.Value))
            {
                if (backwardCompatibleWith.TripleFlavor == RDFModelEnums.RDFTripleFlavors.SPO)
                    ontology.AddStandardAnnotation(RDFSemanticsEnums.RDFOntologyStandardAnnotation.BackwardCompatibleWith, new RDFOntology((RDFResource)backwardCompatibleWith.Object));
                else
                    RDFSemanticsEvents.RaiseSemanticsWarning(string.Format("BackwardCompatibleWith annotation on ontology '{0}' cannot be imported from graph, because it does not link a resource.", ontology.Value));
            }
            #endregion

            #region IncompatibleWith
            foreach (RDFTriple incompatibleWith in prefetchContext[nameof(RDFVocabulary.OWL.INCOMPATIBLE_WITH)].SelectTriplesBySubject((RDFResource)ontology.Value))
            {
                if (incompatibleWith.TripleFlavor == RDFModelEnums.RDFTripleFlavors.SPO)
                    ontology.AddStandardAnnotation(RDFSemanticsEnums.RDFOntologyStandardAnnotation.IncompatibleWith, new RDFOntology((RDFResource)incompatibleWith.Object));
                else
                    RDFSemanticsEvents.RaiseSemanticsWarning(string.Format("IncompatibleWith annotation on ontology '{0}' cannot be imported from graph, because it does not link a resource.", ontology.Value));
            }
            #endregion

            #region PriorVersion
            foreach (RDFTriple priorVersion in prefetchContext[nameof(RDFVocabulary.OWL.PRIOR_VERSION)].SelectTriplesBySubject((RDFResource)ontology.Value))
                ontology.AddStandardAnnotation(RDFSemanticsEnums.RDFOntologyStandardAnnotation.PriorVersion, new RDFOntologyResource() { Value = priorVersion.Object });
            #endregion

            #region Imports
            foreach (RDFTriple imports in prefetchContext[nameof(RDFVocabulary.OWL.IMPORTS)].SelectTriplesBySubject((RDFResource)ontology.Value))
            { 
                if (imports.TripleFlavor == RDFModelEnums.RDFTripleFlavors.SPO)
                    ontology.AddStandardAnnotation(RDFSemanticsEnums.RDFOntologyStandardAnnotation.Imports, new RDFOntology((RDFResource)imports.Object));
                else
                    RDFSemanticsEvents.RaiseSemanticsWarning(string.Format("Imports annotation on ontology '{0}' cannot be imported from graph, because it does not link a resource.", ontology.Value));
            }
            #endregion

            #region CustomAnnotations
            foreach (RDFOntologyProperty annotationProperty in ontology.Model.PropertyModel.Where(ap => ap.IsAnnotationProperty() && !StandardAnnotationProperties.Contains(ap.PatternMemberID)).ToList())
                foreach (RDFTriple annotation in ontGraph.SelectTriplesBySubject((RDFResource)ontology.Value).SelectTriplesByPredicate((RDFResource)annotationProperty.Value))
                    ontology.AddCustomAnnotation((RDFOntologyAnnotationProperty)annotationProperty, new RDFOntologyResource { Value = annotation.Object });
            #endregion
        }

        /// <summary>
        /// Finalizes the class model annotations
        /// </summary>
        private static void FinalizeClassModelAnnotations(RDFOntology ontology, RDFGraph ontGraph, Dictionary<string, RDFGraph> prefetchContext)
        {
            foreach (RDFOntologyClass ontologyClass in ontology.Model.ClassModel.Where(cls => !RDFOntologyChecker.CheckReservedClass(cls)).ToList())
            {
                #region VersionInfo
                foreach (RDFTriple versionInfo in prefetchContext[nameof(RDFVocabulary.OWL.VERSION_INFO)].SelectTriplesBySubject((RDFResource)ontologyClass.Value))
                {
                    if (versionInfo.TripleFlavor == RDFModelEnums.RDFTripleFlavors.SPL)
                        ontology.Model.ClassModel.AddStandardAnnotation(RDFSemanticsEnums.RDFOntologyStandardAnnotation.VersionInfo, ontologyClass, ((RDFLiteral)versionInfo.Object).ToRDFOntologyLiteral());
                    else
                        RDFSemanticsEvents.RaiseSemanticsWarning(string.Format("VersionInfo annotation on class '{0}' cannot be imported from graph, because it does not link a literal.", ontologyClass.Value));
                }
                #endregion

                #region Comment
                foreach (RDFTriple comment in prefetchContext[nameof(RDFVocabulary.RDFS.COMMENT)].SelectTriplesBySubject((RDFResource)ontologyClass.Value))
                {
                    if (comment.TripleFlavor == RDFModelEnums.RDFTripleFlavors.SPL)
                        ontology.Model.ClassModel.AddStandardAnnotation(RDFSemanticsEnums.RDFOntologyStandardAnnotation.Comment, ontologyClass, ((RDFLiteral)comment.Object).ToRDFOntologyLiteral());
                    else
                        RDFSemanticsEvents.RaiseSemanticsWarning(string.Format("Comment annotation on class '{0}' cannot be imported from graph, because it does not link a literal.", ontologyClass.Value));
                }
                #endregion

                #region Label
                foreach (RDFTriple label in prefetchContext[nameof(RDFVocabulary.RDFS.LABEL)].SelectTriplesBySubject((RDFResource)ontologyClass.Value))
                {
                    if (label.TripleFlavor == RDFModelEnums.RDFTripleFlavors.SPL)
                        ontology.Model.ClassModel.AddStandardAnnotation(RDFSemanticsEnums.RDFOntologyStandardAnnotation.Label, ontologyClass, ((RDFLiteral)label.Object).ToRDFOntologyLiteral());
                    else
                        RDFSemanticsEvents.RaiseSemanticsWarning(string.Format("Label annotation on class '{0}' cannot be imported from graph, because it does not link a literal.", ontologyClass.Value));
                }
                #endregion

                #region SeeAlso
                foreach (RDFTriple seeAlso in prefetchContext[nameof(RDFVocabulary.RDFS.SEE_ALSO)].SelectTriplesBySubject((RDFResource)ontologyClass.Value))
                    ontology.Model.ClassModel.AddStandardAnnotation(RDFSemanticsEnums.RDFOntologyStandardAnnotation.SeeAlso, ontologyClass, new RDFOntologyResource { Value = seeAlso.Object });
                #endregion

                #region IsDefinedBy
                foreach (RDFTriple isDefinedBy in prefetchContext[nameof(RDFVocabulary.RDFS.IS_DEFINED_BY)].SelectTriplesBySubject((RDFResource)ontologyClass.Value))
                    ontology.Model.ClassModel.AddStandardAnnotation(RDFSemanticsEnums.RDFOntologyStandardAnnotation.IsDefinedBy, ontologyClass, new RDFOntologyResource { Value = isDefinedBy.Object });
                #endregion

                #region CustomAnnotations
                RDFGraph classGraph = ontGraph.SelectTriplesBySubject((RDFResource)ontologyClass.Value);
                foreach (RDFOntologyProperty annotationProperty in ontology.Model.PropertyModel.Where(ap => ap.IsAnnotationProperty() && !StandardAnnotationProperties.Contains(ap.PatternMemberID)).ToList())
                    foreach (RDFTriple classAnnotation in classGraph.SelectTriplesByPredicate((RDFResource)annotationProperty.Value))
                        ontology.Model.ClassModel.AddCustomAnnotation((RDFOntologyAnnotationProperty)annotationProperty, ontologyClass, new RDFOntologyResource { Value = classAnnotation.Object });
                #endregion
            }
        }

        /// <summary>
        /// Finalizes the property model annotations
        /// </summary>
        private static void FinalizePropertyModelAnnotations(RDFOntology ontology, RDFGraph ontGraph, Dictionary<string, RDFGraph> prefetchContext)
        {
            foreach (RDFOntologyProperty ontologyProperty in ontology.Model.PropertyModel.Where(prop => !RDFOntologyChecker.CheckReservedProperty(prop)).ToList())
            {
                #region VersionInfo
                foreach (RDFTriple versionInfo in prefetchContext[nameof(RDFVocabulary.OWL.VERSION_INFO)].SelectTriplesBySubject((RDFResource)ontologyProperty.Value))
                {
                    if (versionInfo.TripleFlavor == RDFModelEnums.RDFTripleFlavors.SPL)
                        ontology.Model.PropertyModel.AddStandardAnnotation(RDFSemanticsEnums.RDFOntologyStandardAnnotation.VersionInfo, ontologyProperty, ((RDFLiteral)versionInfo.Object).ToRDFOntologyLiteral());
                    else
                        RDFSemanticsEvents.RaiseSemanticsWarning(string.Format("VersionInfo annotation on property '{0}' cannot be imported from graph, because it does not link a literal.", ontologyProperty.Value));
                }
                #endregion

                #region Comment
                foreach (RDFTriple comment in prefetchContext[nameof(RDFVocabulary.RDFS.COMMENT)].SelectTriplesBySubject((RDFResource)ontologyProperty.Value))
                {
                    if (comment.TripleFlavor == RDFModelEnums.RDFTripleFlavors.SPL)
                        ontology.Model.PropertyModel.AddStandardAnnotation(RDFSemanticsEnums.RDFOntologyStandardAnnotation.Comment, ontologyProperty, ((RDFLiteral)comment.Object).ToRDFOntologyLiteral());
                    else
                        RDFSemanticsEvents.RaiseSemanticsWarning(string.Format("Comment annotation on property '{0}' cannot be imported from graph, because it does not link a literal.", ontologyProperty.Value));
                }
                #endregion

                #region Label
                foreach (RDFTriple label in prefetchContext[nameof(RDFVocabulary.RDFS.LABEL)].SelectTriplesBySubject((RDFResource)ontologyProperty.Value))
                {
                    if (label.TripleFlavor == RDFModelEnums.RDFTripleFlavors.SPL)
                        ontology.Model.PropertyModel.AddStandardAnnotation(RDFSemanticsEnums.RDFOntologyStandardAnnotation.Label, ontologyProperty, ((RDFLiteral)label.Object).ToRDFOntologyLiteral());
                    else
                        RDFSemanticsEvents.RaiseSemanticsWarning(string.Format("Label annotation on property '{0}' cannot be imported from graph, because it does not link a literal.", ontologyProperty.Value));
                }
                #endregion

                #region SeeAlso
                foreach (RDFTriple seeAlso in prefetchContext[nameof(RDFVocabulary.RDFS.SEE_ALSO)].SelectTriplesBySubject((RDFResource)ontologyProperty.Value))
                    ontology.Model.PropertyModel.AddStandardAnnotation(RDFSemanticsEnums.RDFOntologyStandardAnnotation.SeeAlso, ontologyProperty, new RDFOntologyResource { Value = seeAlso.Object });
                #endregion

                #region IsDefinedBy
                foreach (RDFTriple isDefinedBy in prefetchContext[nameof(RDFVocabulary.RDFS.IS_DEFINED_BY)].SelectTriplesBySubject((RDFResource)ontologyProperty.Value))
                    ontology.Model.PropertyModel.AddStandardAnnotation(RDFSemanticsEnums.RDFOntologyStandardAnnotation.IsDefinedBy, ontologyProperty, new RDFOntologyResource { Value = isDefinedBy.Object });
                #endregion

                #region CustomAnnotations
                RDFGraph propertyGraph = ontGraph.SelectTriplesBySubject((RDFResource)ontologyProperty.Value);
                foreach (RDFOntologyProperty annotationProperty in ontology.Model.PropertyModel.Where(ap => ap.IsAnnotationProperty() && !StandardAnnotationProperties.Contains(ap.PatternMemberID)).ToList())
                    foreach (RDFTriple propertyAnnotation in propertyGraph.SelectTriplesByPredicate((RDFResource)annotationProperty.Value))
                        ontology.Model.PropertyModel.AddCustomAnnotation((RDFOntologyAnnotationProperty)annotationProperty, ontologyProperty, new RDFOntologyResource { Value = propertyAnnotation.Object });
                #endregion
            }
        }

        /// <summary>
        /// Finalizes the data annotations
        /// </summary>
        private static void FinalizeDataAnnotations(RDFOntology ontology, RDFGraph ontGraph, Dictionary<string, RDFGraph> prefetchContext)
        {
            foreach (RDFOntologyIndividual individual in ontology.Data)
            {
                #region VersionInfo
                foreach (RDFTriple versionInfo in prefetchContext[nameof(RDFVocabulary.OWL.VERSION_INFO)].SelectTriplesBySubject((RDFResource)individual.Value))
                {
                    if (versionInfo.TripleFlavor == RDFModelEnums.RDFTripleFlavors.SPL)
                        ontology.Data.AddStandardAnnotation(RDFSemanticsEnums.RDFOntologyStandardAnnotation.VersionInfo, individual, ((RDFLiteral)versionInfo.Object).ToRDFOntologyLiteral());
                    else
                        RDFSemanticsEvents.RaiseSemanticsWarning(string.Format("VersionInfo annotation on individual '{0}' cannot be imported from graph, because it does not link a literal.", individual.Value));
                }
                #endregion

                #region Comment
                foreach (RDFTriple comment in prefetchContext[nameof(RDFVocabulary.RDFS.COMMENT)].SelectTriplesBySubject((RDFResource)individual.Value))
                {
                    if (comment.TripleFlavor == RDFModelEnums.RDFTripleFlavors.SPL)
                        ontology.Data.AddStandardAnnotation(RDFSemanticsEnums.RDFOntologyStandardAnnotation.Comment, individual, ((RDFLiteral)comment.Object).ToRDFOntologyLiteral());
                    else
                        RDFSemanticsEvents.RaiseSemanticsWarning(string.Format("Comment annotation on individual '{0}' cannot be imported from graph, because it does not link a literal.", individual.Value));
                }
                #endregion

                #region Label
                foreach (RDFTriple label in prefetchContext[nameof(RDFVocabulary.RDFS.LABEL)].SelectTriplesBySubject((RDFResource)individual.Value))
                {
                    if (label.TripleFlavor == RDFModelEnums.RDFTripleFlavors.SPL)
                        ontology.Data.AddStandardAnnotation(RDFSemanticsEnums.RDFOntologyStandardAnnotation.Label, individual, ((RDFLiteral)label.Object).ToRDFOntologyLiteral());
                    else
                        RDFSemanticsEvents.RaiseSemanticsWarning(string.Format("Label annotation on individual '{0}' cannot be imported from graph, because it does not link a literal.", individual.Value));
                }
                #endregion

                #region SeeAlso
                foreach (RDFTriple seeAlso in prefetchContext[nameof(RDFVocabulary.RDFS.SEE_ALSO)].SelectTriplesBySubject((RDFResource)individual.Value))
                    ontology.Data.AddStandardAnnotation(RDFSemanticsEnums.RDFOntologyStandardAnnotation.SeeAlso, individual, new RDFOntologyResource { Value = seeAlso.Object });
                #endregion

                #region IsDefinedBy
                foreach (RDFTriple isDefinedBy in prefetchContext[nameof(RDFVocabulary.RDFS.IS_DEFINED_BY)].SelectTriplesBySubject((RDFResource)individual.Value))
                    ontology.Data.AddStandardAnnotation(RDFSemanticsEnums.RDFOntologyStandardAnnotation.IsDefinedBy, individual, new RDFOntologyResource { Value = isDefinedBy.Object });
                #endregion

                #region CustomAnnotations
                RDFGraph individualGraph = ontGraph.SelectTriplesBySubject((RDFResource)individual.Value);
                foreach (RDFOntologyProperty annotationProperty in ontology.Model.PropertyModel.Where(ap => ap.IsAnnotationProperty() && !StandardAnnotationProperties.Contains(ap.PatternMemberID)).ToList())
                    foreach (RDFTriple individualAnnotation in individualGraph.SelectTriplesByPredicate((RDFResource)annotationProperty.Value))
                        ontology.Data.AddCustomAnnotation((RDFOntologyAnnotationProperty)annotationProperty, individual, new RDFOntologyResource { Value = individualAnnotation.Object });
                #endregion
            }
        }

        /// <summary>
        /// Finalizes the axiom annotations
        /// </summary>
        private static void FinalizeAxiomAnnotations(RDFOntology ontology, RDFGraph ontGraph, Dictionary<string, RDFGraph> prefetchContext)
        {
            #region AxiomAnnotations [OWL2]
            foreach (RDFTriple axiom in prefetchContext[nameof(RDFVocabulary.OWL.AXIOM)])
            {
                #region owl:annotatedSource
                RDFPatternMember annotatedSource = prefetchContext[nameof(RDFVocabulary.OWL.ANNOTATED_SOURCE)].SelectTriplesBySubject((RDFResource)axiom.Subject).FirstOrDefault()?.Object;
                if (annotatedSource == null || annotatedSource is RDFLiteral)
                {
                    RDFSemanticsEvents.RaiseSemanticsWarning(string.Format("Axiom annotation '{0}' cannot be imported from graph, because owl:annotatedSource triple is not found in the graph or it does not link a resource.", axiom.Subject));
                    continue;
                }
                #endregion

                #region owl:annotatedProperty
                RDFPatternMember annotatedProperty = prefetchContext[nameof(RDFVocabulary.OWL.ANNOTATED_PROPERTY)].SelectTriplesBySubject((RDFResource)axiom.Subject).FirstOrDefault()?.Object;
                if (annotatedProperty == null || annotatedProperty is RDFLiteral)
                {
                    RDFSemanticsEvents.RaiseSemanticsWarning(string.Format("Axiom annotation '{0}' cannot be imported from graph, because owl:annotatedProperty triple is not found in the graph or it does not link a resource.", axiom.Subject));
                    continue;
                }
                #endregion

                #region owl:annotatedTarget
                RDFPatternMember annotatedTarget = prefetchContext[nameof(RDFVocabulary.OWL.ANNOTATED_TARGET)].SelectTriplesBySubject((RDFResource)axiom.Subject).FirstOrDefault()?.Object;
                if (annotatedTarget == null)
                { 
                    RDFSemanticsEvents.RaiseSemanticsWarning(string.Format("Axiom annotation '{0}' cannot be imported from graph, because owl:annotatedTarget triple is not found in the graph.", axiom.Subject));
                    continue;
                }
                #endregion

                //Fetch annotations owned by this axiom
                RDFGraph axiomAnnotationsGraph = ontGraph.SelectTriplesBySubject((RDFResource)axiom.Subject)
                                                         .RemoveTriplesByPredicateObject(RDFVocabulary.RDF.TYPE, RDFVocabulary.OWL.AXIOM)
                                                         .RemoveTriplesByPredicate(RDFVocabulary.OWL.ANNOTATED_SOURCE)
                                                         .RemoveTriplesByPredicate(RDFVocabulary.OWL.ANNOTATED_PROPERTY)
                                                         .RemoveTriplesByPredicate(RDFVocabulary.OWL.ANNOTATED_TARGET);

                //Assign fetched annotations to proper taxonomy (recognition driven by owl:AnnotatedProperty)
                bool annotatedTargetIsResource = annotatedTarget is RDFResource;
                foreach (RDFTriple axiomAnnotationTriple in axiomAnnotationsGraph.Where(axn => axn.TripleFlavor == RDFModelEnums.RDFTripleFlavors.SPL))
                {
                    RDFOntologyAxiomAnnotation axiomAnnotation = new RDFOntologyAxiomAnnotation(((RDFResource)axiomAnnotationTriple.Predicate).ToRDFOntologyProperty(), ((RDFLiteral)axiomAnnotationTriple.Object).ToRDFOntologyLiteral());

                    #region ClassModel(SubClassOf)
                    if (annotatedProperty.Equals(RDFVocabulary.RDFS.SUB_CLASS_OF))
                        ontology.Model.ClassModel.AddSubClassOfRelation(((RDFResource)annotatedSource).ToRDFOntologyClass(), ((RDFResource)annotatedTarget).ToRDFOntologyClass(), axiomAnnotation);
                    #endregion

                    #region ClassModel(EquivalentClass)
                    else if (annotatedProperty.Equals(RDFVocabulary.OWL.EQUIVALENT_CLASS))
                        ontology.Model.ClassModel.AddEquivalentClassRelation(((RDFResource)annotatedSource).ToRDFOntologyClass(), ((RDFResource)annotatedTarget).ToRDFOntologyClass(), axiomAnnotation);
                    #endregion

                    #region ClassModel(DisjointWith)
                    else if (annotatedProperty.Equals(RDFVocabulary.OWL.DISJOINT_WITH))
                        ontology.Model.ClassModel.AddDisjointWithRelation(((RDFResource)annotatedSource).ToRDFOntologyClass(), ((RDFResource)annotatedTarget).ToRDFOntologyClass(), axiomAnnotation);
                    #endregion

                    #region PropertyModel(SubPropertyOf)
                    else if (annotatedProperty.Equals(RDFVocabulary.RDFS.SUB_PROPERTY_OF))
                        ontology.Model.PropertyModel.AddSubPropertyOfRelation(((RDFResource)annotatedSource).ToRDFOntologyObjectProperty(), ((RDFResource)annotatedTarget).ToRDFOntologyObjectProperty(), axiomAnnotation);
                    #endregion

                    #region PropertyModel(EquivalentProperty)
                    else if (annotatedProperty.Equals(RDFVocabulary.OWL.EQUIVALENT_PROPERTY))
                        ontology.Model.PropertyModel.AddEquivalentPropertyRelation(((RDFResource)annotatedSource).ToRDFOntologyObjectProperty(), ((RDFResource)annotatedTarget).ToRDFOntologyObjectProperty(), axiomAnnotation);
                    #endregion

                    #region PropertyModel(PropertyDisjointWith) [OWL2]
                    else if (annotatedProperty.Equals(RDFVocabulary.OWL.PROPERTY_DISJOINT_WITH))
                        ontology.Model.PropertyModel.AddPropertyDisjointWithRelation(((RDFResource)annotatedSource).ToRDFOntologyObjectProperty(), ((RDFResource)annotatedTarget).ToRDFOntologyObjectProperty(), axiomAnnotation);
                    #endregion

                    #region PropertyModel(InverseOf)
                    else if (annotatedProperty.Equals(RDFVocabulary.OWL.INVERSE_OF))
                        ontology.Model.PropertyModel.AddInverseOfRelation(((RDFResource)annotatedSource).ToRDFOntologyObjectProperty(), ((RDFResource)annotatedTarget).ToRDFOntologyObjectProperty(), axiomAnnotation);
                    #endregion

                    #region Data(ClassType)
                    else if (annotatedProperty.Equals(RDFVocabulary.RDF.TYPE))
                        ontology.Data.AddClassTypeRelation(((RDFResource)annotatedSource).ToRDFOntologyIndividual(), ((RDFResource)annotatedTarget).ToRDFOntologyClass(), axiomAnnotation);
                    #endregion

                    #region Data(SameAs)
                    else if (annotatedProperty.Equals(RDFVocabulary.OWL.SAME_AS))
                        ontology.Data.AddSameAsRelation(((RDFResource)annotatedSource).ToRDFOntologyIndividual(), ((RDFResource)annotatedTarget).ToRDFOntologyIndividual(), axiomAnnotation);
                    #endregion

                    #region Data(DifferentFrom)
                    else if (annotatedProperty.Equals(RDFVocabulary.OWL.DIFFERENT_FROM))
                        ontology.Data.AddDifferentFromRelation(((RDFResource)annotatedSource).ToRDFOntologyIndividual(), ((RDFResource)annotatedTarget).ToRDFOntologyIndividual(), axiomAnnotation);
                    #endregion

                    #region Data(Member) [SKOS]
                    else if (annotatedProperty.Equals(RDFVocabulary.SKOS.MEMBER))
                        ontology.Data.AddMemberRelation(((RDFResource)annotatedSource).ToRDFOntologyIndividual(), ((RDFResource)annotatedTarget).ToRDFOntologyIndividual(), axiomAnnotation);
                    #endregion

                    #region Data(MemberList) [SKOS]
                    else if (annotatedProperty.Equals(RDFVocabulary.SKOS.MEMBER_LIST))
                        ontology.Data.AddMemberListRelation(((RDFResource)annotatedSource).ToRDFOntologyIndividual(), ((RDFResource)annotatedTarget).ToRDFOntologyIndividual(), axiomAnnotation);
                    #endregion

                    #region Data(Assertions)
                    else
                    {
                        if (annotatedTargetIsResource)
                            ontology.Data.AddObjectAssertion(((RDFResource)annotatedSource).ToRDFOntologyIndividual(), ((RDFResource)annotatedProperty).ToRDFOntologyObjectProperty(), ((RDFResource)annotatedTarget).ToRDFOntologyIndividual(), axiomAnnotation);
                        else
                            ontology.Data.AddDataAssertion(((RDFResource)annotatedSource).ToRDFOntologyIndividual(), ((RDFResource)annotatedProperty).ToRDFOntologyDatatypeProperty(), ((RDFLiteral)annotatedTarget).ToRDFOntologyLiteral(), axiomAnnotation);
                    }
                    #endregion
                }
            }
            #endregion
        }

        /// <summary>
        /// Ends the graph->ontology process
        /// </summary>
        private static void EndProcess(ref RDFOntology ontology)
        {
            //Unexpand from BASE ontology
            RDFPatternMember ontologyValue = ontology.Value;
            ontology = ontology.DifferenceWith(RDFBASEOntology.Instance);
            ontology.Value = ontologyValue;
        }

        /// <summary>
        /// Gets a graph representation of the given ontology, exporting inferences according to the selected behavior
        /// </summary>
        internal static RDFGraph ToRDFGraph(RDFOntology ontology, RDFSemanticsEnums.RDFOntologyInferenceExportBehavior infexpBehavior)
        {
            RDFGraph result = new RDFGraph();
            if (ontology != null)
            {
                #region Step 1: Export ontology
                result.AddTriple(new RDFTriple((RDFResource)ontology.Value, RDFVocabulary.RDF.TYPE, RDFVocabulary.OWL.ONTOLOGY));
                result = result.UnionWith(ontology.Annotations.VersionInfo.ReifyToRDFGraph(infexpBehavior, nameof(ontology.Annotations.VersionInfo)))
                               .UnionWith(ontology.Annotations.VersionIRI.ReifyToRDFGraph(infexpBehavior, nameof(ontology.Annotations.VersionIRI)))
                               .UnionWith(ontology.Annotations.Comment.ReifyToRDFGraph(infexpBehavior, nameof(ontology.Annotations.Comment)))
                               .UnionWith(ontology.Annotations.Label.ReifyToRDFGraph(infexpBehavior, nameof(ontology.Annotations.Label)))
                               .UnionWith(ontology.Annotations.SeeAlso.ReifyToRDFGraph(infexpBehavior, nameof(ontology.Annotations.SeeAlso)))
                               .UnionWith(ontology.Annotations.IsDefinedBy.ReifyToRDFGraph(infexpBehavior, nameof(ontology.Annotations.IsDefinedBy)))
                               .UnionWith(ontology.Annotations.BackwardCompatibleWith.ReifyToRDFGraph(infexpBehavior, nameof(ontology.Annotations.BackwardCompatibleWith)))
                               .UnionWith(ontology.Annotations.IncompatibleWith.ReifyToRDFGraph(infexpBehavior, nameof(ontology.Annotations.IncompatibleWith)))
                               .UnionWith(ontology.Annotations.PriorVersion.ReifyToRDFGraph(infexpBehavior, nameof(ontology.Annotations.PriorVersion)))
                               .UnionWith(ontology.Annotations.Imports.ReifyToRDFGraph(infexpBehavior, nameof(ontology.Annotations.Imports)))
                               .UnionWith(ontology.Annotations.CustomAnnotations.ReifyToRDFGraph(infexpBehavior, nameof(ontology.Annotations.CustomAnnotations)));
                #endregion

                #region Step 2: Export model
                result = result.UnionWith(ontology.Model.ToRDFGraph(infexpBehavior));
                #endregion

                #region Step 3: Export data
                result = result.UnionWith(ontology.Data.ToRDFGraph(infexpBehavior));
                #endregion

                #region Step 4: Finalize
                result.SetContext(((RDFResource)ontology.Value).URI);
                #endregion
            }
            return result;
        }
        #endregion
    }
}