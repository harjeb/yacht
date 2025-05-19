using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Windows.Forms; // Required for CheckBox and other UI elements

namespace ComputerYacht.Tests
{
    // Fake Computer class for testing purposes
    public class FakeComputer : Computer
    {
        public bool DecideDiceToHoldCalled { get; private set; } = false;
        public bool[] LastAvailableCategories { get; private set; }

        public FakeComputer(Random rand) : base(rand) { }

        public override void DecideDiceToHold(bool[] availableCategories, int[] dicesValue, bool[] dicesHold, int rollIndex, int turnScore, int totalScore)
        {
            DecideDiceToHoldCalled = true;
            LastAvailableCategories = availableCategories;
            // Minimal implementation, or can be left empty if only tracking calls
        }

        // Add any other methods or properties needed for testing if Computer class has them as abstract or virtual
    }

    [TestClass]
    public class FrmMainTests
    {
        private frmMain mainForm;
        private CheckBox[] categoryCheckBoxes; // To access the checkboxes for assertions

        // Helper to access private categoryCheckBoxes array via reflection for testing
        private CheckBox[] GetCategoryCheckBoxes(frmMain form)
        {
            var fieldInfo = typeof(frmMain).GetField("categoryCheckBoxes", System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Instance);
            if (fieldInfo != null)
            {
                return (CheckBox[])fieldInfo.GetValue(form);
            }
            throw new InvalidOperationException("Could not find private field 'categoryCheckBoxes'.");
        }
        
        // Helper to access private compPlayer field via reflection for testing and setting the fake
        private void SetComputerPlayer(frmMain form, Computer player)
        {
            var fieldInfo = typeof(frmMain).GetField("compPlayer", System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Instance);
            if (fieldInfo != null)
            {
                fieldInfo.SetValue(form, player);
            }
            else
            {
                throw new InvalidOperationException("Could not find private field 'compPlayer'.");
            }
        }


        [TestInitialize]
        public void TestInitialize()
        {
            mainForm = new frmMain(); // This will call InitializeComponent which should init checkboxes
            
            // InitializeCategoryCheckBoxArray is called in frmMain constructor after InitializeComponent,
            // so checkboxes should be available.
            // We need to access the private categoryCheckBoxes field for our tests.
            categoryCheckBoxes = GetCategoryCheckBoxes(mainForm);
        }

        [TestMethod]
        public void InitializeNewGame_SetsAllCategoryCheckBoxesToTrue()
        {
            // Arrange
            // Set some checkboxes to false to ensure InitializeNewGame changes them
            if (categoryCheckBoxes.Length > 0)
            {
                categoryCheckBoxes[0].Checked = false;
            }
            if (categoryCheckBoxes.Length > 1)
            {
                categoryCheckBoxes[1].Checked = false;
            }

            // Act
            mainForm.InitializeNewGame(); // This method should set all checkboxes to true

            // Assert
            foreach (var checkBox in categoryCheckBoxes)
            {
                Assert.IsTrue(checkBox.Checked, $"CheckBox {checkBox.Name} should be checked after InitializeNewGame.");
            }
    
            [TestMethod]
            public void BtnGetHoldSuggestion_Click_BuildsAvailableCategoriesCorrectly()
            {
                // Arrange
                var fakeComputer = new FakeComputer(new Random());
                SetComputerPlayer(mainForm, fakeComputer); // Inject the fake computer
    
                // Set specific checkbox states
                // Assuming there are at least 13 checkboxes as per frmMain.cs
                bool[] expectedCategories = new bool[13];
                for (int i = 0; i < categoryCheckBoxes.Length && i < expectedCategories.Length; i++)
                {
                    if (i % 2 == 0)
                    {
                        categoryCheckBoxes[i].Checked = true;
                        expectedCategories[i] = true;
                    }
                    else
                    {
                        categoryCheckBoxes[i].Checked = false;
                        expectedCategories[i] = false;
                    }
                }
    
                // Act
                // Call btnGetHoldSuggestion_Click using reflection as it's private
                var methodInfo = typeof(frmMain).GetMethod("btnGetHoldSuggestion_Click", System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Instance);
                if (methodInfo == null)
                {
                    throw new InvalidOperationException("Could not find private method 'btnGetHoldSuggestion_Click'.");
                }
                methodInfo.Invoke(mainForm, new object[] { null, EventArgs.Empty }); // Parameters for event handler
    
                // Assert
                Assert.IsTrue(fakeComputer.DecideDiceToHoldCalled, "DecideDiceToHold should have been called.");
                Assert.IsNotNull(fakeComputer.LastAvailableCategories, "LastAvailableCategories should not be null.");
                Assert.AreEqual(expectedCategories.Length, fakeComputer.LastAvailableCategories.Length, "Available categories array length mismatch.");
                for (int i = 0; i < expectedCategories.Length; i++)
                {
                    Assert.AreEqual(expectedCategories[i], fakeComputer.LastAvailableCategories[i], $"Category at index {i} mismatch.");
                }
        
                [TestMethod]
                public void BtnGetHoldSuggestion_Click_CallsDecideDiceToHoldWithCorrectCategories()
                {
                    // Arrange
                    var fakeComputer = new FakeComputer(new Random());
                    SetComputerPlayer(mainForm, fakeComputer); // Inject the fake computer
        
                    // Set a specific, easily verifiable checkbox state
                    bool[] expectedCategories = new bool[13];
                    for (int i = 0; i < categoryCheckBoxes.Length && i < expectedCategories.Length; i++)
                    {
                        if (i == 0 || i == 5 || i == 12) // Example: Check only Ones, Sixes, and Chance
                        {
                            categoryCheckBoxes[i].Checked = true;
                            expectedCategories[i] = true;
                        }
                        else
                        {
                            categoryCheckBoxes[i].Checked = false;
                            expectedCategories[i] = false;
                        }
                    }
                    
                    // Initialize dice values and holds as btnGetHoldSuggestion_Click might use them
                    // Accessing iDicesValue and bDicesHold from frmMain. If they are private, reflection would be needed
                    // or ensure they are initialized by a public method if possible.
                    // For simplicity, assuming they are accessible or initialized to a default state that doesn't break the method.
                    // If frmMain.iDicesValue or frmMain.bDicesHold are not directly settable or initialized,
                    // this part might need adjustment based on frmMain's actual implementation.
                    // For now, we focus on availableCategories.
        
                    // Act
                    var methodInfo = typeof(frmMain).GetMethod("btnGetHoldSuggestion_Click", System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Instance);
                    if (methodInfo == null)
                    {
                        throw new InvalidOperationException("Could not find private method 'btnGetHoldSuggestion_Click'.");
                    }
                    methodInfo.Invoke(mainForm, new object[] { null, EventArgs.Empty });
        
                    // Assert
                    Assert.IsTrue(fakeComputer.DecideDiceToHoldCalled, "DecideDiceToHold should have been called on the computer player.");
                    Assert.IsNotNull(fakeComputer.LastAvailableCategories, "The available categories passed to DecideDiceToHold should not be null.");
                    Assert.AreEqual(expectedCategories.Length, fakeComputer.LastAvailableCategories.Length, "The length of available categories array is incorrect.");
                    for (int i = 0; i < expectedCategories.Length; i++)
                    {
                        Assert.AreEqual(expectedCategories[i], fakeComputer.LastAvailableCategories[i],
                            $"The available category at index {i} (CheckBox: {categoryCheckBoxes[i].Name}) was expected to be {expectedCategories[i]} but was {fakeComputer.LastAvailableCategories[i]}.");
                    }
                }
            }
        }
    }
}