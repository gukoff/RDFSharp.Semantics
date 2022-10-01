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

using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using RDFSharp.Model;

namespace RDFSharp.Semantics
{
    /// <summary>
    /// OWLOntologyClassModel represents the T-BOX of application domain entities (classes)
    /// </summary>
    public class OWLOntologyClassModel : IEnumerable<RDFResource>
    {
        #region Properties
        /// <summary>
        /// Count of the classes
        /// </summary>
        public long ClassesCount
            => Classes.Count;

        /// <summary>
        /// Count of the deprecated classes
        /// </summary>
        public long DeprecatedClassesCount
        {
            get
            {
                long count = 0;
                IEnumerator<RDFResource> deprecatedClasses = DeprecatedClassesEnumerator;
                while (deprecatedClasses.MoveNext())
                    count++;
                return count;
            }
        }

        /// <summary>
        /// Count of the restrictions
        /// </summary>
        public long RestrictionsCount
        { 
            get
            {
                long count = 0;
                IEnumerator<RDFResource> restrictions = RestrictionsEnumerator;
                while (restrictions.MoveNext())
                    count++;
                return count;
            }
        }

        /// <summary>
        /// Count of the enumerate classes
        /// </summary>
        public long EnumeratesCount
        {
            get
            {
                long count = 0;
                IEnumerator<RDFResource> enumerates = EnumeratesEnumerator;
                while (enumerates.MoveNext())
                    count++;
                return count;
            }
        }

        /// <summary>
        /// Count of the composite classes
        /// </summary>
        public long CompositesCount
        {
            get
            {
                long count = 0;
                IEnumerator<RDFResource> composites = CompositesEnumerator;
                while (composites.MoveNext())
                    count++;
                return count;
            }
        }

        /// <summary>
        /// Count of the owl:AllDisjointClasses [OWL2]
        /// </summary>
        public long AllDisjointClassesCount
        {
            get
            {
                long count = 0;
                IEnumerator<RDFResource> allDisjointClasses = AllDisjointClassesEnumerator;
                while (allDisjointClasses.MoveNext())
                    count++;
                return count;
            }
        }

        /// <summary>
        /// Gets the enumerator on the classes for iteration
        /// </summary>
        public IEnumerator<RDFResource> ClassesEnumerator
            => Classes.Values.GetEnumerator();

        /// <summary>
        /// Gets the enumerator on the deprecated classes for iteration
        /// </summary>
        public IEnumerator<RDFResource> DeprecatedClassesEnumerator
        {
            get
            {
                IEnumerator<RDFResource> classes = ClassesEnumerator;
                while (classes.MoveNext())
                {
                    if (TBoxGraph[classes.Current, RDFVocabulary.RDF.TYPE, RDFVocabulary.OWL.DEPRECATED_CLASS, null].Any())
                        yield return classes.Current;
                }
            }
        }

        /// <summary>
        /// Gets the enumerator on the restrictions for iteration
        /// </summary>
        public IEnumerator<RDFResource> RestrictionsEnumerator
        { 
            get
            {
                IEnumerator<RDFResource> classes = ClassesEnumerator;
                while (classes.MoveNext())
                {
                    if (TBoxGraph[classes.Current, RDFVocabulary.RDF.TYPE, RDFVocabulary.OWL.RESTRICTION, null].Any())
                        yield return classes.Current;
                }
            }
        }

        /// <summary>
        /// Gets the enumerator on the enumerate classes for iteration
        /// </summary>
        public IEnumerator<RDFResource> EnumeratesEnumerator
        {
            get
            {
                IEnumerator<RDFResource> classes = ClassesEnumerator;
                while (classes.MoveNext())
                {
                    if (TBoxGraph[classes.Current, RDFVocabulary.OWL.ONE_OF, null, null].Any())
                        yield return classes.Current;
                }
            }
        }

        /// <summary>
        /// Gets the enumerator on the composite classes for iteration
        /// </summary>
        public IEnumerator<RDFResource> CompositesEnumerator
        {
            get
            {
                IEnumerator<RDFResource> classes = ClassesEnumerator;
                while (classes.MoveNext())
                {
                    if (TBoxGraph.Any(t => t.Subject.Equals(classes.Current) &&
                                                (t.Predicate.Equals(RDFVocabulary.OWL.UNION_OF) || 
                                                    t.Predicate.Equals(RDFVocabulary.OWL.INTERSECTION_OF) ||
                                                        t.Predicate.Equals(RDFVocabulary.OWL.COMPLEMENT_OF))))
                        yield return classes.Current;
                }
            }
        }

        /// <summary>
        /// Gets the enumerator on the owl:AllDisjointClasses for iteration [OWL2]
        /// </summary>
        public IEnumerator<RDFResource> AllDisjointClassesEnumerator
            => TBoxGraph[null, RDFVocabulary.RDF.TYPE, RDFVocabulary.OWL.ALL_DISJOINT_CLASSES, null]
                .Select(t => (RDFResource)t.Subject)
                .GetEnumerator();

