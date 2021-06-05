using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TdV : MonoBehaviour
{
    public float tdV;
    // Start is called before the first frame update
    void Start()
    {
        Destroy(gameObject, tdV);
    }
}
