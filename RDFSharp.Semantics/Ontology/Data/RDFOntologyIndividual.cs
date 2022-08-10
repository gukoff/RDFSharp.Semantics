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

namespace RDFSharp.Semantics
{
    /// <summary>
    /// RDFOntologyIndividual represents an instance of an ontology class within an ontology data.
    /// </summary>
    public class RDFOntologyIndividual : RDFOntologyResource
    {
        #region Ctors
        /// <summary>
        /// Default-ctor to build an ontology individual with the given name.
        /// </summary>
        public RDFOntologyIndividual(RDFResource individualName)
        {
            if (individualName == null)
                throw new RDFSemanticsException("Cannot create RDFOntologyIndividual because given \"individualName\" parameter is null.");

            this.Value = individualName;
        }
        #endregion
    }
}