/*
<copyright file="BGCalcUnit2ControlsA.cs" company="BansheeGz">
    Copyright (c) 2018-2023 All Rights Reserved
</copyright>
*/

namespace BansheeGz.BGDatabase
{
    /// <summary>
    /// abstract class for unit with 2 controls port (enter and exit)
    /// </summary>
    public abstract class BGCalcUnit2ControlsA : BGCalcUnit
    {
        public const string EnterPortName = "y";
        public const string ExitPortName = "z";

        protected BGCalcControlInput enterPort;
        protected BGCalcControlOutput exitPort;

        public override void Definition()
        {
            enterPort = ControlInput("enter", EnterPortName, RunMe);
            exitPort = ControlOutput("exit", ExitPortName);
        }

        private BGCalcControlOutputI RunMe(BGCalcFlowI flow)
        {
            Run(flow);
            return exitPort;
        }

        /// <summary>
        /// method to be invoked on unit execution
        /// </summary>
        protected abstract void Run(BGCalcFlowI flow);
    }
}