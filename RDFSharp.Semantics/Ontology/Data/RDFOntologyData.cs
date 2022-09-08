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

using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using RDFSharp.Model;

namespace RDFSharp.Semantics
{
    /// <summary>
    /// RDFOntologyData represents the A-BOX available to an ontology from the application domain
    /// </summary>
    public class RDFOntologyData : IEnumerable<RDFResource>
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
        /// A-BOX knowledge inferred
        /// </summary>
        internal RDFGraph ABoxInferenceGraph { get; set; }

        /// <summary>
        /// A-BOX virtual knowledge (comprehensive of both available and inferred)
        /// </summary>
        internal RDFGraph ABoxVirtualGraph 
            => ABoxGraph.UnionWith(ABoxInferenceGraph);
        #endregion

        #region Ctors
        /// <summary>
        /// Builds an empty ontology data
        /// </summary>
        public RDFOntologyData()
        {
            Individuals = new Dictionary<long, RDFResource>();
            ABoxGraph = new RDFGraph();
            ABoxInferenceGraph = new RDFGraph();
        }
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
        #endregion

        #region Methods
        /// <summary>
        /// Declares the given owl:NamedIndividual instance to the data
        /// </summary>
        public RDFOntologyData DeclareIndividual(RDFResource owlIndividual)
        {
            if (owlIndividual == null)
                throw new RDFSemanticsException("Cannot declare owl:Individual instance to the data because given \"owlIndividual\" parameter is null");

            //Declare individual to the data
            if (!Individuals.ContainsKey(owlIndividual.PatternMemberID))
                Individuals.Add(owlIndividual.PatternMemberID, owlIndividual);

            //Add knowledge to the A-BOX
            ABoxGraph.AddTriple(new RDFTriple(owlIndividual, RDFVocabulary.RDF.TYPE, RDFVocabulary.OWL.NAMED_INDIVIDUAL));

            return this;
        }

        //TAXONOMIES

        /// <summary>
        /// Annotates the given individual with the given "annotationProperty -> annotationValue"
        /// </summary>
        public RDFOntologyData AnnotateIndividual(RDFResource owlIndividual, RDFResource annotationProperty, RDFResource annotationValue)
        {
            if (owlIndividual == null)
                throw new RDFSemanticsException("Cannot annotate individual because given \"owlIndividual\" parameter is null");
            if (annotationProperty == null)
                throw new RDFSemanticsException("Cannot annotate individual because given \"annotationProperty\" parameter is null");
            if (annotationProperty.IsBlank)
                throw new RDFSemanticsException("Cannot annotate individual because given \"annotationProperty\" parameter is a blank predicate");
            if (annotationValue == null)
                throw new RDFSemanticsException("Cannot annotate individual because given \"annotationValue\" parameter is null");

            //Add knowledge to the A-BOX
            ABoxGraph.AddTriple(new RDFTriple(owlIndividual, annotationProperty, annotationValue));

            return this;
        }

        /// <summary>
        /// Annotates the given individual with the given "annotationProperty -> annotationValue"
        /// </summary>
        public RDFOntologyData AnnotateIndividual(RDFResource owlIndividual, RDFResource annotationProperty, RDFLiteral annotationValue)
        {
            if (owlIndividual == null)
                throw new RDFSemanticsException("Cannot annotate individual because given \"owlIndividual\" parameter is null");
            if (annotationProperty == null)
                throw new RDFSemanticsException("Cannot annotate individual because given \"annotationProperty\" parameter is null");
            if (annotationProperty.IsBlank)
                throw new RDFSemanticsException("Cannot annotate individual because given \"annotationProperty\" parameter is a blank predicate");
            if (annotationValue == null)
                throw new RDFSemanticsException("Cannot annotate individual because given \"annotationValue\" parameter is null");

            //Add knowledge to the A-BOX
            ABoxGraph.AddTriple(new RDFTriple(owlIndividual, annotationProperty, annotationValue));

            return this;
        }

