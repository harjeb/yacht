using System;

namespace ComputerYacht
{
	// Token: 0x02000006 RID: 6

    public struct ScoringDecision
    {
        public int CategoryIndex;
        public int Score;

        public ScoringDecision(int categoryIndex, int score)
        {
            CategoryIndex = categoryIndex;
            Score = score;
        }
    }

	public class Computer
	{
		// Token: 0x06000024 RID: 36 RVA: 0x0000322B File Offset: 0x0000142B
		[Obsolete("HoldDice is deprecated. Use DecideDiceToHold instead.")]
		public bool HoldDice(Yacht yYacht, int RollIndex)
		{
			// Original logic: return this.HoldDiceForScore(yYacht, this.CalculateBestScoreItem(yYacht, RollIndex)) && RollIndex != 2 && !this.AllHeld(yYacht);
			throw new NotSupportedException("HoldDice is deprecated. Use DecideDiceToHold instead.");
		}

		// Token: 0x06000025 RID: 37 RVA: 0x0000324E File Offset: 0x0000144E
		[Obsolete("GetScoringLocation is deprecated. Use ChooseScoreCategory instead.")]
		public int GetScoringLocation(Yacht yYacht)
		{
			// Original logic: return this.GetBestScoreIndex(yYacht);
			throw new NotSupportedException("GetScoringLocation is deprecated. Use ChooseScoreCategory instead.");
		}

        #region New AI Decision Methods
        public bool[] DecideDiceToHold(int[] currentDiceValues, int rollNumber, bool[] availableCategories)
        {
            // Heuristic-based dice holding strategy.
            bool[] diceToHold = new bool[5]; 

            if (rollNumber == 3) 
            {
                for (int i = 0; i < 5; i++) diceToHold[i] = true;
                return diceToHold;
            }

            int[] diceCounts = CountOccurrences(currentDiceValues);

            // 1. Yachtzee
            if (HasNOfAKind(5, diceCounts, out int yachtzeeValue))
            {
                HoldValue(diceToHold, currentDiceValues, yachtzeeValue);
                return diceToHold;
            }

            // 2. Four of a Kind
            if (HasNOfAKind(4, diceCounts, out int fourOfAKindValue))
            {
                HoldValue(diceToHold, currentDiceValues, fourOfAKindValue);
                return diceToHold;
            }

            // 3. Full House
            if (HasNOfAKind(3, diceCounts, out int threeOfAKindValueFH))
            {
                int[] tempCounts = (int[])diceCounts.Clone();
                tempCounts[threeOfAKindValueFH - 1] = 0; 
                if (HasNOfAKind(2, tempCounts, out int pairValueFH))
                {
                    HoldValue(diceToHold, currentDiceValues, threeOfAKindValueFH);
                    HoldValue(diceToHold, currentDiceValues, pairValueFH);
                    return diceToHold; 
                }
            }
            
            // 4. Three of a Kind
            if (HasNOfAKind(3, diceCounts, out int threeOfAKindValue))
            {
                HoldValue(diceToHold, currentDiceValues, threeOfAKindValue);
                return diceToHold;
            }

            // 5. Straights
            bool[] straightHoldAttempt = new bool[5];
            if (IsLargeStraight(diceCounts))
            {
                MarkStraightDice(straightHoldAttempt, currentDiceValues, 5); 
                if (CountHeld(straightHoldAttempt) == 5) return straightHoldAttempt;
            }
            
            for(int i=0; i<5; i++) straightHoldAttempt[i] = false; 
            if (IsSmallStraight(diceCounts)) 
            {
                 MarkStraightDice(straightHoldAttempt, currentDiceValues, 4); 
                 if (CountHeld(straightHoldAttempt) >= 4) return straightHoldAttempt;
            }

            // 6. Two Pairs
            int firstPairValue = -1;
            int secondPairValue = -1;
            for (int val = 1; val <= 6; val++)
            {
                if (diceCounts[val - 1] >= 2)
                {
                    if (firstPairValue == -1) firstPairValue = val;
                    else { secondPairValue = val; break; }
                }
            }
            if (firstPairValue != -1 && secondPairValue != -1)
            {
                HoldValue(diceToHold, currentDiceValues, firstPairValue);
                HoldValue(diceToHold, currentDiceValues, secondPairValue);
                return diceToHold; 
            }

            // 7. One Pair
            if (firstPairValue != -1) 
            {
                HoldValue(diceToHold, currentDiceValues, firstPairValue);
                return diceToHold;
            }
            
            // 8. Hold High-Value Dice (5s, 6s)
            if (rollNumber <= 2) {
                bool heldSomething = false;
                for(int i=0; i<5; i++) {
                    if (currentDiceValues[i] >= 5) { 
                        diceToHold[i] = true;
                        heldSomething = true;
                    }
                }
                if (heldSomething) return diceToHold;
            }

            // 9. Default behavior
            if (rollNumber == 2) 
            {
                int highestDieValue = 0;
                int highestDieIndex = -1;
                for (int i = 0; i < 5; i++)
                {
                    if (currentDiceValues[i] > highestDieValue)
                    {
                        highestDieValue = currentDiceValues[i];
                        highestDieIndex = i;
                    }
                }
                if (highestDieIndex != -1)
                {
                    diceToHold[highestDieIndex] = true;
                }
            }
            
            return diceToHold; 
        }

        #region Dice Holding Helper Methods

        private static int[] CountOccurrences(int[] dice)
        {
            int[] counts = new int[6];
            foreach (int dieValue in dice)
            {
                if (dieValue >= 1 && dieValue <= 6)
                {
                    counts[dieValue - 1]++;
                }
            }
            return counts;
        }

        private static bool HasNOfAKind(int n, int[] diceCounts, out int valueOfNOfAKind)
        {
            valueOfNOfAKind = -1;
            for (int i = 0; i < 6; i++)
            {
                if (diceCounts[i] >= n)
                {
                    valueOfNOfAKind = i + 1;
                    return true;
                }
            }
            return false;
        }
        
