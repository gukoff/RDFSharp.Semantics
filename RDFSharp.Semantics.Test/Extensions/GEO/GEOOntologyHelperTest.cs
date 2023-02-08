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

using Microsoft.VisualStudio.TestTools.UnitTesting;
using RDFSharp.Model;
using RDFSharp.Query;
using System.Collections.Generic;

namespace RDFSharp.Semantics.Extensions.GEO.Test
{
    [TestClass]
    public class GEOOntologyHelperTest
    {
        #region Tests
        [TestMethod]
        public void ShouldDeclarePoint()
        {
            GEOOntology geoOnt = new GEOOntology("ex:geoOnt");
            geoOnt.Data.DeclarePoint(new RDFResource("ex:Milan"), 45.4654219, 9.1859243);

            //Test evolution of GEO knowledge
            Assert.IsTrue(geoOnt.URI.Equals(geoOnt.URI));
            Assert.IsTrue(geoOnt.Model.ClassModel.ClassesCount == 19);
            Assert.IsTrue(geoOnt.Model.PropertyModel.PropertiesCount == 34);
            Assert.IsTrue(geoOnt.Data.IndividualsCount == 1);
            Assert.IsTrue(geoOnt.Data.CheckHasIndividual(new RDFResource("ex:Milan")));
            Assert.IsTrue(geoOnt.Data.CheckIsIndividualOf(geoOnt.Model, new RDFResource("ex:Milan"), RDFVocabulary.GEOSPARQL.SPATIAL_OBJECT));
            Assert.IsTrue(geoOnt.Data.CheckIsIndividualOf(geoOnt.Model, new RDFResource("ex:Milan"), RDFVocabulary.GEOSPARQL.GEOMETRY));
            Assert.IsTrue(geoOnt.Data.CheckIsIndividualOf(geoOnt.Model, new RDFResource("ex:Milan"), RDFVocabulary.GEOSPARQL.SF.POINT));
            Assert.IsTrue(geoOnt.Data.CheckHasDatatypeAssertion(new RDFResource("ex:Milan"), RDFVocabulary.GEOSPARQL.AS_GML, new RDFTypedLiteral("<gml:Point srsName=\"http://www.opengis.net/def/crs/EPSG/0/4326\" xmlns:gml=\"http://www.opengis.net/gml\"><gml:pos>45.4654219 9.1859243</gml:pos></gml:Point>", RDFModelEnums.RDFDatatypes.GEOSPARQL_GML)));
            Assert.IsTrue(geoOnt.Data.CheckHasDatatypeAssertion(new RDFResource("ex:Milan"), RDFVocabulary.GEOSPARQL.AS_WKT, new RDFTypedLiteral("<http://www.opengis.net/def/crs/EPSG/0/4326>POINT (9.1859243 45.4654219)", RDFModelEnums.RDFDatatypes.GEOSPARQL_WKT)));

            //Test counters and enumerators
            Assert.IsTrue(geoOnt.SpatialObjectsCount == 1);
            int so1 = 0;
            IEnumerator<RDFResource> spatialObjectsEnumerator = geoOnt.SpatialObjectsEnumerator;
            while (spatialObjectsEnumerator.MoveNext()) so1++;
            Assert.IsTrue(so1 == 1);

            Assert.IsTrue(geoOnt.PointsCount == 1);
            int pt = 0;
            IEnumerator<RDFResource> pointsEnumerator = geoOnt.PointsEnumerator;
            while (pointsEnumerator.MoveNext()) pt++;
            Assert.IsTrue(pt == 1);

            Assert.IsTrue(geoOnt.LineStringsCount == 0);
            int ls = 0;
            IEnumerator<RDFResource> lineStringsEnumerator = geoOnt.LineStringsEnumerator;
            while (lineStringsEnumerator.MoveNext()) ls++;
            Assert.IsTrue(ls == 0);

            Assert.IsTrue(geoOnt.PolygonsCount == 0);
            int pl = 0;
            IEnumerator<RDFResource> polygonsEnumerator = geoOnt.PolygonsEnumerator;
            while (polygonsEnumerator.MoveNext()) pl++;
            Assert.IsTrue(pl == 0);

            Assert.IsTrue(geoOnt.MultiPointsCount == 0);
            int mpt = 0;
            IEnumerator<RDFResource> multiPointsEnumerator = geoOnt.MultiPointsEnumerator;
            while (multiPointsEnumerator.MoveNext()) mpt++;
            Assert.IsTrue(mpt == 0);

            Assert.IsTrue(geoOnt.MultiLineStringsCount == 0);
            int mls = 0;
            IEnumerator<RDFResource> multiLineStringsEnumerator = geoOnt.MultiLineStringsEnumerator;
            while (multiLineStringsEnumerator.MoveNext()) mls++;
            Assert.IsTrue(mls == 0);

            Assert.IsTrue(geoOnt.MultiPolygonsCount == 0);
            int mpl = 0;
            IEnumerator<RDFResource> multiPolygonsEnumerator = geoOnt.MultiPolygonsEnumerator;
            while (multiPolygonsEnumerator.MoveNext()) mpl++;
            Assert.IsTrue(mpl == 0);
        }

        [TestMethod]
        public void ShouldThrowExceptionOnDeclaringPointBecauseNullUri()
            => Assert.ThrowsException<OWLSemanticsException>(() => new GEOOntology("ex:geoOnt").Data.DeclarePoint(null, 0, 0));

