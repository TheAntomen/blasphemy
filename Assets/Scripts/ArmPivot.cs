using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Class for handling the rotation of the knight´s arm based on the angle
/// between the knight and the mouse position, also handles attack movement
/// </summary>
public class ArmPivot : MonoBehaviour
{

    // Private variables
    private bool isAttacking = false;
    private float attackingArc = 150;
    private float angle;
    private float absAngle;
    private float attackInterp;
    private float attackSpeed;
    private int layer;
    private Vector2 leftPos;
    private Vector2 rightPos;
    private GameObject parent;
    private SpriteRenderer s_renderer;
    private Quaternion startRot, endRot, rotationZ;

    
    void Start()
    {
        parent = transform.parent.gameObject;
        s_renderer = GetComponentInChildren<SpriteRenderer>();

        // Define different positios for arm.
        rightPos = transform.localPosition;
        leftPos = rightPos;
        leftPos.x *= -1;
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