        /// <summary>
        /// Collection of classes
        /// </summary>
        internal Dictionary<long, RDFResource> Classes { get; set; }

        /// <summary>
        /// T-BOX knowledge describing classes
        /// </summary>
        internal RDFGraph TBoxGraph { get; set; }
        #endregion

        #region Ctors
        /// <summary>
        /// Builds an empty class model
        /// </summary>
        public OWLOntologyClassModel()
        {
            Classes = new Dictionary<long, RDFResource>();
            TBoxGraph = new RDFGraph();
        }
        #endregion

        #region Interfaces
        /// <summary>
        /// Exposes a typed enumerator on the classes for iteration
        /// </summary>
        IEnumerator<RDFResource> IEnumerable<RDFResource>.GetEnumerator()
            => ClassesEnumerator;

        /// <summary>
        /// Exposes an untyped enumerator on the classes for iteration
        /// </summary>
        IEnumerator IEnumerable.GetEnumerator()
            => ClassesEnumerator;
        #endregion

        #region Methods
        /// <summary>
        /// Declares the existence of the given owl:Class to the model
        /// </summary>
        public OWLOntologyClassModel DeclareClass(RDFResource owlClass, OWLOntologyClassBehavior owlClassBehavior=null)
        {
            if (owlClass == null)
                throw new OWLSemanticsException("Cannot declare owl:Class to the model because given \"owlClass\" parameter is null");
            if (owlClassBehavior == null)
                owlClassBehavior = new OWLOntologyClassBehavior();

            if (!Classes.ContainsKey(owlClass.PatternMemberID))
                Classes.Add(owlClass.PatternMemberID, owlClass);

            //Add knowledge to the T-BOX
            TBoxGraph.AddTriple(new RDFTriple(owlClass, RDFVocabulary.RDF.TYPE, RDFVocabulary.OWL.CLASS));
            if (owlClassBehavior.Deprecated)
                TBoxGraph.AddTriple(new RDFTriple(owlClass, RDFVocabulary.RDF.TYPE, RDFVocabulary.OWL.DEPRECATED_CLASS));

            return this;
        }

        //RESTRICTIONS

        /// <summary>
        /// Declares the existence of the given owl:allValuesFrom restriction to the model
        /// </summary>
        public OWLOntologyClassModel DeclareAllValuesFromRestriction(RDFResource owlRestriction, RDFResource onProperty, RDFResource allValuesFromClass)
        {
            if (allValuesFromClass == null)
                throw new OWLSemanticsException("Cannot declare owl:allValuesFrom restriction to the model because given \"allValuesFromClass\" parameter is null");

            //Declare restriction to the model
            DeclareRestriction(owlRestriction, onProperty);

            //Add knowledge to the T-BOX
            TBoxGraph.AddTriple(new RDFTriple(owlRestriction, RDFVocabulary.OWL.ALL_VALUES_FROM, allValuesFromClass));

            return this;
        }

        /// <summary>
        /// Declares the existence of the given owl:someValuesFrom restriction to the model
        /// </summary>
        public OWLOntologyClassModel DeclareSomeValuesFromRestriction(RDFResource owlRestriction, RDFResource onProperty, RDFResource someValuesFromClass)
        {
            if (someValuesFromClass == null)
                throw new OWLSemanticsException("Cannot declare owl:someValuesFrom restriction to the model because given \"someValuesFromClass\" parameter is null");

            //Declare restriction to the model
            DeclareRestriction(owlRestriction, onProperty);

            //Add knowledge to the T-BOX
            TBoxGraph.AddTriple(new RDFTriple(owlRestriction, RDFVocabulary.OWL.SOME_VALUES_FROM, someValuesFromClass));

            return this;
        }

        /// <summary>
        /// Declares the existence of the given owl:hasSelf restriction to the model [OWL2]
        /// </summary>
        public OWLOntologyClassModel DeclareHasSelfRestriction(RDFResource owlRestriction, RDFResource onProperty, bool hasSelf)
        {
            //Declare restriction to the model
            DeclareRestriction(owlRestriction, onProperty);

            //Add knowledge to the T-BOX
            TBoxGraph.AddTriple(new RDFTriple(owlRestriction, RDFVocabulary.OWL.HAS_SELF, hasSelf ? RDFTypedLiteral.True : RDFTypedLiteral.False));

            return this;
        }

        /// <summary>
        /// Declares the existence of the given owl:hasValue restriction to the model
        /// </summary>
        public OWLOntologyClassModel DeclareHasValueRestriction(RDFResource owlRestriction, RDFResource onProperty, RDFResource value)
        {
            if (value == null)
                throw new OWLSemanticsException("Cannot declare owl:hasValue restriction to the model because given \"value\" parameter is null");

            //Declare restriction to the model
            DeclareRestriction(owlRestriction, onProperty);

            //Add knowledge to the T-BOX
            TBoxGraph.AddTriple(new RDFTriple(owlRestriction, RDFVocabulary.OWL.HAS_VALUE, value));

            return this;
        }

