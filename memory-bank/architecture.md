# Yacht 游戏模拟器软件架构

**日期:** 2025-05-18 (更新于: 2025-05-18 21:25:00)

## 1. 架构概述

Yacht 游戏模拟器采用以 Windows Forms 为中心的事件驱动架构。其核心模拟流程，经过最近的细化，允许用户通过 "手动单步模拟" 按钮点击，手动逐步执行计算机玩家回合中的每一个具体步骤（例如，第一次掷骰、AI决策保留骰子、第二次掷骰、AI决策选择计分项等）。此外，还新增了允许用户手动输入5个骰子点数并获取AI保留建议的功能。虽然没有严格的分层，但可以观察到以下概念层：

*   **表示层 (Presentation Layer):** 由 [`ComputerYacht/frmMain.cs`](ComputerYacht/frmMain.cs:0) 处理，负责用户界面、事件处理（如按钮点击）以及统计数据的显示。
*   **应用逻辑/领域层 (Application Logic/Domain Layer):**
    *   [`ComputerYacht/Yacht.cs`](ComputerYacht/Yacht.cs:0): 实现核心的 Yacht 游戏规则，包括掷骰子、计分、回合管理等。
    *   [`ComputerYacht/Computer.cs`](ComputerYacht/Computer.cs:0): 包含复杂的计算机 AI 策略，用于决定保留哪些骰子以及选择最佳的计分组合。
*   **实用/辅助层 (Utility/Helper Layer):**
    *   [`ComputerYacht/Dice.cs`](ComputerYacht/Dice.cs:0): 提供骰子点数出现概率的精确计算。

该架构的核心是一个模拟引擎，允许用户通过 "手动单步模拟" 按钮，细致地观察并控制AI玩家在游戏每一回合中各个决策阶段的逐步进展，并且也支持直接输入骰子获取AI的即时保留建议。

## 2. 主要功能实现及模块交互

以下是主要功能在架构中的实现方式以及模块间的交互：

### 2.1. 游戏引擎与规则 (`Yacht.cs`)

*   **功能:**
    *   管理游戏状态（当前玩家、掷骰子次数、分数）。
    *   实现标准的 5 骰子掷骰机制（可能受到 `ROLL_DICES` 查找表的影响）。
    *   管理 13 个标准计分类型。
    *   计算上半区奖励和多次 Yachtzee 奖励。
    *   原有的 `ComputerNextMove` 方法（执行一整轮操作）的逻辑已被分解。现在提供更细粒度的方法来支持分步执行：
        *   `PerformRoll(rollAttemptInTurn AS INTEGER) AS INTEGER[5]`: 执行第 `rollAttemptInTurn` 次掷骰，更新内部骰子状态，并返回骰子结果。
        *   `ApplyHoldDecision(diceToHoldFromAI[5] AS BOOLEAN)`: 应用AI的保留决策，更新 `bDicesHold` 状态。
        *   `ApplyScoreAndFinalizeTurn(categoryIndexToScore AS INTEGER, actualScore AS INTEGER) AS BOOLEAN`: 记录分数，更新奖励，检查游戏是否结束，为下一回合做准备。
        *   `GetCurrentDiceValues() AS INTEGER[5]`, `GetCurrentHeldDice() AS BOOLEAN[5]`, `GetCurrentRollNumberInTurn() AS INTEGER`, `GetAvailableCategories() AS BOOLEAN[]`, `IsGameOver() AS BOOLEAN` 等访问器方法，供 [`ComputerYacht/frmMain.cs`](ComputerYacht/frmMain.cs:0) 查询状态和更新UI。
        *   `SetManuallyEnteredDice(diceValues AS INTEGER[5])`: 新增方法，允许外部（如UI）直接设置骰子的当前值。此方法会更新内部的 `iDicesValue`，并可能需要重置 `iRollIndex` 和 `bDicesHold` 以反映这是一个“最终”的骰子状态，等待AI决策。
