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

namespace RDFSharp.Semantics
{
    /// <summary>
    /// RDFOntologyPropertyModelHelper contains methods for analyzing relations describing application domain properties
    /// </summary>
    public static class RDFOntologyPropertyModelHelper
    {
        #region Declarer
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
                && propertyModel.TBoxGraph[owlProperty, RDFVocabulary.OWL.PROPERTY_CHAIN_AXIOM, null, null].Any();

        /// <summary>
        /// Checks for the existence of the given owl:AllDisjointProperties declaration within the model [OWL2]
        /// </summary>
        public static bool CheckHasAllDisjointProperties(this RDFOntologyPropertyModel propertyModel, RDFResource owlProperty)
            => propertyModel.TBoxGraph.ContainsTriple(new RDFTriple(owlProperty, RDFVocabulary.RDF.TYPE, RDFVocabulary.OWL.ALL_DISJOINT_PROPERTIES));
        #endregion

        #region Analyzer
        /// <summary>
        /// Checks for the existence of "SubProperty(childProperty,motherProperty)" relations within the model
        /// </summary>
        public static bool CheckIsSubPropertyOf(this RDFOntologyPropertyModel propertyModel, RDFResource childProperty, RDFResource motherProperty)
            => childProperty != null && motherProperty != null && propertyModel != null && propertyModel.GetSuperPropertiesOf(childProperty).Any(prop => prop.Equals(motherProperty));

        /// <summary>
        /// Analyzes "SubProperty(owlProperty, X)" relations of the model to answer the sub property of the given owl:Property
        /// </summary>
        public static List<RDFResource> GetSubPropertiesOf(this RDFOntologyPropertyModel propertyModel, RDFResource owlProperty)
        {
            List<RDFResource> subProperties = new List<RDFResource>();

            if (propertyModel != null && owlProperty != null)
            {
                //Reason on the given property
                subProperties.AddRange(propertyModel.FindSubPropertiesOf(owlProperty));

                //Reason on the equivalent properties
                foreach (RDFResource equivalentProperty in propertyModel.GetEquivalentPropertiesOf(owlProperty))
                    subProperties.AddRange(propertyModel.FindSubPropertiesOf(equivalentProperty));

                //We don't want to also enlist the given owl:Property
                subProperties.RemoveAll(prop => prop.Equals(owlProperty));
            }

            return subProperties;
        }

        /// <summary>
        /// Finds "SubProperty(owlProperty, X)" relations to enlist the sub properties of the given owl:Property
        /// </summary>
        internal static List<RDFResource> FindSubPropertiesOf(this RDFOntologyPropertyModel propertyModel, RDFResource owlProperty)
        {
            //Direct subsumption of "rdfs:subPropertyOf" taxonomy
            List<RDFResource> subProperties = propertyModel.SubsumeSubPropertyHierarchy(owlProperty);

            //Enlist equivalent properties of subproperties
            foreach (RDFResource subProperty in subProperties.ToList())
                subProperties.AddRange(propertyModel.GetEquivalentPropertiesOf(subProperty)
                                                    .Union(propertyModel.GetSubPropertiesOf(subProperty)));

            return subProperties;
        }

        /// <summary>
        /// Subsume "SubProperty(owlProperty,X)" relations of the model to answer the sub properties of the given owl:Property
        /// </summary>
        internal static List<RDFResource> SubsumeSubPropertyHierarchy(this RDFOntologyPropertyModel propertyModel, RDFResource owlProperty, RDFGraph workingGraph=null)
        {
            List<RDFResource> subProperties = new List<RDFResource>();
            if (workingGraph == null)
                workingGraph = propertyModel.TBoxVirtualGraph;

            // SUBPROPERTY(A,B) ^ SUBPROPERTY(B,C) -> SUBPROPERTY(A,C)
            foreach (RDFTriple subPropertyRelation in workingGraph[null, RDFVocabulary.RDFS.SUB_PROPERTY_OF, owlProperty, null])
            {
                subProperties.Add((RDFResource)subPropertyRelation.Subject);
                subProperties.AddRange(propertyModel.SubsumeSubPropertyHierarchy((RDFResource)subPropertyRelation.Subject, workingGraph));
            }

            return subProperties;
        }

