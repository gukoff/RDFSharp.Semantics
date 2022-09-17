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
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace RDFSharp.Semantics
{
    /// <summary>
    /// RDFOntologyPropertyModelHelper contains methods for analyzing relations describing application domain properties
    /// </summary>
    public static class RDFOntologyPropertyModelHelper
    {
        #region Analyzer
        /// <summary>
        /// Checks for the existence of the given owl:Property declaration within the model
        /// </summary>
        public static bool CheckHasProperty(this RDFOntologyPropertyModel propertyModel, RDFResource owlProperty)
            => owlProperty != null && propertyModel != null && propertyModel.Properties.ContainsKey(owlProperty.PatternMemberID);

        /// <summary>
        /// Checks for the existence of the given owl:AnnotationProperty declaration within the model
        /// </summary>
        public static bool CheckHasAnnotationProperty(this RDFOntologyPropertyModel propertyModel, RDFResource owlProperty)
            => CheckHasProperty(propertyModel, owlProperty)
                && propertyModel.TBoxGraph.ContainsTriple(new RDFTriple(owlProperty, RDFVocabulary.RDF.TYPE, RDFVocabulary.OWL.ANNOTATION_PROPERTY));

        /// <summary>
        /// Checks for the existence of the given owl:ObjectProperty declaration within the model
        /// </summary>
        public static bool CheckHasObjectProperty(this RDFOntologyPropertyModel propertyModel, RDFResource owlProperty)
            => CheckHasProperty(propertyModel, owlProperty)
                && propertyModel.TBoxGraph.ContainsTriple(new RDFTriple(owlProperty, RDFVocabulary.RDF.TYPE, RDFVocabulary.OWL.OBJECT_PROPERTY));

        /// <summary>
        /// Checks for the existence of the given owl:DatatypeProperty declaration within the model
        /// </summary>
        public static bool CheckHasDatatypeProperty(this RDFOntologyPropertyModel propertyModel, RDFResource owlProperty)
            => CheckHasProperty(propertyModel, owlProperty)
                && propertyModel.TBoxGraph.ContainsTriple(new RDFTriple(owlProperty, RDFVocabulary.RDF.TYPE, RDFVocabulary.OWL.DATATYPE_PROPERTY));

        /// <summary>
        /// Checks for the existence of the given owl:DeprecatedProperty declaration within the model
        /// </summary>
        public static bool CheckHasDeprecatedProperty(this RDFOntologyPropertyModel propertyModel, RDFResource owlProperty)
            => CheckHasProperty(propertyModel, owlProperty)
                && propertyModel.TBoxGraph.ContainsTriple(new RDFTriple(owlProperty, RDFVocabulary.RDF.TYPE, RDFVocabulary.OWL.DEPRECATED_PROPERTY));

        /// <summary>
        /// Checks for the existence of the given owl:FunctionalProperty declaration within the model
        /// </summary>
        public static bool CheckHasFunctionalProperty(this RDFOntologyPropertyModel propertyModel, RDFResource owlProperty)
            => CheckHasProperty(propertyModel, owlProperty)
                && propertyModel.TBoxGraph.ContainsTriple(new RDFTriple(owlProperty, RDFVocabulary.RDF.TYPE, RDFVocabulary.OWL.FUNCTIONAL_PROPERTY));

        /// <summary>
        /// Checks for the existence of the given owl:InverseFunctionalProperty declaration within the model
        /// </summary>
        public static bool CheckHasInverseFunctionalProperty(this RDFOntologyPropertyModel propertyModel, RDFResource owlProperty)
            => CheckHasObjectProperty(propertyModel, owlProperty)
                && propertyModel.TBoxGraph.ContainsTriple(new RDFTriple(owlProperty, RDFVocabulary.RDF.TYPE, RDFVocabulary.OWL.INVERSE_FUNCTIONAL_PROPERTY));

        /// <summary>
        /// Checks for the existence of the given owl:SymmetricProperty declaration within the model
        /// </summary>
        public static bool CheckHasSymmetricProperty(this RDFOntologyPropertyModel propertyModel, RDFResource owlProperty)
            => CheckHasObjectProperty(propertyModel, owlProperty)
                && propertyModel.TBoxGraph.ContainsTriple(new RDFTriple(owlProperty, RDFVocabulary.RDF.TYPE, RDFVocabulary.OWL.SYMMETRIC_PROPERTY));

        /// <summary>
        /// Checks for the existence of the given owl:TransitiveProperty declaration within the model
        /// </summary>
        public static bool CheckHasTransitiveProperty(this RDFOntologyPropertyModel propertyModel, RDFResource owlProperty)
            => CheckHasObjectProperty(propertyModel, owlProperty)
                && propertyModel.TBoxGraph.ContainsTriple(new RDFTriple(owlProperty, RDFVocabulary.RDF.TYPE, RDFVocabulary.OWL.TRANSITIVE_PROPERTY));

        /// <summary>
        /// Checks for the existence of the given owl:AsymmetricProperty declaration within the model [OWL2]
        /// </summary>
        public static bool CheckHasAsymmetricProperty(this RDFOntologyPropertyModel propertyModel, RDFResource owlProperty)
            => CheckHasObjectProperty(propertyModel, owlProperty)
                && propertyModel.TBoxGraph.ContainsTriple(new RDFTriple(owlProperty, RDFVocabulary.RDF.TYPE, RDFVocabulary.OWL.ASYMMETRIC_PROPERTY));

        /// <summary>
        /// Checks for the existence of the given owl:ReflexiveProperty declaration within the model [OWL2]
        /// </summary>
        public static bool CheckHasReflexiveProperty(this RDFOntologyPropertyModel propertyModel, RDFResource owlProperty)
            => CheckHasObjectProperty(propertyModel, owlProperty)
                && propertyModel.TBoxGraph.ContainsTriple(new RDFTriple(owlProperty, RDFVocabulary.RDF.TYPE, RDFVocabulary.OWL.REFLEXIVE_PROPERTY));

        /// <summary>
        /// Checks for the existence of the given owl:IrreflexiveProperty declaration within the model [OWL2]
        /// </summary>
        public static bool CheckHasIrreflexiveProperty(this RDFOntologyPropertyModel propertyModel, RDFResource owlProperty)
            => CheckHasObjectProperty(propertyModel, owlProperty)
                && propertyModel.TBoxGraph.ContainsTriple(new RDFTriple(owlProperty, RDFVocabulary.RDF.TYPE, RDFVocabulary.OWL.IRREFLEXIVE_PROPERTY));

        /// <summary>
        /// Checks for the existence of the given owl:Property annotation within the model
        /// </summary>
        public static bool CheckHasAnnotation(this RDFOntologyPropertyModel propertyModel, RDFResource owlProperty, RDFResource annotationProperty, RDFResource annotationValue)
            => owlProperty != null && annotationProperty != null && annotationValue != null && propertyModel != null && propertyModel.TBoxGraph.ContainsTriple(new RDFTriple(owlProperty, annotationProperty, annotationValue));

        /// <summary>
        /// Checks for the existence of the given owl:Property annotation within the model
        /// </summary>
        public static bool CheckHasAnnotation(this RDFOntologyPropertyModel propertyModel, RDFResource owlProperty, RDFResource annotationProperty, RDFLiteral annotationValue)
            => owlProperty != null && annotationProperty != null && annotationValue != null && propertyModel != null && propertyModel.TBoxGraph.ContainsTriple(new RDFTriple(owlProperty, annotationProperty, annotationValue));

        /// <summary>
        /// Checks for the existence of the given owl:propertyChainAxiom declaration within the model [OWL2]
        /// </summary>
        public static bool CheckHasPropertyChainAxiom(this RDFOntologyPropertyModel propertyModel, RDFResource owlProperty)
            => CheckHasObjectProperty(propertyModel, owlProperty)
                && propertyModel.TBoxGraph.Any(t => t.Subject.Equals(owlProperty) && t.Predicate.Equals(RDFVocabulary.OWL.PROPERTY_CHAIN_AXIOM));

        /// <summary>
        /// Checks for the existence of "SubProperty(childProperty,motherProperty)" relations within the model
        /// </summary>
        public static bool CheckAreSubProperties(this RDFOntologyPropertyModel propertyModel, RDFResource childProperty, RDFResource motherProperty)
            => childProperty != null && motherProperty != null && propertyModel != null && propertyModel.AnswerSubProperties(motherProperty).Any(prop => prop.Equals(childProperty));

        /// <summary>
        /// Analyzes "SubProperty(owlProperty, X)" relations of the model to answer the sub property of the given owl:Property
        /// </summary>
        public static List<RDFResource> AnswerSubProperties(this RDFOntologyPropertyModel propertyModel, RDFResource owlProperty)
        {
            List<RDFResource> subProperties = new List<RDFResource>();

            if (propertyModel != null && owlProperty != null)
            {
                subProperties.AddRange(propertyModel.FindSubProperties(owlProperty, propertyModel.TBoxVirtualGraph, new Dictionary<long, RDFResource>()));

                //We don't want to also enlist the given owl:Property
                subProperties.RemoveAll(prop => prop.Equals(owlProperty));
            }

            return subProperties;
        }

        /// <summary>
        /// Finds "SubProperty(owlProperty, X)" relations to enlist the sub properties of the given owl:Property
        /// </summary>
        internal static List<RDFResource> FindSubProperties(this RDFOntologyPropertyModel propertyModel, RDFResource owlProperty, RDFGraph tboxGraph, Dictionary<long, RDFResource> visitContext)
        {
            List<RDFResource> subProperties = new List<RDFResource>();

            #region VisitContext
            if (!visitContext.ContainsKey(owlProperty.PatternMemberID))
                visitContext.Add(owlProperty.PatternMemberID, owlProperty);
            else
                return subProperties;
            #endregion

            //DIRECT
            foreach (RDFTriple subPropertyRelation in tboxGraph[null, RDFVocabulary.RDFS.SUB_PROPERTY_OF, owlProperty, null])
                subProperties.Add((RDFResource)subPropertyRelation.Subject);

            //INDIRECT (TRANSITIVE)
            foreach (RDFResource subProperty in subProperties.ToList())
                subProperties.AddRange(propertyModel.FindSubProperties(subProperty, tboxGraph, visitContext));

            return subProperties;
        }

        /// <summary>
        /// Checks for the existence of "SuperProperty(motherProperty,childProperty)" relations within the model
        /// </summary>
        public static bool CheckAreSuperPropertes(this RDFOntologyPropertyModel propertyModel, RDFResource motherProperty, RDFResource childProperty)
            => childProperty != null && motherProperty != null && propertyModel != null && propertyModel.AnswerSuperProperties(childProperty).Any(prop => prop.Equals(motherProperty));

        /// <summary>
        /// Analyzes "SuperProperty(owlProperty, X)" relations of the model to answer the super properties of the given owl:Property
        /// </summary>
        public static List<RDFResource> AnswerSuperProperties(this RDFOntologyPropertyModel propertyModel, RDFResource owlProperty)
        {
            List<RDFResource> superProperties = new List<RDFResource>();

            if (propertyModel != null && owlProperty != null)
            {
                superProperties.AddRange(propertyModel.FindSuperProperties(owlProperty, propertyModel.TBoxVirtualGraph, new Dictionary<long, RDFResource>()));

                //We don't want to also enlist the given owl:Property
                superProperties.RemoveAll(prop => prop.Equals(owlProperty));
            }

            return superProperties;
        }

        /// <summary>
        /// Finds "SuperProperty(owlProperty, X)" relations to enlist the super properties of the given owl:Property
        /// </summary>
        internal static List<RDFResource> FindSuperProperties(this RDFOntologyPropertyModel propertyModel, RDFResource owlProperty, RDFGraph tboxGraph, Dictionary<long, RDFResource> visitContext)
        {
            List<RDFResource> superProperties = new List<RDFResource>();

            #region VisitContext
            if (!visitContext.ContainsKey(owlProperty.PatternMemberID))
                visitContext.Add(owlProperty.PatternMemberID, owlProperty);
            else
                return superProperties;
            #endregion

            //DIRECT
            foreach (RDFTriple superPropertyRelation in tboxGraph[owlProperty, RDFVocabulary.RDFS.SUB_PROPERTY_OF, null, null])
                superProperties.Add((RDFResource)superPropertyRelation.Object);

            //INDIRECT (TRANSITIVE)
            foreach (RDFResource superProperty in superProperties.ToList())
                superProperties.AddRange(propertyModel.FindSuperProperties(superProperty, tboxGraph, visitContext));

            return superProperties;
        }

        /// <summary>
        /// Checks for the existence of "EquivalentProperty(leftProperty,rightProperty)" relations within the model
        /// </summary>
        public static bool CheckAreEquivalentProperties(this RDFOntologyPropertyModel propertyModel, RDFResource leftProperty, RDFResource rightProperty)
            => leftProperty != null && rightProperty != null && propertyModel != null && propertyModel.AnswerEquivalentProperties(leftProperty).Any(prop => prop.Equals(rightProperty));

        /// <summary>
        /// Analyzes "EquivalentProperty(owlProperty, X)" relations of the model to answer the equivalent properties of the given owl:Property
        /// </summary>
        public static List<RDFResource> AnswerEquivalentProperties(this RDFOntologyPropertyModel propertyModel, RDFResource owlProperty)
        {
            List<RDFResource> equivalentProperties = new List<RDFResource>();

            if (propertyModel != null && owlProperty != null)
            {
                equivalentProperties.AddRange(propertyModel.FindEquivalentProperties(owlProperty, propertyModel.TBoxVirtualGraph, new Dictionary<long, RDFResource>()));

                //We don't want to also enlist the given owl:Property
                equivalentProperties.RemoveAll(prop => prop.Equals(owlProperty));
            }

            return equivalentProperties;
        }

        /// <summary>
        /// Finds "EquivalentProperty(owlProperty, X)" relations to enlist the equivalent properties of the given owl:Property
        /// </summary>
        internal static List<RDFResource> FindEquivalentProperties(this RDFOntologyPropertyModel propertyModel, RDFResource owlProperty, RDFGraph tboxGraph, Dictionary<long, RDFResource> visitContext)
        {
            List<RDFResource> equivalentProperties = new List<RDFResource>();

            #region VisitContext
            if (!visitContext.ContainsKey(owlProperty.PatternMemberID))
                visitContext.Add(owlProperty.PatternMemberID, owlProperty);
            else
                return equivalentProperties;
            #endregion

            //DIRECT
            foreach (RDFTriple equivalentPropertyRelation in tboxGraph[owlProperty, RDFVocabulary.OWL.EQUIVALENT_PROPERTY, null, null])
                equivalentProperties.Add((RDFResource)equivalentPropertyRelation.Object);

            //INDIRECT (TRANSITIVE)
            foreach (RDFResource equivalentProperty in equivalentProperties.ToList())
                equivalentProperties.AddRange(propertyModel.FindEquivalentProperties(equivalentProperty, tboxGraph, visitContext));

            return equivalentProperties;
        }

        /// <summary>
        /// Checks for the existence of "PropertyDisjointWith(leftProperty,rightProperty)" relations within the model [OWL2]
        /// </summary>
        public static bool CheckAreDisjointProperties(this RDFOntologyPropertyModel propertyModel, RDFResource leftProperty, RDFResource rightProperty)
            => leftProperty != null && rightProperty != null && propertyModel != null && propertyModel.AnswerDisjointProperties(leftProperty).Any(prop => prop.Equals(rightProperty));

        /// <summary>
        /// Analyzes "PropertyDisjointWith(leftProperty,rightProperty)" relations of the model to answer the disjoint properties of the given owl:Property [OWL2]
        /// </summary>
        public static List<RDFResource> AnswerDisjointProperties(this RDFOntologyPropertyModel propertyModel, RDFResource owlProperty)
        {
            List<RDFResource> disjointProperties = new List<RDFResource>();

            if (propertyModel != null && owlProperty != null)
            {
                disjointProperties.AddRange(propertyModel.FindDisjointProperties(owlProperty, propertyModel.TBoxVirtualGraph, new Dictionary<long, RDFResource>()));

                //We don't want to also enlist the given owl:Property
                disjointProperties.RemoveAll(prop => prop.Equals(owlProperty));
            }

            return disjointProperties;
        }

        /// <summary>
        /// Finds "PropertyDisjointWith(owlProperty, X)" relations to enlist the disjoint properties of the given owl:Property [OWL2]
        /// </summary>
        internal static List<RDFResource> FindDisjointProperties(this RDFOntologyPropertyModel propertyModel, RDFResource owlProperty, RDFGraph tboxGraph, Dictionary<long, RDFResource> visitContext)
        {
            List<RDFResource> disjointProperties = new List<RDFResource>();

            #region VisitContext
            if (!visitContext.ContainsKey(owlProperty.PatternMemberID))
                visitContext.Add(owlProperty.PatternMemberID, owlProperty);
            else
                return disjointProperties;
            #endregion

            #region Discovery
            // Find disjoint properties linked to the given one with owl:AllDisjointProperties shortcut [OWL2]
            List<RDFResource> allDisjointProperties = new List<RDFResource>();
            IEnumerator<RDFResource> allDisjoint = propertyModel.AllDisjointPropertiesEnumerator;
            while (allDisjoint.MoveNext())
                foreach (RDFTriple allDisjointMembers in tboxGraph[allDisjoint.Current, RDFVocabulary.OWL.MEMBERS, null, null])
                {
                    RDFCollection allDisjointCollection = RDFModelUtilities.DeserializeCollectionFromGraph(tboxGraph, (RDFResource)allDisjointMembers.Object, RDFModelEnums.RDFTripleFlavors.SPO);
                    if (allDisjointCollection.Items.Any(item => item.Equals(owlProperty)))
                        allDisjointProperties.AddRange(allDisjointCollection.OfType<RDFResource>());
                }
            allDisjointProperties.RemoveAll(idv => idv.Equals(owlProperty));

            // Find disjoint properties linked to the given one with owl:propertyDisjointWith relation [OWL2]
            List<RDFResource> disjointFromProperties = tboxGraph[owlProperty, RDFVocabulary.OWL.PROPERTY_DISJOINT_WITH, null, null]
                                                         .Select(t => (RDFResource)t.Object)
                                                         .ToList();

            // Merge properties from both sets into a unique deduplicate working set
            List<RDFResource> disjointPropertiesSet = RDFQueryUtilities.RemoveDuplicates(allDisjointProperties.Union(disjointFromProperties)
                                                                                                              .ToList());
            #endregion

            #region Analyze
            // Inference: PROPERTYDISJOINTWITH(A,B) ^ EQUIVALENTPROPERTY(B,C) -> PROPERTYDISJOINTWITH(A,C)
            foreach (RDFResource disjointProperty in disjointPropertiesSet)
            {
                disjointProperties.Add(disjointProperty);
                disjointProperties.AddRange(propertyModel.FindEquivalentProperties(disjointProperty, tboxGraph, visitContext));
            }

            // Inference: EQUIVALENTPROPERTY(A,B) ^ PROPERTYDISJOINTWITH(B,C) -> PROPERTYDISJOINTWITH(A,C)
            foreach (RDFResource equivalentProperty in propertyModel.AnswerEquivalentProperties(owlProperty))
                disjointProperties.AddRange(propertyModel.FindDisjointProperties(equivalentProperty, tboxGraph, visitContext));
            #endregion

            return disjointProperties;
        }

        /// <summary>
        /// Checks for the existence of "InverseOf(leftProperty,rightProperty)" relations within the model
        /// </summary>
        public static bool CheckAreInverseProperties(this RDFOntologyPropertyModel propertyModel, RDFResource leftProperty, RDFResource rightProperty)
            => leftProperty != null && rightProperty != null && propertyModel != null && propertyModel.AnswerInverseProperties(leftProperty).Any(prop => prop.Equals(rightProperty));

        /// <summary>
        /// Analyzes "InverseOf(owlProperty, X)" relations of the model to answer the inverse properties of the given owl:Property
        /// </summary>
        public static List<RDFResource> AnswerInverseProperties(this RDFOntologyPropertyModel propertyModel, RDFResource owlProperty)
        {
            List<RDFResource> inverseProperties = new List<RDFResource>();

            if (propertyModel != null && owlProperty != null)
            {
                inverseProperties.AddRange(propertyModel.FindInverseProperties(owlProperty, propertyModel.TBoxVirtualGraph));

                //We don't want to also enlist the given owl:Property
                inverseProperties.RemoveAll(prop => prop.Equals(owlProperty));
            }

            return inverseProperties;
        }

        /// <summary>
        /// Finds "InverseOf(owlProperty, X)" relations to enlist the inverse properties of the given owl:Property
        /// </summary>
        internal static List<RDFResource> FindInverseProperties(this RDFOntologyPropertyModel propertyModel, RDFResource owlProperty, RDFGraph tboxGraph)
        {
            List<RDFResource> inverseProperties = new List<RDFResource>();

            //DIRECT
            foreach (RDFTriple inversePropertyRelation in tboxGraph[owlProperty, RDFVocabulary.OWL.INVERSE_OF, null, null])
                inverseProperties.Add((RDFResource)inversePropertyRelation.Object);

            //INDIRECT (SYMMETRIC)
            foreach (RDFTriple inversePropertyRelation in tboxGraph[null, RDFVocabulary.OWL.INVERSE_OF, owlProperty, null])
                inverseProperties.Add((RDFResource)inversePropertyRelation.Subject);

            return inverseProperties;
        }

        /// <summary>
        ///  Analyzes "propertyChainAxiom(owlProperty,X)" relations of the model to answer the chain axiom properties of the given owl:Property [OWL2]
        /// </summary>
        public static List<RDFResource> AnswerChainAxiomProperties(this RDFOntologyPropertyModel propertyModel, RDFResource owlProperty)
        {
            List<RDFResource> chainAxiomProperties = new List<RDFResource>();

            if (propertyModel != null && owlProperty != null)
            {
                RDFGraph tboxVirtualGraph = propertyModel.TBoxVirtualGraph;

                //Restrict T-BOX knowledge to owl:propertyChainAxiom relations of the given owl:Property (both explicit and inferred)
                RDFResource chainAxiomPropertiesRepresentative = tboxVirtualGraph[owlProperty, RDFVocabulary.OWL.PROPERTY_CHAIN_AXIOM, null, null]
                                                                    .Select(t => t.Object)
                                                                    .OfType<RDFResource>()
                                                                    .FirstOrDefault();
                if (chainAxiomPropertiesRepresentative != null)
                {
                    //Reconstruct collection of chain axiom properties from T-BOX knowledge
                    RDFCollection chainAxiomPropertiesCollection = RDFModelUtilities.DeserializeCollectionFromGraph(tboxVirtualGraph, chainAxiomPropertiesRepresentative, RDFModelEnums.RDFTripleFlavors.SPO);
                    foreach (RDFPatternMember chainAxiomProperty in chainAxiomPropertiesCollection)
                        chainAxiomProperties.Add((RDFResource)chainAxiomProperty);
                }
            }

            return chainAxiomProperties;
        }
        #endregion

        #region Checker
        /// <summary>
        /// Checks if the given owl:Property is a reserved ontology property
        /// </summary>
        internal static bool CheckReservedProperty(this RDFResource owlProperty) =>
            RDFSemanticsUtilities.ReservedProperties.Contains(owlProperty.PatternMemberID);

        /// <summary>
        /// Checks if the given childProperty can be subProperty of the given motherProperty without tampering OWL-DL integrity<br/>
        /// Does not accept property chain definitions (OWL2-DL decidability)
        /// </summary>
        internal static bool CheckSubPropertyCompatibility(this RDFOntologyPropertyModel propertyModel, RDFResource childProperty, RDFResource motherProperty)
            => !propertyModel.CheckAreSubProperties(motherProperty, childProperty)
                  && !propertyModel.CheckAreEquivalentProperties(motherProperty, childProperty)
                    && !propertyModel.CheckAreDisjointProperties(motherProperty, childProperty)
                      //OWL2-DL decidability
                      && !propertyModel.CheckHasPropertyChainAxiom(childProperty)
                        && !propertyModel.CheckHasPropertyChainAxiom(motherProperty);

        /// <summary>
        /// Checks if the given leftProperty can be equivalentProperty of the given rightProperty without tampering OWL-DL integrity<br/>
        /// Does not accept property chain definitions (OWL2-DL decidability)
        /// </summary>
        internal static bool CheckEquivalentPropertyCompatibility(this RDFOntologyPropertyModel propertyModel, RDFResource leftProperty, RDFResource rightProperty)
            => !propertyModel.CheckAreSubProperties(leftProperty, rightProperty)
                  && !propertyModel.CheckAreSuperPropertes(leftProperty, rightProperty)
                    && !propertyModel.CheckAreDisjointProperties(leftProperty, rightProperty)
                      //OWL2-DL decidability
                      && !propertyModel.CheckHasPropertyChainAxiom(leftProperty)
                        && !propertyModel.CheckHasPropertyChainAxiom(rightProperty);

        /// <summary>
        /// Checks if the given leftProperty can be propertyDisjointWith of the given rightProperty without tampering OWL-DL integrity [OWL2]
        /// </summary>
        internal static bool CheckDisjointPropertyCompatibility(this RDFOntologyPropertyModel propertyModel, RDFResource leftProperty, RDFResource rightProperty)
            => !propertyModel.CheckAreSubProperties(leftProperty, rightProperty)
                  && !propertyModel.CheckAreSuperPropertes(leftProperty, rightProperty)
                    && !propertyModel.CheckAreEquivalentProperties(leftProperty, rightProperty);

        /// <summary>
        /// Checks if the given leftProperty can be inverse of the given rightProperty without tampering OWL-DL integrity [OWL2]
        /// </summary>
        internal static bool CheckInversePropertyCompatibility(this RDFOntologyPropertyModel propertyModel, RDFResource leftProperty, RDFResource rightProperty)
            => !propertyModel.CheckAreSubProperties(leftProperty, rightProperty)
                  && !propertyModel.CheckAreSuperPropertes(leftProperty, rightProperty)
                    && !propertyModel.CheckAreEquivalentProperties(leftProperty, rightProperty);
        #endregion
    }
}