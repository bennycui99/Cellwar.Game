using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class test1 : MonoBehaviour
{
    //public GameObject num;
    //public GameObject ground;
    //public GameObject food;
    Renderer Renderer;
    TextMesh textmesh;
    List<GameObject> neighbour = new List<GameObject>();
    public int current_number;
    public int max_number;
    public int spread_number;
    public float growth_time;
    float current_time;
    public int nutrient1;
    public Vector3 num_position;
    float xoffood;
    float zoffood;
    float xofnum;
    float zofnum;
    float deltax;
    float deltaz;

    // Use this for initialization
    void Start()
    {
        current_time = 0;
        Renderer = GetComponent<Renderer>();
        textmesh = GetComponentInChildren<TextMesh>();
    }
    private void  OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag=="ground")
        {
            neighbour.Add(other.gameObject);
        }
    }
    // Update is called once per frame
    void Update()
    {
        updatePopulation();
        //spread();
    }
    private void FixedUpdate()
    {
        current_time += Time.deltaTime;
        if (current_time>=growth_time)
        {
            DoublePopulation();
            current_time = 0;
        }
        if(current_number>=spread_number)
        {
            spreadNew();
        }
    }

    void DoublePopulation()
    {
        current_number = current_number * 2;

        if (current_number >= max_number)
        {
            current_number = max_number;

        }
    }
        //the number of the ground multiplies 2 by second until it reaches 1000
        private void updatePopulation()
        {
            textmesh.text = current_number.ToString();
            printcolor();
        }

    //the color of the ground becomes darker as the number becomes bigger
    private void printcolor()
    {
        if (current_number > 10)
        {
            Renderer.material.color = new Color(1f, 0.4f, 0.4f, 0.5f);
        }
        if (current_number > 50)
        {
            Renderer.material.color = new Color(1f, 0.2f, 0.2f, 0.5f);
        }
        if (current_number > 100)
        {
            Renderer.material.color = new Color(1f, 0f, 0f, 0.5f);
        }
        if (current_number > 500)
        {
            Renderer.material.color = new Color(0.8f, 0f, 0f, 0.5f);
        }
        if (current_number > 1000)
        {
            Renderer.material.color = new Color(0.5f, 0f, 0f, 0.5f);
        }
    }

    //when the number reaches 1000 its remaining spread to adjacent grounds
    private void spread()
    {
        if (current_number > 1000)
        {
            int remaining = current_number - 1000;
            int x_sprd = (int)(remaining * deltax / (deltax + deltaz));
            int z_sprd = (int)(remaining * deltaz / (deltax + deltaz));
            int xofnbx = (int)xofnum + 1;
            int zofnbx = (int)zofnum;
            int xofnbz = (int)xofnum;
            int zofnbz = (int)zofnum + 1;
            string name_x = "num (" + xofnbx.ToString() + "," + zofnbx.ToString() + ")";
            string name_z = "num (" + xofnbz.ToString() + "," + zofnbz.ToString() + ")";
            //Debug.Log(name_x + "&" + name_z);
            GameObject.Find(name_x).GetComponent<test0>().number = GameObject.Find(name_x).GetComponent<test0>().number + x_sprd;
            //Debug.Log(1111);

            //GameObject.Find(name_z).GetComponent<test0>().number = GameObject.Find(name_z).GetComponent<test0>().number + z_sprd;

            current_number = 1000;
            string thenum = current_number.ToString();
            //num.GetComponent<TextMesh>().text = thenum;
        }
    }
    void spreadNew()
    {
        //Debug.Log(neighbour.Capacity);

        foreach (GameObject i in neighbour)
        {
            //Debug.Log(i.name);
            
            test1 test1 =  i.GetComponent<test1>();
            if (test1.current_number==0)
            {
                test1.current_number = 1;

            }
        }
    }
}
