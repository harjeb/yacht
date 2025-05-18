# Product Context: ComputerYacht Simulator

**Date:** 2025-05-18 (Updated: 2025-05-18)

## 1. Product Overview

ComputerYacht is a C# Windows Forms application that simulates the dice game Yacht (similar to Yahtzee). Its primary purpose appears to be the development, testing, and analysis of a sophisticated computer AI player for this game. The application allows for simulation of games, records detailed scoring data, and provides statistical summaries of the AI's performance. It also supports manual dice input for AI hold suggestions.

## 2. Core Goals & Objectives

*   **AI Strategy Evaluation:** To provide a platform for evaluating the effectiveness of different AI strategies in Yacht. The current AI uses a complex system of weights, heuristics, and probability calculations.
*   **Intelligent AI Suggestions:** To provide users with smart dice holding suggestions based on comprehensive game context, including current dice, roll number, available categories, and current scores.
*   **Performance Analysis:** To gather statistical data on AI performance over many games, including average scores, frequency of high-scoring combinations (like Yachtzee), and bonus acquisition rates.
*   **Game Logic Implementation:** To accurately implement the rules of Yacht, including dice rolling, scoring categories, and bonuses.
*   **Controlled Simulation & Analysis:** To allow users to step through game simulations manually and to get AI feedback on manually entered dice states.

## 3. Key Features (Identified from Code Analysis & Recent Changes)

*   **Yacht Game Engine:**
    *   Standard 5-dice rolling mechanism.
    *   13 standard scoring categories.
    *   Upper section bonus calculation.
    *   Multiple Yachtzee bonus.
*   **Computer AI Player:**
    *   Decision-making for holding dice after each roll (up to 2 re-rolls).
    *   **Enhanced Decision Logic:** The AI's `DecideDiceToHold` method considers not only the current dice and available categories, but also the current roll number (1, 2, or 3) and the player's current upper section score to make more contextually aware suggestions.
    *   Decision-making for selecting the optimal scoring category after the final roll.
    *   Utilizes a detailed internal representation of game states and scoring chances.
    *   Employs pre-defined weights and probability calculations to guide decisions.
    *   Strategy for sacrificing low-value categories if necessary.
*   **Simulation & Statistics:**
    *   **Manual Step-Through Simulation:** Users can trigger the execution of individual steps within a computer player's turn.
    *   **Manual Dice Input with AI Hold Suggestion:**
        *   Users can manually input five dice values.
        *   Users can specify the current roll number (1, 2, or 3) for the entered dice.
        *   Users can input their current upper section score.
        *   The AI provides a suggestion on which dice to hold based on this comprehensive context.
    *   UI updates for dice, scores, and statistics.
    *   Logging of game scores to "Games.txt".
    *   GUI display of running statistics.
*   **Basic UI:**
    *   Windows Forms interface for displaying dice, scores, and statistics.
    *   Controls for advancing the game step-by-step.
    *   **Controls for Manual Dice Input & Context:**
        *   Five `TextBox` controls for dice input (`txtDice1` - `txtDice5`).
        *   A `ComboBox` or `NumericUpDown` (`cmbRollNumber`) for selecting the current roll number.
        *   A `TextBox` (`txtCurrentUpperScore`) for entering the current upper section score.
        *   A button ("获取保留建议") to trigger the AI suggestion.
    *   Controls for copying stats.

## 4. Target Audience (Inferred)

*   Developers or researchers interested in game AI, particularly for games involving probability and strategic decision-making.
*   Hobbyists interested in Yacht or Yahtzee who wish to explore AI strategies, observe game simulations step-by-step, or get AI-driven advice for specific dice situations.

## 5. Non-Goals (Inferred)

*   Human vs. Computer play (the current setup seems focused on Computer vs. itself for data generation and AI-assisted manual input).
*   Advanced graphical user interface or animations.
*   Network play.
---
### Feature Enhancement: Manual Category Control for AI Suggestions (UI)
**Date:** 2025-05-18
**Goal:** Allow users to manually select which scoring categories are available to the AI when requesting hold suggestions via UI checkboxes in [`ComputerYacht/frmMain.cs`](ComputerYacht/frmMain.cs:0).
**Benefit:** Provides finer-grained control for testing AI's suggestion logic under various scenarios by simulating different game states or category availability. This enhances the "manual dice input for hold suggestion" feature.
**Status:** Spec and Pseudocode defined.