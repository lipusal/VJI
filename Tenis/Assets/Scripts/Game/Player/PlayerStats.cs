namespace Game.Player
{
    public class PlayerStats
    {
        private static PlayerStats _instance;
        public const int DefaultHitForce = 45; 
        public const int DefaultServeForce = 45; 
        public const int DefaultSpeed = 45;
        public const int MaxStatSum = 201;

        public static PlayerStats GetInstance()
        {
            if (_instance == null)
            {
                _instance = new PlayerStats();
            }

            return _instance;
        }

        private PlayerStats()
        {
            HitForce = DefaultHitForce;
            ServeForce = DefaultServeForce;
            Speed = DefaultSpeed;
        }

        /// <summary>
        /// Set all values to 0.
        /// </summary>
        public void Zero()
        {
            HitForce = 0;
            ServeForce = 0;
            Speed = 0;
        }

        /// <summary>
        /// Set all values to defaults.
        /// </summary>
        public void Reset()
        {
            HitForce = DefaultHitForce;
            ServeForce = DefaultServeForce;
            Speed = DefaultSpeed;
        }

        /// <summary>
        /// Get the sum of all stats.
        /// </summary>
        /// <returns>Sum of all stats.</returns>
        public float GetTotal()
        {
            return HitForce + ServeForce + Speed;
        }

        public float HitForce { get; set; }

        public float ServeForce { get; set; }

        public float Speed { get; set; }
    }
}
