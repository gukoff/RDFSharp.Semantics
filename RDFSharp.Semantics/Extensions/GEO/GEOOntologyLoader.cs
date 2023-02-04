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

namespace RDFSharp.Semantics.Extensions.GEO
{
    /// <summary>
    /// GEOOntologyLoader is responsible for loading spatial ontologies from remote sources or alternative representations
    /// </summary>
    internal static class GEOOntologyLoader
    {
        #region Methods
        /// <summary>
        /// Gets a spatial ontology representation of the given graph
        /// </summary>
        internal static GEOOntology FromRDFGraph(RDFGraph graph, OWLOntologyLoaderOptions loaderOptions)
        {
            if (graph == null)
                throw new OWLSemanticsException("Cannot get GEO ontology from RDFGraph because given \"graph\" parameter is null");

            //Get OWL ontology with spatial extension points
            OWLOntology ontology = OWLOntologyLoader.FromRDFGraph(graph, loaderOptions,
               classModelExtensionPoint: GEOClassModelExtensionPoint,
               propertyModelExtensionPoint: GEOPropertyModelExtensionPoint,
               dataExtensionPoint: GEODataExtensionPoint);

            return new GEOOntology(ontology.URI.ToString()) { Ontology = ontology };
        }
        #endregion

        #region Utilities
        /// <summary>
        /// Extends OWL class model loading with support for spatial artifacts
        /// </summary>
        internal static void GEOClassModelExtensionPoint(OWLOntology ontology, RDFGraph graph)
            => ontology.Model.ClassModel = BuildGEOClassModel(ontology.Model.ClassModel);

        /// <summary>
        /// Extends OWL property model loading with support for spatial artifacts
        /// </summary>
        internal static void GEOPropertyModelExtensionPoint(OWLOntology ontology, RDFGraph graph)
            => ontology.Model.PropertyModel = BuildGEOPropertyModel(ontology.Model.PropertyModel);

        /// <summary>
        /// Extends OWL data loading with support for spatial artifacts
        /// </summary>
        internal static void GEODataExtensionPoint(OWLOntology ontology, RDFGraph graph)
        { 
            //TODO
        }

        /// <summary>
        /// Builds a reference spatial model
        /// </summary>
        internal static OWLOntologyModel BuildGEOModel()
            => new OWLOntologyModel() { ClassModel = BuildGEOClassModel(), PropertyModel = BuildGEOPropertyModel() };

