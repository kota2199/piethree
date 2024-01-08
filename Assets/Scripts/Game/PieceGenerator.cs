using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PieceGenerator : MonoBehaviour
{
    public GameObject[] pieces;

    [SerializeField]
    private int numOfLine = 9;

    public GameObject[,] pieceArray = new GameObject[9, 9];


    private List<GameObject> deleteList = new List<GameObject>();

    public bool movingFlag;

    private bool isStart;

    public GameMaster gameMaster;

    public AudioClip clip, clip2;

    private AudioSource audioSource;

    public GameObject GetParticle, LoseParticle;

    private ParticleSystem particle;

    private GameObject ParticleForDelete;

    public enum Version
    {
        Halloween, Xmas, Valentine
    }
    public Version ver;

    // Start is called before the first frame update
    void Start()
    {
        audioSource = GetComponent<AudioSource>();

        gameMaster = GameObject.FindWithTag("GameMaster").GetComponent<GameMaster>();

        //初期のピースをランダム配置
        for (int i = 0; i < 9; i++)
        {
            for (int j = 0; j < 9; j++)
            {
                int r = Random.Range(0, 5);
                GameObject piece = Instantiate(pieces[r]);
                piece.transform.position = new Vector2(i, j);
                pieceArray[i, j] = piece;
            }
        }
        CheckBeforeStart();
    }

    void CheckBeforeStart()
    {

        for (int i = 0; i < 9; i++)
        {
            //右から２つ目以降は確認不要（width-2）
            for (int j = 0; j < 9 - 2; j++)
            {
                //同じタグのキャンディが３つ並んでいたら。Ｘ座標がｊなので注意。
                if ((pieceArray[j, i].tag == pieceArray[j + 1, i].tag) && (pieceArray[j, i].tag == pieceArray[j + 2, i].tag))
                {
                    pieceArray[j, i].GetComponent<PieceController>().isMatching = true;
                    pieceArray[j + 1, i].GetComponent<PieceController>().isMatching = true;
                    pieceArray[j + 2, i].GetComponent<PieceController>().isMatching = true;
                }
            }
        }

        //左の列からタテのつながりを確認

        for (int i = 0; i < 9; i++)
        {
            //上から２つ目以降は確認不要。height-2
            for (int j = 0; j < 9 - 2; j++)
            {
                if ((pieceArray[i, j].tag == pieceArray[i, j + 1].tag) && (pieceArray[i, j].tag == pieceArray[i, j + 2].tag))
                {
                    pieceArray[i, j].GetComponent<PieceController>().isMatching = true;
                    pieceArray[i, j + 1].GetComponent<PieceController>().isMatching = true;
                    pieceArray[i, j + 2].GetComponent<PieceController>().isMatching = true;
                }
            }
        }

        //isMatching=trueのものをListに入れる
        foreach (var item in pieceArray)
        {
            item.GetComponent<PieceController>().moveCountTx.SetActive(false);
            if (item.GetComponent<PieceController>().isMatching)
            {
                deleteList.Add(item);
            }
        }

        //List内にピースがある場合
        if (deleteList.Count > 0)
        {
            //該当する配列をnullにして（内部管理）、キャンディを消去する（見た目）。
            foreach (var item in deleteList)
            {
                pieceArray[(int)item.transform.position.x, (int)item.transform.position.y] = null;
                Destroy(item);
                gameMaster.isHolding = false;
            }

            //Listを空っぽに。
            deleteList.Clear();

            //空欄に新しいキャンディを入れる。
            Invoke("SpawnNewCandy", 0.5f);
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
                if ((pieceArray[j, i].tag == pieceArray[j + 1, i].tag) && (pieceArray[j, i].tag == pieceArray[j + 2, i].tag))
                {
                    pieceArray[i, j + 2].GetComponent<PieceController>().disanableMove();
                    pieceArray[j, i].GetComponent<PieceController>().isMatching = true;
                    pieceArray[j + 1, i].GetComponent<PieceController>().isMatching = true;
                    pieceArray[j + 2, i].GetComponent<PieceController>().isMatching = true;
                }
            }
        }

        //左の列からタテのつながりを確認
        for (int i = 0; i < 9; i++)
        {
            for (int j = 0; j < 7; j++)
            {
                if ((pieceArray[i, j].tag == pieceArray[i, j + 1].tag) && (pieceArray[i, j].tag == pieceArray[i, j + 2].tag))
                {
                    pieceArray[i, j + 2].GetComponent<PieceController>().disanableMove();
                    pieceArray[i, j].GetComponent<PieceController>().isMatching = true;
                    pieceArray[i, j + 1].GetComponent<PieceController>().isMatching = true;
                    pieceArray[i, j + 2].GetComponent<PieceController>().isMatching = true;
                }
            }
        }

        //isMatching=trueのものをListに入れる
        foreach (var item in pieceArray)
        {
            if (item.GetComponent<PieceController>().isMatching)
            {
                ////３つ以上そろったとき、キャンディを半透明にする。
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
            }
        }
    }

    void DeleteCandies()
    {
        if (gameMaster.isPlaying)
        {
            //List内のキャンディを消去。かつ、その配列をnullに。
            foreach (var item in deleteList)
            {
                item.GetComponent<PieceController>().ReadyEnableMove();
                gameMaster.isHolding = false;
                pieceArray[(int)item.transform.position.x, (int)item.transform.position.y] = null;
                if (item.gameObject.tag == "Piece_5")
                {
                    gameMaster.LoseScore();
                    ParticleForDelete = Instantiate(LoseParticle, item.transform.position, Quaternion.identity);
                    particle = ParticleForDelete.GetComponent<ParticleSystem>();
                    audioSource.PlayOneShot(clip2);
                    if (ver.ToString() == "Xmas" || ver.ToString() == "Valentine")
                    {
                        gameMaster.itemCountMinus();
                    }
                }
                else
                {
                    gameMaster.GetScore();
                    ParticleForDelete = Instantiate(GetParticle, item.transform.position, Quaternion.identity);
                    particle = ParticleForDelete.GetComponent<ParticleSystem>();
                    audioSource.PlayOneShot(clip);
                    if (ver.ToString() == "Xmas" || ver.ToString() == "Valentine")
                    {
                        gameMaster.ItemCountPlus();
                    }
                }
                Destroy(item);
            }
            //Listを空っぽに。
            particle.Play();
            Invoke("DeleteParticle", 3f);
            deleteList.Clear();
            //キャンディの落下を待って、空欄に新しいキャンディを入れる。
            Invoke("SpawnNewCandy", 0.5f);
        }
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
            CheckBeforeStart();
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

            CheckMatching();

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
            }
        }
    }
    void DeleteCandies_Item()
    {
        //List内のキャンディを消去。かつ、その配列をnullに。
        foreach (var item in deleteList)
        {
            item.GetComponent<PieceController>().ReadyEnableMove();
            gameMaster.isHolding = false;
            pieceArray[(int)item.transform.position.x, (int)item.transform.position.y] = null;
            gameMaster.GetScore();
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
        Invoke("SpawnNewCandy", 0.5f);
    }
}