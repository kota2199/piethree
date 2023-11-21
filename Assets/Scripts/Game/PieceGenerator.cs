using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PieceGenerator : MonoBehaviour
{
    public GameObject[] pieces;

    GameObject basePoint;

    GameObject clickedGameObject;

    int row, column = 0;

    public GameObject[,] pieceArray = new GameObject[9, 9];

    Vector3 piece_pos;

    private List<GameObject> deleteList = new List<GameObject>();


    private List<GameObject> AlldeleteList = new List<GameObject>();

    private bool isStart;

    public GameObject gameMaster;

    public AudioClip clip, clip2;

    AudioSource audioSource;

    public bool movingFlag;

    public GameObject GetParticle, LoseParticle;

    ParticleSystem particle;

    GameObject ParticleForDelete;

    public enum Version
    {
        Halloween, Xmas, Valentine
    }
    public Version ver;

    // Start is called before the first frame update
    void Start()
    {
        audioSource = GetComponent<AudioSource>();
        //初期のピースをランダム配置
        for (int i = 0; i < 9; i++)
        {
            for (int j = 0; j < 9; j++)
            {
                int r = Random.Range(0, 5);
                var piece = Instantiate(pieces[r]);
                piece.transform.position = new Vector2(i, j);
                pieceArray[i, j] = piece;
            }
        }
        CheckStartset();
    }

    // Update is called once per frame
    void Update()
    {
        Debug.Log(isStart);
    }
    void CheckStartset()
    {
        for (int i = 0; i < 9; i++)
        {
            //i == y
            //右から２つ目以降は確認不要（width-2）
            for (int j = 0; j < 7; j++)
            {
                //j == x
                //同じタグのキャンディが３つ並んでいたら。Ｘ座標がｊなので注意。
                if ((pieceArray[j, i].tag == pieceArray[j + 1, i].tag) && (pieceArray[j, i].tag == pieceArray[j + 2, i].tag))
                {
                    //CandyのisMatchingをtrueに
                    pieceArray[j, i].GetComponent<PieceController>().isMatching = true;
                    pieceArray[j + 1, i].GetComponent<PieceController>().isMatching = true;
                    pieceArray[j + 2, i].GetComponent<PieceController>().isMatching = true;
                }
            }
        }//

        //左の列からタテのつながりを確認

        for (int i = 0; i < 9; i++)
        //i == x
        {
            //上から２つ目以降は確認不要。height-2
            for (int j = 0; j < 7; j++)
            //j == y
            {
                //Ｙ座標がｊ。
                if ((pieceArray[i, j].tag == pieceArray[i, j + 1].tag) && (pieceArray[i, j].tag == pieceArray[i, j + 2].tag))
                {
                    pieceArray[i, j].GetComponent<PieceController>().isMatching = true;
                    pieceArray[i, j + 1].GetComponent<PieceController>().isMatching = true;
                    pieceArray[i, j + 2].GetComponent<PieceController>().isMatching = true;
                }
            }
        }
        //isMatching=trueのものをＬｉｓｔに入れる

        foreach (var item in pieceArray)
        {
            item.GetComponent<PieceController>().moveCountTx.SetActive(false);
            if (item.GetComponent<PieceController>().isMatching)
            {
                deleteList.Add(item);
            }
        }

        //List内にキャンディがある場合
        if (deleteList.Count > 0)
        {
            //該当する配列をnullにして（内部管理）、キャンディを消去する（見た目）。
            foreach (var item in deleteList)
            {
                pieceArray[(int)item.transform.position.x, (int)item.transform.position.y] = null;
                Destroy(item);
                GameObject.FindWithTag("GameMaster").GetComponent<GameMaster>().isHolding = false;
            }

            //Listを空っぽに。

            deleteList.Clear();

            //空欄に新しいキャンディを入れる。

            Invoke("SpawnNewCandy", 1f);
            //SpawnNewCandy();
        }
        foreach (var item in pieceArray)

        {

            if (item.GetComponent<PieceController>().isMatching)

            {

                deleteList.Add(item);

            }

        }
        //List内にキャンディがある場合

        if (deleteList.Count > 0)

        {
            //該当する配列をnullにして（内部管理）、キャンディを消去する（見た目）。

            foreach (var item in deleteList)

            {
                pieceArray[(int)item.transform.position.x, (int)item.transform.position.y] = null;

                Destroy(item);
                GameObject.FindWithTag("GameMaster").GetComponent<GameMaster>().isHolding = false;
            }
            foreach (var item in pieceArray)
            {
                item.GetComponent<PieceController>().disanableMove();
            }

            //Listを空っぽに。
            deleteList.Clear();

            //空欄に新しいキャンディを入れる。
            Invoke("SpawnNewCandy", 0.5f);
            //SpawnNewCandy();
        }
        else
        {
            isStart = true;
        }
    }

    //プレイ中のジャッジ
    public void CheckMatching()
    {
        //下の行からヨコのつながりを確認
        for (int i = 0; i < 9; i++)
        {
            //右から２つ目以降は確認不要
            for (int j = 0; j < 7; j++)
            {
                //同じタグのキャンディが３つ並んでいたら。Ｘ座標がｊ。
                if ((pieceArray[j, i].tag == pieceArray[j + 1, i].tag) && (pieceArray[j, i].tag == pieceArray[j + 2, i].tag))
                {
                    pieceArray[i, j + 2].GetComponent<PieceController>().disanableMove();
                    //CandyのisMatchingをtrueに
                    pieceArray[j, i].GetComponent<PieceController>().isMatching = true;
                    pieceArray[j + 1, i].GetComponent<PieceController>().isMatching = true;
                    pieceArray[j + 2, i].GetComponent<PieceController>().isMatching = true;
                }
            }
        }

        //左の列からタテのつながりを確認

        for (int i = 0; i < 9; i++)
        {
            //上から２つ目以降は確認不要。
            for (int j = 0; j < 7; j++)
            {
                //Ｙ座標がｊ。
                if ((pieceArray[i, j].tag == pieceArray[i, j + 1].tag) && (pieceArray[i, j].tag == pieceArray[i, j + 2].tag))
                {
                    pieceArray[i, j + 2].GetComponent<PieceController>().disanableMove();
                    pieceArray[i, j].GetComponent<PieceController>().isMatching = true;
                    pieceArray[i, j + 1].GetComponent<PieceController>().isMatching = true;
                    pieceArray[i, j + 2].GetComponent<PieceController>().isMatching = true;
                }
            }
        }

        //isMatching=trueのものをＬｉｓｔに入れる

        foreach (var item in pieceArray)
        {
            //item.GetComponent<PieceController>().moveCountTx.SetActive(false);
            if (item.GetComponent<PieceController>().isMatching)
            {
                ////３つ以上そろったとき、キャンディを半透明にする。
                //item.GetComponent<SpriteRenderer>().color = new Color(1, 1, 1, 0.5f);
                deleteList.Add(item);
            }
        }

        //List内にキャンディがある場合
        if (deleteList.Count > 0)
        {
            //キャンディを消去するとき、一瞬の間を持たせるためIvoke関数にする。
            Invoke("DeleteCandies", 0.05f);
            foreach (var item in pieceArray)
            {
                item.GetComponent<PieceController>().disanableMove();
                //effect
            }
        }
    }

    void DeleteCandies()
    {
        //List内のキャンディを消去。かつ、その配列をnullに。
        foreach (var item in deleteList)
        {
            item.GetComponent<PieceController>().ReadyEnableMove();
            GameObject.FindWithTag("GameMaster").GetComponent<GameMaster>().isHolding = false;
            pieceArray[(int)item.transform.position.x, (int)item.transform.position.y] = null;
            if (item.gameObject.tag == "Piece_5")
            {
                gameMaster.GetComponent<GameMaster>().LoseScore();
                ParticleForDelete = Instantiate(LoseParticle, item.transform.position, Quaternion.identity);
                particle = ParticleForDelete.GetComponent<ParticleSystem>();
                audioSource.PlayOneShot(clip2);
                if (ver.ToString() == "Xmas" || ver.ToString() == "Valentine")
                {
                    GameObject.FindWithTag("GameMaster").GetComponent<GameMaster>().itemCountMinus();
                }
            }
            else
            {
                gameMaster.GetComponent<GameMaster>().GetScore();
                ParticleForDelete = Instantiate(GetParticle, item.transform.position, Quaternion.identity);
                particle = ParticleForDelete.GetComponent<ParticleSystem>();
                audioSource.PlayOneShot(clip);
                if (ver.ToString() == "Xmas" || ver.ToString() == "Valentine")
                {
                    GameObject.FindWithTag("GameMaster").GetComponent<GameMaster>().ItemCountPlus();
                }
            }
            Destroy(item);
        }
        //Listを空っぽに。
        particle.Play();
        Invoke("DeleteParticle", 3f);
        deleteList.Clear();
        //キャンディの落下を待って、空欄に新しいキャンディを入れる。
        Invoke("SpawnNewCandy", 1f);
    }

    public bool ChekingHolding()
    {
        movingFlag = false;
        foreach (var item in pieceArray)
        {
            bool isholding = item.GetComponent<PieceController>().isMoving;
            if (isholding)
            {
                movingFlag = true;
            }
            else
            {
                movingFlag = false;
            }
        }
        return movingFlag;
    }
    void DeleteParticle()
    {
        Destroy(particle);
    }


    void SpawnNewCandy()

    {
        for (int i = 0; i < 9; i++)
        {
            for (int j = 0; j < 9; j++)
            {
                if (pieceArray[i, j] == null)
                {
                    int r = Random.Range(0, 5);
                    var piece = Instantiate(pieces[r]);
                    //見た目の処理
                    piece.transform.position = new Vector2(i, j + 0.3f);
                    //内部管理の処理
                    pieceArray[i, j] = piece;
                }
            }
        }
        if (isStart == false)
        {
            CheckStartset();
        }
        else //isStart==trueのとき。
        {
            //新しい位置をmyPreviousPosに設定

            foreach (var item in pieceArray)
            {
                int column = (int)item.transform.position.x;
                int row = (int)item.transform.position.y;
                item.GetComponent<PieceController>().myPreviousPos = new Vector2(column, row);
            }

            //続けざまに３つそろっているかどうか判定。

            Invoke("CheckMatching", 0.2f);

        }
    }

    public void AllDeleteItem()
    {
        foreach (var item in pieceArray)
        {
            deleteList.Add(item);
        }

        //List内にキャンディがある場合
        if (deleteList.Count > 0)
        {
            //キャンディを消去するとき、一瞬の間を持たせるためIvoke関数にする。
            Invoke("DeleteCandies_Item", 0.05f);
            foreach (var item in pieceArray)
            {
                item.GetComponent<PieceController>().disanableMove();
                //effect
            }
        }
    }
    void DeleteCandies_Item()
    {
        //List内のキャンディを消去。かつ、その配列をnullに。
        foreach (var item in deleteList)
        {
            item.GetComponent<PieceController>().ReadyEnableMove();
            GameObject.FindWithTag("GameMaster").GetComponent<GameMaster>().isHolding = false;
            pieceArray[(int)item.transform.position.x, (int)item.transform.position.y] = null;
                gameMaster.GetComponent<GameMaster>().GetScore();
                ParticleForDelete = Instantiate(GetParticle, item.transform.position, Quaternion.identity);
                particle = ParticleForDelete.GetComponent<ParticleSystem>();
                audioSource.PlayOneShot(clip);


            Destroy(item);
        }
        //Listを空っぽに。
        particle.Play();
        Invoke("DeleteParticle", 3f);
        deleteList.Clear();
        //キャンディの落下を待って、空欄に新しいキャンディを入れる。
        Invoke("SpawnNewCandy", 1f);
    }
}