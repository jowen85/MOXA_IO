using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Configuration;

namespace CombinedDemo
{
    /// <summary>
    /// Sets up methods for modifying properties of the PolarisControllerModel.
    /// </summary>
    class PolarisControllerController
    {
        private PolarisControllerModel _polarisControllerModel;

        public PolarisControllerController(PolarisControllerModel polarisControllerModel)
        {
            _polarisControllerModel = polarisControllerModel;
        }
    }
}
