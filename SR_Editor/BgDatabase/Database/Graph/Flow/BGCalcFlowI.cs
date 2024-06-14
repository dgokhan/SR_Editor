/*
<copyright file="BGCalcFlowI.cs" company="BansheeGz">
    Copyright (c) 2018-2023 All Rights Reserved
</copyright>
*/

namespace BansheeGz.BGDatabase
{
    /// <summary>
    /// Flow object is used during graph execution to hold graph values
    /// </summary>
    public interface BGCalcFlowI : BGCalcVarsOwnerI
    {
        /// <summary>
        /// execution level (for nested graphs)
        /// </summary>
        int Level { get; set; }

        /// <summary>
        /// parent flow object 
        /// </summary>
        BGCalcFlowI Parent { get; set; }

        /// <summary>
        /// execution context 
        /// </summary>
        BGCalcFlowContext Context { get; }

        /// <summary>
        /// Get/set execution result
        /// </summary>
        object Result { get; set; }

        /// <summary>
        /// Break is requested for loops (like break C# statement)
        /// </summary>
        bool BreakIsRequested { get; set; }

        /// <summary>
        /// Execute graph input port 
        /// </summary>
        BGCalcControlOutputI Run(BGCalcControlInputI port);

        /// <summary>
        /// Get input port value
        /// </summary>
        T GetValue<T>(BGCalcValueInputI input);

        /// <summary>
        /// Get input port value as object
        /// </summary>
        // object GetValue(BGCalcValueInputI input, System.Type type);
        object GetValue(BGCalcValueInputI input);

        /// <summary>
        /// Get output port value as object
        /// </summary>
        object GetValue(BGCalcValueOutputI output);

        /// <summary>
        /// Set port local variable value 
        /// </summary>
        void SetValue(BGCalcPortI port, object value);

        /// <summary>
        /// Run as nested 
        /// </summary>
        void RunNested(BGCalcControlInputI connectedPort);

        /// <summary>
        /// Get port local variable value 
        /// </summary>
        object GetLocalVar(BGCalcPortI port);
    }
}