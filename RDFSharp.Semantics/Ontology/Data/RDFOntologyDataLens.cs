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
using System.Data;
using System.Linq;
using System.Threading.Tasks;

namespace RDFSharp.Semantics
{
    /// <summary>
    /// RDFOntologyDataLens represents a magnifying glass on the knowledge available for a given owl:Individual instance within an ontology
    /// </summary>
    public class RDFOntologyDataLens
    {
        #region Properties
        /// <summary>
        /// Individual observed by the lens
        /// </summary>
        public RDFResource Individual { get; internal set; }

        /// <summary>
        /// Ontology observed by the lens
        /// </summary>
        public RDFOntology Ontology { get; internal set; }
        #endregion

        #region Ctors
        /// <summary>
        /// Builds a data lens for the given owl:Individual instance on the given ontology
        /// </summary>
        public RDFOntologyDataLens(RDFResource owlIndividual, RDFOntology ontology)
        {
            if (owlIndividual == null)
                throw new RDFSemanticsException("Cannot create data lens because given \"owlIndividual\" parameter is null");
            if (ontology == null)
                throw new RDFSemanticsException("Cannot create data lens because given \"ontology\" parameter is null");

            Individual = owlIndividual;
            Ontology = ontology;
        }
        #endregion

        #region Methods
        /// <summary>
        /// Enlists the individuals which are related with the lens individual by owl:sameAs
        /// </summary>
        public List<RDFResource> SameIndividuals()
            => Ontology.Data.GetSameIndividuals(Individual);

        /// <summary>
        /// Asynchronously enlists the individuals which are related with the lens individual by owl:sameAs
        /// </summary>
        public Task<List<RDFResource>> SameIndividualsAsync()
            => Task.Run(() => SameIndividuals());

        /// <summary>
        /// Enlists the individuals which are related with the lens individual by owl:differentFrom
        /// </summary>
        public List<RDFResource> DifferentIndividuals()
            => Ontology.Data.GetDifferentIndividuals(Individual);

        /// <summary>
        /// Asynchronously enlists the individuals which are related with the lens individual by owl:differentFrom
        /// </summary>
        public Task<List<RDFResource>> DifferentIndividualsAsync()
            => Task.Run(() => DifferentIndividuals());

        /// <summary>
        /// Enlists the classes to which the lens individual belongs by rdf:type
        /// </summary>
        public List<RDFResource> ClassTypes()
        {
            List<RDFResource> result = new List<RDFResource>();

            Dictionary<long, List<RDFResource>> classTypesCache = new Dictionary<long, List<RDFResource>>();

            //Enumerates (owl:oneOf)
            foreach (RDFResource enumerateClass in Ontology.Model.ClassModel.Where(cls => Ontology.Model.ClassModel.CheckHasEnumerateClass(cls)))
            {
                if (!classTypesCache.ContainsKey(enumerateClass.PatternMemberID))
                    classTypesCache.Add(enumerateClass.PatternMemberID, Ontology.Data.GetIndividualsOf(Ontology.Model, enumerateClass));

                if (classTypesCache[enumerateClass.PatternMemberID].Any(individual => individual.Equals(Individual)))
                    result.Add(enumerateClass);
            }

            //Restrictions (owl:Restriction)
            foreach (RDFResource restrictionClass in Ontology.Model.ClassModel.Where(cls => Ontology.Model.ClassModel.CheckHasRestrictionClass(cls)))
            {
                if (!classTypesCache.ContainsKey(restrictionClass.PatternMemberID))
                    classTypesCache.Add(restrictionClass.PatternMemberID, Ontology.Data.GetIndividualsOf(Ontology.Model, restrictionClass));

                if (classTypesCache[restrictionClass.PatternMemberID].Any(individual => individual.Equals(Individual)))
                    result.Add(restrictionClass);
            }

            //Classes (owl:Class)
            foreach (RDFResource simpleClass in Ontology.Model.ClassModel.Where(cls => Ontology.Model.ClassModel.CheckHasSimpleClass(cls)))
            {
                if (!classTypesCache.ContainsKey(simpleClass.PatternMemberID))
                    classTypesCache.Add(simpleClass.PatternMemberID, Ontology.Data.GetIndividualsOf(Ontology.Model, simpleClass));

                if (classTypesCache[simpleClass.PatternMemberID].Any(individual => individual.Equals(Individual)))
                    result.Add(simpleClass);
            }

            //Composites (owl:unionOf, owl:intersectionOf, owl:complementOf)
            foreach (RDFResource compositeClass in Ontology.Model.ClassModel.Where(cls => Ontology.Model.ClassModel.CheckHasCompositeClass(cls)))
            {
                if (!classTypesCache.ContainsKey(compositeClass.PatternMemberID))
                    classTypesCache.Add(compositeClass.PatternMemberID, Ontology.Data.GetIndividualsOf(Ontology.Model, compositeClass));

                if (classTypesCache[compositeClass.PatternMemberID].Any(individual => individual.Equals(Individual)))
                    result.Add(compositeClass);
            }

            return result;
        }

