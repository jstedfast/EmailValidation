//
// EmailValidationErrorCode.cs
//
// Author: Jeffrey Stedfast <jestedfa@microsoft.com>
//
// Copyright (c) 2013-2025 Jeffrey Stedfast
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
	/// An enumeration of possible email address validation error codes.
	/// </summary>
	/// <remarks>
	/// An enumeration of possible email address validation error codes.
	/// </remarks>
	public enum EmailValidationErrorCode
	{
		/// <summary>
		/// No error.
		/// </summary>
		None,

		/// <summary>
		/// The email address is empty.
		/// </summary>
		EmptyAddress,

		/// <summary>
		/// The email address exceeds the maximum length of 254 characters.
		/// </summary>
		AddressTooLong,

		/// <summary>
		/// The local-part of the email address contains an unterminated quoted-string.
		/// </summary>
		UnterminatedQuotedString,

		/// <summary>
		/// The local-part of the email address contains an invalid character.
		/// </summary>
		InvalidLocalPartCharacter,

		/// <summary>
		/// The local-part of the email address is incomplete.
		/// </summary>
		IncompleteLocalPart,

		/// <summary>
		/// The local-part of the email address exceeds the maximum length of 64 characters.
		/// </summary>
		LocalPartTooLong,

		/// <summary>
		/// The domain of the email address exceeds the maximum length of 253 characters.
		/// </summary>
		DomainTooLong,

		/// <summary>
		/// A domain label (subdomain) of the email address exceeds the maximum length of 63 characters.
		/// </summary>
		DomainLabelTooLong,

		/// <summary>
		/// The domain of the email address contains an invalid character.
		/// </summary>
		InvalidDomainCharacter,

		/// <summary>
		/// The domain of the email address is incomplete.
		/// </summary>
		IncompleteDomain,

		/// <summary>
		/// A domain label (subdomain) of the email address is incomplete.
		/// </summary>
		IncompleteDomainLabel,

		/// <summary>
		/// The IP address literal is incomplete.
		/// </summary>
		InvalidIPAddress,

		/// <summary>
		/// The IP address literal of the email address is missing the closing ']'.
		/// </summary>
		UnterminatedIPAddressLiteral,

		/// <summary>
		/// The email address contains unexpected characters after the end of the domain.
		/// </summary>
		UnexpectedCharactersAfterDomain,
	}
}
