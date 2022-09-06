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
using System.Globalization;
using System.Linq;
using RDFSharp.Model;
using RDFSharp.Query;

namespace RDFSharp.Semantics
{
    /// <summary>
    /// RDFOntologyClassModelLoader is responsible for loading ontology class models from remote sources or alternative representations
    /// </summary>
    internal static class RDFOntologyClassModelLoader
    {
        #region Methods
        /// <summary>
        /// Gets an ontology class model representation of the given graph
        /// </summary>
        internal static void LoadClassModel(this RDFOntology ontology, RDFGraph graph)
        {
            if (graph == null)
                throw new RDFSemanticsException("Cannot get ontology class model from RDFGraph because given \"graph\" parameter is null");

            RDFSemanticsEvents.RaiseSemanticsInfo(string.Format("Graph '{0}' is going to be parsed as ClassModel...", graph.Context));

            #region Declarations
            HashSet<long> annotationProperties = graph.GetAnnotationPropertyHashes();
            annotationProperties.UnionWith(RDFSemanticsUtilities.StandardResourceAnnotations);

            //Class
            List<RDFResource> classes = GetClassDeclarations(graph)
                                         .Union(GetRDFSClassDeclarations(graph))
                                          .ToList();
            foreach (RDFResource owlClass in classes)
                ontology.Model.ClassModel.DeclareClass(owlClass, GetClassBehavior(owlClass, graph));

            //Restriction
            foreach (RDFResource owlRestriction in GetRestrictionDeclarations(graph))
                ontology.LoadRestriction(owlRestriction, graph);

            //Enumerate
            foreach (RDFResource owlEnumerate in GetEnumerateDeclarations(graph))
                ontology.LoadEnumerateClass(owlEnumerate, graph);

            //Composite
            List<RDFResource> composites = GetCompositeUnionDeclarations(graph)
                                            .Union(GetCompositeIntersectionDeclarations(graph))
                                             .Union(GetCompositeComplementDeclarations(graph))
                                              .ToList();
            foreach (RDFResource owlComposite in composites)
                ontology.LoadCompositeClass(owlComposite, graph);

            //DisjointUnion [OWL2]
            foreach (RDFResource owlDisjointUnion in GetDisjointUnionDeclarations(graph))
                ontology.LoadDisjointUnionClass(owlDisjointUnion, graph);
            #endregion

            #region Taxonomies
            foreach (RDFResource owlClass in ontology.Model.ClassModel)
            {
                //Annotations
                foreach (RDFTriple classAnnotation in graph[owlClass, null, null, null].Where(t => annotationProperties.Contains(t.Predicate.PatternMemberID)))
                {
                    if (classAnnotation.TripleFlavor == RDFModelEnums.RDFTripleFlavors.SPO)
                        ontology.Model.ClassModel.AnnotateClass(owlClass, (RDFResource)classAnnotation.Predicate, (RDFResource)classAnnotation.Object);
                    else
                        ontology.Model.ClassModel.AnnotateClass(owlClass, (RDFResource)classAnnotation.Predicate, (RDFLiteral)classAnnotation.Object);
                }

                //Relations
                foreach (RDFTriple subClassRelation in graph[owlClass, RDFVocabulary.RDFS.SUB_CLASS_OF, null, null])
                    ontology.Model.ClassModel.DeclareSubClass(owlClass, (RDFResource)subClassRelation.Object);
                foreach (RDFTriple equivalentClassRelation in graph[owlClass, RDFVocabulary.OWL.EQUIVALENT_CLASS, null, null])
                    ontology.Model.ClassModel.DeclareEquivalentClasses(owlClass, (RDFResource)equivalentClassRelation.Object);
                foreach (RDFTriple disjointClassRelation in graph[owlClass, RDFVocabulary.OWL.DISJOINT_WITH, null, null])
                    ontology.Model.ClassModel.DeclareDisjointClasses(owlClass, (RDFResource)disjointClassRelation.Object);
                foreach (RDFTriple disjointUnionRelation in graph[owlClass, RDFVocabulary.OWL.DISJOINT_UNION_OF, null, null]) //OWL2
                {
                    List<RDFResource> disjointClasses = new List<RDFResource>();
                    RDFCollection disjointClassesCollection = RDFModelUtilities.DeserializeCollectionFromGraph(graph, (RDFResource)disjointUnionRelation.Object, RDFModelEnums.RDFTripleFlavors.SPO);
                    foreach (RDFPatternMember disjointClass in disjointClassesCollection)
                        disjointClasses.Add((RDFResource)disjointClass);
                    ontology.Model.ClassModel.DeclareDisjointUnionClass(owlClass, disjointClasses);
                }
                foreach (RDFTriple hasKeyRelation in graph[owlClass, RDFVocabulary.OWL.HAS_KEY, null, null]) //OWL2
                {
                    List<RDFResource> keyProperties = new List<RDFResource>();
                    RDFCollection keyPropertiesCollection = RDFModelUtilities.DeserializeCollectionFromGraph(graph, (RDFResource)hasKeyRelation.Object, RDFModelEnums.RDFTripleFlavors.SPO);
                    foreach (RDFPatternMember keyProperty in keyPropertiesCollection)
                        keyProperties.Add((RDFResource)keyProperty);
                    ontology.Model.ClassModel.DeclareHasKey(owlClass, keyProperties);
                }
            }

            //owl:AllDisjointClasses [OWL2]
            IEnumerator<RDFResource> allDisjointClasses = ontology.Model.ClassModel.AllDisjointClassesEnumerator;
            while (allDisjointClasses.MoveNext())
                foreach (RDFTriple allDisjointClassesMembers in graph[allDisjointClasses.Current, RDFVocabulary.OWL.MEMBERS, null, null])
                {
                    List<RDFResource> disjointClasses = new List<RDFResource>();
                    RDFCollection disjointClassesCollection = RDFModelUtilities.DeserializeCollectionFromGraph(graph, (RDFResource)allDisjointClassesMembers.Object, RDFModelEnums.RDFTripleFlavors.SPO);
                    foreach (RDFPatternMember disjointClass in disjointClassesCollection)
                        disjointClasses.Add((RDFResource)disjointClass);
                    ontology.Model.ClassModel.DeclareAllDisjointClasses(allDisjointClasses.Current, disjointClasses);
                }
            #endregion

            RDFSemanticsEvents.RaiseSemanticsInfo(string.Format("Graph '{0}' has been parsed as ClassModel", graph.Context));
        }
        #endregion

