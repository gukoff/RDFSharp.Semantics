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
using System.Collections.Generic;
using System.Data;
using System.Linq;

namespace RDFSharp.Semantics
{
    /// <summary>
    /// RDFOntologyClassModelHelper contains utility methods supporting RDFS/OWL-DL modeling, validation and reasoning (T-BOX)
    /// </summary>
    public static class RDFOntologyClassModelHelper
    {
        #region SubClassOf
        /// <summary>
        /// Checks if the given childClass is subClass of the given motherClass within the given class model
        /// </summary>
        public static bool CheckIsSubClassOf(this RDFOntologyClassModel classModel, RDFOntologyClass childClass, RDFOntologyClass motherClass)
            => childClass != null && motherClass != null && classModel != null && classModel.GetSuperClassesOf(childClass).Classes.ContainsKey(motherClass.PatternMemberID);

        /// <summary>
        /// Enlists the subClasses of the given class within the given class model
        /// </summary>
        public static RDFOntologyClassModel GetSubClassesOf(this RDFOntologyClassModel classModel, RDFOntologyClass ontologyClass)
        {
            RDFOntologyClassModel result = new RDFOntologyClassModel();
            if (ontologyClass != null && classModel != null)
            {
                //Step 1: Reason on the given class
                result = classModel.GetSubClassesOfInternal(ontologyClass);

                //Step 2: Reason on the equivalent classes
                foreach (RDFOntologyClass ec in classModel.GetEquivalentClassesOf(ontologyClass))
                    result = result.UnionWith(classModel.GetSubClassesOfInternal(ec));
            }
            return result;
        }

        /// <summary>
        /// Subsumes the "rdfs:subClassOf" taxonomy to discover direct and indirect subClasses of the given class
        /// </summary>
        internal static RDFOntologyClassModel GetSubClassesOfInternalVisitor(this RDFOntologyClassModel classModel, RDFOntologyClass ontologyClass)
        {
            RDFOntologyClassModel result = new RDFOntologyClassModel();

            // Transitivity of "rdfs:subClassOf" taxonomy: ((A SUBCLASSOF B)  &&  (B SUBCLASSOF C))  =>  (A SUBCLASSOF C)
            foreach (RDFOntologyTaxonomyEntry sc in classModel.Relations.SubClassOf.SelectEntriesByObject(ontologyClass))
            {
                result.AddClass((RDFOntologyClass)sc.TaxonomySubject);
                result = result.UnionWith(classModel.GetSubClassesOfInternalVisitor((RDFOntologyClass)sc.TaxonomySubject));
            }

            return result;
        }
        internal static RDFOntologyClassModel GetSubClassesOfInternal(this RDFOntologyClassModel classModel, RDFOntologyClass ontologyClass)
        {
            // Step 1: Direct subsumption of "rdfs:subClassOf" taxonomy
            RDFOntologyClassModel result = classModel.GetSubClassesOfInternalVisitor(ontologyClass);

            // Step 2: Enlist equivalent classes of subclasses
            foreach (RDFOntologyClass sc in result.ToList())
                result = result.UnionWith(classModel.GetEquivalentClassesOf(sc)
                                                    .UnionWith(classModel.GetSubClassesOf(sc)));

            return result;
        }
        #endregion

        #region SuperClassOf
        /// <summary>
        /// Checks if the given motherClass is superClass of the given childClass within the given class model
        /// </summary>
        public static bool CheckIsSuperClassOf(this RDFOntologyClassModel classModel, RDFOntologyClass motherClass, RDFOntologyClass childClass)
            => motherClass != null && childClass != null && classModel != null && classModel.GetSubClassesOf(motherClass).Classes.ContainsKey(childClass.PatternMemberID);

        /// <summary>
        /// Enlists the superClasses of the given class within the given class model
        /// </summary>
        public static RDFOntologyClassModel GetSuperClassesOf(this RDFOntologyClassModel classModel, RDFOntologyClass ontologyClass)
        {
            RDFOntologyClassModel result = new RDFOntologyClassModel();
            if (ontologyClass != null && classModel != null)
            {
                //Step 1: Reason on the given class
                result = classModel.GetSuperClassesOfInternal(ontologyClass);

                //Step 2: Reason on the equivalent classes
                foreach (RDFOntologyClass ec in classModel.GetEquivalentClassesOf(ontologyClass))
                    result = result.UnionWith(classModel.GetSuperClassesOfInternal(ec));
            }
            return result;
        }