*   **交互:**
    *   与 [`ComputerYacht/frmMain.cs`](ComputerYacht/frmMain.cs:0) 紧密交互，由 `frmMain.cs` 调用其细粒度的公共方法（如 `PerformRoll`, `ApplyHoldDecision`, `ApplyScoreAndFinalizeTurn`）来逐步推进游戏回合。
    *   通过其访问器方法向 `frmMain.cs` 提供当前游戏状态（骰子、保留状态、分数、可用计分项等）以更新UI。
    *   不再直接调用 [`ComputerYacht/Computer.cs`](ComputerYacht/Computer.cs:0) 进行决策；AI决策的调用现在由 `frmMain.cs` 在适当的步骤协调。
    *   对于手动输入骰子功能，`frmMain.cs` 会调用新增的 `SetManuallyEnteredDice()` 方法。

### 2.2. 计算机 AI 玩家 (`Computer.cs`)

*   **功能:**
    *   提供细粒度的决策方法，由 [`ComputerYacht/frmMain.cs`](ComputerYacht/frmMain.cs:0) 在适当的回合步骤中调用：
        *   `ComputerDecideDiceToHold(currentDiceValues[5] AS INTEGER, rollNumber AS INTEGER) AS BOOLEAN[5]`: 根据当前骰子和掷骰次数，决定要保留的骰子。
        *   `ComputerSelectScoringCategory(finalDiceValues[5] AS INTEGER, availableCategories AS BOOLEAN[]) AS ScoringDecision`: 在最终掷骰后，根据可用计分项选择最佳计分类型和计算得分。
    *   利用详细的内部游戏状态表示和计分机会（65 个“计算机分数索引”）。
    *   使用预定义的权重和概率计算（通过 [`ComputerYacht/Dice.cs`](ComputerYacht/Dice.cs:0) 的穷举法精确计算）来指导决策。
    *   实现必要时牺牲低价值计分类型的策略。
*   **交互:**
    *   其决策方法 (`ComputerDecideDiceToHold`, `ComputerSelectScoringCategory`) 由 [`ComputerYacht/frmMain.cs`](ComputerYacht/frmMain.cs:0) 调用。
    *   大量使用 [`ComputerYacht/Dice.cs`](ComputerYacht/Dice.cs:0) 来计算不同骰子组合和保留策略的精确概率。
    *   其决策结果（保留的骰子数组、选择的计分项及分数）返回给 [`ComputerYacht/frmMain.cs`](ComputerYacht/frmMain.cs:0)，后者再指示 [`ComputerYacht/Yacht.cs`](ComputerYacht/Yacht.cs:0) 应用这些决策。
    *   在手动输入骰子功能中，`ComputerDecideDiceToHold()` 方法会接收由 `Yacht.cs` 传递过来的、用户手动输入的骰子值。

### 2.3. 模拟与统计 (`frmMain.cs`)

*   **功能:**
    *   **管理手动单步模拟的状态机:** 通过内部状态变量 (如 `currentPhase` of type `TurnStepPhase`) 跟踪计算机玩家当前回合的具体步骤（例如，准备第一次掷骰、等待AI保留决策、准备第二次掷骰等）。
    *   **响应 "手动单步模拟" 按钮点击:** 根据 `currentPhase`，执行回合中的下一个逻辑步骤。这包括：
        *   调用 [`ComputerYacht/Yacht.cs`](ComputerYacht/Yacht.cs:0) 的 `PerformRoll()` 方法。
        *   调用 [`ComputerYacht/Computer.cs`](ComputerYacht/Computer.cs:0) 的 `ComputerDecideDiceToHold()` 方法，然后调用 [`ComputerYacht/Yacht.cs`](ComputerYacht/Yacht.cs:0) 的 `ApplyHoldDecision()`。
        *   调用 [`ComputerYacht/Computer.cs`](ComputerYacht/Computer.cs:0) 的 `ComputerSelectScoringCategory()` 方法，然后调用 [`ComputerYacht/Yacht.cs`](ComputerYacht/Yacht.cs:0) 的 `ApplyScoreAndFinalizeTurn()`。
    *   **UI 更新:** 在每个步骤后，从 [`ComputerYacht/Yacht.cs`](ComputerYacht/Yacht.cs:0) 获取最新状态（骰子值、保留状态、分数、提示信息）并更新界面显示。
    *   管理游戏开始、回合结束和游戏结束的转换。
    *   在游戏结束时收集并显示统计数据，并将游戏分数记录到 "Games.txt"。
    *   **新增手动骰子输入与建议功能:**
        *   提供5个文本输入框供用户输入1-6之间的骰子点数。
        *   提供一个“获取建议”按钮。
        *   当按钮点击时，读取输入框中的骰子点数，进行验证。
        *   调用 `yYacht.SetManuallyEnteredDice()` 将验证后的骰子值传递给 `Yacht` 对象。
        *   调用 `compPlayer.ComputerDecideDiceToHold(yYacht.GetCurrentDiceValues(), 3)` (假设手动输入等同于第三次掷骰后的决策点，或者根据实际情况调整rollNumber参数)。
        *   接收 `Computer.cs` 返回的建议保留的骰子 (boolean array)。
        *   在UI上高亮或标记建议保留的骰子。
