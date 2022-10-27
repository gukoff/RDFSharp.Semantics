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

using System.Collections.Generic;
using System.Threading.Tasks;

namespace RDFSharp.Semantics
{
    /// <summary>
    /// OWLReasoner analyzes an ontology in order to infer knowledge from its model and data
    /// </summary>
    public class OWLReasoner
    {
        #region Properties
        /// <summary>
        /// List of standard rules applied by the reasoner
        /// </summary>
        internal List<OWLSemanticsEnums.OWLReasonerStandardRules> StandardRules { get; set; }

        /// <summary>
        /// List of custom rules applied by the reasoner
        /// </summary>
        internal List<OWLReasonerRule> CustomRules { get; set; }
        #endregion

        #region Ctors
        /// <summary>
        /// Default-ctor to build an empty reasoner
        /// </summary>
        public OWLReasoner()
        {
            StandardRules = new List<OWLSemanticsEnums.OWLReasonerStandardRules>();
            CustomRules = new List<OWLReasonerRule>();
        }
        #endregion

        #region Methods
        /// <summary>
        /// Adds the given standard rule to the reasoner
        /// </summary>
        public OWLReasoner AddStandardRule(OWLSemanticsEnums.OWLReasonerStandardRules standardRule)
        {
            if (!StandardRules.Contains(standardRule))
                StandardRules.Add(standardRule);
            return this;
        }

        /// <summary>
        /// Adds the given custom rule to the reasoner
        /// </summary>
        public OWLReasoner AddCustomRule(OWLReasonerRule customRule)
        {
            if (customRule == null)
                throw new OWLSemanticsException("Cannot add custom rule to reasoner because given \"customRule\" parameter is null");

            CustomRules.Add(customRule);
            return this;
        }

        /// <summary>
        /// Applies the reasoner on the given ontology
        /// </summary>
        public OWLReasonerReport ApplyToOntology(OWLOntology ontology)
        {
            OWLReasonerReport reasonerReport = new OWLReasonerReport();

            if (ontology != null)
            {
                OWLSemanticsEvents.RaiseSemanticsInfo($"Reasoner is going to be applied on Ontology '{ontology.URI}': this may require intensive processing, depending on size and complexity of domain knowledge and rules");

                //Standard Rules
                Parallel.ForEach(StandardRules, 
                    standardRule =>
                    {
                        OWLSemanticsEvents.RaiseSemanticsInfo($"Launching standard reasoner rule '{standardRule}'");

                        OWLReasonerReport standardRuleReport = new OWLReasonerReport();
                        switch (standardRule)
                        {
                            //TODO
                        }
                        reasonerReport.MergeEvidences(standardRuleReport);

                        OWLSemanticsEvents.RaiseSemanticsInfo($"Completed standard reasoner rule '{standardRule}': found {standardRuleReport.EvidencesCount} evidences");
                    });

                //Custom Rules
                Parallel.ForEach(CustomRules, 
                    customRule =>
                    {
                        OWLSemanticsEvents.RaiseSemanticsInfo($"Launching custom reasoner rule '{customRule.RuleName}'");

                        OWLReasonerReport customRuleReport = customRule.ExecuteRule(ontology);
                        reasonerReport.MergeEvidences(customRuleReport);

                        OWLSemanticsEvents.RaiseSemanticsInfo($"Completed custom reasoner rule '{customRule.RuleName}': found {customRuleReport.EvidencesCount} evidences");
                    });

                OWLSemanticsEvents.RaiseSemanticsInfo($"easoner has been applied on Ontology '{ontology.URI}': found {reasonerReport.EvidencesCount} evidences");
            }

            return reasonerReport;
        }

        /// <summary>
        /// Asynchronously applies the reasoner on the given ontology
        /// </summary>
        public Task<OWLReasonerReport> ApplyToOntologyAsync(OWLOntology ontology)
            => Task.Run(() => ApplyToOntology(ontology));
        #endregion
    }
}