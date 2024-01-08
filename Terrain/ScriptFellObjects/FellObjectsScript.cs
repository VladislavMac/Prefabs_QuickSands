using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FellObjectsScript : MonoBehaviour
{
    //public bool _isFall = false;
    //private Collision _fellObject;

    private void OnCollisionEnter(Collision fellObject) {
            //_isFall = true;
            //_fellObject = fellObject;

        fellObject.collider.gameObject.transform.position = new Vector3(fellObject.collider.gameObject.transform.position.x, 300f, fellObject.collider.gameObject.transform.position.z);
            
        //fellObject.collider.GetComponent<Rigidbody>().constraints = RigidbodyConstraints.FreezePositionX | RigidbodyConstraints.FreezePositionZ;
            //Debug.Log("Ground");
            //_fellObject.collider.GetComponent<Rigidbody>().constraints = RigidbodyConstraints.None;
            //_isFall = false;
            //_fellObject = null;
    }
}
