using UnityEngine;

namespace KM1
{
    public static class ProjectileMath
    {
        public const float Gravity = 9.81f;

        public static Vector2 Velocity(float angleDeg, float speed)
        {
            float rad = angleDeg * Mathf.Deg2Rad;
            return new Vector2(Mathf.Cos(rad), Mathf.Sin(rad)) * speed;
        }
        public static Vector2 PositionAt(Vector2 origin, Vector2 v0, float t, float g = Gravity)
        {
            return new Vector2(
                origin.x + v0.x * t,
                origin.y + v0.y * t - 0.5f * g * t * t);
        }
        public static float TimeOfFlight(Vector2 origin, Vector2 v0, float groundY, float g = Gravity)
        {
            float h = origin.y - groundY;
            return (v0.y + Mathf.Sqrt(v0.y * v0.y + 2f * g * h)) / g;
        }
        public static float MaxHeight(Vector2 origin, Vector2 v0, float g = Gravity)
        {
            if (v0.y <= 0f) return origin.y;
            return origin.y + v0.y * v0.y / (2f * g);
        }
    }
}