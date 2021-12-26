using UnityEngine;

public class snake : MonoBehaviour
{
    public float speed = 3f;
    public float rotation_speed = 300f;
    float horizontal = 0f;
    public string InputAxis = "Horizontal";
    public GameObject clientObj;

    // Update is called once per frame
    void Update()
    {
        // horizontal = Input.GetAxisRaw(InputAxis);
        if (Input.GetKey(KeyCode.D))
        {
            clientObj.GetComponent<ClientUDP>().AddInput(KeyCode.D, "Pressed");
        }

        if (Input.GetKey(KeyCode.A))
        {
            clientObj.GetComponent<ClientUDP>().AddInput(KeyCode.A, "Pressed");
        }

        if (Input.GetKeyUp(KeyCode.D))
        {
            clientObj.GetComponent<ClientUDP>().AddInput(KeyCode.D, "KeyUp");
        }

        if (Input.GetKeyUp(KeyCode.A))
        {
            clientObj.GetComponent<ClientUDP>().AddInput(KeyCode.A, "KeyUp");
        }
    }

}
