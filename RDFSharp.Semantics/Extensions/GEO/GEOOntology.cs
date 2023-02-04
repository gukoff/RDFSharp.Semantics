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
using Microsoft.Spatial;
using System.Collections.Generic;
using System.Linq;
using System.Collections;

namespace RDFSharp.Semantics.Extensions.GEO
{
    /// <summary>
    /// GEOOntology represents an OWL ontology specialized in describing relations between spatial entities
    /// </summary>
    public class GEOOntology : RDFResource, IEnumerable<RDFResource>
    {
        #region Properties
        /// <summary>
        /// Count of the spatial objects
        /// </summary>
        public long SpatialObjectsCount
        {
            get
            {
                long count = 0;
                IEnumerator<RDFResource> spatialObjects = SpatialObjectsEnumerator;
                while (spatialObjects.MoveNext())
                    count++;
                return count;
            }
        }

        /// <summary>
        /// Count of the spatial objects of type sf:Point
        /// </summary>
        public long PointsCount
        {
            get
            {
                long count = 0;
                IEnumerator<RDFResource> pointObjects = PointsEnumerator;
                while (pointObjects.MoveNext())
                    count++;
                return count;
            }
        }

        /// <summary>
        /// Gets the enumerator on the spatial objects for iteration
        /// </summary>
        public IEnumerator<RDFResource> SpatialObjectsEnumerator
            => Ontology.Data.ABoxGraph[null, RDFVocabulary.RDF.TYPE, RDFVocabulary.GEOSPARQL.SPATIAL_OBJECT, null]
                            .Select(t => t.Subject)
                            .OfType<RDFResource>()
                            .GetEnumerator();

        /// <summary>
        /// Gets the enumerator on the spatial objects of type sf:Point for iteration
        /// </summary>
        public IEnumerator<RDFResource> PointsEnumerator
            => Ontology.Data.ABoxGraph[null, RDFVocabulary.RDF.TYPE, new RDFResource("http://www.opengis.net/ont/sf#Point"), null]
                            .Select(t => t.Subject)
                            .OfType<RDFResource>()
                            .GetEnumerator();

        /// <summary>
        /// Knowledge describing the spatial ontology
        /// </summary>
        internal OWLOntology Ontology { get; set; }
        #endregion

        #region Ctors
        /// <summary>
        /// Builds a spatial ontology with the given URI (internal T-BOX is initialized with GeoSPARQL ontology)
        /// </summary>
        public GEOOntology(string geoOntologyURI) : base(geoOntologyURI)
            => Ontology = new OWLOntology(geoOntologyURI) { Model = GEOOntologyLoader.BuildGEOModel() };
        #endregion

        #region Interfaces
        /// <summary>
        /// Exposes a typed enumerator on the spatial objects for iteration
        /// </summary>
        IEnumerator<RDFResource> IEnumerable<RDFResource>.GetEnumerator()
            => SpatialObjectsEnumerator;

        /// <summary>
        /// Exposes an untyped enumerator on the spatial objects for iteration
        /// </summary>
        IEnumerator IEnumerable.GetEnumerator()
            => SpatialObjectsEnumerator;
        #endregion

        #region Methods
        /// <summary>
        /// Declares the given point instance to the spatial ontology (coordinates should be expressed in WGS84)
        /// </summary>
        public GEOOntology DeclarePoint(RDFResource pointUri, double latitude, double longitude)
        {
            if (pointUri == null)
                throw new OWLSemanticsException("Cannot declare point instance to the concept scheme because given \"pointUri\" parameter is null");

            //Add knowledge to the A-BOX
            Ontology.Data.DeclareIndividual(pointUri);
            Ontology.Data.DeclareIndividualType(pointUri, RDFVocabulary.GEOSPARQL.GEOMETRY);
            Ontology.Data.DeclareIndividualType(pointUri, new RDFResource("http://www.opengis.net/ont/sf#Point"));

            //Create spatial point
            GeographyPoint geoPoint = GeographyFactory.Point(latitude, longitude);
            string wktPoint = WellKnownTextSqlFormatter.Create().Write(geoPoint);
            Ontology.Data.DeclareDatatypeAssertion(pointUri, RDFVocabulary.GEOSPARQL.AS_WKT, new RDFPlainLiteral($"{wktPoint}^^{RDFVocabulary.GEOSPARQL.WKT_LITERAL}"));

            return this;
        }
        #endregion
    }
}