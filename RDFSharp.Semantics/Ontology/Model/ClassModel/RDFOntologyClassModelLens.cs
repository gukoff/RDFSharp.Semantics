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
using System.Linq;
using System.Threading.Tasks;

namespace RDFSharp.Semantics
{
    /// <summary>
    /// RDFOntologyClassModelLens represents a magnifying glass on the knowledge available for a class within an ontology
    /// </summary>
    public class RDFOntologyClassModelLens
    {
        #region Properties
        /// <summary>
        /// Class being observed by the class model lens
        /// </summary>
        public RDFOntologyClass OntologyClass { get; internal set; }

        /// <summary>
        /// Ontology being observed by the class model lens
        /// </summary>
        public RDFOntology Ontology { get; internal set; }
        #endregion

        #region Ctors
        /// <summary>
        /// Builds a class model lens for the given class on the given ontology
        /// </summary>
        public RDFOntologyClassModelLens(RDFOntologyClass ontologyClass, RDFOntology ontology)
        {
            if (ontologyClass == null)
                throw new RDFSemanticsException("Cannot create class model lens because given \"ontologyClass\" parameter is null");
            if (ontology == null)
                throw new RDFSemanticsException("Cannot create class model lens because given \"ontology\" parameter is null");

            this.OntologyClass = ontologyClass;
            this.Ontology = ontology.UnionWith(RDFBASEOntology.Instance);
        }
        #endregion

        #region Methods
        /// <summary>
        /// Enlists the classes which are directly (or indirectly, if inference is requested) children of the lens class
        /// </summary>
        public List<(bool, RDFOntologyClass)> SubClasses(bool enableInference)
        {
            List<(bool, RDFOntologyClass)> result = new List<(bool, RDFOntologyClass)>();

            //First-level enlisting of subclasses
            foreach (RDFOntologyTaxonomyEntry subClass in this.Ontology.Model.ClassModel.Relations.SubClassOf.SelectEntriesByObject(this.OntologyClass).Where(te => !te.IsInference()))
                result.Add((false, (RDFOntologyClass)subClass.TaxonomySubject));

            //Inference-enabled discovery of subclasses
            if (enableInference)
            {
                List<RDFOntologyClass> subClasses = RDFOntologyClassModelHelper.GetSubClassesOf(this.Ontology.Model.ClassModel, this.OntologyClass).ToList();
                foreach (RDFOntologyClass subClass in subClasses)
                {
                    if (!result.Any(c => c.Item2.Equals(subClass)))
                        result.Add((true, subClass));
                }
            }

            return result;
        }

        /// <summary>
        /// Asynchronously enlists the classes which are directly (or indirectly, if inference is requested) children of the lens class
        /// </summary>
        public Task<List<(bool, RDFOntologyClass)>> SubClassesAsync(bool enableInference)
            => Task.Run(() => SubClasses(enableInference));

        /// <summary>
        /// Enlists the classes which are directly (or indirectly, if inference is requested) parents of the lens class
        /// </summary>
        public List<(bool, RDFOntologyClass)> SuperClasses(bool enableInference)
        {
            List<(bool, RDFOntologyClass)> result = new List<(bool, RDFOntologyClass)>();

            //First-level enlisting of superclasses
            foreach (RDFOntologyTaxonomyEntry subClass in this.Ontology.Model.ClassModel.Relations.SubClassOf.SelectEntriesBySubject(this.OntologyClass).Where(te => !te.IsInference()))
                result.Add((false, (RDFOntologyClass)subClass.TaxonomyObject));

            //Inference-enabled discovery of superclasses
            if (enableInference)
            {
                List<RDFOntologyClass> superClasses = RDFOntologyClassModelHelper.GetSuperClassesOf(this.Ontology.Model.ClassModel, this.OntologyClass).ToList();
                foreach (RDFOntologyClass superClass in superClasses)
                {
                    if (!result.Any(c => c.Item2.Equals(superClass)))
                        result.Add((true, superClass));
                }
            }

            return result;
        }