        /// <summary>
        /// Declares the existence of the given owl:hasValue restriction to the model
        /// </summary>
        public OWLOntologyClassModel DeclareHasValueRestriction(RDFResource owlRestriction, RDFResource onProperty, RDFLiteral value)
        {
            if (value == null)
                throw new OWLSemanticsException("Cannot declare owl:hasValue restriction to the model because given \"value\" parameter is null");

            //Declare restriction to the model
            DeclareRestriction(owlRestriction, onProperty);

            //Add knowledge to the T-BOX
            TBoxGraph.AddTriple(new RDFTriple(owlRestriction, RDFVocabulary.OWL.HAS_VALUE, value));

            return this;
        }

        /// <summary>
        /// Declares the existence of the given owl:cardinality restriction to the model
        /// </summary>
        public OWLOntologyClassModel DeclareCardinalityRestriction(RDFResource owlRestriction, RDFResource onProperty, uint cardinality)
        {
            if (cardinality == 0)
                throw new OWLSemanticsException("Cannot declare owl:cardinality restriction to the model because given \"cardinality\" value must be greater than zero");

            //Declare restriction to the model
            DeclareRestriction(owlRestriction, onProperty);

            //Add knowledge to the T-BOX
            TBoxGraph.AddTriple(new RDFTriple(owlRestriction, RDFVocabulary.OWL.CARDINALITY, new RDFTypedLiteral(cardinality.ToString(), RDFModelEnums.RDFDatatypes.XSD_NONNEGATIVEINTEGER)));

            return this;
        }

        /// <summary>
        /// Declares the existence of the given owl:minCardinality restriction to the model
        /// </summary>
        public OWLOntologyClassModel DeclareMinCardinalityRestriction(RDFResource owlRestriction, RDFResource onProperty, uint minCardinality)
        {
            if (minCardinality == 0)
                throw new OWLSemanticsException("Cannot declare owl:minCardinality restriction to the model because given \"minCardinality\" value must be greater than zero");

            //Declare restriction to the model
            DeclareRestriction(owlRestriction, onProperty);

            //Add knowledge to the T-BOX
            TBoxGraph.AddTriple(new RDFTriple(owlRestriction, RDFVocabulary.OWL.MIN_CARDINALITY, new RDFTypedLiteral(minCardinality.ToString(), RDFModelEnums.RDFDatatypes.XSD_NONNEGATIVEINTEGER)));

            return this;
        }

        /// <summary>
        /// Declares the existence of the given owl:maxCardinality restriction to the model
        /// </summary>
        public OWLOntologyClassModel DeclareMaxCardinalityRestriction(RDFResource owlRestriction, RDFResource onProperty, uint maxCardinality)
        {
            if (maxCardinality == 0)
                throw new OWLSemanticsException("Cannot declare owl:maxCardinality restriction to the model because given \"maxCardinality\" value must be greater than zero");

            //Declare restriction to the model
            DeclareRestriction(owlRestriction, onProperty);

            //Add knowledge to the T-BOX
            TBoxGraph.AddTriple(new RDFTriple(owlRestriction, RDFVocabulary.OWL.MAX_CARDINALITY, new RDFTypedLiteral(maxCardinality.ToString(), RDFModelEnums.RDFDatatypes.XSD_NONNEGATIVEINTEGER)));

            return this;
        }

        /// <summary>
        /// Declares the existence of the given owl:minCardinality and owl:maxCardinality restriction to the model
        /// </summary>
        public OWLOntologyClassModel DeclareMinMaxCardinalityRestriction(RDFResource owlRestriction, RDFResource onProperty, uint minCardinality, uint maxCardinality)
        {
            if (minCardinality == 0)
                throw new OWLSemanticsException("Cannot declare owl:minCardinality and owl:maxCardinality restriction to the model because given \"minCardinality\" value must be greater than zero");
            if (maxCardinality == 0)
                throw new OWLSemanticsException("Cannot declare owl:minCardinality and owl:maxCardinality restriction to the model because given \"maxCardinality\" value must be greater than zero");
            if (maxCardinality < minCardinality)
                throw new OWLSemanticsException("Cannot declare owl:minCardinality and owl:maxCardinality restriction to the model because given \"maxCardinality\" value must be greater or equal than given \"minCardinality\" value");

            //Declare restriction to the model
            DeclareRestriction(owlRestriction, onProperty);

            //Add knowledge to the T-BOX
            TBoxGraph.AddTriple(new RDFTriple(owlRestriction, RDFVocabulary.OWL.MIN_CARDINALITY, new RDFTypedLiteral(minCardinality.ToString(), RDFModelEnums.RDFDatatypes.XSD_NONNEGATIVEINTEGER)));
            TBoxGraph.AddTriple(new RDFTriple(owlRestriction, RDFVocabulary.OWL.MAX_CARDINALITY, new RDFTypedLiteral(maxCardinality.ToString(), RDFModelEnums.RDFDatatypes.XSD_NONNEGATIVEINTEGER)));

            return this;
        }

