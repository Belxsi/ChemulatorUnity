

using System;
using System.Globalization;
using System.Numerics;
using System.Runtime.CompilerServices;
using System.Text;
using System.Windows;


namespace System.Numerics
{
    [Serializable]
    public struct Vector2Int : IEquatable<Vector2Int>, IFormattable
    {

        public int X;


        public int Y;


        public static Vector2Int Zero
        {

            get
            {
                return default(Vector2Int);
            }
        }


        public static Vector2Int One
        {

            get
            {
                return new Vector2Int(1, 1);
            }
        }


        public static Vector2Int UnitX
        {

            get
            {
                return new Vector2Int(1, 0);
            }
        }


        public static Vector2Int UnitY
        {

            get
            {
                return new Vector2Int(0, 1);
            }
        }


        public override int GetHashCode()
        {
            int hashCode = X.GetHashCode();
            return HashCode.Combine(hashCode, Y.GetHashCode());
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]

        public override bool Equals(object obj)
        {
            if (!(obj is Vector2Int))
            {
                return false;
            }

            return Equals((Vector2Int)obj);
        }


        public override string ToString()
        {
            return ToString("G", CultureInfo.CurrentCulture);
        }


        public string ToString(string format)
        {
            return ToString(format, CultureInfo.CurrentCulture);
        }


        public string ToString(string format, IFormatProvider formatProvider)
        {
            StringBuilder stringBuilder = new StringBuilder();
            string numberGroupSeparator = NumberFormatInfo.GetInstance(formatProvider).NumberGroupSeparator;
            stringBuilder.Append('<');
            stringBuilder.Append(X.ToString(format, formatProvider));
            stringBuilder.Append(numberGroupSeparator);
            stringBuilder.Append(' ');
            stringBuilder.Append(Y.ToString(format, formatProvider));
            stringBuilder.Append('>');
            return stringBuilder.ToString();
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]

        public float Length()
        {
            

            float num2 = X * X + Y * Y;
            return (float)Math.Sqrt(num2);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]

        public float LengthSquared()
        {
            

            return X * X + Y * Y;
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]

        public static float Distance(Vector2Int value1, Vector2Int value2)
        {
           

            float num2 = value1.X - value2.X;
            float num3 = value1.Y - value2.Y;
            float num4 = num2 * num2 + num3 * num3;
            return (float)Math.Sqrt(num4);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]

        public static float DistanceSquared(Vector2Int value1, Vector2Int value2)
        {
            

            float num = value1.X - value2.X;
            float num2 = value1.Y - value2.Y;
            return num * num + num2 * num2;
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]

        public static Vector2Int Normalize(Vector2Int value)
        {
            

            float num2 = value.X * value.X + value.Y * value.Y;
            float num3 = 1f / (float)Math.Sqrt(num2);
            return new Vector2Int((int)(value.X * num3), (int)(value.Y * num3));
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]

        public static Vector2Int Reflect(Vector2Int vector, Vector2Int normal)
        {
           

            float num2 = vector.X * normal.X + vector.Y * normal.Y;
            return new Vector2Int((int)(vector.X - 2f * num2 * normal.X), (int)(vector.Y - 2f * num2 * normal.Y));
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]

        public static Vector2Int Clamp(Vector2Int value1, Vector2Int min, Vector2Int max)
        {
            float x = value1.X;
            x = ((x > max.X) ? max.X : x);
            x = ((x < min.X) ? min.X : x);
            float y = value1.Y;
            y = ((y > max.Y) ? max.Y : y);
            y = ((y < min.Y) ? min.Y : y);
            return new Vector2Int((int)(x), (int)(y));
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]

        public static Vector2Int Lerp(Vector2Int value1, Vector2Int value2, float amount)
        {
            return new Vector2Int((int)(value1.X + (value2.X - value1.X) * amount), (int)(value1.Y + (value2.Y - value1.Y) * amount));
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]

        public static Vector2Int Transform(Vector2Int position, Matrix3x2 matrix)
        {
            return new Vector2Int((int)(position.X * matrix.M11 + position.Y * matrix.M21 + matrix.M31), (int)(position.X * matrix.M12 + position.Y * matrix.M22 + matrix.M32));
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]

        public static Vector2Int Transform(Vector2Int position, Matrix4x4 matrix)
        {
            return new Vector2Int((int)(position.X * matrix.M11 + position.Y * matrix.M21 + matrix.M41), (int)(position.X * matrix.M12 + position.Y * matrix.M22 + matrix.M42));
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]

        public static Vector2Int TransformNormal(Vector2Int normal, Matrix3x2 matrix)
        {
            return new Vector2Int((int)(normal.X * matrix.M11 + normal.Y * matrix.M21), (int)(normal.X * matrix.M12 + normal.Y * matrix.M22));
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]

        public static Vector2Int TransformNormal(Vector2Int normal, Matrix4x4 matrix)
        {
            return new Vector2Int((int)(normal.X * matrix.M11 + normal.Y * matrix.M21), (int)(normal.X * matrix.M12 + normal.Y * matrix.M22));
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]

