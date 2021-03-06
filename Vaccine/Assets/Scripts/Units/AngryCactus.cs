using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class AngryCactus : Unit
{
    [SerializeField]
    private float attackInterval = 0.5f;
    private List<GameObject> target = new List<GameObject>();

    protected override void Start()
    {
        base.Start();
        MaxHealth = Health = 0;
        OriAttackSpeed = attackInterval;
        attackSpeedInterval = OriAttackSpeed;
        Damage = 1.0f;
        Phrase = "머리를 밟고 지나가는 세균들에게 데미지를 준다.";
    }

    public override void GetDamaged(float damage)
    {

        if (!Invincible) base.GetDamaged(damage);

    }

    protected override Queue<IEnumerator> DecideNextRoutine()
    {
        Queue<IEnumerator> nextRoutines = new Queue<IEnumerator>();

        nextRoutines.Enqueue(NewActionRoutine(Attack(attackInterval)));

        return nextRoutines;
    }

    private IEnumerator Attack(float interval)
    {
        GameObject t;
        for(int i = target.Count - 1; i >= 0 ; i--)
        {
            t = target[i];
            t.GetComponent<VirusClass>().GetDamaged(Damage);
        }

        yield return new WaitForSeconds(interval);
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.GetComponent<VirusClass>() != null)
        {
            target.Add(collision.gameObject);
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.GetComponent<VirusClass>() != null)
        {
            target.Remove(collision.gameObject);
        }
    }
}
