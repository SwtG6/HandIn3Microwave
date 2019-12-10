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
    [TestFixture]
    class IntegrationTest5_UICC
    {
        #region Properties

        private UserInterface _uut;
        //private IUserInterface _userInterface;

        private IButton _startCancelButton;
        private IButton _powerButton;
        private IButton _timeButton;
        private IDoor _door;

        private ILight _light;
        private IDisplay _display;

        private CookController _cookController;
        private IPowerTube _powerTube;
        private ITimer _timer;
        private IOutput _output;

        #endregion Properties

        #region SetUp

        [SetUp]
        public void SetUp()
        {
            _startCancelButton = new Button();
            _powerButton = new Button();
            _timeButton = new Button();
            _door = new Door();

            _light = Substitute.For<ILight>();
            _display = new Display(_output);

            _powerTube = Substitute.For<IPowerTube>();
            _timer = Substitute.For<ITimer>();

            //_userInterface = Substitute.For<IUserInterface>();

            _output = Substitute.For<IOutput>();

            _cookController = new CookController
            (
                _timer,
                _display,
                _powerTube
            );


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

            _cookController.UI = _uut;

        }

        #endregion SetUp

        #region Fulde Use case tests

        //[Test]
        //public void UserInterfaceCookController_TurnOnTest()
        //{
        //    _powerButton.Press();
        //    _timeButton.Press();
        //    _startCancelButton.Press();

        //    _timer.Received(1).Start(Arg.Any<int>());
        //    _powerTube.Received(1).TurnOn(Arg.Any<int>());
        //}


        //[Test] // Test 1: Tester om Power Tube bliver indstillet til det rigtige Power level ved ét tryk på powerknappen. UC 1-10.
        //public void CorrectPowerOnCookStart_PowerTubeTest()
        //{
        //    _door.Open();
        //    _door.Close();

        //    _powerButton.Press();
        //    //_powerButton.Pressed += Raise.EventWith(this, EventArgs.Empty);
        //    _timeButton.Press();
        //    _startCancelButton.Press();

        //    _timer.Received(1).Stop();
        //    _powerTube.Received(1).TurnOn(50);
        //}

        //[TestCase(5, 250)]
        //// Test 2: Tester om Power Tube bliver indstillet til det rigtige Power level ved flere tryk på powerknappen. UC 1-10.
        //public void CorrectPowerOnCookStart_MultiplePowerTubeTest(int NumberOfPresses, int PowerLevel)
        //{
        //    // NumberOfPresses = 5;
        //    // PowerLevel = 250;

        //    _door.Open();
        //    _door.Close();

        //    for(int i = 0; i < NumberOfPresses; i++)
        //    {
        //        _powerButton.Press();
        //    }

        //    _timeButton.Press();
        //    _startCancelButton.Press();

        //    _powerTube.Received(1).TurnOn(PowerLevel);
        //}

        //[Test] // Test 3: Tester om Timer bliver indstillet til den rigtige Timer Setting ved ét tryk på timerknappen. UC 1-10.
        //public void CorrectTimerOnCookStart_TimerTest()
        //{
        //    _door.Open();
        //    _door.Close();

        //    _powerButton.Press();
        //    _timeButton.Press();
        //    _startCancelButton.Press();

        //    // _timer.Received(1).Start(60);   // tid i sekunder, ikke i minutter.
        //    //_display.Received(1).ShowTime(01, 00);
        //    //_output.Received(1).OutputLine($"Display shows: {01:D2}:{00:D2}");
        //    _timer.TimeRemaining.Returns(60);
        //}

        //[TestCase(5, 5)]
        //// Test 4: Tester om Timer bliver indstillet til den rigtige Timer Setting ved flere tryk på timerknappen. UC 1-10.
        //public void CorrectTimerOnCookStart_MultipleTimerTest(int NumberOfPresses, int TimerSetting)
        //{
        //    // NumberOfPresses = 5;
        //    // TimerSetting = 5;

        //    _door.Open();
        //    _door.Close();

        //    _powerButton.Press();

        //    for (int i = 0; i < NumberOfPresses; i++)
        //    {
        //        _timeButton.Press();
        //    }

        //    _startCancelButton.Press();


        //    _timer.Received(1).Start(TimerSetting*60);   // tid i sekunder, ikke i minutter.
        //}


        //[Test] // Test 5: Tester om PowerTube slukker når der trykkes på StartCancelButton under tilberedning. Extension 3.
        //public void CancelCooking_PowerTubeTest()
        //{
        //    //_door.Open();
        //    //_door.Close();

        //    _powerButton.Press();
        //    _timeButton.Press();
        //    _startCancelButton.Press();


        //    _startCancelButton.Press();
        //    //_powerTube.Received(1).TurnOff();
        //}

        //[Test] // Test 6: Tester om Timer slukker når der trykkes på StartCancelButton under tilberedning. Extension 3.
        //public void CancelCooking_TimerAndPowerTubeTest()
        //{
        //    _door.Open();
        //    _door.Close();

        //    _powerButton.Press();
        //    _timeButton.Press();
        //    _startCancelButton.Press();


        //    _startCancelButton.Press();
        //    _timer.Received(1).Stop();
        //}

        //[Test] // Test 7: Tester om PowerTube slukker når døren åbnes under tilberedning. Extension 4.
        //public void DoorOpenCooking_PowerTubeTest()
        //{
        //    _door.Open();
        //    _door.Close();

        //    _powerButton.Press();
        //    _timeButton.Press();
        //    _startCancelButton.Press();


        //    //_door.Opened += Raise.EventWith(this, EventArgs.Empty);
        //    _door.Open();
        //    _powerTube.Received(1).TurnOff();
        //}

        //[Test] // Test 8: Tester om Timer slukker når døren åbnes under tilberedning. Extension 4.
        //public void DoorOpenCooking_TimerTest()
        //{
        //    _door.Open();
        //    _door.Close();

        //    _powerButton.Press();
        //    _timeButton.Press();
        //    _startCancelButton.Press();


        //    //_door.Opened += Raise.EventWith(this, EventArgs.Empty);
        //    _door.Open();
        //    _timer.Received(1).Stop();
        //}

        #endregion Fulde Use case tests
    }
}
