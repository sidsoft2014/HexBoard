using UnityEngine;

public class OnClickDisableObj : MonoBehaviour
{
    private void OnClick()
    {
        this.gameObject.SetActive(false);
    }
}