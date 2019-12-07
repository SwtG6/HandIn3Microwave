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


            //uut
            _uut = new CookController(_timer, _display, _powerTube, _userInterface);

        }

        #endregion

        #region tests

        [Test] // test for om Timer "ticker" ned som tiden går generelt
        public void TimerTick(int power, int time, int tick)
        {
            power = 100;
            time = 50;
            tick = 20;



            _uut.StartCooking(power, time);
            _timer.TimeRemaining.Returns(time - tick);
            _timer.TimerTick += Raise.EventWith(this, EventArgs.Empty);

            _output.Received(1).OutputLine($"Display shows: {0:D2}:{30:D2}");
        }

        [Test] // test for om Timer "ticker" ned som tiden går løbende
        public void TimerTicks(int power, int time, int tick)
        {
            power = 500;
            time = 120;
            tick = 60;


            _uut.StartCooking(100, time);

            for (int i = 1; i < tick + 1; i++)
            {
                _timer.TimeRemaining.Returns(time - i);
                _timer.TimerTick += Raise.EventWith(this, EventArgs.Empty);
            }
            _output.Received(1).OutputLine($"Display shows: {01:D2}:{00:D2}");

        }

        #endregion
    }
}
