using System;
using UnityEngine;

public class CameraController : MonoBehaviour
{
    [SerializeField] private float moveSpeed = 5f;
    
    private Vector2 moveInput;
    private void Start()
    {
        InputManager.OnMoveInput += SetMoveInput;
    }
    
    private void Update()
    {
        Move();
    }

    private void SetMoveInput(Vector2 input)
    {
        moveInput = input;
    }

    private void Move()
    {
        Vector3 move = new Vector3(moveInput.x, 0, moveInput.y);
        transform.Translate(move * (Time.deltaTime * moveSpeed), Space.World);
    }
}