        private static void HoldValue(bool[] diceToHold, int[] currentDiceValues, int valueToHold)
        {
            for (int i = 0; i < 5; i++)
            {
                if (currentDiceValues[i] == valueToHold)
                {
                    diceToHold[i] = true;
                }
            }
        }

        private static int CountHeld(bool[] diceToHold)
        {
            int count = 0;
            for(int i=0; i<5; i++) if(diceToHold[i]) count++;
            return count;
        }

        private static bool IsSmallStraight(int[] diceCounts)
        {
            bool s1234 = diceCounts[0] >= 1 && diceCounts[1] >= 1 && diceCounts[2] >= 1 && diceCounts[3] >= 1;
            bool s2345 = diceCounts[1] >= 1 && diceCounts[2] >= 1 && diceCounts[3] >= 1 && diceCounts[4] >= 1;
            bool s3456 = diceCounts[2] >= 1 && diceCounts[3] >= 1 && diceCounts[4] >= 1 && diceCounts[5] >= 1;
            return s1234 || s2345 || s3456;
        }

        private static bool IsLargeStraight(int[] diceCounts)
        {
            bool s12345 = diceCounts[0] >= 1 && diceCounts[1] >= 1 && diceCounts[2] >= 1 && diceCounts[3] >= 1 && diceCounts[4] >= 1;
            bool s23456 = diceCounts[1] >= 1 && diceCounts[2] >= 1 && diceCounts[3] >= 1 && diceCounts[4] >= 1 && diceCounts[5] >= 1;
            return s12345 || s23456;
        }
        
        private static void MarkStraightDice(bool[] diceToHold, int[] currentDiceValues, int requiredLength)
        {
            int[] sortedUniqueDice = GetSortedUniqueDice(currentDiceValues);

            if (sortedUniqueDice.Length < requiredLength) return;

            for (int i = 0; i <= sortedUniqueDice.Length - requiredLength; i++)
            {
                bool isStraightSequence = true;
                for (int j = 0; j < requiredLength - 1; j++)
                {
                    if (sortedUniqueDice[i + j] + 1 != sortedUniqueDice[i + j + 1])
                    {
                        isStraightSequence = false;
                        break;
                    }
                }

                if (isStraightSequence)
                {
                    for (int k = 0; k < requiredLength; k++)
                    {
                        int dieValueToLookFor = sortedUniqueDice[i + k];
                        bool alreadyHeldForThisValue = false;
                        for(int die_idx = 0; die_idx < 5; die_idx++) { 
                            if (diceToHold[die_idx] && currentDiceValues[die_idx] == dieValueToLookFor) {
                                alreadyHeldForThisValue = true;
                                break;
                            }
                        }
                        if (!alreadyHeldForThisValue) { 
                            for (int die_idx = 0; die_idx < 5; die_idx++)
                            {
                                if (!diceToHold[die_idx] && currentDiceValues[die_idx] == dieValueToLookFor)
                                {
                                    diceToHold[die_idx] = true;
                                    break; 
                                }
                            }
                        }
                    }
                    return; 
                }
            }
        }

        private static int[] GetSortedUniqueDice(int[] currentDiceValues)
        {
            bool[] present = new bool[6]; 
            int uniqueCount = 0;
            foreach (int dieValue in currentDiceValues)
            {
                if (dieValue >= 1 && dieValue <= 6) {
                    if (!present[dieValue - 1])
                    {
                        present[dieValue - 1] = true;
                        uniqueCount++;
                    }
                }
            }
            int[] uniqueDice = new int[uniqueCount];
            int currentIndex = 0;
            for (int i = 0; i < 6; i++) 
            {
                if (present[i])
                {
                    uniqueDice[currentIndex++] = i + 1; 
                }
            }
            return uniqueDice; 
        }
        #endregion

