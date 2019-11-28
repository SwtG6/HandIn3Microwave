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
    class IntegrationTest5_UICC
    {
        #region Properties

        private UserInterface _uut;
        private IUserInterface _userInterface;

        private IButton _startCancelButton;
        private IButton _powerButton;
        private IButton _timeButton;
        private IDoor _door;

        private ILight _light;
        private IDisplay _display;

        private ICookController _cookController;
        private IPowerTube _powerTube;
        private ITimer _timer;

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
            _display = Substitute.For<IDisplay>();

            _powerTube = Substitute.For<IPowerTube>();
            _timer = Substitute.For<ITimer>();

            _userInterface = Substitute.For<IUserInterface>();


            _cookController = new CookController
            (
                _timer,
                _display,
                _powerTube,
                _userInterface
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


        }

        #endregion SetUp

    }
}
