using UnityEngine;

namespace Utility
{
    namespace Vector
    {
        /// <summary>
        /// Serializable 2D vector with basic operations such as adding two 2D vectors together.
        /// </summary>
        [System.Serializable]
        public struct SerializableVector2
        {
#pragma warning disable IDE1006 // Naming Styles
            public float x;
            public float y;
#pragma warning restore IDE1006 // Naming Styles

            public SerializableVector2(float x, float y)
            {
                this.x = x;
                this.y = y;
            }

            public static SerializableVector2 operator +(SerializableVector2 lhs, SerializableVector2 rhs)
            {
                return new SerializableVector2(lhs.x + rhs.x, lhs.y + rhs.y);
            }

            public static Vector2 operator +(SerializableVector2 lhs, Vector2 rhs)
            {
                return new Vector2(lhs.x + rhs.x, lhs.y + rhs.y);
            }

            public static SerializableVector2 operator -(SerializableVector2 lhs, SerializableVector2 rhs)
            {
                return new SerializableVector2(lhs.x - rhs.x, lhs.y - rhs.y);
            }

            public static Vector2 operator -(SerializableVector2 lhs, Vector2 rhs)
            {
                return new Vector2(lhs.x - rhs.x, lhs.y - rhs.y);
            }

            public static SerializableVector2 operator *(SerializableVector2 lhs, float rhs)
            {
                return new SerializableVector2(lhs.x * rhs, lhs.y * rhs);
            }

            public static SerializableVector2 operator *(float lhs, SerializableVector2 rhs)
            {
                return new SerializableVector2(rhs.x * lhs, rhs.y * lhs);
            }

            public static SerializableVector2 operator /(SerializableVector2 lhs, float rhs)
            {
                return new SerializableVector2(lhs.x / rhs, lhs.y / rhs);
            }

            public static implicit operator Vector2(SerializableVector2 serializableVector)
            {
                return new Vector2(serializableVector.x, serializableVector.y);
            }

            public static implicit operator Vector3(SerializableVector2 serializableVector)
            {
                return new Vector3(serializableVector.x, serializableVector.y);
            }

            public static explicit operator SerializableVector2(Vector2 vector)
            {
                return new SerializableVector2(vector.x, vector.y);
            }


            public static explicit operator SerializableVector2(Vector3 vector)
            {
                return new SerializableVector2(vector.x, vector.y);
            }

            public override bool Equals(object obj)
            {
                if ((obj == null) || !GetType().Equals(obj.GetType()))
                {
                    return false;
                }

                SerializableVector2 serializableVector = (SerializableVector2)obj;
                return x.Equals(serializableVector.x) && y.Equals(serializableVector.y);
            }

            public override int GetHashCode()
            {
                return x.GetHashCode() ^ y.GetHashCode() << 2; ;
            }

            public static bool operator ==(SerializableVector2 left, SerializableVector2 right)
            {
                return left.Equals(right);
            }

            public static bool operator !=(SerializableVector2 left, SerializableVector2 right)
            {
                return !(left == right);
            }
        }
    }
}
