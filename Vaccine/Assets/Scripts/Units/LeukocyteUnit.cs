using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LeukocyteUnit : Unit
{
    [SerializeField]
    private GameObject bullet;
    [SerializeField]
    private float range = 9.5f;
    private int debuffCount = 0;
    private Vector3 shootPos;
    private float bulletSpeedTravel = 8f;

    protected override void Start()
    {
        base.Start();
        MaxHealth = Health = 9;
        OriAttackSpeed = 1.0f;
        Damage = 3.0f;
        attackSpeedInterval = OriAttackSpeed;
        Phrase = "다가오는 세균을 향해 공격한다. 가장 기본적인 세포이다.";
        shootPos = gameObject.transform.position;
    }

    public override void GetDamaged(float damage)
    {

        if (!Invincible) base.GetDamaged(damage);

    }

    protected override Queue<IEnumerator> DecideNextRoutine()
    {
        Queue<IEnumerator> nextRoutines = new Queue<IEnumerator>();

        nextRoutines.Enqueue(NewActionRoutine(Fire(range, attackSpeedInterval)));

        //if (debuff && debuffCount < 2)
        //{
        //    debuffCount++;
        //    nextRoutines.Enqueue(NewActionRoutine(Fire(range, attackSpeedInterval + 1)));
        //}
        //else
        //{
        //    debuffCount = 0;
        //    debuff = false;
            

        //}

        return nextRoutines;
    }

    private IEnumerator Fire(float range, float interval)
    {
        GameObject[] objects = GameObject.FindGameObjectsWithTag("Virus");
        GameObject temp = null;
        bool find = false;

        if (objects.Length != 0)
        {
            float min = range;
            foreach (GameObject element in objects)
            {
                float dist = Vector3.Distance(element.transform.position, GetObjectPos());
                float yDist = Mathf.Abs(element.transform.position.y - GetObjectPos().y);
                if (dist < min && yDist <= 1)
                {
                    temp = element;
                    min = dist;
                    find = true;
                }
            }
            if (find)
            {
                GameObject cur = Instantiate(bullet, shootPos, Quaternion.identity);
                cur.GetComponent<Rigidbody2D>().velocity = (temp.transform.position - GetObjectPos()).normalized * bulletSpeedTravel;
            }


        }

        yield return new WaitForSeconds(interval);
    }

}