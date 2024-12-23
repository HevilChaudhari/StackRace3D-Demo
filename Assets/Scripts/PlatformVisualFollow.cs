using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlatformVisualFollow : MonoBehaviour
{

    [SerializeField] private Transform movingPlatform;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        transform.position = new Vector3(movingPlatform.position.x, transform.position.y, transform.position.z);
    }
}
