using System;
using System.Collections.Generic;
using System.Text;

namespace Tools
{
    public class ErrorMessages
    {
        public string UnableToConnectToOctave(string path)
        {
            string updatedPath = path;
            return "Unable to find Octave binaries using path " + updatedPath + GetDefaultErrorEnding();
        }
        public string UnableToConnectToDatabase()
        {
            return "Unable to connect to the database!";
        }
        public string GenericError(string error)
        {
            return error + GetDefaultErrorEnding();
        }

        private string GetDefaultErrorEnding()
        {
            return ". Press any key to continue...";
        }
    }
}
