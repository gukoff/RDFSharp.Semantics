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
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace RDFSharp.Semantics.Extensions.SKOS
{
    /// <summary>
    /// SKOSLabelHelper represents the SKOS-XL extension of SKOSConceptScheme for describing relations between skos:Concept and skosxl:Label individuals
    /// </summary>
    public static class SKOSLabelHelper
    {
        #region Methods
        /// <summary>
        /// Declares the given skosxl:Label instance to the concept scheme [SKOS-XL]
        /// </summary>
        public static SKOSConceptScheme DeclareLabel(this SKOSConceptScheme conceptScheme, RDFResource skosLabel)
        {
            if (conceptScheme == null)
                throw new OWLSemanticsException("Cannot declare skosxl:Label instance to the concept scheme because given \"conceptScheme\" parameter is null");
            if (skosLabel == null)
                throw new OWLSemanticsException("Cannot declare skosxl:Label instance to the concept scheme because given \"skosLabel\" parameter is null");

            //Add knowledge to the A-BOX
            conceptScheme.Ontology.Data.DeclareIndividual(skosLabel);
            conceptScheme.Ontology.Data.DeclareIndividualType(skosLabel, RDFVocabulary.SKOS.SKOSXL.LABEL);
            conceptScheme.Ontology.Data.DeclareObjectAssertion(skosLabel, RDFVocabulary.SKOS.IN_SCHEME, conceptScheme);

            return conceptScheme;
        }

        //ANNOTATIONS

        /// <summary>
        /// Annotates the given skosxl:Label with the given "annotationProperty -> annotationValue" [SKOS-XL]
        /// </summary>
        public static SKOSConceptScheme AnnotateLabel(this SKOSConceptScheme conceptScheme, RDFResource skosxlLabel, RDFResource annotationProperty, RDFResource annotationValue)
        {
            if (conceptScheme == null)
                throw new OWLSemanticsException("Cannot annotate label because given \"conceptScheme\" parameter is null");
            if (skosxlLabel == null)
                throw new OWLSemanticsException("Cannot annotate label because given \"skosxlLabel\" parameter is null");
            if (annotationProperty == null)
                throw new OWLSemanticsException("Cannot annotate label because given \"annotationProperty\" parameter is null");
            if (annotationProperty.IsBlank)
                throw new OWLSemanticsException("Cannot annotate label because given \"annotationProperty\" parameter is a blank predicate");
            if (annotationValue == null)
                throw new OWLSemanticsException("Cannot annotate label because given \"annotationValue\" parameter is null");

            //Add knowledge to the A-BOX
            conceptScheme.Ontology.Data.ABoxGraph.AddTriple(new RDFTriple(skosxlLabel, annotationProperty, annotationValue));

            return conceptScheme;
        }

        /// <summary>
        /// Annotates the given skosxl:Label with the given "annotationProperty -> annotationValue" [SKOS-XL]
        /// </summary>
        public static SKOSConceptScheme AnnotateLabel(this SKOSConceptScheme conceptScheme, RDFResource skosxlLabel, RDFResource annotationProperty, RDFLiteral annotationValue)
        {
            if (conceptScheme == null)
                throw new OWLSemanticsException("Cannot annotate label because given \"conceptScheme\" parameter is null");
            if (skosxlLabel == null)
                throw new OWLSemanticsException("Cannot annotate label because given \"skosxlLabel\" parameter is null");
            if (annotationProperty == null)
                throw new OWLSemanticsException("Cannot annotate label because given \"annotationProperty\" parameter is null");
            if (annotationProperty.IsBlank)
                throw new OWLSemanticsException("Cannot annotate label because given \"annotationProperty\" parameter is a blank predicate");
            if (annotationValue == null)
                throw new OWLSemanticsException("Cannot annotate label because given \"annotationValue\" parameter is null");

            //Add knowledge to the A-BOX
            conceptScheme.Ontology.Data.ABoxGraph.AddTriple(new RDFTriple(skosxlLabel, annotationProperty, annotationValue));

            return conceptScheme;
        }

        //RELATIONS

        /// <summary>
        /// Declares the existence of the given "PrefLabel(skosConcept,skosxlLabel) ^ LiteralForm(skosxlLabel,preferredLabelValue)" relations to the concept scheme [SKOS-XL]
        /// </summary>
        public static SKOSConceptScheme DeclarePreferredLabel(this SKOSConceptScheme conceptScheme, RDFResource skosConcept, RDFResource skosxlLabel, RDFPlainLiteral preferredLabelValue)
        {
            #region SKOS Integrity Checks
            bool SKOSIntegrityChecks()
                => conceptScheme.CheckPreferredLabelCompatibility(skosConcept, preferredLabelValue);
            #endregion

            if (conceptScheme == null)
                throw new OWLSemanticsException("Cannot declare skosxl:prefLabel relation to the concept scheme because given \"conceptScheme\" parameter is null");
            if (skosConcept == null)
                throw new OWLSemanticsException("Cannot declare skosxl:prefLabel relation to the concept scheme because given \"skosConcept\" parameter is null");
            if (skosxlLabel == null)
                throw new OWLSemanticsException("Cannot declare skosxl:prefLabel relation to the concept scheme because given \"skosxlLabel\" parameter is null");
            if (preferredLabelValue == null)
                throw new OWLSemanticsException("Cannot declare skosxl:prefLabel relation to the concept scheme because given \"preferredLabelValue\" parameter is null");

            //Add knowledge to the A-BOX (or raise warning if violations are detected)
            if (SKOSIntegrityChecks())
            {
                //Add knowledge to the A-BOX
                conceptScheme.Ontology.Data.ABoxGraph.AddTriple(new RDFTriple(skosConcept, RDFVocabulary.SKOS.SKOSXL.PREF_LABEL, skosxlLabel));
                conceptScheme.Ontology.Data.ABoxGraph.AddTriple(new RDFTriple(skosxlLabel, RDFVocabulary.SKOS.SKOSXL.LITERAL_FORM, preferredLabelValue));
            }
            else
                OWLSemanticsEvents.RaiseSemanticsWarning(string.Format("PrefLabel relation between concept '{0}' and label '{1}' with value '{2}' cannot be declared to the concept scheme because it would violate SKOS integrity", skosConcept, skosxlLabel, preferredLabelValue));

            return conceptScheme;
        }

        /// <summary>
        /// Declares the existence of the given "AltLabel(skosConcept,skosxlLabel) ^ LiteralForm(skosxlLabel,alternativeLabelValue)" relations to the concept scheme [SKOS-XL]
        /// </summary>
        public static SKOSConceptScheme DeclareAlternativeLabel(this SKOSConceptScheme conceptScheme, RDFResource skosConcept, RDFResource skosxlLabel, RDFPlainLiteral alternativeLabelValue)
        {
            #region SKOS Integrity Checks
            bool SKOSIntegrityChecks()
                => conceptScheme.CheckAlternativeLabelCompatibility(skosConcept, alternativeLabelValue);
            #endregion

            if (conceptScheme == null)
                throw new OWLSemanticsException("Cannot declare skosxl:altLabel relation to the concept scheme because given \"conceptScheme\" parameter is null");
            if (skosConcept == null)
                throw new OWLSemanticsException("Cannot declare skosxl:altLabel relation to the concept scheme because given \"skosConcept\" parameter is null");
            if (skosxlLabel == null)
                throw new OWLSemanticsException("Cannot declare skosxl:altLabel relation to the concept scheme because given \"skosxlLabel\" parameter is null");
            if (alternativeLabelValue == null)
                throw new OWLSemanticsException("Cannot declare skosxl:altLabel relation to the concept scheme because given \"alternativeLabelValue\" parameter is null");

            //Add knowledge to the A-BOX (or raise warning if violations are detected)
            if (SKOSIntegrityChecks())
            {
                //Add knowledge to the A-BOX
                conceptScheme.Ontology.Data.ABoxGraph.AddTriple(new RDFTriple(skosConcept, RDFVocabulary.SKOS.SKOSXL.ALT_LABEL, skosxlLabel));
                conceptScheme.Ontology.Data.ABoxGraph.AddTriple(new RDFTriple(skosxlLabel, RDFVocabulary.SKOS.SKOSXL.LITERAL_FORM, alternativeLabelValue));
            }
            else
                OWLSemanticsEvents.RaiseSemanticsWarning(string.Format("AltLabel relation between concept '{0}' and label '{1}' with value '{2}' cannot be declared to the concept scheme because it would violate SKOS integrity", skosConcept, skosxlLabel, alternativeLabelValue));

            return conceptScheme;
        }

        /// <summary>
        /// Declares the existence of the given "HiddenLabel(skosConcept,skosxlLabel) ^ LiteralForm(skosxlLabel,hiddenLabelValue)" relations to the concept scheme [SKOS-XL]
        /// </summary>
        public static SKOSConceptScheme DeclareHiddenLabel(this SKOSConceptScheme conceptScheme, RDFResource skosConcept, RDFResource skosxlLabel, RDFPlainLiteral hiddenLabelValue)
        {
            #region SKOS Integrity Checks
            bool SKOSIntegrityChecks()
                => conceptScheme.CheckHiddenLabelCompatibility(skosConcept, hiddenLabelValue);
            #endregion

            if (conceptScheme == null)
                throw new OWLSemanticsException("Cannot declare skosxl:hiddenLabel relation to the concept scheme because given \"conceptScheme\" parameter is null");
            if (skosConcept == null)
                throw new OWLSemanticsException("Cannot declare skosxl:hiddenLabel relation to the concept scheme because given \"skosConcept\" parameter is null");
            if (skosxlLabel == null)
                throw new OWLSemanticsException("Cannot declare skosxl:hiddenLabel relation to the concept scheme because given \"skosxlLabel\" parameter is null");
            if (hiddenLabelValue == null)
                throw new OWLSemanticsException("Cannot declare skosxl:hiddenLabel relation to the concept scheme because given \"hiddenLabelValue\" parameter is null");

            //Add knowledge to the A-BOX (or raise warning if violations are detected)
            if (SKOSIntegrityChecks())
            {
                //Add knowledge to the A-BOX
                conceptScheme.Ontology.Data.ABoxGraph.AddTriple(new RDFTriple(skosConcept, RDFVocabulary.SKOS.SKOSXL.HIDDEN_LABEL, skosxlLabel));
                conceptScheme.Ontology.Data.ABoxGraph.AddTriple(new RDFTriple(skosxlLabel, RDFVocabulary.SKOS.SKOSXL.LITERAL_FORM, hiddenLabelValue));
            }
            else
                OWLSemanticsEvents.RaiseSemanticsWarning(string.Format("HiddenLabel relation between concept '{0}' and label '{1}' with value '{2}' cannot be declared to the concept scheme because it would violate SKOS integrity", skosConcept, skosxlLabel, hiddenLabelValue));

            return conceptScheme;
        }

        /// <summary>
        /// Declares the existence of the given "LabelRelation(leftLabel,rightLabel)" relation to the concept scheme [SKOS-XL]
        /// </summary>
        public static SKOSConceptScheme DeclareRelatedLabels(this SKOSConceptScheme conceptScheme, RDFResource leftLabel, RDFResource rightLabel)
        {
            if (conceptScheme == null)
                throw new OWLSemanticsException("Cannot declare skosxl:labelRelation relation to the concept scheme because given \"conceptScheme\" parameter is null");
            if (leftLabel == null)
                throw new OWLSemanticsException("Cannot declare skosxl:labelRelation relation to the concept scheme because given \"leftLabel\" parameter is null");
            if (rightLabel == null)
                throw new OWLSemanticsException("Cannot declare skosxl:labelRelation relation to the concept scheme because given \"rightLabel\" parameter is null");
            if (leftLabel.Equals(rightLabel))
                throw new OWLSemanticsException("Cannot declare skosxl:labelRelation relation to the concept scheme because given \"leftLabel\" parameter refers to the same label as the given \"rightLabel\" parameter");

            //Add knowledge to the A-BOX
            conceptScheme.Ontology.Data.ABoxGraph.AddTriple(new RDFTriple(leftLabel, RDFVocabulary.SKOS.SKOSXL.LABEL_RELATION, rightLabel));

            //Also add an automatic A-BOX inference exploiting simmetry of skosxl:labelRelation relation
            conceptScheme.Ontology.Data.ABoxGraph.AddTriple(new RDFTriple(rightLabel, RDFVocabulary.SKOS.SKOSXL.LABEL_RELATION, leftLabel));

            return conceptScheme;
        }

        /// <summary>
        /// Declares the existence of the given "LiteralForm(skosxlLabel,literalFormValue)" relation to the concept scheme [SKOS-XL]
        /// </summary>
        public static SKOSConceptScheme DeclareLiteralForm(this SKOSConceptScheme conceptScheme, RDFResource skosxlLabel, RDFLiteral literalFormValue)
        {
            if (conceptScheme == null)
                throw new OWLSemanticsException("Cannot declare skosxl:literalForm relation to the concept scheme because given \"conceptScheme\" parameter is null");
            if (skosxlLabel == null)
                throw new OWLSemanticsException("Cannot declare skosxl:literalForm relation to the concept scheme because given \"skosxlLabel\" parameter is null");
            if (literalFormValue == null)
                throw new OWLSemanticsException("Cannot declare skosxl:literalForm relation to the concept scheme because given \"literalFormValue\" parameter is null");

            //Add knowledge to the A-BOX
            conceptScheme.Ontology.Data.ABoxGraph.AddTriple(new RDFTriple(skosxlLabel, RDFVocabulary.SKOS.SKOSXL.LITERAL_FORM, literalFormValue));

            return conceptScheme;
        }
        #endregion
    }
}