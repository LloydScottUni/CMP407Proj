using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Movement : MonoBehaviour
{
    private CharacterController _controller;
    
    //Jumping vars
    private Vector3 _playerAirVelocity;
    private bool _groundedPlayer;
    private float _jumpHeight = 10.0f;
    private float _gravityValue = -30.81f;
    
    //Walk speed vars
    private float _defaultPlayerSpeed = 24.0f;
    private float _playerSpeed = 0;
    
    //Step rate/count/sound vars
    private float _stepCount = 0.0f;
    private float _footstepRate = 0f;
    public AK.Wwise.Event footstepSound = new AK.Wwise.Event();
    
    private void Start()
    {
        _playerSpeed = _defaultPlayerSpeed;
        _controller = gameObject.AddComponent<CharacterController>();
        _controller.stepOffset = 1.0f;
    }

    private void Update()
    {
        //Jumping logic
        _playerAirVelocity.y += _gravityValue * Time.deltaTime;
        _controller.Move(_playerAirVelocity * Time.deltaTime);
        
        _groundedPlayer = _controller.isGrounded;
        if (_groundedPlayer && _playerAirVelocity.y < 0)
        {
            _playerAirVelocity.y = 0.0f;
        }
        
        if (Input.GetButtonDown("Jump") && _groundedPlayer)
        {
            //Equation taken from Unity documentation
            _playerAirVelocity.y += Mathf.Sqrt(_jumpHeight * -3.0f * _gravityValue);
        }
        
        //Sprinting
        if (Input.GetKey(KeyCode.LeftShift))
            _playerSpeed = _defaultPlayerSpeed * 2;
        else if (Input.GetKeyUp(KeyCode.LeftShift))
            _playerSpeed = _defaultPlayerSpeed;

        //General Movement
        Vector3 move = transform.right * Input.GetAxis("Horizontal") + transform.forward * Input.GetAxis("Vertical");
        _controller.Move(move * Time.deltaTime * _playerSpeed);
        
        //Step sound rhythm logic
        _footstepRate = 1f / (_playerSpeed / (_playerSpeed - 1f));
        
        if (_controller.velocity.magnitude > 0f && _groundedPlayer)
        {
            _stepCount += Time.deltaTime * (_playerSpeed/10.0f);
            if(_stepCount > _footstepRate)
            {
                footstepSound.Post(gameObject);
                _stepCount = 0.0f;
            }
        }
    }
    private void OnTriggerEnter(Collider other)
    {
        //Slow down or speed the player up based on which environment they are in.
        if (other.gameObject.CompareTag("Grass") || other.gameObject.CompareTag("Hellstone"))
        {
            _defaultPlayerSpeed = 24.0f;
        }
        else if (other.gameObject.CompareTag("Slime") || other.gameObject.CompareTag("Ocean"))
        {
            _defaultPlayerSpeed = 12.0f;
        }
        _playerSpeed = _defaultPlayerSpeed;
    }
}