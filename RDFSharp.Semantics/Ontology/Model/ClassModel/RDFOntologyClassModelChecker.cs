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
    /// RDFOntologyClassModelChecker is responsible for real-time OWL-DL validation of ontologies during T-BOX class modeling
    /// </summary>
    public static class RDFOntologyClassModelChecker
    {
        #region Methods
        /// <summary>
        /// Checks if the given owl:Class is a reserved ontology class
        /// </summary>
        internal static bool CheckReservedClass(this RDFResource owlClass) =>
            RDFSemanticsUtilities.ReservedClasses.Contains(owlClass.PatternMemberID);

        /// <summary>
        /// Checks if the given childClass can be subClass of the given motherClass without tampering OWL-DL integrity
        /// </summary>
        internal static bool CheckSubClassCompatibility(this RDFOntologyClassModel classModel, RDFResource childClass, RDFResource motherClass)
            => !classModel.CheckAreSubClasses(motherClass, childClass)
                 && !classModel.CheckAreEquivalentClasses(motherClass, childClass)
                   && !classModel.CheckAreDisjointClasses(motherClass, childClass);

        /// <summary>
        /// Checks if the given leftClass can be equivalentClass of the given rightClass without tampering OWL-DL integrity
        /// </summary>
        internal static bool CheckEquivalentClassCompatibility(this RDFOntologyClassModel classModel, RDFResource leftClass, RDFResource rightClass)
            => !classModel.CheckAreSubClasses(leftClass, rightClass)
                    && !classModel.CheckAreSuperClasses(leftClass, rightClass)
                        && !classModel.CheckAreDisjointClasses(leftClass, rightClass);

        /// <summary>
        /// Checks if the given leftClass can be disjoint class of the given right class without tampering OWL-DL integrity
        /// </summary>
        internal static bool CheckDisjointWithCompatibility(this RDFOntologyClassModel classModel, RDFResource leftClass, RDFResource rightClass)
            => !classModel.CheckAreSubClasses(leftClass, rightClass)
                    && !classModel.CheckAreSuperClasses(leftClass, rightClass)
                        && !classModel.CheckAreEquivalentClasses(leftClass, rightClass);
        #endregion
    }
}