        [TestMethod]
        public void ShouldDeclareLineString()
        {
            GEOOntology geoOnt = new GEOOntology("ex:geoOnt");
            geoOnt.Data.DeclareLineString(new RDFResource("ex:MilanToRomeToNaples"), new List<(double, double)>() { (45.4654219, 9.1859243), (41.902784, 12.496366), (40.8517746, 14.2681244) });

            //Test evolution of GEO knowledge
            Assert.IsTrue(geoOnt.URI.Equals(geoOnt.URI));
            Assert.IsTrue(geoOnt.Model.ClassModel.ClassesCount == 19);
            Assert.IsTrue(geoOnt.Model.PropertyModel.PropertiesCount == 34);
            Assert.IsTrue(geoOnt.Data.IndividualsCount == 1);
            Assert.IsTrue(geoOnt.Data.CheckHasIndividual(new RDFResource("ex:MilanToRomeToNaples")));
            Assert.IsTrue(geoOnt.Data.CheckIsIndividualOf(geoOnt.Model, new RDFResource("ex:MilanToRomeToNaples"), RDFVocabulary.GEOSPARQL.SPATIAL_OBJECT));
            Assert.IsTrue(geoOnt.Data.CheckIsIndividualOf(geoOnt.Model, new RDFResource("ex:MilanToRomeToNaples"), RDFVocabulary.GEOSPARQL.GEOMETRY));
            Assert.IsTrue(geoOnt.Data.CheckIsIndividualOf(geoOnt.Model, new RDFResource("ex:MilanToRomeToNaples"), RDFVocabulary.GEOSPARQL.SF.LINESTRING));
            Assert.IsTrue(geoOnt.Data.CheckHasDatatypeAssertion(new RDFResource("ex:MilanToRomeToNaples"), RDFVocabulary.GEOSPARQL.AS_GML, new RDFTypedLiteral("<gml:LineString srsName=\"http://www.opengis.net/def/crs/EPSG/0/4326\" xmlns:gml=\"http://www.opengis.net/gml\"><gml:pos>45.4654219 9.1859243</gml:pos><gml:pos>41.902784 12.496366</gml:pos><gml:pos>40.8517746 14.2681244</gml:pos></gml:LineString>", RDFModelEnums.RDFDatatypes.GEOSPARQL_GML)));
            Assert.IsTrue(geoOnt.Data.CheckHasDatatypeAssertion(new RDFResource("ex:MilanToRomeToNaples"), RDFVocabulary.GEOSPARQL.AS_WKT, new RDFTypedLiteral("<http://www.opengis.net/def/crs/EPSG/0/4326>LINESTRING (9.1859243 45.4654219, 12.496366 41.902784, 14.2681244 40.8517746)", RDFModelEnums.RDFDatatypes.GEOSPARQL_WKT)));

            //Test counters and enumerators
            Assert.IsTrue(geoOnt.SpatialObjectsCount == 1);
            int so1 = 0;
            IEnumerator<RDFResource> spatialObjectsEnumerator = geoOnt.SpatialObjectsEnumerator;
            while (spatialObjectsEnumerator.MoveNext()) so1++;
            Assert.IsTrue(so1 == 1);

            Assert.IsTrue(geoOnt.PointsCount == 0);
            int pt = 0;
            IEnumerator<RDFResource> pointsEnumerator = geoOnt.PointsEnumerator;
            while (pointsEnumerator.MoveNext()) pt++;
            Assert.IsTrue(pt == 0);

            Assert.IsTrue(geoOnt.LineStringsCount == 1);
            int ls = 0;
            IEnumerator<RDFResource> lineStringsEnumerator = geoOnt.LineStringsEnumerator;
            while (lineStringsEnumerator.MoveNext()) ls++;
            Assert.IsTrue(ls == 1);

            Assert.IsTrue(geoOnt.PolygonsCount == 0);
            int pl = 0;
            IEnumerator<RDFResource> polygonsEnumerator = geoOnt.PolygonsEnumerator;
            while (polygonsEnumerator.MoveNext()) pl++;
            Assert.IsTrue(pl == 0);

            Assert.IsTrue(geoOnt.MultiPointsCount == 0);
            int mpt = 0;
            IEnumerator<RDFResource> multiPointsEnumerator = geoOnt.MultiPointsEnumerator;
            while (multiPointsEnumerator.MoveNext()) mpt++;
            Assert.IsTrue(mpt == 0);

            Assert.IsTrue(geoOnt.MultiLineStringsCount == 0);
            int mls = 0;
            IEnumerator<RDFResource> multiLineStringsEnumerator = geoOnt.MultiLineStringsEnumerator;
            while (multiLineStringsEnumerator.MoveNext()) mls++;
            Assert.IsTrue(mls == 0);

            Assert.IsTrue(geoOnt.MultiPolygonsCount == 0);
            int mpl = 0;
            IEnumerator<RDFResource> multiPolygonsEnumerator = geoOnt.MultiPolygonsEnumerator;
            while (multiPolygonsEnumerator.MoveNext()) mpl++;
            Assert.IsTrue(mpl == 0);
        }

        [TestMethod]
        public void ShouldThrowExceptionOnDeclaringLineStringBecauseNullUri()
            => Assert.ThrowsException<OWLSemanticsException>(() => new GEOOntology("ex:geoOnt").Data.DeclareLineString(null, new List<(double, double)>() { (0d, 0d), (0d, 0d) }));

        [TestMethod]
        public void ShouldThrowExceptionOnDeclaringLineStringBecauseNullPoints()
            => Assert.ThrowsException<OWLSemanticsException>(() => new GEOOntology("ex:geoOnt").Data.DeclareLineString(new RDFResource("ex:lineString"), null));

        [TestMethod]
        public void ShouldThrowExceptionOnDeclaringLineStringBecauseLessThan2Points()
            => Assert.ThrowsException<OWLSemanticsException>(() => new GEOOntology("ex:geoOnt").Data.DeclareLineString(new RDFResource("ex:lineString"), new List<(double, double)>() { (0d, 0d) }));

