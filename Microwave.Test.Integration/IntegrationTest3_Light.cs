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
        private IUserInterface _userInterface;
        private ILight _light;
        private UserInterface _iut;

        [SetUp]
        public void SetUp()
        {
            //fakes
            _output = Substitute.For<IOutput>();
            _powerTube = Substitute.For<IPowerTube>();
            _timer = Substitute.For<ITimer>();

            //Ctors
            _powerButton = new Button();
            _timeButton = new Button();
            _startCancelButton = new Button();
            _cookController = new CookController(_timer, _display, _powerTube, _iut);
            _display = new Display(_output);
            _door = new Door();
            _light = new Light(_output);

            //uit
            _iut = new UserInterface(_powerButton, _timeButton, _startCancelButton, _door, _display, _light, _cookController);

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

        //TODO add test for light on when MW is running
        //TODO add test for light off when MW is running and is canceled using button
        //TODO add test for light off when MW is running reaches timer cooking point


    }

}