        /// <summary>
        /// Declares the existence of the given owl:qualifiedCardinality restriction to the model [OWL2]
        /// </summary>
        public OWLOntologyClassModel DeclareQualifiedCardinalityRestriction(RDFResource owlRestriction, RDFResource onProperty, uint cardinality, RDFResource onClass)
        {
            if (onClass == null)
                throw new OWLSemanticsException("Cannot declare owl:qualifiedCardinality restriction to the model because given \"onClass\" parameter is null");
            if (cardinality == 0)
                throw new OWLSemanticsException("Cannot declare owl:qualifiedCardinality restriction to the model because given \"cardinality\" value must be greater than zero");

            //Declare restriction to the model
            DeclareRestriction(owlRestriction, onProperty);

            //Add knowledge to the T-BOX
            TBoxGraph.AddTriple(new RDFTriple(owlRestriction, RDFVocabulary.OWL.QUALIFIED_CARDINALITY, new RDFTypedLiteral(cardinality.ToString(), RDFModelEnums.RDFDatatypes.XSD_NONNEGATIVEINTEGER)));
            TBoxGraph.AddTriple(new RDFTriple(owlRestriction, RDFVocabulary.OWL.ON_CLASS, onClass));

            return this;
        }

        /// <summary>
        /// Declares the existence of the given owl:minQUalifiedCardinality restriction to the model [OWL2]
        /// </summary>
        public OWLOntologyClassModel DeclareMinQualifiedCardinalityRestriction(RDFResource owlRestriction, RDFResource onProperty, uint minCardinality, RDFResource onClass)
        {
            if (onClass == null)
                throw new OWLSemanticsException("Cannot declare owl:minQualifiedCardinality restriction to the model because given \"onClass\" parameter is null");
            if (minCardinality == 0)
                throw new OWLSemanticsException("Cannot declare owl:minQualifiedCardinality restriction to the model because given \"minCardinality\" value must be greater than zero");

            //Declare restriction to the model
            DeclareRestriction(owlRestriction, onProperty);

            //Add knowledge to the T-BOX
            TBoxGraph.AddTriple(new RDFTriple(owlRestriction, RDFVocabulary.OWL.MIN_QUALIFIED_CARDINALITY, new RDFTypedLiteral(minCardinality.ToString(), RDFModelEnums.RDFDatatypes.XSD_NONNEGATIVEINTEGER)));
            TBoxGraph.AddTriple(new RDFTriple(owlRestriction, RDFVocabulary.OWL.ON_CLASS, onClass));

            return this;
        }

        /// <summary>
        /// Declares the existence of the given owl:maxQualifiedCardinality restriction to the model [OWL2]
        /// </summary>
        public OWLOntologyClassModel DeclareMaxQualifiedCardinalityRestriction(RDFResource owlRestriction, RDFResource onProperty, uint maxCardinality, RDFResource onClass)
        {
            if (onClass == null)
                throw new OWLSemanticsException("Cannot declare owl:maxQualifiedCardinality restriction to the model because given \"onClass\" parameter is null");
            if (maxCardinality == 0)
                throw new OWLSemanticsException("Cannot declare owl:maxQualifiedCardinality restriction to the model because given \"maxCardinality\" value must be greater than zero");

            //Declare restriction to the model
            DeclareRestriction(owlRestriction, onProperty);

            //Add knowledge to the T-BOX
            TBoxGraph.AddTriple(new RDFTriple(owlRestriction, RDFVocabulary.OWL.MAX_QUALIFIED_CARDINALITY, new RDFTypedLiteral(maxCardinality.ToString(), RDFModelEnums.RDFDatatypes.XSD_NONNEGATIVEINTEGER)));
            TBoxGraph.AddTriple(new RDFTriple(owlRestriction, RDFVocabulary.OWL.ON_CLASS, onClass));

            return this;
        }