        [TestMethod]
        public void ShouldDeclareOpenPolygon()
        {
            GEOOntology geoOnt = new GEOOntology("ex:geoOnt");
            geoOnt.Data.DeclarePolygon(new RDFResource("ex:MilanToRomeToNaples"), new List<(double, double)>() { (45.4654219, 9.1859243), (41.902784, 12.496366), (40.8517746, 14.2681244) });

            //Test evolution of GEO knowledge
            Assert.IsTrue(geoOnt.URI.Equals(geoOnt.URI));
            Assert.IsTrue(geoOnt.Model.ClassModel.ClassesCount == 19);
            Assert.IsTrue(geoOnt.Model.PropertyModel.PropertiesCount == 34);
            Assert.IsTrue(geoOnt.Data.IndividualsCount == 1);
            Assert.IsTrue(geoOnt.Data.CheckHasIndividual(new RDFResource("ex:MilanToRomeToNaples")));
            Assert.IsTrue(geoOnt.Data.CheckIsIndividualOf(geoOnt.Model, new RDFResource("ex:MilanToRomeToNaples"), RDFVocabulary.GEOSPARQL.SPATIAL_OBJECT));
            Assert.IsTrue(geoOnt.Data.CheckIsIndividualOf(geoOnt.Model, new RDFResource("ex:MilanToRomeToNaples"), RDFVocabulary.GEOSPARQL.GEOMETRY));
            Assert.IsTrue(geoOnt.Data.CheckIsIndividualOf(geoOnt.Model, new RDFResource("ex:MilanToRomeToNaples"), RDFVocabulary.GEOSPARQL.SF.POLYGON));
            Assert.IsTrue(geoOnt.Data.CheckHasDatatypeAssertion(new RDFResource("ex:MilanToRomeToNaples"), RDFVocabulary.GEOSPARQL.AS_GML, new RDFTypedLiteral("<gml:Polygon srsName=\"http://www.opengis.net/def/crs/EPSG/0/4326\" xmlns:gml=\"http://www.opengis.net/gml\"><gml:exterior><gml:LinearRing><gml:pos>45.4654219 9.1859243</gml:pos><gml:pos>41.902784 12.496366</gml:pos><gml:pos>40.8517746 14.2681244</gml:pos><gml:pos>45.4654219 9.1859243</gml:pos></gml:LinearRing></gml:exterior></gml:Polygon>", RDFModelEnums.RDFDatatypes.GEOSPARQL_GML)));
            Assert.IsTrue(geoOnt.Data.CheckHasDatatypeAssertion(new RDFResource("ex:MilanToRomeToNaples"), RDFVocabulary.GEOSPARQL.AS_WKT, new RDFTypedLiteral("<http://www.opengis.net/def/crs/EPSG/0/4326>POLYGON ((9.1859243 45.4654219, 12.496366 41.902784, 14.2681244 40.8517746, 9.1859243 45.4654219))", RDFModelEnums.RDFDatatypes.GEOSPARQL_WKT)));

            //Test counters and enumerators
            Assert.IsTrue(geoOnt.SpatialObjectsCount == 1);
            int so1 = 0;
            IEnumerator<RDFResource> spatialObjectsEnumerator = geoOnt.SpatialObjectsEnumerator;
            while (spatialObjectsEnumerator.MoveNext()) so1++;
            Assert.IsTrue(so1 == 1);

            Assert.IsTrue(geoOnt.PointsCount == 0);
            int pt = 0;
            IEnumerator<RDFResource> pointsEnumerator = geoOnt.PointsEnumerator;
            while (pointsEnumerator.MoveNext()) pt++;
            Assert.IsTrue(pt == 0);

            Assert.IsTrue(geoOnt.LineStringsCount == 0);
            int ls = 0;
            IEnumerator<RDFResource> lineStringsEnumerator = geoOnt.LineStringsEnumerator;
            while (lineStringsEnumerator.MoveNext()) ls++;
            Assert.IsTrue(ls == 0);

            Assert.IsTrue(geoOnt.PolygonsCount == 1);
            int pl = 0;
            IEnumerator<RDFResource> polygonsEnumerator = geoOnt.PolygonsEnumerator;
            while (polygonsEnumerator.MoveNext()) pl++;
            Assert.IsTrue(pl == 1);

            Assert.IsTrue(geoOnt.MultiPointsCount == 0);
            int mpt = 0;
            IEnumerator<RDFResource> multiPointsEnumerator = geoOnt.MultiPointsEnumerator;
            while (multiPointsEnumerator.MoveNext()) mpt++;
            Assert.IsTrue(mpt == 0);

            Assert.IsTrue(geoOnt.MultiLineStringsCount == 0);
            int mls = 0;
            IEnumerator<RDFResource> multiLineStringsEnumerator = geoOnt.MultiLineStringsEnumerator;
            while (multiLineStringsEnumerator.MoveNext()) mls++;
            Assert.IsTrue(mls == 0);

            Assert.IsTrue(geoOnt.MultiPolygonsCount == 0);
            int mpl = 0;
            IEnumerator<RDFResource> multiPolygonsEnumerator = geoOnt.MultiPolygonsEnumerator;
            while (multiPolygonsEnumerator.MoveNext()) mpl++;
            Assert.IsTrue(mpl == 0);
        }

        [TestMethod]
        public void ShouldDeclareClosedPolygon()
        {
            GEOOntology geoOnt = new GEOOntology("ex:geoOnt");
            geoOnt.Data.DeclarePolygon(new RDFResource("ex:MilanToRomeToNaplesToMilan"), new List<(double, double)>() { (45.4654219, 9.1859243), (41.902784, 12.496366), (40.8517746, 14.2681244), (45.4654219, 9.1859243) });

            //Test evolution of GEO knowledge
            Assert.IsTrue(geoOnt.URI.Equals(geoOnt.URI));
            Assert.IsTrue(geoOnt.Model.ClassModel.ClassesCount == 19);
            Assert.IsTrue(geoOnt.Model.PropertyModel.PropertiesCount == 34);
            Assert.IsTrue(geoOnt.Data.IndividualsCount == 1);
            Assert.IsTrue(geoOnt.Data.CheckHasIndividual(new RDFResource("ex:MilanToRomeToNaplesToMilan")));
            Assert.IsTrue(geoOnt.Data.CheckIsIndividualOf(geoOnt.Model, new RDFResource("ex:MilanToRomeToNaplesToMilan"), RDFVocabulary.GEOSPARQL.SPATIAL_OBJECT));
            Assert.IsTrue(geoOnt.Data.CheckIsIndividualOf(geoOnt.Model, new RDFResource("ex:MilanToRomeToNaplesToMilan"), RDFVocabulary.GEOSPARQL.GEOMETRY));
            Assert.IsTrue(geoOnt.Data.CheckIsIndividualOf(geoOnt.Model, new RDFResource("ex:MilanToRomeToNaplesToMilan"), RDFVocabulary.GEOSPARQL.SF.POLYGON));
            Assert.IsTrue(geoOnt.Data.CheckHasDatatypeAssertion(new RDFResource("ex:MilanToRomeToNaplesToMilan"), RDFVocabulary.GEOSPARQL.AS_GML, new RDFTypedLiteral("<gml:Polygon srsName=\"http://www.opengis.net/def/crs/EPSG/0/4326\" xmlns:gml=\"http://www.opengis.net/gml\"><gml:exterior><gml:LinearRing><gml:pos>45.4654219 9.1859243</gml:pos><gml:pos>41.902784 12.496366</gml:pos><gml:pos>40.8517746 14.2681244</gml:pos><gml:pos>45.4654219 9.1859243</gml:pos></gml:LinearRing></gml:exterior></gml:Polygon>", RDFModelEnums.RDFDatatypes.GEOSPARQL_GML)));
            Assert.IsTrue(geoOnt.Data.CheckHasDatatypeAssertion(new RDFResource("ex:MilanToRomeToNaplesToMilan"), RDFVocabulary.GEOSPARQL.AS_WKT, new RDFTypedLiteral("<http://www.opengis.net/def/crs/EPSG/0/4326>POLYGON ((9.1859243 45.4654219, 12.496366 41.902784, 14.2681244 40.8517746, 9.1859243 45.4654219))", RDFModelEnums.RDFDatatypes.GEOSPARQL_WKT)));

            //Test counters and enumerators
            Assert.IsTrue(geoOnt.SpatialObjectsCount == 1);
            int so1 = 0;
            IEnumerator<RDFResource> spatialObjectsEnumerator = geoOnt.SpatialObjectsEnumerator;
            while (spatialObjectsEnumerator.MoveNext()) so1++;
            Assert.IsTrue(so1 == 1);

            Assert.IsTrue(geoOnt.PointsCount == 0);
            int pt = 0;
            IEnumerator<RDFResource> pointsEnumerator = geoOnt.PointsEnumerator;
            while (pointsEnumerator.MoveNext()) pt++;
            Assert.IsTrue(pt == 0);

            Assert.IsTrue(geoOnt.LineStringsCount == 0);
            int ls = 0;
            IEnumerator<RDFResource> lineStringsEnumerator = geoOnt.LineStringsEnumerator;
            while (lineStringsEnumerator.MoveNext()) ls++;
            Assert.IsTrue(ls == 0);

            Assert.IsTrue(geoOnt.PolygonsCount == 1);
            int pl = 0;
            IEnumerator<RDFResource> polygonsEnumerator = geoOnt.PolygonsEnumerator;
            while (polygonsEnumerator.MoveNext()) pl++;
            Assert.IsTrue(pl == 1);

            Assert.IsTrue(geoOnt.MultiPointsCount == 0);
            int mpt = 0;
            IEnumerator<RDFResource> multiPointsEnumerator = geoOnt.MultiPointsEnumerator;
            while (multiPointsEnumerator.MoveNext()) mpt++;
            Assert.IsTrue(mpt == 0);

            Assert.IsTrue(geoOnt.MultiLineStringsCount == 0);
            int mls = 0;
            IEnumerator<RDFResource> multiLineStringsEnumerator = geoOnt.MultiLineStringsEnumerator;
            while (multiLineStringsEnumerator.MoveNext()) mls++;
            Assert.IsTrue(mls == 0);

            Assert.IsTrue(geoOnt.MultiPolygonsCount == 0);
            int mpl = 0;
            IEnumerator<RDFResource> multiPolygonsEnumerator = geoOnt.MultiPolygonsEnumerator;
            while (multiPolygonsEnumerator.MoveNext()) mpl++;
            Assert.IsTrue(mpl == 0);
        }

