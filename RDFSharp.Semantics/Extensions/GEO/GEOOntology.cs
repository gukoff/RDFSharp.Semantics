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

namespace RDFSharp.Semantics.Extensions.GEO
{
    /// <summary>
    /// GEOOntology represents an OWL ontology specialized in describing relations between spatial entities
    /// </summary>
    public class GEOOntology : OWLOntology
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
            => Data.FindIndividualsOfClass(Model, RDFVocabulary.GEOSPARQL.SPATIAL_OBJECT).GetEnumerator();

        /// <summary>
        /// Gets the enumerator on the spatial objects of type sf:Point for iteration
        /// </summary>
        public IEnumerator<RDFResource> PointsEnumerator
            => Data.FindIndividualsOfClass(Model, RDFVocabulary.GEOSPARQL.SF.POINT).GetEnumerator();

        /// <summary>
        /// Gets the enumerator on the spatial objects of type sf:LineString for iteration
        /// </summary>
        public IEnumerator<RDFResource> LineStringsEnumerator
            => Data.FindIndividualsOfClass(Model, RDFVocabulary.GEOSPARQL.SF.LINESTRING).GetEnumerator();

        /// <summary>
        /// Gets the enumerator on the spatial objects of type sf:Polygon for iteration
        /// </summary>
        public IEnumerator<RDFResource> PolygonsEnumerator
            => Data.FindIndividualsOfClass(Model, RDFVocabulary.GEOSPARQL.SF.POLYGON).GetEnumerator();

        /// <summary>
        /// Gets the enumerator on the spatial objects of type sf:MultiPoint for iteration
        /// </summary>
        public IEnumerator<RDFResource> MultiPointsEnumerator
            => Data.FindIndividualsOfClass(Model, RDFVocabulary.GEOSPARQL.SF.MULTI_POINT).GetEnumerator();

        /// <summary>
        /// Gets the enumerator on the spatial objects of type sf:MultiLineString for iteration
        /// </summary>
        public IEnumerator<RDFResource> MultiLineStringsEnumerator
            => Data.FindIndividualsOfClass(Model, RDFVocabulary.GEOSPARQL.SF.MULTI_LINESTRING).GetEnumerator();

        /// <summary>
        /// Gets the enumerator on the spatial objects of type sf:MultiPolygon for iteration
        /// </summary>
        public IEnumerator<RDFResource> MultiPolygonsEnumerator
            => Data.FindIndividualsOfClass(Model, RDFVocabulary.GEOSPARQL.SF.MULTI_POLYGON).GetEnumerator();
        #endregion

        #region Ctors
        /// <summary>
        /// Builds a spatial ontology with the given URI (internal T-BOX is initialized with GeoSPARQL ontology)
        /// </summary>
        public GEOOntology(string geoOntologyURI) : base(geoOntologyURI)
            => Model = GEOOntologyLoader.BuildGEOModel();
        #endregion
    }
}