        /// <summary>
        /// Checks for the existence of "SuperProperty(motherProperty,childProperty)" relations within the model
        /// </summary>
        public static bool CheckIsSuperPropertyOf(this RDFOntologyPropertyModel propertyModel, RDFResource motherProperty, RDFResource childProperty)
            => childProperty != null && motherProperty != null && propertyModel != null && propertyModel.GetSuperPropertiesOf(childProperty).Any(prop => prop.Equals(motherProperty));

        /// <summary>
        /// Analyzes "SuperProperty(owlProperty, X)" relations of the model to answer the super properties of the given owl:Property
        /// </summary>
        public static List<RDFResource> GetSuperPropertiesOf(this RDFOntologyPropertyModel propertyModel, RDFResource owlProperty)
        {
            List<RDFResource> subProperties = new List<RDFResource>();

            if (propertyModel != null && owlProperty != null)
            {
                //Reason on the given property
                subProperties.AddRange(propertyModel.FindSuperPropertiesOf(owlProperty));

                //Reason on the equivalent properties
                foreach (RDFResource equivalentProperty in propertyModel.GetEquivalentPropertiesOf(owlProperty))
                    subProperties.AddRange(propertyModel.FindSuperPropertiesOf(equivalentProperty));

                //We don't want to also enlist the given owl:Property
                subProperties.RemoveAll(prop => prop.Equals(owlProperty));
            }

            return subProperties;
        }

        /// <summary>
        /// Finds "SuperProperty(owlProperty, X)" relations to enlist the super properties of the given owl:Property
        /// </summary>
        internal static List<RDFResource> FindSuperPropertiesOf(this RDFOntologyPropertyModel propertyModel, RDFResource owlProperty)
        {
            //Direct subsumption of "rdfs:subPropertyOf" taxonomy
            List<RDFResource> superProperties = propertyModel.SubsumeSuperPropertyHierarchy(owlProperty);

            //Enlist equivalent classes of superclasses
            foreach (RDFResource superProperty in superProperties.ToList())
                superProperties.AddRange(propertyModel.GetEquivalentPropertiesOf(superProperty)
                                                      .Union(propertyModel.GetSuperPropertiesOf(superProperty)));

            return superProperties;
        }

        /// <summary>
        /// Subsumes "SubProperty(X,owlProperty)" relations of the model to answer the super properties of the given owl:Property
        /// </summary>
        internal static List<RDFResource> SubsumeSuperPropertyHierarchy(this RDFOntologyPropertyModel propertyModel, RDFResource owlProperty, RDFGraph workingGraph=null)
        {
            List<RDFResource> superProperties = new List<RDFResource>();
            if (workingGraph == null)
                workingGraph = propertyModel.TBoxVirtualGraph;

            // SUBPROPERTY(A,B) ^ SUBPROPERTY(B,C) -> SUBPROPERTY(A,C)
            foreach (RDFTriple subPropertyRelation in workingGraph[owlProperty, RDFVocabulary.RDFS.SUB_PROPERTY_OF, null, null])
            {
                superProperties.Add((RDFResource)subPropertyRelation.Object);
                superProperties.AddRange(propertyModel.SubsumeSuperPropertyHierarchy((RDFResource)subPropertyRelation.Object, workingGraph));
            }

            return superProperties;
        }

        /// <summary>
        /// Checks for the existence of "EquivalentProperty(leftProperty,rightProperty)" relations within the model
        /// </summary>
        public static bool CheckIsEquivalentPropertyOf(this RDFOntologyPropertyModel propertyModel, RDFResource leftProperty, RDFResource rightProperty)
            => leftProperty != null && rightProperty != null && propertyModel != null && propertyModel.GetEquivalentPropertiesOf(leftProperty).Any(prop => prop.Equals(rightProperty));