        /// <summary>
        /// Asynchronously enlists the classes which are directly (or indirectly, if inference is requested) parents of the lens class
        /// </summary>
        public Task<List<(bool, RDFOntologyClass)>> SuperClassesAsync(bool enableInference)
            => Task.Run(() => SuperClasses(enableInference));

        /// <summary>
        /// Enlists the classes which are directly (or indirectly, if inference is requested) equivalent to the lens class
        /// </summary>
        public List<(bool, RDFOntologyClass)> EquivalentClasses(bool enableInference)
        {
            List<(bool, RDFOntologyClass)> result = new List<(bool, RDFOntologyClass)>();

            //First-level enlisting of equivalent classes
            foreach (RDFOntologyTaxonomyEntry equivalentClass in this.Ontology.Model.ClassModel.Relations.EquivalentClass.SelectEntriesBySubject(this.OntologyClass).Where(te => !te.IsInference()))
                result.Add((false, (RDFOntologyClass)equivalentClass.TaxonomyObject));

            //Inference-enabled discovery of equivalent classes
            if (enableInference)
            {
                List<RDFOntologyClass> equivalentClasses = RDFOntologyClassModelHelper.GetEquivalentClassesOf(this.Ontology.Model.ClassModel, this.OntologyClass).ToList();
                foreach (RDFOntologyClass equivalentClass in equivalentClasses)
                {
                    if (!result.Any(c => c.Item2.Equals(equivalentClass)))
                        result.Add((true, equivalentClass));
                }
            }

            return result;
        }

        /// <summary>
        /// Asynchronously enlists the classes which are directly (or indirectly, if inference is requested) equivalent to the lens class
        /// </summary>
        public Task<List<(bool, RDFOntologyClass)>> EquivalentClassesAsync(bool enableInference)
            => Task.Run(() => EquivalentClasses(enableInference));

        /// <summary>
        /// Enlists the classes which are directly (or indirectly, if inference is requested) disjoint from the lens class
        /// </summary>
        public List<(bool, RDFOntologyClass)> DisjointClasses(bool enableInference)
        {
            List<(bool, RDFOntologyClass)> result = new List<(bool, RDFOntologyClass)>();

            //First-level enlisting of disjoint classes
            foreach (RDFOntologyTaxonomyEntry disjointClass in this.Ontology.Model.ClassModel.Relations.DisjointWith.SelectEntriesBySubject(this.OntologyClass).Where(te => !te.IsInference()))
                result.Add((false, (RDFOntologyClass)disjointClass.TaxonomyObject));

            //Inference-enabled discovery of disjoint classes
            if (enableInference)
            {
                List<RDFOntologyClass> disjointClasses = RDFOntologyClassModelHelper.GetDisjointClassesWith(this.Ontology.Model.ClassModel, this.OntologyClass).ToList();
                foreach (RDFOntologyClass disjointClass in disjointClasses)
                {
                    if (!result.Any(c => c.Item2.Equals(disjointClass)))
                        result.Add((true, disjointClass));
                }
            }

            return result;
        }

        /// <summary>
        /// Asynchronously enlists the classes which are directly (or indirectly, if inference is requested) disjoint from the lens class
        /// </summary>
        public Task<List<(bool, RDFOntologyClass)>> DisjointClassesAsync(bool enableInference)
            => Task.Run(() => DisjointClasses(enableInference));

        /// <summary>
        /// Enlists the properties which are keys of the lens class
        /// </summary>
        public List<(bool, RDFOntologyProperty)> Keys()
        {
            List<(bool, RDFOntologyProperty)> result = new List<(bool, RDFOntologyProperty)>();

            //First-level enlisting of keys (reasoning is not available on this concept)
            foreach (RDFOntologyTaxonomyEntry hasKey in this.Ontology.Model.ClassModel.Relations.HasKey.SelectEntriesBySubject(this.OntologyClass).Where(te => !te.IsInference()))
                result.Add((false, (RDFOntologyProperty)hasKey.TaxonomyObject));

            return result;
        }

