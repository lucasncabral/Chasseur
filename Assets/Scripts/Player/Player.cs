using UnityEngine;
using System.Collections;

[RequireComponent (typeof (PlayerController))]
[RequireComponent(typeof(FieldOfView))]
[RequireComponent(typeof(GunController))]
public class Player : MonoBehaviour {

	public float moveSpeed = 5;

	Camera viewCamera;
    public Camera shootCamera;
	PlayerController controller;
    GunController gunController;
    FieldOfView playerFOV;
    public bool aiming;

    //public override void Start () {
    void Start()
    {
        playerFOV = GetComponent<FieldOfView>();
        controller = GetComponent<PlayerController> ();
        gunController = GetComponent<GunController>();
        viewCamera = Camera.main;
        aiming = false;
	}

    private void FixedUpdate()
    {
        Vector3 position = this.transform.position;

        Vector3 shootCam = position + new Vector3();
        shootCamera.transform.position = position + new Vector3(0, 14.25f, 0f);
        
        Vector3 camPosition = position + new Vector3(0f, 15.35f, -13.71f);
        viewCamera.transform.position = camPosition;
    }

    void Update () {
        // Movement input
		Vector3 moveInput = new Vector3 (Input.GetAxisRaw ("Horizontal"), 0, Input.GetAxisRaw ("Vertical"));
		Vector3 moveVelocity = moveInput.normalized * moveSpeed;
		controller.Move (moveVelocity);

        // Look input
		Ray ray = shootCamera.ScreenPointToRay (Input.mousePosition);
		Plane groundPlane = new Plane (Vector3.up, Vector3.zero);
		float rayDistance;
		if (groundPlane.Raycast(ray,out rayDistance)) {
			Vector3 point = ray.GetPoint(rayDistance);
			Debug.DrawLine(ray.origin,point,Color.red);
            //Debug.DrawRay(ray.origin,ray.direction * 100,Color.red);
            
            if (aiming)
            {
                controller.LookAt(point);
            } else
            {
                controller.LookAt(transform.position + moveVelocity);
            }
            
		}

        // Weapon input
        if (Input.GetMouseButton(0))
        {
            if(aiming)
                gunController.OnTriggerHold();
        }

        if (Input.GetMouseButtonUp(0))
        {
            if (aiming)
                gunController.OnTriggerRelease();
        }

        if (Input.GetMouseButtonDown(1))
        {
            aiming = true;
            playerFOV.isAiming();
        }

        if (Input.GetMouseButtonUp(1))
        {
            aiming = false;
            playerFOV.stopAiming();
        }
    }
}