        /// <summary>
        /// Analyzes "EquivalentProperty(owlProperty, X)" relations of the model to answer the equivalent properties of the given owl:Property
        /// </summary>
        public static List<RDFResource> GetEquivalentPropertiesOf(this RDFOntologyPropertyModel propertyModel, RDFResource owlProperty)
        {
            List<RDFResource> equivalentProperties = new List<RDFResource>();

            if (propertyModel != null && owlProperty != null)
            {
                equivalentProperties.AddRange(propertyModel.FindEquivalentPropertiesOf(owlProperty, new Dictionary<long, RDFResource>()));

                //We don't want to also enlist the given owl:Property
                equivalentProperties.RemoveAll(prop => prop.Equals(owlProperty));
            }

            return RDFQueryUtilities.RemoveDuplicates(equivalentProperties);
        }

        /// <summary>
        /// Finds "EquivalentProperty(owlProperty, X)" relations to enlist the equivalent properties of the given owl:Property
        /// </summary>
        internal static List<RDFResource> FindEquivalentPropertiesOf(this RDFOntologyPropertyModel propertyModel, RDFResource owlProperty, Dictionary<long, RDFResource> visitContext, RDFGraph workingGraph=null)
        {
            List<RDFResource> equivalentProperties = new List<RDFResource>();
            if (workingGraph == null)
                workingGraph = propertyModel.TBoxVirtualGraph;

            #region VisitContext
            if (!visitContext.ContainsKey(owlProperty.PatternMemberID))
                visitContext.Add(owlProperty.PatternMemberID, owlProperty);
            else
                return equivalentProperties;
            #endregion

            //DIRECT
            foreach (RDFTriple equivalentPropertyRelation in workingGraph[owlProperty, RDFVocabulary.OWL.EQUIVALENT_PROPERTY, null, null])
                equivalentProperties.Add((RDFResource)equivalentPropertyRelation.Object);

            //INDIRECT (TRANSITIVE)
            foreach (RDFResource equivalentProperty in equivalentProperties.ToList())
                equivalentProperties.AddRange(propertyModel.FindEquivalentPropertiesOf(equivalentProperty, visitContext, workingGraph));

            return equivalentProperties;
        }

        /// <summary>
        /// Checks for the existence of "PropertyDisjointWith(leftProperty,rightProperty)" relations within the model [OWL2]
        /// </summary>
        public static bool CheckIsDisjointPropertyWith(this RDFOntologyPropertyModel propertyModel, RDFResource leftProperty, RDFResource rightProperty)
            => leftProperty != null && rightProperty != null && propertyModel != null && propertyModel.GetDisjointPropertiesWith(leftProperty).Any(prop => prop.Equals(rightProperty));

        /// <summary>
        /// Analyzes "PropertyDisjointWith(leftProperty,rightProperty)" relations of the model to answer the disjoint properties of the given owl:Property [OWL2]
        /// </summary>
        public static List<RDFResource> GetDisjointPropertiesWith(this RDFOntologyPropertyModel propertyModel, RDFResource owlProperty)
        {
            List<RDFResource> disjointProperties = new List<RDFResource>();

            if (propertyModel != null && owlProperty != null)
            {
                disjointProperties.AddRange(propertyModel.FindDisjointPropertiesWith(owlProperty, new Dictionary<long, RDFResource>()));

                //We don't want to also enlist the given owl:Property
                disjointProperties.RemoveAll(prop => prop.Equals(owlProperty));
            }

            return RDFQueryUtilities.RemoveDuplicates(disjointProperties);
        }