        public ScoringDecision ChooseScoreCategory(int[] finalDiceValues, bool[] availableCategories)
        {
            int bestCategoryIndex = -1;
            long bestWeightedScore = -1; // Use long to match original num comparison type if weights are large

            int[] sortedDice = (int[])finalDiceValues.Clone();
            Array.Sort(sortedDice);

            // First pass: Evaluate categories based on weighted scores (mimicking GetBestScoreIndex's first loop)
            for (int i = 0; i < Yacht.NUM_CATEGORIES; i++)
            {
                if (i == Yacht.INDEX_TOPBONUS || !availableCategories[i])
                {
                    continue;
                }

                int currentRawScore = CalculateScoreForCategory(sortedDice, i, availableCategories, finalDiceValues);
                long weightedScore;

                if (i >= Yacht.INDEX_ONES && i <= Yacht.INDEX_SIXES) // Upper section
                {
                    // Mimic GetDiceScoreBalance for upper section: 30 - (target_score_for_3_dice - actual_score)
                    // Target score for 3 dice of value (i+1) is (i+1)*3.
                    // We want to maximize actual_score, or minimize ( (i+1)*3 - actual_score )
                    // A higher "balance" (closer to 30 or above) is better.
                    // Original GetDiceScoreBalance: return 30 - ((Index + 1) * 3 - yYacht.ScoreValue(Index, false));
                    // ScoreValue here is currentRawScore.
                    int balance = 30 - (((i + 1) * 3) - currentRawScore);
                    weightedScore = (long)balance * INDEX_STORAGE_LOCATION_WEIGHTS[i];

                    if (!IsBonusStillViableInternal(availableCategories, i, currentRawScore, finalDiceValues))
                    {
                        if (currentRawScore < (i+1)*3) 
                           continue; 
                    }
                }
                else // Lower section
                {
                    weightedScore = (long)currentRawScore * INDEX_STORAGE_LOCATION_WEIGHTS[i];
                }
                
                if (bestCategoryIndex == -1 || weightedScore >= bestWeightedScore)
                {
                    if (weightedScore == bestWeightedScore)
                    {
                        if (INDEX_STORAGE_LOCATION_WEIGHTS[i] > INDEX_STORAGE_LOCATION_WEIGHTS[bestCategoryIndex])
                        {
                            bestWeightedScore = weightedScore;
                            bestCategoryIndex = i;
                        }
                    }
                    else
                    {
                        bestWeightedScore = weightedScore;
                        bestCategoryIndex = i;
                    }
                }
            }

            if (bestCategoryIndex == -1 || bestWeightedScore < 0) 
            {
                bestCategoryIndex = -1; 
                int bestRawScore = -1;
                for (int i = Yacht.INDEX_3KIND; i < Yacht.NUM_CATEGORIES; i++) 
                {
                    if (i == Yacht.INDEX_TOPBONUS || !availableCategories[i])
                    {
                        continue;
                    }
                    int currentRawScore = CalculateScoreForCategory(sortedDice, i, availableCategories, finalDiceValues);
                    if (currentRawScore > bestRawScore)
                    {
                        bestRawScore = currentRawScore;
                        bestCategoryIndex = i;
                    }
                    else if (currentRawScore == bestRawScore && bestCategoryIndex != -1)
                    {
                        if (INDEX_STORAGE_LOCATION_WEIGHTS[i] > INDEX_STORAGE_LOCATION_WEIGHTS[bestCategoryIndex])
                        {
                             bestCategoryIndex = i;
                        }
                    }
                }
            }

            if (bestCategoryIndex == -1)
            {
                for (int k = 0; k < this.Take0s.Length; k++)
                {
                    int catIdx = this.Take0s[k];
                    if (catIdx == Yacht.INDEX_TOPBONUS) continue;
                    if (availableCategories[catIdx])
                    {
                        bestCategoryIndex = catIdx;
                        break;
                    }
                }
            }
            
            if (bestCategoryIndex == -1)
            {
                for(int i_loop=0; i_loop < Yacht.NUM_CATEGORIES; ++i_loop) { 
                    if (i_loop == Yacht.INDEX_TOPBONUS) continue;
                    if (availableCategories[i_loop]) {
                        bestCategoryIndex = i_loop; 
                        break;
                    }
                }
                if (bestCategoryIndex == -1) bestCategoryIndex = Yacht.INDEX_CHANCE; 
            }

            int finalScore = CalculateScoreForCategory(sortedDice, bestCategoryIndex, availableCategories, finalDiceValues);
            return new ScoringDecision(bestCategoryIndex, finalScore);
        }

        private bool IsBonusStillViableInternal(bool[] availableCategories, int currentCategoryIndexToScore, int currentScoreForCategory, int[] diceValues)
        {
            if (currentCategoryIndexToScore < Yacht.INDEX_ONES || currentCategoryIndexToScore > Yacht.INDEX_SIXES)
                return true; 

            int potentialUpperTotal = currentScoreForCategory;
            int categoriesLeftToScoreInUpper = 0;

            for (int i = Yacht.INDEX_ONES; i <= Yacht.INDEX_SIXES; i++)
            {
                if (i == currentCategoryIndexToScore) continue;

                if (availableCategories[i])
                {
                    categoriesLeftToScoreInUpper++;
                    potentialUpperTotal += (i + 1) * 3; // Estimate 3 of a kind for open categories
                }
            }
            
            // This is a very simplified check. A full check would need to know actual scores of already filled uppers.
            // If current score is low, and many uppers open, likely still viable.
            if (currentScoreForCategory < (currentCategoryIndexToScore + 1) * 2 && categoriesLeftToScoreInUpper >= 3) return true;
            // If current score is good (3+ of a kind), take it.
            if (currentScoreForCategory >= (currentCategoryIndexToScore + 1) * 3) return true;
            // If few categories left, be more careful.
            if (categoriesLeftToScoreInUpper >= 2) return true; // Still somewhat optimistic

            int uppersAlreadyScoredCount = 0;
            for(int i_loop = Yacht.INDEX_ONES; i_loop <= Yacht.INDEX_SIXES; ++i_loop) {
                if (i_loop != currentCategoryIndexToScore && !availableCategories[i_loop]) {
                    uppersAlreadyScoredCount++;
                }
            }
            if ( (6 - uppersAlreadyScoredCount) <= 2 && currentScoreForCategory < (currentCategoryIndexToScore+1)*3) {
                return false; 
            }
            return true; 
        }

