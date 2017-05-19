using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

[RequireComponent(typeof(Rigidbody2D))]
public class PlayerController : NetworkBehaviour {

    private Transform m_Top;
    private Transform m_Bot;

    private Rigidbody2D m_Rb;
    private GameObject m_Camera;

    private float m_MovementSpeed = 100f;

    private float m_FireCD = 0.2f;
    private float m_NextFire = 0f;

    public GameObject _BulletPrefab;
    public GameObject _PlayerCamPrefab;

    public override void OnStartClient()
    {
        Debug.Log("Client started.");
        GameObject c = Instantiate(_PlayerCamPrefab, transform.position, Quaternion.identity) as GameObject;
        m_Camera = c;
        m_Camera.GetComponent<PlayerCam>().m_Target = transform;
    }

    private void Awake()
    {
        m_Top = transform.FindChild("top");
        m_Bot = transform.FindChild("bot");
        m_Rb = GetComponent<Rigidbody2D>();
    }

    private void Update()
    {
        if (!isLocalPlayer)
        {
            return;
        }
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
        movementDirection.Normalize();
        //Aim
        aimDirection.x = Input.GetAxis("HorizontalAim");
        aimDirection.y = Input.GetAxis("VerticalAim");
        aimDirection.Normalize();
        //buttons
        if (Input.GetAxis("Fire") < -0.75f && m_NextFire < Time.time)
        {
            m_NextFire = Time.time + m_FireCD;
            CmdFire();
        }
    }

    private void Aim()
    {
        if (aimDirection.y != 0 || aimDirection.x != 0)
        {
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
            m_Bot.eulerAngles = new Vector3(0, 0, Mathf.Atan2(movementDirection.y, movementDirection.x) * 180 / Mathf.PI);
            legsRotation = m_Bot.eulerAngles;          
        }
        else
        {
            m_Bot.eulerAngles = legsRotation;          
        }        
    }



    #endregion

    #region Commands
    [Command]
    void CmdFire()
    {
        // Create the Bullet from the Bullet Prefab
        var bullet = (GameObject)Instantiate(
            _BulletPrefab,
            m_Top.position,
            m_Top.rotation);

        // Add velocity to the bullet
        bullet.GetComponent<Rigidbody2D>().velocity = bullet.transform.right * 10f;

        // Spawn the bullet on the Clients
        NetworkServer.Spawn(bullet);

        // Destroy the bullet after 2 seconds
        Destroy(bullet, 2.0f);
    }
    #endregion
}
