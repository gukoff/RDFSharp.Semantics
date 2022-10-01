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

namespace RDFSharp.Semantics
{
    /// <summary>
    /// OWLOntologyModelLoader is responsible for loading ontology models from remote sources or alternative representations
    /// </summary>
    internal static class OWLOntologyModelLoader
    {
        #region Methods
        /// <summary>
        /// Gets an ontology model representation of the given graph
        /// </summary>
        internal static void LoadModel(this OWLOntology ontology, RDFGraph graph, Action<OWLOntology,RDFGraph> classModelExtensionPoint=null, Action<OWLOntology,RDFGraph> propertyModelExtensionPoint=null)
        {
            if (graph == null)
                throw new OWLSemanticsException("Cannot get ontology model from RDFGraph because given \"graph\" parameter is null");

            OWLSemanticsEvents.RaiseSemanticsInfo(string.Format("Graph '{0}' is going to be parsed as Model...", graph.Context));
            ontology.LoadPropertyModel(graph, classModelExtensionPoint);
            ontology.LoadClassModel(graph, propertyModelExtensionPoint);
            OWLSemanticsEvents.RaiseSemanticsInfo(string.Format("Graph '{0}' has been parsed as Model", graph.Context));
        }
        #endregion
    }
}