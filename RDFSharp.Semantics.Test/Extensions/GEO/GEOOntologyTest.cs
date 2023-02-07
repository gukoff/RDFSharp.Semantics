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
using System.Collections.Generic;

namespace RDFSharp.Semantics.Extensions.GEO.Test
{
    [TestClass]
    public class GEOOntologyTest
    {
        #region Tests
        [TestMethod]
        public void ShouldCreateGEOOntology()
        {
            GEOOntology geoOnt = new GEOOntology("ex:geoOnt");

            Assert.IsNotNull(geoOnt);
            Assert.IsNotNull(geoOnt.Ontology);

            //Test initialization of GEO knowledge
            Assert.IsTrue(geoOnt.Ontology.URI.Equals(geoOnt.URI));
            Assert.IsTrue(geoOnt.Ontology.Model.ClassModel.ClassesCount == 19);
            Assert.IsTrue(geoOnt.Ontology.Model.PropertyModel.PropertiesCount == 34);
            Assert.IsTrue(geoOnt.Ontology.Data.IndividualsCount == 0);

            //Test counters and enumerators
            Assert.IsTrue(geoOnt.SpatialObjectsCount == 0);
            int so1 = 0;
            IEnumerator<RDFResource> spatialObjectsEnumerator = geoOnt.SpatialObjectsEnumerator;
            while (spatialObjectsEnumerator.MoveNext()) so1++;
            Assert.IsTrue(so1 == 0);

            int so2 = 0;
            foreach (RDFResource spatialObject in geoOnt) so2++;
            Assert.IsTrue(so2 == 0);

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
        }

        [TestMethod]
        public void ShouldDeclarePoint()
        {
            GEOOntology geoOnt = new GEOOntology("ex:geoOnt");
            geoOnt.DeclarePoint(new RDFResource("ex:Milan"), 45.4654219, 9.1859243);

            //Test evolution of GEO knowledge
            Assert.IsTrue(geoOnt.Ontology.URI.Equals(geoOnt.URI));
            Assert.IsTrue(geoOnt.Ontology.Model.ClassModel.ClassesCount == 19);
            Assert.IsTrue(geoOnt.Ontology.Model.PropertyModel.PropertiesCount == 34);
            Assert.IsTrue(geoOnt.Ontology.Data.IndividualsCount == 1);
            Assert.IsTrue(geoOnt.Ontology.Data.CheckHasIndividual(new RDFResource("ex:Milan")));
            Assert.IsTrue(geoOnt.Ontology.Data.CheckIsIndividualOf(geoOnt.Ontology.Model, new RDFResource("ex:Milan"), RDFVocabulary.GEOSPARQL.SPATIAL_OBJECT));
            Assert.IsTrue(geoOnt.Ontology.Data.CheckIsIndividualOf(geoOnt.Ontology.Model, new RDFResource("ex:Milan"), RDFVocabulary.GEOSPARQL.GEOMETRY));
            Assert.IsTrue(geoOnt.Ontology.Data.CheckIsIndividualOf(geoOnt.Ontology.Model, new RDFResource("ex:Milan"), RDFVocabulary.GEOSPARQL.SF.POINT));
            Assert.IsTrue(geoOnt.Ontology.Data.CheckHasDatatypeAssertion(new RDFResource("ex:Milan"), RDFVocabulary.GEOSPARQL.AS_GML, new RDFTypedLiteral("<gml:Point srsName=\"http://www.opengis.net/def/crs/EPSG/0/4326\" xmlns:gml=\"http://www.opengis.net/gml\"><gml:pos>45.4654219 9.1859243</gml:pos></gml:Point>", RDFModelEnums.RDFDatatypes.GEOSPARQL_GML)));

            //Test counters and enumerators
            Assert.IsTrue(geoOnt.SpatialObjectsCount == 1);
            int so1 = 0;
            IEnumerator<RDFResource> spatialObjectsEnumerator = geoOnt.SpatialObjectsEnumerator;
            while (spatialObjectsEnumerator.MoveNext()) so1++;
            Assert.IsTrue(so1 == 1);

            int so2 = 0;
            foreach (RDFResource spatialObject in geoOnt) so2++;
            Assert.IsTrue(so2 == 1);

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
        }

