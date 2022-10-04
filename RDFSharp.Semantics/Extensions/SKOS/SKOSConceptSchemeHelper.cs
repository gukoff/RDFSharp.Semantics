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
using System.Collections.Generic;
using System.Linq;

namespace RDFSharp.Semantics.Extensions.SKOS
{
    /// <summary>
    /// SKOSConceptSchemeHelper contains methods for analyzing relations describing SKOS concept schemes
    /// </summary>
    public static class SKOSConceptSchemeHelper
    {
        #region Declarer
        /// <summary>
        /// Checks if the given leftConcept can be skos:[broader|broaderTransitive|broadMatch] than the given rightConcept without tampering SKOS integrity
        /// </summary>
        internal static bool CheckBroaderCompatibility(this SKOSConceptScheme conceptScheme, RDFResource childConcept, RDFResource motherConcept)
        {
            //Avoid clash with hierarchical relations
            bool canAddBroaderRelation = !conceptScheme.CheckHasNarrowerConcept(childConcept, motherConcept);

            //Avoid clash with associative relations
            if (canAddBroaderRelation)
                canAddBroaderRelation = !conceptScheme.CheckHasRelatedConcept(childConcept, motherConcept);

            //Avoid clash with mapping relations
            if (canAddBroaderRelation)
                canAddBroaderRelation = !conceptScheme.CheckHasNarrowMatchConcept(childConcept, motherConcept) 
                                          && !conceptScheme.CheckHasCloseMatchConcept(childConcept, motherConcept) 
                                            && !conceptScheme.CheckHasExactMatchConcept(childConcept, motherConcept) 
                                              && !conceptScheme.CheckHasRelatedMatchConcept(childConcept, motherConcept);

            return canAddBroaderRelation;
        }

        /// <summary>
        /// Checks if the given leftConcept can be skos:[narrower|narrowerTransitive|narrowMatch] than the given rightConcept without tampering SKOS integrity
        /// </summary>
        internal static bool CheckNarrowerCompatibility(this SKOSConceptScheme conceptScheme, RDFResource motherConcept, RDFResource childConcept)
        {
            //Avoid clash with hierarchical relations
            bool canAddNarrowerRelation = !conceptScheme.CheckHasBroaderConcept(motherConcept, childConcept);

            //Avoid clash with associative relations
            if (canAddNarrowerRelation)
                canAddNarrowerRelation = !conceptScheme.CheckHasRelatedConcept(motherConcept, childConcept);
            
            //Avoid clash with mapping relations
            if (canAddNarrowerRelation)
                canAddNarrowerRelation = !conceptScheme.CheckHasBroadMatchConcept(motherConcept, childConcept) 
                                           && !conceptScheme.CheckHasCloseMatchConcept(motherConcept, childConcept) 
                                             && !conceptScheme.CheckHasExactMatchConcept(motherConcept, childConcept) 
                                               && !conceptScheme.CheckHasRelatedMatchConcept(motherConcept, childConcept);

            return canAddNarrowerRelation;
        }

        /// <summary>
        /// Checks if the given leftConcept can be skos:[related|relatedMatch] than the given rightConcept without tampering SKOS integrity
        /// </summary>
        internal static bool CheckRelatedCompatibility(this SKOSConceptScheme conceptScheme, RDFResource leftConcept, RDFResource rightConcept)
        {
            //Avoid clash with hierarchical relations
            bool canAddRelatedRelation = !conceptScheme.CheckHasBroaderConcept(leftConcept, rightConcept)
                                           && !conceptScheme.CheckHasNarrowerConcept(leftConcept, rightConcept);

            //Avoid clash with mapping relations
            if (canAddRelatedRelation)
                canAddRelatedRelation = !conceptScheme.CheckHasBroadMatchConcept(leftConcept, rightConcept)
                                          && !conceptScheme.CheckHasNarrowMatchConcept(leftConcept, rightConcept)
                                            && !conceptScheme.CheckHasCloseMatchConcept(leftConcept, rightConcept) 
                                              && !conceptScheme.CheckHasExactMatchConcept(leftConcept, rightConcept);

            return canAddRelatedRelation;
        }

        /// <summary>
        /// Checks if the given leftConcept can be skos:[closeMatch|exactMatch] than the given rightConcept without tampering SKOS integrity
        /// </summary>
        internal static bool CheckCloseOrExactMatchCompatibility(this SKOSConceptScheme conceptScheme, RDFResource leftConcept, RDFResource rightConcept)
        {
            //Avoid clash with hierarchical relations
            bool canAddCloseOrExactMatchRelation = !conceptScheme.CheckHasBroaderConcept(leftConcept, rightConcept) 
                                                     && !conceptScheme.CheckHasNarrowerConcept(leftConcept, rightConcept);

            //Avoid clash with mapping relations
            if (canAddCloseOrExactMatchRelation)
                canAddCloseOrExactMatchRelation = !conceptScheme.CheckHasBroadMatchConcept(leftConcept, rightConcept)
                                                    && !conceptScheme.CheckHasNarrowMatchConcept(leftConcept, rightConcept) 
                                                      && !conceptScheme.CheckHasRelatedMatchConcept(leftConcept, rightConcept);

            return canAddCloseOrExactMatchRelation;
        }
        #endregion

        #region Analyzer
        /// <summary>
        /// Checks for the existence of "Broader(childConcept,motherConcept)" relations within the concept scheme
        /// </summary>
        public static bool CheckHasBroaderConcept(this SKOSConceptScheme conceptScheme, RDFResource childConcept, RDFResource motherConcept)
            => childConcept != null && motherConcept != null && conceptScheme != null && conceptScheme.GetBroaderConcepts(childConcept).Any(concept => concept.Equals(motherConcept));

