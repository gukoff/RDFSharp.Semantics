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
    /// RDFOntologyClassModelHelper contains methods for analyzing relations describing application domain entities
    /// </summary>
    public static class RDFOntologyClassModelHelper
    {
        #region Declarer
        /// <summary>
        /// Checks for the existence of the given owl:Class declaration within the model
        /// </summary>
        public static bool CheckHasClass(this RDFOntologyClassModel classModel, RDFResource owlClass)
            => owlClass != null && classModel != null && classModel.Classes.ContainsKey(owlClass.PatternMemberID);

        /// <summary>
        /// Checks for the existence of the given owl:DeprecatedClass declaration within the model
        /// </summary>
        public static bool CheckHasDeprecatedClass(this RDFOntologyClassModel classModel, RDFResource owlClass)
            => CheckHasClass(classModel, owlClass)
                && classModel.TBoxGraph.ContainsTriple(new RDFTriple(owlClass, RDFVocabulary.RDF.TYPE, RDFVocabulary.OWL.DEPRECATED_CLASS));

        /// <summary>
        /// Checks for the existence of the given owl:Restriction declaration within the model
        /// </summary>
        public static bool CheckHasRestrictionClass(this RDFOntologyClassModel classModel, RDFResource owlRestriction)
            => CheckHasClass(classModel, owlRestriction)
                && classModel.TBoxGraph.ContainsTriple(new RDFTriple(owlRestriction, RDFVocabulary.RDF.TYPE, RDFVocabulary.OWL.RESTRICTION));

        /// <summary>
        /// Checks for the existence of the given owl:oneOf declaration within the model
        /// </summary>
        public static bool CheckHasEnumerateClass(this RDFOntologyClassModel classModel, RDFResource owlEnumerate)
            => CheckHasClass(classModel, owlEnumerate)
                && classModel.TBoxGraph.Any(t => t.Subject.Equals(owlEnumerate) && t.Predicate.Equals(RDFVocabulary.OWL.ONE_OF));

        /// <summary>
        /// Checks for the existence of the given owl:[unionOf|intersectionOf|complementOf] declaration within the model
        /// </summary>
        public static bool CheckHasCompositeClass(this RDFOntologyClassModel classModel, RDFResource owlComposite)
            => CheckHasClass(classModel, owlComposite)
                && classModel.TBoxGraph.Any(t => t.Subject.Equals(owlComposite) &&
                                                    (t.Predicate.Equals(RDFVocabulary.OWL.UNION_OF) ||
                                                     t.Predicate.Equals(RDFVocabulary.OWL.INTERSECTION_OF) ||
                                                     t.Predicate.Equals(RDFVocabulary.OWL.COMPLEMENT_OF)));

        /// <summary>
        /// Checks for the existence of the given owl:unionOf declaration within the model
        /// </summary>
        internal static bool CheckHasCompositeUnionClass(this RDFOntologyClassModel classModel, RDFResource owlComposite)
            => CheckHasCompositeClass(classModel, owlComposite)
                && classModel.TBoxGraph.Any(t => t.Subject.Equals(owlComposite) && t.Predicate.Equals(RDFVocabulary.OWL.UNION_OF));

        /// <summary>
        /// Checks for the existence of the given owl:intersectionOf declaration within the model
        /// </summary>
        internal static bool CheckHasCompositeIntersectionClass(this RDFOntologyClassModel classModel, RDFResource owlComposite)
            => CheckHasCompositeClass(classModel, owlComposite)
                && classModel.TBoxGraph.Any(t => t.Subject.Equals(owlComposite) && t.Predicate.Equals(RDFVocabulary.OWL.INTERSECTION_OF));

        /// <summary>
        /// Checks for the existence of the given owl:complementOf declaration within the model
        /// </summary>
        internal static bool CheckHasCompositeComplementClass(this RDFOntologyClassModel classModel, RDFResource owlComposite)
            => CheckHasCompositeClass(classModel, owlComposite)
                && classModel.TBoxGraph.Any(t => t.Subject.Equals(owlComposite) && t.Predicate.Equals(RDFVocabulary.OWL.COMPLEMENT_OF));

        /// <summary>
        /// Checks for the existence of the given owl:allValuesFromRestriction declaration within the model
        /// </summary>
        internal static bool CheckHasAllValuesFromRestrictionClass(this RDFOntologyClassModel classModel, RDFResource owlRestriction)
            => CheckHasRestrictionClass(classModel, owlRestriction)
                && classModel.TBoxGraph.Any(t => t.Subject.Equals(owlRestriction) && t.Predicate.Equals(RDFVocabulary.OWL.ALL_VALUES_FROM));

        /// <summary>
        /// Checks for the existence of the given owl:someValuesFromRestriction declaration within the model
        /// </summary>
        internal static bool CheckHasSomeValuesFromRestrictionClass(this RDFOntologyClassModel classModel, RDFResource owlRestriction)
            => CheckHasRestrictionClass(classModel, owlRestriction)
                && classModel.TBoxGraph.Any(t => t.Subject.Equals(owlRestriction) && t.Predicate.Equals(RDFVocabulary.OWL.SOME_VALUES_FROM));

        /// <summary>
        /// Checks for the existence of the given owl:hasValueRestriction declaration within the model
        /// </summary>
        internal static bool CheckHasValueRestrictionClass(this RDFOntologyClassModel classModel, RDFResource owlRestriction)
            => CheckHasRestrictionClass(classModel, owlRestriction)
                && classModel.TBoxGraph.Any(t => t.Subject.Equals(owlRestriction) && t.Predicate.Equals(RDFVocabulary.OWL.HAS_VALUE));

        /// <summary>
        /// Checks for the existence of the given owl:hasSelfRestriction declaration within the model [OWL2]
        /// </summary>
        internal static bool CheckHasSelfRestrictionClass(this RDFOntologyClassModel classModel, RDFResource owlRestriction)
            => CheckHasRestrictionClass(classModel, owlRestriction)
                && classModel.TBoxGraph.Any(t => t.Subject.Equals(owlRestriction) && t.Predicate.Equals(RDFVocabulary.OWL.HAS_SELF));

        /// <summary>
        /// Checks for the existence of the given owl:CardinalityRestriction declaration within the model
        /// </summary>
        internal static bool CheckHasCardinalityRestrictionClass(this RDFOntologyClassModel classModel, RDFResource owlRestriction)
            => CheckHasRestrictionClass(classModel, owlRestriction)
                && classModel.TBoxGraph.Any(t => t.Subject.Equals(owlRestriction) && t.Predicate.Equals(RDFVocabulary.OWL.CARDINALITY));

        /// <summary>
        /// Checks for the existence of the given owl:[Min]CardinalityRestriction declaration within the model
        /// </summary>
        internal static bool CheckHasMinCardinalityRestrictionClass(this RDFOntologyClassModel classModel, RDFResource owlRestriction)
            => CheckHasRestrictionClass(classModel, owlRestriction)
                && classModel.TBoxGraph.Any(t => t.Subject.Equals(owlRestriction) && t.Predicate.Equals(RDFVocabulary.OWL.MIN_CARDINALITY))
                && !classModel.TBoxGraph.Any(t => t.Subject.Equals(owlRestriction) && t.Predicate.Equals(RDFVocabulary.OWL.MAX_CARDINALITY));

        /// <summary>
        /// Checks for the existence of the given owl:[Max]CardinalityRestriction declaration within the model
        /// </summary>
        internal static bool CheckHasMaxCardinalityRestrictionClass(this RDFOntologyClassModel classModel, RDFResource owlRestriction)
            => CheckHasRestrictionClass(classModel, owlRestriction)
                && classModel.TBoxGraph.Any(t => t.Subject.Equals(owlRestriction) && t.Predicate.Equals(RDFVocabulary.OWL.MAX_CARDINALITY))
                && !classModel.TBoxGraph.Any(t => t.Subject.Equals(owlRestriction) && t.Predicate.Equals(RDFVocabulary.OWL.MIN_CARDINALITY));

        /// <summary>
        /// Checks for the existence of the given owl:[Min|Max]CardinalityRestriction declaration within the model
        /// </summary>
        internal static bool CheckHasMinMaxCardinalityRestrictionClass(this RDFOntologyClassModel classModel, RDFResource owlRestriction)
            => CheckHasRestrictionClass(classModel, owlRestriction)
                && classModel.TBoxGraph.Any(t => t.Subject.Equals(owlRestriction) && t.Predicate.Equals(RDFVocabulary.OWL.MIN_CARDINALITY))
                && classModel.TBoxGraph.Any(t => t.Subject.Equals(owlRestriction) && t.Predicate.Equals(RDFVocabulary.OWL.MAX_CARDINALITY));

        /// <summary>
        /// Checks for the existence of the given owl:QualifiedCardinalityRestriction declaration within the model [OWL2]
        /// </summary>
        internal static bool CheckHasQualifiedCardinalityRestrictionClass(this RDFOntologyClassModel classModel, RDFResource owlRestriction)
            => CheckHasRestrictionClass(classModel, owlRestriction)
                && classModel.TBoxGraph.Any(t => t.Subject.Equals(owlRestriction) && t.Predicate.Equals(RDFVocabulary.OWL.QUALIFIED_CARDINALITY));

        /// <summary>
        /// Checks for the existence of the given owl:[Min]QualifiedCardinalityRestriction declaration within the model [OWL2]
        /// </summary>
        internal static bool CheckHasMinQualifiedCardinalityRestrictionClass(this RDFOntologyClassModel classModel, RDFResource owlRestriction)
            => CheckHasRestrictionClass(classModel, owlRestriction)
                && classModel.TBoxGraph.Any(t => t.Subject.Equals(owlRestriction) && t.Predicate.Equals(RDFVocabulary.OWL.MIN_QUALIFIED_CARDINALITY))
                && !classModel.TBoxGraph.Any(t => t.Subject.Equals(owlRestriction) && t.Predicate.Equals(RDFVocabulary.OWL.MAX_QUALIFIED_CARDINALITY));

        /// <summary>
        /// Checks for the existence of the given owl:[Max]QualifiedCardinalityRestriction declaration within the model [OWL2]
        /// </summary>
        internal static bool CheckHasMaxQualifiedCardinalityRestrictionClass(this RDFOntologyClassModel classModel, RDFResource owlRestriction)
            => CheckHasRestrictionClass(classModel, owlRestriction)
                && classModel.TBoxGraph.Any(t => t.Subject.Equals(owlRestriction) && t.Predicate.Equals(RDFVocabulary.OWL.MAX_QUALIFIED_CARDINALITY))
                && !classModel.TBoxGraph.Any(t => t.Subject.Equals(owlRestriction) && t.Predicate.Equals(RDFVocabulary.OWL.MIN_QUALIFIED_CARDINALITY));

        /// <summary>
        /// Checks for the existence of the given owl:[Min|Max]QualifiedCardinalityRestriction declaration within the model [OWL2]
        /// </summary>
        internal static bool CheckHasMinMaxQualifiedCardinalityRestrictionClass(this RDFOntologyClassModel classModel, RDFResource owlRestriction)
            => CheckHasRestrictionClass(classModel, owlRestriction)
                && classModel.TBoxGraph.Any(t => t.Subject.Equals(owlRestriction) && t.Predicate.Equals(RDFVocabulary.OWL.MIN_QUALIFIED_CARDINALITY))
                && classModel.TBoxGraph.Any(t => t.Subject.Equals(owlRestriction) && t.Predicate.Equals(RDFVocabulary.OWL.MAX_QUALIFIED_CARDINALITY));

        /// <summary>
        /// Checks for the existence of the given owl:disjointUnionOf declaration within the model [OWL2]
        /// </summary>
        public static bool CheckHasDisjointUnionClass(this RDFOntologyClassModel classModel, RDFResource owlClass)
            => CheckHasClass(classModel, owlClass)
                && classModel.TBoxGraph.Any(t => t.Subject.Equals(owlClass) && t.Predicate.Equals(RDFVocabulary.OWL.DISJOINT_UNION_OF));

        /// <summary>
        /// Checks for the existence of the given owl:AllDisjointClasses declaration within the model [OWL2]
        /// </summary>
        public static bool CheckHasAllDisjointClasses(this RDFOntologyClassModel classModel, RDFResource owlClass)
            => CheckHasClass(classModel, owlClass)
                && classModel.TBoxGraph.ContainsTriple(new RDFTriple(owlClass, RDFVocabulary.RDF.TYPE, RDFVocabulary.OWL.ALL_DISJOINT_CLASSES));

        /// <summary>
        /// Checks for the existence of the given owl:Class declaration within the model
        /// </summary>
        public static bool CheckHasSimpleClass(this RDFOntologyClassModel classModel, RDFResource owlClass)
            => CheckHasClass(classModel, owlClass)
                && !CheckHasRestrictionClass(classModel, owlClass)
                 && !CheckHasEnumerateClass(classModel, owlClass)
                  && !CheckHasCompositeClass(classModel, owlClass)
                   && !CheckHasDisjointUnionClass(classModel, owlClass)
                    && !CheckHasAllDisjointClasses(classModel, owlClass);

        /// <summary>
        /// Checks for the existence of the given owl:Class annotation within the model
        /// </summary>
        public static bool CheckHasAnnotation(this RDFOntologyClassModel classModel, RDFResource owlClass, RDFResource annotationProperty, RDFResource annotationValue)
            => owlClass != null && annotationProperty != null && annotationValue != null && classModel != null && classModel.TBoxGraph.ContainsTriple(new RDFTriple(owlClass, annotationProperty, annotationValue));

        /// <summary>
        /// Checks for the existence of the given owl:Class annotation within the model
        /// </summary>
        public static bool CheckHasAnnotation(this RDFOntologyClassModel classModel, RDFResource owlClass, RDFResource annotationProperty, RDFLiteral annotationValue)
            => owlClass != null && annotationProperty != null && annotationValue != null && classModel != null && classModel.TBoxGraph.ContainsTriple(new RDFTriple(owlClass, annotationProperty, annotationValue));
        #endregion

        #region Analyzer
        /// <summary>
        /// Checks for the existence of "SubClass(childClass,motherClass)" relations within the model
        /// </summary>
        public static bool CheckIsSubClassOf(this RDFOntologyClassModel classModel, RDFResource childClass, RDFResource motherClass)
            => childClass != null && motherClass != null && classModel != null && classModel.GetSuperClassesOf(childClass).Any(cls => cls.Equals(motherClass));

        /// <summary>
        /// Analyzes "SubClass(owlClass,X)" relations of the model to answer the sub classes of the given owl:Class
        /// </summary>
        public static List<RDFResource> GetSubClassesOf(this RDFOntologyClassModel classModel, RDFResource owlClass)
        {
            List<RDFResource> subClasses = new List<RDFResource>();

            if (classModel != null && owlClass != null)
            {
                //Reason on the given class
                subClasses.AddRange(classModel.FindSubClassesOf(owlClass));

                //Reason on the equivalent classes
                foreach (RDFResource equivalentClass in classModel.GetEquivalentClassesOf(owlClass))
                    subClasses.AddRange(classModel.FindSubClassesOf(equivalentClass));

                //We don't want to also enlist the given owl:Class
                subClasses.RemoveAll(cls => cls.Equals(owlClass));
            }

            return subClasses;
        }

        /// <summary>
        /// Finds "SubClass(owlClass,X)" relations of the model to answer the sub classes of the given owl:Class
        /// </summary>
        internal static List<RDFResource> FindSubClassesOf(this RDFOntologyClassModel classModel, RDFResource owlClass)
        {
            //Direct subsumption of "rdfs:subClassOf" taxonomy
            List<RDFResource> subClasses = classModel.SubsumeSubClassHierarchy(owlClass);

            //Enlist equivalent classes of subclasses
            foreach (RDFResource subClass in subClasses.ToList())
                subClasses.AddRange(classModel.GetEquivalentClassesOf(subClass)
                                              .Union(classModel.GetSubClassesOf(subClass)));

            return subClasses;
        }

        /// <summary>
        /// Subsume "SubClass(owlClass,X)" relations of the model to answer the sub classes of the given owl:Class
        /// </summary>
        internal static List<RDFResource> SubsumeSubClassHierarchy(this RDFOntologyClassModel classModel, RDFResource owlClass, RDFGraph workingGraph=null)
        {
            List<RDFResource> subClasses = new List<RDFResource>();
            if (workingGraph == null)
                workingGraph = classModel.TBoxVirtualGraph;

            // SUBCLASS(A,B) ^ SUBCLASS(B,C) -> SUBCLASS(A,C)
            foreach (RDFTriple subClassRelation in workingGraph[null, RDFVocabulary.RDFS.SUB_CLASS_OF, owlClass, null])
            {
                subClasses.Add((RDFResource)subClassRelation.Subject);
                subClasses.AddRange(classModel.SubsumeSubClassHierarchy((RDFResource)subClassRelation.Subject, workingGraph));
            }

            return subClasses;
        }

        /// <summary>
        /// Checks for the existence of "SubClass(motherClass,childClass)" relations within the model
        /// </summary>
        public static bool CheckIsSuperClassOf(this RDFOntologyClassModel classModel, RDFResource motherClass, RDFResource childClass)
            => childClass != null && motherClass != null && classModel != null && classModel.GetSubClassesOf(motherClass).Any(cls => cls.Equals(childClass));

        /// <summary>
        /// Analyzes "SubClass(X,owlClass)" relations of the model to answer the super classes of the given owl:Class
        /// </summary>
        public static List<RDFResource> GetSuperClassesOf(this RDFOntologyClassModel classModel, RDFResource owlClass)
        {
            List<RDFResource> subClasses = new List<RDFResource>();

            if (classModel != null && owlClass != null)
            {
                //Reason on the given class
                subClasses.AddRange(classModel.FindSuperClassesOf(owlClass));

                //Reason on the equivalent classes
                foreach (RDFResource equivalentClass in classModel.GetEquivalentClassesOf(owlClass))
                    subClasses.AddRange(classModel.FindSuperClassesOf(equivalentClass));

                //We don't want to also enlist the given owl:Class
                subClasses.RemoveAll(cls => cls.Equals(owlClass));
            }

            return subClasses;
        }

        /// <summary>
        /// Finds "SubClass(X,owlClass)" relations of the model to answer the super classes of the given owl:Class
        /// </summary>
        internal static List<RDFResource> FindSuperClassesOf(this RDFOntologyClassModel classModel, RDFResource owlClass)
        {
            //Direct subsumption of "rdfs:subClassOf" taxonomy
            List<RDFResource> superClasses = classModel.SubsumeSuperClassHierarchy(owlClass);

            //Enlist equivalent classes of superclasses
            foreach (RDFResource superClass in superClasses.ToList())
                superClasses.AddRange(classModel.GetEquivalentClassesOf(superClass)
                                                .Union(classModel.GetSuperClassesOf(superClass)));

            return superClasses;
        }

        /// <summary>
        /// Subsumes "SubClass(X,owlClass)" relations of the model to answer the super classes of the given owl:Class
        /// </summary>
        internal static List<RDFResource> SubsumeSuperClassHierarchy(this RDFOntologyClassModel classModel, RDFResource owlClass, RDFGraph workingGraph=null)
        {
            List<RDFResource> superClasses = new List<RDFResource>();
            if (workingGraph == null)
                workingGraph = classModel.TBoxVirtualGraph;

            // SUBCLASS(A,B) ^ SUBCLASS(B,C) -> SUBCLASS(A,C)
            foreach (RDFTriple subClassRelation in workingGraph[owlClass, RDFVocabulary.RDFS.SUB_CLASS_OF, null, null])
            {
                superClasses.Add((RDFResource)subClassRelation.Object);
                superClasses.AddRange(classModel.SubsumeSuperClassHierarchy((RDFResource)subClassRelation.Object, workingGraph));
            }

            return superClasses;
        }

        /// <summary>
        /// Checks for the existence of "EquivalentClass(leftClass,rightClass)" relations within the model
        /// </summary>
        public static bool CheckIsEquivalentClassOf(this RDFOntologyClassModel classModel, RDFResource leftClass, RDFResource rightClass)
            => leftClass != null && rightClass != null && classModel != null && classModel.GetEquivalentClassesOf(leftClass).Any(cls => cls.Equals(rightClass));

        /// <summary>
        /// Analyzes "EquivalentClass(owlClass, X)" relations of the model to answer the equivalent classes of the given owl:Class
        /// </summary>
        public static List<RDFResource> GetEquivalentClassesOf(this RDFOntologyClassModel classModel, RDFResource owlClass)
        {
            List<RDFResource> equivalentClasses = new List<RDFResource>();

            if (classModel != null && owlClass != null)
            {
                equivalentClasses.AddRange(classModel.FindEquivalentClassesOf(owlClass, new Dictionary<long, RDFResource>()));

                //We don't want to also enlist the given owl:Class
                equivalentClasses.RemoveAll(cls => cls.Equals(owlClass));
            }

            return RDFQueryUtilities.RemoveDuplicates(equivalentClasses);
        }

        /// <summary>
        /// Finds "EquivalentClass(owlClass, X)" relations to enlist the equivalent classes of the given owl:Class
        /// </summary>
        internal static List<RDFResource> FindEquivalentClassesOf(this RDFOntologyClassModel classModel, RDFResource owlClass, Dictionary<long, RDFResource> visitContext, RDFGraph workingGraph=null)
        {
            List<RDFResource> equivalentClasses = new List<RDFResource>();
            if (workingGraph == null)
                workingGraph = classModel.TBoxVirtualGraph;

            #region VisitContext
            if (!visitContext.ContainsKey(owlClass.PatternMemberID))
                visitContext.Add(owlClass.PatternMemberID, owlClass);
            else
                return equivalentClasses;
            #endregion

            //DIRECT
            foreach (RDFTriple equivalentClassRelation in workingGraph[owlClass, RDFVocabulary.OWL.EQUIVALENT_CLASS, null, null])
                equivalentClasses.Add((RDFResource)equivalentClassRelation.Object);

            //INDIRECT (TRANSITIVE)
            foreach (RDFResource equivalentClass in equivalentClasses.ToList())
                equivalentClasses.AddRange(classModel.FindEquivalentClassesOf(equivalentClass, visitContext, workingGraph));

            return equivalentClasses;
        }

        /// <summary>
        /// Checks for the existence of "DisjointWith(leftClass,rightClass)" relations within the model
        /// </summary>
        public static bool CheckIsDisjointClassWith(this RDFOntologyClassModel classModel, RDFResource leftClass, RDFResource rightClass)
            => leftClass != null && rightClass != null && classModel != null && classModel.GetDisjointClassesWith(leftClass).Any(cls => cls.Equals(rightClass));

        /// <summary>
        /// Analyzes "DisjointWith(leftClass,rightClass)" relations of the model to answer the disjoint classes of the given owl:Class
        /// </summary>
        public static List<RDFResource> GetDisjointClassesWith(this RDFOntologyClassModel classModel, RDFResource owlClass)
        {
            List<RDFResource> disjointClasses = new List<RDFResource>();

            if (classModel != null && owlClass != null)
            {
                disjointClasses.AddRange(classModel.FindDisjointClassesWith(owlClass, new Dictionary<long, RDFResource>()));

                //We don't want to also enlist the given owl:Class
                disjointClasses.RemoveAll(cls => cls.Equals(owlClass));
            }

            return RDFQueryUtilities.RemoveDuplicates(disjointClasses);
        }

        /// <summary>
        /// Finds "DisjointWith(owlClass,X)" relations to enlist the disjoint classes of the given owl:Class
        /// </summary>
        internal static List<RDFResource> FindDisjointClassesWith(this RDFOntologyClassModel classModel, RDFResource owlClass, Dictionary<long, RDFResource> visitContext, RDFGraph workingGraph=null)
        {
            List<RDFResource> disjointClasses = new List<RDFResource>();
            if (workingGraph == null)
                workingGraph = classModel.TBoxVirtualGraph;

            #region VisitContext
            if (!visitContext.ContainsKey(owlClass.PatternMemberID))
                visitContext.Add(owlClass.PatternMemberID, owlClass);
            else
                return disjointClasses;
            #endregion

            #region Discovery
            // Find disjoint classes linked to the given one with owl:AllDisjointClasses shortcut [OWL2]
            List<RDFResource> allDisjointClasses = new List<RDFResource>();
            IEnumerator<RDFResource> allDisjoint = classModel.AllDisjointClassesEnumerator;
            while (allDisjoint.MoveNext())
                foreach (RDFTriple allDisjointMembers in workingGraph[allDisjoint.Current, RDFVocabulary.OWL.MEMBERS, null, null])
                {
                    RDFCollection allDisjointCollection = RDFModelUtilities.DeserializeCollectionFromGraph(workingGraph, (RDFResource)allDisjointMembers.Object, RDFModelEnums.RDFTripleFlavors.SPO);
                    if (allDisjointCollection.Items.Any(item => item.Equals(owlClass)))
                        allDisjointClasses.AddRange(allDisjointCollection.OfType<RDFResource>());
                }
            allDisjointClasses.RemoveAll(adm => adm.Equals(owlClass));

            // Find disjoint classes linked to the given one with owl:disjointWith relation
            List<RDFResource> disjointFromClasses = workingGraph[owlClass, RDFVocabulary.OWL.DISJOINT_WITH, null, null]
                                                      .Select(t => (RDFResource)t.Object)
                                                      .ToList();

            // Merge classes from both sets into a unique deduplicate working set
            List<RDFResource> disjointClassesSet = RDFQueryUtilities.RemoveDuplicates(allDisjointClasses.Union(disjointFromClasses).ToList());
            #endregion

            #region Analyze
            // Inference: DISJOINTWITH(A,B) ^ EQUIVALENTCLASS(B,C) -> DISJOINTWITH(A,C)
            foreach (RDFResource disjointClass in disjointClassesSet)
            {
                disjointClasses.Add(disjointClass);
                disjointClasses.AddRange(classModel.FindEquivalentClassesOf(disjointClass, visitContext, workingGraph));
            }

            // Inference: DISJOINTWITH(A,B) ^ SUBCLASS(C,B) -> DISJOINTWITH(A,C)
            foreach (RDFResource disjointClass in disjointClasses.ToList())
                disjointClasses.AddRange(classModel.FindSubClassesOf(disjointClass));

            // Inference: EQUIVALENTCLASS(A,B) ^ DISJOINTWITH(B,C) -> DISJOINTWITH(A,C)
            foreach (RDFResource compatibleClass in classModel.GetSuperClassesOf(owlClass)
                                                              .Union(classModel.GetEquivalentClassesOf(owlClass)))
                disjointClasses.AddRange(classModel.FindDisjointClassesWith(compatibleClass, visitContext, workingGraph));
            #endregion

            return disjointClasses;
        }

        /// <summary>
        /// Checks for the existence of "Domain(owlProperty,owlClass)" relations within the model
        /// </summary>
        public static bool CheckIsDomainOf(this RDFOntologyClassModel classModel, RDFResource owlClass, RDFResource owlProperty)
            => owlClass != null && owlProperty != null && classModel != null && classModel.GetDomainOf(owlProperty).Any(cls => cls.Equals(owlClass));

        /// <summary>
        /// Analyzes "Domain(owlProperty,X)" relations of the model to enlist the domain classes of the given owl:Property
        /// </summary>
        public static List<RDFResource> GetDomainOf(this RDFOntologyClassModel classModel, RDFResource owlProperty)
        {
            List<RDFResource> domainClasses = new List<RDFResource>();

            if (classModel != null && owlProperty != null)
                domainClasses.AddRange(classModel.FindDomainOf(owlProperty));

            //We don't want to enlist duplicate classes
            return RDFQueryUtilities.RemoveDuplicates(domainClasses);
        }

        /// <summary>
        /// Finds "Domain(owlProperty,X)" relations to enlist the domain classes of the given owl:Property
        /// </summary>
        internal static List<RDFResource> FindDomainOf(this RDFOntologyClassModel classModel, RDFResource owlProperty, RDFGraph workingGraph=null)
        {
            List<RDFResource> domainClasses = new List<RDFResource>();
            if (workingGraph == null)
                workingGraph = classModel.TBoxVirtualGraph;

            //DIRECT
            RDFGraph domainGraph = workingGraph[owlProperty, RDFVocabulary.RDFS.DOMAIN, null, null];
            foreach (RDFTriple domainTriple in domainGraph)
                domainClasses.Add((RDFResource)domainTriple.Object);

            //Inference: DOMAIN(P,A) ^ SUBCLASS(B,A) -> DOMAIN(P,B) 
            foreach (RDFResource domainClass in domainClasses.ToList())
                domainClasses.AddRange(classModel.GetSubClassesOf(domainClass));

            //Inference: DOMAIN(P,A) ^ EQUIVALENTCLASS(B,A) -> DOMAIN(P,B) 
            foreach (RDFResource domainClass in domainClasses.ToList())
                domainClasses.AddRange(classModel.GetEquivalentClassesOf(domainClass));

            return domainClasses;
        }

        /// <summary>
        /// Checks for the existence of "Range(owlProperty,owlClass)" relations within the model
        /// </summary>
        public static bool CheckIsRangeOf(this RDFOntologyClassModel classModel, RDFResource owlClass, RDFResource owlProperty)
            => owlClass != null && owlProperty != null && classModel != null && classModel.GetRangeOf(owlProperty).Any(cls => cls.Equals(owlClass));

        /// <summary>
        /// Analyzes "Range(owlProperty,X)" relations of the model to enlist the range classes of the given owl:Property
        /// </summary>
        public static List<RDFResource> GetRangeOf(this RDFOntologyClassModel classModel, RDFResource owlProperty)
        {
            List<RDFResource> rangeClasses = new List<RDFResource>();

            if (classModel != null && owlProperty != null)
                rangeClasses.AddRange(classModel.FindRangeOf(owlProperty));

            //We don't want to enlist duplicate classes
            return RDFQueryUtilities.RemoveDuplicates(rangeClasses);
        }

        /// <summary>
        /// Finds "Range(owlProperty,X)" relations to enlist the range classes of the given owl:Property
        /// </summary>
        internal static List<RDFResource> FindRangeOf(this RDFOntologyClassModel classModel, RDFResource owlProperty, RDFGraph workingGraph=null)
        {
            List<RDFResource> rangeClasses = new List<RDFResource>();
            if (workingGraph == null)
                workingGraph = classModel.TBoxVirtualGraph;

            //DIRECT
            RDFGraph rangeGraph = workingGraph[owlProperty, RDFVocabulary.RDFS.RANGE, null, null];
            foreach (RDFTriple rangeTriple in rangeGraph)
                rangeClasses.Add((RDFResource)rangeTriple.Object);

            //Inference: RANGE(P,A) ^ SUBCLASS(B,A) -> RANGE(P,B) 
            foreach (RDFResource rangeClass in rangeClasses.ToList())
                rangeClasses.AddRange(classModel.GetSubClassesOf(rangeClass));

            //Inference: RANGE(P,A) ^ EQUIVALENTCLASS(B,A) -> RANGE(P,B) 
            foreach (RDFResource rangeClass in rangeClasses.ToList())
                rangeClasses.AddRange(classModel.GetEquivalentClassesOf(rangeClass));

            return rangeClasses;
        }

        /// <summary>
        ///  Analyzes "hasKey(owlClass,X)" relations of the model to answer the key properties of the given owl:Class [OWL2]
        /// </summary>
        public static List<RDFResource> GetKeyPropertiesOf(this RDFOntologyClassModel classModel, RDFResource owlClass)
        {
            List<RDFResource> keyProperties = new List<RDFResource>();

            if (classModel != null && owlClass != null)
            {
                //Restrict T-BOX knowledge to owl:hasKey relations of the given owl:Class (both explicit and inferred)
                RDFResource hasKeyRepresentative = classModel.TBoxGraph[owlClass, RDFVocabulary.OWL.HAS_KEY, null, null]
                                                     .Select(t => t.Object)
                                                     .OfType<RDFResource>()
                                                     .FirstOrDefault();
                if (hasKeyRepresentative != null)
                {
                    //Reconstruct collection of key properties from T-BOX knowledge
                    RDFCollection keyPropertiesCollection = RDFModelUtilities.DeserializeCollectionFromGraph(classModel.TBoxGraph, hasKeyRepresentative, RDFModelEnums.RDFTripleFlavors.SPO);
                    foreach (RDFPatternMember keyProperty in keyPropertiesCollection)
                        keyProperties.Add((RDFResource)keyProperty);
                }
            }

            return keyProperties;
        }
        #endregion

        #region Checker
        /// <summary>
        /// Checks if the given owl:Class is a reserved ontology class
        /// </summary>
        internal static bool CheckReservedClass(this RDFResource owlClass) =>
            RDFSemanticsUtilities.ReservedClasses.Contains(owlClass.PatternMemberID);

        /// <summary>
        /// Checks if the given childClass can be subClass of the given motherClass without tampering OWL-DL integrity
        /// </summary>
        internal static bool CheckSubClassCompatibility(this RDFOntologyClassModel classModel, RDFResource childClass, RDFResource motherClass)
            => !classModel.CheckIsSubClassOf(motherClass, childClass)
                 && !classModel.CheckIsEquivalentClassOf(motherClass, childClass)
                   && !classModel.CheckIsDisjointClassWith(motherClass, childClass);

        /// <summary>
        /// Checks if the given leftClass can be equivalentClass of the given rightClass without tampering OWL-DL integrity
        /// </summary>
        internal static bool CheckEquivalentClassCompatibility(this RDFOntologyClassModel classModel, RDFResource leftClass, RDFResource rightClass)
            => !classModel.CheckIsSubClassOf(leftClass, rightClass)
                 && !classModel.CheckIsSuperClassOf(leftClass, rightClass)
                   && !classModel.CheckIsDisjointClassWith(leftClass, rightClass);

        /// <summary>
        /// Checks if the given leftClass can be disjoint class of the given right class without tampering OWL-DL integrity
        /// </summary>
        internal static bool CheckDisjointWithCompatibility(this RDFOntologyClassModel classModel, RDFResource leftClass, RDFResource rightClass)
            => !classModel.CheckIsSubClassOf(leftClass, rightClass)
                 && !classModel.CheckIsSuperClassOf(leftClass, rightClass)
                   && !classModel.CheckIsEquivalentClassOf(leftClass, rightClass);
        #endregion
    }
}