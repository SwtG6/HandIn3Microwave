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
    [TestFixture]
    class IntegrationTest2_ButtonUI
    {
        private IButton _startCancelButton;
        private IButton _powerButton;
        private IButton _timeButton;

        private IDoor _door;

        private IUserInterface _uut;
        private ILight _light;
        private IDisplay _display;

        private ICookController _cookController;

        #region SetUp

        [SetUp]
        public void SetUp()
        {
            _startCancelButton = new Button();
            _powerButton = new Button();
            _timeButton = new Button();

            _door = Substitute.For<IDoor>();

            _light = Substitute.For<ILight>();
            _display = Substitute.For<IDisplay>();

            _cookController = Substitute.For<ICookController>();


            _uut = new UserInterface
            (
                _startCancelButton,
                _powerButton,
                _timeButton,
                _door,
                _display,
                _light,
                _cookController
            );
        }

        #endregion SetUp


        #region Tests

        [Test]
        public void StartCancelButton_StartTest()
        {
            _startCancelButton.Press();


        }



        #endregion Tests

    }
}
