using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TurnToZombie : MonoBehaviour
{
    [SerializeField] private Material zombieMaterial;
    private MeshRenderer mRenderer;
    private void Awake()
    {
        mRenderer = gameObject.GetComponent<MeshRenderer>();
    }
    private void OnCollisionEnter(Collision collision)
    {
        if(gameObject.tag == "Civilian" && collision.gameObject.tag == "Zombie")
        {
            gameObject.tag = "Zombie";
            mRenderer.material = zombieMaterial;
            gameObject.AddComponent<Seek>();
            gameObject.GetComponent<Seek>().target = null;
            collision.gameObject.GetComponent<Seek>().target = null;
            gameObject.GetComponent<RaycastTest>().steeringAgent = gameObject.GetComponent<Seek>();
            Destroy(gameObject.GetComponent<Flee>());
        }
    }
}
