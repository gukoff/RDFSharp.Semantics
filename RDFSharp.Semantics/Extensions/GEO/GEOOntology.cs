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
using System.Collections;
using System.Linq;

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
        /// Declares the given sf:Point instance to the spatial ontology (coordinate system is EPSG:4326)
        /// </summary>
        public GEOOntology DeclarePoint(RDFResource pointUri, double latitude, double longitude)
        {
            if (pointUri == null)
                throw new OWLSemanticsException("Cannot declare sf:Point instance to the spatial ontology because given \"pointUri\" parameter is null");

            //Build sf:Point instance
            GeographyPoint sfPoint = GeographyFactory.Point(CoordinateSystem.DefaultGeography, latitude, longitude).Build();
            string sfPointWKT = WellKnownTextSqlFormatter.Create().Write(sfPoint).Replace("SRID=4326;", string.Empty);
            string sfPointGML = GmlFormatter.Create().Write(sfPoint);

            //Add knowledge to the A-BOX
            Ontology.Data.DeclareIndividual(pointUri);
            Ontology.Data.DeclareIndividualType(pointUri, RDFVocabulary.GEOSPARQL.SF.POINT);
            Ontology.Data.DeclareDatatypeAssertion(pointUri, RDFVocabulary.GEOSPARQL.AS_GML, new RDFTypedLiteral(sfPointGML, RDFModelEnums.RDFDatatypes.GEOSPARQL_GML));

            return this;
        }

        /// <summary>
        /// Declares the given sf:LineString instance to the spatial ontology (coordinate system is EPSG:4326)
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
            GeographyFactory<GeographyLineString> sfLineStringFactory = GeographyFactory.LineString(CoordinateSystem.DefaultGeography);
            foreach ((double, double) point in points)
                sfLineStringFactory.LineTo(point.Item1, point.Item2);
            GeographyLineString sfLineString = sfLineStringFactory.Build();
            string sfLineStringWKT = WellKnownTextSqlFormatter.Create().Write(sfLineString).Replace("SRID=4326;", string.Empty);
            string sfLineStringGML = GmlFormatter.Create().Write(sfLineString);

            //Add knowledge to the A-BOX
            Ontology.Data.DeclareIndividual(lineStringUri);
            Ontology.Data.DeclareIndividualType(lineStringUri, RDFVocabulary.GEOSPARQL.SF.LINESTRING);
            Ontology.Data.DeclareDatatypeAssertion(lineStringUri, RDFVocabulary.GEOSPARQL.AS_GML, new RDFTypedLiteral(sfLineStringGML, RDFModelEnums.RDFDatatypes.GEOSPARQL_GML));

            return this;
        }

        /// <summary>
        /// Declares the given sf:Polygon instance to the spatial ontology (coordinate system is EPSG:4326)
        /// </summary>
        public GEOOntology DeclarePolygon(RDFResource polygonUri, (double,double)[] points)
        {
            if (polygonUri == null)
                throw new OWLSemanticsException("Cannot declare sf:Polygon instance to the spatial ontology because given \"polygonUri\" parameter is null");
            if (points == null)
                throw new OWLSemanticsException("Cannot declare sf:Polygon instance to the spatial ontology because given \"points\" parameter is null");
            if (points.Length < 3)
                throw new OWLSemanticsException("Cannot declare sf:Polygon instance to the spatial ontology because given \"points\" parameter must have at least 3 perimeter points");

            //Automatically close polygon
            List<(double,double)> sfPolygonPoints = points.ToList();
            if (sfPolygonPoints[0].Item1 != sfPolygonPoints[sfPolygonPoints.Count-1].Item1
                 && sfPolygonPoints[0].Item2 != sfPolygonPoints[sfPolygonPoints.Count-1].Item2)
                sfPolygonPoints.Add(sfPolygonPoints[0]);

            //Build sf:Polygon instance
            GeographyFactory<GeographyPolygon> sfPolygonFactory = GeographyFactory.Polygon(CoordinateSystem.DefaultGeography);
            foreach ((double, double) sfPolygonPoint in sfPolygonPoints)
                sfPolygonFactory.LineTo(sfPolygonPoint.Item1, sfPolygonPoint.Item2);
            GeographyPolygon sfPolygon = sfPolygonFactory.Build();
            string sfPolygonWKT = WellKnownTextSqlFormatter.Create().Write(sfPolygon).Replace("SRID=4326;", string.Empty);
            string sfPolygonGML = GmlFormatter.Create().Write(sfPolygon);

            //Add knowledge to the A-BOX
            Ontology.Data.DeclareIndividual(polygonUri);
            Ontology.Data.DeclareIndividualType(polygonUri, RDFVocabulary.GEOSPARQL.SF.POLYGON);
            Ontology.Data.DeclareDatatypeAssertion(polygonUri, RDFVocabulary.GEOSPARQL.AS_GML, new RDFTypedLiteral(sfPolygonGML, RDFModelEnums.RDFDatatypes.GEOSPARQL_GML));

            return this;
        }
        #endregion
    }
}