        /// <summary>
        /// Finds "PropertyDisjointWith(owlProperty, X)" relations to enlist the disjoint properties of the given owl:Property [OWL2]
        /// </summary>
        internal static List<RDFResource> FindDisjointPropertiesWith(this RDFOntologyPropertyModel propertyModel, RDFResource owlProperty, Dictionary<long, RDFResource> visitContext, RDFGraph workingGraph=null)
        {
            List<RDFResource> disjointProperties = new List<RDFResource>();
            if (workingGraph == null)
                workingGraph = propertyModel.TBoxVirtualGraph;

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
                foreach (RDFTriple allDisjointMembers in workingGraph[allDisjoint.Current, RDFVocabulary.OWL.MEMBERS, null, null])
                {
                    RDFCollection allDisjointCollection = RDFModelUtilities.DeserializeCollectionFromGraph(workingGraph, (RDFResource)allDisjointMembers.Object, RDFModelEnums.RDFTripleFlavors.SPO);
                    if (allDisjointCollection.Items.Any(item => item.Equals(owlProperty)))
                        allDisjointProperties.AddRange(allDisjointCollection.OfType<RDFResource>());
                }
            allDisjointProperties.RemoveAll(adm => adm.Equals(owlProperty));

            // Find disjoint properties linked to the given one with owl:propertyDisjointWith relation [OWL2]
            List<RDFResource> disjointFromProperties = workingGraph[owlProperty, RDFVocabulary.OWL.PROPERTY_DISJOINT_WITH, null, null]
                                                         .Select(t => (RDFResource)t.Object)
                                                         .ToList();

            // Merge properties from both sets into a unique deduplicate working set
            List<RDFResource> disjointPropertiesSet = RDFQueryUtilities.RemoveDuplicates(allDisjointProperties.Union(disjointFromProperties).ToList());
            #endregion

            #region Analyze
            // Inference: PROPERTYDISJOINTWITH(A,B) ^ EQUIVALENTPROPERTY(B,C) -> PROPERTYDISJOINTWITH(A,C)
            foreach (RDFResource disjointProperty in disjointPropertiesSet)
            {
                disjointProperties.Add(disjointProperty);
                disjointProperties.AddRange(propertyModel.FindEquivalentPropertiesOf(disjointProperty, visitContext, workingGraph));
            }

            // Inference: PROPERTYDISJOINTWITH(A,B) ^ SUBPROPERTY(C,B) -> PROPERTYDISJOINTWITH(A,C)
            foreach (RDFResource disjointProperty in disjointProperties.ToList())
                disjointProperties.AddRange(propertyModel.FindSubPropertiesOf(disjointProperty));

            // Inference: EQUIVALENTPROPERTY(A,B) ^ PROPERTYDISJOINTWITH(B,C) -> PROPERTYDISJOINTWITH(A,C)
            foreach (RDFResource compatibleClass in propertyModel.GetSuperPropertiesOf(owlProperty)
                                                                 .Union(propertyModel.GetEquivalentPropertiesOf(owlProperty)))
                disjointProperties.AddRange(propertyModel.FindDisjointPropertiesWith(compatibleClass, visitContext, workingGraph));
            #endregion

            return disjointProperties;
        }

        /// <summary>
        /// Checks for the existence of "InverseOf(leftProperty,rightProperty)" relations within the model
        /// </summary>
        public static bool CheckIsInversePropertyOf(this RDFOntologyPropertyModel propertyModel, RDFResource leftProperty, RDFResource rightProperty)
            => leftProperty != null && rightProperty != null && propertyModel != null && propertyModel.GetInversePropertiesOf(leftProperty).Any(prop => prop.Equals(rightProperty));

        /// <summary>
        /// Analyzes "InverseOf(owlProperty, X)" relations of the model to answer the inverse properties of the given owl:Property
        /// </summary>
        public static List<RDFResource> GetInversePropertiesOf(this RDFOntologyPropertyModel propertyModel, RDFResource owlProperty)
        {
            List<RDFResource> inverseProperties = new List<RDFResource>();

            if (propertyModel != null && owlProperty != null)
            {
                inverseProperties.AddRange(propertyModel.FindInversePropertiesOf(owlProperty));

                //We don't want to also enlist the given owl:Property
                inverseProperties.RemoveAll(prop => prop.Equals(owlProperty));
            }

            return RDFQueryUtilities.RemoveDuplicates(inverseProperties);
        }

        /// <summary>
        /// Finds "InverseOf(owlProperty, X)" relations to enlist the inverse properties of the given owl:Property
        /// </summary>
        internal static List<RDFResource> FindInversePropertiesOf(this RDFOntologyPropertyModel propertyModel, RDFResource owlProperty, RDFGraph workingGraph=null)
        {
            List<RDFResource> inverseProperties = new List<RDFResource>();
            if (workingGraph == null)
                workingGraph = propertyModel.TBoxVirtualGraph;

            //DIRECT
            foreach (RDFTriple inversePropertyRelation in workingGraph[owlProperty, RDFVocabulary.OWL.INVERSE_OF, null, null])
                inverseProperties.Add((RDFResource)inversePropertyRelation.Object);

            //INDIRECT (SYMMETRIC)
            foreach (RDFTriple inversePropertyRelation in workingGraph[null, RDFVocabulary.OWL.INVERSE_OF, owlProperty, null])
                inverseProperties.Add((RDFResource)inversePropertyRelation.Subject);

            return inverseProperties;
        }