        /// <summary>
        /// Asynchronously enlists the properties which are keys of the lens class
        /// </summary>
        public Task<List<(bool, RDFOntologyProperty)>> KeysAsync()
            => Task.Run(() => Keys());

        /// <summary>
        /// Enlists the individuals/literals which are directly (or indirectly, if inference is requested) members of the lens class
        /// </summary>
        public List<(bool, RDFOntologyResource)> Members(bool enableInference)
        {
            List<(bool, RDFOntologyResource)> result = new List<(bool, RDFOntologyResource)>();

            //First-level enlisting of members (datatype class)
            if (RDFOntologyClassModelHelper.CheckIsLiteralCompatibleClass(this.Ontology.Model.ClassModel, this.OntologyClass))
            {
                IEnumerable<RDFOntologyLiteral> literals = this.Ontology.Data.Literals.Values.Where(ol => ol.Value is RDFTypedLiteral);
                foreach (RDFOntologyLiteral literal in literals.Where(ol => RDFModelUtilities.GetDatatypeFromEnum(((RDFTypedLiteral)ol.Value).Datatype).Equals(this.OntologyClass.ToString())))
                {
                    if (!result.Any(ol => ol.Equals(literal)))
                        result.Add((false, literal));
                }
            }
            //First-level enlisting of members (object class)
            else
            {
                foreach (RDFOntologyTaxonomyEntry classType in this.Ontology.Data.Relations.ClassType.SelectEntriesByObject(this.OntologyClass).Where(te => !te.IsInference()))
                    result.Add((false, classType.TaxonomySubject));
            }

            //Inference-enabled discovery of members
            if (enableInference)
            {
                RDFOntologyData members = RDFOntologyDataHelper.GetMembersOf(this.Ontology, this.OntologyClass);
                foreach (RDFOntologyIndividual individual in members)
                {
                    if (!result.Any(r => r.Item2.Equals(individual)))
                        result.Add((true, individual));
                }
                foreach (RDFOntologyLiteral literal in members.Literals.Values)
                {
                    if (!result.Any(r => r.Item2.Equals(literal)))
                        result.Add((true, literal));
                }
            }

            return result;
        }

        /// <summary>
        /// Asynchronously enlists the individuals/literals which are directly (or indirectly, if inference is requested) members of the lens class
        /// </summary>
        public Task<List<(bool, RDFOntologyResource)>> MembersAsync(bool enableInference)
            => Task.Run(() => Members(enableInference));

        /// <summary>
        /// Enlists the object annotations which are assigned to the lens class
        /// </summary>
        public List<(RDFOntologyAnnotationProperty, RDFOntologyResource)> ObjectAnnotations()
        {
            List<(RDFOntologyAnnotationProperty, RDFOntologyResource)> result = new List<(RDFOntologyAnnotationProperty, RDFOntologyResource)>();

            //SeeAlso
            foreach (RDFOntologyTaxonomyEntry seeAlso in this.Ontology.Model.ClassModel.Annotations.SeeAlso.SelectEntriesBySubject(this.OntologyClass).Where(te => !te.TaxonomyObject.IsLiteral()))
                result.Add(((RDFOntologyAnnotationProperty)seeAlso.TaxonomyPredicate, seeAlso.TaxonomyObject));

            //IsDefinedBy
            foreach (RDFOntologyTaxonomyEntry isDefinedBy in this.Ontology.Model.ClassModel.Annotations.IsDefinedBy.SelectEntriesBySubject(this.OntologyClass).Where(te => !te.TaxonomyObject.IsLiteral()))
                result.Add(((RDFOntologyAnnotationProperty)isDefinedBy.TaxonomyPredicate, isDefinedBy.TaxonomyObject));

            //Custom Annotations
            foreach (RDFOntologyTaxonomyEntry customAnnotation in this.Ontology.Model.ClassModel.Annotations.CustomAnnotations.SelectEntriesBySubject(this.OntologyClass).Where(te => !te.TaxonomyObject.IsLiteral()))
                result.Add(((RDFOntologyAnnotationProperty)customAnnotation.TaxonomyPredicate, customAnnotation.TaxonomyObject));

            return result;
        }