        /// <summary>
        /// Asynchronously enlists the classes to which the lens individual belongs by rdf:type
        /// </summary>
        public Task<List<RDFResource>> ClassTypesAsync()
            => Task.Run(() => ClassTypes());

        /// <summary>
        /// Enlists the object assertions to which the lens individual is related as subject or object
        /// </summary>
        public List<RDFTriple> ObjectAssertions()
        {
            List<RDFTriple> result = new List<RDFTriple>();

            RDFGraph aboxVirtualGraph = Ontology.Data.ABoxVirtualGraph;
            result.AddRange(aboxVirtualGraph.Where(asn => asn.TripleFlavor == RDFModelEnums.RDFTripleFlavors.SPO
                                                            && Ontology.Model.PropertyModel.CheckHasObjectProperty((RDFResource)asn.Predicate)
                                                              && (asn.Subject.Equals(Individual) || asn.Object.Equals(Individual))));

            return result;
        }

        /// <summary>
        /// Asynchronously enlists the object assertions to which the lens individual is related as subject or object
        /// </summary>
        public Task<List<RDFTriple>> ObjectAssertionsAsync()
            => Task.Run(() => ObjectAssertions());

        /// <summary>
        /// Enlists the data assertions to which the lens individual is related as subject
        /// </summary>
        public List<RDFTriple> DataAssertions()
        {
            List<RDFTriple> result = new List<RDFTriple>();

            RDFGraph aboxVirtualGraph = Ontology.Data.ABoxVirtualGraph;
            result.AddRange(aboxVirtualGraph.Where(asn => asn.TripleFlavor == RDFModelEnums.RDFTripleFlavors.SPL
                                                            && Ontology.Model.PropertyModel.CheckHasDatatypeProperty((RDFResource)asn.Predicate)
                                                              && asn.Subject.Equals(Individual)));

            return result;
        }

        /// <summary>
        /// Asynchronously enlists the data assertions to which the lens individual is related as subject
        /// </summary>
        public Task<List<RDFTriple>> DataAssertionsAsync()
            => Task.Run(() => DataAssertions());

