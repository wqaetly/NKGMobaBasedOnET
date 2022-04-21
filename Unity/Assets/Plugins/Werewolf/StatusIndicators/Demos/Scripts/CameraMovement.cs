using UnityEngine;

namespace Werewolf.StatusIndicators.Demo {
  public class CameraMovement : MonoBehaviour {
    public GameObject player;
    public float offsetX = -5;
    public float offsetZ = 0;
    public float maximumDistance = 2;
    public float playerVelocity = 10;
 
    private float movementX;
    private float movementZ;

    void Update() {
      movementX = (player.transform.position.x + offsetX - transform.position.x) / maximumDistance; 
      movementZ = (player.transform.position.z + offsetZ - transform.position.z) / maximumDistance; 
      transform.position += new Vector3((movementX * playerVelocity * Time.deltaTime), 0, (movementZ * playerVelocity * Time.deltaTime)); 
    }
  }
}