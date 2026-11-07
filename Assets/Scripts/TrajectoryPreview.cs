using System.Collections.Generic;
using UnityEngine;

namespace KM1
{
    public class TrajectoryPreview : MonoBehaviour
    {
        [Tooltip("Odległość między kropkami wzdłuż toru [m]")]
        public float spacing = 0.6f;
        [Tooltip("Maksymalna liczba kropek (0 = cały tor)")]
        public int maxDots = 0;
        public float startSize = 0.22f;
        public float endSize = 0.08f;
        public Color color = Color.white;
        [Range(0f, 1f)] public float endAlpha = 0.35f;
        public int sortingOrder = 3;

        const int Resolution = 400; 
        readonly List<SpriteRenderer> dots = new List<SpriteRenderer>();

        public void Draw(Vector2 origin, float angleDeg, float speed, float groundY)
        {
            Vector2 v0 = ProjectileMath.Velocity(angleDeg, speed);
            float duration = ProjectileMath.TimeOfFlight(origin, v0, groundY);

            var points = new List<Vector2>();
            Vector2 prev = origin;
            float sinceLast = 0f;
            for (int i = 1; i <= Resolution && !Full(points.Count); i++)
            {
                Vector2 next = ProjectileMath.PositionAt(origin, v0, duration * i / Resolution);
                float seg = Vector2.Distance(prev, next);
                while (seg > 0f && sinceLast + seg >= spacing && !Full(points.Count))
                {
                    float step = spacing - sinceLast;
                    prev = Vector2.MoveTowards(prev, next, step);
                    seg -= step;
                    sinceLast = 0f;
                    points.Add(prev);
                }
                sinceLast += seg;
                prev = next;
            }

            while (dots.Count < points.Count)
                dots.Add(SpriteFactory.CreateDot(transform, "AimDot", sortingOrder));

            for (int i = 0; i < dots.Count; i++)
            {
                bool on = i < points.Count;
                dots[i].gameObject.SetActive(on);
                if (!on) continue;

                float f = points.Count > 1 ? (float)i / (points.Count - 1) : 0f;
                float size = Mathf.Lerp(startSize, endSize, f);
                Color c = color;
                c.a = Mathf.Lerp(color.a, endAlpha, f);

                dots[i].transform.position = new Vector3(points[i].x, points[i].y, 0f);
                dots[i].transform.localScale = new Vector3(size, size, 1f);
                dots[i].color = c;
            }
        }

        public void Hide()
        {
            foreach (var d in dots) d.gameObject.SetActive(false);
        }

        bool Full(int count) => maxDots > 0 && count >= maxDots;
    }
}