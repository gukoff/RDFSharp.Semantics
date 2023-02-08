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
using System.Drawing;
using System.Reflection;

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
        /// Count of the spatial objects of type sf:MultiPoint
        /// </summary>
        public long MultiPointsCount
        {
            get
            {
                long count = 0;
                IEnumerator<RDFResource> multiPointObjects = MultiPointsEnumerator;
                while (multiPointObjects.MoveNext())
                    count++;
                return count;
            }
        }

        /// <summary>
        /// Count of the spatial objects of type sf:MultiLineString
        /// </summary>
        public long MultiLineStringsCount
        {
            get
            {
                long count = 0;
                IEnumerator<RDFResource> multiLineStringObjects = MultiLineStringsEnumerator;
                while (multiLineStringObjects.MoveNext())
                    count++;
                return count;
            }
        }

        /// <summary>
        /// Count of the spatial objects of type sf:MultiPolygon
        /// </summary>
        public long MultiPolygonsCount
        {
            get
            {
                long count = 0;
                IEnumerator<RDFResource> multiPolygonObjects = MultiPolygonsEnumerator;
                while (multiPolygonObjects.MoveNext())
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
        /// Gets the enumerator on the spatial objects of type sf:MultiPoint for iteration
        /// </summary>
        public IEnumerator<RDFResource> MultiPointsEnumerator
            => Ontology.Data.FindIndividualsOfClass(Ontology.Model, RDFVocabulary.GEOSPARQL.SF.MULTI_POINT)
                            .GetEnumerator();

        /// <summary>
        /// Gets the enumerator on the spatial objects of type sf:MultiLineString for iteration
        /// </summary>
        public IEnumerator<RDFResource> MultiLineStringsEnumerator
            => Ontology.Data.FindIndividualsOfClass(Ontology.Model, RDFVocabulary.GEOSPARQL.SF.MULTI_LINESTRING)
                            .GetEnumerator();

        /// <summary>
        /// Gets the enumerator on the spatial objects of type sf:MultiPolygon for iteration
        /// </summary>
        public IEnumerator<RDFResource> MultiPolygonsEnumerator
            => Ontology.Data.FindIndividualsOfClass(Ontology.Model, RDFVocabulary.GEOSPARQL.SF.MULTI_POLYGON)
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
        public GEOOntology DeclarePoint(RDFResource pointUri, double pointLatitude, double pointLongitude)
        {
            if (pointUri == null)
                throw new OWLSemanticsException("Cannot declare sf:Point instance to the spatial ontology because given \"multiPolygonUri\" parameter is null");

            //Build sf:Point instance
            GeographyPoint point = GeographyFactory.Point(CoordinateSystem.DefaultGeography, pointLatitude, pointLongitude).Build();
            string pointGML = GmlFormatter.Create().Write(point);
            string pointWKT = WellKnownTextSqlFormatter.Create().Write(point).Replace("SRID=4326;", "<http://www.opengis.net/def/crs/EPSG/0/4326>");

            //Add knowledge to the A-BOX
            Ontology.Data.DeclareIndividual(pointUri);
            Ontology.Data.DeclareIndividualType(pointUri, RDFVocabulary.GEOSPARQL.SF.POINT);
            Ontology.Data.DeclareDatatypeAssertion(pointUri, RDFVocabulary.GEOSPARQL.AS_GML, new RDFTypedLiteral(pointGML, RDFModelEnums.RDFDatatypes.GEOSPARQL_GML));
            Ontology.Data.DeclareDatatypeAssertion(pointUri, RDFVocabulary.GEOSPARQL.AS_WKT, new RDFTypedLiteral(pointWKT, RDFModelEnums.RDFDatatypes.GEOSPARQL_WKT));

            return this;
        }

        /// <summary>
        /// Declares the given sf:LineString instance to the spatial ontology (coordinate system is EPSG:4326)
        /// </summary>
        public GEOOntology DeclareLineString(RDFResource lineStringUri, List<(double,double)> lineStringPoints)
        {
            if (lineStringUri == null)
                throw new OWLSemanticsException("Cannot declare sf:LineString instance to the spatial ontology because given \"lineStringUri\" parameter is null");
            if (lineStringPoints == null)
                throw new OWLSemanticsException("Cannot declare sf:LineString instance to the spatial ontology because given \"lineStringPoints\" parameter is null");
            if (lineStringPoints.Count < 2)
                throw new OWLSemanticsException("Cannot declare sf:LineString instance to the spatial ontology because given \"lineStringPoints\" parameter must have at least 2 elements");

            //Build sf:LineString instance
            GeographyFactory<GeographyLineString> lineStringFactory = GeographyFactory.LineString(CoordinateSystem.DefaultGeography);
            foreach ((double, double) lineStringPoint in lineStringPoints)
                lineStringFactory.LineTo(lineStringPoint.Item1, lineStringPoint.Item2);
            GeographyLineString lineString = lineStringFactory.Build();
            string lineStringGML = GmlFormatter.Create().Write(lineString);
            string lineStringWKT = WellKnownTextSqlFormatter.Create().Write(lineString).Replace("SRID=4326;", "<http://www.opengis.net/def/crs/EPSG/0/4326>");

            //Add knowledge to the A-BOX
            Ontology.Data.DeclareIndividual(lineStringUri);
            Ontology.Data.DeclareIndividualType(lineStringUri, RDFVocabulary.GEOSPARQL.SF.LINESTRING);
            Ontology.Data.DeclareDatatypeAssertion(lineStringUri, RDFVocabulary.GEOSPARQL.AS_GML, new RDFTypedLiteral(lineStringGML, RDFModelEnums.RDFDatatypes.GEOSPARQL_GML));
            Ontology.Data.DeclareDatatypeAssertion(lineStringUri, RDFVocabulary.GEOSPARQL.AS_WKT, new RDFTypedLiteral(lineStringWKT, RDFModelEnums.RDFDatatypes.GEOSPARQL_WKT));

            return this;
        }

        /// <summary>
        /// Declares the given sf:Polygon instance to the spatial ontology (coordinate system is EPSG:4326)
        /// </summary>
        public GEOOntology DeclarePolygon(RDFResource polygonUri, List<(double,double)> polygonPoints)
        {
            if (polygonUri == null)
                throw new OWLSemanticsException("Cannot declare sf:Polygon instance to the spatial ontology because given \"polygonUri\" parameter is null");
            if (polygonPoints == null)
                throw new OWLSemanticsException("Cannot declare sf:Polygon instance to the spatial ontology because given \"polygonPoints\" parameter is null");
            if (polygonPoints.Count < 3)
                throw new OWLSemanticsException("Cannot declare sf:Polygon instance to the spatial ontology because given \"polygonPoints\" parameter must have at least 3 elements");

            //Automatically close polygon
            if (polygonPoints[0].Item1 != polygonPoints[polygonPoints.Count-1].Item1 
                 && polygonPoints[0].Item2 != polygonPoints[polygonPoints.Count-1].Item2)
                polygonPoints.Add(polygonPoints[0]);

            //Build sf:Polygon instance
            GeographyFactory<GeographyPolygon> polygonFactory = GeographyFactory.Polygon(CoordinateSystem.DefaultGeography);
            foreach ((double, double) polygonPoint in polygonPoints)
                polygonFactory.LineTo(polygonPoint.Item1, polygonPoint.Item2);
            GeographyPolygon polygon = polygonFactory.Build();
            string polygonGML = GmlFormatter.Create().Write(polygon);
            string polygonWKT = WellKnownTextSqlFormatter.Create().Write(polygon).Replace("SRID=4326;", "<http://www.opengis.net/def/crs/EPSG/0/4326>");

            //Add knowledge to the A-BOX
            Ontology.Data.DeclareIndividual(polygonUri);
            Ontology.Data.DeclareIndividualType(polygonUri, RDFVocabulary.GEOSPARQL.SF.POLYGON);
            Ontology.Data.DeclareDatatypeAssertion(polygonUri, RDFVocabulary.GEOSPARQL.AS_GML, new RDFTypedLiteral(polygonGML, RDFModelEnums.RDFDatatypes.GEOSPARQL_GML));
            Ontology.Data.DeclareDatatypeAssertion(polygonUri, RDFVocabulary.GEOSPARQL.AS_WKT, new RDFTypedLiteral(polygonWKT, RDFModelEnums.RDFDatatypes.GEOSPARQL_WKT));

            return this;
        }

        /// <summary>
        /// Declares the given sf:MultiPoint instance to the spatial ontology (coordinate system is EPSG:4326)
        /// </summary>
        public GEOOntology DeclareMultiPoint(RDFResource multiPointUri, List<(double,double)> multiPointElements)
        {
            if (multiPointUri == null)
                throw new OWLSemanticsException("Cannot declare sf:MultiPoint instance to the spatial ontology because given \"multiPolygonUri\" parameter is null");
            if (multiPointElements == null)
                throw new OWLSemanticsException("Cannot declare sf:MultiPoint instance to the spatial ontology because given \"multiPointElements\" parameter is null");
            if (multiPointElements.Count < 2)
                throw new OWLSemanticsException("Cannot declare sf:MultiPoint instance to the spatial ontology because given \"multiPointElements\" parameter must have at least 2 elements");

            //Build sf:MultiPoint instance
            GeographyFactory<GeographyMultiPoint> sfMultiPointFactory = GeographyFactory.MultiPoint(CoordinateSystem.DefaultGeography);
            foreach ((double, double) point in multiPointElements)
                sfMultiPointFactory.Point(point.Item1, point.Item2);
            GeographyMultiPoint sfMultiPoint = sfMultiPointFactory.Build();
            string sfMultiPointGML = GmlFormatter.Create().Write(sfMultiPoint);
            string sfMultiPointWKT = WellKnownTextSqlFormatter.Create().Write(sfMultiPoint).Replace("SRID=4326;", "<http://www.opengis.net/def/crs/EPSG/0/4326>");

            //Add knowledge to the A-BOX
            Ontology.Data.DeclareIndividual(multiPointUri);
            Ontology.Data.DeclareIndividualType(multiPointUri, RDFVocabulary.GEOSPARQL.SF.MULTI_POINT);
            Ontology.Data.DeclareDatatypeAssertion(multiPointUri, RDFVocabulary.GEOSPARQL.AS_GML, new RDFTypedLiteral(sfMultiPointGML, RDFModelEnums.RDFDatatypes.GEOSPARQL_GML));
            Ontology.Data.DeclareDatatypeAssertion(multiPointUri, RDFVocabulary.GEOSPARQL.AS_WKT, new RDFTypedLiteral(sfMultiPointWKT, RDFModelEnums.RDFDatatypes.GEOSPARQL_WKT));

            return this;
        }

        /// <summary>
        /// Declares the given sf:MultiLineString instance to the spatial ontology (coordinate system is EPSG:4326)
        /// </summary>
        public GEOOntology DeclareMultiLineString(RDFResource multiLineStringUri, List<List<(double,double)>> multiLineStringElements)
        {
            if (multiLineStringUri == null)
                throw new OWLSemanticsException("Cannot declare sf:MultiLineString instance to the spatial ontology because given \"multiPolygonUri\" parameter is null");
            if (multiLineStringElements == null)
                throw new OWLSemanticsException("Cannot declare sf:MultiLineString instance to the spatial ontology because given \"multiPolygonElements\" parameter is null");
            if (multiLineStringElements.Count < 2)
                throw new OWLSemanticsException("Cannot declare sf:MultiLineString instance to the spatial ontology because given \"multiPolygonElements\" parameter must have at least 2 elements");
            if (multiLineStringElements.Any(mlsElement => mlsElement == null || mlsElement.Count < 2))
                throw new OWLSemanticsException("Cannot declare sf:MultiLineString instance to the spatial ontology because given \"multiPolygonElements\" parameter contains a null element, or an element with less than 2 items");

            //Build sf:MultiLineString instance
            GeographyFactory<GeographyMultiLineString> sfMultiLineStringFactory = GeographyFactory.MultiLineString(CoordinateSystem.DefaultGeography);
            foreach (List<(double, double)> multiLineStringElement in multiLineStringElements)
            {
                //Model sub-linestring
                sfMultiLineStringFactory.LineString();
                foreach ((double, double) lineStringPoint in multiLineStringElement)
                    sfMultiLineStringFactory.LineTo(lineStringPoint.Item1, lineStringPoint.Item2);
            }                
            GeographyMultiLineString sfMultiLineString = sfMultiLineStringFactory.Build();
            string sfMultiLineStringGML = GmlFormatter.Create().Write(sfMultiLineString);
            string sfMultiLineStringWKT = WellKnownTextSqlFormatter.Create().Write(sfMultiLineString).Replace("SRID=4326;", "<http://www.opengis.net/def/crs/EPSG/0/4326>");

            //Add knowledge to the A-BOX
            Ontology.Data.DeclareIndividual(multiLineStringUri);
            Ontology.Data.DeclareIndividualType(multiLineStringUri, RDFVocabulary.GEOSPARQL.SF.MULTI_LINESTRING);
            Ontology.Data.DeclareDatatypeAssertion(multiLineStringUri, RDFVocabulary.GEOSPARQL.AS_GML, new RDFTypedLiteral(sfMultiLineStringGML, RDFModelEnums.RDFDatatypes.GEOSPARQL_GML));
            Ontology.Data.DeclareDatatypeAssertion(multiLineStringUri, RDFVocabulary.GEOSPARQL.AS_WKT, new RDFTypedLiteral(sfMultiLineStringWKT, RDFModelEnums.RDFDatatypes.GEOSPARQL_WKT));

            return this;
        }

        /// <summary>
        /// Declares the given sf:MultiPolygon instance to the spatial ontology (coordinate system is EPSG:4326)
        /// </summary>
        public GEOOntology DeclareMultiPolygon(RDFResource multiPolygonUri, List<List<(double, double)>> multiPolygonElements)
        {
            if (multiPolygonUri == null)
                throw new OWLSemanticsException("Cannot declare sf:MultiPolygon instance to the spatial ontology because given \"multiPolygonUri\" parameter is null");
            if (multiPolygonElements == null)
                throw new OWLSemanticsException("Cannot declare sf:MultiPolygon instance to the spatial ontology because given \"multiPolygonElements\" parameter is null");
            if (multiPolygonElements.Count < 2)
                throw new OWLSemanticsException("Cannot declare sf:MultiPolygon instance to the spatial ontology because given \"multiPolygonElements\" parameter must have at least 2 elements");
            if (multiPolygonElements.Any(mplElement => mplElement == null || mplElement.Count < 3))
                throw new OWLSemanticsException("Cannot declare sf:MultiPolygon instance to the spatial ontology because given \"multiPolygonElements\" parameter contains a null element, or an element with less than 3 items");

            //Build sf:MultiPolygon instance
            GeographyFactory<GeographyMultiPolygon> sfMultiPolygonFactory = GeographyFactory.MultiPolygon(CoordinateSystem.DefaultGeography);
            foreach (List<(double, double)> multiPolygonElement in multiPolygonElements)
            {
                //Model sub-polygon
                sfMultiPolygonFactory.Polygon();
                foreach ((double, double) polygonPoint in multiPolygonElement)
                    sfMultiPolygonFactory.LineTo(polygonPoint.Item1, polygonPoint.Item2);

                //Automatically close sub-polygon
                if (multiPolygonElement[0].Item1 != multiPolygonElement[multiPolygonElement.Count-1].Item1
                     && multiPolygonElement[0].Item2 != multiPolygonElement[multiPolygonElement.Count-1].Item2)
                    sfMultiPolygonFactory.LineTo(multiPolygonElement[0].Item1, multiPolygonElement[0].Item2);
            }
            GeographyMultiPolygon sfMultiPolygon = sfMultiPolygonFactory.Build();
            string sfMultiPolygonGML = GmlFormatter.Create().Write(sfMultiPolygon);
            string sfMultiPolygonWKT = WellKnownTextSqlFormatter.Create().Write(sfMultiPolygon).Replace("SRID=4326;", "<http://www.opengis.net/def/crs/EPSG/0/4326>");

            //Add knowledge to the A-BOX
            Ontology.Data.DeclareIndividual(multiPolygonUri);
            Ontology.Data.DeclareIndividualType(multiPolygonUri, RDFVocabulary.GEOSPARQL.SF.MULTI_POLYGON);
            Ontology.Data.DeclareDatatypeAssertion(multiPolygonUri, RDFVocabulary.GEOSPARQL.AS_GML, new RDFTypedLiteral(sfMultiPolygonGML, RDFModelEnums.RDFDatatypes.GEOSPARQL_GML));
            Ontology.Data.DeclareDatatypeAssertion(multiPolygonUri, RDFVocabulary.GEOSPARQL.AS_WKT, new RDFTypedLiteral(sfMultiPolygonWKT, RDFModelEnums.RDFDatatypes.GEOSPARQL_WKT));

            return this;
        }
        #endregion
    }
}