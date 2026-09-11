using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerMovement : MonoBehaviour
{
    public float xySpeed = 10f;//Speed of movement in the XY plane
    public float rotatioSpeed = 100f;//Speed of rotation
    public float tiltValue = 40f;//Tiltvalue

    [SerializeField]

    InputActionReference moveAction;


    public GameObject aimObject;
    public Transform modelChild;


    private void Update()
    {
        Vector2 xyVector = moveAction.action.ReadValue<Vector2>();

        LocalMove(xyVector.x, xyVector.y, xySpeed);

        ClampPosition();

        RotationLook(xyVector.x, xyVector.y, rotatioSpeed);

        HorizontalTilt(modelChild, xyVector.x, tiltValue, 0.1f);
    }

    public void LocalMove(float x, float y, float speed)
    {
        transform.localPosition += new Vector3(x, y, 0) * speed * Time.deltaTime;

    }

    void ClampPosition()
    {
        Vector3 pos = Camera.main.WorldToViewportPoint(transform.position);
        pos.x = Mathf.Clamp01(pos.x);
        pos.y = Mathf.Clamp01(pos.y);
        transform.position = Camera.main.ViewportToWorldPoint(pos);
    }

    void RotationLook(float h, float v, float speed)
    {
        aimObject.transform.localPosition = new Vector3(h, v, 1);
        gameObject.transform.rotation = Quaternion.RotateTowards(transform.rotation, Quaternion.LookRotation(aimObject.transform.position), Mathf.Deg2Rad * speed * Time.deltaTime);//rotation

    }

    void HorizontalTilt(Transform target, float axis, float tiltValue,float lerpTime)
    {
        Vector3 targetEulerAngles = target.localEulerAngles;
        target.localEulerAngles = new Vector3(targetEulerAngles.x, targetEulerAngles.y, Mathf.LerpAngle(targetEulerAngles.z, -axis * tiltValue, lerpTime));
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.green;
        Gizmos.DrawWireSphere(aimObject.transform.position, .5f);
        Gizmos.DrawSphere(transform.position, .15f);
    }
}
