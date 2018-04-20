namespace Portfolio
{
    public class burningEffect : TickEffect
    {

        protected override void Action()
        {
            target.ApplyDamage((int)statsValue);
        }
    }
}