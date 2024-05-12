//
// EmailValidationError.cs
//
// Author: Jeffrey Stedfast <jestedfa@microsoft.com>
//
// Copyright (c) 2013-2024 Jeffrey Stedfast
//
// Permission is hereby granted, free of charge, to any person obtaining a copy
// of this software and associated documentation files (the "Software"), to deal
// in the Software without restriction, including without limitation the rights
// to use, copy, modify, merge, publish, distribute, sublicense, and/or sell
// copies of the Software, and to permit persons to whom the Software is
// furnished to do so, subject to the following conditions:
//
// The above copyright notice and this permission notice shall be included in
// all copies or substantial portions of the Software.
//
// THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR
// IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY,
// FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL THE
// AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER
// LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE, ARISING FROM,
// OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN
// THE SOFTWARE.

namespace EmailValidation
{
	/// <summary>
	/// An email validation error.
	/// </summary>
	/// <remarks>
	/// Represents an email validation error, containing information about the type of error
	/// and the index of the offending character.
	/// </remarks>
	public class EmailValidationError
	{
		/// <summary>
		/// A special instance of <see cref="EmailValidationError"/> that indicates no error.
		/// </summary>
		/// <remarks>
		/// A special instance of <see cref="EmailValidationError"/> that indicates no error.
		/// </remarks>
		public static readonly EmailValidationError None = new EmailValidationError (EmailValidationErrorCode.None);

		/// <summary>
		/// Initializes a new instance of the <see cref="EmailValidationError"/> class.
		/// </summary>
		/// <remarks>
		/// Creates a new <see cref="EmailValidationError"/>.
		/// </remarks>
		/// <param name="code">The error code.</param>
		/// <param name="tokenIndex">The character index indicating the starting position of the token that had the syntax error.</param>
		/// <param name="errorIndex">The character index indicating the position of the syntax error.</param>
		public EmailValidationError (EmailValidationErrorCode code, int? tokenIndex = null, int? errorIndex = null)
		{
			Code = code;
			TokenIndex = tokenIndex;
			ErrorIndex = errorIndex ?? tokenIndex;
		}

		/// <summary>
		/// Get the email validation error code.
		/// </summary>
		/// <remarks>
		/// Gets the email validation error code.
		/// </remarks>
		/// <value>The email validation error code.</value>
		public EmailValidationErrorCode Code {
			get; private set;
		}

		/// <summary>
		/// Get the character index indicating the starting position of the token that had the syntax error.
		/// </summary>
		/// <remarks>
		/// Gets the character index indicating the starting position of the token that had the syntax error.
		/// </remarks>
		/// <value>The character index indicating the starting position of the token that had the syntax error.</value>
		public int? TokenIndex {
			get; private set;
		}

		/// <summary>
		/// Get the character index indicating the position of the syntax error.
		/// </summary>
		/// <remarks>
		/// Gets the character index indicating the position of the syntax error.
		/// </remarks>
		/// <value>The character index indicating the position of the syntax error.</value>
		public int? ErrorIndex {
			get; private set;
		}
	}
}