        /// <summary>
        ///  Analyzes "propertyChainAxiom(owlProperty,X)" relations of the model to answer the chain axiom properties of the given owl:Property [OWL2]
        /// </summary>
        public static List<RDFResource> GetChainAxiomPropertiesOf(this RDFOntologyPropertyModel propertyModel, RDFResource owlProperty)
        {
            List<RDFResource> chainAxiomProperties = new List<RDFResource>();

            if (propertyModel != null && owlProperty != null)
            {
                RDFGraph workingGraph = propertyModel.TBoxVirtualGraph;

                //Restrict T-BOX knowledge to owl:propertyChainAxiom relations of the given owl:Property (both explicit and inferred)
                RDFResource chainAxiomPropertiesRepresentative = workingGraph[owlProperty, RDFVocabulary.OWL.PROPERTY_CHAIN_AXIOM, null, null]
                                                                    .Select(t => t.Object)
                                                                    .OfType<RDFResource>()
                                                                    .FirstOrDefault();
                if (chainAxiomPropertiesRepresentative != null)
                {
                    //Reconstruct collection of chain axiom properties from T-BOX knowledge
                    RDFCollection chainAxiomPropertiesCollection = RDFModelUtilities.DeserializeCollectionFromGraph(workingGraph, chainAxiomPropertiesRepresentative, RDFModelEnums.RDFTripleFlavors.SPO);
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
            => !propertyModel.CheckIsSubPropertyOf(motherProperty, childProperty)
                  && !propertyModel.CheckIsEquivalentPropertyOf(motherProperty, childProperty)
                    && !propertyModel.CheckIsDisjointPropertyWith(motherProperty, childProperty)
                      //OWL2-DL decidability
                      && !propertyModel.CheckHasPropertyChainAxiom(childProperty)
                        && !propertyModel.CheckHasPropertyChainAxiom(motherProperty);

        /// <summary>
        /// Checks if the given leftProperty can be equivalentProperty of the given rightProperty without tampering OWL-DL integrity<br/>
        /// Does not accept property chain definitions (OWL2-DL decidability)
        /// </summary>
        internal static bool CheckEquivalentPropertyCompatibility(this RDFOntologyPropertyModel propertyModel, RDFResource leftProperty, RDFResource rightProperty)
            => !propertyModel.CheckIsSubPropertyOf(leftProperty, rightProperty)
                  && !propertyModel.CheckIsSuperPropertyOf(leftProperty, rightProperty)
                    && !propertyModel.CheckIsDisjointPropertyWith(leftProperty, rightProperty)
                      //OWL2-DL decidability
                      && !propertyModel.CheckHasPropertyChainAxiom(leftProperty)
                        && !propertyModel.CheckHasPropertyChainAxiom(rightProperty);

        /// <summary>
        /// Checks if the given leftProperty can be propertyDisjointWith of the given rightProperty without tampering OWL-DL integrity [OWL2]
        /// </summary>
        internal static bool CheckDisjointPropertyCompatibility(this RDFOntologyPropertyModel propertyModel, RDFResource leftProperty, RDFResource rightProperty)
            => !propertyModel.CheckIsSubPropertyOf(leftProperty, rightProperty)
                  && !propertyModel.CheckIsSuperPropertyOf(leftProperty, rightProperty)
                    && !propertyModel.CheckIsEquivalentPropertyOf(leftProperty, rightProperty);

        /// <summary>
        /// Checks if the given leftProperty can be inverse of the given rightProperty without tampering OWL-DL integrity
        /// </summary>
        internal static bool CheckInversePropertyCompatibility(this RDFOntologyPropertyModel propertyModel, RDFResource leftProperty, RDFResource rightProperty)
            => !propertyModel.CheckIsSubPropertyOf(leftProperty, rightProperty)
                  && !propertyModel.CheckIsSuperPropertyOf(leftProperty, rightProperty)
                    && !propertyModel.CheckIsEquivalentPropertyOf(leftProperty, rightProperty);
        #endregion
    }
}