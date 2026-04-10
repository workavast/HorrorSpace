namespace App.Health
{
    public interface IDamageable
    {
        public bool CanBeDamaged();
        public void TakeDamage(float damage);
    }
}