        /// <summary>
        /// Declares the existence of the given "Type(owlIndividual,owlClass)" relation to the data
        /// </summary>
        public RDFOntologyData DeclareIndividualType(RDFResource owlIndividual, RDFResource owlClass)
        {
            #region OWL-DL Integrity Checks
            bool OWLDLIntegrityChecks()
                => !owlClass.CheckReservedClass();
            #endregion

            if (owlIndividual == null)
                throw new RDFSemanticsException("Cannot declare rdf:type relation because given \"owlIndividual\" parameter is null");
            if (owlClass == null)
                throw new RDFSemanticsException("Cannot declare rdf:type relation because given \"owlClass\" parameter is null");

            //Add knowledge to the A-BOX (or raise warning if integrity policy is active and violations are detected)
            if (!RDFSemanticsOptions.ShouldCheckOWLDLIntegrity || OWLDLIntegrityChecks())
                ABoxGraph.AddTriple(new RDFTriple(owlIndividual, RDFVocabulary.RDF.TYPE, owlClass));
            else
                RDFSemanticsEvents.RaiseSemanticsWarning(string.Format("Type relation between individual '{0}' and class '{1}' cannot be added to the data because it would violate OWL-DL integrity", owlIndividual, owlClass));

            return this;
        }

        /// <summary>
        /// Declares the existence of the given "SameAs(leftIndividual,rightIndividual)" relation to the data
        /// </summary>
        public RDFOntologyData DeclareSameIndividuals(RDFResource leftIndividual, RDFResource rightIndividual)
        {
            #region OWL-DL Integrity Checks
            bool OWLDLIntegrityChecks()
                => this.CheckSameAsCompatibility(leftIndividual, rightIndividual);
            #endregion

            if (leftIndividual == null)
                throw new RDFSemanticsException("Cannot declare owl:sameAs relation because given \"leftIndividual\" parameter is null");
            if (rightIndividual == null)
                throw new RDFSemanticsException("Cannot declare owl:sameAs relation because given \"rightIndividual\" parameter is null");
            if (leftIndividual.Equals(rightIndividual))
                throw new RDFSemanticsException("Cannot declare owl:sameAs relation because given \"leftIndividual\" parameter refers to the same individual as the given \"rightIndividual\" parameter");

            //Add knowledge to the A-BOX (or raise warning if integrity policy is active and violations are detected)
            if (!RDFSemanticsOptions.ShouldCheckOWLDLIntegrity || OWLDLIntegrityChecks())
            {
                ABoxGraph.AddTriple(new RDFTriple(leftIndividual, RDFVocabulary.OWL.SAME_AS, rightIndividual));

                //Also add an automatic A-BOX inference exploiting symmetry of owl:sameAs relation
                ABoxInferenceGraph.AddTriple(new RDFTriple(rightIndividual, RDFVocabulary.OWL.SAME_AS, leftIndividual));
            }
            else
                RDFSemanticsEvents.RaiseSemanticsWarning(string.Format("SameAs relation between individual '{0}' and individual '{1}' cannot be added to the data because it would violate OWL-DL integrity", leftIndividual, rightIndividual));

            return this;
        }

        /// <summary>
        /// Declares the existence of the given "DifferentFrom(leftIndividual,rightIndividual)" relation to the data
        /// </summary>
        public RDFOntologyData DeclareDifferentIndividuals(RDFResource leftIndividual, RDFResource rightIndividual)
        {
            #region OWL-DL Integrity Checks
            bool OWLDLIntegrityChecks()
                => this.CheckDifferentFromCompatibility(leftIndividual, rightIndividual);
            #endregion

            if (leftIndividual == null)
                throw new RDFSemanticsException("Cannot declare owl:differentFrom relation because given \"leftIndividual\" parameter is null");
            if (rightIndividual == null)
                throw new RDFSemanticsException("Cannot declare owl:differentFrom relation because given \"rightIndividual\" parameter is null");
            if (leftIndividual.Equals(rightIndividual))
                throw new RDFSemanticsException("Cannot declare owl:differentFrom relation because given \"leftIndividual\" parameter refers to the same individual as the given \"rightIndividual\" parameter");

            //Add knowledge to the A-BOX (or raise warning if integrity policy is active and violations are detected)
            if (!RDFSemanticsOptions.ShouldCheckOWLDLIntegrity || OWLDLIntegrityChecks())
            {
                ABoxGraph.AddTriple(new RDFTriple(leftIndividual, RDFVocabulary.OWL.DIFFERENT_FROM, rightIndividual));

                //Also add an automatic A-BOX inference exploiting symmetry of owl:differentFrom relation
                ABoxInferenceGraph.AddTriple(new RDFTriple(rightIndividual, RDFVocabulary.OWL.DIFFERENT_FROM, leftIndividual));
            }
            else
                RDFSemanticsEvents.RaiseSemanticsWarning(string.Format("DifferentFrom relation between individual '{0}' and individual '{1}' cannot be added to the data because it would violate OWL-DL integrity", leftIndividual, rightIndividual));

            return this;
        }

