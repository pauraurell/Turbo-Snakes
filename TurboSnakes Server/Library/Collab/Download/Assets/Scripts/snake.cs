using UnityEngine;
using System.Collections.Generic;
using System.Collections;
using System;

public class snake : MonoBehaviour
{
    public float speed = 3f;
    public float rotation_speed = 300f;
    float horizontal = 0f;
    List<PlayerInput> inputs = new List<PlayerInput>();
    public GameObject serverObj;
    public bool alive = true;

    // Update is called once per frame
    void Update()
    {
        //horizontal = Input.GetAxisRaw(InputAxis);
        //ProcessInputs();
    }

    void FixedUpdate()
    {
        if (alive) 
        {
            ProcessInputs();
        }
        
        if(serverObj.GetComponent<ServerUDP>().player2connected)
        {
            transform.Translate(Vector2.up * speed * Time.fixedDeltaTime, Space.Self);
            transform.Rotate(Vector3.forward * -horizontal * rotation_speed * Time.fixedDeltaTime);
        }
    }

    void OnTriggerEnter2D(Collider2D col)
    {
        if (col.tag == "killsPlayer") 
        {
            speed = 0f;
            rotation_speed = 0f;
            inputs.Clear();
            alive = false;
            //GameObject.FindObjectOfType<GameManager>().EndGame();
        }
    }

    public void AddInput(PlayerInput input)
    {
        inputs.Add(input);  
    }

    public void ProcessInputs()
    {
        if(inputs.Count > 0)
        {
            
            List<PlayerInput> temp = new List<PlayerInput>(inputs);
            inputs.Clear();
            foreach (PlayerInput input in temp)
            {
                float temp_rotation = gameObject.GetComponent<Rigidbody2D>().rotation;
                if (input.type != null) 
                {
                    
                    switch (input.type)
                    {
                        case "Pressed":
                            switch (input.key)
                            {
                                case "D":
                                    temp_rotation = 1f;
                                    horizontal = temp_rotation;
                                    break;
                                case "A":
                                    temp_rotation = -1f;
                                    horizontal = temp_rotation;
                                    break;

                                case "Space":
                                    speed = 3.2f;
                                    break;

                                case "M":
                                    speed = 1f;
                                    break;
                            }
                            break;

                        case "KeyUp":
                            switch (input.key)
                            {
                                case "D":
                                    horizontal = 0f;
                                    break;
                                case "A":
                                    horizontal = 0f;
                                    break;

                                case "Space":
                                    speed = 2f;
                                    break;

                                case "M":
                                    speed = 2f;
                                    break;

                            }
                            break;
                    }

                    
                }     
            }
        }
    }
}
