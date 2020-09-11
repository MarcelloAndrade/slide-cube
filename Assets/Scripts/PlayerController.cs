using UnityEngine;
using UnityEngine.Animations;

public class PlayerController : MonoBehaviour {

    public float speed = 5f;
    public Transform moveToPoint;
    public LayerMask mapLimitLayer;
    public LayerMask squareLayer;
    public LayerMask checkPointLayer;

    //private Animator anim;
    private Player player;
    private AudioSource audioSource;

    private AudioClip clipMerge;
    private AudioClip clipBlock;

    private void Awake() {
        //anim = GetComponent<Animator>();
        SetPlayerAttributes(gameObject.tag);
        player = new Player();

        audioSource = GetComponent<AudioSource>();
        clipMerge = Resources.Load<AudioClip>("bubble_03");
        clipBlock = Resources.Load<AudioClip>("bubble_04");

    }

    private void Start() {
        moveToPoint.parent = null;
    }

    private void Update() {
        transform.position = Vector3.MoveTowards(transform.position, moveToPoint.position, speed * Time.deltaTime);

        if (Vector3.Distance(transform.position, moveToPoint.position) == 0) {
            Axis direction = Axis.None;
            float moveY = 0f;
            float moveX = 0f;
            if (Input.GetKeyDown(KeyCode.W) || Input.GetKeyDown(KeyCode.UpArrow)) {
                direction = Axis.Y;
                moveY = +11f;
            } else if (Input.GetKeyDown(KeyCode.S) || Input.GetKeyDown(KeyCode.DownArrow)) {
                direction = Axis.Y;
                moveY = -11f;
            } else if (Input.GetKeyDown(KeyCode.D) || Input.GetKeyDown(KeyCode.RightArrow)) {
                direction = Axis.X;
                moveX = +11f;
            } else if (Input.GetKeyDown(KeyCode.A) || Input.GetKeyDown(KeyCode.LeftArrow)) {
                direction = Axis.X;
                moveX = -11f;
            }

            if (direction != Axis.None) {
                while (GetNextPositionToMove(direction, moveY, moveX)) {
                }
            }
        }
    }

    // Check colider with mapLimitLayer or objectsLayer for stop player movement    
    private bool GetNextPositionToMove(Axis direction, float moveY, float moveX) {
        Vector3 moveTo = new Vector3();
        if (direction == Axis.X) {
            moveTo = moveToPoint.position + new Vector3(moveX, 0f, 0f);
            player.LastDirectionMovement = Axis.X;
            player.LastValueMovement = moveX;

        } else if (direction == Axis.Y) {
            moveTo = moveToPoint.position + new Vector3(0f, moveY, 0f);
            player.LastDirectionMovement = Axis.Y;
            player.LastValueMovement = moveY;
        }

        if (!Physics2D.OverlapCircle(moveTo, 0.2f, mapLimitLayer)) {
            // Check colider with checkPointLayer
            Collider2D checkPoint = Physics2D.OverlapCircle(moveTo, 0.2f, checkPointLayer);
            if (checkPoint != null && checkPoint.gameObject.CompareTag(gameObject.tag)) {
                moveToPoint.position = moveTo;
                return false;
            } else if (checkPoint != null && !checkPoint.gameObject.CompareTag(gameObject.tag)) {
                return false;
            }

            // Check colider with squareLayer
            Collider2D square = Physics2D.OverlapCircle(moveTo, 0.2f, squareLayer);
            if (square != null && square.gameObject.CompareTag(gameObject.tag)) {
                audioSource.PlayOneShot(clipBlock);
                return false;
            } else if (square != null && !square.gameObject.CompareTag(gameObject.tag)) {
                audioSource.PlayOneShot(clipMerge);
                moveToPoint.position = moveTo;
                return false;
            }

            moveToPoint.position = moveTo;
            return true;
        }

        return false;
    }

    private void OnTriggerEnter2D(Collider2D collider) {
        if (gameObject.CompareTag(collider.tag) && collider.gameObject.layer == 10) {
            Debug.Log("YOU WIN");
            return;
        }

        if (!gameObject.CompareTag(collider.tag) && collider.gameObject.layer == 9) {
            ChangePositionBetweenPlayerAndSquare(collider);
        }
        SetPlayerAttributes(collider.tag);
    }

    private void ChangePositionBetweenPlayerAndSquare(Collider2D collider) {
        if (player.LastDirectionMovement == Axis.X) {
            collider.transform.position += new Vector3(player.LastValueMovement * -1, 0f, 0f);
        } else if (player.LastDirectionMovement == Axis.Y) {
            collider.transform.position += new Vector3(0f, player.LastValueMovement * -1, 0f);
        }
    }

    private void SetPlayerAttributes(string tagName) {
        if (tagName.Equals("Blue")) {
            //anim.SetInteger("player-transforme", 0);
            gameObject.tag = "Blue";

        } else if (tagName.Equals("Green")) {
            //anim.SetInteger("player-transforme", 1);
            gameObject.tag = "Green";

        } else if (tagName.Equals("Orange")) {
            //anim.SetInteger("player-transforme", 2);
            gameObject.tag = "Orange";

        } else if (tagName.Equals("Pink")) {
            //anim.SetInteger("player-transforme", 3);
            gameObject.tag = "Pink";

        } else {
            //anim.SetInteger("player-transforme", 0);
            gameObject.tag = "Blue";
        }
    }

    class Player {
        public Axis LastDirectionMovement { get; set; }
        public float LastValueMovement { get; set; }
    }
}