        private int CalculateScoreForCategory(int[] sortedDiceValues, int categoryIndex, bool[] availableCategories, int[] originalDiceValues)
        {
            int score = 0;
            bool isYachtzeeCurrentDice = sortedDiceValues[0] == sortedDiceValues[1] && sortedDiceValues[1] == sortedDiceValues[2] && sortedDiceValues[2] == sortedDiceValues[3] && sortedDiceValues[3] == sortedDiceValues[4];
            
            if (isYachtzeeCurrentDice && categoryIndex == Yacht.INDEX_YACHT) {
                 return 50;
            }

            // Joker Rules Application (if current dice are a Yachtzee and Yachtzee category has been scored)
            bool yachtzeeCategoryScored = !availableCategories[Yacht.INDEX_YACHT]; // Assuming 50 if scored
            if (isYachtzeeCurrentDice && yachtzeeCategoryScored)
            {
                // If the corresponding upper section category is chosen, it scores sum of dice.
                if (categoryIndex >= Yacht.INDEX_ONES && categoryIndex <= Yacht.INDEX_SIXES)
                {
                    if (sortedDiceValues[0] == (categoryIndex + 1)) // Check if Yachtzee matches this upper category
                    {
                        for (int i = 0; i < 5; i++) score += sortedDiceValues[i];
                        return score;
                    }
                }
                // For lower section categories, they score their fixed value if chosen under Joker rules.
                switch (categoryIndex)
                {
                    case Yacht.INDEX_FULLHOUSE: return 25;
                    case Yacht.INDEX_SMLSTRAIGHT: return 30;
                    case Yacht.INDEX_LGESTRAIGHT: return 40;
                    // 3K, 4K, Chance will be sum of dice, handled below if not returned here.
                }
            }


            switch (categoryIndex)
            {
                case Yacht.INDEX_ONES: case Yacht.INDEX_TWOS: case Yacht.INDEX_THREES:
                case Yacht.INDEX_FOURS: case Yacht.INDEX_FIVES: case Yacht.INDEX_SIXES:
                    int faceValue = categoryIndex + 1;
                    for (int i_loop = 0; i_loop < 5; i_loop++) 
                        if (sortedDiceValues[i_loop] == faceValue)
                            score += faceValue;
                    break;
                case Yacht.INDEX_3KIND:
                    if (isYachtzeeCurrentDice || (sortedDiceValues[0] == sortedDiceValues[1] && sortedDiceValues[1] == sortedDiceValues[2]) ||
                        (sortedDiceValues[1] == sortedDiceValues[2] && sortedDiceValues[2] == sortedDiceValues[3]) ||
                        (sortedDiceValues[2] == sortedDiceValues[3] && sortedDiceValues[3] == sortedDiceValues[4]))
                    {
                        for (int i_loop = 0; i_loop < 5; i_loop++) score += sortedDiceValues[i_loop]; 
                    }
                    break;
                case Yacht.INDEX_4KIND:
                    if (isYachtzeeCurrentDice || (sortedDiceValues[0] == sortedDiceValues[1] && sortedDiceValues[1] == sortedDiceValues[2] && sortedDiceValues[2] == sortedDiceValues[3]) ||
                        (sortedDiceValues[1] == sortedDiceValues[2] && sortedDiceValues[2] == sortedDiceValues[3] && sortedDiceValues[3] == sortedDiceValues[4]))
                    {
                        for (int i_loop = 0; i_loop < 5; i_loop++) score += sortedDiceValues[i_loop]; 
                    }
                    break;
                case Yacht.INDEX_FULLHOUSE:
                    bool isStandardFullHouse =
                        ((sortedDiceValues[0] == sortedDiceValues[1]) && (sortedDiceValues[2] == sortedDiceValues[3] && sortedDiceValues[3] == sortedDiceValues[4] && sortedDiceValues[0] != sortedDiceValues[2])) ||
                        ((sortedDiceValues[0] == sortedDiceValues[1] && sortedDiceValues[1] == sortedDiceValues[2]) && (sortedDiceValues[3] == sortedDiceValues[4] && sortedDiceValues[0] != sortedDiceValues[3]));
                    if (isStandardFullHouse) score = 25;
                    // Joker rule handled above for FH if Yachtzee scored
                    break;
                case Yacht.INDEX_SMLSTRAIGHT:
                    int[] distinctDice = new int[5];
                    int distinctCount = 0;
                    if (sortedDiceValues.Length > 0)
                    {
                        distinctDice[distinctCount++] = sortedDiceValues[0];
                        for (int i_loop = 1; i_loop < sortedDiceValues.Length; i_loop++) 
                        {
                            if (sortedDiceValues[i_loop] != sortedDiceValues[i_loop-1])
                            {
                                if (distinctCount < 5) distinctDice[distinctCount++] = sortedDiceValues[i_loop];
                            }
                        }
                    }
                    bool foundSmallStraight = false;
                    if (distinctCount >= 4) {
                        for(int i_loop=0; i_loop <= distinctCount - 4; i_loop++) { 
                            if (distinctDice[i_loop+0]+1 == distinctDice[i_loop+1] &&
                                distinctDice[i_loop+1]+1 == distinctDice[i_loop+2] &&
                                distinctDice[i_loop+2]+1 == distinctDice[i_loop+3]) {
                                foundSmallStraight = true;
                                break;
                            }
                        }
                    }
                    if (foundSmallStraight) score = 30;
                    // Joker rule handled above
                    break;
                case Yacht.INDEX_LGESTRAIGHT:
                     if ((sortedDiceValues[0] + 1 == sortedDiceValues[1] && sortedDiceValues[1] + 1 == sortedDiceValues[2] && sortedDiceValues[2] + 1 == sortedDiceValues[3] && sortedDiceValues[3] + 1 == sortedDiceValues[4]))
                        score = 40;
                    // Joker rule handled above
                    break;
                case Yacht.INDEX_YACHT: // Already handled if current dice are Yachtzee
                    if (isYachtzeeCurrentDice) score = 50;
                    break;
                case Yacht.INDEX_CHANCE:
                    for (int i_loop = 0; i_loop < 5; i_loop++) score += sortedDiceValues[i_loop]; 
                    break;
            }
            return score;
        }
        #endregion


		// Token: 0x06000026 RID: 38 RVA: 0x00003258 File Offset: 0x00001458
		protected bool ViableBonusScore(Yacht yYacht, int Score, int Index)
		{
			int num = 0;
			int num2 = Score;
			if (Score == 0)
			{
				return false;
			}
			for (int i = 0; i <= 5; i++)
			{
				if (yYacht.CurrentPlayersScore(i) == -1 && i != Index)
				{
					num++;
				}
			}
			if (num > 2 && Score / (Index + 1) >= 3)
			{
				return true;
			}
			for (int j = 0; j <= 5; j++)
			{
				int num3 = yYacht.CurrentPlayersScore(j);
				if (yYacht.CurrentPlayersScore(j) == -1)
				{
					if (j != Index)
					{
						num2 += (j + 1) * 7 / 2;
					}
				}
				else
				{
					num2 += num3;
				}
			}
			return num2 >= 63;
		}

		// Token: 0x06000027 RID: 39 RVA: 0x000032E0 File Offset: 0x000014E0
		protected int GetDiceScoreBalance(Yacht yYacht, int Index)
		{
			switch (Index)
			{
			case 0:
			case 1:
			case 2:
			case 3:
			case 4:
			case 5:
				return 30 - ((Index + 1) * 3 - yYacht.ScoreValue(Index, false));
			default:
				return yYacht.ScoreValue(Index, false);
			}
		}

