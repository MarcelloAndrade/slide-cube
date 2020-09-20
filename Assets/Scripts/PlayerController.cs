using UnityEngine;
using UnityEngine.Animations;

public class PlayerController : MonoBehaviour {

    public float speed = 5f;
    public Transform moveToPoint;
    public LayerMask mapLimitLayer;
    public LayerMask squareLayer;
    public LayerMask checkPointLayer;

    public Sprite spriteBlue;
    public Sprite spriteGreen;
    public Sprite spritePink;
    public Sprite spriteOrange;

    private GameManager gameManager;
    private Animator anim;
    private SpriteRenderer playerSpriteRenderer;

    // Mobile Touch
    private float fingerStartTime = 0.0f;
    private Vector2 fingerStartPos = Vector2.zero;
    private bool isSwipe = false;
    private float minSwipeDist = 50.0f;
    private float maxSwipeTime = 1.5f;

    private Player player;

    private void Awake() {
        gameManager = GameObject.FindObjectOfType<GameManager>();
        anim = GetComponent<Animator>();
        playerSpriteRenderer = gameObject.GetComponent<SpriteRenderer>();        
        SetPlayerAttributes(gameObject.tag);
        player = new Player();        
    }

    private void Start() {
        moveToPoint.parent = null;
    }

    private void Update() {
        if(!gameManager.pauseGame) {        
            transform.position = Vector3.MoveTowards(transform.position, moveToPoint.position, speed * Time.deltaTime);
            if (Vector3.Distance(transform.position, moveToPoint.position) < 0.3) {
                Axis direction = Axis.None;
                float moveY = 0f;
                float moveX = 0f;

    #if UNITY_EDITOR || UNITY_STANDALONE || UNITY_WEBPLAYE            
                if (Input.GetKeyDown(KeyCode.W) || Input.GetKeyDown(KeyCode.UpArrow)) {
                    direction = Axis.Y;
                    moveY = +1f;
                } else if (Input.GetKeyDown(KeyCode.S) || Input.GetKeyDown(KeyCode.DownArrow)) {
                    direction = Axis.Y;
                    moveY = -1f;
                } else if (Input.GetKeyDown(KeyCode.D) || Input.GetKeyDown(KeyCode.RightArrow)) {
                    direction = Axis.X;
                    moveX = +1f;
                } else if (Input.GetKeyDown(KeyCode.A) || Input.GetKeyDown(KeyCode.LeftArrow)) {
                    direction = Axis.X;
                    moveX = -1f;
                }
    #endif

    #if UNITY_IOS || UNITY_ANDROID            
                if (Input.touchCount > 0) {
                    foreach (Touch touch in Input.touches) {

                        switch (touch.phase) {
                            case TouchPhase.Began:
                                /* this is a new touch */
                                isSwipe = true;
                                fingerStartTime = Time.time;
                                fingerStartPos = touch.position;
                                break;

                            case TouchPhase.Canceled:
                                /* The touch is being canceled */
                                isSwipe = false;
                                break;

                            case TouchPhase.Ended:
                                float gestureTime = Time.time - fingerStartTime;
                                float gestureDist = (touch.position - fingerStartPos).magnitude;

                                if (isSwipe && gestureTime < maxSwipeTime && gestureDist > minSwipeDist) {
                                    Vector2 directionTouch = touch.position - fingerStartPos;
                                    Vector2 swipeType = Vector2.zero;

                                    if (Mathf.Abs(directionTouch.x) > Mathf.Abs(directionTouch.y)) {
                                        // the swipe is horizontal:
                                        swipeType = Vector2.right * Mathf.Sign(directionTouch.x);
                                    } else {
                                        // the swipe is vertical:
                                        swipeType = Vector2.up * Mathf.Sign(directionTouch.y);
                                    }

                                    if (swipeType.x != 0.0f) {                                    
                                        if (swipeType.x > 0.0f) {
                                            // MOVE RIGHT
                                            direction = Axis.X;
                                            moveX = +1f;
                                        } else {                                        
                                            // MOVE LEFT
                                            direction = Axis.X;
                                            moveX = -1f;
                                        }
                                    }

                                    if (swipeType.y != 0.0f) {
                                        if (swipeType.y > 0.0f) {
                                            // MOVE UP
                                            direction = Axis.Y;
                                            moveY = +1f;
                                        } else {                                        
                                            // MOVE DOWN
                                            direction = Axis.Y;
                                            moveY = -1f;
                                        }
                                    }

                                }

                                break;
                        }
                    }
                }
    #endif
                if (direction != Axis.None) {                
                    gameManager.activeTouchlUI = false;
                    while (GetNextPositionToMove(direction, moveY, moveX)) {
                    }
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
                GameSounds.PlayerSound(GameSounds.Sound.PlayerBlock);
                return false;
            }

            // Check colider with squareLayer
            Collider2D square = Physics2D.OverlapCircle(moveTo, 0.2f, squareLayer);
            if (square != null && square.gameObject.CompareTag(gameObject.tag)) {
                GameSounds.PlayerSound(GameSounds.Sound.PlayerBlock);
                return false;
            } else if (square != null && !square.gameObject.CompareTag(gameObject.tag)) {
                GameSounds.PlayerSound(GameSounds.Sound.PlayerChange);
                moveToPoint.position = moveTo;
                return false;
            }

            moveToPoint.position = moveTo;
            return true;
        } else {
            GameSounds.PlayerSound(GameSounds.Sound.PlayerMovement);
        }       
        return false;
    }

    private void OnTriggerEnter2D(Collider2D collider) {
        if (!gameObject.CompareTag(collider.tag) && collider.gameObject.layer == 9) {
            ChangePositionBetweenPlayerAndSquare(collider);
        }
        SetPlayerAttributes(collider.tag);
    }

    private void ChangePositionBetweenPlayerAndSquare(Collider2D collider) {
        collider.GetComponent<Animator>().SetTrigger("square-transforme");        
        if (player.LastDirectionMovement == Axis.X) {
            collider.transform.position += new Vector3(player.LastValueMovement * -1, 0f, 0f);
        } else if (player.LastDirectionMovement == Axis.Y) {
            collider.transform.position += new Vector3(0f, player.LastValueMovement * -1, 0f);
        }
    }

    private void SetPlayerAttributes(string tagName) {
        float intensity = 0.036f;
        
        if (tagName.Equals("Blue")) {
            anim.SetTrigger("player-transforme");
            playerSpriteRenderer.material.SetColor("_Color", new Color(0 * intensity, 92 * intensity, 191 * intensity, 0)); // blue color                        
            playerSpriteRenderer.sprite = spriteBlue;
            gameObject.tag = "Blue";

        } else if (tagName.Equals("Green")) {
            anim.SetTrigger("player-transforme");
            playerSpriteRenderer.material.SetColor("_Color", new Color(18 * intensity, 191 * intensity, 0, 0)); // green color
            playerSpriteRenderer.sprite = spriteGreen;
            gameObject.tag = "Green";

        } else if (tagName.Equals("Orange")) {
            anim.SetTrigger("player-transforme");
            playerSpriteRenderer.material.SetColor("_Color", new Color(191 * intensity, 51 * intensity, 0, 0)); // orange color
            playerSpriteRenderer.sprite = spriteOrange;            
            gameObject.tag = "Orange";

        } else if (tagName.Equals("Pink")) {
            anim.SetTrigger("player-transforme");
            playerSpriteRenderer.material.SetColor("_Color", new Color(191 * intensity, 0, 92 * intensity, 0)); // pink color
            playerSpriteRenderer.sprite = spritePink;            
            gameObject.tag = "Pink";

        } else {
            anim.SetTrigger("player-transforme");
            playerSpriteRenderer.material.SetColor("_Color", new Color(0 * intensity, 92 * intensity, 191 * intensity, 0)); // blue color                        
            playerSpriteRenderer.sprite = spriteBlue;
            gameObject.tag = "Blue";
        }
    }

    class Player {
        public Axis LastDirectionMovement { get; set; }
        public float LastValueMovement { get; set; }
    }
}