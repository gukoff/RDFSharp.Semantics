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
using System.Threading.Tasks;

namespace RDFSharp.Semantics
{
    /// <summary>
    /// OWLSemanticsEvents represents a collector for events generated within the "RDFSharp.Semantics" namespace
    /// </summary>
    public static class OWLSemanticsEvents
    {
        #region OnSemanticsInfo
        /// <summary>
        /// Event representing an informational message
        /// </summary>
        public static event RDFSemanticsInfoEventHandler OnSemanticsInfo = delegate { };

        /// <summary>
        /// Delegate to handle informational events
        /// </summary>
        public delegate void RDFSemanticsInfoEventHandler(string eventMessage);

        /// <summary>
        /// Internal invoker of the subscribed informational event handler
        /// </summary>
        internal static void RaiseSemanticsInfo(string eventMessage)
            => Parallel.Invoke(() => OnSemanticsInfo(string.Concat(DateTime.UtcNow.ToString("yyyy-MM-ddTHH:mm:ssZ"), ";INFO;", eventMessage)));
        #endregion

        #region OnSemanticsWarning
        /// <summary>
        /// Event representing a warning message
        /// </summary>
        public static event RDFSemanticsWarningEventHandler OnSemanticsWarning = delegate { };

        /// <summary>
        /// Delegate to handle warning events
        /// </summary>
        public delegate void RDFSemanticsWarningEventHandler(string eventMessage);

        /// <summary>
        /// Internal invoker of the subscribed warning event handler
        /// </summary>
        internal static void RaiseSemanticsWarning(string eventMessage)
            => Parallel.Invoke(() => OnSemanticsWarning(string.Concat(DateTime.UtcNow.ToString("yyyy-MM-ddTHH:mm:ssZ"), ";WARNING;", eventMessage)));
        #endregion
    }
}