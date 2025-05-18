# Product Context: ComputerYacht Simulator

**Date:** 2025-05-18 (Updated: 2025-05-18)

## 1. Product Overview

ComputerYacht is a C# Windows Forms application that simulates the dice game Yacht (similar to Yahtzee). Its primary purpose appears to be the development, testing, and analysis of a sophisticated computer AI player for this game. The application allows for simulation of games, records detailed scoring data, and provides statistical summaries of the AI's performance.

## 2. Core Goals & Objectives

*   **AI Strategy Evaluation:** To provide a platform for evaluating the effectiveness of different AI strategies in Yacht. The current AI uses a complex system of weights, heuristics, and probability calculations.
*   **Performance Analysis:** To gather statistical data on AI performance over many games, including average scores, frequency of high-scoring combinations (like Yachtzee), and bonus acquisition rates.
*   **Game Logic Implementation:** To accurately implement the rules of Yacht, including dice rolling, scoring categories, and bonuses.
*   **Controlled Simulation:** To allow users to step through game simulations manually, advancing step-by-step *within* each computer player's turn (e.g., roll dice, AI decides dice to hold, AI chooses scoring category). This facilitates closer inspection of AI decision-making and game progression.

## 3. Key Features (Identified from Code Analysis & Recent Changes)

*   **Yacht Game Engine:**
    *   Standard 5-dice rolling mechanism (with a potentially non-standard `ROLL_DICES` lookup table influencing outcomes).
    *   13 standard scoring categories.
    *   Upper section bonus calculation.
    *   Multiple Yachtzee bonus.
*   **Computer AI Player:**
    *   Decision-making for holding dice after each roll (up to 2 re-rolls).
    *   Decision-making for selecting the optimal scoring category after the final roll.
    *   Utilizes a detailed internal representation of game states and scoring chances (65 "computer score indexes").
    *   Employs pre-defined weights and probability calculations (including exact probabilities via dice outcome enumeration) to guide decisions.
    *   Strategy for sacrificing low-value categories if necessary.
*   **Simulation & Statistics:**
    *   **Manual Step-Through Simulation:** Users can trigger the execution of individual steps within a computer player's turn by repeatedly clicking a dedicated button. Each click advances the turn through phases: 1. Roll Dice, 2. AI Decides Dice to Hold (if applicable), 3. Re-roll (if applicable), 4. AI Decides Dice to Hold (if applicable), 5. Final Roll (if applicable), 6. AI Chooses Scoring Category. This allows for detailed observation of the turn's progression. (Note: Current AI decision logic for holding and scoring is a placeholder).
    *   UI updates for dice, scores, and statistics after each completed game (and potentially during turns for dice/scores).
    *   Logging of game scores to "Games.txt".
    *   GUI display of running statistics (games played, min/max/avg score, Yachtzee %, Bonus %, score distribution).
*   **Basic UI:**
    *   Windows Forms interface for displaying dice, scores, and statistics.
    *   **Control for advancing the game step-by-step within a turn (e.g., "手动单步模拟" or "Turn Step" button).** Each click performs one action corresponding to the current phase of the turn, such as rolling dice, the AI making a hold decision, or the AI selecting a score category.
    *   Controls for copying stats.

## 4. Target Audience (Inferred)

*   Developers or researchers interested in game AI, particularly for games involving probability and strategic decision-making.
*   Hobbyists interested in Yacht or Yahtzee who wish to explore AI strategies or observe game simulations step-by-step.

## 5. Non-Goals (Inferred)

*   Human vs. Computer play (the current setup seems focused on Computer vs. itself for data generation).
*   Advanced graphical user interface or animations.
*   Network play.