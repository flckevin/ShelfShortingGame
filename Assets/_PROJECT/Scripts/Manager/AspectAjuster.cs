using UnityEngine;
using UnityEngine.UI;

public class AspectAjuster : MonoBehaviour
{

    CanvasScaler scaler = null;

    void Awake()
    {

        scaler = GetComponent<CanvasScaler>();
        CheckAspectRatio();
    }

    void CheckAspectRatio()
    {
        //Kiem tra ty le man hinh, tuy tung do phan giai ma chinh scaler ve theo dai hay rong
        float acturalRatio = (float)Screen.width / (float)Screen.height;
        float originalRatio = (float)scaler.referenceResolution.x / (float)scaler.referenceResolution.y;

        if (acturalRatio > originalRatio)
        {
            scaler.matchWidthOrHeight = 1.0f;
        }
        else
        {
            scaler.matchWidthOrHeight = 0.0f;
        }
    }
}
