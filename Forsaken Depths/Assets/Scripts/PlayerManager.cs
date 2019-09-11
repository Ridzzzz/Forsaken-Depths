using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerManager : StatSystem
{
    protected float playerBaseHealth = 50f;
    protected float playerBaseSpeed = 25f;
    protected float playerBaseAttackDamage = 15f;
    protected float playerBaseResistance = 10f;

    protected Rigidbody playerRigidBody;
    protected Transform playerTransform;
    protected Joystick joystick;

    void Start()
    {
        playerRigidBody = GetComponent<Rigidbody>();
        playerTransform = transform;
        joystick = FindObjectOfType<Joystick>();
        Speed = playerBaseSpeed;
    }

    void Update()
    {
        Move();
    }

    protected override void Move()
    {
        playerRigidBody.velocity = new Vector3(joystick.Horizontal * Speed, 0, joystick.Vertical * Speed);

        if (playerRigidBody.velocity != Vector3.zero)
        {
            playerRigidBody.rotation = Quaternion.Slerp(playerRigidBody.rotation, Quaternion.LookRotation(playerRigidBody.velocity), Time.deltaTime * Speed);
        }
    }
}
