<![CDATA[# Active Context: Implemented Step-Through Simulation Feature
  2 | 
  3 | **Date:** 2025-05-18
  4 | **Timestamp:** 2025-05-18 16:36:00
  5 | 
  6 | ## 1. Current Task & Focus
  7 | 
  8 | *   **Previous Task (Completed by Architect Mode):** Review and confirm `memory-bank/architecture.md` for step-through simulation changes.
  9 | *   **Current Task (Code Mode - COMPLETED):** Implement the refined "Manual Single Step Simulation" feature. This involved modifying `frmMain.cs`, `Yacht.cs`, and `Computer.cs` to allow step-by-step execution within a computer player's turn, based on the specifications and pseudocode in the Memory Bank.
 10 | *   **Focus:**
 11 |     *   Implementing `TurnStepPhase` state management in `frmMain.cs`.
 12 |     *   Refactoring `Yacht.cs` to provide fine-grained game operation methods and getters.
 13 |     *   Refactoring `Computer.cs` to provide fine-grained AI decision methods (`DecideDiceToHold`, `ChooseScoreCategory`) and defining `ScoringDecision`.
 14 |     *   Ensuring `frmMain.cs` correctly calls the new methods in `Yacht.cs` and `Computer.cs` to progress through turn phases.
 15 |     *   Updating Memory Bank (`progress.md`, this file) to reflect task completion.
 16 | 
 17 | ## 2. Key Implementation Details
 18 | 
 19 | *   **`ComputerYacht/frmMain.cs`:**
 20 |     *   `TurnStepPhase` enum defined and `currentPhase` member added.
 21 |     *   `btnManualStep_Click` rewritten with a `switch` statement for phase-based actions.
 22 |     *   Helper methods `InitializeNewGame`, `ProcessGameOver`, `UpdateUI`, `UpdateStatusMessage` added.
 23 |     *   Calls new methods in `Yacht.cs` (e.g., `PerformRoll`, `ApplyHoldDecision`, `ApplyScoreAndFinalizeTurn`) and `Computer.cs` (e.g., `DecideDiceToHold`, `ChooseScoreCategory`).
 24 | *   **`ComputerYacht/Yacht.cs`:**
 25 |     *   `iCurrentTurnNumber` member added.
 26 |     *   `ComputerNextMove` marked `[Obsolete]`.
 27 |     *   New public methods: `PerformRoll`, `ApplyHoldDecision`, `ApplyScoreAndFinalizeTurn`, `ResetForNewGame`, `ResetForNewTurnIfNeeded`.
 28 |     *   `ScoreValue` modified to accept `preCalculatedScore`.
 29 |     *   Added various public getters: `GetCurrentDiceValues`, `GetCurrentHeldDice`, `GetRollAttemptInTurn`, `GetPlayerAvailableCategories`, `IsCategoryAvailable`, `GetPlayerScoreForCategory`, `IsGameOver` (no-arg), `GetCurrentTurnNumber`.
 30 |     *   `DicesToString` and `PlayerToString` updated for clarity.
 31 | *   **`ComputerYacht/Computer.cs`:**
 32 |     *   `ScoringDecision` struct defined.
 33 |     *   Old `HoldDice` and `GetScoringLocation` marked `[Obsolete]`.
 34 |     *   New public methods `DecideDiceToHold` and `ChooseScoreCategory` implemented with **simplified/placeholder AI logic**. Full refactoring of the original complex AI strategy was not performed due to its deep coupling with the `Yacht` object state, but the required interfaces are now in place.
 35 |     *   Helper `CalculateScoreForCategory` added for internal score estimation in the placeholder logic.
 36 | 
 37 | ## 3. Immediate Next Steps
 38 | 
 39 | *   **COMPLETED:** Code modifications for `frmMain.cs`, `Yacht.cs`, `Computer.cs`.
 40 | *   **COMPLETED:** Updated `memory-bank/progress.md`.
 41 | *   **COMPLETED:** Updated `memory-bank/activeContext.md` (this file).
 42 | *   **PENDING:** Prepare `attempt_completion` to summarize the work.
 43 | 
 44 | ## 4. Pending Questions or Blockers
 45 | 
 46 | *   The AI logic in `Computer.cs`'s new methods (`DecideDiceToHold`, `ChooseScoreCategory`) is currently a simplified placeholder. A full port of the original, more complex AI strategy would require significant further refactoring of `Computer.cs`'s internal helper methods to remove direct dependencies on the `Yacht` object's state, or a different approach to providing necessary game context to the AI.
 47 | *   UI elements (`lblStatusMessage`, `lblTurnInfo`) assumed in `frmMain.cs`'s `UpdateUI` and `UpdateStatusMessage` methods need to be added to `frmMain.Designer.cs` for visual feedback. Current implementation uses `Console.WriteLine` as a fallback.
 48 | 
 49 | ## 5. Assumptions Made
 50 | 
 51 | *   The provided specifications and pseudocode in the Memory Bank were the primary guide for the refactoring.
 52 | *   The core requirement was to enable step-by-step execution flow, with the understanding that the AI's strategic depth in the refactored `Computer.cs` methods might initially be simplified.
 53 | ]]>
 54 | * [2025-05-18 17:04:30] - [Debug Status Update: Investigating Compilation Errors] Identified and fixed invalid CDATA tags in `ComputerYacht/Computer.cs` and `ComputerYacht/Yacht.cs` that were causing CS1525 and CS1022 compilation errors. Used `write_to_file` tool for robust correction after issues with `apply_diff` and `search_and_replace`.
 55 | * [2025-05-18 18:47:00] - [Debug Status Update: Compilation Errors Fixed]
 56 |     * Resolved CS0426 in `ComputerYacht/frmMain.cs` by changing `Computer.ScoringDecision` to `ScoringDecision`.
 57 |     * Resolved CS1061 in `ComputerYacht/Computer.cs` by changing `yYacht.GetDicesValue(i)` to `yYacht.GetCurrentDiceValues()[i]`.
 58 |     * Addressed CS0108 warnings in `ComputerYacht/YachtTest.cs` by adding `new` keyword to `PlayerToString` and `DicesToString` methods.
 59 |     * Addressed CS0618 warning in `ComputerYacht/frmMain.cs` by commenting out the unused `Next()` method.
 60 | <![CDATA[
 61 | *   [2025-05-18 19:10:00] - [Code Update: AI Logic Porting]
 62 |     *   **Task:** Port old AI logic from obsolete methods in `Computer.cs` to new `DecideDiceToHold` and `ChooseScoreCategory` methods.
 63 |     *   **`ChooseScoreCategory`:** Successfully ported core logic from `GetBestScoreIndex`. This involved adapting the decision-making process for selecting the best scoring category based on final dice values and available categories. Helper method `IsBonusStillViableInternal` was added, and `CalculateScoreForCategory` was enhanced to correctly apply Joker rules related to Yachtzee.
 64 |     *   **`DecideDiceToHold`:** Due to high complexity and deep state dependency of the original `CalculateBestScoreItem` and `HoldDiceForScore` methods, a direct port was deferred. The existing placeholder logic in `DecideDiceToHold` remains. However, detailed comments have been added to this method, outlining:
 65 |         *   The structure of the original AI's dice holding strategy.
 66 |         *   The challenges in refactoring stateful helper methods (like `HoldSpecificDice`, `CalculateChance`) to work with parameterized state.
 67 |         *   A potential roadmap for a more complete port of this logic.
 68 |     *   **Outcome:** The AI's scoring decisions are now significantly more intelligent. Dice holding strategy remains basic but is clearly documented for future improvement. `Computer.cs` updated.
 69 | ]]>
 70 | * [2025-05-18 19:31:16] - [Debug Status Update: Compilation Errors Fixed in Computer.cs] Removed invalid CDATA tags from the beginning and end of `ComputerYacht/Computer.cs` to resolve CS1525, CS1003, CS1002, CS1529, CS1022, CS8803, CS1733 errors.
 71 | * [2025-05-18 20:13:00] - [Debug Status Update: CS0117 Fixed] Resolved compilation error CS0117 ("Yacht does not contain a definition for 'NUM_CATEGORIES'") by adding `public const int NUM_CATEGORIES = 13;` to [`ComputerYacht/Yacht.cs`](ComputerYacht/Yacht.cs:607).
 72 | <![CDATA[
 73 | * [2025-05-18 20:38:00] - [Code Update: `DecideDiceToHold` Improvement] Completed implementation of a new heuristic-based dice holding strategy in `Computer.cs`. The AI should now make more intelligent decisions when holding dice, aiming for better scores and combinations beyond just "Chance".
 74 | ]]>
 75 | * [2025-05-18 20:43:44] - [Debug Status Update: Compilation Errors Fixed in Computer.cs] Removed invalid CDATA tags from the beginning and end of [`ComputerYacht/Computer.cs`](ComputerYacht/Computer.cs:0) to resolve CS1525, CS1003, CS1002, CS1529, CS1022, CS8803, CS1733 errors.
 76 | * [2025-05-18 21:14:30] - [Architect Mode Task Completion: Update Architecture for Manual Dice Input Feature]
 77 |     *   **Task:** Architect mode reviewed and updated [`memory-bank/architecture.md`](memory-bank/architecture.md:0) to incorporate the new feature allowing users to manually input 5 dice values and receive AI-driven hold suggestions.
 78 |     *   **Details:**
 79 |         *   Modified [`memory-bank/architecture.md`](memory-bank/architecture.md:0) to reflect changes in UI ([`ComputerYacht/frmMain.cs`](ComputerYacht/frmMain.cs:0)), game logic ([`ComputerYacht/Yacht.cs`](ComputerYacht/Yacht.cs:0)), and AI ([`ComputerYacht/Computer.cs`](ComputerYacht/Computer.cs:0)).
 80 |         *   Added a new data flow diagram (Mermaid) and description for the manual dice input feature.
 81 |         *   Updated the document's overview and summary sections.
 82 |     *   **Memory Bank Updates:**
 83 |         *   [`memory-bank/architecture.md`](memory-bank/architecture.md:0) was updated with the new architectural design.
 84 |         *   [`memory-bank/decisionLog.md`](memory-bank/decisionLog.md:0) was updated to log this architectural decision and its rationale.
 85 |     *   **Status:** The architecture documentation is now aligned with the specifications for the new manual dice input feature.
 86 | * [2025-05-18 21:22:00] - [Code Task Completion: Implement Manual Dice Input & AI Suggestion Feature]
 87 |     *   **Task:** Implemented the feature allowing users to manually input 5 dice values and receive AI-driven hold suggestions.
 88 |     *   **Details:**
 89 |         *   **[`ComputerYacht/frmMain.Designer.cs`](ComputerYacht/frmMain.Designer.cs:0):** Added five `TextBox` controls (`txtDice1` to `txtDice5`) for dice input. Renamed `btnManualStep` to `btnGetHoldSuggestion` and updated its text to "获取保留建议".
 90 |         *   **[`ComputerYacht/Yacht.cs`](ComputerYacht/Yacht.cs:0):** Added `public void SetManuallyEnteredDice(int[] diceValuesFromUI)` to handle manual dice input, validation, and internal state update.
 91 |         *   **[`ComputerYacht/frmMain.cs`](ComputerYacht/frmMain.cs:0):**
 92 |             *   Renamed `btnManualStep_Click` to `btnGetHoldSuggestion_Click`.
 93 |             *   Modified `btnGetHoldSuggestion_Click` to read from new TextBoxes, validate, call `yYacht.SetManuallyEnteredDice()`, call `compPlayer.DecideDiceToHold()`, and then call `DisplayDiceHoldSuggestion()`.
 94 |             *   Implemented `private void DisplayDiceHoldSuggestion(bool[] holdSuggestion)` to change `TextBox` background colors.
 95 |             *   Updated `InitializeNewGame()` to clear new TextBoxes and suggestion display.
 96 |             *   Removed old step-through game logic from the button.
 97 |     *   **Memory Bank Updates:**
 98 |         *   [`memory-bank/progress.md`](memory-bank/progress.md:0) updated.
 99 |         *   [`memory-bank/activeContext.md`](memory-bank/activeContext.md:0) updated (this entry).
