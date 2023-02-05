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
using System.Collections;
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
        /// Count of the spatial objects of type sf:LineString
        /// </summary>
        public long LineStringsCount
        {
            get
            {
                long count = 0;
                IEnumerator<RDFResource> lineStringObjects = LineStringsEnumerator;
                while (lineStringObjects.MoveNext())
                    count++;
                return count;
            }
        }

        /// <summary>
        /// Count of the spatial objects of type sf:Polygon
        /// </summary>
        public long PolygonsCount
        {
            get
            {
                long count = 0;
                IEnumerator<RDFResource> polygonObjects = PolygonsEnumerator;
                while (polygonObjects.MoveNext())
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
        /// Gets the enumerator on the spatial objects of type sf:LineString for iteration
        /// </summary>
        public IEnumerator<RDFResource> LineStringsEnumerator
            => Ontology.Data.FindIndividualsOfClass(Ontology.Model, RDFVocabulary.GEOSPARQL.SF.LINESTRING)
                            .GetEnumerator();

        /// <summary>
        /// Gets the enumerator on the spatial objects of type sf:Polygon for iteration
        /// </summary>
        public IEnumerator<RDFResource> PolygonsEnumerator
            => Ontology.Data.FindIndividualsOfClass(Ontology.Model, RDFVocabulary.GEOSPARQL.SF.POLYGON)
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
        IEnumerator<RDFResource> IEnumerable<RDFResource>.GetEnumerator() => SpatialObjectsEnumerator;

        /// <summary>
        /// Exposes an untyped enumerator on the spatial objects for iteration
        /// </summary>
        IEnumerator IEnumerable.GetEnumerator() => SpatialObjectsEnumerator;
        #endregion

        #region Methods
        /// <summary>
        /// Declares the given sf:Point instance to the spatial ontology
        /// </summary>
        public GEOOntology DeclarePoint(RDFResource pointUri, (double,double) point)
        {
            if (pointUri == null)
                throw new OWLSemanticsException("Cannot declare sf:Point instance to the spatial ontology because given \"pointUri\" parameter is null");

            //Build sf:Point instance
            Point sfPoint = new Point(new Coordinate(point.Item1, point.Item2));

            //Add knowledge to the A-BOX
            Ontology.Data.DeclareIndividual(pointUri);
            Ontology.Data.DeclareIndividualType(pointUri, RDFVocabulary.GEOSPARQL.SF.POINT);
            Ontology.Data.DeclareDatatypeAssertion(pointUri, RDFVocabulary.GEOSPARQL.AS_WKT, new RDFTypedLiteral(sfPoint.ToString(), RDFModelEnums.RDFDatatypes.GEOSPARQL_WKT));

            return this;
        }

        /// <summary>
        /// Declares the given sf:LineString instance to the spatial ontology
        /// </summary>
        public GEOOntology DeclareLineString(RDFResource lineStringUri, (double,double)[] points)
        {
            if (lineStringUri == null)
                throw new OWLSemanticsException("Cannot declare sf:LineString instance to the spatial ontology because given \"lineStringUri\" parameter is null");
            if (points == null)
                throw new OWLSemanticsException("Cannot declare sf:LineString instance to the spatial ontology because given \"points\" parameter is null");
            if (points.Length < 2)
                throw new OWLSemanticsException("Cannot declare sf:LineString instance to the spatial ontology because given \"points\" parameter must have at least 2 points");

            //Build sf:LineString instance
            List<Coordinate> sfLineStringPoints = new List<Coordinate>();
            foreach ((double, double) point in points)
                sfLineStringPoints.Add(new Coordinate(point.Item1, point.Item2));
            LineString sfLineString = new LineString(sfLineStringPoints.ToArray());           

            //Add knowledge to the A-BOX
            Ontology.Data.DeclareIndividual(lineStringUri);
            Ontology.Data.DeclareIndividualType(lineStringUri, RDFVocabulary.GEOSPARQL.SF.LINESTRING);
            Ontology.Data.DeclareDatatypeAssertion(lineStringUri, RDFVocabulary.GEOSPARQL.AS_WKT, new RDFTypedLiteral(sfLineString.ToString(), RDFModelEnums.RDFDatatypes.GEOSPARQL_WKT));

            return this;
        }

        /// <summary>
        /// Declares the given sf:Polygon instance to the spatial ontology
        /// </summary>
        public GEOOntology DeclarePolygon(RDFResource polygonUri, (double,double)[] points)
        {
            if (polygonUri == null)
                throw new OWLSemanticsException("Cannot declare sf:Polygon instance to the spatial ontology because given \"polygonUri\" parameter is null");
            if (points == null)
                throw new OWLSemanticsException("Cannot declare sf:Polygon instance to the spatial ontology because given \"points\" parameter is null");
            if (points.Length < 3)
                throw new OWLSemanticsException("Cannot declare sf:Polygon instance to the spatial ontology because given \"points\" parameter must have at least 3 perimeter points");

            //Build sf:Polygon instance (close it automatically if needed)
            List<Coordinate> sfPolygonPoints = new List<Coordinate>();
            foreach ((double, double) point in points)
                sfPolygonPoints.Add(new Coordinate(point.Item1, point.Item2));
            if (!sfPolygonPoints[0].Equals2D(sfPolygonPoints[sfPolygonPoints.Count-1]))
                sfPolygonPoints.Add(sfPolygonPoints[0]);
            Polygon sfPolygon = new Polygon(new LinearRing(sfPolygonPoints.ToArray()));

            //Add knowledge to the A-BOX
            Ontology.Data.DeclareIndividual(polygonUri);
            Ontology.Data.DeclareIndividualType(polygonUri, RDFVocabulary.GEOSPARQL.SF.POLYGON);
            Ontology.Data.DeclareDatatypeAssertion(polygonUri, RDFVocabulary.GEOSPARQL.AS_WKT, new RDFTypedLiteral(sfPolygon.ToString(), RDFModelEnums.RDFDatatypes.GEOSPARQL_WKT));

            return this;
        }
        #endregion
    }
}