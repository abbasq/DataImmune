using System;
using System.Text;
using System.IO;
using Xunit;
using DataImmune;
using Tools;

namespace Tests
{
    public class UserInterFaceTests
    {
        [Fact]
        public void CannotFindOctaveFile()
        {
            //Arrange
            string baseOctavePath = "C:\\Program Files\\"; //Invalid path
            string fullOctavePath = baseOctavePath + "DataImmune.Program\\Octave_bin";
            string fileName = "octave-cli-4.4.1.exe";
            string finalOctavePath = fullOctavePath + "\\" + fileName;
            ErrorMessages em = new ErrorMessages();

            //Act
            OctaveController octave = new OctaveController();
            string proposedErrorMessage = em.UnableToConnectToOctave(finalOctavePath);
            string realErrorMessage = octave.StartOctave(fullOctavePath, fileName, false);

            //Assert
            //Assert.Matches(proposedErrorMessage, realErrorMessage);
            Assert.Equal(proposedErrorMessage, realErrorMessage);
        }

        [Fact]
        public void OpenOctaveSuccessfully()
        {
            //Arrange
            string baseOctavePath = "C:\\Source\\";
            string fullOctavePath = baseOctavePath + "DataImmune.Program\\Octave_bin";
            string fileName = "octave-cli-4.4.1.exe";
            ErrorMessages em = new ErrorMessages();

            //Act
            OctaveController octave = new OctaveController();
            string errorMessage = octave.StartOctave(fullOctavePath, fileName, false);

            //Assert
            Assert.Matches("", errorMessage);
        }
    }
}
