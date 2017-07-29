using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum Direction { Left, Right }

public class MoveDirectionChangedEventArgs : EventArgs {
    private Direction moveDirection;

    public MoveDirectionChangedEventArgs(Direction moveDirection) {
        this.moveDirection = moveDirection;
    }

    public Direction MoveDirection {
        get {
            return this.moveDirection;
        }
    }
}

public class DudeController : MonoBehaviour {

    [SerializeField] private float maxRunSpeed = 50;
    [SerializeField] private float maxWalkSpeed = 25;

    private Rigidbody2D dudeRB;
    private Animator animator;

    private Direction moveDirection;
    private bool isRunPressed = false;

    // Jumping fields
    [SerializeField] private Transform groundChecker;

    public delegate void MoveDirectionChangedHandler(DudeController sender, MoveDirectionChangedEventArgs e);
    public event MoveDirectionChangedHandler MoveDirectionChanged;

    public Direction MoveDirection {
        get {
            return this.moveDirection;
        }
        set {
            if (this.moveDirection != value) {
                this.moveDirection = value;

                var e = new MoveDirectionChangedEventArgs(this.moveDirection);
                this.MoveDirectionChanged(this, e);
            }
        }
    }

	// Use this for initialization
	void Start () {
        //dudeRB = this.GetComponentInChildren<Rigidbody2D>();
        dudeRB = this.GetComponent<Rigidbody2D>();
        animator = this.GetComponent<Animator>();
        this.MoveDirectionChanged += OnMoveDirectionChanged;
	}
	
	// Update is called once per frame
	void Update () {
        this.animator.SetBool("isRunPressed", this.isRunPressed);
        this.animator.SetFloat("xSpeed", Mathf.Abs(this.dudeRB.velocity.x));
	}

    // Update is called every fixed period of time
    void FixedUpdate() {
        isRunPressed = Input.GetAxisRaw("Run") > 0;
        float xSpeed = Input.GetAxis("Horizontal") * ((isRunPressed) ? this.maxRunSpeed : this.maxWalkSpeed);

        this.dudeRB.velocity = new Vector2(xSpeed, this.dudeRB.velocity.y);
        if (xSpeed > 0) {
            this.MoveDirection = Direction.Right;
        } else if (xSpeed < 0) {
            this.MoveDirection = Direction.Left;
        }
    }

    private void OnMoveDirectionChanged(DudeController sender, MoveDirectionChangedEventArgs e) {
        Vector3 localScale = sender.transform.localScale;
        if (e.MoveDirection == Direction.Right) {
            localScale.x = Mathf.Abs(localScale.x);
        } else if (e.MoveDirection == Direction.Left) {
            localScale.x = -Mathf.Abs(localScale.x);
        }
        sender.transform.localScale = localScale;
    }
}
