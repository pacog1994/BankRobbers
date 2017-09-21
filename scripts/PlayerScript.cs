using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PlayerScript : MonoBehaviour {

    public RoomScript currentRoom;
    public float moveSpeed = 0.8f;
    public SpriteRenderer exclamation, fail;
    public AudioSource audioClip;

    private bool moving = false;
    private float t = 0.0f;
    private Vector3 startPosition;
    private Animator animator;
    private SpriteRenderer sprite;
    private bool isFacingRight = true;
    private bool isGameOver = false;

    void LoadNewScene ()
    {
        SceneManager.LoadScene("Scene", LoadSceneMode.Single);
    }

	// Use this for initialization
	void Start () { 
        fail.enabled = false;
        animator = GetComponent<Animator>();
        sprite = GetComponent<SpriteRenderer>();
        transform.position = currentRoom.transform.position;
        exclamation.enabled = false;
    }
	
    void RestartGame ()
    {
        SceneManager.LoadScene("scenes/stealth");
    }

    void Fail ()
    {
        fail.enabled = true;
        audioClip.Play();

        Invoke("RestartGame", 4);
    }

    private bool lost = false;

	// Update is called once per frame
	void Update () {
        if (!moving && currentRoom.loseOnEnter && !lost) {
            lost = true;
            Debug.Log("LOSE");
            isGameOver = true;
            exclamation.enabled = true;
            Invoke("Fail", 3);
        }
        if (!moving && currentRoom.winOnEnter) {
            Debug.Log("WIN");
            isGameOver = true;
            Invoke("LoadNewScene", 2);
        }
        if (!moving && !isGameOver) {
            if (Input.GetKeyDown("up")) {
                Debug.Log("UP");
                if (currentRoom.roomUp != null) {
                    currentRoom = currentRoom.roomUp;
                    moving = true;
                    startPosition = transform.position;
                    animator.SetBool("run", true);
                }
            }
            else if (Input.GetKeyDown("down")) {
                Debug.Log("DOWN");
                if (currentRoom.roomDown != null) {
                    currentRoom = currentRoom.roomDown;
                    moving = true;
                    startPosition = transform.position;
                    animator.SetBool("run", true);
                }
            }
            else if (Input.GetKeyDown("left")) {
                Debug.Log("LEFT");
                if (currentRoom.roomLeft != null) {
                    currentRoom = currentRoom.roomLeft;
                    moving = true;
                    startPosition = transform.position;
                    animator.SetBool("run", true);
                    if (isFacingRight) {
                        sprite.flipX = true;
                    }
                    isFacingRight = false;
                }
            }
            else if (Input.GetKeyDown("right")) {
                Debug.Log("RIGHT");
                if (currentRoom.roomRight != null) {
                    currentRoom = currentRoom.roomRight;
                    moving = true;
                    startPosition = transform.position;
                    animator.SetBool("run", true);
                    if (!isFacingRight) {
                        sprite.flipX = false;
                    }
                    isFacingRight = true;
                }
            }
        }

        if (transform.position == currentRoom.transform.position) {
            t = 0.0f;
            moving = false;
            animator.SetBool("run", false);
        }
        if (moving) {
            t += Time.deltaTime * moveSpeed;
            transform.position = Vector3.Lerp(startPosition, currentRoom.transform.position, t);
        }
	}
}
