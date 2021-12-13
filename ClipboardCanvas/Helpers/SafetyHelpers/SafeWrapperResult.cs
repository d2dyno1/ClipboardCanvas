﻿using System;
using ClipboardCanvas.GlobalizationExtensions;

using ClipboardCanvas.Enums;
using ClipboardCanvas.Helpers.SafetyHelpers.ExceptionReporters;

namespace ClipboardCanvas.Helpers.SafetyHelpers
{
    public class SafeWrapperResult
    {
        public static readonly SafeWrapperResult SUCCESS = new SafeWrapperResult(OperationErrorCode.Success, "Operation completed successfully.");

        public static readonly SafeWrapperResult CANCEL = new SafeWrapperResult(OperationErrorCode.Canceled, "OperationCanceled".GetLocalized2());

        public static readonly SafeWrapperResult UNKNOWN_FAIL = new SafeWrapperResult(OperationErrorCode.UnknownFailed, new Exception(), "UnknownError".GetLocalized2());

        public string Message => Details?.message;

        public OperationErrorCode ErrorCode => Details?.errorCode ?? OperationErrorCode.UnknownFailed;

        public Exception Exception => Details?.innerException;

        public SafeWrapperResultDetails Details { get; private set; }

        public SafeWrapperResult(OperationErrorCode status, Exception innerException)
            : this(status, innerException, null)
        {
        }

        public SafeWrapperResult(OperationErrorCode status, string message)
            : this(status, null, message)
        {
        }

        public SafeWrapperResult(OperationErrorCode status, Exception innerException, string message)
            : this(new SafeWrapperResultDetails(status, innerException, message))
        {
        }

        public SafeWrapperResult(SafeWrapperResultDetails details)
        {
            this.Details = details;
        }

        public static SafeWrapperResult FromException(Exception exception, ISafeWrapperExceptionReporter exceptionReporter = null)
        {
            if (exceptionReporter == null)
            {
                exceptionReporter = StaticExceptionReporters.DefaultSafeWrapperExceptionReporter;
            }

            return exceptionReporter.GetStatusResult(exception);
        }

        public static implicit operator OperationErrorCode(SafeWrapperResult wrapperResult)
            => wrapperResult?.Details?.errorCode ?? OperationErrorCode.InvalidArgument;

        public static implicit operator bool(SafeWrapperResult wrapperResult)
            => (wrapperResult?.Details?.errorCode ?? OperationErrorCode.UnknownFailed) == OperationErrorCode.Success;

        public static implicit operator SafeWrapperResult(bool result)
            => result ? SafeWrapperResult.SUCCESS : new SafeWrapperResult(OperationErrorCode.UnknownFailed, innerException: null);

        public static implicit operator SafeWrapperResult(SafeWrapperResultDetails details)
            => new SafeWrapperResult(details);

        public static implicit operator SafeWrapperResult((OperationErrorCode errorCode, Exception innerException) details)
            => new SafeWrapperResult(details.errorCode, details.innerException);

        public static implicit operator SafeWrapperResult((OperationErrorCode errorCode, string message) details)
            => new SafeWrapperResult(details.errorCode, details.message);

        public static implicit operator SafeWrapperResult((OperationErrorCode errorCode, Exception innerException, string message) details)
            => new SafeWrapperResult(details.errorCode, details.innerException, details.message);
    }
}