        /// <summary>
        /// Analyzes "Broader(skosConcept, X)" relations of the concept scheme to answer the broader concepts of the given skos:Concept
        /// </summary>
        public static List<RDFResource> GetBroaderConcepts(this SKOSConceptScheme conceptScheme, RDFResource skosConcept)
        {
            List<RDFResource> broaderConcepts = new List<RDFResource>();

            if (skosConcept != null && conceptScheme != null)
            {
                //Get skos:broader concepts
                foreach (RDFTriple broaderRelation in conceptScheme.Ontology.Data.ABoxGraph[skosConcept, RDFVocabulary.SKOS.BROADER, null, null])
                    broaderConcepts.Add((RDFResource)broaderRelation.Object);

                //Get skos:broaderTransitive concepts
                broaderConcepts.AddRange(conceptScheme.GetBroaderConceptsInternal(skosConcept, new Dictionary<long, RDFResource>()));
            }

            return broaderConcepts;
        }

        /// <summary>
        /// Subsumes the "skos:broaderTransitive" taxonomy to discover direct and indirect broader concepts of the given skos:Concept
        /// </summary>
        internal static List<RDFResource> GetBroaderConceptsInternal(this SKOSConceptScheme conceptScheme, RDFResource skosConcept, Dictionary<long, RDFResource> visitContext)
        {
            List<RDFResource> broaderTransitiveConcepts = new List<RDFResource>();

            #region visitContext
            if (!visitContext.ContainsKey(skosConcept.PatternMemberID))
                visitContext.Add(skosConcept.PatternMemberID, skosConcept);
            else
                return broaderTransitiveConcepts;
            #endregion

            #region Discovery
            //Find broader concepts linked to the given one with skos:broaderTransitive relation
            foreach (RDFTriple broaderTransitiveRelation in conceptScheme.Ontology.Data.ABoxGraph[skosConcept, RDFVocabulary.SKOS.BROADER_TRANSITIVE, null, null])
                broaderTransitiveConcepts.Add((RDFResource)broaderTransitiveRelation.Object);
            #endregion

            // Inference: BROADERTRANSITIVE(A,B) ^ BROADERTRANSITIVE(B,C) -> BROADERTRANSITIVE(A,C)
            foreach (RDFResource broaderTransitiveConcept in broaderTransitiveConcepts.ToList())
                broaderTransitiveConcepts.AddRange(conceptScheme.GetBroaderConceptsInternal(broaderTransitiveConcept, visitContext));

            return broaderTransitiveConcepts;
        }

        /// <summary>
        /// Checks for the existence of "Narrower(motherConcept,childConcept)" relations within the concept scheme
        /// </summary>
        public static bool CheckHasNarrowerConcept(this SKOSConceptScheme conceptScheme, RDFResource motherConcept, RDFResource childConcept)
            => childConcept != null && motherConcept != null && conceptScheme != null && conceptScheme.GetNarrowerConcepts(motherConcept).Any(concept => concept.Equals(childConcept));

        /// <summary>
        /// Analyzes "Narrower(skosConcept, X)" relations of the concept scheme to answer the narrower concepts of the given skos:Concept
        /// </summary>
        public static List<RDFResource> GetNarrowerConcepts(this SKOSConceptScheme conceptScheme, RDFResource skosConcept)
        {
            List<RDFResource> narrowerConcepts = new List<RDFResource>();

            if (skosConcept != null && conceptScheme != null)
            {
                //Get skos:narrower concepts
                foreach (RDFTriple narrowerRelation in conceptScheme.Ontology.Data.ABoxGraph[skosConcept, RDFVocabulary.SKOS.NARROWER, null, null])
                    narrowerConcepts.Add((RDFResource)narrowerRelation.Object);

                //Get skos:narrowerTransitive concepts
                narrowerConcepts.AddRange(conceptScheme.GetNarrowerConceptsInternal(skosConcept, new Dictionary<long, RDFResource>()));
            }

            return narrowerConcepts;
        }

        /// <summary>
        /// Subsumes the "skos:narrowerTransitive" taxonomy to discover direct and indirect narrower concepts of the given skos:Concept
        /// </summary>
        internal static List<RDFResource> GetNarrowerConceptsInternal(this SKOSConceptScheme conceptScheme, RDFResource skosConcept, Dictionary<long, RDFResource> visitContext)
        {
            List<RDFResource> narrowerTransitiveConcepts = new List<RDFResource>();

            #region visitContext
            if (!visitContext.ContainsKey(skosConcept.PatternMemberID))
                visitContext.Add(skosConcept.PatternMemberID, skosConcept);
            else
                return narrowerTransitiveConcepts;
            #endregion

            #region Discovery
            //Find narrower concepts linked to the given one with skos:narrowerTransitive relation
            foreach (RDFTriple narrowerTransitiveRelation in conceptScheme.Ontology.Data.ABoxGraph[skosConcept, RDFVocabulary.SKOS.NARROWER_TRANSITIVE, null, null])
                narrowerTransitiveConcepts.Add((RDFResource)narrowerTransitiveRelation.Object);
            #endregion

            // Inference: NARROWERTRANSITIVE(A,B) ^ NARROWERTRANSITIVE(B,C) -> NARROWERTRANSITIVE(A,C)
            foreach (RDFResource narrowerTransitiveConcept in narrowerTransitiveConcepts.ToList())
                narrowerTransitiveConcepts.AddRange(conceptScheme.GetNarrowerConceptsInternal(narrowerTransitiveConcept, visitContext));

            return narrowerTransitiveConcepts;
        }
        #endregion
    }
}