namespace KM1
{
    public struct ShotStats
    {
        public float AngleDeg;
        public float Speed;
        public float FlightTime; // s
        public float Range;      // m
        public float MaxHeight;  // m nad podłożem

        public override string ToString()
        {
            return $"kąt {AngleDeg:0}°, v0 {Speed:0.0} m/s, t {FlightTime:0.00} s, " +
                   $"zasięg {Range:0.00} m, h_max {MaxHeight:0.00} m";
        }
    }
}
