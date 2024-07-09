using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Solver : MonoBehaviour
{
    public float m1; //Sprung mass
    public float m2; //Unsprung mass
    public float DampingConstant;
    public float k1; //Stiffness of the spring
    public float k2; //Tire stiffness
    public float F; //Force
    public float TimeTerm;
    public float Velocity;
    private float x1, x2, v1, a1, v2, a2;
    public float c1; // Damping coefficient between sprung and unsprung mass
    public float c2; // Tire damping
    public GameObject SprungMass;
    public GameObject UnsprungMass;
    public GameObject Bottom;
    float g = 9.8f;


    // Start is called before the first frame update
    void Start()
    {
        x1 = SprungMass.transform.position.y;
        x2 = UnsprungMass.transform.position.y;
    }

    // Update is called once per frame
    void Update()
    {
        //Calculate SprungMass position
        float dt = Time.deltaTime;
        a1 = (- k1 * (x1 - x2) - DampingConstant * (v1 - v2)) / m1;
        v1 += a1 * dt;
        x1 += v1 * dt;
        SprungMass.transform.position += new Vector3(Velocity * dt, x1/100, 0);
        Debug.Log("x1=" + x1);

        //Calculate UnSprungMass position
        a2 = (k1 * (x1 - x2) + DampingConstant * (v1 - v2) - k2 * (x2 - Ground(UnsprungMass.transform.position.x))) / m2;
        v2 += a2 * dt;
        x2 += v2 * dt;
        UnsprungMass.transform.position += new Vector3(Velocity * dt, x2/100, 0);
        Debug.Log("x2=" + x2);

        float X = UnsprungMass.transform.position.x;
        //Calculate Bottom position
        Bottom.transform.position = new Vector3(X, Ground(X), 0);
    }


    static float Ground(float X)
    {
        return Mathf.Sin(X);
    }
}