        #region Utilities
        /// <summary>
        /// Gets the owl:Class declarations
        /// </summary>
        private static HashSet<RDFResource> GetClassDeclarations(RDFGraph graph)
            => new HashSet<RDFResource>(graph[null, RDFVocabulary.RDF.TYPE, RDFVocabulary.OWL.CLASS, null]
                                           .Select(t => t.Subject)
                                           .OfType<RDFResource>());

        /// <summary>
        /// Gets the rdfs:Class declarations
        /// </summary>
        private static HashSet<RDFResource> GetRDFSClassDeclarations(RDFGraph graph)
            => new HashSet<RDFResource>(graph[null, RDFVocabulary.RDF.TYPE, RDFVocabulary.RDFS.CLASS, null]
                                           .Select(t => t.Subject)
                                           .OfType<RDFResource>());

        /// <summary>
        /// Gets the owl:Restriction declarations
        /// </summary>
        private static HashSet<RDFResource> GetRestrictionDeclarations(RDFGraph graph)
            => new HashSet<RDFResource>(graph[null, RDFVocabulary.RDF.TYPE, RDFVocabulary.OWL.RESTRICTION, null]
                                           .Select(t => t.Subject)
                                           .OfType<RDFResource>());

        /// <summary>
        /// Gets the owl:onProperty declaration of the given owl:Restriction
        /// </summary>
        private static RDFResource GetRestrictionProperty(RDFGraph graph, RDFResource owlRestriction)
            => graph[owlRestriction, RDFVocabulary.OWL.ON_PROPERTY, null, null]
                  .FirstOrDefault().Object as RDFResource;

