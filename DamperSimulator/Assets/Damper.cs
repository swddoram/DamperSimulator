using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Damper : MonoBehaviour
{
    public float m1; //Sprung mass
    public float m2; //Unsprung mass
    public float DampingConstant;
    public float k1; //Stiffness of the spring
    public float k2; //Tire stiffness
    public float F; //Force
    public float TimeTerm;
    float Position;
    float Velocity;
    private double z_s, v_s, z_u, v_u;
    public double c1; // Damping coefficient between sprung and unsprung mass
    public double c2; // Tire damping
    public GameObject SprungMass;
    public GameObject UnsprungMass;

    // Start is called before the first frame update
    void Start()
    {
        z_s = 0.0;
        v_s = 0.0;
        z_u = 0.1;
        v_u = 0.0;
    }

    // Update is called once per frame
    void Update()
    {
        double dt = Time.deltaTime;
        MidpointStep(dt);
        CalculatePosition();
    }

    void CalculatePosition()
    {
        float Ts = Time.deltaTime * TimeTerm;
        float a11 = 1f;
        float a12 = Ts;
        float a21 = -(Ts * k1) / m1;
        float a22 = 1f - (Ts * DampingConstant) / m1;
        float b1 = 0f;
        float b2 = Ts / m1;


        Position = a11 * Position + a12 * Velocity + b1 * F;
        Velocity = a21 * Position + a22 * Velocity + b2 * F;
        SprungMass.transform.position = new Vector3(SprungMass.transform.position.x, Position, SprungMass.transform.position.z);
    }

    void MidpointStep(double dt)
    {
        double[] k1 = ComputeDerivatives(z_s, v_s, z_u, v_u);
        double z_s_mid = z_s + 0.5 * dt * k1[0];
        double v_s_mid = v_s + 0.5 * dt * k1[1];
        double z_u_mid = z_u + 0.5 * dt * k1[2];
        double v_u_mid = v_u + 0.5 * dt * k1[3];

        double[] k2 = ComputeDerivatives(z_s_mid, v_s_mid, z_u_mid, v_u_mid);

        z_s += dt * k2[0];
        v_s += dt * k2[1];
        z_u += dt * k2[2];
        v_u += dt * k2[3];
    }

    double[] ComputeDerivatives(double z_s, double v_s, double z_u, double v_u)
    {
        double dz_s = v_s;
        double dv_s = (-c1 * (v_s - v_u) - k1 * (z_s - z_u)) / m1;
        double dz_u = v_u;
        double dv_u = (c1 * (v_s - v_u) + k1 * (z_s - z_u) - c2 * v_u - k2 * z_u) / m2;

        return new double[] { dz_s, dv_s, dz_u, dv_u };
    }


}
