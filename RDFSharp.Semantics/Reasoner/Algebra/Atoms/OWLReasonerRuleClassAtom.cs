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
using RDFSharp.Query;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Linq;

namespace RDFSharp.Semantics
{
    /// <summary>
    /// OWLReasonerRuleClassAtom represents a SWRL atom inferring instances of a given ontology class 
    /// </summary>
    public class OWLReasonerRuleClassAtom : OWLReasonerRuleAtom
    {
        #region Ctors
        /// <summary>
        /// Default-ctor to build a class atom with the given class and arguments
        /// </summary>
        public OWLReasonerRuleClassAtom(RDFResource owlClass, RDFVariable leftArgument)
            : base(owlClass, leftArgument, null) { }
        #endregion

        #region Methods
        /// <summary>
        /// Evaluates the atom in the context of an antecedent
        /// </summary>
        internal override DataTable EvaluateOnAntecedent(OWLOntology ontology)
        {
            string leftArgumentString = LeftArgument.ToString();

            //Initialize the structure of the atom result
            DataTable atomResult = new DataTable();
            RDFQueryEngine.AddColumn(atomResult, leftArgumentString);

            //Materialize members of the atom class
            List<RDFResource> classMembers = ontology.Data.GetIndividualsOf(ontology.Model, Predicate);
            foreach (RDFResource classMember in classMembers)
            {
                Dictionary<string, string> bindings = new Dictionary<string, string>();
                bindings.Add(leftArgumentString, classMember.ToString());

                RDFQueryEngine.AddRow(atomResult, bindings);
            }

            //Return the atom result
            return atomResult;
        }

        /// <summary>
        /// Evaluates the atom in the context of an consequent
        /// </summary>
        internal override OWLReasonerReport EvaluateOnConsequent(DataTable antecedentResults, OWLOntology ontology)
        {
            OWLReasonerReport report = new OWLReasonerReport();
            string leftArgumentString = LeftArgument.ToString();

            #region Guards
            //The antecedent results table MUST have a column corresponding to the atom's left argument
            if (!antecedentResults.Columns.Contains(leftArgumentString))
                return report;
            #endregion

            //Materialize members of the atom class
            List<RDFResource> atomClassMembers = ontology.Data.GetIndividualsOf(ontology.Model, Predicate);

            //Iterate the antecedent results table to materialize the atom's reasoner evidences
            IEnumerator rowsEnum = antecedentResults.Rows.GetEnumerator();
            while (rowsEnum.MoveNext())
            {
                DataRow currentRow = (DataRow)rowsEnum.Current;

                #region Guards
                //The current row MUST have a BOUND value in the column corresponding to the atom's left argument
                if (currentRow.IsNull(leftArgumentString))
                    continue;
                #endregion

                //Parse the value of the column corresponding to the atom's left argument
                RDFPatternMember leftArgumentValue = RDFQueryUtilities.ParseRDFPatternMember(currentRow[leftArgumentString].ToString());
                if (leftArgumentValue is RDFResource leftArgumentValueResource)
                {
                    //Protect atom's inferences with implicit taxonomy checks (only if taxonomy protection has been requested)
                    if (atomClassMembers.Any(atomClassMember => atomClassMember.Equals(leftArgumentValueResource)))
                    {
                        //Create the inference
                        RDFTriple atomInference = new RDFTriple(leftArgumentValueResource, RDFVocabulary.RDF.TYPE, Predicate);

                        //Add the inference to the report
                        if (!ontology.Data.ABoxGraph.ContainsTriple(atomInference))
                            report.AddEvidence(new OWLReasonerEvidence(OWLSemanticsEnums.OWLReasonerEvidenceCategory.Data, this.ToString(), atomInference));
                    }
                }
            }

            return report;
        }
        #endregion
    }
}