﻿/*
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

using static RDFSharp.Semantics.OWLValidatorRule;

namespace RDFSharp.Semantics
{
    /// <summary>
    /// OWLValidatorRule represents a rule which analyzes a specific syntactic/semantic aspect of a given ontology
    /// </summary>
    public class OWLValidatorRule
    {
        #region Properties
        /// <summary>
        /// Name of the rule
        /// </summary>
        public string RuleName { get; internal set; }

        /// <summary>
        /// Description of the rule
        /// </summary>
        public string RuleDescription { get; internal set; }

        /// <summary>
        /// Delegate definition for the function which will be executed as body of the rule
        /// </summary>
        public delegate OWLValidatorReport ValidatorRuleDelegate(OWLOntology ontology);

        /// <summary>
        /// Function which will be executed as body of the rule
        /// </summary>
        public ValidatorRuleDelegate ExecuteRule { get; set; }
        #endregion

        #region Ctors
        /// <summary>
        /// Default-ctor to build a validator rule with given name, description and delegate
        /// </summary>
        public OWLValidatorRule(string ruleName, string ruleDescription, ValidatorRuleDelegate ruleDelegate)
        {
            if (ruleDelegate == null)
                throw new OWLSemanticsException("Cannot create validator rule because given \"ruleDelegate\" parameter is null");

            RuleName = ruleName;
            RuleDescription = ruleDescription;
            ExecuteRule = ruleDelegate;
        }
        #endregion
    }
}