        /// <summary>
        /// Gets the owl:allValuesFrom declaration of the given owl:Restriction
        /// </summary>
        private static RDFResource GetRestrictionAllValuesFromClass(RDFGraph graph, RDFResource owlRestriction)
            => graph[owlRestriction, RDFVocabulary.OWL.ALL_VALUES_FROM, null, null]
                  .FirstOrDefault().Object as RDFResource;

        /// <summary>
        /// Gets the owl:someValuesFrom declaration of the given owl:Restriction
        /// </summary>
        private static RDFResource GetRestrictionSomeValuesFromClass(RDFGraph graph, RDFResource owlRestriction)
            => graph[owlRestriction, RDFVocabulary.OWL.SOME_VALUES_FROM, null, null]
                  .FirstOrDefault().Object as RDFResource;

        /// <summary>
        /// Gets the owl:hasValue declaration of the given owl:Restriction
        /// </summary>
        private static RDFPatternMember GetRestrictionHasValue(RDFGraph graph, RDFResource owlRestriction)
            => graph[owlRestriction, RDFVocabulary.OWL.HAS_VALUE, null, null]
                  .FirstOrDefault().Object;

        /// <summary>
        /// Gets the owl:hasSelf declaration of the given owl:Restriction [OWL2]
        /// </summary>
        private static RDFTypedLiteral GetRestrictionHasSelf(RDFGraph graph, RDFResource owlRestriction)
            => graph[owlRestriction, RDFVocabulary.OWL.HAS_SELF, null, null]
                  .FirstOrDefault().Object as RDFTypedLiteral;

        /// <summary>
        /// Gets the owl:cardinality declaration of the given owl:Restriction
        /// </summary>
        private static RDFTypedLiteral GetRestrictionCardinality(RDFGraph graph, RDFResource owlRestriction)
            => graph[owlRestriction, RDFVocabulary.OWL.CARDINALITY, null, null]
                  .FirstOrDefault().Object as RDFTypedLiteral;

        /// <summary>
        /// Gets the owl:minCardinality declaration of the given owl:Restriction
        /// </summary>
        private static RDFTypedLiteral GetRestrictionMinCardinality(RDFGraph graph, RDFResource owlRestriction)
            => graph[owlRestriction, RDFVocabulary.OWL.MIN_CARDINALITY, null, null]
                  .FirstOrDefault().Object as RDFTypedLiteral;

        /// <summary>
        /// Gets the owl:maxCardinality declaration of the given owl:Restriction
        /// </summary>
        private static RDFTypedLiteral GetRestrictionMaxCardinality(RDFGraph graph, RDFResource owlRestriction)
            => graph[owlRestriction, RDFVocabulary.OWL.MAX_CARDINALITY, null, null]
                  .FirstOrDefault().Object as RDFTypedLiteral;

        /// <summary>
        /// Gets the owl:onClass declaration of the given owl:Restriction [OWL2]
        /// </summary>
        private static RDFResource GetRestrictionClass(RDFGraph graph, RDFResource owlRestriction)
            => graph[owlRestriction, RDFVocabulary.OWL.ON_CLASS, null, null]
                  .FirstOrDefault().Object as RDFResource;

        /// <summary>
        /// Gets the owl:qualifiedCardinality declaration of the given owl:Restriction [OWL2]
        /// </summary>
        private static RDFTypedLiteral GetRestrictionQualifiedCardinality(RDFGraph graph, RDFResource owlRestriction)
            => graph[owlRestriction, RDFVocabulary.OWL.QUALIFIED_CARDINALITY, null, null]
                  .FirstOrDefault().Object as RDFTypedLiteral;