        /// <summary>
        /// Subsumes the "rdfs:subClassOf" taxonomy to discover direct and indirect superClasses of the given class
        /// </summary>
        internal static RDFOntologyClassModel GetSuperClassesOfInternalVisitor(this RDFOntologyClassModel classModel, RDFOntologyClass ontologyClass)
        {
            RDFOntologyClassModel result = new RDFOntologyClassModel();

            // Transitivity of "rdfs:subClassOf" taxonomy: ((A SUPERCLASSOF B)  &&  (B SUPERCLASSOF C))  =>  (A SUPERCLASSOF C)
            foreach (RDFOntologyTaxonomyEntry sc in classModel.Relations.SubClassOf.SelectEntriesBySubject(ontologyClass))
            {
                result.AddClass((RDFOntologyClass)sc.TaxonomyObject);
                result = result.UnionWith(classModel.GetSuperClassesOfInternalVisitor((RDFOntologyClass)sc.TaxonomyObject));
            }

            return result;
        }
        internal static RDFOntologyClassModel GetSuperClassesOfInternal(this RDFOntologyClassModel classModel, RDFOntologyClass ontologyClass)
        {
            // Step 1: Direct subsumption of "rdfs:subClassOf" taxonomy
            RDFOntologyClassModel result = classModel.GetSuperClassesOfInternalVisitor(ontologyClass);

            // Step 2: Enlist equivalent classes of superclasses
            foreach (RDFOntologyClass sc in result.ToList())
                result = result.UnionWith(classModel.GetEquivalentClassesOf(sc)
                                                    .UnionWith(classModel.GetSuperClassesOf(sc)));

            return result;
        }
        #endregion

        #region EquivalentClass
        /// <summary>
        /// Checks if the given aClass is equivalentClass of the given bClass within the given class model
        /// </summary>
        public static bool CheckIsEquivalentClassOf(this RDFOntologyClassModel classModel, RDFOntologyClass aClass, RDFOntologyClass bClass)
            => aClass != null && bClass != null && classModel != null && classModel.GetEquivalentClassesOf(aClass).Classes.ContainsKey(bClass.PatternMemberID);

        /// <summary>
        /// Enlists the equivalentClasses of the given class within the given class model
        /// </summary>
        public static RDFOntologyClassModel GetEquivalentClassesOf(this RDFOntologyClassModel classModel, RDFOntologyClass ontologyClass)
        {
            RDFOntologyClassModel result = new RDFOntologyClassModel();

            if (ontologyClass != null && classModel != null)
                result = classModel.GetEquivalentClassesOfInternal(ontologyClass, null)
                                   .RemoveClass(ontologyClass); //Safety deletion

            return result;
        }

        /// <summary>
        /// Subsumes the "owl:equivalentClass" taxonomy to discover direct and indirect equivalentClasses of the given class
        /// </summary>
        internal static RDFOntologyClassModel GetEquivalentClassesOfInternal(this RDFOntologyClassModel classModel, RDFOntologyClass ontClass, Dictionary<long, RDFOntologyClass> visitContext)
        {
            RDFOntologyClassModel result = new RDFOntologyClassModel();

            #region visitContext
            if (visitContext == null)
                visitContext = new Dictionary<long, RDFOntologyClass>() { { ontClass.PatternMemberID, ontClass } };
            else
            {
                if (!visitContext.ContainsKey(ontClass.PatternMemberID))
                    visitContext.Add(ontClass.PatternMemberID, ontClass);
                else
                    return result;
            }
            #endregion

            // Transitivity of "owl:equivalentClass" taxonomy: ((A EQUIVALENTCLASSOF B)  &&  (B EQUIVALENTCLASS C))  =>  (A EQUIVALENTCLASS C)
            foreach (RDFOntologyTaxonomyEntry ec in classModel.Relations.EquivalentClass.SelectEntriesBySubject(ontClass))
            {
                result.AddClass((RDFOntologyClass)ec.TaxonomyObject);
                result = result.UnionWith(classModel.GetEquivalentClassesOfInternal((RDFOntologyClass)ec.TaxonomyObject, visitContext));
            }

            return result;
        }
        #endregion

