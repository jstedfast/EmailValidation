﻿//
// EmailValidator.cs
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

using System;

namespace EmailValidation
{
	/// <summary>
	/// An Email validator.
	/// </summary>
	/// <remarks>
	/// An Email validator.
	/// </remarks>
	public static class EmailValidator
	{
		const string AtomCharacters = "!#$%&'*+-/=?^_`{|}~";
		const int MaxEmailAddressLength = 254;
		const int MaxDomainLabelLength = 63;
		const int MaxLocalPartLength = 64;

		[Flags]
		enum SubDomainType {
			None           = 0,
			Alphabetic     = 1,
			Numeric        = 2,
			AlphaNumeric   = 3
		}

		static int Measure (string text, int startIndex, int endIndex, bool allowInternational)
		{
			int count;

			if (allowInternational) {
				int index = startIndex;

				count = 0;
				while (index < endIndex) {
					if (index + 1 < endIndex && char.IsSurrogatePair (text, index))
						index++;

					index++;
					count++;
				}
			} else {
				count = endIndex - startIndex;
			}

			return count;
		}

		static bool IsControl (char c)
		{
			return c <= 31 || c == 127;
		}

		static bool IsDigit (char c)
		{
			return (c >= '0' && c <= '9');
		}

		static bool IsLetter (char c)
		{
			return (c >= 'A' && c <= 'Z') || (c >= 'a' && c <= 'z');
		}

		static bool IsLetterOrDigit (char c)
		{
			return IsLetter (c) || IsDigit (c);
		}

		static bool IsAtom (char c, bool allowInternational)
		{
			// check for control characters
			if (IsControl (c))
				return false;

			return c < 128 ? IsLetterOrDigit (c) || AtomCharacters.IndexOf (c) != -1 : allowInternational && !char.IsWhiteSpace (c);
		}

		static bool IsDomain (char c, bool allowInternational, ref SubDomainType type)
		{
			if (c < 128) {
				if (IsLetter (c) || c == '-') {
					type |= SubDomainType.Alphabetic;
					return true;
				}

				if (IsDigit (c)) {
					type |= SubDomainType.Numeric;
					return true;
				}

				return false;
			}

			if (allowInternational && !char.IsWhiteSpace (c)) {
				type |= SubDomainType.Alphabetic;
				return true;
			}

			return false;
		}

		static bool IsDomainStart (char c, bool allowInternational, out SubDomainType type)
		{
			if (c < 128) {
				if (IsLetter (c)) {
					type = SubDomainType.Alphabetic;
					return true;
				}

				if (IsDigit (c)) {
					type = SubDomainType.Numeric;
					return true;
				}

				type = SubDomainType.None;

				return false;
			}

			if (allowInternational && !char.IsWhiteSpace (c)) {
				type = SubDomainType.Alphabetic;
				return true;
			}

			type = SubDomainType.None;

			return false;
		}

		static bool SkipAtom (string text, ref int index, bool allowInternational)
		{
			int startIndex = index;

			while (index < text.Length && IsAtom (text[index], allowInternational))
				index++;

			return index > startIndex;
		}

		static bool SkipSubDomain (string text, ref int index, bool allowInternational, out SubDomainType type, out EmailValidationError error)
		{
			int startIndex = index;

			if (!IsDomainStart (text[index], allowInternational, out type)) {
				error = new EmailValidationError (EmailValidationErrorCode.InvalidDomainCharacter, index);
				return false;
			}

			index++;

			while (index < text.Length && IsDomain (text[index], allowInternational, ref type))
				index++;

			// https://datatracker.ietf.org/doc/html/rfc2181#section-11
			// The length of any one label is limited to between 1 and 63 octets. A full domain
			// name is limited to 255 octets (including the separators).
			int length = Measure (text, startIndex, index, allowInternational);

			// Don't allow single-character top-level domains.
			if (index == text.Length && length == 1) {
				error = new EmailValidationError (EmailValidationErrorCode.IncompleteDomainLabel, startIndex, index);
				return false;
			}

			if (length > MaxDomainLabelLength) {
				error = new EmailValidationError (EmailValidationErrorCode.DomainLabelTooLong, startIndex, index);
				return false;
			}

			if (text[index - 1] == '-') {
				error = new EmailValidationError (EmailValidationErrorCode.InvalidDomainCharacter, index - 1);
				return false;
			}

			error = EmailValidationError.None;

			return true;
		}