        /// <summary>
        /// Gets the owl:minQualifiedCardinality declaration of the given owl:Restriction [OWL2]
        /// </summary>
        private static RDFTypedLiteral GetRestrictionMinQualifiedCardinality(RDFGraph graph, RDFResource owlRestriction)
            => graph[owlRestriction, RDFVocabulary.OWL.MIN_QUALIFIED_CARDINALITY, null, null]
                  .FirstOrDefault().Object as RDFTypedLiteral;

        /// <summary>
        /// Gets the owl:maxQualifiedCardinality declaration of the given owl:Restriction [OWL2]
        /// </summary>
        private static RDFTypedLiteral GetRestrictionMaxQualifiedCardinality(RDFGraph graph, RDFResource owlRestriction)
            => graph[owlRestriction, RDFVocabulary.OWL.MAX_QUALIFIED_CARDINALITY, null, null]
                  .FirstOrDefault().Object as RDFTypedLiteral;

        /// <summary>
        /// Gets the owl:oneOf declarations
        /// </summary>
        private static HashSet<RDFResource> GetEnumerateDeclarations(RDFGraph graph)
            => new HashSet<RDFResource>(graph[null, RDFVocabulary.OWL.ONE_OF, null, null]
                                           .Select(t => t.Subject)
                                           .OfType<RDFResource>());

        /// <summary>
        /// Gets the owl:unionOf declarations
        /// </summary>
        private static HashSet<RDFResource> GetCompositeUnionDeclarations(RDFGraph graph)
            => new HashSet<RDFResource>(graph[null, RDFVocabulary.OWL.UNION_OF, null, null]
                                           .Select(t => t.Subject)
                                           .OfType<RDFResource>());

        /// <summary>
        /// Gets the owl:intersectionOf declarations
        /// </summary>
        private static HashSet<RDFResource> GetCompositeIntersectionDeclarations(RDFGraph graph)
            => new HashSet<RDFResource>(graph[null, RDFVocabulary.OWL.INTERSECTION_OF, null, null]
                                           .Select(t => t.Subject)
                                           .OfType<RDFResource>());

        /// <summary>
        /// Gets the owl:complementOf declarations
        /// </summary>
        private static HashSet<RDFResource> GetCompositeComplementDeclarations(RDFGraph graph)
            => new HashSet<RDFResource>(graph[null, RDFVocabulary.OWL.COMPLEMENT_OF, null, null]
                                           .Select(t => t.Subject)
                                           .OfType<RDFResource>());

        /// <summary>
        /// Gets the owl:disjointUnionOf declarations [OWL2]
        /// </summary>
        private static HashSet<RDFResource> GetDisjointUnionDeclarations(RDFGraph graph)
            => new HashSet<RDFResource>(graph[null, RDFVocabulary.OWL.DISJOINT_UNION_OF, null, null]
                                           .Select(t => t.Subject)
                                           .OfType<RDFResource>());

        /// <summary>
        /// Gets the behavior of the given class
        /// </summary>
        private static RDFOntologyClassBehavior GetClassBehavior(RDFResource owlClass, RDFGraph graph)
            => new RDFOntologyClassBehavior()
            {
                Deprecated = graph.ContainsTriple(new RDFTriple(owlClass, RDFVocabulary.RDF.TYPE, RDFVocabulary.OWL.DEPRECATED_CLASS))
            };

