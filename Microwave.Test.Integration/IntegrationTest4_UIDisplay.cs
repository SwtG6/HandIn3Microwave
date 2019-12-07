using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MicrowaveOvenClasses.Boundary;
using MicrowaveOvenClasses.Controllers;
using MicrowaveOvenClasses.Interfaces;
using NSubstitute;
using NUnit.Framework;

namespace Microwave.Test.Integration
{
    [TestFixture]

    class IntegrationTest4_UIDisplay
    {
        private IUserInterface _uut;

        private IButton _startCancelButton;
        private IButton _powerButton;
        private IButton _timeButton;
        private IDoor _door;

        private ILight _light;
        private IDisplay _display;

        private ICookController _cookController;
        private IPowerTube _powerTube;
        private ITimer _timer;

        private IOutput _output;

        #region SetUp

        [SetUp]
        public void SetUp()
        {
            _startCancelButton = new Button();
            _powerButton = new Button();
            _timeButton = new Button();
            _door = new Door();

            _cookController = Substitute.For<ICookController>();
            _powerTube = Substitute.For<IPowerTube>();
            _timer = Substitute.For<ITimer>();

            _output = Substitute.For<IOutput>();

            _light = new Light(_output);
            _display = new Display(_output);

            _uut = new UserInterface
            (
                _powerButton,
                _timeButton,
                _startCancelButton,
                _door,
                _display,
                _light,
                _cookController
             );

        }

        #endregion SetUp

        #region Tests

        [Test] // Test 1: Ved et enkelt tryk på PowerButton viser displayet den rigtige værdi i W. UC 6.
        public void DisplayShowsCorrectPower_OutputTest()
        {
            _powerButton.Press();

            _output.Received(1).OutputLine($"Display shows: 50 W");
        }

        [TestCase(3, 150)]
        [TestCase(5, 250)]
        [TestCase(10, 500)]
        // Test 2: Ved flere tryk på PowerButton viser displayet den rigtige værdi i W. UC 6.
        public void DisplayShowsCorrectPowerMultiplePresses_OutputTest(int NumberOfPresses, int PowerLevel)
        {
            NumberOfPresses = 3;
            PowerLevel = 150;

            for(int i = 0; i < NumberOfPresses; i++)
            {
                _powerButton.Press();
            }

            _output.Received(1).OutputLine($"Display shows: {PowerLevel} W");
        }

        [Test] // Test 3: Ved et enkelt tryk på TimeButton viser displayet den rigtige værdi for tidsindstillingen. UC 7.
        public void DisplayShowsCorrectTime_OutputTest()
        {
            _powerButton.Press();
            _timeButton.Press();

            _output.Received(1).OutputLine($"Display shows: {01:D2}:{00:D2}");
        }

        [TestCase(3, 03)]
        [TestCase(5, 05)]
        [TestCase(10, 10)]
        // Test 4: Ved flere tryk på TimeButton viser displayet den rigtige værdi for tidsindstillingen. UC 7.
        public void DisplayShowsCorrectTimeMultiplePresses_OutputTest(int NumberOfPresses, int TimerSetting)
        {

            NumberOfPresses = 3;
            TimerSetting = 03;

            _powerButton.Press();

            for (int i = 0; i < NumberOfPresses; i++)
            {
                _timeButton.Press();
            }

            _output.Received(1).OutputLine($"Display shows: {TimerSetting:D2}:{00:D2}");
        }

        [Test] // Test 5: Display blankes når der afbrydes med StartCancelButton i Power indstilling. UC Extension 1.
        public void ClearDisplayOnPowerSetting_OutputTest()
        {
            _powerButton.Press();
            _startCancelButton.Press();

            _output.Received(1).OutputLine($"Display cleared");

        }

        [Test] // Test 6: Display blankes når der afbrydes med StartCancelButton i Timer indstilling. UC Extension 3.
        public void ClearDisplayOnTimerSetting_OutputTest()
        {
            _powerButton.Press();
            _startCancelButton.Press();

            _output.Received(1).OutputLine($"Display cleared");
        }

        #endregion Tests

    }
}
