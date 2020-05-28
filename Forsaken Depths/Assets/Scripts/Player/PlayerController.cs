using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    public GameObject attackEffectGameObject;
    public ParticleSystem attackEffect;

    public GameObject boundaryEffects;

    public ParticleSystem boundaryEffectTop, boundaryEffectRight, boundaryEffectBottom, boundaryEffectLeft;

    Joystick joystick;

    public float playerSpeed = 20;
    public float turnSpeed = 10;
    public float height = 1f;
    public float heightPadding = 0.05f;
    public LayerMask ground;
    public float maxGroundAngle = 120;

    Vector2 input;
    float angle;
    float groundAngle;
    Quaternion targetRotation;

    Vector3 forward;
    RaycastHit playerTerrainRayInfo;
    bool grounded;

    public IsMenuActive isMenuActive;

    public bool isDead;

    public bool debug;


    void Start()
    {
        StartCoroutine(EnableVFX());

        joystick = FindObjectOfType<Joystick>();
        
        isDead = false;
    }

    void FixedUpdate()
    {
        if (!isMenuActive.isMenuActivated)
        {
            GetInput();                
        }

        CalculateDirection();
        CalculateForward();
        CalculateGroundAngle();
        CheckGround();
        ApplyGravity();
        DrawDebugLines();

        if ((Mathf.Abs(input.x) < 1) && (Mathf.Abs(input.y) < 1)) return;

        Rotate();
        Move();
    }

    IEnumerator EnableVFX()
    {
        attackEffectGameObject.SetActive(true);
        attackEffect.Stop();

        boundaryEffects.SetActive(true);
        boundaryEffectTop.Stop();
        boundaryEffectRight.Stop();
        boundaryEffectBottom.Stop();
        boundaryEffectLeft.Stop();

        boundaryEffectTop.Play();
        boundaryEffectRight.Play();
        boundaryEffectBottom.Play();
        boundaryEffectLeft.Play();

        var bETopEmissionRate = boundaryEffectTop.emission;
        var bERightEmissionRate = boundaryEffectRight.emission;
        var bEBottomEmissionRate = boundaryEffectBottom.emission;
        var bELeftEmissionRate = boundaryEffectLeft.emission;

        WaitForSeconds waitTime = new WaitForSeconds(0.25f);
        for (float i = 0; i < 11; i++)
        {
            bETopEmissionRate.rateOverTime = i;        
            bERightEmissionRate.rateOverTime = i;        
            bEBottomEmissionRate.rateOverTime = i;        
            bELeftEmissionRate.rateOverTime = i;

            yield return waitTime;
        }

        yield return null;
    }

    void GetInput()
    {
        input.x = joystick.Horizontal * playerSpeed;
        input.y = joystick.Vertical * playerSpeed;
    } 

    void Move()
    {
        if (groundAngle >= maxGroundAngle) return;
        transform.position += forward * playerSpeed * Time.deltaTime;
    }

    void Rotate()
    {
        targetRotation = Quaternion.Euler(0, angle, 0);
        transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, turnSpeed * Time.deltaTime);
    }

    void CalculateDirection()
    {
        angle = Mathf.Atan2(input.x, input.y);
        angle = Mathf.Rad2Deg * angle;
    }

    void CalculateForward()
    {
        if (!grounded)
        {
            forward = transform.forward;
            return;
        }   

        forward = Vector3.Cross(transform.right, playerTerrainRayInfo.normal);
    }

    void CalculateGroundAngle()
    {
        if (!grounded)
        {
            groundAngle = 90;
            return;
        }

        groundAngle = Vector3.Angle(playerTerrainRayInfo.normal, transform.forward);
    }

    void CheckGround()
    {
        if (Physics.Raycast(transform.position, -Vector3.up, out playerTerrainRayInfo, height + heightPadding, ground))
        {
            if (playerTerrainRayInfo.distance < height)
                transform.position = Vector3.Lerp(transform.position, transform.position + Vector3.up * height, 10 * Time.deltaTime);

            grounded = true;
        }

        else
        {
            grounded = false;
        }
    }

    void ApplyGravity()
    {
        if (!grounded)
        {
            transform.position += Physics.gravity * Time.deltaTime;
        }
    }

    void DrawDebugLines()
    {
        if (!debug) return;

        Debug.DrawLine(transform.position, transform.position + forward * height * 2, Color.blue);
        Debug.DrawLine(transform.position, transform.position - Vector3.up * height, Color.green);
    }
}
