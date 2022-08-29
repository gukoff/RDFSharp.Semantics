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

using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using RDFSharp.Model;

namespace RDFSharp.Semantics
{
    /// <summary>
    /// RDFOntologyModel represents the T-BOX of the application domain formalized by the ontology
    /// </summary>
    public class RDFOntologyModel
    {
        #region Properties
        /// <summary>
        /// Model of the entities contained within the application domain
        /// </summary>
        public RDFOntologyClassModel ClassModel { get; internal set; }

        /// <summary>
        /// Model of the properties linking the entities of the application domain
        /// </summary>
        public RDFOntologyPropertyModel PropertyModel { get; internal set; }
        #endregion

        #region Ctors
        /// <summary>
        /// Builds an empty ontology model
        /// </summary>
        public RDFOntologyModel()
        {
            ClassModel = new RDFOntologyClassModel();
            PropertyModel = new RDFOntologyPropertyModel();
        }

        /// <summary>
        /// Builds an ontology model having the given T-BOX knowledge
        /// </summary>
        public RDFOntologyModel(RDFOntologyClassModel classModel, RDFOntologyPropertyModel propertyModel) : this()
        {
            ClassModel = classModel ?? new RDFOntologyClassModel();
            PropertyModel = propertyModel ?? new RDFOntologyPropertyModel();
        }
        #endregion

        #region Methods
        /// <summary>
        /// Gets a graph representation of the model
        /// </summary>
        public RDFGraph ToRDFGraph(bool includeInferences)
            => ClassModel.ToRDFGraph(includeInferences)
                  .UnionWith(PropertyModel.ToRDFGraph(includeInferences));

        /// <summary>
        /// Asynchronously gets a graph representation of the model
        /// </summary>
        public Task<RDFGraph> ToRDFGraphAsync(bool includeInferences)
            => Task.Run(() => ToRDFGraph(includeInferences));
        #endregion
    }
}