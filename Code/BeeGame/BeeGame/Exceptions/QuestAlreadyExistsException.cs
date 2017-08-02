using System;

namespace BeeGame.Exceptions
{
    public class QuestAlreadyExistsException : Exception
    {
        public QuestAlreadyExistsException() : base()
        {

        }

        public QuestAlreadyExistsException(string message) : base(message)
        {

        }

        public QuestAlreadyExistsException(string message, Exception innerException) : base(message, innerException)
        {

        }
    }
}