		static bool SkipDomain (string text, ref int index, bool allowTopLevelDomains, bool allowInternational, out EmailValidationError error)
		{
			int startIndex = index;

			if (!SkipSubDomain (text, ref index, allowInternational, out var type, out error))
				return false;

			if (index < text.Length) {
				if (text[index] == '.') {
					do {
						index++;

						if (index == text.Length) {
							error = new EmailValidationError (EmailValidationErrorCode.IncompleteDomain, startIndex, index);
							return false;
						}

						if (!SkipSubDomain (text, ref index, allowInternational, out type, out error))
							return false;
					} while (index < text.Length && text[index] == '.');
				} else {
					error = new EmailValidationError (EmailValidationErrorCode.InvalidDomainCharacter, index);
					return false;
				}
			} else if (!allowTopLevelDomains) {
				error = new EmailValidationError (EmailValidationErrorCode.IncompleteDomain, startIndex, index);
				return false;
			}

			// Note: by allowing AlphaNumeric, we get away with not having to support punycode.
			if (type == SubDomainType.Numeric) {
				error = new EmailValidationError (EmailValidationErrorCode.InvalidDomainCharacter, index);
				return false;
			}

			error = EmailValidationError.None;

			return true;
		}

		static bool SkipQuoted (string text, ref int index, bool allowInternational, out EmailValidationError error)
		{
			int startIndex = index;
			bool escaped = false;

			// skip over leading '"'
			index++;

			while (index < text.Length) {
				if (IsControl (text[index]) || (text[index] >= 128 && !allowInternational)) {
					error = new EmailValidationError (EmailValidationErrorCode.InvalidLocalPartCharacter, index);
					return false;
				}

				if (text[index] == '\\') {
					escaped = !escaped;
				} else if (!escaped) {
					if (text[index] == '"')
						break;
				} else {
					escaped = false;
				}

				index++;
			}

			if (index >= text.Length || text[index] != '"') {
				error = new EmailValidationError (EmailValidationErrorCode.UnterminatedQuotedString, startIndex, index);
				return false;
			}

			index++;

			error = EmailValidationError.None;

			return true;
		}

		static bool SkipIPv4Literal (string text, ref int index, out EmailValidationError error)
		{
			int tokenIndex = index;
			int groups = 0;

			while (index < text.Length && groups < 4) {
				int startIndex = index;
				int value = 0;

				while (index < text.Length && IsDigit (text[index])) {
					value = (value * 10) + (text[index] - '0');
					index++;
				}

				if (index == startIndex || index - startIndex > 3 || value > 255) {
					error = new EmailValidationError (EmailValidationErrorCode.InvalidIPAddress, tokenIndex, index);
					return false;
				}

				groups++;

				if (groups < 4 && index < text.Length && text[index] == '.')
					index++;
			}

			if (groups < 4) {
				error = new EmailValidationError (EmailValidationErrorCode.InvalidIPAddress, tokenIndex, index);
				return false;
			}

			error = EmailValidationError.None;

			return true;
		}

		static bool IsHexDigit (char c)
		{
			return (c >= 'A' && c <= 'F') || (c >= 'a' && c <= 'f') || (c >= '0' && c <= '9');
		}

