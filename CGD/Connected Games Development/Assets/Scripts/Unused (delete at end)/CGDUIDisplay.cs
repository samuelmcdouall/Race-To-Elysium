using Photon.Pun;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CGDUIDisplay : MonoBehaviour
{
    PhotonView _view; // may just remove this script

    private void Start()
    {
        _view = GetComponent<PhotonView>();
    }
    public void DisplayUI(float duration)
    {
        if (_view.IsMine || true)
        {
            gameObject.GetComponent<Image>().color = new Color(1.0f, 1.0f, 1.0f, 1.0f);
            Invoke("HideUI", duration);
        }
    }

    void HideUI()
    {
        gameObject.GetComponent<Image>().color = new Color(1.0f, 1.0f, 1.0f, 0.0f);
    }
}
