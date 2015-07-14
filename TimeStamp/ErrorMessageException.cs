using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Autodesk.Revit.DB;

namespace TimeStamp
{
    /// <summary>
    /// Retrive the error message for displaying it in the Revit interface
    /// </summary>
    public class ErrorMessageException : ApplicationException
    {
        /// <summary>
        /// constructor entirely using baseclass'
        /// </summary>
        public ErrorMessageException()
            : base()
        {
        }

        /// <summary>
        /// constructor entirely using baseclass'
        /// </summary>
        /// <param name="message">error message</param>
        public ErrorMessageException(String message)
            : base(message)
        {
        }
    }


    /// <summary>
    /// Manage Warning in the Revit interface
    /// </summary>
    public class PlintePreprocessor : IFailuresPreprocessor
    {
        public FailureProcessingResult PreprocessFailures(FailuresAccessor failuresAccessor)
        {
            IList<FailureMessageAccessor> failList = new List<FailureMessageAccessor>();
            // Inside event handler, get all warnings
            failList = failuresAccessor.GetFailureMessages();
            foreach (FailureMessageAccessor failure in failList)
            {
                // check FailureDefinitionIds against ones that you want to dismiss,
                FailureDefinitionId failID = failure.GetFailureDefinitionId();
                // prevent Revit from showing Unenclosed room warnings
                if (failID == BuiltInFailures.OverlapFailures.WallsOverlap)
                {
                    failuresAccessor.DeleteWarning(failure);
                }
            }

            return FailureProcessingResult.Continue;
        }
    }
}