        #region DisjointWith
        /// <summary>
        /// Checks if the given aClass is disjointClass with the given bClass within the given class model
        /// </summary>
        public static bool CheckIsDisjointClassWith(this RDFOntologyClassModel classModel, RDFOntologyClass aClass, RDFOntologyClass bClass)
            => aClass != null && bClass != null && classModel != null && classModel.GetDisjointClassesWith(aClass).Classes.ContainsKey(bClass.PatternMemberID);

        /// <summary>
        /// Enlists the disjointClasses with the given class within the given class model
        /// </summary>
        public static RDFOntologyClassModel GetDisjointClassesWith(this RDFOntologyClassModel classModel, RDFOntologyClass ontClass)
        {
            RDFOntologyClassModel result = new RDFOntologyClassModel();

            if (ontClass != null && classModel != null)
                result = classModel.GetDisjointClassesWithInternal(ontClass, null)
                                   .RemoveClass(ontClass); //Safety deletion

            return result;
        }

        /// <summary>
        /// Subsumes the "owl:disjointWith" taxonomy to discover direct and indirect disjointClasses of the given class
        /// </summary>
        internal static RDFOntologyClassModel GetDisjointClassesWithInternal(this RDFOntologyClassModel classModel, RDFOntologyClass ontClass, Dictionary<long, RDFOntologyClass> visitContext)
        {
            RDFOntologyClassModel result = new RDFOntologyClassModel();

            #region visitContext
            if (visitContext == null)
                visitContext = new Dictionary<long, RDFOntologyClass>() { { ontClass.PatternMemberID, ontClass } };
            else
            {
                if (!visitContext.ContainsKey(ontClass.PatternMemberID))
                    visitContext.Add(ontClass.PatternMemberID, ontClass);
                else
                    return result;
            }
            #endregion

            // Inference: ((A DISJOINTWITH B)   &&  (B EQUIVALENTCLASS C))  =>  (A DISJOINTWITH C)
            foreach (RDFOntologyTaxonomyEntry dw in classModel.Relations.DisjointWith.SelectEntriesBySubject(ontClass))
                result = result.UnionWith(classModel.GetEquivalentClassesOfInternal((RDFOntologyClass)dw.TaxonomyObject, visitContext))
                               .AddClass((RDFOntologyClass)dw.TaxonomyObject);

            // Inference: ((A DISJOINTWITH B)   &&  (B SUPERCLASS C))  =>  (A DISJOINTWITH C)
            foreach (RDFOntologyClass sc in result.ToList())
                result = result.UnionWith(classModel.GetSubClassesOfInternal(sc));

            // Inference: ((A EQUIVALENTCLASS B || A SUBCLASSOF B)  &&  (B DISJOINTWITH C))  =>  (A DISJOINTWITH C)
            RDFOntologyClassModel compatibleCls = classModel.GetSuperClassesOf(ontClass)
                                                            .UnionWith(classModel.GetEquivalentClassesOf(ontClass));
            foreach (RDFOntologyClass ec in compatibleCls)
                result = result.UnionWith(classModel.GetDisjointClassesWithInternal(ec, visitContext));

            return result;
        }
        #endregion

