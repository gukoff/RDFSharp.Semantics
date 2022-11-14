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
                throw new OWLSemanticsException("Cannot add custom rule to validator because given \"customRule\" parameter is null");

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

                //Initialize validator registry
                Dictionary<string, OWLValidatorReport> validatorRegistry = new Dictionary<string, OWLValidatorReport>();
                foreach (OWLSemanticsEnums.OWLValidatorStandardRules standardRule in StandardRules)
                    validatorRegistry.Add(standardRule.ToString(), null);
                foreach (OWLValidatorRule customRule in CustomRules)
                    validatorRegistry.Add(customRule.RuleName, null);

                //Execute standard rules
                Parallel.ForEach(StandardRules, 
                    standardRule =>
                    {
                        OWLSemanticsEvents.RaiseSemanticsInfo($"Launching standard validator rule '{standardRule}'");

                        switch (standardRule)
                        {
                            case OWLSemanticsEnums.OWLValidatorStandardRules.TermDisjointness:
                                validatorRegistry[OWLSemanticsEnums.OWLValidatorStandardRules.TermDisjointness.ToString()] = OWLTermDisjointnessRule.ExecuteRule(ontology);
                                break;
                            case OWLSemanticsEnums.OWLValidatorStandardRules.TermDeclaration:
                                validatorRegistry[OWLSemanticsEnums.OWLValidatorStandardRules.TermDeclaration.ToString()] = OWLTermDeclarationRule.ExecuteRule(ontology);
                                break;
                            case OWLSemanticsEnums.OWLValidatorStandardRules.TermDeprecation:
                                validatorRegistry[OWLSemanticsEnums.OWLValidatorStandardRules.TermDeprecation.ToString()] = OWLTermDeprecationRule.ExecuteRule(ontology);
                                break;
                            case OWLSemanticsEnums.OWLValidatorStandardRules.DomainRange:
                                validatorRegistry[OWLSemanticsEnums.OWLValidatorStandardRules.DomainRange.ToString()] = OWLDomainRangeRule.ExecuteRule(ontology);
                                break;
                            case OWLSemanticsEnums.OWLValidatorStandardRules.InverseOf:
                                validatorRegistry[OWLSemanticsEnums.OWLValidatorStandardRules.InverseOf.ToString()] = OWLInverseOfRule.ExecuteRule(ontology);
                                break;
                            case OWLSemanticsEnums.OWLValidatorStandardRules.SymmetricProperty:
                                validatorRegistry[OWLSemanticsEnums.OWLValidatorStandardRules.SymmetricProperty.ToString()] = OWLSymmetricPropertyRule.ExecuteRule(ontology);
                                break;
                            case OWLSemanticsEnums.OWLValidatorStandardRules.AsymmetricProperty:
                                validatorRegistry[OWLSemanticsEnums.OWLValidatorStandardRules.AsymmetricProperty.ToString()] = OWLAsymmetricPropertyRule.ExecuteRule(ontology);
                                break;
                            case OWLSemanticsEnums.OWLValidatorStandardRules.IrreflexiveProperty:
                                validatorRegistry[OWLSemanticsEnums.OWLValidatorStandardRules.IrreflexiveProperty.ToString()] = OWLIrreflexivePropertyRule.ExecuteRule(ontology);
                                break;
                            case OWLSemanticsEnums.OWLValidatorStandardRules.PropertyDisjoint:
                                validatorRegistry[OWLSemanticsEnums.OWLValidatorStandardRules.PropertyDisjoint.ToString()] = OWLPropertyDisjointRule.ExecuteRule(ontology);
                                break;
                            case OWLSemanticsEnums.OWLValidatorStandardRules.ClassKey:
                                validatorRegistry[OWLSemanticsEnums.OWLValidatorStandardRules.ClassKey.ToString()] = OWLClassKeyRule.ExecuteRule(ontology);
                                break;
                            case OWLSemanticsEnums.OWLValidatorStandardRules.PropertyChainAxiom:
                                validatorRegistry[OWLSemanticsEnums.OWLValidatorStandardRules.PropertyChainAxiom.ToString()] = OWLPropertyChainAxiomRule.ExecuteRule(ontology);
                                break;
                            case OWLSemanticsEnums.OWLValidatorStandardRules.ClassType:
                                validatorRegistry[OWLSemanticsEnums.OWLValidatorStandardRules.ClassType.ToString()] = OWLClassTypeRule.ExecuteRule(ontology);
                                break;
                            case OWLSemanticsEnums.OWLValidatorStandardRules.NegativeAssertions:
                                validatorRegistry[OWLSemanticsEnums.OWLValidatorStandardRules.NegativeAssertions.ToString()] = OWLNegativeAssertionsRule.ExecuteRule(ontology);
                                break;
                            case OWLSemanticsEnums.OWLValidatorStandardRules.GlobalCardinality:
                                validatorRegistry[OWLSemanticsEnums.OWLValidatorStandardRules.GlobalCardinality.ToString()] = OWLGlobalCardinalityRule.ExecuteRule(ontology);
                                break;
                            case OWLSemanticsEnums.OWLValidatorStandardRules.LocalCardinality:
                                validatorRegistry[OWLSemanticsEnums.OWLValidatorStandardRules.LocalCardinality.ToString()] = OWLLocalCardinalityRule.ExecuteRule(ontology);
                                break;
                            case OWLSemanticsEnums.OWLValidatorStandardRules.PropertyConsistency:
                                validatorRegistry[OWLSemanticsEnums.OWLValidatorStandardRules.PropertyConsistency.ToString()] = OWLPropertyConsistencyRule.ExecuteRule(ontology);
                                break;
                        }

                        OWLSemanticsEvents.RaiseSemanticsInfo($"Completed standard validator rule '{standardRule}': found {validatorRegistry[standardRule.ToString()].EvidencesCount} evidences");
                    });

                //Execute custom rules
                Parallel.ForEach(CustomRules, 
                    customRule =>
                    {
                        OWLSemanticsEvents.RaiseSemanticsInfo($"Launching custom validator rule '{customRule.RuleName}'");

                        validatorRegistry[customRule.RuleName] = customRule.ExecuteRule(ontology);

                        OWLSemanticsEvents.RaiseSemanticsInfo($"Completed custom validator rule '{customRule.RuleName}': found {validatorRegistry[customRule.RuleName].EvidencesCount} evidences");
                    });

                //Process validator registry
                foreach (OWLValidatorReport validatorRegistryReport in validatorRegistry.Values)
                    validatorReport.MergeEvidences(validatorRegistryReport);

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