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
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace RDFSharp.Semantics
{
    /// <summary>
    /// RDFOntologyDataLens represents a magnifying glass on the knowledge available for a individual within an ontology
    /// </summary>
    public class RDFOntologyDataLens
    {
        #region Properties
        /// <summary>
        /// Individual being observed by the data lens
        /// </summary>
        public RDFOntologyIndividual OntologyIndividual { get; internal set; }

        /// <summary>
        /// Ontology being observed by the data lens
        /// </summary>
        public RDFOntology Ontology { get; internal set; }
        #endregion

        #region Ctors
        /// <summary>
        /// Builds a data lens for the given individual on the given ontology
        /// </summary>
        public RDFOntologyDataLens(RDFOntologyIndividual ontologyIndividual, RDFOntology ontology)
        {
            if (ontologyIndividual == null)
                throw new RDFSemanticsException("Cannot create data lens because given \"ontologyIndividual\" parameter is null");
            if (ontology == null)
                throw new RDFSemanticsException("Cannot create data lens because given \"ontology\" parameter is null");

            this.OntologyIndividual = ontologyIndividual;
            this.Ontology = ontology.UnionWith(RDFBASEOntology.Instance);
        }
        #endregion

        #region Methods
        /// <summary>
        /// Enlists the individuals which are directly (or indirectly, if inference is requested) equivalent to the lens individual
        /// </summary>
        public List<(bool, RDFOntologyIndividual)> SameIndividuals(bool enableInference)
        {
            List<(bool, RDFOntologyIndividual)> result = new List<(bool, RDFOntologyIndividual)>();

            //First-level enlisting of same individuals
            foreach (RDFOntologyTaxonomyEntry sameAs in this.Ontology.Data.Relations.SameAs.SelectEntriesBySubject(this.OntologyIndividual).Where(te => !te.IsInference()))
                result.Add((false, (RDFOntologyIndividual)sameAs.TaxonomyObject));

            //Inference-enabled discovery of same individuals
            if (enableInference)
            {
                List<RDFOntologyIndividual> sameIndividuals = RDFOntologyDataHelper.GetSameIndividuals(this.Ontology.Data, this.OntologyIndividual).ToList();
                foreach (RDFOntologyIndividual sameIndividual in sameIndividuals)
                {
                    if (!result.Any(r => r.Item2.Equals(sameIndividual)))
                        result.Add((true, sameIndividual));
                }
            }

            return result;
        }

        /// <summary>
        /// Asynchronously enlists the individuals which are directly (or indirectly, if inference is requested) equivalent to the lens individual
        /// </summary>
        public Task<List<(bool, RDFOntologyIndividual)>> SameIndividualsAsync(bool enableInference)
            => Task.Run(() => SameIndividuals(enableInference));

        /// <summary>
        /// Enlists the individuals which are directly (or indirectly, if inference is requested) different from the lens individual
        /// </summary>
        public List<(bool, RDFOntologyIndividual)> DifferentIndividuals(bool enableInference)
        {
            List<(bool, RDFOntologyIndividual)> result = new List<(bool, RDFOntologyIndividual)>();

            //First-level enlisting of different individuals
            foreach (RDFOntologyTaxonomyEntry differentFrom in this.Ontology.Data.Relations.DifferentFrom.SelectEntriesBySubject(this.OntologyIndividual).Where(te => !te.IsInference()))
                result.Add((false, (RDFOntologyIndividual)differentFrom.TaxonomyObject));

            //Inference-enabled discovery of different individuals
            if (enableInference)
            {
                List<RDFOntologyIndividual> differentIndividuals = RDFOntologyDataHelper.GetDifferentIndividuals(this.Ontology.Data, this.OntologyIndividual).ToList();
                foreach (RDFOntologyIndividual differentIndividual in differentIndividuals)
                {
                    if (!result.Any(r => r.Item2.Equals(differentIndividual)))
                        result.Add((true, differentIndividual));
                }
            }

            return result;
        }

        /// <summary>
        /// Asynchronously enlists the individuals which are directly (or indirectly, if inference is requested) equivalent to the lens individual
        /// </summary>
        public Task<List<(bool, RDFOntologyIndividual)>> DifferentIndividualsAsync(bool enableInference)
            => Task.Run(() => DifferentIndividuals(enableInference));

        /// <summary>
        /// Enlists the classes to which the lens individual directly (or indirectly, if inference is requested) belongs
        /// </summary>
        public List<(bool, RDFOntologyClass)> ClassTypes(bool enableInference)
        {
            List<(bool, RDFOntologyClass)> result = new List<(bool, RDFOntologyClass)>();

            //First-level enlisting of class types
            foreach (RDFOntologyTaxonomyEntry classType in this.Ontology.Data.Relations.ClassType.SelectEntriesBySubject(this.OntologyIndividual).Where(te => !te.IsInference()))
                result.Add((false, (RDFOntologyClass)classType.TaxonomyObject));

            //Inference-enabled discovery of class types
            if (enableInference)
            {
                //Skip already enlisted classes and also reserved/literal-compatible classes
                var availableClasses = this.Ontology.Model.ClassModel.Where(cls => !result.Any(res => res.Item2.Equals(cls))
                                                                                                        && !RDFOntologyChecker.CheckReservedClass(cls)
                                                                                                            && !RDFOntologyClassModelHelper.CheckIsLiteralCompatibleClass(this.Ontology.Model.ClassModel, cls));
                var membersCache = new Dictionary<long, RDFOntologyData>();

                //Evaluate enumerations
                foreach (RDFOntologyClass enumerateClass in availableClasses.Where(cls => cls.IsEnumerateClass()))
                {
                    if (!membersCache.ContainsKey(enumerateClass.PatternMemberID))
                        membersCache.Add(enumerateClass.PatternMemberID, this.Ontology.GetMembersOfEnumerate((RDFOntologyEnumerateClass)enumerateClass));

                    if (membersCache[enumerateClass.PatternMemberID].Individuals.ContainsKey(this.OntologyIndividual.PatternMemberID))
                        result.Add((true, enumerateClass));
                }

                //Evaluate restrictions
                foreach (RDFOntologyClass restrictionClass in availableClasses.Where(cls => cls.IsRestrictionClass()))
                {
                    if (!membersCache.ContainsKey(restrictionClass.PatternMemberID))
                        membersCache.Add(restrictionClass.PatternMemberID, this.Ontology.GetMembersOfRestriction((RDFOntologyRestriction)restrictionClass));

                    if (membersCache[restrictionClass.PatternMemberID].Individuals.ContainsKey(this.OntologyIndividual.PatternMemberID))
                        result.Add((true, restrictionClass));
                }

                //Evaluate simple classes
                foreach (RDFOntologyClass simpleClass in availableClasses.Where(cls => cls.IsSimpleClass()))
                {
                    if (!membersCache.ContainsKey(simpleClass.PatternMemberID))
                        membersCache.Add(simpleClass.PatternMemberID, this.Ontology.GetMembersOfClass(simpleClass));

                    if (membersCache[simpleClass.PatternMemberID].Individuals.ContainsKey(this.OntologyIndividual.PatternMemberID))
                        result.Add((true, simpleClass));
                }

                //Evaluate composite classes
                foreach (RDFOntologyClass compositeClass in availableClasses.Where(cls => cls.IsCompositeClass()))
                {
                    if (!membersCache.ContainsKey(compositeClass.PatternMemberID))
                        membersCache.Add(compositeClass.PatternMemberID, this.Ontology.GetMembersOfComposite(compositeClass, membersCache));

                    if (membersCache[compositeClass.PatternMemberID].Individuals.ContainsKey(this.OntologyIndividual.PatternMemberID))
                        result.Add((true, compositeClass));
                }
            }

            return result;
        }

        /// <summary>
        /// Asynchronously enlists the classes to which the lens individual directly (or indirectly, if inference is requested) belongs
        /// </summary>
        public Task<List<(bool, RDFOntologyClass)>> ClassTypesAsync(bool enableInference)
            => Task.Run(() => ClassTypes(enableInference));

        /// <summary>
        /// Enlists the object assertions which are directly (or indirectly, if inference is requested) assigned to the lens individual
        /// </summary>
        public List<(bool, RDFOntologyIndividual, RDFOntologyObjectProperty, RDFOntologyIndividual)> ObjectAssertions(bool enableInference)
        {
            List<(bool, RDFOntologyIndividual, RDFOntologyObjectProperty, RDFOntologyIndividual)> result = new List<(bool, RDFOntologyIndividual, RDFOntologyObjectProperty, RDFOntologyIndividual)>();

            //Subject
            RDFOntologyTaxonomy assertionsWithSubjectIndividual = this.Ontology.Data.Relations.Assertions.SelectEntriesBySubject(this.OntologyIndividual);
            if (enableInference)
            {
                //Inference-enabled discovery of assigned object relations
                foreach (RDFOntologyTaxonomyEntry asn in assertionsWithSubjectIndividual.Where(te => te.TaxonomyPredicate.IsObjectProperty()))
                    result.Add((asn.IsInference(), this.OntologyIndividual, (RDFOntologyObjectProperty)asn.TaxonomyPredicate, (RDFOntologyIndividual)asn.TaxonomyObject));
            }
            else
            {
                //First-level enlisting of assigned object relations
                foreach (RDFOntologyTaxonomyEntry asn in assertionsWithSubjectIndividual.Where(te => te.TaxonomyPredicate.IsObjectProperty() && !te.IsInference()))
                    result.Add((false, this.OntologyIndividual, (RDFOntologyObjectProperty)asn.TaxonomyPredicate, (RDFOntologyIndividual)asn.TaxonomyObject));
            }

            //Object
            RDFOntologyTaxonomy assertionsWithObjectIndividual = this.Ontology.Data.Relations.Assertions.SelectEntriesByObject(this.OntologyIndividual);
            if (enableInference)
            {
                //Inference-enabled discovery of assigned object relations
                foreach (RDFOntologyTaxonomyEntry asn in assertionsWithObjectIndividual.Where(te => te.TaxonomyPredicate.IsObjectProperty()))
                    result.Add((asn.IsInference(), (RDFOntologyIndividual)asn.TaxonomySubject, (RDFOntologyObjectProperty)asn.TaxonomyPredicate, this.OntologyIndividual));
            }
            else
            {
                //First-level enlisting of assigned object relations
                foreach (RDFOntologyTaxonomyEntry asn in assertionsWithObjectIndividual.Where(te => te.TaxonomyPredicate.IsObjectProperty() && !te.IsInference()))
                    result.Add((false, (RDFOntologyIndividual)asn.TaxonomySubject, (RDFOntologyObjectProperty)asn.TaxonomyPredicate, this.OntologyIndividual));
            }

            return result;
        }

        /// <summary>
        /// Asynchronously enlists the object assertions which are directly (or indirectly, if inference is requested) assigned to the lens individual
        /// </summary>
        public Task<List<(bool, RDFOntologyIndividual, RDFOntologyObjectProperty, RDFOntologyIndividual)>> ObjectAssertionsAsync(bool enableInference)
            => Task.Run(() => ObjectAssertions(enableInference));

        /// <summary>
        /// Enlists the negative object assertions which are directly (or indirectly, if inference is requested) assigned to the lens individual
        /// </summary>
        public List<(bool, RDFOntologyIndividual, RDFOntologyObjectProperty, RDFOntologyIndividual)> NegativeObjectAssertions(bool enableInference)
        {
            List<(bool, RDFOntologyIndividual, RDFOntologyObjectProperty, RDFOntologyIndividual)> result = new List<(bool, RDFOntologyIndividual, RDFOntologyObjectProperty, RDFOntologyIndividual)>();

            //Subject
            RDFOntologyTaxonomy negativeAssertionsWithSubjectIndividual = this.Ontology.Data.Relations.NegativeAssertions.SelectEntriesBySubject(this.OntologyIndividual);
            if (enableInference)
            {
                //Inference-enabled discovery of assigned object negative relations
                foreach (RDFOntologyTaxonomyEntry nasn in negativeAssertionsWithSubjectIndividual.Where(te => te.TaxonomyPredicate.IsObjectProperty()))
                    result.Add((nasn.IsInference(), this.OntologyIndividual, (RDFOntologyObjectProperty)nasn.TaxonomyPredicate, (RDFOntologyIndividual)nasn.TaxonomyObject));
            }
            else
            {
                //First-level enlisting of assigned object negative relations
                foreach (RDFOntologyTaxonomyEntry nasn in negativeAssertionsWithSubjectIndividual.Where(te => te.TaxonomyPredicate.IsObjectProperty() && !te.IsInference()))
                    result.Add((false, this.OntologyIndividual, (RDFOntologyObjectProperty)nasn.TaxonomyPredicate, (RDFOntologyIndividual)nasn.TaxonomyObject));
            }

            //Object
            RDFOntologyTaxonomy negativeAssertionsWithObjectIndividual = this.Ontology.Data.Relations.NegativeAssertions.SelectEntriesByObject(this.OntologyIndividual);
            if (enableInference)
            {
                //Inference-enabled discovery of assigned object relations
                foreach (RDFOntologyTaxonomyEntry nasn in negativeAssertionsWithObjectIndividual.Where(te => te.TaxonomyPredicate.IsObjectProperty()))
                    result.Add((nasn.IsInference(), (RDFOntologyIndividual)nasn.TaxonomySubject, (RDFOntologyObjectProperty)nasn.TaxonomyPredicate, this.OntologyIndividual));
            }
            else
            {
                //First-level enlisting of assigned object relations
                foreach (RDFOntologyTaxonomyEntry nasn in negativeAssertionsWithObjectIndividual.Where(te => te.TaxonomyPredicate.IsObjectProperty() && !te.IsInference()))
                    result.Add((false, (RDFOntologyIndividual)nasn.TaxonomySubject, (RDFOntologyObjectProperty)nasn.TaxonomyPredicate, this.OntologyIndividual));
            }

            return result;
        }

        /// <summary>
        /// Asynchronously enlists the negative object assertions which are directly (or indirectly, if inference is requested) assigned to the lens individual
        /// </summary>
        public Task<List<(bool, RDFOntologyIndividual, RDFOntologyObjectProperty, RDFOntologyIndividual)>> NegativeObjectAssertionsAsync(bool enableInference)
            => Task.Run(() => NegativeObjectAssertions(enableInference));

        /// <summary>
        /// Enlists the data assertions which are directly (or indirectly, if inference is requested) assigned to the lens individual
        /// </summary>
        public List<(bool,RDFOntologyDatatypeProperty, RDFOntologyLiteral)> DataAssertions(bool enableInference)
        {
            List<(bool, RDFOntologyDatatypeProperty, RDFOntologyLiteral)> result = new List<(bool, RDFOntologyDatatypeProperty, RDFOntologyLiteral)>();

            RDFOntologyTaxonomy assertionsWithSubjectIndividual = this.Ontology.Data.Relations.Assertions.SelectEntriesBySubject(this.OntologyIndividual);
            if (enableInference)
            {
                //Inference-enabled discovery of assigned literal relations
                foreach (RDFOntologyTaxonomyEntry asn in assertionsWithSubjectIndividual.Where(te => te.TaxonomyPredicate.IsDatatypeProperty()))
                    result.Add((asn.IsInference(), (RDFOntologyDatatypeProperty)asn.TaxonomyPredicate, (RDFOntologyLiteral)asn.TaxonomyObject));
            }
            else
            {
                //First-level enlisting of assigned literal relations
                foreach (RDFOntologyTaxonomyEntry asn in assertionsWithSubjectIndividual.Where(te => te.TaxonomyPredicate.IsDatatypeProperty() && !te.IsInference()))
                    result.Add((false, (RDFOntologyDatatypeProperty)asn.TaxonomyPredicate, (RDFOntologyLiteral)asn.TaxonomyObject));
            }

            return result;
        }

        /// <summary>
        /// Asynchronously enlists the data assertions which are directly (or indirectly, if inference is requested) assigned to the lens individual
        /// </summary>
        public Task<List<(bool, RDFOntologyDatatypeProperty, RDFOntologyLiteral)>> DataAssertionsAsync(bool enableInference)
            => Task.Run(() => DataAssertions(enableInference));

        /// <summary>
        /// Enlists the negative data assertions which are directly (or indirectly, if inference is requested) assigned to the lens individual
        /// </summary>
        public List<(bool, RDFOntologyDatatypeProperty, RDFOntologyLiteral)> NegativeDataAssertions(bool enableInference)
        {
            List<(bool, RDFOntologyDatatypeProperty, RDFOntologyLiteral)> result = new List<(bool, RDFOntologyDatatypeProperty, RDFOntologyLiteral)>();

            RDFOntologyTaxonomy negativeAssertionsWithSubjectIndividual = this.Ontology.Data.Relations.NegativeAssertions.SelectEntriesBySubject(this.OntologyIndividual);
            if (enableInference)
            {
                //Inference-enabled discovery of assigned literal negative relations
                foreach (RDFOntologyTaxonomyEntry nasn in negativeAssertionsWithSubjectIndividual.Where(te => te.TaxonomyPredicate.IsDatatypeProperty()))
                    result.Add((nasn.IsInference(), (RDFOntologyDatatypeProperty)nasn.TaxonomyPredicate, (RDFOntologyLiteral)nasn.TaxonomyObject));
            }
            else
            {
                //First-level enlisting of assigned literal negative relations
                foreach (RDFOntologyTaxonomyEntry nasn in negativeAssertionsWithSubjectIndividual.Where(te => te.TaxonomyPredicate.IsDatatypeProperty() && !te.IsInference()))
                    result.Add((false, (RDFOntologyDatatypeProperty)nasn.TaxonomyPredicate, (RDFOntologyLiteral)nasn.TaxonomyObject));
            }

            return result;
        }

        /// <summary>
        /// Asynchronously enlists the negative data assertions which are directly (or indirectly, if inference is requested) assigned to the lens individual
        /// </summary>
        public Task<List<(bool, RDFOntologyDatatypeProperty, RDFOntologyLiteral)>> NegativeDataAssertionsAsync(bool enableInference)
            => Task.Run(() => NegativeDataAssertions(enableInference));

        /// <summary>
        /// Enlists the object annotations which are assigned to the lens individual
        /// </summary>
        public List<(RDFOntologyAnnotationProperty, RDFOntologyIndividual)> ObjectAnnotations()
        {
            List<(RDFOntologyAnnotationProperty, RDFOntologyIndividual)> result = new List<(RDFOntologyAnnotationProperty, RDFOntologyIndividual)>();

            //SeeAlso
            foreach (RDFOntologyTaxonomyEntry ann in this.Ontology.Data.Annotations.SeeAlso.SelectEntriesBySubject(this.OntologyIndividual)
                                                                                           .Where(te => te.TaxonomyObject.IsIndividual()))
                result.Add(((RDFOntologyAnnotationProperty)ann.TaxonomyPredicate, (RDFOntologyIndividual)ann.TaxonomyObject));

            //IsDefinedBy
            foreach (RDFOntologyTaxonomyEntry ann in this.Ontology.Data.Annotations.IsDefinedBy.SelectEntriesBySubject(this.OntologyIndividual)
                                                                                               .Where(te => te.TaxonomyObject.IsIndividual()))
                result.Add(((RDFOntologyAnnotationProperty)ann.TaxonomyPredicate, (RDFOntologyIndividual)ann.TaxonomyObject));

            //Custom Annotations
            foreach (RDFOntologyTaxonomyEntry ann in this.Ontology.Data.Annotations.CustomAnnotations.SelectEntriesBySubject(this.OntologyIndividual)
                                                                                                     .Where(te => te.TaxonomyObject.IsIndividual()))
                result.Add(((RDFOntologyAnnotationProperty)ann.TaxonomyPredicate, (RDFOntologyIndividual)ann.TaxonomyObject));

            return result;
        }

        /// <summary>
        /// Asynchronously enlists the object annotations which are assigned to the lens individual
        /// </summary>
        public Task<List<(RDFOntologyAnnotationProperty, RDFOntologyIndividual)>> ObjectAnnotationsAsync()
            => Task.Run(() => ObjectAnnotations());

        /// <summary>
        /// Enlists the literal annotations which are assigned to the lens individual
        /// </summary>
        public List<(RDFOntologyAnnotationProperty, RDFOntologyLiteral)> DataAnnotations()
        {
            List<(RDFOntologyAnnotationProperty, RDFOntologyLiteral)> result = new List<(RDFOntologyAnnotationProperty, RDFOntologyLiteral)>();

            //VersionInfo
            foreach (RDFOntologyTaxonomyEntry ann in this.Ontology.Data.Annotations.VersionInfo.SelectEntriesBySubject(this.OntologyIndividual)
                                                                                               .Where(te => te.TaxonomyObject.IsLiteral()))
                result.Add(((RDFOntologyAnnotationProperty)ann.TaxonomyPredicate, (RDFOntologyLiteral)ann.TaxonomyObject));

            //Comment
            foreach (RDFOntologyTaxonomyEntry ann in this.Ontology.Data.Annotations.Comment.SelectEntriesBySubject(this.OntologyIndividual)
                                                                                           .Where(te => te.TaxonomyObject.IsLiteral()))
                result.Add(((RDFOntologyAnnotationProperty)ann.TaxonomyPredicate, (RDFOntologyLiteral)ann.TaxonomyObject));

            //Label
            foreach (RDFOntologyTaxonomyEntry ann in this.Ontology.Data.Annotations.Label.SelectEntriesBySubject(this.OntologyIndividual)
                                                                                         .Where(te => te.TaxonomyObject.IsLiteral()))
                result.Add(((RDFOntologyAnnotationProperty)ann.TaxonomyPredicate, (RDFOntologyLiteral)ann.TaxonomyObject));

            //SeeAlso
            foreach (RDFOntologyTaxonomyEntry ann in this.Ontology.Data.Annotations.SeeAlso.SelectEntriesBySubject(this.OntologyIndividual)
                                                                                           .Where(te => te.TaxonomyObject.IsLiteral()))
                result.Add(((RDFOntologyAnnotationProperty)ann.TaxonomyPredicate, (RDFOntologyLiteral)ann.TaxonomyObject));

            //IsDefinedBy
            foreach (RDFOntologyTaxonomyEntry ann in this.Ontology.Data.Annotations.IsDefinedBy.SelectEntriesBySubject(this.OntologyIndividual)
                                                                                               .Where(te => te.TaxonomyObject.IsLiteral()))
                result.Add(((RDFOntologyAnnotationProperty)ann.TaxonomyPredicate, (RDFOntologyLiteral)ann.TaxonomyObject));

            //Custom Annotations
            foreach (RDFOntologyTaxonomyEntry ann in this.Ontology.Data.Annotations.CustomAnnotations.SelectEntriesBySubject(this.OntologyIndividual)
                                                                                                     .Where(te => te.TaxonomyObject.IsLiteral()))
                result.Add(((RDFOntologyAnnotationProperty)ann.TaxonomyPredicate, (RDFOntologyLiteral)ann.TaxonomyObject));

            return result;
        }

        /// <summary>
        /// Asynchronously enlists the literal annotations which are assigned to the lens individual
        /// </summary>
        public Task<List<(RDFOntologyAnnotationProperty, RDFOntologyLiteral)>> DataAnnotationsAsync()
            => Task.Run(() => DataAnnotations());
        #endregion
    }
}