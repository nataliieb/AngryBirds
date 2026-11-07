using System;
using UnityEngine;

namespace KM1
{
    public class Projectile : MonoBehaviour
    {
        public float groundY = 0.25f;
        [Tooltip("Obracaj pocisk zgodnie z kierunkiem lotu")]
        public bool faceVelocity = true;

        public event Action<ShotStats> Landed;
        public bool IsFlying { get; private set; }

        Vector2 origin;
        Vector2 v0;
        float t;
        float tLand;
        ShotStats current;

        public void ResetTo(Vector2 start)
        {
            IsFlying = false;
            origin = start;
            transform.rotation = Quaternion.identity;
            SetPosition(start);
        }

        public void Launch(Vector2 start, float angleDeg, float speed)
        {
            origin = start;
            v0 = ProjectileMath.Velocity(angleDeg, speed);
            tLand = ProjectileMath.TimeOfFlight(origin, v0, groundY);
            t = 0f;
            IsFlying = true;

            current = new ShotStats
            {
                AngleDeg = angleDeg,
                Speed = speed,
                FlightTime = tLand,
                Range = v0.x * tLand,
                MaxHeight = ProjectileMath.MaxHeight(origin, v0) - groundY
            };

            SetPosition(origin);
            if (faceVelocity) FaceVelocity();
        }

        void Update()
        {
            if (!IsFlying) return;

            t += Time.deltaTime;
            if (t >= tLand)
            {
                t = tLand;
                IsFlying = false;
            }

            SetPosition(ProjectileMath.PositionAt(origin, v0, t));
            if (faceVelocity) FaceVelocity();

            if (!IsFlying)
                Landed?.Invoke(current);
        }
        void FaceVelocity()
        {
            Vector2 v = new Vector2(v0.x, v0.y - ProjectileMath.Gravity * t);
            float angle = Mathf.Atan2(v.y, v.x) * Mathf.Rad2Deg;
            transform.rotation = Quaternion.Euler(0f, 0f, angle);
        }

        void SetPosition(Vector2 p)
        {
            transform.position = new Vector3(p.x, p.y, transform.position.z);
        }
    }
}