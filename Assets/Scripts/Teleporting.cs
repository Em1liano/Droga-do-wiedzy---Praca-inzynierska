using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Teleporting : MonoBehaviour
{
    // index mapy, na którą chcemy się przeteleportować
    public int destination;
    // pozycja na następnej mapie
    public Vector3 position;

    private void OnTriggerEnter(Collider other)
    {
        StartCoroutine(WaitForFade());
    }

    IEnumerator WaitForFade()
    {
        SoundManager.PlaySound(SoundManager.Sound.Teleport, transform.position);
        yield return new WaitForSeconds(4f);

        GameController.instance.RecapturePlayerReference();
        GameController.instance.player.SetPosition(position);
        SceneManager.LoadSceneAsync(destination);
    }
}
