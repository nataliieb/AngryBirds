using UnityEngine;

namespace KM1
{
    public static class SpriteFactory
    {
        static Sprite circle;

        public static Sprite Circle
        {
            get
            {
                if (circle == null) circle = MakeCircle(64);
                return circle;
            }
        }

        public static SpriteRenderer CreateDot(Transform parent, string name, int sortingOrder)
        {
            var go = new GameObject(name);
            go.transform.SetParent(parent, false);
            var sr = go.AddComponent<SpriteRenderer>();
            sr.sprite = Circle;
            sr.sortingOrder = sortingOrder;
            return sr;
        }

        static Sprite MakeCircle(int size)
        {
            var tex = new Texture2D(size, size, TextureFormat.RGBA32, false);
            tex.wrapMode = TextureWrapMode.Clamp;
            float r = size * 0.5f;
            var pixels = new Color32[size * size];
            for (int y = 0; y < size; y++)
            {
                for (int x = 0; x < size; x++)
                {
                    float dx = x + 0.5f - r;
                    float dy = y + 0.5f - r;
                    float d = Mathf.Sqrt(dx * dx + dy * dy);
                    float a = Mathf.Clamp01(r - d); 
                    pixels[y * size + x] = new Color32(255, 255, 255, (byte)(a * 255));
                }
            }
            tex.SetPixels32(pixels);
            tex.Apply();
            return Sprite.Create(tex, new Rect(0, 0, size, size), new Vector2(0.5f, 0.5f), size);
        }
    }
}