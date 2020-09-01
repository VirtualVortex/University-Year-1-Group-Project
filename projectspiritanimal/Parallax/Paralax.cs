using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Paralax : MonoBehaviour {

    public Transform[] backgrounds;
    public float smoothing = 1f;

    public bool verticalMovement = false;
    public bool horizontalMovement = true;

    private float[] parallaxScale;

    private Transform cam;
    private Vector3 previousCamPos;

    private void Awake()
    {
        cam = Camera.main.transform;
    }


    // Use this for initialization
    void Start () {
        previousCamPos = cam.position;

        parallaxScale = new float[backgrounds.Length];
        for (int i = 0; i < backgrounds.Length; i++)
        {
            if (verticalMovement)
                parallaxScale[i] = backgrounds[i].position.y * -1;

            if (horizontalMovement)
                parallaxScale[i] = backgrounds[i].position.z * -1;
        }
	}
	
	// Update is called once per frame
	void Update () {
        for (int i = 0; i < backgrounds.Length; i++)
        {
            float parallaxX;
            float parallaxY;
            float backgroundTargetPosX = backgrounds[i].position.x;
            float backgroundTargetPosY = backgrounds[i].position.y;

            if (verticalMovement)
            {
                parallaxY = (previousCamPos.y - cam.position.y) * parallaxScale[i];

                backgroundTargetPosY = backgrounds[i].position.y + parallaxY;
            }
            if (horizontalMovement)
            {
                parallaxX = (previousCamPos.x - cam.position.x) * parallaxScale[i];

                backgroundTargetPosX = backgrounds[i].position.x + parallaxX;
            }
            
            

            Vector3 backgroundTargetPos = new Vector3(backgroundTargetPosX, backgroundTargetPosY, backgrounds[i].position.z);

            backgrounds[i].position = Vector3.Lerp(backgrounds[i].position, backgroundTargetPos, smoothing * Time.deltaTime);
        }

        previousCamPos = cam.position;
	}
}
