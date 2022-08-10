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
using System.Data;
using System.Linq;

namespace RDFSharp.Semantics
{
    /// <summary>
    /// RDFOntologyDataHelper contains utility methods supporting RDFS/OWL-DL modeling, validation and reasoning (A-BOX)
    /// </summary>
    public static class RDFOntologyDataHelper
    {
        #region SameAs
        /// <summary>
        /// Checks if the given aIndividual is sameAs the given bIndividual within the given data
        /// </summary>
        public static bool CheckIsSameIndividual(this RDFOntologyData data, RDFOntologyIndividual aIndividual, RDFOntologyIndividual bIndividual)
            => aIndividual != null && bIndividual != null && data != null && data.GetSameIndividuals(aIndividual).Individuals.ContainsKey(bIndividual.PatternMemberID);

        /// <summary>
        /// Enlists the same individuals of the given individual within the given data
        /// </summary>
        public static RDFOntologyData GetSameIndividuals(this RDFOntologyData data, RDFOntologyIndividual ontologyIndividual)
        {
            RDFOntologyData result = new RDFOntologyData();

            if (ontologyIndividual != null && data != null)
                result = data.GetSameIndividualsInternal(ontologyIndividual, null)
                             .RemoveIndividual(ontologyIndividual); //Safety deletion

            return result;
        }

        /// <summary>
        /// Subsumes the "owl:sameAs" taxonomy to discover direct and indirect sameindividuals of the given individuals
        /// </summary>
        internal static RDFOntologyData GetSameIndividualsInternal(this RDFOntologyData data, RDFOntologyIndividual ontologyIndividual, Dictionary<long, RDFOntologyIndividual> visitContext)
        {
            RDFOntologyData result = new RDFOntologyData();

            #region visitContext
            if (visitContext == null)
                visitContext = new Dictionary<long, RDFOntologyIndividual>() { { ontologyIndividual.PatternMemberID, ontologyIndividual } };
            else
            {
                if (!visitContext.ContainsKey(ontologyIndividual.PatternMemberID))
                    visitContext.Add(ontologyIndividual.PatternMemberID, ontologyIndividual);
                else
                    return result;
            }
            #endregion

            // Transitivity of "owl:sameAs" taxonomy: ((A SAMEAS B)  &&  (B SAMEAS C))  =>  (A SAMEAS C)
            foreach (RDFOntologyTaxonomyEntry sameAs in data.Relations.SameAs.SelectEntriesBySubject(ontologyIndividual))
            {
                result.AddIndividual((RDFOntologyIndividual)sameAs.TaxonomyObject);
                result = result.UnionWith(data.GetSameIndividualsInternal((RDFOntologyIndividual)sameAs.TaxonomyObject, visitContext));
            }

            return result;
        }
        #endregion

        #region DifferentFrom
        /// <summary>
        /// Checks if the given aIndividual is differentFrom the given bIndividual within the given data
        /// </summary>
        public static bool CheckIsDifferentIndividual(this RDFOntologyData data, RDFOntologyIndividual aIndividual, RDFOntologyIndividual bIndividual)
            => aIndividual != null && bIndividual != null && data != null && data.GetDifferentIndividuals(aIndividual).Individuals.ContainsKey(bIndividual.PatternMemberID);

        /// <summary>
        /// Enlists the different individuals of the given individual within the given data
        /// </summary>
        public static RDFOntologyData GetDifferentIndividuals(this RDFOntologyData data, RDFOntologyIndividual ontologyIndividual)
        {
            RDFOntologyData result = new RDFOntologyData();

            if (ontologyIndividual != null && data != null)
                result = data.GetDifferentIndividualsInternal(ontologyIndividual, null)
                             .RemoveIndividual(ontologyIndividual); //Safety deletion

            return result;
        }

        /// <summary>
        /// Subsumes the "owl:differentFrom" taxonomy to discover direct and indirect different individuals of the given individuals
        /// </summary>
        internal static RDFOntologyData GetDifferentIndividualsInternal(this RDFOntologyData data, RDFOntologyIndividual ontologyIndividual, Dictionary<long, RDFOntologyIndividual> visitContext)
        {
            RDFOntologyData result = new RDFOntologyData();

            #region visitContext
            if (visitContext == null)
                visitContext = new Dictionary<long, RDFOntologyIndividual>() { { ontologyIndividual.PatternMemberID, ontologyIndividual } };
            else
            {
                if (!visitContext.ContainsKey(ontologyIndividual.PatternMemberID))
                    visitContext.Add(ontologyIndividual.PatternMemberID, ontologyIndividual);
                else
                    return result;
            }
            #endregion

            // Inference: (A DIFFERENTFROM B  &&  B SAMEAS C         =>  A DIFFERENTFROM C)
            foreach (RDFOntologyTaxonomyEntry differentFrom in data.Relations.DifferentFrom.SelectEntriesBySubject(ontologyIndividual))
            {
                result.AddIndividual((RDFOntologyIndividual)differentFrom.TaxonomyObject);
                result = result.UnionWith(data.GetSameIndividualsInternal((RDFOntologyIndividual)differentFrom.TaxonomyObject, visitContext));
            }

            // Inference: (A SAMEAS B         &&  B DIFFERENTFROM C  =>  A DIFFERENTFROM C)
            foreach (RDFOntologyIndividual sameAs in data.GetSameIndividuals(ontologyIndividual))
                result = result.UnionWith(data.GetDifferentIndividualsInternal(sameAs, visitContext));

            return result;
        }
        #endregion

