using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PipeCherryMoving : pipeMoving_script
{
    public float widthOffset;
    public float heightOffset;
    // Start is called before the first frame update
    void Start()
    {
        base.Setup();
        SetupCherry();
    }

    void SetupCherry()
    {
        //Cherry position is random
        BoxCollider2D collider = GetComponentInChildren<Collider2D>() as BoxCollider2D;
        // Size of the rectangle.These values are specified relative to a center point,
        // so the distance from the center to the left edge is actually width/2.
        Vector2 size = collider.size;
        //Get world position of collider's local offset
        Vector3 worldPos = collider.transform.TransformPoint(collider.offset);
        float right = worldPos.x + (size.x / 2f);
        float left = worldPos.x - (size.x / 2f);
        float upOffset = transform.position.y + heightOffset;
        float downOffset = transform.position.y - heightOffset;
        transform.GetChild(2).gameObject.SetActive(true);
        
        Vector3 cherryPos = new Vector3(Random.Range(right+1, widthOffset), Random.Range(downOffset, upOffset), 0);
        transform.GetChild(2).gameObject.transform.position = cherryPos;
    }

    // Update is called once per frame
    void Update()
    {
        base.Update();
    }
}
