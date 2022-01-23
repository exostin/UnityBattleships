using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PressAnyKey : MonoBehaviour
{
    public TextMeshProUGUI pressAnyKeyText;

    private float _dottingInterval = 0.6f;
    private bool _dottingRunning = true;
    private string _dots;

    private void Start()
    {
        StartCoroutine(AnyKeyDotting());
    }

    private void Update()
    {
        if (Input.anyKeyDown)
        {
            _dottingRunning = false;
            SceneManager.LoadScene(1);
        }
    }

    private IEnumerator AnyKeyDotting()
    {
        while (_dottingRunning)
        {
            for (int i = 0; i < 4; i++)
            {
                _dots = new string('.', i);
                pressAnyKeyText.SetText("Press any key to continue" + _dots);

                yield return new WaitForSeconds(_dottingInterval);
            }
        }
    }
}