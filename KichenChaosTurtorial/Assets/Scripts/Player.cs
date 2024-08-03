using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using UnityEngine;

public class Player : MonoBehaviour
{
    [SerializeField] private float playerSpeed = 7f;
    [SerializeField] private GameInput gameInput;

    private bool isWalking = false;
    private void Update()
    {
        HandleMovement();
        HandleInteractions();
    }

    public bool IsWalking()
    {
        return isWalking;
    }

    private void HandleInteractions()
    {
        Vector2 inputVector = gameInput.GetInputVectorNormalized();

        Vector3 dir = new Vector3(inputVector.x, 0, inputVector.y);

        float interactDistance = 2.0f;
        Physics.Raycast(transform.position, dir, out RaycastHit raycastHit, interactDistance);
    }
    private void HandleMovement()
    {

        Vector2 inputVector = gameInput.GetInputVectorNormalized();

        Vector3 dir = new Vector3(inputVector.x, 0, inputVector.y);

        float moveDistance = playerSpeed * Time.deltaTime;
        float playerHeight = 2.0f;
        float playerRadius = 0.7f;
        bool canMove = !Physics.CapsuleCast(transform.position, transform.position + Vector3.up * playerHeight, playerRadius, dir, moveDistance);

        if (!canMove)
        {
            Vector3 moveDirX = new Vector3(dir.x, 0, 0).normalized;
            canMove = !Physics.CapsuleCast(transform.position, transform.position + Vector3.up * playerHeight, playerRadius, moveDirX, moveDistance);

            if (canMove)
            {
                dir = moveDirX;
            }
            else
            {
                Vector3 moveDirZ = new Vector3(0, 0, dir.z).normalized;
                canMove = !Physics.CapsuleCast(transform.position, transform.position + Vector3.up * playerHeight, playerRadius, moveDirZ, moveDistance);

                if (canMove)
                {
                    dir = moveDirZ;
                }
            }
        }

        if (canMove)
        {
            transform.position += dir * moveDistance;
        }

        isWalking = dir != Vector3.zero;
        float rotSpeed = 10f;
        transform.forward = Vector3.Slerp(transform.forward, dir, Time.deltaTime * rotSpeed);
    }
}
