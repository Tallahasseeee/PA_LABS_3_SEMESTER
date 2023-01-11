using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class Thirteen : MonoBehaviour
{

    [SerializeField] public Sprite[] sprites;
    [SerializeField] public Transform dice1;
    [SerializeField] public Transform dice2;
    [SerializeField] TextMeshProUGUI playerSumText;
    [SerializeField] TextMeshProUGUI AISumText;
    [SerializeField] TextMeshProUGUI playerScoreText;
    [SerializeField] TextMeshProUGUI AIScoreText;
    public int playerSum;
    public int AISum;
    public int playerScore;
    public int AIScore;
    public int dice1num;
    public int dice2num;
    public int playerIterations;
    public int AIIterations;
    public bool canMakeMovePlayer;
    public bool canMakeMoveAI;
    // Start is called before the first frame update
    void Start()
    {
        AIIterations = 0;
        playerIterations = 0;
        playerScore = 0;
        AIScore = 0;
        playerSum = 0;
        AISum = 0;
        canMakeMovePlayer = true;
        canMakeMoveAI = false;
        UpdateScore(true);
        UpdateScore(false);
        UpdateSum(true);
        UpdateSum(false);
    }

    // Update is called once per frame
    void Update()
    {
        
    }



    public int AI(int diceNum, int iterations, int sum, int score)
    {
        int a = Minimax(diceNum, iterations + 1, sum + diceNum, sum % 13 == 0 ? sum / 13 : score);
        int b = Minimax(diceNum, iterations + 1, sum - diceNum, sum % 13 == 0 ? sum / 13 : score);
        int c = Minimax(diceNum, iterations + 1, sum + 2 * diceNum, sum % 13 == 0 ? sum / 13 : score);
        int d = Minimax(diceNum, iterations + 1, sum + 3 * diceNum, sum % 13 == 0 ? sum / 13 : score);
        int e = Minimax(diceNum, iterations + 1, sum + 4 * diceNum, sum % 13 == 0 ? sum / 13 : score);
        int f = Minimax(diceNum, iterations + 1, sum + diceNum / 4, sum % 13 == 0 ? sum / 13 : score);
        int g = Minimax(diceNum, iterations + 1, sum + diceNum / 3, sum % 13 == 0 ? sum / 13 : score);
        int h = Minimax(diceNum, iterations + 1, sum + diceNum / 2, sum % 13 == 0 ? sum / 13 : score);

        int max = Max(a, b, c, d, e, f, g, h);
        if(a == max)
        {
            return 1;
        }
        else if(b == max)
        {
            return 2;
        }
        else if (c == max)
        {
            return 3;
        }
        else if (d == max)
        {
            return 4;
        }
        else if (e == max)
        {
            return 5;
        }
        else if (f == max)
        {
            return 6;
        }
        else if (g == max)
        {
            return 7;
        }
        else if (h == max)
        {
            return 8;
        }
        return 0;
    }

    public int Minimax(int diceNum, int iterations, int sum, int score)
    {
        if(iterations >= 5)
        {
            return sum % 13 == 0 ? sum / 13 : score;
        }

        int max = -1;

        for (int i = 3; i < 14; i++)
        {
            int a = Minimax(i, iterations + 1, sum + diceNum, sum % 13 == 0 ? sum / 13 : score);
            int b = Minimax(i, iterations + 1, sum - diceNum, sum % 13 == 0 ? sum / 13 : score);
            int c = Minimax(i, iterations + 1, sum + 2*diceNum, sum % 13 == 0 ? sum / 13 : score);
            int d = Minimax(i, iterations + 1, sum + 3*diceNum, sum % 13 == 0 ? sum / 13 : score);
            int e = Minimax(i, iterations + 1, sum + 4*diceNum, sum % 13 == 0 ? sum / 13 : score);
            int f = Minimax(i, iterations + 1, sum + diceNum / 4, sum % 13 == 0 ? sum / 13 : score);
            int g = Minimax(i, iterations + 1, sum + diceNum/3, sum % 13 == 0 ? sum / 13 : score);
            int h = Minimax(i, iterations + 1, sum + diceNum/2, sum % 13 == 0 ? sum / 13 : score);
            int m = Max(a, b, c, d, e, f, g, h);
            if(m > max)
            {
                max = m;
            }
        }

        return max;

    }

    public int Max(int a, int b, int c, int d, int e, int f, int g, int h)
    {
        List<int> list = new List<int>();
        list.Add(a);
        list.Add(b);
        list.Add(c);
        list.Add(d);
        list.Add(e);
        list.Add(f);
        list.Add(g);
        list.Add(h);

        int max = -1;

        for (int i = 0; i < list.Count; i++)
        {
            if(list[i] > max)
            {
                max = list[i];
            }
        }

        return max;
    }

    public void RollTheDice(bool isPlayer)
    {
        if (isPlayer && canMakeMovePlayer || !isPlayer && canMakeMoveAI)
        {
            System.Random rnd = new System.Random();
            dice1num = rnd.Next(1, 7);
            dice2num = rnd.Next(2, 8);

            dice1.GetComponent<SpriteRenderer>().sprite = sprites[dice1num - 1];
            dice2.GetComponent<SpriteRenderer>().sprite = sprites[dice2num - 1];


            
            if (isPlayer)
            {
                playerIterations++;
                canMakeMovePlayer = false;
            }
            else
            {
                AIIterations++;
                canMakeMoveAI = false;
            }
        }

    }

    public void AITurn()
    {
        while(AIIterations < 5)
        {
            Debug.Log(AIIterations);
            RollTheDice(false);
            int command = AI(dice1num + dice2num, AIIterations, AISum, AIScore);
            if(command == 1)
            {
                AddSum(false);
            }
            else if(command == 2)
            {
                SubtractSum(false);
            }
            else if (command == 3)
            {
                MultiplyByTwo(false);
            }
            else if (command == 4)
            {
                MultiplyByThree(false);
            }
            else if (command == 5)
            {
                MultiplyByFour(false);
            }
            else if (command == 6)
            {
                TakeQuarter(false);
            }
            else if (command == 7)
            {
                TakeThird(false);
            }
            else if (command == 8)
            {
                TakeHalf(false);
            }
        }
    }

    public void UpdateSum(bool isPlayer)
    {
        if (isPlayer)
        {
            playerSumText.text = "Player sum is " + playerSum.ToString();
        }
        else if(!isPlayer)
        {
            AISumText.text = "AI sum is " + AISum.ToString();
        }
    }

    public void UpdateScore(bool isPlayer)
    {
        if (isPlayer)
        {
            playerScoreText.text = "Player score is " + playerScore.ToString();
        }
        else if(!isPlayer && !canMakeMoveAI)
        {
            AIScoreText.text = "AI score is " + AIScore.ToString();
        }
    }

    public void AddSum(bool isPlayer)
    {
        if (isPlayer && !canMakeMovePlayer && playerIterations < 5)
        {
            playerSum += dice1num + dice2num;
            canMakeMovePlayer = true;
        }
        else if(!isPlayer && !canMakeMoveAI && AIIterations < 5)
        {
            AISum += dice1num + dice2num;
            canMakeMoveAI = true;
        }

        ManageScore(isPlayer);

    }

    public void SubtractSum(bool isPlayer)
    {
        if (isPlayer && !canMakeMovePlayer && playerIterations < 5)
        {
            playerSum -= dice1num + dice2num;
            canMakeMovePlayer = true;
        }
        else if(!isPlayer && !canMakeMoveAI && AIIterations < 5)
        {
            AISum -= dice1num + dice2num;
            canMakeMoveAI = true;
        }
        ManageScore(isPlayer);
    }

    public void MultiplyByTwo(bool isPlayer) 
    {
        if (isPlayer && !canMakeMovePlayer && playerIterations < 5)
        {
            playerSum += (dice1num + dice2num) * 2;
            canMakeMovePlayer = true;
        }
        else if(!isPlayer && !canMakeMoveAI && AIIterations < 5)
        {
            AISum += (dice1num + dice2num) * 2;
            canMakeMoveAI = true;
        }
        ManageScore(isPlayer);
    }

    public void MultiplyByThree(bool isPlayer)
    {
        if (isPlayer && !canMakeMovePlayer && playerIterations < 5)
        {
            playerSum += (dice1num + dice2num) * 3;
            canMakeMovePlayer = true;
        }
        else if(!isPlayer && !canMakeMoveAI && AIIterations < 5)
        {
            AISum += (dice1num + dice2num) * 3;
            canMakeMoveAI = true;
        }
        ManageScore(isPlayer);
    }

    public void MultiplyByFour(bool isPlayer)
    {
        if (isPlayer && !canMakeMovePlayer && playerIterations < 5)
        {
            playerSum += (dice1num + dice2num) * 4;
            canMakeMovePlayer = true;
        }
        else if(!isPlayer && !canMakeMoveAI && AIIterations < 5)
        {
            AISum += (dice1num + dice2num) * 4;
            canMakeMoveAI = true;
        }
        ManageScore(isPlayer);
    }

    public void TakeQuarter(bool isPlayer)
    {
        if (isPlayer && !canMakeMovePlayer && playerIterations < 5)
        {
            playerSum += (dice1num + dice2num) / 4;
            canMakeMovePlayer = true;
        }
        else if(!isPlayer && !canMakeMoveAI && AIIterations < 5)
        {
            AISum += (dice1num + dice2num) / 4;
            canMakeMoveAI = true;
        }

        ManageScore(isPlayer);

    }

    public void TakeThird(bool isPlayer)
    {
        if (isPlayer && !canMakeMovePlayer && playerIterations < 5)
        {
            playerSum += (dice1num + dice2num) / 3;
            canMakeMovePlayer = true;
        }
        else if (!isPlayer && !canMakeMoveAI && AIIterations < 5)
        {
            AISum += (dice1num + dice2num) / 3;
            canMakeMoveAI = true;
        }

        ManageScore(isPlayer);

    }
    public void TakeHalf(bool isPlayer)
    {
        if (isPlayer && !canMakeMovePlayer && playerIterations < 5)
        {
            playerSum += (dice1num + dice2num) / 2;
            canMakeMovePlayer = true;
        }
        else if (!isPlayer && !canMakeMoveAI && AIIterations < 5)
        {
            AISum += (dice1num + dice2num) / 2;
            canMakeMoveAI = true;
        }

        ManageScore(isPlayer);

    }

    public void ManageScore(bool isPlayer)
    {
        if (isPlayer)
        {
            if (playerSum % 13 == 0)
            {
                playerScore = playerSum/13;
            }
        }
        else
        {
            if (AISum % 13 == 0)
            {
                AIScore = AISum / 13;
            }
        }

        if(playerIterations == 5)
        {
            canMakeMovePlayer = false;
            AITurn();
        }

        if(AIIterations == 5)
        {
            canMakeMoveAI = false;
        }

        UpdateSum(isPlayer);
        UpdateScore(isPlayer);

    }

}