        /// <summary>
        /// Declares the existence of the given owl:minQualifiedCardinality and owl:maxQualifiedCardinality restriction to the model, working on the given property [OWL2]
        /// </summary>
        public OWLOntologyClassModel DeclareMinMaxQualifiedCardinalityRestriction(RDFResource owlRestriction, RDFResource onProperty, uint minCardinality, uint maxCardinality, RDFResource onClass)
        {
            if (onClass == null)
                throw new OWLSemanticsException("Cannot declare owl:minQualifiedCardinality and owl:maxQualifiedCardinality restriction to the model because given \"onClass\" parameter is null");
            if (minCardinality == 0)
                throw new OWLSemanticsException("Cannot declare owl:minQualifiedCardinality and owl:maxQualifiedCardinality restriction to the model because given \"minCardinality\" value must be greater than zero");
            if (maxCardinality == 0)
                throw new OWLSemanticsException("Cannot declare owl:minQualifiedCardinality and owl:maxQualifiedCardinality restriction to the model because given \"maxCardinality\" value must be greater than zero");
            if (maxCardinality < minCardinality)
                throw new OWLSemanticsException("Cannot declare owl:minQualifiedCardinality and owl:maxQualifiedCardinality restriction to the model because given \"maxCardinality\" value must be greater or equal than given \"minCardinality\" value");

            //Declare restriction to the model
            DeclareRestriction(owlRestriction, onProperty);

            //Add knowledge to the T-BOX
            TBoxGraph.AddTriple(new RDFTriple(owlRestriction, RDFVocabulary.OWL.MIN_QUALIFIED_CARDINALITY, new RDFTypedLiteral(minCardinality.ToString(), RDFModelEnums.RDFDatatypes.XSD_NONNEGATIVEINTEGER)));
            TBoxGraph.AddTriple(new RDFTriple(owlRestriction, RDFVocabulary.OWL.MAX_QUALIFIED_CARDINALITY, new RDFTypedLiteral(maxCardinality.ToString(), RDFModelEnums.RDFDatatypes.XSD_NONNEGATIVEINTEGER)));
            TBoxGraph.AddTriple(new RDFTriple(owlRestriction, RDFVocabulary.OWL.ON_CLASS, onClass));

            return this;
        }
        
        //ENUMERATES

        /// <summary>
        /// Declares the existence of the given owl:oneOf enumerate class to the model
        /// </summary>
        public OWLOntologyClassModel DeclareEnumerateClass(RDFResource owlClass, List<RDFResource> individuals)
        {
            if (owlClass == null)
                throw new OWLSemanticsException("Cannot declare owl:oneOf class to the model because given \"owlClass\" parameter is null");
            if (individuals == null)
                throw new OWLSemanticsException("Cannot declare owl:oneOf class to the model because given \"individuals\" parameter is null");
            if (individuals.Count == 0)
                throw new OWLSemanticsException("Cannot declare owl:oneOf class to the model because given \"individuals\" parameter is an empty list");

            //Declare class to the model
            DeclareClass(owlClass);

            //Add knowledge to the T-BOX
            RDFCollection enumeratesCollection = new RDFCollection(RDFModelEnums.RDFItemTypes.Resource);
            individuals.ForEach(individual => enumeratesCollection.AddItem(individual));
            TBoxGraph.AddCollection(enumeratesCollection);
            TBoxGraph.AddTriple(new RDFTriple(owlClass, RDFVocabulary.OWL.ONE_OF, enumeratesCollection.ReificationSubject));

            return this;
        }

        //COMPOSITES

        /// <summary>
        /// Declares the existence of the given owl:unionOf class to the model
        /// </summary>
        public OWLOntologyClassModel DeclareUnionClass(RDFResource owlClass, List<RDFResource> unionClasses)
        {
            if (owlClass == null)
                throw new OWLSemanticsException("Cannot declare owl:unionOf class to the model because given \"owlClass\" parameter is null");
            if (unionClasses == null)
                throw new OWLSemanticsException("Cannot declare owl:unionOf class to the model because given \"unionClasses\" parameter is null");
            if (unionClasses.Count == 0)
                throw new OWLSemanticsException("Cannot declare owl:unionOf class to the model because given \"unionClasses\" parameter is an empty list");
            if (unionClasses.Any(cls => cls.Equals(owlClass)))
                throw new OWLSemanticsException("Cannot declare owl:unionOf class to the model because given \"unionClasses\" parameter contains given \"owlClass\" element, which is not allowed");

            //Add class to the model
            DeclareClass(owlClass);

            //Add knowledge to the T-BOX
            RDFCollection classesCollection = new RDFCollection(RDFModelEnums.RDFItemTypes.Resource);
            unionClasses.ForEach(cls => classesCollection.AddItem(cls));
            TBoxGraph.AddCollection(classesCollection);
            TBoxGraph.AddTriple(new RDFTriple(owlClass, RDFVocabulary.OWL.UNION_OF, classesCollection.ReificationSubject));

            return this;
        }

        /// <summary>
        /// Declares the existence of the given owl:intersectionOf class to the model
        /// </summary>
        public OWLOntologyClassModel DeclareIntersectionClass(RDFResource owlClass, List<RDFResource> intersectionClasses)
        {
            if (owlClass == null)
                throw new OWLSemanticsException("Cannot declare owl:intersectionOf class to the model because given \"owlClass\" parameter is null");
            if (intersectionClasses == null)
                throw new OWLSemanticsException("Cannot declare owl:intersectionOf class to the model because given \"intersectionClasses\" parameter is null");
            if (intersectionClasses.Count == 0)
                throw new OWLSemanticsException("Cannot declare owl:intersectionOf class to the model because given \"intersectionClasses\" parameter is an empty list");
            if (intersectionClasses.Any(cls => cls.Equals(owlClass)))
                throw new OWLSemanticsException("Cannot declare owl:intersectionOf class to the model because given \"intersectionClasses\" parameter contains given \"owlClass\" element, which is not allowed");

            //Declare class to the model
            DeclareClass(owlClass);

            //Add knowledge to the T-BOX
            RDFCollection classesCollection = new RDFCollection(RDFModelEnums.RDFItemTypes.Resource);
            intersectionClasses.ForEach(cls => classesCollection.AddItem(cls));
            TBoxGraph.AddCollection(classesCollection);
            TBoxGraph.AddTriple(new RDFTriple(owlClass, RDFVocabulary.OWL.INTERSECTION_OF, classesCollection.ReificationSubject));

            return this;
        }