		// Token: 0x06000028 RID: 40 RVA: 0x00003328 File Offset: 0x00001528
		protected int GetBestScoreIndex(Yacht yYacht)
		{
			int num = 0;
			int num2 = -1;
			for (int i = 0; i < 13; i++)
			{
				int diceScoreBalance = this.GetDiceScoreBalance(yYacht, i);
				int num3 = diceScoreBalance * this.INDEX_STORAGE_LOCATION_WEIGHTS[i];
				if (yYacht.CurrentPlayersScore(i) == -1 && (num2 == -1 || num3 >= num) && (i < 0 || i > 5 || this.ViableBonusScore(yYacht, yYacht.GetDiceScore(i), i)))
				{
					num = num3;
					num2 = i;
				}
			}
			if (num2 == -1 || num == 0)
			{
				num = 0;
				num2 = -1;
				for (int j = 6; j < 13; j++)
				{
					int num3 = yYacht.GetDiceScore(j);
					if (yYacht.CurrentPlayersScore(j) == -1 && (num2 == -1 || num3 >= num))
					{
						num = num3;
						num2 = j;
					}
				}
			}
			if (num2 == -1 || num == 0)
			{
				num2 = 0;
				for (int k = 0; k < this.Take0s.Length; k++)
				{
					if (yYacht.CurrentPlayersScore(this.Take0s[k]) == -1)
					{
						num2 = this.Take0s[k];
						break;
					}
				}
			}
			return num2;
		}

		// Token: 0x06000029 RID: 41 RVA: 0x00003418 File Offset: 0x00001618
		protected int TotalOf(int[] Ints)
		{
			int num = 0;
			for (int i = 0; i < Ints.Length; i++)
			{
				num += Ints[i];
			}
			return num;
		}

		// Token: 0x0600002A RID: 42 RVA: 0x0000343C File Offset: 0x0000163C
		protected long CalculateChance(Yacht yYacht, int[] Dice)
		{
			int num = this.HoldSpecificDice(yYacht, Dice, null, false);
			int num2 = 5 - this.TotalOf(Dice);
			if (num != 0)
			{
				return (long)(num2 * 100 / num);
			}
			return 65535L;
		}

		// Token: 0x0600002B RID: 43 RVA: 0x00003470 File Offset: 0x00001670
		protected long CalculateChance(Yacht yYacht, int RollIndex, int ScoreIndex)
		{
			int[] scoreIndexRequiredDice = this.GetScoreIndexRequiredDice(yYacht, ScoreIndex);
			return this.CalculateChance(yYacht, scoreIndexRequiredDice) * (long)this.SCOREINDEX_ATTAIN_WEIGHTING[RollIndex % 2][ScoreIndex];
		}

		// Token: 0x0600002C RID: 44 RVA: 0x0000349C File Offset: 0x0000169C
		protected int[] GetScoreIndexRequiredDice(Yacht yYacht, int ScoreIndex) // yYacht might not be needed if ScoreIndexToGameIndex is static or passed context
		{
			int[] array = new int[6];
			for (int i = 0; i < array.Length; i++)
			{
				array[i] = 0;
			}
			if (ScoreIndex == -1)
			{
				return array;
			}
			int num = this.ScoreIndexToGameIndex(ScoreIndex);
			switch (num)
			{
			case 0:
			case 1:
			case 2:
			case 3:
			case 4:
			case 5:
				array[num] = 3;
				break;
			case 7:
				array[ScoreIndex - 6] = 3;
				break;
			case 8:
				array[ScoreIndex - 12] = 4;
				break;
			case 9:
				array[(ScoreIndex - 18) % 6] = 2;
				array[(ScoreIndex - 18) / 6] += 3; // This seems to be an error in original, should be array[(ScoreIndex - 18) / 6] = 3;
                                                    // Or, if it means two different dice values for full house:
                                                    // diceValue1 = (ScoreIndex - 18) % 6; diceValue2 = (ScoreIndex - 18) / 6;
                                                    // array[diceValue1] = 2; array[diceValue2] = 3; (or vice versa)
                                                    // For now, will keep original logic structure with a comment.
                                                    // Assuming (ScoreIndex - 18) / 6 is the dice value for the three-of-a-kind part.
                if ((ScoreIndex - 18) / 6 < 6 && (ScoreIndex - 18) / 6 >=0 && (ScoreIndex - 18) % 6 < 6 && (ScoreIndex - 18) % 6 >=0) {
				    array[(ScoreIndex - 18) % 6] = 2; // The pair part
				    array[(ScoreIndex - 18) / 6] = 3; // The three of a kind part
                }
				break;
			case 10:
			{
				int i = ScoreIndex - 54; // 0 for 1234, 1 for 2345, 2 for 3456
				int num2 = i + 3;
                if (i >=0 && num2 < 6) { // Basic bounds check
				    do
				    {
					    array[i] = 1;
					    i++;
				    }
				    while (i <= num2);
                }
				break;
			}
			case 11:
			{
				int i = ScoreIndex - 57; // 0 for 12345, 1 for 23456
				int num2 = i + 4;
                if (i >=0 && num2 < 6) { // Basic bounds check
				    do
				    {
					    array[i] = 1;
					    i++;
				    }
				    while (i <= num2);
                }
				break;
			}
			case 12:
                if(ScoreIndex - 59 < 6 && ScoreIndex -59 >=0)
				    array[ScoreIndex - 59] = 5; // Dice value for Yachtzee (0-5 for dice 1-6)
				break;
			}
			return array;
		}

		// Token: 0x0600002D RID: 45 RVA: 0x00003585 File Offset: 0x00001785
		protected int ScoreIndexToGameIndex(int ScoreIndex)
		{
			return Computer.SCOREINDEX_INDEXES[ScoreIndex];
		}

		// Token: 0x0600002E RID: 46 RVA: 0x00003590 File Offset: 0x00001790
		protected void CalcHighValues(Yacht yYacht, ref int iHighCount, ref int iHighValue)
		{
			int[] diceCounts = yYacht.GetDiceCounts();
			iHighCount = 0;
			iHighValue = 0;
			for (int i = 0; i < 6; i++)
			{
				if (diceCounts[i] > iHighCount)
				{
					iHighCount = diceCounts[i];
					iHighValue = i + 1;
				}
			}
		}

