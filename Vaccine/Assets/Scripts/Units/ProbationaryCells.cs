using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ProbationaryCells : Unit
{
    [SerializeField]
    private GameObject bullet;
    [SerializeField]
    private float range = 7.125f;
    private int debuffCount = 0;
    private Vector3 shootPos;
    private int fireCount = 0;
    private float bulletSpeedTravel = 12f;

    protected override void Start()
    {
        base.Start();
        MaxHealth = Health = 5;
        OriAttackSpeed = 0.25f;
        attackSpeedInterval = OriAttackSpeed;
        Damage = 3.0f;
        Phrase = "빠르게 다가오는 세균을 향해 공격하지만, 일정 시간 공격 후 휴식 시간을 가진다.";
        shootPos = gameObject.transform.position;
    }

    public override void GetDamaged(float damage)
    {

        if (!Invincible) base.GetDamaged(damage);

    }

    protected override Queue<IEnumerator> DecideNextRoutine()
    {
        Queue<IEnumerator> nextRoutines = new Queue<IEnumerator>();

        if (debuff && debuffCount < 2)
        {
            debuffCount++;
            nextRoutines.Enqueue(NewActionRoutine(Fire(range, 2 * attackSpeedInterval)));
        }
        else
        {
            debuffCount = 0;
            debuff = false;
            nextRoutines.Enqueue(NewActionRoutine(Fire(range, attackSpeedInterval)));
        }

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
                fireCount++;
            }


        }

        if (fireCount < 8)
            yield return new WaitForSeconds(interval);
        else
        {
            fireCount = 0;
            gameObject.GetComponent<SpriteRenderer>().color = Color.red;
            yield return new WaitForSeconds(4f);
            gameObject.GetComponent<SpriteRenderer>().color = Color.white;
        }
    }
}
