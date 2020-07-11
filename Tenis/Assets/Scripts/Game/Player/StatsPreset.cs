namespace Game.Player
{
    public class StatsPreset
    {
        public readonly string name;
        public readonly int serveForce;
        public readonly int hitForce;
        public readonly int speed;

        public StatsPreset(string name, int serveForce, int hitForce, int speed)
        {
            this.name = name;
            this.serveForce = serveForce;
            this.hitForce = hitForce;
            this.speed = speed;
        }

        public static StatsPreset[] defaultPresets = new[]
        {
            new StatsPreset("Default", PlayerStats.DefaultServeForce, PlayerStats.DefaultHitForce, PlayerStats.DefaultSpeed),
            new StatsPreset("Mr. Perfect", 67, 67, 67),
            new StatsPreset("Usain Bolt", 50, 51, 100),
            new StatsPreset("Johnny Cannon", 100, 61, 40),
            new StatsPreset("Grandpa", 30, 30, 1),
            // new StatsPreset("Haven't played in years", 30, 30, 1),
            // new StatsPreset("Grandpa", 30, 30, 1),
        };
    }
}