        /// <summary>
        /// Declares the exostence of the given owl:complementOf class to the model
        /// </summary>
        public OWLOntologyClassModel DeclareComplementClass(RDFResource owlClass, RDFResource complementClass)
        {
            if (owlClass == null)
                throw new OWLSemanticsException("Cannot declare owl:complementOf class to the model because given \"owlClass\" parameter is null");
            if (complementClass == null)
                throw new OWLSemanticsException("Cannot declare owl:complementOf class to the model because given \"complementClass\" parameter is null");
            if (owlClass.Equals(complementClass))
                throw new OWLSemanticsException("Cannot declare owl:complementOf class to the model because given \"owlClass\" parameter corresponds to given \"complementClass\" parameter, which is not allowed");

            //Declare class to the model
            DeclareClass(owlClass);

            //Add knowledge to the T-BOX
            TBoxGraph.AddTriple(new RDFTriple(owlClass, RDFVocabulary.OWL.COMPLEMENT_OF, complementClass));

            return this;
        }

        //SHORTCUTS [OWL2]

        /// <summary>
        /// Declares the existence of the given owl:disjointUnionOf class to the model [OWL2]
        /// </summary>
        public OWLOntologyClassModel DeclareDisjointUnionClass(RDFResource owlClass, List<RDFResource> disjointClasses)
        {
            if (owlClass == null)
                throw new OWLSemanticsException("Cannot declare owl:disjointUnionOf class to the model because given \"owlClass\" parameter is null");
            if (disjointClasses == null)
                throw new OWLSemanticsException("Cannot declare owl:disjointUnionOf class to the model because given \"disjointClasses\" parameter is null");
            if (disjointClasses.Count == 0)
                throw new OWLSemanticsException("Cannot declare owl:disjointUnionOf class to the model because given \"disjointClasses\" parameter is an empty list");

            //Add class to the model
            DeclareClass(owlClass);

            //Add knowledge to the T-BOX
            RDFCollection disjointClassesCollection = new RDFCollection(RDFModelEnums.RDFItemTypes.Resource);
            disjointClasses.ForEach(cls => disjointClassesCollection.AddItem(cls));
            TBoxGraph.AddCollection(disjointClassesCollection);
            TBoxGraph.AddTriple(new RDFTriple(owlClass, RDFVocabulary.OWL.DISJOINT_UNION_OF, disjointClassesCollection.ReificationSubject));

            return this;
        }

        /// <summary>
        /// Declares the existence of the given owl:AllDisjointClasses class to the model [OWL2]
        /// </summary>
        public OWLOntologyClassModel DeclareAllDisjointClasses(RDFResource owlClass, List<RDFResource> disjointClasses)
        {
            if (owlClass == null)
                throw new OWLSemanticsException("Cannot declare owl:AllDisjointClasses class to the model because given \"owlClass\" parameter is null");
            if (disjointClasses == null)
                throw new OWLSemanticsException("Cannot declare owl:AllDisjointClasses class to the model because given \"disjointClasses\" parameter is null");
            if (disjointClasses.Count == 0)
                throw new OWLSemanticsException("Cannot declare owl:AllDisjointClasses class to the model because given \"disjointClasses\" parameter is an empty list");

            //Declare class to the model
            DeclareClass(owlClass);
            
            //Add knowledge to the T-BOX
            RDFCollection allDisjointClassesCollection = new RDFCollection(RDFModelEnums.RDFItemTypes.Resource);
            disjointClasses.ForEach(disjointClass => allDisjointClassesCollection.AddItem(disjointClass));
            TBoxGraph.AddCollection(allDisjointClassesCollection);
            TBoxGraph.AddTriple(new RDFTriple(owlClass, RDFVocabulary.OWL.MEMBERS, allDisjointClassesCollection.ReificationSubject));
            TBoxGraph.AddTriple(new RDFTriple(owlClass, RDFVocabulary.RDF.TYPE, RDFVocabulary.OWL.ALL_DISJOINT_CLASSES));

            return this;
        }

        //ANNOTATIONS