		// This needs to handle the following forms:
		//
		// IPv6-addr = IPv6-full / IPv6-comp / IPv6v4-full / IPv6v4-comp
		// IPv6-hex  = 1*4HEXDIG
		// IPv6-full = IPv6-hex 7(":" IPv6-hex)
		// IPv6-comp = [IPv6-hex *5(":" IPv6-hex)] "::" [IPv6-hex *5(":" IPv6-hex)]
		//             ; The "::" represents at least 2 16-bit groups of zeros
		//             ; No more than 6 groups in addition to the "::" may be
		//             ; present
		// IPv6v4-full = IPv6-hex 5(":" IPv6-hex) ":" IPv4-address-literal
		// IPv6v4-comp = [IPv6-hex *3(":" IPv6-hex)] "::"
		//               [IPv6-hex *3(":" IPv6-hex) ":"] IPv4-address-literal
		//             ; The "::" represents at least 2 16-bit groups of zeros
		//             ; No more than 4 groups in addition to the "::" and
		//             ; IPv4-address-literal may be present
		static bool SkipIPv6Literal (string text, ref int index, out EmailValidationError error)
		{
			int tokenIndex = index;
			bool needGroup = false;
			bool compact = false;
			int groups = 0;

			while (index < text.Length) {
				int startIndex = index;

				while (index < text.Length && IsHexDigit (text[index]))
					index++;

				if (index >= text.Length)
					break;

				if (index > startIndex && text[index] == '.' && (compact || groups == 6)) {
					// IPv6v4
					index = startIndex;

					if (!SkipIPv4Literal (text, ref index, out error))
						return false;

					return compact ? groups <= 4 : groups == 6;
				}

				int count = index - startIndex;
				if (count > 4) {
					error = new EmailValidationError (EmailValidationErrorCode.InvalidIPAddress, tokenIndex, index);
					return false;
				}

				bool comp;

				if (count > 0) {
					needGroup = false;
					comp = false;
					groups++;

					if (text[index] != ':')
						break;
				} else if (text[index] == ':') {
					// There were no hex digits at the start, so this must be an IPv6-comp
					// or an IPv6v4-comp which means we will need exactly 2 colons.
					comp = true;
				} else {
					break;
				}

				startIndex = index;
				while (index < text.Length && text[index] == ':')
					index++;

				count = index - startIndex;
				if (count > 2) {
					error = new EmailValidationError (EmailValidationErrorCode.InvalidIPAddress, tokenIndex, index);
					return false;
				}

				if (count == 2) {
					if (compact) {
						error = new EmailValidationError (EmailValidationErrorCode.InvalidIPAddress, tokenIndex, index);
						return false;
					}

					compact = true;
				} else if (comp) {
					// expected exactly 2 colons for IPv6-comp or IPv6v4-comp address
					error = new EmailValidationError (EmailValidationErrorCode.InvalidIPAddress, tokenIndex, index);
					return false;
				} else {
					needGroup = true;
				}
			}

			if (needGroup || !(compact ? groups <= 6 : groups == 8)) {
				error = new EmailValidationError (EmailValidationErrorCode.InvalidIPAddress, tokenIndex, index);
				return false;
			}

			error = EmailValidationError.None;

			return true;
		}

