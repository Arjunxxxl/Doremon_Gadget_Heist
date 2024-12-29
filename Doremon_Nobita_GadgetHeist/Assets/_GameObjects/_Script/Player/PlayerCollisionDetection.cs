using UnityEngine;

public class PlayerCollisionDetection : MonoBehaviour
{
    [System.Serializable]
    private class GroundDetectionData
    {
        public bool isGroundedPhantom;
        public bool isGrounded;
        public bool wasGrounded;
        public float groundedPhantomTime;
        public float groundedPhantomTimeELapsed;
        public Vector3 groundCheckOffset;
        public float groundCheckRadius;
        public LayerMask groundLayerMask;
    }

    [Header("Ground Detection")]
    [SerializeField] private GroundDetectionData groundDetectionData;
    
    // Update is called once per frame
    void Update()
    {
        CheckGrounded();
        GroundPhantomTime();
    }
    
    #region Grounded

    private void CheckGrounded()
    {
        Collider[] colliders = Physics.OverlapSphere(transform.position + groundDetectionData.groundCheckOffset,
                                                            groundDetectionData.groundCheckRadius, 
                                                            groundDetectionData.groundLayerMask);
        
        Debug.Log(colliders.Length);
        groundDetectionData.isGroundedPhantom = colliders.Length > 0;
        
        if (groundDetectionData.isGroundedPhantom)
        {
            groundDetectionData.isGrounded = true;
            
            if (!groundDetectionData.wasGrounded)
            {
                //TODO: Play land effects    
            }
            
            groundDetectionData.groundedPhantomTimeELapsed = 0;
            groundDetectionData.wasGrounded = groundDetectionData.isGrounded;
        }
    }

    private void GroundPhantomTime()
    {
        if (!groundDetectionData.isGroundedPhantom)
        {
            if (groundDetectionData.groundedPhantomTimeELapsed >= groundDetectionData.groundedPhantomTime)
            {
                groundDetectionData.isGrounded = false;
                groundDetectionData.wasGrounded = groundDetectionData.isGrounded;
            }
            else
            {
                groundDetectionData.groundedPhantomTimeELapsed += Time.deltaTime;
            }
        }
    }

    #endregion
    
    #region Gizmo

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.black;
        Gizmos.DrawSphere(transform.position + groundDetectionData.groundCheckOffset,
                          groundDetectionData.groundCheckRadius);
    }

    #endregion
}
