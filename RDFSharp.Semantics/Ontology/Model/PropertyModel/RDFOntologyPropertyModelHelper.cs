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
    /// RDFOntologyPropertyModelHelper contains utility methods supporting RDFS/OWL-DL modeling, validation and reasoning (T-BOX)
    /// </summary>
    public static class RDFOntologyPropertyModelHelper
    {
        #region SubPropertyOf
        /// <summary>
        /// Checks if the given childProperty is subProperty of the given motherProperty within the given property model
        /// </summary>
        public static bool CheckIsSubPropertyOf(this RDFOntologyPropertyModel propertyModel, RDFOntologyProperty childProperty, RDFOntologyProperty motherProperty)
            => childProperty != null && motherProperty != null && propertyModel != null && propertyModel.GetSuperPropertiesOf(childProperty).Properties.ContainsKey(motherProperty.PatternMemberID);

        /// <summary>
        /// Enlists the sub properties of the given property within the given property model
        /// </summary>
        public static RDFOntologyPropertyModel GetSubPropertiesOf(this RDFOntologyPropertyModel propertyModel, RDFOntologyProperty ontologyProperty)
        {
            RDFOntologyPropertyModel result = new RDFOntologyPropertyModel();
            if (ontologyProperty != null && propertyModel != null)
            {
                //Step 1: Reason on the given property
                result = propertyModel.GetSubPropertiesOfInternal(ontologyProperty);

                //Step 2: Reason on the equivalent properties
                foreach (RDFOntologyProperty ep in propertyModel.GetEquivalentPropertiesOf(ontologyProperty))
                    result = result.UnionWith(propertyModel.GetSubPropertiesOfInternal(ep));
            }
            return result;
        }

        /// <summary>
        /// Subsumes the "rdfs:subPropertyOf" taxonomy to discover direct and indirect subProperties of the given property
        /// </summary>
        internal static RDFOntologyPropertyModel GetSubPropertiesOfInternalVisitor(this RDFOntologyPropertyModel propertyModel, RDFOntologyProperty ontologyProperty)
        {
            RDFOntologyPropertyModel result = new RDFOntologyPropertyModel();

            // Transitivity of "rdfs:subPropertyOf" taxonomy: ((A SUBPROPERTYOF B)  &&  (B SUBPROPERTYOF C))  =>  (A SUBPROPERTYOF C)
            foreach (RDFOntologyTaxonomyEntry sp in propertyModel.Relations.SubPropertyOf.SelectEntriesByObject(ontologyProperty))
            {
                result.AddProperty((RDFOntologyProperty)sp.TaxonomySubject);
                result = result.UnionWith(propertyModel.GetSubPropertiesOfInternalVisitor((RDFOntologyProperty)sp.TaxonomySubject));
            }

            return result;
        }
        internal static RDFOntologyPropertyModel GetSubPropertiesOfInternal(this RDFOntologyPropertyModel propertyModel, RDFOntologyProperty ontologyProperty)
        {
            RDFOntologyPropertyModel result = new RDFOntologyPropertyModel();

            // Step 1: Direct subsumption of "rdfs:subPropertyOf" taxonomy
            RDFOntologyPropertyModel directSubProperties = propertyModel.GetSubPropertiesOfInternalVisitor(ontologyProperty);

            // Step 2: Enlist equivalent properties of subproperties
            result = result.UnionWith(directSubProperties);
            foreach (RDFOntologyProperty sp in directSubProperties)
                result = result.UnionWith(propertyModel.GetEquivalentPropertiesOf(sp)
                                                       .UnionWith(propertyModel.GetSubPropertiesOf(sp)));

            return result;
        }
        #endregion

        #region SuperPropertyOf
        /// <summary>
        /// Checks if the given motherProperty is superProperty of the given childProperty within the given property model
        /// </summary>
        public static bool CheckIsSuperPropertyOf(this RDFOntologyPropertyModel propertyModel, RDFOntologyProperty motherProperty, RDFOntologyProperty childProperty)
            => motherProperty != null && childProperty != null && propertyModel != null && propertyModel.GetSubPropertiesOf(motherProperty).Properties.ContainsKey(childProperty.PatternMemberID);

        /// <summary>
        /// Enlists the super properties of the given property within the given property model
        /// </summary>
        public static RDFOntologyPropertyModel GetSuperPropertiesOf(this RDFOntologyPropertyModel propertyModel, RDFOntologyProperty ontologyProperty)
        {
            RDFOntologyPropertyModel result = new RDFOntologyPropertyModel();
            if (ontologyProperty != null && propertyModel != null)
            {
                //Step 1: Reason on the given property
                result = propertyModel.GetSuperPropertiesOfInternal(ontologyProperty);

                //Step 2: Reason on the equivalent properties
                foreach (RDFOntologyProperty ep in propertyModel.GetEquivalentPropertiesOf(ontologyProperty))
                    result = result.UnionWith(propertyModel.GetSuperPropertiesOfInternal(ep));
            }
            return result;
        }

        /// <summary>
        /// Subsumes the "rdfs:subPropertyOf" taxonomy to discover direct and indirect superProperties of the given property
        /// </summary>
        internal static RDFOntologyPropertyModel GetSuperPropertiesOfInternalVisitor(this RDFOntologyPropertyModel propertyModel, RDFOntologyProperty ontologyProperty)
        {
            RDFOntologyPropertyModel result = new RDFOntologyPropertyModel();

            // Transitivity of "rdfs:subPropertyOf" taxonomy: ((A SUPERPROPERTYOF B)  &&  (B SUPERPROPERTYOF C))  =>  (A SUPERPROPERTYOF C)
            foreach (RDFOntologyTaxonomyEntry sp in propertyModel.Relations.SubPropertyOf.SelectEntriesBySubject(ontologyProperty))
            {
                result.AddProperty((RDFOntologyProperty)sp.TaxonomyObject);
                result = result.UnionWith(propertyModel.GetSuperPropertiesOfInternalVisitor((RDFOntologyProperty)sp.TaxonomyObject));
            }

            return result;
        }
        internal static RDFOntologyPropertyModel GetSuperPropertiesOfInternal(this RDFOntologyPropertyModel propertyModel, RDFOntologyProperty ontologyProperty)
        {
            RDFOntologyPropertyModel result = new RDFOntologyPropertyModel();

            // Step 1: Direct subsumption of "rdfs:subPropertyOf" taxonomy
            RDFOntologyPropertyModel directSuperProperties = propertyModel.GetSuperPropertiesOfInternalVisitor(ontologyProperty);

            // Step 2: Enlist equivalent properties of subproperties
            result = result.UnionWith(directSuperProperties);
            foreach (RDFOntologyProperty sp in directSuperProperties)
                result = result.UnionWith(propertyModel.GetEquivalentPropertiesOf(sp)
                                                       .UnionWith(propertyModel.GetSuperPropertiesOf(sp)));

            return result;
        }
        #endregion

        #region EquivalentProperty
        /// <summary>
        /// Checks if the given aProperty is equivalentProperty of the given bProperty within the given property model
        /// </summary>
        public static bool CheckIsEquivalentPropertyOf(this RDFOntologyPropertyModel propertyModel, RDFOntologyProperty aProperty, RDFOntologyProperty bProperty)
            => aProperty != null && bProperty != null && propertyModel != null && propertyModel.GetEquivalentPropertiesOf(aProperty).Properties.ContainsKey(bProperty.PatternMemberID);

        /// <summary>
        /// Enlists the equivalentProperties of the given property within the given property model
        /// </summary>
        public static RDFOntologyPropertyModel GetEquivalentPropertiesOf(this RDFOntologyPropertyModel propertyModel, RDFOntologyProperty ontologyProperty)
        {
            RDFOntologyPropertyModel result = new RDFOntologyPropertyModel();

            if (ontologyProperty != null && propertyModel != null)
                result = propertyModel.GetEquivalentPropertiesOfInternal(ontologyProperty, null)
                                      .RemoveProperty(ontologyProperty); //Safety deletion

            return result;
        }

        /// <summary>
        /// Subsumes the "owl:equivalentProperty" taxonomy to discover direct and indirect equivalentProperties of the given property
        /// </summary>
        internal static RDFOntologyPropertyModel GetEquivalentPropertiesOfInternal(this RDFOntologyPropertyModel propertyModel, RDFOntologyProperty ontologyProperty, Dictionary<long, RDFOntologyProperty> visitContext)
        {
            RDFOntologyPropertyModel result = new RDFOntologyPropertyModel();

            #region visitContext
            if (visitContext == null)
                visitContext = new Dictionary<long, RDFOntologyProperty>() { { ontologyProperty.PatternMemberID, ontologyProperty } };
            else
            {
                if (!visitContext.ContainsKey(ontologyProperty.PatternMemberID))
                    visitContext.Add(ontologyProperty.PatternMemberID, ontologyProperty);
                else
                    return result;
            }
            #endregion

            // Transitivity of "owl:equivalentProperty" taxonomy: ((A EQUIVALENTPROPERTY B)  &&  (B EQUIVALENTPROPERTY C))  =>  (A EQUIVALENTPROPERTY C)
            foreach (RDFOntologyTaxonomyEntry ep in propertyModel.Relations.EquivalentProperty.SelectEntriesBySubject(ontologyProperty))
            {
                result.AddProperty((RDFOntologyProperty)ep.TaxonomyObject);
                result = result.UnionWith(propertyModel.GetEquivalentPropertiesOfInternal((RDFOntologyProperty)ep.TaxonomyObject, visitContext));
            }

            return result;
        }
        #endregion

        #region DisjointPropertyWith [OWL2]
        /// <summary>
        /// Checks if the given aProperty is disjointProperty with the given bProperty within the given property model
        /// </summary>
        public static bool CheckIsPropertyDisjointWith(this RDFOntologyPropertyModel propertyModel, RDFOntologyProperty aProperty, RDFOntologyProperty bProperty)
            => aProperty != null && bProperty != null && propertyModel != null && propertyModel.GetPropertiesDisjointWith(aProperty).Properties.ContainsKey(bProperty.PatternMemberID);

        /// <summary>
        /// Enlists the disjointProperties of the given property within the given property model
        /// </summary>
        public static RDFOntologyPropertyModel GetPropertiesDisjointWith(this RDFOntologyPropertyModel propertyModel, RDFOntologyProperty ontologyProperty)
        {
            RDFOntologyPropertyModel result = new RDFOntologyPropertyModel();

            if (ontologyProperty != null && propertyModel != null)
                result = propertyModel.GetPropertiesDisjointWithInternal(ontologyProperty, null)
                                      .RemoveProperty(ontologyProperty); //Safety deletion

            return result;
        }

        /// <summary>
        /// Subsumes the "owl:propertyDisjointWith" taxonomy to discover direct and indirect disjointProperties of the given property
        /// </summary>
        internal static RDFOntologyPropertyModel GetPropertiesDisjointWithInternal(this RDFOntologyPropertyModel propertyModel, RDFOntologyProperty ontologyProperty, Dictionary<long, RDFOntologyProperty> visitContext)
        {
            RDFOntologyPropertyModel result1 = new RDFOntologyPropertyModel();
            RDFOntologyPropertyModel result2 = new RDFOntologyPropertyModel();

            #region visitContext
            if (visitContext == null)
                visitContext = new Dictionary<long, RDFOntologyProperty>() { { ontologyProperty.PatternMemberID, ontologyProperty } };
            else
            {
                if (!visitContext.ContainsKey(ontologyProperty.PatternMemberID))
                    visitContext.Add(ontologyProperty.PatternMemberID, ontologyProperty);
                else
                    return result1;
            }
            #endregion

            // Inference: ((A PROPERTYDISJOINTWITH B)   &&  (B EQUIVALENTPROPERTY C))  =>  (A PROPERTYDISJOINTWITH C)
            foreach (RDFOntologyTaxonomyEntry dw in propertyModel.Relations.PropertyDisjointWith.SelectEntriesBySubject(ontologyProperty))
            {
                result1.AddProperty((RDFOntologyProperty)dw.TaxonomyObject);
                result1 = result1.UnionWith(propertyModel.GetEquivalentPropertiesOfInternal((RDFOntologyProperty)dw.TaxonomyObject, visitContext));
            }

            // Inference: ((A PROPERTYDISJOINTWITH B)   &&  (B SUPERPROPERTY C))  =>  (A PROPERTYDISJOINTWITH C)
            result2 = result2.UnionWith(result1);
            foreach (RDFOntologyProperty p in result1)
                result2 = result2.UnionWith(propertyModel.GetSubPropertiesOfInternal(p));
            result1 = result1.UnionWith(result2);

            // Inference: ((A EQUIVALENTPROPERTY B || A SUBPROPERTYOF B)  &&  (B PROPERTYDISJOINTWITH C))  =>  (A PROPERTYDISJOINTWITH C)
            RDFOntologyPropertyModel compatiblePrp = propertyModel.GetSuperPropertiesOf(ontologyProperty)
                                                                  .UnionWith(propertyModel.GetEquivalentPropertiesOf(ontologyProperty));
            foreach (RDFOntologyProperty ep in compatiblePrp)
                result1 = result1.UnionWith(propertyModel.GetPropertiesDisjointWithInternal(ep, visitContext));

            return result1;
        }
        #endregion

        #region PropertyChainAxiom [OWL2]
        /// <summary>
        /// Checks if the given ontProperty is a property chain within the given property model
        /// </summary>
        public static bool CheckIsPropertyChain(this RDFOntologyPropertyModel propertyModel, RDFOntologyObjectProperty ontologyProperty)
        {
            if (ontologyProperty != null && propertyModel != null)
                return propertyModel.Relations.PropertyChainAxiom.Any(te => te.TaxonomySubject.Equals(ontologyProperty));

            return false;
        }

        /// <summary>
        /// Checks if the given aProperty is a property chain step of the given bProperty within the given ontology
        /// </summary>
        public static bool CheckIsPropertyChainStepOf(this RDFOntologyPropertyModel propertyModel, RDFOntologyObjectProperty aProperty, RDFOntologyObjectProperty bProperty)
        {
            if (aProperty != null && bProperty != null && propertyModel != null)
                return propertyModel.GetPropertyChainStepsOf(bProperty).Any(step => step.StepProperty.Equals(aProperty.Value));

            return false;
        }

        /// <summary>
        /// Gets the assertions for each property chain declared in the given ontology [OWL2]
        /// </summary>
        public static Dictionary<string, RDFOntologyData> GetPropertyChainAxiomsData(this RDFOntology ontology)
        {
            Dictionary<string, RDFOntologyData> result = new Dictionary<string, RDFOntologyData>();

            //Materialize graph representation of the given ontology
            RDFGraph ontologyGraph = ontology.ToRDFGraph(RDFSemanticsEnums.RDFOntologyInferenceExportBehavior.ModelAndData);

            //Iterate property chain axiom taxonomy of the given ontology
            foreach (IGrouping<RDFOntologyResource, RDFOntologyTaxonomyEntry> propertyChainAxiomTaxonomy in ontology.Model.PropertyModel.Relations.PropertyChainAxiom.GroupBy(t => t.TaxonomySubject))
            {
                result.Add(propertyChainAxiomTaxonomy.Key.ToString(), new RDFOntologyData());

                //Transform property chain axiom of current property into equivalent property path
                RDFPropertyPath propertyChainAxiomPath = new RDFPropertyPath(new RDFVariable("?PROPERTY_CHAIN_AXIOM_START"), new RDFVariable("?PROPERTY_CHAIN_AXIOM_END"));
                List<RDFPropertyPathStep> propertyChainAxiomPathSteps = ontology.Model.PropertyModel.GetPropertyChainStepsOf(propertyChainAxiomTaxonomy.Key);
                foreach (RDFPropertyPathStep propertyChainAxiomPathStep in propertyChainAxiomPathSteps)
                    propertyChainAxiomPath.AddSequenceStep(propertyChainAxiomPathStep);

                //Execute construct query for getting property chain axiom data from ontology
                RDFConstructQueryResult queryResult =
                    new RDFConstructQuery()
                        .AddPatternGroup(new RDFPatternGroup()
                            .AddPropertyPath(propertyChainAxiomPath))
                        .AddTemplate(new RDFPattern(new RDFVariable("?PROPERTY_CHAIN_AXIOM_START"), (RDFResource)propertyChainAxiomTaxonomy.Key.Value, new RDFVariable("?PROPERTY_CHAIN_AXIOM_END")))
                        .ApplyToGraph(ontologyGraph);

                //Populate result with corresponding ontology assertions
                foreach (RDFTriple queryResultTriple in queryResult.ToRDFGraph())
                {
                    RDFOntologyFact assertionSubject = ontology.Data.SelectFact(queryResultTriple.Subject.ToString());
                    RDFOntologyProperty assertionPredicate = ontology.Model.PropertyModel.SelectProperty(queryResultTriple.Predicate.ToString());
                    RDFOntologyFact assertionObject = ontology.Data.SelectFact(queryResultTriple.Object.ToString());
                    if (assertionPredicate is RDFOntologyObjectProperty)
                        result[propertyChainAxiomTaxonomy.Key.ToString()].AddAssertionRelation(assertionSubject, (RDFOntologyObjectProperty)assertionPredicate, assertionObject);
                }
            }
            return result;
        }

        /// <summary>
        /// Gets the direct and indirect properties composing the path of the given property chain [OWL2]
        /// </summary>
        internal static List<RDFPropertyPathStep> GetPropertyChainStepsOf(this RDFOntologyPropertyModel propertyModel, RDFOntologyResource propertyChain, HashSet<long> visitContext = null)
        {
            List<RDFPropertyPathStep> result = new List<RDFPropertyPathStep>();

            if (propertyChain != null && propertyModel != null)
            {
                #region visitContext
                if (visitContext == null)
                    visitContext = new HashSet<long>() { { propertyChain.Value.PatternMemberID } };
                else
                {
                    if (!visitContext.Contains(propertyChain.Value.PatternMemberID))
                        visitContext.Add(propertyChain.Value.PatternMemberID);
                    else
                        return result;
                }
                #endregion

                //owl:propertyChainAxiom
                foreach (RDFOntologyTaxonomyEntry propertyChainAxiomTaxonomyEntry in propertyModel.Relations.PropertyChainAxiom.SelectEntriesBySubject(propertyChain))
                {
                    bool containsPropertyChainAxiom = propertyModel.Relations.PropertyChainAxiom.SelectEntriesBySubject(propertyChainAxiomTaxonomyEntry.TaxonomyObject).EntriesCount > 0;
                    if (containsPropertyChainAxiom)
                        result.AddRange(propertyModel.GetPropertyChainStepsOf(propertyChainAxiomTaxonomyEntry.TaxonomyObject, visitContext));
                    else
                        result.Add(new RDFPropertyPathStep((RDFResource)propertyChainAxiomTaxonomyEntry.TaxonomyObject.Value));
                }
            }

            return result;
        }
        #endregion

        #region InverseOf
        /// <summary>
        /// Checks if the given aProperty is inverse property of the given bProperty within the given property model
        /// </summary>
        public static bool CheckIsInversePropertyOf(this RDFOntologyPropertyModel propertyModel, RDFOntologyObjectProperty aProperty, RDFOntologyObjectProperty bProperty)
            => aProperty != null && bProperty != null && propertyModel != null && propertyModel.GetInversePropertiesOf(aProperty).Properties.ContainsKey(bProperty.PatternMemberID);

        /// <summary>
        /// Enlists the inverse properties of the given property within the given property model
        /// </summary>
        public static RDFOntologyPropertyModel GetInversePropertiesOf(this RDFOntologyPropertyModel propertyModel, RDFOntologyObjectProperty ontologyProperty)
        {
            RDFOntologyPropertyModel result = new RDFOntologyPropertyModel();

            if (ontologyProperty != null && propertyModel != null)
            {
                //Subject-side inverseOf relation
                foreach (RDFOntologyTaxonomyEntry invOf in propertyModel.Relations.InverseOf.SelectEntriesBySubject(ontologyProperty))
                    result.AddProperty((RDFOntologyObjectProperty)invOf.TaxonomyObject);
                
                //Object-side inverseOf relation
                foreach (RDFOntologyTaxonomyEntry invOf in propertyModel.Relations.InverseOf.SelectEntriesByObject(ontologyProperty))
                    result.AddProperty((RDFOntologyObjectProperty)invOf.TaxonomySubject);

                result.RemoveProperty(ontologyProperty); //Safety deletion
            }

            return result;
        }
        #endregion
     }
}