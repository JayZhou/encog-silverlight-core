//
// Encog(tm) Core v3.0 - .Net Version
// http://www.heatonresearch.com/encog/
//
// Copyright 2008-2011 Heaton Research, Inc.
//
// Licensed under the Apache License, Version 2.0 (the "License");
// you may not use this file except in compliance with the License.
// You may obtain a copy of the License at
//
//  http://www.apache.org/licenses/LICENSE-2.0
//
// Unless required by applicable law or agreed to in writing, software
// distributed under the License is distributed on an "AS IS" BASIS,
// WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, either express or implied.
// See the License for the specific language governing permissions and
// limitations under the License.
//   
// For more information on Heaton Research copyrights, licenses 
// and trademarks visit:
// http://www.heatonresearch.com/copyright
//

using System;

namespace Encog.Util.Normalize.Target
{
    /// <summary>
    /// Output the normalized data to a 1D array.
    /// </summary>
#if !SILVERLIGHT
    [Serializable]
#endif
    public class NormalizationStorageArray1D : INormalizationStorage
    {
        /// <summary>
        /// The array to store to.
        /// </summary>
        private readonly double[] _array;

        /// <summary>
        /// The current index.
        /// </summary>
        private int _currentIndex;


        /// <summary>
        /// Construct an object to store to a 2D array.
        /// </summary>
        /// <param name="array">The array to store to.</param>
        public NormalizationStorageArray1D(double[] array)
        {
            _array = array;
            _currentIndex = 0;
        }

        #region INormalizationStorage Members

        /// <summary>
        /// Not needed for this storage type.
        /// </summary>
        public void Close()
        {
        }

        /// <summary>
        /// Not needed for this storage type.
        /// </summary>
        public void Open()
        {
        }

        /// <summary>
        /// Write an array.
        /// </summary>
        /// <param name="data">The data to write.</param>
        /// <param name="inputCount">How much of the data is input.</param>
        public void Write(double[] data, int inputCount)
        {
            _array[_currentIndex++] = data[0];
        }

        #endregion

        public double[] GetArray()
        {
            return _array;
        }
    }
}