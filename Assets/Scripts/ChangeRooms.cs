using UnityEngine;
using System.Collections;

public class ChangeRooms : MonoBehaviour {

    public static ChangeRooms Instance;

    public enum Dir {
        Up = 0, Down = 1, Left = 2, Right = 3
    }

    public Camera camera1;
    public Camera camera2;
    public GameObject worldBounds;

    private float offH = 10f;
    private float offW = 13.4f;
    private float camZ = -10f;
    private float movePlayerDist = 2f;
    private float transitionTime = 2f;

    private Vector3[] cam2Start;
    private Vector3[] cam1End;
    private Vector3[] movePlayer;

    private void Awake() {
        Instance = this;

        cam2Start = new Vector3[] { new Vector3(0, -offH, camZ), new Vector3(0, offH, camZ), new Vector3(offW, 0, camZ), new Vector3(-offW, 0, camZ) };
        cam1End = new Vector3[] { new Vector3(0, offH, camZ), new Vector3(0, -offH, camZ), new Vector3(-offW, 0, camZ), new Vector3(offW, 0, camZ) };
        movePlayer = new Vector3[] { new Vector3(0, movePlayerDist, 0), new Vector3(0, -movePlayerDist, 0), new Vector3(-movePlayerDist, 0, 0), new Vector3(movePlayerDist, 0, 0) };
    }
    private void OnDestroy() {
        Instance = null;
    }

    public void ChangeRoom(Dir dir) {

        //camera1.clearFlags = CameraClearFlags.Nothing;
        camera2.gameObject.SetActive(true);
        camera2.transform.position = cam2Start[(int)dir];

        PlayerControls.Instance.OnRoomTransitionStarted();
        Game.isRoomTransition = true;
        int id = (int)dir;

        GameObject player = PlayerControls.Instance.gameObject;

        Vector3 playerPos = player.transform.position;
        Vector3 wrappedPos = playerPos + cam2Start[id];
        wrappedPos = new Vector3(wrappedPos.x, wrappedPos.y, 0);

        player.transform.position = wrappedPos;
        Vector3 newPlayerPos = wrappedPos + movePlayer[id];
        LeanTween.move(player, newPlayerPos, transitionTime).setIgnoreTimeScale(true);

        Time.timeScale = 0f;
        Game.Instace.OnLeaveRoom();

        worldBounds.SetActive(false);

        LeanTween.move(camera1.gameObject, cam1End[id], transitionTime).setIgnoreTimeScale(true);
        LeanTween.move(camera2.gameObject, new Vector3(0, 0, camZ), transitionTime).setIgnoreTimeScale(true).setOnComplete(() => {
            camera1.transform.position = new Vector3(0, 0, -10);
            camera2.gameObject.SetActive(false);
            PlayerControls.Instance.OnRoomTranisiontCompleted();
            Game.Instace.OnEnterNewRoom(DirToString(dir));
            Game.isRoomTransition = false;
            Time.timeScale = 1;
            worldBounds.SetActive(true);
        });
    }

    private string DirToString(Dir d) {
        string[] s = new string[] { "U", "D", "L", "R" };
        return s[(int)d];
    }
}
