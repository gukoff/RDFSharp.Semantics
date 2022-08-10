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
        /// Checks if the given aFact is sameAs the given bFact within the given data
        /// </summary>
        public static bool CheckIsSameFactAs(this RDFOntologyData data, RDFOntologyFact aFact, RDFOntologyFact bFact)
            => aFact != null && bFact != null && data != null && data.GetSameFactsAs(aFact).Facts.ContainsKey(bFact.PatternMemberID);

        /// <summary>
        /// Enlists the sameFacts of the given fact within the given data
        /// </summary>
        public static RDFOntologyData GetSameFactsAs(this RDFOntologyData data, RDFOntologyFact ontologyFact)
        {
            RDFOntologyData result = new RDFOntologyData();

            if (ontologyFact != null && data != null)
                result = data.GetSameFactsAsInternal(ontologyFact, null)
                             .RemoveFact(ontologyFact); //Safety deletion

            return result;
        }

        /// <summary>
        /// Subsumes the "owl:sameAs" taxonomy to discover direct and indirect samefacts of the given facts
        /// </summary>
        internal static RDFOntologyData GetSameFactsAsInternal(this RDFOntologyData data, RDFOntologyFact ontologyFact, Dictionary<long, RDFOntologyFact> visitContext)
        {
            RDFOntologyData result = new RDFOntologyData();

            #region visitContext
            if (visitContext == null)
                visitContext = new Dictionary<long, RDFOntologyFact>() { { ontologyFact.PatternMemberID, ontologyFact } };
            else
            {
                if (!visitContext.ContainsKey(ontologyFact.PatternMemberID))
                    visitContext.Add(ontologyFact.PatternMemberID, ontologyFact);
                else
                    return result;
            }
            #endregion

            // Transitivity of "owl:sameAs" taxonomy: ((A SAMEAS B)  &&  (B SAMEAS C))  =>  (A SAMEAS C)
            foreach (RDFOntologyTaxonomyEntry sf in data.Relations.SameAs.SelectEntriesBySubject(ontologyFact))
            {
                result.AddFact((RDFOntologyFact)sf.TaxonomyObject);
                result = result.UnionWith(data.GetSameFactsAsInternal((RDFOntologyFact)sf.TaxonomyObject, visitContext));
            }

            return result;
        }
        #endregion

        #region DifferentFrom
        /// <summary>
        /// Checks if the given aFact is differentFrom the given bFact within the given data
        /// </summary>
        public static bool CheckIsDifferentFactFrom(this RDFOntologyData data, RDFOntologyFact aFact, RDFOntologyFact bFact)
            => aFact != null && bFact != null && data != null && data.GetDifferentFactsFrom(aFact).Facts.ContainsKey(bFact.PatternMemberID);

        /// <summary>
        /// Enlists the different facts of the given fact within the given data
        /// </summary>
        public static RDFOntologyData GetDifferentFactsFrom(this RDFOntologyData data, RDFOntologyFact ontFact)
        {
            RDFOntologyData result = new RDFOntologyData();

            if (ontFact != null && data != null)
                result = data.GetDifferentFactsFromInternal(ontFact, null)
                             .RemoveFact(ontFact); //Safety deletion

            return result;
        }

        /// <summary>
        /// Subsumes the "owl:differentFrom" taxonomy to discover direct and indirect differentFacts of the given facts
        /// </summary>
        internal static RDFOntologyData GetDifferentFactsFromInternal(this RDFOntologyData data, RDFOntologyFact ontFact, Dictionary<long, RDFOntologyFact> visitContext)
        {
            RDFOntologyData result = new RDFOntologyData();

            #region visitContext
            if (visitContext == null)
                visitContext = new Dictionary<long, RDFOntologyFact>() { { ontFact.PatternMemberID, ontFact } };
            else
            {
                if (!visitContext.ContainsKey(ontFact.PatternMemberID))
                    visitContext.Add(ontFact.PatternMemberID, ontFact);
                else
                    return result;
            }
            #endregion

            // Inference: (A DIFFERENTFROM B  &&  B SAMEAS C         =>  A DIFFERENTFROM C)
            foreach (RDFOntologyTaxonomyEntry df in data.Relations.DifferentFrom.SelectEntriesBySubject(ontFact))
            {
                result.AddFact((RDFOntologyFact)df.TaxonomyObject);
                result = result.UnionWith(data.GetSameFactsAsInternal((RDFOntologyFact)df.TaxonomyObject, visitContext));
            }

            // Inference: (A SAMEAS B         &&  B DIFFERENTFROM C  =>  A DIFFERENTFROM C)
            foreach (RDFOntologyFact sa in data.GetSameFactsAs(ontFact))
                result = result.UnionWith(data.GetDifferentFactsFromInternal(sa, visitContext));

            return result;
        }
        #endregion

        #region TransitiveProperty
        /// <summary>
        /// Checks if the given "aFact -> transProp" assertion links to the given bFact within the given data
        /// </summary>
        public static bool CheckIsTransitiveAssertionOf(this RDFOntologyData data, RDFOntologyFact aFact, RDFOntologyObjectProperty transProp, RDFOntologyFact bFact)
            => aFact != null && transProp != null && transProp.IsTransitiveProperty() && bFact != null && data != null && data.GetTransitiveAssertionsOf(aFact, transProp).Facts.ContainsKey(bFact.PatternMemberID);

        /// <summary>
        /// Enlists the given "aFact -> transOntProp" assertions within the given data
        /// </summary>
        public static RDFOntologyData GetTransitiveAssertionsOf(this RDFOntologyData data, RDFOntologyFact ontFact, RDFOntologyObjectProperty transOntProp)
        {
            RDFOntologyData result = new RDFOntologyData();

            if (ontFact != null && transOntProp != null && transOntProp.IsTransitiveProperty() && data != null)
                result = data.GetTransitiveAssertionsOfInternal(ontFact, transOntProp, null);

            return result;
        }

        /// <summary>
        /// Enlists the transitive assertions of the given fact and the given property within the given data
        /// </summary>
        internal static RDFOntologyData GetTransitiveAssertionsOfInternal(this RDFOntologyData data, RDFOntologyFact ontFact, RDFOntologyObjectProperty ontProp, Dictionary<long, RDFOntologyFact> visitContext)
        {
            RDFOntologyData result = new RDFOntologyData();

            #region visitContext
            if (visitContext == null)
                visitContext = new Dictionary<long, RDFOntologyFact>() { { ontFact.PatternMemberID, ontFact } };
            else
            {
                if (!visitContext.ContainsKey(ontFact.PatternMemberID))
                    visitContext.Add(ontFact.PatternMemberID, ontFact);
                else
                    return result;
            }
            #endregion

            // ((F1 P F2)    &&  (F2 P F3))  =>  (F1 P F3)
            foreach (RDFOntologyTaxonomyEntry ta in data.Relations.Assertions.SelectEntriesBySubject(ontFact)
                                                                             .SelectEntriesByPredicate(ontProp))
            {
                result.AddFact((RDFOntologyFact)ta.TaxonomyObject);
                result = result.UnionWith(data.GetTransitiveAssertionsOfInternal((RDFOntologyFact)ta.TaxonomyObject, ontProp, visitContext));
            }

            return result;
        }
        #endregion

        #region Assertions
        /// <summary>
        /// Checks if the given "aFact -> objectProperty -> bFact" is an assertion within the given data
        /// </summary>
        public static bool CheckIsObjectAssertion(this RDFOntologyData data, RDFOntologyFact aFact, RDFOntologyObjectProperty objectProperty, RDFOntologyFact bFact)
        {
            if (aFact != null && bFact != null && objectProperty != null && data != null)
            {
                //Reason over subject/object facts to detect indirect potential taxonomy violations
                RDFOntologyData compatibleSubjects = data.GetSameFactsAs(aFact).AddFact(aFact);
                RDFOntologyData compatibleObjects = data.GetSameFactsAs(aFact).AddFact(bFact);

                return data.Relations.Assertions.Any(te => compatibleSubjects.Any(x => x.Equals(te.TaxonomySubject))
                                                               && te.TaxonomyPredicate.Equals(objectProperty)
                                                                   && compatibleObjects.Any(x => x.Equals(te.TaxonomyObject)));
            }
            return false;
        }

        /// <summary>
        /// Checks if the given "aFact -> datatypeProperty -> ontologyLiteral" is an assertion within the given data
        /// </summary>
        public static bool CheckIsDataAssertion(this RDFOntologyData data, RDFOntologyFact aFact, RDFOntologyDatatypeProperty datatypeProperty, RDFOntologyLiteral ontologyLiteral)
        {
            if (aFact != null && ontologyLiteral != null && datatypeProperty != null && data != null)
            {
                //Reason over subject facts to detect indirect potential taxonomy violations
                RDFOntologyData compatibleSubjects = data.GetSameFactsAs(aFact).AddFact(aFact);

                return data.Relations.Assertions.Any(te => compatibleSubjects.Any(x => x.Equals(te.TaxonomySubject))
                                                               && te.TaxonomyPredicate.Equals(datatypeProperty)
                                                                   && te.TaxonomyObject.Equals(ontologyLiteral));
            }
            return false;
        }

        /// <summary>
        /// Checks if the given "aFact -> objectProperty -> bFact" is a negative assertion within the given data
        /// </summary>
        public static bool CheckIsNegativeObjectAssertion(this RDFOntologyData data, RDFOntologyFact aFact, RDFOntologyObjectProperty objectProperty, RDFOntologyFact bFact)
        {
            if (aFact != null && bFact != null && objectProperty != null && data != null)
            {
                //Reason over subject/object facts to detect indirect potential taxonomy violations
                RDFOntologyData compatibleSubjects = data.GetSameFactsAs(aFact).AddFact(aFact);
                RDFOntologyData compatibleObjects = data.GetSameFactsAs(aFact).AddFact(bFact);

                return data.Relations.NegativeAssertions.Any(te => compatibleSubjects.Any(x => x.Equals(te.TaxonomySubject))
                                                                       && te.TaxonomyPredicate.Equals(objectProperty)
                                                                           && compatibleObjects.Any(x => x.Equals(te.TaxonomyObject)));
            }
            return false;
        }

        /// <summary>
        /// Checks if the given "aFact -> datatypeProperty -> ontologyLiteral" is a negative assertion within the given data
        /// </summary>
        public static bool CheckIsNegativeDataAssertion(this RDFOntologyData data, RDFOntologyFact aFact, RDFOntologyDatatypeProperty datatypeProperty, RDFOntologyLiteral ontologyLiteral)
        {
            if (aFact != null && ontologyLiteral != null && datatypeProperty != null && data != null)
            {
                //Reason over subject facts to detect indirect potential taxonomy violations
                RDFOntologyData compatibleSubjects = data.GetSameFactsAs(aFact).AddFact(aFact);

                return data.Relations.NegativeAssertions.Any(te => compatibleSubjects.Any(x => x.Equals(te.TaxonomySubject))
                                                                       && te.TaxonomyPredicate.Equals(datatypeProperty)
                                                                           && te.TaxonomyObject.Equals(ontologyLiteral));
            }
            return false;
        }
        #endregion

        #region MemberOf
        /// <summary>
        /// Checks if the given fact is member of the given class within the given ontology
        /// </summary>
        public static bool CheckIsMemberOf(this RDFOntology ontology, RDFOntologyFact ontologyFact, RDFOntologyClass ontologyClass)
            => ontologyFact != null && ontologyClass != null && ontology != null && ontology.GetMembersOf(ontologyClass).Facts.ContainsKey(ontologyFact.PatternMemberID);

        /// <summary>
        /// Enlists the facts which are members of the given class within the given ontology
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
        /// Enlists the facts which are members of the given class within the given ontology
        /// </summary>
        internal static RDFOntologyData GetMembersOfClass(this RDFOntology ontology, RDFOntologyClass ontologyClass)
        {
            RDFOntologyData result = new RDFOntologyData();

            //Get the compatible classes
            RDFOntologyClassModel compClasses = ontology.Model.ClassModel.GetSubClassesOf(ontologyClass)
                                                                         .UnionWith(ontology.Model.ClassModel.GetEquivalentClassesOf(ontologyClass))
                                                                         .AddClass(ontologyClass);

            //Get the facts belonging to compatible classes
            List<RDFOntologyResource> compFacts = ontology.Data.Relations.ClassType.Where(te => compClasses.Any(c => c.Equals(te.TaxonomyObject)))
                                                                                   .Select(te => te.TaxonomySubject)
                                                                                   .ToList();

            //Add the fact and its synonyms
            Dictionary<long, RDFOntologyData> sameFactsCache = new Dictionary<long, RDFOntologyData>();
            foreach (RDFOntologyResource compFact in compFacts)
            {
                if (!sameFactsCache.ContainsKey(compFact.PatternMemberID))
                {
                    sameFactsCache.Add(compFact.PatternMemberID, ontology.Data.GetSameFactsAs((RDFOntologyFact)compFact));

                    result = result.UnionWith(sameFactsCache[compFact.PatternMemberID])
                                   .AddFact((RDFOntologyFact)compFact);
                }
            }

            return result;
        }

        /// <summary>
        /// Enlists the facts which are members of the given composite within the given ontology.
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
        /// Enlists the facts which are members of the given enumeration within the given ontology
        /// </summary>
        internal static RDFOntologyData GetMembersOfEnumerate(this RDFOntology ontology, RDFOntologyEnumerateClass ontologyEnumerateClass)
        {
            RDFOntologyData result = new RDFOntologyData();

            //Filter "oneOf" relations made with the given enumerate class
            RDFOntologyTaxonomy enTaxonomy = ontology.Model.ClassModel.Relations.OneOf.SelectEntriesBySubject(ontologyEnumerateClass);
            foreach (RDFOntologyTaxonomyEntry tEntry in enTaxonomy)
            {
                //Add the fact and its synonyms
                if (tEntry.TaxonomySubject.IsEnumerateClass() && tEntry.TaxonomyObject.IsFact())
                {
                    result = result.UnionWith(ontology.Data.GetSameFactsAs((RDFOntologyFact)tEntry.TaxonomyObject))
                                   .AddFact((RDFOntologyFact)tEntry.TaxonomyObject);
                }
            }

            return result;
        }

        /// <summary>
        /// Enlists the facts which are members of the given restriction within the given ontology
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
            foreach (RDFOntologyProperty property in restrictionProperties)
                restrictionAssertions = restrictionAssertions.UnionWith(ontology.Data.Relations.Assertions.SelectEntriesByPredicate(property));

            #region Cardinality
            if (ontologyRestrictionClass is RDFOntologyCardinalityRestriction cardinalityRestriction)
            {
                //Item2 is a counter for occurrences of the restricted property within the subject fact
                var cardinalityRestrictionRegistry = new Dictionary<long, Tuple<RDFOntologyFact, long>>();

                //Iterate the compatible assertions
                foreach (RDFOntologyTaxonomyEntry assertion in restrictionAssertions)
                {
                    if (!cardinalityRestrictionRegistry.ContainsKey(assertion.TaxonomySubject.PatternMemberID))
                        cardinalityRestrictionRegistry.Add(assertion.TaxonomySubject.PatternMemberID, new Tuple<RDFOntologyFact, long>((RDFOntologyFact)assertion.TaxonomySubject, 1));
                    else
                    {
                        long occurrencyCounter = cardinalityRestrictionRegistry[assertion.TaxonomySubject.PatternMemberID].Item2;
                        cardinalityRestrictionRegistry[assertion.TaxonomySubject.PatternMemberID] = new Tuple<RDFOntologyFact, long>((RDFOntologyFact)assertion.TaxonomySubject, occurrencyCounter + 1);
                    }
                }

                //Apply the cardinality restriction on the tracked facts
                var cardinalityRestrictionRegistryEnumerator = cardinalityRestrictionRegistry.Values.GetEnumerator();
                while (cardinalityRestrictionRegistryEnumerator.MoveNext())
                {
                    bool passesMinCardinality = true;
                    bool passesMaxCardinality = true;

                    //MinCardinality: signal tracked facts having "#occurrences < MinCardinality"
                    if (cardinalityRestriction.MinCardinality > 0)
                    {
                        if (cardinalityRestrictionRegistryEnumerator.Current.Item2 < cardinalityRestriction.MinCardinality)
                            passesMinCardinality = false;
                    }

                    //MaxCardinality: signal tracked facts having "#occurrences > MaxCardinality"
                    if (cardinalityRestriction.MaxCardinality > 0)
                    {
                        if (cardinalityRestrictionRegistryEnumerator.Current.Item2 > cardinalityRestriction.MaxCardinality)
                            passesMaxCardinality = false;
                    }

                    //Save the candidate fact if it passes cardinality restriction
                    if (passesMinCardinality && passesMaxCardinality)
                        result.AddFact(cardinalityRestrictionRegistryEnumerator.Current.Item1);
                }

            }
            #endregion

            #region QualifiedCardinality [OWL2]
            else if (ontologyRestrictionClass is RDFOntologyQualifiedCardinalityRestriction qualifiedCardinalityRestriction)
            {

                //Item2 is a counter for occurrences of the restricted property within the subject fact
                var qualifiedCardinalityRestrictionRegistry = new Dictionary<long, Tuple<RDFOntologyFact, long>>();

                //Enlist the classes which are compatible with the restricted "OnClass"
                RDFOntologyClassModel onClasses = ontology.Model.ClassModel.GetSubClassesOf(qualifiedCardinalityRestriction.OnClass)
                                                                           .UnionWith(ontology.Model.ClassModel.GetEquivalentClassesOf(qualifiedCardinalityRestriction.OnClass))
                                                                           .AddClass(qualifiedCardinalityRestriction.OnClass);

                //Iterate the compatible assertions
                var classTypesCache = new Dictionary<long, RDFOntologyClassModel>();
                foreach (RDFOntologyTaxonomyEntry assertion in restrictionAssertions)
                {
                    //Iterate the class types of the object fact, checking presence of the restricted "OnClass"
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
                            qualifiedCardinalityRestrictionRegistry.Add(assertion.TaxonomySubject.PatternMemberID, new Tuple<RDFOntologyFact, long>((RDFOntologyFact)assertion.TaxonomySubject, 1));
                        else
                        {
                            long occurrencyCounter = qualifiedCardinalityRestrictionRegistry[assertion.TaxonomySubject.PatternMemberID].Item2;
                            qualifiedCardinalityRestrictionRegistry[assertion.TaxonomySubject.PatternMemberID] = new Tuple<RDFOntologyFact, long>((RDFOntologyFact)assertion.TaxonomySubject, occurrencyCounter + 1);
                        }
                    }
                }

                //Apply the qualified cardinality restriction on the tracked facts
                var qualifiedCardinalityRestrictionRegistryEnumerator = qualifiedCardinalityRestrictionRegistry.Values.GetEnumerator();
                while (qualifiedCardinalityRestrictionRegistryEnumerator.MoveNext())
                {
                    bool passesMinQualifiedCardinality = true;
                    bool passesMaxQualifiedCardinality = true;

                    //MinQualifiedCardinality: signal tracked facts having "#occurrences < MinQualifiedCardinality"
                    if (qualifiedCardinalityRestriction.MinQualifiedCardinality > 0)
                    {
                        if (qualifiedCardinalityRestrictionRegistryEnumerator.Current.Item2 < qualifiedCardinalityRestriction.MinQualifiedCardinality)
                            passesMinQualifiedCardinality = false;
                    }

                    //MaxQualifiedCardinality: signal tracked facts having "#occurrences > MaxQualifiedCardinality"
                    if (qualifiedCardinalityRestriction.MaxQualifiedCardinality > 0)
                    {
                        if (qualifiedCardinalityRestrictionRegistryEnumerator.Current.Item2 > qualifiedCardinalityRestriction.MaxQualifiedCardinality)
                            passesMaxQualifiedCardinality = false;
                    }

                    //Save the candidate fact if it passes qualified cardinality restriction
                    if (passesMinQualifiedCardinality && passesMaxQualifiedCardinality)
                        result.AddFact(qualifiedCardinalityRestrictionRegistryEnumerator.Current.Item1);
                }

            }
            #endregion

            #region AllValuesFrom/SomeValuesFrom
            else if (ontologyRestrictionClass is RDFOntologyAllValuesFromRestriction || ontologyRestrictionClass is RDFOntologySomeValuesFromRestriction)
            {

                //Item2 is a counter for occurrences of the restricted property with a range member of the restricted "FromClass"
                //Item3 is a counter for occurrences of the restricted property with a range member not of the restricted "FromClass"
                var valuesFromRegistry = new Dictionary<long, Tuple<RDFOntologyFact, long, long>>();

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
                    //Initialize the occurrence counters of the subject fact
                    if (!valuesFromRegistry.ContainsKey(assertion.TaxonomySubject.PatternMemberID))
                        valuesFromRegistry.Add(assertion.TaxonomySubject.PatternMemberID, new Tuple<RDFOntologyFact, long, long>((RDFOntologyFact)assertion.TaxonomySubject, 0, 0));

                    //Iterate the class types of the object fact, checking presence of the restricted "FromClass"
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

                    //Update the occurrence counters of the subject fact
                    long equalityCounter = valuesFromRegistry[assertion.TaxonomySubject.PatternMemberID].Item2;
                    long differenceCounter = valuesFromRegistry[assertion.TaxonomySubject.PatternMemberID].Item3;
                    if (fromClassFound)
                        valuesFromRegistry[assertion.TaxonomySubject.PatternMemberID] = new Tuple<RDFOntologyFact, long, long>((RDFOntologyFact)assertion.TaxonomySubject, equalityCounter + 1, differenceCounter);
                    else
                        valuesFromRegistry[assertion.TaxonomySubject.PatternMemberID] = new Tuple<RDFOntologyFact, long, long>((RDFOntologyFact)assertion.TaxonomySubject, equalityCounter, differenceCounter + 1);
                }

                //Apply the restriction on the subject facts
                var valuesFromRegistryEnumerator = valuesFromRegistry.Values.GetEnumerator();
                while (valuesFromRegistryEnumerator.MoveNext())
                {
                    //AllValuesFrom
                    if (ontologyRestrictionClass is RDFOntologyAllValuesFromRestriction)
                    {
                        if (valuesFromRegistryEnumerator.Current.Item2 >= 1 && valuesFromRegistryEnumerator.Current.Item3 == 0)
                            result.AddFact(valuesFromRegistryEnumerator.Current.Item1);
                    }
                    //SomeValuesFrom
                    else
                    {
                        if (valuesFromRegistryEnumerator.Current.Item2 >= 1)
                            result.AddFact(valuesFromRegistryEnumerator.Current.Item1);
                    }
                }

            }
            #endregion

            #region HasSelf [OWL2]
            else if (ontologyRestrictionClass is RDFOntologyHasSelfRestriction hasSelfRestriction)
            {
                //Iterate the compatible assertions
                var sameFactsCache = new Dictionary<long, RDFOntologyData>();
                foreach (RDFOntologyTaxonomyEntry assertion in restrictionAssertions.Where(x => x.TaxonomyObject.IsFact()))
                {
                    //Enlist the same facts of the assertion subject
                    if (!sameFactsCache.ContainsKey(assertion.TaxonomySubject.PatternMemberID))
                        sameFactsCache.Add(assertion.TaxonomySubject.PatternMemberID, ontology.Data.GetSameFactsAs((RDFOntologyFact)assertion.TaxonomySubject)
                                                                                                   .AddFact((RDFOntologyFact)assertion.TaxonomySubject));
                    
                    if (sameFactsCache[assertion.TaxonomySubject.PatternMemberID].SelectFact(assertion.TaxonomySubject.ToString()) != null
                            && sameFactsCache[assertion.TaxonomySubject.PatternMemberID].SelectFact(assertion.TaxonomyObject.ToString()) != null)
                        result.AddFact((RDFOntologyFact)assertion.TaxonomySubject);
                }
            }
            #endregion

            #region HasValue
            else if (ontologyRestrictionClass is RDFOntologyHasValueRestriction hasValueRestriction)
            {
                if (hasValueRestriction.RequiredValue.IsFact())
                {
                    //Enlist the same facts of the restriction's "RequiredValue"
                    RDFOntologyData facts = ontology.Data.GetSameFactsAs((RDFOntologyFact)hasValueRestriction.RequiredValue)
                                                         .AddFact((RDFOntologyFact)hasValueRestriction.RequiredValue);

                    //Iterate the compatible assertions and track the subject facts having the required value
                    foreach (RDFOntologyTaxonomyEntry assertion in restrictionAssertions.Where(x => x.TaxonomyObject.IsFact()))
                    {
                        if (facts.SelectFact(assertion.TaxonomyObject.ToString()) != null)
                            result.AddFact((RDFOntologyFact)assertion.TaxonomySubject);
                    }
                }
                else if (hasValueRestriction.RequiredValue.IsLiteral())
                {
                    //Iterate the compatible assertions and track the subject facts having the required value
                    foreach (RDFOntologyTaxonomyEntry assertion in restrictionAssertions.Where(x => x.TaxonomyObject.IsLiteral()))
                    {
                        if (RDFQueryUtilities.CompareRDFPatternMembers(hasValueRestriction.RequiredValue.Value, assertion.TaxonomyObject.Value) == 0)
                            result.AddFact((RDFOntologyFact)assertion.TaxonomySubject);
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
        /// Enlists the facts which are members of the given non literal-compatible class within the given ontology
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