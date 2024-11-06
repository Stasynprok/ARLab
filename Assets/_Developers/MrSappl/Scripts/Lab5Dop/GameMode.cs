using UnityEngine;
using static InteractionManager;

public class GameMode : MonoBehaviour, IInteractionManagerMode
{
    [SerializeField] private GameObject _ui;
    [SerializeField] private GameObject _uiGameOver;
    [SerializeField] private GameObject _uiWin;
    [SerializeField] private Timer _timer;
    [SerializeField] private SpawnInPolygon _spawnInPolygon;

    private bool _isGameOver = false;

    public void Activate()
    {
        InteractionManager.Instance.SetManagerMode(ManagerMode.Planes);
        _spawnInPolygon.GenerateObjects();
        _spawnInPolygon.NeedFindClear += WinAction;

        _ui.SetActive(true);
        _uiGameOver.SetActive(false);
        _uiWin.SetActive(false);

        _timer.OnTimeIsUp += OnGameOver;
        _timer.Activate();

        _isGameOver = false;
    }

    public void Deactivate()
    {
        _ui.SetActive(false);
        _uiGameOver.SetActive(false);
        _uiWin.SetActive(false);
        _timer.Deactivate();
        _timer.OnTimeIsUp -= OnGameOver;
        _spawnInPolygon.DestroyAllobjects();
        _spawnInPolygon.NeedFindClear -= WinAction;
    }

    public void TouchInteraction(UnityEngine.Touch[] touches)
    {
        if (_isGameOver)
        {
            return;
        }
        UnityEngine.Touch touch = touches[0];
        bool overUI = touch.position.IsPointOverUIObject();

        if (touch.phase != TouchPhase.Began || overUI)
            return;

        TrySelectObject(touch.position);
    }

    public void BackToDefaultScreen()
    {
        InteractionManager.Instance.SelectMode(0);
    }

    private void OnGameOver()
    {
        _uiGameOver.SetActive(true);
        _isGameOver = true;
    }

    public void OnResetButton()
    {
        _uiGameOver.SetActive(false);
        _uiWin.SetActive(false);
        _isGameOver = false;
        _spawnInPolygon.GenerateObjects();
        _timer.Activate();
    }

    private void TrySelectObject(Vector2 pos)
    {
        // fire a ray from camera to the target screen position
        Ray ray = InteractionManager.Instance.ARCamera.ScreenPointToRay(pos);
        RaycastHit hitObject;
        if (!Physics.Raycast(ray, out hitObject))
        {
            return;
        }
            

        if (!hitObject.collider.CompareTag("CreatedObject"))
        {
            return;
        }

        // if we hit a spawned object tag, try to get info from it
        GameObject selectedObject = hitObject.collider.transform.root.gameObject;
        ObjectForGame objectForGame = selectedObject.GetComponent<ObjectForGame>();

        _spawnInPolygon.GetSelectedObject(objectForGame);
    }

    private void WinAction()
    {
        _uiWin.SetActive(true);
        _timer.Deactivate();
    }
}
