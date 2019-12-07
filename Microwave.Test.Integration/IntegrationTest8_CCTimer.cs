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
    class IntegrationTest8_CCTimer
    {
        private IDisplay _display;
        private CookController _uut;
        private IPowerTube _powerTube;
        private ITimer _timer;
        private IOutput _output;
        private IUserInterface _userInterface;
        private ILight _light;
        private IDoor _door;
        private IButton _powerButton;
        private IButton _timeButton;
        private IButton _startCancelButton;

        [SetUp]
        public void SetUp()
        {
            _output = Substitute.For<IOutput>();
            _powerButton = new Button();
            _timeButton = new Button();
            _startCancelButton = new Button();
            _door = new Door();
            _powerTube = new PowerTube(_output);
            _display = new Display(_output);
            _light = new Light(_output);
            _timer = new Timer();
            _uut = new CookController(_timer, _display, _powerTube);
            _userInterface = new UserInterface(_powerButton, _timeButton, _startCancelButton, _door, _display, _light, _uut);
            _uut.UI = _userInterface;
        }

        [TestCase(1, "Displayet viser 00:05", 1000)]
        [TestCase(2, "Display viser 00:55", 2000)]
        public void OnTimerTickCookControllerLogsOutput(int times, string output, int delay)
        {
            _door.Open();
            _door.Close();

            _powerButton.Pressed += Raise.EventWith(this, EventArgs.Empty);
            for (int i = times; (i > 0); i--)
            {
                _timeButton.Pressed += Raise.EventWith(this, EventArgs.Empty);
            }

            _startCancelButton.Pressed += Raise.EventWith(this, EventArgs.Empty);


            System.Threading.Thread.Sleep(delay);
            _output.Received(1).OutputLine(output);
        }

        [TestCase(1, "PowerTube er slukket", 1 * 30000)]
        [TestCase(2, "PowerTube er slukket", 2 * 30000)]
        public void OntimerExpire(int times, string output, int delay)
        {
            _door.Open();
            _door.Close();

            _powerButton.Pressed += Raise.EventWith(this, EventArgs.Empty);
            for (int i = times; (i > 0); i--)
            {
                _timeButton.Pressed += Raise.EventWith(this, EventArgs.Empty);
            }

            _startCancelButton.Pressed += Raise.EventWith(this, EventArgs.Empty);


            System.Threading.Thread.Sleep(delay);
            _output.Received(1).OutputLine(output);
        }
    }

}
