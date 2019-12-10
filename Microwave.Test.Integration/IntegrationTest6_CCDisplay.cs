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
    class IntegrationTest6_CCDisplay
    {
        private IDisplay _display;
        private IPowerTube _powerTube;
        private IOutput _output;
        private ITimer _timer;
        private IUserInterface _userInterface;
        private ICookController _uut;

        private IDoor _door;
        private IButton _powerButton, _timeButton, _startCancelButton;

        #region Setup

        [SetUp]
        public void SetUp()
        {
            //fakes
            _powerTube = Substitute.For<IPowerTube>();
            _output = Substitute.For<IOutput>();
            _timer = Substitute.For<ITimer>();
            _userInterface = Substitute.For<IUserInterface>();

            // ctor
            _display = new Display(_output);

            _door = new Door();
            _powerButton = new Button();
            _timeButton = new Button();
            _startCancelButton = new Button();


            //uut
            _uut = new CookController(_timer, _display, _powerTube, _userInterface);

        }

        #endregion

        #region tests

        [Test]
        public void UserInterfaceCookController_TurnOnTest()
        {
            _powerButton.Press();
            _timeButton.Press();
            _startCancelButton.Press();

            //_timer.Received(1).Start(Arg.Any<int>());
            _powerTube.Received(1).TurnOn(Arg.Any<int>());
        }



        [TestCase(100, 60, 1)]
        // test for om Timer "ticker" ned som tiden går generelt
        public void TimerTick(int power, int time, int tick)
        {
            _uut.StartCooking(power, time);
            _timer.TimeRemaining.Returns(time - tick);
            _timer.TimerTick += Raise.EventWith(this, EventArgs.Empty);

            _output.Received(1).OutputLine($"Display shows: {0:D2}:{59:D2}");
        }

        [TestCase(100, 60, 10)]
        [TestCase(100, 60, 20)]
        [TestCase(100, 60, 30)]
        // test for om Timer "ticker" ned som tiden går løbende
        public void TimerTicks(int power, int time, int tick)
        {

            _uut.StartCooking(power, time);

            for (int i = 1; i < tick + 1; i++)
            {
                _timer.TimeRemaining.Returns(time - i);
                _timer.TimerTick += Raise.EventWith(this, EventArgs.Empty);
            }
            _output.Received(1).OutputLine($"Display shows: {00:D2}:{_timer.TimeRemaining:D2}");

        }

        #endregion
    }
}