        /// <summary>
        /// Builds a reference spatial class model
        /// </summary>
        internal static OWLOntologyClassModel BuildGEOClassModel(OWLOntologyClassModel existingClassModel = null)
        {
            OWLOntologyClassModel classModel = existingClassModel ?? new OWLOntologyClassModel();

            //GeoSPARQL
            classModel.DeclareClass(RDFVocabulary.GEOSPARQL.SPATIAL_OBJECT);
            classModel.DeclareClass(RDFVocabulary.GEOSPARQL.GEOMETRY);
            classModel.DeclareClass(RDFVocabulary.GEOSPARQL.FEATURE);
            classModel.DeclareSubClasses(RDFVocabulary.GEOSPARQL.GEOMETRY, RDFVocabulary.GEOSPARQL.SPATIAL_OBJECT);
            classModel.DeclareSubClasses(RDFVocabulary.GEOSPARQL.FEATURE, RDFVocabulary.GEOSPARQL.SPATIAL_OBJECT);
            classModel.DeclareDisjointClasses(RDFVocabulary.GEOSPARQL.GEOMETRY, RDFVocabulary.GEOSPARQL.FEATURE);

            //Simple Features (Geometry)
            classModel.DeclareClass(new RDFResource("http://www.opengis.net/ont/sf#Point"));
            classModel.DeclareClass(new RDFResource("http://www.opengis.net/ont/sf#Curve"));
            classModel.DeclareClass(new RDFResource("http://www.opengis.net/ont/sf#Surface"));
            classModel.DeclareClass(new RDFResource("http://www.opengis.net/ont/sf#Polygon"));
            classModel.DeclareClass(new RDFResource("http://www.opengis.net/ont/sf#Triangle"));
            classModel.DeclareClass(new RDFResource("http://www.opengis.net/ont/sf#LineString"));
            classModel.DeclareClass(new RDFResource("http://www.opengis.net/ont/sf#LinearRing"));
            classModel.DeclareClass(new RDFResource("http://www.opengis.net/ont/sf#Line"));
            classModel.DeclareClass(new RDFResource("http://www.opengis.net/ont/sf#GeometryCollection"));
            classModel.DeclareClass(new RDFResource("http://www.opengis.net/ont/sf#MultiPoint"));
            classModel.DeclareClass(new RDFResource("http://www.opengis.net/ont/sf#MultiCurve"));
            classModel.DeclareClass(new RDFResource("http://www.opengis.net/ont/sf#MultiSurface"));
            classModel.DeclareClass(new RDFResource("http://www.opengis.net/ont/sf#MultiPolygon"));
            classModel.DeclareClass(new RDFResource("http://www.opengis.net/ont/sf#MultiLineString"));
            classModel.DeclareClass(new RDFResource("http://www.opengis.net/ont/sf#PolyhedralSurface"));
            classModel.DeclareClass(new RDFResource("http://www.opengis.net/ont/sf#TIN"));
            classModel.DeclareSubClasses(new RDFResource("http://www.opengis.net/ont/sf#Point"), RDFVocabulary.GEOSPARQL.GEOMETRY);
            classModel.DeclareSubClasses(new RDFResource("http://www.opengis.net/ont/sf#Curve"), RDFVocabulary.GEOSPARQL.GEOMETRY);
            classModel.DeclareSubClasses(new RDFResource("http://www.opengis.net/ont/sf#Surface"), RDFVocabulary.GEOSPARQL.GEOMETRY);
            classModel.DeclareSubClasses(new RDFResource("http://www.opengis.net/ont/sf#Polygon"), RDFVocabulary.GEOSPARQL.GEOMETRY);
            classModel.DeclareSubClasses(new RDFResource("http://www.opengis.net/ont/sf#Polygon"), new RDFResource("http://www.opengis.net/ont/sf#Surface"));
            classModel.DeclareSubClasses(new RDFResource("http://www.opengis.net/ont/sf#Triangle"), RDFVocabulary.GEOSPARQL.GEOMETRY);
            classModel.DeclareSubClasses(new RDFResource("http://www.opengis.net/ont/sf#Triangle"), new RDFResource("http://www.opengis.net/ont/sf#Polygon"));
            classModel.DeclareSubClasses(new RDFResource("http://www.opengis.net/ont/sf#LineString"), RDFVocabulary.GEOSPARQL.GEOMETRY);
            classModel.DeclareSubClasses(new RDFResource("http://www.opengis.net/ont/sf#LineString"), new RDFResource("http://www.opengis.net/ont/sf#Curve"));
            classModel.DeclareSubClasses(new RDFResource("http://www.opengis.net/ont/sf#LinearRing"), RDFVocabulary.GEOSPARQL.GEOMETRY);
            classModel.DeclareSubClasses(new RDFResource("http://www.opengis.net/ont/sf#LinearRing"), new RDFResource("http://www.opengis.net/ont/sf#LineString"));
            classModel.DeclareSubClasses(new RDFResource("http://www.opengis.net/ont/sf#Line"), RDFVocabulary.GEOSPARQL.GEOMETRY);
            classModel.DeclareSubClasses(new RDFResource("http://www.opengis.net/ont/sf#Line"), new RDFResource("http://www.opengis.net/ont/sf#LineString"));
            classModel.DeclareSubClasses(new RDFResource("http://www.opengis.net/ont/sf#GeometryCollection"), RDFVocabulary.GEOSPARQL.GEOMETRY);
            classModel.DeclareSubClasses(new RDFResource("http://www.opengis.net/ont/sf#MultiPoint"), RDFVocabulary.GEOSPARQL.GEOMETRY);
            classModel.DeclareSubClasses(new RDFResource("http://www.opengis.net/ont/sf#MultiPoint"), new RDFResource("http://www.opengis.net/ont/sf#GeometryCollection"));
            classModel.DeclareSubClasses(new RDFResource("http://www.opengis.net/ont/sf#MultiCurve"), RDFVocabulary.GEOSPARQL.GEOMETRY);
            classModel.DeclareSubClasses(new RDFResource("http://www.opengis.net/ont/sf#MultiCurve"), new RDFResource("http://www.opengis.net/ont/sf#GeometryCollection"));
            classModel.DeclareSubClasses(new RDFResource("http://www.opengis.net/ont/sf#MultiSurface"), RDFVocabulary.GEOSPARQL.GEOMETRY);
            classModel.DeclareSubClasses(new RDFResource("http://www.opengis.net/ont/sf#MultiSurface"), new RDFResource("http://www.opengis.net/ont/sf#GeometryCollection"));
            classModel.DeclareSubClasses(new RDFResource("http://www.opengis.net/ont/sf#MultiPolygon"), RDFVocabulary.GEOSPARQL.GEOMETRY);
            classModel.DeclareSubClasses(new RDFResource("http://www.opengis.net/ont/sf#MultiPolygon"), new RDFResource("http://www.opengis.net/ont/sf#MultiSurface"));
            classModel.DeclareSubClasses(new RDFResource("http://www.opengis.net/ont/sf#MultiLineString"), RDFVocabulary.GEOSPARQL.GEOMETRY);
            classModel.DeclareSubClasses(new RDFResource("http://www.opengis.net/ont/sf#MultiLineString"), new RDFResource("http://www.opengis.net/ont/sf#MultiCurve"));
            classModel.DeclareSubClasses(new RDFResource("http://www.opengis.net/ont/sf#PolyhedralSurface"), RDFVocabulary.GEOSPARQL.GEOMETRY);
            classModel.DeclareSubClasses(new RDFResource("http://www.opengis.net/ont/sf#PolyhedralSurface"), new RDFResource("http://www.opengis.net/ont/sf#Surface"));
            classModel.DeclareSubClasses(new RDFResource("http://www.opengis.net/ont/sf#TIN"), RDFVocabulary.GEOSPARQL.GEOMETRY);
            classModel.DeclareSubClasses(new RDFResource("http://www.opengis.net/ont/sf#TIN"), new RDFResource("http://www.opengis.net/ont/sf#PolyhedralSurface"));

            return classModel;
        }

