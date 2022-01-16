using UnityEngine;

public class snake : MonoBehaviour
{
    public float speed = 3f;
    public float rotation_speed = 300f;
    float horizontal = 0f;
    public string InputAxis = "Horizontal";
    public CharacterController myCC;
   

    // Update is called once per frame
    void Update()
    {
        BasicMovement();
        // horizontal = Input.GetAxisRaw(InputAxis);
        /*if (Input.GetKey(KeyCode.D))
        {
            clientObj.GetComponent<ClientUDP>().AddInput(KeyCode.D, "Pressed");
        }

        if (Input.GetKey(KeyCode.A))
        {
            clientObj.GetComponent<ClientUDP>().AddInput(KeyCode.A, "Pressed");
        }

        if (Input.GetKey(KeyCode.Space))
        {
            clientObj.GetComponent<ClientUDP>().AddInput(KeyCode.Space, "Pressed");
        }

        if (Input.GetKey(KeyCode.M))
        {
            clientObj.GetComponent<ClientUDP>().AddInput(KeyCode.M, "Pressed");
        }

        if (Input.GetKeyUp(KeyCode.D))
        {
            clientObj.GetComponent<ClientUDP>().AddInput(KeyCode.D, "KeyUp");
        }

        if (Input.GetKeyUp(KeyCode.A))
        {
            clientObj.GetComponent<ClientUDP>().AddInput(KeyCode.A, "KeyUp");
        }

        if (Input.GetKeyUp(KeyCode.Space))
        {
            clientObj.GetComponent<ClientUDP>().AddInput(KeyCode.Space, "KeyUp");
        }

        if (Input.GetKeyUp(KeyCode.M))
        {
            clientObj.GetComponent<ClientUDP>().AddInput(KeyCode.M, "KeyUp");
        }*/
    }

    void BasicMovement()
    {
        if (Input.GetKey(KeyCode.W))
        {
            myCC.Move(transform.up * Time.deltaTime * speed);
        }
        if (Input.GetKey(KeyCode.A))
        {
            myCC.Move(-transform.right * Time.deltaTime * speed);
        }
        if (Input.GetKey(KeyCode.S))
        {
            myCC.Move(-transform.up * Time.deltaTime * speed);
        }
        if (Input.GetKey(KeyCode.D))
        {
            myCC.Move(transform.right * Time.deltaTime * speed);
        }
    }

}
