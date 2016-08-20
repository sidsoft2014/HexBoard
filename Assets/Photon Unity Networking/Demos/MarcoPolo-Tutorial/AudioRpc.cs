using UnityEngine;

public class AudioRpc : Photon.MonoBehaviour
{
    public AudioClip marco;
    public AudioClip polo;

    private AudioSource m_Source;

    private void Awake()
    {
        m_Source = GetComponent<AudioSource>();
    }

    [PunRPC]
    private void Marco()
    {
        if (!this.enabled)
        {
            return;
        }

        Debug.Log("Marco");

        m_Source.clip = marco;
        m_Source.Play();
    }

    [PunRPC]
    private void Polo()
    {
        if (!this.enabled)
        {
            return;
        }

        Debug.Log("Polo");

        m_Source.clip = polo;
        m_Source.Play();
    }

    private void OnApplicationFocus(bool focus)
    {
        this.enabled = focus;
    }
}