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
    /// RDFOntologyClassModelAnalyzer contains methods for analyzing relations describing application domain entities
    /// </summary>
    public static class RDFOntologyClassModelAnalyzer
    {
        #region Properties
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
                &&  classModel.TBoxGraph.Any(t => t.Subject.Equals(owlRestriction) && t.Predicate.Equals(RDFVocabulary.OWL.MIN_CARDINALITY))
                && !classModel.TBoxGraph.Any(t => t.Subject.Equals(owlRestriction) && t.Predicate.Equals(RDFVocabulary.OWL.MAX_CARDINALITY));

        /// <summary>
        /// Checks for the existence of the given owl:[Max]CardinalityRestriction declaration within the model
        /// </summary>
        internal static bool CheckHasMaxCardinalityRestrictionClass(this RDFOntologyClassModel classModel, RDFResource owlRestriction)
            => CheckHasRestrictionClass(classModel, owlRestriction)
                &&  classModel.TBoxGraph.Any(t => t.Subject.Equals(owlRestriction) && t.Predicate.Equals(RDFVocabulary.OWL.MAX_CARDINALITY))
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
                &&  classModel.TBoxGraph.Any(t => t.Subject.Equals(owlRestriction) && t.Predicate.Equals(RDFVocabulary.OWL.MIN_QUALIFIED_CARDINALITY))
                && !classModel.TBoxGraph.Any(t => t.Subject.Equals(owlRestriction) && t.Predicate.Equals(RDFVocabulary.OWL.MAX_QUALIFIED_CARDINALITY));

        /// <summary>
        /// Checks for the existence of the given owl:[Max]QualifiedCardinalityRestriction declaration within the model [OWL2]
        /// </summary>
        internal static bool CheckHasMaxQualifiedCardinalityRestrictionClass(this RDFOntologyClassModel classModel, RDFResource owlRestriction)
            => CheckHasRestrictionClass(classModel, owlRestriction)
                &&  classModel.TBoxGraph.Any(t => t.Subject.Equals(owlRestriction) && t.Predicate.Equals(RDFVocabulary.OWL.MAX_QUALIFIED_CARDINALITY))
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

        /// <summary>
        /// Checks for the existence of "SubClass(childClass,motherClass)" relations within the model
        /// </summary>
        public static bool CheckAreSubClasses(this RDFOntologyClassModel classModel, RDFResource childClass, RDFResource motherClass)
            => childClass != null && motherClass != null && classModel != null && classModel.AnswerSubClasses(motherClass).Any(cls => cls.Equals(childClass));

        /// <summary>
        /// Analyzes "SubClass(owlClass, X)" relations of the model to answer the sub classes of the given owl:Class
        /// </summary>
        public static List<RDFResource> AnswerSubClasses(this RDFOntologyClassModel classModel, RDFResource owlClass)
        {
            List<RDFResource> subClasses = new List<RDFResource>();

            if (classModel != null && owlClass != null)
            {
                //Restrict T-BOX knowledge to rdfs:subClassOf relations (both explicit and inferred)
                RDFGraph filteredTBox = classModel.TBoxGraph[null, RDFVocabulary.RDFS.SUB_CLASS_OF, null, null].UnionWith(
                                           classModel.TBoxInferenceGraph[null, RDFVocabulary.RDFS.SUB_CLASS_OF, null, null]);
                subClasses.AddRange(classModel.FindSubClasses(owlClass, filteredTBox, new Dictionary<long, RDFResource>()));

                //We don't want to also enlist the given owl:Class
                subClasses.RemoveAll(cls => cls.Equals(owlClass));
            }

            return subClasses;
        }

        /// <summary>
        /// Finds "SubClass(owlClass, X)" relations to enlist the sub classes of the given owl:Class
        /// </summary>
        internal static List<RDFResource> FindSubClasses(this RDFOntologyClassModel classModel, RDFResource owlClass, RDFGraph filteredTBox, Dictionary<long, RDFResource> visitContext)
        {
            List<RDFResource> subClasses = new List<RDFResource>();

            #region VisitContext
            if (!visitContext.ContainsKey(owlClass.PatternMemberID))
                visitContext.Add(owlClass.PatternMemberID, owlClass);
            else
                return subClasses;
            #endregion

            //DIRECT
            foreach (RDFTriple subClassRelation in filteredTBox[null, null, owlClass, null])
                subClasses.Add((RDFResource)subClassRelation.Subject);

            //INDIRECT (TRANSITIVE)
            foreach (RDFResource subClass in subClasses.ToList())
                subClasses.AddRange(classModel.FindSubClasses(subClass, filteredTBox, visitContext));

            return subClasses;
        }

        /// <summary>
        /// Checks for the existence of "SuperClass(motherClass,childClass)" relations within the model
        /// </summary>
        public static bool CheckAreSuperClasses(this RDFOntologyClassModel classModel, RDFResource motherClass, RDFResource childClass)
            => childClass != null && motherClass != null && classModel != null && classModel.AnswerSuperClasses(childClass).Any(cls => cls.Equals(motherClass));

        /// <summary>
        /// Analyzes "SuperClass(owlClass, X)" relations of the model to answer the super classes of the given owl:Class
        /// </summary>
        public static List<RDFResource> AnswerSuperClasses(this RDFOntologyClassModel classModel, RDFResource owlClass)
        {
            List<RDFResource> superClasses = new List<RDFResource>();

            if (classModel != null && owlClass != null)
            {
                //Restrict T-BOX knowledge to rdfs:subClassOf relations (both explicit and inferred)
                RDFGraph filteredTBox = classModel.TBoxGraph[null, RDFVocabulary.RDFS.SUB_CLASS_OF, null, null].UnionWith(
                                           classModel.TBoxInferenceGraph[null, RDFVocabulary.RDFS.SUB_CLASS_OF, null, null]);
                superClasses.AddRange(classModel.FindSuperClasses(owlClass, filteredTBox, new Dictionary<long, RDFResource>()));

                //We don't want to also enlist the given owl:Class
                superClasses.RemoveAll(cls => cls.Equals(owlClass));
            }

            return superClasses;
        }

        /// <summary>
        /// Finds "SuperClass(owlClass, X)" relations to enlist the super classes of the given owl:Class
        /// </summary>
        internal static List<RDFResource> FindSuperClasses(this RDFOntologyClassModel classModel, RDFResource owlClass, RDFGraph filteredTBox, Dictionary<long, RDFResource> visitContext)
        {
            List<RDFResource> superClasses = new List<RDFResource>();

            #region VisitContext
            if (!visitContext.ContainsKey(owlClass.PatternMemberID))
                visitContext.Add(owlClass.PatternMemberID, owlClass);
            else
                return superClasses;
            #endregion

            //DIRECT
            foreach (RDFTriple superClassRelation in filteredTBox[owlClass, null, null, null])
                superClasses.Add((RDFResource)superClassRelation.Object);

            //INDIRECT (TRANSITIVE)
            foreach (RDFResource superClass in superClasses.ToList())
                superClasses.AddRange(classModel.FindSuperClasses(superClass, filteredTBox, visitContext));

            return superClasses;
        }

        /// <summary>
        /// Checks for the existence of "EquivalentClass(leftClass,rightClass)" relations within the model
        /// </summary>
        public static bool CheckAreEquivalentClasses(this RDFOntologyClassModel classModel, RDFResource leftClass, RDFResource rightClass)
            => leftClass != null && rightClass != null && classModel != null && classModel.AnswerEquivalentClasses(leftClass).Any(cls => cls.Equals(rightClass));

        /// <summary>
        /// Analyzes "EquivalentClass(owlClass, X)" relations of the model to answer the equivalent classes of the given owl:Class
        /// </summary>
        public static List<RDFResource> AnswerEquivalentClasses(this RDFOntologyClassModel classModel, RDFResource owlClass)
        {
            List<RDFResource> equivalentClasses = new List<RDFResource>();

            if (classModel != null && owlClass != null)
            {
                //Restrict T-BOX knowledge to owl:equivalentClass relations (both explicit and inferred)
                RDFGraph filteredTBox = classModel.TBoxGraph[null, RDFVocabulary.OWL.EQUIVALENT_CLASS, null, null].UnionWith(
                                           classModel.TBoxInferenceGraph[null, RDFVocabulary.OWL.EQUIVALENT_CLASS, null, null]);
                equivalentClasses.AddRange(classModel.FindEquivalentClasses(owlClass, filteredTBox, new Dictionary<long, RDFResource>()));

                //We don't want to also enlist the given owl:Class
                equivalentClasses.RemoveAll(cls => cls.Equals(owlClass));
            }

            return equivalentClasses;
        }

        /// <summary>
        /// Finds "EquivalentClass(owlClass, X)" relations to enlist the equivalent classes of the given owl:Class
        /// </summary>
        internal static List<RDFResource> FindEquivalentClasses(this RDFOntologyClassModel classModel, RDFResource owlClass, RDFGraph filteredTBox, Dictionary<long, RDFResource> visitContext)
        {
            List<RDFResource> equivalentClasses = new List<RDFResource>();

            #region VisitContext
            if (!visitContext.ContainsKey(owlClass.PatternMemberID))
                visitContext.Add(owlClass.PatternMemberID, owlClass);
            else
                return equivalentClasses;
            #endregion

            //DIRECT
            foreach (RDFTriple equivalentClassRelation in filteredTBox[owlClass, null, null, null])
                equivalentClasses.Add((RDFResource)equivalentClassRelation.Object);

            //INDIRECT (TRANSITIVE)
            foreach (RDFResource equivalentClass in equivalentClasses.ToList())
                equivalentClasses.AddRange(classModel.FindEquivalentClasses(equivalentClass, filteredTBox, visitContext));

            return equivalentClasses;
        }

        /// <summary>
        /// Checks for the existence of "DisjointWith(leftClass,rightClass)" relations within the model
        /// </summary>
        public static bool CheckAreDisjointClasses(this RDFOntologyClassModel classModel, RDFResource leftClass, RDFResource rightClass)
            => leftClass != null && rightClass != null && classModel != null && classModel.AnswerDisjointClasses(leftClass).Any(cls => cls.Equals(rightClass));

        /// <summary>
        /// Analyzes "DisjointWith(leftClass,rightClass)" relations of the model to answer the disjoint classes of the given owl:Class
        /// </summary>
        public static List<RDFResource> AnswerDisjointClasses(this RDFOntologyClassModel classModel, RDFResource owlClass)
        {
            List<RDFResource> disjointClasses = new List<RDFResource>();

            if (classModel != null && owlClass != null)
            {
                //Restrict T-BOX knowledge to owl:disjointWith and owl:equivalentClass relations (both explicit and inferred)
                RDFGraph filteredTBox =
                    classModel.TBoxGraph[null, RDFVocabulary.OWL.DISJOINT_WITH, null, null].UnionWith(
                       classModel.TBoxInferenceGraph[null, RDFVocabulary.OWL.DISJOINT_WITH, null, null].UnionWith(
                          classModel.TBoxGraph[null, RDFVocabulary.OWL.EQUIVALENT_CLASS, null, null].UnionWith(
                             classModel.TBoxInferenceGraph[null, RDFVocabulary.OWL.EQUIVALENT_CLASS, null, null])));
                disjointClasses.AddRange(classModel.FindDisjointClasses(owlClass, filteredTBox));

                //We don't want to also enlist the given owl:Class
                disjointClasses.RemoveAll(cls => cls.Equals(owlClass));
            }

            return disjointClasses;
        }

        /// <summary>
        /// Finds "DisjointWith(owlClass, X)" relations to enlist the disjoint classes of the given owl:Class
        /// </summary>
        internal static List<RDFResource> FindDisjointClasses(this RDFOntologyClassModel classModel, RDFResource owlClass, RDFGraph filteredTBox)
        {
            List<RDFResource> disjointClasses = new List<RDFResource>();

            //DIRECT
            RDFGraph disjointWithGraph = filteredTBox[owlClass, RDFVocabulary.OWL.DISJOINT_WITH, null, null];
            foreach (RDFTriple disjointWithTriple in disjointWithGraph)
                disjointClasses.Add((RDFResource)disjointWithTriple.Object);

            //INDIRECT (EXPLOITING EQUIVALENTCLASS)
            Dictionary<long, RDFResource> visitContext = new Dictionary<long, RDFResource>();
            RDFGraph equivalentClassGraph = filteredTBox[null, RDFVocabulary.OWL.EQUIVALENT_CLASS, null, null];
            foreach (RDFResource disjointClass in disjointClasses.ToList())
                disjointClasses.AddRange(classModel.FindEquivalentClasses(disjointClass, equivalentClassGraph, visitContext));

            return disjointClasses;
        }

        /// <summary>
        /// Checks for the existence of "Domain(owlProperty,owlClass)" relations within the model
        /// </summary>
        public static bool CheckIsDomainOfProperty(this RDFOntologyClassModel classModel, RDFResource owlClass, RDFResource owlProperty)
            => owlClass != null && owlProperty != null && classModel != null && classModel.AnswerDomainOfProperty(owlProperty).Any(cls => cls.Equals(owlClass));

        /// <summary>
        /// Analyzes "Domain(owlProperty,X)" relations of the model to enlist the domain classes of the given owl:Property
        /// </summary>
        public static List<RDFResource> AnswerDomainOfProperty(this RDFOntologyClassModel classModel, RDFResource owlProperty)
        {
            List<RDFResource> domainClasses = new List<RDFResource>();

            if (classModel != null && owlProperty != null)
            {
                //Restrict T-BOX knowledge to rdfs:domain and owl:equivalentClass relations (both explicit and inferred)
                RDFGraph filteredTBox =
                    classModel.TBoxGraph[null, RDFVocabulary.RDFS.DOMAIN, null, null].UnionWith(
                       classModel.TBoxInferenceGraph[null, RDFVocabulary.RDFS.DOMAIN, null, null].UnionWith(
                          classModel.TBoxGraph[null, RDFVocabulary.OWL.EQUIVALENT_CLASS, null, null].UnionWith(
                             classModel.TBoxInferenceGraph[null, RDFVocabulary.OWL.EQUIVALENT_CLASS, null, null])));
                domainClasses.AddRange(classModel.FindDomainOfProperty(owlProperty, filteredTBox));
            }

            return domainClasses;
        }

        /// <summary>
        /// Finds "Domain(owlProperty, X)" relations to enlist the domain classes of the given owl:Property
        /// </summary>
        internal static List<RDFResource> FindDomainOfProperty(this RDFOntologyClassModel classModel, RDFResource owlProperty, RDFGraph filteredTBox)
        {
            List<RDFResource> domainClasses = new List<RDFResource>();

            //DIRECT
            RDFGraph domainGraph = filteredTBox[owlProperty, RDFVocabulary.RDFS.DOMAIN, null, null];
            foreach (RDFTriple domainTriple in domainGraph)
                domainClasses.Add((RDFResource)domainTriple.Object);

            //INDIRECT (EXPLOITING EQUIVALENTCLASS)
            Dictionary<long, RDFResource> visitContext = new Dictionary<long, RDFResource>();
            RDFGraph equivalentClassGraph = filteredTBox[null, RDFVocabulary.OWL.EQUIVALENT_CLASS, null, null];
            foreach (RDFResource domainClass in domainClasses.ToList())
                domainClasses.AddRange(classModel.FindEquivalentClasses(domainClass, equivalentClassGraph, visitContext));

            return domainClasses;
        }

        /// <summary>
        /// Checks for the existence of "Range(owlProperty,owlClass)" relations within the model
        /// </summary>
        public static bool CheckIsRangeOfProperty(this RDFOntologyClassModel classModel, RDFResource owlClass, RDFResource owlProperty)
            => owlClass != null && owlProperty != null && classModel != null && classModel.AnswerRangeOfProperty(owlProperty).Any(cls => cls.Equals(owlClass));

        /// <summary>
        /// Analyzes "Range(owlProperty,X)" relations of the model to enlist the range classes of the given owl:Property
        /// </summary>
        public static List<RDFResource> AnswerRangeOfProperty(this RDFOntologyClassModel classModel, RDFResource owlProperty)
        {
            List<RDFResource> rangeClasses = new List<RDFResource>();

            if (classModel != null && owlProperty != null)
            {
                //Restrict T-BOX knowledge to rdfs:range and owl:equivalentClass relations (both explicit and inferred)
                RDFGraph filteredTBox =
                    classModel.TBoxGraph[null, RDFVocabulary.RDFS.RANGE, null, null].UnionWith(
                       classModel.TBoxInferenceGraph[null, RDFVocabulary.RDFS.RANGE, null, null].UnionWith(
                          classModel.TBoxGraph[null, RDFVocabulary.OWL.EQUIVALENT_CLASS, null, null].UnionWith(
                             classModel.TBoxInferenceGraph[null, RDFVocabulary.OWL.EQUIVALENT_CLASS, null, null])));
                rangeClasses.AddRange(classModel.FindRangeOfProperty(owlProperty, filteredTBox));
            }

            return rangeClasses;
        }

        /// <summary>
        /// Finds "Range(owlProperty, X)" relations to enlist the range classes of the given owl:Property
        /// </summary>
        internal static List<RDFResource> FindRangeOfProperty(this RDFOntologyClassModel classModel, RDFResource owlProperty, RDFGraph filteredTBox)
        {
            List<RDFResource> rangeClasses = new List<RDFResource>();

            //DIRECT
            RDFGraph rangeGraph = filteredTBox[owlProperty, RDFVocabulary.RDFS.RANGE, null, null];
            foreach (RDFTriple rangeTriple in rangeGraph)
                rangeClasses.Add((RDFResource)rangeTriple.Object);

            //INDIRECT (EXPLOITING EQUIVALENTCLASS)
            Dictionary<long, RDFResource> visitContext = new Dictionary<long, RDFResource>();
            RDFGraph equivalentClassGraph = filteredTBox[null, RDFVocabulary.OWL.EQUIVALENT_CLASS, null, null];
            foreach (RDFResource rangeClass in rangeClasses.ToList())
                rangeClasses.AddRange(classModel.FindEquivalentClasses(rangeClass, equivalentClassGraph, visitContext));

            return rangeClasses;
        }

        /// <summary>
        ///  Analyzes "hasKey(owlClass,X)" relations of the model to answer the key properties of the given owl:Class [OWL2]
        /// </summary>
        public static List<RDFResource> AnswerKeyProperties(this RDFOntologyClassModel classModel, RDFResource owlClass)
        {
            List<RDFResource> keyProperties = new List<RDFResource>();

            if (classModel != null && owlClass != null)
            {
                RDFGraph tboxVirtualGraph = classModel.TBoxVirtualGraph;

                //Restrict T-BOX knowledge to owl:hasKey relations of the given owl:Class (both explicit and inferred)
                RDFResource hasKeyRepresentative = tboxVirtualGraph[owlClass, RDFVocabulary.OWL.HAS_KEY, null, null]
                                                     .Select(t => t.Object)
                                                     .OfType<RDFResource>()
                                                     .FirstOrDefault();
                if (hasKeyRepresentative != null)
                {
                    //Reconstruct collection of key properties from T-BOX knowledge
                    RDFCollection keyPropertiesCollection = RDFModelUtilities.DeserializeCollectionFromGraph(tboxVirtualGraph, hasKeyRepresentative, RDFModelEnums.RDFTripleFlavors.SPO);
                    foreach (RDFPatternMember keyProperty in keyPropertiesCollection)
                        keyProperties.Add((RDFResource)keyProperty);
                }
            }   

            return keyProperties;
        }
        #endregion
    }
}