        /// <summary>
        /// Declares the existence of the given owl:AllDifferent class to the data [OWL2]
        /// </summary>
        public RDFOntologyData DeclareAllDifferentIndividuals(RDFResource owlClass, List<RDFResource> differentIndividuals)
        {
            if (owlClass == null)
                throw new RDFSemanticsException("Cannot declare owl:AllDifferent class to the data because given \"owlClass\" parameter is null");
            if (differentIndividuals == null)
                differentIndividuals = new List<RDFResource>();

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
        public RDFOntologyData DeclareObjectAssertion(RDFResource leftIndividual, RDFResource objectProperty, RDFResource rightIndividual)
        {
            #region OWL-DL Integrity Checks
            bool OWLDLIntegrityChecks()
                => !objectProperty.CheckReservedProperty()
                     && this.CheckObjectAssertionCompatibility(leftIndividual, objectProperty, rightIndividual);
            #endregion

            if (leftIndividual == null)
                throw new RDFSemanticsException("Cannot declare object assertion relation because given \"leftIndividual\" parameter is null");
            if (objectProperty == null)
                throw new RDFSemanticsException("Cannot declare object assertion relation because given \"objectProperty\" parameter is null");
            if (rightIndividual == null)
                throw new RDFSemanticsException("Cannot declare object assertion relation because given \"rightIndividual\" parameter is null");

            //Add knowledge to the A-BOX (or raise warning if integrity policy is active and violations are detected)
            if (!RDFSemanticsOptions.ShouldCheckOWLDLIntegrity || OWLDLIntegrityChecks())
                ABoxGraph.AddTriple(new RDFTriple(leftIndividual, objectProperty, rightIndividual));
            else
                RDFSemanticsEvents.RaiseSemanticsWarning(string.Format("ObjectAssertion relation between individual '{0}' and individual '{1}' through property '{2}' cannot be added to the data because it would violate OWL-DL integrity", leftIndividual, rightIndividual, objectProperty));

            return this;
        }

        /// <summary>
        /// Declares the existence of the given "DatatypeProperty(individual,value)" assertion to the data
        /// </summary>
        public RDFOntologyData DeclareDatatypeAssertion(RDFResource individual, RDFResource datatypeProperty, RDFLiteral value)
        {
            #region OWL-DL Integrity Checks
            bool OWLDLIntegrityChecks()
                => !datatypeProperty.CheckReservedProperty()
                     && this.CheckDatatypeAssertionCompatibility(individual, datatypeProperty, value);
            #endregion

            if (individual == null)
                throw new RDFSemanticsException("Cannot declare datatype assertion relation because given \"individual\" parameter is null");
            if (datatypeProperty == null)
                throw new RDFSemanticsException("Cannot declare datatype assertion relation because given \"datatypeProperty\" parameter is null");
            if (value == null)
                throw new RDFSemanticsException("Cannot declare datatype assertion relation because given \"value\" parameter is null");

            //Add knowledge to the A-BOX (or raise warning if integrity policy is active and violations are detected)
            if (!RDFSemanticsOptions.ShouldCheckOWLDLIntegrity || OWLDLIntegrityChecks())
                ABoxGraph.AddTriple(new RDFTriple(individual, datatypeProperty, value));
            else
                RDFSemanticsEvents.RaiseSemanticsWarning(string.Format("DatatypeAssertion relation between individual '{0}' and value '{1}' through property '{2}' cannot be added to the data because it would violate OWL-DL integrity", individual, value, datatypeProperty));

            return this;
        }

        /// <summary>
        /// Declares the existence of the given "ObjectProperty(leftIndividual,rightIndividual)" negative assertion to the data [OWL2]
        /// </summary>
        public RDFOntologyData DeclareNegativeObjectAssertion(RDFResource leftIndividual, RDFResource objectProperty, RDFResource rightIndividual)
        {
            #region OWL-DL Integrity Checks
            bool OWLDLIntegrityChecks()
                => !objectProperty.CheckReservedProperty()
                     && this.CheckNegativeObjectAssertionCompatibility(leftIndividual, objectProperty, rightIndividual);
            #endregion

            if (leftIndividual == null)
                throw new RDFSemanticsException("Cannot declare negative object assertion relation because given \"leftIndividual\" parameter is null");
            if (objectProperty == null)
                throw new RDFSemanticsException("Cannot declare negative object assertion relation because given \"objectProperty\" parameter is null");
            if (rightIndividual == null)
                throw new RDFSemanticsException("Cannot declare negative object assertion relation because given \"rightIndividual\" parameter is null");

            //Add knowledge to the A-BOX (or raise warning if integrity policy is active and violations are detected)
            if (!RDFSemanticsOptions.ShouldCheckOWLDLIntegrity || OWLDLIntegrityChecks())
            {
                RDFTriple negativeObjectAssertion = new RDFTriple(leftIndividual, objectProperty, rightIndividual);
                ABoxGraph.AddTriple(new RDFTriple(negativeObjectAssertion.ReificationSubject, RDFVocabulary.OWL.SOURCE_INDIVIDUAL, leftIndividual));
                ABoxGraph.AddTriple(new RDFTriple(negativeObjectAssertion.ReificationSubject, RDFVocabulary.OWL.ASSERTION_PROPERTY, objectProperty));
                ABoxGraph.AddTriple(new RDFTriple(negativeObjectAssertion.ReificationSubject, RDFVocabulary.OWL.TARGET_INDIVIDUAL, rightIndividual));
                ABoxGraph.AddTriple(new RDFTriple(negativeObjectAssertion.ReificationSubject, RDFVocabulary.RDF.TYPE, RDFVocabulary.OWL.NEGATIVE_PROPERTY_ASSERTION));
            }
            else
                RDFSemanticsEvents.RaiseSemanticsWarning(string.Format("NegativeObjectAssertion relation between individual '{0}' and individual '{1}' through property '{2}' cannot be added to the data because it would violate OWL-DL integrity", leftIndividual, rightIndividual, objectProperty));

            return this;
        }

        /// <summary>
        /// Declares the existence of the given "DatatypeProperty(individual,value)" negative assertion to the data [OWL2]
        /// </summary>
        public RDFOntologyData DeclareNegativeDatatypeAssertion(RDFResource individual, RDFResource datatypeProperty, RDFLiteral value)
        {
            #region OWL-DL Integrity Checks
            bool OWLDLIntegrityChecks()
                => !datatypeProperty.CheckReservedProperty()
                     && this.CheckNegativeDatatypeAssertionCompatibility(individual, datatypeProperty, value);
            #endregion

            if (individual == null)
                throw new RDFSemanticsException("Cannot declare negative datatype assertion relation because given \"individual\" parameter is null");
            if (datatypeProperty == null)
                throw new RDFSemanticsException("Cannot declare negative datatype assertion relation because given \"datatypeProperty\" parameter is null");
            if (value == null)
                throw new RDFSemanticsException("Cannot declare negative datatype assertion relation because given \"value\" parameter is null");

            //Add knowledge to the A-BOX (or raise warning if integrity policy is active and violations are detected)
            if (!RDFSemanticsOptions.ShouldCheckOWLDLIntegrity || OWLDLIntegrityChecks())
            {
                RDFTriple negativeDatatypeAssertion = new RDFTriple(individual, datatypeProperty, value);
                ABoxGraph.AddTriple(new RDFTriple(negativeDatatypeAssertion.ReificationSubject, RDFVocabulary.OWL.SOURCE_INDIVIDUAL, individual));
                ABoxGraph.AddTriple(new RDFTriple(negativeDatatypeAssertion.ReificationSubject, RDFVocabulary.OWL.ASSERTION_PROPERTY, datatypeProperty));
                ABoxGraph.AddTriple(new RDFTriple(negativeDatatypeAssertion.ReificationSubject, RDFVocabulary.OWL.TARGET_VALUE, value));
                ABoxGraph.AddTriple(new RDFTriple(negativeDatatypeAssertion.ReificationSubject, RDFVocabulary.RDF.TYPE, RDFVocabulary.OWL.NEGATIVE_PROPERTY_ASSERTION));
            }
            else
                RDFSemanticsEvents.RaiseSemanticsWarning(string.Format("NegativeDatatypeAssertion relation between individual '{0}' and value '{1}' through property '{2}' cannot be added to the data because it would violate OWL-DL integrity", individual, value, datatypeProperty));

            return this;
        }

        //EXPORT

        /// <summary>
        /// Gets a graph representation of the data
        /// </summary>
        public RDFGraph ToRDFGraph(bool includeInferences)
            => includeInferences ? ABoxVirtualGraph : ABoxGraph;

        /// <summary>
        /// Asynchronously gets a graph representation of the data
        /// </summary>
        public Task<RDFGraph> ToRDFGraphAsync(bool includeInferences)
            => Task.Run(() => ToRDFGraph(includeInferences));
        #endregion
    }
}