*   **交互:**
    *   实例化并管理 [`ComputerYacht/Yacht.cs`](ComputerYacht/Yacht.cs:0) 和 [`ComputerYacht/Computer.cs`](ComputerYacht/Computer.cs:0) 对象。
    *   响应用户点击 "手动单步模拟" 按钮。
    *   协调对 [`ComputerYacht/Yacht.cs`](ComputerYacht/Yacht.cs:0) 和 [`ComputerYacht/Computer.cs`](ComputerYacht/Computer.cs:0) 中细粒度方法的调用。
    *   对于手动输入骰子功能，会调用 `yYacht.SetManuallyEnteredDice()` 和 `compPlayer.ComputerDecideDiceToHold()`。
    *   直接进行文件 I/O 以记录游戏数据。

### 2.4. 骰子概率计算 (`Dice.cs`)

*   **功能:**
    *   提供一个静态方法 (`CalculateExactChance`)，通过穷举所有可能的骰子结果来计算获得特定骰子组合（例如，三条、四条、顺子）的确切概率。
*   **交互:**
    *   主要由 [`ComputerYacht/Computer.cs`](ComputerYacht/Computer.cs:0) 调用，为其决策过程提供关键的概率数据。

## 3. 数据流和控制流 (手动单步回合内模拟)

1.  **初始化/新游戏:**
    a.  [`ComputerYacht/frmMain.cs`](ComputerYacht/frmMain.cs:0) 实例化 `Yacht` 和 `Computer` 对象。
    b.  调用 `yYacht.ResetYacht()`。
    c.  `frmMain.cs` 设置其内部状态 `currentPhase` 为 `READY_FOR_ROLL_1`。
    d.  UI 更新以显示初始状态和提示信息 ("点击按钮进行第一次掷骰")。

