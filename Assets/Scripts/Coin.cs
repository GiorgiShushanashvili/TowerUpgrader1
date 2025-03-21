using UnityEngine;


[CreateAssetMenu(fileName = "CoinData", menuName = "ScriptableObjects/CoinData", order = 3)]
public class Coin:ScriptableObject
{
        [SerializeField] public float coinAmount;

        public void AddCoins(float amount)
        {
                coinAmount += amount;
        }

        public void ReduceCoins(float amount)
        {
                coinAmount -= amount;
        }
}