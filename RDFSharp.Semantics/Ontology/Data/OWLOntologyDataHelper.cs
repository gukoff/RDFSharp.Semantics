﻿/*
   Copyright 2012-2023 Marco De Salvo

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
using System.Collections.Generic;
using System.Globalization;
using System.Linq;

namespace RDFSharp.Semantics
{
    /// <summary>
    /// OWLOntologyDataHelper contains methods for analyzing relations describing application domain individuals
    /// </summary>
    public static class OWLOntologyDataHelper
    {
        #region Declarer
        /// <summary>
        /// Checks for the existence of the given owl:NamedIndividual declaration within the data
        /// </summary>
        public static bool CheckHasIndividual(this OWLOntologyData data, RDFResource owlIndividual)
            => owlIndividual != null && data != null && data.Individuals.ContainsKey(owlIndividual.PatternMemberID);

        /// <summary>
        /// Checks for the existence of the given owl:NamedIndividual annotation within the data
        /// </summary>
        public static bool CheckHasAnnotation(this OWLOntologyData data, RDFResource owlIndividual, RDFResource annotationProperty, RDFResource annotationValue)
            => owlIndividual != null && annotationProperty != null && annotationValue != null && data != null && data.OBoxGraph.ContainsTriple(new RDFTriple(owlIndividual, annotationProperty, annotationValue));

        /// <summary>
        /// Checks for the existence of the given owl:NamedIndividual annotation within the data
        /// </summary>
        public static bool CheckHasAnnotation(this OWLOntologyData data, RDFResource owlIndividual, RDFResource annotationProperty, RDFLiteral annotationValue)
            => owlIndividual != null && annotationProperty != null && annotationValue != null && data != null && data.OBoxGraph.ContainsTriple(new RDFTriple(owlIndividual, annotationProperty, annotationValue));

        /// <summary>
        /// Checks for the existence of the given "ObjectProperty(leftIndividual,rightIndividual)" assertion within the data
        /// </summary>
        public static bool CheckHasObjectAssertion(this OWLOntologyData data, RDFResource leftIndividual, RDFResource owlProperty, RDFResource rightIndividual)
            => leftIndividual != null && owlProperty != null && rightIndividual != null && data != null && data.ABoxGraph.ContainsTriple(new RDFTriple(leftIndividual, owlProperty, rightIndividual));

        /// <summary>
        /// Checks for the existence of the given "DatatypeProperty(leftIndividual,value)" assertion within the data
        /// </summary>
        public static bool CheckHasDatatypeAssertion(this OWLOntologyData data, RDFResource owlIndividual, RDFResource owlProperty, RDFLiteral value)
            => owlIndividual != null && owlProperty != null && value != null && data != null && data.ABoxGraph.ContainsTriple(new RDFTriple(owlIndividual, owlProperty, value));

        /// <summary>
        /// Checks for the existence of the given "NegativeObjectProperty(leftIndividual,rightIndividual)" assertion within the data [OWL2]
        /// </summary>
        public static bool CheckHasNegativeObjectAssertion(this OWLOntologyData data, RDFResource leftIndividual, RDFResource owlProperty, RDFResource rightIndividual)
        {
            if (leftIndividual != null && owlProperty != null && rightIndividual != null && data != null)
                return data.ABoxGraph[null, RDFVocabulary.OWL.SOURCE_INDIVIDUAL, leftIndividual, null].TriplesCount > 0
                         && data.ABoxGraph[null, RDFVocabulary.OWL.ASSERTION_PROPERTY, owlProperty, null].TriplesCount > 0
                           && data.ABoxGraph[null, RDFVocabulary.OWL.TARGET_INDIVIDUAL, rightIndividual, null].TriplesCount > 0;
            return false;
        }

        /// <summary>
        /// Checks for the existence of the given "NegativeDatatypeProperty(individual,value)" assertion within the data [OWL2]
        /// </summary>
        public static bool CheckHasNegativeDatatypeAssertion(this OWLOntologyData data, RDFResource individual, RDFResource owlProperty, RDFLiteral value)
        {
            if (individual != null && owlProperty != null && value != null && data != null)
                return data.ABoxGraph[null, RDFVocabulary.OWL.SOURCE_INDIVIDUAL, individual, null].TriplesCount > 0
                         && data.ABoxGraph[null, RDFVocabulary.OWL.ASSERTION_PROPERTY, owlProperty, null].TriplesCount > 0
                           && data.ABoxGraph[null, RDFVocabulary.OWL.TARGET_VALUE, null, value].TriplesCount > 0;
            return false;
        }
        #endregion

        #region Analyzer
        /// <summary>
        /// Checks for the existence of "SameAs(leftIndividual,rightIndividual)" relations within the data
        /// </summary>
        public static bool CheckIsSameIndividual(this OWLOntologyData data, RDFResource leftIndividual, RDFResource rightIndividual)
            => leftIndividual != null && rightIndividual != null && data != null && data.GetSameIndividuals(leftIndividual).Any(individual => individual.Equals(rightIndividual));

        /// <summary>
        /// Analyzes "SameAs(owlIndividual, X)" relations of the data to answer the same individuals of the given owl:Individual
        /// </summary>
        public static List<RDFResource> GetSameIndividuals(this OWLOntologyData data, RDFResource owlIndividual)
        {
            List<RDFResource> sameIndividuals = new List<RDFResource>();

            if (data != null && owlIndividual != null)
            {
                sameIndividuals.AddRange(data.FindSameIndividuals(owlIndividual, new Dictionary<long, RDFResource>()));

                //We don't want to also enlist the given owl:Individual
                sameIndividuals.RemoveAll(individual => individual.Equals(owlIndividual));
            }

            return RDFQueryUtilities.RemoveDuplicates(sameIndividuals);
        }

        /// <summary>
        /// Finds "SameAs(owlIndividual, X)" relations to enlist the same individuals of the given owl:Individual
        /// </summary>
        internal static List<RDFResource> FindSameIndividuals(this OWLOntologyData data, RDFResource owlIndividual, Dictionary<long, RDFResource> visitContext)
        {
            List<RDFResource> sameIndividuals = new List<RDFResource>();

            #region VisitContext
            if (!visitContext.ContainsKey(owlIndividual.PatternMemberID))
                visitContext.Add(owlIndividual.PatternMemberID, owlIndividual);
            else
                return sameIndividuals;
            #endregion

            #region Discovery
            //Find same individuals linked to the given one with owl:sameAs relation
            foreach (RDFTriple sameAsRelation in data.ABoxGraph[owlIndividual, RDFVocabulary.OWL.SAME_AS, null, null])
                sameIndividuals.Add((RDFResource)sameAsRelation.Object);
            #endregion

            // Inference: SAMEAS(A,B) ^ SAMEAS(B,C) -> SAMEAS(A,C)
            foreach (RDFResource sameIndividual in sameIndividuals.ToList())
                sameIndividuals.AddRange(data.FindSameIndividuals(sameIndividual, visitContext));

            return sameIndividuals;
        }

        /// <summary>
        /// Checks for the existence of "DifferentFrom(leftIndividual,rightIndividual)" relations within the data
        /// </summary>
        public static bool CheckIsDifferentIndividual(this OWLOntologyData data, RDFResource leftIndividual, RDFResource rightIndividual)
            => leftIndividual != null && rightIndividual != null && data != null && data.GetDifferentIndividuals(leftIndividual).Any(individual => individual.Equals(rightIndividual));

        /// <summary>
        /// Analyzes "DifferentFrom(owlIndividual, X)" relations of the data to answer the different individuals of the given owl:Individual
        /// </summary>
        public static List<RDFResource> GetDifferentIndividuals(this OWLOntologyData data, RDFResource owlIndividual)
        {
            List<RDFResource> differentIndividuals = new List<RDFResource>();

            if (data != null && owlIndividual != null)
            {
                differentIndividuals.AddRange(data.FindDifferentIndividuals(owlIndividual, new Dictionary<long, RDFResource>()));

                //We don't want to also enlist the given owl:Individual
                differentIndividuals.RemoveAll(individual => individual.Equals(owlIndividual));
            }

            return RDFQueryUtilities.RemoveDuplicates(differentIndividuals);
        }

        /// <summary>
        /// Finds "DifferentFrom(owlIndividual, X)" relations to enlist the different individuals of the given owl:Individual
        /// </summary>
        internal static List<RDFResource> FindDifferentIndividuals(this OWLOntologyData data, RDFResource owlIndividual, Dictionary<long, RDFResource> visitContext)
        {
            List<RDFResource> differentIndividuals = new List<RDFResource>();
            
            #region VisitContext
            if (!visitContext.ContainsKey(owlIndividual.PatternMemberID))
                visitContext.Add(owlIndividual.PatternMemberID, owlIndividual);
            else
                return differentIndividuals;
            #endregion

            #region Discovery
            // Find different individuals linked to the given one with owl:AllDifferent shortcut [OWL2]
            List<RDFResource> allDifferentIndividuals = new List<RDFResource>();
            IEnumerator<RDFResource> allDifferent = data.AllDifferentEnumerator;
            while (allDifferent.MoveNext())
                foreach (RDFTriple allDifferentMembers in data.ABoxGraph[allDifferent.Current, RDFVocabulary.OWL.DISTINCT_MEMBERS, null, null])
                {
                    RDFCollection allDifferentCollection = RDFModelUtilities.DeserializeCollectionFromGraph(data.ABoxGraph, (RDFResource)allDifferentMembers.Object, RDFModelEnums.RDFTripleFlavors.SPO);
                    if (allDifferentCollection.Items.Any(item => item.Equals(owlIndividual)))
                        allDifferentIndividuals.AddRange(allDifferentCollection.OfType<RDFResource>());
                }
            allDifferentIndividuals.RemoveAll(idv => idv.Equals(owlIndividual));

            // Find different individuals linked to the given one with owl:differentFrom relation
            List<RDFResource> differentFromIndividuals = data.ABoxGraph[owlIndividual, RDFVocabulary.OWL.DIFFERENT_FROM, null, null]
                                                           .Select(t => (RDFResource)t.Object)
                                                           .ToList();

            // Merge individuals from both sets into a unique deduplicate working set
            List<RDFResource> differentIndividualsSet = RDFQueryUtilities.RemoveDuplicates(allDifferentIndividuals.Union(differentFromIndividuals).ToList());
            #endregion

            #region Analyze
            // Inference: DIFFERENTFROM(A,B) ^ SAMEAS(B,C) -> DIFFERENTFROM(A,C)
            foreach (RDFResource differentIndividual in differentIndividualsSet)
            {
                differentIndividuals.Add(differentIndividual);
                differentIndividuals.AddRange(data.FindSameIndividuals(differentIndividual, visitContext));
            }

            // Inference: SAMEAS(A,B) ^ DIFFERENTFROM(B,C) -> DIFFERENTFROM(A,C)
            foreach (RDFResource sameAsIndividual in data.GetSameIndividuals(owlIndividual))
                differentIndividuals.AddRange(data.FindDifferentIndividuals(sameAsIndividual, visitContext));
            #endregion

            return differentIndividuals;
        }

        /// <summary>
        /// Checks for the existence of "TransitiveObjectProperty(leftIndividual,rightIndividual)" relations within the data
        /// </summary>
        internal static bool CheckIsTransitiveRelatedIndividual(this OWLOntologyData data, RDFResource leftIndividual, RDFResource transitiveObjectProperty, RDFResource rightIndividual)
            => leftIndividual != null && rightIndividual != null && transitiveObjectProperty != null && data != null && data.GetTransitiveRelatedIndividuals(leftIndividual, transitiveObjectProperty).Any(individual => individual.Equals(rightIndividual));

        /// <summary>
        /// Analyzes "TransitiveObjectProperty(owlIndividual,X)" relations of the data to enlist the individuals which are related to the given owl:Individual through the given owl:TransitiveObjectProperty
        /// </summary>
        internal static List<RDFResource> GetTransitiveRelatedIndividuals(this OWLOntologyData data, RDFResource owlIndividual, RDFResource transitiveObjectProperty)
        {
            List<RDFResource> transitiveRelatedIndividuals = new List<RDFResource>();

            if (data != null && owlIndividual != null)
                transitiveRelatedIndividuals.AddRange(data.FindTransitiveRelatedIndividuals(owlIndividual, transitiveObjectProperty, new Dictionary<long, RDFResource>()));

            return transitiveRelatedIndividuals;
        }

        /// <summary>
        /// Finds "TransitiveObjectProperty(owlIndividual,X)" relations to enlist the individuals which are related to the given owl:Individual through the given owl:TransitiveObjectProperty
        /// </summary>
        internal static List<RDFResource> FindTransitiveRelatedIndividuals(this OWLOntologyData data, RDFResource owlIndividual, RDFResource transitiveObjectProperty, Dictionary<long, RDFResource> visitContext)
        {
            List<RDFResource> transitiveRelatedIndividuals = new List<RDFResource>();

            #region VisitContext
            if (!visitContext.ContainsKey(owlIndividual.PatternMemberID))
                visitContext.Add(owlIndividual.PatternMemberID, owlIndividual);
            else
                return transitiveRelatedIndividuals;
            #endregion

            //DIRECT
            foreach (RDFTriple transitiveRelation in data.ABoxGraph[owlIndividual, transitiveObjectProperty, null, null]
                                                         .Where(t => t.TripleFlavor == RDFModelEnums.RDFTripleFlavors.SPO))
                transitiveRelatedIndividuals.Add((RDFResource)transitiveRelation.Object);

            //INDIRECT (TRANSITIVE)
            foreach (RDFResource transitiveRelatedIndividual in transitiveRelatedIndividuals.ToList())
                transitiveRelatedIndividuals.AddRange(data.FindTransitiveRelatedIndividuals(transitiveRelatedIndividual, transitiveObjectProperty, visitContext));

            return transitiveRelatedIndividuals;
        }

        /// <summary>
        /// Checks for the existence of "Type(owlIndividual,owlClass)" relations within the data and model
        /// </summary>
        public static bool CheckIsIndividualOf(this OWLOntologyData data, OWLOntologyModel model, RDFResource owlIndividual, RDFResource owlClass)
            => owlIndividual != null && owlClass != null && model != null && data != null && data.GetIndividualsOf(model, owlClass).Any(individual => individual.Equals(owlIndividual));

        /// <summary>
        /// Checks for the existence of "Type(X,owlClass)" relations of the data and model to answer the individuals of the given owl:Class
        /// </summary>
        public static List<RDFResource> GetIndividualsOf(this OWLOntologyData data, OWLOntologyModel model, RDFResource owlClass)
        {
            List<RDFResource> individuals = new List<RDFResource>();

            if (data != null && model != null && owlClass != null)
            {
                //Restriction
                if (model.ClassModel.CheckHasRestrictionClass(owlClass))
                    individuals.AddRange(data.FindIndividualsOfRestriction(model, owlClass));

                //Composite
                else if (model.ClassModel.CheckHasCompositeClass(owlClass))
                    individuals.AddRange(data.FindIndividualsOfComposite(model, owlClass));

                //Enumerate
                else if (model.ClassModel.CheckHasEnumerateClass(owlClass))
                    individuals.AddRange(data.FindIndividualsOfEnumerate(model, owlClass));

                //Class
                else if (model.ClassModel.CheckHasClass(owlClass))
                    individuals.AddRange(data.FindIndividualsOfClass(model, owlClass));
            }

            //We don't want to enlist duplicate individuals
            return RDFQueryUtilities.RemoveDuplicates(individuals);
        }

        /// <summary>
        /// Finds "Type(X,owlRestriction)" relations to enlist the individuals of the given owl:Restriction
        /// </summary>
        internal static List<RDFResource> FindIndividualsOfRestriction(this OWLOntologyData data, OWLOntologyModel model, RDFResource owlRestriction)
        {
            //Get owl:onProperty of the given owl:Restriction
            RDFResource onProperty = (RDFResource)model.ClassModel.TBoxGraph[owlRestriction, RDFVocabulary.OWL.ON_PROPERTY, null, null].First().Object;

            //Try handling OWL2-Full anonymous inline property expressions (https://www.w3.org/2007/OWL/wiki/FullSemanticsInversePropertyExpressions)
            //ex:svfRest rdf:type owl:Restriction ;
            //           owl:onProperty [ owl:inverseOf ex:propB ] ;
            //           owl:someValuesFrom  ex:class .
            //:propB owl:inverseOf :propA .
            if (onProperty.IsBlank)
            {
                RDFResource inverseOfAnonymousInlineProperty = model.PropertyModel.GetInversePropertiesOf(onProperty).FirstOrDefault(); //ex:propB
                if (inverseOfAnonymousInlineProperty != null)
                {
                    RDFResource effectiveOnProperty = model.PropertyModel.GetInversePropertiesOf(inverseOfAnonymousInlineProperty).FirstOrDefault(); //ex:propA
                    if (effectiveOnProperty != null)
                        onProperty = effectiveOnProperty;
                }
            }

            //Make the given owl:Restriction also work with sub properties and equivalent properties of the given owl:onProperty
            List<RDFResource> compatibleProperties = model.PropertyModel.GetSubPropertiesOf(onProperty)
                                                       .Union(model.PropertyModel.GetEquivalentPropertiesOf(onProperty)).ToList();

            //Compute graph of assertions impacted by restricted properties
            RDFGraph assertionsGraph = data.ABoxGraph[null, onProperty, null, null];
            foreach (RDFResource compatibleProperty in compatibleProperties)
                assertionsGraph = assertionsGraph.UnionWith(data.ABoxGraph[null, compatibleProperty, null, null]);

            //Detect and handle owl:[Min|Max]CardinalityRestriction
            if (model.ClassModel.CheckHasCardinalityRestrictionClass(owlRestriction)
                 || model.ClassModel.CheckHasMinCardinalityRestrictionClass(owlRestriction)
                  || model.ClassModel.CheckHasMaxCardinalityRestrictionClass(owlRestriction)
                   || model.ClassModel.CheckHasMinMaxCardinalityRestrictionClass(owlRestriction))
                return data.FindIndividualsOfCardinalityRestriction(model, owlRestriction, assertionsGraph, false);

            //Detect and handle owl:[Min|Max]QualifiedCardinalityRestriction [OWL2]
            else if (model.ClassModel.CheckHasQualifiedCardinalityRestrictionClass(owlRestriction)
                      || model.ClassModel.CheckHasMinQualifiedCardinalityRestrictionClass(owlRestriction)
                       || model.ClassModel.CheckHasMaxQualifiedCardinalityRestrictionClass(owlRestriction)
                        || model.ClassModel.CheckHasMinMaxQualifiedCardinalityRestrictionClass(owlRestriction))
                return data.FindIndividualsOfCardinalityRestriction(model, owlRestriction, assertionsGraph, true);

            //Detect and handle owl:[All|Some]ValuesFromRestriction
            else if (model.ClassModel.CheckHasAllValuesFromRestrictionClass(owlRestriction)
                      || model.ClassModel.CheckHasSomeValuesFromRestrictionClass(owlRestriction))
                return data.FindIndividualsOfValuesFromRestriction(model, owlRestriction, assertionsGraph);

            //Detect and handle owl:HasValueRestriction
            else if (model.ClassModel.CheckHasValueRestrictionClass(owlRestriction))
                return data.FindIndividualsOfHasValueRestriction(model, owlRestriction, assertionsGraph);

            //Detect and handle owl:HasSelfRestriction [OWL2]
            else if (model.ClassModel.CheckHasSelfRestrictionClass(owlRestriction))
                return data.FindIndividualsOfHasSelfRestriction(model, owlRestriction, assertionsGraph);

            else
                throw new OWLSemanticsException($"Cannot find individuals of '{owlRestriction}' unknown restriction");
        }

        /// <summary>
        /// Finds "Type(X,owlRestriction)" relations to enlist the individuals of the given owl:[Min|Max][Qualified]CardinalityRestriction [OWL2]
        /// </summary>
        internal static List<RDFResource> FindIndividualsOfCardinalityRestriction(this OWLOntologyData data, OWLOntologyModel model, RDFResource owlRestriction, RDFGraph assertionsGraph, bool isQualified)
        {
            List<RDFResource> individuals = new List<RDFResource>();
            List<RDFResource> onClassIndividuals = new List<RDFResource>();

            #region Parse
            int minCardinality = -99, maxCardinality = -99; //Convention meaning that [min|max]cardinality are not expressed
            RDFResource onClass = null;

            //owl:[Qualified]CardinalityRestriction
            if (model.ClassModel.CheckHasCardinalityRestrictionClass(owlRestriction)
                 || model.ClassModel.CheckHasQualifiedCardinalityRestrictionClass(owlRestriction))
            {
                RDFTriple cardinalityTriple = model.ClassModel.TBoxGraph[owlRestriction, isQualified ? RDFVocabulary.OWL.QUALIFIED_CARDINALITY : RDFVocabulary.OWL.CARDINALITY, null, null].First();
                if (cardinalityTriple.Object is RDFTypedLiteral cardinalityLiteral && cardinalityLiteral.HasDecimalDatatype())
                {
                    int.TryParse(cardinalityLiteral.Value, NumberStyles.Integer, CultureInfo.InvariantCulture, out minCardinality);
                    int.TryParse(cardinalityLiteral.Value, NumberStyles.Integer, CultureInfo.InvariantCulture, out maxCardinality);
                }
            }

            //owl:[Min|Max][Qualified]CardinalityRestriction
            else
            {
                //owl:Min[Qualified]Cardinality
                RDFTriple minCardinalityTriple = model.ClassModel.TBoxGraph[owlRestriction, isQualified ? RDFVocabulary.OWL.MIN_QUALIFIED_CARDINALITY : RDFVocabulary.OWL.MIN_CARDINALITY, null, null].FirstOrDefault();
                if (minCardinalityTriple != null && minCardinalityTriple.Object is RDFTypedLiteral minCardinalityLiteral && minCardinalityLiteral.HasDecimalDatatype())
                    int.TryParse(minCardinalityLiteral.Value, NumberStyles.Integer, CultureInfo.InvariantCulture, out minCardinality);

                //owl:Max[Qualified]Cardinality
                RDFTriple maxCardinalityTriple = model.ClassModel.TBoxGraph[owlRestriction, isQualified ? RDFVocabulary.OWL.MAX_QUALIFIED_CARDINALITY : RDFVocabulary.OWL.MAX_CARDINALITY, null, null].FirstOrDefault();
                if (maxCardinalityTriple != null && maxCardinalityTriple.Object is RDFTypedLiteral maxCardinalityLiteral && maxCardinalityLiteral.HasDecimalDatatype())
                    int.TryParse(maxCardinalityLiteral.Value, NumberStyles.Integer, CultureInfo.InvariantCulture, out maxCardinality);
            }

            //owl:onClass [OWL2]
            if (isQualified)
            {
                //Get owl:onClass of the given owl:Restriction
                onClass = model.ClassModel.TBoxGraph[owlRestriction, RDFVocabulary.OWL.ON_CLASS, null, null].FirstOrDefault()?.Object as RDFResource;
                if (onClass == null)
                    throw new OWLSemanticsException($"Cannot find individuals of owl:[Min|Max]QualifiedCardinalityRestriction '{owlRestriction}' because required owl:onClass information is not declared in the model");

                //Prefetch individuals of owl:onClass
                onClassIndividuals = data.GetIndividualsOf(model, onClass);
            }
            #endregion

            #region Optimize
            //Min[Qualified]Cardinality=0
            // This case is equivalent to targeting all individuals
            if (minCardinality == 0 && maxCardinality == -99)
                return data.Individuals.Values.ToList();

            //Max[Qualified]Cardinality=0
            // This case is equivalent to targeting only individuals:
            // unqualified => not asserting the restricted property at all
            // qualified   => not asserting the restricted property against individuals of restricted OnClass
            if (maxCardinality == 0)
                return isQualified ? data.Where(idv => !assertionsGraph[idv, null, null, null].Any(idvAsn => onClassIndividuals.Any(ocIdv => ocIdv.Equals(idvAsn.Object)))).ToList()
                                   : data.Where(idv => !assertionsGraph[idv, null, null, null].Any()).ToList();

            //Max[Qualified]Cardinality=N
            // This case requires preliminar targeting of individuals:
            // unqualified => not asserting the restricted property at all
            // qualified   => not asserting the restricted property against individuals of restricted OnClass
            if (minCardinality == -99 && maxCardinality != -99)
                individuals.AddRange(isQualified ? data.Where(idv => !assertionsGraph[idv, null, null, null].Any(idvAsn => onClassIndividuals.Any(ocIdv => ocIdv.Equals(idvAsn.Object))))
                                                 : data.Where(idv => !assertionsGraph[idv, null, null, null].Any()));
            #endregion

            #region Count
            //Build: we need to count occurrences (Item2) of each subject individual (Item1);
            //       In case of owl:[Min|Max]QualifiedCardinalityRestriction we must first check
            //       that the object individual effectively belongs to the specified owl:onClass
            var cardinalityRestrictionRegistry = new Dictionary<long, (RDFPatternMember, long)>();
            foreach (RDFTriple assertionTriple in assertionsGraph)
            {
                //Initialize new subject individual's counter
                if (!cardinalityRestrictionRegistry.ContainsKey(assertionTriple.Subject.PatternMemberID))
                    cardinalityRestrictionRegistry.Add(assertionTriple.Subject.PatternMemberID, (assertionTriple.Subject, 0));

                //owl:[Min|Max]QualifiedCardinalityRestriction [OWL2]
                if (isQualified)
                {
                    //Since we have to qualify the object individual, we consider only SPO assertions
                    if (assertionTriple.TripleFlavor == RDFModelEnums.RDFTripleFlavors.SPO
                          && onClassIndividuals.Any(idv => idv.Equals(assertionTriple.Object)))
                    {
                        long occurrencyCounter = cardinalityRestrictionRegistry[assertionTriple.Subject.PatternMemberID].Item2;
                        cardinalityRestrictionRegistry[assertionTriple.Subject.PatternMemberID] = (assertionTriple.Subject, occurrencyCounter + 1);
                    }
                }

                //owl:[Min|Max]CardinalityRestriction
                else
                {
                    long occurrencyCounter = cardinalityRestrictionRegistry[assertionTriple.Subject.PatternMemberID].Item2;
                    cardinalityRestrictionRegistry[assertionTriple.Subject.PatternMemberID] = (assertionTriple.Subject, occurrencyCounter + 1);
                }
            }

            //Analyze: we have to consider only individuals that satisfy given Min/Max occurrences
            var cardinalityRestrictionRegistryEnumerator = cardinalityRestrictionRegistry.Values.GetEnumerator();
            while (cardinalityRestrictionRegistryEnumerator.MoveNext())
            {
                bool passesMinCardinality = true;
                bool passesMaxCardinality = true;

                //owl:MinCardinality requires to reach *at least* the given number of occurrences
                if (minCardinality > 0 && cardinalityRestrictionRegistryEnumerator.Current.Item2 < minCardinality)
                    passesMinCardinality = false;
                //owl:MaxCardinality requires to reach *at most* the given number of occurrences
                if (maxCardinality > 0 && cardinalityRestrictionRegistryEnumerator.Current.Item2 > maxCardinality)
                    passesMaxCardinality = false;

                if (passesMinCardinality && passesMaxCardinality)
                    individuals.Add((RDFResource)cardinalityRestrictionRegistryEnumerator.Current.Item1);
            }
            #endregion

            return individuals;
        }

        /// <summary>
        /// Finds "Type(X,owlRestriction)" relations to enlist the individuals of the given owl:[All|Some]ValuesFromRestriction
        /// </summary>
        internal static List<RDFResource> FindIndividualsOfValuesFromRestriction(this OWLOntologyData data, OWLOntologyModel model, RDFResource owlRestriction, RDFGraph assertionsGraph)
        {
            List<RDFResource> individuals = new List<RDFResource>();

            #region Parse
            //Get owl:[all|some]ValuesFrom class of the given owl:Restriction
            bool isAllValuesFrom = model.ClassModel.CheckHasAllValuesFromRestrictionClass(owlRestriction);
            RDFResource valuesFromClass = model.ClassModel.TBoxGraph[owlRestriction, isAllValuesFrom ? RDFVocabulary.OWL.ALL_VALUES_FROM : RDFVocabulary.OWL.SOME_VALUES_FROM, null, null].First().Object as RDFResource;
            if (valuesFromClass == null)
                throw new OWLSemanticsException($"Cannot find individuals of owl:[All|Some]ValuesFromRestriction '{owlRestriction}' because required owl:[all|some]ValuesFrom information is not declared in the model");

            //Materialize individuals of the given owl:[all|some]ValuesFrom class
            List<RDFResource> acceptableIndividuals = data.GetIndividualsOf(model, valuesFromClass);
            #endregion

            #region Count
            //Build: we need to count occurrences of range individuals belonging to compatible classes (Item2) or not (Item3)
            //       for each subject individual (Item1)
            var valuesFromRegistry = new Dictionary<long, (RDFPatternMember, long, long)>();
            foreach (RDFTriple assertionTriple in assertionsGraph.Where(asn => asn.TripleFlavor == RDFModelEnums.RDFTripleFlavors.SPO))
            {
                //Initialize new subject individual's counter
                if (!valuesFromRegistry.ContainsKey(assertionTriple.Subject.PatternMemberID))
                    valuesFromRegistry.Add(assertionTriple.Subject.PatternMemberID, (assertionTriple.Subject, 0, 0));

                //Update the occurrence counters of the subject individual
                long equalityCounter = valuesFromRegistry[assertionTriple.Subject.PatternMemberID].Item2;
                long differenceCounter = valuesFromRegistry[assertionTriple.Subject.PatternMemberID].Item3;
                if (acceptableIndividuals.Any(idv => idv.Equals(assertionTriple.Object)))
                    valuesFromRegistry[assertionTriple.Subject.PatternMemberID] = (assertionTriple.Subject, equalityCounter + 1, differenceCounter);
                else
                    valuesFromRegistry[assertionTriple.Subject.PatternMemberID] = (assertionTriple.Subject, equalityCounter, differenceCounter + 1);
            }

            //Analyze: we have to consider only individuals that satisfy given All/Some occurrences constraint
            var valuesFromRegistryEnumerator = valuesFromRegistry.Values.GetEnumerator();
            while (valuesFromRegistryEnumerator.MoveNext())
            {
                //owl:allValuesFromRestriction => differenceCounter strictly required to be zero
                if (isAllValuesFrom)
                {
                    if (valuesFromRegistryEnumerator.Current.Item2 >= 1 && valuesFromRegistryEnumerator.Current.Item3 == 0)
                        individuals.Add((RDFResource)valuesFromRegistryEnumerator.Current.Item1);
                }
                //owl:someValuesFromRestriction
                else
                {
                    if (valuesFromRegistryEnumerator.Current.Item2 >= 1)
                        individuals.Add((RDFResource)valuesFromRegistryEnumerator.Current.Item1);
                }
            }
            #endregion

            return individuals;
        }

        /// <summary>
        /// Finds "Type(X,owlRestriction)" relations to enlist the individuals of the given owl:HasValueRestriction
        /// </summary>
        internal static List<RDFResource> FindIndividualsOfHasValueRestriction(this OWLOntologyData data, OWLOntologyModel model, RDFResource owlRestriction, RDFGraph assertionsGraph)
        {
            List<RDFResource> individuals = new List<RDFResource>();

            //Get owl:hasValue of the given owl:Restriction
            RDFPatternMember hasValue = model.ClassModel.TBoxGraph[owlRestriction, RDFVocabulary.OWL.HAS_VALUE, null, null].First().Object;
            if (hasValue is RDFResource hasValueIndividual)
            {
                //Make the given owl:Restriction also work with same individuals of the given owl:hasValue individual
                List<RDFResource> sameHasValueIndividuals = data.GetSameIndividuals(hasValueIndividual);

                //Find SPO assertions having object individual compatible with owl:hasValue individual
                foreach (RDFTriple assertionTriple in assertionsGraph.Where(t => t.TripleFlavor == RDFModelEnums.RDFTripleFlavors.SPO))
                {
                    if (assertionTriple.Object.Equals(hasValueIndividual) || sameHasValueIndividuals.Any(sameHasValueIndividual => sameHasValueIndividual.Equals((RDFResource)assertionTriple.Object)))
                        individuals.Add((RDFResource)assertionTriple.Subject);
                }
            }
            else
            {
                //Find SPL assertions having object literal compatible with owl:hasValue literal
                foreach (RDFTriple assertionTriple in assertionsGraph.Where(t => t.TripleFlavor == RDFModelEnums.RDFTripleFlavors.SPL))
                {
                    if (RDFQueryUtilities.CompareRDFPatternMembers(hasValue, assertionTriple.Object) == 0)
                        individuals.Add((RDFResource)assertionTriple.Subject);
                }
            }

            //We don't want to enlist duplicate individuals
            return RDFQueryUtilities.RemoveDuplicates(individuals);
        }

        /// <summary>
        /// Finds "Type(X,owlRestriction)" relations to enlist the individuals of the given owl:HasSelfRestriction [OWL2]
        /// </summary>
        internal static List<RDFResource> FindIndividualsOfHasSelfRestriction(this OWLOntologyData data, OWLOntologyModel model, RDFResource owlRestriction, RDFGraph assertionsGraph)
        {
            List<RDFResource> individuals = new List<RDFResource>();

            bool hasSelfTrue = model.ClassModel.TBoxGraph.ContainsTriple(new RDFTriple(owlRestriction, RDFVocabulary.OWL.HAS_SELF, RDFTypedLiteral.True));
            bool hasSelfFalse = model.ClassModel.TBoxGraph.ContainsTriple(new RDFTriple(owlRestriction, RDFVocabulary.OWL.HAS_SELF, RDFTypedLiteral.False));
            foreach (IGrouping<RDFPatternMember, RDFTriple> assertionGroup in assertionsGraph.GroupBy(asn => asn.Subject))
            {
                //owl:hasSelf(TRUE) => At least one occurrence of the restricted property must link the same subject/object individual
                if (hasSelfTrue && assertionGroup.Any(asn => asn.Subject.Equals(asn.Object)))
                    individuals.Add((RDFResource)assertionGroup.Key);

                //owl:hasSelf(FALSE) => No occurrences of the restricted property must link the same subject/object individual
                if (hasSelfFalse && !assertionGroup.Any(asn => asn.Subject.Equals(asn.Object)))
                    individuals.Add((RDFResource)assertionGroup.Key);
            }

            //We don't want to enlist duplicate individuals
            return RDFQueryUtilities.RemoveDuplicates(individuals);
        }

        /// <summary>
        /// Finds "Type(X,owlClass)" relations to enlist the individuals of the given composite owl:[unionOf|intersectionOf|complementOf] class
        /// </summary>
        internal static List<RDFResource> FindIndividualsOfComposite(this OWLOntologyData data, OWLOntologyModel model, RDFResource owlComposite)
        {
            List<RDFResource> compositeIndividuals = new List<RDFResource>();

            //owl:unionOf
            if (model.ClassModel.CheckHasCompositeUnionClass(owlComposite))
            {
                //Restrict T-BOX knowledge to owl:unionOf relations (explicit)
                RDFGraph unionOfGraph = model.ClassModel.TBoxGraph[owlComposite, RDFVocabulary.OWL.UNION_OF, null, null];

                //Compute union of answered individuals
                RDFCollection unionOfCollection = RDFModelUtilities.DeserializeCollectionFromGraph(model.ClassModel.TBoxGraph, (RDFResource)unionOfGraph.First().Object, RDFModelEnums.RDFTripleFlavors.SPO);
                foreach (RDFResource unionOfClass in unionOfCollection)
                    compositeIndividuals.AddRange(data.GetIndividualsOf(model, unionOfClass));
            }

            //owl:intersectionOf
            else if (model.ClassModel.CheckHasCompositeIntersectionClass(owlComposite))
            {
                //Restrict T-BOX knowledge to owl:intersectionOf relations (explicit)
                RDFGraph intersectionOfGraph = model.ClassModel.TBoxGraph[owlComposite, RDFVocabulary.OWL.INTERSECTION_OF, null, null];

                //Compute intersection of answered individuals
                bool isFirstIntersectionClass = true;
                RDFCollection intersectionOfCollection = RDFModelUtilities.DeserializeCollectionFromGraph(model.ClassModel.TBoxGraph, (RDFResource)intersectionOfGraph.First().Object, RDFModelEnums.RDFTripleFlavors.SPO);
                foreach (RDFResource intersectionOfClass in intersectionOfCollection)
                {
                    List<RDFResource> currentClassIndividuals = data.GetIndividualsOf(model, intersectionOfClass);
                    if (isFirstIntersectionClass)
                    {
                        compositeIndividuals.AddRange(currentClassIndividuals);
                        isFirstIntersectionClass = false;
                    }
                    else
                        compositeIndividuals.RemoveAll(individual => !currentClassIndividuals.Any(idv => idv.Equals(individual)));
                }
            }

            //owl:complementOf
            else if (model.ClassModel.CheckHasCompositeComplementClass(owlComposite))
            {
                //Restrict T-BOX knowledge to owl:complementOf relations (explicit)
                RDFGraph complementOfGraph = model.ClassModel.TBoxGraph[owlComposite, RDFVocabulary.OWL.COMPLEMENT_OF, null, null];

                //Compute complement of answered individuals
                List<RDFResource> complementedClassIndividuals = data.GetIndividualsOf(model, (RDFResource)complementOfGraph.First().Object);
                compositeIndividuals.AddRange(data.Where(individual => !complementedClassIndividuals.Any(idv => idv.Equals(individual))));
            }

            return compositeIndividuals;
        }

        /// <summary>
        /// Finds "Type(X,owlClass)" relations to enlist the individuals of the given composite owl:oneOf class
        /// </summary>
        internal static List<RDFResource> FindIndividualsOfEnumerate(this OWLOntologyData data, OWLOntologyModel model, RDFResource owlEnumerate)
        {
            List<RDFResource> enumerateIndividuals = new List<RDFResource>();

            //Restrict T-BOX knowledge to owl:oneOf relations (explicit)
            RDFGraph oneOfGraph = model.ClassModel.TBoxGraph[owlEnumerate, RDFVocabulary.OWL.ONE_OF, null, null];

            //Compute answered individuals
            RDFCollection enumerateIndividualsCollection = RDFModelUtilities.DeserializeCollectionFromGraph(model.ClassModel.TBoxGraph, (RDFResource)oneOfGraph.First().Object, RDFModelEnums.RDFTripleFlavors.SPO);
            foreach (RDFResource enumerateIndividual in enumerateIndividualsCollection)
                enumerateIndividuals.Add(enumerateIndividual);

            return enumerateIndividuals;
        }

        /// <summary>
        /// Finds "Type(X,owlClass)" relations to enlist the individuals of the given owl:Class
        /// </summary>
        internal static List<RDFResource> FindIndividualsOfClass(this OWLOntologyData data, OWLOntologyModel model, RDFResource owlClass)
        {
            List<RDFResource> classIndividuals = new List<RDFResource>();

            //Get the classes compatible with the given class
            List<RDFResource> compatibleClasses = new List<RDFResource>() { owlClass }
                                                    .Union(model.ClassModel.GetSubClassesOf(owlClass))
                                                    .Union(model.ClassModel.GetEquivalentClassesOf(owlClass))
                                                    .ToList();

            //Get the individuals belonging to the compatible classes
            List<RDFResource> compatibleIndividuals = data.ABoxGraph[null, RDFVocabulary.RDF.TYPE, null, null]
                                                          .Where(t => compatibleClasses.Any(cls => cls.Equals(t.Object)))
                                                          .Select(t => t.Subject)
                                                          .OfType<RDFResource>()
                                                          .ToList();

            //Add the individuals to the results
            classIndividuals.AddRange(compatibleIndividuals);

            //Add compatible individuals to the results (exploit owl:sameAs relations)
            foreach (RDFResource compatibleIndividual in compatibleIndividuals)
                classIndividuals.AddRange(data.GetSameIndividuals(compatibleIndividual));

            return classIndividuals;
        }
        #endregion

        #region Checker
        /// <summary>
        /// Checks if the given leftIndividual can be same as the given rightIndividual without tampering OWL-DL integrity
        /// </summary>
        internal static bool CheckSameAsCompatibility(this OWLOntologyData data, RDFResource leftIndividual, RDFResource rightIndividual)
            => !data.CheckIsDifferentIndividual(rightIndividual, leftIndividual);

        /// <summary>
        /// Checks if the given leftIndividual can be different from the given rightIndividual without tampering OWL-DL integrity
        /// </summary>
        internal static bool CheckDifferentFromCompatibility(this OWLOntologyData data, RDFResource leftIndividual, RDFResource rightIndividual)
            => !data.CheckIsSameIndividual(rightIndividual, leftIndividual);

        /// <summary>
        /// Checks if the given leftIndividual can be linked to the given rightIndividual through the given objectProperty without tampering OWL-DL integrity
        /// </summary>
        internal static bool CheckObjectAssertionCompatibility(this OWLOntologyData data, RDFResource leftIndividual, RDFResource objectProperty, RDFResource rightIndividual)
            => !data.CheckHasNegativeObjectAssertion(leftIndividual, objectProperty, rightIndividual);

        /// <summary>
        /// Checks if the given leftIndividual can be linked to the given value through the given datatypeProperty without tampering OWL-DL integrity
        /// </summary>
        internal static bool CheckDatatypeAssertionCompatibility(this OWLOntologyData data, RDFResource individual, RDFResource datatypeProperty, RDFLiteral value)
            => !data.CheckHasNegativeDatatypeAssertion(individual, datatypeProperty, value);

        /// <summary>
        /// Checks if the given leftIndividual can be linked to the given rightIndividual through the given negative objectProperty without tampering OWL-DL integrity [OWL2]
        /// </summary>
        internal static bool CheckNegativeObjectAssertionCompatibility(this OWLOntologyData data, RDFResource leftIndividual, RDFResource objectProperty, RDFResource rightIndividual)
            => !data.CheckHasObjectAssertion(leftIndividual, objectProperty, rightIndividual);

        /// <summary>
        /// Checks if the given leftIndividual can be linked to the given value through the given negative datatypeProperty without tampering OWL-DL integrity [OWL2]
        /// </summary>
        internal static bool CheckNegativeDatatypeAssertionCompatibility(this OWLOntologyData data, RDFResource individual, RDFResource datatypeProperty, RDFLiteral value)
            => !data.CheckHasDatatypeAssertion(individual, datatypeProperty, value);
        #endregion
    }
}