        #region TransitiveProperty
        /// <summary>
        /// Checks if the given "aIndividual -> objectProperty" assertion links to the given bIndividual within the given data
        /// </summary>
        public static bool CheckIsTransitiveObjectAssertion(this RDFOntologyData data, RDFOntologyIndividual aIndividual, RDFOntologyObjectProperty objectProperty, RDFOntologyIndividual bIndividual)
            => aIndividual != null && objectProperty != null && objectProperty.IsTransitiveProperty() && bIndividual != null && data != null && data.GetTransitiveObjectAssertions(aIndividual, objectProperty).Individuals.ContainsKey(bIndividual.PatternMemberID);

        /// <summary>
        /// Enlists the given "aIndividual -> objectProperty" assertions within the given data
        /// </summary>
        public static RDFOntologyData GetTransitiveObjectAssertions(this RDFOntologyData data, RDFOntologyIndividual ontologyIndividual, RDFOntologyObjectProperty objectProperty)
        {
            RDFOntologyData result = new RDFOntologyData();

            if (ontologyIndividual != null && objectProperty != null && objectProperty.IsTransitiveProperty() && data != null)
                result = data.GetTransitiveObjectAssertionsInternal(ontologyIndividual, objectProperty, null);

            return result;
        }

        /// <summary>
        /// Enlists the transitive assertions of the given individual and the given property within the given data
        /// </summary>
        internal static RDFOntologyData GetTransitiveObjectAssertionsInternal(this RDFOntologyData data, RDFOntologyIndividual ontologyIndividual, RDFOntologyObjectProperty objectProperty, Dictionary<long, RDFOntologyIndividual> visitContext)
        {
            RDFOntologyData result = new RDFOntologyData();

            #region visitContext
            if (visitContext == null)
                visitContext = new Dictionary<long, RDFOntologyIndividual>() { { ontologyIndividual.PatternMemberID, ontologyIndividual } };
            else
            {
                if (!visitContext.ContainsKey(ontologyIndividual.PatternMemberID))
                    visitContext.Add(ontologyIndividual.PatternMemberID, ontologyIndividual);
                else
                    return result;
            }
            #endregion

            // ((F1 P F2)    &&  (F2 P F3))  =>  (F1 P F3)
            foreach (RDFOntologyTaxonomyEntry assertion in data.Relations.Assertions.SelectEntriesBySubject(ontologyIndividual)
                                                                                    .SelectEntriesByPredicate(objectProperty))
            {
                result.AddIndividual((RDFOntologyIndividual)assertion.TaxonomyObject);
                result = result.UnionWith(data.GetTransitiveObjectAssertionsInternal((RDFOntologyIndividual)assertion.TaxonomyObject, objectProperty, visitContext));
            }

            return result;
        }
        #endregion

        #region Assertions
        /// <summary>
        /// Checks if the given "aIndividual -> objectProperty -> bIndividual" is an assertion within the given data
        /// </summary>
        public static bool CheckIsObjectAssertion(this RDFOntologyData data, RDFOntologyIndividual aIndividual, RDFOntologyObjectProperty objectProperty, RDFOntologyIndividual bIndividual)
        {
            if (aIndividual != null && bIndividual != null && objectProperty != null && data != null)
            {
                //Reason over subject/object individuals to detect indirect potential taxonomy violations
                RDFOntologyData compatibleSubjects = data.GetSameIndividuals(aIndividual).AddIndividual(aIndividual);
                RDFOntologyData compatibleObjects = data.GetSameIndividuals(aIndividual).AddIndividual(bIndividual);

                return data.Relations.Assertions.Any(asn => compatibleSubjects.Any(x => x.Equals(asn.TaxonomySubject))
                                                               && asn.TaxonomyPredicate.Equals(objectProperty)
                                                                   && compatibleObjects.Any(x => x.Equals(asn.TaxonomyObject)));
            }
            return false;
        }

        /// <summary>
        /// Checks if the given "aIndividual -> datatypeProperty -> ontologyLiteral" is an assertion within the given data
        /// </summary>
        public static bool CheckIsDataAssertion(this RDFOntologyData data, RDFOntologyIndividual aIndividual, RDFOntologyDatatypeProperty datatypeProperty, RDFOntologyLiteral ontologyLiteral)
        {
            if (aIndividual != null && ontologyLiteral != null && datatypeProperty != null && data != null)
            {
                //Reason over subject individuals to detect indirect potential taxonomy violations
                RDFOntologyData compatibleSubjects = data.GetSameIndividuals(aIndividual).AddIndividual(aIndividual);

                return data.Relations.Assertions.Any(asn => compatibleSubjects.Any(x => x.Equals(asn.TaxonomySubject))
                                                               && asn.TaxonomyPredicate.Equals(datatypeProperty)
                                                                   && asn.TaxonomyObject.Equals(ontologyLiteral));
            }
            return false;
        }