        [TestMethod]
        public void ShouldThrowExceptionOnDeclaringPointBecauseNullUri()
            => Assert.ThrowsException<OWLSemanticsException>(() => new GEOOntology("ex:geoOnt").DeclarePoint(null, 0, 0));

        [TestMethod]
        public void ShouldDeclareLineString()
        {
            GEOOntology geoOnt = new GEOOntology("ex:geoOnt");
            geoOnt.DeclareLineString(new RDFResource("ex:MilanToRomeToNaples"), new[] { (45.4654219, 9.1859243), (41.902784, 12.496366), (40.8517746, 14.2681244) });

            //Test evolution of GEO knowledge
            Assert.IsTrue(geoOnt.Ontology.URI.Equals(geoOnt.URI));
            Assert.IsTrue(geoOnt.Ontology.Model.ClassModel.ClassesCount == 19);
            Assert.IsTrue(geoOnt.Ontology.Model.PropertyModel.PropertiesCount == 34);
            Assert.IsTrue(geoOnt.Ontology.Data.IndividualsCount == 1);
            Assert.IsTrue(geoOnt.Ontology.Data.CheckHasIndividual(new RDFResource("ex:MilanToRomeToNaples")));
            Assert.IsTrue(geoOnt.Ontology.Data.CheckIsIndividualOf(geoOnt.Ontology.Model, new RDFResource("ex:MilanToRomeToNaples"), RDFVocabulary.GEOSPARQL.SPATIAL_OBJECT));
            Assert.IsTrue(geoOnt.Ontology.Data.CheckIsIndividualOf(geoOnt.Ontology.Model, new RDFResource("ex:MilanToRomeToNaples"), RDFVocabulary.GEOSPARQL.GEOMETRY));
            Assert.IsTrue(geoOnt.Ontology.Data.CheckIsIndividualOf(geoOnt.Ontology.Model, new RDFResource("ex:MilanToRomeToNaples"), RDFVocabulary.GEOSPARQL.SF.LINESTRING));
            Assert.IsTrue(geoOnt.Ontology.Data.CheckHasDatatypeAssertion(new RDFResource("ex:MilanToRomeToNaples"), RDFVocabulary.GEOSPARQL.AS_GML, new RDFTypedLiteral("<gml:LineString srsName=\"http://www.opengis.net/def/crs/EPSG/0/4326\" xmlns:gml=\"http://www.opengis.net/gml\"><gml:pos>45.4654219 9.1859243</gml:pos><gml:pos>41.902784 12.496366</gml:pos><gml:pos>40.8517746 14.2681244</gml:pos></gml:LineString>", RDFModelEnums.RDFDatatypes.GEOSPARQL_GML)));

            //Test counters and enumerators
            Assert.IsTrue(geoOnt.SpatialObjectsCount == 1);
            int so1 = 0;
            IEnumerator<RDFResource> spatialObjectsEnumerator = geoOnt.SpatialObjectsEnumerator;
            while (spatialObjectsEnumerator.MoveNext()) so1++;
            Assert.IsTrue(so1 == 1);

            int so2 = 0;
            foreach (RDFResource spatialObject in geoOnt) so2++;
            Assert.IsTrue(so2 == 1);

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
        }

        [TestMethod]
        public void ShouldThrowExceptionOnDeclaringLineStringBecauseNullUri()
            => Assert.ThrowsException<OWLSemanticsException>(() => new GEOOntology("ex:geoOnt").DeclareLineString(null, new[] { (0d, 0d), (0d, 0d) }));

