<![CDATA[# Decision Log: ComputerYacht Analysis & Modifications

**Date:** 2025-05-18

| Timestamp | Decision                                                                 | Reason                                                                                                                               | Alternatives Considered                 | Outcome/Impact                                                                                                |
| :-------- | :----------------------------------------------------------------------- | :----------------------------------------------------------------------------------------------------------------------------------- | :------------------------------------ | :------------------------------------------------------------------------------------------------------------ |
| 15:42     | Start analysis by reading `frmMain.cs`.                                  | `frmMain.cs` typically handles UI and application entry point/main loop, providing a good overview of user interaction and program flow. | Reading `Program.cs` (usually minimal). | Confirmed UI structure, main simulation loop, and references to `Yacht` and `Computer` classes.                 |
| 15:42     | Next, read `Yacht.cs`.                                                   | Referenced by `frmMain.cs` and likely contains core game logic.                                                                      | Reading `Computer.cs` first.          | Understood game rules, dice mechanics, scoring, and how it interacts with a `Computer` object for AI moves.     |
| 15:42     | Next, read `Computer.cs`.                                                | Referenced by `Yacht.cs` and `frmMain.cs`; expected to contain AI decision-making logic.                                             | Reading utility classes like `Dice.cs`. | Revealed complex AI strategy involving weighted choices for holding dice and selecting scoring categories.      |
| 15:43     | Next, read `Dice.cs`.                                                    | Referenced by `Computer.cs` for probability calculations (`CalculateExactChance`).                                                   | Skipping less central files.          | Understood the dice probability calculation method (exhaustive enumeration).                                  |
| 15:43     | Defer analysis of `ComputerTester.cs`, `YachtTest.cs`, `TransmutableComputer.cs`. | These seem like testing or auxiliary classes, not central to the core functionality requested by the user task.                  | Analyzing all `.cs` files.            | Focused on core logic for initial summary and pseudocode. These files can be analyzed later if more depth is needed. |
| 15:43     | Proceed to generate functional overview and pseudocode.                  | Sufficient understanding of core modules (`frmMain`, `Yacht`, `Computer`, `Dice`) has been achieved.                                 | Further detailed file analysis.       | Able to produce the requested deliverables.                                                                   |
| 15:43     | Create Memory Bank files (`productContext.md`, `activeContext.md`, etc.). | Task requirement to store analysis in Memory Bank. Files were missing.                                                               | Inform user and stop.                 | Memory Bank populated with analysis results.                                                                  |
| 15:46     | Create `architecture.md` to document the software architecture. | User request to describe software architecture based on existing Memory Bank analysis. | - | Provided a structured overview of the system's architecture, components, and interactions. |
| 16:00     | Implement manual single-step (full game) simulation.                     | User requirement to change from automated continuous simulation to manual per-game simulation.                                       | Modifying timer for slower steps; adding more granular pause/play. | `frmMain.cs` timer removed. New button `btnManualStep` added. Logic in `btnManualStep_Click` now runs one full game. UI updates dice, scores, and stats. Memory Bank docs updated. |
---
### Decision (Code)
[2025-05-18 16:05:00] - Implemented manual simulation logic in `btnManualStep_Click`

**Rationale:**
The user requested a manual, full-game simulation per button click. The logic iterates `yYacht.ComputerNextMove()` until a game completes, then updates statistics and resets the game. UI updates for dice and scores are performed after each turn within the game loop for better user feedback, and final stats are updated upon game completion. This approach directly addresses the requirements and reuses existing game logic and statistical update mechanisms.

**Details:**
- Modified: [`ComputerYacht/frmMain.cs`](ComputerYacht/frmMain.cs:0) (added `btnManualStep_Click` handler)
- Modified: [`ComputerYacht/frmMain.Designer.cs`](ComputerYacht/frmMain.Designer.cs:0) (added `btnManualStep` control and event wiring)
- Removed timer-based simulation (`tmrMain`, `tmrMain_Tick`) and pause functionality (`btnPause`).
17 | ---
18 | | (Timestamp from env) | Refine "Manual Single Step Simulation" to be step-by-step within a turn. | User clarified that "manual single step" should mean one step within a turn (roll, AI hold, AI score) rather than one full game. | Keep existing full-game step; Add a new button for finer-grained steps. | Chosen approach modifies existing button's behavior for clarity and directness. Requires significant refactoring of `ComputerNextMove` and state management in `frmMain.cs`. |
19 | | (Timestamp from env) | Decompose `Computer.cs` AI logic and `Yacht.cs` game flow. | To allow `frmMain.cs` to control each step of a turn, the monolithic `ComputerNextMove()` (or equivalent) logic needs to be broken into smaller, publicly callable methods. | Expose internal state variables from `Yacht.cs`/`Computer.cs` for `frmMain.cs` to manipulate (more complex, breaks encapsulation). | New methods proposed: `Computer.ComputerDecideDiceToHold()`, `Computer.ComputerSelectScoringCategory()`, `Yacht.PerformRoll()`, `Yacht.ApplyHoldDecision()`, `Yacht.ApplyScoreAndFinalizeTurn()`. |
20 | | (Timestamp from env) | Introduce `TurnStepPhase` state enumeration in `frmMain.cs`. | To manage the sequence of operations within a single turn when "手动单步模拟" button is clicked. Each click advances to the next phase. | Using multiple boolean flags (less clear, harder to manage); Using integer state codes. | `TurnStepPhase` enum (e.g., `READY_FOR_ROLL_1`, `AWAITING_HOLD_DECISION_1`, etc.) provides a clear and manageable way to control the step-through logic in `btnManualStep_Click`. |
---
### Decision (Architecture Review)
[2025-05-18 16:24:59] - Confirmed `architecture.md` accurately reflects step-through simulation changes.

**Rationale:**
The `spec-pseudocode` mode previously updated `architecture.md` to detail the new `TurnStepPhase` in `frmMain.cs`, the refined public interfaces of `Yacht.cs` and `Computer.cs`, and the updated control/data flow for the "手动单步模拟" feature. This review by Architect mode confirms these descriptions are accurate and align with the refined requirements for step-by-step turn execution. Minor textual adjustments were made for clarity and to update the review timestamp.

**Implications/Details:**
- `architecture.md` is now considered up-to-date regarding the manual step-through simulation logic.
- This confirmation allows other modes or future development to confidently use `architecture.md` as a reference for this feature.
- Files updated: [`memory-bank/architecture.md`](memory-bank/architecture.md:0) (minor review edits), [`memory-bank/activeContext.md`](memory-bank/activeContext.md:0), [`memory-bank/decisionLog.md`](memory-bank/decisionLog.md:0) (this entry), [`memory-bank/progress.md`](memory-bank/progress.md:0).
---
### Decision (Debug)
[2025-05-18 17:04:55] - [Bug Fix Strategy: Remove Invalid CDATA Tags]

**Rationale:**
Compilation errors CS1525 and CS1022 in `Computer.cs` and `Yacht.cs` indicated invalid characters at the beginning and end of the files. `read_file` confirmed the presence of `<![CDATA[` at the start and `]]>` at the end of both files. These tags are not valid C# syntax and were causing the compilation to fail. The most direct fix was to remove these tags.

**Details:**
- Affected components/files: [`ComputerYacht/Computer.cs`](ComputerYacht/Computer.cs:0), [`ComputerYacht/Yacht.cs`](ComputerYacht/Yacht.cs:0)
- Action: Used `write_to_file` tool to rewrite both files without the offending CDATA tags. This was chosen after `apply_diff` and `search_and_replace` tools encountered persistent issues with either diff formatting or locating the exact strings at the file boundaries. `write_to_file` provided a more robust way to ensure the files were correctly formatted.
---
### Decision (Debug)
[2025-05-18 18:47:00] - [Bug Fix Strategy: Address Compilation Errors and Warnings]

**Rationale:**
The primary goal was to resolve compilation errors CS0426 and CS1061 reported in the build log to allow the project to compile successfully. Secondary goal was to address warnings CS0108 and CS0618 where feasible without major refactoring.

**Details:**
*   **CS0426 (Error in `frmMain.cs`):**
    *   **Issue:** `ScoringDecision` was incorrectly referenced as `Computer.ScoringDecision`.
    *   **Fix:** Changed `Computer.ScoringDecision` to `ScoringDecision` in [`ComputerYacht/frmMain.cs`](ComputerYacht/frmMain.cs:266) as `ScoringDecision` is defined in the `ComputerYacht` namespace, not within the `Computer` class.
*   **CS1061 (Error in `Computer.cs`):**
    *   **Issue:** `Yacht` object was called with a non-existent method `GetDicesValue`.
    *   **Fix:** Replaced calls to `yYacht.GetDicesValue(i)` with `yYacht.GetCurrentDiceValues()[i]` in [`ComputerYacht/Computer.cs`](ComputerYacht/Computer.cs:663) and [`ComputerYacht/Computer.cs`](ComputerYacht/Computer.cs:677), as `GetCurrentDiceValues()` is the correct method returning an array.
*   **CS0108 (Warning in `YachtTest.cs`):**
    *   **Issue:** Methods `PlayerToString` and `DicesToString` in `YachtTest` were hiding base class members.
    *   **Fix:** Added the `new` keyword to `PlayerToString` and `DicesToString` method signatures in [`ComputerYacht/YachtTest.cs`](ComputerYacht/YachtTest.cs:0).
*   **CS0618 (Warning in `frmMain.cs`):**
    *   **Issue:** Obsolete method `Yacht.ComputerNextMove()` was called in the `Next()` method.
    *   **Fix:** Commented out the entire `Next()` method in [`ComputerYacht/frmMain.cs`](ComputerYacht/frmMain.cs:0) as it appeared to be unused by the current step-through simulation logic.
*   **CS0618 (Warning in `TransmutableComputer.cs`):**
    *   **Decision:** Deferred, as `TransmutableComputer.cs` is likely not part of the core compilation path for the main application and fixing it might involve more complex refactoring outside the immediate scope of fixing primary compilation errors.
<![CDATA[
---
### Decision (Code)
[2025-05-18 19:10:00] - AI Logic Porting Strategy for `Computer.cs`

**Rationale:**
The task was to port the old, complex AI logic from obsolete methods (`HoldDice`, `GetScoringLocation`) into the new `DecideDiceToHold` and `ChooseScoreCategory` methods in `Computer.cs`.

*   **For `ChooseScoreCategory`:** The logic from the old `GetBestScoreIndex` (and its helpers like `GetDiceScoreBalance`, `ViableBonusScore`) was deemed adaptable. The approach was to refactor these to work with parameters (`finalDiceValues`, `availableCategories`) instead of direct `Yacht` object access. A new helper `IsBonusStillViableInternal` was created to encapsulate parts of `ViableBonusScore`'s logic. The existing `CalculateScoreForCategory` was enhanced to better handle Joker rules for Yachtzee scoring. This provides a significant improvement over the placeholder.

*   **For `DecideDiceToHold`:** The original logic (primarily `CalculateBestScoreItem` -> `HoldDiceForScore` -> `HoldSpecificDice` and complex chance calculations) was found to be deeply intertwined with the `Yacht` object's state. Methods like `HoldSpecificDice` directly modified `yYacht.DicesHold`, and chance calculations relied on this state. A full, direct port to a stateless, parameter-driven model would require extensive refactoring of many helper methods and careful simulation of state changes, which is a very complex and error-prone task.
    Given the complexity and the risk of introducing subtle bugs, the decision was made to:
    1.  Retain the simpler placeholder logic for `DecideDiceToHold` for now.
    2.  Add extensive comments within `DecideDiceToHold` detailing:
        *   The structure and key components of the original AI's dice-holding strategy.
        *   The specific challenges related to state dependency and the refactoring required for methods like `HoldSpecificDice`, `CalculateChance`, and `CalculateBestScoreItem`.
        *   A high-level outline of how a more complete port could be approached in the future.
    This approach fulfills the task requirement of documenting difficulties if a direct port is too complex, while still delivering improvements to `ChooseScoreCategory`.

**Details:**
- Modified: [`ComputerYacht/Computer.cs`](ComputerYacht/Computer.cs:0)
    - `ChooseScoreCategory` reimplemented with ported logic.
    - `IsBonusStillViableInternal` helper added.
    - `CalculateScoreForCategory` enhanced for Joker rules.
    - `DecideDiceToHold` kept placeholder logic but with extensive new comments explaining porting challenges of old AI.
- Tools: Used `write_to_file` for `Computer.cs` after encountering persistent issues with `apply_diff` on complex multi-method replacements.
]]>
---
### Decision (Debug)
[2025-05-18 19:31:35] - [Bug Fix Strategy: Remove Invalid CDATA Tags from Computer.cs]

**Rationale:**
The compilation log showed multiple errors (CS1525, CS1003, CS1002, CS1529, CS1022, CS8803, CS1733) pointing to invalid syntax at the beginning and end of `ComputerYacht/Computer.cs`. Inspection confirmed the presence of `<![CDATA[` at the start and `]]>` at the end of the file. These tags are not valid C# syntax and were the root cause of the compilation failure. The most direct fix was to remove these tags. This is consistent with previous similar issues in other files.

**Details:**
- Affected components/files: [`ComputerYacht/Computer.cs`](ComputerYacht/Computer.cs:0)
- Action: Used the `write_to_file` tool to rewrite the file, removing `<![CDATA[` from the first line and `]]>` from the last line. The `write_to_file` tool was chosen for its robustness in handling file boundary modifications, based on past experiences documented in the Memory Bank.
---
### Decision (Debug)
[2025-05-18 20:13:00] - [Bug Fix Strategy: Define NUM_CATEGORIES constant]

**Rationale:**
Compilation error CS0117 indicated that `Yacht.NUM_CATEGORIES` was undefined. The `Computer.cs` file uses this to iterate through scoring categories. The most direct fix is to define this constant in the `Yacht` class. The value 13 is confirmed by `productContext.md` (line 20: "13 standard scoring categories").

**Details:**
- Affected components/files: [`ComputerYacht/Yacht.cs`](ComputerYacht/Yacht.cs:0), [`ComputerYacht/Computer.cs`](ComputerYacht/Computer.cs:0) (indirectly, by resolving its dependency)
- Action: Added `public const int NUM_CATEGORIES = 13;` to the `Yacht` class in [`ComputerYacht/Yacht.cs`](ComputerYacht/Yacht.cs:607).
<![CDATA[
---
### Decision (Code)
[2025-05-18 20:38:00] - Implemented a new heuristic-based dice holding strategy in `Computer.cs`'s `DecideDiceToHold` method.

**Rationale:**
The previous placeholder logic for `DecideDiceToHold` was too simplistic, causing the AI to underperform. The new strategy uses a series of prioritized rules (checking for Yachtzee, Four of a Kind, Full House, Straights, Three of a Kind, Pairs, and high dice) to make more intelligent decisions about which dice to hold across rolls 1 and 2. This aims to significantly improve AI gameplay by better pursuing valuable combinations. Full porting of the original complex AI was deemed too complex for this task, so a rule-based heuristic was chosen as a significant improvement.

**Details:**
- Modified: [`ComputerYacht/Computer.cs`](ComputerYacht/Computer.cs:0) (updated `DecideDiceToHold` and added helper methods like `CountOccurrences`, `HasNOfAKind`, `IsSmallStraight`, `IsLargeStraight`, `MarkStraightDice`, `HoldValue`, `GetSortedUniqueDice`, `CountHeld`).
]]>
---
### Decision (Debug)
[2025-05-18 20:43:34] - [Bug Fix Strategy: Remove Invalid CDATA Tags from Computer.cs]

**Rationale:**
The compilation log showed multiple errors (CS1525, CS1003, CS1002, CS1529, CS1022, CS8803, CS1733) pointing to invalid syntax at the beginning and end of `ComputerYacht/Computer.cs`. Inspection confirmed the presence of `<![CDATA[` at the start and `]]>` at the end of the file. These tags are not valid C# syntax and were the root cause of the compilation failure. The most direct fix was to remove these tags. This is consistent with previous similar issues in other files and user instruction.

**Details:**
- Affected components/files: [`ComputerYacht/Computer.cs`](ComputerYacht/Computer.cs:0)
- Action: Used the `write_to_file` tool to rewrite the file, removing `<![CDATA[` from the first line and `]]>` from the last line.
---
### Decision (Architecture Update)
[2025-05-18 21:14:00] - 更新架构文档以包含手动骰子输入和AI建议功能。

**Rationale:**
响应用户提出的新功能需求（允许用户手动输入5个骰子点数，然后应用基于这些点数提供 AI 的保留建议），并确保架构文档准确反映预期的系统行为和组件交互。`spec-pseudocode` 模式已为此功能生成了规范和伪代码，这些信息也为本次架构更新提供了依据。

**Implications/Details:**
- [`memory-bank/architecture.md`](memory-bank/architecture.md:0) 已更新，主要修改包括：
    - 在概述中加入了新功能的描述。
    - 更新了对 [`ComputerYacht/frmMain.cs`](ComputerYacht/frmMain.cs:0) 的描述，说明了新增的UI元素（5个骰子输入框，1个建议按钮）及其事件处理流程。
    - 更新了对 [`ComputerYacht/Yacht.cs`](ComputerYacht/Yacht.cs:0) 的描述，加入了新的公共方法 `SetManuallyEnteredDice()` 用于接收手动输入的骰子。
    - 更新了对 [`ComputerYacht/Computer.cs`](ComputerYacht/Computer.cs:0) 的描述，说明其 `DecideDiceToHold()` 方法将如何处理来自手动设置的骰子。
    - 新增了一个专门描述此功能数据流和控制流的子章节，并包含了一个Mermaid序列图。
    - 更新了文档的总结部分。
- 此更新为后续开发和模式（如 `code` 模式）实现此功能提供了清晰的架构指导。
---
### Decision (Code Implementation)
[2025-05-18 21:22:00] - Tooling Choice: `write_to_file` over `apply_diff` for complex/multi-part file modifications.

**Rationale:**
During the implementation of the "Manual Dice Input & AI Suggestion" feature, multiple attempts to use `apply_diff` for modifications to [`ComputerYacht/frmMain.Designer.cs`](ComputerYacht/frmMain.Designer.cs:0) and [`ComputerYacht/frmMain.cs`](ComputerYacht/frmMain.cs:0) failed due to persistent "Diff block is malformed: marker '=======' found in your diff content" errors. Despite several attempts to correct the diff format, the issue persisted, likely due to the complexity of the changes involving multiple insertion points and renamings.

To ensure reliable and timely completion of the file modifications, the strategy was switched to using `read_file` to get the current content, manually constructing the complete desired file content in memory, and then using `write_to_file` to overwrite the target file.

**Details:**
- Affected files: [`ComputerYacht/frmMain.Designer.cs`](ComputerYacht/frmMain.Designer.cs:0), [`ComputerYacht/frmMain.cs`](ComputerYacht/frmMain.cs:0).
- Alternative considered: Continued attempts to debug `apply_diff` formatting, breaking down diffs into smaller pieces.
- Outcome: Using `write_to_file` proved successful and allowed the task to proceed efficiently. While `apply_diff` is preferred for surgical changes, `write_to_file` is a more robust fallback for complex or problematic diffs.
]]>
---
### Decision (Debug)
[2025-05-18 21:29:02] - [Bug Fix Strategy: Remove Invalid CDATA Tags from Multiple Files]

**Rationale:**
The compilation log showed multiple errors (CS1519, CS1525, CS1003, CS1001, CS1002, CS1529) pointing to invalid syntax at the beginning and/or end of `ComputerYacht/Yacht.cs`, `ComputerYacht/frmMain.Designer.cs`, and `ComputerYacht/frmMain.cs`. Inspection of these files (and similar issues in Memory Bank files) confirmed the presence of `&lt;![CDATA[` at the start and `]]&gt;` at the end of these code files. These tags are not valid C# syntax and were the root cause of the compilation failures. The most direct and previously successful fix strategy for such issues is to remove these tags.

**Details:**
- Affected components/files: [`ComputerYacht/Yacht.cs`](ComputerYacht/Yacht.cs:0), [`ComputerYacht/frmMain.Designer.cs`](ComputerYacht/frmMain.Designer.cs:0), [`ComputerYacht/frmMain.cs`](ComputerYacht/frmMain.cs:0)
- Action: Used the `write_to_file` tool to rewrite each file, removing the `&lt;![CDATA[` and `]]&gt;` tags from their respective beginnings and endings. The `write_to_file` tool was chosen for its robustness in handling file boundary modifications, consistent with past successful interventions documented in the Memory Bank.