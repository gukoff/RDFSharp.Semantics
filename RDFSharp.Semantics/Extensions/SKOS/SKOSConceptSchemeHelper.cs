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

namespace RDFSharp.Semantics.Extensions.SKOS
{
    /// <summary>
    /// SKOSConceptSchemeHelper contains methods for analyzing relations describing SKOS concept schemes
    /// </summary>
    public static class SKOSConceptSchemeHelper
    {
        #region Methods
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
                                           && !conceptScheme.CheckHasNarrowerConcept(leftConcept, rightConcept));

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
    }
}