        [TestMethod]
        public void ShouldThrowExceptionOnDeclaringPolygonBecauseNullUri()
            => Assert.ThrowsException<OWLSemanticsException>(() => new GEOOntology("ex:geoOnt").Data.DeclarePolygon(null, new List<(double, double)>() { (0d, 0d), (0d, 0d) }));

        [TestMethod]
        public void ShouldThrowExceptionOnDeclaringPolygonBecauseNullPoints()
            => Assert.ThrowsException<OWLSemanticsException>(() => new GEOOntology("ex:geoOnt").Data.DeclarePolygon(new RDFResource("ex:polygon"), null));

        [TestMethod]
        public void ShouldThrowExceptionOnDeclaringPolygonBecauseLessThan3Points()
            => Assert.ThrowsException<OWLSemanticsException>(() => new GEOOntology("ex:geoOnt").Data.DeclarePolygon(new RDFResource("ex:polygon"), new List<(double, double)>() { (0d, 0d), (0d, 0d) }));

        [TestMethod]
        public void ShouldDeclareMultiPoint()
        {
            GEOOntology geoOnt = new GEOOntology("ex:geoOnt");
            geoOnt.Data.DeclareMultiPoint(new RDFResource("ex:MilanAndRomeAndNaples"), new List<(double, double)>() { (45.4654219, 9.1859243), (41.902784, 12.496366), (40.8517746, 14.2681244) });

            //Test evolution of GEO knowledge
            Assert.IsTrue(geoOnt.URI.Equals(geoOnt.URI));
            Assert.IsTrue(geoOnt.Model.ClassModel.ClassesCount == 19);
            Assert.IsTrue(geoOnt.Model.PropertyModel.PropertiesCount == 34);
            Assert.IsTrue(geoOnt.Data.IndividualsCount == 1);
            Assert.IsTrue(geoOnt.Data.CheckHasIndividual(new RDFResource("ex:MilanAndRomeAndNaples")));
            Assert.IsTrue(geoOnt.Data.CheckIsIndividualOf(geoOnt.Model, new RDFResource("ex:MilanAndRomeAndNaples"), RDFVocabulary.GEOSPARQL.SPATIAL_OBJECT));
            Assert.IsTrue(geoOnt.Data.CheckIsIndividualOf(geoOnt.Model, new RDFResource("ex:MilanAndRomeAndNaples"), RDFVocabulary.GEOSPARQL.GEOMETRY));
            Assert.IsTrue(geoOnt.Data.CheckIsIndividualOf(geoOnt.Model, new RDFResource("ex:MilanAndRomeAndNaples"), RDFVocabulary.GEOSPARQL.SF.MULTI_POINT));
            Assert.IsTrue(geoOnt.Data.CheckHasDatatypeAssertion(new RDFResource("ex:MilanAndRomeAndNaples"), RDFVocabulary.GEOSPARQL.AS_GML, new RDFTypedLiteral("<gml:MultiPoint srsName=\"http://www.opengis.net/def/crs/EPSG/0/4326\" xmlns:gml=\"http://www.opengis.net/gml\"><gml:pointMembers><gml:Point><gml:pos>45.4654219 9.1859243</gml:pos></gml:Point><gml:Point><gml:pos>41.902784 12.496366</gml:pos></gml:Point><gml:Point><gml:pos>40.8517746 14.2681244</gml:pos></gml:Point></gml:pointMembers></gml:MultiPoint>", RDFModelEnums.RDFDatatypes.GEOSPARQL_GML)));
            Assert.IsTrue(geoOnt.Data.CheckHasDatatypeAssertion(new RDFResource("ex:MilanAndRomeAndNaples"), RDFVocabulary.GEOSPARQL.AS_WKT, new RDFTypedLiteral("<http://www.opengis.net/def/crs/EPSG/0/4326>MULTIPOINT ((9.1859243 45.4654219), (12.496366 41.902784), (14.2681244 40.8517746))", RDFModelEnums.RDFDatatypes.GEOSPARQL_WKT)));

            //Test counters and enumerators
            Assert.IsTrue(geoOnt.SpatialObjectsCount == 1);
            int so1 = 0;
            IEnumerator<RDFResource> spatialObjectsEnumerator = geoOnt.SpatialObjectsEnumerator;
            while (spatialObjectsEnumerator.MoveNext()) so1++;
            Assert.IsTrue(so1 == 1);

            Assert.IsTrue(geoOnt.PointsCount == 0);
            int pt = 0;
            IEnumerator<RDFResource> pointsEnumerator = geoOnt.PointsEnumerator;
            while (pointsEnumerator.MoveNext()) pt++;
            Assert.IsTrue(pt == 0);

            Assert.IsTrue(geoOnt.LineStringsCount == 0);
            int ls = 0;
            IEnumerator<RDFResource> lineStringsEnumerator = geoOnt.LineStringsEnumerator;
            while (lineStringsEnumerator.MoveNext()) ls++;
            Assert.IsTrue(ls == 0);

            Assert.IsTrue(geoOnt.PolygonsCount == 0);
            int pl = 0;
            IEnumerator<RDFResource> polygonsEnumerator = geoOnt.PolygonsEnumerator;
            while (polygonsEnumerator.MoveNext()) pl++;
            Assert.IsTrue(pl == 0);

            Assert.IsTrue(geoOnt.MultiPointsCount == 1);
            int mpt = 0;
            IEnumerator<RDFResource> multiPointsEnumerator = geoOnt.MultiPointsEnumerator;
            while (multiPointsEnumerator.MoveNext()) mpt++;
            Assert.IsTrue(mpt == 1);

            Assert.IsTrue(geoOnt.MultiLineStringsCount == 0);
            int mls = 0;
            IEnumerator<RDFResource> multiLineStringsEnumerator = geoOnt.MultiLineStringsEnumerator;
            while (multiLineStringsEnumerator.MoveNext()) mls++;
            Assert.IsTrue(mls == 0);

            Assert.IsTrue(geoOnt.MultiPolygonsCount == 0);
            int mpl = 0;
            IEnumerator<RDFResource> multiPolygonsEnumerator = geoOnt.MultiPolygonsEnumerator;
            while (multiPolygonsEnumerator.MoveNext()) mpl++;
            Assert.IsTrue(mpl == 0);
        }