        /// <summary>
        /// Checks if the given "aIndividual -> objectProperty -> bIndividual" is a negative assertion within the given data
        /// </summary>
        public static bool CheckIsNegativeObjectAssertion(this RDFOntologyData data, RDFOntologyIndividual aIndividual, RDFOntologyObjectProperty objectProperty, RDFOntologyIndividual bIndividual)
        {
            if (aIndividual != null && bIndividual != null && objectProperty != null && data != null)
            {
                //Reason over subject/object individuals to detect indirect potential taxonomy violations
                RDFOntologyData compatibleSubjects = data.GetSameIndividuals(aIndividual).AddIndividual(aIndividual);
                RDFOntologyData compatibleObjects = data.GetSameIndividuals(aIndividual).AddIndividual(bIndividual);

                return data.Relations.NegativeAssertions.Any(nasn => compatibleSubjects.Any(x => x.Equals(nasn.TaxonomySubject))
                                                                       && nasn.TaxonomyPredicate.Equals(objectProperty)
                                                                           && compatibleObjects.Any(x => x.Equals(nasn.TaxonomyObject)));
            }
            return false;
        }

        /// <summary>
        /// Checks if the given "aIndividual -> datatypeProperty -> ontologyLiteral" is a negative assertion within the given data
        /// </summary>
        public static bool CheckIsNegativeDataAssertion(this RDFOntologyData data, RDFOntologyIndividual aIndividual, RDFOntologyDatatypeProperty datatypeProperty, RDFOntologyLiteral ontologyLiteral)
        {
            if (aIndividual != null && ontologyLiteral != null && datatypeProperty != null && data != null)
            {
                //Reason over subject individuals to detect indirect potential taxonomy violations
                RDFOntologyData compatibleSubjects = data.GetSameIndividuals(aIndividual).AddIndividual(aIndividual);

                return data.Relations.NegativeAssertions.Any(nasn => compatibleSubjects.Any(x => x.Equals(nasn.TaxonomySubject))
                                                                       && nasn.TaxonomyPredicate.Equals(datatypeProperty)
                                                                           && nasn.TaxonomyObject.Equals(ontologyLiteral));
            }
            return false;
        }
        #endregion

        #region MemberOf
        /// <summary>
        /// Checks if the given individual is member of the given class within the given ontology
        /// </summary>
        public static bool CheckIsMemberOf(this RDFOntology ontology, RDFOntologyIndividual ontologyIndividual, RDFOntologyClass ontologyClass)
            => ontologyIndividual != null && ontologyClass != null && ontology != null && ontology.GetMembersOf(ontologyClass).Individuals.ContainsKey(ontologyIndividual.PatternMemberID);

        /// <summary>
        /// Enlists the individuals which are members of the given class within the given ontology
        /// </summary>
        public static RDFOntologyData GetMembersOf(this RDFOntology ontology, RDFOntologyClass ontologyClass)
        {
            RDFOntologyData result = new RDFOntologyData();

            if (ontologyClass != null && ontology != null)
            {
                //Expand ontology
                RDFOntology expOnt = ontology.UnionWith(RDFBASEOntology.Instance);

                //DataRange/Literal-Compatible
                if (expOnt.Model.ClassModel.CheckIsLiteralCompatibleClass(ontologyClass))
                    result = expOnt.GetMembersOfLiteralCompatibleClass(ontologyClass);

                //Restriction/Composite/Enumerate/Class
                else
                    result = expOnt.GetMembersOfNonLiteralCompatibleClass(ontologyClass);
            }

            return result;
        }

        /// <summary>
        /// Enlists the individuals which are members of the given class within the given ontology
        /// </summary>
        internal static RDFOntologyData GetMembersOfClass(this RDFOntology ontology, RDFOntologyClass ontologyClass)
        {
            RDFOntologyData result = new RDFOntologyData();

            //Get the compatible classes
            RDFOntologyClassModel compatibleClasses = ontology.Model.ClassModel.GetSubClassesOf(ontologyClass)
                                                                               .UnionWith(ontology.Model.ClassModel.GetEquivalentClassesOf(ontologyClass))
                                                                               .AddClass(ontologyClass);

            //Get the individuals belonging to compatible classes
            List<RDFOntologyResource> compatibleIndividuals = ontology.Data.Relations.ClassType.Where(te => compatibleClasses.Any(c => c.Equals(te.TaxonomyObject)))
                                                                                               .Select(te => te.TaxonomySubject)
                                                                                               .ToList();

            //Add the individual and its synonyms
            Dictionary<long, RDFOntologyData> sameIndividualsCache = new Dictionary<long, RDFOntologyData>();
            foreach (RDFOntologyResource compatibleIndividual in compatibleIndividuals)
            {
                if (!sameIndividualsCache.ContainsKey(compatibleIndividual.PatternMemberID))
                {
                    sameIndividualsCache.Add(compatibleIndividual.PatternMemberID, ontology.Data.GetSameIndividuals((RDFOntologyIndividual)compatibleIndividual));

                    result = result.UnionWith(sameIndividualsCache[compatibleIndividual.PatternMemberID])
                                   .AddIndividual((RDFOntologyIndividual)compatibleIndividual);
                }
            }

            return result;
        }