2.  **用户点击 "手动单步模拟" 按钮:** [`ComputerYacht/frmMain.cs`](ComputerYacht/frmMain.cs:0) 的 `btnManualStep_Click` 事件触发。
    *   **IF `currentPhase` is `READY_FOR_ROLL_1` (第一次掷骰):**
        i.  `frmMain.cs` 调用 `yYacht.PerformRoll(1)`。
        ii. `yYacht` 掷所有5个骰子，更新其内部 `iDicesValue`，`iRollIndex` 设为0。返回骰子值。
        iii. `frmMain.cs` 接收骰子值，更新UI显示骰子。
        iv. `frmMain.cs` 将 `currentPhase` 更新为 `AWAITING_HOLD_DECISION_1`。
        v.  `frmMain.cs` 自动调用 `compPlayer.ComputerDecideDiceToHold(yYacht.GetCurrentDiceValues(), 1)`。
        vi. `compPlayer` 返回建议保留的骰子 (boolean array)。
        vii. `frmMain.cs` 调用 `yYacht.ApplyHoldDecision(heldDiceFromAI)`。
        viii. `yYacht` 更新其内部 `bDicesHold`。
        ix. `frmMain.cs` 更新UI显示保留的骰子和提示信息 ("AI已决策，点击进行第二次掷骰")。
        x.  `frmMain.cs` 将 `currentPhase` 更新为 `READY_FOR_ROLL_2`。
    *   **IF `currentPhase` is `READY_FOR_ROLL_2` (第二次掷骰):**
        i.  `frmMain.cs` 调用 `yYacht.PerformRoll(2)`。
        ii. `yYacht` 根据 `bDicesHold` 掷未保留的骰子，更新 `iDicesValue`，`iRollIndex` 设为1。返回骰子值。
        iii. (类似步骤 iii-x 上述，但 `rollNumber` 为2, `currentPhase` 变为 `AWAITING_HOLD_DECISION_2` 然后 `READY_FOR_ROLL_3`)。
    *   **IF `currentPhase` is `READY_FOR_ROLL_3` (第三次掷骰):**
        i.  `frmMain.cs` 调用 `yYacht.PerformRoll(3)`。
        ii. `yYacht` 根据 `bDicesHold` 掷未保留的骰子，更新 `iDicesValue`，`iRollIndex` 设为2。返回骰子值。
        iii. `frmMain.cs` 更新UI显示骰子。
        iv. `frmMain.cs` 将 `currentPhase` 更新为 `AWAITING_SCORING_DECISION`。
        v.  `frmMain.cs` 自动调用 `compPlayer.ComputerSelectScoringCategory(yYacht.GetCurrentDiceValues(), yYacht.GetAvailableCategories())`。
        vi. `compPlayer` 返回 `ScoringDecision` (选择的计分项索引和分数)。
        vii. `frmMain.cs` 调用 `yYacht.ApplyScoreAndFinalizeTurn(decision.bestCategoryIndex, decision.scoreForCategory)`。
        viii. `yYacht` 更新分数、奖励、`bScoreRecorded`，`iCurrentTurnNumber` 增加，重置 `iRollIndex` 和 `bDicesHold`。返回游戏是否结束。
        ix. `frmMain.cs` 更新UI显示分数和提示信息 ("AI已计分...")。
        x.  IF 游戏结束 (`yYacht.IsGameOver()` is true):
            `frmMain.cs` 将 `currentPhase` 更新为 `GAME_OVER`。显示最终统计和消息 ("游戏结束，点击开始新游戏")。记录分数到文件。
        xi. ELSE (游戏未结束):
            `frmMain.cs` 将 `currentPhase` 更新为 `TURN_COMPLETED` (或直接到 `READY_FOR_ROLL_1` for next turn)。显示消息 ("回合结束，点击开始下一回合")。
    *   **IF `currentPhase` is `TURN_COMPLETED` (且游戏未结束):**
        i.  `frmMain.cs` 将 `currentPhase` 更新为 `READY_FOR_ROLL_1`。
        ii. UI 更新提示开始新回合。
    *   **IF `currentPhase` is `GAME_OVER`:**
        i.  `frmMain.cs` 调用 `InitializeNewGame()`，流程回到步骤1。

3.  **等待下一次用户点击 "手动单步模拟" 按钮。**

### 3.1. 数据流和控制流 (手动输入骰子获取AI建议)

```mermaid
sequenceDiagram
    participant User
    participant frmMain as frmMain.cs (UI)
    participant yYacht as Yacht.cs
    participant compPlayer as Computer.cs

    User->>frmMain: 输入5个骰子点数
    User->>frmMain: 点击 "获取建议" 按钮
    frmMain->>frmMain: 读取并验证骰子点数
    alt 输入有效
        frmMain->>yYacht: SetManuallyEnteredDice(diceValues)
        yYacht->>yYacht: 更新内部骰子状态 (iDicesValue)
        frmMain->>compPlayer: DecideDiceToHold(yYacht.GetCurrentDiceValues(), rollNumber=3)
        compPlayer->>compPlayer: 计算最佳保留策略
        compPlayer-->>frmMain: 返回建议保留的骰子 (boolean[])
        frmMain->>frmMain: 更新UI显示建议
    else 输入无效
        frmMain->>User: 显示错误信息
    end
```

1.  **用户操作:**
    a.  用户在UI ([`ComputerYacht/frmMain.cs`](ComputerYacht/frmMain.cs:0)) 提供的5个输入框中输入骰子点数。
    b.  用户点击“获取建议”按钮。
2.  **UI 处理 ([`ComputerYacht/frmMain.cs`](ComputerYacht/frmMain.cs:0)):**
    a.  事件处理器被触发。
    b.  读取5个输入框的值。
    c.  验证输入值是否为1-6之间的有效骰子点数。
    d.  如果验证失败，向用户显示错误提示。
    e.  如果验证成功，将5个骰子点数（`diceValues`）传递给 `yYacht.SetManuallyEnteredDice(diceValues)`。