        /// <summary>
        /// Loads the definition of the given restriction class
        /// </summary>
        private static void LoadRestriction(this RDFOntology ontology, RDFResource owlRestriction, RDFGraph graph)
        {
            //Get mandatory owl:onProperty information
            RDFResource onProperty = GetRestrictionProperty(graph, owlRestriction);
            if (onProperty == null)
                throw new RDFSemanticsException($"Cannot load Restriction '{owlRestriction}' from graph because it does not have required owl:onProperty information");

            //Try load the given restriction as instance of owl:[all|some]ValuesFrom
            if (TryLoadAllValuesFromRestriction(ontology, owlRestriction, onProperty, graph)
                 || TryLoadSomeValuesFromRestriction(ontology, owlRestriction, onProperty, graph))
                return;

            //Try load the given restriction as instance of owl:hasValue
            if (TryLoadHasValueRestriction(ontology, owlRestriction, onProperty, graph))
                return;

            //Try load the given restriction as instance of owl:hasSelf [OWL2]
            if (TryLoadHasSelfRestriction(ontology, owlRestriction, onProperty, graph))
                return;

            //Try load the given restriction as instance of owl:cardinality
            if (TryLoadCardinalityRestriction(ontology, owlRestriction, onProperty, graph))
                return;

            //Try load the given restriction as instance of owl:[min|max]Cardinality
            if (TryLoadMinMaxCardinalityRestriction(ontology, owlRestriction, onProperty, graph))
                return;

            //Try load the given restriction as instance of owl:qualifiedCardinality [OWL2]
            if (TryLoadQualifiedCardinalityRestriction(ontology, owlRestriction, onProperty, graph))
                return;

            //Try load the given restriction as instance of owl:[min|max]QualifiedCardinality [OWL2]
            TryLoadMinMaxQualifiedCardinalityRestriction(ontology, owlRestriction, onProperty, graph);
        }

        /// <summary>
        /// Tries to load the given owl:Restriction as instance of owl:allValuesFrom restriction
        /// </summary>
        private static bool TryLoadAllValuesFromRestriction(this RDFOntology ontology, RDFResource owlRestriction, RDFResource onProperty, RDFGraph graph)
        {
            RDFResource allValuesFromClass = GetRestrictionAllValuesFromClass(graph, owlRestriction);
            if (allValuesFromClass != null)
            {
                ontology.Model.ClassModel.DeclareAllValuesFromRestriction(owlRestriction, onProperty, allValuesFromClass);
                return true;
            }
            return false;
        }

        /// <summary>
        /// Tries to load the given owl:Restriction as instance of owl:someValuesFrom restriction
        /// </summary>
        private static bool TryLoadSomeValuesFromRestriction(this RDFOntology ontology, RDFResource owlRestriction, RDFResource onProperty, RDFGraph graph)
        {
            RDFResource someValuesFromClass = GetRestrictionSomeValuesFromClass(graph, owlRestriction);
            if (someValuesFromClass != null)
            {
                ontology.Model.ClassModel.DeclareSomeValuesFromRestriction(owlRestriction, onProperty, someValuesFromClass);
                return true;
            }
            return false;
        }

        /// <summary>
        /// Tries to load the given owl:Restriction as instance of owl:hasValue restriction
        /// </summary>
        private static bool TryLoadHasValueRestriction(this RDFOntology ontology, RDFResource owlRestriction, RDFResource onProperty, RDFGraph graph)
        {
            RDFPatternMember hasValue = GetRestrictionHasValue(graph, owlRestriction);
            if (hasValue != null)
            {
                if (hasValue is RDFResource)
                    ontology.Model.ClassModel.DeclareHasValueRestriction(owlRestriction, onProperty, (RDFResource)hasValue);
                else
                    ontology.Model.ClassModel.DeclareHasValueRestriction(owlRestriction, onProperty, (RDFLiteral)hasValue);
                return true;
            }
            return false;
        }

        /// <summary>
        /// Tries to load the given owl:Restriction as instance of owl:hasSelf restriction [OWL2]
        /// </summary>
        private static bool TryLoadHasSelfRestriction(this RDFOntology ontology, RDFResource owlRestriction, RDFResource onProperty, RDFGraph graph)
        {
            RDFTypedLiteral hasSelf = GetRestrictionHasSelf(graph, owlRestriction);
            if (hasSelf != null)
            {
                if (hasSelf.Equals(RDFTypedLiteral.True))
                {
                    ontology.Model.ClassModel.DeclareHasSelfRestriction(owlRestriction, onProperty, true);
                    return true;
                }
                else if (hasSelf.Equals(RDFTypedLiteral.False))
                {
                    ontology.Model.ClassModel.DeclareHasSelfRestriction(owlRestriction, onProperty, false);
                    return true;
                }
            }
            return false;
        }

