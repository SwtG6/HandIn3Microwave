using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using MicrowaveOvenClasses.Boundary;
using MicrowaveOvenClasses.Controllers;
using MicrowaveOvenClasses.Interfaces;
using NUnit.Framework;
using Timer = MicrowaveOvenClasses.Boundary.Timer;

namespace Microwave.Test.Integration
{
    class IntegrationTest9_Output
    {

        private IDisplay _display;
        private CookController _cookController;
        private IPowerTube _powerTube;
        private ITimer _timer;
        private IUserInterface _userInterface;
        private ILight _light;
        private IDoor _door;
        private IButton _powerButton;
        private IButton _timeButton;
        private IButton _startCancelButton;
        private IOutput _uut;

        private StringWriter stringWriter;

        #region SetUp
        
        [SetUp]
        public void SetUp()
        {
            _uut = new Output();

            _display = new Display(_uut);

            _powerTube = new PowerTube(_uut);
            _timer = new Timer();

            _light = new Light(_uut);
            _door = new Door();
            _powerButton = new Button();
            _timeButton = new Button();
            _startCancelButton = new Button();


            _cookController = new CookController(_timer, _display, _powerTube, _userInterface);
            _userInterface = new UserInterface(_powerButton, _timeButton, _startCancelButton, _door, _display, _light, _cookController);


            stringWriter = new StringWriter();
            Console.SetOut(stringWriter);
        }

        #endregion


        #region tests

        [Test]
        public void Test_Turning_on_Light()
        {
            _door.Open();
            Assert.That(stringWriter.ToString(), Does.StartWith(("Light is turned on")));

        }

        [Test]
        public void Test_Turning_light_off()
        {
            _door.Open();
            _door.Close();
            Assert.That(stringWriter.ToString(), Does.StartWith(("Light is turned off")));

        }

        [Test]
        public void Test_Turning_on_Light_StartCooking()
        {
            _powerButton.Press();
            _timeButton.Press();
            _startCancelButton.Press();

            _userInterface.CookingIsDone();

            Assert.That(stringWriter.ToString(), Does.Contain("Light is turned off"));

        }

        [Test]
        public void Test_Turn_Off_Light_CookingIsDone()
        {
            _powerButton.Press();
            _timeButton.Press();
            _startCancelButton.Press();

            _userInterface.CookingIsDone();

            Assert.That(stringWriter.ToString(), Does.Contain("Light is turned off"));
        }

        [Test]
        public void Test_PowerMeasurement()
        {
            _powerButton.Press();
            Assert.That(stringWriter.ToString(), Does.Contain("Display shows: 50W"));

        }

        [Test]
        public void Test_TimeMeasurement()
        {
           _powerButton.Press();
           _timeButton.Press();
           Assert.That(stringWriter.ToString(), Does.Contain("Display shows 01:00"));
        }

        [Test]
        public void Test_TimerTicks_OnTick()
        {
            _cookController.StartCooking(50,60);
            Thread.Sleep(6000);
            Assert.That(stringWriter.ToString(), Does.Contain("Display shows: 00:55"));
        }

        [Test]
        public void Test_ClearDisplayOnCookingDone()
        {
            _powerButton.Press();
            _timeButton.Press();
            _startCancelButton.Press();
            _userInterface.CookingIsDone();
            Assert.That(stringWriter.ToString(), Does.Contain("Display cleared"));
        }

        [Test]
        public void Test_PowertubeOff_OnTimerExpired()
        {
            _cookController.StartCooking(50,10);
            Thread.Sleep(11000);
            Assert.That(stringWriter.ToString(), Does.Contain("PowerTube is turned off"));
        }
        
        #endregion

    }
}