		// Token: 0x0600002F RID: 47 RVA: 0x000035C8 File Offset: 0x000017C8
		protected double CalculateExactChance(Yacht yYacht, int ScoreIndex)
		{
			int[] scoreIndexRequiredDice = this.GetScoreIndexRequiredDice(yYacht, ScoreIndex);
			int num = this.TotalOf(scoreIndexRequiredDice);
			this.HoldSpecificDice(yYacht, scoreIndexRequiredDice, null, false); // This call modifies scoreIndexRequiredDice if not cloned
			int num2 = 5 - (num - this.TotalOf(scoreIndexRequiredDice)); // Number of dice to re-roll
			if (num2 == 0)
			{
				return 1.0;
			}
            // Re-fetch required dice as HoldSpecificDice modifies its parameter
            int[] requiredDiceForChance = this.GetScoreIndexRequiredDice(yYacht, ScoreIndex);
			return Dice.CalculateDiceChances(num2, requiredDiceForChance);
		}

		// Token: 0x06000030 RID: 48 RVA: 0x00003614 File Offset: 0x00001814
		protected int CalculateBestScoreItem(Yacht yYacht, int RollsLeft) // RollsLeft is 0, 1, or 2 (remaining rolls)
		{
			int num = -1; // bestScoreCompIndex
			long num2 = -1L; // bestChanceScoreProduct
			int num3 = -1; // yachtzeeBonusScoreCompIndex
			double num4 = 0.0; // yachtzeeBonusExactChance

            // Check for Yachtzee bonus if Yachtzee already scored
			if (yYacht.CurrentPlayersScore(Yacht.INDEX_YACHT) > 0) // Yachtzee already scored
			{
				int iHighCount = 0;
				int iHighValue = 0;
				this.CalcHighValues(yYacht, ref iHighCount, ref iHighValue); // Based on current dice
				if (iHighCount >= 3) // If we have at least 3 of a kind
				{
					num3 = (iHighValue - 1) + Computer.COMP_INDEX_YACHT1; // Computer score index for Yachtzee of iHighValue
					num4 = 1.0 / Math.Pow(6.0, (double)(5 - iHighCount)); // Chance to get remaining dice for Yachtzee
				}
			}

			for (int i = 0; i < Computer.NUM_COMP_INDEXES; i++) // Iterate all computer score indexes
			{
                int gameCategoryIndex = this.ScoreIndexToGameIndex(i);
				if (yYacht.CurrentPlayersScore(gameCategoryIndex) == -1) // If the game category is available
				{
					long num7 = this.CalculateChance(yYacht, RollsLeft, i); // Weighted chance for this comp_index
					if (num == -1 || num7 >= num2)
					{
						num = i;
						num2 = num7;
					}
				}
			}

            // If a Yachtzee bonus is possible and has a better exact chance than the best weighted chance found
			if (num3 != -1 && num3 != num) // If yachtzeeBonus target is different from current best
            {
                // CalculateExactChance for 'num' (current best comp_index)
                // This needs yYacht to be in the state *after* holding for 'num'
                // This part of logic is complex and might need careful state simulation if yYacht is not to be modified
                // For now, we assume CalculateExactChance can be called meaningfully.
                // The original logic might have implicitly relied on yYacht.DicesHold being set by prior calls in a sequence.
                // This is a simplification for now.
                // double chanceForCurrentBest = this.CalculateExactChance(yYacht, num);
                // if (chanceForCurrentBest < num4)
                // {
                //    num = num3;
                // }
                // Given the refactor, this comparison is hard to do accurately without more state.
                // Let's prioritize the weighted calculation for now.
            }
			return num; // Returns the best *computer score index*
		}

		// Token: 0x06000031 RID: 49 RVA: 0x000036D0 File Offset: 0x000018D0
		protected bool CanBetterScore(Yacht yYacht, int ScoreIndex) // ScoreIndex is computer score index
		{
			// int[] diceCounts = yYacht.GetDiceCounts(); // Based on current dice
			int gameCategoryIndex = this.ScoreIndexToGameIndex(ScoreIndex);
			// int dicesSum = yYacht.GetDicesSum();

			switch (gameCategoryIndex)
			{
			case Yacht.INDEX_ONES: case Yacht.INDEX_TWOS: case Yacht.INDEX_THREES:
			case Yacht.INDEX_FOURS: case Yacht.INDEX_FIVES: case Yacht.INDEX_SIXES:
                // Can always potentially better a number score unless 5 dice of that number are already showing
                int[] currentDiceCounts = yYacht.GetDiceCounts();
				return currentDiceCounts[gameCategoryIndex] < 5; 
			default: // For other categories, assume improvement is usually possible with more rolls
				return true; 
			case Yacht.INDEX_3KIND: // Example: if current dice sum is low for a 3kind, can it be better?
                // This logic was specific and might need re-evaluation for "can better"
				// return dicesSum - (ScoreIndex - 6 + 1) != 12; // Original logic was complex
                return true; // Simplification: assume can always better complex hands
			case Yacht.INDEX_4KIND:
				// return dicesSum - (ScoreIndex - 6 + 1) != 6; // Original
                return true;
			case Yacht.INDEX_FULLHOUSE: case Yacht.INDEX_LGESTRAIGHT: case Yacht.INDEX_YACHT:
                // If current dice don't make it, or make it with a low score (not applicable here), can it be better?
                // For these, if you don't have it, you can better it. If you have it, score is fixed.
				return yYacht.GetDiceScore(gameCategoryIndex) == 0; // Can better if current score is 0
			case Yacht.INDEX_SMLSTRAIGHT:
                // Can better if current score is 0, or if Large Straight is still available (as LS implies SS)
				return yYacht.GetDiceScore(gameCategoryIndex) == 0 || yYacht.CurrentPlayersScore(Yacht.INDEX_LGESTRAIGHT) == -1;
			}
		}