        /// <summary>
        /// Enlists the negative object assertions to which the lens individual is related as subject or object [OWL2]
        /// </summary>
        public List<RDFTriple> NegativeObjectAssertions()
        {
            List<RDFTriple> result = new List<RDFTriple>();

            RDFGraph aboxVirtualGraph = Ontology.Data.ABoxVirtualGraph;
            RDFSelectQuery negativeObjectAssertionQuery = new RDFSelectQuery()
                //Subject
                .AddPatternGroup(new RDFPatternGroup()
                    .AddPattern(new RDFPattern(new RDFVariable("?NASN_REPRESENTATIVE"), RDFVocabulary.RDF.TYPE, RDFVocabulary.OWL.NEGATIVE_PROPERTY_ASSERTION))
                    .AddPattern(new RDFPattern(new RDFVariable("?NASN_REPRESENTATIVE"), RDFVocabulary.OWL.SOURCE_INDIVIDUAL, Individual))
                    .AddPattern(new RDFPattern(new RDFVariable("?NASN_REPRESENTATIVE"), RDFVocabulary.OWL.ASSERTION_PROPERTY, new RDFVariable("?NASN_PROPERTY")))
                    .AddPattern(new RDFPattern(new RDFVariable("?NASN_REPRESENTATIVE"), RDFVocabulary.OWL.TARGET_INDIVIDUAL, new RDFVariable("?NASN_TARGET")))
                    .AddFilter(new RDFIsUriFilter(new RDFVariable("?NASN_PROPERTY")))
                    .AddFilter(new RDFIsUriFilter(new RDFVariable("?NASN_TARGET")))
                    .UnionWithNext())
                //Object
                .AddPatternGroup(new RDFPatternGroup()
                    .AddPattern(new RDFPattern(new RDFVariable("?NASN_REPRESENTATIVE"), RDFVocabulary.RDF.TYPE, RDFVocabulary.OWL.NEGATIVE_PROPERTY_ASSERTION))
                    .AddPattern(new RDFPattern(new RDFVariable("?NASN_REPRESENTATIVE"), RDFVocabulary.OWL.SOURCE_INDIVIDUAL, new RDFVariable("?NASN_SOURCE")))
                    .AddPattern(new RDFPattern(new RDFVariable("?NASN_REPRESENTATIVE"), RDFVocabulary.OWL.ASSERTION_PROPERTY, new RDFVariable("?NASN_PROPERTY")))
                    .AddPattern(new RDFPattern(new RDFVariable("?NASN_REPRESENTATIVE"), RDFVocabulary.OWL.TARGET_INDIVIDUAL, Individual))
                    .AddFilter(new RDFIsUriFilter(new RDFVariable("?NASN_PROPERTY")))
                    .AddFilter(new RDFIsUriFilter(new RDFVariable("?NASN_SOURCE"))))
                .AddProjectionVariable(new RDFVariable("?NASN_SOURCE"))
                .AddProjectionVariable(new RDFVariable("?NASN_PROPERTY"))
                .AddProjectionVariable(new RDFVariable("?NASN_TARGET"));
            RDFSelectQueryResult negativeObjectAssertionQueryResult = negativeObjectAssertionQuery.ApplyToGraph(aboxVirtualGraph);

            foreach (DataRow negativeObjectAssertion in negativeObjectAssertionQueryResult.SelectResults.Rows)
            {
                //Subject
                if (negativeObjectAssertion.IsNull("?NASN_SOURCE"))
                    result.Add(new RDFTriple(Individual, new RDFResource(negativeObjectAssertion["?NASN_PROPERTY"].ToString()), new RDFResource(negativeObjectAssertion["?NASN_TARGET"].ToString())));
                //Object
                else if (negativeObjectAssertion.IsNull("?NASN_TARGET"))
                    result.Add(new RDFTriple(new RDFResource(negativeObjectAssertion["?NASN_SOURCE"].ToString()), new RDFResource(negativeObjectAssertion["?NASN_PROPERTY"].ToString()), Individual));
            }

            return result;
        }

        /// <summary>
        /// Asynchronously enlists the negative object assertions to which the lens individual is related as subject or object [OWL2]
        /// </summary>
        public Task<List<RDFTriple>> NegativeObjectAssertionsAsync()
            => Task.Run(() => NegativeObjectAssertions());