        [TestMethod]
        public void ShouldThrowExceptionOnDeclaringMultiPointBecauseNullUri()
            => Assert.ThrowsException<OWLSemanticsException>(() => new GEOOntology("ex:geoOnt").Data.DeclareMultiPoint(null, new List<(double, double)>() { (0d, 0d), (0d, 0d) }));

        [TestMethod]
        public void ShouldThrowExceptionOnDeclaringMultiPointBecauseNullPoints()
            => Assert.ThrowsException<OWLSemanticsException>(() => new GEOOntology("ex:geoOnt").Data.DeclareMultiPoint(new RDFResource("ex:multiPoint"), null));

        [TestMethod]
        public void ShouldThrowExceptionOnDeclaringMultiPointBecauseLessThan2Points()
            => Assert.ThrowsException<OWLSemanticsException>(() => new GEOOntology("ex:geoOnt").Data.DeclareMultiPoint(new RDFResource("ex:multiPoint"), new List<(double, double)>() { (0d, 0d) }));

        [TestMethod]
        public void ShouldDeclareMultiLineString()
        {
            GEOOntology geoOnt = new GEOOntology("ex:geoOnt");
            geoOnt.Data.DeclareMultiLineString(new RDFResource("ex:MilanToRomeAndRomeToNaples"), 
                new List<List<(double, double)>>() { 
                    new List<(double, double)>() { (45.4654219, 9.1859243), (41.902784,  12.496366) },
                    new List<(double, double)>() { (41.902784,  12.496366), (40.8517746, 14.2681244) }
                });

            //Test evolution of GEO knowledge
            Assert.IsTrue(geoOnt.URI.Equals(geoOnt.URI));
            Assert.IsTrue(geoOnt.Model.ClassModel.ClassesCount == 19);
            Assert.IsTrue(geoOnt.Model.PropertyModel.PropertiesCount == 34);
            Assert.IsTrue(geoOnt.Data.IndividualsCount == 1);
            Assert.IsTrue(geoOnt.Data.CheckHasIndividual(new RDFResource("ex:MilanToRomeAndRomeToNaples")));
            Assert.IsTrue(geoOnt.Data.CheckIsIndividualOf(geoOnt.Model, new RDFResource("ex:MilanToRomeAndRomeToNaples"), RDFVocabulary.GEOSPARQL.SPATIAL_OBJECT));
            Assert.IsTrue(geoOnt.Data.CheckIsIndividualOf(geoOnt.Model, new RDFResource("ex:MilanToRomeAndRomeToNaples"), RDFVocabulary.GEOSPARQL.GEOMETRY));
            Assert.IsTrue(geoOnt.Data.CheckIsIndividualOf(geoOnt.Model, new RDFResource("ex:MilanToRomeAndRomeToNaples"), RDFVocabulary.GEOSPARQL.SF.MULTI_LINESTRING));
            Assert.IsTrue(geoOnt.Data.CheckHasDatatypeAssertion(new RDFResource("ex:MilanToRomeAndRomeToNaples"), RDFVocabulary.GEOSPARQL.AS_GML, new RDFTypedLiteral("<gml:MultiCurve srsName=\"http://www.opengis.net/def/crs/EPSG/0/4326\" xmlns:gml=\"http://www.opengis.net/gml\"><gml:curveMembers><gml:LineString><gml:pos>45.4654219 9.1859243</gml:pos><gml:pos>41.902784 12.496366</gml:pos></gml:LineString><gml:LineString><gml:pos>41.902784 12.496366</gml:pos><gml:pos>40.8517746 14.2681244</gml:pos></gml:LineString></gml:curveMembers></gml:MultiCurve>", RDFModelEnums.RDFDatatypes.GEOSPARQL_GML)));
            Assert.IsTrue(geoOnt.Data.CheckHasDatatypeAssertion(new RDFResource("ex:MilanToRomeAndRomeToNaples"), RDFVocabulary.GEOSPARQL.AS_WKT, new RDFTypedLiteral("<http://www.opengis.net/def/crs/EPSG/0/4326>MULTILINESTRING ((9.1859243 45.4654219, 12.496366 41.902784), (12.496366 41.902784, 14.2681244 40.8517746))", RDFModelEnums.RDFDatatypes.GEOSPARQL_WKT)));

            //Test counters and enumerators
            Assert.IsTrue(geoOnt.SpatialObjectsCount == 1);
            int so1 = 0;
            IEnumerator<RDFResource> spatialObjectsEnumerator = geoOnt.SpatialObjectsEnumerator;
            while (spatialObjectsEnumerator.MoveNext()) so1++;
            Assert.IsTrue(so1 == 1);

            Assert.IsTrue(geoOnt.PointsCount == 0);
            int pt = 0;
            IEnumerator<RDFResource> pointsEnumerator = geoOnt.PointsEnumerator;
            while (pointsEnumerator.MoveNext()) pt++;
            Assert.IsTrue(pt == 0);

            Assert.IsTrue(geoOnt.LineStringsCount == 0);
            int ls = 0;
            IEnumerator<RDFResource> lineStringsEnumerator = geoOnt.LineStringsEnumerator;
            while (lineStringsEnumerator.MoveNext()) ls++;
            Assert.IsTrue(ls == 0);

            Assert.IsTrue(geoOnt.PolygonsCount == 0);
            int pl = 0;
            IEnumerator<RDFResource> polygonsEnumerator = geoOnt.PolygonsEnumerator;
            while (polygonsEnumerator.MoveNext()) pl++;
            Assert.IsTrue(pl == 0);

            Assert.IsTrue(geoOnt.MultiPointsCount == 0);
            int mpt = 0;
            IEnumerator<RDFResource> multiPointsEnumerator = geoOnt.MultiPointsEnumerator;
            while (multiPointsEnumerator.MoveNext()) mpt++;
            Assert.IsTrue(mpt == 0);

            Assert.IsTrue(geoOnt.MultiLineStringsCount == 1);
            int mls = 0;
            IEnumerator<RDFResource> multiLineStringsEnumerator = geoOnt.MultiLineStringsEnumerator;
            while (multiLineStringsEnumerator.MoveNext()) mls++;
            Assert.IsTrue(mls == 1);

            Assert.IsTrue(geoOnt.MultiPolygonsCount == 0);
            int mpl = 0;
            IEnumerator<RDFResource> multiPolygonsEnumerator = geoOnt.MultiPolygonsEnumerator;
            while (multiPolygonsEnumerator.MoveNext()) mpl++;
            Assert.IsTrue(mpl == 0);
        }

