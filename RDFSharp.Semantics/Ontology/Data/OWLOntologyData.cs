﻿/*
   Copyright 2012-2023 Marco De Salvo

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

using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using RDFSharp.Model;

namespace RDFSharp.Semantics
{
    /// <summary>
    /// OWLOntologyData represents the A-BOX available to an ontology from the application domain
    /// </summary>
    public class OWLOntologyData : IEnumerable<RDFResource>, IDisposable
    {
        #region Properties
        /// <summary>
        /// Count of the individuals
        /// </summary>
        public long IndividualsCount 
            => Individuals.Count;

        /// <summary>
        /// Count of the owl:AllDifferent [OWL2]
        /// </summary>
        public long AllDifferentCount
        {
            get
            {
                long count = 0;
                IEnumerator<RDFResource> allDifferent = AllDifferentEnumerator;
                while (allDifferent.MoveNext())
                    count++;
                return count;
            }
        }

        /// <summary>
        /// Gets the enumerator on the individuals for iteration
        /// </summary>
        public IEnumerator<RDFResource> IndividualsEnumerator
            => Individuals.Values.GetEnumerator();

        /// <summary>
        /// Gets the enumerator on the owl:AllDifferent for iteration [OWL2]
        /// </summary>
        public IEnumerator<RDFResource> AllDifferentEnumerator
            => ABoxGraph[null, RDFVocabulary.RDF.TYPE, RDFVocabulary.OWL.ALL_DIFFERENT, null]
                .Select(t => (RDFResource)t.Subject)
                .GetEnumerator();

        /// <summary>
        /// Collection of individuals
        /// </summary>
        internal Dictionary<long, RDFResource> Individuals { get; set; }

        /// <summary>
        /// A-BOX knowledge available to the data
        /// </summary>
        internal RDFGraph ABoxGraph { get; set; }

        /// <summary>
        /// A-BOX knowledge annotating the data
        /// </summary>
        internal RDFGraph OBoxGraph { get; set; }

        /// <summary>
        /// Flag indicating that the ontology data has already been disposed
        /// </summary>
        internal bool Disposed { get; set; }
        #endregion

        #region Ctors
        /// <summary>
        /// Builds an empty ontology data
        /// </summary>
        public OWLOntologyData()
        {
            Individuals = new Dictionary<long, RDFResource>();
            ABoxGraph = new RDFGraph();
            OBoxGraph = new RDFGraph();
        }

        /// <summary>
        /// Destroys the ontology data instance
        /// </summary>
        ~OWLOntologyData() => Dispose(false);
        #endregion

        #region Interfaces
        /// <summary>
        /// Exposes a typed enumerator on the individuals for iteration
        /// </summary>
        IEnumerator<RDFResource> IEnumerable<RDFResource>.GetEnumerator() 
            => IndividualsEnumerator;

        /// <summary>
        /// Exposes an untyped enumerator on the individuals for iteration
        /// </summary>
        IEnumerator IEnumerable.GetEnumerator() 
            => IndividualsEnumerator;

        /// <summary>
        /// Disposes the ontology data (IDisposable)
        /// </summary>
        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        /// <summary>
        /// Disposes the ontology data
        /// </summary>
        protected virtual void Dispose(bool disposing)
        {
            if (Disposed)
                return;

            if (disposing)
            {
                Individuals.Clear();
                ABoxGraph.Dispose();
                OBoxGraph.Dispose();

                Individuals = null;
                ABoxGraph = null;
                OBoxGraph = null;
            }

            Disposed = true;
        }
        #endregion

        #region Methods
        /// <summary>
        /// Declares the given owl:NamedIndividual instance to the data
        /// </summary>
        public OWLOntologyData DeclareIndividual(RDFResource owlIndividual)
        {
            if (owlIndividual == null)
                throw new OWLSemanticsException("Cannot declare owl:Individual instance to the data because given \"owlIndividual\" parameter is null");

            //Declare individual to the data
            if (!Individuals.ContainsKey(owlIndividual.PatternMemberID))
                Individuals.Add(owlIndividual.PatternMemberID, owlIndividual);

            //Add knowledge to the A-BOX
            ABoxGraph.AddTriple(new RDFTriple(owlIndividual, RDFVocabulary.RDF.TYPE, RDFVocabulary.OWL.NAMED_INDIVIDUAL));

            return this;
        }

        //ANNOTATIONS

        /// <summary>
        /// Annotates the given individual with the given "annotationProperty -> annotationValue"
        /// </summary>
        public OWLOntologyData AnnotateIndividual(RDFResource owlIndividual, RDFResource annotationProperty, RDFResource annotationValue)
        {
            if (owlIndividual == null)
                throw new OWLSemanticsException("Cannot annotate individual because given \"owlIndividual\" parameter is null");
            if (annotationProperty == null)
                throw new OWLSemanticsException("Cannot annotate individual because given \"annotationProperty\" parameter is null");
            if (annotationProperty.IsBlank)
                throw new OWLSemanticsException("Cannot annotate individual because given \"annotationProperty\" parameter is a blank predicate");
            if (annotationValue == null)
                throw new OWLSemanticsException("Cannot annotate individual because given \"annotationValue\" parameter is null");

            //Add knowledge to the O-BOX
            OBoxGraph.AddTriple(new RDFTriple(owlIndividual, annotationProperty, annotationValue));

            return this;
        }

        /// <summary>
        /// Annotates the given individual with the given "annotationProperty -> annotationValue"
        /// </summary>
        public OWLOntologyData AnnotateIndividual(RDFResource owlIndividual, RDFResource annotationProperty, RDFLiteral annotationValue)
        {
            if (owlIndividual == null)
                throw new OWLSemanticsException("Cannot annotate individual because given \"owlIndividual\" parameter is null");
            if (annotationProperty == null)
                throw new OWLSemanticsException("Cannot annotate individual because given \"annotationProperty\" parameter is null");
            if (annotationProperty.IsBlank)
                throw new OWLSemanticsException("Cannot annotate individual because given \"annotationProperty\" parameter is a blank predicate");
            if (annotationValue == null)
                throw new OWLSemanticsException("Cannot annotate individual because given \"annotationValue\" parameter is null");

            //Add knowledge to the O-BOX
            OBoxGraph.AddTriple(new RDFTriple(owlIndividual, annotationProperty, annotationValue));

            return this;
        }

        //RELATIONS

        /// <summary>
        /// Declares the existence of the given "Type(owlIndividual,owlClass)" relation to the data
        /// </summary>
        public OWLOntologyData DeclareIndividualType(RDFResource owlIndividual, RDFResource owlClass)
            => DeclareIndividualType(owlIndividual, owlClass, OWLOntologyLoaderOptions.DefaultOptions);
        internal OWLOntologyData DeclareIndividualType(RDFResource owlIndividual, RDFResource owlClass, OWLOntologyLoaderOptions loaderOptions)
        {
            #region OWL-DL Integrity Checks
            bool OWLDLIntegrityChecks()
                => !owlClass.CheckReservedClass();
            #endregion

            if (owlIndividual == null)
                throw new OWLSemanticsException("Cannot declare rdf:type relation to the data because given \"owlIndividual\" parameter is null");
            if (owlClass == null)
                throw new OWLSemanticsException("Cannot declare rdf:type relation to the data because given \"owlClass\" parameter is null");

            //Add knowledge to the A-BOX (or raise warning if violations are detected)
            if (OWLDLIntegrityChecks())
                ABoxGraph.AddTriple(new RDFTriple(owlIndividual, RDFVocabulary.RDF.TYPE, owlClass));
            else
                OWLSemanticsEvents.RaiseSemanticsWarning(string.Format("Type relation between individual '{0}' and class '{1}' cannot be declared to the data because it would violate OWL-DL integrity", owlIndividual, owlClass));

            return this;
        }

        /// <summary>
        /// Declares the existence of the given "SameAs(leftIndividual,rightIndividual)" relation to the data
        /// </summary>
        public OWLOntologyData DeclareSameIndividuals(RDFResource leftIndividual, RDFResource rightIndividual)
            => DeclareSameIndividuals(leftIndividual, rightIndividual, OWLOntologyLoaderOptions.DefaultOptions);
        internal OWLOntologyData DeclareSameIndividuals(RDFResource leftIndividual, RDFResource rightIndividual, OWLOntologyLoaderOptions loaderOptions)
        {
            #region OWL-DL Integrity Checks
            bool OWLDLIntegrityChecks()
                => !loaderOptions.EnableTaxonomyProtection || this.CheckSameAsCompatibility(leftIndividual, rightIndividual);
            #endregion

            if (leftIndividual == null)
                throw new OWLSemanticsException("Cannot declare owl:sameAs relation to the data because given \"leftIndividual\" parameter is null");
            if (rightIndividual == null)
                throw new OWLSemanticsException("Cannot declare owl:sameAs relation to the data because given \"rightIndividual\" parameter is null");
            if (leftIndividual.Equals(rightIndividual))
                throw new OWLSemanticsException("Cannot declare owl:sameAs relation to the data because given \"leftIndividual\" parameter refers to the same individual as the given \"rightIndividual\" parameter");

            //Add knowledge to the A-BOX (or raise warning if violations are detected)
            if (OWLDLIntegrityChecks())
            {
                //If instructed, we have to automatically declare entities
                if (loaderOptions.EnableAutomaticEntityDeclaration)
                {
                    DeclareIndividual(leftIndividual);
                    DeclareIndividual(rightIndividual);
                }

                ABoxGraph.AddTriple(new RDFTriple(leftIndividual, RDFVocabulary.OWL.SAME_AS, rightIndividual));

                //Also add an automatic A-BOX inference exploiting symmetry of owl:sameAs relation
                ABoxGraph.AddTriple(new RDFTriple(rightIndividual, RDFVocabulary.OWL.SAME_AS, leftIndividual));
            }
            else
                OWLSemanticsEvents.RaiseSemanticsWarning(string.Format("SameAs relation between individual '{0}' and individual '{1}' cannot be declared to the data because it would violate OWL-DL integrity", leftIndividual, rightIndividual));

            return this;
        }

        /// <summary>
        /// Declares the existence of the given "DifferentFrom(leftIndividual,rightIndividual)" relation to the data
        /// </summary>
        public OWLOntologyData DeclareDifferentIndividuals(RDFResource leftIndividual, RDFResource rightIndividual)
            => DeclareDifferentIndividuals(leftIndividual, rightIndividual, OWLOntologyLoaderOptions.DefaultOptions);
        internal OWLOntologyData DeclareDifferentIndividuals(RDFResource leftIndividual, RDFResource rightIndividual, OWLOntologyLoaderOptions loaderOptions)
        {
            #region OWL-DL Integrity Checks
            bool OWLDLIntegrityChecks()
                => !loaderOptions.EnableTaxonomyProtection || this.CheckDifferentFromCompatibility(leftIndividual, rightIndividual);
            #endregion

            if (leftIndividual == null)
                throw new OWLSemanticsException("Cannot declare owl:differentFrom relation to the data because given \"leftIndividual\" parameter is null");
            if (rightIndividual == null)
                throw new OWLSemanticsException("Cannot declare owl:differentFrom relation to the data because given \"rightIndividual\" parameter is null");
            if (leftIndividual.Equals(rightIndividual))
                throw new OWLSemanticsException("Cannot declare owl:differentFrom relation to the data because given \"leftIndividual\" parameter refers to the same individual as the given \"rightIndividual\" parameter");

            //Add knowledge to the A-BOX (or raise warning if violations are detected)
            if (OWLDLIntegrityChecks())
            {
                //If instructed, we have to automatically declare entities
                if (loaderOptions.EnableAutomaticEntityDeclaration)
                {
                    DeclareIndividual(leftIndividual);
                    DeclareIndividual(rightIndividual);
                }

                ABoxGraph.AddTriple(new RDFTriple(leftIndividual, RDFVocabulary.OWL.DIFFERENT_FROM, rightIndividual));

                //Also add an automatic A-BOX inference exploiting symmetry of owl:differentFrom relation
                ABoxGraph.AddTriple(new RDFTriple(rightIndividual, RDFVocabulary.OWL.DIFFERENT_FROM, leftIndividual));
            }
            else
                OWLSemanticsEvents.RaiseSemanticsWarning(string.Format("DifferentFrom relation between individual '{0}' and individual '{1}' cannot be declared to the data because it would violate OWL-DL integrity", leftIndividual, rightIndividual));

            return this;
        }

        /// <summary>
        /// Declares the existence of the given owl:AllDifferent individuals to the data [OWL2]
        /// </summary>
        public OWLOntologyData DeclareAllDifferentIndividuals(RDFResource owlClass, List<RDFResource> differentIndividuals)
            => DeclareAllDifferentIndividuals(owlClass, differentIndividuals, OWLOntologyLoaderOptions.DefaultOptions);
        internal OWLOntologyData DeclareAllDifferentIndividuals(RDFResource owlClass, List<RDFResource> differentIndividuals, OWLOntologyLoaderOptions loaderOptions)
        {
            if (owlClass == null)
                throw new OWLSemanticsException("Cannot declare owl:AllDifferent class to the data because given \"owlClass\" parameter is null");
            if (differentIndividuals == null)
                throw new OWLSemanticsException("Cannot declare owl:AllDifferent class to the data because given \"differentIndividuals\" parameter is null");
            if (differentIndividuals.Count == 0)
                throw new OWLSemanticsException("Cannot declare owl:AllDifferent class to the data because given \"differentIndividuals\" parameter is an empty list");

            //If instructed, we have to automatically declare entities
            if (loaderOptions.EnableAutomaticEntityDeclaration)
                differentIndividuals.ForEach(idv => DeclareIndividual(idv));

            //Add knowledge to the A-BOX
            RDFCollection allDifferentIndividualsCollection = new RDFCollection(RDFModelEnums.RDFItemTypes.Resource);
            differentIndividuals.ForEach(differentIndividual => allDifferentIndividualsCollection.AddItem(differentIndividual));
            ABoxGraph.AddCollection(allDifferentIndividualsCollection);
            ABoxGraph.AddTriple(new RDFTriple(owlClass, RDFVocabulary.OWL.DISTINCT_MEMBERS, allDifferentIndividualsCollection.ReificationSubject));
            ABoxGraph.AddTriple(new RDFTriple(owlClass, RDFVocabulary.RDF.TYPE, RDFVocabulary.OWL.ALL_DIFFERENT));

            return this;
        }

        /// <summary>
        /// Declares the existence of the given "ObjectProperty(leftIndividual,rightIndividual)" assertion to the data
        /// </summary>
        public OWLOntologyData DeclareObjectAssertion(RDFResource leftIndividual, RDFResource objectProperty, RDFResource rightIndividual)
            => DeclareObjectAssertion(leftIndividual, objectProperty, rightIndividual, OWLOntologyLoaderOptions.DefaultOptions);
        internal OWLOntologyData DeclareObjectAssertion(RDFResource leftIndividual, RDFResource objectProperty, RDFResource rightIndividual, OWLOntologyLoaderOptions loaderOptions)
        {
            #region OWL-DL Integrity Checks
            bool OWLDLIntegrityChecks()
                => !objectProperty.CheckReservedProperty()
                     && (!loaderOptions.EnableTaxonomyProtection || this.CheckObjectAssertionCompatibility(leftIndividual, objectProperty, rightIndividual));
            #endregion

            if (leftIndividual == null)
                throw new OWLSemanticsException("Cannot declare object assertion relation to the data because given \"leftIndividual\" parameter is null");
            if (objectProperty == null)
                throw new OWLSemanticsException("Cannot declare object assertion relation to the data because given \"objectProperty\" parameter is null");
            if (objectProperty.IsBlank)
                throw new OWLSemanticsException("Cannot declare object assertion relation to the data because given \"objectProperty\" parameter is a blank predicate");
            if (rightIndividual == null)
                throw new OWLSemanticsException("Cannot declare object assertion relation to the data because given \"rightIndividual\" parameter is null");

            //Add knowledge to the A-BOX (or raise warning if violations are detected)
            if (OWLDLIntegrityChecks())
            {
                //If instructed, we have to automatically declare entities
                if (loaderOptions.EnableAutomaticEntityDeclaration)
                {
                    DeclareIndividual(leftIndividual);
                    DeclareIndividual(rightIndividual);
                }

                ABoxGraph.AddTriple(new RDFTriple(leftIndividual, objectProperty, rightIndividual));
            }
            else
                OWLSemanticsEvents.RaiseSemanticsWarning(string.Format("ObjectAssertion relation between individual '{0}' and individual '{1}' through property '{2}' cannot be declared to the data because it would violate OWL-DL integrity", leftIndividual, rightIndividual, objectProperty));

            return this;
        }

        /// <summary>
        /// Declares the existence of the given "DatatypeProperty(individual,value)" assertion to the data
        /// </summary>
        public OWLOntologyData DeclareDatatypeAssertion(RDFResource individual, RDFResource datatypeProperty, RDFLiteral value)
            => DeclareDatatypeAssertion(individual, datatypeProperty, value, OWLOntologyLoaderOptions.DefaultOptions);
        internal OWLOntologyData DeclareDatatypeAssertion(RDFResource individual, RDFResource datatypeProperty, RDFLiteral value, OWLOntologyLoaderOptions loaderOptions)
        {
            #region OWL-DL Integrity Checks
            bool OWLDLIntegrityChecks()
                => !datatypeProperty.CheckReservedProperty()
                     && (!loaderOptions.EnableTaxonomyProtection || this.CheckDatatypeAssertionCompatibility(individual, datatypeProperty, value));
            #endregion

            if (individual == null)
                throw new OWLSemanticsException("Cannot declare datatype assertion relation to the data because given \"individual\" parameter is null");
            if (datatypeProperty == null)
                throw new OWLSemanticsException("Cannot declare datatype assertion relation to the data because given \"datatypeProperty\" parameter is null");
            if (datatypeProperty.IsBlank)
                throw new OWLSemanticsException("Cannot declare datatype assertion relation to the data because given \"datatypeProperty\" parameter is a blank predicate");
            if (value == null)
                throw new OWLSemanticsException("Cannot declare datatype assertion relation to the data because given \"value\" parameter is null");

            //Add knowledge to the A-BOX (or raise warning if violations are detected)
            if (OWLDLIntegrityChecks())
            {
                //If instructed, we have to automatically declare entities
                if (loaderOptions.EnableAutomaticEntityDeclaration)
                    DeclareIndividual(individual);

                ABoxGraph.AddTriple(new RDFTriple(individual, datatypeProperty, value));
            }
            else
                OWLSemanticsEvents.RaiseSemanticsWarning(string.Format("DatatypeAssertion relation between individual '{0}' and value '{1}' through property '{2}' cannot be declared to the data because it would violate OWL-DL integrity", individual, value, datatypeProperty));

            return this;
        }

        /// <summary>
        /// Declares the existence of the given "ObjectProperty(leftIndividual,rightIndividual)" negative assertion to the data [OWL2]
        /// </summary>
        public OWLOntologyData DeclareNegativeObjectAssertion(RDFResource leftIndividual, RDFResource objectProperty, RDFResource rightIndividual)
            => DeclareNegativeObjectAssertion(leftIndividual, objectProperty, rightIndividual, OWLOntologyLoaderOptions.DefaultOptions);
        internal OWLOntologyData DeclareNegativeObjectAssertion(RDFResource leftIndividual, RDFResource objectProperty, RDFResource rightIndividual, OWLOntologyLoaderOptions loaderOptions)
        {
            #region OWL-DL Integrity Checks
            bool OWLDLIntegrityChecks()
                => !objectProperty.CheckReservedProperty()
                     && (!loaderOptions.EnableTaxonomyProtection || this.CheckNegativeObjectAssertionCompatibility(leftIndividual, objectProperty, rightIndividual));
            #endregion

            if (leftIndividual == null)
                throw new OWLSemanticsException("Cannot declare negative object assertion relation to the data because given \"leftIndividual\" parameter is null");
            if (objectProperty == null)
                throw new OWLSemanticsException("Cannot declare negative object assertion relation to the data because given \"objectProperty\" parameter is null");
            if (objectProperty.IsBlank)
                throw new OWLSemanticsException("Cannot declare negative object assertion relation to the data because given \"objectProperty\" parameter is a blank predicate");
            if (rightIndividual == null)
                throw new OWLSemanticsException("Cannot declare negative object assertion relation to the data because given \"rightIndividual\" parameter is null");

            //Add knowledge to the A-BOX (or raise warning if violations are detected)
            if (OWLDLIntegrityChecks())
            {
                //If instructed, we have to automatically declare entities
                if (loaderOptions.EnableAutomaticEntityDeclaration)
                {
                    DeclareIndividual(leftIndividual);
                    DeclareIndividual(rightIndividual);
                }

                RDFTriple negativeObjectAssertion = new RDFTriple(leftIndividual, objectProperty, rightIndividual);
                ABoxGraph.AddTriple(new RDFTriple(negativeObjectAssertion.ReificationSubject, RDFVocabulary.OWL.SOURCE_INDIVIDUAL, leftIndividual));
                ABoxGraph.AddTriple(new RDFTriple(negativeObjectAssertion.ReificationSubject, RDFVocabulary.OWL.ASSERTION_PROPERTY, objectProperty));
                ABoxGraph.AddTriple(new RDFTriple(negativeObjectAssertion.ReificationSubject, RDFVocabulary.OWL.TARGET_INDIVIDUAL, rightIndividual));
                ABoxGraph.AddTriple(new RDFTriple(negativeObjectAssertion.ReificationSubject, RDFVocabulary.RDF.TYPE, RDFVocabulary.OWL.NEGATIVE_PROPERTY_ASSERTION));
            }
            else
                OWLSemanticsEvents.RaiseSemanticsWarning(string.Format("NegativeObjectAssertion relation between individual '{0}' and individual '{1}' through property '{2}' cannot be declared to the data because it would violate OWL-DL integrity", leftIndividual, rightIndividual, objectProperty));

            return this;
        }

        /// <summary>
        /// Declares the existence of the given "DatatypeProperty(individual,value)" negative assertion to the data [OWL2]
        /// </summary>
        public OWLOntologyData DeclareNegativeDatatypeAssertion(RDFResource individual, RDFResource datatypeProperty, RDFLiteral value)
            => DeclareNegativeDatatypeAssertion(individual, datatypeProperty, value, OWLOntologyLoaderOptions.DefaultOptions);
        internal OWLOntologyData DeclareNegativeDatatypeAssertion(RDFResource individual, RDFResource datatypeProperty, RDFLiteral value, OWLOntologyLoaderOptions loaderOptions)
        {
            #region OWL-DL Integrity Checks
            bool OWLDLIntegrityChecks()
                => !datatypeProperty.CheckReservedProperty()
                     && (!loaderOptions.EnableTaxonomyProtection || this.CheckNegativeDatatypeAssertionCompatibility(individual, datatypeProperty, value));
            #endregion

            if (individual == null)
                throw new OWLSemanticsException("Cannot declare negative datatype assertion relation to the data because given \"individual\" parameter is null");
            if (datatypeProperty == null)
                throw new OWLSemanticsException("Cannot declare negative datatype assertion relation to the data because given \"datatypeProperty\" parameter is null");
            if (datatypeProperty.IsBlank)
                throw new OWLSemanticsException("Cannot declare negative datatype assertion relation to the data because given \"datatypeProperty\" parameter is a blank predicate");
            if (value == null)
                throw new OWLSemanticsException("Cannot declare negative datatype assertion relation to the data because given \"value\" parameter is null");

            //Add knowledge to the A-BOX (or raise warning if violations are detected)
            if (OWLDLIntegrityChecks())
            {
                //If instructed, we have to automatically declare entities
                if (loaderOptions.EnableAutomaticEntityDeclaration)
                    DeclareIndividual(individual);

                RDFTriple negativeDatatypeAssertion = new RDFTriple(individual, datatypeProperty, value);
                ABoxGraph.AddTriple(new RDFTriple(negativeDatatypeAssertion.ReificationSubject, RDFVocabulary.OWL.SOURCE_INDIVIDUAL, individual));
                ABoxGraph.AddTriple(new RDFTriple(negativeDatatypeAssertion.ReificationSubject, RDFVocabulary.OWL.ASSERTION_PROPERTY, datatypeProperty));
                ABoxGraph.AddTriple(new RDFTriple(negativeDatatypeAssertion.ReificationSubject, RDFVocabulary.OWL.TARGET_VALUE, value));
                ABoxGraph.AddTriple(new RDFTriple(negativeDatatypeAssertion.ReificationSubject, RDFVocabulary.RDF.TYPE, RDFVocabulary.OWL.NEGATIVE_PROPERTY_ASSERTION));
            }
            else
                OWLSemanticsEvents.RaiseSemanticsWarning(string.Format("NegativeDatatypeAssertion relation between individual '{0}' and value '{1}' through property '{2}' cannot be declared to the data because it would violate OWL-DL integrity", individual, value, datatypeProperty));

            return this;
        }

        //EXPORT

        /// <summary>
        /// Gets a graph representation of the data
        /// </summary>
        public RDFGraph ToRDFGraph()
            => ABoxGraph.UnionWith(OBoxGraph);

        /// <summary>
        /// Asynchronously gets a graph representation of the data
        /// </summary>
        public Task<RDFGraph> ToRDFGraphAsync()
            => Task.Run(() => ToRDFGraph());
        #endregion
    }
}