        /// <summary>
        /// Annotates the given class with the given "annotationProperty -> annotationValue"
        /// </summary>
        public OWLOntologyClassModel AnnotateClass(RDFResource owlClass, RDFResource annotationProperty, RDFResource annotationValue)
        {
            if (owlClass == null)
                throw new OWLSemanticsException("Cannot annotate class because given \"owlClass\" parameter is null");
            if (annotationProperty == null)
                throw new OWLSemanticsException("Cannot annotate class because given \"annotationProperty\" parameter is null");
            if (annotationProperty.IsBlank)
                throw new OWLSemanticsException("Cannot annotate class because given \"annotationProperty\" parameter is a blank predicate");
            if (annotationValue == null)
                throw new OWLSemanticsException("Cannot annotate class because given \"annotationValue\" parameter is null");

            //Add knowledge to the T-BOX
            TBoxGraph.AddTriple(new RDFTriple(owlClass, annotationProperty, annotationValue));

            return this;
        }

        /// <summary>
        /// Annotates the given class with the given "annotationProperty -> annotationValue"
        /// </summary>
        public OWLOntologyClassModel AnnotateClass(RDFResource owlClass, RDFResource annotationProperty, RDFLiteral annotationValue)
        {
            if (owlClass == null)
                throw new OWLSemanticsException("Cannot annotate class because given \"owlClass\" parameter is null");
            if (annotationProperty == null)
                throw new OWLSemanticsException("Cannot annotate class because given \"annotationProperty\" parameter is null");
            if (annotationProperty.IsBlank)
                throw new OWLSemanticsException("Cannot annotate class because given \"annotationProperty\" parameter is a blank predicate");
            if (annotationValue == null)
                throw new OWLSemanticsException("Cannot annotate class because given \"annotationValue\" parameter is null");

            //Add knowledge to the T-BOX
            TBoxGraph.AddTriple(new RDFTriple(owlClass, annotationProperty, annotationValue));

            return this;
        }

        //RELATIONS

        /// <summary>
        /// Declares the existence of the given "SubClass(childClass,motherClass)" relation to the model
        /// </summary>
        public OWLOntologyClassModel DeclareSubClasses(RDFResource childClass, RDFResource motherClass)
        {
            #region OWL-DL Integrity Checks
            bool OWLDLIntegrityChecks()
                => !childClass.CheckReservedClass()
                      && !motherClass.CheckReservedClass()
                        && this.CheckSubClassCompatibility(childClass, motherClass);
            #endregion

            if (childClass == null)
                throw new OWLSemanticsException("Cannot declare rdfs:subClassOf relation to the model because given \"childClass\" parameter is null");
            if (motherClass == null)
                throw new OWLSemanticsException("Cannot declare rdfs:subClassOf relation to the model because given \"motherClass\" parameter is null");
            if (childClass.Equals(motherClass))
                throw new OWLSemanticsException("Cannot declare rdfs:subClassOf relation to the model because given \"childClass\" parameter refers to the same class as the given \"motherClass\" parameter");

            //Add knowledge to the T-BOX (or raise warning if violations are detected)
            if (OWLDLIntegrityChecks())
                TBoxGraph.AddTriple(new RDFTriple(childClass, RDFVocabulary.RDFS.SUB_CLASS_OF, motherClass));
            else
                OWLSemanticsEvents.RaiseSemanticsWarning(string.Format("SubClass relation between class '{0}' and class '{1}' cannot be declared to the model because it would violate OWL-DL integrity", childClass, motherClass));

            return this;
        }

        /// <summary>
        /// Declares the existence of the given "EquivalentClass(leftClass,rightClass)" relation to the model
        /// </summary>
        public OWLOntologyClassModel DeclareEquivalentClasses(RDFResource leftClass, RDFResource rightClass)
        {
            #region OWL-DL Integrity Checks
            bool OWLDLIntegrityChecks()
                => !leftClass.CheckReservedClass()
                      && !rightClass.CheckReservedClass()
                        && this.CheckEquivalentClassCompatibility(leftClass, rightClass);
            #endregion

            if (leftClass == null)
                throw new OWLSemanticsException("Cannot declare owl:equivalentClass relation to the model because given \"leftClass\" parameter is null");
            if (rightClass == null)
                throw new OWLSemanticsException("Cannot declare owl:equivalentClass relation to the model because given \"rightClass\" parameter is null");
            if (leftClass.Equals(rightClass))
                throw new OWLSemanticsException("Cannot declare owl:equivalentClass relation to the model because given \"leftClass\" parameter refers to the same class as the given \"rightClass\" parameter");

            //Add knowledge to the T-BOX (or raise warning if violations are detected)
            if (OWLDLIntegrityChecks())
            {
                TBoxGraph.AddTriple(new RDFTriple(leftClass, RDFVocabulary.OWL.EQUIVALENT_CLASS, rightClass));

                //Also add an automatic T-BOX inference exploiting symmetry of owl:equivalentClass relation
                TBoxGraph.AddTriple(new RDFTriple(rightClass, RDFVocabulary.OWL.EQUIVALENT_CLASS, leftClass));
            }
            else
                OWLSemanticsEvents.RaiseSemanticsWarning(string.Format("EquivalentClass relation between class '{0}' and class '{1}' cannot be declared to the model because it would violate OWL-DL integrity", leftClass, rightClass));

            return this;
        }

