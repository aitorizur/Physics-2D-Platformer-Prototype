using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraFollow : MonoBehaviour {

    public GameObject objective;

    Vector3 cameraFollowObjective;


    public float cameraMoveSpeed = 5f;

    // Use this for initialization
    void Start ()
    {
        objective = GameObject.Find("Player");

        transform.position = new Vector3 (objective.transform.position.x, objective.transform.position.y, transform.position.z);
	}
	
	// Update is called once per frame
	void Update ()
    {

        //PRUEBAS DE CAMBIO DE OBJETIVO
        if (Input.GetKeyDown(KeyCode.Alpha1))
        {
           GameObject newobjective = GameObject.Find("Sign");
            SetCamObjective(newobjective);
        }
        else if (Input.GetKeyDown(KeyCode.Alpha2))
        {
            GameObject newobjective = GameObject.Find("Player");
            SetCamObjective(newobjective);
        }
        //FIN DE PRUEBAS

        cameraFollowObjective = objective.transform.position;
        cameraFollowObjective.z = transform.position.z;

        Vector3 cameraMovDir = (cameraFollowObjective - transform.position).normalized;
        float distance = Vector3.Distance(cameraFollowObjective, transform.position);
        
        if (distance > 0)
        {

            Vector3 newCamPosition = transform.position + cameraMovDir * distance * cameraMoveSpeed * Time.deltaTime;

            float distanceAfterMoving = Vector3.Distance(cameraFollowObjective, newCamPosition);

            if (distanceAfterMoving > distance)
            {

                transform.position = cameraFollowObjective;

            }
            else
            {
                transform.position = transform.position + cameraMovDir * distance * cameraMoveSpeed * Time.deltaTime;
            }
        }
        
	}

    //FUNCION PUBLICA PARA CAMBIAR EL OBJETIVO DE LA CAMERA DESDE CUALQUIER SCRIPT
    public void SetCamObjective(GameObject newObjective)
    {
        objective = newObjective;
    }

}
