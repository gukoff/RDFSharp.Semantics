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
using System.Threading.Tasks;
using RDFSharp.Model;

namespace RDFSharp.Semantics
{
    /// <summary>
    /// RDFOntology represents the formal description of an application domain in terms of T-BOX (Model) and A-BOX (Data)
    /// </summary>
    public class RDFOntology : RDFResource
    {
        #region Properties
        /// <summary>
        /// T-BOX of the application domain formalized by the ontology
        /// </summary>
        public RDFOntologyModel Model { get; internal set; }

        /// <summary>
        /// A-BOX available to the ontology from the application domain
        /// </summary>
        public RDFOntologyData Data { get; internal set; }

        /// <summary>
        /// Knowledge describing ontology itself (annotations)
        /// </summary>
        internal RDFGraph OBoxGraph { get; set; }
        #endregion

        #region Ctors
        /// <summary>
        /// Builds an empty ontology having the given URI
        /// </summary>
        public RDFOntology(string ontologyURI) : base(ontologyURI)
        {
            Model = new RDFOntologyModel();
            Data = new RDFOntologyData();
            OBoxGraph = new RDFGraph().SetContext(new Uri(ontologyURI));

            //Add knowledge to the O-BOX
            OBoxGraph.AddTriple(new RDFTriple(new RDFResource(ontologyURI), RDFVocabulary.RDF.TYPE, RDFVocabulary.OWL.ONTOLOGY));
        }

        /// <summary>
        /// Builds an ontology having the given URI and the given T-BOX/A-BOX knowledge
        /// </summary>
        public RDFOntology(string ontologyURI, RDFOntologyModel model, RDFOntologyData data) : this(ontologyURI)
        {
            Model = model ?? new RDFOntologyModel();
            Data = data ?? new RDFOntologyData();
        }
        #endregion

        #region Methods
        /// <summary>
        /// Annotates the ontology with the given "annotationProperty -> annotationValue"
        /// </summary>
        public RDFOntology Annotate(RDFResource annotationProperty, RDFResource annotationValue)
        {
            if (annotationProperty == null)
                throw new RDFSemanticsException("Cannot annotate ontology because given \"annotationProperty\" parameter is null");
            if (annotationProperty.IsBlank)
                throw new RDFSemanticsException("Cannot annotate ontology because given \"annotationProperty\" parameter is a blank predicate");
            if (annotationValue == null)
                throw new RDFSemanticsException("Cannot annotate ontology because given \"annotationValue\" parameter is null");

            //Add knowledge to the O-BOX
            OBoxGraph.AddTriple(new RDFTriple(this, annotationProperty, annotationValue));

            return this;
        }

        /// <summary>
        /// Annotates the ontology with the given "annotationProperty -> annotationValue"
        /// </summary>
        public RDFOntology Annotate(RDFResource annotationProperty, RDFLiteral annotationValue)
        {
            if (annotationProperty == null)
                throw new RDFSemanticsException("Cannot annotate ontology because given \"annotationProperty\" parameter is null");
            if (annotationProperty.IsBlank)
                throw new RDFSemanticsException("Cannot annotate ontology because given \"annotationProperty\" parameter is a blank predicate");
            if (annotationValue == null)
                throw new RDFSemanticsException("Cannot annotate ontology because given \"annotationValue\" parameter is null");

            //Add knowledge to the O-BOX
            OBoxGraph.AddTriple(new RDFTriple(this, annotationProperty, annotationValue));

            return this;
        }

        /// <summary>
        /// Gets a graph representation of the ontology
        /// </summary>
        public RDFGraph ToRDFGraph(bool includeInferences)
            => Model.ToRDFGraph(includeInferences)
                 .UnionWith(Data.ToRDFGraph(includeInferences))
                    .UnionWith(OBoxGraph);

        /// <summary>
        /// Asynchronously gets a graph representation of the ontology
        /// </summary>
        public Task<RDFGraph> ToRDFGraphAsync(bool includeInferences)
            => Task.Run(() => ToRDFGraph(includeInferences));

        /// <summary>
        /// Gets an ontology representation from the given graph
        /// </summary>
        public static RDFOntology FromRDFGraph(RDFGraph graph)
            => RDFOntologyLoader.FromRDFGraph(graph);

        /// <summary>
        /// Asynchronously gets an ontology representation from the given graph
        /// </summary>
        public static Task<RDFOntology> FromRDFGraphAsync(RDFGraph graph)
            => Task.Run(() => FromRDFGraph(graph));
        #endregion
    }
}