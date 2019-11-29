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
    class IntegrationTest1_DoorUI
    {
        //Referenced interfaces constructed for usage
        private IButton _powerButton;
        private IButton _timeButton;
        private IButton _startCancelButton;

        private ICookController _cookController;
        private IDisplay _display;
        private IDoor _door;
        private ILight _light;
        private IUserInterface _uut;

        #region Setup


        [SetUp]
        public void SetUp()
        {
            //fakes
            _cookController = Substitute.For<ICookController>();
            _display = Substitute.For<IDisplay>();
            _light = Substitute.For<ILight>();
            _powerButton = Substitute.For<IButton>();
            _startCancelButton = Substitute.For<IButton>();
            _timeButton = Substitute.For<IButton>();

            //Ctors
            _door = new Door();

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

        [Test]
        public void OpenDoor()
        {
            _door.Open();
            _light.Received(1).TurnOn();
        }

        [Test]
        public void OpenAndCloseDoor()
        {
            _door.Open();
            _light.Received(1).TurnOn();
            _door.Close();
            _light.Received(1).TurnOff();
        }

        [Test]
        public void OpenDoorWhenCooking()
        {
            
            _door.Open();
            _light.Received(1).TurnOn();
            _door.Close();
            _powerButton.Press();
            _timeButton.Press();
            _startCancelButton.Press();
            _door.Open();
            _cookController.Received(1).Stop();
            
        }

        [Test]
        public void PressStartButtonWhenCooking()
        {
            _door.Open();
            _light.Received(1).TurnOn();
            _door.Close();
            _powerButton.Press();
            _timeButton.Press();
            _startCancelButton.Press();
            System.Threading.Thread.Sleep(1000);

            _startCancelButton.Press();
            _cookController.Received(1).Stop();
            _light.Received(2).TurnOff();
        }

        #endregion


    }

}