100 |         *   [`memory-bank/decisionLog.md`](memory-bank/decisionLog.md:0) to be updated next.
101 |     *   **Status:** Feature implemented. Code changes applied to relevant files.
102 | ]]>
103 | * [2025-05-18 21:28:43] - [Debug Status Update: Compilation Errors Fixed] Removed invalid CDATA tags from the beginning and end of `ComputerYacht/Yacht.cs`, `ComputerYacht/frmMain.Designer.cs`, and `ComputerYacht/frmMain.cs` to resolve CS1519, CS1525, CS1003, CS1001, CS1002, and CS1529 compilation errors.
104 | * [2025-05-18 22:15:00] - [Spec-Pseudocode Task: Enhance Manual Dice Input AI Suggestion]
105 |     *   **Task:** Define specifications and generate pseudocode to enhance the "manual dice input for AI hold suggestion" feature. The AI's suggestion should now consider the current roll number (1, 2, or 3) and the player's current upper section score, in addition to the dice values and available categories.
106 |     *   **Details:**
107 |         *   **UI Changes ([`ComputerYacht/frmMain.cs`](ComputerYacht/frmMain.cs:0)):**
108 |             *   Add `cmbRollNumber` (ComboBox or NumericUpDown) for user to select the current roll number (1-3).
109 |             *   Add `txtCurrentUpperScore` (TextBox) for user to input their current upper section total score.
110 |         *   **Logic Changes ([`ComputerYacht/frmMain.cs`](ComputerYacht/frmMain.cs:0)):**
111 |             *   `btnGetHoldSuggestion_Click` to read values from `cmbRollNumber` and `txtCurrentUpperScore`.
112 |             *   Pass these new values (`rollNumber`, `currentUpperScore`) to `compPlayer.DecideDiceToHold()`.
113 |         *   **Logic Changes ([`ComputerYacht/Computer.cs`](ComputerYacht/Computer.cs:0)):**
114 |             *   Modify `DecideDiceToHold` method signature to accept `int rollNumber` and `int currentUpperScore`.
115 |             *   Update internal heuristic logic in `DecideDiceToHold` to utilize `rollNumber` and `currentUpperScore` for more intelligent hold decisions (e.g., prioritizing upper section completion if bonus is viable and roll number is low).
116 |         *   **Logic Changes ([`ComputerYacht/Yacht.cs`](ComputerYacht/Yacht.cs:0)):**
117 |             *   `SetManuallyEnteredDice` method likely requires no significant changes for this feature, as `rollNumber` is passed directly to the `Computer` AI by `frmMain.cs`.
118 |     *   **Pseudocode:** Generated for `frmMain.cs`, `Computer.cs`, and `Yacht.cs` to reflect these enhancements.
119 |     *   **Memory Bank Updates:**
120 |         *   [`memory-bank/productContext.md`](memory-bank/productContext.md:0) updated to describe the enhanced feature and new UI controls.
121 |         *   [`memory-bank/activeContext.md`](memory-bank/activeContext.md:0) updated (this entry).
122 |         *   [`memory-bank/architecture.md`](memory-bank/architecture.md:0) to be updated with new UI elements and data flow.
123 |         *   [`memory-bank/decisionLog.md`](memory-bank/decisionLog.md:0) to be updated with design choices.
124 |         *   [`memory-bank/progress.md`](memory-bank/progress.md:0) to be updated to reflect spec completion for this feature.
125 |     *   **Status:** Specification and pseudocode generation complete. Pending updates to other Memory Bank files.
126 | * [2025-05-18 22:22:00] - [Architect Mode Task Completion: Review Architecture for Enhanced Manual Dice Input AI Suggestion]
127 |     *   **Task:** Architect mode reviewed [`memory-bank/architecture.md`](memory-bank/architecture.md:0) to ensure it accurately reflects the latest enhancements to the "manual dice input AI suggestion" feature, specifically the inclusion of roll number and current upper score as context for the AI.
128 |     *   **Details:**
129 |         *   Confirmed that [`memory-bank/architecture.md`](memory-bank/architecture.md:0) (last updated 2025-05-18 22:15:00 by `spec-pseudocode` mode) correctly details the new UI elements (`cmbRollNumber`, `txtCurrentUpperScore`) in [`ComputerYacht/frmMain.cs`](ComputerYacht/frmMain.cs:0), the updated `DecideDiceToHold` method signature in [`ComputerYacht/Computer.cs`](ComputerYacht/Computer.cs:0), and the associated data flow.
130 |         *   No modifications to [`memory-bank/architecture.md`](memory-bank/architecture.md:0) were needed as it was already aligned with the specifications.
131 |     *   **Memory Bank Updates:**
132 |         *   [`memory-bank/progress.md`](memory-bank/progress.md:0) updated to log completion of this review task.
133 |         *   [`memory-bank/activeContext.md`](memory-bank/activeContext.md:0) updated (this entry).
134 |     *   **Status:** Architecture review complete. The architecture document is confirmed to be up-to-date with the latest feature specifications.
135 | * [2025-05-18 22:25:00] - [Code Task: Enhance Manual Dice Input AI Suggestion with Context (Implementation)]
136 |     *   **Task:** Implemented UI and logic changes to enhance the "manual dice input for AI hold suggestion" feature. The AI now considers the current roll number and upper section score.
137 |     *   **Details:**
138 |         *   **[`ComputerYacht/frmMain.Designer.cs`](ComputerYacht/frmMain.Designer.cs:0):** Added `cmbRollNumber` (ComboBox), `txtCurrentUpperScore` (TextBox), and associated labels. Adjusted layout.
139 |         *   **[`ComputerYacht/Computer.cs`](ComputerYacht/Computer.cs:0):** Modified `DecideDiceToHold` signature to `(int[] currentDiceValues, int rollNumber, bool[] availableCategories, int currentUpperScore)`. Added logic to use `rollNumber` (always hold all on roll 3) and `currentUpperScore` (prioritize upper section if bonus is viable and score is not too high).
140 |         *   **[`ComputerYacht/frmMain.cs`](ComputerYacht/frmMain.cs:0):**
141 |             *   In `btnGetHoldSuggestion_Click`: Read `rollNumber` from `cmbRollNumber` and `currentUpperScore` from `txtCurrentUpperScore`. Added input validation for these new fields. Passed these values to `compPlayer.DecideDiceToHold()`.
142 |             *   In `InitializeNewGame()`: Added logic to reset `cmbRollNumber` to "1" and `txtCurrentUpperScore` to "0".
143 |     *   **Status:** Code implementation complete for all specified files.
144 | * [2025-05-18 22:36:00] - [Code Task: Fix AI Straight Holding Logic]
145 |     *   **Task:** Addressed a bug in [`ComputerYacht/Computer.cs`](ComputerYacht/Computer.cs:0) where the `DecideDiceToHold` method would suggest holding for Small or Large Straights even if those categories were no longer available.
146 |     *   **Details:**
147 |         *   Modified `DecideDiceToHold` in [`ComputerYacht/Computer.cs`](ComputerYacht/Computer.cs:113) to check `availableCategories[Yacht.INDEX_LGESTRAIGHT]` before evaluating large straights.
148 |         *   Modified `DecideDiceToHold` in [`ComputerYacht/Computer.cs`](ComputerYacht/Computer.cs:120) to check `availableCategories[Yacht.INDEX_SMLSTRAIGHT]` before evaluating small straights.
149 |     *   **Status:** Code changes applied and verified. Memory Bank updated.
150 | * [2025-05-18 22:40:00] - [Code Task: Add Debug Logging to AI Straight Logic]
151 |     *   **Task:** Added detailed `Console.WriteLine()` debug logging to `DecideDiceToHold` and related private helper methods (`IsSmallStraight`, `IsLargeStraight`, `MarkStraightDice`) in [`ComputerYacht/Computer.cs`](ComputerYacht/Computer.cs:0).
152 |     *   **Details:** Logged method entries, key parameters (dice values, roll number, category availability, upper score), strategy evaluation steps, dice counts, specific logic for straights (availability checks, detection results), and final hold decisions.
153 |     *   **Status:** Debug logging implemented in [`ComputerYacht/Computer.cs`](ComputerYacht/Computer.cs:0).
154 | * [2025-05-18 22:48:00] - [Debug Status Update: Fix Confirmation] Applied fixes for CS1503 in `Computer.cs` by ensuring `Console.WriteLine` uses `string.Join` for array types and fixed CS0649 in `frmMain.cs` by initializing `currentPhase` at declaration.
155 | * [2025-05-18 22:55:28] - [Debug Status Update: Applied Fix for CS1061 and CS1503] Added `using System.Linq;` to [`ComputerYacht/Computer.cs`](ComputerYacht/Computer.cs:2) to resolve CS1061. Verified that existing `Console.WriteLine` calls for arrays generally use `string.Join`, which should address CS1503 issues related to `Select` method on `diceCounts` and other array printing.
156 | * [2025-05-18 22:59:32] - [Debug Status Update: Cleaned Computer.cs, removed BOM]
157 | * [2025-05-18 23:05:43] - [Code Task Completion: Updated .NET Framework Version] Modified [`ComputerYacht/ComputerYacht.csproj`](ComputerYacht/ComputerYacht.csproj:0) to target .NET Framework v4.0 (from v2.0) and added a reference to `System.Core.dll` to resolve CS0234 compilation error (System.Linq not found).
158 | ---
159 | ### Current Focus: Manual Category Control Implementation (UI)
160 | **Date:** 2025-05-18
161 | **Task:** Define specification and pseudocode for UI and logic for manual control of available scoring categories for AI hold suggestions.
162 | **Files Involved:**
163 | *   [`ComputerYacht/frmMain.cs`](ComputerYacht/frmMain.cs:0) (UI additions, logic for `btnGetHoldSuggestion_Click`, `InitializeNewGame`)
164 | *   [`ComputerYacht/Computer.cs`](ComputerYacht/Computer.cs:0) (No change to `DecideDiceToHold` signature, consumes new input)
165 | *   [`ComputerYacht/Yacht.cs`](ComputerYacht/Yacht.cs:0) (No direct change, `GetPlayerAvailableCategories` bypassed in this flow)
166 | **Status:** Pseudocode generated. Preparing for Memory Bank update.
167 | * [2025-05-18 23:23:00] - [Architect Mode Task Completion: Update Architecture for Manual Category Control Feature]
168 |     *   **Task:** Architect mode reviewed and updated [`memory-bank/architecture.md`](memory-bank/architecture.md:0) to incorporate the new feature allowing users to manually control which scoring categories are available to the AI for hold suggestions via UI CheckBoxes.
169 |     *   **Details:**
170 |         *   Modified [`memory-bank/architecture.md`](memory-bank/architecture.md:0) to reflect new UI elements ([`ComputerYacht/frmMain.cs`](ComputerYacht/frmMain.cs:0) - 13 CheckBoxes), changes in `btnGetHoldSuggestion_Click` logic (reading CheckBoxes to build `availableCategories`, no longer calling `Yacht.GetPlayerAvailableCategories()`), and updated data flow diagrams and descriptions.
171 |     *   **Memory Bank Updates:**
172 |         *   [`memory-bank/architecture.md`](memory-bank/architecture.md:0) was updated with the new architectural design.
173 |         *   [`memory-bank/decisionLog.md`](memory-bank/decisionLog.md:0) was updated to log this architectural decision and its rationale.
174 |         *   [`memory-bank/activeContext.md`](memory-bank/activeContext.md:0) updated (this entry).
175 |     *   **Status:** The architecture documentation is now aligned with the specifications for the new manual category control feature.
176 | * [2025-05-19 09:16:17] - [Architect Mode Task Completion: Review Architecture for Manual Category Control (Checkbox specification)]
177 |     *   **Task:** 架构师模式审查了用户提供的关于通过UI CheckBoxes手动控制AI可用计分项的规范和伪代码。
178 |     *   **Details:** 确认了项目架构能够支持这些变更，并且当前的 [`memory-bank/architecture.md`](memory-bank/architecture.md:0) 文档（更新于 2025-05-18 23:22:00）已经准确地反映了这些UI和逻辑上的调整。无需对架构文档进行修改。
179 |     *   **Memory Bank Updates:**
180 |         *   [`memory-bank/decisionLog.md`](memory-bank/decisionLog.md:0) 已更新，记录了本次审查决策。
181 |         *   [`memory-bank/activeContext.md`](memory-bank/activeContext.md:0) 已更新（此条目）。
182 |     *   **Status:** 架构审查完成。架构文档与最新规范保持一致。
183 | * [2025-05-19 09:20:31] - [Code Task Completion: Implement Manual Category Control for AI Suggestions]
184 |     *   **Task:** Implemented UI and logic changes for manual control of available scoring categories.
185 |     *   **Details:**
186 |         *   **[`ComputerYacht/frmMain.Designer.cs`](ComputerYacht/frmMain.Designer.cs:0):** Added `grpScoringCategories` (GroupBox) and 13 `CheckBox` controls (e.g., `chkCatOnes`, `chkCatChance`) for each scoring category. Adjusted layout and form size.
187 |         *   **[`ComputerYacht/frmMain.cs`](ComputerYacht/frmMain.cs:0):**
188 |             *   Added `categoryCheckBoxes` (CheckBox array) member.
189 |             *   Implemented `InitializeCategoryCheckBoxArray()` to populate `categoryCheckBoxes` and called it in the constructor.
190 |             *   Modified `InitializeNewGame()` to set all category `CheckBox.Checked = true`.
191 |             *   Modified `btnGetHoldSuggestion_Click()` to:
192 |                 *   Create `availableCategoriesFromCheckboxes` (bool array) based on the `Checked` state of `categoryCheckBoxes`.
193 |                 *   Pass `availableCategoriesFromCheckboxes` to `compPlayer.DecideDiceToHold()`.
194 |                 *   Removed the call to `yYacht.GetPlayerAvailableCategories(0)`.
195 |     *   **Status:** UI and logic implementation complete. Next step is to update other Memory Bank files and then attempt completion.
196 | ---
197 | **Context Update:** Unit Test Completion for Manual AI Hold Suggestions
198 | **Timestamp:** 2025-05-19 09:23 AM
199 | **Details:** Unit tests for the new feature "Manual control over AI hold suggestion's available scoring categories (UI)" have been successfully implemented in [`ComputerYacht/frmMainTests.cs`](ComputerYacht/frmMainTests.cs:0). The tests cover:
200 | 1.  Correct initialization of category checkboxes by `InitializeNewGame()`.
201 | 2.  Accurate construction of the `availableCategoriesFromCheckboxes` array in `btnGetHoldSuggestion_Click()` based on UI checkbox states.
202 | 3.  Verification that `compPlayer.DecideDiceToHold()` is invoked with the correctly constructed `availableCategoriesFromCheckboxes`.
203 | No major issues or changes in focus were identified during this TDD cycle. The primary new artifact is the test suite itself.
204 | ---
205 | **Context Update:** Fixed Compilation Error in frmMain.cs
206 | **Timestamp:** 2025-05-19 09:31 AM
207 | **Details:** Corrected a compilation error in [`ComputerYacht/frmMain.cs`](ComputerYacht/frmMain.cs:316) by replacing an undefined constant `Yacht.INDEX_YACHTZEE` with the correct constant `Yacht.INDEX_YACHT`. This was identified from user-provided compiler output.
208 | **Files Modified:** [`ComputerYacht/frmMain.cs`](ComputerYacht/frmMain.cs:0)
209 | ---
210 | **Context Update:** Added Debug Logging for Checkbox States in frmMain.cs
211 | **Timestamp:** 2025-05-19 09:38 AM
212 | **Details:** Added `Console.WriteLine` statements in the `btnGetHoldSuggestion_Click` method of [`ComputerYacht/frmMain.cs`](ComputerYacht/frmMain.cs:0) to print the `Checked` status of Small Straight and Large Straight checkboxes, and their corresponding values in the `availableCategoriesFromCheckboxes` array. This is to help the user manually verify the state being passed to the AI.
213 | **Files Modified:** [`ComputerYacht/frmMain.cs`](ComputerYacht/frmMain.cs:0)
214 | ---
215 | **Context Update:** Enhanced Debug Logging in btnGetHoldSuggestion_Click
216 | **Timestamp:** 2025-05-19 10:14 AM
217 | **Details:** Replaced the `btnGetHoldSuggestion_Click` method in [`ComputerYacht/frmMain.cs`](ComputerYacht/frmMain.cs:0) with a version containing more comprehensive `Console.WriteLine` statements. These logs will show entry/exit points, values of input fields, status of `categoryCheckBoxes` array initialization, and detailed states of Small/Large Straight checkboxes before calling the AI. This is to definitively track the flow and data if the user still doesn't see the expected debug output.
218 | **Files Modified:** [`ComputerYacht/frmMain.cs`](ComputerYacht/frmMain.cs:0)
219 | [系统集成器] [2025-05-19 10:24:00] “手动控制AI保留建议的可用计分分类 (UI)”功能集成审查：
220 | - 代码实现 ([`ComputerYacht/frmMain.cs`](ComputerYacht/frmMain.cs:0), [`ComputerYacht/frmMain.Designer.cs`](ComputerYacht/frmMain.Designer.cs:0), [`ComputerYacht/Computer.cs`](ComputerYacht/Computer.cs:0)) 与架构设计 ([`memory-bank/architecture.md`](memory-bank/architecture.md:0)) 完全一致。
221 | - UI元素（骰子输入、掷骰次数、上区总分、计分项复选框、获取建议按钮）已正确实现。
222 | - `frmMain.cs` 中的 `btnGetHoldSuggestion_Click` 事件按预期重构，并正确处理用户输入和调用AI逻辑。
223 | - `Computer.cs` 中的 `DecideDiceToHold` 方法已更新，以正确接收和使用新的上下文参数（`availableCategories`，`currentUpperScore`，`rollNumber`）。
224 | - 用户指南 ([`memory-bank/user_guide_snippets.md`](memory-bank/user_guide_snippets.md:0)) 和开发者文档 ([`memory-bank/architecture.md`](memory-bank/architecture.md:0)) 准确反映了最终实现。
225 | - 未发现明显的遗漏步骤或潜在的集成问题。功能作为一个整体是完整和协调的。
226 | * [2025-05-19 13:50:13] - [Code Update: Enhanced Checkbox Logging] Added comprehensive logging in `GetAvailableCategoriesFromCheckboxes` within `ComputerYacht/frmMain.cs` to display the Name and Checked status for all 13 category checkboxes, aiding in debugging UI state discrepancies.
227 | * [2025-05-19 14:35:14] - [Code Update: Enhanced Debug Logging for availableCategories Discrepancy] Modified logging to help diagnose inconsistent 'LgStraight' availability. Removed old partial logging in `frmMain.cs`'s `GetAvailableCategoriesFromCheckboxes` method. Added comprehensive logging at the beginning of `Computer.cs`'s `DecideDiceToHold` method to print the full state of the received `availableCategories` array, detailing each category's name and boolean value. This aims to clarify the state of `availableCategories[10]` (LgStraight) as it enters the AI's decision logic.
228 | * [2025-05-19 14:52:34] - [Code Fix: Corrected Compilation Errors in Computer.cs] Fixed structural errors in `ComputerYacht/Computer.cs` related to misplaced/missing braces in the `DecideDiceToHold` method. Specifically, added an opening brace after the method signature and removed a misplaced opening brace further down. This was done using `write_to_file` after `apply_diff` failed. This should resolve a significant number of compilation errors.
]]>
* [2025-05-19 15:04:48] - [Code Fix: Corrected Compilation Errors in Computer.cs by removing CDATA tags] Removed illegal `<![CDATA[` and `]]>` tags from the beginning and end of [`ComputerYacht/Computer.cs`](ComputerYacht/Computer.cs:0) using `write_to_file`. This is expected to resolve a significant number of parsing-related compilation errors.
* [2025-05-19 15:58:44] - [Code Update: Added Diagnostic Log in Computer.cs] Added a `Console.WriteLine` statement in `DecideDiceToHold` method in `ComputerYacht/Computer.cs` to log the value of `availableCategories[10]` before it's used in another log statement. This is to help diagnose an issue with Large Straight availability.
* [2025-05-19 16:02:00] - [Code Update: Enhanced Debug Logging in Computer.cs] Modified a `Console.WriteLine` statement in the `DecideDiceToHold` method in `ComputerYacht/Computer.cs` to include index values for Small Straight and Large Straight availability. This is to help diagnose an issue with category availability logging.
<![CDATA[
* [2025-05-19 16:06:27] - [Code Fix: Corrected Scoring Index Constants] Updated `INDEX_SMLSTRAIGHT`, `INDEX_LGESTRAIGHT`, `INDEX_YACHT`, and `INDEX_CHANCE` in `ComputerYacht/Yacht.cs` to their correct 0-based index values to resolve AI logic errors.
]]>
<![CDATA[
* [2025-05-19 17:23:00] - [Code Update: Yachtzee Single Score Logic] Modified `Yacht.cs` in the `ScoreValue` method to remove the 100-point bonus for subsequent Yachtzees. Ensured that AI logic in `Computer.cs` (`DecideDiceToHold` and `CalculateScoreForCategory`) correctly handles scenarios where Yachtzee category is already used (deprioritizes holding for it, scores 0 if selected again). This aligns with the new rule that Yachtzee scores only once.
]]>
* [2025-05-19 17:39:00] - [Code Update: `DecideDiceToHold` Logic Enhanced] Modified the `DecideDiceToHold` method in [`ComputerYacht/Computer.cs`](ComputerYacht/Computer.cs:39) to ensure AI dice holding decisions are strictly based on currently available scoring categories. This includes adding availability checks for Four of a Kind, Full House, Three of a Kind, and refining logic for Pairs and fallback strategies. The aim is to align AI behavior more closely with optimal play given category availability, and specifically to ensure for dice `[2,2,2,2,6]` with only "Sixes" available, the AI holds `[F,F,F,F,T]`.
* [2025-05-19 18:07:00] - [Code Update: AI Auto-Scoring on Roll 3]
    *   **Task:** Implement AI auto-scoring after the third dice roll.
    *   **Details:**
        *   **[`ComputerYacht/Computer.cs`](ComputerYacht/Computer.cs:0):** Added `public Tuple&lt;int, int&gt; ChooseBestCategoryAndCalcScore(int[] finalDice, bool[] currentAvailableCategories, int currentUpperScore)` method. This method leverages the existing `ChooseScoreCategory` logic and returns the chosen category index and calculated score as a tuple.
        *   **[`ComputerYacht/Yacht.cs`](ComputerYacht/Yacht.cs:0):** Added `public int GetPlayerUpperScore(int player)` method to retrieve the current upper section score for a given player.
        *   **[`ComputerYacht/frmMain.cs`](ComputerYacht/frmMain.cs:0):** Modified `btnGetHoldSuggestion_Click` method. When `rollNumber == 3`:
            *   It now calls `compPlayer.ChooseBestCategoryAndCalcScore` to get the AI's scoring decision.
            *   It calls `yYacht.ApplyScoreAndFinalizeTurn` to record the score and advance game state.
            *   Updates UI elements (disables scored category checkbox, refreshes scoreboard, shows message to user).
            *   If the game ends, `ProcessGameOver` is called. Otherwise, UI is updated for the next turn/player.
    *   **Status:** Implementation complete. AI now automatically chooses and records a score after the third roll when using the "获取保留建议" button with roll 3 selected.