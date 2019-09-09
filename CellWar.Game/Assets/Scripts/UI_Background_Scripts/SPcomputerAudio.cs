using System.Collections;
using UnityEngine;

public class SPcomputerAudio : MonoBehaviour
{
    AudioSource m_audioSource;
    public AudioClip buz_1;
    public AudioClip buz_2;
    public AudioClip buz_3;
    public AudioClip buz_4;
    public AudioClip buz_5;
    private float Time_counter = 0.0f;
    private float Random_time = 10.0f;
    private int Buz_No_old;
    private int Buz_No_new;

    public Light m_Light;
    public float Blink_Seconds;
    public float Blink_Intensity;
    // Start is called before the first frame update
    void Start()
    {
        m_audioSource = GetComponent<AudioSource>();
        Buz_No_old = Random.Range(0, 4);
    }

    // Update is called once per frame
    void Update()
    {
        if (Time_counter > Random_time)
        {
            Random_time = Random.Range(30.0f, 50.0f);
            Time_counter = 0.0f;
            m_audioSource.Stop();
            ChooseBuz();
            m_audioSource.Play();
            if ( Buz_No_old <= 1)
            {
                StartCoroutine(BlinkTwiceBlue());
            }
            else
            {
                StartCoroutine(BlinkTwiceRed());
            }
            
        }
        Time_counter += Time.deltaTime;
    }
    IEnumerator BlinkTwiceBlue()
    {
        m_Light.color = Color.blue;
        float waitTime = Blink_Seconds / 2;
        // Get half of the seconds (One half to get brighter and one to get darker)
        while (m_Light.intensity < Blink_Intensity)
        {
            m_Light.intensity += Time.deltaTime / waitTime;        // Increase intensity
            yield return null;
        }
        while (m_Light.intensity > 2)
        {
            m_Light.intensity -= Time.deltaTime / waitTime;        //Decrease intensity
            yield return null;
        }
        while (m_Light.intensity < Blink_Intensity)
        {
            m_Light.intensity += Time.deltaTime / waitTime;        // Increase intensity
            yield return null;
        }
        while (m_Light.intensity > 2)
        {
            m_Light.intensity -= Time.deltaTime / waitTime;        //Decrease intensity
            yield return null;
        }
        yield return null;
    }
    IEnumerator BlinkTwiceRed()
    {
        m_Light.color = Color.red;
        float waitTime = Blink_Seconds / 2;
        // Get half of the seconds (One half to get brighter and one to get darker)
        while (m_Light.intensity < Blink_Intensity)
        {
            m_Light.intensity += Time.deltaTime / waitTime;        // Increase intensity
            yield return null;
        }
        while (m_Light.intensity > 2)
        {
            m_Light.intensity -= Time.deltaTime / waitTime;        //Decrease intensity
            yield return null;
        }
        while (m_Light.intensity < Blink_Intensity)
        {
            m_Light.intensity += Time.deltaTime / waitTime;        // Increase intensity
            yield return null;
        }
        while (m_Light.intensity > 2)
        {
            m_Light.intensity -= Time.deltaTime / waitTime;        //Decrease intensity
            yield return null;
        }
        yield return null;
    }
    void ChooseBuz()
    {
        //Promise to select a different one
        do
        {
            Buz_No_new = Random.Range(0, 4);
        } while (Buz_No_old == Buz_No_new);
        Buz_No_old = Buz_No_new;
        switch (Buz_No_new)
        {
            case 0:
                m_audioSource.clip = buz_1;
                break;
            case 1:
                m_audioSource.clip = buz_2;
                break;
            case 2:
                m_audioSource.clip = buz_3;
                break;
            case 3:
                m_audioSource.clip = buz_4;
                break;
            case 4:
                m_audioSource.clip = buz_5;
                break;
            default:
                break;
        }
    }
}
