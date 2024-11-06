using UnityEngine;

public class ObjectForGame : MonoBehaviour
{
    [SerializeField] private GameObject _ui;
    [SerializeField] private bool _needFind = false;
    public int Index = -1;


    public void NeedFindThis()
    {
        _ui.SetActive(true);
        _needFind = true;
    }
    public void DisableSign()
    {
        _ui.SetActive(false);
    }

    public bool ImFind()
    {
        if (_needFind)
        {
            return true;
        }

        return false;
    }
}