        #region HasKey [OWL2]
        /// <summary>
        /// Gets the key values for each member of the given class having a complete (or partial, if allowed) key representation [OWL2]
        /// </summary>
        public static Dictionary<string, List<RDFOntologyResource>> GetKeyValuesOf(this RDFOntology ontology, RDFOntologyClass ontologyClass, bool allowPartialKeyValues)
        {
            Dictionary<string, List<RDFOntologyResource>> result = new Dictionary<string, List<RDFOntologyResource>>();

            RDFOntologyTaxonomy hasKeyClassTaxonomy = ontology.Model.ClassModel.Relations.HasKey.SelectEntriesBySubject(ontologyClass);
            if (hasKeyClassTaxonomy.Any())
            {
                //Enlist members of owl:hasKey class
                RDFOntologyData hasKeyClassMembers = RDFOntologyDataHelper.GetMembersOf(ontology, ontologyClass);

                //Fetch owl:hasKey property values for each of owl:haskey class members
                foreach (RDFOntologyTaxonomyEntry hasKeyClassTaxonomyEntry in hasKeyClassTaxonomy)
                {
                    foreach (RDFOntologyFact hasKeyClassMember in hasKeyClassMembers)
                    {
                        List<RDFOntologyResource> keyPropertyValues = ontology.Data.Relations.Assertions.SelectEntriesBySubject(hasKeyClassMember)
                                                                                                        .SelectEntriesByPredicate(hasKeyClassTaxonomyEntry.TaxonomyObject)
                                                                                                        .Select(te => te.TaxonomyObject)
                                                                                                        .ToList();

                        //This is to signal partial owl:hasKey property value
                        if (keyPropertyValues.Count == 0)
                            keyPropertyValues.Add(null);

                        if (!result.ContainsKey(hasKeyClassMember.ToString()))
                            result.Add(hasKeyClassMember.ToString(), keyPropertyValues);
                        else
                            result[hasKeyClassMember.ToString()].AddRange(keyPropertyValues);
                    }
                }
            }

            //If partial key values are not allowed, remove them from result
            return allowPartialKeyValues ? result : result.Where(res => res.Value.TrueForAll(x => x != null))
                                                          .ToDictionary(kv => kv.Key, kv => kv.Value);
        }
        #endregion

        #region Domain
        /// <summary>
        /// Checks if the given ontology class is domain of the given ontology property within the given ontology class model
        /// </summary>
        public static bool CheckIsDomainOf(this RDFOntologyClassModel classModel, RDFOntologyClass domainClass, RDFOntologyProperty ontologyProperty)
            => domainClass != null && ontologyProperty != null && classModel != null && classModel.GetDomainOf(ontologyProperty).Classes.ContainsKey(domainClass.PatternMemberID);

        /// <summary>
        /// Enlists the domain classes of the given property within the given ontology class model
        /// </summary>
        public static RDFOntologyClassModel GetDomainOf(this RDFOntologyClassModel classModel, RDFOntologyProperty ontologyProperty)
        {
            RDFOntologyClassModel result = new RDFOntologyClassModel();

            if (ontologyProperty != null && classModel != null && ontologyProperty.Domain != null)
                result = classModel.GetSubClassesOf(ontologyProperty.Domain)
                                    .UnionWith(classModel.GetEquivalentClassesOf(ontologyProperty.Domain))
                                    .AddClass(ontologyProperty.Domain);

            return result;
        }
        #endregion

        #region Range
        /// <summary>
        /// Checks if the given ontology class is range of the given ontology property within the given ontology class model
        /// </summary>
        public static bool CheckIsRangeOf(this RDFOntologyClassModel classModel, RDFOntologyClass rangeClass, RDFOntologyProperty ontologyProperty)
            => rangeClass != null && ontologyProperty != null && classModel != null && classModel.GetRangeOf(ontologyProperty).Classes.ContainsKey(rangeClass.PatternMemberID);

        /// <summary>
        /// Enlists the range classes of the given property within the given ontology class model
        /// </summary>
        public static RDFOntologyClassModel GetRangeOf(this RDFOntologyClassModel classModel, RDFOntologyProperty ontProperty)
        {
            RDFOntologyClassModel result = new RDFOntologyClassModel();

            if (ontProperty != null && classModel != null && ontProperty.Range != null)
                result = classModel.GetSubClassesOf(ontProperty.Range)
                                   .UnionWith(classModel.GetEquivalentClassesOf(ontProperty.Range))
                                   .AddClass(ontProperty.Range);

            return result;
        }
        #endregion

        #region Literal
        /// <summary>
        /// Checks if the given ontology class is compatible with 'rdfs:Literal' within the given class model
        /// </summary>
        public static bool CheckIsLiteralCompatibleClass(this RDFOntologyClassModel classModel, RDFOntologyClass ontologyClass)
        {
            bool result = false;

            if (ontologyClass != null && classModel != null)
                result = ontologyClass.IsDataRangeClass()
                            || ontologyClass.Equals(RDFVocabulary.RDFS.LITERAL.ToRDFOntologyClass())
                                || classModel.CheckIsSubClassOf(ontologyClass, RDFVocabulary.RDFS.LITERAL.ToRDFOntologyClass());

            return result;
        }
        #endregion
    }
}