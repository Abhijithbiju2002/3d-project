using System.Collections;
using UnityEngine;

public class ParkourController : MonoBehaviour
{
    bool inAction;

    EnvironmentChecker environment_checker;
    Animator animator;
    PlayerMovement playerMovement;

    private void Awake()
    {
        environment_checker = GetComponent<EnvironmentChecker>();
        animator = GetComponent<Animator>();
        playerMovement = GetComponent<PlayerMovement>();
    }
    private void Update()
    {

        if (Input.GetKeyDown(KeyCode.LeftAlt) && !inAction)
        {


            var hitData = environment_checker.CheckObstacle();
            if (hitData.forwardHitFound)
            {
                StartCoroutine(DoParkourAction());
            }

        }

    }
    IEnumerator DoParkourAction()
    {
        inAction = true;
        playerMovement.SetControl(false);

        animator.CrossFade("Low Jump", 0.2f);
        yield return null;

        var animState = animator.GetNextAnimatorStateInfo(0);
        yield return new WaitForSeconds(animState.length);

        playerMovement.SetControl(true);
        inAction = false;

    }
}