        /// <summary>
        /// Enlists the negative data assertions to which the lens individual is related as subject [OWL2]
        /// </summary>
        public List<RDFTriple> NegativeDataAssertions()
        {
            List<RDFTriple> result = new List<RDFTriple>();

            RDFGraph aboxVirtualGraph = Ontology.Data.ABoxVirtualGraph;
            RDFSelectQuery negativeDatatypeAssertionQuery = new RDFSelectQuery()
                .AddPatternGroup(new RDFPatternGroup()
                    .AddPattern(new RDFPattern(new RDFVariable("?NASN_REPRESENTATIVE"), RDFVocabulary.RDF.TYPE, RDFVocabulary.OWL.NEGATIVE_PROPERTY_ASSERTION))
                    .AddPattern(new RDFPattern(new RDFVariable("?NASN_REPRESENTATIVE"), RDFVocabulary.OWL.SOURCE_INDIVIDUAL, Individual))
                    .AddPattern(new RDFPattern(new RDFVariable("?NASN_REPRESENTATIVE"), RDFVocabulary.OWL.ASSERTION_PROPERTY, new RDFVariable("?NASN_PROPERTY")))
                    .AddPattern(new RDFPattern(new RDFVariable("?NASN_REPRESENTATIVE"), RDFVocabulary.OWL.TARGET_VALUE, new RDFVariable("?NASN_TARGET")))
                    .AddFilter(new RDFIsUriFilter(new RDFVariable("?NASN_PROPERTY")))
                    .AddFilter(new RDFIsLiteralFilter(new RDFVariable("?NASN_TARGET"))))
                .AddProjectionVariable(new RDFVariable("?NASN_PROPERTY"))
                .AddProjectionVariable(new RDFVariable("?NASN_TARGET"));
            RDFSelectQueryResult negativeDatatypeAssertionQueryResult = negativeDatatypeAssertionQuery.ApplyToGraph(aboxVirtualGraph);

            foreach (DataRow negativeDatatypeAssertion in negativeDatatypeAssertionQueryResult.SelectResults.Rows)
                result.Add(new RDFTriple(Individual, new RDFResource(negativeDatatypeAssertion["?NASN_PROPERTY"].ToString()), (RDFLiteral)RDFQueryUtilities.ParseRDFPatternMember(negativeDatatypeAssertion["?NASN_TARGET"].ToString())));

            return result;
        }

        /// <summary>
        /// Asynchronously enlists the negative data assertions to which the lens individual is related as subject [OWL2]
        /// </summary>
        public Task<List<RDFTriple>> NegativeDataAssertionsAsync()
            => Task.Run(() => NegativeDataAssertions());

        /// <summary>
        /// Enlists the object annotations to which the lens individual is related as subject
        /// </summary>
        public List<RDFTriple> ObjectAnnotations()
        {
            List<RDFTriple> result = new List<RDFTriple>();

            result.AddRange(Ontology.Data.ABoxGraph.Where(ann => ann.TripleFlavor == RDFModelEnums.RDFTripleFlavors.SPO
                                                                    && Ontology.Model.PropertyModel.CheckHasAnnotationProperty((RDFResource)ann.Predicate)
                                                                        && ann.Subject.Equals(Individual)));

            return result;
        }

        /// <summary>
        /// Asynchronously enlists the object annotations to which the lens individual is related as subject
        /// </summary>
        public Task<List<RDFTriple>> ObjectAnnotationsAsync()
            => Task.Run(() => ObjectAnnotations());

        /// <summary>
        /// Enlists the data annotations to which the lens individual is related as subject
        /// </summary>
        public List<RDFTriple> DataAnnotations()
        {
            List<RDFTriple> result = new List<RDFTriple>();

            result.AddRange(Ontology.Data.ABoxGraph.Where(ann => ann.TripleFlavor == RDFModelEnums.RDFTripleFlavors.SPL
                                                                    && Ontology.Model.PropertyModel.CheckHasAnnotationProperty((RDFResource)ann.Predicate)
                                                                        && ann.Subject.Equals(Individual)));

            return result;
        }

        /// <summary>
        /// Asynchronously enlists the data annotations to which the lens individual is related as subject
        /// </summary>
        public Task<List<RDFTriple>> DataAnnotationsAsync()
            => Task.Run(() => DataAnnotations());
        #endregion
    }
}