        [TestMethod]
        public void ShouldThrowExceptionOnDeclaringLineStringBecauseNullPoints()
            => Assert.ThrowsException<OWLSemanticsException>(() => new GEOOntology("ex:geoOnt").DeclareLineString(new RDFResource("ex:lineString"), null));

        [TestMethod]
        public void ShouldThrowExceptionOnDeclaringLineStringBecauseLessThan2Points()
            => Assert.ThrowsException<OWLSemanticsException>(() => new GEOOntology("ex:geoOnt").DeclareLineString(new RDFResource("ex:lineString"), new[] { (0d, 0d) }));

        [TestMethod]
        public void ShouldDeclareOpenPolygon()
        {
            GEOOntology geoOnt = new GEOOntology("ex:geoOnt");
            geoOnt.DeclarePolygon(new RDFResource("ex:MilanToRomeToNaples"), new[] { (45.4654219, 9.1859243), (41.902784, 12.496366), (40.8517746, 14.2681244) });

            //Test evolution of GEO knowledge
            Assert.IsTrue(geoOnt.Ontology.URI.Equals(geoOnt.URI));
            Assert.IsTrue(geoOnt.Ontology.Model.ClassModel.ClassesCount == 19);
            Assert.IsTrue(geoOnt.Ontology.Model.PropertyModel.PropertiesCount == 34);
            Assert.IsTrue(geoOnt.Ontology.Data.IndividualsCount == 1);
            Assert.IsTrue(geoOnt.Ontology.Data.CheckHasIndividual(new RDFResource("ex:MilanToRomeToNaples")));
            Assert.IsTrue(geoOnt.Ontology.Data.CheckIsIndividualOf(geoOnt.Ontology.Model, new RDFResource("ex:MilanToRomeToNaples"), RDFVocabulary.GEOSPARQL.SPATIAL_OBJECT));
            Assert.IsTrue(geoOnt.Ontology.Data.CheckIsIndividualOf(geoOnt.Ontology.Model, new RDFResource("ex:MilanToRomeToNaples"), RDFVocabulary.GEOSPARQL.GEOMETRY));
            Assert.IsTrue(geoOnt.Ontology.Data.CheckIsIndividualOf(geoOnt.Ontology.Model, new RDFResource("ex:MilanToRomeToNaples"), RDFVocabulary.GEOSPARQL.SF.POLYGON));
            Assert.IsTrue(geoOnt.Ontology.Data.CheckHasDatatypeAssertion(new RDFResource("ex:MilanToRomeToNaples"), RDFVocabulary.GEOSPARQL.AS_GML, new RDFTypedLiteral("<gml:Polygon srsName=\"http://www.opengis.net/def/crs/EPSG/0/4326\" xmlns:gml=\"http://www.opengis.net/gml\"><gml:exterior><gml:LinearRing><gml:pos>45.4654219 9.1859243</gml:pos><gml:pos>41.902784 12.496366</gml:pos><gml:pos>40.8517746 14.2681244</gml:pos><gml:pos>45.4654219 9.1859243</gml:pos></gml:LinearRing></gml:exterior></gml:Polygon>", RDFModelEnums.RDFDatatypes.GEOSPARQL_GML)));

            //Test counters and enumerators
            Assert.IsTrue(geoOnt.SpatialObjectsCount == 1);
            int so1 = 0;
            IEnumerator<RDFResource> spatialObjectsEnumerator = geoOnt.SpatialObjectsEnumerator;
            while (spatialObjectsEnumerator.MoveNext()) so1++;
            Assert.IsTrue(so1 == 1);

            int so2 = 0;
            foreach (RDFResource spatialObject in geoOnt) so2++;
            Assert.IsTrue(so2 == 1);

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
        }

