/*
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
using System.Runtime.Serialization;

namespace RDFSharp.Semantics
{
    /// <summary>
    /// OWLSemanticsException represents an exception thrown within "RDFSharp.Semantics" namespace
    /// </summary>
    [Serializable]
    public class OWLSemanticsException : Exception
    {
        #region Ctors
        /// <summary>
        /// Basic ctor to throw an empty OWLSemanticsException
        /// </summary>
        public OWLSemanticsException() { }

        /// <summary>
        /// Basic ctor to throw an OWLSemanticsException with message
        /// </summary>
        public OWLSemanticsException(string message) : base(message) { }

        /// <summary>
        /// Basic ctor to throw an OWLSemanticsException with message and inner exception
        /// </summary>
        public OWLSemanticsException(string message, Exception innerException) : base(message, innerException) { }

        /// <summary>
        /// Basic ctor to support serialization of a remotely thrown OWLSemanticsException
        /// </summary>
        protected OWLSemanticsException(SerializationInfo info, StreamingContext context) : base(info, context) { }
        #endregion
    }
}