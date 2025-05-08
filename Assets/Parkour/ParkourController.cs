using System.Collections.Generic;
using UnityEngine;

public class ParkourController : MonoBehaviour
{
    public EnvironmentChecker environmentChecker;
    bool playerInAction;
    public Animator animator;

    [Header("Parkour Action Area")]
    public List<NewParkourAction> newParkourActions;

    private void Update()
    {
        if (Input.GetButton("Jump") && !playerInAction)
        {
            var hitData = environmentChecker.CheckObstacle();
            //  environmentChecker.CheckObstacle();
            if (hitData.hitFound)
            {

            }
        }


    }
}
