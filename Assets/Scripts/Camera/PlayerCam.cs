using UnityEngine;

public class PlayerCam : MonoBehaviour {

    public Transform m_Target;
    public Vector3 m_Offset;

    [Tooltip("Speed the camera moves.")]
    public float m_CameraSpeed = 5f;
    [Tooltip("This scales the camera speed by a factor of X by the distance between the camera and its target.")]
    public float m_SpeedScaling = 2f;

	
	// Update is called once per frame
	void FixedUpdate () {
        float scaledSpeed = Vector3.Distance(transform.position, m_Target.position + m_Offset) * m_SpeedScaling;
        transform.position = Vector3.Lerp(transform.position, m_Target.position + m_Offset, Time.deltaTime * m_CameraSpeed * scaledSpeed);
    }
}
