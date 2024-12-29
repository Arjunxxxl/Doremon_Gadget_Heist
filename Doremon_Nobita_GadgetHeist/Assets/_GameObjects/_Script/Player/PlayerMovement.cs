using System;
using UnityEngine;
using UnityEngine.Serialization;

public class PlayerMovement : MonoBehaviour
{
    [System.Serializable]
    private class MovementData
    {
        public float moveSpeed;
        public float moveSpeedLerpFac;
        public float moveSpeedWalk;
        public float moveSpeedRun;
        public Vector3 moveDir;
        public float ySpeed;
    }

    [Header("Input")]
    [SerializeField] private Vector2 inputDir;
    [FormerlySerializedAs("isRunning")]
    [SerializeField] private bool isWalking;

    [Header("Movement Data")]
    [SerializeField] private MovementData movementData;

    private CharacterController characterController;

    private void Start()
    {
        characterController = GetComponent<CharacterController>();
    }

    private void Update()
    {
        GetInput();
    }

    private void FixedUpdate()
    {
        Move();
    }

    #region Input

    private void GetInput()
    {
        inputDir.x = Input.GetAxis("Horizontal");
        inputDir.y = Input.GetAxis("Vertical");
        inputDir.Normalize();

        isWalking = Input.GetKey(KeyCode.LeftShift);
    }

    #endregion

    #region Movement

    private void Move()
    {
        movementData.moveDir = inputDir.x * transform.right +
                               inputDir.y * transform.forward;

        movementData.moveDir.y = movementData.ySpeed;

        movementData.moveSpeed = Mathf.Lerp(movementData.moveSpeed,
                                            isWalking ? movementData.moveSpeedWalk : movementData.moveSpeedRun,
                                            Time.deltaTime * movementData.moveSpeedLerpFac);

        if (movementData.moveDir.x == 0 && movementData.moveDir.z == 0)
        {
            movementData.moveSpeed = 1;
        }
        
        characterController.Move(movementData.moveDir * movementData.moveSpeed * Time.deltaTime);
    }

    #endregion
}