        /// <summary>
        /// Declares the existence of the given "DisjointWith(leftClass,rightClass)" relation to the model
        /// </summary>
        public OWLOntologyClassModel DeclareDisjointClasses(RDFResource leftClass, RDFResource rightClass)
        {
            #region OWL-DL Integrity Checks
            bool OWLDLIntegrityChecks()
                => !leftClass.CheckReservedClass()
                      && !rightClass.CheckReservedClass()
                        && this.CheckDisjointWithCompatibility(leftClass, rightClass);
            #endregion

            if (leftClass == null)
                throw new OWLSemanticsException("Cannot declare owl:disjointWith relation to the model because given \"leftClass\" parameter is null");
            if (rightClass == null)
                throw new OWLSemanticsException("Cannot declare owl:disjointWith relation to the model because given \"rightClass\" parameter is null");
            if (leftClass.Equals(rightClass))
                throw new OWLSemanticsException("Cannot declare owl:disjointWith relation to the model because given \"leftClass\" parameter refers to the same class as the given \"rightClass\" parameter");

            //Add knowledge to the T-BOX (or raise warning if violations are detected)
            if (OWLDLIntegrityChecks())
            {
                TBoxGraph.AddTriple(new RDFTriple(leftClass, RDFVocabulary.OWL.DISJOINT_WITH, rightClass));

                //Also add an automatic T-BOX inference exploiting symmetry of owl:disjointWith relation
                TBoxGraph.AddTriple(new RDFTriple(rightClass, RDFVocabulary.OWL.DISJOINT_WITH, leftClass));
            }
            else
                OWLSemanticsEvents.RaiseSemanticsWarning(string.Format("DisjointWith relation between class '{0}' and class '{1}' cannot be declared to the model because it would violate OWL-DL integrity", leftClass, rightClass));

            return this;
        }

        /// <summary>
        /// Declares the existence of the given "HasKey(owlClass,keyProperties)" relation to the model [OWL2]
        /// </summary>
        public OWLOntologyClassModel DeclareHasKey(RDFResource owlClass, List<RDFResource> keyProperties)
        {
            if (owlClass == null)
                throw new OWLSemanticsException("Cannot declare owl:hasKey relation to the model because given \"owlClass\" parameter is null");
            if (keyProperties == null)
                throw new OWLSemanticsException("Cannot declare owl:hasKey relation to the model because given \"keyProperties\" parameter is null");
            if (keyProperties.Count == 0)
                throw new OWLSemanticsException("Cannot declare owl:hasKey relation to the model because given \"keyProperties\" parameter is an empty list");

            //Add knowledge to the T-BOX
            RDFCollection keyPropertiesCollection = new RDFCollection(RDFModelEnums.RDFItemTypes.Resource);
            keyProperties.ForEach(keyProperty => keyPropertiesCollection.AddItem(keyProperty));
            TBoxGraph.AddCollection(keyPropertiesCollection);
            TBoxGraph.AddTriple(new RDFTriple(owlClass, RDFVocabulary.OWL.HAS_KEY, keyPropertiesCollection.ReificationSubject));

            return this;
        }

        /// <summary>
        /// Declares the given owl:Restriction to the model
        /// </summary>
        internal OWLOntologyClassModel DeclareRestriction(RDFResource owlRestriction, RDFResource onProperty)
        {
            if (owlRestriction == null)
                throw new OWLSemanticsException("Cannot declare owl:Restriction to the model because given \"owlRestriction\" parameter is null");
            if (onProperty == null)
                throw new OWLSemanticsException("Cannot declare owl:Restriction to the model because given \"onProperty\" parameter is null");

            //Add class to the model
            DeclareClass(owlRestriction);

            //Add knowledge to the T-BOX
            TBoxGraph.AddTriple(new RDFTriple(owlRestriction, RDFVocabulary.RDF.TYPE, RDFVocabulary.OWL.RESTRICTION));
            TBoxGraph.AddTriple(new RDFTriple(owlRestriction, RDFVocabulary.OWL.ON_PROPERTY, onProperty));

            return this;
        }

        //EXPORT

        /// <summary>
        /// Gets a graph representation of the model
        /// </summary>
        public RDFGraph ToRDFGraph()
            => TBoxGraph;

        /// <summary>
        /// Asynchronously gets a graph representation of the model
        /// </summary>
        public Task<RDFGraph> ToRDFGraphAsync()
            => Task.Run(() => ToRDFGraph());
        #endregion
    }

    #region Behaviors
    /// <summary>
    /// OWLOntologyClassBehavior defines the mathematical aspects of an owl:Class instance
    /// </summary>
    public class OWLOntologyClassBehavior
    {
        #region Properties
        /// <summary>
        /// Defines the class as instance of owl:DeprecatedClass
        /// </summary>
        public bool Deprecated { get; set; }
        #endregion
    }
    #endregion
}