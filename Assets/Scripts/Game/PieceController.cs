using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PieceController : MonoBehaviour
{
    private PieceGenerator pieceGenerator;

    private int count = 0;

    private int column, row;

    public Vector2 myPreviousPos;

    private GameObject neighborCandy;

    public bool isMoving = false;

    public bool isMatching = false;

    private AudioSource audioSource;

    public AudioClip clip1, clip2;

    public GameObject Shade,moveCountTx;

    private int moveCount = 3;

    private GameMaster gameMaster;

    // Start is called before the first frame update
    void Start()
    {
        gameMaster = GameObject.FindWithTag("GameMaster").GetComponent<GameMaster>();

        pieceGenerator = FindObjectOfType<PieceGenerator>();
        row = (int)transform.position.x;
        column = (int)transform.position.y;
        myPreviousPos = new Vector2(row, column);
        audioSource = GetComponent<AudioSource>();
    }

    // Update is called once per frame
    void Update()
    {
        if (isMoving)
        {
            if (gameMaster.isPlaying && Input.GetKeyDown(KeyCode.W) || Input.GetKeyDown(KeyCode.UpArrow) && moveCount > 1)
            {
                audioSource.PlayOneShot(clip1);
                moveCount--;
                moveCountTx.GetComponent<Text>().text = moveCount.ToString();
                if (column <= 7)
                {
                    MoveCount();
                    //上のキャンディ情報を取得
                    neighborCandy = pieceGenerator.pieceArray[row,column + 1];
                    //隣のキャンディを１行下へ。
                    neighborCandy.GetComponent<PieceController>().column --;
                    //自身は１行上へ。
                    column++;
                    Invoke("DoCheckMatching", 0.3f);
                }
            }
            if (gameMaster.isPlaying && Input.GetKeyDown(KeyCode.A) || Input.GetKeyDown(KeyCode.LeftArrow) && moveCount > 1)
            {
                if (row > 0)
                    {
                        audioSource.PlayOneShot(clip1);
                        moveCount--;
                        moveCountTx.GetComponent<Text>().text = moveCount.ToString();
                        MoveCount();

                        //左隣りのキャンディ情報を取得
                        neighborCandy = pieceGenerator.pieceArray[row - 1, column];

                        //隣のキャンディを１列右へ。
                        neighborCandy.GetComponent<PieceController>().row ++;

                        //自身は１列左へ。
                        row--;


                        //マッチングチェック
                        Invoke("DoCheckMatching", 0.3f);
                }
            }
            if (gameMaster.isPlaying && Input.GetKeyDown(KeyCode.S) || Input.GetKeyDown(KeyCode.DownArrow) && moveCount > 1)
            {
                if (column > 0)
                    {
                        audioSource.PlayOneShot(clip1);
                        moveCount--;
                        moveCountTx.GetComponent<Text>().text = moveCount.ToString();
                        MoveCount();

                        //下のキャンディ情報を取得
                        neighborCandy = pieceGenerator.pieceArray[row, column - 1];

                        //隣のキャンディを１行上へ。
                        neighborCandy.GetComponent<PieceController>().column ++;

                        //自身は１行下へ。
                        column--;

                        //マッチングチェック
                        Invoke("DoCheckMatching", 0.3f);
                }
            }
            if (gameMaster.isPlaying && Input.GetKeyDown(KeyCode.D) || Input.GetKeyDown(KeyCode.RightArrow) && moveCount > 1)
            {
                if (row <= 7)
                    {
                        audioSource.PlayOneShot(clip1);
                        moveCount--;
                        moveCountTx.GetComponent<Text>().text = moveCount.ToString();
                        MoveCount();

                        //右隣りのキャンディ情報をneighborCandyに代入
                        neighborCandy = pieceGenerator.pieceArray[row + 1, column];

                        //隣のキャンディを１列左へ。
                        neighborCandy.GetComponent<PieceController>().row --;

                        row++;

                        //マッチングチェック
                        Invoke("DoCheckMatching", 0.3f);
                }
            }
        }

        if (transform.position.x != row || transform.position.y != column)
        {
            transform.position = Vector2.Lerp(transform.position, new Vector2(row, column), 0.3f);
            Vector2 dif = (Vector2)transform.position - new Vector2(row, column);

            SetCandyToArray();
            if (Mathf.Abs(dif.magnitude) < 0.1f)
            {
                transform.position = new Vector2(row, column);
            }
        }
        else if (column > 0 && pieceGenerator.pieceArray[row, column - 1] == null)
        {
            FallCandy();
        }
    }

    private void OnMouseDown()
    {
        if (!gameMaster.isHolding)
        {
            bool flag = GameObject.FindWithTag("Generator").GetComponent<PieceGenerator>().ChekingHolding();
            if (!flag)
            {
                isMoving = true;
                Shade.SetActive(true);
                moveCount = 3;
                moveCountTx.SetActive(true);
                moveCountTx.GetComponent<Text>().text = moveCount.ToString();
                gameMaster.isHolding = true;
            }
        }
    }

    void MoveCount()
    {
        count++;
        if (count > 2)
        {
            isMoving = false;
            Shade.SetActive(false);
            moveCountTx.SetActive(false);
            moveCount = 0;
            gameMaster.isHolding = false;
            count = 0;
        }
    }

    public void SetCandyToArray()

    {
        pieceGenerator.pieceArray[row, column] = gameObject;
    }

    void FallCandy()

    {
        //自分のいた配列を空にする
        pieceGenerator.pieceArray[row, column] = null;
        //自分を下に移動させる
        column -= 1;
    }

    void DoCheckMatching()
    {
        pieceGenerator.CheckMatching();
    }

    public void ReadyEnableMove()
    {
        isMoving = true;
    }

    public void disanableMove()
    {
        isMoving = false;
        Shade.SetActive(false);
        moveCountTx.GetComponent<Text>().text = "";
        moveCountTx.SetActive(false);
    }
}
