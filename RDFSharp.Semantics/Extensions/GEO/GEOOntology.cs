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
using System.Collections.Generic;
using System.Linq;
using System.Collections;
using System.Globalization;
using NetTopologySuite.Geometries;

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
        /// Count of the spatial objects of type sf:Line
        /// </summary>
        public long LinesCount
        {
            get
            {
                long count = 0;
                IEnumerator<RDFResource> lineObjects = LinesEnumerator;
                while (lineObjects.MoveNext())
                    count++;
                return count;
            }
        }

        /// <summary>
        /// Gets the enumerator on the spatial objects for iteration
        /// </summary>
        public IEnumerator<RDFResource> SpatialObjectsEnumerator
            => Ontology.Data.FindIndividualsOfClass(Ontology.Model, RDFVocabulary.GEOSPARQL.SPATIAL_OBJECT)
                            .GetEnumerator();

        /// <summary>
        /// Gets the enumerator on the spatial objects of type sf:Point for iteration
        /// </summary>
        public IEnumerator<RDFResource> PointsEnumerator
            => Ontology.Data.FindIndividualsOfClass(Ontology.Model, RDFVocabulary.GEOSPARQL.SF.POINT)
                            .GetEnumerator();

        /// <summary>
        /// Gets the enumerator on the spatial objects of type sf:Line for iteration
        /// </summary>
        public IEnumerator<RDFResource> LinesEnumerator
            => Ontology.Data.FindIndividualsOfClass(Ontology.Model, RDFVocabulary.GEOSPARQL.SF.LINE)
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
        /// Declares the given sf:Point instance to the spatial ontology. Latitude/Longitude must be expressed in EPSG:4326 (WGS84)
        /// </summary>
        public GEOOntology DeclarePoint(RDFResource pointUri, (double,double) point)
        {
            if (pointUri == null)
                throw new OWLSemanticsException("Cannot declare point instance to the concept scheme because given \"pointUri\" parameter is null");

            //Build sf:Point instance
            Point sfPoint = new Point(new Coordinate(point.Item1, point.Item2));

            //Add knowledge to the A-BOX
            Ontology.Data.DeclareIndividual(pointUri);
            Ontology.Data.DeclareIndividualType(pointUri, RDFVocabulary.GEOSPARQL.SF.POINT);
            Ontology.Data.DeclareDatatypeAssertion(pointUri, RDFVocabulary.GEOSPARQL.AS_WKT, new RDFTypedLiteral(sfPoint.ToString(), RDFModelEnums.RDFDatatypes.GEOSPARQL_WKT));

            return this;
        }

        /// <summary>
        /// Declares the given sf:Line instance to the spatial ontology. Latitude/Longitude pairs must be expressed in EPSG:4326 (WGS84)
        /// </summary>
        public GEOOntology DeclareLine(RDFResource lineUri, (double,double) startPoint, (double,double) endPoint)
        {
            if (lineUri == null)
                throw new OWLSemanticsException("Cannot declare line instance to the concept scheme because given \"lineUri\" parameter is null");

            //Build sf:Line instance
            LineString sfLineString = new LineString(new Coordinate[] { new Coordinate(startPoint.Item1, startPoint.Item2), new Coordinate(endPoint.Item1, endPoint.Item2) });

            //Add knowledge to the A-BOX
            Ontology.Data.DeclareIndividual(lineUri);
            Ontology.Data.DeclareIndividualType(lineUri, RDFVocabulary.GEOSPARQL.SF.LINE);
            Ontology.Data.DeclareDatatypeAssertion(lineUri, RDFVocabulary.GEOSPARQL.AS_WKT, new RDFTypedLiteral(sfLineString.ToString(), RDFModelEnums.RDFDatatypes.GEOSPARQL_WKT));

            return this;
        }
        #endregion
    }
}