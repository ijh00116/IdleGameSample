public interface ShootForAnimationEvent
{
    void StartAttack();
    void ShootProjectile();
    void ShootEnd();
    void EndAttack();


}

public interface HitforAnimationEvent
{
    void HitByattack();

    void HitEnd();
   

}
