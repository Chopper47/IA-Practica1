using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditor.ShaderGraph.Internal;
using UnityEngine;
using static UnityEditor.Progress;

public class ChooseTarget : MonoBehaviour
{
    private SteeringBehaviour steeringAgent;
    [SerializeField] private List<GameObject> civilians = new List<GameObject>();
    [SerializeField] private List<GameObject> zombies = new List<GameObject>();
    private float distanceToTarget = Mathf.Infinity;
    [SerializeField] private float detectionRange = 10f;
    private GameObject newTarget;
    [SerializeField] int random;
    private void Start()
    {
        
    }
    // Update is called once per frame
    void Update()
    {
        
        if (gameObject.tag == "Civilian") ChooseZombie();
        else if (gameObject.tag == "Zombie") ChooseCivilian();
    }

    /// <summary>
    /// Civilian is chosen randomly across all in the scene
    /// </summary>
    private void ChooseCivilian()
    {
        //Changes target if the current one turns into zombie, if not chases it until finding it.
        if (gameObject.GetComponent<SteeringBehaviour>().target != null)
        {
            if (gameObject.GetComponent<SteeringBehaviour>().target.gameObject.tag == "Civilian")
            {
                return;
            }
        }

        civilians.Clear();
        foreach(GameObject civ in GameObject.FindGameObjectsWithTag("Civilian"))
        {
            civilians.Add(civ);
        }

        //If there's no more civilians, deactivates the steering components
        if(civilians.Count > 0)
        {
            random = UnityEngine.Random.Range(0, civilians.Count);
            newTarget = civilians[random];
            gameObject.GetComponent<SteeringBehaviour>().target = newTarget.transform;

        }

        else
        {
            this.GetComponent<SteeringBehaviour>().enabled = false;
        }
    }


    //Closest zombie is chosen as target to escape from. Activates the flee function only if there's a zombie in range
    private void ChooseZombie()
    {
        distanceToTarget = Mathf.Infinity;
        zombies.Clear();
        foreach (GameObject zom in GameObject.FindGameObjectsWithTag("Zombie"))
        {
            zombies.Add(zom);
        }
        foreach (var item in zombies)
        {
            float newDistanceToTarget = Vector3.Distance(transform.position, item.transform.position);
            if (newDistanceToTarget < distanceToTarget)
            {
                distanceToTarget = newDistanceToTarget;
                newTarget = item;

            }
        }
        gameObject.GetComponent<SteeringBehaviour>().target = newTarget.transform;

        if(Vector3.Distance(transform.position, newTarget.transform.position) >= detectionRange)
        {
            gameObject.GetComponent<SteeringBehaviour>().enabled = false;
        }
        else
        {
            gameObject.GetComponent<SteeringBehaviour>().enabled = true;
        }
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, detectionRange);
    }
}