        /// <summary>
        /// Tries to load the given owl:Restriction as instance of owl:cardinality restriction
        /// </summary>
        private static bool TryLoadCardinalityRestriction(this RDFOntology ontology, RDFResource owlRestriction, RDFResource onProperty, RDFGraph graph)
        {
            RDFTypedLiteral cardinality = GetRestrictionCardinality(graph, owlRestriction);
            if (cardinality != null && cardinality.Datatype == RDFModelEnums.RDFDatatypes.XSD_NONNEGATIVEINTEGER)
            {
                uint cardinalityValue = uint.Parse(cardinality.Value, NumberStyles.Integer, CultureInfo.InvariantCulture);
                ontology.Model.ClassModel.DeclareCardinalityRestriction(owlRestriction, onProperty, cardinalityValue);
                return true;
            }
            return false;
        }

        /// <summary>
        /// Tries to load the given owl:Restriction as instance of owl:[min|max]Cardinality restriction
        /// </summary>
        private static bool TryLoadMinMaxCardinalityRestriction(this RDFOntology ontology, RDFResource owlRestriction, RDFResource onProperty, RDFGraph graph)
        {
            //Try detect owl:minCardinality
            uint minCardinalityValue = 0;
            bool hasMinCardinality = false;            
            RDFTypedLiteral minCardinality = GetRestrictionMinCardinality(graph, owlRestriction);
            if (minCardinality != null && minCardinality.Datatype == RDFModelEnums.RDFDatatypes.XSD_NONNEGATIVEINTEGER)
            {
                minCardinalityValue = uint.Parse(minCardinality.Value, NumberStyles.Integer, CultureInfo.InvariantCulture);
                hasMinCardinality = true;
            }

            //Try detect owl:maxCardinality
            uint maxCardinalityValue = 0;
            bool hasMaxCardinality = false;
            RDFTypedLiteral maxCardinality = GetRestrictionMaxCardinality(graph, owlRestriction);
            if (maxCardinality != null && maxCardinality.Datatype == RDFModelEnums.RDFDatatypes.XSD_NONNEGATIVEINTEGER)
            {
                maxCardinalityValue = uint.Parse(maxCardinality.Value, NumberStyles.Integer, CultureInfo.InvariantCulture);
                hasMaxCardinality = true;
            }
            
            if (hasMinCardinality && !hasMaxCardinality)
            {
                ontology.Model.ClassModel.DeclareMinCardinalityRestriction(owlRestriction, onProperty, minCardinalityValue);
                return true;
            }
            else if (!hasMinCardinality && hasMaxCardinality)
            {
                ontology.Model.ClassModel.DeclareMaxCardinalityRestriction(owlRestriction, onProperty, maxCardinalityValue);
                return true;
            }
            else if (hasMinCardinality && hasMaxCardinality)
            {
                ontology.Model.ClassModel.DeclareMinMaxCardinalityRestriction(owlRestriction, onProperty, minCardinalityValue, maxCardinalityValue);
                return true;
            }
            return false;
        }

