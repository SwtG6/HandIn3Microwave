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
                _startCancelButton,
                _powerButton,
                _timeButton,
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

        [Test] // Test 2: Der trykkes på StartCancelButton under Power-setup og displayet blankes. UC Extension 1.
        public void StartCancelButton_DisplayTest()
        {
            _startCancelButton.Press();

            _display.Received(1).Clear();
        }

        [Test] // Test 3: Der trykkes op StartCancelButton under cooking og PT slukkes samt displayet blankes. UC Extension 3.
        public void StartCancelButton_PTDisplayTest()
        {
            _startCancelButton.Press();

            _cookController.Received(1).Stop();
        }

        [Test] // Test 4: Der trykkes på PowerButton én eller flere gange, og det valgte power level ændres samt opdateres på displayet. UC 6.
        public void PowerButton_SettingDisplayTest()
        {
            _powerButton.Press();

        }

        #endregion Små tests af enkelte Use Case steps

        #region Fulde Use case tests

        [Test]
        public void testetstetsts()
        {



        }


        #endregion Fulde Use case tests

    }
}
