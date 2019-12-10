using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using MicrowaveOvenClasses.Boundary;
using MicrowaveOvenClasses.Controllers;
using MicrowaveOvenClasses.Interfaces;
using NSubstitute;
using NSubstitute.Core.Arguments;
using NSubstitute.ReceivedExtensions;
using NUnit.Framework;

namespace Microwave.Test.Integration
{
    public class IT_CC
    {
        private IButton _powerButton;
        private IButton _timeButton;
        private IButton _startCancelButton;
        private IDoor _door;


        private IOutput _output;

        private CookController _cookController;
        private IDisplay _display;
        private IPowerTube _powerTube;
        private ITimer _timer;
        private UserInterface _uut;

        private ILight _light;

        [SetUp]
        public void SetUp()
        {
            _output = Substitute.For<IOutput>();
            _timer = Substitute.For<ITimer>();
            _powerTube = Substitute.For<IPowerTube>();
            _light = Substitute.For<ILight>(); 

            _powerButton = new Button();
            _timeButton = new Button();
            _startCancelButton = new Button();
            _door = new Door();

            _display = new Display(_output);

            _cookController = new CookController(_timer, _display, _powerTube);
            _uut = new UserInterface(_powerButton, _timeButton, _startCancelButton, _door, _display, _light, _cookController);
            _cookController.UI = _uut;

        }

        [Test]
        public void UserInterfaceCookController_TurnOnTest()
        {
            _powerButton.Press();
            _timeButton.Press();
            _startCancelButton.Press();

            _timer.Received(1).Start(Arg.Any<int>());
            _powerTube.Received(1).TurnOn(Arg.Any<int>());
        }
    }
}
