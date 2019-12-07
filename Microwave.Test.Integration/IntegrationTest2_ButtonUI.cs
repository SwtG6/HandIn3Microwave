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
    class IntegrationTest2_ButtonUI
    {
        private IButton _startCancelButton;
        private IButton _powerButton;
        private IButton _timeButton;

        private IDoor _door;

        private IUserInterface _uut;
        private ILight _light;
        private IDisplay _display;

        private ICookController _cookController;

        #region SetUp

        [SetUp]
        public void SetUp()
        {
            _startCancelButton = new Button();
            _powerButton = new Button();
            _timeButton = new Button();

            _door = Substitute.For<IDoor>();

            _light = Substitute.For<ILight>();
            _display = Substitute.For<IDisplay>();

            _cookController = Substitute.For<ICookController>();


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

        #region Små tests af enkelte Use Case steps

        [Test] // Test 1: Der trykkes på StartCancelButton og lyset i mikrobølgeovnen tændes. UC: 8 & 9.
        public void StartCancelButton_LightTest()
        {
            _startCancelButton.Press();

            _light.Received(1).TurnOn();
        }

        //[Test] // Test 2: Der trykkes på StartCancelButton under Power-setup og displayet blankes. UC Extension 1.
        //public void StartCancelButton_DisplayTest()
        //{
        //    _startCancelButton.Press();

        //    _display.Received(1).Clear();
        //}

        [Test] // Test 3: Der trykkes op StartCancelButton under cooking og PT slukkes samt displayet blankes. UC Extension 3.
        public void StartCancelButton_PTAndDisplayTest()
        {
            _powerButton.Press();
            _timeButton.Press();
            _startCancelButton.Press();

            System.Threading.Thread.Sleep(1000);
            _startCancelButton.Press();

            _cookController.Received(1).Stop();
        }

        [Test] // Test 4: Der trykkes på PowerButton én gang, og det valgte power level ændres samt opdateres på displayet. UC 6.
        public void PowerButton_SettingAndDisplayTest()
        {
            _powerButton.Press();

            _display.Received(1).ShowPower(Arg.Any<int>());
        }

        [Test] // Test 5: Der trykkes på TimerButton én gang, og den valgte tid vises på displayet. UC 7
        public void TimeButton_DisplayTest()
        {
            _powerButton.Press();
            _timeButton.Press();

            _display.Received(1).ShowTime(Arg.Is(01), Arg.Is(00));
            //_display.Received(1).ShowTime(Arg.Any<int>(), Arg.Any<int>()); - Virker måske bedre, da vi ikke kan garantere at første tryk giver 01:00 (Tjek unit tests).
            //_display.Received(1).ShowTime($"Display shows: {01:D2}:{00:D2}");
        }

        #endregion Små tests af enkelte Use Case steps

        #region Fulde Use Case tests

        [Test] // Test 1 : Der trykkes på én gang hver på  PowerButton, TimeButton og StartCancelButton, og lyset i mikrobølgeovnen tændes til sidst. UC 6, 7 & 8 (ish).
        public void PowerButton_TimeButton_StartCancelButton_LightTest()
        {
            _powerButton.Press();
            _timeButton.Press();
            _startCancelButton.Press();

            _light.Received(1).TurnOn();
        }

        [TestCase(5, 250)]
        [TestCase(6, 300)]
        [TestCase(7, 350)]
        // Test 2: Der trykkes flere gange på PowerButton og Display viser den korrekte værdi for det valgte Power level. UC 6.
        public void MultiplePowerButton_DisplayTest(int NumberOfPresses, int PowerLevel)
        {
            //NumberOfPresses = 5;    // Der trykkes 5 gange på PowerButton
            //PowerLevel = 250;       // Dette skulle gerne resultere i at Display viser Power level 250W.
            
            for(int i = 0; i < NumberOfPresses; i++)
            {
                _powerButton.Press();
            }
            _display.Received(1).ShowPower(PowerLevel);
        }

        [Test] // Test 3: Der trykkes på StartCancelButton under Power Setup, og Display blankes. Extension 1.
        public void StartCancelButtonOnPowerSetup_DisplayTest()
        {
            _powerButton.Press();
            _startCancelButton.Press();

            _display.Received(1).Clear();
        }

        [TestCase(5, 05)]
        [TestCase(6, 06)]
        [TestCase(7, 07)]
        // Test 4: Der trykkes flere gange på TimeButton og Display viser den korrekte værdi for den valgte tid. UC 7.
        public void MultipleTimeButton_DisplayTest(int NumberOfPresses, int TimerSetting)
        {
            //NumberOfPresses = 5;
            //TimerSetting = 05;

            _powerButton.Press();

            for(int i = 0; i < NumberOfPresses; i++)
            {
                _timeButton.Press();
            }

            _display.Received(1).ShowTime(TimerSetting,00);
        }

        [Test] // Test 4: Døren åbnes under Timer Setup, og Light tændes samt Display blankes. Extension 2.
        public void DoorOpenTimerSetting_LightAndDisplayTest()
        {
            _powerButton.Press();
            _timeButton.Press();

            _door.Open();

            _light.Received(1).TurnOff();
            _display.Received(1).Clear();
        }

        #endregion Fulde Use Case tests

    }
}