        /// <summary>
        /// Tries to load the given owl:Restriction as instance of owl:qualifiedCardinality restriction [OWL2]
        /// </summary>
        private static bool TryLoadQualifiedCardinalityRestriction(this RDFOntology ontology, RDFResource owlRestriction, RDFResource onProperty, RDFGraph graph)
        {
            //Get mandatory owl:onClass information
            RDFResource onClass = GetRestrictionClass(graph, owlRestriction);
            if (onClass == null)
                throw new RDFSemanticsException($"Cannot load qualified Restriction '{owlRestriction}' from graph because it does not have required owl:onClass information");

            RDFTypedLiteral qualifiedCardinality = GetRestrictionQualifiedCardinality(graph, owlRestriction);
            if (qualifiedCardinality != null && qualifiedCardinality.Datatype == RDFModelEnums.RDFDatatypes.XSD_NONNEGATIVEINTEGER)
            {
                uint qualifiedCardinalityValue = uint.Parse(qualifiedCardinality.Value, NumberStyles.Integer, CultureInfo.InvariantCulture);
                ontology.Model.ClassModel.DeclareQualifiedCardinalityRestriction(owlRestriction, onProperty, qualifiedCardinalityValue, onClass);
                return true;
            }
            return false;
        }

        /// <summary>
        /// Tries to load the given owl:Restriction as instance of owl:[min|max]QualifiedCardinality restriction [OWL2]
        /// </summary>
        private static bool TryLoadMinMaxQualifiedCardinalityRestriction(this RDFOntology ontology, RDFResource owlRestriction, RDFResource onProperty, RDFGraph graph)
        {
            //Get mandatory owl:onClass information
            RDFResource onClass = GetRestrictionClass(graph, owlRestriction);
            if (onClass == null)
                throw new RDFSemanticsException($"Cannot load qualified Restriction '{owlRestriction}' from graph because it does not have required owl:onClass information");

            //Try detect owl:minQualifiedCardinality
            uint minQualifiedCardinalityValue = 0;
            bool hasMinQualifiedCardinality = false;
            RDFTypedLiteral minQualifiedCardinality = GetRestrictionMinQualifiedCardinality(graph, owlRestriction);
            if (minQualifiedCardinality != null && minQualifiedCardinality.Datatype == RDFModelEnums.RDFDatatypes.XSD_NONNEGATIVEINTEGER)
            {
                minQualifiedCardinalityValue = uint.Parse(minQualifiedCardinality.Value, NumberStyles.Integer, CultureInfo.InvariantCulture);
                hasMinQualifiedCardinality = true;
            }

            //Try detect owl:maxQualifiedCardinality
            uint maxQualifiedCardinalityValue = 0;
            bool hasMaxQualifiedCardinality = false;
            RDFTypedLiteral maxQualifiedCardinality = GetRestrictionMaxQualifiedCardinality(graph, owlRestriction);
            if (maxQualifiedCardinality != null && maxQualifiedCardinality.Datatype == RDFModelEnums.RDFDatatypes.XSD_NONNEGATIVEINTEGER)
            {
                maxQualifiedCardinalityValue = uint.Parse(maxQualifiedCardinality.Value, NumberStyles.Integer, CultureInfo.InvariantCulture);
                hasMaxQualifiedCardinality = true;
            }

            if (hasMinQualifiedCardinality && !hasMaxQualifiedCardinality)
            {
                ontology.Model.ClassModel.DeclareMinQualifiedCardinalityRestriction(owlRestriction, onProperty, minQualifiedCardinalityValue, onClass);
                return true;
            }
            else if (!hasMinQualifiedCardinality && hasMaxQualifiedCardinality)
            {
                ontology.Model.ClassModel.DeclareMaxQualifiedCardinalityRestriction(owlRestriction, onProperty, maxQualifiedCardinalityValue, onClass);
                return true;
            }
            else if (hasMinQualifiedCardinality && hasMaxQualifiedCardinality)
            {
                ontology.Model.ClassModel.DeclareMinMaxQualifiedCardinalityRestriction(owlRestriction, onProperty, minQualifiedCardinalityValue, maxQualifiedCardinalityValue, onClass);
                return true;
            }
            return false;
        }

