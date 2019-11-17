namespace Thijs.Platformer
{
    public interface IDamageReceiver
    {
        void GetDamaged(DamageType type, float amount);
    }
}