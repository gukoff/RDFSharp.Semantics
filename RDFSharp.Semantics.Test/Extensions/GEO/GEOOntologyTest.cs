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
    public class GEOOntologyTest
    {
        #region Tests
        [TestMethod]
        public void ShouldCreateGEOOntology()
        {
            GEOOntology geoOnt = new GEOOntology("ex:geoOnt");

            Assert.IsNotNull(geoOnt);

            //Test initialization of GEO knowledge
            Assert.IsTrue(geoOnt.URI.Equals(geoOnt.URI));
            Assert.IsTrue(geoOnt.Model.ClassModel.ClassesCount == 19);
            Assert.IsTrue(geoOnt.Model.PropertyModel.PropertiesCount == 34);
            Assert.IsTrue(geoOnt.Data.IndividualsCount == 0);

            //Test counters and enumerators
            Assert.IsTrue(geoOnt.SpatialObjectsCount == 0);
            int so1 = 0;
            IEnumerator<RDFResource> spatialObjectsEnumerator = geoOnt.SpatialObjectsEnumerator;
            while (spatialObjectsEnumerator.MoveNext()) so1++;
            Assert.IsTrue(so1 == 0);

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

            Assert.IsTrue(geoOnt.MultiPolygonsCount == 0);
            int mpl = 0;
            IEnumerator<RDFResource> multiPolygonsEnumerator = geoOnt.MultiPolygonsEnumerator;
            while (multiPolygonsEnumerator.MoveNext()) mpl++;
            Assert.IsTrue(mpl == 0);
        }
        #endregion
    }
}