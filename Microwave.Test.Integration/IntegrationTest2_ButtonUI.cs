using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
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

        private IUserInterface _UUT;
        private ILight _light;
        private IDisplay _display;

        private ICookController _cookController;
    }
}