        public static Vector2Int Transform(Vector2Int value, Quaternion rotation)
        {
            float num = rotation.X + rotation.X;
            float num2 = rotation.Y + rotation.Y;
            float num3 = rotation.Z + rotation.Z;
            float num4 = rotation.W * num3;
            float num5 = rotation.X * num;
            float num6 = rotation.X * num2;
            float num7 = rotation.Y * num2;
            float num8 = rotation.Z * num3;
            return new Vector2Int((int)(value.X * (1f - num7 - num8) + value.Y * (num6 - num4)), (int)(value.X * (num6 + num4) + value.Y * (1f - num5 - num8)));
        }
       

        [MethodImpl(MethodImplOptions.AggressiveInlining)]

        public static Vector2Int Add(Vector2Int left, Vector2Int right)
        {
            return left + right;
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]

        public static Vector2Int Subtract(Vector2Int left, Vector2Int right)
        {
            return left - right;
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]

        public static Vector2Int Multiply(Vector2Int left, Vector2Int right)
        {
            return left * right;
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]

        public static Vector2Int Multiply(Vector2Int left, float right)
        {
            return left * right;
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]

        public static Vector2Int Multiply(float left, Vector2Int right)
        {
            return left * right;
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]

        public static Vector2Int Divide(Vector2Int left, Vector2Int right)
        {
            return left / right;
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]

        public static Vector2Int Divide(Vector2Int left, float divisor)
        {
            return left / divisor;
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]

        public static Vector2Int Negate(Vector2Int value)
        {
            return -value;
        }



        public Vector2Int(int value)
            : this(value, value)
        {
        }



        public Vector2Int(int x, int y)
        {
            X = x;
            Y = y;
        }
        
        [MethodImpl(MethodImplOptions.AggressiveInlining)]








        public bool Equals(Vector2Int other)
        {
            if (X == other.X)
            {
                return Y == other.Y;
            }

            return false;
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]


        public static float Dot(Vector2Int value1, Vector2Int value2)
        {
            return value1.X * value2.X + value1.Y * value2.Y;
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]


        public static Vector2Int Min(Vector2Int value1, Vector2Int value2)
        {
            return new Vector2Int((value1.X < value2.X) ? value1.X : value2.X, (value1.Y < value2.Y) ? value1.Y : value2.Y);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]


        public static Vector2Int Max(Vector2Int value1, Vector2Int value2)
        {
            return new Vector2Int((value1.X > value2.X) ? value1.X : value2.X, (value1.Y > value2.Y) ? value1.Y : value2.Y);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]


        public static Vector2Int Abs(Vector2Int value)
        {
            return new Vector2Int(Math.Abs(value.X), Math.Abs(value.Y));
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]


        public static Vector2Int SquareRoot(Vector2Int value)
        {
            return new Vector2Int((int)Math.Sqrt(value.X), (int)Math.Sqrt(value.Y));
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]


        public static Vector2Int operator +(Vector2Int left, Vector2Int right)
        {
            return new Vector2Int(left.X + right.X, left.Y + right.Y);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]


        public static Vector2Int operator -(Vector2Int left, Vector2Int right)
        {
            return new Vector2Int(left.X - right.X, left.Y - right.Y);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]


        public static Vector2Int operator *(Vector2Int left, Vector2Int right)
        {
            return new Vector2Int(left.X * right.X, left.Y * right.Y);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]


        public static Vector2Int operator *(float left, Vector2Int right)
        {
            return new Vector2Int((int)(left), (int)(left)) * right;
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]


        public static Vector2Int operator *(Vector2Int left, float right)
        {
            return left * new Vector2Int((int)(right), (int)(right));
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]


        public static Vector2Int operator /(Vector2Int left, Vector2Int right)
        {
            return new Vector2Int(left.X / right.X, left.Y / right.Y);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]


        public static Vector2Int operator /(Vector2Int value1, float value2)
        {
            float num = 1f / value2;
            return new Vector2Int((int)(value1.X * num), (int)(value1.Y * num));
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]

        public static Vector2Int operator -(Vector2Int value)
        {
            return Zero - value;
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]

        public static bool operator ==(Vector2Int left, Vector2Int right)
        {
            return left.Equals(right);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]

        public static bool operator !=(Vector2Int left, Vector2Int right)
        {
            return !(left == right);
        }

        public static implicit operator Vector2Int(Vector2 v)
        {
            return new Vector2Int((int)v.X,(int)v.Y);
        }
    }
#if false // Журнал декомпиляции
    Элементов в кэше: "13"
    ------------------
    Разрешить: "mscorlib, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089"
    Найдена одна сборка: "mscorlib, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089"
    Загрузить из: "C:\Program Files (x86)\Reference Assemblies\Microsoft\Framework\.NETFramework\v4.8\mscorlib.dll"
#endif

}
