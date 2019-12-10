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
    class IntegrationTest3_Light
    {
        private IButton _powerButton;
        private IButton _timeButton;
        private IButton _startCancelButton;

        private ICookController _cookController;
        private IDisplay _display;
        //private IPowerTube _powerTube;
        //private ITimer _timer;
        private IDoor _door;
        private IOutput _output;
        private ILight _light;
        private UserInterface _uut;

        #region Setup

        [SetUp]
        public void SetUp()
        {
            //fakes
            _output = Substitute.For<IOutput>();
            //_powerTube = Substitute.For<IPowerTube>();
            //_timer = Substitute.For<ITimer>();
            _cookController = Substitute.For<ICookController>();
            _display = Substitute.For<IDisplay>();


            //Ctors
            _powerButton = new Button();
            _timeButton = new Button();
            _startCancelButton = new Button();
            _door = new Door();
            _light = new Light(_output);

            //uut
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

        #endregion

        #region tests


        [Test] // Test for om Light tænder når Door åbnes
        public void OpenDoorTurnsLightOn()
        {
            _door.Open();
            _output.Received(1).OutputLine(Arg.Is<string>(str => str.Contains("Light is turned on")));

        }

        [Test] // Test for om Light slukker når Door lukkes
        public void ClosingDoorTurnsLightOff()
        {
            _door.Open();
            _door.Close();
            _output.Received(1).OutputLine(Arg.Is<string>(str => str.Contains("Light is turned off")));

        }

        [Test] // test for om Light tænder når start knap trykkes på
        public void LightOnWhenCooking()
        {
            _door.Open();
            _door.Close();
            _powerButton.Press();
            _timeButton.Press();
            _startCancelButton.Press();
            _output.Received(2).OutputLine(Arg.Is<string>(str => str.Contains("Light is turned on")));

        }

        [Test] // test for om Light slukker når man trykker på cancel imens der "kokkereres"
        public void LightOffWhenPressingCancelButtonWhenCooking()
        {
            _door.Open();
            _door.Close();
            _powerButton.Press();
            _timeButton.Press();
            _startCancelButton.Press();
            System.Threading.Thread.Sleep(1000);

            _startCancelButton.Press();
            _output.Received(2).OutputLine(Arg.Is<string>(str => str.Contains("Light is turned off")));

        }

        [Test] // test for om Light slukker når timer udløber og "CookingIsDone()"
        public void LightOffWhenCookingDone()
        {
            _door.Open();
            _door.Close();
            _powerButton.Press();
            _timeButton.Press();
            _startCancelButton.Press();
            _uut.CookingIsDone();
            _output.Received(2).OutputLine(Arg.Is<string>(str => str.Contains("Light is turned off")));
            // I Added a _output in the original code that states "Cooking job done"
        }

        #endregion

    }
}
