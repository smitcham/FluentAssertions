using System;
using FluentAssertions.Execution;

namespace FluentAssertions.Primitives
{
    /// <summary>
    /// Dedicated class for comparing two strings and generating consistent error messages.
    /// </summary>
    internal abstract class StringValidator
    {
        #region Private Definition

        protected readonly string subject;
        protected readonly string expected;
        protected Verifier verification;
        private const int HumanReadableLength = 8;

        #endregion

        protected StringValidator(string subject, string expected, string reason, object[] reasonArgs)
        {
            verification = Execute.Verification.BecauseOf(reason, reasonArgs);

            this.subject = subject;
            this.expected = expected;
        }

        public void Validate()
        {
            if ((expected != null) || (subject != null))
            {
                if (ValidateAgainstNulls())
                {

                    if (IsLongOrMultiline(expected) || IsLongOrMultiline(subject))
                    {
                        verification = verification.UsingLineBreaks;
                    }

                    ValidateAgainstSuperfluousWhitespace();
                    ValidateAgainstLengthDifferences();
                    ValidateAgainstMismatch();
                }
            }
        }

        private bool ValidateAgainstNulls()
        {
            if (((expected == null) && (subject != null)) || ((expected != null) && (subject == null)))
            {
                verification.FailWith(ExpectationDescription + "{0}{reason}, but found {1}.", expected, subject);
                return false;
            }

            return true;
        }

        private bool IsLongOrMultiline(string value)
        {
            return (value.Length > HumanReadableLength) || value.Contains(Environment.NewLine);
        }

        protected virtual void ValidateAgainstSuperfluousWhitespace()
        {
        }

        protected virtual void ValidateAgainstLengthDifferences()
        {
        }

        protected abstract void ValidateAgainstMismatch();
        
        protected abstract string ExpectationDescription { get; }
    }
}