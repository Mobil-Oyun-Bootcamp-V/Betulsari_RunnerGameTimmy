using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using UnityEngine.UIElements;

public class PlayerController : MonoBehaviour
{
    [SerializeField] private float _firstLine;
    [SerializeField] private float _secondLine;
    [SerializeField] private float _thirdLine;

    [SerializeField] private float _moveTreshold;
    [SerializeField] private float _speed;
    [SerializeField] private float _moveSpeed;

    private float _lastMoveTime;
    private Rigidbody _rigidbody;
    private bool hitted;
    
    Animator animator { get {return GetComponent<Animator>(); } }
    int score, hscore;
    public Text scoreText, hScoreText;
    public Transform rayOrigin;
    public ParticleSystem effectPrefab;
    GameManager gameManager { get {return FindObjectOfType<GameManager>();}}

    public GameObject FinishPanel;



    enum Lane
    {
        First,
        Second,
        Third
    }

    private Lane _lane = Lane.Second;
    
    Vector3 moveTo;

    private void Start()
    {
        animator.SetTrigger("gameStarted");
    }
    
    Vector3 effectPos;

    private void Awake()
    {
        _rigidbody = GetComponent<Rigidbody>();
    }

    private void Update()
    {
        if (Input.touchCount > 0)
        {
            Touch touch = Input.touches[0];
            float movePos = touch.deltaPosition.normalized.x;
            if (Math.Abs(movePos) > _moveTreshold && Time.time-_lastMoveTime > 0.5f)
            {
                _lastMoveTime = Time.time;
                if (movePos < 0)
                {
                    switch (_lane)
                    {
                        case Lane.First:
                            break;

                        case Lane.Second:
                            moveTo = new Vector3(_firstLine, 0, transform.position.z);
                            _lane = Lane.First;
                            break;

                        case Lane.Third:
                            //transform.position += new Vector3(_secondLine, 0, 0);
                            moveTo = new Vector3(_secondLine, 0, 0);
                            _lane = Lane.Second;
                            break;
                    }
                }

                if (movePos > 0)
                {
                    switch (_lane)
                    {
                        case Lane.First:
                            moveTo = new Vector3(_secondLine, 0, 0);
                            _lane = Lane.Second;
                            break;

                        case Lane.Second: 
                            moveTo = new Vector3(_thirdLine, 0, 0);
                            _lane = Lane.Third;
                            break;

                        case Lane.Third:
                            break;

                    }
                }
            }
        }
        Move(moveTo);
    }

    private void FixedUpdate()
    {
        if (!hitted)
        {
            gameManager.StartGame();
            _rigidbody.velocity = transform.forward * (Time.deltaTime * _moveSpeed);
        } 
    }

    private void OnTriggerEnter(Collider other)
    {
        Destroy(other.gameObject);

        if(other.tag.Equals("crystal"))
        {
            effectPos=other.transform.position;
            MakeScore();
            other.gameObject.SetActive(false);
            MakeEffect();
        }
    }
    
    private void MakeScore()
    {
        score++;
        scoreText.text=score.ToString();
        if(score > hscore)
        {
            hscore=score;
            hScoreText.text=hscore.ToString();
            PlayerPrefs.SetInt("myhscore", hscore);
        }
    }
    
    
    private void MakeEffect()
    {
        var effect = Instantiate(effectPrefab,effectPos, Quaternion.identity);
        Destroy(effect.gameObject, 1f);
    }

    private void OnCollisionEnter(Collision other)
    {
        hitted = true;
        score--;
        scoreText.text=score.ToString();

        switch (_lane)
        {
            case Lane.First:
                moveTo = new Vector3(_secondLine, 0, 0);
                _lane = Lane.Second;
                break;

            case Lane.Second:
                if (UnityEngine.Random.Range(0f, 1f) > 0.5f)
                {
                    moveTo = new Vector3(_firstLine, 0, 0);
                    _lane = Lane.First;
                }
                else
                {
                    moveTo = new Vector3(_thirdLine, 0, 0);
                    _lane = Lane.Third;
                }
                break;
            
            case Lane.Third:
                moveTo = new Vector3(_secondLine, 0, 0);
                _lane = Lane.Second;
                break;
        }

        if (other.gameObject.tag == "FinishArea")
        {
            Debug.Log("Finish Area ! ");
            animator.SetTrigger("gameStopped");
            _rigidbody.velocity = transform.forward * 0;
            
            FinishPanel.SetActive(true);
        }
    }

    private void Move(Vector3 moveTo)
    {
        moveTo = new Vector3(moveTo.x, 0, transform.position.z);
        transform.position = Vector3.MoveTowards(transform.position, moveTo, Time.deltaTime * _speed);
        hitted = false;
    }
}