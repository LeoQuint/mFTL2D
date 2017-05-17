using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

[RequireComponent(typeof(Rigidbody2D))]
public class PlayerController : NetworkBehaviour {

    private Transform m_Top;
    private Transform m_Bot;

    private Rigidbody2D m_Rb;

    private float m_MovementSpeed = 100f;
    private float m_RotationSpeedTop = 100f;
    private float m_RotationSpeedBot = 10f;

    public override void OnStartClient()
    {



    }

    private void Awake()
    {
        m_Top = transform.FindChild("top");
        m_Bot = transform.FindChild("bot");
        m_Rb = GetComponent<Rigidbody2D>();
    }

    private void Update()
    {
        GetInput();

    }

    private void FixedUpdate()
    {
        Aim();
        Move();
        RotateFeet();
    }

    #region PlayerMovements

    private Vector3 movementDirection = Vector3.zero;
    private Vector3 aimDirection = Vector3.zero;

    private Vector3 legsRotation = Vector3.zero;
    private Vector3 armsRotation = Vector3.zero;

    private void GetInput()
    {
        ///Twin stick movement.
        //Movement
        movementDirection.x = Input.GetAxis("HorizontalMove");
        movementDirection.y = Input.GetAxis("VerticalMove");
        //Aim
        aimDirection.x = Input.GetAxis("HorizontalAim");
        aimDirection.y = Input.GetAxis("VerticalAim");
        //Debug.Log(movementDirection);
        //Debug.Log(aimDirection);
    }

    private void Aim()
    {
        if (aimDirection.y != 0 || aimDirection.x != 0)
        {
            //Debug.Log(Mathf.Atan2(aimDirection.y, aimDirection.x) * 180 / Mathf.PI);
            m_Top.eulerAngles = new Vector3(0, 0, Mathf.Atan2(aimDirection.y, aimDirection.x) * 180 / Mathf.PI);
            armsRotation = m_Top.eulerAngles;
        }
        else
        {
            m_Top.eulerAngles = armsRotation;
        }
    }

    private void Move()
    {
        m_Rb.velocity = movementDirection * m_MovementSpeed * Time.deltaTime;
    }

    private void RotateFeet()
    {
        if (movementDirection.y != 0 || movementDirection.x != 0)
        {
            //Debug.Log(Mathf.Atan2(movementDirection.y, movementDirection.x) * 180 / Mathf.PI);
            m_Bot.eulerAngles = new Vector3(0, 0, Mathf.Atan2(movementDirection.y, movementDirection.x) * 180 / Mathf.PI);
            legsRotation = m_Bot.eulerAngles;          
        }
        else
        {
            m_Bot.eulerAngles = legsRotation;          
        }        
    }

    #endregion
}