        /// <summary>
        /// Asynchronously enlists the object annotations which are assigned to the lens class
        /// </summary>
        public Task<List<(RDFOntologyAnnotationProperty, RDFOntologyResource)>> ObjectAnnotationsAsync()
            => Task.Run(() => ObjectAnnotations());

        /// <summary>
        /// Enlists the literal annotations which are assigned to the lens class
        /// </summary>
        public List<(RDFOntologyAnnotationProperty, RDFOntologyLiteral)> DataAnnotations()
        {
            List<(RDFOntologyAnnotationProperty, RDFOntologyLiteral)> result = new List<(RDFOntologyAnnotationProperty, RDFOntologyLiteral)>();

            //VersionInfo
            foreach (RDFOntologyTaxonomyEntry versionInfo in this.Ontology.Model.ClassModel.Annotations.VersionInfo.SelectEntriesBySubject(this.OntologyClass).Where(te => te.TaxonomyObject.IsLiteral()))
                result.Add(((RDFOntologyAnnotationProperty)versionInfo.TaxonomyPredicate, (RDFOntologyLiteral)versionInfo.TaxonomyObject));

            //Comment
            foreach (RDFOntologyTaxonomyEntry comment in this.Ontology.Model.ClassModel.Annotations.Comment.SelectEntriesBySubject(this.OntologyClass).Where(te => te.TaxonomyObject.IsLiteral()))
                result.Add(((RDFOntologyAnnotationProperty)comment.TaxonomyPredicate, (RDFOntologyLiteral)comment.TaxonomyObject));

            //Label
            foreach (RDFOntologyTaxonomyEntry label in this.Ontology.Model.ClassModel.Annotations.Label.SelectEntriesBySubject(this.OntologyClass).Where(te => te.TaxonomyObject.IsLiteral()))
                result.Add(((RDFOntologyAnnotationProperty)label.TaxonomyPredicate, (RDFOntologyLiteral)label.TaxonomyObject));

            //SeeAlso
            foreach (RDFOntologyTaxonomyEntry seeAlso in this.Ontology.Model.ClassModel.Annotations.SeeAlso.SelectEntriesBySubject(this.OntologyClass).Where(te => te.TaxonomyObject.IsLiteral()))
                result.Add(((RDFOntologyAnnotationProperty)seeAlso.TaxonomyPredicate, (RDFOntologyLiteral)seeAlso.TaxonomyObject));

            //IsDefinedBy
            foreach (RDFOntologyTaxonomyEntry isDefinedBy in this.Ontology.Model.ClassModel.Annotations.IsDefinedBy.SelectEntriesBySubject(this.OntologyClass).Where(te => te.TaxonomyObject.IsLiteral()))
                result.Add(((RDFOntologyAnnotationProperty)isDefinedBy.TaxonomyPredicate, (RDFOntologyLiteral)isDefinedBy.TaxonomyObject));

            //Custom Annotations
            foreach (RDFOntologyTaxonomyEntry customAnnotation in this.Ontology.Model.ClassModel.Annotations.CustomAnnotations.SelectEntriesBySubject(this.OntologyClass).Where(te => te.TaxonomyObject.IsLiteral()))
                result.Add(((RDFOntologyAnnotationProperty)customAnnotation.TaxonomyPredicate, (RDFOntologyLiteral)customAnnotation.TaxonomyObject));

            return result;
        }

        /// <summary>
        /// Asynchronously enlists the literal annotations which are assigned to the lens class
        /// </summary>
        public Task<List<(RDFOntologyAnnotationProperty, RDFOntologyLiteral)>> DataAnnotationsAsync()
            => Task.Run(() => DataAnnotations());
        #endregion
    }
}