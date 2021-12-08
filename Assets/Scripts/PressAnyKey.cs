using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PressAnyKey : MonoBehaviour
{
    public TextMeshProUGUI pressAnyKeyText;

    private float dottingInterval = 0.6f;
    private bool dottingRunning = true;
    private string dots;

    private void Start()
    {
        StartCoroutine(AnyKeyDotting());
    }

    private void Update()
    {
        if (Input.anyKeyDown)
        {
            dottingRunning = false;
            SceneManager.LoadScene(1);
        }
    }

    private IEnumerator AnyKeyDotting()
    {
        while (dottingRunning)
        {
            for (int i = 0; i < 4; i++)
            {
                dots = new string('.', i);
                pressAnyKeyText.SetText("Press any key to continue" + dots);

                yield return new WaitForSeconds(dottingInterval);
            }
        }
    }
}