using Photon.Pun;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Theactualgame.Movement
{

    [RequireComponent(typeof(Rigidbody2D))]
    public class Movement : MonoBehaviourPun
    {
        [SerializeField] private float movementSpeed = 0f;
        [SerializeField] private GameObject crossHair = null;
        [SerializeField] private bool useController = false;
        [SerializeField] private GameObject projectilePrefab;

        [Space]
        [Header("Character Attributes:")]
        public float CROSSHAIR_DISTANCE = 1.0f;
        public float PROJECTILE_SPEED = 2f;

        [Space]
        [Header("Prefabs:")]



        private Rigidbody2D rb = null;

        Vector2 aim;
        Vector2 movement;


        private void Start() => rb = GetComponent<Rigidbody2D>();

        void Awake()
        {
            Cursor.lockState = CursorLockMode.Locked;
            Cursor.visible = false;
        }

        // Update is called once per frame
        void Update()
        {
            if (photonView.IsMine)
            {
                ProcessInputs();
                Move();
                Aim();
                //photonView.RPC("Shoot", RpcTarget.AllBuffered);
                Shoot();
            }
        }

        private void Move()
        {
            rb.MovePosition(rb.position + movement * movementSpeed * Time.deltaTime);
        }

        private void Aim()
        {
            if (aim.magnitude > 0.0f)
            {
                crossHair.transform.localPosition = aim * CROSSHAIR_DISTANCE;
                crossHair.SetActive(true);

            }
            else
            {
                crossHair.SetActive(false);
            }
        }
        

        private void Shoot()
        {
            if (!Input.GetMouseButtonDown(0)) { return; }

            photonView.RPC("FireProjectile", RpcTarget.All);

        }

        [PunRPC]
        private void FireProjectile()
        {
            Vector2 shootingDirection = new Vector2(aim.x, aim.y);
            shootingDirection.Normalize();

            var projectileInstance = Instantiate(projectilePrefab, transform.position, Quaternion.identity);
            projectileInstance.GetComponent<Rigidbody2D>().velocity = shootingDirection * PROJECTILE_SPEED;
            //projectileInstance.transform.Rotate(0.0f, 0.0f, Mathf.Atan2(shootingDirection.y, shootingDirection.x) * Mathf.Rad2Deg + 90f);
            //Destroy(projectileInstance, 3.0f);
        }

        void ProcessInputs()
        {

            if (useController) { 


            }
            else
            {
                movement = new Vector2
                {
                    x = Input.GetAxisRaw("Horizontal"),
                    y = Input.GetAxisRaw("Vertical")
                }.normalized;

                Vector2 mouseMovement = new Vector2(Input.GetAxis("Mouse X"), Input.GetAxis("Mouse Y"));
                aim = aim + mouseMovement;
                if (aim.magnitude > 1.0f)
                {
                    aim.Normalize();
                }
            }

 
        }

    }
}



