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
    public class IntegrationTest7_CCPowerTube
    {
        

        private IUserInterface _userInterface;
        private ICookController _uut;
        private IDisplay _display;
        private IPowerTube _powerTube;
        private ITimer _timer;
        private IOutput _output;
        

        

        [SetUp]
        public void SetUp()
        {

            // fakes
            _output = Substitute.For<IOutput>();
            _timer = Substitute.For<ITimer>();
            _userInterface = Substitute.For<IUserInterface>();


            // ctors
            _display = new Display(_output);
            _powerTube = new PowerTube(_output);

            // uut
            _uut = new CookController(_timer, _display, _powerTube, _userInterface);
        }

        

        [TestCase(50, 5)]
        [TestCase(350, 50)]
        [TestCase(700, 100)]
        public void TestTurnsOn(int power, int time)
        {
            _uut.StartCooking(power, time);

            _output.Received(1).OutputLine(Arg.Is<string>(str => str.Contains($"PowerTube works with {power} W")));
        }

        [Test]
        public void TestTurnsOff()
        {
            _uut.StartCooking(150, 100);

            _timer.Expired += Raise.EventWith(this, EventArgs.Empty);
            _output.Received(1).OutputLine(Arg.Is<string>(str => str.Contains("Off")));
        }

        [Test]
        public void TestTurnsOffWhenButtonIsPressed()
        {
            _uut.StartCooking(150, 100);
            _uut.Stop();

            _output.Received(1).OutputLine(Arg.Is<string>(str => str.Contains("Off")));

        }

     
    }
}

