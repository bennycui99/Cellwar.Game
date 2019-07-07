using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class test0 : MonoBehaviour {
    public GameObject num;
    public GameObject ground;
    public GameObject food;
    public int number;
    public Vector3 num_position;
    float xoffood;
    float zoffood;
    float xofnum;
    float zofnum;
    float deltax;
    float deltaz;
    float sec;

    // Use this for initialization
    void Start () {
        food = GameObject.Find("food");
        Vector3 food_position = food.transform.position;
        num_position = num.transform.position;
        xoffood = food_position.x;
        zoffood = food_position.z;
        xofnum = num_position.x;
        zofnum = num_position.z;
        deltax = xoffood - xofnum;
        deltaz = zoffood - zofnum;
	}
	
	// Update is called once per frame
	void Update () {
        upbysec();
        //spread();
    }

    //the number of the ground multiplies 2 by second until it reaches 1000
    private void upbysec()
    {
        if (sec >= 0.5f)
        {
            if (number < 1000)
            {
                string thenum = num.GetComponent<TextMesh>().text;
                number = int.Parse(thenum);
                number = number * 2;
                thenum = number.ToString();
                num.GetComponent<TextMesh>().text = thenum;

                printcolor(ground);
                sec = 0;
            }
        }
        sec = sec + Time.deltaTime;
    }

    //the color of the ground becomes darker as the number becomes bigger
    private void printcolor(GameObject gd)
    {
        if (number > 10)
        {
            gd.GetComponent<Renderer>().material.color = new Color(1f, 0.4f, 0.4f, 0.5f);
        }
        if (number > 50)
        {
            gd.GetComponent<Renderer>().material.color = new Color(1f, 0.2f, 0.2f, 0.5f);
        }
        if (number > 100)
        {
            gd.GetComponent<Renderer>().material.color = new Color(1f, 0f, 0f, 0.5f);
        }
        if (number > 500)
        {
            gd.GetComponent<Renderer>().material.color = new Color(0.8f, 0f, 0f, 0.5f);
        }
        if (number > 1000)
        {
            gd.GetComponent<Renderer>().material.color = new Color(0.5f, 0f, 0f, 0.5f);
        }
    }

    //when the number reaches 1000 its remaining spread to adjacent grounds
    private void spread()
    {
        if (number > 1000)
        {
            int remaining = number - 1000;
            int x_sprd = (int)(remaining * deltax / (deltax + deltaz));
            int z_sprd = (int)(remaining * deltaz / (deltax + deltaz));
            int xofnbx = (int)xofnum + 1;
            int zofnbx = (int)zofnum;
            int xofnbz = (int)xofnum;
            int zofnbz = (int)zofnum + 1;
            string name_x = "num (" + xofnbx.ToString() + "," + zofnbx.ToString() + ")";
            string name_z = "num (" + xofnbz.ToString() + "," + zofnbz.ToString() + ")";
            Debug.Log(name_x+"&" +name_z);
            GameObject.Find(name_x).GetComponent<test0>().number = GameObject.Find(name_x).GetComponent<test0>().number + x_sprd;
            Debug.Log(1111);

            //GameObject.Find(name_z).GetComponent<test0>().number = GameObject.Find(name_z).GetComponent<test0>().number + z_sprd;

            number = 1000;
            string thenum = number.ToString();
            num.GetComponent<TextMesh>().text = thenum;
        }
    }
}
