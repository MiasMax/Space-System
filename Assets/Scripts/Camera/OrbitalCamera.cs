/*using UnityEngine;

public class OrbitalCamera : MonoBehaviour
{
    public Transform target;                         // Objet � suivre
    public float distanceFactor = 3f;                // facteur multiplicateur de la taille de l'objet

    public float zoomSpeed = 10f;
    public float zoomMin = 0.1f;
    public float zoomMax = 10f;

    public float rotationSpeed = 5f;
    public float smoothSpeed = 5f;

    private float distance = 10f;
    private float yaw = 0f;
    private float pitch = 20f;

    private bool justSelected = false;
    private bool viewFromTop = true; // Indique si on est en vue de dessus fixe

    void Start()
    {
        Vector3 angles = transform.eulerAngles;
        yaw = angles.y;
        pitch = angles.x;
    }

    void Update()
    {
        Transform oldTarget = target;
        target = ObjectSelector.HandleClickSelection(Camera.main, target);

        if (target == null)
            return;

        // Si nouvel objet s�lectionn�, on repositionne la cam�ra AU-DESSUS dans le plan XY (axe Z+)
        if (target != oldTarget)
        {
            float objectSize = GetObjectSize(target.gameObject);
            distance = objectSize * distanceFactor;
            justSelected = true;
            viewFromTop = true;

            // Position imm�diate de la cam�ra au-dessus
            transform.position = target.position + new Vector3(0, 0, distance);
            transform.LookAt(target.position, Vector3.up);

            // Forcer pitch/yaw pour correspondre � cette vue fixe
            pitch = 90f;
            yaw = 0f;
        }

        // Zoom avec molette (zoom relatif � la distance actuelle)
        float scroll = Input.GetAxis("Mouse ScrollWheel");
        if (scroll != 0)
        {
            distance -= scroll * zoomSpeed;
            distance = Mathf.Clamp(distance, zoomMin * distance, zoomMax * distance);
            justSelected = false; // si zoom manuel, on d�sactive le repositionnement automatique
            viewFromTop = false;   // zoom manuel d�sactive aussi vue de dessus fixe
        }

        // Rotation avec clic droit
        if (Input.GetMouseButton(1))
        {
            yaw += Input.GetAxis("Mouse X") * rotationSpeed;
            pitch -= Input.GetAxis("Mouse Y") * rotationSpeed;
            pitch = Mathf.Clamp(pitch, -80f, 80f);
            justSelected = false; // rotation manuelle d�sactive repositionnement automatique
            viewFromTop = false;   // rotation manuelle d�sactive vue de dessus fixe
        }

        Vector3 desiredPosition;
        if (viewFromTop)
        {
            // Position au-dessus dans le plan XY (axe Z+)
            desiredPosition = target.position + new Vector3(0, 0, distance);
            transform.position = Vector3.Lerp(transform.position, desiredPosition, Time.deltaTime * smoothSpeed);
            transform.LookAt(target.position, Vector3.up);
        }
        else
        {
            // Position selon pitch/yaw (vue libre)
            Quaternion rotation = Quaternion.Euler(pitch, yaw, 0);
            desiredPosition = target.position + rotation * new Vector3(0, 0, -distance);
            float lerpSpeed = justSelected ? smoothSpeed * 5f : smoothSpeed;
            transform.position = Vector3.Lerp(transform.position, desiredPosition, Time.deltaTime * lerpSpeed);
            transform.LookAt(target.position);

            if (justSelected && Vector3.Distance(transform.position, desiredPosition) < 0.1f)
            {
                justSelected = false; // fin repositionnement automatique
            }
        }
    }

    float GetObjectSize(GameObject obj)
    {
        Renderer rend = obj.GetComponent<Renderer>();
        if (rend != null)
        {
            return rend.bounds.size.magnitude;
        }
        else
        {
            // Si pas de renderer, valeur par d�faut
            return 1f;
        }
    }
}
*/

using UnityEngine;

public class FreeFlyCamera : MonoBehaviour
{
    public float moveSpeed = 1000f;         // Vitesse de d�placement
    public int rotationSpeed = 2;        // Sensibilit� de la souris
    public float zoomSpeed = 1000f;         // Vitesse zoom molette
    public int runSpeed = 10;            // Acc�l�ration

    private float yaw = 0f;
    private float pitch = 0f;

    void Start()
    {
        Vector3 angles = transform.eulerAngles;
        yaw = angles.y;
        pitch = angles.x;
    }

    void Update()
    {
        // --- D�placement clavier ---
        

        Vector3 moveDir = Vector3.zero;
        if (Input.GetKey(KeyCode.E)) moveDir += transform.forward;
        if (Input.GetKey(KeyCode.D)) moveDir -= transform.forward;
        if (Input.GetKey(KeyCode.S)) moveDir -= transform.right;
        if (Input.GetKey(KeyCode.Z)) moveDir += transform.right;
        if (Input.GetKey(KeyCode.Space)) moveDir += transform.up;
        if (Input.GetKey(KeyCode.LeftShift) || Input.GetKey(KeyCode.A)) moveDir -= transform.up;
        
        moveDir.Normalize();

        if (Input.GetKey(KeyCode.Q))
        {
            transform.position += moveDir * moveSpeed * Time.deltaTime * runSpeed;
        }
        else
        {
            transform.position += moveDir * moveSpeed * Time.deltaTime;
        }
        

        // Zoom avec la molette
        float scroll = Input.GetAxis("Mouse ScrollWheel");
        transform.Translate(Vector3.forward * scroll * rotationSpeed * Time.deltaTime);

        // Rotation seulement si clic droit maintenu
        if (Input.GetMouseButton(0))
        {
            yaw -= Input.GetAxis("Mouse X") * rotationSpeed;
            pitch += Input.GetAxis("Mouse Y") * rotationSpeed;

            transform.rotation = Quaternion.Euler(pitch, yaw, 0f);
        }
    }
}
