namespace Game.Player
{
    public class PlayerStats
    {
        private float _hitForce;
        private float _serveForce;
        private float _speed;

        public PlayerStats(float hitForce, float serveForce, float speed)
        {
            _hitForce = hitForce;
            _serveForce = serveForce;
            _speed = speed;
        }

        public float GetHitForce()
        {
            return _hitForce;
        }

        public float GetServeForce()
        {
            return _serveForce;
        }

        public float GetSpeed()
        {
            return _speed;
        }
    }
}