namespace App.Health.Heal
{
    public class HealContext
    {
        public readonly HealthModel HealthModel;

        public HealContext(HealthModel healthModel)
        {
            HealthModel = healthModel;
        }
    }
}