using System.Collections.Generic;
using System.Text;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace KM1
{
    public class Launcher : MonoBehaviour
    {
        [Header("Scena")]
        public Projectile projectile;
        public TrajectoryPreview trajectory;
        public Transform launchPoint;
        public float groundLevel = 0f;
        public float projectileRadius = 0.25f;


        [Header("UI")]
        public Slider angleSlider;
        public Slider speedSlider;
        public TMP_Text angleLabel;
        public TMP_Text speedLabel;
        public TMP_Text statsLabel;
        public Button fireButton;
        public Button resetButton;

        const int ShownHistory = 5;
        readonly List<ShotStats> history = new List<ShotStats>();

        float Angle => angleSlider.value;
        float Speed => speedSlider.value;
        Vector2 Origin => launchPoint.position;
        bool landed;
        FlightTrail trail;

        void Start()
        {
            trail = projectile.GetComponent<FlightTrail>();
            projectile.groundY = groundLevel + projectileRadius;
            projectile.Landed += OnLanded;
            projectile.ResetTo(Origin);

            angleSlider.onValueChanged.AddListener(_ => OnAimChanged());
            speedSlider.onValueChanged.AddListener(_ => OnAimChanged());
            fireButton.onClick.AddListener(Fire);
            resetButton.onClick.AddListener(ResetShot);

            OnAimChanged();
            RefreshStats();
        }

       
        void OnDestroy()
        {
            if (projectile != null)
                projectile.Landed -= OnLanded;
        }

        void Update()
        {
            bool idle = !projectile.IsFlying;
            bool canAim = idle && !landed;
            fireButton.interactable = canAim;
            angleSlider.interactable = canAim;
            speedSlider.interactable = canAim;
            resetButton.interactable = idle;
        }

        void OnAimChanged()
        {
            angleLabel.text = $"Kąt: {Angle:0}°";
            speedLabel.text = $"Prędkość: {Speed:0.0} m/s";
            trajectory.Draw(Origin, Angle, Speed, projectile.groundY);
        }

        void Fire()
        {
            if (projectile.IsFlying || landed) return;
            trajectory.Hide();
            projectile.Launch(Origin, Angle, Speed);
        }

        void ResetShot()
        {
            if (projectile.IsFlying) return;
            landed = false;
            projectile.ResetTo(Origin);
            if (trail != null) trail.Clear();
            OnAimChanged();
        }

        void OnLanded(ShotStats stats)
        {
            landed = true;
            history.Add(stats);
            RefreshStats();
        }

        void RefreshStats()
        {
            if (history.Count == 0)
            {
                statsLabel.text = "Brak strzałów.\nUstaw kąt i prędkość, potem kliknij Strzał.";
                return;
            }

            var sb = new StringBuilder();
            ShotStats last = history[history.Count - 1];
            sb.AppendLine($"Ostatni strzał (#{history.Count}):");
            sb.AppendLine($"  Kąt: {last.AngleDeg:0}°   Prędkość: {last.Speed:0.0} m/s");
            sb.AppendLine($"  Czas lotu: {last.FlightTime:0.00} s");
            sb.AppendLine($"  Zasięg: {last.Range:0.00} m");
            sb.AppendLine($"  Maks. wysokość: {last.MaxHeight:0.00} m");
            sb.AppendLine();
            sb.AppendLine("Historia (zasięg / wys. maks. / czas):");
            for (int i = history.Count - 1; i >= 0 && i >= history.Count - ShownHistory; i--)
            {
                ShotStats s = history[i];
                sb.AppendLine($"  #{i + 1}: {s.Range:0.00} m / {s.MaxHeight:0.00} m / {s.FlightTime:0.00} s");
            }

            statsLabel.text = sb.ToString();
        }
    }
}