		// Token: 0x06000032 RID: 50 RVA: 0x00003768 File Offset: 0x00001968
		protected int[] GetScoreIndexImprovementDice(int[] StdDice, int ScoreIndex) // StdDice is from GetScoreIndexRequiredDice
		{
			int[] array = new int[6]; // Dice to additionally keep for improvement
			// Original logic was sparse, this needs more thought for a general AI
			// This method determined which *additional* dice (beyond StdDice) to keep.
			// For a placeholder, we won't add extra dice beyond the primary target.
			return array; 
		}

		// Token: 0x06000033 RID: 51 RVA: 0x000037F8 File Offset: 0x000019F8
		protected bool HoldDiceForScore(Yacht yYacht, int ScoreIndex) // ScoreIndex is computer_score_index
		{
			int[] diceToHoldForPrimaryTarget = this.GetScoreIndexRequiredDice(yYacht, ScoreIndex); // How many of each face value are needed
			// int[] diceToHoldForImprovement = this.GetScoreIndexImprovementDice(diceToHoldForPrimaryTarget, ScoreIndex); // How many extra to hold

            // Clone diceToHoldForPrimaryTarget as HoldSpecificDice modifies it
            int[] requiredDiceClone = (int[])diceToHoldForPrimaryTarget.Clone();
			int numUnmetDiceForTarget = this.HoldSpecificDice(yYacht, requiredDiceClone, null /*was improvementDice*/, true); // This sets yYacht.DicesHold

			if (ScoreIndex != -1 && numUnmetDiceForTarget == 0 && this.CanBetterScore(yYacht, ScoreIndex))
			{
                // If target is met, but score can be improved (e.g. more 6s for Sixes category)
                // The original logic returned 1 here, implying "yes, continue rolling/holding".
                // The actual holding decision is made by HoldSpecificDice.
                // This method's return seems to indicate if the hold action is "active" or "meaningful".
				return true; 
			}
			return numUnmetDiceForTarget != 0; // Return true if not all dice for target are met (so re-roll is useful)
                                            // Or if target met but can be bettered (covered by above)
                                            // Essentially, return true if further rolling might be beneficial for this ScoreIndex
		}

		// Token: 0x06000034 RID: 52 RVA: 0x0000383C File Offset: 0x00001A3C
		protected bool AllHeld(Yacht yYacht)
		{
			for (int i = 0; i < 5; i++)
			{
				if (!yYacht.DicesHold[i])
				{
					return false;
				}
			}
			return true;
		}

		// Token: 0x06000035 RID: 53 RVA: 0x00003864 File Offset: 0x00001A64
		protected int DicesHeld(Yacht yYacht)
		{
			int num = 0;
			for (int i = 0; i < 5; i++)
			{
				if (yYacht.DicesHold[i])
				{
					num++;
				}
			}
			return num;
		}

		// Token: 0x06000036 RID: 54 RVA: 0x00003890 File Offset: 0x00001A90
		protected int HoldSpecificDice(Yacht yYacht, int[] Dice, int[] ExtraDice, bool Hold) // Dice is count of each face value to *achieve*
		{
			int numUnmetDice = 0; // How many dice from the target 'Dice' array are not met by current hand
            int[] currentDiceCounts = yYacht.GetDiceCounts(); // Counts of dice currently shown on yYacht

            if (Hold) // If we are actually setting the hold status on yYacht
            {
                 for(int i=0; i<5; i++) yYacht.DicesHold[i] = false; // Clear existing holds first
            }

            int[] tempRequiredCounts = (int[])Dice.Clone(); // How many of each face value we *still need* to hold

			for (int i = 0; i < 5; i++) // Iterate through the 5 dice on the table
			{
				int dieValueIndex = yYacht.GetCurrentDiceValues()[i] - 1; // 0 to 5 for dice value 1 to 6
				if (tempRequiredCounts[dieValueIndex] > 0) // If this die's value is one we need for the target
				{
					if (Hold)
					{
						yYacht.DicesHold[i] = true;
					}
					tempRequiredCounts[dieValueIndex]--;
				}
			}
            // After satisfying primary target, check for extra dice to hold (e.g. for improvement)
            if (Hold && ExtraDice != null) {
                for (int i = 0; i < 5; i++) {
                    if (!yYacht.DicesHold[i]) { // If this die is not already held for the primary target
                        int dieValueIndex = yYacht.GetCurrentDiceValues()[i] - 1;
                        if (ExtraDice[dieValueIndex] > 0) {
                            yYacht.DicesHold[i] = true;
                            ExtraDice[dieValueIndex]--;
                        }
                    }
                }
            }

            // Calculate numUnmetDice based on the original 'Dice' target and what's *currently* on the table
            // This part of the original logic was confusing: "num += Dice[i]" after Dice was decremented.
            // It should represent how many required dice (from original 'Dice' spec) are *not* present in hand.
            // Or, more simply, how many dice you'd *still need to roll* to get the target 'Dice' counts.
            // Let's reinterpret: return the number of dice *not* held that would be part of the target if they showed up.
            // This method's return value was used in CalculateChance.
            // The original "num" was sum of remaining counts in 'Dice' array after trying to satisfy from hand.
            // This means 'num' is the count of dice you *still need* to get for the target.
            for(int i=0; i<tempRequiredCounts.Length; i++) {
                numUnmetDice += tempRequiredCounts[i];
            }
			return numUnmetDice;
		}