        /// <summary>
        /// Builds a reference spatial property model
        /// </summary>
        internal static OWLOntologyPropertyModel BuildGEOPropertyModel(OWLOntologyPropertyModel existingPropertyModel = null)
        {
            OWLOntologyPropertyModel propertyModel = existingPropertyModel ?? new OWLOntologyPropertyModel();

            //GeoSPARQL
            propertyModel.DeclareObjectProperty(RDFVocabulary.GEOSPARQL.HAS_GEOMETRY, new OWLOntologyObjectPropertyBehavior() { Domain = RDFVocabulary.GEOSPARQL.FEATURE, Range = RDFVocabulary.GEOSPARQL.GEOMETRY });
            propertyModel.DeclareObjectProperty(RDFVocabulary.GEOSPARQL.DEFAULT_GEOMETRY, new OWLOntologyObjectPropertyBehavior() { Domain = RDFVocabulary.GEOSPARQL.FEATURE, Range = RDFVocabulary.GEOSPARQL.GEOMETRY });
            propertyModel.DeclareObjectProperty(RDFVocabulary.GEOSPARQL.SF_CONTAINS, new OWLOntologyObjectPropertyBehavior() { Domain = RDFVocabulary.GEOSPARQL.SPATIAL_OBJECT, Range = RDFVocabulary.GEOSPARQL.SPATIAL_OBJECT });
            propertyModel.DeclareObjectProperty(RDFVocabulary.GEOSPARQL.SF_CROSSES, new OWLOntologyObjectPropertyBehavior() { Domain = RDFVocabulary.GEOSPARQL.SPATIAL_OBJECT, Range = RDFVocabulary.GEOSPARQL.SPATIAL_OBJECT });
            propertyModel.DeclareObjectProperty(RDFVocabulary.GEOSPARQL.SF_DISJOINT, new OWLOntologyObjectPropertyBehavior() { Domain = RDFVocabulary.GEOSPARQL.SPATIAL_OBJECT, Range = RDFVocabulary.GEOSPARQL.SPATIAL_OBJECT });
            propertyModel.DeclareObjectProperty(RDFVocabulary.GEOSPARQL.SF_EQUALS, new OWLOntologyObjectPropertyBehavior() { Domain = RDFVocabulary.GEOSPARQL.SPATIAL_OBJECT, Range = RDFVocabulary.GEOSPARQL.SPATIAL_OBJECT });
            propertyModel.DeclareObjectProperty(RDFVocabulary.GEOSPARQL.SF_INTERSECTS, new OWLOntologyObjectPropertyBehavior() { Domain = RDFVocabulary.GEOSPARQL.SPATIAL_OBJECT, Range = RDFVocabulary.GEOSPARQL.SPATIAL_OBJECT });
            propertyModel.DeclareObjectProperty(RDFVocabulary.GEOSPARQL.SF_OVERLAPS, new OWLOntologyObjectPropertyBehavior() { Domain = RDFVocabulary.GEOSPARQL.SPATIAL_OBJECT, Range = RDFVocabulary.GEOSPARQL.SPATIAL_OBJECT });
            propertyModel.DeclareObjectProperty(RDFVocabulary.GEOSPARQL.SF_TOUCHES, new OWLOntologyObjectPropertyBehavior() { Domain = RDFVocabulary.GEOSPARQL.SPATIAL_OBJECT, Range = RDFVocabulary.GEOSPARQL.SPATIAL_OBJECT });
            propertyModel.DeclareObjectProperty(RDFVocabulary.GEOSPARQL.SF_WITHIN, new OWLOntologyObjectPropertyBehavior() { Domain = RDFVocabulary.GEOSPARQL.SPATIAL_OBJECT, Range = RDFVocabulary.GEOSPARQL.SPATIAL_OBJECT });
            propertyModel.DeclareObjectProperty(RDFVocabulary.GEOSPARQL.EH_CONTAINS, new OWLOntologyObjectPropertyBehavior() { Domain = RDFVocabulary.GEOSPARQL.SPATIAL_OBJECT, Range = RDFVocabulary.GEOSPARQL.SPATIAL_OBJECT });
            propertyModel.DeclareObjectProperty(RDFVocabulary.GEOSPARQL.EH_COVERS, new OWLOntologyObjectPropertyBehavior() { Domain = RDFVocabulary.GEOSPARQL.SPATIAL_OBJECT, Range = RDFVocabulary.GEOSPARQL.SPATIAL_OBJECT });
            propertyModel.DeclareObjectProperty(RDFVocabulary.GEOSPARQL.EH_COVERED_BY, new OWLOntologyObjectPropertyBehavior() { Domain = RDFVocabulary.GEOSPARQL.SPATIAL_OBJECT, Range = RDFVocabulary.GEOSPARQL.SPATIAL_OBJECT });
            propertyModel.DeclareObjectProperty(RDFVocabulary.GEOSPARQL.EH_DISJOINT, new OWLOntologyObjectPropertyBehavior() { Domain = RDFVocabulary.GEOSPARQL.SPATIAL_OBJECT, Range = RDFVocabulary.GEOSPARQL.SPATIAL_OBJECT });
            propertyModel.DeclareObjectProperty(RDFVocabulary.GEOSPARQL.EH_EQUALS, new OWLOntologyObjectPropertyBehavior() { Domain = RDFVocabulary.GEOSPARQL.SPATIAL_OBJECT, Range = RDFVocabulary.GEOSPARQL.SPATIAL_OBJECT });
            propertyModel.DeclareObjectProperty(RDFVocabulary.GEOSPARQL.EH_INSIDE, new OWLOntologyObjectPropertyBehavior() { Domain = RDFVocabulary.GEOSPARQL.SPATIAL_OBJECT, Range = RDFVocabulary.GEOSPARQL.SPATIAL_OBJECT });
            propertyModel.DeclareObjectProperty(RDFVocabulary.GEOSPARQL.EH_MEET, new OWLOntologyObjectPropertyBehavior() { Domain = RDFVocabulary.GEOSPARQL.SPATIAL_OBJECT, Range = RDFVocabulary.GEOSPARQL.SPATIAL_OBJECT });
            propertyModel.DeclareObjectProperty(RDFVocabulary.GEOSPARQL.EH_OVERLAP, new OWLOntologyObjectPropertyBehavior() { Domain = RDFVocabulary.GEOSPARQL.SPATIAL_OBJECT, Range = RDFVocabulary.GEOSPARQL.SPATIAL_OBJECT });
            propertyModel.DeclareObjectProperty(RDFVocabulary.GEOSPARQL.RCC8DC, new OWLOntologyObjectPropertyBehavior() { Domain = RDFVocabulary.GEOSPARQL.SPATIAL_OBJECT, Range = RDFVocabulary.GEOSPARQL.SPATIAL_OBJECT });
            propertyModel.DeclareObjectProperty(RDFVocabulary.GEOSPARQL.RCC8EC, new OWLOntologyObjectPropertyBehavior() { Domain = RDFVocabulary.GEOSPARQL.SPATIAL_OBJECT, Range = RDFVocabulary.GEOSPARQL.SPATIAL_OBJECT });
            propertyModel.DeclareObjectProperty(RDFVocabulary.GEOSPARQL.RCC8EQ, new OWLOntologyObjectPropertyBehavior() { Domain = RDFVocabulary.GEOSPARQL.SPATIAL_OBJECT, Range = RDFVocabulary.GEOSPARQL.SPATIAL_OBJECT });
            propertyModel.DeclareObjectProperty(RDFVocabulary.GEOSPARQL.RCC8NTPP, new OWLOntologyObjectPropertyBehavior() { Domain = RDFVocabulary.GEOSPARQL.SPATIAL_OBJECT, Range = RDFVocabulary.GEOSPARQL.SPATIAL_OBJECT });
            propertyModel.DeclareObjectProperty(RDFVocabulary.GEOSPARQL.RCC8NTPPI, new OWLOntologyObjectPropertyBehavior() { Domain = RDFVocabulary.GEOSPARQL.SPATIAL_OBJECT, Range = RDFVocabulary.GEOSPARQL.SPATIAL_OBJECT });
            propertyModel.DeclareObjectProperty(RDFVocabulary.GEOSPARQL.RCC8PO, new OWLOntologyObjectPropertyBehavior() { Domain = RDFVocabulary.GEOSPARQL.SPATIAL_OBJECT, Range = RDFVocabulary.GEOSPARQL.SPATIAL_OBJECT });
            propertyModel.DeclareObjectProperty(RDFVocabulary.GEOSPARQL.RCC8TPP, new OWLOntologyObjectPropertyBehavior() { Domain = RDFVocabulary.GEOSPARQL.SPATIAL_OBJECT, Range = RDFVocabulary.GEOSPARQL.SPATIAL_OBJECT });
            propertyModel.DeclareObjectProperty(RDFVocabulary.GEOSPARQL.RCC8TPPI, new OWLOntologyObjectPropertyBehavior() { Domain = RDFVocabulary.GEOSPARQL.SPATIAL_OBJECT, Range = RDFVocabulary.GEOSPARQL.SPATIAL_OBJECT });
            propertyModel.DeclareDatatypeProperty(RDFVocabulary.GEOSPARQL.DIMENSION, new OWLOntologyDatatypePropertyBehavior() { Domain = RDFVocabulary.GEOSPARQL.GEOMETRY, Range = RDFVocabulary.XSD.INTEGER });
            propertyModel.DeclareDatatypeProperty(RDFVocabulary.GEOSPARQL.COORDINATE_DIMENSION, new OWLOntologyDatatypePropertyBehavior() { Domain = RDFVocabulary.GEOSPARQL.GEOMETRY, Range = RDFVocabulary.XSD.INTEGER });
            propertyModel.DeclareDatatypeProperty(RDFVocabulary.GEOSPARQL.SPATIAL_DIMENSION, new OWLOntologyDatatypePropertyBehavior() { Domain = RDFVocabulary.GEOSPARQL.GEOMETRY, Range = RDFVocabulary.XSD.INTEGER });
            propertyModel.DeclareDatatypeProperty(RDFVocabulary.GEOSPARQL.HAS_SERIALIZATION, new OWLOntologyObjectPropertyBehavior() { Domain = RDFVocabulary.GEOSPARQL.GEOMETRY, Range = RDFVocabulary.RDFS.LITERAL });
            propertyModel.DeclareDatatypeProperty(RDFVocabulary.GEOSPARQL.AS_WKT, new OWLOntologyObjectPropertyBehavior() { Domain = RDFVocabulary.GEOSPARQL.GEOMETRY, Range = RDFVocabulary.GEOSPARQL.WKT_LITERAL });
            propertyModel.DeclareDatatypeProperty(RDFVocabulary.GEOSPARQL.AS_GML, new OWLOntologyObjectPropertyBehavior() { Domain = RDFVocabulary.GEOSPARQL.GEOMETRY, Range = RDFVocabulary.GEOSPARQL.GML_LITERAL });
            propertyModel.DeclareDatatypeProperty(RDFVocabulary.GEOSPARQL.IS_EMPTY, new OWLOntologyObjectPropertyBehavior() { Domain = RDFVocabulary.GEOSPARQL.GEOMETRY, Range = RDFVocabulary.XSD.BOOLEAN });
            propertyModel.DeclareDatatypeProperty(RDFVocabulary.GEOSPARQL.IS_SIMPLE, new OWLOntologyObjectPropertyBehavior() { Domain = RDFVocabulary.GEOSPARQL.GEOMETRY, Range = RDFVocabulary.XSD.BOOLEAN });
            propertyModel.DeclareSubProperties(RDFVocabulary.GEOSPARQL.DEFAULT_GEOMETRY, RDFVocabulary.GEOSPARQL.HAS_GEOMETRY);
            propertyModel.DeclareSubProperties(RDFVocabulary.GEOSPARQL.AS_WKT, RDFVocabulary.GEOSPARQL.HAS_SERIALIZATION);
            propertyModel.DeclareSubProperties(RDFVocabulary.GEOSPARQL.AS_GML, RDFVocabulary.GEOSPARQL.HAS_SERIALIZATION);
            propertyModel.DeclareInverseProperties(RDFVocabulary.GEOSPARQL.EH_COVERS, RDFVocabulary.GEOSPARQL.EH_COVERED_BY);

            //Simple Features (Geometry)


            return propertyModel;
        }
        #endregion
    }
}