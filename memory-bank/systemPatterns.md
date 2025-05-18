# System Patterns: ComputerYacht Simulator

**Date:** 2025-05-18

## 1. Architectural Style

*   **Event-Driven Architecture:** The application uses a Windows Forms UI, where user interactions (button clicks) and timer events (`tmrMain_Tick`) drive the application flow, particularly the game simulation loop.
*   **Layered (Implicit):** While not strictly enforced with separate projects or namespaces for each layer, there's a conceptual separation:
    *   **Presentation Layer:** `frmMain.cs` (UI, event handling, display updates).
    *   **Application Logic/Domain Layer:** `Yacht.cs` (core game rules), `Computer.cs` (AI strategy).
    *   **Utility/Helper Layer:** `Dice.cs` (probability calculations).
*   **Simulation Core:** The application is fundamentally a simulation engine designed to run many iterations of a game played by an AI.

## 2. Key Design Patterns & Principles Observed

*   **Strategy Pattern (Implicit in AI):** The `Computer.cs` class encapsulates the AI's strategy for playing Yacht. Different AI strategies could potentially be implemented by creating different classes that adhere to a common interface for `HoldDice` and `GetScoringLocation` (though no explicit interface was observed, `Computer` itself acts as the concrete strategy). The `TransmutableComputer.cs` might be an example of an alternative or adaptable strategy.
*   **State Pattern (Implicit in Game Flow):** The `Yacht.cs` class manages the game state through variables like `iRollIndex`, `iPlayerIndex`, and the `iScores` array. The behavior of methods like `ComputerNextMove` changes based on this state (e.g., whether to roll dice, hold dice, or score).
*   **Observer Pattern (UI Updates):** `frmMain.cs` implicitly observes the state of the `yYacht` (YachtTest) object. After game actions, `frmMain` reads data from `yYacht` and updates UI elements (`tbStats`, `tbDices`, `tbScores`). This is a manual observation rather than a formal Observer pattern implementation.
*   **Constants for Configuration/Behavior:** The `Computer.cs` class heavily relies on static arrays of constants (e.g., `SCOREINDEX_ATTAIN_WEIGHTING`, `INDEX_STORAGE_LOCATION_WEIGHTS`, `Take0s`) to define the AI's behavior and decision-making weights. This makes parts of the AI's logic data-driven.
*   **Utility Class:** `Dice.cs` is a static utility class providing a specific function (dice probability calculation) used by other parts of the system.
*   **File Logging for Data Collection:** The application uses simple file I/O (`StreamWriter`) to log game results to "Games.txt", a common pattern for collecting data from simulations.

## 3. Data Structures & Management

*   **Arrays for Game Data:** Core game data like dice values, hold status, and scores are managed using arrays (`iDicesValue`, `bDicesHold`, `iScores` in `Yacht.cs`).
*   **Multi-dimensional Arrays for AI Weights:** The AI uses multi-dimensional arrays (e.g., `SCOREINDEX_ATTAIN_WEIGHTING` in `Computer.cs`) to store complex weighting schemes.
*   **Random Number Generation:** `System.Random` is used for dice rolls, though the `Yacht.ROLL_DICES` lookup table introduces a deterministic or biased element to the "random" roll.

## 4. Potential Areas for Improvement (from a patterns perspective)

*   **Explicit Interfaces for AI:** Defining an `IComputerAI` interface could make it easier to plug in different AI strategies.
*   **Formal Observer Pattern:** For more complex UIs or multiple views, a formal Observer pattern could decouple the game logic from the UI more cleanly.
*   **Configuration Files:** Some of the AI's weighting constants could potentially be moved to external configuration files for easier tuning without recompiling, if that were a goal.

## 5. Code Structure & Modularity

*   The code is reasonably modular, with distinct responsibilities for `frmMain` (UI), `Yacht` (game logic), `Computer` (AI), and `Dice` (utility).
*   The use of `partial class` for `frmMain` (with `frmMain.Designer.cs`) is standard for Windows Forms to separate UI designer code from event handling code.