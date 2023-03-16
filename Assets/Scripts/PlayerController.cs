using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    private CharacterController controller;
    private Vector3 direction;
    public float forwardSpeed;
    public float maxSpeed;


    private int desiredLane = 1; //0: left 1: middle 2:right
    public float laneDistance = 4; // the distance between two lan

    public bool isGrounded;
    public LayerMask groundLayer;
    public Transform groundCheck;

    public float jumpForce;
    public float Gravity = -20;

    public Animator animator;
    private bool IsSliding = false;
    private float originalSpeed;
    private float speedMultiplier = 1f;
    private bool IsRunningL = false;
    private bool IsRunningR = false;

    //private bool IsBumped = false;

    void Start()
    {
        controller = GetComponent<CharacterController>();
        originalSpeed = forwardSpeed;
        speedMultiplier = originalSpeed / forwardSpeed;
        GameStateManager.Instance.OnGameStateChanged += OnGameStateChanged;
    }

    private void OnDestroy()
    {
        GameStateManager.Instance.OnGameStateChanged -= OnGameStateChanged;
    }
    void Update()
    {
        if (!PlayerManager.isGameStarted)
            return;

        //Increase Speed
        if(forwardSpeed < maxSpeed)
            forwardSpeed += 0.1f * Time.deltaTime;

        animator.SetBool("IsGameStarted", true);
        direction.z = forwardSpeed;

        isGrounded = Physics.CheckSphere(groundCheck.position, 0.15f, groundLayer);
        animator.SetBool("IsGrounded", isGrounded);

        if (isGrounded)
        {
            if (SwipeManager.swipeUp)
            {
                Jump();
            }
            if (SwipeManager.swipeDown && !IsSliding)
            {
                StartCoroutine(Slide());
            }
        }

        else
        {
            direction.y += Gravity * Time.deltaTime;
            if (SwipeManager.swipeDown && !IsSliding)
            {
                StartCoroutine(Slide());
                direction.y = -8;
            }
        }

        

        //Gather the inputs on which lane we should be
        if (SwipeManager.swipeRight)
        {
            desiredLane++;
            if (desiredLane == 3)
                desiredLane = 2;
            //animator.SetBool("IsRunningR", true);
            animator.Play("IsRunningR");
        }
        if (SwipeManager.swipeLeft)
        {
            desiredLane--;
            if (desiredLane == -1)
                desiredLane = 0;
            //animator.SetBool("IsRunningL", true);
            animator.Play("IsRunningL");

        }

        //Calculate where we should be in the future

        Vector3 targetPosition = transform.position.z * transform.forward + transform.position.y * transform.up;

        if(desiredLane == 0)
        {
            targetPosition += Vector3.left * laneDistance;
        }
        
        else if (desiredLane == 2)
        {
            targetPosition += Vector3.right * laneDistance;
        }

        //transform.position = targetPosition;

        if (transform.position != targetPosition)
        {
            Vector3 diff = targetPosition - transform.position;
            Vector3 moveDir = 25 * Time.deltaTime * diff.normalized;

            if (moveDir.sqrMagnitude < diff.magnitude)
                controller.Move(moveDir);
            else
                controller.Move(diff);
        }

        //transform.position = Vector3.Lerp(transform.position, targetPosition, 20f * Time.fixedDeltaTime);

        //Move Player
        controller.Move(direction * Time.deltaTime);
    }


    private void Jump()
    {
        direction.y = jumpForce;
    }

    private void OnControllerColliderHit(ControllerColliderHit hit)
    {
        if (hit.transform.CompareTag("Obstacle"))
        {
            PlayerManager.gameOver = true;
            FindObjectOfType<AudioManager>().PlaySound("GameOver");
        }
    }

    private IEnumerator Slide()
    {
        IsSliding = true;
        controller.center = new Vector3(0, 1.5f, 0);
        controller.height = 2.88f;
        animator.SetBool("IsSliding", true);


        yield return new WaitForSeconds(1.1f * speedMultiplier);

        IsSliding = false;
        controller.center = new Vector3(0, 2.2f, 0);
        controller.height = 4.4f;
        animator.SetBool("IsSliding", false);
        
    }

    private void OnGameStateChanged(GameState newGameState)
    {
        enabled = newGameState == GameState.Gameplay;
    }
}
