using assemblyCsharp;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WorldBank
{
    // The index is the index of the nation; the value is the number of 20 coin deposits the nation has made in the World Bank 
    private List<int> deposits = new List<int>();
    private List<int> loans = new List<int>();
    private List<bool> bankruptThisTurn = new List<bool>();

    private int totalDeposits;
    private int totalLoans;
    private float interestCollected;

    // Interest Divisor Factor - increasing this value would result in lower interest rate, decreasing it would result in 
    // higher interest rates. It is also the multiple at which the Bank can lend our money before interest reaches 100%. 
    private int IDF = 10;
    private int bondSize = 10;


    public void initializeWorldBank()
    {
        Debug.Log("Number of Nations is: " + State.getNations().Count + "________________________________________________________________________________________________________________________");
        for (int i = 0; i < State.getNations().Count; i++)
        {
            deposits.Add(0);
            loans.Add(0);
            bankruptThisTurn.Add(false);
        }
    }


    public void makeDeposit(Nation player)
    {
        if (player.getGold() < bondSize)
        {
            return;
        }
        player.payGold(bondSize);
        int nationIndex = player.getIndex();
        deposits[nationIndex]++;
        totalDeposits++;

    }

    public void withdrawDeposit(Nation player)
    {
        int nationIndex = player.getIndex();

        if (deposits[nationIndex] < 1)
        {
            return;
        }
        player.receiveGold(bondSize);
        deposits[nationIndex]--;
        totalDeposits--;
    }

    public void takeLoan(Nation player)
    {
        player.receiveGold(bondSize);
        int nationIndex = player.getIndex();
        loans[nationIndex]++;
        totalLoans++;
    }

    public void payOffLoan(Nation player)
    {
        player.payGold(bondSize);
        int nationIndex = player.getIndex();
        loans[nationIndex]--;
        totalLoans--;
    }

    public float getInterestRate()
    {
        if (totalDeposits == 0)
        {
            Debug.Log("No deposits");
            return 0;
        }
        else
        {
            Debug.Log("Total Loans: " + totalLoans);
            Debug.Log("Total Deposits: " + totalDeposits);
          //  Debug.Log("IDF: " + IDF);
            float interestRate = (float)totalLoans / (float)(totalDeposits * IDF);
            Debug.Log("Interest Rate is: " + interestRate);
            return interestRate;

        }
    }


    public int determineCreditLimit(Nation player)
    {
        float assetsToDeptFactor = 2f;
        if (player.Bankrupt == true)
        {
            assetsToDeptFactor = 1f;
        }

        int baseAssets = PlayerCalculator.caclulateBaseAssets(player);
        int amount = (int)(baseAssets * assetsToDeptFactor / bondSize);
        if(amount < 1)
        {
            return 1;
        }
        else
        {
            return amount;
        }
       
    }

    // Remember to call this at the end of the EVENT phase
    public void resetBankruptThisTurn()
    {
        for (int i = 0; i < State.getNations().Count; i++)
        {
            bankruptThisTurn[i] = false;
        }
    }

    public void processBankruptcy(Nation player)
    {
        player.Bankrupt = true;
        int playerIndex = player.getIndex();
        bankruptThisTurn[playerIndex] = true;
        int assetsLost = loans[playerIndex];
        totalLoans -= assetsLost;
        //totalDeposits -= assetsLost;

        player.decreasePrestige(assetsLost);
        player.InfulencePoints -= assetsLost * 2;
        player.decraseStability();
        foreach (int index in State.getMajorNations())
        {
            if (index == playerIndex)
            {
                continue;
            }
            Nation power = State.getNations()[index];
            float assetShare = getAssetShare(power);
            if (assetShare >= 20)
            {
                player.adjustRelation(power, -15);
            }
        }
        loans[playerIndex] = 0;
        
    }

    public float getAssetShare(Nation player)
    {
        int thisPlayersAssets = deposits[player.getIndex()];
        float assetShare =  (float)thisPlayersAssets / (float)totalDeposits;
        Debug.Log("Asset Share is : " + assetShare);
        return assetShare;
    }

    public void collectInterest()
    {
        float collection = 0;
        float interestRate = getInterestRate();
        Debug.Log("Interest Rate is: " + interestRate);


        foreach (Nation nation in State.getNations().Values)
        {
            int thisNationIndex = nation.getIndex();
            int thisNationsDebt = loans[thisNationIndex];
            if (thisNationsDebt > 0)
            {
                Debug.Log("This Nations's Debt: " + thisNationsDebt);
                float interestOwed = interestRate * thisNationsDebt * bondSize;
                Debug.Log("Nation: " + nation.getName() + "  Interest Owed: " + interestOwed + " ____________________________________________________ ");
                // Player can afford interest payment
                if (nation.getGold() >= interestOwed)
                {
                    Debug.Log("Pays Interest");
                    nation.payGold(interestOwed);
                    collection += interestOwed;
                    nation.InterestPayedLastTurn = interestOwed;
                }
                // Player cannot afford interest payment
                else
                {
                    Debug.Log("Not enough money");

                    //see if player can take out another loan
                    int maxNumberOfLoans = determineCreditLimit(nation);
                    Debug.Log("Credit Limit is : " + maxNumberOfLoans);
                    bool paidInterest = false;
                    int loopBreaker = 0;
                    while (thisNationsDebt < maxNumberOfLoans && paidInterest == false && loopBreaker < 5)
                    {
                        loopBreaker++;
                        if (nation.getGold() >= interestOwed)
                        {
                            nation.payGold(interestOwed);
                            collection += interestOwed;
                            nation.InterestPayedLastTurn = interestOwed;
                            paidInterest = true;
                            Debug.Log("Pays Interest");
                        }
                        else
                        {
                            Debug.Log("Takes out another loan");

                            takeLoan(nation);
                            thisNationsDebt = loans[thisNationIndex];
                        }
                    }

                    // Player cannot pay interest even by taking on more debt -> bankrupt
                    //
                    if (paidInterest == false)
                    {
                        Debug.Log("Player has gome bankrupt");
                        int assetsLost = loans[thisNationIndex];
                        collection -= assetsLost * bondSize;
                        processBankruptcy(nation);
                    }
                }
            }
            else
            {
                nation.InterestPayedLastTurn = 0;
            }
        }
        Debug.Log("Interest Collected :  " + collection);
        interestCollected = collection;
    }


    public void distributeCollectedInterest()
    {
        Debug.Log("Distribute Collected Interest");
        foreach (Nation nation in State.getNations().Values)
        {
            int nationIndex = nation.getIndex();
            Debug.Log(nationIndex);

            if (deposits[nationIndex] > 0)
            {
                float assetShare = getAssetShare(nation);
                float payment = assetShare * interestCollected;
                nation.InterestCollectedLastTurn = payment;
                nation.receiveGold(payment);
                Debug.Log(nation.getName() + " receives " + payment);
                if (payment < 0 && nation.getGold() < 0)
                {
                    int loopBreaker = 0;
                    Debug.Log("About to enter While Loop");
                    while (nation.getGold() < 0 && deposits[nationIndex] > 0 && loopBreaker < 4)
                    {
                        loopBreaker++;
                        withdrawDeposit(nation);
                    }
                }
            }
            else
            {
                nation.InterestCollectedLastTurn = 0;
            }
        }
        Debug.Log("Completed distributed interest");
    }

    public int getDebt(Nation nation)
    {
        int index = nation.getIndex();
        return loans[index];
    }

    public int getDeposits(Nation nation)
    {
        int index = nation.getIndex();
        Debug.Log(index);
        Debug.Log(deposits.Count);
        return deposits[index];
    }

    public int getTotalLoans()
    {
        return totalLoans;
    }

    public int getTotalDeposits()
    {
        return totalDeposits;
    }


public int BondSize { get => bondSize; set => bondSize = value; }

}
