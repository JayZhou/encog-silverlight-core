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
using Encog.Util;

namespace Encog.MathUtil.Matrices.Decomposition
{
    /// <summary>
    /// Cholesky Decomposition.
    /// 
    /// For a symmetric, positive definite matrix A, the Cholesky decomposition is an
    /// lower triangular matrix L so that A = L*L'.
    /// 
    /// If the matrix is not symmetric or positive definite, the constructor returns
    /// a partial decomposition and sets an internal flag that may be queried by the
    /// isSPD() method.
    /// 
    /// This file based on a class from the public domain JAMA package.
    /// http://math.nist.gov/javanumerics/jama/
    /// </summary>
    public class CholeskyDecomposition
    {
        /// <summary>
        /// Symmetric and positive definite flag.
        /// </summary>
        private readonly bool isspd;

        /// <summary>
        /// Array for internal storage of decomposition.
        /// </summary>
        private readonly double[][] l;

        /// <summary>
        /// Row and column dimension (square matrix).
        /// </summary>
        private readonly int n;

        /// <summary>
        /// Cholesky algorithm for symmetric and positive definite matrix.
        /// </summary>
        /// <param name="matrix">Square, symmetric matrix.</param>
        public CholeskyDecomposition(Matrix matrix)
        {
            // Initialize.
            double[][] a = matrix.Data;
            n = matrix.Rows;
            l = EngineArray.AllocateDouble2D(n,n);
            isspd = (matrix.Cols == n);
            // Main loop.
            for (int j = 0; j < n; j++)
            {
                double[] lrowj = l[j];
                double d = 0.0;
                for (int k = 0; k < j; k++)
                {
                    l[k] = new double[n];
                    double[] lrowk = l[k];
                    double s = 0.0;
                    for (int i = 0; i < k; i++)
                    {
                        s += lrowk[i]*lrowj[i];
                    }
                    s = (a[j][k] - s)/l[k][k];
                    lrowj[k] = s;
                    d = d + s*s;
                    isspd = isspd & (a[k][j] == a[j][k]);
                }
                d = a[j][j] - d;
                isspd = isspd & (d > 0.0);
                l[j][j] = Math.Sqrt(Math.Max(d, 0.0));
                for (int k = j + 1; k < n; k++)
                {
                    l[j][k] = 0.0;
                }
            }
        }

        /// <summary>
        /// Is the matrix symmetric and positive definite?
        /// </summary>
        public bool IsSPD
        {
            get { return isspd; }
        }

        /// <summary>
        /// Return triangular factor.
        /// </summary>
        public Matrix L
        {
            get { return new Matrix(l); }
        }

        /// <summary>
        /// Solve A*X = B.
        /// </summary>
        /// <param name="b">A Matrix with as many rows as A and any number of columns.</param>
        /// <returns>X so that L*L'*X = b.</returns>
        public Matrix Solve(Matrix b)
        {
            if (b.Rows != n)
            {
                throw new MatrixError(
                    "Matrix row dimensions must agree.");
            }
            if (!isspd)
            {
                throw new MatrixError(
                    "Matrix is not symmetric positive definite.");
            }

            // Copy right hand side.
            double[][] x = b.GetArrayCopy();
            int nx = b.Cols;

            // Solve L*Y = B;
            for (int k = 0; k < n; k++)
            {
                for (int j = 0; j < nx; j++)
                {
                    for (int i = 0; i < k; i++)
                    {
                        x[k][j] -= x[i][j]*l[k][i];
                    }
                    x[k][j] /= l[k][k];
                }
            }

            // Solve L'*X = Y;
            for (int k = n - 1; k >= 0; k--)
            {
                for (int j = 0; j < nx; j++)
                {
                    for (int i = k + 1; i < n; i++)
                    {
                        x[k][j] -= x[i][j]*l[i][k];
                    }
                    x[k][j] /= l[k][k];
                }
            }

            return new Matrix(x);
        }
    }
}