        /// <summary>
        /// Enlists the individuals which are members of the given composite within the given ontology.
        /// </summary>
        internal static RDFOntologyData GetMembersOfComposite(this RDFOntology ontology, RDFOntologyClass ontologyCompositeClass, Dictionary<long, RDFOntologyData> membersCache = null)
        {
            RDFOntologyData result = new RDFOntologyData();

            #region Intersection
            if (ontologyCompositeClass is RDFOntologyIntersectionClass)
            {
                //Filter "intersectionOf" relations made with the given intersection class
                bool firstIter = true;
                RDFOntologyTaxonomy iTaxonomy = ontology.Model.ClassModel.Relations.IntersectionOf.SelectEntriesBySubject(ontologyCompositeClass);
                foreach (RDFOntologyTaxonomyEntry tEntry in iTaxonomy)
                {
                    if (firstIter)
                    {
                        if (membersCache != null)
                        {
                            if (!membersCache.ContainsKey(tEntry.TaxonomyObject.PatternMemberID))
                                membersCache.Add(tEntry.TaxonomyObject.PatternMemberID, ontology.GetMembersOf((RDFOntologyClass)tEntry.TaxonomyObject));

                            result = membersCache[tEntry.TaxonomyObject.PatternMemberID];
                        }
                        else
                            result = ontology.GetMembersOf((RDFOntologyClass)tEntry.TaxonomyObject);

                        firstIter = false;
                    }
                    else
                    {
                        if (membersCache != null)
                        {
                            if (!membersCache.ContainsKey(tEntry.TaxonomyObject.PatternMemberID))
                                membersCache.Add(tEntry.TaxonomyObject.PatternMemberID, ontology.GetMembersOf((RDFOntologyClass)tEntry.TaxonomyObject));

                            result = result.IntersectWith(membersCache[tEntry.TaxonomyObject.PatternMemberID]);
                        }
                        else
                            result = result.IntersectWith(ontology.GetMembersOf((RDFOntologyClass)tEntry.TaxonomyObject));
                    }
                }
            }
            #endregion

            #region Union
            else if (ontologyCompositeClass is RDFOntologyUnionClass)
            {
                //Filter "unionOf" relations made with the given union class
                RDFOntologyTaxonomy uTaxonomy = ontology.Model.ClassModel.Relations.UnionOf.SelectEntriesBySubject(ontologyCompositeClass);
                foreach (RDFOntologyTaxonomyEntry tEntry in uTaxonomy)
                {
                    if (membersCache != null)
                    {
                        if (!membersCache.ContainsKey(tEntry.TaxonomyObject.PatternMemberID))
                            membersCache.Add(tEntry.TaxonomyObject.PatternMemberID, ontology.GetMembersOf((RDFOntologyClass)tEntry.TaxonomyObject));

                        result = result.UnionWith(membersCache[tEntry.TaxonomyObject.PatternMemberID]);
                    }
                    else
                        result = result.UnionWith(ontology.GetMembersOf((RDFOntologyClass)tEntry.TaxonomyObject));
                }
            }
            #endregion

            #region Complement
            else if (ontologyCompositeClass is RDFOntologyComplementClass ontologyComplementClass)
            {
                if (membersCache != null)
                {
                    if (!membersCache.ContainsKey(ontologyComplementClass.ComplementOf.PatternMemberID))
                        membersCache.Add(ontologyComplementClass.ComplementOf.PatternMemberID, ontology.GetMembersOf(ontologyComplementClass.ComplementOf));

                    result = ontology.Data.DifferenceWith(membersCache[ontologyComplementClass.ComplementOf.PatternMemberID]);
                }
                else
                    result = ontology.Data.DifferenceWith(ontology.GetMembersOf(ontologyComplementClass.ComplementOf));
            }
            #endregion

            return result;
        }

        /// <summary>
        /// Enlists the individuals which are members of the given enumeration within the given ontology
        /// </summary>
        internal static RDFOntologyData GetMembersOfEnumerate(this RDFOntology ontology, RDFOntologyEnumerateClass ontologyEnumerateClass)
        {
            RDFOntologyData result = new RDFOntologyData();

            //Filter "oneOf" relations made with the given enumerate class
            RDFOntologyTaxonomy enTaxonomy = ontology.Model.ClassModel.Relations.OneOf.SelectEntriesBySubject(ontologyEnumerateClass);
            foreach (RDFOntologyTaxonomyEntry tEntry in enTaxonomy)
            {
                //Add the individual and its synonyms
                if (tEntry.TaxonomySubject.IsEnumerateClass() && tEntry.TaxonomyObject.IsIndividual())
                {
                    result = result.UnionWith(ontology.Data.GetSameIndividuals((RDFOntologyIndividual)tEntry.TaxonomyObject))
                                   .AddIndividual((RDFOntologyIndividual)tEntry.TaxonomyObject);
                }
            }

            return result;
        }