		/// <summary>
		/// Validate the specified email address.
		/// </summary>
		/// <remarks>
		/// <para>Validates the syntax of an email address.</para>
		/// <para>If <paramref name="allowTopLevelDomains"/> is <c>true</c>, then the validator will
		/// allow addresses with top-level domains like <c>postmaster@dk</c>.</para>
		/// <para>If <paramref name="allowInternational"/> is <c>true</c>, then the validator
		/// will use the newer International Email standards for validating the email address.</para>
		/// </remarks>
		/// <returns><c>true</c> if the email address is valid; otherwise, <c>false</c>.</returns>
		/// <param name="email">An email address.</param>
		/// <param name="allowTopLevelDomains"><c>true</c> if the validator should allow addresses at top-level domains; otherwise, <c>false</c>.</param>
		/// <param name="allowInternational"><c>true</c> if the validator should allow international characters; otherwise, <c>false</c>.</param>
		/// <param name="error">The error if the email address is invalid.</param>
		/// <exception cref="System.ArgumentNullException">
		/// <paramref name="email"/> is <c>null</c>.
		/// </exception>
		public static bool TryValidate (string email, bool allowTopLevelDomains, bool allowInternational, out EmailValidationError error)
		{
			int index = 0;

			if (email == null)
				throw new ArgumentNullException (nameof (email));

			if (email.Length == 0) {
				error = new EmailValidationError (EmailValidationErrorCode.EmptyAddress);
				return false;
			}

			if (Measure (email, 0, email.Length, allowInternational) > MaxEmailAddressLength) {
				error = new EmailValidationError (EmailValidationErrorCode.AddressTooLong);
				return false;
			}

			// Local-part = Dot-string / Quoted-string
			//       ; MAY be case-sensitive
			//
			// Dot-string = Atom *("." Atom)
			//
			// Quoted-string = DQUOTE *qcontent DQUOTE
			if (email[index] == '"') {
				if (!SkipQuoted (email, ref index, allowInternational, out error))
					return false;
			} else {
				if (!SkipAtom (email, ref index, allowInternational)) {
					error = new EmailValidationError (EmailValidationErrorCode.InvalidLocalPartCharacter, index);
					return false;
				}

				while (index < email.Length && email[index] == '.') {
					index++;

					if (index >= email.Length) {
						error = new EmailValidationError (EmailValidationErrorCode.IncompleteLocalPart, 0, index);
						return false;
					}

					if (!SkipAtom (email, ref index, allowInternational)) {
						error = new EmailValidationError (EmailValidationErrorCode.InvalidLocalPartCharacter, index);
						return false;
					}
				}
			}

			// https://datatracker.ietf.org/doc/html/rfc5321#section-4.5.3.1.1
			// The maximum total length of a user name or other local-part is 64 octets.
			int localPartLength = Measure (email, 0, index, allowInternational);
			if (localPartLength > MaxLocalPartLength) {
				error = new EmailValidationError (EmailValidationErrorCode.LocalPartTooLong, 0, index);
				return false;
			}

			if (index >= email.Length) {
				error = new EmailValidationError (EmailValidationErrorCode.IncompleteLocalPart, 0, index);
				return false;
			}

			if (email[index] != '@') {
				error = new EmailValidationError (EmailValidationErrorCode.InvalidLocalPartCharacter, index);
				return false;
			}

			// Skip over '@'
			index++;

			if (index >= email.Length) {
				error = new EmailValidationError (EmailValidationErrorCode.IncompleteDomain, index);
				return false;
			}

			if (email[index] != '[') {
				// domain
				if (!SkipDomain (email, ref index, allowTopLevelDomains, allowInternational, out error))
					return false;

				return index == email.Length;
			}

			// address literal
			index++;

			// We need at least 7 more characters. "1.1.1.1" and "IPv6:::" are the shortest literals we can have.
			if (index + 7 >= email.Length) {
				error = new EmailValidationError (EmailValidationErrorCode.InvalidIPAddress, index);
				return false;
			}

			if (string.Compare (email, index, "IPv6:", 0, 5, StringComparison.OrdinalIgnoreCase) == 0) {
				index += "IPv6:".Length;
				if (!SkipIPv6Literal (email, ref index, out error))
					return false;
			} else {
				if (!SkipIPv4Literal (email, ref index, out error))
					return false;
			}

			if (index >= email.Length || email[index++] != ']') {
				error = new EmailValidationError (EmailValidationErrorCode.UnterminatedIPAddressLiteral, index);
				return false;
			}

			if (index < email.Length) {
				error = new EmailValidationError (EmailValidationErrorCode.UnexpectedCharactersAfterDomain, index);
				return false;
			}

			error = EmailValidationError.None;

			return true;
		}

		/// <summary>
		/// Validate the specified email address.
		/// </summary>
		/// <remarks>
		/// <para>Validates the syntax of an email address.</para>
		/// <para>If <paramref name="allowTopLevelDomains"/> is <c>true</c>, then the validator will
		/// allow addresses with top-level domains like <c>postmaster@dk</c>.</para>
		/// <para>If <paramref name="allowInternational"/> is <c>true</c>, then the validator
		/// will use the newer International Email standards for validating the email address.</para>
		/// </remarks>
		/// <returns><c>true</c> if the email address is valid; otherwise, <c>false</c>.</returns>
		/// <param name="email">An email address.</param>
		/// <param name="allowTopLevelDomains"><c>true</c> if the validator should allow addresses at top-level domains; otherwise, <c>false</c>.</param>
		/// <param name="allowInternational"><c>true</c> if the validator should allow international characters; otherwise, <c>false</c>.</param>
		/// <exception cref="System.ArgumentNullException">
		/// <paramref name="email"/> is <c>null</c>.
		/// </exception>
		public static bool Validate (string email, bool allowTopLevelDomains = false, bool allowInternational = false)
		{
			return TryValidate (email, allowTopLevelDomains, allowInternational, out _);
		}
	}
}
