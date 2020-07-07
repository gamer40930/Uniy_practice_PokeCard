using UnityEngine;
using System.Collections.Generic; // List
using System.Linq;                // 查詢 使用linQ
using System.Collections;         // 協程

public class CardSystem : MonoBehaviour
{
    /// <summary>
    /// 撲克牌: 所有撲克 52 張
    /// </summary>
    public List<GameObject> cards = new List<GameObject>();

    /// <summary>
    /// 花色 : 黑桃、方塊、愛心、梅花
    /// </summary>
    private string[] type = { "Spades", "Diamond", "Heart", "Club" };

    private void Start()
    {
        GetAllCard();
    }


    /// <summary>
    /// 取得所有撲克牌
    /// </summary>
    void GetAllCard()

    {

        if (cards.Count == 52) return;        // 避免執行太多次 超出52張

        // 跑四個花色 (4 個花色 type)
        for (int i = 0; i < type.Length; i++)
        {
            // 跑 1 ~13 張
            for (int j = 1; j < 14; j++)
            {
                string number = j.ToString();   // 數字 = j.轉字串

                switch (j)
                {
                    case 1:
                        number = "A";          // 數字  1 改為 A
                        break;

                    case 11:
                        number = "J";          // 數字 11 改為 J
                        break;

                    case 12:
                        number = "Q";          // 數字 12 改為 Q
                        break;

                    case 13:
                        number = "K";          // 數字 13 改為 K
                        break;

                }

                // 卡牌 = 素材.載入遊戲 <遊戲物件>("素材名稱")
                // 素材名稱 : "playingCards_" (素材包檔名開頭的共同名字) + number ( 1~13) + i (四種花色) )
                GameObject _Cards =  Resources.Load<GameObject>("playingCards_" + number + type[i]);
                cards.Add(_Cards);
            }

        }

    }

    /// <summary>
    /// 透過花色選取卡片
    /// </summary>
    /// <param name="type"></param>
    public void ChooseCardByType(string type)  // 在按鈕設定欄位中輸入Heart，會搜尋包含Heart
    {
        DeletAllChild();                    // 刪除所有子物件

        // 暫存牌組 = 撲克牌.哪裡( (物件) => 物件.名稱.包含(花色) )
        var temp =  cards.Where ((x) => x.name.Contains(type));

        // 迴圈 遍巡 暫存排組 生成(卡牌，父物件)
        foreach (var item in temp) Instantiate(item, transform);


        StartCoroutine(setChildPosition());  // 啟動協程
    }


    /// <summary>
    /// 洗牌
    /// </summary>
    public void Shuffle()
    {
        DeletAllChild();

        // 參考類型 - ToArray() 轉為陣列實質類別 ToList() 轉回清單
        List<GameObject> shuffle = cards.ToArray().ToList();         // 另存一份洗牌用原始排組
        List<GameObject> newCards = new List<GameObject>();          // 儲存洗牌後的新牌組

        // 跑 52 張卡牌
        for (int i = 0; i < 52; i++)
        {

        int r = Random.Range(0, shuffle.Count);                      // 從洗牌用牌組隨機挑一張   

        GameObject temp = shuffle[r];                                // 挑出來的隨機卡牌   
        newCards.Add(temp);                                          // 添增到新牌組
        shuffle.RemoveAt(r);                                         // 刪除挑出來的卡牌

        }

        foreach (var item in newCards)
        {
            Instantiate(item, transform);
        }

        StartCoroutine(setChildPosition());


    }

    /// <summary>
    /// 排序、花色、數字由小到大
    /// </summary>
    public void Sort()
    {
        DeletAllChild();

        // 排序後的卡排 = 從 cards 找資料放到 card中
        // where 條件 - "Spades", "Diamond", "Heart", "Club"
        // select 選取 card

        var sort = from card in cards
                   where card.name.Contains(type[0]) || card.name.Contains(type[1]) || card.name.Contains(type[2]) || card.name.Contains(type[3])
                   select card;

        foreach (var item in sort)
        {
            Instantiate(item, transform);
        }

        StartCoroutine(setChildPosition());

    }

    /// <summary>
    /// 刪除所有子物件
    /// </summary>
    private void DeletAllChild()
    {
        for (int i = 0; i < transform .childCount; i++)
        {
            Destroy(transform.GetChild(i).gameObject);
        }
    }

    /// <summary>
    /// 設定子物件座標 : 排序、大小、角度
    /// </summary>
    /// <returns></returns>
    private IEnumerator setChildPosition()
    {
        yield return new WaitForSeconds(0.1f);                   // 避免刪除這次卡牌

        for (int i = 0; i < transform.childCount; i++)           // 迴圈執行每一個子物件
        {
            Transform child = transform.GetChild(i);             // 取得子物件
            child.eulerAngles = new Vector3(180, 0, 0);          // 設定角度
            child.localScale = Vector3.one  * 20;                // 設定尺寸 放大 20 倍

            // x = 1 % 13 (讓每13張從一開始 換排)
            // x軸 = (i-6) * 間距
            float x = i % 13;
            // y = i /13 取得每一排高度
            // 4-y * (間距)
            int y = i / 13;
            child.position = new Vector3((x - 6) * 1.3f, 4-y *2, 0);

            yield return null;

        }
    }
}