3.  **游戏逻辑处理 ([`ComputerYacht/Yacht.cs`](ComputerYacht/Yacht.cs:0)):**
    a.  `SetManuallyEnteredDice(diceValues)` 方法被调用。
    b.  内部的骰子状态 `iDicesValue` 被更新为用户提供的值。
    c.  可能需要将 `iRollIndex` 设置为2（模拟第三次掷骰后的状态）或类似逻辑，以确保AI决策基于“最终”骰子。 `bDicesHold` 可能会被清空。
4.  **AI 决策请求 ([`ComputerYacht/frmMain.cs`](ComputerYacht/frmMain.cs:0)):**
    a.  `frmMain.cs` 调用 `compPlayer.ComputerDecideDiceToHold(yYacht.GetCurrentDiceValues(), 3)`。参数 `rollNumber` 设为3（或适合的值）表示这是最终掷骰后的决策。
5.  **AI 计算 ([`ComputerYacht/Computer.cs`](ComputerYacht/Computer.cs:0)):**
    a.  `ComputerDecideDiceToHold()` 方法接收手动设置的骰子值和掷骰次数。
    b.  AI执行其决策逻辑，计算出建议保留的骰子。
    c.  返回一个布尔数组 (`suggestedHoldDice`) 给 `frmMain.cs`。
6.  **UI 显示建议 ([`ComputerYacht/frmMain.cs`](ComputerYacht/frmMain.cs:0)):**
    a.  `frmMain.cs` 接收到 `suggestedHoldDice`。
    b.  更新UI，例如高亮显示建议保留的骰子图片或文本。
7.  **流程结束，等待用户下一步操作。** (例如，用户可能会根据建议玩游戏，或再次输入不同的骰子)

## 4. 架构模式和原则

根据 [`memory-bank/systemPatterns.md`](memory-bank/systemPatterns.md:0) 的分析（部分模式可能因移除计时器而减弱或改变）：

*   **事件驱动架构:** UI 事件（按钮点击）驱动应用流程。
*   **策略模式 (隐式):** [`ComputerYacht/Computer.cs`](ComputerYacht/Computer.cs:0) 封装了 AI 策略。
*   **状态模式 (显式/隐式):** [`ComputerYacht/Yacht.cs`](ComputerYacht/Yacht.cs:0) 隐式管理整体游戏状态。[`ComputerYacht/frmMain.cs`](ComputerYacht/frmMain.cs:0) 现在显式管理当前回合内具体步骤的状态 (`TurnStepPhase`)，其行为（调用的方法）根据此状态而改变。
*   **观察者模式 (手动):** [`ComputerYacht/frmMain.cs`](ComputerYacht/frmMain.cs:0) 手动观察 [`ComputerYacht/Yacht.cs`](ComputerYacht/Yacht.cs:0) 的状态并更新 UI。
*   **常量配置:** AI 行为在很大程度上由 [`ComputerYacht/Computer.cs`](ComputerYacht/Computer.cs:0) 中的静态常量数组驱动。
*   **实用工具类:** [`ComputerYacht/Dice.cs`](ComputerYacht/Dice.cs:0) 是一个提供特定功能的静态类。

## 5. 总结

Yacht 游戏模拟器的架构以功能为中心，模块化程度合理。[`ComputerYacht/frmMain.cs`](ComputerYacht/frmMain.cs:0) 作为表示层和精细控制中心，管理回合内步骤的状态机（用于逐步模拟）和处理用户直接输入（用于手动骰子建议），并协调对其他模块的调用。[`ComputerYacht/Yacht.cs`](ComputerYacht/Yacht.cs:0) 封装核心游戏规则和状态，并提供细粒度的操作方法以及手动设置骰子的接口。[`ComputerYacht/Computer.cs`](ComputerYacht/Computer.cs:0) 实现AI决策逻辑，同样提供细粒度的决策方法，这些方法现在可以处理来自模拟掷骰或用户手动输入的骰子数据。[`ComputerYacht/Dice.cs`](ComputerYacht/Dice.cs:0) 提供概率计算支持。模块间的交互现在更加细化，由 `frmMain.cs` 通过按钮点击逐步驱动或响应特定功能请求。