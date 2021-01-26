﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Fever : VirusClass
{

    private float damage = 1.0f;
    private float interval = 3.0f;

    protected override void Start()
    {
        base.Start();
        MaxHealth = Health = 30;
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
            nextRoutines.Enqueue(NewActionRoutine(SelfDestruct()));
        }

        return nextRoutines;
    }

    private IEnumerator SelfDestruct()
    {
        GameObject[] objects = GameObject.FindGameObjectsWithTag("Unit");
        GameObject temp = null;
        bool find = false;

        if (objects.Length != 0)
        {
            float min = 999f;
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
                for (float t = 0; t < interval; t += Time.deltaTime) yield return null;
                animator.SetTrigger("Destruct");
                temp.GetComponent<Unit>().GetDamaged(damage);
                gameObject.GetComponent<VirusClass>().GetDamaged(MaxHealth);
            }

        }

        yield return null;

    }

    private IEnumerator Move()
    {
        yield return MoveRoutine(GetObjectPos() + new Vector3(-0.1f, 0, 0), 0.05f);

    }

    private IEnumerator IdleRoutine(float time)
    {
        yield return new WaitForSeconds(time);
    }

}