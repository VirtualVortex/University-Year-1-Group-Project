using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SquishMover : MonoBehaviour {
    public float minScale;
    public float maxScale;
    public float scaleTime;
    bool isScalingUp;
    float currentTime;

	// Use this for initialization
	void Start () {
        currentTime = 0;
        isScalingUp = true;
	}
	
	// Update is called once per frame
	void Update () {
        var temp = transform.localScale;
        temp.y = Mathf.Lerp(minScale, maxScale, currentTime / scaleTime);
        transform.localScale = temp;
        if (isScalingUp == true)
        {
            currentTime += Time.deltaTime;
            if (currentTime > scaleTime)
            {
                currentTime = scaleTime;
                isScalingUp = false;
            }
        }
        else
        {
            currentTime -= Time.deltaTime;
            if (currentTime < 0)
            {
                currentTime = 0;
                isScalingUp = true;
            }
        }

	}
}
