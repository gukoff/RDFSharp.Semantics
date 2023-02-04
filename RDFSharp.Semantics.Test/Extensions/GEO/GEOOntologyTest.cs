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
            int p = 0;
            IEnumerator<RDFResource> pointsEnumerator = geoOnt.PointsEnumerator;
            while (pointsEnumerator.MoveNext()) p++;
            Assert.IsTrue(p == 0);

            Assert.IsTrue(geoOnt.LinesCount == 0);
            int l = 0;
            IEnumerator<RDFResource> linesEnumerator = geoOnt.LinesEnumerator;
            while (linesEnumerator.MoveNext()) l++;
            Assert.IsTrue(l == 0);
        }

        [TestMethod]
        public void ShouldDeclarePoint()
        {
            GEOOntology geoOnt = new GEOOntology("ex:geoOnt");
            geoOnt.DeclarePoint(new RDFResource("ex:Milan"), (45.4654219, 9.1859243));

            //Test evolution of GEO knowledge
            Assert.IsTrue(geoOnt.Ontology.URI.Equals(geoOnt.URI));
            Assert.IsTrue(geoOnt.Ontology.Model.ClassModel.ClassesCount == 19);
            Assert.IsTrue(geoOnt.Ontology.Model.PropertyModel.PropertiesCount == 34);
            Assert.IsTrue(geoOnt.Ontology.Data.IndividualsCount == 1);
            Assert.IsTrue(geoOnt.Ontology.Data.CheckHasIndividual(new RDFResource("ex:Milan")));
            Assert.IsTrue(geoOnt.Ontology.Data.CheckIsIndividualOf(geoOnt.Ontology.Model, new RDFResource("ex:Milan"), RDFVocabulary.GEOSPARQL.SPATIAL_OBJECT));
            Assert.IsTrue(geoOnt.Ontology.Data.CheckIsIndividualOf(geoOnt.Ontology.Model, new RDFResource("ex:Milan"), RDFVocabulary.GEOSPARQL.GEOMETRY));
            Assert.IsTrue(geoOnt.Ontology.Data.CheckIsIndividualOf(geoOnt.Ontology.Model, new RDFResource("ex:Milan"), RDFVocabulary.GEOSPARQL.SF.POINT));
            Assert.IsTrue(geoOnt.Ontology.Data.CheckHasDatatypeAssertion(new RDFResource("ex:Milan"), RDFVocabulary.GEOSPARQL.AS_WKT, new RDFTypedLiteral("POINT (45.4654219 9.1859243)", RDFModelEnums.RDFDatatypes.GEOSPARQL_WKT)));

            //Test counters and enumerators
            Assert.IsTrue(geoOnt.SpatialObjectsCount == 1);
            int i1 = 0;
            IEnumerator<RDFResource> spatialObjectsEnumerator = geoOnt.SpatialObjectsEnumerator;
            while (spatialObjectsEnumerator.MoveNext()) i1++;
            Assert.IsTrue(i1 == 1);

            int i2 = 0;
            foreach (RDFResource skosConcept in geoOnt) i2++;
            Assert.IsTrue(i2 == 1);

            Assert.IsTrue(geoOnt.PointsCount == 1);
            int j = 0;
            IEnumerator<RDFResource> pointsEnumerator = geoOnt.PointsEnumerator;
            while (pointsEnumerator.MoveNext()) j++;
            Assert.IsTrue(j == 1);
        }

        [TestMethod]
        public void ShouldThrowExceptionOnDeclaringPointBecauseNullUri()
            => Assert.ThrowsException<OWLSemanticsException>(() => new GEOOntology("ex:geoOnt").DeclarePoint(null, (0, 0)));

        [TestMethod]
        public void ShouldDeclareLine()
        {
            GEOOntology geoOnt = new GEOOntology("ex:geoOnt");
            geoOnt.DeclareLine(new RDFResource("ex:MilanToRome"), (45.4654219, 9.1859243), (41.902784, 12.496366));

            //Test evolution of GEO knowledge
            Assert.IsTrue(geoOnt.Ontology.URI.Equals(geoOnt.URI));
            Assert.IsTrue(geoOnt.Ontology.Model.ClassModel.ClassesCount == 19);
            Assert.IsTrue(geoOnt.Ontology.Model.PropertyModel.PropertiesCount == 34);
            Assert.IsTrue(geoOnt.Ontology.Data.IndividualsCount == 1);
            Assert.IsTrue(geoOnt.Ontology.Data.CheckHasIndividual(new RDFResource("ex:MilanToRome")));
            Assert.IsTrue(geoOnt.Ontology.Data.CheckIsIndividualOf(geoOnt.Ontology.Model, new RDFResource("ex:MilanToRome"), RDFVocabulary.GEOSPARQL.SPATIAL_OBJECT));
            Assert.IsTrue(geoOnt.Ontology.Data.CheckIsIndividualOf(geoOnt.Ontology.Model, new RDFResource("ex:MilanToRome"), RDFVocabulary.GEOSPARQL.GEOMETRY));
            Assert.IsTrue(geoOnt.Ontology.Data.CheckIsIndividualOf(geoOnt.Ontology.Model, new RDFResource("ex:MilanToRome"), RDFVocabulary.GEOSPARQL.SF.LINE));
            Assert.IsTrue(geoOnt.Ontology.Data.CheckHasDatatypeAssertion(new RDFResource("ex:MilanToRome"), RDFVocabulary.GEOSPARQL.AS_WKT, new RDFTypedLiteral("LINESTRING (45.4654219 9.1859243, 41.902784 12.496366)", RDFModelEnums.RDFDatatypes.GEOSPARQL_WKT)));

            //Test counters and enumerators
            Assert.IsTrue(geoOnt.SpatialObjectsCount == 1);
            int i1 = 0;
            IEnumerator<RDFResource> spatialObjectsEnumerator = geoOnt.SpatialObjectsEnumerator;
            while (spatialObjectsEnumerator.MoveNext()) i1++;
            Assert.IsTrue(i1 == 1);

            int i2 = 0;
            foreach (RDFResource skosConcept in geoOnt) i2++;
            Assert.IsTrue(i2 == 1);

            Assert.IsTrue(geoOnt.LinesCount == 1);
            int j = 0;
            IEnumerator<RDFResource> linesEnumerator = geoOnt.LinesEnumerator;
            while (linesEnumerator.MoveNext()) j++;
            Assert.IsTrue(j == 1);
        }

        [TestMethod]
        public void ShouldThrowExceptionOnDeclaringLineBecauseNullUri()
            => Assert.ThrowsException<OWLSemanticsException>(() => new GEOOntology("ex:geoOnt").DeclareLine(null, (0, 0),(0, 0)));
        #endregion
    }
}