        /// <summary>
        /// Loads the definition of the given enumerate class
        /// </summary>
        private static void LoadEnumerateClass(this RDFOntology ontology, RDFResource owlEnumerate, RDFGraph graph)
        {
            RDFResource oneOfRepresentative = graph[owlEnumerate, RDFVocabulary.OWL.ONE_OF, null, null].FirstOrDefault()?.Object as RDFResource;
            if (oneOfRepresentative != null)
            {
                List<RDFResource> oneOfMembers = new List<RDFResource>();
                RDFCollection oneOfMembersCollection = RDFModelUtilities.DeserializeCollectionFromGraph(graph, oneOfRepresentative, RDFModelEnums.RDFTripleFlavors.SPO);
                foreach (RDFPatternMember oneOfMember in oneOfMembersCollection)
                    oneOfMembers.Add((RDFResource)oneOfMember);
                ontology.Model.ClassModel.DeclareEnumerateClass(owlEnumerate, oneOfMembers);
            }
        }

        /// <summary>
        /// Loads the definition of the given composite class
        /// </summary>
        private static void LoadCompositeClass(this RDFOntology ontology, RDFResource owlComposite, RDFGraph graph)
        {
            #region owl:unionOf
            RDFResource unionRepresentative = graph[owlComposite, RDFVocabulary.OWL.UNION_OF, null, null].FirstOrDefault()?.Object as RDFResource;
            if (unionRepresentative != null)
            {
                List<RDFResource> unionMembers = new List<RDFResource>();
                RDFCollection unionMembersCollection = RDFModelUtilities.DeserializeCollectionFromGraph(graph, unionRepresentative, RDFModelEnums.RDFTripleFlavors.SPO);
                foreach (RDFPatternMember unionMember in unionMembersCollection)
                    unionMembers.Add((RDFResource)unionMember);
                ontology.Model.ClassModel.DeclareUnionClass(owlComposite, unionMembers);
                return;
            }
            #endregion

            #region owl:intersectionOf
            RDFResource intersectionRepresentative = graph[owlComposite, RDFVocabulary.OWL.INTERSECTION_OF, null, null].FirstOrDefault()?.Object as RDFResource;
            if (intersectionRepresentative != null)
            {
                List<RDFResource> intersectionMembers = new List<RDFResource>();
                RDFCollection intersectionMembersCollection = RDFModelUtilities.DeserializeCollectionFromGraph(graph, intersectionRepresentative, RDFModelEnums.RDFTripleFlavors.SPO);
                foreach (RDFPatternMember intersectionMember in intersectionMembersCollection)
                    intersectionMembers.Add((RDFResource)intersectionMember);
                ontology.Model.ClassModel.DeclareIntersectionClass(owlComposite, intersectionMembers);
                return;
            }
            #endregion

            #region owl:complementOf
            RDFResource complementClass = graph[owlComposite, RDFVocabulary.OWL.COMPLEMENT_OF, null, null].FirstOrDefault()?.Object as RDFResource;
            if (complementClass != null)
                ontology.Model.ClassModel.DeclareComplementClass(owlComposite, complementClass);
            #endregion
        }

        /// <summary>
        /// Loads the definition of the given disjoint union class [OWL2]
        /// </summary>
        private static void LoadDisjointUnionClass(this RDFOntology ontology, RDFResource owlDisjointUnion, RDFGraph graph)
        {
            RDFResource disjointUnionRepresentative = graph[owlDisjointUnion, RDFVocabulary.OWL.DISJOINT_UNION_OF, null, null].FirstOrDefault()?.Object as RDFResource;
            if (disjointUnionRepresentative != null)
            {
                List<RDFResource> disjointUnionMembers = new List<RDFResource>();
                RDFCollection disjointUnionMembersCollection = RDFModelUtilities.DeserializeCollectionFromGraph(graph, disjointUnionRepresentative, RDFModelEnums.RDFTripleFlavors.SPO);
                foreach (RDFPatternMember disjointUnionMember in disjointUnionMembersCollection)
                    disjointUnionMembers.Add((RDFResource)disjointUnionMember);
                ontology.Model.ClassModel.DeclareDisjointUnionClass(owlDisjointUnion, disjointUnionMembers);
            }
        }
        #endregion
    }
}