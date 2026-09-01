using UnityEngine;
using TMPro;

public class Coin : MonoBehaviour
{
    public AudioClip coinClip;
    public int coinsToGive = 1;            // in the inspector we can modfiy coins to give as per goldcoin, silvercoin, bronzecoin
    private TextMeshProUGUI coinText;


    private void Start()
    {
        coinText = GameObject.FindWithTag("CoinText").GetComponent<TextMeshProUGUI>();  //Unity searches the scene for a GameObject with the tag "CoinText" and get its component 
    }
    private void OnTriggerEnter2D(Collider2D collision)            // onTrigger method is used for detecting overlaping
    {
        if(collision.gameObject.tag == "Player")
        {
            Player player = collision.gameObject.GetComponent<Player>();    // we got access to the player script here
            player.coins += coinsToGive;
            player.PlaySFX(coinClip,0.2f);
            coinText.text = player.coins.ToString();       // .text- this property takes string so that's why we converted coins from int to string
            Destroy(gameObject);
        }
    }

}
