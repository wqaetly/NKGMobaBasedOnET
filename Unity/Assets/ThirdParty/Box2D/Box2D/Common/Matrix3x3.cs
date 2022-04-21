using System.Numerics;

namespace Box2DSharp.Common
{
    public struct Matrix3x3
    {
        public Vector3 Ex;

        public Vector3 Ey;

        public Vector3 Ez;

        /// Construct this matrix using columns.
        public Matrix3x3(in Vector3 c1, in Vector3 c2, in Vector3 c3)
        {
            Ex = c1;
            Ey = c2;
            Ez = c3;
        }

        /// Set this matrix to all zeros.
        public void SetZero()
        {
            Ex.SetZero();
            Ey.SetZero();
            Ez.SetZero();
        }

        /// Solve A * x = b, where b is a column vector. This is more efficient
        /// than computing the inverse in one-shot cases.
        public Vector3 Solve33(in Vector3 b)
        {
            var det = Vector3.Dot(Ex, Vector3.Cross(Ey, Ez));
            if (!det.Equals(0.0f))
            {
                det = 1.0f / det;
            }

            Vector3 x;
            x.X = det * Vector3.Dot(b, Vector3.Cross(Ey, Ez));
            x.Y = det * Vector3.Dot(Ex, Vector3.Cross(b, Ez));
            x.Z = det * Vector3.Dot(Ex, Vector3.Cross(Ey, b));
            return x;
        }

        /// Solve A * x = b, where b is a column vector. This is more efficient
        /// than computing the inverse in one-shot cases. Solve only the upper
        /// 2-by-2 matrix equation.
        public Vector2 Solve22(in Vector2 b)
        {
            var a11 = Ex.X;
            var a12 = Ey.X;
            var a21 = Ex.Y;
            var a22 = Ey.Y;

            var det = a11 * a22 - a12 * a21;
            if (!det.Equals(0.0f))
            {
                det = 1.0f / det;
            }

            Vector2 x;
            x.X = det * (a22 * b.X - a12 * b.Y);
            x.Y = det * (a11 * b.Y - a21 * b.X);
            return x;
        }

        /// Get the inverse of this matrix as a 2-by-2.
        /// Returns the zero matrix if singular.
        public void GetInverse22(ref Matrix3x3 matrix3x3)
        {
            float a = Ex.X, b = Ey.X, c = Ex.Y, d = Ey.Y;
            var det = a * d - b * c;
            if (!det.Equals(0.0f))
            {
                det = 1.0f / det;
            }

            matrix3x3.Ex.X = det * d;
            matrix3x3.Ey.X = -det * b;
            matrix3x3.Ex.Z = 0.0f;
            matrix3x3.Ex.Y = -det * c;
            matrix3x3.Ey.Y = det * a;
            matrix3x3.Ey.Z = 0.0f;
            matrix3x3.Ez.X = 0.0f;
            matrix3x3.Ez.Y = 0.0f;
            matrix3x3.Ez.Z = 0.0f;
        }

        /// Get the symmetric inverse of this matrix as a 3-by-3.
        /// Returns the zero matrix if singular.
        public void GetSymInverse33(ref Matrix3x3 matrix3x3)
        {
            var det = Vector3.Dot(Ex, Vector3.Cross(Ey, Ez));
            if (!det.Equals(0.0f))
            {
                det = 1.0f / det;
            }

            float a11 = Ex.X, a12 = Ey.X, a13 = Ez.X;
            float a22 = Ey.Y, a23 = Ez.Y;
            var a33 = Ez.Z;

            matrix3x3.Ex.X = det * (a22 * a33 - a23 * a23);
            matrix3x3.Ex.Y = det * (a13 * a23 - a12 * a33);
            matrix3x3.Ex.Z = det * (a12 * a23 - a13 * a22);

            matrix3x3.Ey.X = matrix3x3.Ex.Y;
            matrix3x3.Ey.Y = det * (a11 * a33 - a13 * a13);
            matrix3x3.Ey.Z = det * (a13 * a12 - a11 * a23);

            matrix3x3.Ez.X = matrix3x3.Ex.Z;
            matrix3x3.Ez.Y = matrix3x3.Ey.Z;
            matrix3x3.Ez.Z = det * (a11 * a22 - a12 * a12);
        }
    }
}