        [TestMethod]
        public void ShouldThrowExceptionOnDeclaringMultiLineStringBecauseNullUri()
            => Assert.ThrowsException<OWLSemanticsException>(() => new GEOOntology("ex:geoOnt").Data.DeclareMultiLineString(null, new List<List<(double, double)>>() { new List<(double, double)>() { (0d, 0d), (0d, 0d) }, new List<(double, double)>() { (1d, 1d), (1d, 1d) } }));

        [TestMethod]
        public void ShouldThrowExceptionOnDeclaringMultiLineStringBecauseNullLineStrings()
            => Assert.ThrowsException<OWLSemanticsException>(() => new GEOOntology("ex:geoOnt").Data.DeclareMultiLineString(new RDFResource("ex:multiLineString"), null));

        [TestMethod]
        public void ShouldThrowExceptionOnDeclaringMultiLineStringBecauseLessThan2LineStrings()
            => Assert.ThrowsException<OWLSemanticsException>(() => new GEOOntology("ex:geoOnt").Data.DeclareMultiLineString(new RDFResource("ex:multiLineString"), new List<List<(double, double)>>() { new List<(double, double)>() { (0d, 0d), (0d, 0d) } }));

        [TestMethod]
        public void ShouldThrowExceptionOnDeclaringMultiLineStringBecauseOneNullLineString()
            => Assert.ThrowsException<OWLSemanticsException>(() => new GEOOntology("ex:geoOnt").Data.DeclareMultiLineString(new RDFResource("ex:multiLineString"), new List<List<(double, double)>>() { null, new List<(double, double)>() { (0d, 0d), (0d, 0d) } }));

        [TestMethod]
        public void ShouldThrowExceptionOnDeclaringMultiLineStringBecauseOneLineStringHavingLessThan2Elements()
            => Assert.ThrowsException<OWLSemanticsException>(() => new GEOOntology("ex:geoOnt").Data.DeclareMultiLineString(new RDFResource("ex:multiLineString"), new List<List<(double, double)>>() { new List<(double, double)>() { (0d, 0d) }, new List<(double, double)>() { (0d, 0d), (0d, 0d) } }));

