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

using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace RDFSharp.Semantics
{
    /// <summary>
    /// OWLValidator analyzes an ontology in order to discover errors and inconsistencies affecting its model and data
    /// </summary>
    public class OWLValidator
    {
        #region Properties
        /// <summary>
        /// List of standard rules applied by the validator
        /// </summary>
        internal List<OWLSemanticsEnums.OWLValidatorStandardRules> StandardRules { get; set; }

        /// <summary>
        /// List of custom rules applied by the validator
        /// </summary>
        internal List<OWLValidatorRule> CustomRules { get; set; }
        #endregion

        #region Ctors
        /// <summary>
        /// Default-ctor to build an empty validator
        /// </summary>
        public OWLValidator()
        {
            StandardRules = new List<OWLSemanticsEnums.OWLValidatorStandardRules>();
            CustomRules = new List<OWLValidatorRule>();
        }
        #endregion

        #region Methods
        /// <summary>
        /// Adds the given standard rule to the validator
        /// </summary>
        public OWLValidator AddStandardRule(OWLSemanticsEnums.OWLValidatorStandardRules standardRule)
        {
            if (!StandardRules.Contains(standardRule))
                StandardRules.Add(standardRule);
            return this;
        }

        /// <summary>
        /// Adds the given custom rule to the validator
        /// </summary>
        public OWLValidator AddCustomRule(OWLValidatorRule customRule)
        {
            if (customRule == null)
                throw new OWLSemanticsException("Cannot add custom rule to validator because given \"customeRule\" parameter is null");

            CustomRules.Add(customRule);
            return this;
        }

        /// <summary>
        /// Applies the validator on the given ontology
        /// </summary>
        public OWLValidatorReport ApplyToOntology(OWLOntology ontology)
        {
            OWLValidatorReport validatorReport = new OWLValidatorReport();

            if (ontology != null)
            {
                OWLSemanticsEvents.RaiseSemanticsInfo($"Validator is going to be applied on Ontology '{ontology.URI}': this may require intensive processing, depending on size and complexity of domain knowledge and rules");

                //Standard Rules
                Parallel.ForEach(StandardRules, 
                    standardRule =>
                    {
                        OWLSemanticsEvents.RaiseSemanticsInfo($"Launching standard validator rule '{standardRule}'");

                        OWLValidatorReport standardRuleReport = new OWLValidatorReport();
                        switch (standardRule)
                        {
                            case OWLSemanticsEnums.OWLValidatorStandardRules.TermDisjointness:
                                standardRuleReport.MergeEvidences(OWLTermDisjointnessRule.ExecuteRule(ontology));
                                break;

                            case OWLSemanticsEnums.OWLValidatorStandardRules.TermDeclaration:
                                standardRuleReport.MergeEvidences(OWLTermDeclarationRule.ExecuteRule(ontology));
                                break;

                            case OWLSemanticsEnums.OWLValidatorStandardRules.TermDeprecation:
                                standardRuleReport.MergeEvidences(OWLTermDeprecationRule.ExecuteRule(ontology));
                                break;

                            case OWLSemanticsEnums.OWLValidatorStandardRules.DomainRange:
                                //TODO
                                break;

                            case OWLSemanticsEnums.OWLValidatorStandardRules.InverseOf:
                                //TODO
                                break;

                            case OWLSemanticsEnums.OWLValidatorStandardRules.SymmetricProperty:
                                standardRuleReport.MergeEvidences(OWLSymmetricPropertyRule.ExecuteRule(ontology));
                                break;

                            case OWLSemanticsEnums.OWLValidatorStandardRules.AsymmetricProperty:
                                standardRuleReport.MergeEvidences(OWLAsymmetricPropertyRule.ExecuteRule(ontology));
                                break;

                            case OWLSemanticsEnums.OWLValidatorStandardRules.IrreflexiveProperty:
                                standardRuleReport.MergeEvidences(OWLIrreflexivePropertyRule.ExecuteRule(ontology));
                                break;

                            case OWLSemanticsEnums.OWLValidatorStandardRules.PropertyDisjoint:
                                //TODO
                                break;

                            case OWLSemanticsEnums.OWLValidatorStandardRules.NegativeAssertions:
                                //TODO
                                break;

                            case OWLSemanticsEnums.OWLValidatorStandardRules.HasKey:
                                //TODO
                                break;

                            case OWLSemanticsEnums.OWLValidatorStandardRules.PropertyChainAxiom:
                                //TODO
                                break;

                            case OWLSemanticsEnums.OWLValidatorStandardRules.ClassType:
                                standardRuleReport.MergeEvidences(OWLClassTypeRule.ExecuteRule(ontology));
                                break;

                            case OWLSemanticsEnums.OWLValidatorStandardRules.GlobalCardinalityConstraint:
                                //TODO
                                break;

                            case OWLSemanticsEnums.OWLValidatorStandardRules.LocalCardinalityConstraint:
                                //TODO
                                break;
                        }
                        validatorReport.MergeEvidences(standardRuleReport);

                        OWLSemanticsEvents.RaiseSemanticsInfo($"Completed standard validator rule '{standardRule}': found {standardRuleReport.EvidencesCount} evidences");
                    });

                //Custom Rules
                Parallel.ForEach(CustomRules, 
                    customRule =>
                    {
                        OWLSemanticsEvents.RaiseSemanticsInfo($"Launching custom validator rule '{customRule.RuleName}'");

                        OWLValidatorReport customRuleReport = customRule.ExecuteRule(ontology);
                        validatorReport.MergeEvidences(customRuleReport);

                        OWLSemanticsEvents.RaiseSemanticsInfo($"Completed custom validator rule '{customRule.RuleName}': found {customRuleReport.EvidencesCount} evidences");
                    });

                OWLSemanticsEvents.RaiseSemanticsInfo($"Validator has been applied on Ontology '{ontology.URI}': found {validatorReport.EvidencesCount} evidences");
            }

            return validatorReport;
        }

        /// <summary>
        /// Asynchronously applies the validator on the given ontology
        /// </summary>
        public Task<OWLValidatorReport> ApplyToOntologyAsync(OWLOntology ontology)
            => Task.Run(() => ApplyToOntology(ontology));
        #endregion
    }
}