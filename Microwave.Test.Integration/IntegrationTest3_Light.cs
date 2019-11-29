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
        private IPowerTube _powerTube;
        private ITimer _timer;
        private IDoor _door;
        private IOutput _output;
        private ILight _light;
        private UserInterface _uut;

        [SetUp]
        public void SetUp()
        {
            //fakes
            _output = Substitute.For<IOutput>();
            _powerTube = Substitute.For<IPowerTube>();
            _timer = Substitute.For<ITimer>();
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

        [Test]
        public void OpenDoorTurnsLightOn()
        {
            _door.Open();
            _output.Received(1).OutputLine(Arg.Is<string>(str => str.Contains("on")));

        }

        [Test]
        public void ClosingDoorTurnsLightOff()
        {
            _door.Open();
            _door.Close();
            _output.Received(1).OutputLine(Arg.Is<string>(str => str.Contains("off")));

        }

        [Test]
        public void LightOnWhenCooking()
        {
            _door.Open();
            _door.Close();
            _powerButton.Press();
            _timeButton.Press();
            _startCancelButton.Press();
            _output.Received(1).OutputLine(Arg.Is<string>(str => str.Contains("on")));

        }

        [Test]
        public void LightOffWhenPressingCancelButtonWhenCooking()
        {
            _door.Open();
            _door.Close();
            _powerButton.Press();
            _timeButton.Press();
            _startCancelButton.Press();
            System.Threading.Thread.Sleep(1000);

            _startCancelButton.Press();
            _output.Received(1).OutputLine(Arg.Is<string>(str => str.Contains("off")));

        }

        [Test]
        public void LightOffWhenCookingDone()
        {
            _door.Open();
            _door.Close();
            _powerButton.Press();
            _timeButton.Press();
            _startCancelButton.Press();
            _uut.CookingIsDone();
            _output.Received(1).OutputLine(Arg.Is<string>(str => str.Contains("done")));
            // I Added a _output in the original code that states "Cooking job done"
        }

    }
}
