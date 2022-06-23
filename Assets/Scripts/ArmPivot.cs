using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ArmPivot : MonoBehaviour
{
    //Properties
    bool isAttacking = false;

    float attackingArc = 150;
    float angle;
    float absAngle;
    float attackInterp;
    float attackSpeed;
    int layer;
    Vector2 leftPos;
    Vector2 rightPos;
    Vector3 attackStart;
    Vector3 attackEnd;

    GameObject parent;
    SpriteRenderer s_renderer;
    Vector2 armPos;
    Quaternion startRot, endRot, rotationZ, localRot;


    void Start()
    {
        parent = this.transform.parent.gameObject;
        s_renderer = GetComponentInChildren<SpriteRenderer>();

        // Define different positios for arm.
        rightPos = transform.localPosition;
        leftPos = rightPos;
        leftPos.x = leftPos.x * -1;
    }

    void FixedUpdate()
    {
        angle = parent.GetComponent<Knight>().mouseAngle;
        absAngle = Mathf.Abs(angle);
        layer = 1;

        if (angle < 0)
        {
            if (absAngle >= 112.5)
            {
                transform.localPosition = leftPos;
                layer = -1;
            }
            else if (absAngle <= 67.5)
            {
                transform.localPosition = rightPos;
                layer = -1;
            }
            else if (absAngle >= 90)
            {
                transform.localPosition = rightPos;
                layer = 1;
            }
            else
            {
                transform.localPosition = leftPos;
                layer = 1;
            }
        }
        else
        {

            if (absAngle > 90)
            {
                transform.localPosition = rightPos;
                layer = -1;
            }
            else
            {
                transform.localPosition = leftPos;
                layer = -1;
            }

            /*
            if (absAngle > 112.5)
            {
                transform.localPosition = rightPos;
                layer = -1;
            }
            else if (absAngle <= 67.5)
            {
                transform.localPosition = leftPos;
                layer = -1;
            }
            else if (absAngle >= 90)
            {
                transform.localPosition = leftPos;
                layer = -1;
            }
            else
            {
                transform.localPosition = rightPos;
                layer = -1;
            }
            */
        }


        if (absAngle < 90)
        {
            rotationZ = Quaternion.Euler(180, 0, -angle + 150);

            if (isAttacking)
            {
                if (attackInterp == 0)
                {
                    startRot = rotationZ;
                    endRot = Quaternion.Euler(180, 0, -angle + 150 + attackingArc);
                }

                if (attackInterp < 1) attackInterp += Time.deltaTime * attackSpeed;
                transform.rotation = Quaternion.Lerp(startRot, endRot, attackInterp);

                if (transform.rotation == endRot)
                {
                    isAttacking = false;
                }
            }
            else
            {
                transform.localRotation = rotationZ;
            }
        }
        else
        {
            rotationZ = Quaternion.Euler(0f, 0f, angle + 150);
            if (isAttacking)
            {
                if (attackInterp == 0)
                {
                    startRot = rotationZ;
                    endRot = Quaternion.Euler(0f, 0f, angle + 150 + attackingArc);
                }

                if (attackInterp < 1) attackInterp += Time.deltaTime * attackSpeed;
                transform.rotation = Quaternion.Lerp(startRot, endRot, attackInterp);

                if (transform.rotation == endRot)
                {
                    isAttacking = false;
                }
            }
            else
            {
                transform.localRotation = rotationZ;
            }
        }
        s_renderer.sortingOrder = layer;
    }

    public void AttackRotation()
    {
        isAttacking = true;
        attackInterp = 0;
        attackSpeed = 7.0f;
    }


}