        [TestMethod]
        public void ShouldDeclareOpenMultiPolygon()
        {
            GEOOntology geoOnt = new GEOOntology("ex:geoOnt");
            geoOnt.Data.DeclareMultiPolygon(new RDFResource("ex:MilanToRomeToNaplesAndNaplesToRomeToMilan"),
                new List<List<(double, double)>>() {
                    new List<(double, double)>() { (45.4654219, 9.1859243), (41.902784,  12.496366), (40.8517746, 14.2681244) },
                    new List<(double, double)>() { (41.902784,  12.496366), (40.8517746, 14.2681244), (45.4654219, 9.1859243) }
                });

            //Test evolution of GEO knowledge
            Assert.IsTrue(geoOnt.URI.Equals(geoOnt.URI));
            Assert.IsTrue(geoOnt.Model.ClassModel.ClassesCount == 19);
            Assert.IsTrue(geoOnt.Model.PropertyModel.PropertiesCount == 34);
            Assert.IsTrue(geoOnt.Data.IndividualsCount == 1);
            Assert.IsTrue(geoOnt.Data.CheckHasIndividual(new RDFResource("ex:MilanToRomeToNaplesAndNaplesToRomeToMilan")));
            Assert.IsTrue(geoOnt.Data.CheckIsIndividualOf(geoOnt.Model, new RDFResource("ex:MilanToRomeToNaplesAndNaplesToRomeToMilan"), RDFVocabulary.GEOSPARQL.SPATIAL_OBJECT));
            Assert.IsTrue(geoOnt.Data.CheckIsIndividualOf(geoOnt.Model, new RDFResource("ex:MilanToRomeToNaplesAndNaplesToRomeToMilan"), RDFVocabulary.GEOSPARQL.GEOMETRY));
            Assert.IsTrue(geoOnt.Data.CheckIsIndividualOf(geoOnt.Model, new RDFResource("ex:MilanToRomeToNaplesAndNaplesToRomeToMilan"), RDFVocabulary.GEOSPARQL.SF.MULTI_POLYGON));
            Assert.IsTrue(geoOnt.Data.CheckHasDatatypeAssertion(new RDFResource("ex:MilanToRomeToNaplesAndNaplesToRomeToMilan"), RDFVocabulary.GEOSPARQL.AS_GML, new RDFTypedLiteral("<gml:MultiSurface srsName=\"http://www.opengis.net/def/crs/EPSG/0/4326\" xmlns:gml=\"http://www.opengis.net/gml\"><gml:surfaceMembers><gml:Polygon><gml:exterior><gml:LinearRing><gml:pos>45.4654219 9.1859243</gml:pos><gml:pos>41.902784 12.496366</gml:pos><gml:pos>40.8517746 14.2681244</gml:pos><gml:pos>45.4654219 9.1859243</gml:pos></gml:LinearRing></gml:exterior></gml:Polygon><gml:Polygon><gml:exterior><gml:LinearRing><gml:pos>41.902784 12.496366</gml:pos><gml:pos>40.8517746 14.2681244</gml:pos><gml:pos>45.4654219 9.1859243</gml:pos><gml:pos>41.902784 12.496366</gml:pos></gml:LinearRing></gml:exterior></gml:Polygon></gml:surfaceMembers></gml:MultiSurface>", RDFModelEnums.RDFDatatypes.GEOSPARQL_GML)));
            Assert.IsTrue(geoOnt.Data.CheckHasDatatypeAssertion(new RDFResource("ex:MilanToRomeToNaplesAndNaplesToRomeToMilan"), RDFVocabulary.GEOSPARQL.AS_WKT, new RDFTypedLiteral("<http://www.opengis.net/def/crs/EPSG/0/4326>MULTIPOLYGON (((9.1859243 45.4654219, 12.496366 41.902784, 14.2681244 40.8517746, 9.1859243 45.4654219)), ((12.496366 41.902784, 14.2681244 40.8517746, 9.1859243 45.4654219, 12.496366 41.902784)))", RDFModelEnums.RDFDatatypes.GEOSPARQL_WKT)));

            //Test counters and enumerators
            Assert.IsTrue(geoOnt.SpatialObjectsCount == 1);
            int so1 = 0;
            IEnumerator<RDFResource> spatialObjectsEnumerator = geoOnt.SpatialObjectsEnumerator;
            while (spatialObjectsEnumerator.MoveNext()) so1++;
            Assert.IsTrue(so1 == 1);

            Assert.IsTrue(geoOnt.PointsCount == 0);
            int pt = 0;
            IEnumerator<RDFResource> pointsEnumerator = geoOnt.PointsEnumerator;
            while (pointsEnumerator.MoveNext()) pt++;
            Assert.IsTrue(pt == 0);

            Assert.IsTrue(geoOnt.LineStringsCount == 0);
            int ls = 0;
            IEnumerator<RDFResource> lineStringsEnumerator = geoOnt.LineStringsEnumerator;
            while (lineStringsEnumerator.MoveNext()) ls++;
            Assert.IsTrue(ls == 0);

            Assert.IsTrue(geoOnt.PolygonsCount == 0);
            int pl = 0;
            IEnumerator<RDFResource> polygonsEnumerator = geoOnt.PolygonsEnumerator;
            while (polygonsEnumerator.MoveNext()) pl++;
            Assert.IsTrue(pl == 0);

            Assert.IsTrue(geoOnt.MultiPointsCount == 0);
            int mpt = 0;
            IEnumerator<RDFResource> multiPointsEnumerator = geoOnt.MultiPointsEnumerator;
            while (multiPointsEnumerator.MoveNext()) mpt++;
            Assert.IsTrue(mpt == 0);

            Assert.IsTrue(geoOnt.MultiLineStringsCount == 0);
            int mls = 0;
            IEnumerator<RDFResource> multiLineStringsEnumerator = geoOnt.MultiLineStringsEnumerator;
            while (multiLineStringsEnumerator.MoveNext()) mls++;
            Assert.IsTrue(mls == 0);

            Assert.IsTrue(geoOnt.MultiPolygonsCount == 1);
            int mpl = 0;
            IEnumerator<RDFResource> multiPolygonsEnumerator = geoOnt.MultiPolygonsEnumerator;
            while (multiPolygonsEnumerator.MoveNext()) mpl++;
            Assert.IsTrue(mpl == 1);
        }