        [TestMethod]
        public void ShouldDeclareClosedPolygon()
        {
            GEOOntology geoOnt = new GEOOntology("ex:geoOnt");
            geoOnt.DeclarePolygon(new RDFResource("ex:MilanToRomeToNaplesToMilan"), new[] { (45.4654219, 9.1859243), (41.902784, 12.496366), (40.8517746, 14.2681244), (45.4654219, 9.1859243) });

            //Test evolution of GEO knowledge
            Assert.IsTrue(geoOnt.Ontology.URI.Equals(geoOnt.URI));
            Assert.IsTrue(geoOnt.Ontology.Model.ClassModel.ClassesCount == 19);
            Assert.IsTrue(geoOnt.Ontology.Model.PropertyModel.PropertiesCount == 34);
            Assert.IsTrue(geoOnt.Ontology.Data.IndividualsCount == 1);
            Assert.IsTrue(geoOnt.Ontology.Data.CheckHasIndividual(new RDFResource("ex:MilanToRomeToNaplesToMilan")));
            Assert.IsTrue(geoOnt.Ontology.Data.CheckIsIndividualOf(geoOnt.Ontology.Model, new RDFResource("ex:MilanToRomeToNaplesToMilan"), RDFVocabulary.GEOSPARQL.SPATIAL_OBJECT));
            Assert.IsTrue(geoOnt.Ontology.Data.CheckIsIndividualOf(geoOnt.Ontology.Model, new RDFResource("ex:MilanToRomeToNaplesToMilan"), RDFVocabulary.GEOSPARQL.GEOMETRY));
            Assert.IsTrue(geoOnt.Ontology.Data.CheckIsIndividualOf(geoOnt.Ontology.Model, new RDFResource("ex:MilanToRomeToNaplesToMilan"), RDFVocabulary.GEOSPARQL.SF.POLYGON));
            Assert.IsTrue(geoOnt.Ontology.Data.CheckHasDatatypeAssertion(new RDFResource("ex:MilanToRomeToNaplesToMilan"), RDFVocabulary.GEOSPARQL.AS_GML, new RDFTypedLiteral("<gml:Polygon srsName=\"http://www.opengis.net/def/crs/EPSG/0/4326\" xmlns:gml=\"http://www.opengis.net/gml\"><gml:exterior><gml:LinearRing><gml:pos>45.4654219 9.1859243</gml:pos><gml:pos>41.902784 12.496366</gml:pos><gml:pos>40.8517746 14.2681244</gml:pos><gml:pos>45.4654219 9.1859243</gml:pos></gml:LinearRing></gml:exterior></gml:Polygon>", RDFModelEnums.RDFDatatypes.GEOSPARQL_GML)));

            //Test counters and enumerators
            Assert.IsTrue(geoOnt.SpatialObjectsCount == 1);
            int so1 = 0;
            IEnumerator<RDFResource> spatialObjectsEnumerator = geoOnt.SpatialObjectsEnumerator;
            while (spatialObjectsEnumerator.MoveNext()) so1++;
            Assert.IsTrue(so1 == 1);

            int so2 = 0;
            foreach (RDFResource spatialObject in geoOnt) so2++;
            Assert.IsTrue(so2 == 1);

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
        }

        [TestMethod]
        public void ShouldThrowExceptionOnDeclaringPolygonBecauseNullUri()
            => Assert.ThrowsException<OWLSemanticsException>(() => new GEOOntology("ex:geoOnt").DeclarePolygon(null, new[] { (0d, 0d), (0d, 0d) }));

        [TestMethod]
        public void ShouldThrowExceptionOnDeclaringPolygonBecauseNullPoints()
            => Assert.ThrowsException<OWLSemanticsException>(() => new GEOOntology("ex:geoOnt").DeclarePolygon(new RDFResource("ex:polygon"), null));

        [TestMethod]
        public void ShouldThrowExceptionOnDeclaringPolygonBecauseLessThan3Points()
            => Assert.ThrowsException<OWLSemanticsException>(() => new GEOOntology("ex:geoOnt").DeclarePolygon(new RDFResource("ex:polygon"), new[] { (0d, 0d), (0d, 0d) }));
        #endregion
    }
}