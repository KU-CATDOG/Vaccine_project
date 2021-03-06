using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CoughBacteria : VirusClass
{

    protected override void Start()
    {
        base.Start();
        MaxHealth = Health = 30;
        OriAttackSpeed = 1.0f;
        Damage = 1.0f;
        Phrase = "빠르게 기침하여 세포들을 감염시킨다.";
        oriSpeed = 0.15f;
        speed = oriSpeed;
    }

    public override void GetDamaged(float damage)
    {

        if (!Invincible) base.GetDamaged(damage);

    }
    protected override Queue<IEnumerator> DecideNextRoutine()
    {
        Queue<IEnumerator> nextRoutines = new Queue<IEnumerator>();

        if (!collided) nextRoutines.Enqueue(NewActionRoutine(Move()));
        else
        {
            nextRoutines.Enqueue(NewActionRoutine(Cough()));
        }

        return nextRoutines;
    }

    private IEnumerator Move()
    {
        yield return MoveRoutine(GetObjectPos() + new Vector3(-speed, 0, 0), 0.1f);

    }

    private IEnumerator Cough()
    {
        gameObject.GetComponent<SpriteRenderer>().color = Color.red;
        target.GetComponent<Unit>().GetDamaged(Damage);
        yield return new WaitForSeconds(OriAttackSpeed / 2);

        gameObject.GetComponent<SpriteRenderer>().color = Color.white;
        yield return new WaitForSeconds(OriAttackSpeed / 2);
    }
}