        [TestMethod]
        public void ShouldDeclareClosedMultiPolygon()
        {
            GEOOntology geoOnt = new GEOOntology("ex:geoOnt");
            geoOnt.Data.DeclareMultiPolygon(new RDFResource("ex:MilanToRomeToNaplesToMilanAndNaplesToRomeToMilanToNaples"),
                new List<List<(double, double)>>() {
                    new List<(double, double)>() { (45.4654219, 9.1859243), (41.902784,  12.496366), (40.8517746, 14.2681244), (45.4654219, 9.1859243) },
                    new List<(double, double)>() { (41.902784,  12.496366), (40.8517746, 14.2681244), (45.4654219, 9.1859243), (41.902784, 12.496366) }
                });

            //Test evolution of GEO knowledge
            Assert.IsTrue(geoOnt.URI.Equals(geoOnt.URI));
            Assert.IsTrue(geoOnt.Model.ClassModel.ClassesCount == 19);
            Assert.IsTrue(geoOnt.Model.PropertyModel.PropertiesCount == 34);
            Assert.IsTrue(geoOnt.Data.IndividualsCount == 1);
            Assert.IsTrue(geoOnt.Data.CheckHasIndividual(new RDFResource("ex:MilanToRomeToNaplesToMilanAndNaplesToRomeToMilanToNaples")));
            Assert.IsTrue(geoOnt.Data.CheckIsIndividualOf(geoOnt.Model, new RDFResource("ex:MilanToRomeToNaplesToMilanAndNaplesToRomeToMilanToNaples"), RDFVocabulary.GEOSPARQL.SPATIAL_OBJECT));
            Assert.IsTrue(geoOnt.Data.CheckIsIndividualOf(geoOnt.Model, new RDFResource("ex:MilanToRomeToNaplesToMilanAndNaplesToRomeToMilanToNaples"), RDFVocabulary.GEOSPARQL.GEOMETRY));
            Assert.IsTrue(geoOnt.Data.CheckIsIndividualOf(geoOnt.Model, new RDFResource("ex:MilanToRomeToNaplesToMilanAndNaplesToRomeToMilanToNaples"), RDFVocabulary.GEOSPARQL.SF.MULTI_POLYGON));
            Assert.IsTrue(geoOnt.Data.CheckHasDatatypeAssertion(new RDFResource("ex:MilanToRomeToNaplesToMilanAndNaplesToRomeToMilanToNaples"), RDFVocabulary.GEOSPARQL.AS_GML, new RDFTypedLiteral("<gml:MultiSurface srsName=\"http://www.opengis.net/def/crs/EPSG/0/4326\" xmlns:gml=\"http://www.opengis.net/gml\"><gml:surfaceMembers><gml:Polygon><gml:exterior><gml:LinearRing><gml:pos>45.4654219 9.1859243</gml:pos><gml:pos>41.902784 12.496366</gml:pos><gml:pos>40.8517746 14.2681244</gml:pos><gml:pos>45.4654219 9.1859243</gml:pos></gml:LinearRing></gml:exterior></gml:Polygon><gml:Polygon><gml:exterior><gml:LinearRing><gml:pos>41.902784 12.496366</gml:pos><gml:pos>40.8517746 14.2681244</gml:pos><gml:pos>45.4654219 9.1859243</gml:pos><gml:pos>41.902784 12.496366</gml:pos></gml:LinearRing></gml:exterior></gml:Polygon></gml:surfaceMembers></gml:MultiSurface>", RDFModelEnums.RDFDatatypes.GEOSPARQL_GML)));
            Assert.IsTrue(geoOnt.Data.CheckHasDatatypeAssertion(new RDFResource("ex:MilanToRomeToNaplesToMilanAndNaplesToRomeToMilanToNaples"), RDFVocabulary.GEOSPARQL.AS_WKT, new RDFTypedLiteral("<http://www.opengis.net/def/crs/EPSG/0/4326>MULTIPOLYGON (((9.1859243 45.4654219, 12.496366 41.902784, 14.2681244 40.8517746, 9.1859243 45.4654219)), ((12.496366 41.902784, 14.2681244 40.8517746, 9.1859243 45.4654219, 12.496366 41.902784)))", RDFModelEnums.RDFDatatypes.GEOSPARQL_WKT)));

            //Test counters and enumerators
            Assert.IsTrue(geoOnt.SpatialObjectsCount == 1);
            int so1 = 0;
            IEnumerator<RDFResource> spatialObjectsEnumerator = geoOnt.SpatialObjectsEnumerator;
            while (spatialObjectsEnumerator.MoveNext()) so1++;
            Assert.IsTrue(so1 == 1);

            Assert.IsTrue(geoOnt.PointsCount == 0);
            int pt = 0;
            IEnumerator<RDFResource> pointsEnumerator = geoOnt.PointsEnumerator;
            while (pointsEnumerator.MoveNext()) pt++;
            Assert.IsTrue(pt == 0);

            Assert.IsTrue(geoOnt.LineStringsCount == 0);
            int ls = 0;
            IEnumerator<RDFResource> lineStringsEnumerator = geoOnt.LineStringsEnumerator;
            while (lineStringsEnumerator.MoveNext()) ls++;
            Assert.IsTrue(ls == 0);

            Assert.IsTrue(geoOnt.PolygonsCount == 0);
            int pl = 0;
            IEnumerator<RDFResource> polygonsEnumerator = geoOnt.PolygonsEnumerator;
            while (polygonsEnumerator.MoveNext()) pl++;
            Assert.IsTrue(pl == 0);

            Assert.IsTrue(geoOnt.MultiPointsCount == 0);
            int mpt = 0;
            IEnumerator<RDFResource> multiPointsEnumerator = geoOnt.MultiPointsEnumerator;
            while (multiPointsEnumerator.MoveNext()) mpt++;
            Assert.IsTrue(mpt == 0);

            Assert.IsTrue(geoOnt.MultiLineStringsCount == 0);
            int mls = 0;
            IEnumerator<RDFResource> multiLineStringsEnumerator = geoOnt.MultiLineStringsEnumerator;
            while (multiLineStringsEnumerator.MoveNext()) mls++;
            Assert.IsTrue(mls == 0);

            Assert.IsTrue(geoOnt.MultiPolygonsCount == 1);
            int mpl = 0;
            IEnumerator<RDFResource> multiPolygonsEnumerator = geoOnt.MultiPolygonsEnumerator;
            while (multiPolygonsEnumerator.MoveNext()) mpl++;
            Assert.IsTrue(mpl == 1);
        }

        [TestMethod]
        public void ShouldThrowExceptionOnDeclaringMultiPolygonBecauseNullUri()
            => Assert.ThrowsException<OWLSemanticsException>(() => new GEOOntology("ex:geoOnt").Data.DeclareMultiPolygon(null, 
                new List<List<(double, double)>>() { new List<(double, double)>() { (0d, 0d), (0d, 0d), (0d, 0d) }, new List<(double, double)>() { (1d, 1d), (1d, 1d), (1d, 1d) } }));

        [TestMethod]
        public void ShouldThrowExceptionOnDeclaringMultiPolygonBecauseNullPolygons()
            => Assert.ThrowsException<OWLSemanticsException>(() => new GEOOntology("ex:geoOnt").Data.DeclareMultiPolygon(new RDFResource("ex:multiPolygon"), null));

        [TestMethod]
        public void ShouldThrowExceptionOnDeclaringMultiPolygonBecauseLessThan2Polygons()
            => Assert.ThrowsException<OWLSemanticsException>(() => new GEOOntology("ex:geoOnt").Data.DeclareMultiPolygon(new RDFResource("ex:multiPolygon"), 
                new List<List<(double, double)>>() { new List<(double, double)>() { (0d, 0d), (0d, 0d), (0d, 0d) } }));

        [TestMethod]
        public void ShouldThrowExceptionOnDeclaringMultiPolygonBecauseOneNullPolygon()
            => Assert.ThrowsException<OWLSemanticsException>(() => new GEOOntology("ex:geoOnt").Data.DeclareMultiPolygon(new RDFResource("ex:multiPolygon"), 
                new List<List<(double, double)>>() { null, new List<(double, double)>() { (0d, 0d), (0d, 0d), (0d, 0d) } }));

        [TestMethod]
        public void ShouldThrowExceptionOnDeclaringMultiPolygonBecauseOnePolygonHavingLessThan3Elements()
            => Assert.ThrowsException<OWLSemanticsException>(() => new GEOOntology("ex:geoOnt").Data.DeclareMultiPolygon(new RDFResource("ex:multiPolygon"), 
                new List<List<(double, double)>>() { new List<(double, double)>() { (0d, 0d), (0d, 0d) }, new List<(double, double)>() { (0d, 0d), (0d, 0d), (0d, 0d) } }));
        #endregion
    }
}