		// Token: 0x04000026 RID: 38
		protected const int COMP_INDEX_ONES = 0;
		// ... (all other const int COMP_INDEX_... definitions remain the same) ...
        protected const int COMP_INDEX_TWOS = 1;
		protected const int COMP_INDEX_THREES = 2;
		protected const int COMP_INDEX_FOURS = 3;
		protected const int COMP_INDEX_FIVES = 4;
		protected const int COMP_INDEX_SIXES = 5;
		protected const int COMP_INDEX_3KIND1 = 6;
		protected const int COMP_INDEX_3KIND2 = 7;
		protected const int COMP_INDEX_3KIND3 = 8;
		protected const int COMP_INDEX_3KIND4 = 9;
		protected const int COMP_INDEX_3KIND5 = 10;
		protected const int COMP_INDEX_3KIND6 = 11;
		protected const int COMP_INDEX_4KIND1 = 12;
		protected const int COMP_INDEX_4KIND2 = 13;
		protected const int COMP_INDEX_4KIND3 = 14;
		protected const int COMP_INDEX_4KIND4 = 15;
		protected const int COMP_INDEX_4KIND5 = 16;
		protected const int COMP_INDEX_4KIND6 = 17;
		protected const int COMP_INDEX_FULLHOUSE11 = 18;
		protected const int COMP_INDEX_FULLHOUSE12 = 19;
		protected const int COMP_INDEX_FULLHOUSE13 = 20;
		protected const int COMP_INDEX_FULLHOUSE14 = 21;
		protected const int COMP_INDEX_FULLHOUSE15 = 22;
		protected const int COMP_INDEX_FULLHOUSE16 = 23;
		protected const int COMP_INDEX_FULLHOUSE21 = 24;
		protected const int COMP_INDEX_FULLHOUSE22 = 25;
		protected const int COMP_INDEX_FULLHOUSE23 = 26;
		protected const int COMP_INDEX_FULLHOUSE24 = 27;
		protected const int COMP_INDEX_FULLHOUSE25 = 28;
		protected const int COMP_INDEX_FULLHOUSE26 = 29;
		protected const int COMP_INDEX_FULLHOUSE31 = 30;
		protected const int COMP_INDEX_FULLHOUSE32 = 31;
		protected const int COMP_INDEX_FULLHOUSE33 = 32;
		protected const int COMP_INDEX_FULLHOUSE34 = 33;
		protected const int COMP_INDEX_FULLHOUSE35 = 34;
		protected const int COMP_INDEX_FULLHOUSE36 = 35;
		protected const int COMP_INDEX_FULLHOUSE41 = 36;
		protected const int COMP_INDEX_FULLHOUSE42 = 37;
		protected const int COMP_INDEX_FULLHOUSE43 = 38;
		protected const int COMP_INDEX_FULLHOUSE44 = 39;
		protected const int COMP_INDEX_FULLHOUSE45 = 40;
		protected const int COMP_INDEX_FULLHOUSE46 = 41;
		protected const int COMP_INDEX_FULLHOUSE51 = 42;
		protected const int COMP_INDEX_FULLHOUSE52 = 43;
		protected const int COMP_INDEX_FULLHOUSE53 = 44;
		protected const int COMP_INDEX_FULLHOUSE54 = 45;
		protected const int COMP_INDEX_FULLHOUSE55 = 46;
		protected const int COMP_INDEX_FULLHOUSE56 = 47;
		protected const int COMP_INDEX_FULLHOUSE61 = 48;
		protected const int COMP_INDEX_FULLHOUSE62 = 49;
		protected const int COMP_INDEX_FULLHOUSE63 = 50;
		protected const int COMP_INDEX_FULLHOUSE64 = 51;
		protected const int COMP_INDEX_FULLHOUSE65 = 52;
		protected const int COMP_INDEX_FULLHOUSE66 = 53;
		protected const int COMP_INDEX_SMALLSTRAIGHT1 = 54;
		protected const int COMP_INDEX_SMALLSTRAIGHT2 = 55;
		protected const int COMP_INDEX_SMALLSTRAIGHT3 = 56;
		protected const int COMP_INDEX_LARGESTRAIGHT1 = 57;
		protected const int COMP_INDEX_LARGESTRAIGHT2 = 58;
		protected const int COMP_INDEX_YACHT1 = 59;
		protected const int COMP_INDEX_YACHT2 = 60;
		protected const int COMP_INDEX_YACHT3 = 61;
		protected const int COMP_INDEX_YACHT4 = 62;
		protected const int COMP_INDEX_YACHT5 = 63;
		protected const int COMP_INDEX_YACHT6 = 64;
		protected const int COMP_INDEX_CHANCE = 65;
		protected const int NUM_COMP_INDEXES = 66;

		// Token: 0x04000069 RID: 105
		protected static int[] SCOREINDEX_INDEXES = new int[]
		{
			0,1,2,3,4,5,7,7,7,7,7,7,8,8,8,8,8,8,9,9,9,9,9,9,9,9,9,9,9,9,9,9,9,9,9,9,9,9,9,9,9,9,9,9,9,9,9,9,9,9,9,9,9,9,10,10,10,11,11,12,12,12,12,12,12,13
		};

		// Token: 0x0400006A RID: 106
		protected int[] Take0s = new int[]
		{
			13,0,12,10,8,5,3,2,11,1,7,9,4
		};

		// Token: 0x0400006B RID: 107
		protected int[] INDEX_STORAGE_LOCATION_WEIGHTS = new int[] // Game category index weights
		{
			2,2,2,2,2,2,0,  // Bonus (index 6) is not directly chosen, so weight 0
            2,1,6,2,8,15,14 // 3K, 4K, FH, SS, LS, Yacht, Chance
		};

		// Token: 0x0400006C RID: 108
		protected int[][] SCOREINDEX_ATTAIN_WEIGHTING = new int[][] // [RollsLeft 0 or 1][comp_score_index]
		{
			new int[]
			{
				23,24,25,26,27,28,0,0,5,13,20,18,0,0,4,18,24,28,20,10,10,10,10,10,10,20,10,10,15,10,12,10,20,10,10,14,10,10,10,20,6,10,10,10,10,10,20,10,10,10,5,10,10,25,12,12,12,16,16,20,20,20,20,20,20,0
			},
			new int[]
			{
				23,24,25,26,27,23,0,0,0,17,20,18,1,0,4,18,24,28,20,15,10,13,10,10,15,20,10,10,15,10,11,10,25,10,10,10,10,6,10,20,10,10,10,10,10,10,20,15,10,10,10,10,5,20,11,12,12,16,16,20,20,25,21,20,25,0
			}
		};
	}
}