        /// <summary>
        /// Enlists the individuals which are members of the given restriction within the given ontology
        /// </summary>
        internal static RDFOntologyData GetMembersOfRestriction(this RDFOntology ontology, RDFOntologyRestriction ontologyRestrictionClass)
        {
            RDFOntologyData result = new RDFOntologyData();

            //Enlist the properties which are compatible with the restriction's "OnProperty"
            RDFOntologyPropertyModel restrictionProperties = ontology.Model.PropertyModel.GetSubPropertiesOf(ontologyRestrictionClass.OnProperty)
                                                                                         .UnionWith(ontology.Model.PropertyModel.GetEquivalentPropertiesOf(ontologyRestrictionClass.OnProperty))
                                                                                         .AddProperty(ontologyRestrictionClass.OnProperty);

            //Filter assertions made with enlisted compatible properties
            RDFOntologyTaxonomy restrictionAssertions = new RDFOntologyTaxonomy(ontology.Data.Relations.Assertions.Category, ontology.Data.Relations.Assertions.AcceptDuplicates);
            foreach (RDFOntologyProperty restrictionProperty in restrictionProperties)
                restrictionAssertions = restrictionAssertions.UnionWith(ontology.Data.Relations.Assertions.SelectEntriesByPredicate(restrictionProperty));

            #region Cardinality
            if (ontologyRestrictionClass is RDFOntologyCardinalityRestriction cardinalityRestriction)
            {
                //Item2 is a counter for occurrences of the restricted property within the subject individual
                var cardinalityRestrictionRegistry = new Dictionary<long, Tuple<RDFOntologyIndividual, long>>();

                //Iterate the compatible assertions
                foreach (RDFOntologyTaxonomyEntry assertion in restrictionAssertions)
                {
                    if (!cardinalityRestrictionRegistry.ContainsKey(assertion.TaxonomySubject.PatternMemberID))
                        cardinalityRestrictionRegistry.Add(assertion.TaxonomySubject.PatternMemberID, new Tuple<RDFOntologyIndividual, long>((RDFOntologyIndividual)assertion.TaxonomySubject, 1));
                    else
                    {
                        long occurrencyCounter = cardinalityRestrictionRegistry[assertion.TaxonomySubject.PatternMemberID].Item2;
                        cardinalityRestrictionRegistry[assertion.TaxonomySubject.PatternMemberID] = new Tuple<RDFOntologyIndividual, long>((RDFOntologyIndividual)assertion.TaxonomySubject, occurrencyCounter + 1);
                    }
                }

                //Apply the cardinality restriction on the tracked individuals
                var cardinalityRestrictionRegistryEnumerator = cardinalityRestrictionRegistry.Values.GetEnumerator();
                while (cardinalityRestrictionRegistryEnumerator.MoveNext())
                {
                    bool passesMinCardinality = true;
                    bool passesMaxCardinality = true;

                    //MinCardinality: signal tracked individuals having "#occurrences < MinCardinality"
                    if (cardinalityRestriction.MinCardinality > 0)
                    {
                        if (cardinalityRestrictionRegistryEnumerator.Current.Item2 < cardinalityRestriction.MinCardinality)
                            passesMinCardinality = false;
                    }

                    //MaxCardinality: signal tracked individuals having "#occurrences > MaxCardinality"
                    if (cardinalityRestriction.MaxCardinality > 0)
                    {
                        if (cardinalityRestrictionRegistryEnumerator.Current.Item2 > cardinalityRestriction.MaxCardinality)
                            passesMaxCardinality = false;
                    }

                    //Save the candidate individual if it passes cardinality restriction
                    if (passesMinCardinality && passesMaxCardinality)
                        result.AddIndividual(cardinalityRestrictionRegistryEnumerator.Current.Item1);
                }

            }
            #endregion

            #region QualifiedCardinality [OWL2]
            else if (ontologyRestrictionClass is RDFOntologyQualifiedCardinalityRestriction qualifiedCardinalityRestriction)
            {

                //Item2 is a counter for occurrences of the restricted property within the subject individual
                var qualifiedCardinalityRestrictionRegistry = new Dictionary<long, Tuple<RDFOntologyIndividual, long>>();

                //Enlist the classes which are compatible with the restricted "OnClass"
                RDFOntologyClassModel onClasses = ontology.Model.ClassModel.GetSubClassesOf(qualifiedCardinalityRestriction.OnClass)
                                                                           .UnionWith(ontology.Model.ClassModel.GetEquivalentClassesOf(qualifiedCardinalityRestriction.OnClass))
                                                                           .AddClass(qualifiedCardinalityRestriction.OnClass);

                //Iterate the compatible assertions
                var classTypesCache = new Dictionary<long, RDFOntologyClassModel>();
                foreach (RDFOntologyTaxonomyEntry assertion in restrictionAssertions)
                {
                    //Iterate the class types of the object individual, checking presence of the restricted "OnClass"
                    bool onClassFound = false;
                    RDFOntologyTaxonomy objectClassTypes = ontology.Data.Relations.ClassType.SelectEntriesBySubject(assertion.TaxonomyObject);
                    foreach (RDFOntologyTaxonomyEntry objectClassType in objectClassTypes)
                    {
                        if (!classTypesCache.ContainsKey(objectClassType.TaxonomyObject.PatternMemberID))
                            classTypesCache.Add(objectClassType.TaxonomyObject.PatternMemberID,
                                                ontology.Model.ClassModel.GetSuperClassesOf((RDFOntologyClass)objectClassType.TaxonomyObject)
                                                                         .UnionWith(ontology.Model.ClassModel.GetEquivalentClassesOf((RDFOntologyClass)objectClassType.TaxonomyObject))
                                                                         .AddClass((RDFOntologyClass)objectClassType.TaxonomyObject));
                        
                        if (classTypesCache[objectClassType.TaxonomyObject.PatternMemberID].IntersectWith(onClasses).ClassesCount > 0)
                        {
                            onClassFound = true;
                            break;
                        }
                    }

                    //If classtype is compatible, proceed with qualified counters
                    if (onClassFound)
                    {
                        if (!qualifiedCardinalityRestrictionRegistry.ContainsKey(assertion.TaxonomySubject.PatternMemberID))
                            qualifiedCardinalityRestrictionRegistry.Add(assertion.TaxonomySubject.PatternMemberID, new Tuple<RDFOntologyIndividual, long>((RDFOntologyIndividual)assertion.TaxonomySubject, 1));
                        else
                        {
                            long occurrencyCounter = qualifiedCardinalityRestrictionRegistry[assertion.TaxonomySubject.PatternMemberID].Item2;
                            qualifiedCardinalityRestrictionRegistry[assertion.TaxonomySubject.PatternMemberID] = new Tuple<RDFOntologyIndividual, long>((RDFOntologyIndividual)assertion.TaxonomySubject, occurrencyCounter + 1);
                        }
                    }
                }

                //Apply the qualified cardinality restriction on the tracked individuals
                var qualifiedCardinalityRestrictionRegistryEnumerator = qualifiedCardinalityRestrictionRegistry.Values.GetEnumerator();
                while (qualifiedCardinalityRestrictionRegistryEnumerator.MoveNext())
                {
                    bool passesMinQualifiedCardinality = true;
                    bool passesMaxQualifiedCardinality = true;

                    //MinQualifiedCardinality: signal tracked individuals having "#occurrences < MinQualifiedCardinality"
                    if (qualifiedCardinalityRestriction.MinQualifiedCardinality > 0)
                    {
                        if (qualifiedCardinalityRestrictionRegistryEnumerator.Current.Item2 < qualifiedCardinalityRestriction.MinQualifiedCardinality)
                            passesMinQualifiedCardinality = false;
                    }

                    //MaxQualifiedCardinality: signal tracked individuals having "#occurrences > MaxQualifiedCardinality"
                    if (qualifiedCardinalityRestriction.MaxQualifiedCardinality > 0)
                    {
                        if (qualifiedCardinalityRestrictionRegistryEnumerator.Current.Item2 > qualifiedCardinalityRestriction.MaxQualifiedCardinality)
                            passesMaxQualifiedCardinality = false;
                    }

                    //Save the candidate individual if it passes qualified cardinality restriction
                    if (passesMinQualifiedCardinality && passesMaxQualifiedCardinality)
                        result.AddIndividual(qualifiedCardinalityRestrictionRegistryEnumerator.Current.Item1);
                }

            }
            #endregion

            #region AllValuesFrom/SomeValuesFrom
            else if (ontologyRestrictionClass is RDFOntologyAllValuesFromRestriction || ontologyRestrictionClass is RDFOntologySomeValuesFromRestriction)
            {
                //Item2 is a counter for occurrences of the restricted property with a range member of the restricted "FromClass"
                //Item3 is a counter for occurrences of the restricted property with a range member not of the restricted "FromClass"
                var valuesFromRegistry = new Dictionary<long, Tuple<RDFOntologyIndividual, long, long>>();

                //Enlist the classes which are compatible with the restricted "FromClass"
                var classes = ontologyRestrictionClass is RDFOntologyAllValuesFromRestriction
                                ? ontology.Model.ClassModel.GetSubClassesOf(((RDFOntologyAllValuesFromRestriction)ontologyRestrictionClass).FromClass)
                                                            .UnionWith(ontology.Model.ClassModel.GetEquivalentClassesOf(((RDFOntologyAllValuesFromRestriction)ontologyRestrictionClass).FromClass))
                                                            .AddClass(((RDFOntologyAllValuesFromRestriction)ontologyRestrictionClass).FromClass)
                                : ontology.Model.ClassModel.GetSubClassesOf(((RDFOntologySomeValuesFromRestriction)ontologyRestrictionClass).FromClass)
                                                            .UnionWith(ontology.Model.ClassModel.GetEquivalentClassesOf(((RDFOntologySomeValuesFromRestriction)ontologyRestrictionClass).FromClass))
                                                            .AddClass(((RDFOntologySomeValuesFromRestriction)ontologyRestrictionClass).FromClass);

                //Iterate the compatible assertions
                var classTypesCache = new Dictionary<long, RDFOntologyClassModel>();
                foreach (var assertion in restrictionAssertions)
                {
                    //Initialize the occurrence counters of the subject individual
                    if (!valuesFromRegistry.ContainsKey(assertion.TaxonomySubject.PatternMemberID))
                        valuesFromRegistry.Add(assertion.TaxonomySubject.PatternMemberID, new Tuple<RDFOntologyIndividual, long, long>((RDFOntologyIndividual)assertion.TaxonomySubject, 0, 0));

                    //Iterate the class types of the object individual, checking presence of the restricted "FromClass"
                    bool fromClassFound = false;
                    RDFOntologyTaxonomy objectClassTypes = ontology.Data.Relations.ClassType.SelectEntriesBySubject(assertion.TaxonomyObject);
                    foreach (RDFOntologyTaxonomyEntry objectClassType in objectClassTypes)
                    {
                        if (!classTypesCache.ContainsKey(objectClassType.TaxonomyObject.PatternMemberID))
                            classTypesCache.Add(objectClassType.TaxonomyObject.PatternMemberID,
                                                ontology.Model.ClassModel.GetSuperClassesOf((RDFOntologyClass)objectClassType.TaxonomyObject)
                                                                         .UnionWith(ontology.Model.ClassModel.GetEquivalentClassesOf((RDFOntologyClass)objectClassType.TaxonomyObject))
                                                                         .AddClass((RDFOntologyClass)objectClassType.TaxonomyObject));
                        
                        if (classTypesCache[objectClassType.TaxonomyObject.PatternMemberID].IntersectWith(classes).ClassesCount > 0)
                        {
                            fromClassFound = true;
                            break;
                        }
                    }

                    //Update the occurrence counters of the subject individual
                    long equalityCounter = valuesFromRegistry[assertion.TaxonomySubject.PatternMemberID].Item2;
                    long differenceCounter = valuesFromRegistry[assertion.TaxonomySubject.PatternMemberID].Item3;
                    if (fromClassFound)
                        valuesFromRegistry[assertion.TaxonomySubject.PatternMemberID] = new Tuple<RDFOntologyIndividual, long, long>((RDFOntologyIndividual)assertion.TaxonomySubject, equalityCounter + 1, differenceCounter);
                    else
                        valuesFromRegistry[assertion.TaxonomySubject.PatternMemberID] = new Tuple<RDFOntologyIndividual, long, long>((RDFOntologyIndividual)assertion.TaxonomySubject, equalityCounter, differenceCounter + 1);
                }

                //Apply the restriction on the subject individuals
                var valuesFromRegistryEnumerator = valuesFromRegistry.Values.GetEnumerator();
                while (valuesFromRegistryEnumerator.MoveNext())
                {
                    //AllValuesFrom
                    if (ontologyRestrictionClass is RDFOntologyAllValuesFromRestriction)
                    {
                        if (valuesFromRegistryEnumerator.Current.Item2 >= 1 && valuesFromRegistryEnumerator.Current.Item3 == 0)
                            result.AddIndividual(valuesFromRegistryEnumerator.Current.Item1);
                    }
                    //SomeValuesFrom
                    else
                    {
                        if (valuesFromRegistryEnumerator.Current.Item2 >= 1)
                            result.AddIndividual(valuesFromRegistryEnumerator.Current.Item1);
                    }
                }
            }
            #endregion

            #region HasSelf [OWL2]
            else if (ontologyRestrictionClass is RDFOntologyHasSelfRestriction hasSelfRestriction)
            {
                //Iterate the compatible assertions
                var sameIndividualsCache = new Dictionary<long, RDFOntologyData>();
                foreach (RDFOntologyTaxonomyEntry assertion in restrictionAssertions.Where(x => x.TaxonomyObject.IsIndividual()))
                {
                    //Enlist the same individuals of the assertion subject
                    if (!sameIndividualsCache.ContainsKey(assertion.TaxonomySubject.PatternMemberID))
                        sameIndividualsCache.Add(assertion.TaxonomySubject.PatternMemberID, ontology.Data.GetSameIndividuals((RDFOntologyIndividual)assertion.TaxonomySubject)
                                                                                                         .AddIndividual((RDFOntologyIndividual)assertion.TaxonomySubject));
                    
                    if (sameIndividualsCache[assertion.TaxonomySubject.PatternMemberID].SelectIndividual(assertion.TaxonomySubject.ToString()) != null
                            && sameIndividualsCache[assertion.TaxonomySubject.PatternMemberID].SelectIndividual(assertion.TaxonomyObject.ToString()) != null)
                        result.AddIndividual((RDFOntologyIndividual)assertion.TaxonomySubject);
                }
            }
            #endregion

            #region HasValue
            else if (ontologyRestrictionClass is RDFOntologyHasValueRestriction hasValueRestriction)
            {
                if (hasValueRestriction.RequiredValue.IsIndividual())
                {
                    //Enlist the same individuals of the restriction's "RequiredValue"
                    RDFOntologyData individuals = ontology.Data.GetSameIndividuals((RDFOntologyIndividual)hasValueRestriction.RequiredValue)
                                                               .AddIndividual((RDFOntologyIndividual)hasValueRestriction.RequiredValue);

                    //Iterate the compatible assertions and track the subject individuals having the required value
                    foreach (RDFOntologyTaxonomyEntry assertion in restrictionAssertions.Where(x => x.TaxonomyObject.IsIndividual()))
                    {
                        if (individuals.SelectIndividual(assertion.TaxonomyObject.ToString()) != null)
                            result.AddIndividual((RDFOntologyIndividual)assertion.TaxonomySubject);
                    }
                }
                else if (hasValueRestriction.RequiredValue.IsLiteral())
                {
                    //Iterate the compatible assertions and track the subject individuals having the required value
                    foreach (RDFOntologyTaxonomyEntry assertion in restrictionAssertions.Where(x => x.TaxonomyObject.IsLiteral()))
                    {
                        if (RDFQueryUtilities.CompareRDFPatternMembers(hasValueRestriction.RequiredValue.Value, assertion.TaxonomyObject.Value) == 0)
                            result.AddIndividual((RDFOntologyIndividual)assertion.TaxonomySubject);
                    }
                }
            }
            #endregion

            return result;
        }

