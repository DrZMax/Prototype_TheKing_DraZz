using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{

    public float MoveSpeed;

    private Animator _animator;
    private Rigidbody2D _playerRigidBody;
    private bool _isPlayerMoving;
    private Vector2 _lastMove;                  //Permet de savoir la direction de notre dernier mouvement


    void Start()
    {
        _animator = GetComponent<Animator>();
        _playerRigidBody = GetComponent<Rigidbody2D>();
        MoveSpeed = 5;

    }

    void Update()
    {
        movePlayer();
        animatePlayer();
    }


    private void movePlayer()
    {

        _isPlayerMoving = false;

        if (Input.GetAxisRaw("Horizontal") > 0.5f || Input.GetAxisRaw("Horizontal") < -0.5f)
        {
            //Je laisse l'exemple de Translate. C'est une autre facon de faire bouger notre player
            //transform.Translate(new Vector3(Input.GetAxisRaw("Horizontal") * moveSpeed * Time.deltaTime, 0f, 0f));

            _playerRigidBody.velocity = new Vector2(Input.GetAxisRaw("Horizontal") * MoveSpeed, _playerRigidBody.velocity.y);

            _isPlayerMoving = true;
            _lastMove = new Vector2(Input.GetAxisRaw("Horizontal"), 0f);
        }
        else
        {
            _playerRigidBody.velocity = new Vector2(0f, _playerRigidBody.velocity.y);
        }

        if (Input.GetAxisRaw("Vertical") > 0.5f || Input.GetAxisRaw("Vertical") < -0.5f)
        {
            //Je laisse l'exemple de Translate. C'est une autre facon de faire bouger notre player
            //transform.Translate(new Vector3(0f, Input.GetAxisRaw("Vertical") * moveSpeed * Time.deltaTime, 0f));

            _playerRigidBody.velocity = new Vector2(_playerRigidBody.velocity.x, Input.GetAxisRaw("Vertical") * MoveSpeed);

            _isPlayerMoving = true;
            _lastMove = new Vector2(0f, Input.GetAxisRaw("Vertical"));
        }
        else
        {
            _playerRigidBody.velocity = new Vector2(_playerRigidBody.velocity.x, 0f);
        }
    }


    private void animatePlayer()
    {
        //On set les variables a utiliser pour l'animation
        _animator.SetFloat("MoveX", Input.GetAxisRaw("Horizontal"));
        _animator.SetFloat("MoveY", Input.GetAxisRaw("Vertical"));
        _animator.SetBool("IsPlayerMoving", _isPlayerMoving);
        _animator.SetFloat("LastMoveX", _lastMove.x);
        _animator.SetFloat("LastMoveY", _lastMove.y);

    }
}
