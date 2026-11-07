using System.Collections.Generic;
using UnityEngine;

namespace KM1
{
    [RequireComponent(typeof(Projectile))]
    public class FlightTrail : MonoBehaviour
    {
        public float spacing = 0.5f;      
        public float bigSize = 0.2f;
        public float smallSize = 0.11f;
        public Color color = new Color(1f, 1f, 1f, 0.9f);
        public int sortingOrder = 1;

        Projectile projectile;
        Transform container;
        readonly List<SpriteRenderer> dots = new List<SpriteRenderer>();
        int used;
        Vector2 lastDrop;
        bool wasFlying;

        void Awake()
        {
            projectile = GetComponent<Projectile>();
            container = new GameObject("FlightTrailDots").transform;
        }

        void LateUpdate()
        {
            bool flying = projectile.IsFlying;
            Vector2 pos = transform.position;

            if (flying && !wasFlying) 
            {
                Clear();
                lastDrop = pos;
            }

            if (flying && Vector2.Distance(pos, lastDrop) >= spacing)
            {
                Drop(pos);
                lastDrop = pos;
            }

            wasFlying = flying;
        }

        void Drop(Vector2 pos)
        {
            if (used == dots.Count)
                dots.Add(SpriteFactory.CreateDot(container, "TrailDot", sortingOrder));

            SpriteRenderer dot = dots[used];
            float size = used % 2 == 0 ? bigSize : smallSize;
            dot.transform.position = new Vector3(pos.x, pos.y, 0f);
            dot.transform.localScale = new Vector3(size, size, 1f);
            dot.color = color;
            dot.gameObject.SetActive(true);
            used++;
        }

        public void Clear()
        {
            foreach (var d in dots) d.gameObject.SetActive(false);
            used = 0;
        }
    }
}