        /// <summary>
        /// Enlists the literals which are members of the given literal-compatible class within the given ontology
        /// </summary>
        internal static RDFOntologyData GetMembersOfLiteralCompatibleClass(this RDFOntology ontology, RDFOntologyClass ontologyClass)
        {
            RDFOntologyData result = new RDFOntologyData();

            #region DataRange
            if (ontologyClass.IsDataRangeClass())
            {
                //Filter "oneOf" relations made with the given datarange class
                RDFOntologyTaxonomy drTaxonomy = ontology.Model.ClassModel.Relations.OneOf.SelectEntriesBySubject(ontologyClass);
                foreach (RDFOntologyTaxonomyEntry tEntry in drTaxonomy)
                {
                    //Add the literal
                    if (tEntry.TaxonomySubject.IsDataRangeClass() && tEntry.TaxonomyObject.IsLiteral())
                        result.AddLiteral((RDFOntologyLiteral)tEntry.TaxonomyObject);
                }
            }
            #endregion

            #region Literal
            //Asking for "rdfs:Literal" is the only way to get enlistment of plain literals, since they have really no semantic
            else if (ontologyClass.Equals(RDFVocabulary.RDFS.LITERAL.ToRDFOntologyClass()))
            {
                foreach (RDFOntologyLiteral ontLit in ontology.Data.Literals.Values)
                    result.AddLiteral(ontLit);
            }
            #endregion

            #region SubLiteral
            else
            {
                foreach (RDFOntologyLiteral ontLit in ontology.Data.Literals.Values.Where(l => l.Value is RDFTypedLiteral))
                {
                    RDFOntologyClass dTypeClass = ontology.Model.ClassModel.SelectClass(RDFModelUtilities.GetDatatypeFromEnum(((RDFTypedLiteral)ontLit.Value).Datatype));
                    if (dTypeClass != null)
                    {
                        if (dTypeClass.Equals(ontologyClass)
                                || ontology.Model.ClassModel.CheckIsSubClassOf(dTypeClass, ontologyClass)
                                    || ontology.Model.ClassModel.CheckIsEquivalentClassOf(dTypeClass, ontologyClass))
                            result.AddLiteral(ontLit);
                    }
                }
            }
            #endregion

            return result;
        }

        /// <summary>
        /// Enlists the individuals which are members of the given non literal-compatible class within the given ontology
        /// </summary>
        internal static RDFOntologyData GetMembersOfNonLiteralCompatibleClass(this RDFOntology ontology, RDFOntologyClass ontClass)
        {
            RDFOntologyData result = new RDFOntologyData();

            if (ontClass != null && ontology != null)
            {
                //Restriction
                if (ontClass.IsRestrictionClass())
                    result = ontology.GetMembersOfRestriction((RDFOntologyRestriction)ontClass);

                //Composite
                else if (ontClass.IsCompositeClass())
                    result = ontology.GetMembersOfComposite(ontClass);

                //Enumerate
                else if (ontClass.IsEnumerateClass())
                    result = ontology.GetMembersOfEnumerate((RDFOntologyEnumerateClass)ontClass);

                //Class
                else
                    result = ontology.GetMembersOfClass(ontClass);
            }

            return result;
        }
        #endregion
    }
}