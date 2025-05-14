using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ParkourController : MonoBehaviour
{
    [SerializeField] List<NewParkourAction> parkourActions;

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
                foreach (var action in parkourActions)
                {
                    if (action.CheckIfPossible(hitData, transform))
                    {
                        StartCoroutine(DoParkourAction(action));
                        break;

                    }
                }

            }

        }

    }
    IEnumerator DoParkourAction(NewParkourAction action)
    {

        inAction = true;
        playerMovement.SetControl(false);

        animator.CrossFade(action.AnimName, 0.2f);
        while (!animator.GetCurrentAnimatorStateInfo(0).IsName(action.AnimName))
            yield return null;

        //  yield return null;


        var animState = animator.GetNextAnimatorStateInfo(0);
        // if (!animState.IsName(action.AnimName))
        //{
        //    Debug.LogError("the parkour Animation is wrong!");
        // }

        //yield return new WaitForSeconds(animState.length); before //this code waits for the length of the animation
        float animLength = animState.length;
        float timer = 0f;
        bool matched = false;
        while (timer <= animLength) //this code waits for the length of the animation
        {
            timer += Time.deltaTime;

            if (action.RotateToObstacle)
            {  //rotate the player towards the obstacle
                transform.rotation = Quaternion.RotateTowards(transform.rotation, action.TargetRotation,
                    playerMovement.RotationSpeed * Time.deltaTime);

            }
            if (action.EnableTargetMaching && !matched && timer >= animLength * action.MatchStartTime)
            {
                MatchTarget(action);
                matched = true;
            }
            yield return null;
        }
        playerMovement.SetControl(true);
        inAction = false;

    }

    void MatchTarget(NewParkourAction action)
    {
        if (animator.isMatchingTarget) return;

        animator.MatchTarget(action.MatchPos, transform.rotation, action.MatchBodyPart,
            new MatchTargetWeightMask(new Vector3(0, 1, 0), 0), action.MatchStartTime, action.MatchTargetTime);
    }
    public bool IsClimbing()
    {
        return inAction == true;
    }
}
